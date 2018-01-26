using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Core.TimeSheetModule;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.OA.Timesheet
{
    public partial class MoveTimeSheet : BasePage
    {
        private string TimeSheetId;
        private TimeSheetApplication tsApp = new TimeSheetApplication();
        private ITimeSheetRepository tsRepository = ObjectFactory.GetInstance<ITimeSheetRepository>();
        private Service.Timesheet ts = new Service.Timesheet();
        protected void Page_Load(object sender, EventArgs e)
        {
            TimeSheetId = Request.QueryString["tmid"];
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = -1;
            bool isInt = int.TryParse(TimeSheetId, out id);
            if (isInt)
            {
                if (id > 0)
                {
                    TimeSheetsEntity timesheet = tsRepository.Get(id);
                    if (timesheet != null)
                    {
                        DateTime sheetdate_old = timesheet.SheetDate;
                        TimeSheetsEntity moveTimesheet = tsApp.GetByUserId(UserInfo.UserID, DateTime.Parse(txtFrom.Text));
                        if (moveTimesheet == null) //当天没有写时，可直接更改
                        {
                            MoveTimeSheets(timesheet, sheetdate_old);
                        }
                        else
                        {
                            if (moveTimesheet.IsSubmitted)//当前TimeSheet已提交时，不可更改
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), Request.Url.AbsolutePath
                                    , "<p style='color:red;text-align: center;line-height: 30px;margin: 3px;'>Warning:The timesheet of the selected day has been submitted!</p>");
                            }
                            else
                            {
                                MoveTimeSheets(timesheet, sheetdate_old);
                            }
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), Request.Url.AbsolutePath
                            , "<p style='color:red;text-align: center;line-height: 30px;margin: 3px;'>Warning:Paramter is error!</p>");
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), Request.Url.AbsolutePath
                        , "<p style='color:red;text-align: center;line-height: 30px;margin: 3px;'>Warning:Paramter is error!</p>");
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), Request.Url.AbsolutePath
                    , "<p style='color:red;text-align: center;line-height: 30px;margin: 3px;'>Warning:Paramter is error!</p>");
            }
        }

        private void MoveTimeSheets(TimeSheetsEntity timesheet, DateTime sheetdate_old)
        {
            timesheet.SheetDate = DateTime.Parse(txtFrom.Text);
            timesheet.ModifiedBy = UserInfo.UserID;
            timesheet.ModifiedOn = DateTime.Now;
            tsApp.UpdateTimeSheet(timesheet);
            if (timesheet.SheetDate > DateTime.Now) //更改Change后的WeekPlan
            {
                ts.SyncWeekPlan(timesheet);
            }
            if (sheetdate_old > DateTime.Now) //更改之前的WeekPlan
            {
                timesheet.SheetDate = sheetdate_old;
                ts.SyncWeekPlan(timesheet);
            }
            Redirect(Request.RawUrl, false, true);
        }

    }
}