using System;
using System.Collections.Generic;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.publicpage
{
    public partial class Menu : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //document forum;

            var selected = new Dictionary<string, SelectedSunnetMenu>();
            selected.Add("document", SelectedSunnetMenu.Document);
            selected.Add("forum", SelectedSunnetMenu.Forum);

            this.ClientMenu1.CurrentModule = Request.Params["module"];
            if (selected.ContainsKey(Request.Params["module"]))
            {
                this.SunnetMenu1.CurrentModule = selected[Request.Params["module"]];
            }

            if (!IsPostBack)
            {
                if (UserInfo.Role == RolesEnum.CLIENT)
                {
                    ClientMenu1.Visible = true;
                }
                else
                {
                    SunnetMenu1.Visible = true;
                }
            }
        }

        private bool ISIE
        {
            get
            {
                return Request.Browser.Browser.Equals("ie", StringComparison.CurrentCultureIgnoreCase)
                    || Request.Browser.Browser.Equals("InternetExplorer", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        /// <summary>
        /// IE - WebBrowser 模拟器以IE8模式运行.
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/14 17:28
        public string IEEmulator
        {
            get
            {
                if (!ISIE)
                    return "";
                var version = 8.0f;
                if (float.TryParse(Request.Browser.Version, out version) && version < 8)
                {
                    //var ies =
                    //    Request.Browser.Browsers.ToArray()
                    //        .Select(x => x.ToString())
                    //        .ToList()
                    //        .Find(x => x.Contains("to"));
                    //if (ies != null)
                    //    float.TryParse(ies.Substring(ies.IndexOf("to") + 2), out version);
                    version = 8;
                }
                return string.Format("<meta http-equiv='X-UA-Compatible' content='IE=EmulateIE{0}' />", (int)version);
            }
        }
    }
}