using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.OA.Pto
{
    public partial class PtoUserTime : BasePage
    {
        private EventsApplication _eventsApplication;

        private double _totalHours = 0;
        public UsersEntity SelectedUser
        {
            get;
            set;
        }
        public ProjectsEntity SelectedProject
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = QS("user", 0);
            int projectID = QS("project", 0);
            if (userID == 0 || projectID == 0)
            {
                this.Alert("Current Page get an error,please check your argument!", "/Pto/Pto.aspx?viewmodel=detailmodel");
            }
            UserApplication userApp = new UserApplication();
            this.SelectedUser = userApp.GetUser(userID);
            ProjectApplication projApp = new ProjectApplication();
            this.SelectedProject = projApp.Get(projectID);

            //DateTime startDate = DateTime.MinValue;
            //DateTime endDate = DateTime.MinValue;
            //DateTime.TryParse(Request.QueryString["startdate"], out startDate);
            //DateTime.TryParse(Request.QueryString["enddate"], out endDate);

            DateTime StartDate;
            DateTime EndDate;
            if (!DateTime.TryParse(Request.QueryString["year"], out StartDate))
            {
                StartDate = new DateTime(1753, 1, 1);
                EndDate = new DateTime(2200, 1, 1); ;
            }
            else
            {
                EndDate = StartDate.AddYears(1).AddDays(-1);
            }
            _eventsApplication = new EventsApplication();
            DataTable evdt = _eventsApplication.GetPtoByProjectUser(projectID, userID, StartDate, EndDate);
            IList<PtoUserView> eventPtolist = ModelConvertHelper<PtoUserView>.ConvertToModel(evdt);
            List<PtoUserDetailView> ptoUserDetailViews = new List<PtoUserDetailView>();
            if (eventPtolist != null && eventPtolist.Count > 0)
            {
                foreach (var t in eventPtolist)
                {
                    PtoUserDetailView ptoUserDetailView = new PtoUserDetailView
                    {
                        Title = t.Title,
                        Name = t.Name,
                        Details = t.Details
                    };
                    double hours = 0;
                    if (t.Office == "CN")
                    {
                        #region
                        if (t.AllDay)
                        {
                            if (t.ToDay.Date == t.FromDay.Date)
                            {
                                hours += 8;
                            }
                            else
                            {
                                var days = t.ToDay.Day - t.FromDay.Day + 1;
                                hours = hours + days * 8;
                            }
                            ptoUserDetailView.FromDay = t.FromDay.Date.AddHours(8).AddMinutes(30);
                            ptoUserDetailView.ToDay = t.ToDay.Date.AddHours(17).AddMinutes(30);
                        }
                        if (!t.AllDay)
                        {
                            var fromTime = t.FromTime.Split(':');
                            var toTime = t.ToTime.Split(':');
                            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                            dtFormat.ShortDatePattern = "HH:mm";
                            var fromTimeHour = Int32.Parse(fromTime[0]);
                            var fromtimeMinutes = Int32.Parse(fromTime[1]);
                            var toTimeHour = Int32.Parse(toTime[0]);
                            var toTimeMinutes = Int32.Parse(toTime[1]);
                            var fromTimeDate = Convert.ToDateTime(t.FromTime, dtFormat);
                            var toTimeDate = Convert.ToDateTime(t.ToTime, dtFormat);
                            var workTime = Convert.ToDateTime("8:30", dtFormat);
                            var restTimeBegin = Convert.ToDateTime("12:30", dtFormat);
                            var restTimeEnd = Convert.ToDateTime("13:30", dtFormat);
                            var closeTime = Convert.ToDateTime("17:30", dtFormat);
                            if (t.FromTimeType == 2)
                            {
                                if (fromTimeHour <= 5)
                                {
                                    fromTimeDate = Convert.ToDateTime(t.FromTime, dtFormat).AddHours(12);
                                }
                            }
                            if (t.FromTimeType == 1)
                            {
                                if (fromTimeHour == 12)
                                {
                                    fromTimeDate = workTime;
                                }
                            }
                            if (t.ToTimeType == 2)
                            {
                                if (toTimeHour <= 5)
                                {
                                    toTimeDate = Convert.ToDateTime(t.ToTime, dtFormat).AddHours(12);
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
                            if (t.FromTimeType == 1)
                            {
                                ptoUserDetailView.FromDay = t.FromDay.Date.AddHours(fromTimeHour).AddMinutes(fromtimeMinutes);
                            }
                            else
                            {
                                if (fromTimeHour <= 5)
                                {
                                    ptoUserDetailView.FromDay = t.FromDay.Date.AddHours(fromTimeHour).AddHours(12).AddMinutes(fromtimeMinutes);
                                }
                            }
                            if (t.ToTimeType == 1)
                            {
                                ptoUserDetailView.ToDay = t.ToDay.Date.AddHours(toTimeHour).AddMinutes(toTimeMinutes);
                            }
                            else
                            {
                                if (toTimeHour <= 5)
                                {
                                    ptoUserDetailView.ToDay =
                                        t.ToDay.Date.AddHours(toTimeHour).AddHours(12).AddMinutes(toTimeMinutes);
                                }
                                if (toTimeHour > 5 && toTimeHour < 12)
                                {
                                    ptoUserDetailView.ToDay = closeTime;
                                }
                                if (toTimeHour == 12)
                                {
                                    ptoUserDetailView.ToDay = t.ToDay.Date.AddHours(12).AddMinutes(toTimeMinutes);
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        var workTimes = _eventsApplication.GetWorkTime(t.UserID);
                        var worksViews = workTimes.Select(k => new WorkTimeView
                        {
                            FromTime = k.FromTime,
                            ToTime = k.ToTime,
                            FromTimeType = k.FromTimeType,
                            ToTimeType = k.ToTimeType
                        }).OrderBy(c => c.FromDate).ToList();
                        if (!worksViews.Any())
                        {
                            #region
                            if (t.AllDay)
                            {
                                if (t.ToDay.Date == t.FromDay.Date)
                                {
                                    hours += 8;
                                }
                                else
                                {
                                    var days = t.ToDay.Day - t.FromDay.Day + 1;
                                    hours = hours + days * 8;
                                }
                                ptoUserDetailView.FromDay = t.FromDay.Date.AddHours(8).AddMinutes(30);
                                ptoUserDetailView.ToDay = t.ToDay.Date.AddHours(17).AddMinutes(30);
                            }
                            if (!t.AllDay)
                            {
                                var fromTime = t.FromTime.Split(':');
                                var toTime = t.ToTime.Split(':');
                                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                                dtFormat.ShortDatePattern = "HH:mm";
                                var fromTimeHour = Int32.Parse(fromTime[0]);
                                var fromtimeMinutes = Int32.Parse(fromTime[1]);
                                var toTimeHour = Int32.Parse(toTime[0]);
                                var toTimeMinutes = Int32.Parse(toTime[1]);
                                var fromTimeDate = Convert.ToDateTime(t.FromTime, dtFormat);
                                var toTimeDate = Convert.ToDateTime(t.ToTime, dtFormat);
                                var workTime = Convert.ToDateTime("8:30", dtFormat);
                                var restTimeBegin = Convert.ToDateTime("12:30", dtFormat);
                                var restTimeEnd = Convert.ToDateTime("13:30", dtFormat);
                                var closeTime = Convert.ToDateTime("17:30", dtFormat);
                                if (t.FromTimeType == 2)
                                {
                                    if (fromTimeHour <= 5)
                                    {
                                        fromTimeDate = Convert.ToDateTime(t.FromTime, dtFormat).AddHours(12);
                                    }
                                }
                                if (t.FromTimeType == 1)
                                {
                                    if (fromTimeHour == 12)
                                    {
                                        fromTimeDate = workTime;
                                    }
                                }
                                if (t.ToTimeType == 2)
                                {
                                    if (toTimeHour <= 5)
                                    {
                                        toTimeDate = Convert.ToDateTime(t.ToTime, dtFormat).AddHours(12);
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
                                if (t.FromTimeType == 1)
                                {
                                    ptoUserDetailView.FromDay = t.FromDay.Date.AddHours(fromTimeHour).AddMinutes(fromtimeMinutes);
                                }
                                else
                                {
                                    if (fromTimeHour <= 5)
                                    {
                                        ptoUserDetailView.FromDay = t.FromDay.Date.AddHours(fromTimeHour).AddHours(12).AddMinutes(fromtimeMinutes);
                                    }
                                }
                                if (t.ToTimeType == 1)
                                {
                                    ptoUserDetailView.ToDay = t.ToDay.Date.AddHours(toTimeHour).AddMinutes(toTimeMinutes);
                                }
                                else
                                {
                                    if (toTimeHour <= 5)
                                    {
                                        ptoUserDetailView.ToDay =
                                            t.ToDay.Date.AddHours(toTimeHour).AddHours(12).AddMinutes(toTimeMinutes);
                                    }
                                    if (toTimeHour > 5 && toTimeHour < 12)
                                    {
                                        ptoUserDetailView.ToDay = closeTime;
                                    }
                                    if (toTimeHour == 12)
                                    {
                                        ptoUserDetailView.ToDay = t.ToDay.Date.AddHours(12).AddMinutes(toTimeMinutes);
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            var mindate = worksViews.Min(x => x.FromDate);
                            var maxdate = worksViews.Max(x => x.ToDate);
                            #region
                            if (t.AllDay)
                            {
                                if (t.ToDay.Date == t.FromDay.Date)
                                {
                                    hours += 8;
                                }
                                else
                                {
                                    var days = t.ToDay.Day - t.FromDay.Day + 1;
                                    hours = hours + days * 8;
                                }
                                ptoUserDetailView.FromDay = mindate;
                                ptoUserDetailView.ToDay = maxdate;
                            }
                            else
                            {
                                #region
                                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo
                                {
                                    ShortDatePattern = "HH:mm"
                                };
                                var fromTime = t.FromTime.Split(':');
                                var toTime = t.ToTime.Split(':');
                                var fromTimeHour = Int32.Parse(fromTime[0]);
                                var fromtimeMinutes = Int32.Parse(fromTime[1]);
                                var toTimeHour = Int32.Parse(toTime[0]);
                                var toTimeMinutes = Int32.Parse(toTime[1]);
                                var fromTimeDate = Convert.ToDateTime(t.FromTime, dtFormat);
                                var toTimeDate = Convert.ToDateTime(t.ToTime, dtFormat);

                                foreach (var g in worksViews)
                                {
                                    var workTime = g.FromDate;
                                    var closeTime = g.ToDate;
                                    switch (t.FromTimeType)
                                    {
                                        case 1:
                                            fromTimeDate = fromTimeHour == 12 ? Convert.ToDateTime("00:" + fromtimeMinutes, dtFormat) : Convert.ToDateTime(t.FromTime, dtFormat);
                                            break;
                                        case 2:
                                            fromTimeDate = fromTimeHour == 12 ? Convert.ToDateTime(t.FromTime, dtFormat) : Convert.ToDateTime(t.FromTime, dtFormat).AddHours(12);
                                            break;
                                    }
                                    switch (t.ToTimeType)
                                    {
                                        case 1:
                                            toTimeDate = toTimeHour == 12 ? Convert.ToDateTime("00:" + toTimeMinutes, dtFormat) : Convert.ToDateTime(t.ToTime, dtFormat);
                                            break;
                                        case 2:
                                            toTimeDate = toTimeHour == 12 ? Convert.ToDateTime(t.ToTime, dtFormat) : Convert.ToDateTime(t.ToTime, dtFormat).AddHours(12);
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
                                ptoUserDetailView.FromDay = t.FromTimeType == 1 ? t.FromDay.Date.AddHours(fromTimeHour == 12 ? 0 : fromTimeHour).AddMinutes(fromtimeMinutes) : t.FromDay.Date.AddHours(fromTimeHour == 12 ? 0 : fromTimeHour).AddHours(12).AddMinutes(fromtimeMinutes);
                                ptoUserDetailView.ToDay = t.ToTimeType == 1 ? t.ToDay.Date.AddHours(toTimeHour == 12 ? 0 : toTimeHour).AddMinutes(toTimeMinutes) : t.ToDay.Date.AddHours(toTimeHour == 12 ? 0 : toTimeHour).AddHours(12).AddMinutes(toTimeMinutes);
                                #endregion
                            }
                            #endregion
                        }
                    }
                    ptoUserDetailView.Hours = hours;
                    ptoUserDetailViews.Add(ptoUserDetailView);
                }
                this.ptoDetail.DataSource = ptoUserDetailViews;
                this.ptoDetail.DataBind();
            }
            else
            {
                ShowFailMessageToClient("There is no records ");
            }
            litUserName.Text = SelectedUser.FirstName + " " + SelectedUser.LastName;
            if (ptoUserDetailViews.Any())
            {
                foreach (var t in ptoUserDetailViews)
                {
                    _totalHours += t.Hours;
                }
            }
            litTotalhours.Text = _totalHours.ToString("#0.00") + " h";
        }
    }
}