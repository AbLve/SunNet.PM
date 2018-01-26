using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.Core.CompanyModule
{
    //Companys
    public class CompanysEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static CompanysEntity ReaderBind(IDataReader dataReader)
        {
            CompanysEntity model = new CompanysEntity();
            object ojb;
            ojb = dataReader["ComID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ComID = (int)ojb;
            }
            model.CompanyName = dataReader["CompanyName"].ToString();
            model.Phone = dataReader["Phone"].ToString();
            model.Fax = dataReader["Fax"].ToString();
            model.Website = dataReader["Website"].ToString();
            model.AssignedSystemUrl = dataReader["AssignedSystemUrl"].ToString();
            model.Address1 = dataReader["Address1"].ToString();
            model.Address2 = dataReader["Address2"].ToString();
            model.City = dataReader["City"].ToString();
            model.State = dataReader["State"].ToString();
            model.Logo = dataReader["Logo"].ToString();
            model.Status = dataReader["Status"].ToString();
            ojb = dataReader["CreatedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedOn = (DateTime)ojb;
            }
            ojb = dataReader["CreatedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedBy = (int)ojb;
            }
            model.CreateUserName = dataReader["CreateUserName"].ToString();
            ojb = dataReader["ModifiedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedOn = (DateTime)ojb;
            }
            ojb = dataReader["ModifiedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedBy = (int)ojb;
            }
            return model;
        }
        /// <summary>
        /// ComID
        /// </summary>		
        public int ComID { get; set; }
        /// <summary>
        /// CompanyName
        /// </summary>		
        public string CompanyName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>		
        public string Phone { get; set; }
        /// <summary>
        /// Fax
        /// </summary>		
        public string Fax { get; set; }
        /// <summary>
        /// Website
        /// </summary>		
        public string Website { get; set; }
        /// <summary>
        /// AssignedSystemUrl
        /// </summary>		
        public string AssignedSystemUrl { get; set; }
        /// <summary>
        /// Address1
        /// </summary>		
        public string Address1 { get; set; }
        /// <summary>
        /// Address2
        /// </summary>		
        public string Address2 { get; set; }
        /// <summary>
        /// City
        /// </summary>		
        public string City { get; set; }
        /// <summary>
        /// State
        /// </summary>		
        public string State { get; set; }
        /// <summary>
        /// Logo
        /// </summary>		
        public string Logo { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        public string Status { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        public int CreatedBy { get; set; }
        /// <summary>
        /// CreateUserName
        /// </summary>		
        public string CreateUserName { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>		
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// ModifiedBy
        /// </summary>		
        public int ModifiedBy { get; set; }

    }
}