using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.UserControls
{
    public partial class ClientMenu : BaseAscx
    {

        /// <summary>
        /// Module List (ticket | document | event | forum | profile).
        /// </summary>
        /// <value>
        /// The current module.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/22 14:27
        public string CurrentModule { get; set; }

        /// <summary>
        /// 设置导航的Target属性.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/22 14:35
        public string Target { get; set; }
        /// <summary>
        /// 获取每个目录菜单是否激活样式.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/22 14:26
        protected string GetMenuClass(string module)
        {
            if (string.IsNullOrEmpty(CurrentModule))
            {
                return
                    Request.Url.AbsolutePath.IndexOf("/" + module + "/", StringComparison.CurrentCultureIgnoreCase) >= 0
                        ? "active"
                        : "";
            }
            else
            {
                return CurrentModule == module ? "active" : "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}