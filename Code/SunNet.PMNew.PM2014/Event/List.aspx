<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Event.master" CodeBehind="List.aspx.cs" Inherits="SunNet.PMNew.PM2014.Event.List" %>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="searchSection">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td width="30" align="right">Year:</td>
                    <td>
                        <asp:DropDownList ID="ddlYears" runat="server" CssClass="selectw1" Width="120px">
                            <asp:ListItem Value="2013">2013</asp:ListItem>
                            <asp:ListItem Value="2014">2014</asp:ListItem>
                            <asp:ListItem Value="2015">2015</asp:ListItem>
                            <asp:ListItem Value="2016">2016</asp:ListItem>
                            <asp:ListItem Value="2017">2017</asp:ListItem>
                            <asp:ListItem Value="2018">2018</asp:ListItem>
                        </asp:DropDownList>

                    </td>
                    <td width="55" align="right">Month:</td>
                    <td>
                        <asp:DropDownList ID="ddlMonths" runat="server" CssClass="selectw1" Width="120px">
                            <asp:ListItem Value="1">1</asp:ListItem>
                            <asp:ListItem Value="2">2</asp:ListItem>
                            <asp:ListItem Value="3">3</asp:ListItem>
                            <asp:ListItem Value="4">4</asp:ListItem>
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="6">6</asp:ListItem>
                            <asp:ListItem Value="7">7</asp:ListItem>
                            <asp:ListItem Value="8">8</asp:ListItem>
                            <asp:ListItem Value="9">9</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="11">11</asp:ListItem>
                            <asp:ListItem Value="12">12</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="60" align="right">Project:</td>
                    <td>
                        <asp:DropDownList ID="ddlProjects" runat="server" class="selectw1">
                        </asp:DropDownList>
                    </td>
                      <td width="55" align="right">User:</td>
                    <td>
                        <asp:DropDownList ID="ddlUser" runat="server" class="selectw1" DataValueField="ID" DataTextField="FirstAndLastName">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hiUserIds" runat="server" />
                    </td>
                    <td width="30">
                        <asp:Button runat="server" ID="iBtnSearch" CssClass="searchBtn" Text=""/>
                    </td>
                    <td width="30">
                        <a href="/do/iCalFeed.ashx" class="exportBtn" target="_blank">&nbsp;</a>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="dataSection">
    <div class="topbtnbox">
        <ul class="listtopBtn">
            <li>
                <div class="listtopBtn_icon">
                    <img src="/images/icons/wevents.png" />
                </div>
                <div class="listtopBtn_text" href="Add.aspx" data-target="#modalsmall" data-toggle="modal">Create Event </div>
            </li>
        </ul>
    </div>
    <% if (upcomingEvents.Count() == 0)
       {%>
    <div class="mainowConbox" style="min-height: 80px;">
        <div class="ownothingText" style="color: red;">
            No scheduled event.
        </div>
    </div>
    <% }%>
    <% else
       {%>

    <% foreach (SunNet.PMNew.Entity.EventModel.UpcomingEvent upcomingEvent in upcomingEvents)
       {%>
    <div class="contentTitle titleeventlist"><%=upcomingEvent.Day.ToString("MMMM dd, yyyy",System.Globalization.DateTimeFormatInfo.InvariantInfo) %></div>

    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="130">Time
                </th>
                <th>Title</th>
                <th width="80" class="aligncenter">Action</th>
            </tr>
        </thead>

        <tbody>
            <%for (int i = 0; i < upcomingEvent.list.Count; i++)
              {
                  SunNet.PMNew.Entity.EventModel.ListView item = upcomingEvent.list[i]; %>
            <tr class='<%= i % 2 == 0 ? "" : "whiterow" %>'>
                <td><%=item.Time %>
                </td>
                <td><%=item.Name %>
                </td>
                <td class="aligncenter">
                    <a href="javascript:void(0);">
                        <img src="/Images/icons/edit.png" title="edit" href="Edit.aspx?ID=<%=item.ID%>" data-target="#modalsmall" data-toggle="modal">
                    </a>
                </td>
            </tr>
            <%} %>
        </tbody>
    </table>
    <%} %>
    <%} %>


    <script type="text/javascript">
        function OpenAddModuleDialog(day) {
            var result = ShowIFrame("/Sunnet/Events/AddEvent.aspx?Date=" + day + "&" + "pid=<%=ddlProjects.SelectedValue%>", 463, 470, true, "Add Schedules");
            if (!result) {
                document.getElementById('<%=iBtnSearch.ClientID%>').click();
            }
        }

        function OpenEditSchdules(id) {
            var result = ShowIFrame("/Sunnet/Events/EditEvent.aspx?ID=" + id, 463, 470, true, "Edit Schedules");
            if (result == 0) {
                document.getElementById('<%=iBtnSearch.ClientID%>').click();
            }
        }

    </script>
</asp:Content>
