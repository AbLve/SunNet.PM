<%@ Page Title="Schedule Yearly View" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="SchedulesYearly.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.SchedulesYearly" %>

<%@ Register Src="../../UserControls/OpenICalendar.ascx" TagName="OpenICalendar"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Schedule Yearly View
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="300">
                Keyword:
                <asp:TextBox ID="txtKeyword"  CssClass="input200"
                    runat="server"></asp:TextBox>
            </td>
            <td width="220">
                Status:
                <asp:DropDownList ID="ddlStatus" CssClass="select205" runat="server" Width="140">
                </asp:DropDownList>
            </td>
            <td width="300">
                Users:
                <asp:DropDownList ID="ddlUsers" CssClass="select205" runat="server">
                </asp:DropDownList>
            </td>
            <td width="140">
                Years:
                <asp:DropDownList ID="ddlYears" CssClass="select150" runat="server" Width="80">
                </asp:DropDownList>
            </td>
            <td width="*">
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <div class="mainactionBox_left">
            <span><a href="SchedulesList.aspx">
                <img src="/icons/10.gif" border="0" align="absmiddle">
                List View</a></span> <span class="cld">
                    <img src="/icons/11.gif" border="0" align="absmiddle">
                    Yearly View</span><span> <a href="SchedulesMonthly.aspx">
                        <img src="/icons/31.gif" border="0" align="absmiddle">
                        Monthly View</a></span>
            <uc1:OpenICalendar ID="OpenICalendar1" runat="server" />
        </div>
        <div class="mainactionBox_right">
        </div>
    </div>
    <div class="mainrightBoxthree">
        <ul class="yearlyview">
            <asp:Repeater ID="rptMonths" runat="server">
                <ItemTemplate>
                    <li>
                        <div class="sdcyearlyTop">
                            <%#Eval("Datetime","{0:MMMM  yyyy}")%>:</div>
                        <div class="sdcyearlymain">
                            <%#Eval("TicketsCount") %></div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</asp:Content>
