<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Account/Account.master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SunNet.PMNew.PM2014.Account.ChangePassword" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        jQuery(function () {
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div",
                rules: {
                    <%= txtConfirmPassword.UniqueID%>: {
                        equalTo: "#<%= txtPassword.ClientID%>"
                    }
                },
                messages: {
                    <%= txtConfirmPassword.UniqueID%>: {
                        equalTo: "Passwords do not match."
                    }
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="searchSection" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="dataSection" runat="server">
    <div class="form-group-container" style="width:450px;">
        <div class="form-group">
            <label class="col-left-password lefttext">New Password:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtPassword" MaxLength="15" minlength="5" TextMode="Password"
                    CssClass="inputProfle1 required" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="form-group">
            <label class="col-left-password lefttext">Old Password:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtOldPassword" MaxLength="15" TextMode="Password"
                    CssClass="inputProfle1 required" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="form-group">
            <label class="col-left-password lefttext">Confirm:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtConfirmPassword" MaxLength="15" TextMode="Password"
                    CssClass="inputProfle1 required" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="buttonBox1">
            <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" runat="server" Text="Save" OnClick="btnSave_Click"
                OnClientClick="return Validate();" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
