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

namespace DPanel
{
    /// <summary>
    /// 
    /// </summary>
    /// 
            /// <summary>
        /// 向桌面发送消息
        /// </summary>

    public partial class comTime : Window
    {
        public string WeatherJson;
        public dynamic WeatherData;
        public dynamic Main;
        public dynamic ProgramHandle;

        public comTime(MainWindow MainW)
        {
            InitializeComponent();
            Main = MainW;
            //调用主窗口函数
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOSIZE = 0x0001;
        const int HWND_BOTTOM = 1;
        public void sets(object sender, EventArgs e)
        {
            SetWindowPos(new WindowInteropHelper(this).Handle, new IntPtr(HWND_BOTTOM), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            DispatcherTimer BottomTimer = new DispatcherTimer();
            BottomTimer.Interval = new TimeSpan(1);//1s
            BottomTimer.Tick += new EventHandler(sets);
            BottomTimer.Start();




            DispatcherTimer TimeUpdateTimer = new DispatcherTimer();
            TimeUpdateTimer.Interval = new TimeSpan(100000000);//1s
            TimeUpdateTimer.Tick += new EventHandler(TimeUpdate);
            TimeUpdateTimer.Start();
            Task task = new Task(() => {
                TimeSet();
                WeatherSet();
            });
            task.RunSynchronously();
            // 获取并设置更新时间&日期


            try
            {

               string a = Main.IntelligentAnswer();
               dynamic data = JsonConvert.DeserializeObject(a);
                IntelligentText.Text = data.result;
            }
            catch
            {

            }
            //获取智慧问候并应用
        }

        public void TimeSet()
        {
            TimeLabel.Content = DateTime.Now.ToShortTimeString();
            DateLabel.Content = DateTime.Now.ToString("yyyy/MM/dd");
            string[] WeekDays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            DayLabel.Content = WeekDays[Convert.ToInt32(DateTime.Now.DayOfWeek)];


        }

        public void TimeUpdate(object sender, EventArgs e)
        {
            Task task = new Task(TimeSet);
            task.RunSynchronously();


        }

        public void WeatherSet()
        {

            WeatherData = Main.WeatherGet();

            string WeatherStr = WeatherData.data.tianqi.weather;

            if (WeatherStr == "晴")
            {
                WeatherIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherSunny;
            }
            else if (WeatherStr == "阴")
            {
                WeatherIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherCloudy;
            }
            else if (WeatherStr == "雾")
            {
                WeatherIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherFog;
            }
            else if (WeatherStr.Contains("雨"))
            {
                WeatherIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherPouring;
            }
            else if (WeatherStr.Contains("雪"))
            {
                WeatherIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherSnowy;
            }
            else if (WeatherStr.Contains("云"))
            {
                WeatherIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherPartlyCloudy;
            }
            else if (WeatherStr.Contains("雷"))
            {
                WeatherIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherLightning;
            }
            else if (WeatherStr.Contains("风"))
            {
                WeatherIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WeatherWindy;
            }


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
            }
        }

    }

}
