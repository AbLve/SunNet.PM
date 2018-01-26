<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMenu.ascx.cs" Inherits="SunNet.PMNew.Web.UserControls.TopMenu" %>
<asp:Repeater ID="rptTop" runat="server">
    <ItemTemplate>
        <li class="sepline">
            <img src="/images/topsep.jpg" /></li>
        <li class="<%#Convert.ToInt32(Eval("ID").ToString())==CurrentIndex?"currenttop":"none" %>">
            <a href="<%#Eval("ModulePath")%><%#Eval("DefaultPage")%>">
                <%#Eval("ModuleTitle")%></a></li>
    </ItemTemplate>
</asp:Repeater>
