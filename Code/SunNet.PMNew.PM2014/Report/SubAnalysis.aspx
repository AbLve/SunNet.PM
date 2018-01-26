<%@ Page Title="" Language="C#" MasterPageFile="~/Report/Report.Master" AutoEventWireup="true"
    CodeBehind="SubAnalysis.aspx.cs" Inherits="SunNet.PMNew.PM2014.Report.SubAnalysis" %>

<%@ Import Namespace="SunNet.PMNew.Entity.TicketModel" %>
<%@ Import Namespace="SunNet.PMNew.Entity.UserModel" %>

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
                <td></td>

            </tr>
            <tr>
                <td>Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlType" queryparam="type" CssClass="selectProfle1" runat="server">
                        <asp:ListItem Text="All" Value="All"> </asp:ListItem>
                        <asp:ListItem Text="Request" Value="Request"> </asp:ListItem>
                        <asp:ListItem Text="Bug" Value="Bug"> </asp:ListItem>
                        <asp:ListItem Text="Change" Value="Change"> </asp:ListItem>
                        <asp:ListItem Text="Risk" Value="Risk"> </asp:ListItem>
                        <asp:ListItem Text="Issue" Value="Issue"> </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>Source:</td>
                <td>
                    <asp:DropDownList ID="ddlSource" queryparam="source" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
               
                <td id="tdStarText" runat="server">Rating:</td>
                <td id="tdStar" runat="server">
                    <asp:DropDownList ID="ddlStars" queryparam="star" CssClass="selectProfle1" runat="server">
                        <asp:ListItem Text="All" Value=""></asp:ListItem>
                        <asp:ListItem Text="None Star" Value="0"></asp:ListItem>
                        <asp:ListItem Text="1 Star" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2 Stars" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3 Stars" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4 Stars" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5 Stars" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td id="tdButtons" runat="server">
                    <input type="button" class="searchBtn" id="btnSearch" style="margin-right: 10px;" />
                    <asp:Button ID="iBtnDownload" CssClass="otherBtn excelBtn" runat="server" Text="&nbsp;" ToolTip="Download Report" OnClick="iBtnDownload_Click" />

                </td>


            </tr>
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="160" class="order" default="true" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="100" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="*" class="order" orderby="TicketTitle">Ticket Title<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Priority">Priority<span class="arrow"></span></th>
                <th width="100" class="order" orderby="TicketType">Type<span class="arrow"></span></th>
                <th width="100" class="order" orderby="source">Source<span class="arrow"></span></th>
                <th width="130">DEV<span class="arrow"></span></th>
                <th width="130">QA<span class="arrow"></span></th>

            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptReportList" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                    <td>
                        <%#Eval("ProjectTitle")%>
                    </td>
                    <td>
                        <a href='/SunnetTicket/Detail.aspx?tid=<%# Eval("TicketID") %>&returnurl=<%# ReturnUrlOfCurrentPage %>'><%#Eval("TicketID")%></a>
                    </td>
                    <td>
                        <%#Eval("Title")%>
                    </td>
                    <td>
                        <%# ((PriorityState)Eval("Priority")).ToString() %>
                    </td>
                    <td>
                        <%# Eval("TicketType") %>
                    </td>
                    <td>
                        <%# Eval("Source").ToString().Trim()== "0" ? "": ((RolesEnum)int.Parse(Eval("Source").ToString())).ToString() %>
                    </td>
                    <td>
                        <%# Eval("DEV").ToString().Trim().Trim(',') %>
                    </td>
                    <td>
                        <%# Eval("QA").ToString().Trim().Trim(',') %>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="ReportPage" runat="server">
        </webdiyer:AspNetPager>
    </div>
    <div style="width: 100%; text-align: center">
        <input name="button2" tabindex="10" id="btnCancel" type="button" class="redirectback backBtn mainbutton" value="Back">
    </div>

</asp:Content>
