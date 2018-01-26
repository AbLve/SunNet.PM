<%@ Page Title="Password Assistance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="SunNet.PMNew.PM2014.ResetPassword" %>

<%@ Register TagPrefix="uc2" TagName="Messager" Src="~/UserControls/Messager.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="~/UserControls/Footer.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .loginBoxn1 {
            width: 800px;
            margin: 0 auto;
            overflow: auto;
            font-size: 14px;
            padding: 80px 0px 60px 0px;
        }

        .loginleftpicBox {
            width: 412px;
            height: 407px;
            background-image: url(/images/login_picture.png);
        }

        .loginlefttextBox {
            padding-top: 200px;
            padding-left: 60px;
            width: 280px;
            color: #fff;
        }

        .loginleftText1 {
            font-size: 28px;
            color: #ffffff;
        }

        .loginleftText2 {
            color: #ffffff;
            margin-top: 10px;
            line-height: 20px;
        }

        .loginfootern1 {
            width: 800px;
            margin: 0 auto;
            text-align: center;
            font-size: 11px;
            color: #999999;
            text-align: center;
            padding-top: 10px;
            border-top: 1px solid #ccc;
        }

        .footerBox {
            margin: 10px 22px;
            min-width: 640px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="loginBoxn1">
        <uc2:Messager ID="Messager1" runat="server" />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="*">
                    <img src="/images/login_logo.png" width="316" height="54" />
                    <h3>Password Assistance</h3>
                    <table width="100%" border="0" cellspacing="0" cellpadding="4">
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td width="120">Email:</td>
                            <td width="*">
                                <asp:Literal ID="ltlEmail" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td>New Password:</td>
                            <td>
                                <asp:TextBox ID="txtNewPsd" AutoCompleteType="None" MaxLength="15" TextMode="Password"
                                    CssClass="inputProfle1" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Confirm Password:
                            </td>
                            <td>
                                <asp:TextBox ID="txtConfirm" AutoCompleteType="None" MaxLength="15" TextMode="Password"
                                    CssClass="inputProfle1" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnLogin" CssClass="btnLogin" runat="server" Text="Submit" OnClick="btnLogin_Click" />
                                <br />
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
