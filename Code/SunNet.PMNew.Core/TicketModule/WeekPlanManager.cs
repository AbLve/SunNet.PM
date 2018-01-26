using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.TicketModule
{
    public class WeekPlanManager : BaseMgr
    {
         private IWeekPlanRepository repository;

        #region Constructor

         public WeekPlanManager(
                                  IWeekPlanRepository toRespository
                                 )
        {
            this.repository = toRespository;
        }

        #endregion


         public int Add(WeekPlanEntity entity)
         {
             return  repository.Insert(entity);
         }

         public bool Update(WeekPlanEntity entity, bool isVerify)
         {
             return repository.Update(entity, isVerify);
         }


         public List<WeekPlanEntity> GetList(int userid, DateTime startDate, DateTime endDate, RolesEnum role, int pageNo, int pageSize, out int recordCount)
         {
             return repository.GetList(userid, startDate, endDate, role, pageNo, pageSize, out recordCount);
         }

         public List<WeekPlanEntity> GetWeekDay(int userId)
         {
             return repository.GetWeekDay(userId);
         }

         public WeekPlanEntity GetInfo(int id)
         {
             return repository.Get(id);
         }

         public WeekPlanEntity GetInfo(int id, int userId)
         {
             return repository.Get(id,userId);
         }

         public WeekPlanEntity GetInfo(int userId, DateTime day)
         {
             return repository.Get(userId, day);
         }
    }
}
