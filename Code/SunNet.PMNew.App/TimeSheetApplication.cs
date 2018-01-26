using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.TimeSheetModule;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.TimeSheetModel;
using System.Data;

namespace SunNet.PMNew.App
{
    public class TimeSheetApplication : BaseApp
    {
        TimeSheetManager mgr;
        public TimeSheetApplication()
        {
            mgr = new TimeSheetManager(ObjectFactory.GetInstance<IEmailSender>(),
                                    ObjectFactory.GetInstance<ICache<TimeSheetManager>>(),
                                    ObjectFactory.GetInstance<ITimeSheetRepository>());
        }

        public int AddTimeSheet(TimeSheetsEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AddTimeSheet(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public bool UpdateTimeSheet(TimeSheetsEntity model)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.UpdateTimeSheet(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }
        public decimal TotalWeeklyHours(DateTime submitDate, int userID)
        {
            DateTime weekDay = submitDate.Date.AddDays(7);
            int weekOfDay = (int)weekDay.DayOfWeek;
            weekOfDay = weekOfDay == 0 ? 7 : weekOfDay;
            weekDay = weekDay.AddDays(-weekOfDay + 1).AddSeconds(-1);
            DateTime oneDay = weekDay.AddDays(-6).Date;
            TimeSheetApplication tsApp = new TimeSheetApplication();
            string interval = System.Configuration.ConfigurationManager.AppSettings["TimesheetHoursUserID"];
            string[] list = interval.Split(',');
            decimal hours = 0;
            for (var i = 0; i < list.Length; i++)
            {
                if (userID == int.Parse(list[i]))
                {
                    hours = tsApp.GetTimesheetsHoursByWeek(userID, oneDay, weekDay);
                    break;
                }
            }
            return hours;
        }
        public bool SubmitTimeSheets(DateTime submitDate, int userID)
        {
            this.ClearBrokenRuleMessages();
            decimal hours = TotalWeeklyHours(submitDate, userID);
            if (hours > 40)
            {
                this.AddBrokenRuleMessage("Error", "Your weekly work time is over 40 hours, please contact your manager immediately.");
                return false;
            }
            bool result = mgr.SubmitTimeSheets(submitDate, userID);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public bool CancelSubmitTimeSheets(DateTime submitDate, int userID)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.CancelSubmitTimeSheets(submitDate, userID);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public bool DeleteTimeSheet(int id)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.DeleteTimeSheet(id);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public List<TimeSheetTicket> SearchTimeSheets(int categoryID, DateTime startDate, DateTime endDate, int userID, int projectID, bool addDefaultEmptyModel)
        {
            this.ClearBrokenRuleMessages();
            List<TimeSheetTicket> list = mgr.SearchTimeSheets(categoryID, startDate, endDate, userID, projectID, addDefaultEmptyModel);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public List<DateTime> GetUnSubmittedTimeSheetDate(int userID, DateTime startDate, DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            List<DateTime> list = mgr.GetUnSubmittedTimeSheetDate(userID, startDate, endDate);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public List<DateTime> GetUnfinishedTimeSheetDate(int userID, DateTime startDate, DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            List<DateTime> list = mgr.GetUnfinishedTimeSheetDate(userID, startDate, endDate);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public SearchTimeSheetsResponse QueryTimesheet(SearchTimeSheetsRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchTimeSheetsResponse response = mgr.QueryTimesheet(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }

        public SearchTimeSheetsResponse QueryTimesheetsWithTickets(SearchTimeSheetsRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchTimeSheetsResponse response = mgr.QueryTimesheetsWithTickets(request);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return response;
        }

        public DataTable QueryReportTotalHoursByProject(int projectID, int userID, DateTime startDate
           , DateTime endDate, string orderBy, string orderDirectioin)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = mgr.QueryReportTotalHoursByProject(projectID, userID, startDate, endDate, orderBy, orderDirectioin);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return dt;
        }

        public DataTable QueryReportDetailsByProject(int projectID, int userID, DateTime startDate
          , DateTime endDate, string orderBy, string orderDirectioin)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = mgr.QueryReportDetailsByProject(projectID, userID, startDate, endDate, orderBy, orderDirectioin);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return dt;
        }

        public DataTable GetSheetDateByProjectUser(int projectID, int userID, DateTime startDate,
           DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = mgr.GetSheetDateByProjectUser(projectID, userID, startDate, endDate);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return dt;
        }
        public decimal GetTimesheetsHoursByWeek(int userID, DateTime startDate, DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            decimal hours = mgr.GetTimesheetsHoursByWeek(userID, startDate, endDate);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return hours;
        }

        #region 报表

        public DataTable ReportConsumingComparison(SearchTimeSheetsRequest request, out int totalRows)
        {
            DataTable response = mgr.ReportConsumingComparison(request, out totalRows);
            return response;
        }

        public DataTable ReportComparisonAnalysis(SearchTimeSheetsRequest request, out int totalRows)
        {
            DataTable response = mgr.ReportComparisonAnalysis(request, out totalRows);
            return response;
        }

        public List<TimeSheetTicket> ComparisonExport(SearchTimeSheetsRequest request)
        {
            List<TimeSheetTicket> response = mgr.ComparisonExport(request);
            return response;
        }

        #endregion

        #region Events

        public bool DeleteByEventId(int EventID, DateTime Day)
        {
            return mgr.DeleteByEventId(EventID, Day);
        }

        public bool DeleteByUserAndDate(int UserID, DateTime Day)
        {
            return mgr.DeleteByUserAndDate(UserID, Day);
        }

        public TimeSheetsEntity GetByEventId(int EventID, DateTime Day)
        {
            return mgr.GetByEventId(EventID, Day);
        }

        public TimeSheetsEntity GetByUserId(int UserID, DateTime Day)
        {
            return mgr.GetByUserId(UserID, Day);
        }

        #endregion

        public List<CheckTimesheetEntity> GetTimesheetList(DateTime startDate, DateTime endDate)
        {
            return mgr.GetTimesheetList(startDate, endDate);
        }
        public List<TimeSheetTicket> GetTimesheet(int invoiceID)
        {
            return mgr.GetTimesheet(invoiceID);
        }
        public List<TimeSheetTicket> GetTimesheetByProposalId(int proposalTrackerId)
        {
            return mgr.GetTimesheetByProposalId(proposalTrackerId);
        }
    }
}
