<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="AddFeedBacks.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.AddFeedBacks" %>

<%@ Register Src="../../UserControls/AddFeedBack.ascx" TagName="AddFeedBack" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="opendivTopone_left">
        Add Feedback</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:AddFeedBack ID="AddFeedBack1" runat="server" />
</asp:Content>
