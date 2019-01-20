using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using hamming74;

namespace directories
{
    partial class Messenger
    {
        private static Queue<byte[]> ReadQueue;
        private static Thread ReadQueueHandlerThread;
        private static EventWaitHandle WaitingForReadMsg;

        private static EventWaitHandle WaitingForAck;
        private static EventWaitHandle WaitingForAck_0xFF;

        private static EventWaitHandle WaitingForHammingErrorResolve;

        private static LinkedList<List<byte[]>> MsgAssembly;
        private static int LongMsg;

        private static byte CorrectFrame;

        private static void InitRead()
        {
            ReadQueue = new Queue<byte[]>();
            WaitingForReadMsg = new EventWaitHandle(false, EventResetMode.ManualReset);

            WaitingForAck = new EventWaitHandle(false, EventResetMode.AutoReset);
            WaitingForAck_0xFF = new EventWaitHandle(false, EventResetMode.ManualReset);

            WaitingForHammingErrorResolve = new EventWaitHandle(true, EventResetMode.ManualReset);

            MsgAssembly = new LinkedList<List<byte[]>>();
            LongMsg = 0;

            CorrectFrame = 0;

            ReadQueueHandlerThread = new Thread(new ThreadStart(ReadQueueHandler));
            ReadQueueHandlerThread.Start();
        }

        public static void Read(byte[] frames_raw)
        {
            byte[] frames = Hamming74.decode(frames_raw);
            if (frames == null)
            {
                Console.WriteLine("Hamming error");

                OnHammingError();

                return;
            }

            Console.WriteLine("Spliting buffer");
            foreach (byte[] frame in Message.Split(frames))
            {
                CorrectFrame = frame[0];

                Console.Write("Reading ");
                Console.WriteLine(BitConverter.ToString(frame));
                
                ReadQueue.Enqueue(frame);
                WaitingForReadMsg.Set();
            }

        }

        private static void OnHammingError()
        {
            SendDirect(new Message(Message.FrameType.Ret, new byte[] { (byte)(CorrectFrame + 1) }));

            // Flush Read buffer
            Physical.serialPort.DiscardInBuffer();
        }

        private static void OnAck_0xFF() { WaitingForAck_0xFF.Set(); }
        private static void OnAck() { WaitingForAck.Set(); }

        private static void WaitAck()
        {
            Console.WriteLine("Waiting for Ack");
            WaitingForAck.WaitOne();
            Console.WriteLine("Got Ack");
        }

        private static void ExitRead()
        {
            ReadQueueHandlerThread.Abort();
        }

        private static void OnMsgAssembled(List<byte[]> msg)
        {
            Message response = Message.FromBytes(msg);
            if (response != null) Send(response);
        }

        private static void ReadQueueHandler()
        {
            while (true)
            {
                while (ReadQueue.Count > 0)
                {

                    byte[] frame = ReadQueue.Dequeue();

                    //byte num = frame[0];

                    if (frame[0] == Message.MAX_FRAME_NUM)
                    {
                        Console.WriteLine("Received 0xFF frame; Sending Ack 0xFF");
                        Send(new Message(Message.FrameType.Ack, new byte[] { Message.MAX_FRAME_NUM }));
                    }

                    Message.FrameType type = (Message.FrameType)frame[1];

                    switch (type)
                    {
                        case Message.FrameType.Link:
                            Console.WriteLine("Got Link, Sending Ack");

                            IsHost = false;
                            Physical.WaitingForIsConnected.Set();
                            IsConnected = true;

                            Send(new Message(Message.FrameType.Ack));
                            Program.main_form.serialPort_Connected();
                            break;

                        case Message.FrameType.Unlink:
                            Send(new Message(Message.FrameType.Ack));
                            break;

                        case Message.FrameType.Ack:
                            if (frame[2] == frame[3])
                            {
                                switch (frame[2])
                                {
                                    case Message.MAX_FRAME_NUM:
                                        Console.WriteLine("Received Ack 0xFF");
                                        OnAck_0xFF();
                                        break;

                                    default:
                                        OnAck();
                                        break;
                                }
                            }
                            else
                            {
                                // error
                            }

                            break;

                        case Message.FrameType.Ret:

                            WaitingForHammingErrorResolve.Reset();

                            Console.WriteLine("Got Ret " + BitConverter.ToString(frame, 2, 1));

                            ResendFrames(frame[2]);

                            WaitingForHammingErrorResolve.Set();

                            break;

                        default:
                            if ((type & Message.FrameType.Info) == Message.FrameType.Info)
                            {
                                type = type ^ Message.FrameType.Info;
                                switch (type)
                                {
                                    case Message.FrameType.MsgStart:
                                        MsgAssembly.AddLast(new List<byte[]>() { frame });
                                        ++LongMsg;

                                        Program.main_form.status_toolStripProgressBar_setMax(BitConverter.ToInt32(frame, Message.HEADER_SIZE));

                                        break;

                                    case Message.FrameType.MsgEnd:
                                        {
                                            int error = 0;
                                            foreach (List<byte[]> msg in MsgAssembly)
                                            {
                                                if (frame[2] == msg.Last()[0]) // compare prev frame num
                                                {
                                                    msg.Add(frame);

                                                    OnMsgAssembled(msg);
                                                    MsgAssembly.Remove(msg);
                                                    --LongMsg;

                                                    Program.main_form.status_toolStripProgressBar_Clear();

                                                    if (LongMsg < 0) Console.WriteLine("MsgEnd without MsgStart; should never happen!");

                                                    break;
                                                }
                                                else ++error;
                                            }

                                            if (error == MsgAssembly.Count) Console.WriteLine("End Frame came out of order; should never happen!");
                                            
                                        }

                                        break;

                                    case Message.FrameType.Settings:

                                        IsHost = false;

                                        Console.WriteLine("Got Speed");
                                        Send(new Message(Message.FrameType.Ack));

                                        Console.WriteLine("Waiting for sending queue to empty");
                                        Thread.Sleep(1);
                                        WaitingForSendQueue_Empty.WaitOne();

                                        Physical.Close();
                                        Physical.serialPort.BaudRate = BitConverter.ToInt32(frame, Message.HEADER_SIZE);

                                        Console.Write("Changed speed to ");
                                        Console.WriteLine(Physical.serialPort.BaudRate);

                                        Physical.Open();

                                        break;

                                    case Message.FrameType.SetReject:

                                        break;

                                    case Message.FrameType.Data:
                                    case Message.FrameType.File:
                                    case Message.FrameType.Directory:
                                        if (LongMsg > 0)
                                        {
                                            int error = 0;
                                            foreach (List<byte[]> msg in MsgAssembly)
                                            {
                                                if (frame[Message.HEADER_SIZE] == msg.Last()[0]) // compare prev frame num
                                                {
                                                    msg.Add(frame);
                                                    Program.main_form.status_toolStripProgressBar_incValue();
                                                    break;
                                                }
                                                else ++error;
                                            }

                                            if (error == MsgAssembly.Count) Console.WriteLine("Data Frame came out of order; should never happen!");

                                        }
                                        else
                                        {
                                            OnMsgAssembled(new List<byte[]>() { frame });
                                        }

                                        break;
                                }
                            }
                            else
                            if ((type & Message.FrameType.Get) == Message.FrameType.Get)
                            {
                                type = type ^ Message.FrameType.Get;
                                switch (type)
                                {
                                    case Message.FrameType.File:
                                        
                                        OnMsgAssembled(new List<byte[]>() { frame });

                                        break;

                                    case Message.FrameType.Directory:
                                        
                                        OnMsgAssembled(new List<byte[]>() { frame });

                                        break;

                                    default:
                                        // error
                                        break;
                                }
                            }
                            break;
                    }

                }

                Console.WriteLine("Waiting for Messages to Read");
                WaitingForReadMsg.Reset();
                WaitingForReadMsg.WaitOne();
                Console.WriteLine("Got smth to Read");
            }
        }

    }
}
