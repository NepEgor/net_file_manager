namespace directories
{
    partial class Main_Form
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dir_listView = new System.Windows.Forms.ListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.top_toolStrip = new System.Windows.Forms.ToolStrip();
            this.back_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.home_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.path_toolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.update_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.share_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.settings_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.info_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.bottom_toolStrip = new System.Windows.Forms.ToolStrip();
            this.status_toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.status_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.top_toolStrip.SuspendLayout();
            this.bottom_toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dir_listView
            // 
            this.dir_listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.dir_listView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dir_listView.Location = new System.Drawing.Point(0, 32);
            this.dir_listView.MultiSelect = false;
            this.dir_listView.Name = "dir_listView";
            this.dir_listView.Size = new System.Drawing.Size(576, 340);
            this.dir_listView.TabIndex = 1;
            this.dir_listView.TileSize = new System.Drawing.Size(50, 50);
            this.dir_listView.UseCompatibleStateImageBehavior = false;
            this.dir_listView.DoubleClick += new System.EventHandler(this.dir_listView_DoubleClick);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // top_toolStrip
            // 
            this.top_toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.top_toolStrip.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.top_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.back_toolStripButton,
            this.home_toolStripButton,
            this.path_toolStripTextBox,
            this.update_toolStripButton,
            this.toolStripSeparator1,
            this.share_toolStripButton,
            this.settings_toolStripButton,
            this.toolStripSeparator2,
            this.info_toolStripButton});
            this.top_toolStrip.Location = new System.Drawing.Point(0, 0);
            this.top_toolStrip.Margin = new System.Windows.Forms.Padding(3);
            this.top_toolStrip.Name = "top_toolStrip";
            this.top_toolStrip.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.top_toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.top_toolStrip.Size = new System.Drawing.Size(576, 28);
            this.top_toolStrip.TabIndex = 0;
            this.top_toolStrip.Text = "toolStrip";
            // 
            // back_toolStripButton
            // 
            this.back_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.back_toolStripButton.Image = global::directories.Properties.Resources.icon_back;
            this.back_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.back_toolStripButton.Name = "back_toolStripButton";
            this.back_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.back_toolStripButton.Text = "toolStripButton1";
            this.back_toolStripButton.ToolTipText = "Back";
            this.back_toolStripButton.Click += new System.EventHandler(this.back_button_Click);
            // 
            // home_toolStripButton
            // 
            this.home_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.home_toolStripButton.Image = global::directories.Properties.Resources.icon_home;
            this.home_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.home_toolStripButton.Name = "home_toolStripButton";
            this.home_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.home_toolStripButton.Text = "toolStripButton2";
            this.home_toolStripButton.ToolTipText = "Home";
            this.home_toolStripButton.Click += new System.EventHandler(this.home_button_Click);
            // 
            // path_toolStripTextBox
            // 
            this.path_toolStripTextBox.AutoSize = false;
            this.path_toolStripTextBox.Name = "path_toolStripTextBox";
            this.path_toolStripTextBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.path_toolStripTextBox.Size = new System.Drawing.Size(200, 25);
            // 
            // update_toolStripButton
            // 
            this.update_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.update_toolStripButton.Image = global::directories.Properties.Resources.icon_update;
            this.update_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.update_toolStripButton.Name = "update_toolStripButton";
            this.update_toolStripButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.update_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.update_toolStripButton.Text = "toolStripButton3";
            this.update_toolStripButton.ToolTipText = "Update";
            this.update_toolStripButton.Click += new System.EventHandler(this.update_button_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 26);
            // 
            // share_toolStripButton
            // 
            this.share_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.share_toolStripButton.Image = global::directories.Properties.Resources.icon_share;
            this.share_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.share_toolStripButton.Name = "share_toolStripButton";
            this.share_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.share_toolStripButton.Text = "toolStripButton4";
            this.share_toolStripButton.ToolTipText = "Share";
            this.share_toolStripButton.Click += new System.EventHandler(this.share_button_Click);
            // 
            // settings_toolStripButton
            // 
            this.settings_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settings_toolStripButton.Image = global::directories.Properties.Resources.icon_settings;
            this.settings_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settings_toolStripButton.Name = "settings_toolStripButton";
            this.settings_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.settings_toolStripButton.Text = "toolStripButton5";
            this.settings_toolStripButton.ToolTipText = "Settings";
            this.settings_toolStripButton.Click += new System.EventHandler(this.settings_button_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 26);
            // 
            // info_toolStripButton
            // 
            this.info_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.info_toolStripButton.Image = global::directories.Properties.Resources.icon_info;
            this.info_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.info_toolStripButton.Name = "info_toolStripButton";
            this.info_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.info_toolStripButton.Text = "toolStripButton1";
            this.info_toolStripButton.ToolTipText = "About";
            this.info_toolStripButton.Click += new System.EventHandler(this.info_toolStripButton_Click);
            // 
            // bottom_toolStrip
            // 
            this.bottom_toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottom_toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.bottom_toolStrip.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.bottom_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status_toolStripProgressBar,
            this.status_toolStripButton});
            this.bottom_toolStrip.Location = new System.Drawing.Point(0, 373);
            this.bottom_toolStrip.Margin = new System.Windows.Forms.Padding(3);
            this.bottom_toolStrip.Name = "bottom_toolStrip";
            this.bottom_toolStrip.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.bottom_toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.bottom_toolStrip.Size = new System.Drawing.Size(576, 28);
            this.bottom_toolStrip.TabIndex = 2;
            this.bottom_toolStrip.Text = "toolStrip1";
            // 
            // status_toolStripProgressBar
            // 
            this.status_toolStripProgressBar.AutoSize = false;
            this.status_toolStripProgressBar.Name = "status_toolStripProgressBar";
            this.status_toolStripProgressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.status_toolStripProgressBar.Size = new System.Drawing.Size(100, 23);
            // 
            // status_toolStripButton
            // 
            this.status_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.status_toolStripButton.Image = global::directories.Properties.Resources.icon_disconnected;
            this.status_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.status_toolStripButton.Name = "status_toolStripButton";
            this.status_toolStripButton.Size = new System.Drawing.Size(23, 23);
            this.status_toolStripButton.ToolTipText = "Disconnected";
            this.status_toolStripButton.Click += new System.EventHandler(this.status_toolStripButton_Click);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 401);
            this.Controls.Add(this.bottom_toolStrip);
            this.Controls.Add(this.top_toolStrip);
            this.Controls.Add(this.dir_listView);
            this.Name = "Main_Form";
            this.Text = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Form_Closing);
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.Shown += new System.EventHandler(this.Main_Form_Shown);
            this.top_toolStrip.ResumeLayout(false);
            this.top_toolStrip.PerformLayout();
            this.bottom_toolStrip.ResumeLayout(false);
            this.bottom_toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView dir_listView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStrip top_toolStrip;
        private System.Windows.Forms.ToolStripButton back_toolStripButton;
        private System.Windows.Forms.ToolStripButton home_toolStripButton;
        private System.Windows.Forms.ToolStripTextBox path_toolStripTextBox;
        private System.Windows.Forms.ToolStripButton update_toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton share_toolStripButton;
        private System.Windows.Forms.ToolStripButton settings_toolStripButton;
        private System.Windows.Forms.ToolStrip bottom_toolStrip;
        private System.Windows.Forms.ToolStripProgressBar status_toolStripProgressBar;
        private System.Windows.Forms.ToolStripButton status_toolStripButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton info_toolStripButton;
        //private System.IO.Ports.SerialPort serialPort;
    }
}

