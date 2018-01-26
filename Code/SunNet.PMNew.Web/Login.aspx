<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SunNet.PMNew.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="/Styles/login.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/windialog/lhgcore.js" type="text/javascript"></script>

    <script src="Scripts/windialog/lhgdialog.js" type="text/javascript"></script>

    <script type="text/javascript">
        function WinOpen() {
            J.dialog.get({ id: 'forgot', title: 'Forgot Password', page: '/ForgotPassword.aspx', skin: 'default', width: 475, height: 343, rang: true, cover: true, nofoot: true });
        }
    </script>

    <script src="/do/js.ashx" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">

        <div class="loginBoxn1">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="484">
                        <div class="loginleftpicBox">
                            <div class="loginlefttextBox">
                                <div class="loginleftText1">Your Growth Partner</div>
                                <div class="loginleftText2">
                                    Technical Support during normal business
                                    <br />
                                    hours  (9:00AM-3:30PM)<br />
                                    <strong>713-783-8886 ext. 111</strong>
                                </div>
                                <div class="loginleftText2">
                                    Emergency Technical Support During Non-Business Hours*<br />
                                    <strong>713-360-9898</strong>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td width="316">
                        <img src="/images/login_logo.png" width="316" height="54" />
                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                    Fill your email address and password to login</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtUserName" CssClass="inputemail" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="inputemail" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="loginRemember">
                                    <asp:CheckBox ID="chkRemember" Text="Remember Me" runat="server" />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnLogin" CssClass="fwBtn1" runat="server" Text="Login" OnClick="btnLogin_Click" />
                                    <br />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <table width="100%" border="0" cellspacing="0" cellpadding="8" class="loginotherLink">
                            <tr>
                                <td height="30" valign="bottom"><a href="#"></a><a href="#" onclick="WinOpen();">&raquo; Forgot Password</a></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="loginfootern1">Copyrights &copy; 2014 SunNet Solutions.</div>
    </form>

    <script type="text/javascript">
        function isPlaceholderSupport() {
            return 'placeholder' in document.createElement('input');
        }
        jQuery(function () {
            var _txtUserName = jQuery("#<%=txtUserName.ClientID %>");
            var _placeholder = "Email Address";
            if (_txtUserName.val().length < 1) {
                _txtUserName.val(_placeholder);
            }
            _txtUserName.focus(function () {
                if (_txtUserName.val() == _placeholder) {
                    _txtUserName.val("");
                }
            }).blur(function () {
                if (_txtUserName.val().replace(" ", "").length < 1) {
                    _txtUserName.val(_placeholder);
                }
            });
        });
    </script>

</body>
</html>
