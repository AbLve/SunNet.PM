<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.Knowledge.New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        label.col-md-1, .col-md-1 {
            width: 60px;
        }
    </style>
    <script src="/Scripts/webuploader/webuploader.js"></script>
    <script src="/Scripts/webuploader/uploader.js"></script>

    <script type="text/javascript">
        var uploader;
        function getUploaderPrefix() {
            return "<%=UserInfo.UserID%>_";
        }
        jQuery(function () {
            uploader = SunnetWebUploader.CreateWebUploader({
                pick: { id: "#picker", multiple: false },
                container: "#thelist",
                speed: "#speed",
                uploadbutton: "#ctlBtn",
                submitbutton: $("input:submit"),
                targetField: "#<%=hidUploadFile.ClientID%>"
            });

            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
        });
        var _ajaxEvent = 0;
        function beforeSubmit(sender) {
            var $sender = $(sender);
            function completed() {
                clearTimeout(_ajaxEvent);
                $sender.show();
                $sender.siblings(".loading").hide();
            }
            if ($("form").valid()) {
                _ajaxEvent = setTimeout(function () {
                    $sender.hide();
                    $sender.siblings(".loading").show();
                }, 10);
                var uploaderStatus = uploader.getStats();
                if (uploaderStatus.queueNum > 0 && uploader.state == "ready") {
                    $sender.data("clicked", true);
                    uploader.upload();
                    return false;
                }
                if (uploaderStatus.successNum < 1 && uploaderStatus.uploadFailNum > 0) {
                    completed();
                    return false;
                }
                return true;
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Knowledge Share Ticket -
    <asp:Literal ID="ltlTicket" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <div class="form-group">
        <label class="col-md-1 lefttext">Ticket:</label>
        <div class="col-md-3 righttext">
            <div class="ticket-header">
                <%=Ticket.ID %>,&nbsp;<%=Ticket.Title %>
            </div>
        </div>
    </div>
    <div class="form-group">
        <label class="col-md-1 lefttext">Note:<span class="noticeRed">*</span></label>
        <div class="col-md-3 righttext">
            <asp:TextBox TextMode="MultiLine" CssClass="inputpmreview required" Style="width: 430px; max-width: 430px; max-height: 100px; height: 100px;" Rows="3" runat="server" ID="txtDescription"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-md-1 lefttext">Type:<span class="noticeRed">*</span></label>
        <div class="col-md-3 righttext">
            <asp:HiddenField ID="hidType" runat="server" />
            <asp:TextBox ID="txtType"
                data-autocomplete="true"
                data-remote="/Service/Share.ashx?action=getsharetype"
                data-input="value"
                data-width="250"
                data-height="120"
                data-itemwidth="200"
                runat="server" CssClass="required inputw3" autocomplete="off"></asp:TextBox>
        </div>
    </div>
    <div class="form-group DOMNodeInserted">
        <label class="col-md-1 lefttext">File:</label>
        <div class="col-md-3 righttext">
            <h5 id="speed" style="display: none;"></h5>
            <div id="picker">Select File</div>
            <div id="thelist" class="uploader-list"></div>
            <asp:HiddenField ID="hidUploadFile" runat="server" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button runat="server" ID="btnSave" Text="Submit" CssClass="saveBtn1 mainbutton"
        OnClientClick="return beforeSubmit(this);" OnClick="btnSave_Click" />
    <input name="Input322" type="button" class="cancelBtn1 mainbutton" data-dismiss="modal" aria-hidden="true" value="Cancel">
</asp:Content>
