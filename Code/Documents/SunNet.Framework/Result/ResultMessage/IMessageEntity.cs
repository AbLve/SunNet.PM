using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace SF.Framework.ResultMessages.ResultMessage
{
    public interface IMessageEntity
    {
        string Key
        {
            get;
        }
        
        string Message
        {
            get;
        }

        [ScriptIgnore]
        object Data { get; set; }
    }
}
