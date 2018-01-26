<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelationTicketsList.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.RelationTicketsList" %>
<table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
    <tr class="owlistTitle">
        <th style="width: 70px;">
            Ticket Code
        </th>
        <th style="width: 200px;">
            Title
        </th>
        <th>
            Description
        </th>
        <th style="width: 90px;">
            Created Date
        </th>
        <th style="width: 60px;">
            Action
        </th>
    </tr>
    <tr runat="server" id="trNoTickets" visible="false">
        <th colspan="5">
            &nbsp;
        </th>
    </tr>
    <asp:Repeater ID="rptRelationTicketList" runat="server">
        <ItemTemplate>
            <tr class='<%# Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo" %>'>
                <td>
                    <%# Eval("TicketCode").ToString()%>
                </td>
                <td>
                    <%# Eval("Title").ToString()%>
                </td>
                <td>
                    <%# Eval("Description").ToString().Length >= 100 ? Eval("Description").ToString() + "..." : Eval("Description").ToString()%>
                </td>
                <td>
                    <%# Eval("CreatedOn","{0:MM/dd/yyyy}")%>
                </td>
                <td>
                    <a href="#" onclick='OpenViewTicketDialog(<%# Eval("TicketID")%>);return false;'>
                        <img src="/icons/27.gif" border="0" title="View" alt="view"></a>
                    <%# ShowDelteImg(Eval("TicketID"))%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
