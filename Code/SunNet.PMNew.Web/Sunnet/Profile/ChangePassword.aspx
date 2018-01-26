<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Profile.ChangePassword" %>

<%@ Register Src="../../UserControls/ChangePassword.ascx" TagName="ChangePassword"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Change Password
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ChangePassword ID="ChangePassword1" runat="server" />
</asp:Content>
