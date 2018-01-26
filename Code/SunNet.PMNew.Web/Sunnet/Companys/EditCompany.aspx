<%@ Page Title="Edit Company" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="EditCompany.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Companys.EditCompany" %>

<%@ Register src="../../UserControls/CompanyEdit.ascx" tagname="CompanyEdit" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Edit Company
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <uc1:CompanyEdit ID="CompanyEdit1" runat="server" />
    
</asp:Content>
