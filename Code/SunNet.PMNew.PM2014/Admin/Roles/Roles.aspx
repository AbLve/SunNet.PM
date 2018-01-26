    <%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Roles.Roles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
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
                                    <img src="/Images/icons/wnewrole.png">
                                    </div>
                                <a class="listtopBtn_text" style="text-decoration: none" href="AddRole.aspx" data-target="#modalsmall" data-toggle="modal">New Role</a>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
            
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="200px">Role Name<span class="arrow"></span></th>
                <th width="*">Description<span class="arrow"></span></th>
                <th width="100px">Created On<span class="arrow"></span></th>
                <th width="100px">Status <span class="arrow"></span></th>
                <th width="100px" class="aligncenter">Action<span class="arrow"></span></th>
            </tr>   
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="5" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>   
        <asp:Repeater ID="rptRoles" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded --> 
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>
                        <%#Eval("RoleName")%>
                    </td>
                    <td>
                        <%#Eval("Description")%>
                    </td>   
                    <td>
                          <%#Eval("CreatedOn","{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%#RoleStatus[int.Parse(Eval("Status").ToString())]%>
                    </td>
                    <td class="aligncenter action">
                        <a class="listtopBtn_text" style="text-decoration: none; display: none;" href="EditRole.aspx?id=<%# Eval("ID") %>" data-target="#modalsmall" data-toggle="modal">edit Role</a>

                        <a href="RolePages.aspx?id=<%#Eval("ID")%>&returnurl=<%=this.ReturnUrl %>" title="Role authority manager">Bind Pages</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
