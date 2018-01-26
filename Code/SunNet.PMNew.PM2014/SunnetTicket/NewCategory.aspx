<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="NewCategory.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.NewCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         jQuery(function () {
             jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
             $("form").validate({
                 errorElement:"div"
             });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    New Category
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <style>
        .inputw1{
            width:125px !important;
        }
    </style>
    <div class="pmreviwDiv1">
        <label class="assignuserTitle">Category Name:<span class="noticeRed">*</span> </label><span class="rightItem">
            <asp:TextBox ID="txtTitle" runat="server" class="inputw1 required"></asp:TextBox>
        </span>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button runat="server" ID="btnSubmit" CssClass="saveBtn1 mainbutton" Text="Save &amp; Close" OnClick="btnSubmit_Click" />
    <input name="Input322" type="button" class="cancelBtn1 mainbutton" data-dismiss="modal" aria-hidden="true" value="Cancel">
</asp:Content>
