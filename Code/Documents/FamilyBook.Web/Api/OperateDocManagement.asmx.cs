using FamilyBook.Business.DocManagement;
using FamilyBook.Entity.DocManagements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace PM.Document.Web.Api
{
    /// <summary>
    /// OperateDocManagement 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class OperateDocManagement : System.Web.Services.WebService
    {

        [WebMethod]
        public bool AddDocManagement(string value)
        {
            var obj = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(value);
            if (obj.Count > 0)
            {
                DocManagementEntity entity = new DocManagementEntity();
                int projectId = 0;
                int.TryParse(obj[0]["ProjectId"].ToString(), out projectId);
                entity.ProjectID = projectId;
                int companyId = 0;
                int.TryParse(obj[0]["CompanyID"].ToString(), out companyId);
                entity.CompanyID = companyId;
                int createBy = 0;
                int.TryParse(obj[0]["CreatedBy"].ToString(), out createBy);
                entity.UserID = createBy;
                entity.ParentID = 0;
                entity.FileContentType = obj[0]["ContentType"].ToString();
                entity.FileUrl = obj[0]["FilePath"].ToString();
                entity.CreatedOn = DateTime.Now;
                entity.DisplayFileName = obj[0]["FileTitle"].ToString();
                int filesize = 0;
                int.TryParse(obj[0]["FileSize"].ToString(), out filesize);
                entity.FileSize = filesize;
                entity.FileName = obj[0]["FilePath"].ToString().Substring(obj[0]["FilePath"].ToString().LastIndexOf("/") + 1);
                entity.Extenstions = obj[0]["FilePath"].ToString().Substring(obj[0]["FilePath"].ToString().LastIndexOf("."));
                entity.Type = DocType.File;
                return new DocManagementBusiness().Insert(entity) > 0;
            }
            return false;
        }

        [WebMethod]
        public string GetFileInfo(int id)
        {
            DocManagementEntity entity = new DocManagementEntity();
            entity = new DocManagementBusiness().Get(id);
            return entity.FileUrl + "|" + entity.DisplayFileName;
        }
    }
}
