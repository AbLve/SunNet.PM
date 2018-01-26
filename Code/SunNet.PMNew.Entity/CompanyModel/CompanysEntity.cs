using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;
using System.ComponentModel.DataAnnotations;

namespace SunNet.PMNew.Entity.CompanyModel
{
    //Companys
    public class CompanysEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="count">Projects' count and Clients' count</param>
        /// <returns></returns>
        public static CompanysEntity ReaderBind(IDataReader dataReader, bool count)
        {
            CompanysEntity model = new CompanysEntity();
            object ojb;
            ojb = dataReader["ComID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ComID = (int)ojb;
                model.ID = model.ComID;
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
            if (count)
            {
                ojb = dataReader["ProjectsCount"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.ProjectsCount = (int)ojb;
                }
                ojb = dataReader["ClientsCount"];
                if (ojb != null && ojb != DBNull.Value)
                {
                    model.ClientsCount = (int)ojb;
                }
            }
            return model;
        }

        #region Projects' count and Clients' count
        public int ProjectsCount { get; set; }
        public int ClientsCount { get; set; }
        #endregion

        /// <summary>
        /// ComID
        /// </summary>		
        public int ComID { get; set; }
        /// <summary>
        /// CompanyName
        /// </summary>		
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>		
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        /// <summary>
        /// Fax
        /// </summary>		
        //[Required]
        [StringLength(20)]
        public string Fax { get; set; }
        /// <summary>
        /// Website
        /// </summary>		
        //[Required]
        [StringLength(100)]
        //[RegularExpression(@"[a-zA-z]+://[^\s]*",ErrorMessage="Website format error,sample: http://www.google.com")]
        public string Website { get; set; }
        /// <summary>
        /// AssignedSystemUrl
        /// </summary>		
        [Required]
        [StringLength(100)]
        [RegularExpression(@"[a-zA-z]+://[^\s]*")]
        public string AssignedSystemUrl { get; set; }
        /// <summary>
        /// Address1
        /// </summary>		
        [Required]
        [StringLength(500)]
        public string Address1 { get; set; }
        /// <summary>
        /// Address2
        /// </summary>		
        [StringLength(500)]
        public string Address2 { get; set; }
        /// <summary>
        /// City
        /// </summary>		
        [Required]
        [StringLength(100)]
        public string City { get; set; }
        /// <summary>
        /// State
        /// </summary>		
        [Required]
        [StringLength(2)]
        public string State { get; set; }
        /// <summary>
        /// Logo
        /// </summary>		
        [Required]
        [StringLength(100)]
        public string Logo { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        [Required]
        [StringLength(10)]
        public string Status { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        [Required]
        [Range(typeof(DateTime), "2012-10-1", "2112-1-1")]
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        [Required]
        public int CreatedBy { get; set; }
        /// <summary>
        /// CreateUserName
        /// </summary>		
        [Required]
        [StringLength(200)]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>		
        [Required]
        [Range(typeof(DateTime), "2012-10-1", "2112-1-1")]
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// ModifiedBy
        /// </summary>		
        [Required]
        public int ModifiedBy { get; set; }

    }
}