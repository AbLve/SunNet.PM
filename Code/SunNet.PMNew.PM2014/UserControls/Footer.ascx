<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.Footer" %>
<div class="footerBox">
    <div class="footerBox_left">
        <a href="/About/Faqs.aspx?returnurl=<%=this.GlobalPage.ReturnUrl %>">FAQ</a> <span>|</span>
        <a href="/About/Survey.aspx?returnurl=<%=this.GlobalPage.ReturnUrl %>">Survey</a><span>|</span>
        <a href="/About/ContactUs.aspx?returnurl=<%=this.GlobalPage.ReturnUrl %>">Contact Us</a>
    </div>
    <div class="footerBox_right">Copyright &copy; <%=DateTime.Now.Year %> <a href="http://www.sunnet.us" target="_blank">SunNet Solutions</a>. </div>
</div>
