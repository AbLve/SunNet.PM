using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.ProjectModel
{
    public class ProjectDetailDTO : ProjectsEntity
    {

        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static new ProjectDetailDTO ReaderBind(IDataReader dataReader)
        {
            ProjectDetailDTO model = new ProjectDetailDTO();
            object ojb = dataReader["ProjectID"];
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
            model.CompanyName = dataReader["CompanyName"].ToString();
            model.PMUserName = dataReader["PMUserName"].ToString();
            model.PMFirstName = dataReader["PMFirstName"].ToString();
            model.PMLastName = dataReader["PMLastName"].ToString();
            model.CreatedByUserName = dataReader["CreatedByUserName"].ToString();
            model.ModifiedByUserName = dataReader["ModifiedByUserName"].ToString();
            return model;
        }
        public string PMUserName { get; set; }
        public string PMFirstName { get; set; }
        public string PMLastName { get; set; }

        public string ModifiedByUserName { get; set; }
        public string CreatedByUserName { get; set; }
        public string CompanyName { get; set; }
    }
}
