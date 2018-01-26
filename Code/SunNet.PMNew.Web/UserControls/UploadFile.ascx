<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadFile.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.UploadFile" %>
<link rel="stylesheet" type="text/css" href="/Scripts/Upload/uploadify.css" />

<script type="text/javascript">
var demolist<%=hidKey.Value %>;
    $(function() {
        demolist<%=hidKey.Value %>=$("#demolist<%=hidKey.Value %>");
    
        try {
              var iFlash = null;
            var version = null;
            var isIE = navigator.userAgent.toLowerCase().indexOf("msie") != -1
            if (isIE) {
                //for IE

                if (window.ActiveXObject) {
                    var control = null;
                    try {
                        control = new ActiveXObject('ShockwaveFlash.ShockwaveFlash');
                    } catch (e) {
                        iFlash = false;
                    }
                    if (control) {
                        iFlash = true;
                        version = control.GetVariable('$version').substring(4);
                        version = version.split(',');
                        version = parseFloat(version[0] + '.' + version[1]);
                    }
                }
            } else {
                //for other
                if (navigator.plugins) {
                    for (var i = 0; i < navigator.plugins.length; i++) {
                        if (navigator.plugins[i].name.toLowerCase().indexOf("shockwave flash") >= 0) {
                            iFlash = true;
                            version = navigator.plugins[i].description.substring(navigator.plugins[i].description.toLowerCase().lastIndexOf("Flash ") + 6, navigator.plugins[i].description.length);
                        }
                    }
                }
            }
            if (iFlash) {
                $("#downloadFlashMsg<%=hidKey.Value %>").css('display', "none");
            } else {
               $("#downloadFlashMsg<%=hidKey.Value %>").css('display', null).css("background-color","#EEE685");
                                    
            }
                
        
            $('#file_upload<%=hidKey.Value %>').uploadify({
                'auto': false,
                'buttonClass':'btnthreeFile',
                'width':'75',
                'height':'16',
                'fileTypeExts' : '*.gif; *.jpg; *.png; *.doc; *.docx; *.xls; *.xlsx; *.jpeg; *.pdf; *.txt;*.zip; *.rar;*.xlsx ;*.xls',
                'formData': { 'pid': $("#<% = ProjectName %>").val() },
                'swf': '/Scripts/Upload/uploadify.swf',
                'uploader': '/Scripts/Upload/RemoteHandlers/Upload.ashx',
                'onUploadStart': function(file) {
                    $("#file_upload<%=hidKey.Value %>").uploadify("settings", "formData", { "pid": $("#<% = ProjectName %>").val() });
                },
                'onUploadComplete': function(file) { 
                    if($("#<% = ProjectName %>").val().length>0){
                      SetLi_UpdateFiles<%=hidKey.Value %>(file); 
                    }
                },
                'onUploadError': function(file, errorCode, errorMsg, errorString) {
                    alert('The file ' + file.name + ' could not be uploaded: please select a project.' );
                }
            });
        }
        catch (e)
        { }
    });
    function SetLi_UpdateFiles<%=hidKey.Value %>(objFile) {
        var iNextLiIndex = parseInt($("#<%=hd_Index.ClientID%>").val(), 10);
        var sLi = "<li id=\"file-" + iNextLiIndex.toString() + "\" class=\"file\">";
        sLi += "<span><b>" + objFile.name + "</b></span>";
        sLi += "<span class=\"file-size\"><font color=\"#666666\">" + GetFileSize(objFile.size) + "</font></span>";
        sLi += " <a href=\"javascript:DeleteFile<%=hidKey.Value %>(1,'file-" + iNextLiIndex.toString() + "','','" + objFile.name + "');\"><img src=\"/Scripts/Upload/images/error_fuck.png\" width='12' height='12' border=\"0\" /></a></li>";
        $("#<%=hd_Index.ClientID%>").val(iNextLiIndex + 1);
        $(sLi).appendTo(demolist<%=hidKey.Value %>);

        if ($("#<%=hd_UpdateFileValue.ClientID%>").val() != "") {
            $("#<%=hd_UpdateFileValue.ClientID%>").val($("#<%=hd_UpdateFileValue.ClientID%>").val() + "#");
        }
        $("#<%=hd_UpdateFileValue.ClientID%>").val($("#<%=hd_UpdateFileValue.ClientID%>").val() + "0" + "||" + objFile.name + "||" + GetFileSize(objFile.size) + "||" + objFile.filePath + "||" + "file-" + iNextLiIndex.toString());
    }

    function GetFileSize(iBytes) {
        var iFileKB = iBytes / 1024;
        iFileKB = SizeFormat(iFileKB.toString());
        return iFileKB + " KB";
    }

    function SizeFormat(s) {
        s = s.replace(/^(\d*)$/, "$1.");
        s = (s + "00").replace(/(\d*\.\d\d)\d*/, "$1");
        s = s.replace(".", ",");
        var re = /(\d)(\d{3},)/;
        while (re.test(s))
            s = s.replace(re, "$1,$2");
        s = s.replace(/,(\d\d)$/, ".$1");
        return s.replace(/^\./, "0.")
    }

    function DeleteFile<%=hidKey.Value %>(b, liId, sDeleteId, sDeleteFileName) {

        //remove from list
        $("#" + liId).remove();
        //delete
        //DeleteUpdateFileValue(liId);
        switch (b) {
            case 0: //only delete list

                break;
            case 1: //not only delete list,Also set value to text
                if ($("#<%=hd_DeleteFileValue.ClientID%>").val() != "") {
                    $("#<%=hd_DeleteFileValue.ClientID%>").val($("#<%=hd_DeleteFileValue.ClientID%>").val() + "#");
                }
                $("#<%=hd_DeleteFileValue.ClientID%>").val($("#<%=hd_DeleteFileValue.ClientID%>").val() + liId + "|" + sDeleteFileName);
                break;
        }
    }
    function DeleteUpdateFileValue(sDeleteValue) {
        // get upload list & hidden value
        var sUpdateValue = $("#<%=hd_UpdateFileValue.ClientID%>").val();

        //delete file name and element position
        var iFindIndex = sUpdateValue.lastIndexOf(sDeleteValue);

        var iStartIndex = sUpdateValue.substring(0, iFindIndex).lastIndexOf("#");

        var iEndIndex = iFindIndex + sDeleteValue.length - 1;

        if (iStartIndex == -1) {
            var sNewRightValue = $("#<%=hd_UpdateFileValue.ClientID%>").val().substring(iEndIndex + 2);
            $("#<%=hd_UpdateFileValue.ClientID%>").val(sNewRightValue);
        }
        else {
            if ((iEndIndex + 1) == sUpdateValue.length) {
                $("#<%=hd_UpdateFileValue.ClientID%>").val($("#<%=hd_UpdateFileValue.ClientID%>").val().substring(0, iStartIndex));
            }
            else {
                var sNewLeftValue = $("#<%=hd_UpdateFileValue.ClientID%>").val().substring(0, iStartIndex);
                var sNewRightValue = $("#<%=hd_UpdateFileValue.ClientID%>").val().substring(iEndIndex + 1);
                $("#<%=hd_UpdateFileValue.ClientID%>").val(sNewLeftValue + sNewRightValue);
            }
        }
    }

    function enabledFileDeleteImg(b) {
        if (b) {
            demolist<%=hidKey.Value %>.find("a").css("display", "inline");
        }
        else {
            demolist<%=hidKey.Value %>.find("a").attr("style", "display:none");
        }
    }

    function yhLoad() {
        demolist<%=hidKey.Value %>.html($get("hd_FileList").value);
    }
   function deleteImgWhenStatusDraft(s) {
        $.ajax({
            type: "post",
            data: {
                fileid: s
            },
            url: "/Do/DoRemoveFileHandler.ashx",
            success: function(result) {
                ShowMessage(result, 0, true, false);
                window.location.reload();
            }
        });
    }
</script>

<asp:HiddenField ID="hd_Index" runat="server" Value="0" />
<asp:HiddenField ID="hd_UpdateFileValue" runat="server" />
<asp:HiddenField ID="hd_DeleteFileValue" runat="server" />
<asp:HiddenField ID="hidKey" runat="server" />
<div runat="server" id="divFiles" class="files">
    <ul id="demolist<%=hidKey.Value %>">
    </ul>
</div>
<div id="queue">
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="4">
    <tr>
        <div id="downloadFlashMsg<%=hidKey.Value %>" style="height: 20px; width: 270px; font: 14px;
            text-align: center;">
            <a href='http://www.adobe.com/downloads/' target='_blank'>download flash here</a>
        </div>
        <td width="300">
            <input id="file_upload<%=hidKey.Value %>" name="file_upload" type="file" multiple="true" />
        </td>
        <td valign="top">
            <div runat="server" id="divUploadFile" style="margin-top: 1px;">
                <input name="Submit" type="submit" class="btnthree" value="Upload file" onclick="javascript:$('#file_upload<%=hidKey.Value %>').uploadify('upload','*');return false;">
            </div>
        </td>
    </tr>
</table>
