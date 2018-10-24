using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static FuturesModuleExportTool.WindowsApi;

namespace FuturesModuleExportTool
{
    public class SysTreeview32Utils
    {
        public const int TV_FIRST = 0x1100;
        public const int TVM_GETCOUNT = TV_FIRST + 5;
        public const int TVM_GETNEXTITEM = TV_FIRST + 10;
        public const int TVM_GETITEMA = TV_FIRST + 12;
        public const int TVM_GETITEMW = TV_FIRST + 62;

        public const int TVGN_ROOT = 0x0000;
        public const int TVGN_NEXT = 0x0001;
        public const int TVGN_PREVIOUS = 0x0002;
        public const int TVGN_PARENT = 0x0003;
        public const int TVGN_CHILD = 0x0004;
        public const int TVGN_FIRSTVISIBLE = 0x0005;
        public const int TVGN_NEXTVISIBLE = 0x0006;
        public const int TVGN_PREVIOUSVISIBLE = 0x0007;
        public const int TVGN_DROPHILITE = 0x0008;
        public const int TVGN_CARET = 0x0009;
        public const int TVGN_LASTVISIBLE = 0x000A;

        public const int TVIF_TEXT = 0x0001;
        public const int TVIF_IMAGE = 0x0002;
        public const int TVIF_PARAM = 0x0004;
        public const int TVIF_STATE = 0x0008;
        public const int TVIF_HANDLE = 0x0010;
        public const int TVIF_SELECTEDIMAGE = 0x0020;
        public const int TVIF_CHILDREN = 0x0040;
        public const int TVIF_INTEGRAL = 0x0080;

        private const int TVM_SELECTITEM = 0x0000110b;

        [StructLayout(LayoutKind.Sequential)]
        public struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
            public IntPtr HTreeItem;
        }

        public static uint TreeView_GetCount(IntPtr hwnd)
        {
            return (uint)SendMessage(hwnd, TVM_GETCOUNT, 0, 0);
        }

        public static IntPtr TreeView_GetNextItem(IntPtr hwnd, IntPtr hitem, int code)
        {
            return (IntPtr)SendMessage(hwnd, TVM_GETNEXTITEM, code, (int)hitem);
        }

        public static IntPtr TreeView_GetRoot(IntPtr hwnd)
        {
            return TreeView_GetNextItem(hwnd, IntPtr.Zero, TVGN_ROOT);
        }

        public static IntPtr TreeView_GetChild(IntPtr hwnd, IntPtr hitem)
        {
            return TreeView_GetNextItem(hwnd, hitem, TVGN_CHILD);
        }

        public static IntPtr TreeView_GetNextSibling(IntPtr hwnd, IntPtr hitem)
        {
            return TreeView_GetNextItem(hwnd, hitem, TVGN_NEXT);
        }

        public static IntPtr TreeView_GetParent(IntPtr hwnd, IntPtr hitem)
        {
            return TreeView_GetNextItem(hwnd, hitem, TVGN_PARENT);
        }

        //获取TreeView一级节点的handle，并点击
        public static void ClickTreeViewBaseNodes(IntPtr TreeViewHwnd)
        {
            List<IntPtr> result = new List<IntPtr>();
            IntPtr treeItemHandle = TreeView_GetRoot(TreeViewHwnd);
            while (treeItemHandle != IntPtr.Zero)
            {
                result.Add(treeItemHandle);
                treeItemHandle = TreeView_GetNextSibling(TreeViewHwnd, treeItemHandle);
            }
            if(result == null || result.Count == 0)
            {
                return;
            }
            foreach (IntPtr handle in result)
            {
                SelectNode(TreeViewHwnd, handle);
                Thread.Sleep(500);
                Application.DoEvents();
            }
        }

        //模拟点击TreeView的节点
        public static bool SelectNode(IntPtr TreeViewHwnd, IntPtr ItemHwnd)
        {
            int result = SendMessage(TreeViewHwnd, TVM_SELECTITEM, TVGN_CARET, (int)ItemHwnd);
            return result != 0;
        }
    }
}
