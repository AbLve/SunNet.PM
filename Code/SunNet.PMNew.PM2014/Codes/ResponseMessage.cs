using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SunNet.PMNew.Web.Do;

namespace SunNet.PMNew.PM2014.Codes
{
    public class ResponseMessage
    {
        protected static string SuccessMessage = "Operation successful.";
        protected static string FailMessage = "Operation failed.";
        public static string GetResponse(bool success, string msg = "", int value = 0)
        {
            var m = new ResponseMessage();
            m.Success = success;
            if (m.Success)
                m.MessageContent = SuccessMessage;
            else if (!string.IsNullOrEmpty(msg))
                m.MessageContent = msg;
            else
                m.MessageContent = FailMessage;
            m.Key = value;
            return JsonConvert.SerializeObject(m);
        }
        public ResponseMessage() { }
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
        [JsonProperty(PropertyName = "msg")]
        public string MessageContent { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Key { get; set; }
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, DoBase.DateConverter);
        }
    }
}