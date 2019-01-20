using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace directories
{
    partial class Messenger
    {
        public static bool IsConnected { get; private set; }
        public static bool IsConnecting;

        private static int StoredSpeed;

        private static bool IsHost;

        public static void Init()
        {
            IsConnected = false;
            IsHost = true;

            StoredSpeed = Physical.serialPort.BaudRate;

            InitSend();
            InitRead();
        }

        public static void Exit()
        {
            ExitSend();
            ExitRead();            
        }

        private static void Link()
        {
            Console.WriteLine("I am Host");

            Send(new Message(Message.FrameType.Link));
            Program.main_form.serialPort_Connected();

            WaitAck();
            IsConnected = true;
        }

        public static void Connect()
        {
            Console.WriteLine("Connecting");
            Physical.WaitingForIsConnected.WaitOne();
            Console.WriteLine("Physical connected");

            if (IsHost)
            {
                Link();

                ChangeSpeed(StoredSpeed);

                Send(new Message(Program.main_form.local_root));

                Send(new Message(Message.FrameType.Get | Message.FrameType.Directory));
            }

            Console.WriteLine("Connected");
            IsConnecting = false;
        }

        public static void Disconnect()
        {
            if (!IsConnected) return;

            Send(new Message(Message.FrameType.Unlink));

            Console.WriteLine("Disconnected");

            IsConnected = false;
            Program.main_form.serialPort_Disconnected();
        }

        public static void Test()
        {
            if (!IsConnected) return;

            Message msg = new Message(Message.FrameType.Info | Message.FrameType.Data, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
            Send(msg);

        }

        private static void ChangeSpeed(int speed)
        {
            if (!IsConnected) return;

            Console.WriteLine("Sending speed");
            Send(new Message(speed));
            WaitAck();

            Console.WriteLine("Changing speed");
            Physical.Close();
            Physical.serialPort.BaudRate = speed;

            Console.Write("Changed speed to ");
            Console.WriteLine(Physical.serialPort.BaudRate);

            Physical.Open();

            Thread.Sleep(100);
            Physical.WaitingForIsConnected.WaitOne();
            Link();
        }

        public static bool ChangePortSettings(string name, int speed)
        {
            Console.WriteLine("Changing settings");

            bool result = false;

            IsHost = true;

            if (IsConnected && Physical.serialPort.IsOpen)
            {
                if(Physical.serialPort.PortName == name)
                {

                }
                else
                {
                    Disconnect();
                    Physical.Close();
                    Physical.serialPort.PortName = name;
                    result = Physical.Open();
                }

                if (IsHost)
                {
                    ChangeSpeed(speed);
                    result = true;
                }
            }
            else // port closed
            {
                Physical.Close();
                Physical.serialPort.PortName = name;
                result = Physical.Open();
                if (!result) return false;

                Console.WriteLine("Stored speed");
                StoredSpeed = speed;
            }
            
            return result;
        }
        
        public static void SendRoot()
        {
            if (!IsConnected) return;

            Console.WriteLine("Sending root");
            Send(new Message(Program.main_form.local_root));
            //WaitAck();
        }

        public static void RequestDirectory(RemoteDirectoryInfo dir)
        {
            if (!IsConnected) return;

            Console.WriteLine("Requesting directory");
            Send(new Message(Message.FrameType.Get | Message.FrameType.Directory, Encoding.Unicode.GetBytes(dir.FullName)));
        }

        public static void RequestFile(RemoteFileInfo file)
        {
            if (!IsConnected) return;

            Console.WriteLine("Requesting file");
            Send(new Message(Message.FrameType.Get | Message.FrameType.File, Encoding.Unicode.GetBytes(file.FullName)));
        }
    }
}
