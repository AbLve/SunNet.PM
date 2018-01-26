<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoryList.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.HistoryList" %>
<table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
    <tr class="owlistTitle">
        <th orderby="Description">
            Description
        </th>
        <th orderby="ModifiedBy" width="120px;">
            Modified By
        </th>
        <th orderby="ModifiedOn" width="120px;">
            Modified On
        </th>
    </tr>
    <tr runat="server" id="trNoTickets" visible="false">
        <th colspan="3" >
          &nbsp;
        </th>
    </tr>
    <asp:Repeater ID="rptTicketsHistoryList" runat="server">
        <ItemTemplate>
            <tr class='<%# Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo" %>'>
                <td>
                    <%# Eval("Description")%>
                </td>
                <td>
                    <%#  ShowUserName(Eval("ModifiedBy").ToString())%>
                </td>
                <td>
                    <%# Convert.ToDateTime(Eval("ModifiedOn")) > DateTime.MinValue ? Eval("ModifiedOn", "{0:MM/dd/yyyy}") : ""%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
