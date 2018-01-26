using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class UploadFile : BaseAscx
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region attribute

        public string PrimaryKey
        {
            set { hidKey.Value = value; }
        }

        public string ProjectName { get; set; }

        public string FileList
        {
            get { return hd_UpdateFileValue.Value; }
        }
        public string DeleteFileList
        {
            get { return hd_DeleteFileValue.Value; }
        }
        public bool EnableUpload
        {
            set
            {
                divUploadFile.Visible = false;
            }
        }
        #endregion
    }
}