<%@ Page Title="Schedule Monthly View" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="SchedulesMonthly.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.SchedulesMonthly" %>

<%@ Register Src="../../UserControls/OpenICalendar.ascx" TagName="OpenICalendar"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenTicketDetail(tid) {
            window.open("/Sunnet/Tickets/TicketDetail.aspx?tid=" + tid);
        }
    </script>

    <style type="text/css">
        .onscdTicket:hover
        {
            position: absolute;
            background-color: #BAD8F0;
            min-height: 90px;
            min-width: 165px;
            width: 195px;
            max-width: 200px;
        }
        .onscdTicket:hover > li
        {
            margin-bottom: 3px;
        }
        .onscdTicket:hover > li > a
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Schedule Monthly View
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <tr>
                    <td width="40">
                        Keyword:
                    </td>
                    <td width="220">
                        <asp:TextBox ID="txtKeyword" CssClass="input200" runat="server"></asp:TextBox>
                    </td>
                    <td width="40">
                        Year:
                    </td>
                    <td width="140">
                        <asp:DropDownList ID="ddlYears" AutoPostBack="true" CssClass="select205" Width="120"
                            runat="server" OnSelectedIndexChanged="ddlYears_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td width="30">
                        Status:
                    </td>
                    <td width="220">
                        <asp:DropDownList ID="ddlStatus" CssClass="select205" runat="server" Width="140">
                        </asp:DropDownList>
                    </td>
                    <td width="*">
                    </td>
                </tr>
                <tr>
                    <td>
                        Users:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlUsers" CssClass="select205" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Month:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonths" CssClass="select205" Width="120" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                            OnClick="iBtnSearch_Click" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </ContentTemplate>
        </asp:UpdatePanel>
    </table>
    <div class="mainactionBox">
        <div class="mainactionBox_left">
            <span><a href="SchedulesList.aspx">
                <img src="/icons/10.gif" border="0" align="absmiddle" />
                List View</a></span> <span><a href="SchedulesYearly.aspx">
                    <img src="/icons/11.gif" border="0" align="absmiddle" />
                    Yearly View</a></span><span class="cld">
                        <img src="/icons/31.gif" border="0" align="absmiddle" />
                        Monthly View</span>
            <uc1:OpenICalendar ID="OpenICalendar1" runat="server" />
        </div>
        <div class="mainactionBox_right">
        </div>
    </div>
    <div class="mainrightBoxthree">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="monthlyviewBox">
            <tr>
                <td>
                    <ul class="monthlyviewweek">
                        <li>Sunday</li><li>Monday</li><li>Tuesday</li><li>Wednesday</li><li>Thursday</li><li>
                            Friday</li><li>Saturday</li></ul>
                    <ul class="monthlyview">
                        <asp:Repeater ID="rptDays" OnItemDataBound="rptDays_ItemDataBound" runat="server">
                            <ItemTemplate>
                                <li class="<%#GetClassName(Eval("Datetime"),"dateToday","dateDisable","dateWeekend","")%>">
                                    <%#Eval("Datetime","{0:dd}") %>
                                    <asp:Repeater ID="rptTickets" runat="server">
                                        <HeaderTemplate>
                                            <ul class="onscdTicket">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li><a href="#" onclick="OpenTicketDetail(<%#Eval("ID") %>)" title="Ticket Detail">
                                                <%#Eval("TicketCode") %>>
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
</asp:Content>
