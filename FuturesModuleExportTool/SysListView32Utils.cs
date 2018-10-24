using System;
using System.Runtime.InteropServices;
using static FuturesModuleExportTool.WindowsApi;

namespace FuturesModuleExportTool
{
    public class SysListView32Utils
    {
        public const uint LVM_FIRST = 0x1000;
        public const uint LVM_GETHEADER = LVM_FIRST + 31;
        public const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
        public const uint LVM_GETITEMW = LVM_FIRST + 75;

        public const uint HDM_GETITEMCOUNT = 0x1200;//获取列表列数

        public struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            public IntPtr pszText; // string 
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;
        }
        public static int LVIF_TEXT = 0x0001;

        //列表总行数
        public static int ListView_GetItemRows(IntPtr handle)
        {
            return SendMessage(handle, LVM_GETITEMCOUNT, 0, 0);
        }

        //列表总列数
        public static int ListView_GetItemCols(IntPtr handle)
        {
            return SendMessage(handle, HDM_GETITEMCOUNT, 0, 0);
        }

        public static string[,] ListView_GetItemValues(IntPtr hWnd, out int rows, out int cols)
        {
            int processId; //进程pid 
            IntPtr headerHandle = (IntPtr)SendMessage(hWnd, LVM_GETHEADER, 0, 0);//listview的列头句柄
            rows = ListView_GetItemRows(hWnd);//总行数，即进程的数量
            cols = ListView_GetItemCols(headerHandle);//列表列数
            GetWindowThreadProcessId(hWnd, out processId);
            //打开并插入进程
            IntPtr process = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, processId);
            //申请代码的内存区,返回申请到的虚拟内存首地址
            IntPtr pointer = VirtualAllocEx(process, IntPtr.Zero, 4096, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
            return ListView_GetItemValuesInner(rows, cols, process, pointer, hWnd);
        }

        private static string[,] ListView_GetItemValuesInner(int rows, int cols, IntPtr process, IntPtr pointer, IntPtr hWnd)
        {
            string[,] values = new string[rows, cols];//二维数组:保存列表控件的文本信息
            try
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        byte[] vBuffer = new byte[256];//定义一个临时缓冲区
                        LVITEM[] vItem = new LVITEM[1];
                        vItem[0].mask = LVIF_TEXT;//说明pszText是有效的
                        vItem[0].iItem = i;     //行号
                        vItem[0].iSubItem = j;  //列号
                        vItem[0].cchTextMax = vBuffer.Length;//所能存储的最大的文本为256字节
                        vItem[0].pszText = (IntPtr)((int)pointer + Marshal.SizeOf(typeof(LVITEM)));
                        uint vNumberOfBytesRead = 0;

                        //把数据写到vItem中
                        //pointer为申请到的内存的首地址
                        //UnsafeAddrOfPinnedArrayElement:获取指定数组中指定索引处的元素的地址
                        WriteProcessMemory(process, pointer, Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0), Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);

                        //发送LVM_GETITEMW消息给hwnd,将返回的结果写入pointer指向的内存空间
                        SendMessage(hWnd, LVM_GETITEMW, i, pointer.ToInt32());

                        //从pointer指向的内存地址开始读取数据,写入缓冲区vBuffer中
                        ReadProcessMemory(process, (IntPtr)((int)pointer + Marshal.SizeOf(typeof(LVITEM))), Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0), vBuffer.Length, ref vNumberOfBytesRead);

                        string vText = Marshal.PtrToStringUni(Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0));
                        values[i, j] = vText;
                    }
                }
            }
            catch (Exception e)
            {
                values = null;
            }
            finally
            {
                VirtualFreeEx(process, pointer, 0, MEM_RELEASE);//在其它进程中释放申请的虚拟内存空间,MEM_RELEASE方式很彻底,完全回收
                CloseHandle(process);//关闭打开的进程对象
            }
            return values;
        }
    }
}
