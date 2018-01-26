<%@ Page Title="View Related Ticket" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="ViewRelatedTicket.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.ViewRelatedTicket" %>

<%@ Register Src="../../UserControls/AddTicket.ascx" TagName="AddTicket" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    View Related Ticket
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:AddTicket ID="AddTicket1" runat="server" />
</asp:Content>
