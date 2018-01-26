using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;

namespace SunNet.PMNew.PM2014.UserControls.OA
{
    public partial class YearsUserCon : BaseAscx
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var years = new Dictionary<string, string>();
                //years.Add("All", "-1");
                for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 4; i--)
                {           
                    years.Add(i + "", i + "-01-01");
                }
                ddlYears.DataSource = years;
                ddlYears.DataTextField = "key";
                ddlYears.DataValueField = "value";
                ddlYears.DataBind();
                var selYear = string.IsNullOrEmpty(Request.QueryString["year"]) ? DateTime.Now.Year + "-01-01" : Request.QueryString["year"];
                //ddlYears.Items.FindByValue(selYear).Selected = true;
                ddlYears.SelectedValue = selYear;
            }
        }

    }
}