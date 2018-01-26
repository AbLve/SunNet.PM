<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeedbackList.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.Ticket.FeedbackList" %>
<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagPrefix="custom" TagName="fileUpload" %>

<div class="contentTitle titleeventlist">
    <span class="titleeventlist_icons">
        <img src="/Images/chat_icon.png"></span>Chat<span class="titleeventlist_input">
            <asp:CheckBox ID="ckIsPublic" runat="server" Text=" Visible to client" CssClass="mr20 " />
            <%--<asp:CheckBox ID="chkIsWaitClientFeedback" runat="server" Text=" Waiting for client feedback"
                onclick="checkPublic(this);" CssClass="mr20 " />--%>
            <asp:CheckBox ID="chkIsWaitSunnetFeedback" runat="server" Text=" Awaiting SunNet's feedback" CssClass="mr20 " />
        </span>
</div>

<iframe id="iframe1" style="display: none;"></iframe>
<div class="chat_peoples">
    <span class="chat_peoples_s">Request response from:</span>

    <asp:PlaceHolder runat="server" ID="phlClientUsers" Visible="False">
        <ul id="fbl_clients_container" class="assignUser">
            <asp:Repeater ID="rptClient" runat="server">
                <ItemTemplate>
                    <li class="<%#Convert.ToBoolean(Eval("Writed").ToString())?"feedback_writed":"" %>">
                        <input name="Input" type="checkbox" value="<%# Eval("ID") %>" id='chkClient<%# Eval("ID") %>' <%# (bool)Eval("Selected") ?"checked='checked'":"" %> user="<%# Eval("ID") %>" update="<%#Eval("Update") %>" />
                        <label for="chkClient<%# Eval("ID") %>">
                            <%# Eval(BasePage.UserNameDisplayProp) %>
                        </label>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </asp:PlaceHolder>


    <asp:PlaceHolder runat="server" ID="phlClientUsers2" Visible="False">
        <ul id="fbl_clients2_container" class="assignUser">
            <asp:Repeater ID="rptClient2" runat="server">
                <ItemTemplate>
                    <li class="<%#Convert.ToBoolean(Eval("Writed").ToString())?"feedback_writed":"" %>">
                        <input name="AssignClients" type="checkbox" value="<%# Eval("ID") %>" id='chkClient<%# Eval("ID") %>' <%# (bool)Eval("Selected") ?"checked='checked'":"" %> user="<%# Eval("ID") %>" update="<%#Eval("Update") %>" />
                        <label for="chkClient<%# Eval("ID") %>">
                            <%# Eval(BasePage.UserNameDisplayProp) %>
                        </label>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </asp:PlaceHolder>
</div>


<div class="feedbackBox" id="feedbackBox">
    <%if (FbCreatedOn != null && FbCreatedOn.Count > 3)
        {  %>
    <div class="fdshowmore" id="dvShowEarlier"><a>Load Earlier Messages</a></div>
    <%} %>



    <div style="height: 480px; overflow-x: hidden; overflow-y: scroll" id="fbScroll">
        <ul class="fdItembox">
            <asp:Repeater ID="rptFeedBacksList" runat="server">
                <ItemTemplate>
                    <%# GetFeedBackHTMLBegin(Container.DataItem) %>
                    <%# FormatHTML(Eval("Description").ToString(),Server).Replace("\n", "<br/>") %>
                    <%# GetFeedBackHTMLEnd(Container.DataItem) %>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>

</div>

<a href="###" name="lastFeedback"></a>

<div id="Div1" class="chat_input_box" runat="server">

    <span class="chat_btns_u" id="picker"><a class="chat_btn_upload">
        <img src="/Images/chat_fj_icon.png" /></a></span>

    <textarea id="txtContent" data-msg="Please enter your message." rows="14" class="inputFeedback" placeholder="Enter your message..." runat="server"></textarea>

    <span class="chat_btns_p">
        <a class="chat_btn_push">
            <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="/Images/chat_push_icon.png" PostBackUrl="#" OnClientClick="return beforeSubmit(this);" Style="left: 0px; position: relative; top: 15px; border: 0px" />
            <img src="/Images/loading.gif" alt="Loading..." title="Loading..." class="loading" style="display: none; width: 36px; height: 36px;" />
        </a>
    </span>

</div>

<div id="thelist" class="uploader-list hide"></div>
<style type="text/css">
    .fdcontentBox3 {
      background-color: #9CCAFA;
      padding: 8px 20px 8px 8px;
    }

     #uploader .queueList {
         margin: 20px;
         border: 3px dashed #e6e6e6;
     }

    #uploader p {
        margin: 0;
    }

    #uploader .filelist {
        list-style: none;
        margin: 0;
        padding: 0;
    }

    #uploader .filelist:after {
        content: '';
        display: block;
        width: 0;
        height: 0;
        overflow: hidden;
        clear: both;
    }

    #uploader .filelist li {
        width: 110px;
        height: 110px;
        text-align: center;
        /*margin: 0 8px 20px 0;*/
        margin: 8px 10px;
        position: relative;
        display: inline;
        float: left;
        overflow: hidden;
        font-size: 12px;
    }

    #uploader .filelist li p.title {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
        top: 5px;
        text-indent: 5px;
        text-align: left;
    }

    @-webkit-keyframes progressmove {
        0% {
            background-position: 0 0;
        }

        100% {
            background-position: 17px 0;
        }
    }

    @-moz-keyframes progressmove {
        0% {
            background-position: 0 0;
        }

        100% {
            background-position: 17px 0;
        }
    }

    @keyframes progressmove {
        0% {
            background-position: 0 0;
        }

        100% {
            background-position: 17px 0;
        }
    }

    #uploader .filelist li p.imgWrap {
        position: relative;
        z-index: 2;
        line-height: 110px;
        vertical-align: middle;
        overflow: hidden;
        width: 110px;
        height: 110px;
        -webkit-transform-origin: 50% 50%;
        -moz-transform-origin: 50% 50%;
        -o-transform-origin: 50% 50%;
        -ms-transform-origin: 50% 50%;
        transform-origin: 50% 50%;
        -webit-transition: 200ms ease-out;
        -moz-transition: 200ms ease-out;
        -o-transition: 200ms ease-out;
        -ms-transition: 200ms ease-out;
        transition: 200ms ease-out;
    }

    #uploader .filelist li img {
        width: 100%;
    }

    #uploader .filelist div.file-panel {
        position: absolute;
        height: 0;
        /*filter: progid:DXImageTransform.Microsoft.gradient(GradientType=0,startColorstr='#80000000', endColorstr='#80000000')\0;*/
        /*background: rgba( 0, 0, 0, 0.5 );*/
        background: rgba( 0, 0, 0, 1 );
        filter: alpha(opacity=50); /*支持 IE 浏览器*/
        -moz-opacity: 0.50; /*支持 FireFox 浏览器*/
        opacity: 0.50; /*支持 Chrome, Opera, Safari 等浏览器*/
        width: 100%;
        top: 0;
        left: 0;
        overflow: hidden;
        z-index: 300;
    }

    #uploader .filelist div.file-panel span {
        width: 20px;
        height: 22px;
        display: inline;
        float: right;
        text-indent: -9999px;
        overflow: hidden;
        background: url('/Images/icons/cancelled_grey.png');
        margin: 5px 1px 1px;
        cursor: pointer;
    }
</style>
<div id="uploader" class="wu-example">
    <div class="queueList"></div>
</div>
<h5 id="speed" style="display: none;"></h5>
<asp:HiddenField ID="hidUploadFile" runat="server" />

<script src="/Scripts/webuploader/webuploader.js"></script>
<script src="/Scripts/webuploader/uploader.js"></script>
<script type="text/html" id="temp_feedback_date">
    <li class="fdmessageDate" date="{% this.date %}">{% this.date %}</li>
</script>
<script type="text/html" id="temp_feedback">
    <li class="myselfbox" date="{% this.date %}" index="{% this.index %}">

        <div style="text-align: right;">
            <span class="fdUser">{% this.firstname %} {% this.lastname %}</span>
            <span class="fdDate">{% this.time %}</span>
        </div>

        <table border="0" cellpadding="4" cellspacing="0">
            <tbody class="chatMsg">
                <tr>
                    <td class="{% this.tdCss %} fdcontent" style="max-width: 40%; word-break: break-word">
                        <div class="rightClose" onclick="deleteOwnFeedBack(this,{% this.id %},{% this.createdBy %})" title="Delete">×</div>
                        {% if (this.extraStatus && this.extraStatus.waitSunnet)  { %}
                       <span title="Waiting for Client feedback" class="feedback_waiting">#{% this.order %}.</span>
                        {% } else if (this.extraStatus && this.extraStatus.waitClient)  { %}
                        <span title="Waiting Sunnet feedback" class="feedback_waiting">#{% this.order %}.</span>
                        {% } else { %}
                        #{% this.order %}.
                        {% } %}

                        {% this.content %}

                        {% if (this.file) { %}
                        <span class="fdfileBox">{% for (var f in this.file) { %}
                            <span class="fdfile">
                                <a href="/do/DoDownloadFileHandler.ashx?FileID={% this.file[f] %}" target="_blank">
                                    <img src="{% this.basicInfo[f] %}" style="width: 13px;">{% f %}
                                </a>
                            </span>
                            {% } %}
                        </span>
                        {% } %}
                    </td>
            </tbody>
        </table>
    </li>
</script>

<script type="text/javascript">
    var _ajaxEvent = 0;
    var uploadFileCount = 0;
    var $submitBox = $("div.chat_input_box a.chat_btn_push");

    function startSubmit() {
        $submitBox.css("background-color", "#eee");
        $submitBox.find("img").css("display", "inline-block");
        $submitBox.find(":image").css("display", "none");
    }

    function endSubmit() {
        $submitBox.find(":image").css("display", "inline-block");
        $submitBox.css("background-color", "#609629");
        $submitBox.find("img").css("display", "none");
    }

    function ajaxSubmit() {
        var clients = "";
        jQuery("input:checkbox[name='AssignClients']:checked").each(function(index, checkbox) {
            clients += jQuery(checkbox).attr("user") + ",";
        });

        $.ajax({
                url: "/do/DoFeedBacks.ashx",
                type: "post",
                data: {
                    type: "add",
                    project: "<%=TicketsEntityInfo.ProjectID%>",
                    ticket: "<%=TicketsEntityInfo.TicketID%>",
                    content: ConvertScript(jQuery("#<%=txtContent.ClientID%>").val()),
                    isPublic: jQuery("#<%=ckIsPublic.ClientID%>").prop("checked"),

                    isWaitSunnetFeedback: jQuery("#<%=chkIsWaitSunnetFeedback.ClientID%>").prop("checked"),
                    uloadFile: jQuery("#<%=hidUploadFile.ClientID%>").val(),
                    clients: clients
                },
                dataType: "json",
                beforeSend: function (xhr) {
                    startSubmit();
                }
            })
            .success(function(message) {
                if (message && message.success) {
                    console.log(message.data.extraStatus);
                    if (message.data &&
                        message.data.extraStatus &&
                        (
                            message.data.extraStatus.clearClient ||
                                message.data.extraStatus.clearSunnet ||
                                message.data.extraStatus.waitSunnet ||
                                message.data.extraStatus.waitClient)) {
                        location.href = location.pathname + location.search + "&gotofeedback=1";
                    }
                    if (message.data && message.data.extraStatus && message.data.extraStatus.clearClient) {
                        jQuery("#chkClient<%=UserInfo.ID%>").parent().removeClass("feedback_waiting")
                            .addClass("feedback_writed");
                    }
                    jQuery("#<%=txtContent.ClientID%>").val("");

                    if (uploader.getStats().successNum > 0) {
                        uploader.removeFile(uploader.getFiles()[uploader.getFiles().length - 1]);
                    }

                    $("#thelist").empty();
                    $("#speed").empty();

                    var chkIsPublic = jQuery("#<%=ckIsPublic.ClientID%>");

                    var chkIsWaitSunnetFeedback = jQuery("#<%=chkIsWaitSunnetFeedback.ClientID%>");

                    chkIsPublic.prop("checked", false).removeAttr("disabled");

                    chkIsWaitSunnetFeedback.prop("checked", false);
                    jQuery("#<%=hidUploadFile.ClientID%>").val("");

                    var html = "";

                    if (message.data.content)
                        message.data.content = message.data.content + "<br/>";

                    var $existsnotes = $feedbackBox.find("li:not(.fdmessageDate)");
                    message.data.index = $existsnotes.length ? (+($existsnotes.last().attr("index")) + 1) : 1;

                    if ($feedbackBox.find(".fdmessageDate[date='" + message.data.date + "']").length == 0) {
                        html += TemplateEngine(GetTemplateHtml("temp_feedback_date"), message.data);
                        $(html).hide().appendTo($feedbackBox.find("ul:first")).slideDown();
                        html = "";
                    }

                    html += TemplateEngine(GetTemplateHtml("temp_feedback"), message.data);

                    $(html).hide().appendTo($feedbackBox.find("ul:first")).slideDown();
                    $("#fbScroll").scrollTop(1000);
                } else {
                    ShowMessage(message.msg, "danger");
                }
            })
            .complete(function() {
                $("tbody.chatMsg span.fdfileBox span.fdfile a").each(function(i, x) {
                    var th = $(this);
                    var thTxt = String(th.text()).trim();
                    var index = thTxt.indexOf(".");
                    if (index > -1) {
                        var suf = thTxt.substring(index, thTxt.length);
                        var imgFormat = ".gif,.jpg,.jpeg,.bmp,.png,.svg,.raw".split(",");
                        if ($.inArray(suf, imgFormat) > -1) {
                            th.find("img").attr("style", "width: 110px; height: 110px;");
                        }
                    }
                });
                endSubmit();
            });
    }

    function beforeSubmit(sender) {
        startSubmit();

        if ($("form").valid() == false) {
            endSubmit();
            return false;
        }

        if (uploader.getStats().successNum < 1 && uploader.getStats().uploadFailNum > 0) {
            alert("上传件出现错误！");
            endSubmit();
            return false;
        }

        if (uploader.getStats().queueNum > 0 && uploader.state == "ready") {
            uploadFileCount = uploader.getStats().queueNum;
            //上传文件
            uploader.upload(); //最后一个上传完成，回调函数里，提交表信息。
            return false;
        } else {
            ajaxSubmit();
            return false;
        }
    };

    function getUploaderPrefix() {
        return "<%=UserInfo.UserID%>_";
    }

    var uploader;
    var $feedbackBox;
    function deleteOwnFeedBack(elem, fbID, userID) {
        $.get("/do/DoFeedBacks.ashx",
            {
                "FeedbackID": fbID,
                "UID": userID, type: "delete",
                "ticket": "<%=TicketsEntityInfo.TicketID%>"
            }
            , function (msg) {
                if (msg.success) {
                    var $target = $(elem).closest(".myselfbox");
                    $target.slideUp("fast", function () {
                        var date = $(this).attr("date");
                        $(this).remove();
                        if ($feedbackBox.find("li[date='" + date + "']").length == 1) {
                            $feedbackBox.find("li[date='" + date + "']").slideUp("fast", function () {
                                $(this).remove();
                            });
                        }
                    });
                }
                else {
                    ShowMessage(msg.msg, 'error', true, true);
                }
            }, "json");
        }

    function initUploadControls() {
        uploader = SunnetWebUploader.CreateWebUploader({
            fileNumLimit: 5,
            pick: { id: "#picker", multiple: true },
            container: "#thelist",
            uploadbutton: "#ctlBtn",
            submitbutton: $("input:image"),
            targetField: "#<%=hidUploadFile.ClientID%>",
            dnd: "div.form-group-container div.chat_input_box",
            disableGlobalDnd: true
        });
        uploader.on('uploadComplete',
            function(file) {
                uploadFileCount -= 1;
                if (uploadFileCount <= 0) {
                    ajaxSubmit();
                }
            });
    }


    var chkboxLst = [];
    $(document).ready(function() {
        if (urlParams.gotofeedback) {
            location.href = location.pathname + location.search + "#lastFeedback";
        }
        initUploadControls();

        $(".webuploader-pick").removeClass().addClass("antiHover");
        //$(".antiHover").unbind("mouseenter");

        $(".antiHover").mouseenter(function() {
            $(".antiHover").removeClass();
        });

        $feedbackBox = $("#feedbackBox");
        $("#dvShowEarlier").on("click",
            function() {
                var loc = $("#fbScroll").scrollTop();
                for (i = 0; i < 11; i++) {
                    $feedbackBox.find("li:not(:visible):last").slideDown("fast");
                }
                $("#fbScroll").scrollTop(loc);
                if (!$feedbackBox.find("li:not(:visible)").size()) {
                    $("#dvShowEarlier").slideUp("fast");
                }
            });

        jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
        $("form").validate({
            errorElement: "div",
            rules: {
                "<%=txtContent.UniqueID%>": {
                    required: function() {
                        return $("#<%=hidUploadFile.ClientID%>").val().length < 1 && uploader.getStats().queueNum < 1;
                    }
                }
            },
            errorPlacement:
                function(error, element) {
                    error.appendTo(element.parent());
                }
        });

        $("#fbl_clients_container input:checkbox").each(function() {
            checkPublicInit($(this));
        });
        $("#fbl_clients2_container input:checkbox").each(function() {
            checkPublicInit($(this));
        });

        $("#fbl_clients_container").on("click",
            "input:checkbox",
            function() {
                var $this = $(this);
                checkPublic($this);
                assignClient($this);
            });
        $("#fbl_clients2_container").on("click",
            "input:checkbox",
            function() {
                var $this = $(this);
                checkPublic($this);

                if ("<%= IsWaitClientFeedback %>" == "False") {
                    assignClient($this);
                }

            });
    });

    function assignClient($checkbox) {

        var params = {
            action: $checkbox[0].checked ? "set_client_notification_on" : "set_client_notification_off",
            ticket: '<%= TicketID%>',
            user: $checkbox.attr("user"),
            update: $checkbox.attr("update")
        };
        $.post("/Service/TicketUser.ashx",
            params,
            function(response) {
                if (response.success) {
                    $checkbox.closest("li").removeClass("feedback_writed");
                } else {
                    ShowMessage(response.msg, "danger");
                }
            },
            "json");
    }

    function ConvertScript(input) {
        return input.replace("<\/script>", "<@script>");
    }

    function checkPublicInit(o) {
        var siblingText = $(o).siblings("label").text();
        if ($(o).prop('checked')) {
            chkboxLst.push(siblingText);
        }

        if (chkboxLst.length > 0) {
            $("#<%=ckIsPublic.ClientID %>").prop('checked', true);
            $("#<%=ckIsPublic.ClientID %>").prop('disabled', true);
        }
    }

    function checkPublic(o) {
        var siblingText = $(o).siblings("label").text()
        var checked = $(o).prop('checked');
        if ($(o).prop('checked')) {
            chkboxLst.push(siblingText);
        } else {
            var ind = chkboxLst.indexOf(siblingText);
            chkboxLst.splice(ind, 1);
        }

        if (chkboxLst.length > 0) {
            $("#<%=ckIsPublic.ClientID %>").prop('checked', true);
            $("#<%=ckIsPublic.ClientID %>").prop('disabled', true);
        } else {
            $("#<%=ckIsPublic.ClientID %>").prop('checked', false);
            $("#<%=ckIsPublic.ClientID %>").prop('disabled', false);
        }
    }

    $(function() {
        $("#fbScroll").scrollTop(1000);
    });

</script>

<style type="text/css">
    .antiHover {
    }
</style>
