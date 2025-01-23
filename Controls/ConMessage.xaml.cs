using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;

namespace DPanel.Controls
{
    /// <summary>
    /// ConMessage.xaml 的交互逻辑
    /// </summary>
    public partial class ConMessage : UserControl
    {
        public ConMessage(MainMethod main,SettingWindow settingwindow1)
        {
            InitializeComponent();
            Main = main;
            SettingWindow1 = settingwindow1;
        }
        public SettingWindow SettingWindow1;
        public MainMethod Main;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://gitee.com/kaersie/dpanel-update/raw/master/beta/file.zip");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Directory.Delete(@"./UpdateFile", true);
            }
            catch
            {
            }
            try
            {
                ZipFile.ExtractToDirectory("./file.zip", "./UpdateFile");
                Process.Start("Update.exe");
                Main.ConfigData.Version1 = Main.UpdateData.Version1;
                Main.ConfigData.Version2 = Main.UpdateData.Version2;
                Main.ConfigData.Version3 = Main.UpdateData.Version3;
                Main.CurrentVersion1 = Main.UpdateData.Version1;
                Main.CurrentVersion2 = Main.UpdateData.Version1;
                Main.CurrentVersion2 = Main.UpdateData.Version1;

                Main.ConfigData.Name = Main.UpdateData.Name;
                Main.CurrentName = Main.UpdateData.Name;
                Main.ConfigJson = JsonConvert.SerializeObject(Main.ConfigData);
                File.WriteAllText("./Config.json", Main.ConfigJson);
                SettingWindow1.UpdateButton.Content = "检测更新";
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                ErrorWindow err = new ErrorWindow(ex);
                err.Show();
            }
        }
    }
}