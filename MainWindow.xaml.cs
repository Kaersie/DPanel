using System;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO.Compression;
using RestSharp;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace DPanel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public string WeatherJson;
        public dynamic WeatherData;
        public string ConfigJson, ProfilePath, ProfileJson, CurrentVersion, CurrentName, UpdateJson;
        public dynamic ConfigData, ProfileData, UpdateData;
        const string API_KEY = "tLGGAI0y4JC91xcL1n9MqysM";
        const string SECRET_KEY = "WgSQbsnSIhhH8QhfR7uQ8AEJRsq1YMPg";//这是卡尔斯厄的apikey，如需编译请替换


        public void ProfileApply()
        {
            ProfileJson = JsonConvert.SerializeObject(ProfileData);
            File.WriteAllText(ProfilePath, ProfileJson);


        }
        private void AutoStartButton_Checked(object sender, RoutedEventArgs e)
        {
            ProfileData.Settings.AutoStart = AutoStartButton.IsChecked;
            ProfileApply();
            //应用更改
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateButton.Content.ToString() == "检测更新")
            {
                Task task = new Task(UpdateCheck);
                task.RunSynchronously();
            }
            else
            {
                Task task = new Task(UpdateFiles);
                task.RunSynchronously();
            }
        }

        private void UpdateFiles()
        {
            WebClient client = new WebClient();
            client.Credentials = CredentialCache.DefaultCredentials;
            client.Encoding = Encoding.UTF8;
            client.DownloadFile(@"https://gitee.com/kaersie/dpanel-update/raw/master/beta/file.zip", "./file.zip");
            try
            {
                Directory.Delete(@"./UpdateFile", true);
            }
            catch
            {
            }
            ZipFile.ExtractToDirectory("./file.zip", "./UpdateFile");
            Process.Start("Update.exe");
            ConfigData.Version = UpdateData.Version;
            CurrentVersion = UpdateData.Version;
            ConfigData.Name = UpdateData.Name;
            CurrentName = UpdateData.Name;
            ConfigJson = JsonConvert.SerializeObject(ConfigData);
            File.WriteAllText("./Config.json", ConfigJson);
            Environment.Exit(0);


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

            Task task = new Task(() => {
                ProfilesLoad();
                UpdateCheck();
                MarkdownUpdate();
            });
            task.RunSynchronously();



            comTime comTime1 = new comTime(this);
            comTime1.Show();
            comTime1.Top = ProfileData.Components.comTime.Top;
            comTime1.Left = ProfileData.Components.comTime.Left ;

            comAI comAI1 = new comAI(this);
            comAI1.Show();

        }
        private void UpdateCheck()
        {
            WebClient client = new WebClient();
            client.Credentials = CredentialCache.DefaultCredentials;
            client.Encoding = Encoding.UTF8;
            client.DownloadFile(@"https://gitee.com/kaersie/dpanel-update/raw/master/beta/update.json", "./update.json");
            client.DownloadFile(@"https://gitee.com/kaersie/dpanel-update/raw/master/beta/intro.md", "./intro.md");
            UpdateJson = File.ReadAllText("./update.json");
            UpdateData = JsonConvert.DeserializeObject(UpdateJson);
            VersionLabel.Text = "当前版本：" + CurrentVersion + " " + CurrentName + "   最新版本：" + UpdateData.Version + " " + UpdateData.Name;
            if (int.Parse(UpdateData.Version.Value) > int.Parse(CurrentVersion))
            {
                UpdateButton.Content = "重启更新";
                UpdateLabel.Text = "检测到新版本";
                UpdateIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.FileArrowUpDown;
            }



        }
        private void ProfilesLoad()
        {
            ConfigJson = File.ReadAllText("Config.json");
            ConfigData = JsonConvert.DeserializeObject(ConfigJson);

            ProfilePath = ConfigData.Profile;
            ProfileJson = File.ReadAllText(ProfilePath);
            ProfileData = JsonConvert.DeserializeObject(ProfileJson);
            //读取配置文件

            SettingApply(ProfileData.Settings);


        }

        public void SettingApply(dynamic SettingsData)
        {
            AutoStartButton.IsChecked = SettingsData.AutoStart;
            CurrentVersion = ConfigData.Version;
            CurrentName = ConfigData.Name;


        }
        private void MarkdownUpdate()
        {
            UpdateMarkdownLabel.Markdown = File.ReadAllText("./intro.md");


        }
        static string GetAccessToken()
        {
            var client = new RestClient($"https://aip.baidubce.com/oauth/2.0/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", API_KEY);
            request.AddParameter("client_secret", SECRET_KEY);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var result = JsonConvert.DeserializeObject<dynamic>(response.Content);
            return result.access_token.ToString();
        }

        public string IntelligentAnswer( dynamic body)
        {
            var client = new RestClient($"https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/ernie-lite-8k?access_token={GetAccessToken()}");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic data = JsonConvert.DeserializeObject(response.Content);

            return data.result;
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

        //设置窗口位于最底层，具体方法来源于:https://www.cnblogs.com/choumengqizhigou/p/15702980.html
        public static class Win32Func
        {
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string className, string winName);
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessageTimeout(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, uint fuFlage, uint timeout, IntPtr result);
            public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
            [DllImport("user32.dll")]
            public static extern bool EnumWindows(EnumWindowsProc proc, IntPtr lParam);
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string winName);
            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
            [DllImport("user32.dll")]
            public static extern IntPtr SetParent(IntPtr hwnd, IntPtr parentHwnd);
        }
        public void BottomSet(System.Windows.Window TargetForm)
        {
            var ProgramHandle = Win32Func.FindWindow("Progman", null);

   IntPtr result = IntPtr.Zero;
       Win32Func.SendMessageTimeout(ProgramHandle, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 2, result);
     
       Win32Func.EnumWindows((hwnd, lParam) =>
       {
           if (Win32Func.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
           {
               IntPtr tempHwnd = Win32Func.FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);
     
               Win32Func.ShowWindow(tempHwnd, 0);
           }
           return true;
       }, IntPtr.Zero);
          Console.WriteLine(  Win32Func.SetParent(new WindowInteropHelper(TargetForm).Handle, ProgramHandle));
        }

    }
}
