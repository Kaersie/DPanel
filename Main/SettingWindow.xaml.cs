using ClassPanel.Controls;
using HandyControl.Controls;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace ClassPanel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    ///

    public partial class SettingWindow : System.Windows.Window
    {
        public SettingWindow()
        {
            InitializeComponent();

            ConBar Bar = new ConBar();
            grid.Children.Add(Bar);
            Bar.Fathers = this;
            ColorChange();
            SettingRead();
            UpdateCheck();
            Main.MainWindow1.Show();
            ProfileChange();
        }

        public void ProfileChange()//将Profile数据应用在软件本身
        {
        }

        public void ConfigSave()//将config保存
        {
            try
            {
                Main.ConfigJson = JsonConvert.SerializeObject(Main.ConfigData);
                File.WriteAllText("./Config.json", Main.ConfigJson);
            }
            catch (Exception ex)
            {
                Growl.WarningGlobal("错误:" + ex.Message);
            }
        }

        private MainMethod Main = new MainMethod();
        public Color _maincolor = Colors.Blue;
        public bool _isdark;

        public bool IsDark//是否开启黑暗模式
        {
            get { return _isdark; }
            set
            {
                Console.WriteLine(DateTime.Now.ToString() + " : IsDark " + _isdark.ToString() + " => " + value);
                _isdark = value;
                try
                {
                    Main.ProfileData.Settings.IsDark = IsDark;
                }
                catch (Exception ex)
                {
                    Growl.WarningGlobal("更换主题错误:" + ex.Message);
                }
                ProfileSave();
                DarkChange();
            }
        }

        public string ProfileDataFileString//Profile地址
        {
            get { return _profiledatafilestring; }
            set
            {
                _profiledatafilestring = value;
                Console.WriteLine(DateTime.Now.ToString() + " : ProfileDataFileString " + Main.ProfilePath.ToString() + " => " + value);
                try
                {
                    Main.ConfigData.Profile = value;
                    _profiledatafilestring = value;
                    ConfigSave();
                    SettingRead();
                    SettingLoad();
                    ProfileChange();
                    ProfileDataFileText.Text = _profiledatafilestring;
                    Growl.SuccessGlobal("更换数据文件成功");
                }
                catch (Exception ex)
                {
                    Growl.WarningGlobal("更换数据文件错误:" + ex.Message);
                }
            }
        }

        public string _profiledatafilestring;

        public Color MainColor//主题色
        {
            get { return _maincolor; }
            set
            {
                Console.WriteLine(DateTime.Now.ToString() + " : MainColor " + _maincolor.ToString() + " => " + value);
                _maincolor = value;
                try
                {
                    Main.ProfileData.Settings.MainColor = MainColor.ToString();
                }
                catch (Exception ex)
                {
                    Growl.WarningGlobal("更换主题错误:" + ex.Message);
                }
                ProfileSave();
                ColorChange();
            }
        }

        public void ColorChange()//更改主题色
        {
            try
            {
                var paletteHelper = new PaletteHelper();
                Theme theme = paletteHelper.GetTheme();
                theme.SetPrimaryColor(MainColor);
                paletteHelper.SetTheme(theme);
            }
            catch (Exception ex)
            {
                Growl.WarningGlobal("更换主题错误:" + ex.Message);
            }
        }

        public void ProfileSave()//Profile保存
        {
            try
            {
                Main.ProfileJson = JsonConvert.SerializeObject(Main.ProfileData);
                File.WriteAllText(Main.ProfilePath, Main.ProfileJson);
            }
            catch (Exception ex)
            {
                Growl.WarningGlobal("错误:" + ex.Message);
            }
        }

        public async void DarkChange()//改变暗黑主题
        {
            await Task.Run(() =>
            {
                if (IsDark)
                {
                    try
                    {
                        ResourceDictionary resource = new ResourceDictionary();
                        resource.Source = new Uri(@"pack://application:,,,/Resources/MainDictionaryDark.xaml");
                        Application.Current.Resources.MergedDictionaries[0] = resource;
                        var paletteHelper = new PaletteHelper();
                        Theme theme = paletteHelper.GetTheme();
                        theme.SetDarkTheme();
                        paletteHelper.SetTheme(theme);
                    }
                    catch (Exception ex)
                    {
                        Growl.WarningGlobal("更换主题错误:" + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        ResourceDictionary resource = new ResourceDictionary();
                        resource.Source = new Uri("pack://application:,,,/Resources/MainDictionaryLight.xaml");
                        Application.Current.Resources.MergedDictionaries[0] = resource;
                        var paletteHelper = new PaletteHelper();
                        Theme theme = paletteHelper.GetTheme();
                        theme.SetLightTheme();
                        paletteHelper.SetTheme(theme);
                    }
                    catch (Exception ex)
                    {
                        Growl.WarningGlobal("更换主题错误:" + ex.Message);
                    }
                }
            });
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateButton.Content.ToString() == "检测更新")
            {
                UpdateCheck();
            }
            else
            {
                UpdateFiles();
            }
        }

        private void UpdateFiles()//点更新后
        {
            HandyControl.Controls.Dialog.Show(new ConMessage(Main, this));
        }

        private void s(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("hhhhh");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProfileDataFileText.Text = ProfileDataFileString;//bug
        }

        private async void UpdateCheck()
        {
            try
            {
                LoadBar.Visibility = Visibility.Visible;
                await Task.Run(() =>
                {
                    try
                    {
                        WebClient client = new WebClient();
                        client.Credentials = CredentialCache.DefaultCredentials;
                        client.Encoding = Encoding.UTF8;
                        client.DownloadFile(@"https://gitee.com/kaersie/ClassPanel-update/raw/master/beta/update.json", "./update.json");
                        client.DownloadFile(@"https://gitee.com/kaersie/ClassPanel-update/raw/master/beta/intro.md", "./intro.md");
                    }
                    catch (Exception Ex)
                    {
                        Growl.WarningGlobal("网络错误(UpdateCheck):" + Ex.Message);
                    }
                });
                Main.UpdateJson = File.ReadAllText("./update.json");
                Main.UpdateData = JsonConvert.DeserializeObject(Main.UpdateJson);
                VersionLabel.Text = $"当前版本：{Main.CurrentVersion1}.{Main.CurrentVersion2}.{Main.CurrentVersion3}  {Main.CurrentName} ; 最新版本：{Main.UpdateData.Version1}.{Main.UpdateData.Version2}.{Main.UpdateData.Version3}  {Main.CurrentName}  ";
                if (int.Parse(Main.UpdateData.Version1.Value) > int.Parse(Main.CurrentVersion1) || int.Parse(Main.UpdateData.Version1.Value) == int.Parse(Main.CurrentVersion1) && int.Parse(Main.UpdateData.Version2.Value) > int.Parse(Main.CurrentVersion2) || int.Parse(Main.UpdateData.Version1.Value) == int.Parse(Main.CurrentVersion1) && int.Parse(Main.UpdateData.Version2.Value) == int.Parse(Main.CurrentVersion2) && int.Parse(Main.UpdateData.Version3.Value) > int.Parse(Main.CurrentVersion3))
                {
                    UpdateButton.Content = "现在更新";
                    UpdateLabel.Text = "检测到新版本";
                    UpdateIcon.Kind = PackIconBootstrapIconsKind.CloudDownload;
                }
                MarkdownUpdate();

                LoadBar.Visibility = Visibility.Hidden;
            }
            catch
            {
                LoadBar.Visibility = Visibility.Hidden;
            }
        }

        private void SettingRead()
        {
            try
            {
                Main.ConfigJson = File.ReadAllText("Config.json");
                Main.ConfigData = JsonConvert.DeserializeObject(Main.ConfigJson);
                Main.ProfilePath = Main.ConfigData.Profile;
                Main.ProfileJson = File.ReadAllText(Main.ProfilePath);
                Main.ProfileData = JsonConvert.DeserializeObject(Main.ProfileJson);
                //读取配置文件
            }
            catch (Exception ex)
            {
                Growl.WarningGlobal("配置文件加载错误(SettingRead):" + ex.Message);
                Growl.ErrorGlobal("检测到 ClassPanel 的配置文件可能损坏，请尽快到 设置 中重置 ClassPanel 以防软件崩溃");
            }

            SettingLoad();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Hidden)
            {
                this.Visibility = Visibility.Visible;
                Show.Header = "隐藏";
            }
            else
            {
                this.Visibility = Visibility.Hidden;
                Show.Header = "显示";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            Show.Header = "显示";
        }

        private void MainColorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ProfileSave();
            Environment.Exit(0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        public void SettingLoad()
        {
            try
            {
                Main.CurrentVersion1 = Main.ConfigData.Version1;
                Main.CurrentVersion2 = Main.ConfigData.Version2;
                Main.CurrentVersion3 = Main.ConfigData.Version3;
                Main.CurrentName = Main.ConfigData.Name;
                //版本加载

                MainColor = (Color)Main.ProfileData.Settings.MainColor;
                IsDark = (bool)Main.ProfileData.Settings.IsDark;
                //设置选项加载

                Main.ComNum = Main.ProfileData.ComNum;
                for (int i = 0; i <= Main.ComNum - 1; i++)
                {
                    switch (Main.ProfileData.Components[i].Type.ToString())
                    {
                        case "comAI":
                            {
                                comAI comAI1 = new comAI(Main);
                                Main.MainWindow1.MainPanel.Children.Add(comAI1);
                                Main.Components.Add(comAI1);
                                break;
                            }
                        case "comBoard":
                            {
                                comBoard comBoard1 = new comBoard(Main);
                                Main.comBoardLoad(comBoard1);
                                Main.MainWindow1.MainPanel.Children.Add(comBoard1);
                                Main.Components.Add(comBoard1);
                                break;
                            }
                        case "comTime":
                            {
                                comTime comTime1 = new comTime(Main);
                                Main.MainWindow1.MainPanel.Children.Add(comTime1);
                                Main.Components.Add(comTime1);
                                break;
                            }
                    }
                }
                //组件加载
            }
            catch (Exception ex)
            {
                Growl.WarningGlobal("错误(SettingLoad):" + ex.Message);
            }
        }

        private void MarkdownUpdate()
        {
            try
            {
                UpdateMarkdownLabel.Markdown = File.ReadAllText("./intro.md");
            }
            catch (Exception ex)
            {
                Growl.WarningGlobal("错误:" + ex.Message);
            }
        }

        private void ProfileDataFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "ClassPanel 数据文件 (.json)|*.json";
            if (dialog.ShowDialog() == true)
            {
                ProfileDataFileString = dialog.FileName;
            }
        }
    }
}