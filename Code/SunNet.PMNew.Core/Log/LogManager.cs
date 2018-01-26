using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.LogModel;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Core.Log.Validators;

namespace SunNet.PMNew.Core.Log
{
    public class LogManager : BaseMgr
    {
        private ILogRepository logRepository;
        public LogManager(ILogRepository logRepository)
        {
            this.logRepository = logRepository;

        }

        public int Write(LogEntity logEntity)
        {
            BaseValidator<LogEntity> validator = new AddLogValidator();
            if (!validator.Validate(logEntity))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            this.ClearBrokenRuleMessages();
            int id;
            id = logRepository.Insert(logEntity);
            if (id == 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            return id;
        }
    }
}
