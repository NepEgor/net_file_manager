using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace directories
{
    public partial class Main_Form : Form
    {
        public RemoteDirectoryInfo local_root;

        private FileStream root_file;
        private const string root_file_path = "root_dirs";

        private const string dir_ImageKey = "dir";
        private const string file_ImageKey = "file";
        
        public Main_Form()
        {
            InitializeComponent();

            int tbw = top_toolStrip.Width - top_toolStrip.Margin.Left - top_toolStrip.Margin.Right;
            foreach(ToolStripItem tbi in top_toolStrip.Items)
            {
                if (tbi == path_toolStripTextBox) continue;
                tbw -= tbi.Width;
            }
            path_toolStripTextBox.Width = tbw;

            tbw = bottom_toolStrip.Width - bottom_toolStrip.Margin.Left - bottom_toolStrip.Margin.Right;
            foreach (ToolStripItem tbi in bottom_toolStrip.Items)
            {
                if (tbi == status_toolStripProgressBar) continue;
                tbw -= tbi.Width;
            }
            status_toolStripProgressBar.Width = tbw;
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
            Physical.Init();

            Messenger.Init();

            local_root = new RemoteDirectoryInfo("root");

            Main_Form_FileNavigator_Init();

            try
            {
                root_file = File.Open(root_file_path, FileMode.Open);

                byte[] dir_names_raw = new byte[root_file.Length];
                root_file.Read(dir_names_raw, 0, dir_names_raw.Length);
                string[] dir_names = Encoding.Unicode.GetString(dir_names_raw).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                root_file.Close();
                
                foreach (var dir_name in dir_names)
                {
                    local_root.Directories.Add(new RemoteDirectoryInfo(dir_name, local_root, local_root));
                }
                imageList.Images.Add(dir_ImageKey, IconExtractor.Extract("C:\\Windows"));
                imageList.Images.Add(file_ImageKey, IconExtractor.Extract(root_file_path));

                dir_listView.SmallImageList = imageList;
                dir_listView.LargeImageList = imageList;

            }
            catch (FileNotFoundException) { }
            
        }

        private void Main_Form_Shown(object sender, EventArgs e)
        {
            settings_button_Click(sender, e);
        }

        private void Main_Form_Closing(object sender, EventArgs e)
        {
            Messenger.Exit();
            Physical.Close();
        }

        private void share_button_Click(object sender, EventArgs e)
        {
            Share_Form share_form = new Share_Form(local_root);
            share_form.ShowDialog();

            root_file = File.Open(root_file_path, FileMode.Create);
            foreach (var dir in local_root.Directories)
            {
                byte[] dir_name = Encoding.Unicode.GetBytes(dir.Path + '\n');
                root_file.Write(dir_name, 0, dir_name.Length);
            }
            root_file.Close();

            Messenger.SendRoot();
        }

        private void settings_button_Click(object sender, EventArgs e)
        {
            Settings_Form settings_form = new Settings_Form();
            settings_form.ShowDialog();
        }
        
        public void serialPort_Connecting()
        {
            Invoke((Action)delegate
            {
                status_toolStripButton.Image = Properties.Resources.icon_connecting;
                status_toolStripButton.ToolTipText = "Connecting";

                status_toolStripProgressBar.Style = ProgressBarStyle.Marquee;
            });
        }

        public void serialPort_Disconnected()
        {
            Invoke((Action)delegate
            {
                status_toolStripButton.Image = Properties.Resources.icon_disconnected;
                status_toolStripButton.ToolTipText = "Disconnected";

                status_toolStripProgressBar.Style = ProgressBarStyle.Marquee;
            });
        }

        public void serialPort_Connected()
        {
            Invoke((Action)delegate
            {
                status_toolStripButton.Image = Properties.Resources.icon_connected;
                status_toolStripButton.ToolTipText = "Connected";

                status_toolStripProgressBar.Style = ProgressBarStyle.Continuous;
            });
        }

        public void status_toolStripProgressBar_setMax(int max)
        {
            Invoke((Action)delegate
            {
                status_toolStripProgressBar.Maximum = max;
            });
        }
        public void status_toolStripProgressBar_incValue()
        {
            Invoke((Action)delegate
            {
                if(status_toolStripProgressBar.Value < status_toolStripProgressBar.Maximum)
                    ++status_toolStripProgressBar.Value;
            });
        }
        public void status_toolStripProgressBar_Clear()
        {
            Invoke((Action)delegate
            {
                status_toolStripProgressBar.Value = 0;
            });
        }

        private void status_toolStripButton_Click(object sender, EventArgs e)
        {
            Messenger.Test();
        }
        
        private void info_toolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Программа предназначена для передачи файлов между двумя ЭВМ, соединенных нуль-модемно.\n" +
                "Для получения файла выберите файл и кликнете на него два раза.\n\n" +
                "Выполнена в рамках курса \"Сетевые технологии\".\n" +
                "Исполнители:\tМакаров А.В. РТ5-61\n" +
                "\t\tНепочатый Е.В. РТ5-61\n\n" +
                "Преподаватель:\tГалкин В.А.",

                "О программе");
            
        }
    }
}
