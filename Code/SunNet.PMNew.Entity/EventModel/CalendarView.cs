using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public class CalendarView
    {
        public int ID { get; set; }
        /// <summary>
        /// Calendar 中使用
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 弹出层使用
        /// </summary>
        public string Name { get; set; }
        public string Icon { get; set; }
        public string date { get; set; }

        /// <summary>
        /// 创建者与当前用户ID 不相同时，表示该Event是他人邀请的
        /// </summary>
        public int CreatedAt { get; set; }
        public bool Invited { get; set; }
        public int InviteStatus { get; set; }
        public string FullName { get; set; }
        public bool IsEdit { get; set; }

        public int Times { get; set; }

    }
}
