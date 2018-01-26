using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Entity.TimeSheetModel
{
    public class TimeSheetFactory
    {
        public static TimeSheetsEntity CreateTimeSheet(int createUserID, ISystemDateTime datetimeProvider)
        {
            TimeSheetsEntity model = new TimeSheetsEntity();

            model.ID = 0;
            model.CreatedBy = createUserID;
            model.CreatedOn = datetimeProvider.Now;
            model.ModifiedBy = createUserID;
            model.ModifiedOn = datetimeProvider.Now;

            model.Description = string.Empty;
            model.Hours = 0;
            model.IsMeeting = false;
            model.IsSubmitted = false;
            model.Percentage = 0;
            model.ProjectID = 0;
            model.SheetDate = datetimeProvider.Now;
            model.TicketID = 0;
            model.UserID = 0;

            return model;
        }
        public static ExpandTimeSheetsEntity CreateExpandTimeSheet(int createUserID, ISystemDateTime datetimeProvider)
        {
            ExpandTimeSheetsEntity model = new ExpandTimeSheetsEntity();

            model.ID = 0;
            model.CreatedBy = createUserID;
            model.CreatedOn = datetimeProvider.Now;
            model.ModifiedBy = createUserID;
            model.ModifiedOn = datetimeProvider.Now;

            model.Description = string.Empty;
            model.Hours = 0;
            model.IsMeeting = false;
            model.IsSubmitted = false;
            model.Percentage = 0;
            model.ProjectID = 0;
            model.SheetDate = datetimeProvider.Now;
            model.TicketID = 0;
            model.TicketDescription = string.Empty;
            model.UserID = 0;
            
            return model;
        }
    }
}
