using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NCCaptureInjector32
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AutoUpdatePage : Window
    {


        public class UpdateItem
        {
            public string version { get; set; }
            public string date { get; set; }
            public string link { get; set; }
            public string profile { get; set; }
            public int no_skip { get; set; }
            public long edit_time { get; set; }
            public string version_id { get; set; }
            public string apk_name { get; set; }
            public string md5_file { get; set; }

        }

        public class GeneralRecv
        {
            public UpdateItem data;
            public int code;
            public int status;
        }

        GeneralRecv UpdateInfo = new GeneralRecv();
        public Boolean IsStoped = false;
        public Boolean IsDownloadFinished = false;
        public Boolean IsDownloadFileFinished = false;

        public String DownloadDataStr = "";
        public Boolean DownLoadSuccess = false;
        public String TargetFileMD5 = "";


        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();


        [DllImport("kernel32.dll")]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);


        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);


        public String BaseFilePath = "";
        public bool IsFirstTime = true;
        public bool IsVerifyed = false;
        public AutoUpdatePage()
        {
            InitializeComponent();
            
        }

        public String Printf(String InputStr, String a)
        {
            return InputStr.Replace("%s", a);
            // return String.Format(InputStr, a);
        }

        public String Printf(String InputStr, String a, String b)
        {
            return InputStr.Replace("%1$s", a).Replace("%2$s", b);
            //return String.Format(InputStr, a, b);
        }

        public string GetExePath()
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //C:\Program Files\MyApplication
            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            return strWorkPath;
        }
        public static int CLNG(String TmpStr)
        {
            int CVAL = 0;
            int.TryParse(TmpStr, out CVAL);
            return CVAL;
        }
        public void Delay(int Millisecond)
        {
            for (int i = 0; i < Convert.ToInt32(Math.Floor((double)(Millisecond / 10))); i++)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(10);
            }
        }
        public class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                //MessageBox.Show("SetTimeOUT");
                WebRequest WR = base.GetWebRequest(uri);
                WR.Timeout = 10 * 1000;
                return WR;
            }
        }

        public void DownloadDataWebStr(String Url)
        {
            DownloadDataStr = "";
            byte[] data;
            try
            {
                MyWebClient client = new MyWebClient();

                data = client.DownloadData(Url);  //client.DownloadData(Url);
                DownloadDataStr = Encoding.UTF8.GetString(data);
            }
            catch
            {

            }
            IsDownloadFinished = true;

        }

        public Boolean CheckAutoUpdate(String Url, String NowVersion)
        {

            //WebClient client = new WebClient();
            //json转为UpdateItem类对象

            try
            {

                IsDownloadFinished = false;
                Thread DownloadThread = new Thread(() => DownloadDataWebStr(Url));
                DownloadThread.Start();
                for (int i = 0; i < 120; i++)
                {
                    Delay(500);
                    if (IsDownloadFinished == true)
                    {
                        break;
                    }
                }

                //MessageBox.Show(DownloadDataStr);

                UpdateInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<GeneralRecv>(DownloadDataStr);



                string fileName = GetExePath() + "\\AutoUpdate\\";
                UsefulFunction.CheckAndCreateDir(fileName);

                if (UpdateInfo != null)
                {
                    if (UpdateInfo.data != null)
                    {
                        Boolean NeedUpdate = true;
                        Boolean IsUpdate = false;
                        GeneralVariable.NewVersion = UpdateInfo.data.version;

                        // MessageBox.Show(UpdateInfo.data.version.ToString());
                        if (CLNG(NowVersion) >= CLNG(UpdateInfo.data.version))
                        {

                            NeedUpdate = false;
                        }


                        //   MessageBox.Show(fileName + UpdateInfo.data.version.ToString() + ".zip");
                        /*
                        if (File.Exists(fileName + UpdateInfo.data.version + ".zip"))
                        {
                            if (CalculateFileMD5(fileName + UpdateInfo.data.version + ".zip") == UpdateInfo.data.md5_file)
                            {
                                NeedUpdate = false;
                            }
                        }
                        */

                        // MessageBox.Show(NeedUpdate.ToString());

                        if (NeedUpdate == true)
                        {
                            GeneralVariable.NeedUpdate = true;
                            //  MessageBox.Show("NeedUpdate");


                            String Content = "";//"找到更新: (版本號:" + UpdateInfo.data.version + ")" + "\r\n\r\n";

                            Content = Content + "更新內容:" + ":" + "\r\n";

                            UpdateInfo.data.profile = UpdateInfo.data.profile.Replace("\r\n", "\r");
                            UpdateInfo.data.profile = UpdateInfo.data.profile.Replace("\r", "\n");
                            UpdateInfo.data.profile = UpdateInfo.data.profile.Replace("\n", "\r\n");

                            Content = Content + UpdateInfo.data.profile + "\r\n";

                            // this.LabelHeader.Text = Printf(R.String.dialog_update_found, UpdateInfo.data.version);
                            // this.textBoxContent.Text = UpdateInfo.data.profile;

                            String textBoxContent = "發現新版本:" + UpdateInfo.data.version + "\r\n" + "\r\n";
                            textBoxContent = textBoxContent + UpdateInfo.data.profile + "\r\n" + "\r\n";
                            textBoxContent = textBoxContent + "馬上進行更新?" + "\r\n" + "\r\n" + "注意：更新時會自動關閉所有Hit2遊戲 及 特工";



                            if (UpdateInfo.data.no_skip == 1)
                            {
                                IsUpdate = true;
                            }
                            else
                            {
                                //DialogResult dialogResult = MessageBox.Show(UpdateInfo.data.profile, "找到更新: (版本號:" + UpdateInfo.data.version + ")  是否更新?", MessageBoxButtons.YesNo);

                                if (System.Windows.Forms.MessageBox.Show(textBoxContent, "Hit2特工", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    IsUpdate = true;
                                }
                            }




                            // DialogResult dialogResult = MessageBox.Show("Sure", "找到更新: (版本號:" + UpdateInfo.data.version.ToString() + ")  是否更新?", MessageBoxButtons.YesNo);
                            if (IsUpdate == true)
                            {
                                TargetFileMD5 = UpdateInfo.data.md5_file;

                                //panel1.Visible = false;
                                // panel2.Visible = true;
                                UpdateText.Text = "更新內容: (" + UpdateInfo.data.version + ")" + "\r\n\r\n" + UpdateInfo.data.profile;


                                IsStoped = false;
                                //  panel2.Visible = false;
                                //  panel1.Visible = true;
                                DownloadAndUnzip(UpdateInfo.data.link, UpdateInfo.data.version + ".zip");

                                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                                //this.CenterToScreen();
                                this.Topmost = true;
                                this.Show();
                                this.Topmost = false;


                                return true;

                                //do something
                            }
                            else if (IsUpdate == false)
                            {
                                return false;
                                //do something else
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                      //  Form1.appendSysLog(R.String.獲取最新版本錯誤Data回傳為空白);
                    }
                }
            }
            catch (Exception e)
            {
              //  Form1.appendSysLog(R.String.獲取最新版本錯誤 + "\n" + e.ToString());
            }

            return false;
        }
        public long GetFileSize(string url)
        {
            long result = 0;

            WebRequest req = WebRequest.Create(url);
            req.Method = "HEAD";
            using (WebResponse resp = req.GetResponse())
            {
                if (long.TryParse(resp.Headers.Get("Content-Length"), out long contentLength))
                {
                    result = contentLength;
                }
            }

            return result;
        }

        public void SavebatAndRestart()
        {


            String MyFileName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //MessageBox.Show("MyFileName1=" + MyFileName);

            String CmdLine = "";

            CmdLine = CmdLine + "TIMEOUT 2" + "\n";

            CmdLine = CmdLine + "set NowPath=%cd%" + "\n";


            CmdLine = CmdLine + "taskkill /f /PID " + Process.GetCurrentProcess().Id + "\n";


            try
            {
                Process[] localByName = Process.GetProcessesByName(GeneralVariable.GameNameExe.Replace(".exe", ""));
                foreach (Process Tmpprocess in localByName)
                {
                    CmdLine = CmdLine + "taskkill /f /PID " + Tmpprocess.Id + "\n";
                }
            }
            catch
            {

            }

            CmdLine = CmdLine + "TIMEOUT 1" + "\n";

            string dir = GetExePath();
            string extractPath = System.IO.Path.Combine(dir, "NewVersion");

            MyFileName = GeneralVariable.MyExeName;
            try
            {
                string path = GetExePath() + "\\NewVersion\\";
                string[] dirs = Directory.GetFiles(path, "*.exe");

                //MessageBox.Show(path);

                foreach (string TmpName in GeneralVariable.AutoUpdateOpenExeNameList)
                {
                    foreach (string dir2 in dirs)
                    {
                        //if ((System.IO.Path.GetFileName(dir2).Contains("LINE") || System.IO.Path.GetFileName(dir2).Contains("Helper") || System.IO.Path.GetFileName(dir2).Contains("ntoskrnl") || System.IO.Path.GetFileName(dir2).Contains("PurpleBox") || System.IO.Path.GetFileName(dir2).Contains("NCOverlayCefweb32")) && System.IO.Path.GetFileName(dir2).Contains(".exe"))
                        if (System.IO.Path.GetFileName(dir2).Contains(TmpName) && System.IO.Path.GetFileName(dir2).Contains(".exe"))
                        {
                            MyFileName = System.IO.Path.GetFileName(dir2);
                        }
                    }
                }
            }
            catch
            {

            }


            //MessageBox.Show("MyFileName=" + MyFileName);

            if (Directory.Exists(extractPath))
            {
                CmdLine = CmdLine + "XCOPY /E /Y \"%NowPath%\\NewVersion\"" + " " + "\"%NowPath%\"" + "\n";
                //CmdLine = CmdLine + "pause"+"\n";
                CmdLine = CmdLine + "RD /S/Q \"%NowPath%\\NewVersion\\\"" + "\n";

            }

            CmdLine = CmdLine + "start " + MyFileName + "";//\"%NowPath%\\ MyFileName;


            System.IO.File.WriteAllText(GetExePath() + "\\RestartBat.bat", CmdLine);

            try
            {
                int a = ShellExecute(IntPtr.Zero, "open", GetExePath() + "\\RestartBat.bat", "", GetExePath(), 1);

            }
            catch (Exception E)
            {
                System.Windows.MessageBox.Show("錯誤運行X錯誤:" + E.ToString());
                return;
            }


        }

        private async void DownloadAndUnzip(String DownloadUri, String FileName)
        {
            //下载
            this.Title = "正在下載新版本文件,請耐心等待";
            string dir = GetExePath();


            string AutoUpdatefileName = GetExePath() + "\\AutoUpdate\\";

            bool download = false;

            long FileSize = GetFileSize(DownloadUri);

            string zipfile = System.IO.Path.Combine(AutoUpdatefileName, FileName);

            /*
            System.Windows.Forms.Timer dt = new System.Windows.Forms.Timer();
            dt.Interval =100;  //100毫秒
            dt.Tick += (x, y) => {
                if (File.Exists(zipfile) == false)
                    return;
                long size = new FileInfo(zipfile).Length;

                if (download == false)
                {  //是否下载完毕
                    label2.Text = size.ToString() + " / " + FileSize.ToString();  //输出：已下载/总大小

                    double Percent = (double)size / (double)FileSize * (double)100;
                    progressBar1.Value = (int)(Percent);
                }
                else
                {
                    dt.Stop();
                }
                if (IsStoped == true)
                {
                    dt.Stop();
                }


            };
            dt.Start();
            */


            IsDownloadFileFinished = false;


            bool success = await Task.Run(() =>
            {
                try
                {
                    //  WebClient client = new WebClient();
                    //client.DownloadFile(DownloadUri, zipfile);
                    //  DownloadFile(DownloadUri, zipfile, TargetFileMD5);
                    DownloadFileEx(DownloadUri, zipfile, TargetFileMD5);
                    if (IsStoped == true)
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });


            for (int i = 0; i < 3600; i++)
            {
                //  Form1.appendSysLog("123");
                Delay(500);
                if (IsDownloadFileFinished == true)
                {
                    break;
                }
            }


            if (DownLoadSuccess)
            {
                this.Title = "文件已下載，正在复制文件";
            }
            else
            {
                System.Windows.MessageBox.Show("下載新版本文件失敗，請重試");
                // panel1.Visible = false;
                // panel2.Visible = true;

                return;
            }


            string NewVersionfileName = GetExePath() + "\\NewVersion\\";
            DirectoryInfo di = new DirectoryInfo(NewVersionfileName);
            try
            {
                di.Delete(true);
            }
            catch { }




            UsefulFunction.CheckAndCreateDir(NewVersionfileName);
            /*
            //杀死主程序进程
            string appname = "File";
            Process[] processes = Process.GetProcessesByName(appname);
            foreach (var p in processes)
            {
                p.Kill();
            }
            */

            // MessageBox.Show("F");

            //解压缩    //+拷贝+删除
            bool success2 = await Task.Run(() =>
            {
                try
                {
                    string extractPath = System.IO.Path.Combine(dir, "NewVersion");
                    ZipFile.ExtractToDirectory(zipfile, extractPath);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });

            if (success2)
            {

                SavebatAndRestart();


                IntPtr hproc = GetCurrentProcess();
                TerminateProcess(hproc, 0);
            }
            else
            {
                System.Windows.MessageBox.Show("复制更新文件出錯");
            }


            this.Hide();

        }

        static string CalculateFileMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public bool DownloadFileEx(string sourceFile, string desFile, String TargetMD5)
        {
            bool flag = false;
            long SPosition = 0;
            FileStream FStream = null;
            Stream myStream = null;
            int LastTickCount = 0;
            String AddStr = "";

            for (int kk = 1; kk <= 3; kk++)
            {
                LastTickCount = 0;
                AddStr = "";

          
                label2.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() =>
                {
                    label2.Text = "準備下載...";
                }));
                //
                // Delay(1000);
          
                Delay(1000);




                try
                {
                    //判斷要下載的資料夾是否存在
                    long serverFileLength = GetHttpLength(sourceFile);
                    //MessageBox.Show("serverFileLength=" + serverFileLength.ToString());
                    if (File.Exists(desFile))
                    {
                        //開啟上次下載的檔案
                        FStream = File.OpenWrite(desFile);
                        //獲取已經下載的長度
                        SPosition = FStream.Length;


                        if (SPosition == serverFileLength)
                        {//檔案是完整的，直接結束下載任務
                         // Form1.appendSysLog("檔案是完整的");
                            label2.Dispatcher.Invoke(DispatcherPriority.Normal,
                           new Action(() =>
                           {
                               label2.Text = "續傳,下載完成";
                           }));
                            ProgressBar.Dispatcher.Invoke(DispatcherPriority.Normal,
                             new Action(() =>
                             {
                                 ProgressBar.Width = ProgressBarMax.ActualWidth;
                             }));

                            StatusText.Text = "檔案不儲存,建立一個檔案";
                            if (FStream != null)
                            {
                                FStream.Close();
                                FStream.Dispose();
                            }

                            if (TargetMD5.ToUpper() == CalculateFileMD5(desFile).ToUpper())
                            {
                                DownLoadSuccess = true;
                                IsDownloadFileFinished = true;

                                return true;
                            }
                            else
                            {


                                DownLoadSuccess = false;
                                //檔案不儲存建立一個檔案
                                FStream = new FileStream(desFile, FileMode.Create);
                                SPosition = 0;
                            }

                        }

                        AddStr = "續傳:";
                        FStream.Seek(SPosition, SeekOrigin.Current);
                    }
                    else
                    {
                        //檔案不儲存建立一個檔案
                        FStream = new FileStream(desFile, FileMode.Create);
                        SPosition = 0;
                    }




                    //開啟網路連線
                    HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(sourceFile);
                    if (SPosition > 0)
                    {
                        myRequest.AddRange(SPosition);             //設定Range值
                    }
                    //向伺服器請求,獲得伺服器的迴應資料流
                    myStream = myRequest.GetResponse().GetResponseStream();

                    //定義一個位元組資料
                    byte[] btContent = new byte[10240];
                    int intSize = 0;
                    int downloadSize = (int)SPosition;
                    int downloadSpeed = 0;
                    var beginSecond = DateTime.Now.Second;//当前时间秒
                                                          //使用追加方式打开一个文件流


                    intSize = myStream.Read(btContent, 0, 10240);
                    while (intSize > 0)
                    {
                        FStream.Write(btContent, 0, intSize);
                        intSize = myStream.Read(btContent, 0, 10240);

                        if (IsStoped == true)
                        {
                            break;
                        }

                        downloadSize += intSize;
                        downloadSpeed += intSize;


                       
                            try
                            {

                                if (DateTime.Now.Second - LastTickCount > 0 || downloadSize >= serverFileLength)
                                {
                                    //Delay(10);
                                    LastTickCount = DateTime.Now.Second;
                                    var endSecond = DateTime.Now.Second;
                                    if (beginSecond != endSecond)//计算速度
                                    {
                                        ProgressBar.Dispatcher.Invoke(DispatcherPriority.Background,
                                                 new Action(delegate { }));
                                        try
                                        {
                                            label2.Dispatcher.Invoke(DispatcherPriority.Normal,
                                           new Action(() =>
                                           {
                                               downloadSpeed = downloadSpeed / (endSecond - beginSecond);
                                               label2.Text = AddStr + downloadSize.ToString() + "/" + serverFileLength.ToString();// + "(下載速度" + downloadSpeed / 1024 + "KB/S)";

                                           }));
                                        }
                                        catch
                                        {

                                        }


                                        beginSecond = DateTime.Now.Second;
                                        downloadSpeed = 0;//清空
                                    }
                                }
                                else
                                {
                                    Delay(1);
                                }
                            }
                            catch
                            {

                            }



                            double scaleFactor = (double)((double)downloadSize / (double)serverFileLength);
                            try
                            {

                                ProgressBar.Dispatcher.Invoke(DispatcherPriority.Normal,
                                new Action(() =>
                                {


                                    if (scaleFactor > 0 && scaleFactor <= 1)
                                    {

                                        ProgressBar.Width = ProgressBarMax.ActualWidth * scaleFactor;
                                    }
                                    else
                                    {
                                        ProgressBar.Width = 0;
                                    }
                                }));
                            }
                            catch
                            {

                            }

                        

                    }
                    flag = true;        //返回true下載成功

                 
                    label2.Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            label2.Text = AddStr + "下載完成...";
                        }));
                    label2.Dispatcher.Invoke(DispatcherPriority.Background,
                                        new Action(delegate { }));
                    // Thread.Sleep(1000);
                  

                    Delay(1000);
                    //  Delay(1000);


                }
                catch (Exception ex)
                {
                     StatusText.Dispatcher.Invoke(DispatcherPriority.Normal,
                      new Action(() =>
                      {
                          StatusText.Text =  "下載檔案時異常：" + ex.Message;
                      }));
                }
                finally
                {
                    //關閉流
                    if (myStream != null)
                    {
                        myStream.Close();
                        myStream.Dispose();
                    }
                    if (FStream != null)
                    {
                        FStream.Close();
                        FStream.Dispose();
                    }
                }


                // MessageBox.Show(TargetMD5.ToUpper() + "\r\n" + CalculateFileMD5(desFile).ToUpper());
                try
                {
                    if (File.Exists(desFile))
                    {
                        if (TargetMD5.ToUpper() == CalculateFileMD5(desFile).ToUpper())
                        {
                            DownLoadSuccess = true;
                        }
                        else
                        {
                            DownLoadSuccess = false;
                        }
                    }
                    else
                    {
                        DownLoadSuccess = false;
                    }

                }
                catch
                {
                    DownLoadSuccess = false;
                }


                if (DownLoadSuccess == true)
                {
                    break;
                }
                else
                {
                    StatusText.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        StatusText.Text = "下載更新檔案失敗,重試次數:" + kk.ToString();
                    }));
                  
                    Delay(3000);
                }

            }




            IsDownloadFileFinished = true;
            return flag;
        }
        static long GetHttpLength(string url)
        {
            long length = 0;
            try
            {
                var req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                req.Method = "HEAD";
                req.Timeout = 5000;
                var res = (HttpWebResponse)req.GetResponse();
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    length = res.ContentLength;
                }
                res.Close();
                return length;
            }
            catch (WebException wex)
            {
                return 0;
            }
        }



    }
}
