<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Profile.ascx.cs" Inherits="SunNet.PMNew.Web.UserControls.Profile" %>
<div class="mainrightBoxfour">
    <div class="mainmenuBox">
        <img src="/icons/19.gif" align="absmiddle" />
        Basic Information</div>
    <div class="owmainBox">
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <td width="20">
                </td>
                <th width="120">
                    Company:
                </th>
                <td width="*">
                    <asp:DropDownList ID="ddlCompany" CssClass="select635" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <th>
                    First Name:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtFirstName" Validation="true" length="1-20" CssClass="input630"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <th>
                    Last Name:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtLastName" Validation="true" length="1-20" CssClass="input630"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <th>
                    UserName:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtUserName" Validation="true" RegType="email" length="1-50" CssClass="input630"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <th>
                    Title:
                </th>
                <td>
                    <asp:TextBox ID="txtTitle" CssClass="input630" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <th>
                    Phone:
                </th>
                <td>
                    <asp:TextBox ID="txtPhone" Validation="true"  CssClass="input630"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <th>
                    Skype:
                </th>
                <td>
                    <asp:TextBox ID="txtSkype" CssClass="input630" MaxLength="50" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr style="display: none;">
                <td>
                </td>
                <th>
                    Status:
                </th>
                <td>
                    <asp:DropDownList Visible="false" ID="ddlStatus" CssClass="select635" runat="server">
                        <asp:ListItem Text="Active" Value="ACTIVE"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="INACTIVE"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="display: none;">
                <td>
                </td>
                <th>
                    Password:
                </th>
                <td>
                    <asp:TextBox ID="txtPassword" Validation="true" Visible="false" length="1-15" TextMode="Password"
                        RegType="password" CssClass="input630" MaxLength="50" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr style="display: none;">
                <td>
                </td>
                <th>
                    Confirm:
                </th>
                <td>
                    <asp:TextBox ID="txtConfirmPassword" Visible="false" Validation="true" length="1-15"
                        TextMode="Password" RegType="password" CssClass="input630" MaxLength="50" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <asp:PlaceHolder ID="phSunnet" runat="server">
        <div class="mainmenuBox">
            <img src="/icons/19.gif" align="absmiddle" />
            Advance Information</div>
        <div class="owmainBox">
            <table width="100%" border="0" cellspacing="0" cellpadding="5">
                <tr>
                    <td width="20">
                    </td>
                    <th width="120">
                        Role:
                    </th>
                    <td width="*">
                        <asp:DropDownList ID="ddlRole" CssClass="select635" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <th>
                        Client/Sunnet:
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlUserType" CssClass="select635" runat="server">
                            <asp:ListItem Text="Sunnet" Value="SUNNET"></asp:ListItem>
                            <asp:ListItem Text="Client" Value="CLIENT"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <th>
                        Office:
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlOffice" runat="server" CssClass="select635">
                            <asp:ListItem Text="US" Value="US"></asp:ListItem>
                            <asp:ListItem Text="CN" Value="CN"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phClient" runat="server">
        <div class="mainmenuBox">
            <img src="/icons/19.gif" align="absmiddle" />
            Advance Information</div>
        <div class="owmainBox">
            <table width="100%" border="0" cellspacing="0" cellpadding="5">
                <tr>
                    <th colspan="3">
                        &nbsp;&nbsp;&nbsp; Emergency Contact
                    </th>
                </tr>
                <tr>
                    <td width="20">
                    </td>
                    <th width="120">
                        First Name:<span class="redstar">*</span>
                    </th>
                    <td width="*">
                        <asp:TextBox ID="txtEFirstName" MaxLength="20" Validation="true" length="1-20" CssClass="input630"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <th>
                        Last Name:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtELastName" MaxLength="20" Validation="true" length="1-20" CssClass="input630"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <th>
                        Email:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtEEmail" Validation="true" RegType="email" CssClass="input630"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <th>
                        Phone:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtEPhone" Validation="true" length="1-15" CssClass="input630"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </asp:PlaceHolder>
    <div class="btnBoxone" style="text-align: left; padding-left: 165px;">
        <asp:Button ID="btnSave" CssClass="btnone" ValidationGroup="Add" CausesValidation="true"
            runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return Validate();" />
    </div>
</div>

<script type="text/javascript">
    jQuery(function() {
        jQuery("#<%=txtPhone.ClientID %>").add("#<%=txtEPhone.ClientID %>").mask("(999) 999-9999");
    });
    
</script>



