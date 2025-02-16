using ClassPanel.Controls;
using ClassPanel.Windows;
using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using MessageBox = HandyControl.Controls.MessageBox;

namespace ClassPanel
{
    /// <summary>
    /// comBoard.xaml 的交互逻辑
    /// </summary>
    ///

    public partial class comBoard : UserControl
    {
        public MainMethod Main;

        public comBoard(MainMethod main)
        {
            InitializeComponent();

            Main = main;
        }

        public dynamic datas;

        private void ListBoxItem_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        public int ItemNum = 0;
        public object[] conObjs = new object[10000];

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ItemNum <= 10000)
            {
                ItemNum += 1;
                conObjs[ItemNum] = new ConText(this, ItemNum, false, "");
                Card.Items.Add(conObjs[ItemNum]);
            }
            else
            {
                HandyControl.Controls.MessageBox.Show("最多只支持10000条哦");
            }
        }

        public void ExitObj(int Num)
        {
            Card.Items.Remove(conObjs[Num]);
            conObjs[Num] = null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ItemNum <= 10000)
            {
                ItemNum += 1;
                conObjs[ItemNum] = new ConPaint(this, ItemNum);
                Card.Items.Add(conObjs[ItemNum]);
            }
            else
            {
                MessageBox.Show("最多只支持10000条哦");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Card.Items.Clear();

            conObjs = new object[10000];
            ItemNum = 0;
            //  DirectoryInfo di = new DirectoryInfo("./Data");
            //  await Task.Run(() => { di.Delete(true); });
            //  Directory.CreateDirectory("./Data");
        }

        public winBoard winBoard1;
        public bool IsOpen = false;

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!IsOpen)
            {
                winBoard1 = new winBoard(Main, this);
                F.Children.Remove(Items);
                winBoard1.grid2.Children.Add(Items);
                ConBar Bar = new ConBar();
                winBoard1.grid.Children.Add(Bar);
                Bar.Fathers = winBoard1;
                winBoard1.Show();
                IsOpen = true;
            }
        }
    }
}