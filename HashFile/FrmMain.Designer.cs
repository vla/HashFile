namespace HasFile {
    partial class FrmMain {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose ( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent () {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.TxtView = new System.Windows.Forms.TextBox();
            this.BarTime = new System.Windows.Forms.ProgressBar();
            this.BarCount = new System.Windows.Forms.ProgressBar();
            this.CmdBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CmdClear = new System.Windows.Forms.Button();
            this.CmdCopy = new System.Windows.Forms.Button();
            this.CmdSave = new System.Windows.Forms.Button();
            this.ChkTime = new System.Windows.Forms.CheckBox();
            this.ChkSHA1 = new System.Windows.Forms.CheckBox();
            this.ChkMD5 = new System.Windows.Forms.CheckBox();
            this.ChkCRC32 = new System.Windows.Forms.CheckBox();
            this.CmdStop = new System.Windows.Forms.Button();
            this.CmdClose = new System.Windows.Forms.Button();
            this.TxtSeed = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ChkNumber = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // TxtView
            // 
            this.TxtView.Location = new System.Drawing.Point(7, 5);
            this.TxtView.Multiline = true;
            this.TxtView.Name = "TxtView";
            this.TxtView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtView.Size = new System.Drawing.Size(570, 289);
            this.TxtView.TabIndex = 0;
            // 
            // BarTime
            // 
            this.BarTime.Location = new System.Drawing.Point(53, 367);
            this.BarTime.Name = "BarTime";
            this.BarTime.Size = new System.Drawing.Size(453, 16);
            this.BarTime.Step = 1;
            this.BarTime.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.BarTime.TabIndex = 1;
            // 
            // BarCount
            // 
            this.BarCount.Location = new System.Drawing.Point(53, 390);
            this.BarCount.Name = "BarCount";
            this.BarCount.Size = new System.Drawing.Size(453, 16);
            this.BarCount.Step = 1;
            this.BarCount.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.BarCount.TabIndex = 1;
            // 
            // CmdBrowse
            // 
            this.CmdBrowse.Location = new System.Drawing.Point(11, 303);
            this.CmdBrowse.Name = "CmdBrowse";
            this.CmdBrowse.Size = new System.Drawing.Size(75, 23);
            this.CmdBrowse.TabIndex = 2;
            this.CmdBrowse.Text = "浏览(&B)";
            this.CmdBrowse.UseVisualStyleBackColor = true;
            this.CmdBrowse.Click += new System.EventHandler(this.CmdBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 390);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "完成：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "进度：";
            // 
            // CmdClear
            // 
            this.CmdClear.Location = new System.Drawing.Point(94, 303);
            this.CmdClear.Name = "CmdClear";
            this.CmdClear.Size = new System.Drawing.Size(75, 23);
            this.CmdClear.TabIndex = 2;
            this.CmdClear.Text = "清除(&L)";
            this.CmdClear.UseVisualStyleBackColor = true;
            this.CmdClear.Click += new System.EventHandler(this.CmdClear_Click);
            // 
            // CmdCopy
            // 
            this.CmdCopy.Location = new System.Drawing.Point(177, 303);
            this.CmdCopy.Name = "CmdCopy";
            this.CmdCopy.Size = new System.Drawing.Size(75, 23);
            this.CmdCopy.TabIndex = 2;
            this.CmdCopy.Text = "复制(&C)";
            this.CmdCopy.UseVisualStyleBackColor = true;
            this.CmdCopy.Click += new System.EventHandler(this.CmdCopy_Click);
            // 
            // CmdSave
            // 
            this.CmdSave.Location = new System.Drawing.Point(260, 303);
            this.CmdSave.Name = "CmdSave";
            this.CmdSave.Size = new System.Drawing.Size(75, 23);
            this.CmdSave.TabIndex = 2;
            this.CmdSave.Text = "保存(&S)";
            this.CmdSave.UseVisualStyleBackColor = true;
            this.CmdSave.Click += new System.EventHandler(this.CmdSave_Click);
            // 
            // ChkTime
            // 
            this.ChkTime.AutoSize = true;
            this.ChkTime.Checked = true;
            this.ChkTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkTime.Location = new System.Drawing.Point(524, 303);
            this.ChkTime.Name = "ChkTime";
            this.ChkTime.Size = new System.Drawing.Size(48, 16);
            this.ChkTime.TabIndex = 4;
            this.ChkTime.Text = "时间";
            this.ChkTime.UseVisualStyleBackColor = true;
            // 
            // ChkSHA1
            // 
            this.ChkSHA1.AutoSize = true;
            this.ChkSHA1.Checked = true;
            this.ChkSHA1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkSHA1.Location = new System.Drawing.Point(524, 347);
            this.ChkSHA1.Name = "ChkSHA1";
            this.ChkSHA1.Size = new System.Drawing.Size(48, 16);
            this.ChkSHA1.TabIndex = 4;
            this.ChkSHA1.Text = "SHA1";
            this.ChkSHA1.UseVisualStyleBackColor = true;
            // 
            // ChkMD5
            // 
            this.ChkMD5.AutoSize = true;
            this.ChkMD5.Checked = true;
            this.ChkMD5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkMD5.Location = new System.Drawing.Point(524, 325);
            this.ChkMD5.Name = "ChkMD5";
            this.ChkMD5.Size = new System.Drawing.Size(42, 16);
            this.ChkMD5.TabIndex = 4;
            this.ChkMD5.Text = "MD5";
            this.ChkMD5.UseVisualStyleBackColor = true;
            // 
            // ChkCRC32
            // 
            this.ChkCRC32.AutoSize = true;
            this.ChkCRC32.Location = new System.Drawing.Point(524, 369);
            this.ChkCRC32.Name = "ChkCRC32";
            this.ChkCRC32.Size = new System.Drawing.Size(54, 16);
            this.ChkCRC32.TabIndex = 4;
            this.ChkCRC32.Text = "CRC32";
            this.ChkCRC32.UseVisualStyleBackColor = true;
            // 
            // CmdStop
            // 
            this.CmdStop.Location = new System.Drawing.Point(343, 303);
            this.CmdStop.Name = "CmdStop";
            this.CmdStop.Size = new System.Drawing.Size(75, 23);
            this.CmdStop.TabIndex = 2;
            this.CmdStop.Text = "停止(&T)";
            this.CmdStop.UseVisualStyleBackColor = true;
            this.CmdStop.Click += new System.EventHandler(this.CmdStop_Click);
            // 
            // CmdClose
            // 
            this.CmdClose.Location = new System.Drawing.Point(425, 303);
            this.CmdClose.Name = "CmdClose";
            this.CmdClose.Size = new System.Drawing.Size(75, 23);
            this.CmdClose.TabIndex = 2;
            this.CmdClose.Text = "关闭(&X)";
            this.CmdClose.UseVisualStyleBackColor = true;
            this.CmdClose.Click += new System.EventHandler(this.CmdClose_Click);
            // 
            // TxtSeed
            // 
            this.TxtSeed.Location = new System.Drawing.Point(72, 337);
            this.TxtSeed.Name = "TxtSeed";
            this.TxtSeed.Size = new System.Drawing.Size(307, 21);
            this.TxtSeed.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 342);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "SHA1种子：";
            // 
            // ChkNumber
            // 
            this.ChkNumber.AutoSize = true;
            this.ChkNumber.Location = new System.Drawing.Point(402, 338);
            this.ChkNumber.Name = "ChkNumber";
            this.ChkNumber.Size = new System.Drawing.Size(72, 16);
            this.ChkNumber.TabIndex = 4;
            this.ChkNumber.Text = "只留数字";
            this.ChkNumber.UseVisualStyleBackColor = true;
            // 
            // FrmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 415);
            this.Controls.Add(this.TxtSeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CmdClose);
            this.Controls.Add(this.CmdStop);
            this.Controls.Add(this.CmdSave);
            this.Controls.Add(this.CmdCopy);
            this.Controls.Add(this.CmdClear);
            this.Controls.Add(this.CmdBrowse);
            this.Controls.Add(this.BarCount);
            this.Controls.Add(this.BarTime);
            this.Controls.Add(this.TxtView);
            this.Controls.Add(this.ChkNumber);
            this.Controls.Add(this.ChkCRC32);
            this.Controls.Add(this.ChkMD5);
            this.Controls.Add(this.ChkSHA1);
            this.Controls.Add(this.ChkTime);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Hasher - 0.1 By  v.la@live.cn";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtView;
        private System.Windows.Forms.ProgressBar BarTime;
        private System.Windows.Forms.ProgressBar BarCount;
        private System.Windows.Forms.Button CmdBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CmdClear;
        private System.Windows.Forms.Button CmdCopy;
        private System.Windows.Forms.Button CmdSave;
        private System.Windows.Forms.CheckBox ChkTime;
        private System.Windows.Forms.CheckBox ChkSHA1;
        private System.Windows.Forms.CheckBox ChkMD5;
        private System.Windows.Forms.CheckBox ChkCRC32;
        private System.Windows.Forms.Button CmdStop;
        private System.Windows.Forms.Button CmdClose;
        private System.Windows.Forms.TextBox TxtSeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ChkNumber;
    }
}

