<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.Account.LeftMenu" %>
<div class="leftmenuBox">
    <% if (UserInfo.Role == SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
       { %>
    <ul class="newticketBtn">
        <a href="/Ticket/New.aspx?returnurl=<%=this.GlobalPage.ReturnUrl %>">
            <li>New Ticket</li>
        </a>
    </ul>
    <% } %>
    <% if (BasePage.CheckRoleCanAccessPage("/SunnetTicket/New.aspx"))
       { %>
    <ul class="newticketBtn">
        <a href="/SunnetTicket/New.aspx?returnurl=<%=this.GlobalPage.ReturnUrl %>">
            <li>New Ticket</li>
        </a>
    </ul>
    <% } %>
    <ul class="leftMenu profile-left">
        <% for (int i = 0; i < LeftMenus.Count; i++)
           {%>
        <li class="<%=LeftMenus[i].ClickFunctioin %> <%=GetCurrentPageActiveClass(LeftMenus[i].ModulePath) %>">
            <a href="<%=LeftMenus[i].ModulePath %>"><span class="icon"><%=LeftMenus[i].ModuleTitle %></span></a><div class="rightarrow"></div>
            <a href="<%=LeftMenus[i].ModulePath %>" class="full"></a>
        </li>
        <% } %>
    </ul>
</div>
