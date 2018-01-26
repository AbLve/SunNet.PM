<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="AddDocument.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.ProposalTracker.AddDocument" %>

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
    Add Document
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <div class="form-group">
        <label class="col-left-fddeny lefttext">Title:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">
            <asp:TextBox ID="txtFileTitle" MaxLength="100" CssClass="inputw5 required" runat="server"></asp:TextBox>
        </div>
    </div>

     <div class="form-group">
        <label class="col-left-fddeny lefttext">Tags:</label>
        <div class="col-right-fddeny righttext">
            <asp:TextBox ID="txtTags" MaxLength="200" CssClass="inputw5" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="form-group">
        <label class="col-left-fddeny lefttext"> File:<span class="noticeRed">*</span></label>
        <div class="col-right-fddeny righttext">
            <asp:FileUpload ID="fileUpload" runat="server" onchange="validationFile()" />
             <input type="text" style="display: none;" id="hdFileUpolad" name="hdFileUpolad" class="required" data-msg="Please upload file" />
        </div>
    </div>
      <script type="text/javascript">
          function validationFile() {
              jQuery("#hdFileUpolad").val(jQuery("#<%=fileUpload.ClientID %>").val());
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button ID="btnSubmit" runat="server" CssClass="saveBtn1 mainbutton" Text=" Add " OnClick="btnSave_Click" />
    <input name="Input22" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
</asp:Content>


