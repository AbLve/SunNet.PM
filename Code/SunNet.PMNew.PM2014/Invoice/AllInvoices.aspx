﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Invoice/Invoice.master" AutoEventWireup="true" CodeBehind="AllInvoices.aspx.cs" Inherits="SunNet.PMNew.PM2014.Invoice.AllInvoices" %>

<%@ Register Src="~/UserControls/Sunnet/UsersView.ascx" TagPrefix="custom" TagName="ticketUsersView" %>
<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagPrefix="custom" TagName="uploader" %>
<%@ Import Namespace="SunNet.PMNew.Framework" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .note {
            display: inline-block;
            width: 20px;
            height: 20px;
            background: url("/Images/icons/comment.png") no-repeat center center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="60px">Keyword:
                </td>
                <td>
                    <asp:TextBox ID="txtKeyword" runat="server" queryparam="keyword" CssClass="input200" placeholder="Enter Title, Invoice#"></asp:TextBox>
                </td>
                <td width="60px">Company:
                </td>
                <td width="200px">
                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="selectw1" Width="180" queryparam="company">
                    </asp:DropDownList>
                </td>
                <td width="90px">Invoice Status:
                </td>
                <td width="200px">
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="selectw2" Width="180" queryparam="Status">
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
                <th width="10%" class="order order-desc" default="true" orderby="ID">Invoice #<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="CompanyName">Company<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="ProposalTitle">Proposal Title<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="Milestone">Milestone<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="SendOn">Send On<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="DueOn">Due On<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="ReceiveOn">Receive On<span class="arrow"></span></th>
                <th width="12%" class="order" orderby="Status">Invoice Status<span class="arrow"></span></th>
                <th width="8%" class="order" orderby="Hours">Hours<span class="arrow"></span></th>
                <th width="10%" class="aligncenter">Action</th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="10" style="color: Red;">&nbsp; No record found
            </th>
        </tr>
        <asp:Repeater ID="rptTicketsList" runat="server" OnItemDataBound="rptTicketsList_ItemDataBound">
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td><%# Eval("InvoiceNo")%></td>
                    <td><%# Eval("CompanyName")%></td>
                    <td><%# Eval("ProposalTitle")%></td>
                    <td><%# Eval("Milestone")%></td>
                    <td><%# Eval("SendOn","{0:MM/dd/yyyy}")%></td>
                    <td><%# Eval("DueOn","{0:MM/dd/yyyy}")%></td>
                    <td><%# Eval("ReceiveOn","{0:MM/dd/yyyy}")%> </td>
                    <td><%# Eval("Status").ToString().ToStrText()%></td>
                    <td>
                        <u>
                            <a href="javascript:void(0);" onclick="return download_Click(<%# Eval("ID")%>)">
                                <%# (Config.TimesheetReport.Contains(UserInfo.UserID) || Eval("ProposalTitle")=="")?Eval("hours")+"(Excel)":""%> 
                            </a>
                        </u>
                    </td>
                    <td class="aligncenter">
                        <a id="lnkEdit" runat="server" data-target="#modalsmall" data-toggle="modal">
                            <img src="<%#Eval("ProposalId").ToString()=="0"?"/Images/icons/edit-color.png":"/Images/icons/edit.png" %>" title="Edit" /></a>
                        <a data-toggle="popover" onmouseover="PopOver(this)" class="note" title="Notes" href="###" data-container="body" data-placement="left"
                            data-trigger="hover click" data-html="true" data-content="<span class='noticeRed'><%#Eval("Notes") %></span>">&nbsp;</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <asp:Button ID="download" runat="server" Text="Button" OnClick="download_Click1" Style="display: none" />
    <asp:HiddenField ID="hidtsID" runat="server" />
    <script type="text/javascript">
        function download_Click(ID) {
            $("#<%=hidtsID.ClientID%>").val(ID);
            $("#<%=download.ClientID%>").click();
        }
        function PopOver(obj) {
            jQuery(obj).popover('show');
        }

    </script>

</asp:Content>

