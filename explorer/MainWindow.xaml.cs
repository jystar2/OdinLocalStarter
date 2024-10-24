using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace NCCaptureInjector32
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 




    public static class StreamExtensions
        {
            public static byte[] ReadAllBytes(this Stream instream)
            {
                if (instream is MemoryStream)
                    return ((MemoryStream)instream).ToArray();

                using (var memoryStream = new MemoryStream())
                {
                    instream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

    public partial class MainWindow : Window
    {

        public class PurpleSocketClass
        {
            public Boolean IsBind = false;
            public int BindedPID = 0;
            public int Bindedhwnd = 0;
            public String IPPort = "";
            public String Version = "";
        }

        public class RecordPurpleSocketClass
        {
            public int BindedPID = 0;
            public int Bindedhwnd = 0;
            public String IPPort = "";
            public String Version = "";
            public String DLLName = "";
        }

        public static PurpleSocketClass PurpleSocket = new PurpleSocketClass();
        public static RecordPurpleSocketClass RecordPurpleSocket = new RecordPurpleSocketClass();

        //[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        public String BaseFilePath = "";
        public bool IsFirstTime = true;
        public bool IsVerifyed = false;
        public static Boolean IsConnected = false;
        public static AutoUpdatePage AutoUpdateForm;
        public static MainWindow ThisForm;
        public long StartDestoryTime = 0;
        System.Windows.Threading.DispatcherTimer dispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        public class FileListClass
        {
            public String FileName = "";
            public byte[] Content;
            public int FileSize = 0;
        }
        public static List<FileListClass> AttachmemtFile = new List<FileListClass>();
        public static void AppendAllBytes(string path, byte[] bytes)
        {
            //argument-checking here.

            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
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
        public static int InStr(int StartIndex, String InpuStr, String FindStr)
        {
            if (InpuStr == null)
            {
                return 0;
            }
            if (StartIndex > InpuStr.Length || InpuStr.Length == 0)
            {
                return 0;
            }
            else if (StartIndex > 0)
            {
                return InpuStr.IndexOf(FindStr, StartIndex - 1) + 1; // 开始搜索的索引位置，第一个字符是 0 ，第二个是 1  ,如果此字符串中没有这样的字符，则返回 -1。
            }
            return 0;

        }
        private static bool IsWin32Emulator( Process process)
        {
            bool retVal = false;
           // IsWow64Process(process.Handle, out retVal);

          //  MessageBox.Show(retVal.ToString());
            return retVal;
            //return false; // not on 64-bit Windows Emulator
        }


  
        public MainWindow()
        {




            InitializeComponent();
            ThisForm = this;



           // MessageBox.Show("1");


            VersionLabel.Text = GeneralVariable.NowVersion;
            AutoUpdateForm = new AutoUpdatePage();

            GeneralVariable.RandomInjectDLLName = UsefulFunction.GetRandomFilename(15);


            string fileName = GetExePath() + "\\DLLI\\";
            UsefulFunction.CheckAndCreateDir(fileName);

            try
            {
                try
                {
                    string path = GetExePath() + "\\DLLI\\"; ;
                    string[] dirs = Directory.GetFiles(path, "*.dll");

                    foreach (string dir in dirs)
                    {

                        try
                        {
                            File.Delete(dir);
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {

                }

            }
            catch
            {

            }


            fileName = GetExePath() + "\\Exe\\";
            try
            {
                String MyProcessName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);


                UsefulFunction.CheckAndCreateDir(fileName);

                string path = GetExePath() + "\\"; //刪自己FILE下的
                string[] dirs = Directory.GetFiles(path, "*.exe");

                foreach (string dir in dirs)
                {
                    if (System.IO.Path.GetFileName(dir) != MyProcessName)
                    {
                        try
                        {
                            File.Delete(dir);
                        }
                        catch
                        {

                        }
                    }
                }

                UsefulFunction.CheckAndCreateDir(fileName);

                path = GetExePath() + "\\"; //刪自己FILE下的
                dirs = Directory.GetFiles(path, "*.*");

                foreach (string dir in dirs)
                {
                    foreach (string DeleteName in GeneralVariable.DeleteProcessNameList)
                    {
                        if (InStr(1, System.IO.Path.GetFileName(dir), DeleteName) > 0)
                        {
                            try
                            {
                                File.Delete(dir);
                            }
                            catch
                            {

                            }
                        }
                    }
                }




                path = GetExePath() + "\\Exe\\";//刪EXE FILE下的
                dirs = Directory.GetFiles(path, "*.exe");

                foreach (string dir in dirs)
                {
                    if (System.IO.Path.GetFileName(dir) != MyProcessName)
                    {
                        // MessageBox.Show("Same MD5");
                        try
                        {
                            File.Delete(dir);
                        }
                        catch
                        {

                        }
                    }
                }


            }
            catch
            {

            }

            try
            {
                string path = GetExePath() + "\\AutoUpdate\\"; //刪自己FILE下的
                string[] dirs = Directory.GetFiles(path, "*.*");

                foreach (string dir in dirs)
                {

                    try
                    {
                        File.Delete(dir);
                    }
                    catch
                    {

                    }

                }
            }
            catch
            {

            }

            ReadAllAttachment();

            String IsSoftwareMode = "0";
            try
            {
                IsSoftwareMode = System.IO.File.ReadAllText(GetExePath() + "\\SoftwareMode.ini");

            }
            catch
            {

            }
            if (IsSoftwareMode == "1")
            {
                System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
            }

            Boolean IsSuccess = false;
            try
            {
                String ProxyJson = System.IO.File.ReadAllText(GetExePath() + "\\StarterSetting.ini");
                JObject TmpObject = Cjson.Decode(ProxyJson);
                if (TmpObject != null)
                {
                    ReadControlFromJson(TmpObject, (System.Windows.DependencyObject)(this));
                }else 
                {
                    TmpObject = Cjson.Decode("{}");
                    ReadControlFromJson(TmpObject, (System.Windows.DependencyObject)(this));
                }
                IsSuccess = true;

            }
            catch (Exception)
            {
               
            }
            if (IsSuccess == false)
            {
                //MessageBox.Show("");
                JObject TmpObject = Cjson.Decode("{}");
                ReadControlFromJson(TmpObject, (System.Windows.DependencyObject)(this));
            }


           // MessageBox.Show("2");

            String RecordPurpleInfo = "";
            try
            {
                UsefulFunction.CheckAndCreateDir(GetExePath() + "\\Exe\\");
                RecordPurpleInfo = System.IO.File.ReadAllText(GetExePath() + "\\Exe\\RecordHelpInfo.ini");
                JObject TmpObject = Cjson.Decode(RecordPurpleInfo);
                if (TmpObject != null)
                {
                    RecordPurpleSocket.Bindedhwnd = Cjson.GetJsonInt(TmpObject, "Bindedhwnd");
                    RecordPurpleSocket.BindedPID = Cjson.GetJsonInt(TmpObject, "BindedPID");
                    RecordPurpleSocket.DLLName = Cjson.GetJsonStr(TmpObject, "DLLName");
                }

            }
            catch
            {
            }

            try
            {
                if (RecordPurpleSocket.DLLName == "")
                {
                    RecordPurpleSocket.DLLName = GetRandomFilename(12);
                    System.IO.File.WriteAllText(GetExePath() + "\\Exe\\RecordHelpInfo.ini", Cjson.Encode(RecordPurpleSocket));
                }
            }
            catch
            {

            }




            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);

           // HelperSocketHelper.StartServer("127.0.0.1", 29900);

            CheckAutoUpdate(false);

            dispatcherTimer1.Tick += new EventHandler(dispatcherTimer1_Tick);
            dispatcherTimer1.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer1.Start();

        }
        public static long Time()
        {
            long unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }

        private void dispatcherTimer1_Tick(object sender, EventArgs e)
        {
            // code goes here 
            Process[] Game = Process.GetProcessesByName("ProjectLH");
            if (Game != null)
            {
                if (Game.Length > 0)
                {
                    this.Title = "遊戲已開啟,請盡快啟動/關閉本程序,20秒後本程序將自動關閉";
                    if (StartDestoryTime == 0)
                    {
                        StartDestoryTime = Time();
                    }
                    else
                    {
                        if (Time() - StartDestoryTime > 20)
                        {
                            MainWindow_Closing(null, null);
                        }
                    }
                    for (int j = 0; j < Game.Length; j++)
                    {
                        IntPtr ThProcess2 = IntPtr.Zero;
                        ThProcess2 = OpenProcess(0x0001, false, (uint)Game[j].Id);
                        if (ThProcess2 != IntPtr.Zero)
                        {
                            TerminateProcess(ThProcess2, 0);
                        }

                        if (ThProcess2 != IntPtr.Zero)
                        {
                            CloseHandle(ThProcess2);
                        }
                    }
                }
            }

        }


        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);


        public void RunExternalExeNoWait(string filename, string arguments = null)
        {

            try
            {
                int a = ShellExecute(IntPtr.Zero, "open", filename, arguments, "", 1);

            }
            catch (Exception)
            {
                appendSysLog("錯誤運行" + filename + "錯誤" );
                return;
            }

        }

        public static void ReadControlFromJson(JObject JsonObject, DependencyObject Parent)
        {

            foreach (var tmpcontrol in FindLogicalChildren<CheckBox>(Parent))
            {
                try
                {
                    if (JsonObject.SelectToken(tmpcontrol.Name) != null)
                    {
                        ((CheckBox)tmpcontrol).IsChecked = Cjson.GetJsonBoolean(JsonObject, tmpcontrol.Name);
                    }

                }
                catch
                {
                }
            }
            foreach (var tmpcontrol in FindLogicalChildren<TextBox>(Parent))
            {
                try
                {
                    if (JsonObject.SelectToken(tmpcontrol.Name) != null)
                    {
                        ((TextBox)tmpcontrol).Text = Cjson.GetJsonStr(JsonObject, tmpcontrol.Name);
                    }
                }
                catch
                {

                }
            }

         
            foreach (var tmpcontrol in FindLogicalChildren<ComboBox>(Parent))
            {


                try
                {
                    
                        ReadControlBindData(tmpcontrol);

                        //System.Windows.MessageBox.Show("tmpcontrol.Name:" + tmpcontrol.Name);

                        if (JsonObject.SelectToken(tmpcontrol.Name) != null)
                        {
                            tmpcontrol.SelectedIndex = Cjson.GetJsonInt(JsonObject, tmpcontrol.Name);
                            //   System.Windows.MessageBox.Show(tmpcontrol.Name + "=" + ((ComboBox)tmpcontrol).SelectedIndex.ToString());


                        }
                        else
                        {

                        }
                    
                    // 
                }
                catch (Exception E)
                {

                    //  System.Windows.MessageBox.Show(E.ToString());
                }

                try
                {
                    
                        if (((ComboBox)tmpcontrol).IsEditable == false)
                        {
                            if (((ComboBox)tmpcontrol).SelectedIndex < 0)
                            {
                                ((ComboBox)tmpcontrol).SelectedIndex = 0;
                            }
                        }
                    
                    // System.Windows.MessageBox.Show("((ComboBox)tmpcontrol).SelectedIndex:" + ((ComboBox)tmpcontrol).SelectedIndex.ToString());
                }
                catch
                {
                }

                try
                {
                    
                        if (JsonObject.SelectToken(tmpcontrol.Name + "Str") != null)
                        {
                            if (tmpcontrol.IsEditable)
                            {
                                ((ComboBox)tmpcontrol).Text = Cjson.GetJsonStr(JsonObject, tmpcontrol.Name + "Str");
                            }
                        }
                    
                }
                catch
                {
                }
            }



            


        }

        public static void ReadControlBindData(Control tmpcontrol)
        {

            if (tmpcontrol is ComboBox)
            {

                // MessageBox.Show(((ComboBox)tmpcontrol).Name);
                ((ComboBox)tmpcontrol).ItemsSource = null;

                ((ComboBox)tmpcontrol).Items.Clear();


                BindingFlags bindingFlags = BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.Instance |
                        BindingFlags.Static;

                foreach (FieldInfo field in typeof(R.ArrayClass).GetFields(bindingFlags))
                {
                    if (((ComboBox)tmpcontrol).Tag != null)
                    {
                        //MessageBox.Show(((ComboBox)tmpcontrol).Tag.ToString() + "," + field.Name);
                        if (((ComboBox)tmpcontrol).Tag.ToString() == field.Name)
                        {
                            //  MessageBox.Show(((ComboBox)tmpcontrol).Tag.ToString() + Cjson.Encode(field.GetValue(R.array)));
                            //((ComboBox)tmpcontrol).Items.((String[])field.GetValue(R.array));
                            ((ComboBox)tmpcontrol).ItemsSource = (String[])field.GetValue(R.array);

                            /*
                            for (int i = 0; i < 200; i++)
                            {
                                if ((String[])field.GetValue(R.array) != null)
                                {
                                    if (((ComboBox)tmpcontrol).ItemsSource == null)
                                    {
                                        Delay(10);
                                    }
                                }
                                
                            }
                            */

                            break;
                        }
                    }
                    //  Console.WriteLine(field.Name);
                }
                //         ((ComboBox)tmpcontrol).EndUpdate();
            }

        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            SaveControl();

            HelperSocketHelper.StopServer();

            IntPtr hproc = GetCurrentProcess();
            TerminateProcess(hproc, 0);
        }

        public static string[] Split(String TmpStr, String deli)
        {
            string[] Result = null;

            if (TmpStr != null)
            {
                Result = TmpStr.Split(new[] { deli }, StringSplitOptions.None);
            }

            return Result;
        }
        public static int byteArrayToInt(byte[] b, int StartIndex)
        {
            return b[StartIndex + 3] << 24 | (b[StartIndex + 2] & 0xff) << 16 | (b[StartIndex + 1] & 0xff) << 8
                    | (b[StartIndex] & 0xff);
        }

        public void ReadAllAttachment()
        {
    
            int StartIndex = 0;
            int FileSize;
            byte[] FileContent;
            byte[] FileNameArr;
            byte[] NewFileByte;
            byte[] FileNameByte;
            String FileNameStr = "";
            String[] FileNameStrList;
            byte[] DecodeKey = { 0, 0, 0, 0, 0, 0, 0 };
            DecodeKey[0] = 0x69;
            DecodeKey[1] = 0x31;
            DecodeKey[2] = 0x62;
            DecodeKey[3] = 0x93;
            DecodeKey[4] = 0x20;
            DecodeKey[5] = 0x15;
            DecodeKey[6] = 0x88;

            FileContent = Properties.Resources.Data;
            // appendSysLog("[+]FileContent Size=" + CStr(FileContent.Length));

            FileNameArr = Properties.Resources.DataN;
            //  appendSysLog("[+]FileContent Size= " + CStr(FileContent.Length));


            FileSize = byteArrayToInt(FileContent, StartIndex);
            // appendSysLog("[+]FileSize Size="+ CStr(FileSize));


            FileNameByte = new byte[FileNameArr.Length];
            for (int i = 0; i < FileNameArr.Length; i++)
            {
                FileNameByte[i] = (byte)(FileNameArr[StartIndex + i] ^ ((int)DecodeKey[i % DecodeKey.Length]));
            }

            try
            { //不準
                FileNameStr = Encoding.Default.GetString(FileNameByte);
            }
            catch
            {
                FileNameStr = "";
            }

            AttachmemtFile.Clear();

            //MessageBox.Show(FileNameStr);
            if (FileNameStr.Equals("") == false)
            {


                FileNameStrList = Split(FileNameStr, "\n");

                foreach (String TmpFileName in FileNameStrList)
                {

                    // appendSysLog("[+]TmpFileName=" + TmpFileName);
                    FileSize = byteArrayToInt(FileContent, StartIndex);
                    //LogD("[+]FileSize2 Size=", "[+]" + CStr(FileSize));
                    //  LogD("[+]TmpFileName=", "[+]" + TmpFileName);

                    NewFileByte = new byte[FileSize];

                    StartIndex = StartIndex + 4;

                    for (int i = 0; i < FileSize; i++)
                    {
                        NewFileByte[i] = (byte)(FileContent[StartIndex + i] ^ ((int)DecodeKey[i % DecodeKey.Length]));
                    }

                    StartIndex = StartIndex + FileSize;

                    FileListClass TmpItem = new FileListClass();
                    TmpItem.FileName = TmpFileName;
                    TmpItem.Content = NewFileByte;
                    TmpItem.FileSize = FileSize;

                    AttachmemtFile.Add(TmpItem);

                }
            }
          
            //-----------------------------------------------------------------------------------------
        }

        public static Byte[] ReadAttachFileArr(String TargetName)
        {
            foreach (FileListClass TmpItem in AttachmemtFile)
            {
                if (TmpItem.FileName.Equals(TargetName))
                {
                    try
                    { //不準
                        return TmpItem.Content;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private void Recover_Click(object sender, RoutedEventArgs e)
        {
            if (BaseFilePath != "")
            {
                try
                {
                    if (File.Exists(BaseFilePath + "Patches\\Install\\" + "ProjectLH-WindowsNoEditor_3_P.pak"))
                    {
                        File.Delete(BaseFilePath + "Patches\\Install\\" + "ProjectLH-WindowsNoEditor_3_P.pak");
                        File.Delete(BaseFilePath + "Patches\\Install\\" + "ProjectLH-WindowsNoEditor_3_P.sig");
                        MessageBox.Show("恢復漢化成功!");
                    }
                    else{
                        MessageBox.Show("未曾漢化!");
                    }
              
                }
                catch(Exception E)
                {
                    MessageBox.Show("恢復漢化失敗!\n" + E.ToString());
                }
            }
            else
            {
                MessageBox.Show("遊戲路徑未設成功,請點擊 瀏覽 選擇");
            }
        }



        private string GetOdinAddress()
        {
            string info = null;
            try
            {
                RegistryKey Key;
                Key = Registry.CurrentUser;
                RegistryKey myreg = Key.OpenSubKey("Software\\DaumGames\\ODIN_CLIENT");
                info = myreg.GetValue("InstallPath").ToString();

                myreg.Close();
            }
            catch (Exception)
            {

            }
            //MessageBox.Show(info);
            return info;
        }
        private void Browser_Click(object sender, MouseButtonEventArgs e)
        {

    
            var fileContent = string.Empty;
            var filePath = string.Empty;
            MessageBox.Show(System.IO.Path.GetDirectoryName(GetOdinAddress()) + "\\");
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                if (IsFirstTime == true)
                {
                    if (GetOdinAddress() == "null")
                    {
                        openFileDialog.InitialDirectory = "C:\\";
                    }
                    else
                    {
                        openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(GetOdinAddress()) + "\\";
                    }
      
                    IsFirstTime = false;
                }
            

                openFileDialog.Filter = "Odin.exe files (*.exe)|Odin.exe";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    BaseFilePath = filePath;

                    if (BaseFilePath.IndexOf("Odin.exe") > -1)
                    {
                        BaseFilePath = BaseFilePath.Replace("Odin.exe", "");
                        MessageBox.Show("已成功設定遊戲路徑,可以開始漢化");
                    }
                    else
                    {
                        MessageBox.Show("遊戲路徑設定錯誤!");
                    }

                    //Read the contents of the file into a stream

                }
            }
        }

        private void Begin_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

    
        public String MyActivationCode = "";
        private Boolean IsActivated = false;
        public int NowLiscenceCount = 0;
        public int MaxLiscenceCount = 0;
        public string HttpGet(string uri)
        {
            String Result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    Result = reader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {

                throw exception;
            }
            return Result;
        }


        private void BotNewVersionLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CheckAutoUpdate(true);
        }

        private void 檢查版本ToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CheckAutoUpdate(true);
        }


        public static void appendSysLog(string log)
        {
            try
            {
                MainWindow.ThisForm.SystemLog.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => {

                    if (MainWindow.ThisForm.SystemLog.Text != "")
                    {
                        MainWindow.ThisForm.SystemLog.Text = MainWindow.ThisForm.SystemLog.Text + "\r\n";
                    }
                    if (MainWindow.ThisForm.SystemLog.Text.Length > 20000)
                    {

                        MainWindow.ThisForm.SystemLog.Text = "";
                    }
                    MainWindow.ThisForm.SystemLog.Text = MainWindow.ThisForm.SystemLog.Text + DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss] ") + log;
                    MainWindow.ThisForm.SystemLog.CaretIndex = MainWindow.ThisForm.SystemLog.Text.Length;
                    MainWindow.ThisForm.SystemLog.ScrollToEnd();


                }));

            }
            catch
            {

            }

        }
        public int SavePurpleFileToRes(String FromPathName, String TmpFileName)
        {

            foreach (FileListClass TmpItem in AttachmemtFile)
            {
                if (TmpItem.FileName.Equals(TmpFileName))
                {
                    try
                    {
                        File.WriteAllBytes(FromPathName, TmpItem.Content);
                    }
                    catch
                    {

                    }


                    //  IOLib.SaveFileinByte(FromPath, TmpFileName, TmpItem.Content, TmpItem.FileSize);
                    return TmpItem.FileSize;
                }
            }
            return 0;
        }
        public static string GetExePath()
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //C:\Program Files\MyApplication
            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            return strWorkPath;
        }

        public static Boolean IsPurpleConnected()
        {
         
            if (PurpleSocket.IsBind == true)
            {
                try
                {
                    Process TmpProcess = Process.GetProcessById(PurpleSocket.BindedPID);
                    if (TmpProcess != null)
                    {
                        return true;
                    }
                    else
                    {
                        PurpleSocket.IsBind = false;
                    }
                }
                catch
                {
                }
            }

            return false;
        }
        /*
            const int PROCESS_CREATE_THREAD = 0x0002;
            const int PROCESS_QUERY_INFORMATION = 0x0400;
            const int PROCESS_VM_OPERATION = 0x0008;
            const int PROCESS_VM_WRITE = 0x0020;
            const int PROCESS_VM_READ = 0x0010;

            // used for memory allocation
            const uint MEM_COMMIT = 0x00001000;
            const uint MEM_RESERVE = 0x00002000;
            const uint PAGE_READWRITE = 4;



            [DllImport("kernel32.dll")]

            public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr GetModuleHandle(string lpModuleName);

            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

            [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
            static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
                uint dwSize, uint flAllocationType, uint flProtect);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

            [DllImport("kernel32.dll")]
            static extern IntPtr CreateRemoteThread(IntPtr hProcess,
                IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);


            public static int InjectDll(uint pid, String DllPath)
            {
                //  Process targetProcess = Process.GetProcessesByName("testApp")[0];

                // geting the handle of the process - with required privileges
                IntPtr procHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, pid);
                if (procHandle == IntPtr.Zero)
                {
                    return 2;
                }

                // MessageBox.Show("procHandle=" + procHandle.ToString());

                // searching for the address of LoadLibraryA and storing it in a pointer
                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                if (loadLibraryAddr == IntPtr.Zero)
                {
                    return 3;
                }

                //    MessageBox.Show("loadLibraryAddr=" + loadLibraryAddr.ToString());
                // name of the dll we want to inject
                string dllName = DllPath;

                // alocating some memory on the target process - enough to store the name of the dll
                // and storing its address in a pointer
                // Form1.SetPriv();
                // MessageBox.Show(((uint)((Encoding.Default.GetBytes(dllName).Length + 1) * Marshal.SizeOf(typeof(char)))).ToString());
                IntPtr allocMemAddress = IntPtr.Zero;
                for (int i = 0; i < 300; i++)
                {
                    if ((uint)((Encoding.Default.GetBytes(dllName).Length + 1) * Marshal.SizeOf(typeof(char))) < 512)
                    {
                        allocMemAddress = VirtualAllocEx(procHandle, IntPtr.Zero, 512, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
                    }
                    else
                    {
                        allocMemAddress = VirtualAllocEx(procHandle, IntPtr.Zero, (uint)((Encoding.Default.GetBytes(dllName).Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
                    }

                    if (allocMemAddress == IntPtr.Zero)
                    {

                    }
                    else
                    {
                        break;
                    }
                    Delay(20);
                }

                if (allocMemAddress == IntPtr.Zero)
                {
                    return 4;
                }

                // writing the name of the dll there
                UIntPtr bytesWritten;
                WriteProcessMemory(procHandle, allocMemAddress, Encoding.Default.GetBytes(dllName), (uint)((Encoding.Default.GetBytes(dllName).Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);
                if (bytesWritten == UIntPtr.Zero)
                {
                    return 5;
                }
                IntPtr Thread_ID = IntPtr.Zero;
                // creating a thread that will call LoadLibraryA with allocMemAddress as argument
                CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, out Thread_ID);
                if (Thread_ID == IntPtr.Zero)
                {
                    return 6;
                }


                return 0;

            }

            */

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool FreeLibrary(IntPtr hModule);

        public delegate int delegate_InjByMem(byte[] Buff, int BuffSize, uint pid);
        public delegate int delegate_InjectDll([MarshalAs(UnmanagedType.LPWStr)] String DLLPath, uint pid);

        public static int InjectDllByMem(byte[] Buff, int BuffSize, uint pid)
        {


            if (File.Exists(GetExePath() + "\\DLLI\\" + GeneralVariable.RandomInjectDLLName + ".dll") == false)
            {
                File.WriteAllBytes(GetExePath() + "\\DLLI\\" + GeneralVariable.RandomInjectDLLName + ".dll", ReadAttachFileArr("MainLoad64.dll"));
            }
            else
            {

            }

            //appendSysLog("Is啟用特工保護進=" + Is啟用特工保護進程.ToString());
            IntPtr pDll = LoadLibrary(GetExePath() + "\\DLLI\\" + GeneralVariable.RandomInjectDLLName + ".dll");
            //MessageBox.Show(Form1.ThisForm.GetExePath() + "\\DLLI\\" + GeneralVariable.RandomInjectDLLName + ".dll" + "," + pDll.ToString());

            if (pDll != IntPtr.Zero)
            {
                int IJResult = -2;
                IntPtr intPtr = GetProcAddress(pDll, "InjByMem");
                if (intPtr != IntPtr.Zero)
                {
                    delegate_InjByMem InjByMem = (delegate_InjByMem)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(delegate_InjByMem));
                    IJResult = InjByMem(Buff, BuffSize, pid);

                }
                FreeLibrary(pDll);

                //  MessageBox.Show("IJResult=" + IJResult.ToString());
                return IJResult;
            }
            else
            {


                return -1;
            }
        }
        

        public static void Delay(int Millisecond)
        {
            for (int i = 0; i < Convert.ToInt32(Math.Floor((double)(Millisecond / 10))); i++)
            {
                Thread.Sleep(10);
            }
        }
        public static String Replace(String InpuStr, String TargetStr, String ReplaceStr)
        {
            if (InpuStr != null)
            {
                return InpuStr.Replace(TargetStr, ReplaceStr);
            }
            else
            {
                return InpuStr;
            }

        }
        public static int CLNG(String InputText)
        {
            int x = 0;

            if (Int32.TryParse(InputText, out x))
            {
            }
            return x;
        }

        public void CompareMd5AndReleaseNoLog(String Path, byte[] byteArray1)
        {

            try
            {
                if (File.Exists(GetExePath() + "\\" + Path) == true)
                {
                    File.Delete(GetExePath() + "\\" + Path);
                }
            }
            catch
            {

            }
            try
            {

                File.WriteAllBytes( Path, byteArray1);
            }
            catch
            {

            }

        }


        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll ")]
        public static extern bool CloseHandle(IntPtr hProcess);


        public void KillApp(String AppName)
        {
            Process[] ps = Process.GetProcessesByName(AppName);
            if (ps.Length != 0)
            {
                for (int j = 0; j < ps.Length; j++)
                {

                    IntPtr ThProcess2 = IntPtr.Zero;
                    ThProcess2 = OpenProcess(0x0001, false, (uint)ps[j].Id);
                    if (ThProcess2 != IntPtr.Zero)
                    {
                        TerminateProcess(ThProcess2, 0);
                    }

                    if (ThProcess2 != IntPtr.Zero)
                    {
                        CloseHandle(ThProcess2);
                    }
                }
            }
        }

        private void SetGameEnvironment_Click(object sender, RoutedEventArgs e)
        {


            /*
            if (ProcessList.Text == "")
            {
                MessageBox.Show("請選擇啟動程序");
                return;
            }
            */
            UsefulFunction.CheckAndCreateDir(GetExePath() + "\\Exe\\");

            /*
            if (ProcessList.Text.Contains(" - "))
            {
                String[] TmpArr = Split(ProcessList.Text, " - ");

                String ProcessIDTxt = TmpArr[1];
                //MessageBox.Show(ProcessIDTxt);

                Process TargetProcess = Process.GetProcessById(CLNG(ProcessIDTxt));

                if (TargetProcess == null)
                {
                    MessageBox.Show("目標進程已不在,請重新選擇");
                }
                else
                {
            */

                    // MessageBox.Show(TargetProcess.MainModule.FileName);
                    System.Diagnostics.Process NewProcess = null;
                    int ProcessPID = 0;// CLNG(ProcessIDTxt);

                    //if (不新建進程.IsChecked == false)
                    //{

                        /*
                        Process[] ps = Process.GetProcessesByName("notepad");
                        if (ps.Length != 0)
                        {
                            for (int j = 0; j < ps.Length; j++)
                            {

                                IntPtr ThProcess2 = IntPtr.Zero;
                                ThProcess2 = OpenProcess(0x0001, false, (uint)ps[j].Id);
                                if (ThProcess2 != IntPtr.Zero)
                                {
                                    TerminateProcess(ThProcess2, 0);
                                }

                                if (ThProcess2 != IntPtr.Zero)
                                {
                                    CloseHandle(ThProcess2);
                                }
                            }
                        }
                        */

                        Boolean IsOpenError = false;
                        try
                        {

                            
                            if (ModeList.SelectedIndex == 0 || ModeList.SelectedIndex == -1)
                            {
                                NewProcess = CreateShellExProcess("notepad.exe"); //TargetProcess.MainModule.FileName
                            }
                            else if (ModeList.SelectedIndex == 1)
                            {
                                NewProcess = CreateShellExProcess("regedit.exe"); //TargetProcess.MainModule.FileName
                            }
                            else if (ModeList.SelectedIndex == 2)
                            {
                                KillApp("Taskmgr");
                                KillApp("taskmgr");
                                NewProcess = CreateShellExProcess("Taskmgr.exe"); //TargetProcess.MainModule.FileName
                            }
                            else if (ModeList.SelectedIndex == 3)
                            {
                                KillApp("conhost");
                                NewProcess = CreateShellExProcess("conhost.exe"); //TargetProcess.MainModule.FileName
                            }
                            /*
                            if (使用其他進程.IsChecked == true && ModeList.SelectedIndex == 5 )
                            {
                                NewProcess = CreateShellExProcess(其他進程路徑.Text); //TargetProcess.MainModule.FileName
                            }
                            */


                /*
                if (IsWin32Emulator(NewProcess) == false)
                {

                }
                else
                {
                    MessageBox.Show("不是64位元程序!無法啟動助手!");
                }
                */

            }
            catch (Exception E)
                        {
                            IsOpenError = true;
                            MessageBox.Show("啟動錯誤:" + E.ToString());
                        }

                        if (NewProcess != null)
                        {
                             ProcessPID = NewProcess.Id;
                        }
            // }

            if (ProcessPID != 0)
            {
                var stream = Application.GetResourceStream(new Uri(@"pack://application:,,,/Resources/explorer.dll")).Stream;
                byte[] MAinExecDll = null;
                using (stream)
                {
                    MAinExecDll = stream.ReadAllBytes();
                }
                if (MAinExecDll != null)
                {
                    CompareMd5AndReleaseNoLog(Path.Combine(GetExePath() + "\\Exe\\", RecordPurpleSocket.DLLName + ".dll"), MAinExecDll);
                    //CompareMd5AndReleaseNoLog(Path.Combine(GetExePath() + "\\Exe\\", RecordPurpleSocket.DLLName + ".dll"), Properties.Resources.kineCall);
                    //MainWindow.ThisForm.SavePurpleFileToRes(Path.Combine(GetExePath() + "\\Exe\\", RecordPurpleSocket.DLLName + ".dll"), "kineCall.dll");
                    MainWindow.ThisForm.SavePurpleFileToRes(Path.Combine(GetExePath() + "\\Exe\\", "TestCLR.dll"), "TestCLR.dll");

                    File.WriteAllText(Path.Combine(Path.GetTempPath(), "TempCTarget.txt"), GetExePath() + "\\Exe\\" + RecordPurpleSocket.DLLName + ".dll");
                    int InjectResult = -1;
                    //if (ProcessList.Text.Contains(" - "))
                    //{
                    //TmpArr = Split(ProcessList.Text, " - ");

                    // ProcessIDTxt = TmpArr[1];
                    //MessageBox.Show(ProcessIDTxt);
                    // InjectResult = InjectDllByMem(ReadAttachFileArr("TestCLR.dll"), ReadAttachFileArr("TestCLR.dll").Length, (uint)CLNG(ProcessIDTxt));//
                    InjectResult = InjectDllByMem(ReadAttachFileArr("TestCLR.dll"), ReadAttachFileArr("TestCLR.dll").Length, (uint)ProcessPID);//(uint)CLNG(ProcessIDTxt)
                                                                                                                                               //}
                    //MessageBox.Show(InjectResult.ToString());
                    Delay(1000);

                    try
                    {
                        File.Delete(GetExePath() + "\\Exe\\TestCLR.dll");
                    }
                    catch
                    {

                    }

                    if (InjectResult == 1)
                    {
                        //StatusText.Text = "啟動成功!請關閉助手啟動器,防止檢測";
                        //System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("設定環境成功!是否關閉助手?", "助手", System.Windows.Forms.MessageBoxButtons.YesNo);

                        // if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                        // {
                        if (File.Exists(GetExePath() + "\\DLLI\\" + GeneralVariable.RandomInjectDLLName + ".dll") == true)
                        {
                            try
                            {
                                File.Delete(GetExePath() + "\\DLLI\\" + GeneralVariable.RandomInjectDLLName + ".dll");
                            }
                            catch
                            {

                            }

                        }

                        MainWindow_Closing(null, null);
                        // }
                    }
                    else
                    {
                        MessageBox.Show("啟動失敗!原因:" + InjectResult.ToString());
                    }


                }
            }
                   
                    
                  
               // }


               
            //}

        }

        private static System.Diagnostics.Process CreateShellExProcess(string cmd, string workingDir = "")
        {
            //cmd为想要执行的进程名，workingDir为它的所在路径，如果在windows\system32可以为空
            //MessageBox.Show(cmd);
            var pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd);
            pStartInfo.Verb = "runas";//设置该启动动作，会以管理员权限运行进程
            pStartInfo.CreateNoWindow = false;
            pStartInfo.UseShellExecute = true;
            pStartInfo.RedirectStandardError = false;
            pStartInfo.RedirectStandardInput = false;
            pStartInfo.RedirectStandardOutput = false;
            //pStartInfo.Arguments = "这里传入参数";
            if (!string.IsNullOrEmpty(workingDir))
                pStartInfo.WorkingDirectory = workingDir;
            return System.Diagnostics.Process.Start(pStartInfo);
        }

  

        public void CheckAutoUpdate(Boolean IsShowBox)
        {
            GeneralVariable.NeedUpdate = false;

            if (AutoUpdateForm.CheckAutoUpdate(GeneralVariable.UpdateVersionUrl, GeneralVariable.NowVersion) == false)
            {
                if (GeneralVariable.NeedUpdate == false)
                {
                    if (IsShowBox)
                    {
                       // Bigstatus.ShowText("已是最新版本" + ":" + NowVersion);
                        MessageBox.Show("已是最新版本" + ":" + GeneralVariable.NowVersion);
                    }
                    else
                    {
                      //  Bigstatus.ShowText("已是最新版本" + ":" + NowVersion);
                        //appendSysLog("已是最新版本" + ":" + NowVersion);
                    }

                }
                else
                {
                    //MessageBox.Show("123");

                    //Form1.ThisForm.Text = GeneralVariable.RecordHeader + "     *****(" + GetStr(R.String.發現新版本) + ":" + GeneralVariable.NewVersion + ")*****";


                  
                    NewVersionLabel.Text = "(" + "下載新版本" + "!)";
                  
                }


            }
            else
            {
                //MessageBox.Show("123");

                //Form1.ThisForm.Text = GeneralVariable.RecordHeader + "     *****(" + GetStr(R.String.發現新版本) + ":" + GeneralVariable.NewVersion + ")*****";
                NewVersionLabel.Text = "(" + "下載新版本" + "!)";

            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ai.igcps.com");
        }

        private void StackPanel_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void SystemLog_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }




        public void SaveControl()
        {
          
            MainWindow.ThisForm.Dispatcher.Invoke(DispatcherPriority.Normal,
            new Action(() =>
            {
                JObject SetingJson = new JObject();

                AddControlToJson(SetingJson, (System.Windows.DependencyObject)(this));

                string fileName = GetExePath() + "\\";
                UsefulFunction.CheckAndCreateDir(fileName);
                System.IO.File.WriteAllText(fileName + "StarterSetting.ini", Cjson.Encode(SetingJson));

            }));
        
        }


        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(depObj);
            if (depObj != null)
            {
                foreach (object child in children)
                {

                    // DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    if (child is DependencyObject)
                    {
                        DependencyObject depChild = child as DependencyObject;

                        foreach (T childOfChild in FindLogicalChildren<T>((DependencyObject)child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        public static void AddControlToJson(JObject JsonObject, DependencyObject Parent)
        {
            try
            {
                foreach (var tmpcontrol in FindLogicalChildren<TextBox>(Parent))
                {
                    try
                    {
                        if (tmpcontrol.Name == "SystemLog" )
                        {

                        }
                        else
                        {
                            JsonObject.Add(tmpcontrol.Name, ((TextBox)tmpcontrol).Text);
                        }
                        
                    }
                    catch
                    {
                    }
                }

                //     System.Windows.MessageBox.Show(((TimePicker)tmpcontrol).Text);

                foreach (var tmpcontrol in FindLogicalChildren<CheckBox>(Parent))
                {
                    try
                    {
                        JsonObject.Add(tmpcontrol.Name, ((CheckBox)tmpcontrol).IsChecked);
                    }
                    catch
                    {
                    }
                }
                foreach (var tmpcontrol in FindLogicalChildren<ComboBox>(Parent))
                {
                    if (tmpcontrol.IsEditable == false)
                    {
                        try
                        {
                            JsonObject.Add(tmpcontrol.Name, ((ComboBox)tmpcontrol).SelectedIndex);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                    }
                    try
                    {
                        JsonObject.Add(tmpcontrol.Name + "Str", ((ComboBox)tmpcontrol).Text);
                    }
                    catch
                    {
                    }
                }

               

            }
            catch (Exception E)
            {
                // System.Windows.MessageBox.Show(E.ToString());
            }
        }

        private void RemoteMode_DropDownOpened(object sender, EventArgs e)
        {

        }

        private void ProcessList_DropDownOpened(object sender, EventArgs e)
        {
            String[] EnableProcessList = new String[]{ "notepad" }; //,  "calculator","explorer", "msedge", "firefox", "chrome","line", "cmd"
            Process[] OutProcessList = Process.GetProcesses();

            ProcessList.Items.Clear();
            foreach (String EnableProcessName in EnableProcessList)
            {
                Boolean IsFound = false;
                foreach (Process TmpProcess in OutProcessList)
                {
                    if (TmpProcess.ProcessName.ToLower().Contains(EnableProcessName) || 使用其他進程.IsChecked == true)
                    {
                       
                        ProcessList.Items.Add(TmpProcess.ProcessName + ".exe" + " - " + TmpProcess.Id.ToString());
                        
                    }

                }

               
            
            }
        }

        private void 其他進程路徑瀏覽_Click(object sender, RoutedEventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;




            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
              
                openFileDialog.Filter = ".exe files (*.exe)|*.exe";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = false;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    其他進程路徑.Text = filePath;

                }
            }
        }

        private void ModeList_DropDownOpened(object sender, EventArgs e)
        {
          
        }

        private void ModeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModeList.SelectedIndex < 4)
            {
                SetLinkPanel.Visibility = Visibility.Collapsed;
            }
            else if (ModeList.SelectedIndex == 4)
            {
                SetLinkPanel.Visibility = Visibility.Visible;
            }
        }
    }
}
