<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="SunNet.PMNew.PM2014.Invoice.TM.New" %>

<%@ Import Namespace="SunNet.PMNew.Entity.InvoiceModel.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script src="/Scripts/Validate/regex.js"></script>
    <script src="/Scripts/Validate/Validator.js"></script>
    <script src="/Scripts/global.js"></script>
    <style type="text/css">
        td {
            padding-left: 30px;
            height: 40px;
        }

        i {
            color: red;
        }

        .widthleft {
            width: 125px;
        }
    </style>
</asp:Content>

<asp:Content ID="titleSection" ContentPlaceHolderID="titleSection" runat="server">
    <asp:Label ID="lbltitle" runat="server">Add</asp:Label>
    Time Material Invoice
    <asp:Label ID="lblTimeTsheetIDs" Visible="False" runat="server" CssClass="col-left-profile lefttext"></asp:Label>
</asp:Content>
<asp:Content ID="bodySection" ContentPlaceHolderID="bodySection" runat="server">
    <div class="form-group">
        <asp:Label ID="lblCompany1" runat="server" CssClass="col-left-profile lefttext">Company:</asp:Label>
        <div class="col-right-profile1 righttext">
            <u>
                <asp:Label ID="lblCompany" runat="server"></asp:Label>
            </u>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="lblHours1" runat="server" CssClass="col-left-profile lefttext">Total Hours:</asp:Label>
        <div class="col-right-profile1 righttext">
            <u>
                <asp:Label ID="lblHours" runat="server"></asp:Label>
            </u>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="lblStatus1" runat="server" CssClass=" lefttext">Invoice Current Status:</asp:Label>
        <div class="col-right-profile1 righttext">
            <u>
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </u>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="Label1" runat="server" class="col-left-profile lefttext widthleft">Invoice #:<span class="noticeRed">*</span></asp:Label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtInvoice" runat="server" class="righttext required" Style="width: 190px;"></asp:TextBox>
        </div>
    </div>

    <div class="form-group">
        <asp:Label ID="Label2" runat="server" class="col-left-profile lefttext widthleft">Send On:<span class="noticeRed">*</span></asp:Label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox CssClass="inputdate inputProfle1 required" ID="txtSendDate" queryparam="startdate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="Label3" runat="server" class="col-left-profile lefttext widthleft">Due On:<span class="noticeRed">*</span></asp:Label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox CssClass="inputdate inputProfle1  required" ID="txtDueDate" queryparam="startdate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="Label4" runat="server" class="col-left-profile lefttext widthleft">Receive On:</asp:Label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox CssClass="inputdate inputProfle1" ID="txtReceiveDate" queryparam="startdate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="Label5" runat="server" class="col-left-profile lefttext widthleft">New Invoice Status:</asp:Label>
        <div class="col-right-profile1 righttext">
            <asp:DropDownList ID="ddlStatus" runat="server" class="selectProfle1 righttext" Style="width: 196px;">
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="Label6" runat="server" class="col-left-profile lefttext widthleft">Notes:</asp:Label>
        <div class="righttext">
            <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" class="righttext" Style="width: 310px; height: 130px"></asp:TextBox>
        </div>
    </div>
    <br />
    <div class="sepline2"></div>
</asp:Content>
<asp:Content ID="buttonSection" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button ID="btnsave" runat="server" Text="Submit" CssClass="saveBtn1 mainbutton" OnClick="AddInvoice" OnClientClick="return Validate(),ValidInvoiceStatus();" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" data-dismiss="modal" aria-hidden="true" CssClass="cancelBtn1 mainbutton" />

    <script type="text/javascript">
        jQuery(function () {
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
            delCookie('timeTsheetIDs');
        });
        function delCookie(name) {
            var exp = new Date();
            exp.setTime(exp.getTime() - 1);
            var cval = getCookie(name);
            if (cval != null)
                document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
        }
        function getCookie(name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
        }

        function ValidInvoiceStatus() {
            $invoice = $("#<%=txtInvoice.ClientID%>");
            $sendOn = $("#<%=txtSendDate.ClientID%>");
            $dueOn = $("#<%=txtDueDate.ClientID%>");
            $receiveOn = $("#<%=txtReceiveDate.ClientID%>");
            $status = $("#<%=ddlStatus.ClientID%>");
            if ($invoice.val() != "" && $sendOn.val() != "" && $dueOn.val() != "" && $receiveOn.val() != "") {
                if ($status.val() == "<%=(int)InvoiceStatus.Awaiting_Payment%>" && $status.val() != "<%=(int)InvoiceStatus.Waive%>") {
                    alert("If Invoice #,Send On ,Due On,Receive On are not empty,the New Invoice Status should be Payment Received.");
                    return false;
                }
            }
            else if ($invoice.val() != "" && $sendOn.val() != "" && $dueOn.val() != "" && $receiveOn.val() == "") {
                if ($status.val() != "<%=(int)InvoiceStatus.Awaiting_Payment%>" && $status.val() != "<%=(int)InvoiceStatus.Waive%>") {
                    alert("If Invoice #,Send On and Due On are not empty,the New Invoice Status should be Awaiting Payment.");
                    return false;
                }
            }
    }
    </script>
</asp:Content>
