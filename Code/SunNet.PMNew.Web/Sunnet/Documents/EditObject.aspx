<%@ Page Title="Edit Directory" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="EditObject.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Documents.EditObject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Edit Directory
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <table width="90%" border="0" align="center" cellpadding="5" cellspacing="0" class="opendivTable">
            <tr>
                <th width="100">
                    Action
                </th>
                <td width="*">
                    <asp:RadioButton ID="rbtnNewDirectory" Enabled="false" Checked="true" GroupName="action"
                        Text="Edit Directory" runat="server" />
                    <asp:RadioButton ID="rbtnNewFile" Enabled="false" Text="Delete and Upload File" runat="server"
                        GroupName="action" />
                </td>
            </tr>
            <tr>
                <th>
                    Current Directory
                </th>
                <td>
                    <asp:DropDownList ID="ddlCurrent" Enabled="false" runat="server" CssClass="select205">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="directory">
                <th>
                    Title:
                </th>
                <td>
                    <asp:TextBox ID="txtTitle" MaxLength="100" CssClass="input200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr calss="directory file">
                <th>
                    Description:
                </th>
                <td>
                    <asp:TextBox ID="txtDesc" TextMode="MultiLine" CssClass="input200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="file" id="file">
                <th>
                    Select File:
                </th>
                <td>
                    <asp:FileUpload ID="fileCompany" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div class="btnBoxone">
        <asp:Button ID="btnSave" CssClass="btnfive" runat="server" Text="Submit" OnClick="btnSave_Click"
            OnClientClick="return Validate();" />
        <input name="button2" id="btnClientCancel" type="button" class="btnfive" value="Clear" />
    </div>

    <script language="javascript" type="text/javascript">
// <!CDATA[
        jQuery(function() {
            var rbtnNewDirectory = jQuery("#<%=rbtnNewDirectory.ClientID %>");
            rbtnNewDirectory.click(function() {
                jQuery("tr.file").hide();
                jQuery("tr.directory").show();
                return true;
            });
            var rbtnNewFile = jQuery("#<%=rbtnNewFile.ClientID %>");
            rbtnNewFile.click(function() {
                jQuery("tr.directory").hide();
                jQuery("tr.file").show();
                return true;
            });
            jQuery("input:checked").click();
        });

// ]]>
    </script>

</asp:Content>
