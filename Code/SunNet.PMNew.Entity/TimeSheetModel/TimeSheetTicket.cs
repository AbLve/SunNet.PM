using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Data;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Entity.TicketModel.Enums;

namespace SunNet.PMNew.Entity.TimeSheetModel
{
    public class TimeSheetTicket
    {
        public static DateTime LimitDate
        {
            get
            {
                //var spans = DayOfWeek.Friday - DateTime.Now.DayOfWeek;
                //spans = spans > 0 ? spans : 0;

                //改为未来7天均可填写timesheet
                return DateTime.Now.Date.AddDays(7);
            }
        }

        public static bool CanEdit(DateTime date)
        {
            DateTime compareDate = LimitDate;
            DateTime targetDate = date.Date;
            if (targetDate > compareDate)
            {
                return false;
            }
            //TimeSpan ts = compareDate - targetDate;
            TimeSpan ts = DateTime.Now - targetDate;
            if (ts.TotalDays > Config.TimeSheetDays + 1)
            {
                return false;
            }
            return true;
        }

        public static TimeSheetTicket GetEmptyModel()
        {
            TimeSheetTicket model = new TimeSheetTicket();

            string text = "&nbsp;";

            model.Hours = 0;
            model.IsMeeting = false;
            model.IsSubmitted = false;
            model.Percentage = 0;
            model.ProjectID = 0;
            model.ProjectTitle = text;
            model.TicketCode = text;
            model.TicketDescription = text;
            model.TicketID = 0;
            model.TicketTitle = text;
            model.TimeSheetID = 0;
            model.WorkDetail = text;
            model.ModifiedOn = DateTime.Now;
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="defaultDate"></param>
        /// <param name="isSum">是否为统计 Hours 而绑定</param>
        /// <returns></returns>
        public static TimeSheetTicket ReaderBind(IDataReader dataReader, DateTime defaultDate, bool isSum = false)
        {
            TimeSheetTicket model = new TimeSheetTicket();

            object ojb;
            if (!isSum)
            {
                ojb = dataReader["TimeSheetID"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.TimeSheetID = (int)ojb;
                }
                else
                {
                    model.TimeSheetID = 0;
                }
                ojb = dataReader["SheetDate"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.SheetDate = (DateTime)ojb;
                    if (model.SheetDate < new DateTime(1753, 1, 1))
                    {
                        model.SheetDate = defaultDate;
                    }
                }
                else
                {
                    model.SheetDate = defaultDate;
                }
                ojb = dataReader["Percentage"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.Percentage = Convert.ToDecimal(ojb.ToString());
                }
                else
                {
                    model.Percentage = 0;
                }
                model.WorkDetail = dataReader["WorkDetail"].ToString();
                ojb = dataReader["ModifiedOn"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    DateTime dt = (DateTime)ojb;
                    if (dt < new DateTime(1753, 1, 1))
                    {
                        model.ModifiedOn = defaultDate;
                    }
                    else
                    {
                        model.ModifiedOn = dt;
                    }
                }
                else
                {
                    model.ModifiedOn = defaultDate;
                }
                ojb = dataReader["IsSubmitted"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.IsSubmitted = (bool)ojb;
                }
                else
                {
                    model.IsSubmitted = false;
                }
                ojb = dataReader["IsMeeting"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.IsMeeting = (bool)ojb;
                }
            }
            else if (dataReader["FinalTime"] != null && dataReader["FinalTime"] != DBNull.Value)
            {
                model.Estimation = (decimal)dataReader["FinalTime"];
            }
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }
            else
            {
                model.ProjectID = 0;
            }
            model.ProjectTitle = dataReader["ProjectTitle"].ToString();
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            else
            {
                model.TicketID = 0;
            }
            try
            {
                ojb = dataReader["Accounting"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.Accounting = (AccountingState)Enum.Parse(typeof(AccountingState), ojb.ToString());
                }
            }
            catch(Exception)
            {
            }
           
            model.TicketTitle = dataReader["TicketTitle"].ToString();
            model.TicketCode = dataReader["TicketCode"].ToString();
            model.TicketDescription = dataReader["TicketDescription"].ToString();
            //user role name
            model.RoleName = dataReader["RoleName"].ToString();


            ojb = dataReader["Hours"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Hours = (decimal)ojb;
            }
            else
            {
                model.Hours = 0;
            }

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="isSum">是否为统计 Hours 而绑定</param>
        /// <returns></returns>
        public static TimeSheetTicket ReaderBindForReport(IDataReader dataReader, bool isSum = false)
        {
            TimeSheetTicket model = TimeSheetTicket.ReaderBind(dataReader, DateTime.Now, isSum);
            object ojb;
            model.FirstName = dataReader["FirstName"].ToString();
            model.LastName = dataReader["LastName"].ToString();
            ojb = dataReader["UserID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UserID = (int)ojb;
            }
            return model;
        }
        public int ProjectID { get; set; }
        public string ProjectTitle { get; set; }
        public int TicketID { get; set; }
        public int UserID { get; set; }
        public string TicketTitle { get; set; }
        public string TicketCode { get; set; }
        public string TicketDescription { get; set; }
        public int TimeSheetID { get; set; }
        public DateTime SheetDate { get; set; }
        public decimal Hours { get; set; }
        public decimal Percentage { get; set; }
        public string WorkDetail { get; set; }
        public bool IsSubmitted { get; set; }
        private string _submittedText;
        public string SubmittedText
        {
            get
            {
                if (this.IsSubmitted)
                {
                    return "Yes";
                }
                else
                {
                    return "&nbsp;";
                }
            }
            set
            {
                _submittedText = value;
            }
        }
        public bool IsMeeting { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public decimal Estimation { get; set; }
        public AccountingState Accounting { get; set; }
    }
}
