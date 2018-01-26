<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNote.aspx.cs" MasterPageFile="~/Sunnet/InputPop.Master"
 Inherits="SunNet.PMNew.Web.Sunnet.WorkRequest.AddNote" %>

    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .customWidth
        {
            width: 600px;
            height: 270px;
        }
        .owmainBox
        {
            width: 578px;
            height: 142px;
        }
    </style>
    <script type="text/javascript">

        function baseValidate() {
            if ($("#<%=txtTitle.ClientID%>").val() == 0) {
                ShowMessage("Please enter the Title.", 0, false, false);
                $("#<%=txtTitle.ClientID%>").focus();
                return false;
            }
            else if ($("#<%=txtDescription.ClientID%>").val() == "") {
                ShowMessage("Please enter the Description.", 0, false, false);
                $("#<%=txtDescription.ClientID%>").focus();
                return false;
            }

            else if ($("#<%=txtTitle.ClientID%>").val().length>100) {
                ShowMessage("Please enter the Title in 100 words.", 0, false, false);
                $("#<%=txtTitle.ClientID%>").focus();
                return false;
            }
            else if ($("#<%=txtDescription.ClientID%>").val().length >500) {
                ShowMessage("Please enter the Description in 500 words.", 0, false, false);
                $("#<%=txtDescription.ClientID%>").focus();
                return false;
            }
            
            return true;
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="owTopone_left1">
        Add Note</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        
        <table id="fileform" class="owlistone fileform" runat="server" width="100%" border="0"
            cellspacing="0" cellpadding="5">
            <tr>
                <th width="20">
                </th>
                <th width="10">
                    Title:<span class="redstar">*</span>
                </th>
                <td width="*">
                    <asp:TextBox ID="txtTitle"   MaxLength="200" CssClass="input350" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th width="20">
                </th>
                <th width="10" valign="top">
                    Description:<span class="redstar">*</span>
                </th>
                <td width="*">
                    
                    <asp:TextBox ID="txtDescription"  CssClass="input350" TextMode="MultiLine"
                        Rows="5" runat="server" style="width:600"> </asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="btnBoxone">
        <asp:Button ID="btnSave" CssClass="btnone" ValidationGroup="Add" CausesValidation="true"
            runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return baseValidate();" />
        <input id="btnClientCancel" name="button" type="button" class="btnone" value="Clear" />
    </div>

    <script type="text/javascript">
        
    </script>

</asp:Content>