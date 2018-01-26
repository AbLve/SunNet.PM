using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.Common;
using SunNet.PMNew.Framework.Core;
using System.Data;

namespace SunNet.PMNew.Entity.TicketModel
{
    public class WeekPlanEntity : BaseEntity, IShowUserName
    {

        public WeekPlanEntity() { }

        public WeekPlanEntity(IDataReader Reader, bool isList)
        {
            ID = (int)Reader["ID"];
            UserID = (int)Reader["UserID"];
            WeekDay = (DateTime)Reader["WeekDay"];
            CreateDate = (DateTime)Reader["CreateDate"];
            UpdateDate = (DateTime)Reader["UpdateDate"];
            UpdateUserID = (int)Reader["UpdateUserID"];

            Sunday = (string)Reader["Sunday"];
            SundayEstimate = (int)Reader["SundayEstimate"];
            Monday = (string)Reader["Monday"];
            MondayEstimate = (int)Reader["MondayEstimate"];
            Tuesday = (string)Reader["Tuesday"];
            TuesdayEstimate = (int)Reader["TuesdayEstimate"];
            Wednesday = (string)Reader["Wednesday"];
            WednesdayEstimate = (int)Reader["WednesdayEstimate"];
            Thursday = (string)Reader["Thursday"];
            ThursdayEstimate = (int)Reader["ThursdayEstimate"];
            Friday = (string)Reader["Friday"];
            FridayEstimate = (int)Reader["FridayEstimate"];
            Saturday = (string)Reader["Saturday"];
            SaturdayEstimate = (int)Reader["SaturdayEstimate"];

            MondayTickets = (string)Reader["MondayTickets"];
            TuesdayTickets = (string)Reader["TuesdayTickets"];
            WednesdayTickets = (string)Reader["WednesdayTickets"];
            ThursdayTickets = (string)Reader["ThursdayTickets"];
            FridayTickets = (string)Reader["FridayTickets"];
            SaturdayTickets = (string)Reader["SaturdayTickets"];
            SundayTickets = (string)Reader["SundayTickets"];


            IsDeleted = (bool)Reader["IsDeleted"];

            if (isList)
            {
                LastName = (string)Reader["LastName"];
                FirstName = (string)Reader["FirstName"];

                EditLastName = (string)Reader["EditLastName"];
                EditFirstName = (string)Reader["EditFirstName"];
            }
        }


        public int UserID { get; set; }

        /// <summary>
        /// 这周的第一天
        /// </summary>
        public DateTime WeekDay { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdateUserID { get; set; }

        /// <summary>
        /// 星期日
        /// </summary>
        public string Sunday { get; set; }
        public int SundayEstimate { get; set; }
        public string SundayTickets { get; set; }

        /// <summary>
        /// 星期一
        /// </summary>
        public string Monday { get; set; }
        public int MondayEstimate { get; set; }
        public string MondayTickets { get; set; }
        /// <summary>
        /// 星期二
        /// </summary>
        public string Tuesday { get; set; }
        public int TuesdayEstimate { get; set; }
        public string TuesdayTickets { get; set; }
        /// <summary>
        /// 星期三
        /// </summary>
        public string Wednesday { get; set; }
        public int WednesdayEstimate { get; set; }
        public string WednesdayTickets { get; set; }

        /// <summary>
        /// 星期四
        /// </summary>
        public string Thursday { get; set; }
        public int ThursdayEstimate { get; set; }
        public string ThursdayTickets { get; set; }

        /// <summary>
        /// 星期五
        /// </summary>
        public string Friday { get; set; }
        public int FridayEstimate { get; set; }
        public string FridayTickets { get; set; }
        /// <summary>
        /// 星期六
        /// </summary>
        public string Saturday { get; set; }
        public int SaturdayEstimate { get; set; }
        public string SaturdayTickets { get; set; }


        public string WeekenDayHTML
        {
            get
            {
                return GetWeekendHTML();
            }
        }

        public string SundayColumn
        {
            get
            {
                if (!string.IsNullOrEmpty(Sunday.Trim()))
                {
                    return "<td>" + Sunday + "<br />Estimate: " + SundayEstimate + " H</td>";
                }
                else
                {
                    return "";
                }
            }
        }

        public string FridayColumnInFirstRow
        {
            get
            {
                if (string.IsNullOrEmpty(Sunday.Trim()))
                {
                    return "<td>" + Friday + "<br />Estimate: " + FridayEstimate + " H</td>";
                }
                else
                {
                    return "";
                }
            }
        }

        private string GetWeekendHTML()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(Saturday.Trim()) && !string.IsNullOrEmpty(Sunday.Trim()))
            {
                stringBuilder.AppendFormat("<tr class='weekplanw'><td>Friday</td><td>Saturday</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                + "<tr><td>{0}<br />Estimate: {1} H</td><td>{2}<br />Estimate: {3} H</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>", Friday, FridayEstimate, Saturday, SaturdayEstimate);
                return stringBuilder.ToString();
            }
            else if (!string.IsNullOrEmpty(Saturday.Trim()))
            {

                stringBuilder.AppendFormat("<tr class='weekplanw'><td>Saturday</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                + "<tr><td>{0}<br />Estimate: {1} H</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>", Saturday, SaturdayEstimate);
                return stringBuilder.ToString();
            }
            else if (!string.IsNullOrEmpty(Sunday.Trim()))
            {

                stringBuilder.AppendFormat("<tr class='weekplanw'><td>Friday</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>"
                + "<tr><td>{0}<br />Estimate: {1} H</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>", Friday, FridayEstimate);
                return stringBuilder.ToString();
            }
            else
            {
                return "";
            }
        }

        public bool IsDeleted { get; set; }

        #region　扩展属性
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public DateTime WeekDayEnd
        {
            get { return WeekDay.AddDays(6); }
        }

        public string EditLastName { get; set; }

        public string EditFirstName { get; set; }

        public string FirstAndLastName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public string LastNameAndFirst
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }
        #endregion


        public string UserName
        {
            get { return FirstAndLastName; }
            set { throw new NotImplementedException(); }
        }
    }
}
