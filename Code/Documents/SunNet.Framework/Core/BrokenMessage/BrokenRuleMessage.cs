using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using SF.Framework.ResultMessages.ResultMessage;

namespace SF.Framework.Core.BrokenMessage
{
    public class BrokenRuleMessage : IMessageEntity
    {
        public BrokenRuleMessage(string key, string message)
        {
            this.key = key;
            this.message = message;
            this.Data = null;
        }

        private string key;

        public string Key
        {
            get { return key; }
        }
        private string message;

        public string Message
        {
            get { return message; }
        }

        [ScriptIgnore]
        public object Data { get; set; }

    }
}
