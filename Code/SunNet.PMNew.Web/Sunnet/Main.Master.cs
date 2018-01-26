using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.CompanyModel;
using System.IO;
using System.Drawing;

namespace SunNet.PMNew.Web.Sunnet
{
    public partial class Main : System.Web.UI.MasterPage
    {
        public HiddenField HidOrderBy
        {
            get
            {
                if (null != this.Page.FindControl("hidOrderBy"))
                    return (HiddenField)this.Page.FindControl("hidOrderBy");
                else
                    return new HiddenField();
            }
        }


        public HiddenField HidOrderDirection
        {
            get
            {
                if (null != this.Page.FindControl("hidOrderDirection"))
                    return (HiddenField)this.Page.FindControl("hidOrderDirection");
                else
                    return new HiddenField();
            }
        }


        public UsersEntity UserInfo
        {
            get
            {
                BaseWebsitePage bwPage = this.Page as BaseWebsitePage;
                return bwPage.UserInfo;
            }
        }


        public int TopSelectedIndex
        {
            set
            {
                TopMenu1.CurrentIndex = value;
                LeftMenu1.ParentID = value;
            }
        }


        public int LeftSelectedIndex
        {
            set { LeftMenu1.CurrentIndex = value; }
        }


        public int LeftSecondSelectedIndex
        {
            set { LeftMenu1.CurrentSecondIndex = value; }
        }


        private void InitCompanyInfo()
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            CompanyApplication comApp = new CompanyApplication();
            CompanysEntity company = comApp.GetCompany(UserInfo.CompanyID);
            if (UserInfo.CompanyID == 1)  //Sunnet 公司
            {
                ltLogo.Text = string.Format("<a href=\"http://www.sunnet.us\" target=\"_blank\">{0}</a>"
                    , BuilderLogo(company.Logo));
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LtLogo"),
            encrypt.Encrypt(ltLogo.Text), DateTime.Now.AddMinutes(30));
            }
            else
            {
                CompanysEntity sunntCompanyEntity = comApp.GetCompany(1);//获取Sunnet公司

                if (company.Logo.IndexOf("logomain.jpg") >= 0) //没有上传Logo ，则显示 Sunnet 公司Logo
                {
                    ltLogo.Text = string.Format("<a href=\"http://www.sunnet.us\" target=\"_blank\">{0}</a>"
                   , BuilderLogo(sunntCompanyEntity.Logo));
                    UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LtLogo"),
                encrypt.Encrypt(ltLogo.Text), DateTime.Now.AddMinutes(30));
                }
                else
                {
                    ltLogo.Text = BuilderLogo(company.Logo);

                    ltSunnetLogo.Text = string.Format("<a href=\"http://www.sunnet.us\" target=\"_blank\">{0}</a>"
          , BuilderLogo(sunntCompanyEntity.Logo));

                   
                    UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LtLogo"),
                encrypt.Encrypt(ltLogo.Text), DateTime.Now.AddMinutes(30));
                    UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LtSunnetLogo"),
                encrypt.Encrypt(ltSunnetLogo.Text), DateTime.Now.AddMinutes(30));
                }
            }
        }

        private string BuilderLogo(string image)
        {
            string filename = Server.MapPath(image);
            if (File.Exists(filename))
            {
                return string.Format("<img style=\"height:39px;width:126px;border-width:0px;\" src=\"{0}\"/>", image);
            }
            else return string.Format("<img style=\"height:39px;width:126px;border-width:0px;\" src=\"{0}\"/>", "/images/logomain.jpg");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TopMenu1.ParentID = 1;

            UserApplication userApp = new UserApplication();

            string keyPath = "/sunnet/";
            string url = Request.Url.AbsolutePath.ToLower();

            string topModule = url.Replace(keyPath, "");
            topModule = topModule.Substring(0, topModule.IndexOf("/") + 1);
            topModule = keyPath + topModule;

            ModulesEntity moduleTop = userApp.GetModule(topModule);
            if (moduleTop != null)
            {
                if (UserInfo.Role == RolesEnum.CLIENT && moduleTop.ModuleTitle.Trim().ToLower() == "clients")
                {
                    moduleTop.ModuleTitle = "Tickets";
                }
                this.TopSelectedIndex = moduleTop.ID;
                ltlCurrentModule.Text = moduleTop.ModuleTitle;
            }

            //url += Request.Url.Query;
            ModulesEntity moduleCurrent = userApp.GetModule(url);
            if (moduleCurrent != null)
            {
                this.LeftSelectedIndex = moduleCurrent.ParentID;
                this.LeftSecondSelectedIndex = moduleCurrent.ID;
            }

            //CateGory
            CateGoryApplication cgApp = new CateGoryApplication();
            List<CateGoryEntity> listCC = cgApp.GetCateGroyListByUserID(UserInfo.ID);
            rptCategory.DataSource = listCC;
            rptCategory.DataBind();
            rptCateGory2.DataSource = listCC;
            rptCateGory2.DataBind();

            // Client Or Sunnet
            topMenu_category.Visible = UserInfo.Role != RolesEnum.CLIENT;
            InitCompanyInfo();
        }
    }
}
