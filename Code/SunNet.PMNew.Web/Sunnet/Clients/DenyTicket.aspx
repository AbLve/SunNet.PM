<%@ Page Title="Deny" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="DenyTicket.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.DenyTicket" %>

<%@ Register Src="../../UserControls/AddFeedBack.ascx" TagName="AddFeedBack" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <asp:Label runat="server" ID="bannerTitle" CssClass="opendivTopone_left">
    Deny Reason</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainrightBoxtwo">
        <table width="90%" border="0" align="center" cellpadding="5" cellspacing="0" class="opendivTable">
            <tr>
                <th width="23%">
                    Title:<span class="redstar">*</span>
                </th>
                <td width="77%">
                    <input runat="server" id="txtTitle" type="text" class="input98p" />
                    <asp:HiddenField ID="hdTicketID" runat="server" />
                    <asp:HiddenField ID="hdProjectID" runat="server" />
                </td>
            </tr>
            <tr id="trOthers" runat="server">
                <th width="23%">
                    Others:
                </th>
                <td width="10%">
                    <asp:CheckBox ID="ckIsPublic" runat="server" Text="IsPublic" />
                    <asp:CheckBox ID="chkIsWaitClientFeedback" runat="server" Text="IsWaitClientFeedback"
                        onclick="checkPublic(this);" />
                </td>
            </tr>
            <tr runat="server" id="trOriDesc" visible="false">
                <th valign="top">
                    Original Description:
                </th>
                <td>
                    <textarea runat="server" id="txtOriginalContent" rows="4" class="input98p"></textarea>
                </td>
            </tr>
            <tr runat="server" id="trOriFile" visible="false">
                <th valign="top">
                    Original Files:
                </th>
                <td>
                    <label id="lblFiles" runat="server">
                    </label>
                </td>
            </tr>
            <tr runat="server" id="trOriDate" visible="false">
                <th valign="top">
                    Original DateTime:
                </th>
                <td>
                    <label id="lblDate" runat="server">
                    </label>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Description:
                </th>
                <td>
                    <textarea runat="server" id="txtContent" rows="4" class="input98p"></textarea>
                </td>
            </tr>
            <tr>
                <th>
                    Files:
                </th>
                <td>
                    <asp:FileUpload ID="fileUpload" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div class="btnBoxone">
        <asp:Button ID="btnSave" runat="server" CssClass="btnfive" Text="Deny" OnClick="btnSave_Click"
            OnClientClick="return baseValidate();" />
        <input type="button" class="btnfive" value="Cancel" onclick="window.close();" />
    </div>

    <script type="text/javascript">
        function btnclear() {
            $("#<%=txtTitle.ClientID%>").val("");
            $("#<%=txtContent.ClientID%>").val("");
            return false;
        }

        function baseValidate() {
            if ($.trim($("#<%=txtTitle.ClientID%>").val()) == "") {
                alert("Please enter the title.");
                return false;
            }
            return true;
        }
    </script>

</asp:Content>
