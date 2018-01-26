<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewSeal.aspx.cs"
    MasterPageFile="~/Pop.master"
    Inherits="SunNet.PMNew.PM2014.Admin.NewSeal" %>

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
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="titleSection" runat="server">
    New Seal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodySection" runat="server">
    
        <div class="form-group">
            <label class="col-left-profile lefttext">Seal Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtSealName"   MaxLength="200" runat="server"
                    TabIndex="1" CssClass="inputProfle1 required"></asp:TextBox>
                <asp:HiddenField ID="hdID" runat="server" />
            </div>

            <label class="col-left-profile lefttext">Owner:</label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlOwner" runat="server" CssClass="selectProfle1" DataTextField="FirstName" DataValueField="UserID">
                </asp:DropDownList>

            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Approver:</label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlApprover" runat="server" CssClass="selectProfle1" DataTextField="FirstName" DataValueField="UserID">
                </asp:DropDownList>
            </div>

        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Description:</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="inputProfle1"
                    Rows="6"></asp:TextBox>
            </div>

        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Status:</label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="selectProfle1">
                    <asp:ListItem Value="0">Active</asp:ListItem>
                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                </asp:DropDownList>
            </div>

        </div>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">

    <div class="buttonBox3">
        <asp:Button ID="btnSave" Text=" Save " CssClass="saveBtn1 mainbutton" runat="server"  OnClick="btnSave_Click" />
        <input name="btnCancel" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
    </div> 
</asp:Content>
