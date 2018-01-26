using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReminderEmail.ReminderBll;
using ReminderEmail.ReminderEnum;
using ReminderEmail.ReminderModel;
using SF.Framework;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;

namespace ReminderEmail
{
    /// <summary>
    /// 提醒管理类
    /// </summary>
    public class ReminderManager
    {
        private TicketsApplication _ticketsApp = null;
        private UserApplication _usersApp = null;
        private ProjectApplication _projectApp = null;
        private string connStr = "PM";

        private ReminderHistoryManager _historyManager=null;

        public ReminderManager()
        {
            BootStrap.Config();
            _ticketsApp = new TicketsApplication();
            _usersApp = new UserApplication();
            _projectApp = new ProjectApplication();
            _historyManager=new ReminderHistoryManager();
        }


        public bool ReminderAll()
        {
            if (IsCanRun())
            {
                //插入一条新同步记录
                ReminderHistoryModel historyModel = IniReminderHistory();


                //所有用户提醒
                var users = _usersApp.GetActiveUserList();
                foreach (var user in users)
                {
                    //每用户提醒
                    ReminderByUser(user);
                }

                //更新同步记录
                historyModel.RunEndTime = DateTime.Now;
                historyModel.State = ReminderStateEnum.Succeed;
                _historyManager.UpdateModel(historyModel);

            }

            return true;
        }

        private bool ReminderByUser(UsersEntity user)
        {
            ReminderDal.ReminderDal dal=new ReminderDal.ReminderDal();
            var tickes = dal.GetReminderTickets(user, GetReminderStartDate());
            if (tickes.Count>0)
            {
                //发邮件
                SendEmailManager.SendReminderEmail(user, tickes);
            }

            return false;
        }

        private bool IsCanRun()
        {
            //执行时间段
            if (DateTime.Now.Hour != ReminderConfig.RunTimeSpan())
            {
                Console.WriteLine("Not in the period of execution");
                return false;
            }

            //是否存在正在执行的程序
            if (_historyManager.ExistsRunning())
            {
                Console.WriteLine("A reminder is being carried out and will continue later");
                return false;
            }

            //当日是否执行过
            if (_historyManager.GetLastModelByRunDate(DateTime.Now))
            {
                Console.WriteLine("Today has been reminded, no more reminding.");
                return false;
            }

            return true;
        }

        private ReminderHistoryModel IniReminderHistory()
        {
            ReminderHistoryModel historyModel = new ReminderHistoryModel();
            historyModel.Id = 0;
            historyModel.RunStartTime=DateTime.Now;
            historyModel.RunEndTime = null;
            historyModel.RunDate=DateTime.Now;
            historyModel.DataStartTime=new DateTime(1998,1,1);
            historyModel.DataEndTime=DateTime.Now.Date.AddDays(-3);
            historyModel.State=ReminderStateEnum.Reminding;
            historyModel.CreateTime=DateTime.Now;

            return _historyManager.CreateAndReturn(historyModel);
        }

        /// <summary>
        /// 提醒开始日期
        /// （周六和周日除外）
        /// </summary>
        private DateTime GetReminderStartDate()
        {
            DateTime startDate = DateTime.Now.Date;

            int dateCount = 0;

            while (dateCount < 3)
            {
                startDate = startDate.AddDays(-1);
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    dateCount += 1;
                }
            }

            return startDate;
        }
    }
}
