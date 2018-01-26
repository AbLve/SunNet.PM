using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Entity.ProjectModel
{
    public class ProjectsFactory
    {
        public static ProjectsEntity CreateProject(int createUserID, ISystemDateTime datetimeProvider)
        {
            ProjectsEntity model = new ProjectsEntity();

            model.ID = 0;
            model.CreatedBy = createUserID;
            model.CreatedOn = datetimeProvider.Now;
            model.ModifiedBy = createUserID;
            model.ModifiedOn = datetimeProvider.Now;

            model.Billable = false;
            model.BugNeedApproved = false;
            model.CompanyID = 0;
            model.Description = string.Empty;
            model.EndDate = datetimeProvider.Now;
            model.FreeHour = 0;
            model.IsOverFreeTime = false;
            model.PMID = 0;
            model.Priority = string.Empty;
            model.ProjectCode = string.Empty;
            model.ProjectID = 0;
            model.RequestNeedApproved = false;
            model.StartDate = datetimeProvider.Now;
            model.Status = ProjectStatus.Open;
            model.TestLinkURL = string.Empty;
            model.TestPassword = string.Empty;
            model.TestUserName = string.Empty;
            model.Title = string.Empty;

            return model;
        }
        public static ProjectUsersEntity CreateProjectUser(int createUserID, ISystemDateTime datetimeProvider)
        {
            ProjectUsersEntity model = new ProjectUsersEntity();

            model.ID = 0;
            model.CreatedBy = createUserID;
            model.CreatedOn = datetimeProvider.Now;
            model.ModifiedBy = createUserID;
            model.ModifiedOn = datetimeProvider.Now;

            model.ISClient = false;
            model.ProjectID = 0;
            model.UserID = 0;

            return model;
        }
    }
}
