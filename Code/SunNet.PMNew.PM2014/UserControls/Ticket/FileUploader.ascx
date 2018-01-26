<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUploader.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.FileUploader" %>
<script type="text/javascript">
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
</script>
