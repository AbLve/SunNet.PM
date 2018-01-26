using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReminderEmail.ReminderEnum
{
    /// <summary>
    /// 同步历史记录状态枚举
    /// </summary>
    public enum ReminderStateEnum
    {
        Ini = 0,
        Reminding = 1,
        Succeed = 2,
        Failed = 3,
    }
}
