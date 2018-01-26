using System;
using System.Collections.Generic;
using System.Text;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.OA.Timesheet
{
    public partial class Index : BasePage
    {
        public DateTime Today
        {
            get { return ObjectFactory.GetInstance<ISystemDateTime>().Now; }
        }

        public DateTime EditableDate
        {
            get { return ObjectFactory.GetInstance<ISystemDateTime>().Now.AddDays(-Config.TimeSheetDays); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControls();
            }
        }
        private void InitControls()
        {
            DateTime endDate = DateTime.Now.AddDays(-1).Date;
            DateTime startDate = DateTime.Now.AddDays(0 - Config.TimeSheetDays);

            TimeSheetApplication tsApp = new TimeSheetApplication();
            List<DateTime> listUnFinish = tsApp.GetUnfinishedTimeSheetDate(UserInfo.ID, startDate, endDate);
            List<DateTime> listUnSubmitted = tsApp.GetUnSubmittedTimeSheetDate(UserInfo.ID, startDate, endDate);

            string template = "<a href='Date.aspx?date={0}'>{1}</a>";
            bool showTips = false;
            string html = "";
            if (listUnFinish != null && listUnFinish.Count > 0)
            {
                foreach (DateTime date in listUnFinish)
                {
                    string week = date.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        if (string.IsNullOrEmpty(html))
                        {
                            html += " Unfinished:&nbsp;";
                        }
                        html += string.Format(template, date.ToString("yyyy-MM-dd"), date.ToString("MM/dd/yyyy"));
                        html += ",&nbsp;&nbsp;";
                        showTips = true;
                    }
                }
                ltlUnfinished.Text = html.TrimEnd(",&nbsp;&nbsp;".ToCharArray());
            }
            if (listUnSubmitted != null && listUnSubmitted.Count > 0)
            {
                html = "";
                foreach (DateTime date in listUnSubmitted)
                {
                    string week = date.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        if (string.IsNullOrEmpty(html))
                        {
                            html += " Unsubmitted:&nbsp;";
                        }
                        html += string.Format(template, date.ToString("yyyy-MM-dd"), date.ToString("MM/dd/yyyy"));
                        html += ",&nbsp;&nbsp;";
                        showTips = true;
                    }
                }
                ltlUnSubmitted.Text = html.TrimEnd(",&nbsp;&nbsp;".ToCharArray());
            }
            phlUncompleted.Visible = showTips;
        }
    }
}