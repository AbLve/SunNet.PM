<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientMenu.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.ClientMenu" %>
<div class="topBox clearfix">
    <div class="topBox_logo">
        <a href="/Dashboard.aspx" target="_top"><img style="width:100%;"  src="/Images/sunnet_logo.png" alt="Logo" /></a>
    </div>
    <div class="topBox_menu">
        <ul class="topmenu">
            <li class="clientdashboard  <%=GetCurrentItemActiveClass("/Dashboard.aspx") %>">
                <a href="/Dashboard.aspx" target="<%=Target %>">
                    <div class="image"></div>
                    DashBoard
                    <div class="downarrow"></div>
                </a>
                <a class="full" href="/Dashboard.aspx" target="<%=Target %>"></a>
            </li>
            <li class="client  <%=GetCurrentItemActiveClass("/Ticket/") %>">
                <a href="/Ticket/Waiting.aspx" target="<%=Target %>">
                    <div class="image"></div>
                    Tickets
                    <div class="downarrow"></div>
                </a>
                <a class="full" href="/Ticket/Waiting.aspx" target="<%=Target %>"></a>
            </li>
            <li class="document <%=GetMenuClass("document") %>">
                <a href="/document/DocManagement/DocHome/Index" target="<%=Target %>">
                    <div class="image"></div>
                    Documents
                    <div class="downarrow"></div>
                </a>
                <a class="full" href="/document/DocManagement/DocHome/Index" target="<%=Target %>"></a>
            </li>
            <li class="event <%=GetCurrentItemActiveClass("/Event/") %>">
                <a href="/Event/Index.aspx" target="<%=Target %>">
                    <div class="image"></div>
                    Events
                    <div class="downarrow"></div>
                </a>
                <a class="full" href="/Event/Index.aspx" target="<%=Target %>"></a></li>
            <li class="forum <%=GetMenuClass("forum") %>">
                <a href="/forums/login/pm/finish" target="<%=Target %>">
                    <div class="image"></div>
                    Forums
                    <div class="downarrow"></div>
                </a>
                <a class="full" href="/forums/login/pm/finish" target="<%=Target %>"></a>
            </li>
            <li class="profile <%=GetCurrentItemActiveClass("/Account/") %>">
                <a href="/Account/Profile.aspx" target="<%=Target %>">
                    <div class="image"></div>
                    Profile
                    <div class="downarrow"></div>
                </a>
                <a class="full" href="/Account/Profile.aspx" target="<%=Target %>"></a>
            </li>

        </ul>
    </div>
    <%  if (!this.GlobalPage.SourceIsWinform && UserInfo != null)
      {%>
    <div class="toploginInfo-inside">
        <span class="mainTop_rightUser" style="padding-left:0px;padding-right:0px;">Welcome<strong>
            <%= this.BasePage.GetClientUserName(UserInfo) %></strong></span><br/>
        <span class="mainTop_rightUser"><a href="/Logout.aspx" target="_top">Log Out</a></span>
    </div>
    <%} %>
</div>
<style>
    @media(max-width: 992px) {
        ul.leftMenu li{
            padding:12px 3px;
        }
    }
</style>
