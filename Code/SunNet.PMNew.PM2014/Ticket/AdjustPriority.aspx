<%@ Page Title="Adjust Tickets Priority" Language="C#" MasterPageFile="~/Ticket/Client.master" AutoEventWireup="true" CodeBehind="AdjustPriority.aspx.cs" Inherits="SunNet.PMNew.PM2014.Ticket.AdjustPriority" %>

<%@ Import Namespace="SunNet.PMNew.Entity.TicketModel" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>

<%@ Register src="../UserControls/Messager.ascx" tagname="Messager" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript">
        var oldOrders = "";
        var hidNewOrder;
        $(function () {
            delete urlParams.order;
            hidNewOrder = jQuery(<%="'#"+hidNewOrder.ClientID+"'" %>);

            jQuery("#data>tr").each(function () {
                oldOrders = oldOrders + jQuery(this).attr("ticket");
                oldOrders = oldOrders + ",";
            });

            jQuery("#data").sortable({
                cursor: "move",
                placeholder: "ui-state-highlight",
                revert: true
            }).children().mousedown(function () {
                jQuery(this).toggleClass("onclick");
            }).mouseup(function () {
                jQuery(this).toggleClass("onclick");
            });
        });

        function OnSubmit() {
            var newOrders = "";
            jQuery("#data>tr").each(function () {
                newOrders = newOrders + jQuery(this).attr("ticket");
                newOrders = newOrders + ",";
            });
            if (oldOrders == newOrders) {
                ShowMessage("No changes have been applied.", "success", false, false);
                return false;
            }
            else {
                hidNewOrder.val(newOrders);
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <uc1:Messager ID="Messager1" runat="server" />
    <div class="clientfdText">
        Drag the ticket to set a new priority
    </div>
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="50">Project:</td>
                <td>
                    <asp:DropDownList ID="ddlProject" queryparam="project" runat="server" CssClass="selectw3">
                    </asp:DropDownList>
                </td>
                <td width="30">
                    <input type="button" class="searchBtn" id="btnSearch" />
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="80">Ticket ID </th>
                <th width="70">Priority</th>
                <th width="*">Title</th>
                <th width="400">Description</th>
                <th width="150">Status</th>
                <th width="100">Updated Date</th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="6" style="color: Red;">&nbsp; No record found.
                </th>
            </tr>
        </thead>
        <tbody id="data">
            <asp:Repeater ID="rptTickets" runat="server">
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex%2==1?"whiterow":"" %>" ticket="<%#Eval("TicketID") %>">
                        <td><%#Eval("TicketID") %></td>
                        <td><%#Eval("Priority") %></td>
                        <td><%#Eval("Title") %></td>
                        <td><%#Eval("Description") %></td>
                        <td><%#Eval("Status").ToString().ToEnum<TicketsState>().ToText() %></td>
                        <td><%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div class="buttonBox2">
        <asp:HiddenField ID="hidNewOrder" runat="server" />
        <asp:Button ID="btnSave" runat="server" CssClass="saveBtn1 mainbutton" Text="Save" OnClientClick="return OnSubmit();" OnClick="btnSave_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
