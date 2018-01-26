using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.Entity.ComplaintModel.Enums;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.PM2014.SunnetTicket;
using SunNet.PMNew.PM2014.Timesheet;
using System.Configuration;

namespace SunNet.PMNew.PM2014.OA.Pto
{
    public partial class Pto : BasePage
    {
        private string ReportView
        {
            get
            {
                if (Request.QueryString["viewmodel"] == null || string.IsNullOrEmpty(Request.QueryString["viewmodel"]))
                {
                    return "DetailView";
                }
                else if (Request.QueryString["viewmodel"].ToLower() == "hoursmodel")
                {
                    return "HoursView";
                }
                else
                {
                    return "DetailView";
                }
            }
        }
        protected override string DefaultOrderBy
        {
            get
            {
                return "Title";
            }
        }
        protected override string DefaultDirection
        {
            get
            {
                return "ASC";
            }
        }
        UserApplication userApp;
        TimeSheetApplication _tsApp;
        List<ModulesEntity> list = new List<ModulesEntity>();
        static int allid = Convert.ToInt32(ConfigurationManager.AppSettings["PTOVIEWALL"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
            _tsApp = new TimeSheetApplication();
            if (!IsPostBack)
            {
                list = userApp.GetRoleModules(UserInfo.RoleID, true);
                InitSearchControls();
                if (QS("project") != "")
                    ddlProject.SelectedValue = QS("project");
                if (QS("user") != "")
                    ddlUsers.SelectedValue = QS("user");
                InitControls();
            }
        }
        /// <summary>
        /// 初始化搜索 条件
        /// </summary>
        private void InitSearchControls()
        {
            DateTime date = DateTime.Now.AddMonths(-1);
            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> listAll = projApp.GetAllProjects().Where(c => c.Title == "0_PTO").ToList();
            listAll.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0");

            SearchUsersRequest requestUser = new SearchUsersRequest(
                SearchUsersType.All, false, " FirstName ", " ASC ");
            requestUser.IsSunnet = true;
            var selfid = Convert.ToInt32(ConfigurationManager.AppSettings["PTOVIEWSELF"]);

            if (list.Where(t => t.ModuleID == allid).Count() > 0)
            {
                SearchUserResponse responseuser = userApp.SearchUsers(requestUser);
                ddlUsers.Items.Add(new ListItem("All", "0"));
                foreach (UsersEntity user in responseuser.ResultList)
                {
                    ddlUsers.Items.Add(new ListItem(string.Format("{0} {1}", user.FirstName, user.LastName)
                        , user.ID.ToString()));
                }
            }
            else
            {
                ddlUsers.Items.Add(new ListItem(string.Format("{0} {1}", UserInfo.FirstName, UserInfo.LastName)
                      , UserInfo.ID.ToString()));
            }


        }
        /// <summary>
        /// scan all people pto or signal people
        /// </summary>
        private void InitControls()
        {
            //if (list.Where(t => t.ModuleID == allid).Count() == 0 && (UserInfo.ID + "" != ddlUsers.SelectedValue))//防止在手动修改链接参数
            //{
            //    ddlUsers.SelectedValue = UserInfo.ID + "";
            //}

            DateTime StartDate;
            DateTime EndDate;
            if (!DateTime.TryParse(QS("year"), out StartDate))
            {
                if (QS("year") == "-1")
                {
                    StartDate = MinDate;
                    EndDate = new DateTime(2200, 1, 1);
                }
                else
                {
                    StartDate = new DateTime(DateTime.Now.Year, 1, 1);
                    EndDate = StartDate.AddYears(1).AddDays(-1);
                }
            }
            else
            {
                EndDate = StartDate.AddYears(1).AddDays(-1);
            }
            if (this.ReportView == "DetailView")
            {
                rptHoursView.Visible = false;
                rptListReport.Visible = true;

                List<PtoView> ptoViews = PtosHelper.ReGeneratePtos(int.Parse(ddlProject.SelectedValue), StartDate, EndDate, int.Parse(ddlUsers.SelectedValue), OrderBy, OrderDirection, DefaultOrderBy);

                #region Special order
                if (OrderBy == "Hours")
                {
                    if (OrderDirection.ToUpper() == "ASC")
                    {
                        ptoViews = ptoViews.OrderBy(t => t.Hours).ToList();
                    }
                    else
                    {
                        ptoViews = ptoViews.OrderByDescending(t => t.Hours).ToList();
                    }
                }
                if (OrderBy == "Remaining")
                {
                    if (OrderDirection.ToUpper() == "ASC")
                    {
                        ptoViews = ptoViews.OrderBy(t => t.Remaining).ToList();
                    }
                    else
                    {
                        ptoViews = ptoViews.OrderByDescending(t => t.Remaining).ToList();
                    }
                }
                #endregion
                rptListReport.DataSource = ptoViews;
                rptListReport.DataBind();

                if (ptoViews.Count == 0)
                {
                    trNoListRecord.Visible = true;
                }
            }
            else if (this.ReportView == "HoursView")
            {
                rptListReport.Visible = false;
                rptHoursView.Visible = true;

                DataTable dt = _tsApp.QueryReportTotalHoursByProject(
                    int.Parse(ddlProject.SelectedValue),
                    int.Parse(ddlUsers.SelectedValue),
                    StartDate,
                    EndDate,
                   OrderBy,
                    OrderDirection);
                rptHoursView.DataSource = dt;
                rptHoursView.DataBind();
                if (dt.Rows.Count == 0)
                {
                    trNoHourRecord.Visible = true;
                }

            }
            else
            {
                rptListReport.Visible = false;
                rptHoursView.Visible = false;
            }

        }



        /// <summary>
        /// export xls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void iBtnDownload_Click(object sender, EventArgs e)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            if (this.ReportView == "DetailView")
            {
                rptListReport.RenderControl(hw);
            }
            else if (this.ReportView == "HoursView")
            {
                rptHoursView.RenderControl(hw);
            }
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            Page.EnableViewState = false;
            string fileName = string.Format("{0}_{1}.xls", ddlProject.SelectedItem.Text, ddlUsers.SelectedItem.Text).Replace(" ", "_");
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\"><title>Project Report</title></head><body><center>");
            string body = sw.ToString();

            body = body.Replace("width: 30%;", "width:300;");
            body = body.Replace("width: 20%;", "width:100;");
            body = body.Replace("width: 10%;", "width:100;");

            body = body.Replace("width: 71%;", "width:300;");
            body = body.Replace("width: 29%;", "width:100;");
            Regex removeActioin = new Regex(@"<(td|th).*(?=hidethis)(.|\n)*?</(th|td)>");
            body = removeActioin.Replace(body, "");
            body = body.Replace("<td", "<td style='mso-number-format:\"0\\.00\";'");//
            body = body.Replace("<table border=\"0\"", "<table border=\"1\" ");
            Response.Write(body);
            Response.Write("</center></body></html>");
            Response.End();
        }
    }

    public class ModelConvertHelper<T> where T : new()  // 此处一定要加上new()
    {

        public static IList<T> ConvertToModel(DataTable dt)
        {

            IList<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
    }
    public class PtosHelper
    {
        public static List<PtoView> ReGeneratePtos(int ProId, DateTime StartDate, DateTime EndDate, int UserId = 0, string OrderBy = "ProjectTitle", string OrderDirection = "ASC", string DefaultOrderBy = "ProjectTitle")
        {
            EventsApplication _eventsApplication = new EventsApplication();
            DataTable ptoDt = _eventsApplication.QueryReportDetailsByProject(
                ProId,
                UserId,
                StartDate,
                EndDate,
                OrderBy == "Hours" || OrderBy == "Remaining" ? DefaultOrderBy : OrderBy,
                OrderDirection);
            List<PtoView> ptoViews = new List<PtoView>();
            if (ptoDt.Rows.Count > 0)
            {
                IList<EventPtoView> eventPtolist = ModelConvertHelper<EventPtoView>.ConvertToModel(ptoDt);
                var s = eventPtolist.GroupBy(c => c.CreatedBy).ToList();
                foreach (var t in s)
                {
                    PtoView ptoView = new PtoView
                    {
                        ProjectID = t.FirstOrDefault().ProjectID,
                        FirstName = t.FirstOrDefault().FirstName,
                        LastName = t.FirstOrDefault().LastName,
                        UserID = t.FirstOrDefault().CreatedBy,
                        Title = t.FirstOrDefault().Title,
                        PTOHoursOfYear = t.FirstOrDefault().PTOHoursOfYear
                    };
                    double hours = 0;
                    foreach (var m in t)
                    {
                        if (m.Office == "CN")
                        {
                            #region
                            if (m.AllDay)
                            {
                                if (m.ToDay.Date == m.FromDay.Date)
                                {
                                    hours += 8;
                                }
                                else
                                {
                                    var days = m.ToDay.Day - m.FromDay.Day + 1;
                                    hours = hours + days * 8;
                                }
                            }
                            if (!m.AllDay)
                            {
                                var fromTime = m.FromTime.Split(':');
                                var toTime = m.ToTime.Split(':');
                                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                                dtFormat.ShortDatePattern = "HH:mm";
                                var fromTimeHour = Int32.Parse(fromTime[0]);
                                var fromtimeMinutes = Int32.Parse(fromTime[1]);
                                var toTimeHour = Int32.Parse(toTime[0]);
                                var toTimeMinutes = Int32.Parse(toTime[1]);
                                var fromTimeDate = Convert.ToDateTime(m.FromTime, dtFormat);
                                var toTimeDate = Convert.ToDateTime(m.ToTime, dtFormat);
                                var restTimeBegin = Convert.ToDateTime("12:30", dtFormat);
                                var restTimeEnd = Convert.ToDateTime("13:30", dtFormat);
                                var workTime = Convert.ToDateTime("8:30", dtFormat);
                                var closeTime = Convert.ToDateTime("17:30", dtFormat);
                                if (m.FromTimeType == 2)
                                {
                                    if (fromTimeHour <= 5)
                                    {
                                        fromTimeDate = Convert.ToDateTime(m.FromTime, dtFormat).AddHours(12);
                                    }
                                }
                                if (m.FromTimeType == 1)
                                {
                                    if (fromTimeHour == 12)
                                    {
                                        fromTimeDate = workTime;
                                    }
                                }
                                if (m.ToTimeType == 2)
                                {
                                    if (toTimeHour <= 5)
                                    {
                                        toTimeDate = Convert.ToDateTime(m.ToTime, dtFormat).AddHours(12);
                                    }
                                    if (5 < toTimeHour && toTimeHour < 12)
                                    {
                                        toTimeDate = closeTime;
                                    }
                                }
                                if (toTimeDate < restTimeBegin)
                                {
                                    hours = hours + (toTimeDate - fromTimeDate).TotalHours;
                                }
                                if (toTimeDate > restTimeEnd)
                                {
                                    hours = hours + (restTimeBegin - fromTimeDate).TotalHours;
                                    hours = hours + (toTimeDate - restTimeEnd).TotalHours;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            var workTimes = _eventsApplication.GetWorkTime(m.UserID);
                            var worksViews = workTimes.Select(e => new WorkTimeView
                            {
                                FromTime = e.FromTime,
                                ToTime = e.ToTime,
                                FromTimeType = e.FromTimeType,
                                ToTimeType = e.ToTimeType
                            }).OrderBy(c => c.FromDate).ToList();
                            #region
                            if (!worksViews.Any())
                            {
                                #region
                                if (m.AllDay)
                                {
                                    if (m.ToDay.Date == m.FromDay.Date)
                                    {
                                        hours += 8;
                                    }
                                    else
                                    {
                                        var days = m.ToDay.Day - m.FromDay.Day + 1;
                                        hours = hours + days * 8;
                                    }
                                }
                                else
                                {
                                    var fromTime = m.FromTime.Split(':');
                                    var toTime = m.ToTime.Split(':');
                                    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo
                                    {
                                        ShortDatePattern = "HH:mm"
                                    };
                                    var fromTimeHour = Int32.Parse(fromTime[0]);
                                    var fromtimeMinutes = Int32.Parse(fromTime[1]);
                                    var toTimeHour = Int32.Parse(toTime[0]);
                                    var toTimeMinutes = Int32.Parse(toTime[1]);
                                    var fromTimeDate = Convert.ToDateTime(m.FromTime, dtFormat);
                                    var toTimeDate = Convert.ToDateTime(m.ToTime, dtFormat);
                                    var restTimeBegin = Convert.ToDateTime("12:30", dtFormat);
                                    var restTimeEnd = Convert.ToDateTime("13:30", dtFormat);
                                    var workTime = Convert.ToDateTime("8:30", dtFormat);
                                    var closeTime = Convert.ToDateTime("17:30", dtFormat);
                                    if (m.FromTimeType == 2)
                                    {
                                        if (fromTimeHour <= 5)
                                        {
                                            fromTimeDate = Convert.ToDateTime(m.FromTime, dtFormat).AddHours(12);
                                        }
                                    }
                                    if (m.FromTimeType == 1)
                                    {
                                        if (fromTimeHour == 12)
                                        {
                                            fromTimeDate = workTime;
                                        }
                                    }
                                    if (m.ToTimeType == 2)
                                    {
                                        if (toTimeHour <= 5)
                                        {
                                            toTimeDate = Convert.ToDateTime(m.ToTime, dtFormat).AddHours(12);
                                        }
                                        if (5 < toTimeHour && toTimeHour < 12)
                                        {
                                            toTimeDate = closeTime;
                                        }
                                    }
                                    if (toTimeDate < restTimeBegin)
                                    {
                                        hours = hours + (toTimeDate - fromTimeDate).TotalHours;
                                    }
                                    if (toTimeDate > restTimeEnd)
                                    {
                                        hours = hours + (restTimeBegin - fromTimeDate).TotalHours;
                                        hours = hours + (toTimeDate - restTimeEnd).TotalHours;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                if (m.AllDay)
                                {
                                    #region
                                    if (m.ToDay.Date == m.FromDay.Date)
                                    {
                                        hours += 8;
                                    }
                                    else
                                    {
                                        var days = m.ToDay.Day - m.FromDay.Day + 1;
                                        hours = hours + days * 8;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo
                                    {
                                        ShortDatePattern = "HH:mm"
                                    };
                                    var fromTime = m.FromTime.Split(':');
                                    var toTime = m.ToTime.Split(':');

                                    var fromTimeHour = Int32.Parse(fromTime[0]);
                                    var fromtimeMinutes = Int32.Parse(fromTime[1]);
                                    var toTimeHour = Int32.Parse(toTime[0]);
                                    var toTimeMinutes = Int32.Parse(toTime[1]);
                                    var fromTimeDate = new DateTime();
                                    var toTimeDate = new DateTime();
                                    foreach (var g in worksViews)
                                    {
                                        var workTime = g.FromDate;
                                        var closeTime = g.ToDate;
                                        switch (m.FromTimeType)
                                        {
                                            case 1:
                                                fromTimeDate = fromTimeHour == 12 ? Convert.ToDateTime("00:" + fromtimeMinutes, dtFormat) : Convert.ToDateTime(m.FromTime, dtFormat);
                                                break;
                                            case 2:
                                                fromTimeDate = fromTimeHour == 12 ? Convert.ToDateTime(m.FromTime, dtFormat) : Convert.ToDateTime(m.FromTime, dtFormat).AddHours(12);
                                                break;
                                        }
                                        switch (m.ToTimeType)
                                        {
                                            case 1:
                                                toTimeDate = toTimeHour == 12 ? Convert.ToDateTime("00:" + toTimeMinutes, dtFormat) : Convert.ToDateTime(m.ToTime, dtFormat);
                                                break;
                                            case 2:
                                                toTimeDate = toTimeHour == 12 ? Convert.ToDateTime(m.ToTime, dtFormat) : Convert.ToDateTime(m.ToTime, dtFormat).AddHours(12);
                                                break;
                                        }
                                        if (fromTimeDate <= workTime && closeTime <= toTimeDate)
                                        {
                                            if ((closeTime - workTime).TotalHours < 0)
                                            {
                                                continue;
                                            }
                                            hours = hours + (closeTime - workTime).TotalHours;
                                        }
                                        if (fromTimeDate <= workTime && closeTime > toTimeDate)
                                        {
                                            if ((toTimeDate - workTime).TotalHours < 0)
                                            {
                                                continue;
                                            }
                                            hours = hours + (toTimeDate - workTime).TotalHours;
                                        }
                                        if (fromTimeDate > workTime && closeTime <= toTimeDate)
                                        {
                                            if ((closeTime - fromTimeDate).TotalHours < 0)
                                            {
                                                continue;
                                            }
                                            hours = hours + (closeTime - fromTimeDate).TotalHours;
                                        }
                                        if (fromTimeDate > workTime && closeTime > toTimeDate)
                                        {
                                            if ((toTimeDate - fromTimeDate).TotalHours < 0)
                                            {
                                                continue;
                                            }
                                            hours = hours + (toTimeDate - fromTimeDate).TotalHours;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    ptoView.Hours = hours;
                    ptoViews.Add(ptoView);
                }
            }

            return ptoViews;
        }
    }

}