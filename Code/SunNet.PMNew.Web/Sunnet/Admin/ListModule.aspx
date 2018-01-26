<%@ Page Title="Modules" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="ListModule.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.ListModule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        jQuery(function() {
            jQuery("#new_module").click(function() {
                AddChildModule(GetSelectedModule());
            });
        });
        function AddChildModule(selected,parentID) {
          var result=ShowIFrame("/Sunnet/admin/addmodule.aspx?selected=" + selected+"&parent="+parentID, 480, 430, true, "Add New Module");
          if(result==0)
          {
            window.location.reload();
          }
        }
        function GetSelectedModule() {
            return jQuery(<%="'#"+ddlParentModule.ClientID +"'"%>).val();
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphTitle" runat="server">
    Module Manager
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td colspan="6">
                <asp:Literal ID="ltlMenu" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="width: 50px;">
                Parent:
            </td>
            <td style="width: 250px;">
                <asp:DropDownList ID="ddlParentModule" CssClass="select205" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:ImageButton ID="ibtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="ibtnSearch_Click" />
            </td>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <span id="new_module" action="new"><a href="#">
            <img src="/icons/09.gif" border="0" align="absmiddle" />New</a></span>
    </div>
    <div class="mainrightBoxtwo">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="subList">
            <tr class="listsubTitle">
                <th width="10%">
                    Module Title
                </th>
                <th width="20%">
                    Module Path
                </th>
                <th width="15%">
                    Default Page
                </th>
                <th width="15%">
                    Click Function
                </th>
                <th width="10%">
                    Priority
                </th>
                <th width="10%">
                    Show In Menu
                </th>
                <th width="10%">
                    Status
                </th>
                <th width="10%">
                    Action
                </th>
            </tr>
            <asp:Repeater ID="rptModules" runat="server">
                <ItemTemplate>
                    <tr href="/Sunnet/Admin/EditModule.aspx?selected=<%#Eval("ID") %>&parent=<%#Eval("ParentID") %>"
                        opentype="popwindow" dialogwidth="490" dialogheight="420" dialogtitle="" class="<%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
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
                        <td>
                            <%#Eval("Orders")%>
                        </td>
                        <td>
                            <%#((bool)Eval("ShowInMenu"))?"Yes":"No"%>
                        </td>
                        <td>
                            <%#Eval("Status").ToString()=="0"?"Active":"Inactive"%>
                        </td>
                        <td class="action">
                       
                            <a href="ListModule.aspx?selected=<%#Eval("ModuleID") %>&parent=<%#Eval("ParentID") %>" class=" <%#Convert.ToBoolean(Eval("PageOrModule")) ? "" : "hide"%>" 
                                title="Query modules under this">
                                <img src="/icons/27.gif" alt="Query modules under this" /></a> <a href="#"  class=" <%#Convert.ToBoolean(Eval("PageOrModule")) ? "" : "hide"%>"  onclick="AddChildModule(<%#Eval("ModuleID") %>,<%#Eval("ParentID") %>)"
                                    title="Add module under this">
                                    <img src="/icons/28.gif" alt="dd module under this" /></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td style="width: 50px;">
                Page:
            </td>
            <td style="width: 300px;">
                <webdiyer:AspNetPager ID="anpModules" PageSize="40" runat="server" AlwaysShow="true">
                </webdiyer:AspNetPager>
            </td>
            <td>
                Total&nbsp;:&nbsp;<asp:Literal ID="ltlTotal" runat="server"></asp:Literal>
            </td>
            <td>
            </td>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
