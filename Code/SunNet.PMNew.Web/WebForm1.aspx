<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SunNet.PMNew.Web.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Scripts/jquery-1.8.0.min.js" type="text/javascript"></script>

    <style type="text/css">
        .testTd
        {
            white-space: nowrap;
            overflow: hidden;
            word-break: keep-all;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="1" cellpadding="1" cellspacing="1">
            <tr>
                <td id="left" width="200px;" style="testtd">
                    <ul id="testul">
                        <li>this is test td jinbian wo yao cishi</li>
                        <li>this is test td jinbian wo yao cishi</li>
                        <li>this is test td jinbian wo yao cishi</li>
                        <li>this is test td jinbian wo yao cishi</li>
                        <li>this is test td jinbian wo yao cishi</li>
                        <li>this is test td jinbian wo yao cishi</li>
                        <li>this is test td jinbian wo yao cishi</li>
                    </ul>
                </td>
                <td id="right">
                    <a href="javascript:void(0);" onclick="test();">right</a> <a href="javascript:void(0);"
                        onclick="test2();">right2</a>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        $(document).ready(function() {
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
                        alert("FLASH版本号：" + version)
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
                alert(version)
            } else {
                alert("您的浏览器未安装FLASH插件");
            }
        });
        function test() {
            jQuery("#left").animate({ "width": "10px" }, 1000);
        }

        function test2() {
            jQuery("#left").animate({ "width": "200px" }, 1000);
        }
    </script>

    </form>
</body>
</html>
