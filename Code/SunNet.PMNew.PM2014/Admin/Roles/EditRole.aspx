<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="EditRole.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Roles.EditRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.maskedinput-1.3.1.min.js"></script>
    <script type="text/javascript">
        jQuery(function () {
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="titleSection" runat="server">
    Edit Role
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodySection" runat="server">

    <div class="form-group">
        <label class="col-left-profile lefttext">Role Name:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtRoleName" CssClass="inputProfle1 required" MaxLength="50" runat="server"></asp:TextBox>
        </div>

        <label class="col-left-profile lefttext">Description:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtDesc" CssClass="inputProfle1 required" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-profile lefttext">Status:</label>
        <div class="col-right-profile1 righttext">
            <asp:DropDownList ID="ddlStatus" CssClass="selectProfle1" runat="server">
                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
            </asp:DropDownList>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">

    <div class="buttonBox3">
        <asp:Button ID="btnSave" Text=" Save " CssClass="saveBtn1 mainbutton" runat="server" OnClick="btnSave_Click" />
        <input name="btnCancel" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
    </div>
</asp:Content>