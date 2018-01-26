<%@ Page Title="" Language="C#" MasterPageFile="~/Report/Report.Master" AutoEventWireup="true" CodeBehind="Comparison.aspx.cs" Inherits="SunNet.PMNew.PM2014.Report.Comparison" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript">
        function CancelSubmit(sheetdate, userid, username) {
            var msg = "The timesheets Date";
            msg = msg + "(" + sheetdate + ") User(" + username + ") ";
            msg = msg + " will be submitted to the pm, Proceed? ";
            var p = jQuery.confirm(msg, {
                yesText: "Yes",
                noText: "No",
                yesCallback: function () {
                    jQuery.getJSON("/service/Timesheet.ashx?r=" + Math.random(),
                        {
                            action: "cancelsubmit",
                            sheetdate: sheetdate,
                            userid: userid
                        },  
                        function (responseData) {

                            if (responseData.success == true || responseData.success == "true") {
                                document.getElementById("btnSearch").click();
                            }
                        }
                    );

                },
                noCallback: function () { }
            });
            return false;
        }

        function displayTimesheets(projectID, ticketID) {
            var href = "/Report/TicketTimesheet.aspx?project=" + projectID + " &"
                + "ticket=" + ticketID + "&"
                + "startdate=" + $("#<%=txtStartDate.ClientID%>").val() + "&"
            + "enddate=" + $("#<%=txtEndDate.ClientID%>").val() + "&"
            + "user=" + $("#<%=ddlUsers.ClientID%>").val();
            $("#aViewTimeSheet").attr("href", href).click();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="70">Keyword:
                </td>
                <td>
                    <asp:TextBox ID="txtKeyword" placeholder="Enter project or ticket title" queryparam="keyword" runat="server" CssClass="inputProfle1"></asp:TextBox>
                </td>
                <td width="80">Project:
                </td>
                <td>
                    <asp:DropDownList ID="ddlProject" queryparam="project" AutoPostBack="true" CssClass="selectProfle1" runat="server"
                        OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>   
                <td width="60">Ticket:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTickets" queryparam="ticket" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Start Date:
                </td>
                <td>
                    <asp:TextBox CssClass="inputdate inputProfle1" ID="txtStartDate" queryparam="startdate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
                </td>
                <td>End Date:
                </td>
                <td>
                    <asp:TextBox CssClass="inputdate inputProfle1" ID="txtEndDate" queryparam="enddate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
                </td>
                <td>User:
                </td>
                <td>
                    <asp:DropDownList ID="ddlUsers" queryparam="user" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
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

    <asp:Repeater ID="rptReportList" runat="server">
        <HeaderTemplate>
            <table border="0" width="100%" id="rptlist" cellpadding="0" cellspacing="0" class="table-advance">
                <thead>
                    <tr>
                        <th width="*" class="order order-asc" default="true" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                        <th width="150" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                        <th width="*" class="order" orderby="TicketTitle">Ticket Title<span class="arrow"></span></th>
                        <th width="100" class="order" orderby="totalHours">Hours<span class="arrow"></span></th>
                        <th width="120" class="order" orderby="Estimations">Estimation<span class="arrow"></span></th>
                        <th width="120" view="hidethis" class="aligncenter">Action</th>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <!-- collapsed expanded -->
            <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                <td>
                    <%#Eval("ProjectTitle")%>
                </td>
                <td>
                    <a href='/SunnetTicket/Detail.aspx?tid=<%# Eval("TicketID") %>&returnurl=<%# ReturnUrl %>'><%#Eval("TicketID")%></a>
                </td>
                <td>
                    <%#Eval("TicketTitle")%>
                </td>
                <td>
                    <span style='color: <%# (Convert.ToDecimal(Eval("Estimations")) >0 && Convert.ToDecimal(Eval("totalHours")) > Convert.ToDecimal( Eval("Estimations")))? "red" : "" %>'><%# Eval("totalHours") %></span>
                </td>
                <td>
                    <%# Eval("Estimations") %>
                </td>
                <td view="hidethis">
                    <a style="display: none;" id="aViewTimeSheet" data-target="#modalsmall" data-toggle="modal" '>View Timesheet
                    </a>
                    <a onclick="displayTimesheets(<%# Eval("ProjectID") %>,<%# Eval("TicketID") %>)">View Timesheet</a>

                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
    </asp:Repeater>
    <table>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="ReportPage" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>