using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace YourNote
{
    public class SessionReportManager
    {
        private DateTime startTime;
        private StringBuilder textBoxNotes = new StringBuilder();
        private int counter = 0;
        private readonly Random random = new Random();
        private Timer timer;
        private readonly ScreenshotTaker screenshotTaker = new ScreenshotTaker();

        public void SaveSession()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text file(*.txt)|*.txt";

            if (totalTime == TimeSpan.Zero)
            {
                if (textBoxNotes.Length != 0 && dlg.ShowDialog() == true && counter == 1)
                {
                    textBoxNotes.Append($"STOP  {DateTime.Now.ToString()} {Environment.NewLine}");
                    fromStartToEnd = DateTime.Now - startTime;
                    textBoxNotes.Append($"TOTAL: {fromStartToEnd}");
                    File.WriteAllText(dlg.FileName, textBoxNotes.ToString());
                    textBoxNotes.Clear();
                }

                else
                {
                    MessageBox.Show($"{totalTime} {Environment.NewLine} {textBoxNotes.ToString()}");
                    textBoxNotes.Clear();
                }
            }

            else
            {
                if (dlg.ShowDialog() == true)
                {
                    if (counter == 1)
                    {
                        textBoxNotes.Append($"STOP  {DateTime.Now.ToString()} {Environment.NewLine}");
                    }

                    textBoxNotes.Append(this.TextBox.Text);
                    textBoxNotes.Append($"TOTAL WORKED TIME: {totalTime}");
                    File.WriteAllText(dlg.FileName, textBoxNotes.ToString());
                    TextBox.Text = String.Empty;
                }

                textBoxNotes.Clear();
                totalTime = TimeSpan.Zero;
                counter = 0;
                screenshotTaker.ResetScreenshotNumber();
            }
        }

        public void StartSession()
        {
            
                timer = new Timer(TimerCallback, null, random.Next(3000, 7000), Timeout.Infinite);

                startTime = DateTime.Now;

                if (this.TextBox.Text != "")
                {
                    textBoxNotes.Append($"{startTime.ToString()} - {TextBox.Text} {Environment.NewLine}");
                    TextBox.Clear();
                }

                StartButton.Content = "Stop";
                StartButton.Background = Brushes.ForestGreen;
                counter++;
                textBoxNotes.Append($"START {startTime} {Environment.NewLine}");
            
        }

        public TimeSpan EndSession(string note)
        {
            timer.Dispose();

            if (!string.IsNullOrWhiteSpace(note))
            {
                textBoxNotes.AppendLine($"      {startTime} - {note}");
            }

            TimeSpan sessionTime = DateTime.Now - startTime;
            textBoxNotes.Append($"STOP  {DateTime.Now.ToString()} {Environment.NewLine}");

            StartButton.Content = $"          Start {Environment.NewLine} {totalTime} ";
            StartButton.Background = Brushes.Red;

            return sessionTime;
        }

        private void TimerCallback(object state)
        {
            try
            {
                screenshotTaker.SaveScreenshot();
            }
            finally
            {
                timer.Change(random.Next(3000, 7000), Timeout.Infinite);
            }
        }
    }
}
