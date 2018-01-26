<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="SunNet.PMNew.PM2014.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function WinClose() {
            clearInterval(timeEvent);
            ClosePopWindow();
        }

        var timeEvent = 0;
        var _placeholder = "Email Address";
        jQuery(function () {
            var $txtUserName = jQuery("#<%=txtEmail.ClientID%>");
            if ($txtUserName.length) {
                $txtUserName.val(window.top.GetUsername()).focus();
                if ($txtUserName.val().length < 1) {
                    $txtUserName.val(_placeholder);
                }
                $txtUserName.focus(function () {
                    if ($txtUserName.val() == _placeholder) {
                        $txtUserName.val("");
                    }
                }).blur(function () {
                    if ($txtUserName.val().replace(" ", "").length < 1) {
                        $txtUserName.val(_placeholder);
                    }
                });
            }

            if (urlParams.success) {
                var timeClose = 3 * 1000;
                timeEvent = setInterval(function () {
                    if (timeClose <= 0) {
                        WinClose();
                    } else {
                        $("#timetoclose").html(timeClose / 1000);
                    }
                    timeClose = timeClose - 1000;
                }, 1000);
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Forgot Password
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <div class="fwDiv" runat="server" id="divMessage">
        <div class="fwDivtop">
            <table width="100%" border="0" cellspacing="0" cellpadding="4">
                <tr>
                    <td width="72%">
                        <p class="fwText2">Email has been sent.</p>
                        <p class="fwTextbule">Check your e-mail.</p>
                        <p>
                            If the e-mail address you entered is associated with a customer account in our records, you will receive an e-mail 
                                from us with instructions for resetting your password. If you don't receive this 
                                e-mail, please check your junk mail folder or visit our Help pages to contact Customer 
                                Service for further assistance.
                        </p>
                        <p>
                            This window will be closed in <span id="timetoclose"></span>s<br />
                        </p>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:PlaceHolder ID="phlForm" runat="server">
        <div class="clientfdText">
            Fill in your user name to reset your password.
        </div>
        <div class="form-group">
            <label class="col-left-fddeny lefttext">Your email address:</label>
            <div class="col-right-foldername righttext">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="inputw3"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Please enter email."
                    ValidationGroup="forgot" ControlToValidate="txtEmail" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid email format."
                    ControlToValidate="txtEmail" Display="Dynamic" ValidationGroup="forgot" ForeColor="Red"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button ID="btnSubmit" CssClass="btnLogin" runat="server" ValidationGroup="forgot" CausesValidation="true" Text="Submit" OnClick="btnSubmit_Click" />
    <input type="button" class="btnLoginCancel" runat="server" value="Close" data-dismiss="modal" aria-hidden="true" />
</asp:Content>
