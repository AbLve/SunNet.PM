<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="AssignClientUsers.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Projects.AssignClientUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
Assign Client Users
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox" style="height: 300px;">
        <div class="owlistContainer">
            <table width="98%" border="0" id="tbAssignUser" align="center" cellpadding="0" cellspacing="0"
                class="owlistone">
                <tr class="owlistTitle">
                    <th width="40px;">
                        &nbsp;
                    </th>
                    <th width="25%">
                        First Name
                    </th>
                    <th width="25%">
                        Last Name
                    </th>
                    <th>
                        Email
                    </th>
                    <th width="25%">
                        Phone
                    </th>
                </tr>
                <tr runat="server" id="trNoTickets" visible="false">
                    <th colspan="3" style="color: Red;">
                        &nbsp; No records
                    </th>
                </tr>
                <asp:Repeater ID="rptAssignUser" runat="server">
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo" %>'>
                            <td>
                            <%# ShowCheckbox(Eval("UserID")) %>
                            </td>
                            <td>
                                <%#Eval("FirstName") %>
                            </td>
                            <td>
                                <%#Eval("LastName") %>
                            </td>
                            <td>
                                <a href="mailto:<%#Eval("UserName") %>">
                                    <%#Eval("UserName") %></a>
                            </td>
                            <td>
                                <%#Eval("Phone") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="btnBoxone">
        <asp:Button ID="btnSave" runat="server" CssClass="btnone"  Text=" Save " 
            onclick="btnSave_Click"  />
    </div>
</asp:Content>
