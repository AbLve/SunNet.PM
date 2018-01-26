<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeBehind="Modules.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Modules.Modules" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="breadnaviBox">
        <asp:Literal ID="ltlMenu" runat="server"></asp:Literal>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td>
                        <ul class="listtopBtn">
                            <li>
                                <div class="listtopBtn_icon">
                                    <img src="/Images/icons/wnewmodule.png">
                                </div>
                                <div class="listtopBtn_text"><a href="AddModule.aspx?selected=<% =QS("selected",1) %>&parent=<% =QS("parent",0) %>&returnurl=<%=this.ReturnUrl %>">New Module</a></div>
                               
                            </li>
                        </ul>
                         <asp:Literal ID="ltlModules" runat="server" Visible="false"></asp:Literal>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div style="width:100%;overflow-x:auto;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="160px">Title<span class="arrow"></span></th>
                <th width="*">Path <span class="arrow"></span></th>
                <th width="80px">Default<span class="arrow"></span></th>
                <th width="100px">Menu Class <span class="arrow"></span></th>
                <th width="80px" class="aligncenter">Priority<span class="arrow"></span></th>
                <th width="70px" class="aligncenter">Menu<span class="arrow"></span></th>
                <th width="60px" class="aligncenter">Status<span class="arrow"></span></th>
                <th width="100px" class="aligncenter">Action<span class="arrow"></span></th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptModules" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>
                        <%#Eval("ModuleTitle")%>
                    </td>
                    <td>
                        <%#Eval("ModulePath")%>
                    </td>
                    <td>
                        <%#Eval("DefaultPage")%>
                    </td>
                    <td>
                        <%#Eval("ClickFunctioin")%>
                    </td>
                    <td class="aligncenter">
                        <%#Eval("Orders")%>
                    </td>
                    <td class="aligncenter">
                        <%#((bool)Eval("ShowInMenu"))?"Yes":"No"%>
                    </td>
                    <td class="aligncenter">
                        <%#Eval("Status").ToString()=="0"?"Active":"Inactive"%>
                    </td>
                    <td class="aligncenter action">
                        <a class="saveBtn1 mainbutton" href="EditModule.aspx?selected=<%# Eval("ModuleID")%>&parent=<%# Eval("ParentID") %>&returnurl=<%=this.ReturnUrl %>" style="display: none;"></a>

                        <a href="Modules.aspx?selected=<%#Eval("ModuleID") %>&parent=<%#Eval("ParentID") %>" class=" <%#Convert.ToBoolean(Eval("PageOrModule")) ? "" : "hide"%>"
                            title="sub-modules">view</a>

                        <a href="AddModule.aspx?selected=<%# Eval("ModuleID") %>&parent=<%#Eval("ParentID") %>&returnurl=<%=this.ReturnUrl %>" class=" <%#Convert.ToBoolean(Eval("PageOrModule")) ? "" : "hide"%>"
                            title="add sub-modules">add</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>


