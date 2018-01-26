<%@ Page Title="Add Internal Ticket" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="AddTicket.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.AddTicket" %>

<%@ Register Src="../../UserControls/AddTicket.ascx" TagName="AddTicket" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(window).on('beforeunload', function () {
            if ($('#closePopWindow').attr('clicked') == '1' && isChanged()) {
                $('#closePopWindow').attr('clicked', '0');
                return '';
            }
            $('#closePopWindow').attr('clicked', '0');
        });
        $('#closePopWindow').bind('click', function () {
            if (isChanged) {

            }
        });
        function isChanged() {
            var a = [];
            a.push(function () { return $('#ctl00_ContentPlaceHolder1_AddTicket1_ddlProject').val() != ''; });
            a.push(function () { return $('#ctl00_ContentPlaceHolder1_AddTicket1_ddlTicketType').val() != '-1'; });
            a.push(function () { return $('#ctl00_ContentPlaceHolder1_AddTicket1_radioP2').prop('checked') != true; });
            a.push(function () { return $('#ctl00_ContentPlaceHolder1_AddTicket1_txtTitle').val() != ''; });
            a.push(function () { return $('#ctl00_ContentPlaceHolder1_AddTicket1_txtDesc').val() != ''; });
            a.push(function () { return $('#ticketUser').children().length != 0; });
            a.push(function () { return $('#file_uploadAddTicket1-queue').children().length != 0; });
            a.push(function () { return $('#ctl00_ContentPlaceHolder1_AddTicket1_txtStartDate').val() != ''; });
            a.push(function () { return $('#ctl00_ContentPlaceHolder1_AddTicket1_txtDeliveryDate').val() != ''; });
            a.push(function () { return $('#ctl00_ContentPlaceHolder1_AddTicket1_txtUrl').val() != ''; });
            a.push(function () { return $('#ctl00_ContentPlaceHolder1_AddTicket1_ckbEN').prop('checked') != false; });
            return a[0]() || a[1]() || a[2]() || a[3]() || a[4]() || a[5]() || a[6]() || a[7]() || a[8]() || a[9]() || a[10]();
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Add Internal Ticket
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:AddTicket ID="AddTicket1" runat="server" />
    <div class="btnBoxone">
        <input id="btnSave" type="button" satus="1" action="save" value="Submit" class="btnone" />
        <input id="btnClear" type="button" value="Clear" class="btnone" />
    </div>
</asp:Content>
