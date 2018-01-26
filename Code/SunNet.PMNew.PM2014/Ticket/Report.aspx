<%@ Page Title="Tickets Report" Language="C#" MasterPageFile="~/Ticket/Client.master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="SunNet.PMNew.PM2014.Ticket.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .popover {
            max-width: 600px;
            background-color: #f1f0f0;
        }
        .popover.right .arrow:after {
            border-right-color: #f1f0f0;
        }
        @media (max-width: 992px){
            .searchBtn {
                margin-left: 10px !important;
            }
         }
    </style>
    <script id="temp_ticketStatus_desc" type="text/html">
        <div class="faqsText" id="div7">
            <p>
                <strong class="faqsspantext1">Draft </strong>- <u>Drafts</u> are tickets that have
                not yet been entered into the system.
            </p>
            <p>
                <strong class="faqsspantext1">Submitted </strong>- Submitted <u>tickets</u> have
                been successfully entered into the system, but have not yet been reviewed by your
                Project Manager.
            </p>
            <p>
                <strong class="faqsspantext1">In Progress </strong>- In Progress <u>tickets</u>
                have been reviewed by your Project Manager and are currently under review or in
                the development stage.
            </p>
            <p>
                <strong class="faqsspantext1">Quote Required </strong>- Tickets require
                a quote before proceeding.
            </p>
            <p>
                <strong class="faqsspantext1">Quote not Approved </strong>- 
                Tickets are not approved and will not be worked on.
            </p>
            <p>
                <strong class="faqsspantext1">Awaiting Feedback </strong>- 
                Tickets need your review and feedback before we can proceed.
            </p>
            <p>
                <strong class="faqsspantext1">Awaiting Verification </strong>-Tickets
                have been published to your production/live site and need your review and
                approval.
            </p>
            <p>
                <strong class="faqsspantext1">Not Approved </strong>- <u>Not Approved</u> ticket
                have been released to your product server ,but were not approved.
            </p>
            <p>
                <strong class="faqsspantext1">Completed </strong>- <u>Completed</u> tickets have
                been approved and published to your production/live site. These tickets are considered
                closed.
            </p>
            <p>
                <strong class="faqsspantext1">Canceled</strong> -Tickets are
                tickets that are no longer being worked on but continue to be stored in the client
                portal.
            </p>
        </div>
    </script>
     <script id="temp_ticketPriority_desc" type="text/html">
        <div class="faqsText" id="div7">
            <p>
                <img src="/images/icons/low.png" style="margin-top: -3px;">&nbsp;Low
            </p>
            <p>
               <img src="/images/icons/medium.png" style="margin-top: -3px;">&nbsp;Medium
            </p>
             <p>
               <img src="/images/icons/high.png" style="margin-top: -3px;">&nbsp;High
            </p>
            <p>
               <img src="/images/icons/emergency.png" style="margin-top: -3px;">&nbsp;Emergency
            </p>
        </div>
    </script>
    <script type="text/javascript">
        $(function () {
            $("#statusDesc").popover(
                {
                    title: "Ticket Status",
                    container: "body",
                    placement: "left",
                    content: GetTemplateHtml("temp_ticketStatus_desc"),
                    trigger: "hover click",
                    html: true
                });
            $("#priorityDesc").popover(
           {
               title: "Ticket Priority",
               container: "body",
               placement: "left",
               content: GetTemplateHtml("temp_ticketPriority_desc"),
               trigger: "hover click",
               html: true
           });

            $("div.loading").remove();

            $("#<%=txtKeyWord.ClientID%>").focus();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="60" align="right">Keyword:
                </td>
                <td>
                    <asp:TextBox ID="txtKeyWord" placeholder="Enter ID, Title" queryparam="keyword" runat="server" CssClass="inputw1"></asp:TextBox>
                </td>
                <td width="60" align="right">Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" queryparam="status" runat="server" CssClass="selectw1">
                    </asp:DropDownList>
                </td>
                <td width="60" align="right">Project:
                </td>
                <td>
                    <asp:DropDownList ID="ddlProject" queryparam="project" runat="server" CssClass="selectw1">
                    </asp:DropDownList>
                </td>
                <td width="60" align="right">Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTicketType" queryparam="tickettype" runat="server" CssClass="selectw1">
                        <asp:ListItem Value="-1">ALL</asp:ListItem>
                        <asp:ListItem Value="0">Bug</asp:ListItem>
                        <asp:ListItem Value="1">Request</asp:ListItem>
                        <asp:ListItem Value="2">Risk</asp:ListItem>
                        <asp:ListItem Value="3">Issue</asp:ListItem>
                        <asp:ListItem Value="4">Change</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" />
                </td>
                <td>
                    <asp:Button ID="btnDownload" CssClass="otherBtn excelBtn" runat="server" Text="&nbsp;" ToolTip="Download Report" OnClick="btnDownload_Click" />
                </td>
                <td></td>
            </tr>
    <%--        <tr>
                <td colspan="11"><a id="statusDesc" href="###">Ticket Status Definition</a></td>
            </tr>--%>
        </table>
  <%--      <table border="0" cellspacing="0" cellpadding="0" style="padding-top: 10px;">
            <tbody>
                <tr style="">
                    <td style="padding-right: 20px; color: #999;">
                        <img src="/images/icons/low.png" style="margin-top: -3px;">&nbsp;Low</td>
                    <td style="padding-right: 20px; color: #999;">
                        <img src="/images/icons/medium.png" style="margin-top: -3px;">&nbsp;Medium</td>
                    <td style="padding-right: 20px; color: #999;">
                        <img src="/images/icons/high.png" style="margin-top: -3px;">&nbsp;High</td>
                    <td style="padding-right: 20px; color: #999;">
                        <img src="/images/icons/emergency.png" style="margin-top: -3px;">&nbsp;Emergency</td>
                </tr>
            </tbody>
        </table>--%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="50px" class="order order-desc" default="true" orderby="ShowNotification">Notes<span class="arrow"></span></th>
                <th width="120" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="70px" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="*" class="order" orderby="TicketTitle">Title<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="Priority">Priority<a id="priorityDesc" class="info">&nbsp;</a><span class="arrow"></span></th>
                <th width="90px" class="order" orderby="Status">Status<a id="statusDesc" class="info">&nbsp;</a>  <span class="arrow"></span></th>
                <th width="80px" class="order" default="true" orderby="ModifiedOn">Updated<span class="arrow"></span></th>
                <th width="90px" class="order" orderby="FirstName">Created By<span class="arrow"></span></th>
                <th width="110px" class="aligncenter">Action</th>
            </tr>

        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptTicketsList" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <script type="text/html" id='ticket<%# Eval("TicketID")%>Description'><%# Server.HtmlEncode(Eval("Description").ToString())%></script>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> collapsed' ticket="<%# Eval("TicketID")%>">
                    <td class="aligncenter action">
                        <%# FeedBackButtonOrExpanded(Container.DataItem, this.ReturnUrl)%>
                    </td>
                    <td>
                        <%# Eval("ProjectTitle").ToString()%>
                    </td>
                    <td>
                        <%#Eval("TicketID").ToString()%>
                    </td>
                    <td>
                        <%# Eval("Title").ToString()%>
                    </td>
                    <td>
                        <img src="/images/icons/<%# Eval("Priority") %>.png" title="<%# Eval("Priority") %>" />
                    </td>
                    <td>
                        <%# GetClientStatusNameBySatisfyStatus((int)Eval("Status"), (int)Eval("TicketID"))%>
                    </td>
                    <td>
                        <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%# Eval("FirstName")%> <%# Eval("LastName") %>
                    </td>
                    <td class="action aligncenter">
                        <%#GetAction((int)Eval("TicketID"),(SunNet.PMNew.Entity.TicketModel.TicketsState)Eval("Status")) %>
                    </td>

                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpReport" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
