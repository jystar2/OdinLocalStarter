using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCCaptureInjector32
{

    public class HelperSocketHelper
    {


        public static void Logd(string log)
        {
            Console.Write(log + "\n");
           // Form1.appendSysLog(log);
        }

        static List<string> _ListClientIP = new List<string>();


        public delegate void OnReceiveMsgHandler(string ip);

        private static string g_clientIP;
        private static List<string> ListClientIP
        {
            get
            {
                if (_ListClientIP == null)
                    _ListClientIP = new List<string>();
                return _ListClientIP;
            }
            set
            {
                _ListClientIP = value;
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="msg"></param>
        public static string SendMsg(string msg)
        {

            //发送的格式,前8位为长度
            string kmsg = string.Format("{0:D8}", msg.Length) + msg;  //		kmsg	"00000011{\"type\":-1}"	string


            if (ListClientIP.Count > 0 && _sm != null)
            {
                foreach (var item in ListClientIP)
                {
                    try
                    {
                        _sm.SendMsg(msg, item);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return "";
        }

        public static HelperSocketManager _sm = null;
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ipPort">127.0.0.1:57321 (客户端IP:端口)</param>
        public static void SendMsg(string msg, string ipPort)
        {
            if (_sm != null)
                _sm.SendMsg(msg, ipPort);
        }


        public static string SendAndRecv(string ipPort, string msg)
        {
            try
            {
                if (_sm != null)
                {

                    string kmsg = string.Format("{0:D8}", msg.Length) + msg;  //		kmsg	"00000011{\"type\":-1}"	string


                    // _sm._listSocketInfo[ipPort].stopWaitHandle.Reset();
                    // _sm._listSocketInfo[ipPort].RecordMsg.Clear();
                    return _sm.SendMsg(kmsg, ipPort);
                    //_sm._listSocketInfo[ipPort].stopWaitHandle.WaitOne(10000);

                    // return _sm._listSocketInfo[ipPort].RecordMsg.ToString();
                }



            }
            catch (Exception E)
            {
                //MessageBox.Show("SendAndRecv = " + E.ToString());
            }


            return "";
        }


        public static void StartServer(string ip, int port)
        {

            //Logd("StartServer xxxxx ");

            _sm = new HelperSocketManager(ip, port);
            _sm.OnReceiveMsg += OnReceiveMsg;
            _sm.OnConnected += OnConnected;
            _sm.OnDisConnected += OnDisConnected;
            _sm.Start();
        }


        public static void StopServer()
        {

            //Logd("StartServer xxxxx ");

            if (_sm != null)
            {
                _sm.Stop();
            }

        }

        public static void DisconnectPort(string endPoint)
        {
            _sm.DisconnectPort(endPoint);
        }

        private delegate void MyDelegate();

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="ip">客户端的IP</param>
        public static void OnReceiveMsg(string ip)
        {


            try
            {
                byte[] buffer = _sm._listSocketInfo[ip].msgBuffer;
                // _sm._listSocketInfo[ip].isWaitRecv = false;
                _sm._listSocketInfo[ip].stopWaitHandle.Set();
                //byte[] buffer = _sm.g_SocketInfoclient.msgBuffer;

                //string msg = Encoding.ASCII.GetString(buffer).Replace("\0", "");

                string msg = Encoding.UTF8.GetString(buffer);

                _sm._listSocketInfo[ip].RecordMsg.Append(msg);

                Logd("Server OnReceiveMsg = " + msg);

            }
            catch
            {

            }






            //Form1.OnReceiveMsgEX(msg);

            //_sm.OnReceiveMsgExternal(msg);

            //SendMsg("kk Server OnReceiveMsg" + msg);


        }
        public static int CLNG(String TmpStr)
        {
            int CVAL = 0;
            int.TryParse(TmpStr, out CVAL);
            return CVAL;
        }

        /// <summary>
        /// 客户端机子连接到本机
        /// </summary>
        /// <param name="clientIP">客户端的IP</param>
        /// 
        public static string[] Split(String TmpStr, String deli)
        {
            string[] Result = null;

            if (TmpStr != null)
            {
                Result = TmpStr.Split(new String[] { deli }, StringSplitOptions.None);
            }


            return Result;
        }

        public static string GetRandomFilename(int len)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            string[] consonants = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "o", "n", "p", "q", "r", "s", "sh", "zh", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += consonants[r.Next(consonants.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += consonants[r.Next(consonants.Length)];
                b++;
            }

            return Name;
        }
        public static void OnConnected(string clientIP)
        {


            if (!_ListClientIP.Contains(clientIP))
            {
                Logd("加入連接3 :   " + clientIP);
                ListClientIP.Add(clientIP);
            }
            else
            {
                Logd("已加入連接 ");
            }



            string ipstr = Split(clientIP, ":")[0];
            string portstr = Split(clientIP, ":")[1];

            // Hook.Logd("Server OnConnected 444 " + clientIP);

            HelperSocketHelper.g_clientIP = clientIP;


            int Pid = 0;
            int hwnd = 0;
            String Version = "";
            string BasicInfoStr = SendAndRecv(clientIP, "{\"type\": 88}");
            JObject BasicInfo = Cjson.Decode(BasicInfoStr);



            if (BasicInfo != null)
            {



                // String Rst = Cjson.GetJsonStr(Data, "rst");
                // GamePid  = Rst.Split(':')[0];

                Pid = Cjson.GetJsonInt(BasicInfo, "pid");
                hwnd = Cjson.GetJsonInt(BasicInfo, "hwnd");
                Version = Cjson.GetJsonStr(BasicInfo, "Version");

                MainWindow.PurpleSocket.Bindedhwnd = hwnd;
                MainWindow.PurpleSocket.BindedPID = Pid;
                MainWindow.PurpleSocket.Version = Version;
                MainWindow.PurpleSocket.IPPort = clientIP;
                try
                {
                    if (MainWindow.PurpleSocket.Version != "1.0.0" && MainWindow.PurpleSocket.Version != "")
                    {
                        Logd("Purple版本錯誤,中止");
                        BasicInfoStr = SendAndRecv(clientIP, "{\"type\": 99}");

                        MainWindow.RecordPurpleSocket.DLLName = GetRandomFilename(12);
                        System.IO.File.WriteAllText(MainWindow.GetExePath() + "\\Purple\\RecordPurpleInfo.ini", Cjson.Encode(MainWindow.RecordPurpleSocket));

                    }
                }
                catch
                {
                }
                MainWindow.PurpleSocket.IsBind = true;


                try
                {
                    if (File.Exists(MainWindow.GetExePath() + "\\Purple\\LineageWInjectExe.exe"))
                    {
                        File.Delete(MainWindow.GetExePath() + "\\Purple\\LineageWInjectExe.exe");
                    }
                }
                catch
                {
                }
                try
                {
                    if (File.Exists(MainWindow.GetExePath() + "\\Purple\\LineageWInjectExe.exe.config"))
                    {
                        File.Delete(MainWindow.GetExePath() + "\\Purple\\LineageWInjectExe.exe.config");
                    }
                }
                catch
                {
                }
                try
                {
                    if (File.Exists(MainWindow.GetExePath() + "\\Purple\\TestCLR.dll"))
                    {
                        File.Delete(MainWindow.GetExePath() + "\\Purple\\TestCLR.dll");
                    }
                }
                catch
                {
                }


                // MessageBox.Show("GamePid=" + GamePid.ToString());
            }

        
        }
        /// <summary>
        /// 客户端机子断开连接
        /// </summary>
        /// <param name="clientIp">客户端的IP</param>
        public static void OnDisConnected(string clientIp)
        {

            Logd("已斷開連接 " + clientIp);
            HelperSocketHelper.g_clientIP = null;

            if (_ListClientIP.Contains(clientIp))
            {

                ListClientIP.Remove(clientIp);
            }


            //   SendMsg("kk OnDisConnected ");

            //if (txtMsg.InvokeRequired)
            //{
            //    this.Invoke(new MyDelegate(() =>
            //    {
            //        txtMsg.Text += clientIp + "已经断开连接！\r\n";
            //        object obj = new { Value = clientIp, Text = clientIp };
            //        cbClient.Items.Remove(obj);
            //    }));
            //}
            //else
            //{
            //    txtMsg.Text += clientIp + "已经断开连接！\r\n";
            //}
        }


    }
}
