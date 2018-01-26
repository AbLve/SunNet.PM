<%@ Page Title="iCalendar" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="iCalendar.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.iCalendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    iCalendar
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <div class="icldText">
            <strong>Sync Calendar</strong><br>
            The 'iCalendar' function allows you to quickly and easily transfer request information
            directly into your Calendar (Microsoft Outlook, Google Calendar, Yahoo! Calendar,
            Apple iCal, etc.)</div>
        <p class="icldText2">
            Please use the following address to access your calendar from other applications.
            You can copy and paste this into any calendar product that supports the iCal format.
        </p>
        <p class="icldText3">
            <a href="#">http://pm.sunnet.us:443/Home/Page/ServiceRequest/iCalFeed.aspx?Id=4a8ae832-fa41-4d56-bb43-547237eca723&cid=4331e6aa-f1ac-4b0e-a354-1f3f93e5a9f3
            </a>
        </p>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
    </div>
</asp:Content>
