using DPanel.Controls;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Interop;
using System.Windows.Media;

namespace DPanel
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
            ColorChange();
            SettingRead();
            UpdateCheck();
            Main.comTime1 = new comTime(Main);
            Main.comTime1.Show();
            Main.comTime1.Top = Main.ProfileData.Components.comTime.Top;
            Main.comTime1.Left = Main.ProfileData.Components.comTime.Left;
            Main.comTime1.Width = Main.ProfileData.Components.comTime.Width;
            Main.comTime1.Height = Main.ProfileData.Components.comTime.Height;
            Main.comAI1 = new comAI(Main);
            Main.comAI1.Show();
            Main.comAI1.Top = Main.ProfileData.Components.comAI.Top;
            Main.comAI1.Left = Main.ProfileData.Components.comAI.Left;
           Main.comAI1.Width = Main.ProfileData.Components.comAI.Width;
           Main.comAI1.Height = Main.ProfileData.Components.comAI.Height;
           Main.comBoard1 = new comBoard(Main);
           Main.comBoard1.Show();
           Main.comBoard1.Top = Main.ProfileData.Components.comBoard.Top;
           Main.comBoard1.Left = Main.ProfileData.Components.comBoard.Left;
           Main.comBoard1.Width = Main.ProfileData.Components.comBoard.Width;
            Main.comBoard1.Height = Main.ProfileData.Components.comBoard.Height;
            dynamic Json = File.ReadAllText("./Data/data.json");
            Main.comBoardData = JsonConvert.DeserializeObject(Json);
            for (int i = 1; i <= 10000; i++)
            {
                try
                {
                    if (Main.comBoardData[i - 1].type == "text")
                    {
                        Main.comBoard1.ItemNum += 1;
                        string d = Main.comBoardData[i - 1].text;
                        Main.comBoard1.conObjs[i] = new ConText(Main.comBoard1, i, true, d);
                        Main.comBoard1.Card.Items.Add(Main.comBoard1.conObjs[i]);
                    }
                    else if (Main.comBoardData[i - 1].type == "paint")
                    {
                        Main.comBoard1.ItemNum += 1;
                        string fn = Main.comBoardData[i - 1].path;
                        Main.comBoard1.conObjs[i] = new ConPaint(Main.comBoard1, i);
                        using (var fs = new FileStream(fn, FileMode.Open, FileAccess.Read))
                        {
                            StrokeCollection strokes = new StrokeCollection(fs);
                            (Main.comBoard1.conObjs[i] as ConPaint).inkCanvas.Strokes = strokes;
                        }

                        Main.comBoard1.Card.Items.Add(Main.comBoard1.conObjs[i]);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(ArgumentOutOfRangeException))
                    {
                        break;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
        }


        MainMethod Main = new MainMethod();
        public Color _maincolor = Colors.Blue;
        public bool _isdark;
        public bool IsDark
        {
            get { return _isdark; }
            set
            {
                Console.WriteLine(DateTime.Now.ToString() + " : IsDark " + _isdark.ToString() + " => " + value);
                _isdark = value;
                Main.ProfileData.Settings.IsDark = IsDark;
               SettingApply();
                DarkChange();
            }
        }
        public Color MainColor
        {
            get { return _maincolor; }
            set
            {
                Console.WriteLine(DateTime.Now.ToString() + " : MainColor " + _maincolor.ToString() + " => " + value);
                _maincolor = value;
                Main.ProfileData.Settings.MainColor = MainColor.ToString();
               SettingApply();
                ColorChange();
            }
        }

        public void ColorChange()
        {
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();
            theme.SetPrimaryColor(MainColor);
            paletteHelper.SetTheme(theme);

        }
        public void SettingApply()
        {
            Main.ProfileJson = JsonConvert.SerializeObject(Main.ProfileData);
            File.WriteAllText(Main.ProfilePath, Main.ProfileJson);
        }
        public async void DarkChange()
        {
            await Task.Run(() =>
            {
                if (IsDark)
                {
                    ResourceDictionary resource = new ResourceDictionary();
                    resource.Source = new Uri(@"pack://application:,,,/Resources/MainDictionaryDark.xaml");
                    Application.Current.Resources.MergedDictionaries[0] = resource;
                    var paletteHelper = new PaletteHelper();
                    Theme theme = paletteHelper.GetTheme();
                    theme.SetDarkTheme();
                    paletteHelper.SetTheme(theme);
                }
                else
                {
                    ResourceDictionary resource = new ResourceDictionary();
                    resource.Source = new Uri("pack://application:,,,/Resources/MainDictionaryLight.xaml");
                    Application.Current.Resources.MergedDictionaries[0] = resource;
                    var paletteHelper = new PaletteHelper();
                    Theme theme = paletteHelper.GetTheme();
                    theme.SetLightTheme();
                    paletteHelper.SetTheme(theme);
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

        private void UpdateFiles()
        {
            HandyControl.Controls.Dialog.Show(new ConMessage(Main,this));
        }

        private void s(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("hhhhh");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void UpdateCheck()
        {
            LoadBar.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultCredentials;
                client.Encoding = Encoding.UTF8;
                client.DownloadFile(@"https://gitee.com/kaersie/dpanel-update/raw/master/beta/update.json", "./update.json");
                client.DownloadFile(@"https://gitee.com/kaersie/dpanel-update/raw/master/beta/intro.md", "./intro.md");
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

        private void SettingRead()
        {
            Main.ConfigJson = File.ReadAllText("Config.json");
            Main.ConfigData = JsonConvert.DeserializeObject(Main.ConfigJson);
            Main.ProfilePath = Main.ConfigData.Profile;
            Main.ProfileJson = File.ReadAllText(Main.ProfilePath);
            Main.ProfileData = JsonConvert.DeserializeObject(Main.ProfileJson);
            //读取配置文件

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
           Main.ProfileData.Components.comAI.Top = Main.comAI1.Top;
           Main.ProfileData.Components.comAI.Left = Main.comAI1.Left;
           Main.ProfileData.Components.comBoard.Top = Main.comBoard1.Top;
           Main.ProfileData.Components.comBoard.Left = Main.comBoard1.Left;
           Main.ProfileData.Components.comTime.Top = Main.comTime1.Top;
           Main.ProfileData.Components.comTime.Left = Main.comTime1.Left;
           SettingApply();
            List<object> list = new List<object>();
            for (int i = 1; i <= Main.comBoard1.ItemNum; i++)
            {
                try
                {
                    if (Main.comBoard1.conObjs[i] == null)
                    {
                        continue;
                    }
                    else if (Main.comBoard1.conObjs[i].GetType() == typeof(ConText))
                    {
                        var a = new BoardClass();
                        a.type = "text";
                        a.text = (Main.comBoard1.conObjs[i] as ConText).Text.Text;
                        list.Add(a);
                    }
                    else if (Main.comBoard1.conObjs[i].GetType() == typeof(ConPaint))
                    {
                        var a = new BoardClass();
                        a.type = "paint";
                        string path = "./Data/" + DateTime.Now.ToFileTime().ToString() + ".isf";
                        a.path = path;
                        list.Add(a);
                        using (FileStream fileStream = new FileStream(path, FileMode.Create))
                        {
                            (Main.comBoard1.conObjs[i] as ConPaint).inkCanvas.Strokes.Save(fileStream);
                            fileStream.Flush();
                        }
                        Console.WriteLine("Done");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(ArgumentOutOfRangeException))
                    {
                        break;
                    }
                    else { continue; }
                }
            }
            dynamic json = JsonConvert.SerializeObject(list);
            File.WriteAllText("./Data/data.json", json);

            Environment.Exit(0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
           Main.ProfileData.Components.comAI.Top = Main.comAI1.Top;
           Main.ProfileData.Components.comAI.Left = Main.comAI1.Left;
           Main.ProfileData.Components.comBoard.Top = Main.comBoard1.Top;
           Main.ProfileData.Components.comBoard.Left = Main.comBoard1.Left;
           Main.ProfileData.Components.comTime.Top = Main.comTime1.Top;
           Main.ProfileData.Components.comTime.Left = Main.comTime1.Left;
          SettingApply();
        }

        public void SettingLoad()
        {
            Main.CurrentVersion1 = Main.ConfigData.Version1;
            Main.CurrentVersion2 = Main.ConfigData.Version2;
            Main.CurrentVersion3 = Main.ConfigData.Version3;
            Main.CurrentName = Main.ConfigData.Name;
            MainColor =(Color)Main.ProfileData.Settings.MainColor;
            IsDark = (bool)Main.ProfileData.Settings.IsDark;
        }

        private void MarkdownUpdate()
        {
            UpdateMarkdownLabel.Markdown = File.ReadAllText("./intro.md");
        }




    }
}