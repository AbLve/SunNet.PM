<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeedBacksList.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.FeedBacksList" %>
<%@ Register Src="UploadFile.ascx" TagName="UploadFile" TagPrefix="uc1" %>
<iframe id="iframeDownloadFile" style="display: none;"></iframe>
<div class="feedbackBox">
    <ul class="fdItembox">
        <asp:Repeater ID="rptFeedBacksList" runat="server">
            <ItemTemplate>
                <%#GetFeedBackHTML(Convert.ToInt32(Eval("CreatedBy")),(DateTime)Eval("CreatedOn"),(bool)Eval("IsPublic")
                ,Eval("Title").ToString(),Eval("Description").ToString(),Eval("FeedBackID").ToString(),Eval("WaitClientFeedback").ToString(),Eval("WaitPMFeedback").ToString())%>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div class="feedbackBox" runat="server" id="divAddFeedback">
        <table border="0" cellspacing="0" cellpadding="4" width="100%">
            <tbody>
                <tr>
                    <td colspan="3">
                        <textarea runat="server" id="txtContent" rows="4" class="input98p"></textarea>
                    </td>
                </tr>
                <tr id="trOthers" runat="server">
                    <td valign="top">
                        <asp:CheckBox ID="ckIsPublic" runat="server" Text=" Is visible to client" />
                        <asp:CheckBox ID="chkIsWaitClientFeedback" runat="server" Text=" Is wait client feedback"
                            onclick="checkPublic(this);" />
                        <asp:CheckBox ID="chkIsWaitPMFeedback" runat="server" Text=" Is wait PM feedback" />
                    </td>
                    <td valign="top">
                        <asp:Literal ID="lilImgList" runat="server"></asp:Literal>
                        <uc1:UploadFile ID="UploadFile1" runat="server" />
                        <asp:HiddenField ID="hd_Project" runat="server" />
                        <asp:HiddenField ID="hfPublic" runat="server" Value="false" />
                    </td>
                    <td valign="top" align="right">
                        <input id="btnAssign" runat="server" type="button" class="fdsaveBtn" value="Submit" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>


<script type="text/javascript">
    function deleteOwnFeedBack(fbID, userID) {
        $.get("/do/dodeleteownfeedback.ashx", { "FeedbackID": fbID, "UID": userID }
            , function (data) {
                var message = "";
                if (data == 1) {
                    message = "Delete success.";
                }
                else if (data == 2) {
                    message = "Unauthorized.";
                }
                else {
                    message = "Delete feedback failed.";
                }
                ShowMessageAndJumpToAnchor(message, true, "Feedback");
            });
    }

    $(document).ready(function () {
        var content = $("#<%=txtContent.ClientID%>");
        $("#<%=btnAssign.ClientID%>").click(function () {
            var title = '';
            var descr = content.attr("value");
            var ckIsPublic = $("#<%=ckIsPublic.ClientID%>").attr("checked");
            var chkIsWaitClientFeedback = $("#<%=chkIsWaitClientFeedback.ClientID%>").attr("checked");
            var chkIsWaitPMFeedback = $("#<%=chkIsWaitPMFeedback.ClientID%>").attr("checked");
            var imageList = "";
            var imageSizeList = "";

            $('li font').each(function () {
                imageSizeList += $(this).text() + ",";
            })

            $('li b').each(function () {
                imageList += $(this).text() + ",";
            })

            //validate   
            title = $.trim(title);
            descr = $.trim(descr);

            if (ckIsPublic == "checked") {
                ckIsPublic = true;
            } else {
                ckIsPublic = $("#<% =hfPublic.ClientID %>").val();
            }
            if (chkIsWaitClientFeedback == "checked") {
                chkIsWaitClientFeedback = true;
            } else {
                chkIsWaitClientFeedback = false;
            }
            if (chkIsWaitPMFeedback == "checked") {
                chkIsWaitPMFeedback = true;
            } else {
                chkIsWaitPMFeedback = false;
            }

            var fileCount = $(".files").find("ul li").length;
            if (title.length < 1 && descr === _placeholder && fileCount < 1) {
                ShowMessage("Please say something or upload a file.", 0, false, false);
                $("#<%=txtContent.ClientID%>").focus();
                return false;
            }

            if (descr == _placeholder)
            {
                descr = "";
            }

            $("#<%=btnAssign.ClientID %>").hide();
            $.ajax({
                type: "post",
                url: "/Do/DoAddFeedBackHandler.ashx",
                data: {
                    tid: '<%= TicketID %>',
                    projectId: '<%=ProjectID %>',
                    title: title,
                    ckIsPublic: ckIsPublic,
                    chkIsWaitClientFeedback: chkIsWaitClientFeedback,
                    chkIsWaitPMFeedback: chkIsWaitPMFeedback,
                    descr: descr,
                    imageList: imageList,
                    imageSizeList: imageSizeList
                },
                success: function (result) {
                    if (result != "Feedback add success.") {
                        $("#<%=btnAssign.ClientID %>").show();
                    }
                    ShowMessageAndJumpToAnchor(result, true, "Feedback");
                }
            });
        });
        var _placeholder = "Enter your message...";
        content.val(_placeholder);
        content.focus(function () {
            if (content.val() == _placeholder) {
                content.val("");
            }
        }).blur(function () {
            if (content.val().replace(" ", "").length < 1) {
                content.val(_placeholder);
            }
        });
    });

    function checkPublic(o) {
        if ($(o).attr('checked')) {
            $("#<%=ckIsPublic.ClientID %>").attr('checked', true);
            $("#<%=ckIsPublic.ClientID %>").attr('disabled', true);
        }
        else {
            $("#<%=ckIsPublic.ClientID %>").attr('disabled', false);
        }
    }
</script>
