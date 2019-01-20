using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace directories
{
    public partial class Main_Form
    {
        public RemoteDirectoryInfo remote_root;
        private RemoteDirectoryInfo current_remote_dir;

        private DialogResult dialogResult;

        private string FileName;

        private void Main_Form_FileNavigator_Init()
        {
            remote_root = new RemoteDirectoryInfo("root");
            current_remote_dir = remote_root;
        }

        public void UpdateRemoteDirectory(byte[] data)
        {
            RemoteDirectoryInfo.FromBytes(data);
            listView_setContent(current_remote_dir);
        }

        public void SaveRemoteFile(byte[] data)
        {
            Invoke((Action)delegate
            {
                saveFileDialog.FileName = FileName;
                dialogResult = saveFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    FileStream fs = File.Open(saveFileDialog.FileName, FileMode.Create);
                    fs.Write(data, 0, data.Length);

                    fs.Close();
                }
            });
        }

        private void dir_listView_DoubleClick(object sender, EventArgs e)
        {
            if(dir_listView.SelectedIndices[0] < current_remote_dir.Directories.Count)
            {
                current_remote_dir = current_remote_dir.Directories[dir_listView.SelectedIndices[0]];

                if (current_remote_dir.Directories.Count == 0) Messenger.RequestDirectory(current_remote_dir);

                listView_setContent(current_remote_dir);
            }
            else
            {
                FileName = current_remote_dir.Files[dir_listView.SelectedIndices[0] - current_remote_dir.Directories.Count].Name;
                Messenger.RequestFile(current_remote_dir.Files[dir_listView.SelectedIndices[0] - current_remote_dir.Directories.Count]);
            }
        }

        private void back_button_Click(object sender, EventArgs e)
        {
            if (current_remote_dir.Parent != null)
            {
                current_remote_dir = current_remote_dir.Parent;
                listView_setContent(current_remote_dir);
            }
        }

        private void home_button_Click(object sender, EventArgs e)
        {
            current_remote_dir = remote_root;
            listView_setContent(current_remote_dir);
        }

        private void update_button_Click(object sender, EventArgs e)
        {
            Messenger.RequestDirectory(current_remote_dir);
        }

        private void listView_setContent(RemoteDirectoryInfo directory)
        {
            dir_listView.Invoke((Action)delegate
            {
                path_toolStripTextBox.Text = directory.FullName;

                dir_listView.Items.Clear();
                foreach (var dir in directory.Directories)
                {
                    dir_listView.Items.Add(new ListViewItem(dir.Name) { ImageKey = dir_ImageKey });
                }
                foreach (var file in directory.Files)
                {
                    dir_listView.Items.Add(new ListViewItem(file.Name) { ImageKey = file_ImageKey });
                }
            });
        }

    }
}
