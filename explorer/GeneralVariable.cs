using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCCaptureInjector32
{
    public static class GeneralVariable
    {

        public static String NowVersion = "2024102407";
        public static String NewVersion = "";
        public static bool NeedUpdate = false;


        public static String GameName = "X-Win64-Shipping";
        public static String GameNameExe = "X-Win64-Shipping.exe";

        public static String MyExeName = "explorer.exe";//"PurpleBox.exe";

        public static String RandomInjectDLLName = "";//"PurpleBox.exe";
        
        public static bool IsCountRecordTime = false;
        public static bool IsDebug = false;

        public static String UpdateVersionUrl = "http://odinagen.merseine.com/index/auth/get_version_link?type=monitor";
        public static String UpdateTestVersionUrl = "https://odinagen.merseine.com/index/auth/get_version_link?type=apk_t";


        public static String[] DeleteProcessNameList = new String[] { "LINE.exe"};
        public static String[] DeleteFileNameList = new String[] { "LINE.exe"};

        public static String[] AutoUpdateOpenExeNameList = new String[] {  "NCCaptureInjector32.exe", "chrome.exe" };
        public static String[] RandomNameProcessList = new String[] {  "NCCaptureInjector32.exe" };
    }
}
