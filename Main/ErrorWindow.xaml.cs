using System;
using System.Windows;

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