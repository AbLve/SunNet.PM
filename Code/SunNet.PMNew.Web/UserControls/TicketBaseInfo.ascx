<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketBaseInfo.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.TicketBaseInfo" %>
<style type="text/css">
    .faqsText {
        display: none;
        width: auto;
        height: 11px;
    }

    .rdoEstimation {
        vertical-align: middle;
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {
        jQuery("#help").mouseover(function () {
            jQuery("#div7").slideToggle("fast");
        });
        jQuery("#help").mouseleave(function () {
            jQuery("#div7").slideUp("fast");
        });
        $("#" + "<%=rdoEs.ClientID%>").on("click", function () {
            if ($(this).prop("checked") == true) {
                updateStatusConfirm('toEs', true);
            }
        });
        $("#" + "<%= rdoNotEs.ClientID%>").on("click", function () {
            if ($(this).prop("checked") == true) {
                updateStatusConfirm('toNotEs', true);
            }
        });
    });
</script>

<%@ Register Src="ChangeStatus.ascx" TagName="ChangeStatus" TagPrefix="uc1" %>
<table width="95%" border="0" align="center" cellpadding="5" cellspacing="0">
    <tr>
        <td class="owmainTitletwo">
            <span class="text">
                <asp:Literal ID="lilProjectName" runat="server"></asp:Literal>
                ,
                <asp:Literal ID="lilTicketCode" runat="server"></asp:Literal>
                ,
                <asp:Literal ID="lilTicketPriority" runat="server"></asp:Literal>
                ,
                <asp:Literal ID="lilIsInternal" runat="server"></asp:Literal>
                <asp:Literal ID="lilTicketStatus" runat="server"></asp:Literal>
                <asp:Literal ID="lilQuestion" runat="server"></asp:Literal>
                <asp:Literal ID="lilIsABug" runat="server"></asp:Literal>
                <a href="#" id="help">
                    <img src="/icons/icn-help.png" alt="help" /></a> </span><span class="faqsText" id="div7">
                        <strong class="faqsspantext1">Project Name , Ticket Code , Ticket Priority , Ticket
                            Status </strong></span>
        </td>
    </tr>
    <tr>
        <td>
            <uc1:ChangeStatus ID="ChangeStatus1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>Created by:
            <asp:Literal ID="lilCreateBy" runat="server"></asp:Literal>
            <span class="infogray" runat="server">
                <asp:Literal ID="lilCreateTime" runat="server"></asp:Literal>
            </span><span runat="server" id="updateBy">Updated by: </span>
            <asp:Literal ID="lilModify" runat="server"></asp:Literal>
            <asp:Literal ID="lilModifyTime" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td>Estimation Needed:           
            <asp:RadioButton ID="rdoEs" runat="server" Text=" Yes" GroupName="Estimation" CssClass="rdoEstimation" />
            <asp:RadioButton ID="rdoNotEs" runat="server" Text=" No" GroupName="Estimation" CssClass="rdoEstimation" />
            <input runat="server" id="btnAssignUser" type="button" class="btnthree" onclick="OpenAssignUserTsModuleDialog();"
                value="Assign" visible="false" />

        </td>
    </tr>
    <tr runat="server" id="trEst">
        <td>Initial Time:
            <asp:Literal ID="lilInitialTime" runat="server"></asp:Literal>
            Hours <span class="infogray">
                <asp:Literal ID="lilEsUserName" runat="server"></asp:Literal></span>
            <input runat="server" id="btnEsTime" visible="false" type="button" class="btnthree"
                onclick="OpenTicketTsTimeModuleDialog(); return false;" value="Estimation">
            <asp:Literal ID="lilLookInitialEs" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr runat="server" id="trEstLast">
        <td>Final Time: <span style="color: Red;">
            <asp:Literal ID="lilFinalHours" runat="server"></asp:Literal>
            Hours </span>
            <input runat="server" id="btnEsPmTime" type="button" class="btnthree" onclick="OpenTicketTsTimeModuleDialog();"
                value="Final Estimation" visible="false">
            <asp:Literal ID="lilLookFinalEs" runat="server"></asp:Literal>

        </td>
    </tr>
    <tr>
        <td>Schedule Start Date:
            <asp:Literal ID="lilScdDate" runat="server"></asp:Literal>
            Schedule Due Date:
            <asp:Literal ID="lilDueDate" runat="server"></asp:Literal>
            <input runat="server" id="BtnUpdateSc" type="submit" class="btnthree" onclick="UpdateScDate(); return false;"
                value="Update" visible="false">
        </td>
    </tr>
</table>
