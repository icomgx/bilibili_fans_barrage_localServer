using bilibiliFansBarrage.Properties;
using MetroFramework.Forms;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace bilibiliFansBarrage
{
    public partial class Main : MetroForm
    {
        public Main()
        {
            InitializeComponent();
        }

        Thread myThread;
        public delegate void MyDelegateUI(); //定义委托类型
        MyDelegateUI myDelegateUI; //声明委托对象
        private HttpListener m_http_listener = new HttpListener();
        private Thread m_http_thread = null;
        Task MyTask = null;
        static string url = "http://127.0.0.1:2334/";
        static string usbWebServerPath=Application.StartupPath+ "\\USBWebserver\\usbwebserver.exe";
        static long FansCounts = 0;
        static long FansP = 0;
        static string uid = "";
        private readonly object utils;

        private static void start()
        {
            string mid, mtime, uname, face;
            mid = mtime = uname = face = " ";
            string bilijson = Utils.HttpGet("https://api.bilibili.com/x/relation/followers?vmid="+uid);

            JObject jo = JObject.Parse(bilijson);

            if (jo["message"].ToString() != "0")
            {
                Console.WriteLine("Data error");


            }
            Console.WriteLine("Message=0");
            JArray jar = JArray.Parse(((JObject)jo["data"])["list"].ToString());
            for (var i = 0; i < jar.Count; i++)
            {
                JObject j = JObject.Parse(jar[i].ToString());
                mid = j["mid"].ToString();
                mtime = j["mtime"].ToString();
                uname = j["uname"].ToString();
                face = j["face"].ToString();
                //Console.WriteLine(mid + " " + mtime + " " + uname + " " + face);
                if (DBHelper.CheckingBC_Mid(mid) == "0")
                {
                    DBHelper.NewFansToDatabase(mid, mtime, uname, face, "0");
                    Console.WriteLine(mid + " " + mtime + " " + uname + " " + face);
                    Console.WriteLine("新粉丝数据添加到数据库成功...");
                }
                else if (mtime != DBHelper.TakeoutBCBus_tamp(mid))
                {
                    DBHelper.FansUpdateToDatabase(mid, mtime, uname, face);
                    Console.WriteLine(mid + " " + mtime + " " + uname + " " + face);
                    Console.WriteLine("老粉更新完毕...");
                }
                else
                {
                    //Console.WriteLine("重复数据,已丢弃~");
                }
            }
        }

        void doWork()
        {
            start();
            while (true)
            {
                FansP = long.Parse(GetFansCounts(ami_TextBox_uid.Text));
                if (FansCounts < FansP)
                {
                    AddFansLogText(" 当前粉丝总数: " + FansCounts + "   启动拉取...");
                    start();
                    AddFansLogText(" 粉丝数据拉取完毕...");
                    FansCounts = FansP;
                }
                else
                {
                    FansCounts = FansP;
                    AddFansLogText("当前粉丝总数: " + FansCounts + "   无需拉取数据...");
                }
                Thread.Sleep(800);
            }
        }


        protected virtual void QueryHttpServiceThd()
        {
            while (true)
            {
                try
                {
                    HttpListenerContext context = m_http_listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    AddFansLogText(context.Request.Url.ToString());
                    switch (context.Request.QueryString["Mode"])
                    {
                        default:
                            response.StatusCode = 200;
                            response.AddHeader("Access-Control-Allow-Origin", "*");
                            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8))
                            {
                                writer.Write("请通过Get参数方式访问此端口");
                            }
                            break;
                        case "1":
                            response.StatusCode = 200;
                            response.AddHeader("Access-Control-Allow-Origin", "*");
                            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8))
                            {
                                writer.Write(Utils.ResponseA2(DBHelper.TakeoutBC_uname() + " " + StarHubSelector(Convert.ToInt32(DBHelper.TakeoutBC_Flag())), DBHelper.TakeoutBC_face()));
                                DBHelper.TakeoutBC_RemoveTemp();
                            }
                            break;
                        case "2":
                            response.StatusCode = 200;
                            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8))
                            {
                                writer.Write(Utils.ResponseA3(FansP.ToString(), DBHelper.TakeoutBC_uname(), DBHelper.TakeoutBC_face(), DBHelper.TakeoutBC_Flag()));
                            }
                            break;
                    }
                    
                }
                catch (Exception ex)
                {
                    AddApiLogText("[异常] " + ex.Message);
                }
            }
        }

        public bool QueryStart(Task MyTask, string url)
        {
            this.MyTask = MyTask;
            if (!HttpListener.IsSupported)
            {

                return false;
            }
            if (m_http_thread != null)
                return true;

            string http_url = url;
            try
            {
                m_http_listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                m_http_listener.Prefixes.Add(http_url);
                m_http_listener.Start();

                m_http_thread = new Thread(QueryHttpServiceThd);
                m_http_thread.Name = "HttpServiceThd";
                m_http_thread.IsBackground = true;
                m_http_thread.Start();

            }
            catch (Exception ex)
            {
                AddApiLogText("[异常] " + ex.Message);
            }

            return false;
        }







        public void AddFansLogText(string logMsg)
        {
            this.ami_TextBox_Fanslog.AppendText(DateTime.Now.ToString() + Environment.NewLine + logMsg + Environment.NewLine);
        }
        public void AddApiLogText(string logMsg)
        {
            this.txt_apiLog.AppendText(DateTime.Now.ToString() + Environment.NewLine + logMsg + Environment.NewLine);
        }

        private static string GetFansCounts(string uid)
        {
            string bilijson = Utils.HttpGet("https://api.bilibili.com/x/relation/followers?vmid=" + uid);

            JObject jo = JObject.Parse(bilijson);

            return ((JObject)jo["data"])["total"].ToString();
        }





        private void Main_Load(object sender, EventArgs e)
        {
            
            Splash.Show(typeof(MessgeEX.Loading));
            Thread.Sleep(1500);
            ami_TextBox_uid.Text = Utils.LoadUid();
            Splash.Close();
            AddFansLogText("系统启动~");
            TextBox.CheckForIllegalCrossThreadCalls = false;

        }

        private void ami_Button_NewUser_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.showdoc.cc/p/b55908a5a182250c8296616ee6634095");
        }

        private void ami_Button_start_Click(object sender, EventArgs e)
        {
            try
            {
                Settings.Default.uid = ami_TextBox_uid.Text;
                Settings.Default.Save();
                uid = ami_TextBox_uid.Text;
                AddFansLogText("uid保存成功");
                FansCounts = long.Parse(GetFansCounts(ami_TextBox_uid.Text));
                FansP = 0;
                AddFansLogText("连接bilibili服务器成功,当前粉丝数" + FansCounts);
                FansP = FansCounts;

                myThread = new Thread(doWork);
                myDelegateUI = new MyDelegateUI(start);//绑定委托

                myThread.Start();
                QueryStart(MyTask, url);
                
                   Process.Start(usbWebServerPath);
                //Process.Start(usbWebServerPath);


                AddApiLogText("初始化完成,请查看左侧地址");
                ami_TextBox_FansBarrageUrl.Text = "http://127.0.0.1:2333/";
                ami_TextBox_FansBarrageApi.Text = "http://127.0.0.1:2334/";
                ami_Button_start.Enabled = false;

            }
            catch (Exception ex)
            {
                AddApiLogText("[异常] " + ex.Message);
            }



        }
                
           
        private void ami_Button_exit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("记得退出托盘中的USBWebServer~", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Environment.Exit(0);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("记得退出托盘中的USBWebServer~", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Environment.Exit(0);
        }

        private string Star1Hub()
        {
            string[] sHub = new string[] { "点了关注", "加入了粉丝阵营", "成为了你的粉丝", "加入了帮派", "上了UP的贼船" };
            int cHubCount = sHub.Length;
            Random random = new Random();
            return sHub[random.Next(0, cHubCount)].ToString();
        }

        private string Star2Hub()
        {
            string[] sHub = new string[] { "又点了关注", "又加入了粉丝阵营", "又成为了你的粉丝", "又加入了帮派", "又上了UP的贼船" };
            int cHubCount = sHub.Length;
            Random random = new Random();
            return sHub[random.Next(0, cHubCount)].ToString();
        }

        private string Star3Hub()
        {
            string[] sHub = new string[] { "又又点了关注", "又又加入了粉丝阵营", "又又成为了你的粉丝", "又又加入了帮派", "又又上了UP的贼船" };
            int cHubCount = sHub.Length;
            Random random = new Random();
            return sHub[random.Next(0, cHubCount)].ToString();
        }

        private string Star4Hub()
        {
            string[] sHub = new string[] { "又又又点了关注", "又又又加入了粉丝阵营", "又又又成为了你的粉丝", "又又又加入了帮派", "又又又上了UP的贼船" };
            int cHubCount = sHub.Length;
            Random random = new Random();
            return sHub[random.Next(0, cHubCount)].ToString();
        }

        private string Star5Hub()
        {
            string[] sHub = new string[] { "又双叒叕点了关注", "又双叒叕加入了粉丝阵营", "又双叒叕成为了你的粉丝", "又双叒叕加入了帮派", "又双叒叕上了UP的贼船" };
            int cHubCount = sHub.Length;
            Random random = new Random();
            return sHub[random.Next(0, cHubCount)].ToString();
        }

        private string Star6Hub()
        {
            string[] sHub = new string[] { "又点了关注,反复横跳你最闪耀 666", "又加入了粉丝阵营,反复横跳你最闪耀 666", "又成为了你的粉丝,反复横跳你最闪耀 666", "又加入了帮派,反复横跳你最闪耀 666", "又上了UP的贼船,反复横跳你最闪耀 666" };
            int cHubCount = sHub.Length;
            Random random = new Random();
            return sHub[random.Next(0, cHubCount)].ToString();
        }

        private string Star7UpHub(string flag)
        {
            string sHub = "又点了关注，关注Combe X " + flag + "~";
            return sHub;
        }

        private string StarHubSelector(int flag)
        {
            switch (flag)
            {
                default:
                    return Star7UpHub(flag.ToString());

                case 0:
                    return Star1Hub();

                case 1:
                    return Star2Hub();

                case 2:
                    return Star3Hub();

                case 3:
                    return Star4Hub();

                case 4:
                    return Star5Hub();

                case 5:
                    return Star6Hub();

            }
        }

    }
}
