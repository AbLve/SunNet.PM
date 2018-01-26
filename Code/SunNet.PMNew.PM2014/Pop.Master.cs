using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014
{
    public partial class Pop : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 获取或设置弹出窗口的宽度.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/22 10:12
        public int Width { get; set; }
        /// <summary>
        /// 弹出层宽度（带PX的宽度值或者auto）.
        /// </summary>
        /// <value>
        /// The width value.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/22 10:12
        protected string WidthValue
        {
            get
            {
                string w = string.Format("{0}px", Width);
                if (Width < 100)
                    w = "";
                return w;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}