using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework;
namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class ListTimesheet : BaseWebsitePage
    {
        public DateTime Today
        {
            get { return ObjectFactory.GetInstance<ISystemDateTime>().Now; }
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

            string template = "<a href='AddTimesheet.aspx?date={0}'>{1}</a>";
            bool showTips = false;
            if (listUnFinish != null && listUnFinish.Count > 0)
            {
                foreach (DateTime date in listUnFinish)
                {
                    string week = date.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        if (string.IsNullOrEmpty(ltlUnfinished.Text))
                        {
                            ltlUnfinished.Text += " Unfinished:&nbsp;";
                        }
                        ltlUnfinished.Text += string.Format(template, date.ToString("yyyy-MM-dd"), date.ToString("MM/dd/yyyy"));
                        ltlUnfinished.Text += ",&nbsp;&nbsp;";
                        showTips = true;
                    }
                }
            }
            if (listUnSubmitted != null && listUnSubmitted.Count > 0)
            {
                foreach (DateTime date in listUnSubmitted)
                {
                    string week = date.DayOfWeek.ToString();
                    if (week != "Saturday" && week != "Sunday")
                    {
                        if (string.IsNullOrEmpty(ltlUnSubmitted.Text))
                        {
                            ltlUnSubmitted.Text += " Unsubmitted:&nbsp;";
                        }
                        ltlUnSubmitted.Text += string.Format(template, date.ToString("yyyy-MM-dd"), date.ToString("MM/dd/yyyy"));
                        ltlUnSubmitted.Text += ",&nbsp;&nbsp;";
                        showTips = true;
                    }
                }
            }
            tips.Visible = showTips;
        }
    }
}
