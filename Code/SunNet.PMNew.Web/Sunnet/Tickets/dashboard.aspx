<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="dashboard.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenTicketDetail(selectTicketId) {
            window.open("/Sunnet/Tickets/TicketDetail.aspx?tid=" + selectTicketId + "#Feedback");
        }
        function OpenReplyFeedBackDialog(fid, tid) {
            var result = ShowIFrame("/Sunnet/Tickets/AddFeedBacks.aspx?feedbackId=" + fid + "&rtype=r" + "&tid=" + tid, 580, 430, true, "Reply FeedBack");
            if (result == 0) {
                window.location.reload();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Dashboard
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainrightBox">
        <div class="onlistBox">
            <img src="/icons/03.gif" align="absmiddle" />
            <span class="onlistText">Due Date is today</span>
            <img src="/icons/02.gif" align="absmiddle" />
            <span class="onlistText">Due day is 3 days before today</span>
            <img src="/icons/01.gif" align="absmiddle" />
            <span class="onlistText">Passed Due Date</span> <strong>Priority:</strong> The Smaller
            the value, the higher the priority.</div>
        <asp:Repeater runat="server" ID="rptListProjects" OnItemDataBound="rptListProjects_ItemDataBound">
            <ItemTemplate>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listone">
                    <tr>
                        <td colspan="10" class="listtopTitle">
                            <div class="listtopTitle_left">
                                <%# Eval("Title")%>
                            </div>
                            <div class="listtopTitle_right">
                                <%# ShowLinkMore(Eval("ProjectID").ToString())%>
                            </div>
                        </td>
                    </tr>
                    <tr class="listsubTitle">
                        <th style="width: 60px;">
                            Priority
                        </th>
                        <th style="width: 20px;">
                            &nbsp;
                        </th>
                        <td style="width: 70px;">
                            Ticket Code
                        </td>
                        <td>
                            Title
                        </td>
                        <td style="width: 160px;">
                            Status
                        </td>
                        <th style="width: 65px;" orderby="CreatedOn">
                            Created
                        </th>
                        <th style="width: 65px;" orderby="ModifiedOn">
                            Updated
                        </th>
                        <th style="width: 100px;">
                            Created By
                        </th>
                        <th style="width: 70px">
                            Due Date
                        </th>
                    </tr>
                    <tr runat="server" id="trNoTickets" visible="false">
                        <th colspan="10">
                            &nbsp;
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptListTickets">
                        <ItemTemplate>
                            <tr opentype="newtab" dialogtitle="" href="/Sunnet/Tickets/TicketDetail.aspx?tid=<%#Eval("TicketID") %>"
                                class='<%# TrStyle(Container.ItemIndex, (int)Eval("ConvertDelete")) %>'>
                                <td>
                                    <b>
                                        <%# ShowOrderNumberByTid(Convert.ToInt32(Eval("TicketID")), Eval("Priority").ToString())%></b>
                                </td>
                                <td>
                                    <%# ShowPriorityImgByDevDate(Eval("DeliveryDate").ToString())%>
                                </td>
                                <td>
                                    <%# Eval("TicketCode").ToString()%>
                                </td>
                                <td>
                                    <%# Eval("Title").ToString()%>
                                </td>
                                <td <%# ShowAction(Eval("TicketID")) %>>
                                    <%# ChangeStatus(Eval("Status"), (int)Eval("TicketID"), (decimal)Eval("FinalTime"), (bool)Eval("IsEstimates"))%>
                                </td>
                                <td>
                                    <%# Eval("CreatedOn", "{0:MM/dd/yyyy}")%>
                                </td>
                                <td>
                                    <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                                </td>
                                <td>
                                    <%#GetClientUserName(Convert.ToInt32(Eval("CreatedBy"))) %>
                                </td>
                                <td>
                                    <%#Convert.ToDateTime(Eval("DeliveryDate")) <= Convert.ToDateTime("1800-01-01") ? "" : Eval("DeliveryDate", "{0:MM/dd/yyyy}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
