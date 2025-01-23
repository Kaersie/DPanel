using DPanel.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DPanel
{
    /// <summary>
    ///
    /// </summary>
    ///
    /// <summary>
    /// 向桌面发送消息
    /// </summary>

    public partial class comAI : Window
    {
        public string WeatherJson;
        public dynamic WeatherData;
        public MainMethod Main;
        public dynamic ProgramHandle;

        public comAI(MainMethod main)
        {
            InitializeComponent();

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
                  string Result = Main.IntelligentAnswer(@"{""messages"":[" + HistoryAI + @"],""temperature"":0.95,""top_p"":0.7,""penalty_score"":1,""system"":""你叫乔治，DPanel的AI助手""}");
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
    }
}