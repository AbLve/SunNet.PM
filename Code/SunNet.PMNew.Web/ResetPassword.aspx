<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs"
    Inherits="SunNet.PMNew.Web.ResetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <link href="Styles/login.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseCurrent() {
            if (navigator.userAgent.indexOf("MSIE") > 0) {
                if (navigator.userAgent.indexOf("MSIE 6.0") > 0) {
                    window.opener = null; window.close();
                }
                else {
                    window.open('', '_top'); window.top.close();
                }
            }
            else if (navigator.userAgent.indexOf("Firefox") > 0) {
                if (window.opener) {
                    window.close();
                }
                else {
                    window.location.href = 'about:blank ';
                }
            }
            else {
                window.opener = null;
                window.open('', '_self', '');
                window.close();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="loginLogobox">
            <img src="images/login_logo.jpg" />
        </div>
        <div class="loginTop">
            <div class="loginTop_corner">
                <img src="images/login_topright.jpg" />
            </div>
            <div class="loginTop_text">
                Your Growth Partner
            </div>
        </div>

        <div class="loginBox" style="font-family: Arial, Helvetica, sans-serif;">
            <div class="fwText3">Reset Password.</div>
            <div class="fwText4">Passwords must contain uppercase letters lowercase letters and numbers, and the length is greater than 6.</div>
            <div class="fwText5">
                Email:
                <asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
            </div>
            <div style="font-size: 15px;">
                New Password<br />
                <asp:TextBox ID="txtNewPsd" AutoCompleteType="None" MaxLength="15" TextMode="Password"
                    CssClass="inputemail inputpw" runat="server"></asp:TextBox>
                <br />
                Confirm Password<br />
                <asp:TextBox ID="txtConfirm" AutoCompleteType="None" MaxLength="15" TextMode="Password"
                    CssClass="inputemail inputpw" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="Button1" runat="server" CssClass="fwBtn1" Text="Next Step" OnClick="btnNext_Click" />
            </div>
        </div>
        <div class="loginfooter">
            Copyright &copy; 2014 SunNet Solutions.
        </div>
    </form>
</body>
</html>
