<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalerReview.aspx.cs" MasterPageFile="~/Pop.master" Inherits="SunNet.PMNew.PM2014.SunnetTicket.SalerReview" %>

<asp:Content ID="headSection" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="titleSection" ContentPlaceHolderID="titleSection" runat="server">
    Saler Review
</asp:Content>

<asp:Content ID="bodySection" ContentPlaceHolderID="bodySection" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
                <td colspan="2">
                    <div style="font-size: 13px; font-weight: bold; padding-bottom: 20px;">
                        <asp:Literal runat="server" ID="litHead"></asp:Literal>
                    </div>

                </td>
            </tr>
            <tr runat="server" id="trFinalTime">
                <td width="120">Final Time: </td>
                <td>
                    <span>
                        <asp:Literal runat="server" ID="ltrlFinalTime"></asp:Literal>
                    </span>
                </td>
            </tr>
            <tr runat="server" id="trResponsible" >
                <td width="120">Responsible User: </td>
                <td>
                    <span>
                        <asp:DropDownList ID="ddlResponsibleUser" runat="server" class="selectw3" >

                        </asp:DropDownList>
                    </span>
                </td>
            </tr>
        </tbody>
    </table>

</asp:Content>

<asp:Content ID="buttonSection" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button runat="server" ID="btnAgree" CssClass="backBtn mainbutton" Text=" Approve " OnClick="btnAgree_Click" />

    <asp:Button runat="server" ID="btnRefusal" CssClass="saveBtn1 mainbutton" Text=" Deny " OnClick="btnRefusal_Click" />
</asp:Content>

