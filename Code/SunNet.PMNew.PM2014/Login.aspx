<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SunNet.PMNew.PM2014.Login" %>

<%@ Register Src="UserControls/Footer.ascx" TagName="Footer" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Messager.ascx" TagName="Messager" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/styles/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/styles/bootstrap-theme.css" rel="stylesheet" />
    <script type="text/javascript">
        jQuery(function () {
            $("body").off("hidden.bs.modal", ".modal").off("hide.hidden.bs.modal", ".modal");
        });

        function GetUsername() {
            return $("#<%=txtUserName.ClientID%>").val();
        }
    </script>
    <style type="text/css">
        .loginBoxn1 {
            width: 800px;
            margin: 0 auto;
            overflow: auto;
            font-size: 14px;
            padding: 130px 0px 60px 0px;
        }

        .loginleftpicBox {
            width: 412px;
            height: 407px;
        }

        .loginlefttextBox {
            padding-top: 200px;
            padding-left: 60px;
            width: 280px;
            color: #fff;
        }

        .loginleftText1 {
            font-size: 24px;
            color: #333333;
        }

        .loginleftText2 {
            color: #999999;
            line-height: 20px;
        }

        .inputProfle1 {
            margin-bottom: 10px;
        }

        .loginRemember input {
            margin-top: -4px;
            margin-right: 5px;
        }

        .loginfootern1 {
            width: 800px;
            margin: 0 auto;
            text-align: center;
            font-size: 11px;
            color: #999999;
            padding-top: 10px;
            border-top: 1px solid #ccc;
        }




        .footerBox {
            margin: 10px 22px;
            min-width: 640px;
        }
        .auto-style1 {
            height: 40px;
        }
        .auto-style1 input{
            margin-top:0;
        }
        label {
            margin-left: 5px;
        }
        .in {
            display: block !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="loginBoxn1">
        <uc2:Messager ID="Messager1" runat="server" />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="484">
                    <div class="loginleftpicBox">

                        <div id="carousel-example-generic" class="carousel slide" data-ride="carousel">
                            <!-- Indicators -->


                            <!-- Wrapper for slides -->
                            <div class="carousel-inner" role="listbox">
                                <div class="item active">
                                    <img src="Images/left_b_w.png" alt="" />
                                    <div class="carousel-caption">
                                        <a href="http://www.sunnet.us/Houston-Texas-TX-web-application-development-Firm.html" target="blank">WEB APPLICATION
                                            <br/>
                                            DEVELOPMENT</a>
                                    </div>
                                </div>
                                <div class="item">
                                    <img src="Images/left_b_m.png" alt="" />
                                    <div class="carousel-caption">
                                        <a href="http://www.sunnet.us/Houston-Texas-TX-mobile-iphone-application-development-Firm.html" target="blank">MOBILE APPLICATION
                                            <br/>
                                            DEVELOPMENT</a>
                                    </div>
                                </div>
                                <div class="item">
                                    <img src="Images/left_b_p.png" alt="" />
                                    <div class="carousel-caption">
                                        <a href="http://www.sunnet.us/Houston-Texas-TX-computer-programming-Firm.html" target="blank">COMPUTER
                                            <br />
                                            PROGRAMMING</a>
                                    </div>
                                </div>
                                <div class="item">
                                    <img src="Images/left_b_a.png" alt="" />
                                    <div class="carousel-caption">
                                        <a href="http://www.sunnet.us/Houston-Texas-TX-Application-maintenance-support-Firm.html" target="blank">APPLICATION
                                            <br />
                                            MAINTENANCE</a>
                                    </div>
                                </div>
                            </div>

                            <!-- Controls -->
                            <a class="left carousel-control" href="#carousel-example-generic" role="button" data-slide="prev">
                                <img src="Images/b_a_l.png" alt="" />
                            </a>
                            <a class="right carousel-control" href="#carousel-example-generic" role="button" data-slide="next">
                                <img src="Images/b_a_r.png" alt="" />
                            </a>
                        </div>
                    </div>
                </td>
                <td width="316">
                    <img src="/images/login_logo.png" width="316" height="54" style="margin-bottom: 25px;" />
                    <table width="100%" border="0" cellspacing="0" cellpadding="4">

                        <tr>
                            <td>
                                <asp:TextBox ID="txtUserName" CssClass="inputProfle1" runat="server" placeholder="Username"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="inputProfle1" placeholder="Password" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="auto-style1">
                                <asp:CheckBox ID="chkRemember" Text=" Remember Me" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnLogin" CssClass="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" cellspacing="0" cellpadding="8" class="loginotherLink" style="margin-bottom: 20px;">
                        <tr>
                            <td height="30" valign="bottom"><a href="ForgotPassword.aspx" data-toggle="modal" data-target="#smallmodal">&raquo; Forgot Password</a></td>
                        </tr>
                    </table>

                    <table width="100%" border="0" cellspacing="0" style="border-top: 1px #eee dashed;">
                        <tr>
                            <td>

                                <div class="loginleftText2" style="margin-top: 25px;">
                                    Technical Support during normal business
                                    <br />
                                    hours  (9:00AM-3:30PM)<br />
                                    <strong>713-783-8886 ext. 111</strong>
                                </div>
                                <div class="loginleftText2">
                                    Emergency Technical Support During Non-Business Hours*<br />
                                    <strong>713-360-9898</strong>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div class="loginfootern1">
        <uc1:Footer ID="Footer1" runat="server" />
    </div>
</asp:Content>
