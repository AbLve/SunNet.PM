<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddFeedBack.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.AddFeedBack" %>
<%@ Register Src="UploadFile.ascx" TagName="UploadFile" TagPrefix="uc1" %>
<link href="/styles/openwindow.css" rel="stylesheet" type="text/css" />
<link href="/styles/form.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">

    $(document).ready(function () {

        $("#<%=btnAssign.ClientID%>").click(function () {

            var title = $("#<%=txtTitle.ClientID%>").attr("value");
            var descr = $("#<%=txtContent.ClientID%>").attr("value");
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
            if (title.length < 1 && descr.length < 1 && fileCount < 1) {
                ShowMessage("Please say something or upload a file.", 0, false, false);
                $("#<%=txtTitle.ClientID%>").focus();
                return false;
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
                    ClientReplyFeedbackID: '<% =ClientReplyFeedbackID %>',
                    PMReplyFeedbackID: '<% =PMReplyFeedbackID %>',
                    descr: descr,
                    imageList: imageList,
                    imageSizeList: imageSizeList
                },
                success: function (result) {
                    if (result != "Feedback add success.") {
                        $("#<%=btnAssign.ClientID %>").show();
                    }
                    ShowMessage(result, 0, true, true);
                }
            });
        });

        //clear
        $("#<%=btnCancle.ClientID%>").click(function () {

            $("#<%=txtTitle.ClientID%>").attr("value", '')
            $("#<%=txtContent.ClientID%>").attr("value", '')
            $("#<%=ckIsPublic.ClientID%>").attr('checked', false);
            $("#demo-list li").remove("li[id = file-NaN]");
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

<div class="owmainrightBoxtwo">
    <asp:HiddenField ID="hd_Project" runat="server" />
    <table width="90%" border="0" align="center" cellpadding="5" cellspacing="0" class="opendivTable">
        <tr>
            <th width="23%">Title:
            </th>
            <td width="77%">
                <input runat="server" id="txtTitle" type="text" class="input98p" />
                <asp:HiddenField ID="hfPublic" runat="server" Value="false" />
            </td>
        </tr>
        <tr id="trOthers" runat="server">
            <th width="23%">Others:
            </th>
            <td width="10%">
                <asp:CheckBox ID="ckIsPublic" runat="server" Text=" Is visible to client" />
                <asp:CheckBox ID="chkIsWaitClientFeedback" runat="server" Text=" Is wait client feedback"
                    onclick="checkPublic(this);" />
                <asp:CheckBox ID="chkIsWaitPMFeedback" runat="server" Text=" Is wait PM feedback" />
            </td>
        </tr>
        <tr runat="server" id="trOriDesc" visible="false">
            <th valign="top">Original Description:
            </th>
            <td>
                <textarea runat="server" id="txtOriginalContent" rows="4" class="input98p"></textarea>
            </td>
        </tr>
        <tr runat="server" id="trOriFile" visible="false">
            <th valign="top">Original Files:
            </th>
            <td>
                <label id="lblFiles" runat="server">
                </label>
            </td>
        </tr>
        <tr runat="server" id="trOriDate" visible="false">
            <th valign="top">Original DateTime:
            </th>
            <td>
                <label id="lblDate" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <th valign="top">Description:
            </th>
            <td>
                <textarea runat="server" id="txtContent" rows="4" class="input98p"></textarea>
            </td>
        </tr>
        <tr>
            <th>Files:
            </th>
            <td>
                <asp:Literal ID="lilImgList" runat="server"></asp:Literal>
                <uc1:UploadFile ID="UploadFile1" runat="server" />
            </td>
        </tr>
    </table>
</div>
<div class="btnBoxone">
    <input id="btnAssign" runat="server" type="button" class="btnfive" value="Save" />
    <input id="btnCancle" runat="server" type="button" class="btnfive" value="Clear" />
</div>
