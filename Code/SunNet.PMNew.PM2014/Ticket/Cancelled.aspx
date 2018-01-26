<%@ Page Title="Cancelled Tickets" Language="C#" MasterPageFile="~/Ticket/Client.master" AutoEventWireup="true" CodeBehind="Cancelled.aspx.cs" Inherits="SunNet.PMNew.PM2014.Ticket.Cancelled" %>

<%@ Import Namespace="SunNet.PMNew.Entity.TicketModel" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>
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
                <strong class="faqsspantext1">Canceled</strong> -Tickets are
                tickets that are no longer being worked on but continue to be stored in the client
                portal.
            </p>
            <p>
                <strong class="faqsspantext1">Denied </strong>- <u>Denied</u> definitions.
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
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="120" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="80" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="90" class="order" orderby="Priority">Priority<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Status">Status<a id="statusDesc" class="info">&nbsp;</a><span class="arrow"></span></th>

                <th width="80" class="order order-desc" default="true" orderby="ModifiedOn">Updated<span class="arrow"></span></th>
               <th width="90px" class="order" orderby="CreatedByFirstName">Created By<span class="arrow"></span></th>
                <th width="50" class="aligncenter">Action</th>
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
                        <%# Eval("Priority")%>
                    </td>
                    <td>
                        <%# Eval("Status").ToString().ToEnum<TicketsState>().ToText()%>
                    </td>
                    <td>
                        <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%# Eval("CreatedByFirstName")%> <%#  Eval("CreatedByLastName") %>
                    </td>
                    <td class="action aligncenter">
                        <a href="Detail.aspx?tid=<%#Eval("TicketID") %>&returnurl=<%=this.ReturnUrl %>" target="_blank" ticketId='<%# Eval("TicketID")%>'>
                            <img src="/Images/icons/view.png" title="View" id="imageOpen<%# Eval("TicketID")%>">
                        </a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpCancel" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
