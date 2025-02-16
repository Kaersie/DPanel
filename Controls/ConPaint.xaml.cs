using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClassPanel.Controls
{
    /// <summary>
    /// ConPaint.xaml 的交互逻辑
    /// </summary>
    public partial class ConPaint : UserControl
    {
        private dynamic comBoard1;
        private dynamic ItemNum;

        public ConPaint(dynamic comboard1, int Num)
        {
            InitializeComponent();
            comBoard1 = comboard1;
            ItemNum = Num;
            inkCanvas.DefaultDrawingAttributes.Color = Color.FromRgb(255, 0, 0);
            inkCanvas.DefaultDrawingAttributes.FitToCurve = true;
            inkCanvas.DefaultDrawingAttributes.IgnorePressure = true;
            inkCanvas.DefaultDrawingAttributes.Height = 5;
            inkCanvas.DefaultDrawingAttributes.Width = 5;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            comBoard1.ExitObj(ItemNum);
        }
    }
}