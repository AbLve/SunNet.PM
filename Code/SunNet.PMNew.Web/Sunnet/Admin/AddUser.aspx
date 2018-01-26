<%@ Page Title="New Sunneter" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="AddUser.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.AddUser" %>

<%@ Register Src="../../UserControls/UserEdit.ascx" TagName="UserEdit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Add Sunneter
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:UserEdit IsAdd="true" IsSunnet="true" ID="UserEdit1" runat="server" />
</asp:Content>
