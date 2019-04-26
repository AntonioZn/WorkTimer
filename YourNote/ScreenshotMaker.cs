using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace YourNote
{
    public class ScreenshotMaker
    {
        private int screenshotNumber = 1;
        private Timer timer;
        private Random random = new Random();
        private readonly InformationSaver informationSaver = new InformationSaver();

        public void SaveScreenshot()
        {
            string filename = $"Screenshot-{screenshotNumber.ToString()}-" + DateTime.Now.ToString("ddMMyyyy") + ".png";

            int screenLeft = (int)SystemParameters.VirtualScreenLeft;

            int screenTop = (int)SystemParameters.VirtualScreenTop;

            int screenWidth = (int)SystemParameters.VirtualScreenWidth;

            int screenHeight = (int)SystemParameters.VirtualScreenHeight;

            Bitmap bitmap_Screen = new Bitmap(screenWidth, screenHeight);

            Graphics g = Graphics.FromImage(bitmap_Screen);

            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap_Screen.Size);

            bitmap_Screen.Save(System.IO.Path.Combine(informationSaver.Path, filename));

            screenshotNumber++;
        }   

        public void ResetScreenshotNumberToOne()
        {
            screenshotNumber = 1;
        }

        public void Start()
        {
            timer = new Timer(TimerCallback, null, random.Next(3000, 7000), Timeout.Infinite);
        }

        public void Stop()
        {
            timer.Dispose();
        }

        public void TimerCallback(object state)
        {
            try
            {
                SaveScreenshot();
            }

            finally
            {
                timer.Change(random.Next(3000, 7000), Timeout.Infinite);
            }
        }
    }

}
