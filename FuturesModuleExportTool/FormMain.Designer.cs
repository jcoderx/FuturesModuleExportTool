namespace FuturesModuleExportTool
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonOpenDir = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemMainForm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemJobs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonJobs = new System.Windows.Forms.Button();
            this.contextMenuStripNotify.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOpenDir
            // 
            this.buttonOpenDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenDir.Location = new System.Drawing.Point(801, 6);
            this.buttonOpenDir.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOpenDir.Name = "buttonOpenDir";
            this.buttonOpenDir.Size = new System.Drawing.Size(91, 42);
            this.buttonOpenDir.TabIndex = 7;
            this.buttonOpenDir.Text = "打开excel目录";
            this.buttonOpenDir.UseVisualStyleBackColor = true;
            this.buttonOpenDir.Click += new System.EventHandler(this.buttonOpenDir_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.BackColor = System.Drawing.Color.Black;
            this.textBoxLog.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(220)))), ((int)(((byte)(0)))));
            this.textBoxLog.Location = new System.Drawing.Point(-1, 54);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLog.Size = new System.Drawing.Size(1020, 464);
            this.textBoxLog.TabIndex = 6;
            this.textBoxLog.Text = "欢迎使用期货运行模组导出工具...";
            this.textBoxLog.WordWrap = false;
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(691, 6);
            this.buttonExport.Margin = new System.Windows.Forms.Padding(2);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(91, 42);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "开始导出";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(683, 42);
            this.label1.TabIndex = 4;
            this.label1.Text = "警告：\r\n1.导出数据之前，打开各个客户端的“期货运行模组”界面，并确保各个分区各个模组数据加载完成。\r\n2.该工具仅用于“智赢程序化”软件Ver8.3.472版" +
    "本。";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "期货运行模组导出工具";
            this.notifyIcon.BalloonTipTitle = "想否";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStripNotify;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "期货运行模组导出工具";
            this.notifyIcon.Visible = true;
            // 
            // contextMenuStripNotify
            // 
            this.contextMenuStripNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemMainForm,
            this.toolStripMenuItemJobs,
            this.toolStripSeparator1,
            this.toolStripMenuItemExit});
            this.contextMenuStripNotify.Name = "contextMenuStripNotify";
            this.contextMenuStripNotify.Size = new System.Drawing.Size(149, 76);
            // 
            // toolStripMenuItemMainForm
            // 
            this.toolStripMenuItemMainForm.Name = "toolStripMenuItemMainForm";
            this.toolStripMenuItemMainForm.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemMainForm.Text = "主界面";
            this.toolStripMenuItemMainForm.Click += new System.EventHandler(this.toolStripMenuItemMainForm_Click);
            // 
            // toolStripMenuItemJobs
            // 
            this.toolStripMenuItemJobs.Name = "toolStripMenuItemJobs";
            this.toolStripMenuItemJobs.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemJobs.Text = "编辑定时任务";
            this.toolStripMenuItemJobs.Click += new System.EventHandler(this.toolStripMenuItemJobs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemExit.Text = "退出";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            // 
            // buttonJobs
            // 
            this.buttonJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonJobs.Location = new System.Drawing.Point(911, 6);
            this.buttonJobs.Margin = new System.Windows.Forms.Padding(2);
            this.buttonJobs.Name = "buttonJobs";
            this.buttonJobs.Size = new System.Drawing.Size(91, 42);
            this.buttonJobs.TabIndex = 9;
            this.buttonJobs.Text = "编辑定时任务";
            this.buttonJobs.UseVisualStyleBackColor = true;
            this.buttonJobs.Click += new System.EventHandler(this.buttonJobs_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 522);
            this.Controls.Add(this.buttonJobs);
            this.Controls.Add(this.buttonOpenDir);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(923, 560);
            this.Name = "FormMain";
            this.Text = "期货运行模组导出工具";
            this.contextMenuStripNotify.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenDir;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNotify;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMainForm;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemJobs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.Button buttonJobs;
    }
}