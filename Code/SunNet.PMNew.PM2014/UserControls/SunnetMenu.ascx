<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SunnetMenu.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.SunnetMenu" %>
<%if (!this.GlobalPage.SourceIsWinform)
  {%>
<div class="toploginInfo">
    <% if (BasePage != null)
       { %>
    <span class="mainTop_rightUser" style="padding-left:0px;padding-right:0px;">Welcome<strong>

        <%=this.BasePage.GetClientUserName(UserInfo)%></strong></span>&nbsp;|&nbsp;<a href="/Logout.aspx" target="_top">Log Out</a>
    <% } %>
</div>
<%} %>

<div class="topBox clearfix">
    <div class="topBox_logo">
        <a href="/SunnetTicket/Dashboard.aspx" target="_top"><img src="/Images/sunnet_logo.png" alt="Logo" /></a> 
    </div>
    <div class="topBox_menu">
        <ul class="topmenu">
            <asp:Repeater ID="rptTop" runat="server">
                <ItemTemplate>
                    <li class="<%# Eval("ClickFunctioin")%> <%#GetMenuClass(GetSelectedMenu(Eval("ModuleTitle").ToString())) %>">
                        <a href="<%# Eval("ModulePath").ToString()+Eval("DefaultPage").ToString() %>" target="<%=Target %>">
                            <div class="image"></div>
                            <%#Eval("ModuleTitle") %>
                            <div class="downarrow"></div>
                        </a>
                        <%#  Eval("ModuleTitle").ToString().ToUpper().Contains("OA") ? "<span class='number' style='margin-top: -33px;'> " + waitingForCount + " </span>" : ""%>
                        <a class="full" href="<%# Eval("ModulePath").ToString()+Eval("DefaultPage").ToString() %>" target="<%=Target %>"></a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</div>