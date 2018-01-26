using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.EventModel
{
    public class EventsView : EventEntity
    {
        public RepeatType Repeat { get; set; }

        public EndType End { get; set; }

        public int Times { get; set; }

        public DateTime EndDate { get; set; }

        /// <summary>
        /// 分配的组ID
        /// </summary>
        public string RoleIDs { get; set; }

        /// <summary>
        /// 将RoleIds分解为 List
        /// </summary>
        public List<int> RoleList { get; set; }

        /// <summary>
        /// 特邀的用户ID
        /// </summary>
        public string UserIds { get; set; }

        public List<int> InviteFriends { get; set; }

        /// <summary>
        /// 只有controller 中有，需要从web层传进来
        /// </summary>
        public int FamilyID { get; set; }
    }
}
