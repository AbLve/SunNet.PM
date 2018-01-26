<%@ Page Title="User Manager" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="ListUser.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.ListUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        jQuery(function() {
            jQuery("#AddNewObject").add("#addnewClient").click(function() {
                var _this = jQuery(this);
                var result = ShowIFrame(_this.attr("dialoghref"),
                            _this.attr("dialogWidth"),
                            _this.attr("dialogHeight"),
                            true,
                            _this.attr("title"));
                if (result == 0) {
                    RefreshCurrentWindow();
                }
                return false;
            });
        });
        
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphTitle" runat="server">
    User Manager
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td style="width: 50px;">
                Status:
            </td>
            <td style="width: 150px;">
                <asp:DropDownList ID="ddlStatus" CssClass="select100" runat="server">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Active" Value="ACTIVE"></asp:ListItem>
                    <asp:ListItem Text="Inactive" Value="INACTIVE"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 50px;">
                Company:
            </td>
            <td style="width: 200px;">
                <asp:DropDownList ID="ddlCompany" CssClass="select150" runat="server">
                </asp:DropDownList>
            </td>
            <td style="width: 50px;">
                Keyword:
            </td>
            <td style="width: 200px;">
                <asp:TextBox ID="txtKeyword" CssClass="input200" runat="server"></asp:TextBox>
            </td>
            <td width="*">
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="ibtnSearch_Click" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <span id="spanAddSunneter" runat="server"><a id="AddNewObject" dialogwidth="780"
            dialogheight="460" dialoghref="AddUser.aspx" href="javascript:void(0)" href="#"
            title="Add role">
            <img align="absmiddle" border="0" src="/icons/09.gif" />New Sunneter</a></span>
        <span id="spanAddClient" runat="server"><a id="addnewClient" dialogwidth="780" dialogheight="450"
            dialoghref="AddClient.aspx" href="javascript:void(0)" title="Add role">
            <img align="absmiddle" border="0" src="/icons/09.gif" />New Client User</a></span><span></span>
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="FirstName" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="ASC" />
    </div>
    <div class="mainrightBoxtwo">
        <table border="0" cellpadding="0" cellspacing="0" class="listtwo" width="100%">
            <tr class="listsubTitle">
                <th width="15" orderby="CompanyName">
                    Company Name
                </th>
                <th width="10" orderby="FirstName">
                    First Name
                </th>
                <th width="10" orderby="LastName">
                    Last Name
                </th>
                <th width="20" orderby="UserName">
                    User Name
                </th>
                <th width="15">
                    Phone
                </th>
                <th width="10" orderby="Status">
                    Status
                </th>
                <th width="10" orderby="RoleID">
                    Role
                </th>
                <th width="10" orderby="Office">
                    Office
                </th>
            </tr>
            <asp:Repeater ID="rptUsers" runat="server">
                <ItemTemplate>
                    <tr href="/Sunnet/Admin/EditUser.aspx?id=<%#Eval("ID") %>" opentype="popwindow" dialogwidth="780"
                        dialogheight="<%#Eval("Role").ToString()==SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT.ToString()?"590":"460"%>"
                        dialogtitle="" freshbutton="<%#iBtnSearch.ClientID %>" class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
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
                            <%#Eval("Phone")%>
                        </td>
                        <td>
                            <%#Eval("Status")%>
                        </td>
                        <td>
                            <%#Eval("Role").ToString() %>
                        </td>
                        <td>
                            <%#Eval("Office")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpUsers" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpUsers_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>
</asp:Content>
