<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/popWindow.Master" AutoEventWireup="true"
    CodeBehind="SealEdit.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.SealEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" action="DoAddSeal.ashx">
        <div class="owmainBox">
            <table border="0" cellspacing="0" cellpadding="5">
                <tr>
                    <th>Seal Name:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtSealName" runat="server" MaxLength="200" CssClass="input630" Width="250"></asp:TextBox>
                        <asp:HiddenField ID="hdID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Owner:
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlOwner" runat="server" CssClass="select205" Width="255" DataTextField="FirstName" DataValueField="UserID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Approver:
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlApprover" runat="server" CssClass="select205" Width="255" DataTextField="FirstName" DataValueField="UserID">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Description:
                    </th>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="input630"
                            Rows="6" Width="250"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Status:
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select205" Width="255">
                            <asp:ListItem Value="0">Active</asp:ListItem>
                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="btnBoxone">
            <asp:Button ID="btnSave" Text=" Save " CssClass="btnone" runat="server" OnClientClick="return Check(this)" />

        </div>
    </form>
    <script type="text/javascript">
        function Check(elem) {
            if ($("#<%= txtSealName.ClientID %>").val() == "") {
                alert("Please entity Seal Name.");
                return false;
            }
            $(elem).closest("form").ajaxSubmit({
                success: function (responseText, statusText, xhr, $form) {
                    if (responseText == 0) {
                        alert("Seal Name already exists.");
                    }
                    else if (responseText == 2) {
                        alert("Operation failed.");
                    }
                    else {
                        $.Zebra_Dialog.closeCurrent(elem);
                    }
                }
               , error: function (context, xhr, e, status) {
                   try {
                       alert('error');
                   }
                   catch (e) {

                   }
                   finally {

                   }
               }

            });
            return false;
        }
    </script>


</asp:Content>
