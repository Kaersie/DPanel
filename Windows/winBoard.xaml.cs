using ClassPanel.Controls;
using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;
using Window = System.Windows.Window;

namespace ClassPanel.Windows
{
    /// <summary>
    /// winBoard.xaml 的交互逻辑
    /// </summary>
    ///

    public partial class winBoard : Window
    {
        public MainMethod Main;
        public comBoard ComBoard1;

        public winBoard(MainMethod main, comBoard c1)
        {
            InitializeComponent();
            ComBoard1 = c1;

            Main = main;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            grid2.Children.Remove(ComBoard1.Items);
            ComBoard1.F.Children.Add(ComBoard1.Items);
        }
    }
}