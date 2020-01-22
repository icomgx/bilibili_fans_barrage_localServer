using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace bilibiliFansBarrage
{
    internal class ErrorUpload
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


        public static void UploadError(string Error)
        {
            try
            {
                HttpGet("http://ft2.club:1088/e=" + System.Web.HttpUtility.UrlEncode(Error));
            }
            catch (Exception) { }
        }
        public static void AskUploadError(string Error)
        {
            if (MessageBox.Show(Error + "\n\n选择'确定'上报错误给开发者！程序将不会上传您的任何个人信息。", "数据库连接时发生错误：", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                UploadError(Error);
            }
        }
    }
}