using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FuturesModuleExportTool.Job
{
    public partial class FormJobTimeAdd : Form
    {
        private JobTime jobTime;

        public FormJobTimeAdd()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string hour = textBoxHour.Text.Trim();
            string minute = textBoxMinute.Text.Trim();
            if ("".Equals(hour))
            {
                MessageBox.Show("请输入小时");
                return;
            }

            if ("".Equals(minute))
            {
                MessageBox.Show("请输入分钟");
                return;
            }
            int intHour;
            if(int.TryParse(hour, out intHour))
            {
                if(intHour<0|| intHour > 24)
                {
                    MessageBox.Show("请输入有效的小时");
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入有效的小时");
                return;
            }
            int intMinute;
            if (int.TryParse(minute, out intMinute))
            {
                if (intMinute < 0 || intMinute > 60)
                {
                    MessageBox.Show("请输入有效的分钟");
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入有效的分钟");
                return;
            }
            jobTime = new JobTime();
            jobTime.hour = intHour;
            jobTime.minute = intMinute;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public JobTime getResult()
        {
            return jobTime;
        }
    }
}
