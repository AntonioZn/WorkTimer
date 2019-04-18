using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using Brushes = System.Windows.Media.Brushes;
using Timer = System.Threading.Timer;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using MessageBox = System.Windows.MessageBox;
using Button = System.Windows.Controls.Button;
using System.ComponentModel;
using ContextMenu = System.Windows.Forms.ContextMenu;

namespace YourNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            NotifyIcon ni = new NotifyIcon();
            ni.Icon = new Icon("dollar.ico");
            ni.Visible = true;
            ni.Click +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;

                };

            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Exit", new EventHandler(Exit));
            ni.ContextMenu = contextMenu;
        }

        private DateTime startTime;
        private TimeSpan currentTime;
        private TimeSpan totalTime;
        private TimeSpan fromStartToEnd;
        // in case the user just click Start Button and after that EndSessionButton
        private StringBuilder textBoxNotes = new StringBuilder();
        //  save the notes and logs
        private int counter = 0;
        private int screenshotNumber = 1;
        // helps for easily naming the screenshots
        private Timer timer;
        // for making screenshot in random period
        private Random random;


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (counter == 0)
            {

                random = new Random(Environment.TickCount);
                timer = new Timer(TimerCallback, null, random.Next(3000,7000), Timeout.Infinite);

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

            else
            {
                timer.Dispose();

                if (this.TextBox.Text != "")
                {
                    textBoxNotes.Append($"      {startTime.ToString()} - {TextBox.Text} {Environment.NewLine}");
                    TextBox.Clear();
                }

                currentTime = DateTime.Now - startTime;
                textBoxNotes.Append($"STOP  {DateTime.Now.ToString()} {Environment.NewLine}");
                totalTime += currentTime;
                counter--;
                StartButton.Content = $"          Start {Environment.NewLine} {totalTime} ";
                StartButton.Background = Brushes.Red;
            }
        }



        private void TimerCallback(object state)
        {
            try
            {
                SaveScreenshot();
                screenshotNumber++;
            }
            finally
            {
                timer.Change(random.Next(3000,7000), Timeout.Infinite);
            }
        }

        private void EndSessionButton_Click(object sender, RoutedEventArgs e)
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

            }

            screenshotNumber = 1;
            StartButton.Content = "Start";
            StartButton.ClearValue(Button.BackgroundProperty);
        }

        private void ClearContentButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.TextBox.Text != "")
            {
                textBoxNotes.Append($"      {startTime.ToString()} - {TextBox.Text} {Environment.NewLine}");
                TextBox.Clear();
            }
        }

        private void ShowPreviousContentsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{textBoxNotes.ToString()}");
        }

        private void SaveScreenshot()
        {
            string filename = $"Screenshot{screenshotNumber.ToString()}-" + DateTime.Now.ToString("ddMMyyyy") + ".png";

            int screenLeft = (int)SystemParameters.VirtualScreenLeft;

            int screenTop = (int)SystemParameters.VirtualScreenTop;

            int screenWidth = (int)SystemParameters.VirtualScreenWidth;

            int screenHeight = (int)SystemParameters.VirtualScreenHeight;

            Bitmap bitmap_Screen = new Bitmap(screenWidth, screenHeight);

            Graphics g = Graphics.FromImage(bitmap_Screen);

            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap_Screen.Size);

            bitmap_Screen.Save(@"C:\Users\anton\OneDrive\Desktop\" + filename);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();
            
            base.OnStateChanged(e);

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            this.Hide();

            base.OnClosing(e);
            
        }

        private void Exit(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
