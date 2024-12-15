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
    /// ErrorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(Exception Error)
        {
            InitializeComponent();
            ErrorData = Error.Message + "\n 来源：" + Error.Source + "\n " + Error.ToString();
        }
        public string ErrorData;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ErrorLabel.Text = ErrorData;
        }
    }
}
