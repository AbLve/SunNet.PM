<%@ Page Title="Add New Module" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="AddModule.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.AddModule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphTitle" runat="server">
    Add New Module
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owToptwo">
        <img src="/icons/19.gif" align="absmiddle" />
        Basic Information</div>
    <div class="owmainBox">
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th width="40%">
                    Parent Module:
                </th>
                <td width="60%">
                    <asp:DropDownList ID="ddlParentModule" CssClass="select205" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Title:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtModuleTitle" CssClass="input200" Validation="true" length="2-50"
                        MaxLength="50" runat="server"></asp:TextBox><br />
                </td>
            </tr>
            <tr>
                <th>
                    Path (From Root):<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtModulePath" Validation="true" length="6-500" MaxLength="500"
                        CssClass="input200" runat="server"></asp:TextBox><br />
                </td>
            </tr>
            <tr>
                <th>
                    Default Page(Relative):<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtDefaultPage" Validation="true" length="6-500" MaxLength="500"
                        CssClass="input200" Text="Default.aspx" runat="server"></asp:TextBox><br />
                </td>
            </tr>
            <tr>
                <th>
                    Extra Click Function:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtClickFunction" Validation="true" length="4-500" MaxLength="500"
                        Text="DefaultFunction(this)" CssClass="input200" runat="server"></asp:TextBox><br />
                </td>
            </tr>
            <tr>
                <th>
                    Show In Menu:
                </th>
                <td>
                    <asp:CheckBox ID="chkShow" Checked="true" Text="Selected stands for the module/page will show in left navigation"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    Priority:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtOrders" Validation="true" RegType="number" MaxLength="3" CssClass="input200"
                        runat="server"></asp:TextBox><br />
                </td>
            </tr>
        </table>
        <div class="btnBoxone">
            <asp:Button ID="btnSave" CssClass="btnone" runat="server" Text="Save" OnClick="btnSave_Click"
                OnClientClick="return Validate();" />
            <input id="btnClientCancel" name="button" type="button" class="btnone" value="Clear" />
        </div>
    </div>
</asp:Content>
