<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SunNet.PMNew.Web.Register" %>

<%@ Register Src="UserControls/ClientMaintenancePlan.ascx" TagName="ClientMaintenancePlan"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <link href="Styles/login.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.8.0.min.js" type="text/javascript"></script>

    <script src="Scripts/MessageBox.js" type="text/javascript"></script>

    <script src="Scripts/ShowMsgCommon.js" type="text/javascript"></script>

    <script src="Scripts/Validate/regex.js" type="text/javascript"></script>

    <script src="Scripts/Validate/Validator.js" type="text/javascript"></script>

</head>
<body>
   <%-- <form id="form1" runat="server">
    <div class="registLogobox">
        <div class="topleftLogo">
            <img src="images/login_logo.jpg" /></div>
        <div class="toprightLink">
            <a href="Login.aspx">&raquo; Login</a></div>
    </div>
    <div class="loginTop">
        <div class="loginTop_corner">
            <img src="images/login_topright.jpg" /></div>
        <div class="loginTop_text">
            Your Growth Partner</div>
    </div>
    <div class="loginBox">
        <div class="registtitle1">
            <img src="images/ico_registration.gif" width="19" height="19" align="absmiddle" />
            Account Registration
        </div>
        <div class="registBox">
            <table width="100%" border="0" align="center" cellpadding="3" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <div class="registsubTitle">
                            Basic Information: <span class="redStar">* Indicates required fields </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td width="21%">
                        Company Name:<span class="redStar">*</span>
                    </td>
                    <td width="79%">
                        <asp:TextBox ID="txtCompanyName" Validation="true" length="1-20" ValidatorTitle="Company Name Required,length between 1 and 200."
                            AutoCompleteType="None" AutoComplete="off" CssClass="input260" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        First Name:<span class="redStar">*</span>
                    </td>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="26%">
                                    <asp:TextBox ID="txtFirstName" Validation="true" length="1-20" ValidatorTitle="First Name Required,length between 1 and 20."
                                        AutoCompleteType="None" AutoComplete="off" CssClass="input100" runat="server"></asp:TextBox>
                                </td>
                                <td width="17%">
                                    Last Name: <span class="redStar">*</span>
                                </td>
                                <td width="57%">
                                    <asp:TextBox ID="txtLastName" Validation="true" length="1-20" ValidatorTitle="Last Name Required,length between 1 and 20."
                                        CssClass="input100" AutoCompleteType="None" AutoComplete="off" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        Username:<span class="redStar">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" Validation="true" RegType="email" length="1-50" ValidatorTitle="User Name Required a email address."
                            CssClass="input260" AutoCompleteType="None" AutoComplete="off" runat="server"></asp:TextBox>
                        <span class="noticeText"></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        Retype username:<span class="redStar">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserNameConfirm" Validation="true" RegType="email" length="1-50"
                            ValidatorTitle="Please retype your username." AutoCompleteType="None" AutoComplete="off"
                            CssClass="input260" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Password:<span class="redStar">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" Validation="true" RegType="password" length="4-20"
                            ValidatorTitle="Password allows only contain uppercase letters, lowercase letters, numbers and underscores, beginning with a letter."
                            TextMode="Password" CssClass="input260" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Confirm password:<span class="redStar">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPasswordConfirm" Validation="true" ValidatorTitle="Please retype your password."
                            RegType="password" length="4-20" TextMode="Password" CssClass="input260" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="registsubTitle">
                            Emergency Business Contact:</div>
                    </td>
                </tr>
                <tr>
                    <td>
                        First Name:
                    </td>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="26%">
                                    <asp:TextBox ID="txtFirstNameEmger" CssClass="input100" runat="server"></asp:TextBox>
                                </td>
                                <td width="17%">
                                    Last Name:
                                </td>
                                <td width="57%">
                                    <asp:TextBox ID="txtLastNameEmger" CssClass="input100" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="padding-top: 6px;">
                        My Organization:
                    </td>
                    <td>
                        <uc1:ClientMaintenancePlan ID="ClientMaintenancePlan1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="registsubTitle">
                            IT Consulting Agreement:</div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <textarea name="textarea" rows="8" class="textarea460" readonly="readonly">IT CONSULTING SERVICES AGREEMENT

This SOFTWARE CONSULTING AGREEMENT (“Agreement”) is made BETWEEN you (“Client”), and SunNet Solutions Corporation (“Provider”).
 
Client desires to obtain the services (“Services”) of Provider to assist in software development, implementation and deployment, and Provider is willing to provide the Services subject to the terms and conditions set forth herein. The project description and other related information is stated on Appendix A if any. NOW, THEREFORE, in consideration of the foregoing and the mutual covenants, representations and warranties contained in this Agreement, Client and Provider agree as follows: 

Provider makes no further warranty of any kind, whether expressed or implied, of merchantability or fitness of the Services for a particular purpose whatsoever.  In no event shall Provider be responsible for any lost profits or other damages, including direct, indirect, incidental, special, consequential, or any other damages, however caused.  In no event shall the aggregate liability of Provider exceed the amount of fees paid by Client. Provider does warrant that all of the deliverables included in this agreement will be conveyed to Client and subject to acceptance.

The parties acknowledge and agree that all work-product and intellectual property derived from the Services performed by Provider hereunder, including product Documentation prepared by Provider, if any, shall become the sole exclusive property of Client. Provider agrees that sole copyright ownership belongs to Client, and reserves the right to use work for demonstrative purposes only.
The client unconditionally guarantees that any elements of text, graphics, photos, designs, software, business systems, trademarks, or other items furnished to SunNet Solutions Corporation for inclusion in the application are owned by the client, or that the client has permission from the rightful owner to use each of these elements, and will hold harmless, protect, and defend SunNet Solutions Corporation and its subcontractors from any claim or suit arising from the use of such elements furnished by the client.
If Client plans to terminate the project they must notify Provider with 30 days written notice of the intention to terminate, to avoid, amongst other things, possible accrued costs. Upon final termination, whether by project completion or early termination, all outstanding invoices and costs must be paid in full to Provider

Client agrees that for the term of this Agreement and for three hundred (300) days immediately thereafter, it shall not, directly or indirectly, hire any personnel, employed or contracted, by Provider, within the above stated period. In the event that Client breached this clause of the Agreement, Client agrees to pay to Provider a sum equal to 12 times that employee or contractor’s monthly income.

This agreement will affect any additional requests submitted at a later date than when the contract was signed.  If your application is not hosted by SunNet, it is your responsibility to change the entire login information and password after the work has been completed. If any of the information is considered confidential or needs to be returned, it is the client’s responsibility to have a written notice sent to SunNet with a receipt requested. If the client has not received the confidential information within 7 business days, it is the client’s responsibility to send SunNet a written notice.

•Provider reserves the right to use the work for demonstration purposes.
</textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input name="gnAgree" id="chkAgree" value="y" type="radio"  checked="checked"/>
                        I Agree &nbsp;&nbsp;&nbsp;<input name="gnAgree" id="chkNotAgree" value="n" type="radio" />
                        I Do Not Agree
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <div class="loginbtnBox1">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                    OnClientClick="return Validate();" CssClass="loginBtn1" />&nbsp;
                <input id="btnClear" name="button2" type="button" class="loginBtn1" value="Reset" />
            </div>
        </div>
    </div>
    <div class="loginfooter">
        Copyright &copy; 2014 SunNet Solutions.</div>
    </form>

    <script type="text/javascript">
        jQuery("#btnClear").click(function() {
            jQuery("input:text,input:password").val("");
            jQuery("input:radio").removeAttr("checked");
            jQuery("#chkNotAgree").attr("checked", "checked");
        });
        jQuery("#<%=btnSubmit.ClientID %>").click(function() {
            var chkValue = jQuery("#chkAgree").attr("checked");
            if (chkValue == undefined || chkValue != "checked") {
                ShowMessage("You must agree to the IT Consulting Agreement before your account can be registered.", 0, false, false);
                return false;
            }
            else {
                return true;
            }
        });

        jQuery(function() {
            var _txtUserName = jQuery("#<%=txtUserName.ClientID %>");
            var _placeholder = "Email Address";
            if (_txtUserName.val().length < 1) {
                _txtUserName.val(_placeholder);
            }
            _txtUserName.focus(function() {
                if (_txtUserName.val() == _placeholder) {
                    _txtUserName.val("");
                }
            }).blur(function() {
                if (_txtUserName.val().replace(" ", "").length < 1) {
                    _txtUserName.val(_placeholder);
                }
            });
        });

        function RegisteSuccess(msg) {
            alert(msg);
            window.location.href = "/Sunnet/Profile/MyCompany.aspx";
        }
    </script>--%>

</body>
</html>
