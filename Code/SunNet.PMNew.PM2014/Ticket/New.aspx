<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" MasterPageFile="~/Ticket/Client.master" Inherits="SunNet.PMNew.PM2014.Ticket.New" %>

<%@ Register Src="../UserControls/Messager.ascx" TagName="Messager" TagPrefix="custom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #dvType table td, #dvPriority table td {
            padding-right: 10px;
        }

        #dvType, #dvPriority {
            border: 0;
        }

        .saveHiddenBtn {
            visibility: hidden;
        }
        @media (max-width: 992px){
            .selectw2, .inputw2 {
                width: 98% !important;
            }
            #body_body_dataSection_txtDesc{
                width:98% !important;
            }
        }
    </style>
    <script src="/Scripts/webuploader/webuploader.js"></script>
    <script src="/Scripts/webuploader/uploader.js"></script>
    <script type="text/javascript">
        $(function () {
            var $emergency = $("#<%=rdoPriority.ClientID%>").find("input:radio:last");
            var $tdToTip = $emergency.closest("td");
            $('<a data-toggle="popover" class="info" title="Emergency" href="###">&nbsp;</a>').popover({
                container: "body",
                placement: "right",
                content: "<span class=\'noticeRed\'>**The Emergency Support fee will be based on 1.5 times the regular rate. </span>",
                trigger: "hover click",
                html: true
            }).appendTo($tdToTip);

            var project = +($("#<%=ddlProject.ClientID%>").val());
            if (project > 0) {
                $("#<%=txtTitle.ClientID%>").focus();
            } else {
                $("#<%=ddlProject.ClientID%>").focus();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="dataSection" runat="server">
    <div class="form-group-container" style="width: 615px;">
        <custom:Messager ID="Messager1" runat="server" />
        <div class="form-group">
            <label class="col-left-1 lefttext">Project:<span class="noticeRed">*</span></label>
            <div class="col-right-1 righttext">
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="selectw2 required" TabIndex="1">
                </asp:DropDownList><br />
                Project not found? Please contact <a href="mailto:team@sunnet.us">team@sunnet.us</a>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">Title:<span class="noticeRed">*</span></label><div class="col-right-1 righttext">
                <asp:TextBox runat="server" class="inputw2 required" ID="txtTitle" TabIndex="2"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">Description:<span class="noticeRed">*</span></label><div class="col-right-1 righttext">
                <textarea id="txtDesc" runat="server" cols="20" class="inputw2 required" rows="6" tabindex="3"></textarea>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">URL:</label><div class="col-right-1 righttext">
                <asp:TextBox runat="server" class="inputw2" ID="txtUrl" TabIndex="4" MaxLength="290"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">Attachments:</label>
            <div class="col-right-1 righttext">
                <asp:Literal runat="server" ID="litFiles"></asp:Literal>
                <div id="thelist" class="uploader-list"></div>
                <h5 id="speed" style="display: none;"></h5>
                <div id="picker">Select Files (Up to 5 files)</div>
                <asp:HiddenField ID="hidUploadFile" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">Type:<span class="noticeRed">*</span></label>
            <div class="col-right-1 righttext" id="dvType">
                <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal" CssClass="rightItem" CellPadding="0" CellSpacing="0">
                    <asp:ListItem Text=" Request" Value="1" Selected="True" tabindex="6"> </asp:ListItem>
                    <asp:ListItem Text=" Bug" Value="0" tabindex="7"> </asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">Priority:<span class="noticeRed">*</span></label>
            <div class="col-right-1 righttext" id="dvPriority">
                <asp:RadioButtonList runat="server" ID="rdoPriority" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                    <asp:ListItem Text=" Low" Value="1" tabindex="11"></asp:ListItem>
                    <asp:ListItem Text=" Medium" Value="2" Selected="True" tabindex="12"></asp:ListItem>
                    <asp:ListItem Text=" High" Value="3" tabindex="13"></asp:ListItem>
                    <asp:ListItem Text=" Emergency" Value="4" tabindex="14"></asp:ListItem>
                </asp:RadioButtonList>
                <span class="redstar" style="display: none;">**The Emergency Support fee will be based on 1.5 times the regular
                        rate. </span>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">Estimation:</label>
            <div class="col-right-estimate righttext">
                <span class="rightItem">
                    <asp:CheckBox ID="chkEN" runat="server" Text=" Needed" TabIndex="15" />
                </span>
            </div>
            <label class="col-left-source lefttext" runat="server" id="lblSource" style="visibility: hidden">Source:</label>
            <div class="col-right-source righttext" runat="server" id="dvSource" style="visibility: hidden">
                <asp:DropDownList runat="server" ID="ddlSource" CssClass="selectw3" TabIndex="16"></asp:DropDownList>
            </div>
        </div>

        <div class="buttonBox1">
            <asp:HiddenField ID="hidSaveType" runat="server" />
            <img src="/Images/loading.gif" alt="Loading..." title="Loading..." class="loading" style="display: none;" />


            <asp:Button ID="btnSaveAsDraft" runat="server" Text="Save as Draft" CssClass="saveBtn1 mainbutton"
                OnClick="btnSave_Click" OnClientClick=" return beforeSubmit2(this,2);" TabIndex="17" />

            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="saveBtn1 mainbutton" OnClick="btnSave_Click"
                OnClientClick=" return beforeSubmit2(this,1);" TabIndex="18" />

            <asp:Button ID="btnEditSave" runat="server" Text="Save" CssClass="saveBtn1 mainbutton" OnClick="btnSave_Click"
                OnClientClick=" return beforeSubmit2(this,3);" TabIndex="19" />

            <asp:Button ID="btnEditSubmit" runat="server" Text="Submit" CssClass="saveBtn1 mainbutton" OnClick="btnSave_Click"
                OnClientClick=" return beforeSubmit2(this,4);" TabIndex="20" />

            <asp:Button ID="btnSaveAndNew" runat="server" Text="Submit &amp; New" CssClass="saveBtn1 mainbutton"
                OnClientClick=" return beforeSubmit2(this,5);" OnClick="btnSave_Click" TabIndex="21" />

            <input id="btnClear" type="button" value="Clear" class="cancelBtn1 mainbutton" onclick="clearFields();" runat="server" tabindex="22" />
            <input type="button" value="Back" class="backBtn mainbutton redirectback" tabindex="23" />

            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick=" return beforeSubmit(this,0);" CssClass="saveHiddenBtn" />
        </div>
    </div>
    <script type="text/javascript">
        var oldForm = null;
        var projectInfo = null;
        var message = {
            required: "This filed is required!",
            wordCountLimit: "Title should less than 200 character."
        };

        function getUploaderPrefix() {
            return "<%=UserInfo.UserID%>_";
        }

        var uploader;

        var _ajaxEvent = 0;

        function beforeSubmit2(sender, saveType) {

            $hidSaveType.val(saveType);
            beforeSubmit(document.getElementById("<%=btnSave.ClientID%>"), saveType);
            return false;
        }

        var $hidSaveType = $("#<%=hidSaveType.ClientID%>");
        function beforeSubmit(sender, saveType) {
            if (saveType == 0) {
                return true;
            }
            else {
                var $sender = $(sender);

                if ($("form").valid()) {
                    $('.buttonBox1').attr('validated', '1');

                    _ajaxEvent = setTimeout(function () {
                        $sender.hide();
                        $sender.siblings(".loading").show();
                    }, 10);

                    var uploaderStatus = uploader.getStats();
                    var files = uploader.getFiles();

                    if (uploader.getStats().queueNum > 0 && uploader.state == "ready") {

                        $sender.data("clicked", true);
                        uploader.upload();
                        return false;
                    }

                    if (uploaderStatus.successNum < 1 && uploaderStatus.uploadFailNum > 0) {
                        completed();
                        return false;
                    }
                    $sender.click();
                };
                return false;
            }
            return false;
        };


        $(function () {
            initUploadControls();
            projectInfo = eval("<%=jsonProjectInfo%>");
            checkIfCanAdd($("#" + "<%=ddlProject.ClientID%>").val(), "#" + "<%=ddlProject.ClientID%>");
            oldForm = $('form').serialize();
            $('.buttonBox1').on('click', function () { $('.buttonBox1').attr('clicked', '1'); });
            $("#" + "<%=ddlProject.ClientID%>").on("change", function () {
                checkIfCanAdd($(this).val(), this);
                $("#" + "<%=chkEN.ClientID%>").prop("checked", $(this).val() && projectInfo[$(this).val()][0] == "True");
            });

            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
        });

        $(window).on('beforeunload', function () {
            var isSubmit = $('.buttonBox1').attr('clicked') != '1';
            var hasChangeForm = $('form').serialize() != oldForm;
            var isValidated = $('.buttonBox1').attr('validated') == '1';
            if ((hasChangeForm && isSubmit) || (hasChangeForm && !isValidated)) {
                return '';
            }
        });

        function clearFields() {
            document.getElementById("<%= ddlProject.ClientID%>").selectedIndex = 0;
            document.getElementById("<%= txtTitle.ClientID%>").value = "";
            document.getElementById("<%= txtUrl.ClientID%>").value = "";
            document.getElementById("<%=  txtDesc.ClientID%>").value = "";
            document.getElementById("<%= chkEN.ClientID%>").checked = false;
            $("#<%=rdoType.ClientID%>_0").prop("checked", true);
            $("#<%=rdoPriority.ClientID%>_1").prop("checked", true);

            $("input[type=\"file\"").each(function (index, item) {
                if (/MSIE/.test(navigator.userAgent)) {
                    $(item).replaceWith($(this).clone(true));
                } else {
                    $(item).val('');
                }
            });
        }


        function initUploadControls() {
            uploader = SunnetWebUploader.CreateWebUploader({
                fileNumLimit: 5,
                pick: { id: "#picker", multiple: true },
                container: "#thelist",
                uploadbutton: "#ctlBtn",
                submitbutton: $("#<%=btnSave.ClientID%>"),
                targetField: "#<%=hidUploadFile.ClientID%>"
            });
        }

        function deleteImgWhenStatusDraft(s) {
            $.ajax({
                type: "post",
                data: {
                    fileid: s
                },
                url: "/Do/DoRemoveFileHandler.ashx",
                success: function (result) {
                    window.location.reload();
                }
            });
        }

        function checkIfCanAdd(projectId, object) {
            if (projectId && projectInfo[projectId][1] != "True") {
                ShowMessage("This project is closed, if you need to submit new tickets for this project, please contact us at team@sunnet.us.", 2);
                $(object).prop('selectedIndex', 0);
            }
            else {
                $("div[class='alert alert-danger fade hide in']").hide();
            }
        }
    </script>
</asp:Content>

