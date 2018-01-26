<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="AddPayment.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.ProposalTracker.AddPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Add Payment
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <div class="form-group">
        <label class="col-left-fddeny lefttext">Milestone #:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">
            <asp:TextBox ID="txtMilestoneNo" MaxLength="100" CssClass="inputproject required" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-fddeny lefttext">Approved:</label>
        <div class="col-right-fddeny righttext">
            <asp:DropDownList ID="ddlApproved" runat="server" data-msg="Please select status" CssClass="selectproject" min="1">
                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                <asp:ListItem Text="No" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-fddeny lefttext">Payment #:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">
            <asp:TextBox ID="txtPaymentNo" MaxLength="100" CssClass="inputproject required" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-fddeny lefttext">Invoice #:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">
            <asp:TextBox ID="txtInvoiceNo" MaxLength="100" CssClass="inputproject required" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-fddeny lefttext">Send Date:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">
            <asp:TextBox ID="txtInvoiceSentOn" onclick="WdatePicker({isShowClear:false});" CssClass="inputprojectdate date" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-fddeny lefttext">Payment Receive Date:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">
            <asp:TextBox ID="txtReceiveOn" onclick="WdatePicker({isShowClear:false});" CssClass="inputprojectdate date" runat="server"></asp:TextBox>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button ID="btnSave" runat="server" CssClass="saveBtn1 mainbutton" Text=" Add " OnClick="btnSave_Click" />
    <input name="Input22" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
</asp:Content>
