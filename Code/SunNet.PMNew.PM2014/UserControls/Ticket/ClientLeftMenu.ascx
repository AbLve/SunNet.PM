<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientLeftMenu.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.Ticket.ClientLeftMenu" %>
<div class="leftmenuBox">
    <ul class="newticketBtn">
        <a href="/Ticket/New.aspx?returnurl=<%=this.GlobalPage.ReturnUrl %>">
            <li>New Ticket</li>
        </a>
    </ul>
    <ul class="leftMenu client-left">
        
        <li class="waitting <%=GetCurrentPageActiveClass("/Ticket/Waiting.aspx") %>">
            <a href="/Ticket/Waiting.aspx"><span class="icon">Waiting for Response </span></a>
            <a href="/Ticket/Waiting.aspx" class="full"></a>
            <span class="number"><%=waitingResponse %></span>
        </li>
        <li class="myticket  <%=GetCurrentPageActiveClass("/Ticket/MyTicket.aspx") %>">
            <a href="/Ticket/MyTicket.aspx"><span class="icon">MY ONGOING TICKETS </span></a>
            <a href="/Ticket/MyTicket.aspx" class="full"></a>
            <span class="number"><%=myOngoing %></span>
        </li>
        <li class="ongoing  <%=GetCurrentPageActiveClass("/Ticket/Ongoing.aspx") %>">
            <a href="/Ticket/Ongoing.aspx"><span class="icon">ALL ONGOING TICKETS </span></a>
            <a href="/Ticket/Ongoing.aspx" class="full"></a>
            <span class="number"><%=allOngoing %></span>
        </li>
        <li class="cancelled <%=GetCurrentPageActiveClass("/Ticket/Cancelled.aspx") %>">
            <a href="/Ticket/Cancelled.aspx"><span class="icon">Cancelled Tickets</span></a><div class="rightarrow"></div>
            <a href="/Ticket/Cancelled.aspx" class="full"></a></li>
        <li class="completed  <%=GetCurrentPageActiveClass("/Ticket/Completed.aspx") %>">
            <a href="/Ticket/Completed.aspx"><span class="icon">Completed Tickets</span></a><div class="rightarrow"></div>
            <a href="/Ticket/Completed.aspx" class="full"></a></li>
        <li class="draft <%=GetCurrentPageActiveClass("/Ticket/Draft.aspx") %>">
            <a href="/Ticket/Draft.aspx"><span class="icon">Draft Tickets</span></a><div class="rightarrow"></div>
            <a href="/Ticket/Draft.aspx" class="full"></a></li>
        <li class="reports  <%=GetCurrentPageActiveClass("/Ticket/Report.aspx") %>">
            <a href="/Ticket/Report.aspx"><span class="icon">Tickets Report</span></a><div class="rightarrow"></div>
            <a href="/Ticket/Report.aspx" class="full"></a>
        </li>

    </ul>
</div>
