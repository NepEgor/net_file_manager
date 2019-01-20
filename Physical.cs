using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace directories
{
    static class Physical
    {
        public const int DEFAULT_SPEED = 9600;
        public static SerialPort serialPort;

        public static bool IsConnected { get { return serialPort.IsOpen && serialPort.CtsHolding; } }
        
        public static EventWaitHandle WaitingForIsConnected;

        public static void Init()
        {
            serialPort = new SerialPort();

            serialPort.BaudRate = DEFAULT_SPEED;

            serialPort.DataBits = 8;

            //serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;

            serialPort.ReadBufferSize = Message.MAX_FRAME_SIZE * 2;
            serialPort.WriteBufferSize = Message.MAX_FRAME_SIZE * 2;

            serialPort.Handshake = Handshake.RequestToSend;

            serialPort.DataReceived += new SerialDataReceivedEventHandler(Read);
            serialPort.PinChanged += new SerialPinChangedEventHandler(PinChanged);

            WaitingForIsConnected = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public static void Send(byte[] data)
        {
            Console.WriteLine("PHYSICAL Sending");
            serialPort.Write(data, 0, data.Length);
        }

        public static void PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            //Console.Write("PHYSICAL ");
            //Console.WriteLine(e.EventType);
            //Console.WriteLine(serialPort.CDHolding);
            //Console.WriteLine(serialPort.CtsHolding);
            //Console.WriteLine(serialPort.DsrHolding);
            //return;

            if (e.EventType == SerialPinChange.DsrChanged)
            {
                if (IsConnected)
                {
                    Console.WriteLine("Pin connected");
                    WaitingForIsConnected.Set();

                    if (!Messenger.IsConnected && !Messenger.IsConnecting)
                    {
                        Program.main_form.serialPort_Connecting();
                        Messenger.IsConnecting = true;
                        new Thread(new ThreadStart(Messenger.Connect)).Start();
                    }
                }
                else
                {
                    Console.WriteLine("Pin disconnected");
                    WaitingForIsConnected.Reset();
                }

            }
        }

        public static void Read(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("PHYSICAL Reading");
            byte[] data = new byte[serialPort.ReadBufferSize];

            int bytes_read = 0, bytes_to_read = serialPort.BytesToRead;
            for (int i = 0; i < bytes_to_read; i += bytes_read)
            {
                bytes_read = serialPort.Read(data, 0, data.Length);

                byte[] data2 = new byte[bytes_read];
                for (int j = 0; j < bytes_read; ++j) data2[j] = data[j];

                Messenger.Read(data2);
            }
        }

        public static bool Open()
        {
            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine("Can't open Port");
                return false;
            }

            Console.WriteLine("Opened Port");
            WaitingForIsConnected.Set();
            return true;
        }

        public static void Close()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine("Closed Port");
                WaitingForIsConnected.Reset();
                Thread.Sleep(100);
            }
        }
    }
}
