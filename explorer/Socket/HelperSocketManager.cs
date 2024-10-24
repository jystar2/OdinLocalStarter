using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace NCCaptureInjector32
{
    public class HelperSocketManager
    {
        public static void Logd(string log)
        {
            Console.Write(log + "\n");
            MainWindow.appendSysLog(log);

        }

        public Dictionary<string, SocketInfo> _listSocketInfo = null;

        Socket _socket = null;
        EndPoint _endPoint = null;
        bool _isListening = false;
        int BACKLOG = 10;

        public string g_clientIP;

        public SocketInfo g_SocketInfoclient;

        public delegate void OnConnectedHandler(string clientIP);
        public event OnConnectedHandler OnConnected;
        public delegate void OnReceiveMsgHandler(string ip);
        public event OnReceiveMsgHandler OnReceiveMsg;
        public event OnReceiveMsgHandler OnDisConnected;
        //public event OnReceiveMsgHandler OnReceiveMsgExternal;

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();


        [DllImport("kernel32.dll")]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        public HelperSocketManager(string ip, int port)
        {

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            IPAddress _ip = IPAddress.Parse(ip);

            _endPoint = new IPEndPoint(_ip, port);

            _listSocketInfo = new Dictionary<string, SocketInfo>();


        }

        public void Start()
        {
            //Logd("AcceptWork Start 11 ");
            Logd("啟動3號接收Server中");
            try
            {
                _socket.Bind(_endPoint); //绑定端口
                _socket.Listen(BACKLOG); //开启监听
                Thread acceptServer = new Thread(AcceptWork); //开启新线程处理监听
                acceptServer.IsBackground = true;
                _isListening = true;
                acceptServer.Start();

                Logd("啟動3號接收Server完成");
            }
            catch (Exception Ex)
            {


                MainWindow.appendSysLog(Ex.Message.ToString());


                if (MessageBox.Show(_endPoint.ToString() + "端口可能被佔用,請留意是否有其他已開啟的助手!") == MessageBoxResult.OK)
                {
                    IntPtr hproc = GetCurrentProcess();

                    if (GeneralVariable.IsDebug == false)
                    {
                        TerminateProcess(hproc, 0);
                    }

                }
            }

        }

        public void AcceptWork()
        {

            Logd("開始接受IP端口-3 ");
            while (_isListening)
            {
                // Hook.Logd("AcceptWork 22222 ");
                try
                {
                    Socket acceptSocket = _socket.Accept();

                    if (acceptSocket != null && this.OnConnected != null && _isListening == true)
                    {
                        //   Hook.Logd("AcceptWork 55555 ");

                        Logd("接受IP端口-3 =  " + acceptSocket.RemoteEndPoint.ToString());

                        SocketInfo sInfo = new SocketInfo();
                        sInfo.socket = acceptSocket;



                        bool isContains = false;
                        foreach (var item in _listSocketInfo.Keys)
                        {
                            //  Logd("AcceptWork item " + item);
                            if (item == acceptSocket.RemoteEndPoint.ToString())
                            {

                                isContains = true;
                            }
                        }

                        if (isContains == false)
                        {
                            _listSocketInfo.Add(acceptSocket.RemoteEndPoint.ToString(), sInfo);//加到连接的表里面
                        }
                        else
                        {
                            _listSocketInfo[acceptSocket.RemoteEndPoint.ToString()] = sInfo;
                        }

                        sInfo.isConnected = true;


                        g_SocketInfoclient = sInfo; //我这里 直接用一个全局的对象来,只做单开, 如果要多开就用上面加入表

                        OnConnected(acceptSocket.RemoteEndPoint.ToString());

                        //Thread socketConnectedThread = new Thread(newSocketReceive);
                        //socketConnectedThread.IsBackground = true;
                        //socketConnectedThread.Start(acceptSocket);
                    }
                }
                catch (Exception E)
                {
                    MainWindow.appendSysLog(E.ToString());
                }

                Thread.Sleep(200);
                // Hook.Logd("AcceptWork 333333 ");
            }
        }

        public void newSocketReceive(object obj)
        {
            Socket socket = obj as Socket;


            bool isContains = false;
            foreach (var item in _listSocketInfo.Keys)
            {
                Logd("newSocketReceive item " + item);
                if (item == socket.RemoteEndPoint.ToString())
                    isContains = true;
            }

            if (isContains == false)
            {
                return;
            }

            SocketInfo sInfo = _listSocketInfo[socket.RemoteEndPoint.ToString()];

            // SocketInfo sInfo = g_SocketInfoclient;
            sInfo.isConnected = true;
            while (sInfo.isConnected)
            {
                try
                {
                    if (sInfo.socket == null) return;
                    //这里向系统投递一个接收信息的请求，并为其指定ReceiveCallBack做为回调函数 
                    sInfo.socket.BeginReceive(sInfo.buffer, 0, sInfo.buffer.Length, SocketFlags.None, ReceiveCallBack, sInfo.socket.RemoteEndPoint);
                }
                catch
                {
                    return;
                }
                Thread.Sleep(1);
            }
        }

        // 客户端 断开挂在这里
        private void ReceiveCallBack(IAsyncResult ar)
        {
            EndPoint ep = ar.AsyncState as IPEndPoint; //{127.0.0.1:12515}

            int readCount = 0;
            SocketInfo info = null;



            try
            {

                bool isContains = false;
                foreach (var item in _listSocketInfo.Keys)
                {
                    Logd("SendMsg item " + item);
                    if (item == ep.ToString())
                        isContains = true;
                }

                if (isContains == false)
                {
                    return;
                }

                info = _listSocketInfo[ep.ToString()]; //这里会查表找不到 导致奔溃

                //  SocketInfo info = g_SocketInfoclient;

                if (info.socket == null) return;
                readCount = info.socket.EndReceive(ar); //接受到的数据包大小



            }
            catch (Exception)
            {
                return;
            }


            //Hook.Logd("readCount = " + readCount);
            //Hook.Logd("info.buffer.Length = " + info.buffer.Length);


            if (info != null)
            {
                if (readCount > 0)
                {
                    //byte[] buffer = new byte[readCount];
                    //Buffer.BlockCopy(info.buffer, 0, buffer, 0, readCount);
                    if (readCount < info.buffer.Length)
                    {
                        byte[] newBuffer = new byte[readCount];
                        Buffer.BlockCopy(info.buffer, 0, newBuffer, 0, readCount);
                        info.msgBuffer = newBuffer;
                    }
                    else
                    {
                        info.msgBuffer = info.buffer;
                    }

                    string msgTip = Encoding.UTF8.GetString(info.msgBuffer);

                    Logd("msgTip = " + msgTip);

                    if (msgTip == "tcpClose\0")//在发个命令来关闭 msgTip = "tcpClose\0"
                    {
                        Logd("deloe ");
                        info.isConnected = false;

                        if (this.OnDisConnected != null) OnDisConnected(info.socket.RemoteEndPoint.ToString());

                        // _listSocketInfo.Remove(info.socket.RemoteEndPoint.ToString());

                        g_SocketInfoclient = null;

                        info.socket.Close();
                        return;
                    }
                    if (OnReceiveMsg != null) OnReceiveMsg(info.socket.RemoteEndPoint.ToString());
                }
            }



        }



        private void ReceiveCallBackByWait(IAsyncResult ar)
        {
            EndPoint ep = ar.AsyncState as IPEndPoint;

            //


            bool isContains = false;
            foreach (var item in _listSocketInfo.Keys)
            {
                Logd("SendMsg item " + item);
                if (item == ep.ToString())
                    isContains = true;
            }

            if (isContains == false)
            {
                return;
            }
            SocketInfo info = _listSocketInfo[ep.ToString()]; //这里会查表找不到 导致奔溃 //上面已檢查

            // SocketInfo info = g_SocketInfoclient;

            int readCount = 0;
            try
            {
                if (info.socket == null) return;
                readCount = info.socket.EndReceive(ar); //接受到的数据包大小
            }
            catch (Exception)
            {
                return;
            }

            if (readCount > 0)
            {
                //byte[] buffer = new byte[readCount];
                //Buffer.BlockCopy(info.buffer, 0, buffer, 0, readCount);
                if (readCount < info.buffer.Length)
                {
                    byte[] newBuffer = new byte[readCount];
                    Buffer.BlockCopy(info.buffer, 0, newBuffer, 0, readCount);
                    info.msgBuffer = newBuffer;
                }
                else
                {
                    info.msgBuffer = info.buffer;
                }

                string msgTip = Encoding.UTF8.GetString(info.msgBuffer);

                Logd("msgTip = " + msgTip);

                if (msgTip == "tcpClose\0")//在发个命令来关闭 msgTip = "tcpClose\0"
                {
                    Logd("deloe ");
                    info.isConnected = false;

                    if (this.OnDisConnected != null) OnDisConnected(info.socket.RemoteEndPoint.ToString());

                    _listSocketInfo.Remove(info.socket.RemoteEndPoint.ToString());

                    //g_SocketInfoclient = null;

                    info.socket.Close();
                    return;
                }
                if (OnReceiveMsg != null) OnReceiveMsg(info.socket.RemoteEndPoint.ToString());
            }
        }


        public static bool IsConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }


        public void DisconnectPort(string endPoint)
        {
            bool isContains = false;
            foreach (var item in _listSocketInfo.Keys)
            {
                // Logd("endPoint = " + endPoint);
                //  Logd("SendMsg item " + item);
                if (item == endPoint)
                {
                    isContains = true;

                }
            }

            if (isContains && _listSocketInfo[endPoint] != null)
            {
                _listSocketInfo[endPoint].isConnected = false;

                try
                {
                    _listSocketInfo[endPoint].socket.Close();
                }
                catch
                {

                }
                try
                {
                    OnDisConnected(endPoint);
                }
                catch
                {

                }
                //  _listSocketInfo[endPoint];
                _listSocketInfo.Remove(endPoint);
            }

        }
        public long TickCount()
        {
            return Environment.TickCount & Int32.MaxValue;
        }
        public string SendMsg(string text, string endPoint)
        {


            long StartTime = TickCount();

            bool isContains = false;
            foreach (var item in _listSocketInfo.Keys)
            {
                // Logd("endPoint = " + endPoint);
                //  Logd("SendMsg item " + item);
                if (item == endPoint)
                {
                    isContains = true;

                }
            }


            var list = _listSocketInfo.Keys;
            if (isContains && _listSocketInfo[endPoint] != null)
            {
                if (_listSocketInfo[endPoint].isConnected == false)
                {
                    return "";
                }

                if (IsConnected(_listSocketInfo[endPoint].socket) == false || (!_listSocketInfo[endPoint].socket.Connected && _listSocketInfo[endPoint].socket.Available == 0))
                {

                    _listSocketInfo[endPoint].isConnected = false;

                    // MessageBox.Show("Disconnect");
                    Logd("RemoteEndPoint:" + _listSocketInfo[endPoint].socket.RemoteEndPoint.ToString() + " DisConnected!");
                    // if (this.OnDisConnected != null) OnDisConnected(_listSocketInfo[endPoint].socket.RemoteEndPoint.ToString());

                    try
                    {
                        _listSocketInfo[endPoint].socket.Close();
                    }
                    catch
                    {

                    }
                    try
                    {
                        OnDisConnected(endPoint);
                    }
                    catch
                    {

                    }
                    //  _listSocketInfo[endPoint];
                    _listSocketInfo.Remove(endPoint);
                    return "";
                }


                _listSocketInfo[endPoint].socket.SendTimeout = 10000;
                _listSocketInfo[endPoint].socket.ReceiveTimeout = 10000;

                StartTime = TickCount();
                //Logd("text = " + text);
                if (_listSocketInfo[endPoint].isSendAndRecv == true)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        if (_listSocketInfo[endPoint].isSendAndRecv == true)
                        {
                            Thread.Sleep(100);
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                _listSocketInfo[endPoint].isSendAndRecv = true;
                _listSocketInfo[endPoint].socket.Send(Encoding.UTF8.GetBytes(text));

                //}


                // if (g_SocketInfoclient != null)
                // {
                //     g_SocketInfoclient.socket.Send(Encoding.UTF8.GetBytes(text));
                // }

                // Logd("buffersize = " + text);




                byte[] buffersize = new byte[8];

                var receivedsize = _listSocketInfo[endPoint].socket.Receive(buffersize);


                string datasize = Encoding.UTF8.GetString(buffersize, 0, receivedsize);
                string data = "";

                byte[] RecvBuff = new byte[0];


                //MessageBox.Show(RecvBuff.Length.ToString());

                int RecvLen = 0;
                //  Logd("datasize = " + datasize); //datasize = 00000177 包的 大小 固定长度 8

                if (CLNG(datasize) > 0)
                {
                    for (int i = 0; i < CLNG(datasize); i++)
                    {
                        if (RecvLen > 0)
                        {
                            data = Encoding.UTF8.GetString(RecvBuff, 0, RecvLen);
                        }

                        if (RecvLen < CLNG(datasize))
                        {
                            byte[] buffer = new byte[8192];
                            int received = _listSocketInfo[endPoint].socket.Receive(buffer);
                            RecvLen = RecvLen + received;

                            RecvBuff = CombineArray(RecvBuff, buffer, received);
                            //  Logd("received = " + received.ToString());
                            //  Logd("Real received = " + Encoding.UTF8.GetString(RecvBuff, 0, received));

                            //MessageBox.Show("received=" + received.ToString());

                        }
                        else
                        {

                            break;
                        }

                    }
                }


                if (GeneralVariable.IsCountRecordTime == true)
                {
                    Logd("TickCount():" + (TickCount() - StartTime).ToString() + "ms");
                }

                // Logd("data = " + data); //datasize = 00000177 包的 大小 固定长度 8
                _listSocketInfo[endPoint].isSendAndRecv = false;

                return data;




                //newSocketReceive(g_SocketInfoclient.socket);
            }

            return "";
        }


        public static byte[] CombineArray(byte[] first, byte[] second, int Length)
        {

            byte[] ret = new byte[first.Length + Length];
            if (first.Length > 0)
            {
                Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            }
            if (Length > 0)
            {
                Buffer.BlockCopy(second, 0, ret, first.Length, Length);
            }
            return ret;
        }


        public int CLNG(String InputText)
        {
            int x = 0;

            if (Int32.TryParse(InputText, out x))
            {
                // you know that the parsing attempt
                // was successful
            }
            return x;
        }

        public void Stop()
        {
            _isListening = false;
            foreach (SocketInfo s in _listSocketInfo.Values)
            {
                try
                {
                    s.socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {

                }

                try
                {
                    s.socket.Close(0);
                }
                catch
                {

                }
            }
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {

            }
            try
            {
                _socket.Close(0);
            }
            catch
            {

            }
        }

        public class SocketInfo
        {
            public Socket socket = null;
            public byte[] buffer = null;
            public byte[] msgBuffer = null;
            public bool isConnected = false;
            public bool isWaitRecv = false;
            public bool isSendAndRecv = false;
            public StringBuilder RecordMsg = new StringBuilder();
            public AutoResetEvent stopWaitHandle = new AutoResetEvent(false);

            public SocketInfo()
            {
                buffer = new byte[1024 * 4];
            }
        }
    }
}
