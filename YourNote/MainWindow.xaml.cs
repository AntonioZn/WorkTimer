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
        }
        private DateTime startTime;
        private TimeSpan currentTime;
        private TimeSpan totalTime;
        private TimeSpan fromStartToEnd;
        // in case the user just click Start Button and after that EndSessionButton
        private StringBuilder textBoxNotes = new StringBuilder();
        private int counter = 0;


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (counter == 0)
            {
                StartButton.Content = "Stop";
                StartButton.Background = Brushes.ForestGreen;
                startTime = DateTime.Now;
                textBoxNotes.Append($"START {startTime} {Environment.NewLine}");

                if (this.TextBox.Text != "")
                {
                    textBoxNotes.Append($"{startTime.ToString()} - {TextBox.Text} {Environment.NewLine}");
                    TextBox.Clear();
                }

                counter++;
            }

            else 
            {               
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
                    textBoxNotes.Append($"Total: {fromStartToEnd}");
                    File.WriteAllText(dlg.FileName, textBoxNotes.ToString());
                    textBoxNotes.Clear();
                }

                else
                {
                    MessageBox.Show($"{totalTime} Good job :) {Environment.NewLine} {textBoxNotes.ToString()}");
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
                    textBoxNotes.Append($"Total worked time: {totalTime}");
                    File.WriteAllText(dlg.FileName, textBoxNotes.ToString());
                    TextBox.Text = String.Empty;
                }

                textBoxNotes.Clear();
                totalTime = TimeSpan.Zero;
                counter = 0;
            }
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
    }
}
