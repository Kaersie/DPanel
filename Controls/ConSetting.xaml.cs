using MahApps.Metro.IconPacks;
using System.Windows.Controls;

namespace DPanel.Controls
{
    /// <summary>
    /// ConSetting.xaml 的交互逻辑
    /// </summary>
    public partial class ConSetting : UserControl
    {
        public ConSetting()
        {
            InitializeComponent();
        }

        public string _subtitle;
        public string _text;
        public string _link="";
        public PackIconBootstrapIconsKind _kind;

        public string Subtitle
        {
            get { return _subtitle; }
            set { _subtitle = value; }
        }

        public string Link
        {
            get { return _link; }
            set { _link = value; }
        }
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public PackIconBootstrapIconsKind Kind
        {
            get { return _kind; }
            set { _kind = value; }
        }

        private void StackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Link != "")
            {
                System.Diagnostics.Process.Start(Link);
            }
        }
    }
}