<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDocument.aspx.cs" MasterPageFile="~/Sunnet/InputPop.Master"
    Inherits="SunNet.PMNew.Web.Sunnet.WorkRequest.AddDocument" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .customWidth
        {
            width: 600px;
            height: 220px;
        }
        .owmainBox
        {
            width: 578px;
            height: 104px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="owTopone_left1">
        Add Document</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <table id="fileform" class="owlistone fileform" style="margin-bottom: 0" runat="server"
            width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th width="20">
                </th>
                <th width="10">
                    Title:<span class="redstar">*</span>
                </th>
                <td width="*">
                    <asp:TextBox ID="txtFileTitle" Validation="true" length="1-200" MaxLength="200" CssClass="input200"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20">
                </th>
                <th width="10">
                    Tags:
                </th>
                <td width="*">
                    <asp:TextBox ID="txtTags" MaxLength="200" CssClass="input200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                </th>
                <th>
                    File:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:FileUpload ID="fileProject" Validation="true" length="1-4000" Width="205" runat="server" />
                    <asp:Label ID="lblFile" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
       { %>
    <div class="btnBoxone">
        <asp:Button ID="btnSave" CssClass="btnone" ValidationGroup="Add" CausesValidation="true"
            runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return Validate();" />
        <input id="btnClientCancel" name="button" type="button" onclick="window.close()"
            class="btnone" value="Close" />
    </div>
    <%} %>

    <script type="text/javascript">
        
    </script>

</asp:Content>
