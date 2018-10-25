using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static FuturesModuleExportTool.WindowsApi;

namespace FuturesModuleExportTool
{
    public partial class FormMain : Form
    {
        public const string DIALOG_TITLE = "期货运行模组";

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            reset();
            getAllWnd();
        }

        private void buttonOpenDir_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Utils.getExportDir());
        }

        private void reset()
        {
            this.textBoxLog.Clear();
            logAppendTextLine("欢迎使用期货运行模组导出工具...");
        }

        private List<IntPtr> getAllListViewHandle(IntPtr hWnd)
        {
            List<IntPtr> result = new List<IntPtr>();

            List<IntPtr> partitions = new List<IntPtr>();
            EnumChildWindows(hWnd, (h, l) =>
            {
                if (GetParent(h) == hWnd)
                {
                    StringBuilder className = new StringBuilder(200);
                    int len;
                    len = GetClassName(h, className, 200);
                    if (CLASS_DIALOG.Equals(className.ToString()))
                    {
                        partitions.Add(h);
                    }
                }
                return true;
            }, 0);

            foreach (IntPtr partitionDialogHandler in partitions)
            {
                IntPtr firstListViewHandle = FindWindowEx(partitionDialogHandler, IntPtr.Zero, "SysListView32", null);
                IntPtr wantListViewHandle = FindWindowEx(partitionDialogHandler, firstListViewHandle, "SysListView32", null);
                result.Add(wantListViewHandle);
            }
            if (result.Count == 0)
            {
                logAppendTextLine("未搜索到分区。");
                return result;
            }
            logAppendTextLine("共搜索到" + result.Count + "个分区（界面仅展示" + (result.Count - 1) + "个分区）");
            return result;
        }

        //打印日志
        private void logAppendTextLine(string s)
        {
            this.textBoxLog.AppendText(s + "\r\n");
        }

        //打印日志的分割线
        private void logLine()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 274; i++)
            {
                sb.Append("-");
            }
            this.textBoxLog.AppendText(sb.ToString() + "\r\n");
        }

        //获取所有的进程对应的processId、程序目录
        private void getAllWnd()
        {
            List<IntPtr> wndHandles = new List<IntPtr>(); //所有"期货运行模组"窗口句柄
            EnumWindows((h, l) =>
            {
                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);

                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (CLASS_DIALOG.Equals(className.ToString()) && DIALOG_TITLE.Equals(title.ToString()))
                {
                    wndHandles.Add(h);
                }
                return true;
            }, 0);

            if (wndHandles == null || wndHandles.Count == 0)
            {
                logAppendTextLine("没有找到“期货运行模块组”界面");
                return;
            }
            //找到所有程序路径
            List<string> allPaths = new List<string>();
            foreach (IntPtr wnd in wndHandles)
            {
                int processId;
                GetWindowThreadProcessId(wnd, out processId);
                Process process = Process.GetProcessById(processId);
                string fileName = process.MainModule.FileName;
                allPaths.Add(Utils.cutDirName(fileName));
            }

            DialogChooseClient dialog = new DialogChooseClient(allPaths);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                List<int> indexes = dialog.getSelectedIndexes();
                foreach (int i in indexes)
                {
                    IntPtr treeViewHandle = getSysTreeView32Handle(wndHandles[i]);
                    if (treeViewHandle == IntPtr.Zero)
                    {
                        logAppendTextLine("未找到分区。");
                        continue;
                    }

                    logAppendTextLine("模拟点击分区，此过程比较耗时，请耐心等候...");
                    SysTreeview32Utils.ClickTreeViewBaseNodes((IntPtr)treeViewHandle);
                    //开始导出
                    logAppendTextLine(allPaths[i] + "开始查找分区...");
                    List<IntPtr> listViewHandles = getAllListViewHandle(wndHandles[i]);
                    if (listViewHandles == null || listViewHandles.Count == 0)
                    {
                        continue;
                    }
                    //刷新
                    foreach (IntPtr h in listViewHandles)
                    {
                        bool x = UpdateWindow(wndHandles[i]);
                        Console.WriteLine("x:::" + x);
                    }
                    collectionData(listViewHandles, allPaths[i]);
                }
                MessageBox.Show("导出Excel完成！");
            }
        }

        private void collectionData(List<IntPtr> listViewHandles, string dirName)
        {
            logAppendTextLine("开始获取各个分区数据...");
            List<string[,]> allData = new List<string[,]>();
            for (int n = 0; n < listViewHandles.Count; n++)
            {
                IntPtr listViewHandle = listViewHandles[n];
                logAppendTextLine("第" + n + "个分区数据如下：");
                logLine();

                int rows = 0;
                int cols = 0;
                string[,] tempStr = SysListView32Utils.ListView_GetItemValues(listViewHandle, out rows, out cols);

                if (tempStr == null)
                {
                    logAppendTextLine("获取数据失败!!!!!");
                    return;
                }

                //for log print
                for (int i = 0; i < rows; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < cols; j++)
                    {
                        string result = tempStr[i, j];
                        if (result == string.Empty)
                        {
                            result = "@";
                        }
                        sb.Append(result + "\t");
                    }
                    logAppendTextLine(sb.ToString());
                }
                //导出原始数据
                //new ExcelExport().exportExcel(tempStr);

                //for export excel，去掉最后一行合计
                if (rows > 0)
                {
                    string[,] temp = new string[rows - 1, cols];
                    for (int i = 0; i < rows - 1; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            temp[i, j] = tempStr[i, j];
                        }
                    }
                    allData.Add(temp);
                }
            }
            logAppendTextLine("开始导出excel...");
            new ExcelExport().exportExcel(allData, dirName);
            logAppendTextLine("导出excel完成，保存在" + Utils.getExportDir() + "目录下。");
        }

        private IntPtr getSysTreeView32Handle(IntPtr hWnd)
        {
            return FindWindowEx(hWnd, IntPtr.Zero, "SysTreeView32", null);
        }
    }
}
