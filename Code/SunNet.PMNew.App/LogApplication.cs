using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.Log;
using StructureMap;
using SunNet.PMNew.Entity.LogModel;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.App
{
    public class LogApplication:BaseApp
    {
        LogManager logManager;

        public LogApplication()
        {
            logManager = new LogManager(ObjectFactory.GetInstance<ILogRepository>());

        }
        public int Write(LogEntity logEntity)
        {
            this.ClearBrokenRuleMessages();
            int id = logManager.Write(logEntity);
            this.AddBrokenRuleMessages(logManager.BrokenRuleMessages);
            return id;
        }
    }
}
