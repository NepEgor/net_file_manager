using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hamming74;
using System.Threading;

namespace directories
{
    class Message
    {
        //    1     1        2         max 1024
        // || # | Type | Data size || Data (opt) ||
        // Sizes before Hamming74 encoding
        // After - x2
        public const ushort MAX_DATA_SIZE = 1024; // 1kb
        public const byte HEADER_SIZE = 4;
        public const ushort MAX_FRAME_SIZE = MAX_DATA_SIZE + HEADER_SIZE;
        public const byte MAX_FRAME_NUM = 0xFF;

        private static byte _FrameNumber = 0;
        public static byte FrameNumber { get { return ++_FrameNumber; } }
        
        private static byte[] frame_part = null;

        public enum FrameType
        {
            Link   = 0x10,
            Unlink = 0x20,

            Ack    = 0x30,
            Ret    = 0x40,

            Get    = 0x50,           // mb only with File or Directory
            Info   = 0x60,
                MsgStart  = 0x01,
                MsgEnd    = 0x02,

                Settings  = 0x03,
                SetReject = 0x04,

                Data      = 0x05,
                File      = 0x06,
                Directory = 0x07,
        }
        
        public FrameType Type;
        public byte[] Data;

        public Message(FrameType type)
        {
            Type = type;
            Data = null;
        }

        public Message(FrameType type, byte[] data)
        {
            Type = type;
            Data = data;
        }

        public Message(RemoteFileInfo file)
        {
            Type = FrameType.Info | FrameType.File;

            FileStream file_stream = File.Open(file.Path, FileMode.Open);
            
            Data = new byte[file_stream.Length];
            file_stream.Read(Data, 0, Data.Length);

            file_stream.Close();
        }

        public Message(RemoteDirectoryInfo directory)
        {
            Type = FrameType.Info | FrameType.Directory;

            Data = directory.ToBytes();
        }

        public Message(int baud_rate)
        {
            Type = FrameType.Info | FrameType.Settings;
            Data = BitConverter.GetBytes(baud_rate);
        }

        public bool NeedAck()
        {
            return NeedAck(Type);
        }

        public static bool NeedAck(FrameType type)
        {
            return
                //type == FrameType.Link ||
                //type == (FrameType.Info | FrameType.Settings) ||
                //type == (FrameType.Info | FrameType.Data) ||
                type == (FrameType.Info | FrameType.Directory) ||
                type == (FrameType.Info | FrameType.File);
        }

        public IEnumerable<byte[]> ToBytes()
        {
            byte[] frame;

            if (Data == null)
            {
                frame = new byte[HEADER_SIZE];

                frame[0] = FrameNumber;
                frame[1] = (byte)Type;

                frame[2] = 0;
                frame[3] = 0;

                //yield return Hamming74.code(frame);
                yield return frame;
            }
            else
            if (Type == FrameType.Ack || Type == FrameType.Ret)
            {
                frame = new byte[HEADER_SIZE];

                frame[0] = FrameNumber;
                frame[1] = (byte)Type;

                frame[2] = Data[0];
                frame[3] = Data[0];

                //yield return Hamming74.code(frame);
                yield return frame;
            }
            else
            if (Data.Length <= MAX_DATA_SIZE)
            {
                frame = new byte[Data.Length + HEADER_SIZE];

                frame[0] = FrameNumber;
                frame[1] = (byte)Type;

                frame[2] = (byte)Data.Length;
                frame[3] = (byte)(Data.Length >> 8);

                for (int j = 0; j < Data.Length; ++j)
                {
                    frame[j + HEADER_SIZE] = Data[j];
                }

                //yield return Hamming74.code(frame);
                yield return frame;
            }
            else
            {
                int data_length;
                int i = 0;

                byte prev_num;

                int frame_count_i = (Data.Length - 1) / (MAX_DATA_SIZE - 1) + 1;
                byte[] frame_count = BitConverter.GetBytes(frame_count_i);

                frame = new byte[] {
                    FrameNumber,
                    (byte)(FrameType.Info | FrameType.MsgStart),
                    4, 0,      // len
                    frame_count[0],
                    frame_count[1],
                    frame_count[2],
                    frame_count[3]
                };
                prev_num = frame[0];

                //yield return Hamming74.code(frame);
                yield return frame;

                for (int fc = 0; fc < frame_count_i; ++fc)
                {
                    data_length = Data.Length - i + 1 < MAX_DATA_SIZE ? Data.Length - i + 1 : MAX_DATA_SIZE;
                    frame = new byte[data_length + HEADER_SIZE];

                    frame[0] = FrameNumber;
                    frame[1] = (byte)Type;

                    frame[2] = (byte)data_length;
                    frame[3] = (byte)(data_length >> 8);

                    frame[4] = prev_num;
                    prev_num = frame[0];

                    for (int j = 1; j < data_length; ++j, ++i)
                    {
                        frame[j + HEADER_SIZE] = Data[i];
                    }

                    //yield return Hamming74.code(frame);
                    yield return frame;
                }

                frame = new byte[]
                {
                    FrameNumber,
                    (byte)(FrameType.Info | FrameType.MsgEnd),
                    prev_num,
                    prev_num
                };

                //yield return Hamming74.code(frame);
                yield return frame;
            }
                
        }

        public static IEnumerable<byte[]> Split(byte[] frames)
        {
            /*
            byte[] frames = Hamming74.decode(frames_raw);
            if(frames == null)
            {
                Console.WriteLine("Hamming error");

                yield break;
            }
            */

            byte[] frame;
            ushort len;

            if(frame_part != null)
            {
                Console.Write("Frame Part ");
                Console.WriteLine(BitConverter.ToString(frame_part));

                byte[] frames2 = new byte[frame_part.Length + frames.Length];
                for (int i = 0; i < frame_part.Length; ++i) frames2[i] = frame_part[i];
                for (int i = 0; i < frames.Length; ++i) frames2[i + frame_part.Length] = frames[i];

                frames = frames2;

                frame_part = null;
            }

            for (int i = 0; i < frames.Length; i += frame.Length)
            {
                int rest = frames.Length - i;
                if (rest < HEADER_SIZE)
                {
                    //Console.Write("rest ");
                    //Console.WriteLine(rest);
                    frame_part = new byte[rest];
                    for (int j = 0; j < rest; ++j)
                    {
                        frame_part[j] = frames[i + j];
                    }
                    break;
                }

                if (
                    (FrameType)frames[i+1] == FrameType.Ack ||
                    (FrameType)frames[i+1] == FrameType.Ret ||
                    (FrameType)frames[i+1] == (FrameType.Info | FrameType.MsgEnd)
                   )
                {
                    len = 0;
                }
                else
                {
                    len = (ushort)(frames[i + 2] | frames[i + 3] << 8);
                }
                
                int cnt = HEADER_SIZE + len;
                if (cnt > frames.Length - i)
                {
                    cnt = frames.Length - i;

                    frame_part = new byte[cnt];

                    for (int j = 0; j < cnt; ++j)
                    {
                        frame_part[j] = frames[i + j];
                    }
                    break;
                }

                frame = new byte[cnt];

                for (int j = 0; j < cnt; ++j)
                {
                    frame[j] = frames[i + j];
                }

                yield return frame;
            }

        }

        public static Message FromBytes(List<byte[]> frames)
        {
            Message msg = null;
            Message response = null;

            if (
                frames.Count >= 3 &&
                    (
                        frames.First()[1] != (byte)(FrameType.Info | FrameType.MsgStart) ||
                        frames.Last()[1] != (byte)(FrameType.Info | FrameType.MsgEnd)
                    )
               )
            {
                Console.WriteLine("Weird msg; should never happen!");
                return null;
            }

            byte msg_start = 0;
            if (frames.Count >= 3)
            {
                msg_start = 1;
            }

            msg = new Message((FrameType)frames[msg_start][1]);

            //ushort data_len = 0;
            long data_len = 0;
            for (int i = msg_start; i < frames.Count - msg_start; ++i)
            {
                if(msg.Type == (FrameType)frames[i][1])
                {
                    data_len += (ushort)(BitConverter.ToUInt16(frames[i], 2) - msg_start);
                }
            }
            msg.Data = new byte[data_len];

            long k = 0;
            for (int i = msg_start; i < frames.Count - msg_start; ++i)
            {
                if (msg.Type == (FrameType)frames[i][1])
                {
                    uint len = BitConverter.ToUInt16(frames[i], 2);
                    //Console.WriteLine(len);
                    for (int j = HEADER_SIZE + msg_start; j < HEADER_SIZE + len; ++j, ++k)
                    {
                        msg.Data[k] = frames[i][j];
                    }
                }
            }

            switch (msg.Type)
            {
                case FrameType.Info | FrameType.Data:

                    Console.WriteLine("Message");
                    Console.WriteLine(BitConverter.ToString(msg.Data));
                    //response = new Message(FrameType.Ack);
                    response = null;

                    break;
                case FrameType.Info | FrameType.Directory:

                    Console.WriteLine("Got directory; Updating");
                    Program.main_form.UpdateRemoteDirectory(msg.Data);
                    response = new Message(FrameType.Ack);

                    break;
                case FrameType.Info | FrameType.File:

                    Console.WriteLine("Got file; Saving");
                    Program.main_form.SaveRemoteFile(msg.Data);
                    response = new Message(FrameType.Ack);

                    break;


                case FrameType.Get | FrameType.File:
                    {
                        Console.WriteLine("Got file request\nSending file");

                        string path = Encoding.Unicode.GetString(msg.Data);
                        RemoteFileInfo rfi = (RemoteFileInfo)RemoteDirectoryInfo.OnPath(Program.main_form.local_root, path);
                        if (rfi != null)
                        {
                            //Messenger.Send(new Message(rfi));
                            response = new Message(rfi);
                        }
                    }

                    break;
                case FrameType.Get | FrameType.Directory:

                    Console.WriteLine("Got directory request");

                    if (msg.Data.Length == 0)
                    {
                        //Messenger.SendRoot();
                        response = new Message(Program.main_form.local_root);
                    }
                    else
                    {
                        Console.WriteLine("Sending directory");
                        string path = Encoding.Unicode.GetString(msg.Data);
                        RemoteDirectoryInfo rdi = (RemoteDirectoryInfo)RemoteDirectoryInfo.OnPath(Program.main_form.local_root, path);
                        if(rdi != null)
                        {
                            //Messenger.Send(new Message(rdi));
                            response = new Message(rdi);
                        }
                            
                    }

                    break;

                default:

                    break;
            }

            return response;
        }
        
    }
}
