<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="Profile.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Profile.Profile" %>

<%@ Register Src="../../UserControls/UserEdit.ascx" TagName="UserEdit" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/Profile.ascx" TagName="Profile" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    My Profile
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc2:Profile ID="Profile1" runat="server" />
</asp:Content>
