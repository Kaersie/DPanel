using MahApps.Metro.IconPacks;
using System.Windows.Controls;

namespace ClassPanel.Controls
{
    /// <summary>
    /// ConSettingTitle.xaml 的交互逻辑
    /// </summary>
    public partial class ConSettingTitle : UserControl
    {
        public ConSettingTitle()
        {
            InitializeComponent();
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

        public string _text;
        public PackIconBootstrapIconsKind _kind;
    }
}