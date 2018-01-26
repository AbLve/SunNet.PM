<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewClientUser.aspx.cs" 
       MasterPageFile="~/Admin/admin.master"
    Inherits="SunNet.PMNew.PM2014.Admin.Users.NewClientUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #body_body_dataSection_ticketBasicInfo_rdoPriority {
            border: 0;
        }
    </style>
     
    <script src="/Scripts/jquery.maskedinput-1.3.1.min.js"></script>
    <script type="text/javascript">
        jQuery(function ()
        {
            jQuery("#<%=txtPhone.ClientID %>").add("#<%= txtEPhone.ClientID %>").mask("(999) 999-9999");
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
        });

        function BackToList()
        {
            this.location.href = "/Admin/Users.aspx";
        }

    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contentTitle titleprofile">Basic Information </div>
     <div class="form-group-container">
    <div class="form-group">
        <label class="col-left-profile lefttext">Company:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
            <asp:DropDownList ID="ddlCompany" CssClass="selectProfle1 required" runat="server">
            </asp:DropDownList>
        </div>
        <label class="col-left-profile lefttext">Title:</label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtTitle" CssClass="inputProfle1" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-profile lefttext">First Name:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtFirstName"   MaxLength="20" CssClass="inputProfle1 required"
                runat="server"></asp:TextBox>
        </div>
        <label class="col-left-profile lefttext">Last Name:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtLastName"   MaxLength="20" CssClass="inputProfle1 required"
                runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-profile lefttext">User Name:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtUserName" Validation="true" ValidatorTitle="User Name: please enter your email address."
                 MaxLength="50" CssClass="inputProfle1 required email" runat="server"></asp:TextBox>
        </div>
        <label class="col-left-profile lefttext">Phone:</label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtPhone" CssClass="inputProfle1" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-profile lefttext">Skype:</label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtSkype" CssClass="inputProfle1" length="1-50" runat="server"></asp:TextBox>
        </div>
        <label class="col-left-profile lefttext">Status:</label>
        <div class="col-right-profile1 righttext">
            <asp:DropDownList ID="ddlStatus" CssClass="selectProfle1" runat="server">
                <asp:ListItem Text="Active" Value="ACTIVE"></asp:ListItem>
                <asp:ListItem Text="Inactive" Value="INACTIVE"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-profile lefttext">Password:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtPassword" AutoCompleteType="None" autocomplete="off" TextMode="Password"
                MaxLength="14" CssClass="inputProfle1 required password" runat="server"></asp:TextBox>
        </div>
        <label class="col-left-profile lefttext">Confirm:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtConfirmPassword" AutoCompleteType="None" autocomplete="off" MaxLength="14"
                TextMode="Password" CssClass="inputProfle1 required password" runat="server"></asp:TextBox>
        </div>
    </div>
         </div>
    <div class="seplineOne"></div>
    <div class="contentTitle titleprofile"> 
        Emergency Contact
    </div>
     <div class="form-group-container">
    <div class="form-group">
        <label class="col-left-profile lefttext">First Name:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
            <asp:TextBox ID="txtEFirstName" MaxLength="20"   ValidatorTitle="Emergency Contact First Name "
                        CssClass="inputProfle1 required" runat="server"></asp:TextBox>
        </div>

        <label class="col-left-profile lefttext">Last Name:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
           <asp:TextBox ID="txtELastName" MaxLength="20" Validation="true" ValidatorTitle="Emergency Contact Last Name "
                          CssClass="inputProfle1 required" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-profile lefttext">Email:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
          <asp:TextBox ID="txtEEmail"    CssClass="inputProfle1 email required"  runat="server"></asp:TextBox>
        </div>
        
         <label class="col-left-profile lefttext">Phone:<span class="noticeRed">*</span></label>
        <div class="col-right-profile1 righttext">
           <asp:TextBox ID="txtEPhone" Validation="true" ValidatorTitle="The Emergency Contact Phone field is required."
                        RegType="phone" CssClass="inputProfle1 phone required" runat="server"></asp:TextBox>
        </div>
    </div>
      <div class="buttonBox3">
           <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" ValidationGroup="Add" CausesValidation="true"   runat="server" Text="Save" OnClick="btnSave_Click"  />
             <input name="button2" tabindex="10" id="btnCancel"   type="button" class="redirectback backBtn mainbutton" value="Back" />
    </div>
         </div>
</asp:Content>
