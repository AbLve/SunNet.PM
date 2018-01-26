using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Framework.Core.Repository;
using System.Data;

namespace SunNet.PMNew.Core.TimeSheetModule
{
    public interface ITimeSheetRepository : IRepository<TimeSheetsEntity>
    {
        int UpdateTimesheets(DateTime submitDate, int userID, bool submitted);
        List<TimeSheetTicket> SearchTimeSheets(int categoryID, DateTime startDate,
            DateTime endDate, int userID, int projectID, bool addDefaultEmptyModel);
        DataTable GetAllTimeSheetRecord(int userID, DateTime startDate, DateTime endDate);
        DataTable GetUnFinishedTimeSheets(int userID, DateTime startDate, DateTime endDate);
        SearchTimeSheetsResponse QueryTimesheets(SearchTimeSheetsRequest request);
        SearchTimeSheetsResponse QueryTimesheetsWithTickets(SearchTimeSheetsRequest request);
        DataTable QueryReportTotalHoursByProject(int projectID, int userID, DateTime startDate,
            DateTime endDate, string orderBy, string orderDirectioin);
        DataTable QueryReportDetailsByProject(int projectID, int userID, DateTime startDate,
            DateTime endDate, string orderBy, string orderDirectioin);
        DataTable GetSheetDateByProjectUser(int projectID, int userID, DateTime startDate,
            DateTime endDate);
        decimal GetTimesheetsHoursByWeek(int userID, DateTime startDate, DateTime endDate);

        #region Events

        bool DeleteByEventId(int EventID, DateTime Day);
        bool DeleteByUserAndDate(int UserID, DateTime Day);
        TimeSheetsEntity GetByEventId(int EventID, DateTime Day);
        TimeSheetsEntity GetByUserId(int UserID, DateTime Day);

        #endregion

        #region 报表

        DataTable ReportConsumingComparison(SearchTimeSheetsRequest request, out int totalRows);
        DataTable ReportComparisonAnalysis(SearchTimeSheetsRequest request, out int totalRows);
        List<TimeSheetTicket> ComparisonExport(SearchTimeSheetsRequest request);

        #endregion

        List<CheckTimesheetEntity> GetTimesheetList(DateTime startDate, DateTime endDate);
        List<TimeSheetTicket> GetTimesheet(int invoiceID);
        List<TimeSheetTicket> GetTimesheetByProposalId(int proposalTrackerId);
    }
}
