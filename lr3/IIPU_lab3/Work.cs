using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace IIPU_lab3
{
    class Work
    {
        public int PreviousTime { get; set; }
        public int CurrentTime { get; set; }
        public string PowerStatus { get; set; }
        public string Percent { get; set; }
        public string Remaining { get; set; }

        Regex VideoIdleExpression = new Regex("VIDEOIDLE.*\\n.*\\n.*\\n.*\\n.*\\n.*\\n.*");

        public Work()
        {
            PreviousTime = GetTime();
            CurrentTime = PreviousTime;
            UpdateInfo();
        }

        public ProcessStartInfo PrepareStartInfo(string fileName, string arguments, string value)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName;
            startInfo.Arguments = arguments + value;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            return startInfo;

        }

        public int GetTime()
        {
            Process timeProcess = Process.Start(PrepareStartInfo("cmd.exe", "/c powercfg /q", String.Empty));
            string videoIdle = VideoIdleExpression.Match(timeProcess.StandardOutput.ReadToEnd()).Value;
            string currentTime = videoIdle.Substring(videoIdle.Length - 11).TrimEnd();
            timeProcess.WaitForExit();
            timeProcess.Close();
            return Convert.ToInt32(currentTime, 16) / 60;
        }

        public void UpdateInfo(bool timeChanged = false)
        {
            if (PowerStatus != SystemInformation.PowerStatus.PowerLineStatus.ToString())
            {
                SetNewTime(PreviousTime);
                timeChanged = true;
            }
            if (timeChanged) CurrentTime = GetTime();
            PowerStatus = SystemInformation.PowerStatus.PowerLineStatus.ToString();
            Percent = SystemInformation.PowerStatus.BatteryLifePercent * 100 + "%";
            SetPowerLifeRemaining();
        }

        public void SetPowerLifeRemaining()
        {
            if (PowerStatus == "Offline")
            {
                int batteryLife = SystemInformation.PowerStatus.BatteryLifeRemaining;
                if (batteryLife != -1)
                {
                    Remaining = new TimeSpan(0, 0, batteryLife).ToString("c");
                }
                else Remaining = "Is processing";
            }
            else
            {
                Remaining = "Is charging";
            }
        }

        public void SetNewTime(int time)
        {
            Process setTimeProcess = Process.Start(PrepareStartInfo("cmd.exe", "/c powercfg /x -monitor-timeout-dc ", time.ToString()));
            setTimeProcess.WaitForExit();
            setTimeProcess.Close();
        }
    }
}
