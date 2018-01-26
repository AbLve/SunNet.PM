using System;
using System.IO;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using System.Collections.Generic;
using SunNet.PMNew.Entity.ComplaintModel.Enums;
using System.Web;
using System.Web.Script.Serialization;

namespace SunNet.PMNew.PM2014.OA.Complaints
{
    public partial class ComplaintReview : BasePage
    {
        public ComplaintApplication complaintApp;
        public ComplaintEntity cmplEntity = new ComplaintEntity();
        public ComplaintItem comItem;

        protected void Page_Load(object sender, EventArgs e)
        {
            complaintApp = new ComplaintApplication();

            var master = (Pop)this.Master;
            if (master != null) master.Width = 580;

            if (!IsPostBack)
            {
                int complaintID = QS("ComplaintID", 0);
                ltlComplaintID.Text = complaintID.ToString();

                IComplaintRepository comRepository = ObjectFactory.GetInstance<IComplaintRepository>();
                cmplEntity = comRepository.Get(complaintID);

                GetComplaintItem();

                IComplaintHistoryRepository comHisRepository = ObjectFactory.GetInstance<IComplaintHistoryRepository>();
                List<ComplaintHistoryEntity>  list = comHisRepository.GetHistorysByComID(complaintID);

                if (null != list && list.Count > 0)
                {
                    this.rptComplaintHistoryList.DataSource = list;
                }
                else
                {
                    this.trNoComments.Visible = true;
                    this.rptComplaintHistoryList.DataSource = new List<ComplaintHistoryEntity>();
                }

                this.rptComplaintHistoryList.DataBind();
            }
        }

        //[System.Web.Services.WebMethod]
        //public static void GetComplaintItem(HttpContext context)
        //{
        //    //Get System properties from System table
        //    ISystemRepository systemRepository = ObjectFactory.GetInstance<ISystemRepository>();
        //    SystemEntity sysEntity = systemRepository.GetBySysName(cmplEntity.SystemName);

        //    //Get Item Path
        //    string serverName = sysEntity.IP + (sysEntity.Port.Length>0 ? ":" + sysEntity.Port : "");
        //    string connStr = String.Format("server={0};database={1};uid={2};pwd={3};max pool size =1024000", 
        //                                    serverName, sysEntity.DBLocation, sysEntity.UserName, sysEntity.UserPwd);
        //    string type = ((ComplaintTypeEnum)cmplEntity.Type).ToString();

        //    string path = complaintApp.GetComItem(connStr, sysEntity.Procedure, type, cmplEntity.TargetID);

        //    HelpPath helpPath = new HelpPath();
        //    helpPath.Type = type;
        //    helpPath.Path = path;

        //    JavaScriptSerializer jss = new JavaScriptSerializer();
        //    context.Response.Write(jss.Serialize(helpPath));

        //    //if (type == "Photo")
        //    //{
        //    //    this.comImg.ImageUrl = path;
        //    //    this.comImg.Visible = true;
        //    //}
        //    //else if (type == "Video")
        //    //{
        //    //    //ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>setPlayerVideo();</script>");
        //    //    context.Response.Write(path);
        //    //}
        //}


        public void GetComplaintItem()
        {
            //Get System properties from System table
            ISystemRepository systemRepository = ObjectFactory.GetInstance<ISystemRepository>();
            SystemEntity sysEntity = systemRepository.GetBySysName(cmplEntity.SystemName);

            //Get Item Path
            string serverName = sysEntity.IP + (sysEntity.Port.Length > 0 ? ":" + sysEntity.Port : "");
            string connStr = String.Format("server={0};database={1};uid={2};pwd={3};max pool size =1024000",
                                            serverName, sysEntity.DBLocation, sysEntity.UserName, sysEntity.UserPwd);
            string type = ((ComplaintTypeEnum)cmplEntity.Type).ToString();

            string result = complaintApp.GetComItem(connStr, sysEntity.Procedure, type, cmplEntity.TargetID);
            if (string.IsNullOrEmpty(result))
            {
                comItem = new ComplaintItem();
                return;
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            comItem = (ComplaintItem)jss.Deserialize(result, typeof(ComplaintItem));

            if (type == "Photo")
            {
                this.comImg.Style["display"] = "inline";
            }
            else if (type == "Video")
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "setPlayerVideo('" + comItem.Path + "');", true);
            }
            else if (type == "User")
            {
                this.comUser.Style["display"] = "inline";
            }
            else if (type == "Group")
            {
                this.comGroup.Style["display"] = "inline";
            }
            else if (type == "Post")
            {
                this.comPost.Style["display"] = "inline";
            }
        }


        protected void btnOK_Click(object sender, EventArgs e)
        {
            IComplaintRepository comRepository = ObjectFactory.GetInstance<IComplaintRepository>();
            cmplEntity = comRepository.Get(QS("ComplaintID", 0));

            // Update Complaint History
            ComplaintHistoryEntity comHisEntity = new ComplaintHistoryEntity();
            comHisEntity.ComplaintID = QS("ComplaintID", 0);
            comHisEntity.ModifiedOn = DateTime.Now;
            comHisEntity.ModifiedByID = UserInfo.UserID;
            comHisEntity.Comments = txtComments.Text;
            string actionStr = ((ComplaintStatusEnum)this.cmplEntity.Status).ToString() + " To " + ddlAction.SelectedValue;
            comHisEntity.Action = actionStr;
            IComplaintHistoryRepository comHisRepository = ObjectFactory.GetInstance<IComplaintHistoryRepository>();
            comHisRepository.Insert(comHisEntity);

            // Update Complaint
            ComplaintEntity newCmplEntity = new ComplaintEntity();
            newCmplEntity.Comments = txtComments.Text;
            newCmplEntity.UpdatedOn = DateTime.Now;
            newCmplEntity.UpdatedByID = UserInfo.UserID;
            newCmplEntity.ComplaintID = QS("ComplaintID", 0);

            switch (ddlAction.SelectedValue)
            {
                case "DELETE":
                    //Response.Redirect("http://localhost:2777/Complaint/Complaint/Delete?id=" + newCmplEntity.ComplaintID + "&type=" + (SunNet.PMNew.Entity.ComplaintModel.Enums.ComplaintTypeEnum)cmplEntity.Type + "&returnUrl=http://localhost:27273/OA/Complaints/Complaints.aspx");

                    //Get System properties from System table
                    ISystemRepository systemRepository = ObjectFactory.GetInstance<ISystemRepository>();
                    SystemEntity sysEntity = systemRepository.GetBySysName(cmplEntity.SystemName);

                    //Delete Item
                    string serverName = sysEntity.IP + (sysEntity.Port.Length > 0 ? ":" + sysEntity.Port : "");
                    string connStr = String.Format("server={0};database={1};uid={2};pwd={3};max pool size =1024000",
                                                    serverName, sysEntity.DBLocation, sysEntity.UserName, sysEntity.UserPwd);
                    string type = ((ComplaintTypeEnum)cmplEntity.Type).ToString();

                    if (complaintApp.DeleteComItem(connStr, sysEntity.Procedure, type, cmplEntity.TargetID))
                        Response.Write("Deletion Succeeded.");
                    else
                        Response.Write("Deletion Failed.");

                    newCmplEntity.Status = 1;
                    
                    break;

                case "APPROVEBUTNOTDEL":
                    newCmplEntity.Status = 2;
                    break;
                case "DENY":
                    newCmplEntity.Status = 3;
                    break;
            }

            complaintApp.UpdateComplaint(newCmplEntity);

            Redirect(EmptyPopPageUrl, false, true);
        }

    }
}