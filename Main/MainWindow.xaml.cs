using System;
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

namespace ClassPanel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            double h = SystemParameters.WorkArea.Height;
            double w = SystemParameters.WorkArea.Width;
            this.Top = 0;
            if (w * 0.6 >= 700)
            {
                this.Width = 0.6 * w;
            }
            this.Left = w - this.Width;
            this.Height = h;
            MainPanel.Width = this.Width - 40;
            MainPanel.Height = this.Height - 40;
        }
    }
}