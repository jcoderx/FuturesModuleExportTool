namespace FuturesModuleExportTool.Job
{
    partial class FormJobSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobSettings));
            this.buttonDelCustomPeriod = new System.Windows.Forms.Button();
            this.listBoxTime = new System.Windows.Forms.ListBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonDelCustomPeriod
            // 
            this.buttonDelCustomPeriod.Location = new System.Drawing.Point(235, 121);
            this.buttonDelCustomPeriod.Name = "buttonDelCustomPeriod";
            this.buttonDelCustomPeriod.Size = new System.Drawing.Size(79, 23);
            this.buttonDelCustomPeriod.TabIndex = 50;
            this.buttonDelCustomPeriod.Text = "删除";
            this.buttonDelCustomPeriod.UseVisualStyleBackColor = true;
            this.buttonDelCustomPeriod.Click += new System.EventHandler(this.buttonDelCustomPeriod_Click);
            // 
            // listBoxTime
            // 
            this.listBoxTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxTime.Font = new System.Drawing.Font("宋体", 11F);
            this.listBoxTime.FormattingEnabled = true;
            this.listBoxTime.ItemHeight = 15;
            this.listBoxTime.Location = new System.Drawing.Point(12, 12);
            this.listBoxTime.Name = "listBoxTime";
            this.listBoxTime.Size = new System.Drawing.Size(209, 184);
            this.listBoxTime.TabIndex = 48;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(235, 26);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(117, 23);
            this.buttonAdd.TabIndex = 49;
            this.buttonAdd.Text = "添加定时时间";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(93, 218);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(210, 23);
            this.buttonOK.TabIndex = 51;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // FormJobSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 253);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonDelCustomPeriod);
            this.Controls.Add(this.listBoxTime);
            this.Controls.Add(this.buttonAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormJobSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑定时任务";
            this.Load += new System.EventHandler(this.FormJobSettings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDelCustomPeriod;
        private System.Windows.Forms.ListBox listBoxTime;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonOK;
    }
}