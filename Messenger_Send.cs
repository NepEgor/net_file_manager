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
        private static Queue<Message> SendQueue;

        private static EventWaitHandle WaitingForSendMsg;
        private static EventWaitHandle WaitingForSendQueue_Empty;

        private static Thread SendQueueHandlerThread;

        private static byte[][] SendBuffer;
        private static byte CurrentNum;

        private static void InitSend()
        {
            SendQueue = new Queue<Message>();

            WaitingForSendMsg = new EventWaitHandle(false, EventResetMode.ManualReset);
            WaitingForSendQueue_Empty = new EventWaitHandle(false, EventResetMode.ManualReset);

            SendQueueHandlerThread = new Thread(new ThreadStart(SendQueueHandler));
            SendQueueHandlerThread.Start();

            SendBuffer = new byte[256][];
            CurrentNum = 0;
        }

        public static void Send(Message msg)
        {
            SendQueue.Enqueue(msg);
            WaitingForSendMsg.Set();
        }

        private static void ExitSend()
        {
            SendQueueHandlerThread.Abort();
        }

        private static void SendDirect(Message msg)
        {
            IEnumerable<byte[]> frames = msg.ToBytes();
            foreach (byte[] frame in frames)
            {
                Physical.WaitingForIsConnected.WaitOne();
                
                Console.WriteLine("Sending directly " + BitConverter.ToString(frame));

                LogFrame(frame);
                Physical.Send(Hamming74.code(frame));

                if (frame[0] == Message.MAX_FRAME_NUM)
                {
                    Console.WriteLine("Waiting for Ack 0xFF");
                    WaitingForAck_0xFF.Reset();
                    WaitingForAck_0xFF.WaitOne();
                }
            }
        }

        private static void ResendFrames(byte ret_num)
        {
            Console.WriteLine("Resending " + BitConverter.ToString(new byte[] { ret_num }) + ' ' + BitConverter.ToString(new byte[] { CurrentNum }) );
            
            if(CurrentNum < ret_num)
            {
                for (byte i = ret_num; i <= 0xFF; ++i)
                {
                    if (SendBuffer[i] == null) //|| SendBuffer[i][1] == (byte)Message.FrameType.Ret)
                        continue;
                    Physical.Send(Hamming74.code(SendBuffer[i]));
                }
                for(byte i = 0; i <= CurrentNum; ++i)
                {
                    if (SendBuffer[i] == null) //|| SendBuffer[i][1] == (byte)Message.FrameType.Ret)
                        continue;
                    Physical.Send(Hamming74.code(SendBuffer[i]));
                }
            }
            else
            {
                for (byte i = ret_num; i <= CurrentNum; ++i)
                {
                    if (SendBuffer[i] == null) //|| SendBuffer[i][1] == (byte)Message.FrameType.Ret)
                        continue;
                    Physical.Send(Hamming74.code(SendBuffer[i]));
                }
            }
            
        }

        private static void LogFrame(byte[] frame)
        {
            SendBuffer[frame[0]] = frame;
            CurrentNum = frame[0];
        }

        private static void SendQueueHandler()
        {
            while (true)
            {
                while (SendQueue.Count > 0)
                {
                    Message msg = SendQueue.Dequeue();
                    if(msg.Data != null)
                    {
                        Console.Write("Data size: ");
                        Console.WriteLine(msg.Data.Length);
                    }

                    IEnumerable<byte[]> frames = msg.ToBytes();
                    foreach (byte[] frame in frames)
                    {
                        Physical.WaitingForIsConnected.WaitOne();
                        WaitingForHammingErrorResolve.WaitOne();

                        Console.WriteLine("Sending " + BitConverter.ToString(frame));

                        LogFrame(frame);
                        Physical.Send(Hamming74.code(frame));
                        
                        if (frame[0] == Message.MAX_FRAME_NUM)
                        {
                            Console.WriteLine("Waiting for Ack 0xFF");
                            WaitingForAck_0xFF.Reset();
                            WaitingForAck_0xFF.WaitOne();
                        }

                    }

                    if (msg.NeedAck())
                    {
                        WaitAck();
                    }

                }

                Console.WriteLine("Waiting for Messages to Send");
                WaitingForSendQueue_Empty.Set();
                Console.WriteLine("Sending queue is empty");
                WaitingForSendMsg.Reset();
                WaitingForSendMsg.WaitOne();
                Console.WriteLine("Got smth to Send");
                WaitingForSendQueue_Empty.Reset();
            }

        }

    }
}
