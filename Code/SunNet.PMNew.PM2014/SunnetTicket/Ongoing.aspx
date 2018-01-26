<%@ Page Language="C#" MasterPageFile="~/SunnetTicket/Sunnet.master" AutoEventWireup="true" CodeBehind="Ongoing.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.Ongoing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .popover {
            max-width: 600px;
            background-color: #f1f0f0;
        }
        .popover.right .arrow:after {
            border-right-color: #f1f0f0;
        }
    </style>
      <script id="temp_ticketStatus_desc" type="text/html">
        <div class="faqsText" id="div7">
            <p>
                <strong class="faqsspantext1">Submitted </strong>- <u>Submitted</u> definitions 
            </p>
            <p>
                <strong class="faqsspantext1">Estimating </strong>- Estimating <u>tickets</u> definitions 
            </p>
            <p>
                <strong class="faqsspantext1">In Progress </strong>- In Progress <u>tickets</u>
                definitions.
            </p>
            <p>
                <strong class="faqsspantext1">Waiting for Client's feedback </strong>- definitions.
            </p>
            <p>
                <strong class="faqsspantext1">Ready For Review </strong>- 
                definitions.
            </p>
            <p>
                <strong class="faqsspantext1">Not Approved </strong>- <u>Not Approved</u> definitions.
            </p>
            <p>
                <strong class="faqsspantext1">Waiting for SunNet's feedback</strong> -definitions.
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

            $("#<%=txtKeyWord.ClientID%>").focus();
        });

        function delRow(t, tid) {
            var vTr = t.parent("td").parent("tr");
            vTr.remove();
            $.ajax({
                type: "post",
                url: "/Do/SendEmailClientCancelTicketHandler.ashx?r=" + Math.random(),
                data: { tid: tid }
            });
        };
        function cancelImg(tid) {
            jQuery.confirm("Are you sure you want to cancel this ticket? ", {
                yesText: "Yes",
                yesCallback: function () {
                    $.ajax({
                        type: "post",
                        url: "/Do/DoCancelTicketHandler.ashx?r=" + Math.random(),
                        data: {
                            tid: tid
                        },
                        success: function (result) {
                            if (result !== "Remove Success!") {
                                ShowMessage(result, "danger");
                            } else {
                                delRow($("#" + tid), tid);
                            }
                        }
                    });
                },
                noText: "No",
                noCallback: function () { }
            });
        }
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
            </tr>
        </table>
        <%--  <table border="0" cellspacing="0" cellpadding="0" style="padding-top: 10px;">
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
             <%--   <th width="30" class="aligncenter"><a href="AdjustPriority.aspx" class="hide">
                    <img src="/Images/icons/adjust_priority.png" border="0" title="Adjust Priority"></a></th>--%>

                 <th width="50px" class="order order-desc" default="true" orderby="ShowNotification">Notes<span class="arrow"></span></th>
                <th width="120" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="70px" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="Priority">Priority<a id="priorityDesc" class="info">&nbsp;</a><span class="arrow"></span></th>
                <th width="90px" class="order" orderby="Status">Status<a id="statusDesc" class="info">&nbsp;</a><span class="arrow"></span></th>
                <th width="80px" class="order" orderby="ModifiedOn">Updated<span class="arrow"></span></th>
                <th width="90px" class="order" orderby="CreatedByFirstName">Created By<span class="arrow"></span></th>
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
                <script type="text/html" id="ticket<%# Eval("TicketID")%>Description"><%# Server.HtmlEncode(Eval("FullDescription").ToString()) %></script>
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
                    <%# Eval("CreatedByFirstName")%> <%#  Eval("CreatedByLastName") %>
                    </td>
                    <td class="action aligncenter">
                        <%#GetAction((int)Eval("TicketID"),(SunNet.PMNew.Entity.TicketModel.TicketsState)Eval("Status"),(int)Eval("ConfirmEstmateUserId")) %>
                    
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpOngoing" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
