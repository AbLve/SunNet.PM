<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPrincipal.aspx.cs" MasterPageFile="~/Sunnet/popWindow.Master"
    Inherits="SunNet.PMNew.Web.Sunnet.Projects.AddPrincipal" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .listrowone td, .listrowone th {
            padding: 3px 6px 3px 8px;
            background-color: #e0ecf9;
            color: #083583;
        }

        .listrowtwo td, .listrowtwo th {
            padding: 3px 6px 3px 8px;
            background-color: #fff;
            color: #083583;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" accept-charset="gb2312" action="DoAddPrincipal.ashx">
    <div class="owmainBox" style="height: 350px;">
        <div class="owlistContainer" style>
            <table width="98%" border="0" id="tbAssignUser" align="center" cellpadding="0" cellspacing="0" style="margin-bottom:15px">
                <tr style="height:10px"></tr>
                <tr>
                    <th width="25%" style="vertical-align:top">
                        Module/Function:
                    </th>
                    <th>
                        <textarea id="txtModule" style="max-width: 210px; width: 210px; min-width: 210px;height:60px" runat="server"
                            name="txtModule" rows="5" class="input98p"></textarea>
                        
                        <textarea id="txtModuleHid" runat="server" style="display:none"
                            name="txtModuleHid" rows="5"></textarea>
                    </th>
                </tr>
                <tr style="height:10px"></tr>
                <tr>
                    <th width="25%" style="vertical-align:top">
                        PM:
                    </th>
                    <th>
                         <textarea id="txtPM" style="max-width: 210px; width: 210px; min-width: 210px;height:60px"  runat="server"
                            name="txtPM" rows="5" class="input98p"></textarea>
                        
                        <textarea id="txtPMHid" runat="server" style="display:none"
                            name="txtPMHid" rows="5"></textarea>
                    </th>
                </tr>
                <tr style="height:10px"></tr>
                <tr>
                    <th width="25%" style="vertical-align:top">
                        DEV:
                    </th>
                    <th>
                        <textarea id="txtDEV" style="max-width: 210px; width: 210px; min-width: 210px;height:60px"  runat="server"
                            name="txtDEV" rows="5" class="input98p"></textarea>
                        <textarea id="txtDEVHid" runat="server" style="display:none"
                            name="txtDEVHid" rows="5"></textarea>

                    </th>
                </tr>
                <tr style="height:10px"></tr>
                <tr>
                    <th width="25%" style="vertical-align:top">
                        Tester:
                    </th>
                    <th>
                        <textarea id="txtQA" style="max-width: 210px; width: 210px; min-width: 210px;height:60px"  runat="server"
                            name="txtQA" rows="5" class="input98p"></textarea>
                        <textarea id="txtQAHid" runat="server" style="display:none"
                            name="txtQAHid" rows="5"></textarea>

                    </th>
                </tr>
                <tr style="height:10px"></tr>
            </table>
        </div>
    </div>
    <div class="btnBoxone">
            <input type="button" value=" Save " id="Button1" class="btnone" onclick="Check(this);" />
            <asp:HiddenField ID="hdprojectId" runat="server" />
    </div>
        </form>
    
    <script type="text/javascript">
        String.prototype.replaceAll = function (reallyDo, replaceWith, ignoreCase) {
                 if (!RegExp.prototype.isPrototypeOf(reallyDo)) {  
                     return this.replace(new RegExp(reallyDo, (ignoreCase ? "gi": "g")), replaceWith);  
                 } else {  
                     return this.replace(reallyDo, replaceWith);  
                 }  
             }  
        function Check(elem) {
            if ($("#<% =txtModule.ClientID %>").val() == "") {
                alert("Please enter Module/Function.");
                return false;
            }

            $(elem).closest("form").ajaxSubmit({
                success: function (responseText, statusText, xhr, $form) {
                    if (responseText == 0) {
                        alert("Please enter.");
                    }
                    else {
                        ShowMessage("Add Success!", function () {
                            $.Zebra_Dialog.closeCurrent(elem);
                        });  
                    }
                }
                , error: function (context, xhr, e, status) {
                    try {
                        alert('error');
                    }
                    catch (e) {

                    }
                    finally {

                    }
                }

            });
            return false;
        }


    </script>
</asp:Content>
