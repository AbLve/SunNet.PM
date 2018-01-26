<%@ Page Language="C#" MasterPageFile="~/SunnetTicket/Sunnet.master" AutoEventWireup="true" CodeBehind="WaitingResponse.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.WaitingResponse" %>

<%@ Import Namespace="SunNet.PMNew.PM2014.Codes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.bootcss.com/bootstrap-select/1.10.0/css/bootstrap-select.min.css">
    <script src="https://cdn.bootcss.com/bootstrap-select/1.10.0/js/bootstrap-select.min.js"></script>
    <style>
        .statusSelect {
            height: 23px;
            width: 130px !important;
            font-size: 13px !important;
        }

            .statusSelect > button {
                height: 23px;
            }

        .filter-option {
            height: 19px;
            margin-top: -4px;
            border: none;
            display: -moz-inline-box;
        }

        .bootstrap-select .dropdown-toggle:focus {
            outline: none !important;
        }

        .text {
            font-size: 13px !important;
        }
        .mainrightBox {
            overflow: visible !important;
        }
        @media(max-width:992px){
            .pager select{
                width:auto !important;
            }
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var $dropElement = $("#<%=ddlStatus.ClientID %>");
            var $dropSelectedOptions = $("#<%=selectValue.ClientID %>");
            try {
                var optionJson = $dropSelectedOptions.val();
                if (optionJson == null || optionJson == '') {
                    $dropElement.selectpicker('selectAll');
                } else {
                    $dropElement.selectpicker();
                    var options = optionJson.split(',');
                    $dropElement.selectpicker('val', options);
                }
            } catch (e) {
                $dropElement.selectpicker('refresh');
            }

            $dropElement.on("change", function (e) {
                var th = $(this);
                var thVal = th.selectpicker('val');
                if (thVal != null && thVal.length > 0) {
                    $.post("/Service/Ticket.ashx", { action: "TicketStatus", statusJson: JSON.stringify(thVal) }, function (response) { }, "json");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <asp:HiddenField runat="server" ID="selectValue" />
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="60px">Keyword:
                </td>
                <td width="150px">
                    <asp:TextBox ID="txtKeyWord" placeholder="Enter ID, Title" queryparam="keyword" runat="server" CssClass="inputw1" Width="130"></asp:TextBox>
                </td>
                <td width="40px">Project:
                </td>
                <td width="150px">
                    <sunnet:ExtendedDropdownList ID="ddlProject" queryparam="project"
                        DataTextField="Title"
                        DataValueField="ProjectID"
                        DataGroupField="Status"
                        DefaultMode="List" runat="server" CssClass="selectw1" Width="130">
                    </sunnet:ExtendedDropdownList>
                </td>
                <td width="40px">Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" queryparam="status" multiple data-actions-box="true" data-selected-text-format="count > 2" runat="server" CssClass="selectpicker selectw3 statusSelect" Width="130">
                    </asp:DropDownList>
                </td>
                <td width="70px">Created By:
                </td>
                <td >
                    <asp:TextBox ID="txtCreated" placeholder="Enter First name, Last name" queryparam="create" runat="server" CssClass="inputw1" Width="170"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Type:
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
                <td>Priority:
                </td>
                <td>
                    <asp:DropDownList ID="ddlPriority" queryparam="ticketPriority" runat="server" CssClass="selectw1">
                    </asp:DropDownList>
                </td>
                <td colspan="4">Responsible User:
                    <sunnet:ExtendedDropdownList ID="ddlAssignUser" queryparam="ticketAssignedUser"
                        DataTextField="FirstAndLastName"
                        DataValueField="UserID"
                        DataGroupField="Status"
                        DefaultMode="List"
                        runat="server" CssClass="selectw3">
                    </sunnet:ExtendedDropdownList>

                    <input type="button" class="searchBtn" id="btnSearch" style="margin-left: 146px;" />
                </td>
            </tr>
        </table>
        <table border="0" cellspacing="0" cellpadding="0" style="padding-top: 10px;">
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
        </table>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div style="width:100%;overflow-x:auto;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="50px" class="order order-desc" default="true" orderby="ShowNotification">Notes<span class="arrow"></span></th>
                <th width="120" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="70px" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="ResponsibleUserName">Resp User<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="Priority">Priority<span class="arrow"></span></th>
                <th width="90px" class="order" orderby="Status">Status<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="ModifiedOn">Updated<span class="arrow"></span></th>
                <th width="90px" class="order" orderby="CreatedByFirstName">Created By<span class="arrow"></span></th>
                <th width="110px" class="aligncenter">Action</th>
            </tr>

        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found
            </th>
        </tr>
        <asp:Repeater ID="rptTicketsList" runat="server">
            <ItemTemplate>
                <script type="text/html" id='ticket<%# Eval("TicketID")%>Description'><%# Server.HtmlEncode(Eval("FullDescription").ToString()) %></script>
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
                        <%# Eval("ResponsibleUserName").ToString()%>
                    </td>
                    <td>
                        <img src="/images/icons/<%# Eval("Priority") %>.png" title="<%# Eval("Priority") %>" />
                    </td>
                    <td>
                        <%# GetStatus(Eval("Status"))%>
                    </td>
                    <td>
                        <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%# TicketCreateUser( (string)Eval("CreatedByFirstName"),(string)Eval("CreatedByLastName"),(bool)Eval("IsEstimates"))%>
                    </td>
                    <td class="action aligncenter">
                        <a class="saveBtn1 mainbutton" href="Detail.aspx?tid=<%# Eval("TicketID")%>&returnurl=<%=this.ReturnUrl %>" style="display: none;" target="_blank" ticketid='<%# Eval("TicketID")%>'><span id='spanOpen<%# Eval("TicketID")%>'></span></a>
                        <%# GetActionHTML(Eval("ProjectID"),Eval("Status"),Eval("TicketID"),Eval("IsEstimates"),Eval("EsUserID"),Eval("CreatedBy") ,(int)Eval("ConfirmEstmateUserId")) %>
                        <a href='###' action="calladdtocategory" title="Add to category" ticket='<%#Eval("TicketID") %>'>
                            <img src="/Images/icons/category.png" alt="Category" /></a>
                        <a href="Knowledge/New.aspx?ticket=<%#Eval("TicketID") %>" data-target="#modalsmall" data-toggle="modal" title="Add to knowledge share">
                            <img src="/Images/icons/share.png" alt="Knowledge" />
                        </a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox clearfix">
        <webdiyer:AspNetPager ID="anpOngoing" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
