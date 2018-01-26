<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="NewSchedules.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.NewSchedules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenTicketDetail(tid) {
            window.open("/Sunnet/Tickets/TicketDetail.aspx?tid=" + tid);
        }
    </script>

    <style type="text/css">
        .onscdTicket:hover {
            position: absolute;
            background-color: #BAD8F0;
            min-height: 90px;
            min-width: 165px;
            width: 195px;
            max-width: 200px;
        }

            .onscdTicket:hover > li {
                margin-bottom: 3px;
            }

                .onscdTicket:hover > li > a {
                    font-weight: bold;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Schedules List
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="40">Year:
            </td>
            <td width="140">
                <asp:DropDownList ID="ddlYears" CssClass="select205" Width="120" runat="server">
                    <asp:ListItem Value="2013">2013</asp:ListItem>
                    <asp:ListItem Value="2014">2014</asp:ListItem>
                    <asp:ListItem Value="2015">2015</asp:ListItem>
                    <asp:ListItem Value="2016">2016</asp:ListItem>
                    <asp:ListItem Value="2017">2017</asp:ListItem>
                    <asp:ListItem Value="2018">2018</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>Month:
            </td>
            <td>
                <asp:DropDownList ID="ddlMonths" CssClass="select205" Width="120" runat="server">
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
            <td>Users:
            </td>
            <td>
                <asp:DropDownList ID="ddlUsers" CssClass="select205" runat="server" DataValueField="UserID"
                    DataTextField="UserName">
                </asp:DropDownList>
            </td>
            <td>
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <div class="mainactionBox_left">
            <span><a href="#" onclick="OpenAddModuleDialog('<% =DateTime.Now.ToString("MM/dd/yyyy") %>')">
                <img src="/icons/new_schedule.png" border="0" align="absmiddle" alt="new/add" />
                Add Schedules</a></span>
        </div>
        <div class="mainactionBox_right">
        </div>
    </div>
    <div class="mainrightBoxthree">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="monthlyviewBox">
            <tr>
                <td>
                    <ul class="monthlyviewweek">
                        <li>Sunday</li>
                        <li>Monday</li>
                        <li>Tuesday</li>
                        <li>Wednesday</li>
                        <li>Thursday</li>
                        <li>Friday</li>
                        <li>Saturday</li>
                    </ul>
                    <ul class="monthlyview">
                        <asp:Repeater ID="rptDays" runat="server" OnItemDataBound="rptDays_ItemDataBound">
                            <ItemTemplate>
                                <li class="<%#Eval("Css")%>">
                                    <%#Eval("AddUrl")%>
                                    <asp:Repeater ID="rptSchedules" runat="server">
                                        <HeaderTemplate>
                                            <ul class="onscdTicket">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li><a href="#" onclick="OpenEditSchdules(<%#Eval("ID") %>,<%= ddlUsers.SelectedValue %>)"
                                                title="Time: <%#Eval("StartTime")%> - <%#Eval("EndTime")%> ">
                                                <%#Eval("Title") %></a> </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        var dialogParam;
        function OpenAddModuleDialog(day) {
            $.Zebra_Dialog.popWindow("/Sunnet/Tickets/AddSchedules.aspx?Date=" + day + "&r=" + Math.random(), "Add Schedules", 580, 500, function () {
                $("#" + "<%=iBtnSearch.ClientID%>").click();
            });
        }

        function OpenEditSchdules(id, targetUserId) {
            $.Zebra_Dialog.popWindow("/Sunnet/Tickets/EditSchedules.aspx?ID=" + id + "&target=" + targetUserId + "&r=" + Math.random(), "Edit Schedules", 580, 500, function () {
                if (dialogParam && dialogParam.type === "new") {
                    OpenAddModuleDialog(dialogParam.date);
                    dialogParam = null;
                }
                else {
                    $("#" + "<%=iBtnSearch.ClientID%>").click();
                }
            });
        }
    </script>

</asp:Content>
