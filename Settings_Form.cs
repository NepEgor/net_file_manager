using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace directories
{
    public partial class Settings_Form : Form
    {
        public Settings_Form()
        {
            InitializeComponent();
        }

        private void Settings_Form_Load(object sender, EventArgs e)
        {
            speed_comboBox.Items.AddRange(new object[]
            {
                75,
                110,
                150,
                300,
                600,
                1200,
                1800,
                2400,
                4800,
                7200,
                9600,
                14400,
                19200,
                38400,
                56000,
                57600,
                115200,
                128000
            });
            
            speed_comboBox.SelectedItem = Physical.serialPort.BaudRate;

            port_comboBox.Items.AddRange(SerialPort.GetPortNames());

            if (Physical.serialPort.IsOpen)
            {
                port_comboBox.SelectedItem = Physical.serialPort.PortName;
            }
            else
            {
                SerialPort dummy = new SerialPort();
                foreach (string name in port_comboBox.Items)
                {
                    dummy.PortName = name;
                    if (!dummy.IsOpen)
                    {
                        port_comboBox.SelectedItem = name;
                        break;
                    }
                }
            }
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Settings_Form_Close(object sender, FormClosingEventArgs e)
        {
            if (Physical.serialPort.IsOpen &&
                (string)port_comboBox.SelectedItem == Physical.serialPort.PortName &&
                  (int)speed_comboBox.SelectedItem == Physical.serialPort.BaudRate)
            {
                return;
            }

            // if Port can't be opened prevent closing
            e.Cancel = !Messenger.ChangePortSettings((string)port_comboBox.SelectedItem, (int)speed_comboBox.SelectedItem);
        }
    }
}
