using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuturesModuleExportTool
{
    class Utils
    {
        public static String getDate()
        {
            return DateTime.Now.ToShortDateString();
        }

        public static String getTimeMillisecond()
        {
            return DateTime.Now.Ticks + "";
        }

        public static void mkdir(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        public static string getExportDir()
        {
            String dir = System.Windows.Forms.Application.StartupPath + "\\export\\";
            Utils.mkdir(dir);
            return dir;
        }

        public static int convertToInt(string s)
        {
            return Convert.ToInt32(Convert.ToDecimal(s));
        }

        public static bool convertToInt(String s, out int result)
        {
            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Utils.convertToInt(s);
                    return true;
                }
                catch (Exception e)
                {
                    result = 0;
                    return false;
                }
            }
            result = 0;
            return false;
        }

        //截取文件夹名称
        public static string cutDirName(string fileName)
        {
            if (fileName.Contains("\\"))
            {
                string temp = fileName.Substring(0, fileName.LastIndexOf("\\"));
                if (temp.Contains("\\"))
                {
                    fileName = temp.Substring(temp.LastIndexOf("\\") + 1);
                }
            }
            return fileName;
        }
    }
}
