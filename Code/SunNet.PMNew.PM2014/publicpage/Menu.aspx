<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="SunNet.PMNew.PM2014.publicpage.Menu" %>

<%@ Register Src="../UserControls/ClientMenu.ascx" TagName="ClientMenu" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/SunnetMenu.ascx" TagPrefix="uc1" TagName="SunnetMenu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%=IEEmulator %>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Menu</title>
    <link href="/Content/styles/public.css" rel="stylesheet" />
    <style type="text/css">
        html {
            overflow-x: hidden;
            overflow-y: hidden;
        }
    </style>
    <script src="/Scripts/jquery.js"></script>
</head>
<body style="margin: 0px;">
    <form id="form1" runat="server">
        <uc1:ClientMenu ID="ClientMenu1" runat="server" Target="_top" Visible="false" />
        <uc1:SunnetMenu runat="server" ParentID="1" ID="SunnetMenu1" Target="_top" Visible="false" />
    </form>

    <script type="text/javascript">
        function Loaded() {
            window.top.$("iframe:first").css("height", jQuery("html").outerHeight()).css("min-width", 1260);
        }
        Loaded();
    </script>
</body>
</html>
