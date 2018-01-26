using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.SunnetTicket
{
    public partial class NewCategory : BasePage
    {
        CateGoryApplication ccApp = new CateGoryApplication();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private CateGoryEntity GetEntity()
        {
            CateGoryEntity model = CateGoryFactory.CreateCateGoryEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
            model.Title = txtTitle.Text;
            return model;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                CateGoryEntity model = GetEntity();
                int id = ccApp.AddCateGory(model);
                if (id > 0)
                {
                    Redirect(EmptyPopPageUrl, false, true);
                }
                else
                {
                    ShowFailMessageToClient(ccApp.BrokenRuleMessages);
                }
            }
        }
    }
}