using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using System.Text;
using SunNet.PMNew.Entity.FileModel;

namespace SunNet.PMNew.Web.Sunnet.Documents
{
    public partial class MoveObjects : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.Params["objects"]))
            {
                ShowArgumentErrorMessageToClient();
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int target = 0;
            if (!int.TryParse(hidSelectedDirectory.Value, out target))
            {
                ShowMessageToClient("Please select a directory", 0, false, false);
                return;
            }
            FileApplication fileApp = new FileApplication();
            string[] items = Request.Params["objects"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (items.Length > 0)
            {
                StringBuilder sbdireids = new StringBuilder("0,");
                StringBuilder sbobjids = new StringBuilder("0,");
                foreach (string item in items)
                {
                    string[] keyorvalue = item.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (keyorvalue.Length == 2)
                    {
                        int id = 0;
                        if (int.TryParse(keyorvalue[1], out id) && keyorvalue[0] == DirectoryObjectType.Directory.ToString())
                        {
                            sbdireids.Append(id);
                            sbdireids.Append(",");
                        }
                        else
                        {
                            sbobjids.Append(id);
                            sbobjids.Append(",");
                        }
                    }
                }
                sbdireids.Append(";");
                sbdireids.Append(sbobjids);
                if (!fileApp.ChangeParent(sbdireids.ToString(), target))
                {
                    ShowFailMessageToClient(fileApp.BrokenRuleMessages);
                }
                else
                {
                    ShowSuccessMessageToClient();
                }
            }
        }
    }
}
