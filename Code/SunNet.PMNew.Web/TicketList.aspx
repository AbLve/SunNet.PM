<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketList.aspx.cs" Inherits="SunNet.PMNew.Web.TicketList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>SunNet Project Management Application</title>
    <link href="/Styles/global.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <body>
        <div class="mainTop">
            <div class="mainTop_left">
                <img src="/images/logomain.jpg" /></div>
            <div class="mainTop_right">
                <span class="mainTop_rightUser">User: <strong>Suki</strong></span><a href="#">FAQ</a>|<a
                    href="#">Survey</a>|<a href="#">Logout</a></div>
        </div>
        <div class="topMenu">
            <div class="topMenu_left">
                <ul>
                    <li class="sepline">
                        <img src="/images/topsep.jpg" /></li>
                    <li class="currenttop"><a href="#">Tickets</a></li>
                    <li class="sepline">
                        <img src="/images/topsep.jpg" /></li>
                    <li><a href="#">Documents</a></li>
                    <li class="sepline">
                        <img src="/images/topsep.jpg" /></li>
                    <li><a href="#">Reports</a></li>
                    <li class="sepline">
                        <img src="/images/topsep.jpg" /></li>
                    <li><a href="#">Projects</a></li>
                    <li class="sepline">
                        <img src="/images/topsep.jpg" /></li>
                    <li><a href="#">My Company</a></li>
                    <li class="sepline">
                        <img src="/images/topsep.jpg" /></li></ul>
            </div>
            <div class="topMenu_right">
                <img src="/icons/04.gif" align="absmiddle" />My Categories
            </div>
        </div>
        <div class="topTitle">
            <div class="topTitle_left">
                Tickets</div>
            <div class="topTitle_right">
                Dashboard</div>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="mainBox_left">
                    <ul class="leftmenu">
                        <li class="currentleft"><a href="#">Dashboard</a></li>
                        <li>Schedules</li>
                        <li class="sub">Tickets</li>
                        <li class="sub">My Categories</li>
                        <li class="sub">Timesheet</li>
                    </ul>
                </td>
                <td class="mainBox_right">
                    <div class="mainrightBox">
                        <div class="onlistBox">
                            <img src="/icons/01.gif" align="absmiddle" />
                            <span class="onlistText">Due Date is today</span><img src="/icons/03.gif" align="absmiddle" />
                            <span class="onlistText">Due day is 3 days before today</span><img src="/icons/02.gif"
                                align="absmiddle" />
                            <span class="onlistText">Passed Due Date</span><strong>Priority:</strong> The larger
                            the value, the higher the priority.</div>
                        <asp:Repeater ID="rptTicketsList" runat="server">
                            <HeaderTemplate>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listone">
                                    <tr>
                                        <td colspan="8" class="listtopTitle">
                                            <div class="listtopTitle_left">
                                                <%# Eval("Priority")%></div>
                                            <div class="listtopTitle_right">
                                                <a href="#">More...</a></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="30" class="listsubTitle">
                                            Priority
                                        </td>
                                        <td width="30" class="listsubTitle">
                                            &nbsp;
                                        </td>
                                        <td width="90" class="listsubTitle">
                                            Ticket Code
                                        </td>
                                        <td class="listsubTitle">
                                            Title
                                        </td>
                                        <td class="listsubTitle">
                                            Description
                                        </td>
                                        <td class="listsubTitle">
                                            Status
                                        </td>
                                        <td class="listsubTitle">
                                            Due Date
                                        </td>
                                        <td width="30" class="listsubTitle">
                                            Action
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="listrowone">
                                        <%# Eval("TicketId")%>
                                    </td>
                                    <td class="listrowone">
                                        <%# Eval("Priority")%>
                                    </td>
                                    <td class="listrowone">
                                        <%# Eval("Priority")%>
                                    </td>
                                    <td class="listrowone">
                                        <%# Eval("Priority")%>
                                    </td>
                                    <td class="listrowone">
                                        <%# Eval("Priority")%>
                                    </td>
                                    <td class="listrowone">
                                        <%# Eval("Priority")%>
                                    </td>
                                    <td class="listrowone">
                                        <%# Eval("Priority")%>
                                    </td>
                                    <td class="listrowone">
                                        <asp:LinkButton ID="lbDetailId" runat="server" CommandArgument='<%# Eval("TicketId") %>'
                                            PostBackUrl='<%# "~/Detail.aspx?id="+Eval("TicketId").ToString() %>'>
                                            <img src="/icons/05.gif" alt="edit" /> 
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listone">
                            <tr>
                                <td colspan="8" class="listtopTitle">
                                    <div class="listtopTitle_left">
                                        Allied</div>
                                    <div class="listtopTitle_right">
                                        <a href="#">More...</a></div>
                                </td>
                            </tr>
                            <tr>
                                <td width="30" class="listsubTitle">
                                    Priority
                                </td>
                                <td width="30" class="listsubTitle">
                                    &nbsp;
                                </td>
                                <td width="90" class="listsubTitle">
                                    Ticket Code
                                </td>
                                <td class="listsubTitle">
                                    Title
                                </td>
                                <td class="listsubTitle">
                                    Description
                                </td>
                                <td class="listsubTitle">
                                    Status
                                </td>
                                <td class="listsubTitle">
                                    Due Date
                                </td>
                                <td width="30" class="listsubTitle">
                                    Action
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowone">
                                    <strong>10</strong>
                                </td>
                                <td class="listrowone">
                                    <img src="/icons/01.gif" />
                                </td>
                                <td class="listrowone">
                                    B815
                                </td>
                                <td class="listrowone">
                                    Reports &gt; Monitoring/Uonitor
                                </td>
                                <td class="listrowone">
                                    Reports &gt; Monitoring/UonitorReports &gt; Monitoring/Uonitor
                                </td>
                                <td class="listrowone">
                                    Ready for Testing
                                </td>
                                <td class="listrowone">
                                    09/25/2012
                                </td>
                                <td class="listrowone">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowtwo">
                                    <strong>9</strong>
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowone">
                                    <strong>8</strong>
                                </td>
                                <td class="listrowone">
                                    <img src="/icons/03.gif" />
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowtwo">
                                    <strong>7</strong>
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowone">
                                    <strong>6</strong>
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowtwo">
                                    <strong>5</strong>
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowone">
                                    <strong>4</strong>
                                </td>
                                <td class="listrowone">
                                    <img src="/icons/02.gif" />
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowtwo">
                                    <strong>3</strong>
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowone">
                                    <strong>2</strong>
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    &nbsp;
                                </td>
                                <td class="listrowone">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="listrowtwo">
                                    <strong>1</strong>
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    &nbsp;
                                </td>
                                <td class="listrowtwo">
                                    <img src="/icons/05.gif" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <div class="footer">
            Copyright &copy; 2014 SunNet Solutions.</div>
    </body>
    </form>
</body>
</html>
