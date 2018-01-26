using SunNet.PMNew.App;
using SunNet.PMNew.Entity.KPIModel;
using System;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin.KPI
{
    public partial class AddCategory : System.Web.UI.Page
    {
        KPICategoryApplications KPIApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            KPIApp = new KPICategoryApplications();
        }
        private KPICategoriesEntity GetEntity()
        {
            KPICategoriesEntity model = KPICategoriesEntity.CreateKPICategoriesEntity();

            model.CategoryName = txtCategoryName.Text.NoHTML();
            model.Status = int.Parse(ddlCategoryStatus.SelectedValue);
            return model;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {   
            KPICategoriesEntity model = GetEntity();

            int id = KPIApp.AddCategory(model);
            /*if (id > 0)
            {
                 Redirect(EmptyPopPageUrl, false, true);
            }
            else
            {
                this.ShowFailMessageToClient(KPIApp.BrokenRuleMessages);   
            }*/
            
        }

    }
}
