using ClassPanel.Controls;
using HandyControl.Controls;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Media;

namespace ClassPanel
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
        public List<object> Components = new List<object>();
        public int ComNum;
        public MainWindow MainWindow1 = new MainWindow();

        public string IntelligentAnswer(dynamic body)
        {
            try
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
            catch (Exception ex)
            {
                Growl.WarningGlobal("网络错误:" + ex.Message);
                return "";
            }
        }

        public void comBoardSave(comBoard comBoard1)
        {
            try
            {
                List<object> list = new List<object>();
                for (int i = 1; i <= comBoard1.ItemNum; i++)
                {
                    try
                    {
                        if (comBoard1.conObjs[i] == null)
                        {
                            continue;
                        }
                        else if (comBoard1.conObjs[i].GetType() == typeof(ConText))
                        {
                            var a = new BoardClass();
                            a.type = "text";
                            a.text = (comBoard1.conObjs[i] as ConText).Text.Text;
                            list.Add(a);
                        }
                        else if (comBoard1.conObjs[i].GetType() == typeof(ConPaint))
                        {
                            var a = new BoardClass();
                            a.type = "paint";
                            string path = "./Data/" + DateTime.Now.ToFileTime().ToString() + ".isf";
                            a.path = path;
                            list.Add(a);
                            using (FileStream fileStream = new FileStream(path, FileMode.Create))
                            {
                                (comBoard1.conObjs[i] as ConPaint).inkCanvas.Strokes.Save(fileStream);
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
            }
            catch (Exception ex)
            {
                Growl.WarningGlobal("错误:" + ex.Message);
            }
        }

        public void comBoardLoad(comBoard comBoard1)
        {
            try
            {
                dynamic Json = File.ReadAllText("./Data/data.json");
                comBoardData = JsonConvert.DeserializeObject(Json);

                for (int i = 1; i <= 10000; i++)
                {
                    try
                    {
                        if (comBoardData[i - 1].type == "text")
                        {
                            comBoard1.ItemNum += 1;
                            string d = comBoardData[i - 1].text;
                            comBoard1.conObjs[i] = new ConText(comBoard1, i, true, d);
                            comBoard1.Card.Items.Add(comBoard1.conObjs[i]);
                        }
                        else if (comBoardData[i - 1].type == "paint")
                        {
                            comBoard1.ItemNum += 1;
                            string fn = comBoardData[i - 1].path;
                            comBoard1.conObjs[i] = new ConPaint(comBoard1, i);
                            using (var fs = new FileStream(fn, FileMode.Open, FileAccess.Read))
                            {
                                StrokeCollection strokes = new StrokeCollection(fs);
                                (comBoard1.conObjs[i] as ConPaint).inkCanvas.Strokes = strokes;
                            }

                            comBoard1.Card.Items.Add(comBoard1.conObjs[i]);
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
            catch (Exception ex)
            {
                Growl.WarningGlobal("错误:" + ex.Message);
            }
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