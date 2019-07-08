using FuturesModuleExportTool.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using static FuturesModuleExportTool.WindowsApi;

namespace FuturesModuleExportTool
{
    public class WindowsApiUtils
    {
        //关闭窗口
        public static void closeWindow(IntPtr handle)
        {
            SendMessage(handle, WM_CLOSE, 0, 0);
        }

        public static List<string> EXCLUDE_PAGE_TITLES = new List<string>() { "", "自定义分析周期", "已触发预警列表", "文华布告栏", "模型下单", "条件单选择加载对话框", "设置线型、颜色、粗细", "持仓匹配校验信息", "期货运行模组", "运行日志" };
        public static void clearOtherWindows(IntPtr mainHandle, List<IntPtr> excludeHandles)
        {
            int mainProcessId;
            GetWindowThreadProcessId(mainHandle, out mainProcessId);

            List<IntPtr> wndHandles = new List<IntPtr>();
            EnumWindows((h, l) =>
            {
                int processId;
                GetWindowThreadProcessId(h, out processId);

                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);
                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (mainProcessId == processId && className.ToString().Equals(CLASS_DIALOG) && !EXCLUDE_PAGE_TITLES.Contains(title.ToString()))
                {
                    //特殊界面特殊处理
                    if (isSupplyDataHintPage(title.ToString(), h))
                    {
                        closeSupplyDataHintPage(h);
                    }
                    else
                    {
                        wndHandles.Add(h);
                    }
                }

                return true;
            }, 0);
            bool hasOtherWindows = false;
            foreach (IntPtr handle in wndHandles)
            {
                if (excludeHandles != null && excludeHandles.Contains(handle))
                {
                    continue;
                }
                else
                {
                    hasOtherWindows = true;
                    closeWindow(handle);
                }
            }
            if (hasOtherWindows)
            {
                Thread.Sleep(500);
            }
        }

        private static bool isSupplyDataHintPage(string title, IntPtr handle)
        {
            if (!SupplyDataHintPage.SUPPLY_DATA_HINT_PAGE_TITLE.Equals(title))
            {
                return false;
            }
            AutomationElement supplyDataHintPageWindow = AutomationElement.FromHandle(handle);
            if (supplyDataHintPageWindow == null)
            {
                return false;
            }
            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataHintPage.AUTOMATION_ID_LABEL_HINT);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
            AutomationElement labelHint = supplyDataHintPageWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (labelHint != null && labelHint.Current.Name.Contains(SupplyDataHintPage.HINT_MESSAGE))
            {
                return true;
            }
            return false;
        }

        //清理“提醒补充数据界面”，这个界面的关闭按钮无效，需要特殊处理
        private static void closeSupplyDataHintPage(IntPtr handle)
        {
            AutomationElement supplyDataHintPageWindow = AutomationElement.FromHandle(handle);
            if (supplyDataHintPageWindow == null)
            {
                return;
            }
            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataHintPage.AUTOMATION_ID_BUTTON_NO);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
            AutomationElement buttonNo = supplyDataHintPageWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (buttonNo != null)
            {
                SimulateOperating.clickButton(buttonNo);
            }
        }


        //查找上下文菜单
        public static List<IntPtr> findContextMenuHandles()
        {
            List<IntPtr> wndHandles = new List<IntPtr>();
            EnumWindows((h, l) =>
            {
                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);

                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (className.ToString().Equals(CLASS_MENU))
                {
                    wndHandles.Add(h);
                }
                return true;
            }, 0);
            return wndHandles;
        }

        //根据窗口类型和窗口标题，查找对应窗口，模糊搜索
        public static List<IntPtr> findWindowHandlesByClassTitleFuzzy(string expectedClassName, string expectedTitle)
        {
            List<IntPtr> wndHandles = new List<IntPtr>();
            EnumWindows((h, l) =>
            {
                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);

                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (className.ToString().Contains(expectedClassName) && title.ToString().Contains(expectedTitle))
                {
                    wndHandles.Add(h);
                }
                return true;
            }, 0);
            return wndHandles;
        }
    }
}
