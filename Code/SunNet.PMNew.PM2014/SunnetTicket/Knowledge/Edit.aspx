<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.Knowledge.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        label.col-md-1, .col-md-1 {
            width: 60px;
        }

        ul.files li {
            float: left;
            margin-right: 30px;
        }

            ul.files li a:first-child {
                display: inline-block;
                margin-right: 5px;
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
            $("body").on("click", "[data-action='delete']", function () {
                var $this = $(this);
                var options = $.extend({}, { remote: "", key: "id" }, $this.data());
                if (options.remote && options[options.key] > 0) {
                    $.confirm("Confirm to delete?", {
                        yesText: "Delete",
                        yesCallback: function () {
                            jQuery.post(options.remote, options, function (response) {
                                if (response.success) {
                                    $this.closest("li").remove();
                                } else {
                                    ShowMessage(response.msg, "danger");
                                }
                            }, "json");
                        },
                        noText: "Cancel"
                    });
                }
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
    Edit Knowledge Share Ticket - <%=Current.TicketID %>
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
        <div class="col-md-3 righttext righttext-view">
            <ul class="files">
                <asp:Repeater ID="rptFiles" runat="server">
                    <ItemTemplate>
                        <li>
                            <a href='/do/DoDownloadFileHandler.ashx?FileID=<%#Eval("Key") %>' title='Download' target='_blank'><%#Eval("Value") %></a>
                            <a href="javascript:;" title="Delete"
                                data-remote="/Service/File.ashx"
                                data-action="delete"
                                data-id="<%#Eval("Key") %>">
                                <img src="/Images/icons/delete.png" alt="Del" />
                            </a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div class="clear-h5"></div>
            <div>
                <h5 id="speed" style="display: none;"></h5>
                <div id="picker">Select File</div>
                <div id="thelist" class="uploader-list"></div>
                <asp:HiddenField ID="hidUploadFile" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button runat="server" ID="btnSave" Text="Submit" CssClass="saveBtn1 mainbutton"
        OnClientClick="return beforeSubmit(this);" OnClick="btnSave_Click" />
    <input name="Input322" type="button" class="cancelBtn1 mainbutton" data-dismiss="modal" aria-hidden="true" value="Cancel">
</asp:Content>
