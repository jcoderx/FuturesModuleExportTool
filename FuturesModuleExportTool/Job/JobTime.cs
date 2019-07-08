using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuturesModuleExportTool.Job
{
    public class JobTime
    {
        public int hour;
        public int minute;
        public int second = 0;

        public string toString()
        {
            return Utils.zeroize(hour, 2) + ":" + Utils.zeroize(minute, 2);
        }

        public static JobTime parseJobTime(string s)
        {
            if(s == null || s.Length != 5)
            {
                return null;
            }
            string[] arr = s.Split(':');
            try
            {
                string hourStr = arr[0];
                string minuteStr = arr[1];
                int hour = int.Parse(hourStr);
                int minute = int.Parse(minuteStr);
                JobTime jobTime = new JobTime();
                jobTime.hour = hour;
                jobTime.minute = minute;
                return jobTime;
            }
            catch
            {
                return null;
            }
        }
    }
}
