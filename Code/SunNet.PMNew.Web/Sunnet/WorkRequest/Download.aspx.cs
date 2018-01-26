using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;

namespace SunNet.PMNew.Web.Sunnet.WorkRequest
{
    public partial class Download : BaseWebsitePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string filePath = QS("FilePath");
                string fileName = QS("FileName");
                if (filePath.Length > 0)
                {
                    Response.ContentType = "application/x-zip-compressed";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                    //string filename = Server.MapPath(@"~\upload\" + prjname + "/" + this.FileName);
                    Response.TransmitFile(filePath);
                }
            }
        }
    }
}
