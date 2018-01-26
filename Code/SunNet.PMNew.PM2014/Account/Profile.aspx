<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Account/Account.master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SunNet.PMNew.PM2014.Account.Profile" %>

<asp:Content ID="Content4" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        jQuery(function () {
            jQuery("#<%=txtPhone.ClientID %>").add("#<%=txtEPhone.ClientID %>").mask("(999) 999-9999");
            // validate signup form on keyup and submit
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
        });
    </script>
    <style type="text/css">
        .form-group-container { 
            width:720px;
        }
        .col-left-profile { 
            width:125px;
        }
        .col-left-profile-sm {
            width:80px
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="searchSection" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="dataSection" runat="server">

    <div class="contentTitle titleprofile">Basic Information</div>
    <div class="form-group-container">
        <div class="form-group" style="width: 660px;">
            <label class="col-left-profile lefttext">Company:</label>
            <div class="col-right-profile1 righttext">
                <asp:Literal runat="server" ID="ltlCompany"></asp:Literal>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">User Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtUserName" length="50" CssClass="inputProfle1 email required" TabIndex="1"
                    runat="server"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext col-left-profile-sm">Phone:</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtPhone" CssClass="inputProfle1 phone" MaxLength="50" TabIndex="4"
                    runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">First Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtFirstName" MaxLength="20" CssClass="inputProfle1 required" TabIndex="2"
                    runat="server"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext col-left-profile-sm">Title:</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtTitle" CssClass="inputProfle1" MaxLength="100" TabIndex="5" runat="server"></asp:TextBox>
            </div>

        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Last Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtLastName" MaxLength="20" CssClass="inputProfle1 required" TabIndex="3"
                    runat="server"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext col-left-profile-sm">Skype:</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtSkype" CssClass="inputProfle1" MaxLength="50" TabIndex="6" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Notification Email:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtBoxEmail" MaxLength="50" CssClass="inputProfle1 email required" TabIndex="7"
                    runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="seplineOne"></div>
        <asp:PlaceHolder ID="phlEmergency" runat="server" Visible="false">
            <div class="contentTitle titleprofile">Emergency Contact </div>
            <div class="form-group">
                <label class="col-left-profile lefttext">First Name:<span class="noticeRed">*</span></label>
                <div class="col-right-profile1 righttext">
                    <asp:TextBox ID="txtEFirstName" MaxLength="20" CssClass="inputProfle1 required" TabIndex="8"
                        runat="server"></asp:TextBox>
                </div>
                <label class="col-left-profile lefttext">Email:<span class="noticeRed">*</span></label>
                <div class="col-right-profile1 righttext">
                    <asp:TextBox ID="txtEEmail" CssClass="inputProfle1 email required" TabIndex="9"
                        runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label class="col-left-profile lefttext">Last Name:<span class="noticeRed">*</span></label>
                <div class="col-right-profile1 righttext">
                    <asp:TextBox ID="txtELastName" MaxLength="20" CssClass="inputProfle1 required" TabIndex="10"
                        runat="server"></asp:TextBox>
                </div>
                <label class="col-left-profile lefttext">Phone:<span class="noticeRed">*</span></label>
                <div class="col-right-profile1 righttext">
                    <asp:TextBox ID="txtEPhone" MaxLength="15" CssClass="inputProfle1 phone required" TabIndex="11"
                        runat="server"></asp:TextBox>
                </div>
            </div>

        </asp:PlaceHolder>
        <div class="buttonBox1">
            <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" ValidationGroup="Add" Causes
                runat="server" Text="Save" OnClick="btnSave_Click" TabIndex="12" />
                
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>      
