<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeBehind="RolePages.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Roles.RolePages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/Styles/smoothness/jquery-ui-1.9.0.custom.min.css" rel="stylesheet" type="text/css" />
    <link href="/Content/Styles/tree/jquery.checkboxtree.min.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-ui-1.9.0.custom.min.js"></script>
    <script type="text/javascript" src="/Scripts/Tree/jquery.checkboxtree.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="owToptwo">
        All modules/pages in system:
            <asp:Literal ID="ltlModules2" runat="server" Visible="false"></asp:Literal>
    </div>
    <ul id="treeRMP">
        <asp:Literal ID="ltlModules" runat="server"></asp:Literal>
    </ul>
    <br />
    <asp:HiddenField ID="hidSelected" runat="server" />
    <script type="text/javascript">
        jQuery(function () {
            var hidRecord = jQuery('#<%=hidSelected.ClientID %>');
            var selectedModule = hidRecord.val().split(",");
            for (var i = 0; i < selectedModule.length; i++) {
                jQuery("#" + selectedModule[i]).attr("checked", "checked");
            }
            jQuery("#treeRMP").checkboxTree();

            jQuery('#<%=btnSave.ClientID %>').click(function () {
               hidRecord.val("");
               var tempSelected = "";
               jQuery("input[type='checkbox']").each(function () {
                   var _this = jQuery(this);
                   if (_this.attr("checked") == "checked") {
                       tempSelected = tempSelected + _this.attr("id");
                       tempSelected = tempSelected + ",";
                   }
               });
               hidRecord.val(tempSelected);
               //alert(hidRecord.val());
           });

        });

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="buttonBox3">
        <asp:Button ID="btnSave" Text=" Save " CssClass="saveBtn1 mainbutton" runat="server" OnClick="btnSave_Click" />
        <input type="button" value=" Back " class="cancelBtn1 mainbutton redirectback" />
    </div>
</asp:Content>
