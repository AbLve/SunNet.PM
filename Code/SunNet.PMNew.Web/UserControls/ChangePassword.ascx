<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.ChangePassword" %>
<div class="mainrightBoxfour">
    <div class="mainmenuBox">
        <img src="/icons/19.gif" align="absmiddle" />
        Change Password</div>
    <div class="owmainBox">
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <td width="10">
                </td>
                <th width="120">
                    Old Password:
                </th>
                <td width="*">
                    <asp:TextBox ID="txtOldPassword" Validation="true" length="1-15" TextMode="Password"
                         CssClass="input630" MaxLength="50" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
             <td>
                </td>
                <th>
                    New Password:
                </th>
                <td>
                    <asp:TextBox ID="txtPassword" Validation="true" length="6-15" TextMode="Password"
                        CssClass="input630" MaxLength="50" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>  <td>
                </td>
                <th>
                    Confirm:
                </th>
                <td>
                    <asp:TextBox ID="txtConfirmPassword" Validation="true" length="6-15" TextMode="Password"
                         CssClass="input630" MaxLength="50" runat="server"></asp:TextBox>
                </td>
            </tr>
             <tr>  <td>
                </td>
                <th>
                   
                </th>
                <td>
                   <asp:Button ID="btnSave" CssClass="btnone" runat="server" Text="Save" OnClick="btnSave_Click"
            OnClientClick="return Validate();" />
                </td>
            </tr>
        </table>
    </div>
</div>
