<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/OA/OA.master"
    CodeBehind="Pto.aspx.cs"
    Inherits="SunNet.PMNew.PM2014.OA.Pto.Pto" %>

<%@ Register Src="~/UserControls/OA/YearsUserCon.ascx" TagName="Yearsddl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript">
        function yearchange() {
            return;
        }
        function showDetailView() {
            var urlStr = this.location.href;
            var newUrl = urlStr.replace("hoursmodel", "detailmodel");
            RedirectBack(newUrl);
        }

        function showHoursView() {
            var newUrl = this.location.href;
            var urlStr = this.location.href;
            if (urlStr.indexOf("detailmodel") > 0)
                newUrl = urlStr.replace("detailmodel", "hoursmodel");
            else {
                newUrl = "/Report/Consuming.aspx?viewmodel=hoursmodel";
            }
            RedirectBack(newUrl);

        }

        $(function () {
            var model = "";
            if (urlParams["viewmodel"] != null) {
                model = urlParams["viewmodel"].toString().toLowerCase();
            }
            else {
                //  model = urlParams["viewmodel"].toString().toLowerCase();
            }

            var lnkDetail = jQuery("#lnkDetail");
            var lnkHours = jQuery("#lnkHours");
            if (model == "hoursmodel") {
                $("#tableDetail").hide();
                if (lnkDetail.hasClass("cdtopBtn-active")) {
                    lnkDetail.removeClass("cdtopBtn-active");
                }
                if (!lnkHours.hasClass("cdtopBtn-active")) {
                    lnkHours.addClass("cdtopBtn-active");
                }
            }
            else {
                $("#tableHours").hide();
                if (lnkHours.hasClass("cdtopBtn-active")) {
                    lnkHours.removeClass("cdtopBtn-active");
                }
                if (!lnkDetail.hasClass("cdtopBtn-active")) {
                    lnkDetail.addClass("cdtopBtn-active");
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 10%;">Project:
                </td>
                <td>
                    <asp:DropDownList ID="ddlProject" queryparam="project" CssClass="selectProfle1" runat="server" Width="120px">
                    </asp:DropDownList>
                </td>
                <td style="width: 10%; text-align: right">User:
                </td>
                <td>
                    <asp:DropDownList ID="ddlUsers" queryparam="user" CssClass="selectProfle1" runat="server" Width="120px">
                    </asp:DropDownList>
                </td>
                <td style="width: 10%; text-align: right">Year:</td>
                <td>
                    <uc1:Yearsddl ID="Yearsddl1" runat="server" />
                </td>
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" style="margin-right: 10px;" />
                    <asp:Button ID="iBtnDownload" CssClass="otherBtn excelBtn" runat="server" Text="&nbsp;" ToolTip="Download Report" OnClick="iBtnDownload_Click" />
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <asp:Repeater ID="rptListReport" runat="server">
        <HeaderTemplate>
            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="table-advance" id="tableDetail">
                <thead>
                    <tr>
                        <th width="30%" class="order order-asc" default="true" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                        <th width="20%" class="order" orderby="FirstName">First Name<span class="arrow"></span></th>
                        <th width="20%" class="order" orderby="LastName">Last Name<span class="arrow"></span></th>
                        <th width="10%" class="order" orderby="Remaining" style="text-align: center">Remaining<span class="arrow"></span></th>
                        <th width="10%" class="order" orderby="Hours" style="text-align: center">Hours<span class="arrow"></span></th>
                        <th width="*" view="hidethis" class="aligncenter">Action</th>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <!-- collapsed expanded -->
            <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                <td>
                    <%#Eval("Title")%>
                </td>
                <td>
                    <%#Eval("FirstName")%>
                </td>
                <td>
                    <%#Eval("LastName")%>
                </td>
                <td style="text-align: center">
                    <%#Eval("Remaining")%>
                </td>
                <td style="text-align: center">
                    <%#Eval("Hours")%>
                </td>
                <td view="hidethis" class="action aligncenter">
                    <a title="View Details" href='PtoUserTime.aspx?project=<%#Eval("ProjectID")%>&user=<%#Eval("UserID") %>&year=<%=(Yearsddl1.FindControl("ddlYears") as DropDownList).SelectedValue%>&returnurl=<%# this.ReturnUrl %> '>
                        <img src="/Images/icons/18.gif" alt="view" /></a>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <table>
        <tr runat="server" id="trNoListRecord" visible="False">
            <th colspan="9" style="color: Red;">&nbsp; No record found. 
            </th>
        </tr>

    </table>
    <asp:Repeater ID="rptHoursView" runat="server">
        <HeaderTemplate>
            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="table-advance" id="tableHours">
                <thead>
                    <tr>
                        <th width="71%" class="order" default="true" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                        <th width="29%" class="order" orderby="Hours">Hours<span class="arrow"></span></th>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <!-- collapsed expanded -->
            <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                <td>
                    <%#Eval("Title")%>
                </td>
                <td>
                    <%#Eval("Hours")%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <table>
        <tr runat="server" id="trNoHourRecord" visible="False">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
