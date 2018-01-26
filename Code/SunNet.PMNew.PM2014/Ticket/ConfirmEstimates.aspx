<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmEstimates.aspx.cs" MasterPageFile="~/Pop.master" Inherits="SunNet.PMNew.PM2014.Ticket.ConfirmEstimates" %>
<asp:Content ID="headSection" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="titleSection" ContentPlaceHolderID="titleSection" runat="server">
    Waiting Confirm
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
                <td width="120">Estimation: </td>
                <td>
                    <span>
                        <asp:Literal runat="server" ID="ltrlFinalTime"></asp:Literal>
                    </span>

                </td>
                <td>
                    
    <asp:Button runat="server" ID="btnAgree" CssClass="backBtn mainbutton small" Text=" Approve " OnClick="btnAgree_Click" />

    <asp:Button runat="server" ID="btnRefusal" CssClass="saveBtn1 mainbutton small" Text=" Deny " OnClick="btnRefusal_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="border-bottom:1px #ddd dashed; padding-top:10px; "></td>
            </tr>
            <tr>
                <td style="padding-top:10px;">
                Estimation Approver:
                    </td>
                <td>
                    <asp:DropDownList ID="ddlClient" runat="server" DataTextField="FirstAndLastName" DataValueField="UserID"></asp:DropDownList>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSave" CssClass="saveBtn1 mainbutton small" Text=" Save " OnClick="btnSave_Click" />
                </td>
            </tr>
        </tbody>
    </table>

</asp:Content>

<asp:Content ID="buttonSection" ContentPlaceHolderID="buttonSection" runat="server">
</asp:Content>
