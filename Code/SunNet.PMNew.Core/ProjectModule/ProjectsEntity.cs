using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using SunNet.PMNew.Framework.Core;
namespace SunNet.PMNew.Core.ProjectModule
{
    //Projects
    public class ProjectsEntity:BaseEntity
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
                model.Status = (int)ojb;
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
            return model;
        }
        /// <summary>
        /// ProjectID
        /// </summary>		
        public int ProjectID { get; set; }
        /// <summary>
        /// CompanyID
        /// </summary>		
        public int CompanyID { get; set; }
        /// <summary>
        /// ProjectCode
        /// </summary>		
        public string ProjectCode { get; set; }
        /// <summary>
        /// Title
        /// </summary>		
        public string Title { get; set; }
        /// <summary>
        /// Description
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// StartDate
        /// </summary>		
        public DateTime StartDate { get; set; }
        /// <summary>
        /// EndDate
        /// </summary>		
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>		
        public int CreatedBy { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// ModifiedBy
        /// </summary>		
        public int ModifiedBy { get; set; }
        /// <summary>
        /// ModifiedOn
        /// </summary>		
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// PMID
        /// </summary>		
        public int PMID { get; set; }
        /// <summary>
        /// Priority
        /// </summary>		
        public string Priority { get; set; }
        /// <summary>
        /// Billable
        /// </summary>		
        public bool Billable { get; set; }
        /// <summary>
        /// TestLinkURL
        /// </summary>		
        public string TestLinkURL { get; set; }
        /// <summary>
        /// TestUserName
        /// </summary>		
        public string TestUserName { get; set; }
        /// <summary>
        /// TestPassword
        /// </summary>		
        public string TestPassword { get; set; }
        /// <summary>
        /// FreeHour
        /// </summary>		
        public int FreeHour { get; set; }
        /// <summary>
        /// BugNeedApproved
        /// </summary>		
        public bool BugNeedApproved { get; set; }
        /// <summary>
        /// RequestNeedApproved
        /// </summary>		
        public bool RequestNeedApproved { get; set; }
        /// <summary>
        /// IsOverFreeTime
        /// </summary>		
        public bool IsOverFreeTime { get; set; }

    }
}