using FuturesModuleExportTool.Job;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FuturesModuleExportTool
{
    class Utils
    {
        public static String getDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
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
                catch 
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

        public static string zeroize(int digit, int length)
        {
            string digitStr = digit.ToString();
            while (digitStr.Length < length)
            {
                digitStr = "0" + digitStr;
            }
            return digitStr;
        }

        public static string formatJobTime(JobTime jobTime)
        {
            string hour = zeroize(jobTime.hour, 2);
            string minute = zeroize(jobTime.minute, 2);
            string second = zeroize(jobTime.second, 2);
            return hour + ":" + minute + ":" + second;
        }

        public static List<JobTime> readJobTimeFile(string filePath)
        {
            List<JobTime> result = new List<JobTime>();
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string jobTimeStr = line.Trim();
                    if (jobTimeStr != null && !"".Equals(jobTimeStr))
                    {
                        JobTime jobTime = JobTime.parseJobTime(jobTimeStr);
                        if (jobTime != null)
                        {
                            result.Add(jobTime);
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }catch
                {

                }
            }
            
            return result;
        }

        public static void writeJobTimeFile(string filePath,List<JobTime> jobTimes)
        {
            StreamWriter writer = new StreamWriter(filePath);
            foreach(JobTime jobTime in jobTimes)
            {
                if (jobTime != null)
                {
                    writer.WriteLine(jobTime.toString());
                }
            }
            writer.Close();
        }
    }
}
