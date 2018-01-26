<%@ Page Title="Role authority manager" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="RoleModulePage.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.RoleModulePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/tree/jquery.checkboxtree.min.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/smoothness/jquery-ui-1.9.0.custom.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphTitle" runat="server">
    Role Authority Manager&nbsp;:&nbsp;&nbsp;<asp:Literal ID="ltlRole" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainrightBoxtwo">
        <div class="owToptwo">
            All modules/pages in system:
            <asp:Literal ID="ltlModules2" runat="server" Visible="false"></asp:Literal>
        </div>
        <ul id="treeRMP">
                <asp:Literal ID="ltlModules" runat="server"></asp:Literal>
        </ul>
        <br />
        <asp:HiddenField ID="hidSelected" runat="server" />
    </div>
    <div class="btnBoxone">
        <asp:Button ID="btnSave" CssClass="btnone" ValidationGroup="Add" CausesValidation="true"
            runat="server" Text="Save" OnClick="btnSave_Click" />
        <input id="btnClientCancel" name="button" type="button" class="btnone" value="Clear" />
    </div>

    <script type="text/javascript">
        jQuery(function() {
            var hidRecord=jQuery('<%="#"+hidSelected.ClientID %>');    
            var selectedModule=hidRecord.val().split(",");
            for(var i=0;i<selectedModule.length;i++)
            {
                jQuery("#"+selectedModule[i]).attr("checked","checked");
            }
            jQuery("#treeRMP").checkboxTree();
            
            jQuery(<%="'#"+btnSave.ClientID+"'" %>).click(function(){
                 hidRecord.val("");
                 var tempSelected="";
                 jQuery("input[type='checkbox']").each(function(){
                    var _this=jQuery(this);
                    if(_this.attr("checked")=="checked")
                    {
                        tempSelected=tempSelected+_this.attr("id");
                        tempSelected=tempSelected+",";
                    }
                });
                hidRecord.val(tempSelected);
                //alert(hidRecord.val());
            });
           
        });
        
    </script>

</asp:Content>
