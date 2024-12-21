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

namespace DPanel
{
    /// <summary>
    /// comBoard.xaml 的交互逻辑
    /// </summary>
    public partial class comBoard : Window
    {
        public comBoard()
        {
            InitializeComponent();
     
        }
        //拖动：https://blog.csdn.net/u013113678/article/details/121071628
        Point StartPosition;
        bool IsMoved = false;
        void Window_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartPosition = e.GetPosition(this);
        }
        void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && StartPosition != e.GetPosition(this))
            {
                IsMoved = true;
                DragMove();
            }
        }
        void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMoved)
            {
                IsMoved = false;
                e.Handled = true;
            }
        }


    }
}
