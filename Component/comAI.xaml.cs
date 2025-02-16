using ClassPanel.Controls;
using ClassPanel.Windows;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClassPanel
{
    /// <summary>
    ///
    /// </summary>
    ///
    /// <summary>
    /// 向桌面发送消息
    /// </summary>

    public partial class comAI : UserControl
    {
        public MainMethod Main;
        public comAI(MainMethod main)
        {
            InitializeComponent();
            Main = main;
        }
        public winAI winAI1;
        public bool IswinAILoad=false;
        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!IswinAILoad)
            {
                winAI1 = new winAI(Main,this);
                winAI1.Show();
                IswinAILoad = true;
            }
        }
    }
}