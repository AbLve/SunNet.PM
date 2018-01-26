using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReminderEmail.ReminderEnum;

namespace ReminderEmail.ReminderModel
{
    /// <summary>
    /// 提醒历史
    /// </summary>
    public class ReminderHistoryModel
    {
        public ReminderHistoryModel()
        {
            Id = 0;
            RunStartTime = null;
            RunEndTime = null;
            DataStartTime = null;
            DataEndTime = null;
            State=ReminderStateEnum.Ini;
            CreateTime =DateTime.Now;
        }

        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 提醒执行开始时间
        /// </summary>
        public DateTime? RunStartTime { get; set; }

        /// <summary>
        /// 提醒执行结束时间
        /// </summary>
        public DateTime? RunEndTime { get; set; }

        /// <summary>
        /// 提醒执行纯日期
        /// </summary>
        public DateTime RunDate { get; set; }

        /// <summary>
        /// 数据开始时间
        /// </summary>
        public DateTime? DataStartTime { get; set; }

        /// <summary>
        /// 数据结束时间
        /// </summary>
        public DateTime? DataEndTime { get; set; }

        /// <summary>
        /// 提醒总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 提醒成功数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 提醒失败数量
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// 提醒错误或异常数量
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 状态
        /// (0 初始值 1更新中 2更新完成 3更新错误)
        /// </summary>
        public ReminderStateEnum State { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
