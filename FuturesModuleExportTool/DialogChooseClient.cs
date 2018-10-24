using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FuturesModuleExportTool
{
    public partial class DialogChooseClient : Form
    {
        public DialogChooseClient(List<string> items)
        {
            InitializeComponent();
            foreach (string item in items)
            {
                this.clbClient.Items.Add(item);
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            List<int> result = getSelectedIndexes();
            if (result.Count == 0)
            {
                MessageBox.Show("请选择你要导出的客户端");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        public List<int> getSelectedIndexes()
        {
            List<int> result = new List<int>();
            for (int i = 0; i < this.clbClient.Items.Count; i++)
            {
                if (this.clbClient.GetItemChecked(i))
                {
                    result.Add(i);
                }
            }
            return result;
        }
    }
}
