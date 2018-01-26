<%@ Page Title="Dashboard" Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs"
    MasterPageFile="~/Sunnet/Main.Master" Inherits="SunNet.PMNew.Web.Sunnet.Clients.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/global.css" />
    <script type="text/javascript">

        //download
        function download(projectId, id) {
            jQuery("#ifrdownload").prop("src", "/document/DocManagement/DocHome/Download?projectId=" + projectId + "&id=" + id);
        }

    </script>
    <style type="text/css">
        .eventbtn3 {
            background-image: url("/images/but_bg2.png");
            border: 1px solid #C2C2C2;
            border-radius: 3px;
            color: #666666;
            cursor: pointer;
            font-size: 12px;
            font-weight: bold;
            margin-right: 5px;
            padding: 1px 10px;
        }

            .eventbtn3:hover {
                background-image: url("/images/homelog_btnbgover.png");
                border: 1px solid #666666;
                color: #FFFFFF;
            }

        .fade {
            opacity: 0;
            -webkit-transition: opacity 0.15s linear;
            transition: opacity 0.15s linear;
        }

            .fade.in {
                opacity: 1;
            }


        .popover {
            position: absolute;
            top: 0;
            left: 0;
            z-index: 1010;
            display: none;
            max-width: 276px;
            padding: 1px;
            text-align: left;
            background-color: #EDEFF5;
            background-clip: padding-box;
            border: 1px solid #CCCCCC;
            border-radius: 6px;
            -webkit-box-shadow: 0 5px 10px rgba(0, 0, 0, 0.2);
            box-shadow: 0 0 10px #939393;
            white-space: normal;
            max-width: 300px;
            min-width: 240px;
        }

            .popover.top {
                margin-top: -10px;
            }

            .popover.right {
                margin-left: 10px;
            }

            .popover.bottom {
                margin-top: 10px;
            }

            .popover.left {
                margin-left: -10px;
            }

        .popover-title {
            margin: 0;
            padding: 8px 14px;
            font-size: 16px;
            font-weight: bold;
            line-height: 20px;
            background-color: #EDEFF5;
            border-bottom: 1px solid #ebebeb;
            border-radius: 5px 5px 0 0;
            height: auto;
        }

        .popover-content {
            font-size: 12px;
            padding: 9px 14px;
        }


        .popover .arrow, .popover .arrow:after {
            position: absolute;
            display: block;
            width: 0;
            height: 0;
            border-color: transparent;
            border-style: solid;
        }

        .popover .arrow {
            border-width: 11px;
        }

            .popover .arrow:after {
                border-width: 10px;
                content: "";
            }

        .popover.top .arrow {
            left: 50%;
            margin-left: -11px;
            border-bottom-width: 0;
            border-top-color: #999999;
            border-top-color: rgba(0, 0, 0, 0.25);
            bottom: -11px;
        }

            .popover.top .arrow:after {
                content: " ";
                bottom: 1px;
                margin-left: -10px;
                border-bottom-width: 0;
                border-top-color: #ffffff;
            }

        .popover.right .arrow {
            top: 50%;
            left: -11px;
            margin-top: -11px;
            border-left-width: 0;
            border-right-color: #999999;
            border-right-color: rgba(0, 0, 0, 0.25);
        }

            .popover.right .arrow:after {
                content: " ";
                left: 1px;
                bottom: -10px;
                border-left-width: 0;
                border-right-color: #EDEFF5;
            }

        .popover.bottom .arrow {
            left: 50%;
            margin-left: -11px;
            border-top-width: 0;
            border-bottom-color: #999999;
            border-bottom-color: rgba(0, 0, 0, 0.25);
            top: -11px;
        }

            .popover.bottom .arrow:after {
                content: " ";
                top: 1px;
                margin-left: -10px;
                border-top-width: 0;
                border-bottom-color: #ffffff;
            }

        .popover.left .arrow {
            top: 50%;
            right: -11px;
            margin-top: -11px;
            border-right-width: 0;
            border-left-color: #999999;
            border-left-color: rgba(0, 0, 0, 0.25);
        }

            .popover.left .arrow:after {
                content: " ";
                right: 1px;
                border-right-width: 0;
                border-left-color: #ffffff;
                bottom: -10px;
            }

        .popover-content .popover-action {
            border-top: 1px solid #CCCCCC;
            height: 15px;
            padding-top: 8px;
            text-align: right;
        }


        .modal.in .modal-dialog {
            transform: translate(0px, 0px);
        }

        .modal.fade .modal-dialog {
            transform: translate(0px, -25%);
            transition: transform 0.3s ease-out 0s;
        }

        .modal-dialog {
            margin: 105px auto 30px;
            width: 600px;
            position: relative;
            z-index: 1050;
        }

        *, *:before, *:after {
        }

        .modal {
            bottom: 0;
            display: none;
            left: 0;
            overflow-x: auto;
            overflow-y: scroll;
            position: fixed;
            right: 0;
            top: 0;
            z-index: 1040;
        }

        .modal-open .fixedContainer {
            position: fixed !important;
        }

        .fixedContainer {
            width: 100%;
        }


        .modal-backdrop {
            background-color: #000000;
            bottom: 0;
            left: 0;
            position: fixed;
            right: 0;
            top: 0;
            z-index: 1030;
        }

            .modal-backdrop.in {
                opacity: 0.5;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <table width="99%" border="0" cellpadding="0" cellspacing="0" style="min-width: 900px; table-layout: fixed; margin-top: -4px; line-height: 2;">
        <tr>
            <td width="65%">Dashboard
            </td>

        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td vertical-align="top" style="border-bottom: 1px solid rgb(129, 186, 232);">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pdashboardMain">
                    <tr>
                        <td width="10">&nbsp;
                        </td>
                        <td width="49%" class="pdashboardBox">
                            <div class="pdashboardTitle">
                                Tracking Tickets
                            </div>
                            <div class="pdashboardIconbox">
                                <ul class="pdashboardItem">
                                    <%if (UserInfo.Role == SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
                                      {%>
                                    <a href="/sunnet/Clients/ListTicketwaitProcessing.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <div class="pdreddot">
                                                    <asp:Literal runat="server" ID="ltrlWaitforYou"></asp:Literal>
                                                </div>
                                                <img src="/images/pd_response.png" width="60" height="60" />
                                            </div>
                                            <br />
                                            Waiting for Response<br>
                                        </li>
                                    </a>
                                    <%} %>
                                    <a href="/sunnet/Clients/AddBug.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <img src="/images/pd_bug.png" width="60" height="60" />
                                            </div>
                                            <br>
                                            Submit a Bug</li>
                                    </a><a href="/sunnet/Clients/AddRequest.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <img src="/images/pd_request.png" />
                                            </div>
                                            <br>
                                            Submit a Request</li>
                                    </a><a href="/sunnet/Clients/AddRisk.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <img src="/images/pd_risk.png">
                                            </div>
                                            <br>
                                            Submit a Risk</li>
                                    </a><a href="/sunnet/Clients/AddIssue.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <img src="/images/pd_issue.png">
                                            </div>
                                            <br>
                                            Submit an Issue</li>
                                    </a><a href="/sunnet/Clients/AddChange.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <img src="/images/pd_change.png">
                                            </div>
                                            <br>
                                            Submit a Change</li>
                                    </a><a href="/sunnet/Clients/ListTicket.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <div class="pdreddot">
                                                    <asp:Literal runat="server" ID="ltrlViewTicketsProgress"></asp:Literal>
                                                </div>
                                                <img src="/images/pd_ongoing.png">
                                            </div>
                                            <br>
                                            Ongoing Tickets<br>
                                        </li>
                                    </a><a href="/sunnet/Clients/ListTicketDrafted.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <div class="pdreddot">
                                                    <asp:Literal runat="server" ID="ltrlDraftedTickets"></asp:Literal>
                                                </div>
                                                <img src="/images/pd_draft.png" width="60" height="60" border="0">
                                            </div>
                                            <br>
                                            Drafted Tickets<br>
                                        </li>
                                    </a><a href="/sunnet/Clients/ListTicketCompleted.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <div class="pdreddot">
                                                    <asp:Literal runat="server" ID="ltrlCompletedTickets"></asp:Literal>
                                                </div>
                                                <img src="/images/pd_completed.png" width="60" height="60" border="0">
                                            </div>
                                            <br />
                                            Completed Tickets<br />
                                        </li>
                                    </a><a href="/sunnet/Clients/ListTicketCancel.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <div class="pdreddot">
                                                    <asp:Literal runat="server" ID="ltrlCancelledTickets"></asp:Literal>
                                                </div>
                                                <img src="/images/pd_cancelled.png" width="60" height="60" border="0">
                                            </div>
                                            <br />
                                            Cancelled Tickets<br />
                                        </li>
                                    </a><a href="/sunnet/Clients/TicketReport.aspx">
                                        <li>
                                            <div class="pdashboardIcon">
                                                <div class="pdreddot">
                                                    <asp:Literal runat="server" ID="ltrlTicketReport"></asp:Literal>
                                                </div>
                                                <img src="/images/pd_reports.png">
                                            </div>
                                            <br />
                                            Tickets Report<br />
                                        </li>
                                    </a>
                                    <%--<a href="#">
                                            <li>
                                                <div class="pdashboardIcon">
                                                    <div class="pdreddot">
                                                        20</div>
                                                    <img src="/images/pm_enhancement.png" /></div>
                                                <br>
                                                SunNet PM System
                                                <br>
                                                Enhancement</li></a>--%>
                                </ul>
                            </div>
                        </td>
                        <td width="10">&nbsp;
                        </td>
                        <td width="49%" class="pdashboardBox">
                            <div class="pdashboardTitle">
                                <%--<a href="/sunnet/html/project_forums.html" target="_blank">Project Forums</a>--%>
                                <a href="/forums/login/pm/finish" style="text-decoration: underline;" target="targetDashboard">Project Forums >></a>
                            </div>
                            <div class="pdlistTop">
                                <img src="/Images/latesttopic.png" align="absmiddle" />
                                Latest Topics
                            </div>
                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="listtwo" id="tabLatestTopics">
                            </table>
                            <div class="pdlistTop">
                                <img src="/images/reply.png" align="absmiddle" />
                                Latest Reply
                            </div>
                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="listtwo" id="tabLatestReply">
                            </table>
                            <div class="pdlistTop">
                                <img src="/images/hottopic.png" align="absmiddle" />
                                Hot Topics
                            </div>
                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="listtwo" id="tabHotsTopics">
                            </table>
                        </td>
                        <td width="10">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td height="10" colspan="5"></td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td class="pdashboardBox">
                            <div class="pdashboardTitle">
                                <a  style="text-decoration: underline;" href="/sunnet/Events/EventsCalendar.aspx" target="targetDashboard">Upcoming Activities >></a>
                            </div>
                            <div class="activeBtnbox1">
                                <a href="javascript:void(0);" onclick="showEventCalendar()"><span class="activeBtn2"
                                    id="spanCalendar" style="margin-right: 7px;">Calendar</span></a>
                                <a href="javascript:void(0);" onclick="showEventList()"><span
                                        class="activeBtn1" id="spanList">List</span></a> 
                                
                                
                                <a href="javascript:void(0);" onclick="OpenAddModuleDialog('<% =DateTime.Now.ToString("MM/dd/yyyy") %>')"><span
                                            class="createeventBtn">
                                            <img src="/images/event.png" align="absmiddle" />
                                            Create Event</span></a>
                            </div>

                            <script type="text/javascript">
                                function showEventCalendar() {
                                    var spanCalendar = jQuery("#spanCalendar");
                                    var spanList = jQuery("#spanList");
                                    if (spanCalendar.hasClass("activeBtn1")) {
                                        spanCalendar.removeClass("activeBtn1").toggleClass("activeBtn2");
                                        jQuery("#divEventList").toggle();
                                        reloadEventCalendar();
                                    }
                                    if (spanList.hasClass("activeBtn2")) {
                                        spanList.removeClass("activeBtn2").toggleClass("activeBtn1");
                                        jQuery("#divEventCalendar").toggle();
                                    }
                                }

                                function showEventList() {
                                    var spanCalendar = jQuery("#spanCalendar");
                                    var spanList = jQuery("#spanList");
                                    if (spanCalendar.hasClass("activeBtn2")) {
                                        jQuery("#spanCalendar").removeClass("activeBtn2").toggleClass("activeBtn1");
                                        jQuery("#divEventList").toggle();
                                        InitEventList();
                                    }
                                    if (spanList.hasClass("activeBtn1")) {
                                        jQuery("#spanList").removeClass("activeBtn1").toggleClass("activeBtn2");
                                        jQuery("#divEventCalendar").toggle();
                                        InitEventList();
                                    }
                                }
                            </script>

                            <div class="pdlist" id="divEventList" style="display: none;">
                            </div>
                            <div class="pdcalendar" id="divEventCalendar" style="padding-top: 0;">
                                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="pdmonthlyviewBox" style="border-left-width: 0">
                                    <tbody>
                                        <tr>
                                            <script type="text/javascript">

                                                var tmpCalendarStart = "<table id='tb{month}' width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"pdmonthlyviewBox\" style=\"margin-left:1px;\">" +
                                                            "<tbody><tr><td><ul class=\"pdmonthlyviewweek\"><li>Sunday</li><li>Monday</li><li>Tuesday</li><li>Wednesday</li><li>Thursday</li><li>Friday</li><li>Saturday</li></ul> <ul class=\"pdmonthlyview\">";

                                                var tmpCalendarEnd = "</ul></td></tr></tbody></table>";
                                                var tmpCalendarLink = "<li id='liCalendarEvent{ID}'><a href=\"javascript:void(0);\"  data-container=\"body\" data-toggle=\"popover\" data-placement=\"right\"" +
                                                        " data-content=\"{Date}<br/>{Invite}<div class='popover-action' id='divCalendarInviteEvent{ID}'>{InviteEvent}<div>\" title=\"{Icon}{Name}\">";

                                                var tmpCalendarLink2 = "<li id='liCalendarEvent{ID}'><a href=\"javascript:void(0);\"  data-container=\"body\" data-toggle=\"popover\" data-placement=\"right\"" +
                                                        " data-content=\"{Date}<br/><br/><div class='popover-action'>{Edit}<div>\" title=\"{Icon}{Name}\">";
                                                var tmpEventIcon = "<img src='{icon}' style='width:24px; height:24px;' />";
                                                function builderLi(arr) {
                                                    var tmp = "<ul class=\"onscdTicket\">";
                                                    for (var i = 0; i < arr.length; i++) {
                                                        var tmpIcon = tmpEventIcon.replace("{icon}", arr[i].Icon);
                                                        if (arr[i].Invited) {
                                                            tmp += tmpCalendarLink.replace("{Name}", arr[i].Name).replace("{Icon}", tmpIcon).replace("{Date}", arr[i].date).replace("{Invite}", arr[i].FullName)
                                                            .replace(/{ID}/g, arr[i].ID) + arr[i].Title + "</a></li>";
                                                            if (arr[i].InviteStatus == 1)
                                                                tmp = tmp.replace("{InviteEvent}", "<span class='eventbtn3' onclick='JoinEvent(" + arr[i].ID + ");'>Join</span><span class='eventbtn3' onclick='Decline(" + arr[i].ID + ");'>Decline</span>");
                                                            else
                                                                tmp = tmp.replace("{InviteEvent}", "Agreed");
                                                        }
                                                        else {
                                                            var tmpEdit = "";
                                                            if (arr[i].IsEdit) {
                                                                tmpEdit = "<span class='eventbtn3' onclick='OpenEditSchdules(" + arr[i].ID + ",1);'>Edit</span>"
                                                            }
                                                            else {
                                                                tmpEdit = "<span class='eventbtn3' onclick='OpenEditSchdules(" + arr[i].ID + ",2);'>View</span>"
                                                            }
                                                            tmp += tmpCalendarLink2.replace("{Name}", arr[i].Name).replace("{Icon}", tmpIcon).replace("{Date}", arr[i].date).replace("{Edit}", tmpEdit).replace(/{ID}/g, arr[i].ID) + arr[i].Name + "</a></li>";
                                                        }
                                                    }
                                                    return tmp += "</ul>";
                                                }

                                                var calendarDate;
                                                var calendarMonth;
                                                function InitEvents(para, oldId, r) {
                                                    if (r) {
                                                        jQuery("#div" + oldId).remove();
                                                        jQuery("#tb" + oldId).remove();
                                                    }
                                                    $.getJSON("/Do/DoGetCalendarList.ashx?" + Math.random(), para, function (result) {
                                                        var d = result.list;
                                                        calendarDate = d[8].StrDate;
                                                        jQuery("#spnMonth").html(d[8].Month.replace("_", " "));
                                                        calendarMonth = d[8].Month;
                                                        var tmp = "";
                                                        for (var i = 0; i < d.length; i++) {
                                                            switch (d[i].Type) {
                                                                case 0:
                                                                    tmp += "<li class=\"pddateDisable\">" + d[i].Day + "</li>";
                                                                    break;
                                                                case 1:
                                                                    tmp += "<li><div class='pddateforclick' onclick=\"clickDayEventList('" + d[i].StrDate + "');\">" + d[i].Day + "</div>"
                                                                    if (d[i].list.length > 0)
                                                                        tmp += builderLi(d[i].list);
                                                                    tmp += "</li>";
                                                                    break;
                                                                case 2:
                                                                    tmp += "<li><div class='pddateforclick'  onclick=\"clickDayEventList('" + d[i].StrDate + "');\">" + d[i].Day + "</div><img src=\"/images/add1.png\"  style=\"cursor:pointer\"  align=\"right\" title='Create Event'  onclick=\"OpenAddModuleDialog('" + d[i].StrDate + "')\">";
                                                                    if (d[i].list.length > 0)
                                                                        tmp += builderLi(d[i].list);
                                                                    tmp += "</li>";
                                                                    break;
                                                                case 3:
                                                                    tmp += "<li class=\"pddateToday\"><a name='today'></a><div class='pddateforclick'  onclick=\"clickDayEventList('" + d[i].StrDate + "');\">" + d[i].Day + "</div><img src=\"/images/add1.png\" align=\"right\" style=\"cursor:pointer\"  title='Create Event'  onclick=\"OpenAddModuleDialog('" + d[i].StrDate + "')\">";
                                                                    if (d[i].list.length > 0)
                                                                        tmp += builderLi(d[i].list);
                                                                    tmp += "</li>";
                                                                    break;
                                                            }
                                                        }
                                                        $("#tdContext").append(tmpCalendarStart.replace("{month}", calendarMonth).replace("{month}", calendarMonth.replace("_", " ")).replace("{month}", calendarMonth) + tmp + tmpCalendarEnd);
                                                        shopPop();
                                                        if (r) {
                                                            if (r == 2) //today
                                                                location.href = "#today";
                                                        }
                                                        else {
                                                            if (oldId) {
                                                                jQuery("#div" + oldId).remove();
                                                                jQuery("#tb" + oldId).remove();
                                                            }
                                                        }
                                                    });
                                                }

                                                InitEvents({
                                                    'userID': '<%=UserInfo.UserID%>'
                                                });

                                                function shopPop() {
                                                    jQuery("#tdContext").find("[data-toggle='popover']").popover({
                                                        html: true,
                                                        trigger: "hover",
                                                        delay: { show: 0, hide: 500 },
                                                        placement: function (target, source) {
                                                            var popoverObj = this;
                                                            var $day = jQuery(source).closest("ul.pdmonthlyview>li");
                                                            if ($day.index() % 7 == 5 || $day.index() % 7 == 6) {
                                                                return "left";
                                                            }
                                                            else {
                                                                return "right";
                                                            }
                                                        }
                                                    });
                                                };

                                                function backClick(y) {
                                                    InitEvents({ "d": calendarDate, "t": 2, "y": y, "dashboard": 1 }, calendarMonth);
                                                }

                                                function forwardClick(y) {
                                                    InitEvents({ "d": calendarDate, "t": 1, "y": y, "dashboard": 1 }, calendarMonth);
                                                }

                                                function gotoToday() {
                                                    if (calendarMonth == '<%=DateTime.Now.ToString("MMMM_yyyy")%>')
                                                        location.href = "#today"
                                                    else {
                                                        InitEvents({}, calendarMonth, 2);
                                                    }
                                                }
                                            </script>
                                            <td id="tdContext">
                                                <div class="calendartop" style="border-left-width: 0px; border-right-width: 0px; margin-bottom: 0; border-top-width: 0;">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="13%">&nbsp;
                                                            </td>
                                                            <td width="74%">
                                                                <table border="0" align="center" cellpadding="0" cellspacing="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td width="20" align="center">
                                                                                <span class="calendartopArrow" onclick="backClick(1)" title="Previous Year">
                                                                                    <img src="/images/arrowy_cleft.png" width="16" height="9" alt="Previous Year" /></span>
                                                                            </td>
                                                                            <td width="20" align="center">
                                                                                <span class="calendartopArrow" onclick="backClick(0)" title="Previous Month">
                                                                                    <img src="/images/arrow_cleft.png" width="9" height="9" alt="Previous Month" /></span>
                                                                            </td>
                                                                            <td width="120" align="center">
                                                                                <span id="spnMonth"><%=DateTime.Now.ToString("MMMM yyyy") %></span>
                                                                            </td>
                                                                            <td width="20" align="center">
                                                                                <span class="calendartopArrow" onclick="forwardClick(0);" title="Next Month">
                                                                                    <img src="/images/arrow_cright.png" width="9" height="9" alt="Next Month" /></span>
                                                                            </td>
                                                                            <td width="20" align="center">
                                                                                <span class="calendartopArrow" onclick="forwardClick(1)" title="Next Year">
                                                                                    <img src="/images/arrowy_cright.png" width="16" height="9" alt="Next Year" /></span>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                            <td width="13%"></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>

                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td class="pdashboardBox">
                            <div class="pdashboardTitle">
                                <a href="/document/DocManagement/DocHome/Index" style="text-decoration: underline;" target="targetDashboard">Documents >></a>
                            </div>
                            <table id="tbDocument" width="100%" cellspacing="0" cellpadding="0" border="0" class="listtwo">
                                <tr class="listsubTitle">
                                    <td>Project Name</td>
                                    <td>File</td>
                                    <td>Created By</td>
                                    <td style="width: 90px">Upload On</td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
                <div class="dashboardBox">
                    <ul class="dashboardItem">
                    </ul>
                </div>
            </td>
        </tr>
    </table>
    <div class="modal fade" id="divTopInviteEventList">
    </div>
    <div class="modal fade" id="divDayInviteEventList" data-refresh="true">
    </div>
    <iframe id="ifrdownload" frameborder="0" height="0" width="0" scrolling="no" style="position: fixed; left: -50px; visibility: hidden;"></iframe>

    <script type="text/javascript">
        function OpenAddModuleDialog(day, pid) {
            var result = ShowIFrame("/Sunnet/Events/AddEvent.aspx?Date=" + day, 460, 470, true, "Add Schedules");
            if (!result) {
                //在这里List和Calendar会共用，所以要做一下区分。
                if (jQuery("#spanList").hasClass("activeBtn1")) {
                    reloadEventCalendar();
                }
                else {
                    InitEventList();
                }
            }
        }

        function reloadEventCalendar() {
            var $tdContent = $('#tdContext')
            $('#tdContext>table').remove();
            $tdContent.css('display', 'none');
            InitEvents({ "d": calendarDate, "t": 0, "dashboard": 1 }, calendarMonth, true);
            $tdContent.css('display', 'block');
        }

        function OpenEditSchdules(id, canEdit) {
            var result = ShowIFrame("/Sunnet/Events/EditEvent.aspx?ID=" + id + "&c=" + canEdit, 383, 430, true, "Edit Schedules");
            if (result == 0) {
                reloadEventCalendar();
            }
        }

        $(function () {
            $.post("/forums/topics/LatestTopics?top=6", function (data) {
                var str = "";
                data = eval(data);
                str += '<tr class="listsubTitle">';
                str += '<td width="30%">Project Name</td>';
                str += '<td width="50%">Topic Name</td>';
                str += '<td width="20%">Created Date</td>';
                str += '</tr>';
                for (var i = 0; i < data.length; i++) {
                    str += '<tr class="' + ((i % 2 == 0) ? "listrowone" : "listrowtwo") + '">';
                    str += '<td>';
                    str += data[i].Forum.Description;
                    str += '</td>';
                    str += '<td>';
                    str += '<a target="targetDashboard" href="/forums/' + data[i].Forum.ShortName + '/' + data[i].ShortName + '-' + data[i].Id
                        + '/" onclick="TopicForumsRedirect();" ><img style="vertical-align:middle" src="/images/topicleft.png" /> ' + data[i].Title + '</a>';
                    str += '</td>';
                    str += '<td>';
                    str += data[i].Date;
                    str += '</td>';
                    str += '</tr>';
                }
                $("#tabLatestTopics").append(str);
            }, "json");

            $.post("/forums/topics/HotsTopics?top=6", function (data) {
                var str = "";
                str += '<tr class="listsubTitle">';
                str += '<td width="30%">Project Name</td>';
                str += '<td width="50%">Topic Name</td>';
                str += '<td width="20%">Created Date</td>';
                str += '</tr>';
                data = eval(data);
                for (var i = 0; i < data.length; i++) {
                    str += '<tr class="' + ((i % 2 == 0) ? "listrowone" : "listrowtwo") + '">';
                    str += '<td>';
                    str += data[i].Forum.Description;
                    str += '</td>';
                    str += '<td>';
                    str += '<a target="targetDashboard" href="/forums/' + data[i].Forum.ShortName + '/' + data[i].ShortName + '-' + data[i].Id
                        + '/" onclick="TopicForumsRedirect();" ><img style="vertical-align:middle" src="/images/topicleft.png" /> ' + data[i].Title + '</a>';
                    str += '</td>';
                    str += '<td>';
                    str += data[i].Date;
                    str += '</td>';
                    str += "</tr>";
                }
                $("#tabHotsTopics").append(str);
            }, "json");

            $.post("/forums/topics/LatestReply?top=6", function (data) {
                var str = "";
                str += '<tr class="listsubTitle">';
                str += '<td width="30%">Project Name</td>';
                str += '<td width="50%">Topic Name</td>';
                str += '<td width="20%">Created Date</td>';
                str += '</tr>';
                data = eval(data);
                for (var i = 0; i < data.length; i++) {
                    str += '<tr class="' + ((i % 2 == 0) ? "listrowone" : "listrowtwo") + '">';
                    str += '<td>';
                    str += data[i].Forum.Description;
                    str += '</td>';
                    str += '<td>';
                    str += '<a target="targetDashboard" href="/forums/' + data[i].Forum.ShortName + '/' + data[i].ShortName + '-' + data[i].Id
                        + '/" onclick="TopicForumsRedirect();" ><img style="vertical-align:middle" src="/images/topicleft.png" /> ' + data[i].Title + '</a>';
                    str += '</td>';
                    str += '<td>';
                    str += data[i].Date;
                    str += '</td>';
                    str += "</tr>";
                }
                $("#tabLatestReply").append(str);
            }, "json");

            jQuery.get("/document/DocManagement/DocHome/GetRemoteProjectFile", { 'r': Math.random() }, function (data) {
                var str = "";
                for (var i = 0; i < data.length; i++) {
                    str += "<tr class='" + (i % 2 == 0 ? "listrowone" : "listrowtwo") + "' _id=" + data[i].ID + ">";

                    str += "  <td>";
                    str += "        " + data[i].ProjectName;
                    str += "  </td>";
                    str += "  <td>";
                    if (data[i].Type === 1) {
                        str += "      <img src='/Images/folderleft.png' />&nbsp;<a href='/document/DocManagement/DocHome/Index?projectId=" + data[i].ProjectID + "&folderId=" + data[i].ID + "' target='targetDashboard'>" + data[i].DisplayFileName + "</a>";
                    }
                    else {
                        str += "      <img src='/icons/09.gif' />&nbsp;<a href='javascript:void(0);' onclick='window.download(" + data[i].ProjectID + "," + data[i].ID + ")'>" + data[i].DisplayFileName + "</a>";
                    }
                    str += "  </td>";
                    str += "  <td>";
                    str += "        " + data[i].CreateUserName;
                    str += "  </td>";
                    str += "  <td>";
                    str += "        " + data[i].UpdatedOn;
                    str += "  </td>";
                    str += "</tr>";
                }
                jQuery("#tbDocument").append(str);
            }, 'json');

            var trackingTicketLink = $('ul.pdashboardItem>a');
            trackingTicketLink.each(function () {
                $(this).attr('href', $(this).attr('href'));
            });
        })

        function InitEventList() {
            $.get('/Do/DoGetEventList.ashx', { 'r': Math.random() }, function (data) {
                var $dvEventList = $('#divEventList')
                $dvEventList.css('display', 'none');
                $dvEventList.html('');
                $dvEventList.append(data);
                $dvEventList.css('display', 'block');
                jQuery("tr[opentype]").hover(
          function () {
              jQuery(this).addClass("listrowthree");
          },
            function () {
                jQuery(this).removeClass("listrowthree");
            }).css('cursor', 'pointer');
            });
        }

        function TopicForumsRedirect() {
            $.post("/forums/login/pm/finish", function (data) {
            });
        }
        function clickDayEventList(d) {
            jQuery("#divDayInviteEventList").modal("refresh", "/Sunnet/Events/DayEvents.aspx?date=" + d + "&UserID=" + "<%=UserInfo.UserID%>" + "&r=" + Math.random());
        }
        function refresh(target) {
            InitEventList();
        }
    </script>
</asp:Content>
