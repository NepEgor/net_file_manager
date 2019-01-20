namespace directories
{
    partial class Settings_Form
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
            this.ok_button = new System.Windows.Forms.Button();
            this.port_label = new System.Windows.Forms.Label();
            this.speed_label = new System.Windows.Forms.Label();
            this.port_comboBox = new System.Windows.Forms.ComboBox();
            this.speed_comboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(149, 111);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(51, 25);
            this.ok_button.TabIndex = 0;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Location = new System.Drawing.Point(12, 40);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(26, 13);
            this.port_label.TabIndex = 2;
            this.port_label.Text = "Port";
            // 
            // speed_label
            // 
            this.speed_label.AutoSize = true;
            this.speed_label.Location = new System.Drawing.Point(12, 67);
            this.speed_label.Name = "speed_label";
            this.speed_label.Size = new System.Drawing.Size(38, 13);
            this.speed_label.TabIndex = 4;
            this.speed_label.Text = "Speed";
            // 
            // port_comboBox
            // 
            this.port_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.port_comboBox.FormattingEnabled = true;
            this.port_comboBox.Location = new System.Drawing.Point(53, 37);
            this.port_comboBox.Name = "port_comboBox";
            this.port_comboBox.Size = new System.Drawing.Size(147, 21);
            this.port_comboBox.TabIndex = 1;
            // 
            // speed_comboBox
            // 
            this.speed_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.speed_comboBox.Location = new System.Drawing.Point(53, 64);
            this.speed_comboBox.Name = "speed_comboBox";
            this.speed_comboBox.Size = new System.Drawing.Size(147, 21);
            this.speed_comboBox.TabIndex = 2;
            // 
            // Settings_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 151);
            this.Controls.Add(this.speed_comboBox);
            this.Controls.Add(this.port_comboBox);
            this.Controls.Add(this.speed_label);
            this.Controls.Add(this.port_label);
            this.Controls.Add(this.ok_button);
            this.Name = "Settings_Form";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Form_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_Form_Close);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.Label speed_label;
        private System.Windows.Forms.ComboBox port_comboBox;
        private System.Windows.Forms.ComboBox speed_comboBox;
    }
}