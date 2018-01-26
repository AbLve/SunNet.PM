<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpenICalendar.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.OpenICalendar" %>

<script type="text/javascript">
        function OpenICalendar() {
            ShowIFrame("iCalendar.aspx", 870, 450, true, "iCalendar");
        }
</script>

<span onclick="OpenICalendar();"><a href="#">
    <img src="/icons/08.gif" border="0" align="absmiddle" />
    iCalendar </a></span>