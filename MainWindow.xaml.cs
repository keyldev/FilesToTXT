﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace FilesToTXT
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SetDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowDialog();
            PathTextBox.Text = folder.SelectedPath;

            List<string> ls = await GetRecursFiles(folder.SelectedPath);
            foreach (string fname in ls)
            {
                ResultTextBox.Text += fname + "\n";
            }
        }
        private async Task<List<string>> GetRecursFiles(string start_path)
        {
            List<string> ls = new List<string>();
            try
            {
                string[] folders = Directory.GetDirectories(start_path);
                foreach (string folder in folders)
                {
                    ls.Add(folder);
                    ls.AddRange(GetRecursFiles(folder).Result);
                }
                string[] files = Directory.GetFiles(start_path);
                foreach (string filename in files)
                {
                    ls.Add(filename + $"[MD5: {CalculateMD5(filename)}]");
                }
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            return ls;
        }
        string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}