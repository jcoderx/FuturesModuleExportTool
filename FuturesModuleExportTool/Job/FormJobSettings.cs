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
    public partial class FormJobSettings : Form
    {
        private List<JobTime> jobTimes;

        public FormJobSettings(List<JobTime> jobTimes)
        {
            InitializeComponent();
            this.jobTimes = jobTimes;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormJobTimeAdd formJobTimeAdd = new FormJobTimeAdd();
            if (formJobTimeAdd.ShowDialog() == DialogResult.OK)
            {
                JobTime jobTime = formJobTimeAdd.getResult();
                jobTimes.Add(jobTime);
                refreshTime();
            }
        }

        private void buttonDelCustomPeriod_Click(object sender, EventArgs e)
        {
            int index = this.listBoxTime.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("请选择要删除的定时时间");
                return;
            }
            if (index < jobTimes.Count)
            {
                jobTimes.RemoveAt(index);
                refreshTime();
            }
        }

        private void refreshTime()
        {
            this.listBoxTime.Items.Clear();
            foreach (JobTime jobTime in jobTimes)
            {
                this.listBoxTime.Items.Add(Utils.formatJobTime(jobTime));
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<JobTime> getResult()
        {
            return jobTimes;
        }

        private void FormJobSettings_Load(object sender, EventArgs e)
        {
            refreshTime();
        }
    }
}
