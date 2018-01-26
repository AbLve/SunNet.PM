<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientTicketBaseInfo.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.ClientTicketBaseInfo" %>
<style type="text/css">
    .faqsText
    {
        display: none;
        width: auto;
        height: 11px;
    }
</style>

<script type="text/javascript">
    $(document).ready(function() {
        jQuery("#help").mouseover(function() {
            jQuery("#div7").slideToggle("fast");
        });
        jQuery("#help").mouseleave(function() {
            jQuery("#div7").slideUp("fast");
        });
    });
</script>

<div class="owmainactionBox">
    <div class="tickettop_left" style="width: 40px;">
        Description</div>
</div>
<div style="padding-left: 24px; margin-bottom: 1px; overflow: auto;">
    <ul class="workflowStatus">
        <li id="tdWaitPmReview" runat="server" class="grayBoxClient">1 PMReview </li>
        <li id="tdEstimation" runat="server" class="grayBoxClient">2 Estimation </li>
        <li id="tdDeveloping" runat="server" class="grayBoxClient">3 Developing </li>
        <li id="tdTesting" runat="server" class="grayBoxClient">4 Testing </li>
        <li id="tdReadyfor_review" runat="server" class="grayBoxClient">5 Review </li>
        <li id="tdCompeleted" runat="server" class="grayBoxClient">6 Completed </li>
    </ul>
</div>
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
                <a href="#" id="help">
                    <img src="/icons/icn-help.png" alt="help" /></a> </span><span class="faqsText" id="div7">
                        <strong class="faqsspantext1">Project Name , Ticket Code , Ticket Priority , Ticket
                            Status </strong></span>
        </td>
    </tr>
    <tr>
        <td>
            Created by:
            <asp:Literal ID="lilCreateBy" runat="server"></asp:Literal>
            <span class="infogray">
                <asp:Literal ID="lilCreateTime" runat="server"></asp:Literal>
            </span>
        </td>
    </tr>
    <tr>
        <td>
            Estimation Needed:
            <asp:Literal ID="lilEN" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr runat="server" id="trEstLast">
        <td>
            Final Time: <span style="color: Red;">
                <asp:Literal ID="lilFinalHours" runat="server"></asp:Literal>
                Hours </span>
        </td>
    </tr>
</table>
