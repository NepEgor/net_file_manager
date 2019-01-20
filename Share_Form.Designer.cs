namespace directories
{
    partial class Share_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.root_listView = new System.Windows.Forms.ListView();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.add_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.rem_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ok_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // root_listView
            // 
            this.root_listView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.root_listView.Location = new System.Drawing.Point(0, 37);
            this.root_listView.MultiSelect = false;
            this.root_listView.Name = "root_listView";
            this.root_listView.Size = new System.Drawing.Size(418, 342);
            this.root_listView.TabIndex = 0;
            this.root_listView.UseCompatibleStateImageBehavior = false;
            this.root_listView.View = System.Windows.Forms.View.List;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add_toolStripButton,
            this.rem_toolStripButton,
            this.ok_toolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(418, 28);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip";
            // 
            // add_toolStripButton
            // 
            this.add_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.add_toolStripButton.Image = global::directories.Properties.Resources.icon_add;
            this.add_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add_toolStripButton.Name = "add_toolStripButton";
            this.add_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.add_toolStripButton.Text = "Add";
            this.add_toolStripButton.ToolTipText = "Add";
            this.add_toolStripButton.Click += new System.EventHandler(this.add_button_Click);
            // 
            // rem_toolStripButton
            // 
            this.rem_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rem_toolStripButton.Image = global::directories.Properties.Resources.icon_remove;
            this.rem_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rem_toolStripButton.Name = "rem_toolStripButton";
            this.rem_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.rem_toolStripButton.Text = "Remove";
            this.rem_toolStripButton.ToolTipText = "Remove";
            this.rem_toolStripButton.Click += new System.EventHandler(this.rem_button_Click);
            // 
            // ok_toolStripButton
            // 
            this.ok_toolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ok_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ok_toolStripButton.Image = global::directories.Properties.Resources.icon_ok;
            this.ok_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ok_toolStripButton.Name = "ok_toolStripButton";
            this.ok_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.ok_toolStripButton.Text = "OK";
            this.ok_toolStripButton.ToolTipText = "OK";
            this.ok_toolStripButton.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // Share_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 388);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.root_listView);
            this.Name = "Share_Form";
            this.Text = "Share";
            this.Load += new System.EventHandler(this.Share_Form_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Share_From_Closing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView root_listView;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton add_toolStripButton;
        private System.Windows.Forms.ToolStripButton rem_toolStripButton;
        private System.Windows.Forms.ToolStripButton ok_toolStripButton;
    }
}