using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SunNet.PMNew.Entity.ComplaintModel;
using SunNet.PMNew.App;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ComplaintModel.Enums;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using StructureMap;

namespace SunNet.PMNew.PM2014.OA.Complaints
{
    public partial class Complaints : BasePage
    {

        protected override string DefaultOrderBy
        {
            get
            {
                return "ComplaintID";
            }
        }
        protected override string DefaultDirection
        {
            get
            {
                return "ASC";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
               // Get value from QueryStrings
                txtKeyword.Text = QS("Keyword");
                txtUpdatedBy.Text = QS("UpdatedBy");

                if (QS("Type") != "" && QS("Type") != "-1")
                    ddlType.SelectedValue = QS("Type");
                if (QS("Status") != "" && QS("Status") != "-1")
                    ddlStatus.SelectedValue = QS("Status");
                if (QS("Reason") != "" && QS("Reason") != "-1")
                    ddlReason.SelectedValue = QS("Reason");
                if (QS("SystemID") != "" && QS("SystemID") != "-1")
                    ddlSystemID.SelectedValue = QS("SystemID");
                if (QS("AppSrc") != "" && QS("AppSrc") != "-1")
                    ddlAppSrc.SelectedValue = QS("AppSrc");

                // Bind dropdownlists
                List<ListItem> lst = ComplaintTypeHelper.AllComplaintType.Select(x => new ListItem() { Text = x.ToText(), Value = ((int)x).ToString() }).ToList();
                lst.BindDropdown<ListItem>(ddlType, "Text", "Value", "ALL", "-1");

                lst = ComplaintReasonHelper.AllReason.Select(x => new ListItem() { Text = x.ToText(), Value = ((int)x).ToString() }).ToList();
                lst.BindDropdown<ListItem>(ddlReason, "Text", "Value", "ALL", "-1");

                ISystemRepository systemRepository = ObjectFactory.GetInstance<ISystemRepository>();
                List<SystemEntity> sysList = systemRepository.GetAllSystems();
                sysList.BindDropdown(ddlSystemID, "SystemName", "SystemID", "ALL", "-1");

                lst = ComplaintAppSrcHelper.AllAppSrc.Select(x => new ListItem() { Text = x.ToText(), Value = ((int)x).ToString() }).ToList();
                lst.BindDropdown<ListItem>(ddlAppSrc, "Text", "Value", "ALL", "-1");

                lst = ComplaintStatusHelper.AllStatus.Select(x => new ListItem() { Text = x.ToText(), Value = ((int)x).ToString() }).ToList();
                lst.BindDropdown<ListItem>(ddlStatus, "Text", "Value", "ALL", "-1");

                // Search db to display the table
                ComplaintSearchEntity request = new ComplaintSearchEntity(true, OrderBy, OrderDirection);
                request.Type = int.Parse(ddlType.SelectedValue);
                request.Reason = int.Parse(ddlReason.SelectedValue);
                request.SystemID = int.Parse(ddlSystemID.SelectedValue);
                request.AppSrc = int.Parse(ddlAppSrc.SelectedValue);
                request.Status = int.Parse(ddlStatus.SelectedValue);
                request.Keyword = txtKeyword.Text.NoHTML();
                request.UpdatedByName = txtUpdatedBy.Text.NoHTML();

                request.CurrentPage = CurrentPageIndex;
                request.PageCount = ComplaintsPage.PageSize;

                ComplaintApplication complaintApp = new ComplaintApplication();
                int recordCount;
                List<ComplaintEntity> comEntityLst = complaintApp.SearchComplaints(request, out recordCount);
                rptUsers.DataSource = comEntityLst;
                rptUsers.DataBind();

                ComplaintsPage.RecordCount = recordCount;
            }
        }
    }
}