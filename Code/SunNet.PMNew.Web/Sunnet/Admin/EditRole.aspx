<%@ Page Title="Edit Role" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="EditRole.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.EditRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphTitle" runat="server">
    Edit Role
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owToptwo">
        <img src="/icons/19.gif" align="absmiddle" />
        Basic Information</div>
    <div class="owmainBox">
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
             <tr>
                <th  width="70">
                    Role Name:<span class="redstar">*</span>
                </th>
                <td  width="*">
                    <asp:TextBox ID="txtRoleName"  Validation="true" length="1-50" CssClass="input200" MaxLength="50" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Description:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtDesc"  Validation="true" length="1-500"  MaxLength="500" CssClass="input200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Status:
                </th>
                <td>
                    <asp:DropDownList ID="ddlStatus" CssClass="select205" runat="server">
                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div class="btnBoxone">
            <asp:Button ID="btnSave" CssClass="btnone" ValidationGroup="Add" CausesValidation="true"
                runat="server" Text="Save" OnClick="btnSave_Click"  OnClientClick="return Validate();"/>
            <input id="btnClientCancel" name="button" type="button" class="btnone" value="Clear" />
        </div>
    </div>
</asp:Content>
