<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs"
    Inherits="SunNet.PMNew.Web.ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <script type="text/javascript" src="Scripts/jquery-1.8.1.min.js"></script>
    <script type="text/javascript">
        var _placeholder = "Email Address";
        var P = window.parent, D = P.loadinndlg();
        function WinClose() {
            P.cancel();
        }

        window.onload = function () {
            document.getElementById("<%=txtEmail.ClientID %>").value = window.parent.parent.window.document.getElementById("txtUserName").value;
        };
            var timeClose = 1000;

            function CloseWindow() {
                timeClose = 10000;
                setInterval(function () { if (timeClose <= 0) { WinClose(); } else { timetoclose.innerHTML = (timeClose / 1000); } timeClose = timeClose - 1000; }, 1000);
            }


            jQuery(function () {
                var _txtUserName = jQuery("#<%=txtEmail.ClientID%>");

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

    <style type="text/css">
        <!--
        body {
            margin: 0px;
        }

        input, textarea, select {
            color: #333333;
            font-family: Arial;
            font-size: 11px;
        }

        .forgotpasswordBox {
            width: 462px;
            height: 272px;
            background-image: url(/images/forgot_password_bg.jpg);
            margin: 0 auto;
        }

        .inputBox {
            padding: 106px 70px 50px 182px;
        }

        .inputEmail {
            font-family: Arial,Helvetica,sans-serif;
            font-size: 14px;
            color: #666;
            border: 1px solid #6D6D6D;
            padding: 4px;
            width: 77%;
            color: #666666;
        }

        .submitBox {
            text-align: center;
        }

        p.MsoNormal {
            margin-top: 0cm;
            margin-right: 0cm;
            margin-bottom: 10.0pt;
            margin-left: 0cm;
            line-height: 115%;
            font-size: 11.0pt;
            font-family: "Calibri", "sans-serif";
        }

        .message p.title {
            padding: 5px;
            text-align: center;
        }

        .message p.highlight {
            padding: 5px;
            color: #E47911;
        }

        .message p.content {
            text-indent: 2em;
            padding: 5px;
        }
        -->
    </style>
    <link href="Styles/login.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="fwDiv" runat="server" id="divMessage">
            <div class="fwDivtop">
                <table width="100%" border="0" cellspacing="0" cellpadding="4">
                    <tr>
                        <td width="72%">
                            <p class="fwText2">Email has been sent.</p>
                            <p class="fwTextbule">Check your e-mail.</p>
                            <p>
                                If the e-mail address you entered <span class="fwTextbule">[<asp:Literal ID="ltlEmail" runat="server"></asp:Literal>]</span>
                                is associated with a customer account in our records, you will receive an e-mail 
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
        <div class="fwDiv" id="divform" runat="server">
            <div class="fwDivtop">
                <table width="100%" border="0" cellspacing="0" cellpadding="4">
                    <tr>
                        <td>
                            <div class="fwText1">Fill in your user name to reset your password.</div>
                        </td>
                    </tr>

                    <tr>
                        <td width="72%">Your email address<br />
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="inputEmail"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Please enter email."
                                ValidationGroup="forgot" ControlToValidate="txtEmail" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                            <asp:RegularExpressionValidator ID="reeEmail" runat="server" ErrorMessage="Please enter email."
                                ControlToValidate="txtEmail" Display="Dynamic" ValidationGroup="forgot" ForeColor="Red"
                                ValidationExpression="/Email Address/"></asp:RegularExpressionValidator>

                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid email format."
                                ControlToValidate="txtEmail" Display="Dynamic" ValidationGroup="forgot" ForeColor="Red"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-top: 15px;">
                            <asp:Button ID="ibtnSubmit" runat="server" OnClick="ibtnSubmit_Click" ValidationGroup="forgot"
                                CssClass="fwBtn1" Text="Submit" />
                            <input name="Submit2" type="submit" class="fwBtn2" value="Cancel" onclick="WinClose();" /></td>
                    </tr>
                </table>
            </div>
        </div>


    </form>
    <script type="text/javascript">
        //store and rewrite ValidatorUpdateDisplay method which is generated by .net.
        //every time validate use ValidatorUpdateDisplay method to display a error message.
        //so I rewrite that method if all other validate controls is not valid I will set that validate controls is valid.
        //
        var _ValidatorUpdateDisplay = ValidatorUpdateDisplay;
        ValidatorUpdateDisplay = function (val) {
            for (var i = 0, v; i < Page_Validators.length && (v = Page_Validators[i]) != val; i++) {
                if (v.controltovalidate == val.controltovalidate && !v.isvalid)
                    //如果txtEmail是EmailAddress那么 
                    if (jQuery("#<%=txtEmail.ClientID%>").val() == _placeholder) {
                        val.isvalid = true;
                    }
                    else {
                        v.isvalid = true;
                        _ValidatorUpdateDisplay(v);
                    }
            }
            _ValidatorUpdateDisplay(val);
        }
    </script>
</body>
</html>
