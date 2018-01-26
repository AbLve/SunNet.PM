<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeHistory.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.Ticket.Sunnet.ChangeHistory" %>
<div class="contentTitle titleeventlist">Change History </div>
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="table-advance">
    <thead>
        <tr>
            <th orderby="Description">Description
            </th>
            <th orderby="ModifiedBy">Modified By
            </th>
            <th orderby="ModifiedOn">Changed Date
            </th>
            <th orderby="ResponsibleUserId">Responsible User
            </th>
        </tr>
    </thead>
    <tbody>
        <tr runat="server" id="trNoTickets" visible="false">
            <td colspan="4">No record found.
            </td>
        </tr>
        <asp:Repeater ID="rptTicketsHistoryList" runat="server">
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                    <td class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                        <%# Eval("Description")%>
                    </td>
                    <td class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                        <%# BasePage.GetClientUserName(Eval("ModifiedBy"))%>
                    </td>
                    <td class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                        <%# Convert.ToDateTime(Eval("ModifiedOn")) > DateTime.MinValue ? Eval("ModifiedOn", "{0:MM/dd/yyyy HH:mm:ss}") : ""%>
                    </td>
                    <td class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                        <%# BasePage.GetClientUserName(Eval("ResponsibleUserId"))%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
