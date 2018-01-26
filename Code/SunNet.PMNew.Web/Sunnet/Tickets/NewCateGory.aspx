<%@ Page Title="New Category" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="NewCateGory.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.NewCateGory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    New Category
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <table width="90%" border="0" align="center" cellpadding="5" cellspacing="0" class="opendivTable">
            <tr>
                <th width="100">
                    Category Name:<span class="redstar">*</span>
                </th>
                <td width="*">
                    <asp:TextBox ID="txtTitle" Validation="true" length="1-100" MaxLength="12" CssClass="input98p"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="btnBoxone">
        <asp:Button ID="btnSave" CssClass="btnfive" runat="server" Text="Submit" OnClick="btnSave_Click"
            OnClientClick="return Validate();" />
        <input name="button2" id="btnClientCancel" type="button" class="btnfive" value="Clear" />
    </div>
</asp:Content>
