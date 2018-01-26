<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="AddProjectPrincipal.aspx.cs"
     Inherits="SunNet.PMNew.PM2014.Admin.Projects.AddProjectPrincipal" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
          jQuery(function () {
              // validate signup form on keyup and submit
              jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
              $("form").validate({
                  errorElement: "div"
              });
          });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Project Principal
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">   
    <div class="form-group">
        <label class="col-left-fddeny lefttext" > Module/Function:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">
            <textarea id="txtModule" style="max-width: 210px; width: 210px !important; min-width: 210px;height:60px" runat="server"
                              rows="5" class="input98p required"></textarea>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-fddeny lefttext">PM:</label>
        <div class="col-right-fddeny righttext">
           <textarea id="txtPM" style="max-width: 210px; width: 210px !important; min-width: 210px;height:60px"  runat="server"
                            rows="5" class="input98p"></textarea>
        </div>
    </div>
     <div class="form-group">
        <label class="col-left-fddeny lefttext">DEV:</label>
        <div class="col-right-fddeny righttext">
           <textarea id="txtDEV" style="max-width: 210px; width: 210px !important; min-width: 210px;height:60px"  runat="server"
                            rows="5" class="input98p"></textarea>
        </div>
    </div>
     <div class="form-group">
        <label class="col-left-fddeny lefttext">Tester:</label>
        <div class="col-right-fddeny righttext">
           <textarea id="txtQA" style="max-width: 210px; width: 210px !important; min-width: 210px;height:60px"  runat="server"
                           rows="5" class="input98p"></textarea>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button ID="btnSubmit" runat="server" CssClass="saveBtn1 mainbutton" Text=" Add " OnClick="btnSave_Click" />
    <input name="Input22" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
</asp:Content>

