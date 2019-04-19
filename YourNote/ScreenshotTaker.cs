using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YourNote
{
    public class ScreenshotTaker
    {
        private int screenshotNumber = 1;

        public void SaveScreenshot()
        {
            string filename = $"Screenshot-{screenshotNumber}-{DateTime.Now.ToString("ddMMyyyy")}.png";

            int screenLeft = (int)SystemParameters.VirtualScreenLeft;

            int screenTop = (int)SystemParameters.VirtualScreenTop;

            int screenWidth = (int)SystemParameters.VirtualScreenWidth;

            int screenHeight = (int)SystemParameters.VirtualScreenHeight;

            Bitmap bitmap_Screen = new Bitmap(screenWidth, screenHeight);

            Graphics g = Graphics.FromImage(bitmap_Screen);

            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap_Screen.Size);

            bitmap_Screen.Save(@"C:\Users\anton\OneDrive\Desktop\" + filename);

            screenshotNumber++;
        }

        public void ResetScreenshotNumber()
        {
            screenshotNumber = 1;
        }
    }
}
