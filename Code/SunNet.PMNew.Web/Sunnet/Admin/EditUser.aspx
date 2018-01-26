<%@ Page Title="Edit User" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="EditUser.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.EditUser" %>

<%@ Register Src="../../UserControls/UserEdit.ascx" TagName="UserEdit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="/Styles/global.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Edit User
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UserEdit ID="UserEdit1" runat="server" />
</asp:Content>
