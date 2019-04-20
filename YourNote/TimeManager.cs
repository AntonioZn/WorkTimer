using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YourNote
{
    public class TimeManager
    {
        private DateTime startTime;
        private TimeSpan stopTime;
        private TimeSpan sessionDuration;
        private StringBuilder logfile = new StringBuilder();
        private SaveFileDialog dlg = new SaveFileDialog();       

        public DateTime StartTime()
        {
            startTime = DateTime.Now;
            logfile.AppendLine($"START {startTime}");           

            return startTime;
        }

        public TimeSpan StopTime()
        {
            stopTime = DateTime.Now - startTime;
            sessionDuration += stopTime; 
            
            logfile.AppendLine($"STOP  {DateTime.Now}");

            return sessionDuration;
        } 

        public TimeSpan GetDuration()
        {
            return sessionDuration;
        }

        
        public void EndSession()
        {
            sessionDuration = TimeSpan.Zero;
            logfile.Clear();
        }

        public string GetLogfileInfo()
        {
            string info = logfile.ToString() + Environment.NewLine + sessionDuration.ToString();
            return info;
        }

        public void GetDurationFromStartTimeToNow()
        {
            logfile.AppendLine($"STOP  {DateTime.Now}");
            TimeSpan fromStartToEnd = new TimeSpan();
            fromStartToEnd = DateTime.Now - startTime;
            logfile.AppendLine($"TOTAL: {fromStartToEnd}");
        }

        public void AppendTextToLogFile(string text)
        {
            logfile.AppendLine(text);
        }

        public TimeSpan ShowSessionTime()
        {
            return sessionDuration;
        }

        public bool? ShowDialog()
        {
            dlg.Filter = "Text file(*.txt)|*.txt";
            return dlg.ShowDialog();
        }
                  
        public void SaveFile()
        {          
            File.WriteAllText(dlg.FileName, GetLogfileInfo());
        }
    }
}
