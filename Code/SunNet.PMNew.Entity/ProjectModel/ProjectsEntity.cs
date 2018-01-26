using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;
using System.ComponentModel.DataAnnotations;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Entity.ProjectModel
{
    //Projects
    public class ProjectsEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static ProjectsEntity ReaderBind(IDataReader dataReader)
        {
            ProjectsEntity model = new ProjectsEntity();
            object ojb;
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
                model.ID = model.ProjectID;
            }
            ojb = dataReader["CompanyID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CompanyID = (int)ojb;
            }
            model.ProjectCode = dataReader["ProjectCode"].ToString();
            model.Title = dataReader["Title"].ToString();
            model.Description = dataReader["Description"].ToString();
            ojb = dataReader["StartDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.StartDate = (DateTime)ojb;
            }
            ojb = dataReader["EndDate"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EndDate = (DateTime)ojb;
            }
            ojb = dataReader["Status"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Status = (ProjectStatus)ojb;
            }
            ojb = dataReader["CreatedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedBy = (int)ojb;
            }
            ojb = dataReader["CreatedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedOn = (DateTime)ojb;
            }
            ojb = dataReader["ModifiedBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedBy = (int)ojb;
            }
            ojb = dataReader["ModifiedOn"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ModifiedOn = (DateTime)ojb;
            }
            ojb = dataReader["PMID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.PMID = (int)ojb;
            }
            model.Priority = dataReader["Priority"].ToString();
            ojb = dataReader["Billable"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Billable = (bool)ojb;
            }
            model.TestLinkURL = dataReader["TestLinkURL"].ToString();
            model.TestUserName = dataReader["TestUserName"].ToString();
            model.TestPassword = dataReader["TestPassword"].ToString();
            ojb = dataReader["FreeHour"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.FreeHour = (int)ojb;
            }
            ojb = dataReader["BugNeedApproved"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.BugNeedApproved = (bool)ojb;
            }
            ojb = dataReader["RequestNeedApproved"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.RequestNeedApproved = (bool)ojb;
            }
            ojb = dataReader["IsOverFreeTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsOverFreeTime = (bool)ojb;
            }
            ojb = dataReader["TotalHours"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TotalHours = float.Parse(ojb.ToString());
            }
            ojb = dataReader["MaintenancePlanOption"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.MaintenancePlanOption = (string)ojb;
            }
            return model;
        }



        /// <summary>
        /// ProjectID
        /// </summary>		
        public int ProjectID { get; set; }
        /// <summary>
        /// CompanyID
        /// </summary>		
        [Required]
        public int CompanyID { get; set; }
        /// <summary>
        /// ProjectCode
        /// </summary>		
        [Required]
        [StringLength(64)]
        public string ProjectCode { get; set; }
        /// <summary>
        /// Title
        /// </summary>		
        [Required]
        [StringLength(128)]
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>		
        [Required]
        [StringLength(4000)]
        public string Description { get; set; }
        /// <summary>
        /// StartDate
        /// </summary>		
        [Required]
        [Range(typeof(DateTime), "1753-1-1", "2112-1-1")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// EndDate
        /// </summary>		
        [Required]
        [Range(typeof(DateTime), "1753-1-1", "2112-1-1")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        [Required]
        public ProjectStatus Status { get; set; }

        /// <summary>
        /// CreatedBy
        /// </summary>		
        [Required]
        public int CreatedBy { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        [Required]
        [Range(typeof(DateTime), "1753-1-1", "2112-1-1")]
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// ModifiedBy
        /// </summary>		
        [Required]
        public int ModifiedBy { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>		
        [Required]
        [Range(typeof(DateTime), "1753-1-1", "2112-1-1")]
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// PMID
        /// </summary>		
        [Required]
        public int PMID { get; set; }
        /// <summary>
        /// Priority
        /// </summary>		
        [Required]
        [StringLength(128)]
        public string Priority { get; set; }
        /// <summary>
        /// Billable
        /// </summary>		
        [Required]
        public bool Billable { get; set; }
        /// <summary>
        /// TestLinkURL
        /// </summary>		
        [StringLength(250)]
        [RegularExpression(@"[a-zA-z]+://[^\s]*", ErrorMessage = "TestLinkURL format error,sample: http://www.google.com")]
        public string TestLinkURL { get; set; }
        /// <summary>
        /// TestUserName
        /// </summary>		
        [StringLength(50)]
        public string TestUserName { get; set; }
        /// <summary>
        /// TestPassword
        /// </summary>		
        [StringLength(50)]
        public string TestPassword { get; set; }
        /// <summary>
        /// FreeHour
        /// </summary>		
        [Required]
        public int FreeHour { get; set; }
        /// <summary>
        /// BugNeedApproved
        /// </summary>		
        [Required]
        public bool BugNeedApproved { get; set; }
        /// <summary>
        /// RequestNeedApproved
        /// </summary>		
        [Required]
        public bool RequestNeedApproved { get; set; }
        /// <summary>
        /// IsOverFreeTime
        /// </summary>		
        [Required]
        public bool IsOverFreeTime { get; set; }

        [Required]
        public float TotalHours { get; set; }


        [Required]
        public string MaintenancePlanOption { get; set; }

        public UserMaintenancePlanOption MainPlanOption
        {
            get
            {
                if (string.IsNullOrEmpty(this.MaintenancePlanOption))
                {
                    return UserMaintenancePlanOption.NONE;
                }
                return (UserMaintenancePlanOption)System.Enum.Parse(typeof(UserMaintenancePlanOption), this.MaintenancePlanOption, true);
            }
        }

        public bool IsNeedClientEstimate
        {
            get
            {
                if (this.MainPlanOption == UserMaintenancePlanOption.HAS)
                {
                    return false;
                }
                else if (this.MainPlanOption == UserMaintenancePlanOption.NO)
                {
                    return true;
                }
                else if (this.MainPlanOption == UserMaintenancePlanOption.NEEDAPPROVAL)
                {
                    return true;
                }
                else if (this.MainPlanOption == UserMaintenancePlanOption.DONTNEEDAPPROVAL)
                {
                    return false;
                }
                else if (this.MainPlanOption == UserMaintenancePlanOption.ALLOWME)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}