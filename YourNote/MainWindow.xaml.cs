﻿using Microsoft.Win32;
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
using MessageBox = System.Windows.MessageBox;
using Button = System.Windows.Controls.Button;
using System.ComponentModel;
using ContextMenu = System.Windows.Forms.ContextMenu;
using System.Runtime.InteropServices;

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
            informationSaver.DesktopFolder();
            DarkModeOnOff.IsChecked = Properties.Settings.Default.Antonio;
            darkModeOn();
                    
        }

        private bool isStarted;
        private readonly ScreenshotMaker ScreenshotMaker = new ScreenshotMaker();
        private readonly TimeManager TimeManager = new TimeManager();
        private readonly ForegroundWindowSaver foregroundWindowSaver = new ForegroundWindowSaver();
        private readonly InformationSaver informationSaver = new InformationSaver();

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {          
            if (!isStarted)
            {
                foregroundWindowSaver.GetActiveWindowTitle();
                ScreenshotMaker.Start();
                TimeManager.StartTime();
                ClearInfo();

                StartButton.Content = "Stop";
                StartButton.Background = Brushes.ForestGreen;
                isStarted = true;
            }

            else
            {
                ScreenshotMaker.Stop();
                TimeManager.StopTime();
                ClearInfo();

                StartButton.Content = $"          Start {Environment.NewLine} {TimeManager.ShowSessionTime()} ";
                StartButton.Background = Brushes.Red;
                isStarted = false;
            }
        }
       
        private void EndSessionButton_Click(object sender, RoutedEventArgs e)
        {          
            if (TimeManager.GetDuration() == TimeSpan.Zero && isStarted)
            {                                    
                 TimeManager.GetDurationFromStartTimeToNow();                  
            }

            else
            {                
                    if (isStarted == true)
                    {
                        TimeManager.AppendTextToLogFile($"STOP  {DateTime.Now}");
                    }

                    TimeManager.AppendTextToLogFile(this.TextBox.Text);
                    TimeManager.AppendTextToLogFile($"TOTAL WORKED TIME: {TimeManager.GetDuration()}");
                    TimeManager.SaveFile();
                    TextBox.Clear();
                

                isStarted = false;
            }

            MessageBox.Show("SESSION SAVED SUCCESSFULLY");
            TimeManager.EndSession();
            ScreenshotMaker.ResetScreenshotNumberToOne();
            StartButton.Content = "Start";
            StartButton.ClearValue(Button.BackgroundProperty);
        }

        private void ClearContentButton_Click(object sender, RoutedEventArgs e)
        {
            ClearInfo();
        }

        private void ShowPreviousContentsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{TimeManager.GetLogfileInfo()} {Environment.NewLine}");
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

        private void ClearInfo()
        {
            if (this.TextBox.Text != "")
            {
                TimeManager.AppendTextToLogFile($"{DateTime.Now} - {TextBox.Text}");
                TextBox.Clear();
            }
        }

        private void DarkModeOnOff_Click(object sender, RoutedEventArgs e)
        {
            if (DarkModeOnOff.IsChecked == true)
            {
                YourWorkHelper.Background = Brushes.Black;
                Additionalinfo.Foreground = Brushes.White;
                TextBox.Foreground = Brushes.White;
                TextBox.BorderBrush = Brushes.White;
                Properties.Settings.Default.Antonio = this.DarkModeOnOff.IsChecked.GetValueOrDefault();
                Properties.Settings.Default.Save();
            }

            else
            {
                YourWorkHelper.ClearValue(BackgroundProperty);
                Additionalinfo.ClearValue(TextBlock.ForegroundProperty);
                TextBox.ClearValue(ForegroundProperty);
                TextBox.ClearValue(BorderBrushProperty);
                Properties.Settings.Default.Antonio = this.DarkModeOnOff.IsChecked.GetValueOrDefault();
                Properties.Settings.Default.Save();
            }
        } 
        
         private void darkModeOn()
         {
             if (Properties.Settings.Default.Antonio)
             {
                 YourWorkHelper.Background = Brushes.Black;
                 Additionalinfo.Foreground = Brushes.White;
                 TextBox.Foreground = Brushes.White;
                 TextBox.BorderBrush = Brushes.White;
             }
        
             else
             {
                 YourWorkHelper.ClearValue(BackgroundProperty);
                 Additionalinfo.ClearValue(TextBlock.ForegroundProperty);
                 TextBox.ClearValue(ForegroundProperty);
                 TextBox.ClearValue(BorderBrushProperty);
             }
         }       
    }
}
