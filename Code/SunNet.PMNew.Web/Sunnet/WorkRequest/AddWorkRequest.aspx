<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sunnet/InputPop.Master"
CodeBehind="AddWorkRequest.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.WorkRequest.AddWorkRequest" %>


<%@ Register Src="../../UserControls/AddWorkRequest.ascx" TagName="AddWorkRequest" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Add Work Request
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:AddWorkRequest  IsAdd="true" ID="AddWorkRequest1" runat="server" />
</asp:Content>