using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace SF.Framework.Core
{
    public class BaseEntity
    {
        private static readonly BaseEntity _be = new BaseEntity();
        /// <summary>
        /// Instance of BaseEntity
        /// </summary>
        public static BaseEntity DefaultInstance { get { return _be; } }

        /// <summary>
        /// Min date in system
        /// </summary>
        [NotMapped]
        public DateTime MinDate
        {
            get { return DateTime.Parse("1753-1-2 12:00:00 AM").Date; }
        }
        /// <summary>
        ///  Format date to a right format
        /// </summary>
        /// <param name="date">a date type </param>
        /// <returns></returns>
        public string FormatDate(object date)
        {
            if (date == null)
                return string.Empty;
            DateTime dt;
            if (DateTime.TryParse(date.ToString(), out dt))
            {
                if (dt.Equals(MinDate) || dt < MinDate)
                    return string.Empty;
                return dt.ToString("MM/dd/yyyy");
            }
            return string.Empty;
        }

        /// <summary>
        ///  Max date in system
        /// </summary>
        [NotMapped]
        public DateTime MaxDate
        {
            get { return DateTime.Parse("2100-1-1 12:00:00 AM").Date; }
        }
        /// <summary>
        /// Default phone
        /// </summary>
        [NotMapped]
        public string EmptyPhone
        {
            get { return "000-000-0000"; }
        }

        /// <summary>
        /// Format phone to a right format
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string FormatPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)
                || phone.Equals(this.EmptyPhone)
                || phone.Length < this.EmptyPhone.Length - 2)
                return "";
            if (phone.IndexOf("-") < 1)
            {
                return string.Format("{0}-{1}-{2}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6));
            }
            return phone;
        }
        /// <summary>
        /// Check a phone is empty value
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool ISEmptyPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)
                   || phone.Equals(this.EmptyPhone)
                   || phone.Length < this.EmptyPhone.Length - 2)
                return true;
            return false;
        }

        public string TrimEnd(object sourceString, string chars = ", ")
        {
            if (sourceString == null || string.IsNullOrEmpty(sourceString.ToString()))
                return string.Empty;
            return sourceString.ToString().TrimEnd(chars.ToCharArray());
        }

    }
}
