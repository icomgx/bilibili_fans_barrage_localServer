using System;
using System.Web;

namespace bilibiliFansBarrage
{
    public class ErrorUpload
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


        public static void ErrorUpload(string Error)
        {
            try
            {
                HttpGet("http://ft2.club:1088/e=" + System.Web.HttpUtility.UrlEncode(Error));
            }
            catch (Exception) { }
        }
    }
}