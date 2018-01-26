<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="AddSealNote.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Seals.AddSealNote" %>

<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagName="fileuploader" TagPrefix="custom" %>
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
    Seal Note
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">   
    <div class="form-group">
        <label class="col-left-fddeny lefttext" >Title:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">
            <asp:TextBox ID="txtTitle" runat="server"   MaxLength="128" CssClass="inputw5 required"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-fddeny lefttext">Description:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">          
             <asp:TextBox ID="txtDescription" runat="server" Validation="true" MaxLength="500" CssClass="inputw5 required" TextMode="MultiLine" Rows="6"></asp:TextBox>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button ID="btnSubmit" runat="server" CssClass="saveBtn1 mainbutton" Text=" Add " OnClick="btnSave_Click" />
    <input name="Input22" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
</asp:Content>
