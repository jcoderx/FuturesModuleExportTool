using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace FuturesModuleExportTool
{
    public class TreeViewUtils
    {
        public const string AUTOMATION_ID_TREE_VIEW = "4651";

        public static bool clickTreeViewRootNode(IntPtr mainHandle,out string msg)
        {
            bool result = true;
            msg = "";
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow != null)
            {
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, AUTOMATION_ID_TREE_VIEW);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
                AutomationElement treeView = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                if (treeView != null)
                {
                    Condition condition2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                    AutomationElementCollection treeViewItems = treeView.FindAll(TreeScope.Children, condition2);
                    if (treeViewItems != null && treeViewItems.Count > 0)
                    {
                        object temp;
                        foreach (AutomationElement ae in treeViewItems)
                        {
                            Console.WriteLine(ae.Current.Name);
                            if (ae != null)
                            {
                                if (ae.TryGetCurrentPattern(SelectionItemPattern.Pattern, out temp))
                                {
                                    if (temp != null)
                                    {
                                        SelectionItemPattern pattern = temp as SelectionItemPattern;
                                        pattern.Select();
                                        Thread.Sleep(500);
                                    }
                                    else
                                    {
                                        result = false;
                                        msg = "temp is null";
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                                msg = "ae is null";
                            }
                        }
                    }
                }
                else
                {
                    result = false;
                    msg = "treeView is null";
                }
            }
            else
            {
                result = false;
                msg = "targetWindow is null";
            }
            return result;
        }
    }
}
