using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPanel.Controls
{
    /// <summary>
    /// ConBar.xaml 的交互逻辑
    /// </summary>
    public partial class ConBar : UserControl
    {
        public ConBar()
        {
            InitializeComponent();
        }

        private Point StartPosition;
        private bool IsMoved = false;

        private void Window_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartPosition = e.GetPosition(this);
        }

        private void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && StartPosition != e.GetPosition(this))
            {
                IsMoved = true;
                Fathers.DragMove();
            }
        }

        private void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMoved)
            {
                IsMoved = false;
                e.Handled = true;
            }
        }

        public dynamic Fathers
        {
            get { return GetValue(FatherProperty); }
            set { SetValue(FatherProperty, value); }
        }

        public static readonly DependencyProperty FatherProperty =
            DependencyProperty.Register("Fathers", typeof(object), typeof(DependencyObject), new PropertyMetadata(""));
    }
}