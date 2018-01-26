<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskList.ascx.cs" Inherits="SunNet.PMNew.Web.UserControls.TaskList" %>
<table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
    <tr class="owlistTitle">
        <th>
            Title
        </th>
        <th>
            Description
        </th>
        <th style="width: 40px;">
            Status
        </th>
        <th style="width: 60px;">
            Action
        </th>
    </tr>
    <tr runat="server" id="trNoTickets" visible="false">
        <th colspan="4">
            &nbsp;
        </th>
    </tr>
    <asp:Repeater ID="rptTaskList" runat="server">
        <ItemTemplate>
            <tr class='<%# Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo" %>'>
                <td>
                    <%# Eval("Title")%>
                </td>
                <td>
                    <%# Eval("Description").ToString().Length > 100 ? Eval("Description").ToString().Substring(0, 100) + "..." : Eval("Description").ToString()%>
                </td>
                <td>
                    <%#Convert.ToBoolean(Eval("IsCompleted"))==true ? " Done" : "Solving"%>
                </td>
                <td>
                    <a href="#" onclick='OpenTaskDialog(<%# Eval("TaskID")%>);return false;'>
                        <img src="/icons/27.gif" border="0" title="View" alt="view"></a>
                    <%# ShowUpdateTaskStatus(Convert.ToInt32(Eval("TaskID")), Convert.ToInt32(Eval("TicketID")), Convert.ToBoolean(Eval("IsCompleted")))%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
