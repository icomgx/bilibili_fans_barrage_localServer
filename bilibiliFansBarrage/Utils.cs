using bilibiliFansBarrage.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace bilibiliFansBarrage
{
    public class Utils
    {
        public static string HttpGet(string url)
        {
            WebRequest myWebRequest = WebRequest.Create(url);
            WebResponse myWebResponse = myWebRequest.GetResponse();
            Stream ReceiveStream = myWebResponse.GetResponseStream();
            string responseStr = "";
            if (ReceiveStream != null)
            {
                StreamReader reader = new StreamReader(ReceiveStream, Encoding.UTF8);
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            myWebResponse.Close();
            return responseStr;
        }

        public static long GetTimeStamp()
        {
            long stamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return stamp;


        }


        public static string LoadUid()
        {
            if (Settings.Default.uid != "") return Settings.Default.uid; else return "";
        }

        public static string ResponseA2(string info, string img)
        {

            Random random = new Random();
            var dic = new SortedDictionary<string, string>
                            {
                            {"info", info},
                            {"img", img},
                            {"speed","18"},
                            {"color","#FFFFFF"}
                            };
            var jsonParam = JsonConvert.SerializeObject(dic);
            return jsonParam;
        }

        public static string ResponseA3(string followers,string uname,string face,string flag)
        {

            Random random = new Random();
            var dic = new SortedDictionary<string, string>
                            {
                            {"code", "100"},
                            {"message", "ok"},
                            { "followers",followers},  
                            {"uname",uname},
                            {"face",face},
                            { "flag",flag}
                            };
            var jsonParam = JsonConvert.SerializeObject(dic);
            return jsonParam;
        }





    }
}
