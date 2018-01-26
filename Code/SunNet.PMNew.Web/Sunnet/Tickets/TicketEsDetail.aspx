<%@ Page Title="Ticket Estimation Detail" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="TicketEsDetail.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.TicketEsDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tabStyle td
        {
            text-align: center;
        }
        .tabStyle th
        {
            text-align: center;
        }
    </style>

    <script type="text/javascript">
        var IsConfrim = false;
        var GlableStatus = -1;
        var ISModalPage = false;
        function updateStatus() {
            var tid = getUrlParam('tid');
            $.ajax({
                type: "post",
                data: {
                    tid: tid,
                    statusValue: GlableStatus
                },
                url: "/Do/DoUpdateTicketStatus.ashx?r=" + Math.random(),
                success: function(result) {
                    MessageBox.Alert3(null, result, function() {
                        CloseAndRefreshParent();
                        window.close();
                    });
                }
            });
        }

        function ConfirmChangeStatus(e) {
            if (e == true) {
                updateStatus();
            }
            else {
                return false;
            }
        }

        function updateStatusConfirm(statusResult, isSureCofirm) {
            GlableStatus = statusResult;
            if (isSureCofirm) {
                if (IsConfrim) {
                    IsConfrim = false;
                    return false;
                }
                MessageBox.Confirm3(null, "Confirm to change Status?", '', '', ConfirmChangeStatus);
            }
            else {
                updateStatus();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Estimation Detail
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdIsFinal" runat="server" />
    <div class="owmainBox">
        <div class="owlistContainer">
            <span style="font-size: 13; font-weight: bold; color: #083583;">Initial Estimation</span>
            <table id="esTable" width="90%" border="1" align="center" cellpadding="5" cellspacing="0"
                class="tabStyle">
                <tr class="owlistTitle">
                    <th style="width: 30px;">
                        Week
                    </th>
                    <th style="width: 40px;">
                        QA
                    </th>
                    <th style="width: 40px;">
                        DEV
                    </th>
                    <th style="width: 45px;">
                        Grap Time
                    </th>
                    <th style="width: 45px;">
                        Doc Time
                    </th>
                    <th style="width: 80px;">
                        Training Time
                    </th>
                    <th style="width: 30px;">
                        Total
                    </th>
                    <th style="width: 60px;">
                        Create By
                    </th>
                    <th style="width: 100px;">
                        Remark
                    </th>
                </tr>
                <tr runat="server" id="trNoTickets" visible="false">
                    <th colspan="9">
                        <span style="color: Red;">&nbsp; No records</span>
                    </th>
                </tr>
                <asp:Repeater ID="rptTicketsEsList" runat="server">
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo" %>'>
                            <td>
                                <%# Eval("Week").ToString()%>
                            </td>
                            <td>
                                <%# Eval("QaAdjust")%>
                            </td>
                            <td>
                                <%# Eval("DevAdjust").ToString()%>
                            </td>
                            <td>
                                <%# Eval("GrapTime").ToString()%>
                            </td>
                            <td>
                                <%# Eval("DocTime")%>
                            </td>
                            <td>
                                <%# Eval("TrainingTime")%>
                            </td>
                            <td>
                                <%# Eval("TotalTimes")%>
                            </td>
                            <td>
                                <%#GetUserNameByCreateID(Eval("EsByUserId").ToString())%>
                            </td>
                            <td style="text-align: left;">
                                <%# Eval("Remark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div style="height: 20px;">
            </div>
            <span style="font-size: 13; font-weight: bold; color: #083583;">Final Estimation</span>
            <table visible="false" width="90%" border="1" align="center" cellpadding="5" cellspacing="0"
                class="tabStyle">
                <tr class="owlistTitle">
                    <th style="width: 30px;">
                        Week
                    </th>
                    <th style="width: 40px;">
                        QA
                    </th>
                    <th style="width: 40px;">
                        DEV
                    </th>
                    <th style="width: 45px;">
                        Grap Time
                    </th>
                    <th style="width: 45px;">
                        Doc Time
                    </th>
                    <th style="width: 80px;">
                        Training Time
                    </th>
                    <th style="width: 30px;">
                        Total
                    </th>
                    <th style="width: 60px;">
                        Create By
                    </th>
                    <th style="width: 100px;">
                        Remark
                    </th>
                </tr>
                <tr runat="server" id="trNoTickets02" visible="false">
                    <th colspan="9">
                        <span style="color: Red;">&nbsp; No records</span>
                    </th>
                </tr>
                <asp:Repeater ID="rptTicketsEsListFinal" runat="server">
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo" %>'>
                            <td>
                                <%# Eval("Week").ToString()%>
                            </td>
                            <td>
                                <%# Eval("QaAdjust")%>
                            </td>
                            <td>
                                <%# Eval("DevAdjust").ToString()%>
                            </td>
                            <td>
                                <%# Eval("GrapTime").ToString()%>
                            </td>
                            <td>
                                <%# Eval("DocTime")%>
                            </td>
                            <td>
                                <%# Eval("TrainingTime")%>
                            </td>
                            <td>
                                <%# Eval("TotalTimes")%>
                            </td>
                            <td>
                                <%#GetUserNameByCreateID(Eval("EsByUserId").ToString())%>
                            </td>
                            <td style="text-align: left;">
                                <%# Eval("Remark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div class="btnBoxone">
            <asp:Literal ID="lilSalesStatusEsFail" runat="server"></asp:Literal>
            <asp:Literal ID="lilSalesStatusWaitForDev" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>
