using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class NewCateGory : BaseWebsitePage
    {
        CateGoryApplication ccApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            ccApp = new CateGoryApplication();
        }
        private CateGoryEntity GetEntity()
        {
            CateGoryEntity model = CateGoryFactory.CreateCateGoryEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
            model.Title = txtTitle.Text;
            return model;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                CateGoryEntity model = GetEntity();
                int id = ccApp.AddCateGory(model);
                if (id > 0)
                {
                    ShowSuccessMessageToClient();
                }
                else
                {
                    ShowFailMessageToClient(ccApp.BrokenRuleMessages);
                }
            }
        }
    }
}
