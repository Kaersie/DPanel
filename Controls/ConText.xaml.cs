using HandyControl.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClassPanel.Controls
{
    /// <summary>
    /// ConText.xaml 的交互逻辑
    /// </summary>
    public partial class ConText : UserControl
    {
        private dynamic comBoard1;
        private dynamic ItemNum;

        public ConText(dynamic comboard1, int Num, bool IsSaved, string SavedString)
        {
            InitializeComponent();
            comBoard1 = comboard1;
            if (IsSaved == true)
            {
                ControlButton.Template = (ControlTemplate)this.Resources["normal"];
                Text.Text = SavedString;
            }
            ItemNum = Num;
        }

        private void FinishEdit(object sender, RoutedEventArgs e)
        {
            if (IsChange == true)
            {
                ControlButton.Template = (ControlTemplate)this.Resources["normal"];
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IsChange = false;
            comBoard1.ExitObj(ItemNum);
        }

        private bool IsChange = true;
        private string Question;

        private async void AskAI(object sender, RoutedEventArgs e)
        {
            IsChange = false;
            Button AskButton = ControlButton.Template.FindName("AskButton", ControlButton) as Button;
            ProgressBar LoadBar = ControlButton.Template.FindName("LoadBar", ControlButton) as ProgressBar;
            AskButton.IsEnabled = false;
            LoadBar.Visibility = Visibility.Visible;
            Question = Text.Text;
            string Answer = await Task.Run(() =>
            {
                try
                {
                    string x = comBoard1.Main.IntelligentAnswer(@"{""messages"":[{""role"":""user"",""content"":""" + Question + @"""}],""temperature"":0.95,""top_p"":0.5,""penalty_score"":1,""system"":""将用户发送的内容润色并返回，禁止发送除润色后文本以外的任何内容。字数少一点，有文采一点。案例：改变世界去吧    勇敢前行，踏上改变世界的旅程。""}");
                    Console.WriteLine(x);
                    if (x.Length > Question.Length * 8 || x == "")
                    {
                        return Question;
                    }
                    else
                    {
                        Growl.SuccessGlobal("生成成功");
                        return x;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return Question;
                }
            });
            Text.Text = Answer;
            AskButton.IsEnabled = true;
            LoadBar.Visibility = Visibility.Hidden;
            IsChange = true;
        }

        private void Text_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IsChange == true)
            {
                ControlButton.Template = (ControlTemplate)this.Resources["edit"];
            }
        }
    }
}