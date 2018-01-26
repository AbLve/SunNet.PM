<%@ Page Title="" Language="C#" MasterPageFile="~/Invoice/Invoice.master" AutoEventWireup="true" CodeBehind="POList.aspx.cs" Inherits="SunNet.PMNew.PM2014.Invoice.POList" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="60px">PO #:
                </td>
                <td>
                    <asp:TextBox ID="txtKeyword" runat="server" queryparam="keyword" CssClass="input200"></asp:TextBox>
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
                <td width="90px">Approved On:
                </td>
                <td width="200px">
                    <asp:TextBox ID="txtApproveOn" runat="server" queryparam="approveOn" CssClass="input200" onclick="WdatePicker({ isShowClear: false });"></asp:TextBox>
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
                <th width="20%" class="order order-desc" default="true" orderby="C.CompanyName">Company<span class="arrow"></span></th>
                <th width="15%" class="order" orderby="PT.PONo">PO #<span class="arrow"></span></th>
                <th width="15%" class="order" orderby="PT.Title">Proposal Title<span class="arrow"></span></th>
                <th width="10%">Approved On<span class="arrow"></span></th>
                <th width="10%">Milestone<span class="arrow"></span></th>
                <th width="15%">Invoice #<span class="arrow"></span></th>
                <th width="15%">Invoice Status<span class="arrow"></span></th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="10" style="color: Red;">&nbsp; No record found
            </th>
        </tr>
        <asp:Repeater ID="rptTicketsList" runat="server"   >
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td runat="server" id="CompanyNames"><%# Eval("CompanyName") %></td>
                    <td runat="server" id="PONos" style="border-left:1px solid #ddd"><%# Eval("PONo") %></td>
                    <td style="border-left:1px solid #ddd"><%# Eval("Title") %></td>
                    <td><%# Eval("ApprovedOn","{0:MM/dd/yyyy}") %></td>
                    <td><%# Eval("Milestone") %></td>
                    <td><%# Eval("InvoiceNo") %></td>
                    <td><%# Eval("Status").ToString().ToStrText() %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>

    

    </table>
    <iframe id="iframeDownloadFile" style="padding: 0; margin: 0; width: 0; height: 0; display: none;"></iframe>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
