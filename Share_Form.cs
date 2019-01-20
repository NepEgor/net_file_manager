using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace directories
{
    public partial class Share_Form : Form
    {
        private RemoteDirectoryInfo local_root;
        private DialogResult dialogResult;

        public Share_Form(RemoteDirectoryInfo local_root)
        {
            InitializeComponent();

            this.local_root = local_root;
        }

        private void Share_Form_Load(object sender, EventArgs e)
        {
            foreach (var dir in local_root.Directories)
            {
                root_listView.Items.Add(new ListViewItem(dir.FullName));
            }
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                local_root.Directories.Add(new RemoteDirectoryInfo(folderBrowserDialog.SelectedPath, local_root, local_root));
                root_listView.Items.Add(new ListViewItem(folderBrowserDialog.SelectedPath));
            }
        }

        private void rem_button_Click(object sender, EventArgs e)
        {
            if (root_listView.SelectedIndices.Count != 0)
            {
                local_root.Directories.RemoveAt(root_listView.SelectedIndices[0]);
                root_listView.Items.RemoveAt(root_listView.SelectedIndices[0]);
            }
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Share_From_Closing(object sender, EventArgs e)
        {
            //Messenger.SendRoot(local_root);
        }
    }
}
