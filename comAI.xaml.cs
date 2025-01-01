using System;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net;
using System.IO;
using System.Windows.Threading;
using RestSharp;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Forms;

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
        public dynamic Main;
        public dynamic ProgramHandle;

    public comAI(MainWindow MainW)
        {
            InitializeComponent();
            Main = MainW;

    }

   
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                Main.ProfileData.Components.comAI.Top = this.Top;
                Main.ProfileData.Components.comAI.Left = this.Left;
                Main.ProfileApply();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
 
        }

        string HistoryAI= "";

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            AskButton.IsEnabled = false;
            LoadBar.Visibility = Visibility.Visible;
            string Question = QuestionText.Text;
            AskButton.IsEnabled = false;
            TextBlock TextItem1 = new TextBlock();
            TextItem1.TextWrapping = TextWrapping.Wrap;
            TextItem1.Text = "我: " + Question;
            AnswerText.Items.Add(TextItem1);
            QuestionText.Text = "";
            HistoryAI += @"{""role"":""user"",""content"": """ +  Question.Replace("\""," ") + @"""}";
            string Answer=await Task.Run(() =>
            {
             string  Result = Main.IntelligentAnswer(@"{""messages"":["+HistoryAI+@"],""temperature"":1.00,""top_p"":1.0,""penalty_score"":2,""system"":""你叫乔治，DPanel的AI助手""}");
                return Result;
            });
            if(Answer==null || Answer == "")
            {
                HistoryAI = "";
                Answer = "出了点小错，重新问我吧~";
            }
            else
            {
                HistoryAI +=@",{""role"":""assistant"",""content"": """ + Answer.Replace("\"", " " )+ @"""},";
            }
            Console.WriteLine(Question + "   " + Answer + "   " + HistoryAI);
            TextBlock TextItem2 = new TextBlock();
            TextItem2.TextWrapping = TextWrapping.Wrap;
            TextItem2.Text = "乔治: " + Answer;
            AnswerText.Items.Add(TextItem2);
            AskButton.IsEnabled = true;
            TextItem1.MaxWidth = 280;
            TextItem2.MaxWidth = 280;
            LoadBar.Visibility = Visibility.Hidden;


        }




    }

}
