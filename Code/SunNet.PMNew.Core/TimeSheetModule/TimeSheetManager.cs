using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Framework.Core;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Core.TimeSheetModule
{
    public class TimeSheetManager : BaseMgr
    {
        IEmailSender emailSender;
        ICache<TimeSheetManager> cache;
        ITimeSheetRepository tsRepo;

        public TimeSheetManager(IEmailSender emailSender,
                                ICache<TimeSheetManager> cache,
                                ITimeSheetRepository tsRepo)
        {
            this.emailSender = emailSender;
            this.cache = cache;
            this.tsRepo = tsRepo;
        }

        public int AddTimeSheet(TimeSheetsEntity model)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<TimeSheetsEntity> validator = new AddTimeSheetValidator();
            if (!validator.Validate(model))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
            }
            int id = tsRepo.Insert(model);
            if (id == 0)
            {
                this.AddBrokenRuleMessage();
            }
            if (id == -1)
            {
                this.AddBrokenRuleMessage("Insert Error", "You have write this ticket today,please edit it instead of add new.");
            }
            model.ID = id;
            return id;
        }
        public bool UpdateTimeSheet(TimeSheetsEntity model)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<TimeSheetsEntity> validator = new AddTimeSheetValidator();
            if (!validator.Validate(model))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return false;
            }
            if (!tsRepo.Update(model))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }
        public bool SubmitTimeSheets(DateTime submitDate, int userID)
        {
            this.ClearBrokenRuleMessages();
            int count = tsRepo.UpdateTimesheets(submitDate, userID, true);
            if (count == -1)
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            if (count == 0)
            {
                this.AddBrokenRuleMessage("Error", "Timesheets cannot be left blank.");
                return false;
            }
            return true;
        }
        public bool CancelSubmitTimeSheets(DateTime submitDate, int userID)
        {
            this.ClearBrokenRuleMessages();
            int count = tsRepo.UpdateTimesheets(submitDate, userID, false);
            if (count == -1)
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            if (count == 0)
            {
                this.AddBrokenRuleMessage("Error", "No timesheets cancelled.");
                return false;
            }
            return true;
        }
        public bool DeleteTimeSheet(int id)
        {
            this.ClearBrokenRuleMessages();
            if (!tsRepo.Delete(id))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }



        public List<TimeSheetTicket> SearchTimeSheets(int categoryID, DateTime startDate, DateTime endDate,
            int userID, int projectID, bool addDefaultEmptyModel)
        {
            this.ClearBrokenRuleMessages();
            List<TimeSheetTicket> list = tsRepo.SearchTimeSheets(categoryID, startDate, endDate, userID, projectID, addDefaultEmptyModel);
            if (list == null)
            {
                this.AddBrokenRuleMessage();
            }
            return list;
        }
        private List<DateTime> GetDateListFromIDataReader(DataTable dt)
        {
            List<DateTime> list = new List<DateTime>();
            foreach (DataRow dr in dt.Rows)
            {
                object obj = dr["SheetDate"];
                if (obj != null && obj != DBNull.Value)
                {
                    list.Add(Convert.ToDateTime(obj.ToString()).Date);
                }
            }
            return list;
        }
        private List<DateTime> GetDateListFromIDataReader(DataTable dt, int limitHours)
        {
            List<DateTime> list = new List<DateTime>();
            foreach (DataRow dr in dt.Rows)
            {
                object obj = dr["SheetDate"];
                int hour = 0;
                if (obj != null && obj != DBNull.Value
                    && int.TryParse(dr["Hours"].ToString(), out hour)
                    && hour >= limitHours)
                {
                    list.Add(Convert.ToDateTime(obj.ToString()).Date);
                }
            }
            return list;
        }
        public List<DateTime> GetUnSubmittedTimeSheetDate(int userID, DateTime startDate, DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            DataTable dataUnfinished = tsRepo.GetUnFinishedTimeSheets(userID, startDate, endDate);
            if (dataUnfinished == null)
            {
                this.AddBrokenRuleMessage();
                return null;
            }
            List<DateTime> listUnfinished = GetDateListFromIDataReader(dataUnfinished);
            List<DateTime> result = new List<DateTime>();

            for (int i = 0; i < dataUnfinished.Rows.Count; i++)
            {
                // all timesheets hours count >=8 UnSubmitted
                object obj = dataUnfinished.Rows[i]["Hours"];
                if (obj != null && obj != DBNull.Value)
                {
                    decimal hours = decimal.Parse(obj.ToString());
                    if (hours >= 8)
                    {
                        DateTime date = Convert.ToDateTime(dataUnfinished.Rows[i]["SheetDate"].ToString()).Date;
                        if (!result.Contains(date))
                        {
                            result.Add(date);
                        }
                    }
                }
            }

            return result;
        }
        public List<DateTime> GetUnfinishedTimeSheetDate(int userID, DateTime startDate, DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            startDate = startDate.Date;
            DataTable dataAll = tsRepo.GetAllTimeSheetRecord(userID, startDate, endDate);
            DataTable dataUnfinished = tsRepo.GetUnFinishedTimeSheets(userID, startDate, endDate);
            if (dataAll == null || dataUnfinished == null)
            {
                this.AddBrokenRuleMessage();
                return null;
            }
            List<DateTime> listAll = GetDateListFromIDataReader(dataAll);
            List<DateTime> listUnfinished = GetDateListFromIDataReader(dataUnfinished, 8);
            List<DateTime> result = new List<DateTime>();

            for (DateTime date = startDate.Date; date <= endDate; date = date.AddDays(1))
            {
                // there is no records in database
                if (!listAll.Contains(date))
                {
                    result.Add(date);
                }
            }
            for (int i = 0; i < dataUnfinished.Rows.Count; i++)
            {
                // all timesheets hours count <8
                object obj = dataUnfinished.Rows[i]["Hours"];
                if (obj != null && obj != DBNull.Value)
                {
                    decimal hours = decimal.Parse(obj.ToString());
                    if (hours < 8)
                    {
                        DateTime date = Convert.ToDateTime(dataUnfinished.Rows[i]["SheetDate"].ToString()).Date;
                        if (!result.Contains(date))
                        {
                            result.Add(date);
                        }
                    }
                }
            }
            return result;
        }

        public SearchTimeSheetsResponse QueryTimesheet(SearchTimeSheetsRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchTimeSheetsResponse response = tsRepo.QueryTimesheets(request);
            if (response.IsError == true)
                this.AddBrokenRuleMessage();
            return response;
        }

        public SearchTimeSheetsResponse QueryTimesheetsWithTickets(SearchTimeSheetsRequest request)
        {
            this.ClearBrokenRuleMessages();
            SearchTimeSheetsResponse response = tsRepo.QueryTimesheetsWithTickets(request);
            if (response.IsError == true)
                this.AddBrokenRuleMessage();
            return response;
        }

        public DataTable QueryReportTotalHoursByProject(int projectID, int userID, DateTime startDate
            , DateTime endDate, string orderBy, string orderDirectioin)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = tsRepo.QueryReportTotalHoursByProject(projectID, userID, startDate, endDate, orderBy, orderDirectioin);
            if (dt == null)
                this.AddBrokenRuleMessage();
            return dt;
        }
        public DataTable QueryReportDetailsByProject(int projectID, int userID, DateTime startDate
            , DateTime endDate, string orderBy, string orderDirectioin)
        {
            this.ClearBrokenRuleMessages();
            DataTable dt = tsRepo.QueryReportDetailsByProject(projectID, userID, startDate, endDate, orderBy, orderDirectioin);
            if (dt == null)
                this.AddBrokenRuleMessage();
            return dt;
        }
        public DataTable GetSheetDateByProjectUser(int projectID, int userID, DateTime startDate, DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            if (projectID <= 0 || userID <= 0)
            {
                return new DataTable();
            }
            DataTable dt = tsRepo.GetSheetDateByProjectUser(projectID, userID, startDate, endDate);
            if (dt == null)
            {
                this.AddBrokenRuleMessage();
            }
            return dt;
        }

        public decimal GetTimesheetsHoursByWeek(int userID, DateTime startDate, DateTime endDate)
        {
            this.ClearBrokenRuleMessages();
            decimal hours = tsRepo.GetTimesheetsHoursByWeek(userID, startDate, endDate);
            if (hours == 0)
                this.AddBrokenRuleMessage();
            return hours;
        }

        #region 报表部分
        public DataTable ReportConsumingComparison(SearchTimeSheetsRequest request, out int totalRows)
        {
            DataTable response = tsRepo.ReportConsumingComparison(request, out totalRows);
            return response;
        }
        public DataTable ReportComparisonAnalysis(SearchTimeSheetsRequest request, out int totalRows)
        {
            DataTable response = tsRepo.ReportComparisonAnalysis(request, out totalRows);
            return response;
        }
        public List<TimeSheetTicket> ComparisonExport(SearchTimeSheetsRequest request)
        {
            List<TimeSheetTicket> response = tsRepo.ComparisonExport(request);
            return response;
        }

        #endregion

        #region Events

        public bool DeleteByEventId(int EventID, DateTime Day)
        {
            return tsRepo.DeleteByEventId(EventID, Day);
        }
        public bool DeleteByUserAndDate(int UserID, DateTime Day)
        {
            return tsRepo.DeleteByUserAndDate(UserID, Day);
        }
        public TimeSheetsEntity GetByEventId(int EventID, DateTime Day)
        {
            return tsRepo.GetByEventId(EventID, Day);
        }
        public TimeSheetsEntity GetByUserId(int UserID, DateTime Day)
        {
            return tsRepo.GetByUserId(UserID, Day);
        } 

        #endregion

        public List<CheckTimesheetEntity> GetTimesheetList(DateTime startDate, DateTime endDate)
        {
            return tsRepo.GetTimesheetList(startDate, endDate);
        }
        public List<TimeSheetTicket> GetTimesheet(int invoiceID)
        {
            return tsRepo.GetTimesheet(invoiceID);
        }
        public List<TimeSheetTicket> GetTimesheetByProposalId(int proposalTrackerId)
        {
            return tsRepo.GetTimesheetByProposalId(proposalTrackerId);
        }
    }
}
