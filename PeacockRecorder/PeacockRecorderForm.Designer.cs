namespace PeacockRecorder
{
    partial class PeacockRecorderForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PeacockRecorderForm));
            this.buttonStartRecord = new System.Windows.Forms.Button();
            this.buttonStopRecord = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.labelInputDevice = new System.Windows.Forms.Label();
            this.comboBoxInputDevice = new System.Windows.Forms.ComboBox();
            this.labelOutputDevice = new System.Windows.Forms.Label();
            this.comboBoxOutputDevice = new System.Windows.Forms.ComboBox();
            this.notifyIconPeacockRecorder = new System.Windows.Forms.NotifyIcon(this.components);
            this.checkBoxStartHide = new System.Windows.Forms.CheckBox();
            this.buttonOpenRecordingFolder = new System.Windows.Forms.Button();
            this.buttonBrowseSavepath = new System.Windows.Forms.Button();
            this.textBoxSavePath = new System.Windows.Forms.TextBox();
            this.labelRecordingFormat = new System.Windows.Forms.Label();
            this.comboBoxRecordingFormat = new System.Windows.Forms.ComboBox();
            this.checkBoxEnableNotificationSound = new System.Windows.Forms.CheckBox();
            this.labelSavePath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonStartRecord
            // 
            this.buttonStartRecord.Location = new System.Drawing.Point(31, 281);
            this.buttonStartRecord.Name = "buttonStartRecord";
            this.buttonStartRecord.Size = new System.Drawing.Size(75, 23);
            this.buttonStartRecord.TabIndex = 8;
            this.buttonStartRecord.Text = "开始录音(&R)";
            this.buttonStartRecord.UseVisualStyleBackColor = true;
            this.buttonStartRecord.Click += new System.EventHandler(this.buttonStartRecording_Click);
            // 
            // buttonStopRecord
            // 
            this.buttonStopRecord.Enabled = false;
            this.buttonStopRecord.Location = new System.Drawing.Point(111, 281);
            this.buttonStopRecord.Name = "buttonStopRecord";
            this.buttonStopRecord.Size = new System.Drawing.Size(75, 23);
            this.buttonStopRecord.TabIndex = 9;
            this.buttonStopRecord.Text = "停止录音(&S)";
            this.buttonStopRecord.UseVisualStyleBackColor = true;
            this.buttonStopRecord.Click += new System.EventHandler(this.buttonStopRecording_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Enabled = false;
            this.buttonPlay.Location = new System.Drawing.Point(201, 281);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonPlay.TabIndex = 10;
            this.buttonPlay.Text = "开始播放(&P)";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(294, 281);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 11;
            this.buttonStop.Text = "停止播放";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // labelInputDevice
            // 
            this.labelInputDevice.AutoSize = true;
            this.labelInputDevice.Location = new System.Drawing.Point(10, 30);
            this.labelInputDevice.Name = "labelInputDevice";
            this.labelInputDevice.Size = new System.Drawing.Size(53, 12);
            this.labelInputDevice.TabIndex = 0;
            this.labelInputDevice.Text = "输入设备";
            // 
            // comboBoxInputDevice
            // 
            this.comboBoxInputDevice.AccessibleName = "输入设备";
            this.comboBoxInputDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInputDevice.FormattingEnabled = true;
            this.comboBoxInputDevice.Location = new System.Drawing.Point(113, 30);
            this.comboBoxInputDevice.Name = "comboBoxInputDevice";
            this.comboBoxInputDevice.Size = new System.Drawing.Size(272, 20);
            this.comboBoxInputDevice.TabIndex = 0;
            this.comboBoxInputDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxInputDevice_SelectedIndexChanged);
            // 
            // labelOutputDevice
            // 
            this.labelOutputDevice.AutoSize = true;
            this.labelOutputDevice.Location = new System.Drawing.Point(10, 67);
            this.labelOutputDevice.Name = "labelOutputDevice";
            this.labelOutputDevice.Size = new System.Drawing.Size(53, 12);
            this.labelOutputDevice.TabIndex = 1;
            this.labelOutputDevice.Text = "输出设备";
            // 
            // comboBoxOutputDevice
            // 
            this.comboBoxOutputDevice.AccessibleName = "输出设备";
            this.comboBoxOutputDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOutputDevice.FormattingEnabled = true;
            this.comboBoxOutputDevice.Location = new System.Drawing.Point(113, 67);
            this.comboBoxOutputDevice.Name = "comboBoxOutputDevice";
            this.comboBoxOutputDevice.Size = new System.Drawing.Size(272, 20);
            this.comboBoxOutputDevice.TabIndex = 1;
            this.comboBoxOutputDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxOutputDevice_SelectedIndexChanged);
            // 
            // notifyIconPeacockRecorder
            // 
            this.notifyIconPeacockRecorder.BalloonTipText = "孔雀录音机";
            this.notifyIconPeacockRecorder.BalloonTipTitle = "孔雀录音机";
            this.notifyIconPeacockRecorder.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconPeacockRecorder.Icon")));
            this.notifyIconPeacockRecorder.Text = "孔雀录音机";
            this.notifyIconPeacockRecorder.Visible = true;
            this.notifyIconPeacockRecorder.Click += new System.EventHandler(this.notifyIconPeacockRecorder_Click);
            // 
            // checkBoxStartHide
            // 
            this.checkBoxStartHide.AutoSize = true;
            this.checkBoxStartHide.Checked = global::PeacockRecorder.Properties.Settings.Default.StartHide;
            this.checkBoxStartHide.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PeacockRecorder.Properties.Settings.Default, "StartHide", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxStartHide.Location = new System.Drawing.Point(113, 143);
            this.checkBoxStartHide.Name = "checkBoxStartHide";
            this.checkBoxStartHide.Size = new System.Drawing.Size(138, 16);
            this.checkBoxStartHide.TabIndex = 3;
            this.checkBoxStartHide.Text = "启动后隐藏主窗口(&H)";
            this.checkBoxStartHide.UseVisualStyleBackColor = true;
            this.checkBoxStartHide.CheckedChanged += new System.EventHandler(this.checkBoxStartHide_CheckedChanged);
            // 
            // buttonOpenRecordingFolder
            // 
            this.buttonOpenRecordingFolder.Location = new System.Drawing.Point(113, 225);
            this.buttonOpenRecordingFolder.Name = "buttonOpenRecordingFolder";
            this.buttonOpenRecordingFolder.Size = new System.Drawing.Size(130, 23);
            this.buttonOpenRecordingFolder.TabIndex = 7;
            this.buttonOpenRecordingFolder.Text = "打开录音文件夹(&O)";
            this.buttonOpenRecordingFolder.UseVisualStyleBackColor = true;
            this.buttonOpenRecordingFolder.Click += new System.EventHandler(this.buttonOpenRecordingFolder_Click);
            // 
            // buttonBrowseSavepath
            // 
            this.buttonBrowseSavepath.Location = new System.Drawing.Point(315, 183);
            this.buttonBrowseSavepath.Name = "buttonBrowseSavepath";
            this.buttonBrowseSavepath.Size = new System.Drawing.Size(67, 23);
            this.buttonBrowseSavepath.TabIndex = 6;
            this.buttonBrowseSavepath.Text = "浏览(&B)";
            this.buttonBrowseSavepath.UseVisualStyleBackColor = true;
            this.buttonBrowseSavepath.Click += new System.EventHandler(this.buttonSavePath_Click_1);
            // 
            // textBoxSavePath
            // 
            this.textBoxSavePath.AccessibleName = "录音文件保存在";
            this.textBoxSavePath.Location = new System.Drawing.Point(113, 185);
            this.textBoxSavePath.Name = "textBoxSavePath";
            this.textBoxSavePath.ReadOnly = true;
            this.textBoxSavePath.Size = new System.Drawing.Size(200, 21);
            this.textBoxSavePath.TabIndex = 5;
            this.textBoxSavePath.WordWrap = false;
            // 
            // labelRecordingFormat
            // 
            this.labelRecordingFormat.AutoSize = true;
            this.labelRecordingFormat.Location = new System.Drawing.Point(10, 103);
            this.labelRecordingFormat.Name = "labelRecordingFormat";
            this.labelRecordingFormat.Size = new System.Drawing.Size(53, 12);
            this.labelRecordingFormat.TabIndex = 2;
            this.labelRecordingFormat.Text = "录音格式";
            // 
            // comboBoxRecordingFormat
            // 
            this.comboBoxRecordingFormat.AccessibleName = "录音格式";
            this.comboBoxRecordingFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRecordingFormat.FormattingEnabled = true;
            this.comboBoxRecordingFormat.Items.AddRange(new object[] {
            "mp3",
            "wav",
            "wma",
            "ogg"});
            this.comboBoxRecordingFormat.Location = new System.Drawing.Point(113, 103);
            this.comboBoxRecordingFormat.Name = "comboBoxRecordingFormat";
            this.comboBoxRecordingFormat.Size = new System.Drawing.Size(272, 20);
            this.comboBoxRecordingFormat.TabIndex = 2;
            this.comboBoxRecordingFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxRecordingFormat_SelectedIndexChanged);
            // 
            // checkBoxEnableNotificationSound
            // 
            this.checkBoxEnableNotificationSound.AutoSize = true;
            this.checkBoxEnableNotificationSound.Checked = global::PeacockRecorder.Properties.Settings.Default.NotificationSound;
            this.checkBoxEnableNotificationSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableNotificationSound.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PeacockRecorder.Properties.Settings.Default, "NotificationSound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxEnableNotificationSound.Location = new System.Drawing.Point(271, 143);
            this.checkBoxEnableNotificationSound.Name = "checkBoxEnableNotificationSound";
            this.checkBoxEnableNotificationSound.Size = new System.Drawing.Size(114, 16);
            this.checkBoxEnableNotificationSound.TabIndex = 4;
            this.checkBoxEnableNotificationSound.Text = "启用提示音效(&N)";
            this.checkBoxEnableNotificationSound.UseVisualStyleBackColor = true;
            this.checkBoxEnableNotificationSound.CheckedChanged += new System.EventHandler(this.checkBoxEnableNotificationSound_CheckedChanged);
            // 
            // labelSavePath
            // 
            this.labelSavePath.AutoSize = true;
            this.labelSavePath.Location = new System.Drawing.Point(10, 185);
            this.labelSavePath.Name = "labelSavePath";
            this.labelSavePath.Size = new System.Drawing.Size(89, 12);
            this.labelSavePath.TabIndex = 5;
            this.labelSavePath.Text = "录音文件保存在";
            // 
            // PeacockRecorderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.buttonOpenRecordingFolder);
            this.Controls.Add(this.labelSavePath);
            this.Controls.Add(this.buttonBrowseSavepath);
            this.Controls.Add(this.checkBoxEnableNotificationSound);
            this.Controls.Add(this.textBoxSavePath);
            this.Controls.Add(this.comboBoxRecordingFormat);
            this.Controls.Add(this.labelRecordingFormat);
            this.Controls.Add(this.checkBoxStartHide);
            this.Controls.Add(this.comboBoxOutputDevice);
            this.Controls.Add(this.labelOutputDevice);
            this.Controls.Add(this.comboBoxInputDevice);
            this.Controls.Add(this.labelInputDevice);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.buttonStopRecord);
            this.Controls.Add(this.buttonStartRecord);
            this.Name = "PeacockRecorderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "孔雀录音机";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStartRecord;
        private System.Windows.Forms.Button buttonStopRecord;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label labelInputDevice;
        private System.Windows.Forms.ComboBox comboBoxInputDevice;
        private System.Windows.Forms.Label labelOutputDevice;
        private System.Windows.Forms.ComboBox comboBoxOutputDevice;
        private System.Windows.Forms.NotifyIcon notifyIconPeacockRecorder;
        private System.Windows.Forms.CheckBox checkBoxStartHide;
        private System.Windows.Forms.Button buttonBrowseSavepath;
        private System.Windows.Forms.TextBox textBoxSavePath;
        private System.Windows.Forms.Label labelRecordingFormat;
        private System.Windows.Forms.ComboBox comboBoxRecordingFormat;
        private System.Windows.Forms.CheckBox checkBoxEnableNotificationSound;
        private System.Windows.Forms.Button buttonOpenRecordingFolder;
        private System.Windows.Forms.Label labelSavePath;
    }
}

