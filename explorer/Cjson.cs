using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCCaptureInjector32
{

    public static class Cjson
    {
        public static String Encode(object TmpObject)
        {
            string Result = null;
            try
            {
                Result = JsonConvert.SerializeObject(TmpObject);
            }
            catch
            {
                Console.WriteLine("Cjson Encode Error");
            }

            return Result;
        }

        public static JObject Decode(String TmpJson)
        {
            JObject Result = null;
            try
            {
                Result = (JObject)JsonConvert.DeserializeObject(TmpJson);
            }
            catch
            {
                Console.WriteLine("Cjson Encode Error");
            }

            return Result;
        }


        public static JArray DecodeArray(String TmpJson)
        {
            JArray Result = null;
            try
            {
                Result = (JArray)JsonConvert.DeserializeObject(TmpJson);
            }
            catch
            {
                Console.WriteLine("Cjson DecodeArray Error");
            }

            return Result;
        }

        public static String GetJsonStr(JObject TmpObject, String Name)
        {
            String Result = "";
            try
            {
                Result = (String)TmpObject.SelectToken(Name);
            }
            catch
            {

            }
            return Result;
        }



        public static JObject GetJsonObject(JObject TmpObject, String Name)
        {
            JObject Result = null;
            try
            {
                Result = (JObject)TmpObject.SelectToken(Name);
            }
            catch
            {

            }
            return Result;
        }

        public static int GetJsonInt(JObject TmpObject, String Name)
        {
            int Result = 0;
            try
            {
                Result = (int)TmpObject.SelectToken(Name);
            }
            catch
            {

            }
            return Result;
        }


        public static Boolean GetJsonBoolean(JObject TmpObject, String Name)
        {
            Boolean Result = false;
            try
            {
                Result = (Boolean)TmpObject.SelectToken(Name);
            }
            catch
            {

            }
            return Result;
        }





    }
}
