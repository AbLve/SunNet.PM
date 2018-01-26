<%@ Page Title="Events" Language="C#" AutoEventWireup="true" MasterPageFile="~/Sunnet/Main.Master"
    CodeBehind="EventsList.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.EventList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <table width="99%" border="0" cellpadding="0" cellspacing="0" style="min-width: 900px; table-layout: fixed; margin-top: -4px;line-height:2">
        <tr>
            <td width="65%">
                Events > List
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" cellspacing="0" cellpadding="0" border="0">
        <tbody>
            <tr>
                <td vertical-align="top" style="border-bottom: 1px solid rgb(129, 186, 232);">
                    <table width="97%" cellspacing="0" cellpadding="0" border="0" align="center" class="searchBox">
                        <tbody>
                            <tr>
                                <td width="30" align="right">Year:
                                </td>
                                <td width="120">
                                    <asp:DropDownList ID="ddlYears" runat="server" CssClass="select205" Width="120px">

                                        <asp:ListItem Value="2013">2013</asp:ListItem>
                                        <asp:ListItem Value="2014">2014</asp:ListItem>
                                        <asp:ListItem Value="2015">2015</asp:ListItem>
                                        <asp:ListItem Value="2016">2016</asp:ListItem>
                                        <asp:ListItem Value="2017">2017</asp:ListItem>
                                        <asp:ListItem Value="2018">2018</asp:ListItem>
                                    </asp:DropDownList>



                                </td>
                                <td width="60" align="right">Month:
                                </td>
                                <td width="120">
                                    <asp:DropDownList ID="ddlMonths" runat="server" CssClass="select205" Width="120px">
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
                                <td width="60" align="right">Project:
                                </td>
                                <td width="214">
                <asp:DropDownList ID="ddlProjects" runat="server" class="select205"  >
                </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:ImageButton ID="iBtnSearch" BorderWidth="0" ImageUrl="/images/search_btn.jpg" runat="server" OnClick="iBtnSearch_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="mainactionBox">
                        <div class="mainactionBox_left">
                            <span>
                                <a href="#" onclick="OpenAddModuleDialog('<%=DateTime.Now.ToString("MM/dd/yyyy")%>')">
                                <img src="/images/event.png" width="16" height="18" border="0" align="absmiddle"/>
                                Create Event </a></span>
                        </div>
                        <div class="mainactionBox_right">
                        </div>
                    </div>
                    <div class="mainrightBoxtwo">
                        <% if (upcomingEvents.Count() == 0)
                           {%>
                        <div class="mainowConbox" style="min-height: 80px;">
                            <div class="ownothingText">
                                Nothing was scheduled.
                            </div>
                        </div>
                        <% }%>
                        <% else
                           {%>

                        <% foreach (SunNet.PMNew.Entity.EventModel.UpcomingEvent upcomingEvent in upcomingEvents)
                           {%>

                        <div class="pdlistTop">
                            <%=upcomingEvent.Date %>
                        </div>
                        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="listtwo">
                            <tbody>
                                <tr class="listsubTitle">
                                    <td width="120">
                                        <a href="#">Time</a>
                                    </td>
                                    <td>Title
                                    </td>
                                    <td width="150">Action
                                    </td>
                                </tr>
                                <%for (int i = 0; i < upcomingEvent.list.Count; i++)
                                  {
                                      SunNet.PMNew.Entity.EventModel.ListView item = upcomingEvent.list[i]; %>
                                <tr class='<%= i % 2 == 0 ? "listrowone" : "listrowtwo" %>'">
                                    <td><%=item.Time %>
                                    </td>
                                    <td><%=item.Name %>
                                    </td>
                                    <td align="center">
                                        <a href="javascript:void(0);" onclick="OpenEditSchdules(<%=item.ID%>)">Edit</a>
                                        |
                                        <a href="javascript:void(0);" onclick="doDelete(<%=item.ID%>)">Delete</a>
                                    </td>
                                </tr>
                                <%} %>
                            </tbody>
                        </table>
                        <%} %>
                        <%} %>
                    </div>
                        <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpTicketReport" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="10"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpTicketReport_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>
                </td>
            </tr>
        </tbody>
    </table>
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

        function doDelete(id) {
            jQuery.ajax({
                type: "POST",
                url: "/do/DoDeleteEvent.ashx?r=" + Math.random(),
                data: {
                    'id': id,
                },
                success: function (responseData) {
                    if (responseData == "1") {
                        ShowMessage('The event has been deleted.', 0, true, false)
                    }
                    else {
                        ShowMessage('Delete event fail', false, false)
                    }
                }
            });
        }
    </script>
</asp:Content>
