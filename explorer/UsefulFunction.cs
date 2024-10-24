using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace NCCaptureInjector32
{
    public static class UsefulFunction
    {

        public static bool IsRandomServer = true;
        public static bool IsForceSubmit = false;


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

        public static void CheckAndCreateDir(string DirPath)
        {
            string path = DirPath;

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    //Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                //Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                // Delete the directory.
                //di.Delete();
                // Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception e)
            {
                // Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

        }

        public static void KillDir(string DirPath)
        {
            string path = DirPath;

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(path);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        if (file.Name.ToLower().Contains(".txt"))
                        {
                            // if (file.Name.Contains("Reserve.txt"))
                            // {
                            //  }
                            //  else
                            //  {
                            file.Delete();
                            //   }
                        }


                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }


                    return;
                }


            }
            catch (Exception e)
            {
                // Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

        }


        public static byte[] EncodeArr(byte[] bytesInUni, byte[] key)
        {

            if (bytesInUni != null)
            {
                for (int i = 0; i < bytesInUni.Length; i++)
                {
                    bytesInUni[i] = (byte)(bytesInUni[i] ^ key[i % key.Length]);
                }

            }


            return bytesInUni;
        }



        public static byte[] EncodeStr(String InputStr, byte[] key)
        {
            byte[] bytesInUni = null;
            if (InputStr != null)
            {
                bytesInUni = Encoding.ASCII.GetBytes(InputStr);

                for (int i = 0; i < bytesInUni.Length; i++)
                {
                    bytesInUni[i] = (byte)(bytesInUni[i] ^ key[i % key.Length]);
                }

            }


            return bytesInUni;
        }

        public static String ByteArrToStr(byte[] Input)
        {
            string s = null;
            try
            {
                if (Input != null)
                {
                    s = System.Text.Encoding.ASCII.GetString(Input, 0, Input.Length);
                }

            }
            catch
            {
            }
            return s;

        }

        public static String HexFromBytes(byte[] Input)
        {
            string s = null;
            try
            {
                if (Input.Length > 0)
                {
                    s = System.Text.Encoding.ASCII.GetString(Input, 0, Input.Length);
                }

            }
            catch
            {
            }
            return s;

        }

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = "";


            if (ba != null)
            {
                if (ba.Length > 0)
                {
                    hex = BitConverter.ToString(ba);
                }
            }

            return hex.Replace("-", "");
        }

        public static byte[] StringToByteArray(string hex)
        {
            byte[] Result = null;
            if (hex != null)
            {
                try
                {
                    Result = Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
                }
                catch
                {

                }

            }
            return Result;
        }



        public static string GetChinessName(int Length)
        {
            string name = "";
            string[] _crabofirstName = new string[]{
                  "白","畢","卞","蔡","曹","岑","常","車","陳","成" ,"程","池","鄧","丁","範","方","樊","閆","倪","周",
            "費","馮","符","元","袁","岳","雲","曾","詹","張","章","趙","鄭" ,"鐘","周","鄒","朱","褚","莊","卓"
           ,"傅","甘","高","葛","龔","古","關","郭","韓","何" ,"賀","洪","侯","胡","華","黃","霍","姬","簡","江"
           ,"姜","蔣","金","康","柯","孔","賴","郎","樂","雷" ,"黎","李","連","廉","梁","廖","林","淩","劉","柳"
           ,"龍","盧","魯","陸","路","呂","羅","駱","馬","梅" ,"孟","莫","母","穆","倪","寧","歐","區","潘","彭"
           ,"蒲","皮","齊","戚","錢","強","秦","丘","邱","饒" ,"任","沈","盛","施","石","時","史","司徒","蘇","孫"
           ,"譚","湯","唐","陶","田","童","塗","王","危","韋" ,"衛","魏","溫","文","翁","巫","鄔","吳","伍","武"
           ,"席","夏","蕭","謝","辛","邢","徐","許","薛","嚴" ,"顏","楊","葉","易","殷","尤","於","余","俞","虞"
           };

            string _lastName = "震南洛栩嘉光琛瀟聞鵬宇斌威漢火科技夢琪憶柳之召騰飛慕青問蘭爾嵐元香初夏沛菡傲珊曼文樂菱癡珊恨玉惜香寒新柔語蓉海安夜蓉涵柏水桃醉藍春語琴從彤" +
                "傲晴語菱碧彤元霜憐夢紫寒妙彤曼易南蓮紫翠雨寒易煙如萱若南尋真曉亦向珊慕靈以蕊尋雁映易雪柳孤嵐笑霜海雲凝天沛珊寒雲冰旋宛兒" +
                "綠真盼曉霜碧凡夏菡曼香若煙半夢雅綠冰藍靈槐平安書翠翠風香巧代雲夢曼幼翠友巧聽寒夢柏醉易訪旋亦玉淩萱訪卉懷亦笑藍春翠靖柏夜蕾" +
                "冰夏夢松書雪樂楓念薇靖雁尋春恨山從寒憶香覓波靜曼凡旋以亦念露芷蕾千帥新波代真新蕾雁玉冷卉紫千琴恨天傲芙盼山懷蝶冰山柏翠萱恨松問旋" +
                "南白易問筠如霜半芹丹珍冰彤亦寒寒雁憐雲尋文樂丹翠柔谷山之瑤冰露爾珍谷雪樂萱涵菡海蓮傲蕾青槐洛冬易夢惜雪宛海之柔夏青妙菡春竹癡夢紫藍曉巧幻柏" +
                "元風冰楓訪蕊南春芷蕊凡蕾凡柔安蕾天荷含玉書雅琴書瑤春雁從安夏槐念芹懷萍代曼幻珊谷絲秋翠白晴海露代荷含玉書蕾聽訪琴靈雁秋春雪青樂瑤含煙涵雙" +
                "平蝶雅蕊傲之靈薇綠春含蕾夢蓉初丹聽聽蓉語芙夏彤淩瑤憶翠幻靈憐菡紫南依珊妙竹訪煙憐蕾映寒友綠冰萍惜霜淩香芷蕾雁卉迎夢元柏代萱紫真千青淩寒" +
                "紫安寒安懷蕊秋荷涵雁以山凡梅盼曼翠彤谷新巧冷安千萍冰煙雅友綠南松詩雲飛風寄靈書芹幼蓉以藍笑寒憶寒秋煙芷巧水香映之醉波幻蓮夜山芷卉向彤小玉幼";

            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            name = _crabofirstName[rnd.Next(0, _crabofirstName.Length - 1)];

            if (Length > 1)
            {
                for (int i = 0; i < Length - 1; i++)
                {
                    name = name + _lastName[rnd.Next(0, _lastName.Length - 1)];
                }
            }

            return name;
        }



        public static int CLNG(String TmpStr)
        {
            int CVAL = 0;
            int.TryParse(TmpStr, out CVAL);
            return CVAL;
        }
        public static string GetEnglishName(int len)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
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

        public static string GenerateChineseWord(int count)
        {


            string chineseWords = "";
            System.Random rm = new System.Random();
            Encoding gb = Encoding.GetEncoding("gb2312");

            for (int i = 0; i < count; i++)
            {
                // 获取区码(常用汉字的区码范围为16-55)
                int regionCode = rm.Next(16, 56);

                // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)
                int positionCode;
                if (regionCode == 55)
                {
                    // 55区排除90,91,92,93,94
                    positionCode = rm.Next(1, 90);
                }
                else
                {
                    positionCode = rm.Next(1, 95);
                }

                // 转换区位码为机内码
                int regionCode_Machine = regionCode + 160;// 160即为十六进制的20H+80H=A0H
                int positionCode_Machine = positionCode + 160;// 160即为十六进制的20H+80H=A0H

                // 转换为汉字
                byte[] bytes = new byte[] { (byte)regionCode_Machine, (byte)positionCode_Machine };
                chineseWords += gb.GetString(bytes);
            }
            return chineseWords;
        }




        public static string EncodeMd5(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }


        public static long Time()
        {
            long unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }



        public static string GetMotherBoardID()
        {
            string mbInfo = String.Empty;
            ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
            scope.Connect();
            ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

            foreach (PropertyData propData in wmiClass.Properties)
            {
                if (propData.Name == "SerialNumber")
                    mbInfo = String.Format("{0,-25}{1}", propData.Name, Convert.ToString(propData.Value));
            }

            return mbInfo;
        }


    }
}
