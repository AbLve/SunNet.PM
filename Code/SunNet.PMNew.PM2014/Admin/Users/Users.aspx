<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/admin.master"
    CodeBehind="Users.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Users.Users" %>

<%@ Import Namespace="SunNet.PMNew.Entity.UserModel" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function () {
            $("body").on("click", "[data-action='sendemail']", function () {
                var $this = $(this);
                var options = $.extend({}, { remote: "", key: "id" }, $this.data());
                if (options.remote && options[options.key] > 0) {
                    jQuery.post(options.remote, options, function (response) {
                        if (response.success) {
                            ShowMessage(response.msg, "success", 2);
                        }
                    }, "json");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="right">Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" queryparam="status" runat="server">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="Active" Value="ACTIVE"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="INACTIVE"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="60" align="right">Company:
                </td>
                <td>
                    <asp:DropDownList ID="ddlCompany" queryparam="company" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="60" align="right">Keyword:
                </td>
                <td>
                    <asp:TextBox placeholder="Enter FirstName, LastName or UserName" ID="txtKeyword" Width="250px" queryparam="keyword" runat="server" CssClass="inputw1"></asp:TextBox>
                </td>
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" />
                </td>

            </tr>
        </table>
    </div>
    <% if (!this.IsReadOnlyRole)
       { %>
    <div class="topbtnbox">

        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td colspan="7">
                    <ul class="listtopBtn">
                        <li>
                            <div class="listtopBtn_icon">
                                <img src="/images/icons/newuser.png" />
                            </div>
                            <a class="listtopBtn_text" style="text-decoration: none" href="/Admin/users/NewSunneter.aspx?returnurl=<%= this.ReturnUrl %>">New Sunneter</a>
                        </li>
                        <li>
                            <div class="listtopBtn_icon">
                                <img src="/images/icons/newclientuser.png" />
                            </div>
                            <a class="listtopBtn_text" style="text-decoration: none" href="/Admin/users/NewClientUser.aspx?returnurl=<%= this.ReturnUrl %>">New Client User</a>
                        </li>
                    </ul>

                </td>
            </tr>
        </table>

    </div>
    <% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div style="width:100%;overflow-x:auto;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="12%" class="order" orderby="CompanyName">Company Name<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="FirstName">First Name<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="LastName">Last Name<span class="arrow"></span></th>
                <th width="*" class="order" orderby="UserName" default>User Name<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Phone" default>Phone<span class="arrow"></span></th>
                <th width="8%">Daily Notices</th>
                <th width="7%" class="order" orderby="Status">Status<span class="arrow"></span></th>
                <th width="7%" class="order" orderby="RoleID">Role<span class="arrow"></span></th>
                <th width="7%" class="order" orderby="Office">Office<span class="arrow"></span></th>   
                <th width="6%" class="aligncenter">Action</th>
            </tr>
        </thead>    
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptUsers" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                    <td>
                        <%#Eval("CompanyName")%>
                    </td>
                    <td>
                        <%#Eval("FirstName")%>
                    </td>
                    <td>
                        <%#Eval("LastName")%>
                    </td>
                    <td>
                        <%#Eval("UserName")%>
                    </td>
                     <td>
                        <%#Eval("Phone").ToString() %>
                    </td>
                    <td>
                        <%# (bool)Eval("IsNotice")==true? "Yes":"No"%>
                    </td>   
                    <td>
                        <%#Eval("Status")%>
                    </td>
                    <td>
                        <%#Eval("Role").ToString() %>
                    </td>
                    <td>
                        <%#GetOffice(Eval("Office").ToString())%>
                    </td>
                    <td class="action aligncenter">
                        <a href='/Admin/users/<%#Eval("Role").ToString().ToEnum<RolesEnum>() == RolesEnum.CLIENT?"ClientUserDetail":"SunnetUserDetail" %>.aspx?id=<%# Eval("ID")%>&returnurl=<%# this.ReturnUrl %>'>
                            <img src="/Images/icons/edit.png" title="View"></a>
                        <a href="javascript:;"
                            data-remote="/Service/Email.ashx"
                            data-action="sendemail"
                            data-key="user"
                            data-user="<%#Eval("ID") %>">
                            <img alt="Email" width="20" height="20" src="/Images/icons/email.png" title="Email"></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="UsersPage" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
