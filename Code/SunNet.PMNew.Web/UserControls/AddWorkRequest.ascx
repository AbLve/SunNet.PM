<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddWorkRequest.ascx.cs" Inherits="SunNet.PMNew.Web.UserControls.AddWorkRequest" %>
<%@ Register Src="UploadFile.ascx" TagName="UploadFile" TagPrefix="uc1" %>


<script type="text/javascript">

    function baseValidate() {

        if ($("#<%=ddlProject.ClientID%>").get(0).value <= 0) {
            ShowMessage("Please select a project.", 0, false, false);
            $("#<%=ddlProject.ClientID%>").focus();
            return false;
        }
        else if ($("#<%=txtTitle.ClientID%>").val() == "") {
            ShowMessage("Please enter the Title.", 0, false, false);
            $("#<%=txtTitle.ClientID%>").focus();
            return false;
        }
        return true;
    }
</script>
    <div class="owmainBox">
        <table width="696" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th width="100">
                    Project:<span class="redstar">*</span>
                </th>
                <td width="225">
                    <asp:DropDownList ID="ddlProject" Validation="true"  runat="server" CssClass="select230" TabIndex="1">
                    </asp:DropDownList>
                </td>
                <th width="100">
                    Payment:
                </th>
                <td width="225">
                    <asp:DropDownList ID="ddlPayment" runat="server" CssClass="select226"  TabIndex="4">
                        <asp:ListItem Text="Please select..." Value="0"></asp:ListItem>
                        <asp:ListItem Text="Quote Approval" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Invoiced" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Paid" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Request #:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtRequestNo"  TabIndex="2" BackColor="#eeefed" CssClass="input223" runat="server" disabled="disabled"> </asp:TextBox>
                </td>
                <th>
                    Invoice #:
                </th>
                <td>
                    <asp:TextBox ID="txtInvoiceNo" CssClass="input220" runat="server"  TabIndex="5"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Status:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlStatus" CssClass="select230" runat="server"  TabIndex="3">
                        <asp:ListItem Text="Proposal Submitted" Value="1"></asp:ListItem>
                        <asp:ListItem Text="In Progress" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Approval" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Cancel" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th class="prority">
                    Due Date:
                </th>
                <td class="prority">
                    <%--<asp:TextBox ID="txtDueDate" Validation="true" length="8-20" RegType="date" CssClass="input180"
                        onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);' runat="server"> </asp:TextBox>&nbsp;--%>
                    <asp:TextBox ID="txtDueDate"  TabIndex="6" Validation="true" length="8-20" RegType="date" CssClass="input193"
                        onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);' runat="server"> </asp:TextBox>&nbsp;
                    <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtDueDate.ClientID %>"),document.getElementById("<%=txtDueDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                        align="absmiddle">
                </td>
            </tr>
            <tr>
                <th>
                    Title:<span class="redstar">*</span>
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtTitle" CssClass="input570" runat="server" TabIndex="7" > </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Description:
                </th>
                <td  colspan="3">
                    <asp:TextBox ID="txtDescription"  TabIndex="8" Validation="true"  length="1-5" CssClass="input595" TextMode="MultiLine"
                        Rows="5" runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="prority">
                    Work Scope:<span class="redstar">*</span>
                </th>
                <td  colspan="3">
                    <asp:FileUpload ID="fileProject" Width="205" runat="server" />
                    
                    <asp:Label id="lblFile" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    
    
                        <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT) { %>
<div id="btnForm" runat="server" class="owmainBox">
    <div class="btnBoxone" style="padding: 5px 25px;  width:696px;">
        <asp:Button ID="btnSave" CssClass="btnone" ValidationGroup="Add" CausesValidation="true"
            runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return baseValidate();" />
    </div>
</div>
                        <% }%>