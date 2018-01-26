<%@ Page Title="Assign Users to Estimation" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="AssignUserTs.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.AssignUserTs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .customWidth
        {
            width: 460px;
        }
        .opendivBox1
        {
            width: 450px;
            height: 200px;
        }
        .th
        {
            width: 125px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="opendivTopone_left">
        Assign Users to Estimation</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <div class="owlistContainer">
            <table width="90%" border="0" align="center" cellpadding="5" cellspacing="0">
                <tr>
                    <th class="th" style="text-align: right;">
                        Estimation User:
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlEs" runat="server" Width="120">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="btnBoxone">
        <asp:Button ID="BtnSave" runat="server" CssClass="btnfive" Text="Save" OnClick="BtnSave_Click" />
        <asp:Button ID="BtnClear" runat="server" CssClass="btnfive" Text="Clear" OnClick="BtnClear_Click" />
    </div>
</asp:Content>
