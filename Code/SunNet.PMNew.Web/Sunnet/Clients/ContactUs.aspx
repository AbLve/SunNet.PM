<%@ Page Title="Contact us" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="ContactUs.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .org
        {
            color: #FF6600;
        }
        .hui
        {
            color: #666666;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Contact us
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="clear: both; padding: 5px;">
        <p class="org">
            <strong class="hui">For Support during normal business hours (9:00AM- 3:30PM) Please
                contact your Project Manager directly.</strong><br>
            <strong class="hui">For emergency technical support after normal business hours,please
                dial</strong><br>
            <strong>713-360-9898 </strong>
        </p>
        <p class="hui">
            <strong>Company Address:</strong></p>
        <p class="hui">
            <strong>SunNet Solutions Corporation
                <br>
                9990 Richmond Ave, Ste 180
                <br>
                Houston, Texas 77042 </strong>
        </p>
        <p>
            <span class="hui"><strong>Office: </strong></span><strong><span class="org">713-783-8886
            </span></strong>
        </p>
    </div>
</asp:Content>
