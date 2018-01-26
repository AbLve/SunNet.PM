<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkFlow.ascx.cs" Inherits="SunNet.PMNew.Web.UserControls.WorkFlow" %>
<div class="grayBox" id="divPMReviewed" runat="server">
    PM Reviewed
    <table border="0" align="center" cellpadding="0" cellspacing="0">
        <tr runat="server" id="trIsPmRev">
            <td>
                <ul id="PMReviewed" class="flowsubBox">
                    <li>
                        <img src="/icons/29.gif">
                        <strong>Done</strong> </li>
                </ul>
            </td>
        </tr>
    </table>
</div>
<div class="flowarrowBox">
    <asp:Literal ID="lilEstimation" runat="server"></asp:Literal>
    <div class="flowBox">
        <div class="grayBox" runat="server" id="divEstimation">
            Estimation<table border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <ul class="flowsubBox">
                            <li>Estimation Time: <strong>
                                <asp:Literal ID="lilEsHours" runat="server"></asp:Literal>
                            </strong>hours </li>
                            <li>Estimation State:<strong>
                                <asp:Literal ID="lilSaleState" runat="server"></asp:Literal>
                            </strong></li>
                        </ul>
                    </td>
                </tr>
            </table>
        </div>
        <div class="flowarrowBox">
            <asp:Literal ID="lilDeveloping" runat="server"></asp:Literal>
        </div>
        <div id="divDeveloping" class="grayBox" runat="server">
            Developing
            <table border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <ul class="flowsubBox">
                            <li>Task Count: <strong>
                                <asp:Literal ID="lilTaskCount" runat="server"></asp:Literal></strong> </li>
                            <li>Completed Count: <strong>
                                <asp:Literal ID="lilCompCount" runat="server"></asp:Literal></strong> </li>
                            <li>Completion Rate: <strong>
                                <asp:Literal ID="lilTask" runat="server"></asp:Literal></strong> </li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lilShowMsgInfo" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
        <div class="flowarrowBox">
            <asp:Literal ID="lilTsonLocalServer" runat="server"></asp:Literal>
        </div>
        <div class="grayBox" runat="server" id="divTsOnLocalServer">
            Test On Local Server
            <table border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <ul class="flowsubBox">
                            <li><strong>DEV:<asp:Literal ID="lilDevResultOnLocal" runat="server"></asp:Literal>
                                , QA:
                                <asp:Literal ID="lilQaResultOnLocal" runat="server"></asp:Literal></strong></li>
                        </ul>
                    </td>
                </tr>
            </table>
        </div>
        <div class="flowarrowBox">
            <asp:Literal ID="lilTsonClientServer" runat="server"></asp:Literal>
        </div>
        <div class="grayBox" runat="server" id="divTsonClientServer">
            Test On Client Server
            <table border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <ul class="flowsubBox">
                            <li><strong>DEV:<asp:Literal ID="lilDevResultOnCS" runat="server"></asp:Literal>
                                , QA:
                                <asp:Literal ID="lilQaResultOnCS" runat="server"></asp:Literal></strong></li>
                        </ul>
                    </td>
                </tr>
            </table>
        </div>
        <div class="flowarrowBox">
            <asp:Literal ID="lilPmAudit" runat="server"></asp:Literal>
        </div>
        <div class="grayBox" runat="server" id="divPMAudit">
            Project Manager Audit
            <br />
            <asp:Literal ID="lilPmAuditStatus" runat="server"></asp:Literal>
        </div>
        <div class="flowarrowBox">
            <asp:Literal ID="lilPublishPS" runat="server"></asp:Literal>
        </div>
        <div class="grayBox" runat="server" id="divPublshPS">
            Publish to Product Server
        </div>
        <div class="flowarrowBox">
            <asp:Literal ID="lilWaitClientVerified" runat="server"></asp:Literal>
        </div>
        <div class="grayBox" runat="server" id="divWaitClientVerified">
            Client Verified Change<br />
            <asp:Literal ID="lilCompWord" runat="server"></asp:Literal>
        </div>
        <div class="flowarrowBox">
            <asp:Literal ID="lilComplete" runat="server"></asp:Literal>
        </div>
        <div class="flowarrowBox">
            <div class="grayBox" runat="server" id="divComp">
                Compeleted
            </div>
        </div>
    </div>
</div>
