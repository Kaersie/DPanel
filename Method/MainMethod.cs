using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DPanel
{
    internal class BoardClass
    {
        public string type { get; set; }
        public string text { get; set; }
        public string path { get; set; }
    }

    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SolidColorBrush)value).Color;
        }
    }
    public class MainMethod
    {

        public dynamic comBoardData;
        public string ConfigJson, ProfilePath, ProfileJson, CurrentVersion1, CurrentVersion2, CurrentVersion3, CurrentName, UpdateJson;
        public dynamic ConfigData, ProfileData, UpdateData;
        public const string API_KEY = "tLGGAI0y4JC91xcL1n9MqysM";
        public const string SECRET_KEY = "WgSQbsnSIhhH8QhfR7uQ8AEJRsq1YMPg";//这是卡尔斯厄的apikey，如需编译请替换
        public comTime comTime1;
        public comAI comAI1;
        public comBoard comBoard1;



        public string IntelligentAnswer(dynamic body)
        {
            var client = new RestClient($"https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/ernie-lite-8k?access_token={GetAccessToken()}");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic data = JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(data.result);

            return data.result;
        }
        private static string GetAccessToken()
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
      

    }
}
