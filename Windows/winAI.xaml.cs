using ClassPanel.Controls;
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

namespace ClassPanel.Windows
{
    /// <summary>
    /// winAI.xaml 的交互逻辑
    /// </summary>
    public partial class winAI : Window
    {
        public string WeatherJson;
        public dynamic WeatherData;
        public MainMethod Main;
        public dynamic ProgramHandle;
        public comAI c1;
        public winAI(MainMethod main,comAI cc1)
        {
            InitializeComponent();
            c1 = cc1;
            ConBar Bar = new ConBar();
            grid.Children.Add(Bar);
            Main = main;
            Bar.Fathers = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private string HistoryAI = "";

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            AskButton.IsEnabled = false;
            LoadBar.Visibility = Visibility.Visible;
            string Question = QuestionText.Text;
            AskButton.IsEnabled = false;
            TextBlock TextItem1 = new TextBlock();
            TextItem1.TextWrapping = TextWrapping.Wrap;
            TextItem1.Text = "我: " + Question;
            TextItem1.Margin = new Thickness(10, 10, 10, 10);
            AnswerText.Items.Add(TextItem1);
            QuestionText.Text = "";
            HistoryAI += @"{""role"":""user"",""content"": """ + Question.Replace("\"", " ") + @"""}";
            string Answer = await Task.Run(() =>
            {
                string Result = Main.IntelligentAnswer(@"{""messages"":[" + HistoryAI + @"],""temperature"":0.95,""top_p"":0.7,""penalty_score"":1,""system"":""你叫乔治，ClassPanel的AI助手""}");
                return Result;
            });
            if (Answer == null || Answer == "")
            {
                HistoryAI = "";
                Answer = "出了点小错，重新问我吧~";
            }
            else
            {
                HistoryAI += @",{""role"":""assistant"",""content"": """ + Answer.Replace("\"", " ") + @"""},";
            }
            Console.WriteLine(Question + "   " + Answer + "   " + HistoryAI);
            TextBlock TextItem2 = new TextBlock();
            TextItem2.TextWrapping = TextWrapping.Wrap;
            TextItem2.Text = "乔治: " + Answer;
            TextItem2.Margin = new Thickness(10, 10, 10, 10);
            AnswerText.Items.Add(TextItem2);
            AskButton.IsEnabled = true;
            TextItem1.MaxWidth = 280;
            TextItem2.MaxWidth = 280;
            LoadBar.Visibility = Visibility.Hidden;
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            c1.IswinAILoad = false;
        }
    }
}
