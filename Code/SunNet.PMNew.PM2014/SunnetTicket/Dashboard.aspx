<%@ Page Title="" Language="C#" MasterPageFile="~/SunnetTicket/Sunnet.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SunNet.PMNew.PM2014.Ticket.Sunnet.Dashboard" %>
<%@ Register TagPrefix="uc1" TagName="Messager" Src="~/UserControls/Messager.ascx" %>
<%@ Register TagPrefix="custom" TagName="feedbacks" Src="~/UserControls/Ticket/FeedbackList.ascx" %>

<%@ Import Namespace="SunNet.PMNew.Entity.TicketModel" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Utils.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.extend.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#ticketTabNote").popover();
        });
    </script>
    <style type="text/css">
        .popover {
            max-width: 350px;
        }

        .buttonBox4 {
            padding-top: 5px;
        }
        .titleeventlist {
            margin: 0px;
            padding: 15px 10px 10px 0px;
            color: #333;
            text-transform: none;
            font-size: 16px;
            color: #6ca632;
        }

         .titleeventlist .titleeventlist_icons {
             margin-right: 5px;
             margin-left: 10px;
         }

         .titleeventlist .titleeventlist_input {
             font-size: 14px;
             font-weight: normal;
             float: right;
             color: #333;
         }
        .chat_peoples {
          overflow: hidden;
          padding: 15px 10px 10px;
          border-bottom: 1px #ddd dashed;
        }

        .chat_peoples .chat_peoples_s {
            margin-bottom: 10px;
            display: inline-block;
            color: #999;
        }

        .chat_peoples ul.assignUser li {
            width: 30%;
            margin-bottom: 8px;
        }

        .chat_input_box {
            padding: 20px 10px;
            text-align: center;
        }

        .chat_btn_upload {
            background-color: #d0762d;
            padding: 20px 11px;
            border-radius: 30px;
            cursor: pointer;
        }

         .chat_btn_upload:hover {
           background-color: #c16b25;
         }

         .inputFeedback {
           height: 35px;
           width: 60%;
           padding: 0px;
           vertical-align: middle;
           /*border-radius: 10px;*/
           padding: 10px;
           margin: 0 10px;
        }
        
        .chat_btn_push {
            background-color: #6ca632;
            padding: 20px 11px;
            border-radius: 30px;
            cursor: pointer;
        }

        .chat_btn_push:hover {
            background-color: #609629;
        }
        .feedbackBox .requestfrom {
                text-align: center;
                padding: 5px;
                margin: 5px 0px;
                cursor: pointer;
         }

        .feedbackBox .requestfrom a {
            color: #eee;
            padding: 3px 10px;
            border-radius: 15px;
            background-color: #F60;
            text-decoration: none;
        }

        .feedbackBox .requestfrom a:hover {
            color: #ffffff;
            background-color: #F60;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="49%" class="cdmainBox">
                <div class="cdcontentTop">
                    <div class="cdcontentTitle">Tracking Tickets<a href="javascript:;" data-placement="right" id="ticketTabNote" data-container="body" data-trigger="click hover" data-toggle="tooltip" class="info" data-content="Project Title > Ticket Title (Ticket ID) , Status, Priority">&nbsp;</a></div>
                </div>
                <div class="cdashboardIconbox">
                    <div class="panel-group" id="accordion">
                        <asp:Repeater ID="rptTicketsList" runat="server">
                            <ItemTemplate>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a onclick="chageRead(<%#Eval("TicketID") %>)" data-toggle="collapse" data-parent="#accordion" href="#panel_ticket<%#Eval("TicketID") %>">
                                                <img src="../Images/icons/orange.png" title="New Unread Ticket" alt="<%#Eval("TicketID") %>" name="img<%#Eval("IsRead") %>" hidden="hidden"/>
                                                <%#Eval("ProjectTitle").ToString().SubString(20) %> > <%#Eval("Title").ToString().SubString(20) %> (<%#Eval("TicketID") %>), <%# GetStatus(Eval("Status"))%>, <%# Eval("Priority")%>
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="panel_ticket<%#Eval("TicketID") %>" class="panel-collapse collapse <%#Container.ItemIndex==0?"":"" %>">
                                        <div class="panel-body">
                                            <div class="form-group">
                                                <label class="col-left-1 lefttext" style="width: auto;">Project:</label>
                                                <div class="col-right-col2 righttext"><%# Eval("ProjectTitle")%></div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-left-1 lefttext" style="width: auto;">Ticket ID:</label>
                                                <div class="col-right-col2 righttext" style="width: auto; margin-right: 20px;"><%#Eval("TicketID") %></div>
                                                <label class="col-left-1 lefttext" style="width: auto;">Type:</label>
                                                <div class="col-right-col2 righttext" style="width: auto; margin-right: 20px;">
                                                    <%#((TicketsType)Eval("TicketType")).ToText() %>
                                                </div>
                                                <label class="col-left-1 lefttext" style="width: auto;">Priority:</label>
                                                <div class="col-right-col2 righttext" style="width: auto; margin-right: 20px;">
                                                    <%# Eval("Priority")%>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-left-1 lefttext" style="width: auto;">Status:</label>
                                                <div class="col-right-col2 righttext"><%# GetStatus(Eval("Status"))%></div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="form-group">
                                                <label class="col-left-1 lefttext" style="width: 40px;">Title:</label>
                                                <div class="col-right-col2 righttext" style="width: auto;">
                                                    <%#Eval("Title") %>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="form-group">
                                                <label class="col-left-1 lefttext" style="width: 80px;">Description:</label>
                                                <div class="col-right-col2 righttext" style="width: auto;">
                                                    <%# Server.HtmlEncode(Eval("FullDescription").ToString()) %>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="form-group" name="dv<%#Eval("FileID") %>">
                                                <label class="col-left-1 lefttext" style="width: 80px;">Attachments:</label>
                                                <div class="col-right-col2 righttext" style="width: auto;">
                                                    <a target="_blank" href='/do/DoDownloadFileHandler.ashx?FileID=<%#Eval("FileID") %>&size=<%#Eval("FileSize") %>'>
                                                        <%#Eval("FileTitle") %><%#Eval("ContentType") %>
                                                    </a>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="buttonBox4">
                                                <a href="Detail.aspx?tid=<%# Eval("TicketID")%>&returnurl=<%=this.ReturnUrl %>"
                                                    class="backBtn mainbutton small btn-icon btn-view " title="view details" target="_blank">Details</a>
                                                <%# GetActionHTML(Eval("ProjectID"),Eval("Status"),Eval("TicketID"),Eval("IsEstimates"),Eval("EsUserID"),Eval("CreatedBy"),(int)Eval("ConfirmEstmateUserId"),"iconText","saveBtn1 mainbutton small btn-icon btn-review ") %>
                                                <div class="btn-group" data-remote="workingon">
                                                    <button type="button" class="backBtn mainbutton small dropdown-toggle " data-workingstatus="<%# Eval("TicketID")%>" data-toggle="dropdown">
                                                        <span class="caret"></span>
                                                        <span class="text">WorkingOn</span>
                                                    </button>
                                                    <ul class="dropdown-menu" role="menu">
                                                        <li><a href="javascript:;" ticket="<%# Eval("TicketID")%>" data-action="setworkingcomplete">Completed</a></li>
                                                        <li><a href="javascript:;" ticket="<%# Eval("TicketID")%>" data-action="setworkingcancelled">Cancelled</a></li>
                                                        <li class="divider"></li>
                                                        <li><a href="javascript:;" ticket="<%# Eval("TicketID")%>" data-action="setworkingonnone">None</a></li>
                                                    </ul>
                                                </div>

                                                <a href='###' action="calladdtocategory" title="Add to category" ticket='<%#Eval("TicketID") %>'>
                                                    <img src="/Images/icons/category.png" alt="Category" /></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:PlaceHolder ID="phlNoTicket" runat="server" Visible="false">You have no tickets working on now.
                        </asp:PlaceHolder>
                    </div>
                </div>
            </td>
            <td width="20">&nbsp;</td>
            <!--chatList-->
            <td width="49" class="cdmainBox" id="chatTicket" style="display: none">
                  <div class="limitwidth">
                     <div class="form-group-container" style="width: 100%;">
                          <uc1:Messager ID="Messager1" runat="server" />
                          <div id="controlSpan">
                          </div>
                      </div>
                  </div>
            </td>
            <!-- evetns -->
            <td width="49%" class="cdmainBox" id="eventClander">
                <div class="cdcontentTop">
                    <div class="cdcontentTitle">Upcoming Activities </div>
                    <div class="cdMore"><a href="/Event/index.aspx" target="targetDashboard">More&gt;&gt;</a></div>
                </div>
                <div class="cdtopbtnbox">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="160"><% if (CheckRoleCanAccessPage("/Event/Add.aspx"))
                                               { %>
                                <ul class="listtopBtn">
                                    <li href="/Event/Add.aspx" data-target="#modalsmall" data-toggle="modal" id="dvCreateEvent">
                                        <div class="listtopBtn_icon">
                                            <img src="/images/icons/wevents.png" />
                                        </div>
                                        <div class="listtopBtn_text">Create Event </div>
                                    </li>
                                </ul>
                                <% } %>
                            </td>
                            <td width="*">
                                <table border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center" style="width: 40px;">
                                            <a href="javascript:void(0);">
                                                <img src="/images/monthleft2.png" onclick="backClick(1)" title="Previous Year" /></a></td>
                                        <td align="center" style="width: 40px;">
                                            <a href="javascript:void(0);">
                                                <img src="/images/monthleft.png" onclick="backClick(0)" title="Previous Month" />
                                            </a></td>
                                        <td align="center" id="spnMonth"><%=DateTime.Now.ToString("MMMM yyyy") %></td>
               <%--                         <td style="width: 28px;"></td>--%>
                                        <td align="center" style="width: 40px;">
                                            <a href="javascript:void(0);">
                                                <img src="/images/monthright.png" onclick="forwardClick(0);" title="Next Month">
                                            </a></td>
                                        <td align="center" style="width: 40px;">
                                            <a href="javascript:void(0);">
                                                <img src="/images/monthright2.png" onclick="forwardClick(1)" title="Next Year">
                                            </a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="180" align="right"><span class="topbtn cdtopBtn cdtopBtn-active" onclick="showEventCalendar()" id="spanCalendar">Calendar</span>
                                <span class="topbtn cdtopBtn" onclick="showEventList()" id="spanList">Today</span></td>
                            <asp:HiddenField ID="hiUserIds" runat="server" />
                        </tr>
                    </table>
                </div>
                <script type="text/javascript">
                    $(function () {
                        $("div[name='dv']").hide();
                        $("img[name='imgUnread']").show();
                    });
    
                    function chageRead(ticketID)
                    {
                        var params = {
                            action: "chageIsRead",
                            ticketID: ticketID,
                        };
                        $.post("/Service/Ticket.ashx", params, function (response) {
                            if(response == 'true')
                            {
                                $("img[alt='" + ticketID + "']").hide();
                            }
                        }, "json");
                        $("img[alt='" + ticketID + "']").hide();
                        var ds = $('#panel_ticket' + ticketID).hasClass('in');
                        if (!ds) {
                            $("#controlSpan").html("");
                            $("#controlSpan").loadUserControl(ticketID);
                            $("#chatTicket").css("display", "");
                            $("#eventClander").css("display", "none");
                        } else {
                            $("#chatTicket").css("display", "none");
                            $("#eventClander").css("display", "");
                        }
                    }

                    function showEventCalendar() {
                        var spanCalendar = jQuery("#spanCalendar");
                        var spanList = jQuery("#spanList");
                        if (spanList.hasClass("cdtopBtn-active")) {
                            spanList.removeClass("cdtopBtn-active");
                            jQuery("#divEventCalendar").toggle();
                        }
                        if (!spanCalendar.hasClass("cdtopBtn-active")) {
                            spanCalendar.addClass("cdtopBtn-active");
                            jQuery("#divEventList").toggle();
                            reloadEventCalendar();
                        }
                        $("#dvCreateEvent").attr("href", $("#dvCreateEvent").attr("href").replace(/\?eventList=1/, ""));
                    }

                    function showEventList() {
                        var spanCalendar = jQuery("#spanCalendar");
                        var spanList = jQuery("#spanList");
                        if (spanCalendar.hasClass("cdtopBtn-active")) {
                            spanCalendar.removeClass("cdtopBtn-active");
                            jQuery("#divEventList").toggle();
                            InitEventList();
                        }
                        if (!spanList.hasClass("cdtopBtn-active")) {
                            jQuery("#spanList").addClass("cdtopBtn-active");
                            jQuery("#divEventCalendar").toggle();
                            InitEventList();
                        }
                        $("#dvCreateEvent").attr("href", $("#dvCreateEvent").attr("href").replace(/$/, "?eventList=1"));
                    }
                </script>
                <div class="cdcontentBox" id="divEventList" style="display: none;">
                </div>

                <div class="pdcalendar" id="divEventCalendar">
                    <div class="cdtopbtnbox" id="tdContext">
                    </div>
                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="cdmonthlyviewBox">
                        <tbody>
                            <tr>
                                <script type="text/javascript">

                                    var tmpCalendarStart = "<table id='tb{month}' width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"cdmonthlyviewBox\" style=\"margin-left:1px;\">" +
                                        "<tbody><tr><td><ul class=\"cdmonthlyviewweek\"><li>Sun</li><li>Mon</li><li>Tues</li><li>Wed</li><li>Thurs</li><li>Fri</li><li>Sat</li></ul> <ul class=\"cdmonthlyview\">";

                                    var tmpCalendarEnd = "</ul></td></tr></tbody></table>";
                                    var tmpCalendarLink = "<li id='liCalendarEvent{ID}'><a   href='{URL}'  data-target='#modalsmall' data-toggle='modal'>";
                                    var tmpCalendarLink2 = "<li id='liCalendarEvent{ID}'><a   href='{URL}'  data-target='#modalsmall' data-toggle='modal'>";
                                    var tmpEventIcon = "<img src='{icon}' style='width:24px; height:24px;' />";
                                    function builderLi(arr, strdate) {
                                        var tmp = "<ul class=\"cdonscdTicket\">";
                                        for (var i = 0; i < arr.length; i++) {
                                            if (i < 1) {
                                                var tmpIcon = tmpEventIcon.replace("{icon}", arr[i].Icon);
                                                if (arr[i].Invited) {
                                                    tmp += tmpCalendarLink.replace("{Name}", arr[i].Name).replace("{Icon}", tmpIcon).replace("{Date}", arr[i].date).replace("{Invite}", arr[i].FullName).replace("{URL}", getEditUrl(arr[i].ID, arr[i].IsEdit))
                                                        .replace(/{ID}/g, arr[i].ID) + arr[i].Title + "</a></li>";
                                                    if (arr[i].InviteStatus == 1)
                                                        tmp = tmp.replace("{InviteEvent}", "<span class='eventbtn3' onclick='JoinEvent(" + arr[i].ID + ");'>Join</span><span class='eventbtn3' onclick='Decline(" + arr[i].ID + ");'>Decline</span>");
                                                    else
                                                        tmp = tmp.replace("{InviteEvent}", "Agreed");
                                                }
                                                else {
                                                    tmp += tmpCalendarLink2.replace("{Name}", arr[i].Name).replace("{Icon}", tmpIcon).replace(/{ID}/g, arr[i].ID).replace("{URL}", getEditUrl(arr[i].ID, arr[i].IsEdit)) + arr[i].Title + "</a></li>";
                                                }
                                            }
                                        }
                                        if (arr.length > 1) {
                                            tmp += "<li><a href=\"" + getDayEventsUrl(strdate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\" style=\"cursor: pointer;color:#15c\"> More</a></li>";
                                        }
                                        return tmp += "</ul>";
                                    }

                                    var calendarDate;
                                    var calendarMonth;
                                    var doubleClick = true;
                                    function InitEvents(para, oldId, r) {

                                        if (!para.dashboard) {
                                            para["dashboard"] = 1;
                                        }
                                        if (!para.userID) {
                                            para['userID'] = '<%=UserInfo.UserID%>';
                                        }
                                        if (!doubleClick) return;
                                        if (r) {
                                            jQuery("#div" + oldId).remove();
                                            jQuery("#tb" + oldId).remove();
                                        }
                                        $.getJSON("/Do/DoGetCalendarList.ashx?" + Math.random(), para, function (result) {
                                            var d = result.list;
                                            calendarDate = d[8].StrDate;
                                            //var full_month = d[8].Month.split("_");
                                            //var formatStr = handleMonthFormat(full_month[0]) + " " + full_month[1];
                                            //$("#spnMonth").text(formatStr);
                                            jQuery("#spnMonth").html(d[8].Month.replace("_", " "));
                                            calendarMonth = d[8].Month;
                                            var tmp = "";
                                            for (var i = 0; i < d.length; i++) {
                                                switch (d[i].Type) {
                                                    case 0:
                                                        tmp += "<li class=\"cddateDisable\"><div>" + d[i].Day;
                                                        if (d[i].list.length > 0)
                                                            tmp += builderLi(d[i].list, d[i].StrDate);
                                                        tmp += "</div></li>";
                                                        break;
                                                    case 1:
                                                        tmp += "<li><div class='cddateforclick' href=\"" + getDayEventsUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\">" + d[i].Day + "</div>"
                                                        if (d[i].list.length > 0)
                                                            tmp += builderLi(d[i].list, d[i].StrDate); tmp += "</li>";
                                                        break;
                                                    case 2:
                                                        tmp += "<li><div class='cddateforclick'   href=\"" + getDayEventsUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\">" + d[i].Day + "</div><img src=\"/images/icons/add1.png\"  style=\"cursor:pointer\"  align=\"right\" title='Create Event'  href=\"" + getCreateUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\">";
                                                        if (d[i].list.length > 0)
                                                            tmp += builderLi(d[i].list, d[i].StrDate);
                                                        tmp += "<div style=\"padding-top:" + (d[i].list.length * 20 + 20) + "px;width:80px;height:" + (60 - d[i].list.length * 20 - 20) + "px\" href=\"" + getCreateUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"></div>"
                                                        tmp += "</li>";
                                                        break;
                                                    case 3:
                                                        tmp += "<li class=\"cddateToday\"><a name='today'></a><div class='cddateforclick'  href=\"" + getDayEventsUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\">" + d[i].Day + "</div><img src=\"/images/icons/add1.png\" align=\"right\" style=\"cursor:pointer\"  title='Create Event'  href=\"" + getCreateUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\">";
                                                        if (d[i].list.length > 0)
                                                            tmp += builderLi(d[i].list, d[i].StrDate);
                                                        tmp += "<div style=\"padding-top:" + (d[i].list.length * 20 + 20) + "px;width:80px;height:" + (60 - d[i].list.length * 20 - 20) + "px\" href=\"" + getCreateUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"></div>"
                                                        tmp += "</li>";
                                                        break;
                                                    case 4:
                                                        tmp += "<li  class=\"cddateDisable\"><div class='cddateforclick'  href=\"" + getDayEventsUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\">" + d[i].Day + "</div>";
                                                        if (result.isOwner == true) {
                                                            tmp += "<img src=\"/images/icons/add1.png\"  style=\"cursor:pointer\"  align=\"right\" title='Create Event' href=\"" + getCreateUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\" >";
                                                        }
                                                        if (d[i].list.length > 0)
                                                            tmp += builderLi(d[i].list, d[i].StrDate);
                                                        tmp += "<div style=\"padding-top:" + (d[i].list.length * 20 + 20) + "px;width:80px;height:" + (60 - d[i].list.length * 20 - 20) + "px\" href=\"" + getCreateUrl(d[i].StrDate) + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"></div>"
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

                                    function shopPop() {
                                        jQuery("#tdContext").find("[data-toggle='popover']").popover({
                                            html: true,
                                            trigger: "hover",
                                            delay: { show: 0, hide: 500 },
                                            placement: function (target, source) {
                                                var popoverObj = this;
                                                var $day = jQuery(source).closest("ul.cdmonthlyview>li");
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
                                            location.href = "#today";
                                        else {
                                            InitEvents({}, calendarMonth, 2);
                                        }
                                    }
                                </script>


                            </tr>
                        </tbody>
                    </table>
                </div>
                
            </td>
        </tr>
        <tr>
            <td height="20" colspan="3"></td>
        </tr>
        <tr>
            <!-- forms -->
            <td class="cdmainBox">
                <div class="cdcontentTop">
                    <div class="cdcontentTitle">Project Forums </div>
                    <div class="cdMore"><a href="/forums/login/pm/finish" target="targetDashboard">More&gt;&gt;</a></div>
                </div>
                <div class="cdcontentBox" id="tabLatestTopics">
                    <table id="tabForum" width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
                        <thead>
                            <tr>
                                <th width="110">Project Name</th>
                                <th width="*">Topic Name</th>
                                <th width="130">Created On </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </td>
            <td>&nbsp;</td>
            <!-- documents -->
            <td class="cdmainBox">
                <div class="cdcontentTop">
                    <div class="cdcontentTitle">Documents</div>
                    <div class="cdMore"><a href="/document/DocManagement/DocHome/Index" target="targetDashboard">More&gt;&gt;</a></div>
                </div>
                <div class="cdcontentBox" id="tbDocument">
                </div>
            </td>
        </tr>
    </table>
    <iframe id="ifrdownload" name="ifrdownload" frameborder="0" height="0" width="0" scrolling="no" style="position: fixed; left: -50px; visibility: hidden;"></iframe>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <script type="text/html" id="temp_form_forum">
        <tr>
            <td colspan="3">
                <div class="cdforumtitle">
                    <img src="{% this.img %}" />
                    {% this.module %}
                </div>
            </td>
        </tr>
        {% if (this && this.length) { %}
            {% for(var i = 0; i < this.length ; i++) {%}
                <tr class="{% getClass(i,1,'whiterow') %}">
                    <td title="{% this[i].Forum.Description %}">{% this[i].Forum.Name %}</td>
                    <td><a target="targetDashboard" href="/forums/{%  this[i].Forum.ShortName + '/' + this[i].ShortName + '-' + this[i].Id %}/" onclick="TopicForumsRedirect();">
                        <img style="vertical-align: middle" src="/images/topicleft.png" />
                        {% this[i].Title %} </a></td>
                    <td>{% this[i].Date %}</td>
                </tr>
        {% } %}
        {% } %} 
    </script>
    <script type="text/html" id="temp_documents">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
            <thead>
                <tr>
                    <th width="110">Project Name
                    </th>
                    <th>File</th>
                    <th width="130">Updated on </th>
                </tr>
            </thead>
            <tbody>
                {% if (this && this.length) { %}
                    {% for(var i = 0; i < this.length ; i++) {%}
                    <tr class="{% getClass(i,1,'whiterow') %}">
                        <td>{% this[i].ProjectName %}</td>
                        <!-- 1 folder -->
                        <td>{% if (this[i].Type === 1) { %}
                                <img src="/Images/icons/folder.png" />
                            <a href="/document/DocManagement/DocHome/Index?projectId={% this[i].ProjectID %}&folderId={% this[i].ID %}" target="targetDashboard">{% this[i].DisplayFileName %}</a>
                            {% } else { %}
                                <a href="/document/DocManagement/DocHome/Download?projectId={% this[i].ProjectID %}&id={% this[i].ID %}" target="ifrdownload">{% this[i].DisplayFileName %}</a>
                            {% } %}
                        </td>
                        <td>{% this[i].UpdatedOn %}</td>
                    </tr>
                {% } %}
                {% } %}
            </tbody>
        </table>
    </script>
    <script type="text/javascript">

        function reloadEventCalendar() {
            var $dvEventList = $('#divEventList')
            $dvEventList.css('display', 'none');
            var $tdContent = $('#tdContext')
            $('#tdContext table.cdmonthlyviewBox').remove();
            $tdContent.css('display', 'none');
            InitEvents({ "d": calendarDate, "t": 0, "dashboard": 1 }, calendarMonth, true);
            $tdContent.css('display', 'block');
        }

        function getCreateUrl(day) {
            return "/Event/Add.aspx?Date=" + day + "&r=" + Math.random();
        }

        function getEditUrl(id, canEdit) {
            if (canEdit) {
                return "/Event/Edit.aspx?ID=" + id + "&c=" + canEdit + "&r=" + Math.random();
            }
            else {
                return "/Event/View.aspx?ID=" + id + "&r=" + Math.random();
            }
        }

        function getDayEventsUrl(d) {
            return "/Event/DayEvent.aspx?date=" + d + "&r=" + Math.random() + "&allUser=" + $("#<% = hiUserIds.ClientID%>").val();
        }
        function InitEventList() {
            $.get('/Do/DoGetEventList.ashx', { 'r': Math.random() }, function (data) {
                $("#tdContext").css("display", "none");
                var $dvEventList = $('#divEventList')
                $dvEventList.css('display', 'none');
                $dvEventList.html('');
                $dvEventList.append(data);
                $dvEventList.css('display', 'block');
            });
        }

        function clickDayEventList(d) {
            jQuery("#divDayInviteEventList").modal("refresh", "/Sunnet/Events/DayEvents.aspx?date=" + d + "&UserID=" + "<%=UserInfo.UserID%>" + "&r=" + Math.random());
        }
        function refresh(target) {
            InitEventList();
        }

        function getClass(index, expectedValue, expectedClass) {
            if (index % 2 == expectedValue)
                return expectedClass;
            return "";
        }
        function TopicForumsRedirect() {
            $.post("/forums/login/pm/finish", function (data) {
            });
        }

        var months;

        jQuery(function () {
            $.ajaxSetup({
                beforeSend: function (v, v2) {
                    if (v2 && v2.url) {
                        if (v2.url.indexOf("DoGetCalendarList.ashx") > -1) {
                            doubleClick = false;
                        }
                    }
                },
                complete: function () {
                    if (this.url) {
                        if (this.url.indexOf("DoGetCalendarList.ashx") > -1) {
                            doubleClick = true;
                        }
                    }
                }
            });

            InitEvents({

            });

            // Latest Topics /Images/icons/latesttopic.png
            $.post("/forums/topics/LatestTopics?top=6", function (data) {
                if (!data) {
                    data = {}
                }
                data["img"] = "/Images/icons/latesttopic.png";
                data["module"] = "Latest Topics";

                var html = TemplateEngine(GetTemplateHtml("temp_form_forum"), data);
                $("#tabForum").append(html);
            }, "json");
            // Latest Reply /Images/icons/reply.png
            $.post("/forums/topics/LatestReply?top=6", function (data) {
                if (!data) {
                    data = {}
                }
                data["img"] = "/Images/icons/reply.png";
                data["module"] = "Latest Reply";

                var html = TemplateEngine(GetTemplateHtml("temp_form_forum"), data);
                $("#tabForum").append(html);
            }, "json");
            // Hot Topics /Images/icons/hottopic.png
            $.post("/forums/topics/HotsTopics?top=6", function (data) {
                if (!data) {
                    data = {}
                }
                data["img"] = "/Images/icons/hottopic.png";
                data["module"] = "Hot Topics";
                var html = TemplateEngine(GetTemplateHtml("temp_form_forum"), data);
                $("#tabForum").append(html);
            }, "json");
            // documents
            jQuery.get("/document/DocManagement/DocHome/GetRemoteProjectFile", function (data) {
                var html = TemplateEngine(GetTemplateHtml("temp_documents"), data);
                $("#tbDocument").append(html);
            }, "json");

            $("body").on("hide.bs.modal", ".modal", function () {
                var $ifame = $(this).find("iframe");
                var urlParams = $ifame[0].contentWindow.urlParams;
                if ($ifame.attr("src") && $ifame[0].contentWindow.urlParams.close) {
                    if (urlParams.eventList) {
                        window.location.href = window.location.href + "?eventList=1";
                    }
                    else {
                        window.location.href = window.location.href.replace(/\?eventList=1/, "");
                    }
                }
                return true;
            });

            if (window.location.href.match(/(\?eventList=1)/)) {
                window.location.href.replace(/\?eventList=1/, "");
                showEventList();
            }

            months = [
                { zh: "一月", cn: "January" },
                { zh: "二月", cn: "February" },
                { zh: "三月", cn: "March" },
                { zh: "四月", cn: "April" },
                { zh: "五月", cn: "May" },
                { zh: "六月", cn: "June" },
                { zh: "七月", cn: "July" },
                { zh: "八月", cn: "August" },
                { zh: "九月", cn: "September" },
                { zh: "十月", cn: "October" },
                { zh: "十一月", cn: "November" },
                { zh: "十二月", cn: "December" }
            ];
        });

        function handleMonthFormat(item) {
            var month_cn;
            $.each(this.months, function (i, month) {
                if (month.zh == item) {
                    month_cn = month.cn;
                    return false;
                }
            });
            return month_cn;
        }
    </script>
</asp:Content>
