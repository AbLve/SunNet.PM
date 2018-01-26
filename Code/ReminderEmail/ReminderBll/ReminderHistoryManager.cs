using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReminderEmail.ReminderDal;
using SF.Framework;
using SF.Framework.Helpers;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using ReminderEmail.ReminderModel;

namespace ReminderEmail.ReminderBll
{
    public class ReminderHistoryManager
    {
        private ReminderHistoryDal dal=new ReminderHistoryDal();

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>是否成功</returns>
        public bool Create(ReminderHistoryModel model)
        {
            return dal.Create(model);
        }

        /// <summary>
        /// 插入一条数据，并返回插入的实体
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns>插入后的实体</returns>
        public ReminderHistoryModel CreateAndReturn(ReminderHistoryModel model)
        {
            return dal.CreateAndReturn(model);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public bool UpdateModel(ReminderHistoryModel model)
        {
           int result = dal.UpdateModel(model);

            if (result>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 查询指定日期是否执行过
        /// </summary>
        public bool GetLastModelByRunDate(DateTime runDate)
        {
            runDate = runDate.Date;
            return dal.GetLastModelByRunDate(runDate);
        }

        /// <summary>
        /// 是否存在正在执行的数据
        /// </summary>
        public bool ExistsRunning()
        {
            return dal.ExistsRunning();
        }
    }
}
