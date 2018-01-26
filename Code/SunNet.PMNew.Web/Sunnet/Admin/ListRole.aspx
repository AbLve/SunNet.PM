<%@ Page Title="Roles" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="ListRole.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.ListRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenRMDialog(roleID) {
            var url = "/Sunnet/Admin/RoleModulePage.aspx?id=" + roleID;
            var result = ShowIFrame(url,
                            880,
                            600,
                            true,
                            "Role authority manager");
            if (result == 0) {
                window.location.reload();
            }
            return false;
        }
        jQuery(function() {
            jQuery("#AddNewObject").click(function() {
                var _this = jQuery(this);
                var result = ShowIFrame(_this.attr("href"),
                            400,
                            300,
                            true,
                            _this.attr("title"));
                if (result == 0) {
                    window.location.reload();
                }
                return false;
            });

        });
        
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphTitle" runat="server">
    Role Manager
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainactionBox">
        <span action="new"><a id="AddNewObject" href="AddRole.aspx" title="Add role">
            <img src="/icons/09.gif" border="0" align="absmiddle" />New</a></span>
    </div>
    <div class="mainrightBoxtwo">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="subList">
            <tr class="listsubTitle">
                <th width="10%">
                    RoleName
                </th>
                <th width="40%">
                    Descriptioin
                </th>
                <th>
                    Create On
                </th>
                <th width="10%">
                    Status
                </th>
                <th width="10%">
                    Action
                </th>
            </tr>
            <asp:Repeater ID="rptRoles" runat="server">
                <ItemTemplate>
                    <tr href="/Sunnet/Admin/EditRole.aspx?id=<%#Eval("ID") %>" opentype="popwindow" dialogwidth="420"
                        dialogheight="300" dialogtitle="" class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                        <td>
                            <%#Eval("RoleName")%>
                        </td>
                        <td>
                            <%#Eval("Description")%>
                        </td>
                        <td>
                            <%#Eval("CreatedOn")%>
                        </td>
                        <td>
                            <%#RoleStatus[int.Parse(Eval("Status").ToString())]%>
                        </td>
                        <td class="action">
                            <a href="#"  onclick="OpenRMDialog('<%#Eval("ID") %>')"
                                title="Role authority manager">
                                <img src="/icons/27.gif" alt="Role authority manager" /></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
</asp:Content>
