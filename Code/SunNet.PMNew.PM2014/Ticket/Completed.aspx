<%@ Page Title="Completed Tickets" Language="C#" MasterPageFile="~/Ticket/Client.master" AutoEventWireup="true" CodeBehind="Completed.aspx.cs" Inherits="SunNet.PMNew.PM2014.Ticket.Completed" %>

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
                <strong class="faqsspantext1">Completed </strong>- <u>Completed</u> tickets have
                been approved and published to your production/live site. These tickets are considered
                closed.
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
        var oldWidth = 0;
        var starWidth = 16;
        var LoginUserID = '<%=UserInfo.ID %>';
        $(document).ready(function () {
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
            var $canSetStar = jQuery("div.starcontainer div[createuser='" + LoginUserID + "']").children();
            $canSetStar.hover(
               function () {
                   var _div_Star = $(this);
                   oldWidth = _div_Star.parent().prev().width();
                   _div_Star.parent().prev().width((_div_Star.index() + 1) * starWidth);
               },
               function () {
                   var _div_Star = $(this);
                   _div_Star.parent().prev().width(oldWidth);
               }
            ).click(function () {
                var _div_Star = jQuery(this);
                jQuery.getJSON("/Service/Ticket.ashx",
                   {
                       action: "UpdateStar",
                       ticketid: _div_Star.parent().attr("ticketid"),
                       star: _div_Star.index() + 1
                   },
                   function (responseData) {
                       if (responseData == true) {
                           oldWidth = (_div_Star.index() + 1) * starWidth;
                       }
                   }
               );
            });
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
            </tr>
        </table>
     <%--    <table border="0" cellspacing="0" cellpadding="0" style="padding-top: 10px;">
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
                <th width="120" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="70px" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="Priority">Priority<a id="priorityDesc" class="info">&nbsp;</a><span class="arrow"></span></th>
                <th width="90px" class="order" orderby="Status">Status<a id="statusDesc" class="info">&nbsp;</a><span class="arrow"></span></th>

                <th width="80px" class="order order-desc" default="true" orderby="ModifiedOn">Updated<span class="arrow"></span></th>
                <th width="90px" class="order" orderby="CreatedByFirstName">Created By<span class="arrow"></span></th>
                <th width="80px">Ratings
                </th>
                <th width="110px" class="aligncenter">Action</th>
            </tr>

        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="8" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptTicketsList" runat="server">
            <ItemTemplate>

                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> collapsed' ticket="<%# Eval("TicketID")%>">

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
                        <%# Eval("Status")%>
                    </td>
                    <td>
                        <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%# Eval("CreatedByFirstName")%> <%#  Eval("CreatedByLastName") %>
                    </td>
                    <td class="action">
                        <div class="starcontainer">
                            <div class="color_bg">
                            </div>
                            <div style="width: <%#Convert.ToInt32(Eval("Star").ToString())*16 %>px;" class="color">
                            </div>
                            <div createuser='<%#GetCreateUser(Eval("TicketID")) %>' star="<%#Eval("Star") %>" ticketid='<%#Eval("TicketID") %>' class="star">
                                <div title="Poor">
                                </div>
                                <div title="Fair">
                                </div>
                                <div title="Average">
                                </div>
                                <div title="Good">
                                </div>
                                <div title="Excellent">
                                </div>
                            </div>
                        </div>
                    </td>
                    <td class="action aligncenter">
                        <a href="Detail.aspx?tid=<%#Eval("TicketID") %>&returnurl=<%=this.ReturnUrl %>" target="_blank" ticketId='<%# Eval("TicketID")%>'>
                            <img src="/Images/icons/view.png" title="View" id='imageOpen<%# Eval("TicketID")%>'>
                        </a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpCompleted" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
