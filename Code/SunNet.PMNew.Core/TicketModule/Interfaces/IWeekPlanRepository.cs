using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core.Repository;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.TicketModule
{
    public interface IWeekPlanRepository : IRepository<WeekPlanEntity>
    {
        List<WeekPlanEntity> GetList(int userId, DateTime startDate, DateTime endDate, RolesEnum role, int pageNo, int pageSize, out int recordCount);

        WeekPlanEntity Get(int userid, DateTime day);

        WeekPlanEntity Get(int id, int userId);

        bool Update(WeekPlanEntity entity, bool isVerify);

        /// <summary>
        /// 返回的对象只有ID　与WeekDay
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<WeekPlanEntity> GetWeekDay(int userId);
    }
}
