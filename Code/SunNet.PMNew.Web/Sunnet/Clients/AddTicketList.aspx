<%@ Page Title="Add Multiple Tickets" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="AddTicketList.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="SunNet.PMNew.Web.Sunnet.Clients.AddTicketList" %>

<%@ Register Src="../../UserControls/AddTicket.ascx" TagName="AddTicket" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="owTopone_left">
        Add Multiple Tickets
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <uc1:AddTicket ID="AddTicket1" runat="server" />
        <div class="btnBoxone">
            <asp:Button ID="Button1" runat="server" Text="Save as Draft" CssClass="btnone" OnClick="Button1_Click" />
            <asp:Button ID="Button2" runat="server" Text="Submit" CssClass="btnone" OnClick="Button2_Click" />
        </div>
        <uc1:AddTicket ID="AddTicket2" runat="server" />
        <div class="btnBoxone">
            <asp:Button ID="Button3" runat="server" Text="Save as Draft" CssClass="btnone" OnClick="Button3_Click" />
            <asp:Button ID="Button4" runat="server" Text="Submit" CssClass="btnone" OnClick="Button4_Click" />
        </div>
        <uc1:AddTicket ID="AddTicket3" runat="server" />
        <div class="btnBoxone">
            <asp:Button ID="btnSave" runat="server" Text="Save as Draft" CssClass="btnone" OnClick="btnSave_Click" />
            <asp:Button ID="btnSub" runat="server" Text="Submit" CssClass="btnone" OnClick="btnSub_Click" />
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btnone" OnClick="btnClose_Click" />
        </div>
    </div>
</asp:Content>
