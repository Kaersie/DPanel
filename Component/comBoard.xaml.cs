using DPanel.Controls;
using System.Windows;

namespace DPanel
{
    /// <summary>
    /// comBoard.xaml 的交互逻辑
    /// </summary>
    ///

    public partial class comBoard : Window
    {
        public MainMethod Main;

        public comBoard(MainMethod main)
        {
            InitializeComponent();

            ConBar Bar = new ConBar();
            grid.Children.Add(Bar);
            Bar.Fathers = this;
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
                MessageBox.Show("最多只支持10000条哦");
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
    }
}