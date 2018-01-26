<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="YearsUserCon.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.OA.YearsUserCon" %>
<asp:DropDownList ID="ddlYears" queryparam="year" runat="server" CssClass="selectw1" Width="120px" onchange="yearchange(this)">
</asp:DropDownList>