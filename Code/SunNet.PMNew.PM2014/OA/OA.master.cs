﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.OA
{
    public partial class OA : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Normal)this.Master).CurrentModule = Codes.SelectedSunnetMenu.OA;
        }
    }
}