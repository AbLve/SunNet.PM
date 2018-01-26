<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Report/Report.Master"
    CodeBehind="Consuming.aspx.cs"
    Inherits="SunNet.PMNew.PM2014.Report.Consuming" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript" language="javascript">

        function showDetailView() {
            var urlStr = this.location.href;
            var newUrl = urlStr.replace("hoursmodel", "detailmodel");
            RedirectBack(newUrl);
        }

        function showHoursView()
        {
            var newUrl = this.location.href;
            var urlStr = this.location.href;
            if (urlStr.indexOf("detailmodel")>0)
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
            else
            {
              //  model = urlParams["viewmodel"].toString().toLowerCase();
            }
          
            var lnkDetail = jQuery("#lnkDetail");
            var lnkHours = jQuery("#lnkHours");
            if (model == "hoursmodel")
            {
                $("#tableDetail").hide();
                if (lnkDetail.hasClass("cdtopBtn-active"))
                {
                    lnkDetail.removeClass("cdtopBtn-active");
                }
                if (!lnkHours.hasClass("cdtopBtn-active"))
                {
                    lnkHours.addClass("cdtopBtn-active");
                }
            }
            else
            {
                $("#tableHours").hide();
                if (lnkHours.hasClass("cdtopBtn-active"))
                {
                    lnkHours.removeClass("cdtopBtn-active");
                }
                if (!lnkDetail.hasClass("cdtopBtn-active"))
                {
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

                <td width="80" >Project:
                </td>
                <td>
                    <asp:DropDownList ID="ddlProject" queryparam="project" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="80" >User:
                </td>
                <td>
                    <asp:DropDownList ID="ddlUsers" queryparam="user" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td width="80" >Start Date:
                </td>
                <td>
                    <asp:TextBox CssClass="inputdate inputProfle1" ID="txtStartDate" queryparam="startdate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>


                </td>
                <td width="60">End Date:
                </td>
                <td>
                    <asp:TextBox CssClass="inputdate inputProfle1" ID="txtEndDate" queryparam="enddate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
                </td>
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" style="margin-right: 10px;"  />
                    <asp:Button ID="iBtnDownload" CssClass="otherBtn excelBtn" runat="server" Text="&nbsp;" ToolTip="Download Report" OnClick="iBtnDownload_Click" />
                </td>

            </tr>
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="topbtnbox">
        <a class="topbtn cdtopBtn cdtopBtn-active nounderline" onclick="showDetailView();" id="lnkDetail">Detail View</a>
        <a class="topbtn cdtopBtn nounderline" onclick="showHoursView();" id="lnkHours">Hours View</a>
    </div>

    <asp:Repeater ID="rptListReport" runat="server">
        <HeaderTemplate>
            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="table-advance" id="tableDetail">
                <thead>
                    <tr>
                        <th width="30%" class="order order-asc" default="true" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                        <th width="20%" class="order" orderby="FirstName">First Name<span class="arrow"></span></th>
                        <th width="20%" class="order" orderby="LastName">Last Name<span class="arrow"></span></th>
                        <th width="10%" class="order" orderby="Hours">Hours<span class="arrow"></span></th>
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
                <td>
                    <%#Eval("Hours")%>
                </td>
                <td view="hidethis" class="action aligncenter">
                    
                    <a title="View Timesheets" href='ProjectUserTimeSheet.aspx?project=<%#Eval("ProjectID")%>&user=<%#Eval("UserID") %>&startdate=<%# StartDate.ToString("yyyy-MM-dd") %>&enddate=<%# EndDate.ToString("yyyy-MM-dd") %>&returnurl=<%# this.ReturnUrl %> '>
                        <img src="/Images/icons/18.gif" alt="view" /></a>
                </td>

            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <table>
     <tr runat="server" id="trNoListRecord" visible="False" >
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
