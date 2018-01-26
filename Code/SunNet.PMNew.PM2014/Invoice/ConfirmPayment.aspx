<%@ Page Language="C#" MasterPageFile="~/Invoice/Invoice.master" AutoEventWireup="true" CodeBehind="ConfirmPayment.aspx.cs" Inherits="SunNet.PMNew.PM2014.Invoice.ConfirmPayment" %>

<%@ Import Namespace="SunNet.PMNew.Framework" %>

<%@ Register Src="~/UserControls/Sunnet/UsersView.ascx" TagPrefix="custom" TagName="ticketUsersView" %>
<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagPrefix="custom" TagName="uploader" %>
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
                <td width="60px">Project:
                </td>
                <td width="200px">
                        <sunnet:ExtendedDropdownList ID="ddlProject" queryparam="project"
                            DataTextField="Title"
                            DataValueField="ProjectID"
                            DataGroupField="Status"
                            DefaultMode="List" runat="server" CssClass="selectw1" Width="260">
                        </sunnet:ExtendedDropdownList>
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
                <th width="15%" class="order" orderby="ProjectTitle">Project<span class="arrow"></span></th>
                <th width="15%" class="order" orderby="ProposalTitle">Proposal Title<span class="arrow"></span></th>
                <th width="10%" orderby="PONo">PO #<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="Milestone">Milestone<span class="arrow"></span></th>
                <th width="15%" class="order order-desc" default="true" orderby="ID">Invoice #<span class="arrow"></span></th>
                <th width="15%" class="aligncenter">Action</th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="8" style="color: Red;">&nbsp; No record found
            </th>
        </tr>
        <asp:Repeater ID="rptTicketsList" runat="server" OnItemDataBound="rptTicketsList_ItemDataBound">
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td><%# Eval("ProjectTitle")%></td>
                    <td><%# Eval("ProposalTitle")%></td>
                    <td><%# Eval("PONo")%></td>
                    <td><%# Eval("Milestone")%></td>
                    <td><%# Eval("InvoiceNo")%></td>
                    <td class="aligncenter">
                        <a href="javascript:void(0);" onclick="ConfirmPayment(<%# Eval("ID")%>);"><img src="/Images/icons/confirm_payment_c.png" title="Confirm Payment"></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <script type="text/javascript">
        function ConfirmPayment(invoiceId) {
            jQuery.confirm("Are you sure you want to confirm this payment?", {
                yesText: "Confirm",
                yesCallback: function () {
                    $.post("/Service/Invoice.ashx", { action: "confirmpayment", invoiceId: invoiceId }, function (data) {
                        if (data) {
                            location.reload();
                        }
                        else {
                            ShowMessage("Failed.", "failed", false, false);
                        }
                    });
                },
                noText: "Cancel"
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <asp:HiddenField ID="hidtsID" runat="server" />
    <asp:Button ID="download" runat="server" Text="Button" OnClick="download_Click1" Style="display: none" />
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
