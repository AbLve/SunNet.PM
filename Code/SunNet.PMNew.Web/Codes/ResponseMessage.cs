using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Web.Codes
{
    public class ResponseMessage
    {
        public static string SuccessMessage = "Operation successful.";
        public static string FailMessage = "Operation failed.";
        public static string GetResponse(bool success, string msg, int value)
        {
            ResponseMessage m = new ResponseMessage();
            m.Success = success;
            m.MessageContent = msg;
            m.Value = value.ToString();

            return UtilFactory.Helpers.JSONHelper.GetJson<ResponseMessage>(m);
        }
        public ResponseMessage() { }
        public bool Success { get; set; }
        public string MessageContent { get; set; }
        public string Value { get; set; }
        public int TimeSheetID { get; set; }
    }
}
