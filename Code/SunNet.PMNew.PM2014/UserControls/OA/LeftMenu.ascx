<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.OA.LeftMenu" %>
<div class="leftmenuBox">
    <% if (BasePage.CheckRoleCanAccessPage("/SunnetTicket/New.aspx"))
       { %>
        <ul class="newticketBtn">
            <a href="/SunnetTicket/New.aspx?returnurl=<%=this.GlobalPage.ReturnUrl %>">
                <li>New Ticket</li>
            </a>
        </ul>
    <% } %>
    <ul class="leftMenu oa-left">
        <% for (int i = 0; i < LeftMenus.Count; i++)
           {%>
            <li class="<%=LeftMenus[i].ClickFunctioin %> <%=GetCurrentPageActiveClass(LeftMenus[i].ModulePath) %>">

            <% if (LeftMenus[i].ModuleTitle == "TO CRM"||LeftMenus[i].ModuleTitle == "Task List"||LeftMenus[i].ModuleTitle == "BIDS" ||LeftMenus[i].ModuleTitle == "STP Work Space")
                {%>
                    <a target="_blank" href="<%=LeftMenus[i].ModulePath %>"><span class="icon"><%=LeftMenus[i].ModuleTitle %></span></a><div class="rightarrow"></div>
                    <a target="_blank" href="<%=LeftMenus[i].ModulePath %>" class="full"></a>
                <%}
               else if(LeftMenus[i].ModuleTitle == "Waiting for Response")
               { %>
                    <a href="<%=LeftMenus[i].ModulePath %>"><span class="icon"><%=LeftMenus[i].ModuleTitle %></span></a>
                    <span class="number"><% =waitingForRespnseCount %></span>
                    <div class="rightarrow"></div> 
                    <a href="<%=LeftMenus[i].ModulePath %>" class="full"></a>
               <% }
               else
                { %>
                    <a href="<%=LeftMenus[i].ModulePath %>"><span class="icon"><%=LeftMenus[i].ModuleTitle %></span></a><div class="rightarrow"></div>
                    <a href="<%=LeftMenus[i].ModulePath %>" class="full"></a>
            <% } %>
            </li>
        <% } %>
    </ul>
</div>

