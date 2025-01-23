using MahApps.Metro.IconPacks;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;

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
        public MainMethod Main;
        public dynamic ProgramHandle;

        public comTime(MainMethod main)
        {
            InitializeComponent();
            Main = main;
            //调用主窗口函数
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);

        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
        private const int HWND_BOTTOM = 1;

        public void sets(object sender, EventArgs e)
        {
            SetWindowPos(new WindowInteropHelper(this).Handle, new IntPtr(HWND_BOTTOM), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer BottomTimer = new DispatcherTimer();
            BottomTimer.Interval = new TimeSpan(1);//1s
            BottomTimer.Tick += new EventHandler(sets);
            //   BottomTimer.Start();

            DispatcherTimer TimeUpdateTimer = new DispatcherTimer();
            TimeUpdateTimer.Interval = new TimeSpan(100000000);//1s
            TimeUpdateTimer.Tick += new EventHandler(TimeUpdate);
            TimeUpdateTimer.Start();
            TimeSet();
            WeatherSet();

            // 获取并设置更新时间&日期

            IntelligentText.Text = await Task.Run(() =>
            {
                try
                {
                    string x = Main.IntelligentAnswer(@"{""messages"":[{""role"":""user"",""content"":""go""},{""role"":""assistant"",""content"":""春风十里，温暖如你""},{""role"":""user"",""content"":""go""},{""role"":""assistant"",""content"":""晨光熹微，笑靥如花""},{""role"":""user"",""content"":""go""}],""temperature"":0.95,""top_p"":0.7,""penalty_score"":1,""system"":""当用户发送“go”时，回复8字名言，禁止发送除此8字外内容，有文采一点，不要重复""}");
                    if (x.Length >= 20)
                    {
                        x = Main.IntelligentAnswer(@"{""messages"":[{""role"":""user"",""content"":""go""},{""role"":""assistant"",""content"":""春风十里，温暖如你""},{""role"":""user"",""content"":""go""},{""role"":""assistant"",""content"":""晨光熹微，笑靥如花""},{""role"":""user"",""content"":""go""}],""temperature"":0.95,""top_p"":0.7,""penalty_score"":1,""system"":""当用户发送“go”时，回复8字名言，禁止发送除此8字外内容，有文采一点，不要重复""}");
                        if (x.Length >= 20)
                        {
                            return "轻舟已过万重山";
                        }
                        else
                        {
                            return x;
                        }
                    }
                    else
                    {
                        return x;
                    }
                }
                catch
                {
                    return "轻舟已过万重山";
                }
            });
            //获取智慧激励并应用
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

        public dynamic WeatherGet()
        {
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("https://api.vore.top/api/Weather");
            Req.Method = "Get";
            try
            {
                HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                var Stream = Resp.GetResponseStream();
                using (StreamReader reader = new StreamReader(Stream, Encoding.UTF8))
                {
                    WeatherJson = reader.ReadToEnd();
                    WeatherData = JsonConvert.DeserializeObject(WeatherJson);
                }

                Console.WriteLine(WeatherData);
            }
            catch (Exception ErrorData)
            {
                new ErrorWindow(ErrorData).ShowDialog();
            }
            return WeatherData;
        }

        public async void WeatherSet()
        {
            string WeatherStr = "";
            await Task.Run(() =>
            {
                try
                {
                    WeatherData =WeatherGet();
                    WeatherStr = WeatherData.data.tianqi.weather;
                    Console.WriteLine(WeatherStr);
                }
                catch { }
            });

            if (WeatherStr == "晴")
            {
                WeatherIcon.Kind = PackIconBootstrapIconsKind.Sun;
            }
            else if (WeatherStr == "阴")
            {
                WeatherIcon.Kind = PackIconBootstrapIconsKind.Cloud;
            }
            else if (WeatherStr == "雾")
            {
                WeatherIcon.Kind = PackIconBootstrapIconsKind.CloudFog;
            }
            else if (WeatherStr.Contains("雨"))
            {
                WeatherIcon.Kind = PackIconBootstrapIconsKind.CloudRain;
            }
            else if (WeatherStr.Contains("雪"))
            {
                WeatherIcon.Kind = PackIconBootstrapIconsKind.Snow;
            }
            else if (WeatherStr.Contains("云"))
            {
                WeatherIcon.Kind = PackIconBootstrapIconsKind.Clouds;
            }
            else if (WeatherStr.Contains("雷"))
            {
                WeatherIcon.Kind = PackIconBootstrapIconsKind.Lightning;
            }
            else if (WeatherStr.Contains("风"))
            {
                WeatherIcon.Kind = PackIconBootstrapIconsKind.Wind;
            }
        }

        //拖动：https://blog.csdn.net/u013113678/article/details/121071628
        private Point StartPosition;

        private bool IsMoved = false;

        private void Window_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartPosition = e.GetPosition(this);
        }

        private void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && StartPosition != e.GetPosition(this))
            {
                IsMoved = true;
                DragMove();
            }
        }

        private void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMoved)
            {
                IsMoved = false;
                e.Handled = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private async void IntelligentText_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IntelligentText.Text = await Task.Run(() =>
            {
                try
                {
                    string x = Main.IntelligentAnswer(@"{""messages"":[{""role"":""user"",""content"":""go""},{""role"":""assistant"",""content"":""春风十里，温暖如你""},{""role"":""user"",""content"":""go""},{""role"":""assistant"",""content"":""晨光熹微，笑靥如花""},{""role"":""user"",""content"":""go""}],""temperature"":0.95,""top_p"":0.7,""penalty_score"":1,""system"":""当用户发送“go”时，回复8字名言，禁止发送除此8字外内容，有文采一点，不要重复""}");
                    if (x.Length >= 20)
                    {
                        x = Main.IntelligentAnswer(@"{""messages"":[{""role"":""user"",""content"":""go""},{""role"":""assistant"",""content"":""春风十里，温暖如你""},{""role"":""user"",""content"":""go""},{""role"":""assistant"",""content"":""晨光熹微，笑靥如花""},{""role"":""user"",""content"":""go""}],""temperature"":0.95,""top_p"":0.7,""penalty_score"":1,""system"":""当用户发送“go”时，回复8字名言，禁止发送除此8字外内容，有文采一点，不要重复""}");
                        if (x.Length >= 20)
                        {
                            return "轻舟已过万重山";
                        }
                        else
                        {
                            return x;
                        }
                    }
                    else
                    {
                        return x;
                    }
                }
                catch
                {
                    return "轻舟已过万重山";
                }
            });
            //获取智慧激励并应用
        }
    }
}