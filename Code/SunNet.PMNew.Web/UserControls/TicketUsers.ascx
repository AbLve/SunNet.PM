<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketUsers.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.TicketUsers" %>
<table width="95%" border="0" align="center" cellpadding="5" cellspacing="0" class="owUser">
    <tr>
        <td width="12%">
            <strong>Project Manager: </strong>
        </td>
        <td width="35%">
            <asp:Literal ID="lilPmName" runat="server"></asp:Literal>
        </td>
        <td width="8%">
            <strong>Developers:</strong>
        </td>
        <td width="45%">
            <asp:Literal ID="lilDevName" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td>
            <strong>Testers:</strong>
        </td>
        <td>
            <asp:Literal ID="lilTestName" runat="server"></asp:Literal>
        </td>
        <td>
            <strong>Others: </strong>
        </td>
        <td>
            <asp:Literal ID="lilOtherName" runat="server"></asp:Literal>
        </td>
    </tr>
</table>
