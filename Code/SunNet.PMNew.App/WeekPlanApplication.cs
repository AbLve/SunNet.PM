using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.TicketModule;
using StructureMap;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.App
{
    public class WeekPlanApplication : BaseApp
    {

        private WeekPlanManager mgr;

        public WeekPlanApplication()
        {
            mgr = new WeekPlanManager(
                                ObjectFactory.GetInstance<IWeekPlanRepository>());
        }

        public int  Add(WeekPlanEntity entity)
        {
            return mgr.Add(entity);
        }

        public bool Update(WeekPlanEntity entity, bool isVerify)
        {
            return mgr.Update(entity, isVerify);
        }

        public List<WeekPlanEntity> GetList(int userId, DateTime startDate, DateTime endDate, RolesEnum role, int pageNo, int pageSize, out int recordCount)
        {
            return mgr.GetList(userId, startDate, endDate,role, pageNo, pageSize, out recordCount);
        }

        public List<WeekPlanEntity> GetWeekDay(int userId)
        {
            return mgr.GetWeekDay(userId);
        }

        public WeekPlanEntity GetInfo(int id)
        {
            return mgr.GetInfo(id);
        }
       
        public WeekPlanEntity GetInfo(int userId, DateTime day)
        {
            return mgr.GetInfo(userId, day);
        }

        public WeekPlanEntity GetInfo(int id, int userId)
        {
            return mgr.GetInfo(id, userId);
        }
    }
}
