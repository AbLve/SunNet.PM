<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pop.master" ClientIDMode="Predictable"
    CodeBehind="MoveTimeSheet.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Timesheet.MoveTimeSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript">
        $(function () {
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div",
                onkeyup: false,
                onfocusout: false
            });
        });

        function save_Confirm() {
            if ($("form").valid() == false) {
                return false;
            }
            else {
                var r = confirm("Are you sure you want to move this timesheet to this day?");
                return r;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Move to Another Day
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server" class="buttonBox2">
    <div class="form-group" style="text-align: center; padding-left: 37px">
        <div class="col-right-newevent righttext">
            <label class="col-left-newevent">New Date:</label>
            <asp:TextBox CssClass="inputdate required" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy',isShowClear:false});"
                ID="txtFrom" runat="server" Style="width: 180px">
            </asp:TextBox>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button runat="server" Text="Save" CssClass="saveBtn1 mainbutton" ID="btnSave"
        OnClientClick="return save_Confirm();" OnClick="btnSave_Click" />
    <input name="btnCancel" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
</asp:Content>

