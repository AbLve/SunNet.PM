using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Framework.Utils.Helpers
{
    public class JSONHelper
    {
        public T GetEntity<T>(string jsonStr)
        {
            var ds = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr));
            var obj = (T)ds.ReadObject(ms);
            ms.Close();
            return obj;
        }
        public string GetJson<T>(T obj)
        {

            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    json.WriteObject(stream, obj);
                    string jsons = Encoding.UTF8.GetString(stream.ToArray());
                    string p = @"\\/Date\((\d+)(\+|\-)\d+\)\\/";
                    MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                    Regex reg = new Regex(p);
                    jsons = reg.Replace(jsons, matchEvaluator);
                    return jsons;
                }
                catch
                {
                    return "[]";
                }

            }
        }
        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("MM/dd/yyyy HH:mm:ss");
            return result;
        }
    }
}
