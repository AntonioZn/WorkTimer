﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace YourNote
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        
        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(txtPassword.Password);

            if (txtPassword.Password.Length > 6 && txtPassword.Password.Length < 13 && txtUserName.Text != null && txtUserName.Text.Length > 6)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Invalid email, username or password");
            }
        }
    }
}