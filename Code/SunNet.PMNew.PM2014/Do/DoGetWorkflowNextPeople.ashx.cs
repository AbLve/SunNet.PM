using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Entity.SealModel.Enum;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// Summary description for DoGetWorkflowNextPeople
    /// </summary>
    public class DoGetWorkflowNextPeople : DoBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request.QueryString["RequestType"];
            int requestID = int.Parse(context.Request.QueryString["RequestID"]);
            WorkflowAction action = (WorkflowAction)Enum.Parse(typeof(WorkflowAction), context.Request.QueryString["Action"]);

            List<UsersEntity> lstUser = new List<UsersEntity>();

            // Get some entity
            SealsApplication sealApp = new SealsApplication();
            SealRequestsEntity sealRequestEntity = sealApp.GetSealRequests(requestID);

            UserApplication userApp = new UserApplication();

            if (action == WorkflowAction.Deny || action == WorkflowAction.FinishProcess)
            {
                lstUser.Add(userApp.GetUser(sealRequestEntity.RequestedBy));
            }

            else if (action == WorkflowAction.Cancel || action == WorkflowAction.Complete || action == WorkflowAction.Pending || action == WorkflowAction.Save)
            { }

            else if (type == "Seal")
            {
                // Get Seal entity list
                List<SealUnionRequestsEntity> listSealUnionReq = sealApp.GetSealUnionRequestsList(requestID);
                List<SealsEntity> listSeal = new List<SealsEntity>();
                foreach(SealUnionRequestsEntity ent in listSealUnionReq)
                {
                    SealsEntity sealEntity = sealApp.GetList().Find(r=> r.ID == ent.SealID);
                    listSeal.Add(sealEntity);
                }

                if (action == WorkflowAction.Submit)
                {
                    foreach (SealsEntity ent in listSeal)
                    {
                        lstUser.Add(userApp.GetUser(ent.Approver));
                    }
                }
                else if (action == WorkflowAction.Approve)
                {
                    foreach (SealsEntity ent in listSeal)
                    {
                        lstUser.Add(userApp.GetUser(ent.Owner));
                    }
                }
                else
                {
                    SearchUsersRequest requestUser = new SearchUsersRequest(SearchUsersType.All, false, " FirstName ", " ASC ");
                    requestUser.IsSunnet = true;
                    SearchUserResponse responseuser = userApp.SearchUsers(requestUser);
                    lstUser = responseuser.ResultList.FindAll(r =>  r.ID != UserInfo.ID && r.Status == "ACTIVE");
                }
            }

            else
            {
                SearchUsersRequest requestUser = new SearchUsersRequest(SearchUsersType.All, false, " FirstName ", " ASC ");
                requestUser.IsSunnet = true;
                SearchUserResponse responseuser = userApp.SearchUsers(requestUser);
                lstUser = responseuser.ResultList.FindAll(r => r.ID != UserInfo.ID && r.Status == "ACTIVE");//(r.Role == RolesEnum.PM || r.Role== RolesEnum.Sales) && 
            }

            lstUser = lstUser.Distinct().ToList();

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("list", lstUser);
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}