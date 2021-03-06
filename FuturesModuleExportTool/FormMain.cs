﻿using FuturesModuleExportTool.Job;
using FuturesModuleExportTool.Page;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Automation;
using System.Windows.Forms;
using static FuturesModuleExportTool.WindowsApi;

namespace FuturesModuleExportTool
{
    public partial class FormMain : Form
    {
        public const string DIALOG_TITLE = "期货运行模组";
        public List<JobTime> jobTimes;
        public const string FILE_JOB_TIME = "job_time.txt";

        public FormMain()
        {
            InitializeComponent();
            initTimer();
            jobTimes = Utils.readJobTimeFile(FILE_JOB_TIME);
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
            appendLog(s + "\r\n");
        }

        //打印日志的分割线
        private void logLine()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 274; i++)
            {
                sb.Append("-");
            }
            appendLog(sb.ToString() + "\r\n");
        }

        public void appendLog(string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() => { appendLog(message); }));
                }
                else
                {
                    this.textBoxLog.AppendText(message);
                }
            }
            catch { }
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

            //记录模拟点击失败的客户端
            List<string> mockFailClients = new List<string>();
            DialogChooseClient dialog = new DialogChooseClient(allPaths);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                List<int> indexes = dialog.getSelectedIndexes();
                foreach (int i in indexes)
                {
                    //IntPtr treeViewHandle = getSysTreeView32Handle(wndHandles[i]);
                    //if (treeViewHandle == IntPtr.Zero)
                    //{
                    //    logAppendTextLine("未找到分区。");
                    //    continue;
                    //}

                    logAppendTextLine("模拟点击分区，此过程比较耗时，请耐心等候...");
                    //SysTreeview32Utils.ClickTreeViewBaseNodes((IntPtr)treeViewHandle);
                    string clickTreeViewMsg;
                    bool result = TreeViewUtils.clickTreeViewRootNode(wndHandles[i], out clickTreeViewMsg);
                    if (!result)
                    {
                        mockFailClients.Add(allPaths[i]);
                        if (clickTreeViewMsg != null)
                        {
                            logAppendTextLine(allPaths[i] + ":模拟点击分区失败,原因：" + clickTreeViewMsg);
                        }
                    }
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
                if (mockFailClients.Count > 0)
                {
                    logAppendTextLine("重要：：：：：：：：：：：：：：：：");
                    StringBuilder sb = new StringBuilder();
                    foreach (string c in mockFailClients)
                    {
                        sb.Append(c).Append(",");
                    }
                    logAppendTextLine(sb.ToString() + "客户端模拟点击分区失败，可能数据有误，请单独重新导出！！！！");
                }
                this.TopMost = true;
                MessageBox.Show("导出Excel完成！");
                this.TopMost = false;
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

        private void toolStripMenuItemMainForm_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void toolStripMenuItemJobs_Click(object sender, EventArgs e)
        {
            if (jobTimes == null)
            {
                jobTimes = new List<JobTime>();
            }
            FormJobSettings formJobSettings = new FormJobSettings(jobTimes);
            if (formJobSettings.ShowDialog() == DialogResult.OK)
            {
                jobTimes = formJobSettings.getResult();
                Utils.writeJobTimeFile(FILE_JOB_TIME, jobTimes);
            }
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonJobs_Click(object sender, EventArgs e)
        {
            if (jobTimes == null)
            {
                jobTimes = new List<JobTime>();
            }
            FormJobSettings formJobSettings = new FormJobSettings(jobTimes);
            if (formJobSettings.ShowDialog() == DialogResult.OK)
            {
                jobTimes = formJobSettings.getResult();
                Utils.writeJobTimeFile(FILE_JOB_TIME, jobTimes);
            }
        }

        private void initTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 1000; //执行间隔时间,单位为毫秒; 这里实际间隔为1秒  
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(jobTask);
        }

        private void jobTask(object source, ElapsedEventArgs e)
        {
            if (triggerJob())
            {
                Console.WriteLine("triggerJobtriggerJobtriggerJobtriggerJob");
                //1.保存模组
                saveModel();
                //2.导出报表
                exportJob();
            }
        }

        private void saveModel()
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
            for (int i = 0; i < wndHandles.Count; i++)
            {
                WindowsApiUtils.clearOtherWindows(wndHandles[i], null);
                string clickTreeViewMsg;
                bool result = TreeViewUtils.clickTreeViewRootNode(wndHandles[i], out clickTreeViewMsg);
                saveModel(wndHandles[i]);
            }
        }

        private void exportJob()
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

            //记录模拟点击失败的客户端
            List<string> mockFailClients = new List<string>();

            for (int i = 0; i < wndHandles.Count; i++)
            {
                WindowsApiUtils.clearOtherWindows(wndHandles[i], null);
                //IntPtr treeViewHandle = getSysTreeView32Handle(wndHandles[i]);
                //if (treeViewHandle == IntPtr.Zero)
                //{
                //    logAppendTextLine("未找到分区。");
                //    continue;
                //}

                logAppendTextLine("模拟点击分区，此过程比较耗时，请耐心等候...");
                //SysTreeview32Utils.ClickTreeViewBaseNodes((IntPtr)treeViewHandle);
                string clickTreeViewMsg;
                bool result = TreeViewUtils.clickTreeViewRootNode(wndHandles[i], out clickTreeViewMsg);
                if (!result)
                {
                    mockFailClients.Add(allPaths[i]);
                    if (clickTreeViewMsg != null)
                    {
                        logAppendTextLine(allPaths[i] + ":模拟点击分区失败,原因：" + clickTreeViewMsg);
                    }
                }
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
            if (mockFailClients.Count > 0)
            {
                logAppendTextLine("重要：：：：：：：：：：：：：：：：");
                StringBuilder sb = new StringBuilder();
                foreach (string c in mockFailClients)
                {
                    sb.Append(c).Append(",");
                }
                logAppendTextLine(sb.ToString() + "客户端模拟点击分区失败，可能数据有误，请单独重新导出！！！！");
            }
        }

        private bool triggerJob()
        {
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;
            if (jobTimes == null || jobTimes.Count == 0)
            {
                return false;
            }
            foreach (JobTime jobTime in jobTimes)
            {
                if (jobTime.hour == hour && jobTime.minute == minute && jobTime.second == second)
                {
                    return true;
                }
            }
            return false;
        }

        private bool saveModel(IntPtr mainHandle)
        {
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow == null)
            {

                logAppendTextLine("未找到“期货运行模组”界面");
                return false;
            }
            WindowsApiUtils.clearOtherWindows(mainHandle, null);
            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, ModelPage.AUTOMATION_ID_MENU_BAR);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuBar);
            AutomationElement menuBar = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (menuBar == null)
            {
                logAppendTextLine("未找到“期货运行模组”界面中的菜单栏");
                return false;
            }
            PropertyCondition condition2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem);
            AutomationElementCollection menuElementCollection = menuBar.FindAll(TreeScope.Children, condition2);

            if (menuElementCollection == null || menuElementCollection.Count == 0)
            {
                logAppendTextLine("未找到“期货运行模组”界面中的菜单栏中的菜单");
                return false;
            }
            bool found = false;
            foreach (AutomationElement menuItemAE in menuElementCollection)
            {
                if ("模组".Equals(menuItemAE.Current.Name))
                {
                    found = true;
                    WindowsApiUtils.clearOtherWindows(mainHandle, null);
                    SimulateOperating.leftClickAutomationElement(menuItemAE);
                    Thread.Sleep(1000);
                    List<IntPtr> menuHandles = WindowsApiUtils.findContextMenuHandles();
                    if (menuHandles == null || menuHandles.Count == 0)
                    {
                        logAppendTextLine("未找到“期货运行模组”界面中的菜单栏中的模组弹出菜单");
                        return false;
                    }

                    IntPtr menuHandle = menuHandles[0];
                    AutomationElement menuAE = AutomationElement.FromHandle(menuHandle);
                    if (menuAE == null)
                    {
                        logAppendTextLine("未找到“期货运行模组”界面中的弹出菜单");
                        return false;
                    }
                    Condition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem);
                    AutomationElementCollection menuItemElementCollection = menuAE.FindAll(TreeScope.Children, condition3);
                    if (menuItemElementCollection == null || menuItemElementCollection.Count == 0)
                    {
                        logAppendTextLine("未找到“期货运行模组”界面中的弹出菜单子项");
                        return false;
                    }
                    bool foundSaveMenuItem = false;
                    foreach (AutomationElement ae in menuItemElementCollection)
                    {
                        if ("保存全部模组的运行记录".Equals(ae.Current.Name))
                        {
                            foundSaveMenuItem = true;
                            if (!SimulateOperating.clickButton(ae))
                            {
                                logAppendTextLine("点击“保存全部模组的运行记录”子项失败");
                                return false;
                            }
                            Thread.Sleep(500);
                            hasHintPageAndClickOk();
                            return true;
                        }
                    }
                    if (!foundSaveMenuItem)
                    {
                        logAppendTextLine("未找到菜单中的“保存全部模组的运行记录”子项");
                        return false;
                    }
                }
            }
            if (!found)
            {
                logAppendTextLine("未找到菜单中的“模组”菜单");
                return false;
            }
            return true;
        }

        public void hasHintPageAndClickOk()
        {
            Console.WriteLine("hasHintPageAndClickOk");
            List<IntPtr> handles = WindowsApiUtils.findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, HintPage.HINT_PAGE_TITLE);
            if (handles != null && handles.Count >= 0)
            {
                Console.WriteLine("handles");
                //bool has = false;
                foreach (IntPtr handle in handles)
                {
                    AutomationElement hintPageWindow = AutomationElement.FromHandle(handle);
                    if (hintPageWindow == null)
                    {
                        continue;
                    }
                    PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, HintPage.AUTOMATION_ID_LABEL_HINT);
                    PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
                    AutomationElement labelMsg = hintPageWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                    if (labelMsg != null && !"".Equals(labelMsg.Current.Name) && labelMsg.Current.Name.Contains(HintPage.MSG))
                    {
                        //has = true;
                        PropertyCondition condition2 = new PropertyCondition(AutomationElement.AutomationIdProperty, HintPage.AUTOMATION_ID_BUTTON_OK);
                        PropertyCondition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                        AutomationElement buttonOK = hintPageWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition2, condition3));
                        if (buttonOK != null)
                        {
                            if (SimulateOperating.clickButton(buttonOK))
                            {
                                logAppendTextLine("保存全部模组的运行记录，成功");
                                Console.WriteLine("保存全部模组的运行记录，成功");
                            }
                            return;
                        }
                    }
                }
                //return has;
            }
            //return false;
        }
    }
}
