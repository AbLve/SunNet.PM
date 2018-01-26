<%@ Page Title="My Company" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="MyCompany.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Profile.MyCompany" %>

<%@ Register Src="../../UserControls/CompanyEdit.ascx" TagName="CompanyEdit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/openwindow.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    My Company
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:CompanyEdit ID="CompanyEdit1" runat="server" />
</asp:Content>
