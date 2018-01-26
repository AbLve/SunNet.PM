<%@ Page Title="Deny ticket" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="Deny.aspx.cs" Inherits="SunNet.PMNew.PM2014.Ticket.Deny" %>

<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagName="fileuploader" TagPrefix="custom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function sucessCall(parentReloadUrl) {
           layer.msg(
                '<span style="color:#eee;">Operation successful.</span>',
                {
                    time: 5000,
                    shade: [0.3],
                    shadeClose:false,
                    scrollbar: false,
                    offset: '82px'
                },
                function() {
                    window.top.location.href = parentReloadUrl;
                }
            );
            //另一种方法是利用jQuery.sunnet.js方法,注意样式
            //jQuery.alertCustom("success", "Operation successful.", 5, parentReloadUrl);
        }
        $(function () {
            $('#aEmergency').popover({
                container: "body",
                placement: "right",
                content: "<span class=\'noticeRed\'>**The description will be added to the note automatically. </span>",
                trigger: "hover click",
                html: true
            }).appendTo($('#aEmergency').closest("label"));
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Deny Reason
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <div style="font-size: 13px; font-weight: bold; padding-bottom: 20px;">
        <asp:Literal runat="server" ID="litHead"></asp:Literal>
    </div>
    <div class="form-group">
        <label class="col-left-fddeny lefttext" style="width: 92px !important;">Comment:<a href="###" title="" id="aEmergency" style="width: 18px; height: 18px;" class="info" data-toggle="popover" data-original-title="Prompt">&nbsp;</a></label>
        <div class="col-right-fddeny righttext">
            <asp:TextBox TextMode="MultiLine" runat="server" Rows="3" ID="txtDescription" CssClass="inputw5"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-fddeny lefttext" style="width: 92px !important;">Upload File:</label>
        <div class="col-right-fddeny righttext">
            <custom:fileuploader runat="server" ID="fileuploader" FileUploadCount="1" UploadType="Add" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button ID="btnSubmit" runat="server" CssClass="saveBtn1 mainbutton" Text="Submit" OnClick="btnSubmit_Click" />
    <input name="Input22" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
</asp:Content>
