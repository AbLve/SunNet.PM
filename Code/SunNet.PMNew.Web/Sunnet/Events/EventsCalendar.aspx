<%@ Page Title="Events" Language="C#" AutoEventWireup="true" MasterPageFile="~/Sunnet/Main.Master"
    CodeBehind="EventsCalendar.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.EventCalendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <table width="99%" border="0" cellpadding="0" cellspacing="0" style="min-width: 900px; table-layout: fixed; margin-top: -4px; line-height:2">
        <tr>
            <td width="65%">Events > Calendar
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" cellspacing="0" cellpadding="0" border="0">
        <tbody>
            <tr>
                <td vertical-align="top" style="border-bottom: 1px solid rgb(129, 186, 232);">
                    <table width="97%" cellspacing="0" cellpadding="0" border="0" align="center" class="searchBox">
                        <tbody>
                            <tr>
                                <td width="30" align="right">Year:
                                </td>
                                <td width="120">
                                    <asp:DropDownList ID="ddlYears" runat="server" CssClass="select205" Width="120px">
                                        <asp:ListItem Value="2013">2013</asp:ListItem>
                                        <asp:ListItem Value="2014">2014</asp:ListItem>
                                        <asp:ListItem Value="2015">2015</asp:ListItem>
                                        <asp:ListItem Value="2016">2016</asp:ListItem>
                                        <asp:ListItem Value="2017">2017</asp:ListItem>
                                        <asp:ListItem Value="2018">2018</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="60" align="right">Month:
                                </td>
                                <td width="120">
                                    <asp:DropDownList ID="ddlMonths" runat="server" CssClass="select205" Width="120px">
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="6">6</asp:ListItem>
                                        <asp:ListItem Value="7">7</asp:ListItem>
                                        <asp:ListItem Value="8">8</asp:ListItem>
                                        <asp:ListItem Value="9">9</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="11">11</asp:ListItem>
                                        <asp:ListItem Value="12">12</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="60" align="right">Project:
                                </td>
                                <td width="214">
                                    <asp:DropDownList ID="ddlProjects" runat="server" class="select205">
                                    </asp:DropDownList>

                                </td>
                                <td>
                                    <input type="image" style="border-width: 0px;" src="/images/search_btn.jpg" id="iBtnSearch"
                                        name="iBtnSearch" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="mainactionBox">
                        <div class="mainactionBox_left">
                            <span><a href="#" onclick="OpenAddModuleDialog('<%=DateTime.Now.ToString("MM/dd/yyyy")%>')">
                                <img src="/images/event.png" width="16" height="18" border="0" align="absmiddle">
                                Create Event </a></span>
                        </div>
                        <div class="mainactionBox_right">
                        </div>
                    </div>
                    <div class="mainrightBoxthree">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="monthlyviewBox">
                            <tbody>
                                <tr>
                                    <script type="text/javascript">

                                        var tmpCalendarStart = "<table id='tb{month}' width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"monthlyviewBox\">" +
                                                    "<tbody><tr><td><ul class=\"monthlyviewweek\"><li>Sunday</li><li>Monday</li><li>Tuesday</li><li>Wednesday</li><li>Thursday</li><li>Friday</li><li>Saturday</li></ul> <ul class=\"monthlyview\">";

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
                                                    tmp += tmpCalendarLink2.replace("{Name}", arr[i].Name).replace("{Icon}", tmpIcon).replace("{Date}", arr[i].date).replace("{Edit}", tmpEdit).replace(/{ID}/g, arr[i].ID) + arr[i].Title + "</a></li>";
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
                                                            tmp += "<li class=\"dateDisable\">" + d[i].Day + "</li>";
                                                            break;

                                                        case 1:
                                                            tmp += "<li><div class='dateforclick' onclick=\"clickDayEventList('" + d[i].StrDate + "');\">" + d[i].Day + "</div>"
                                                            if (d[i].list.length > 0)
                                                                tmp += builderLi(d[i].list);
                                                            tmp += "</li>";
                                                            break;

                                                        case 2:
                                                            tmp += "<li><div class='dateforclick'  onclick=\"clickDayEventList('" + d[i].StrDate + "');\">" + d[i].Day + "</div>";
                                                            if (result.isOwner == true) {
                                                                tmp += "<img src=\"/images/add1.png\"  style=\"cursor:pointer\"  align=\"right\" title='Create Event'  onclick=\"OpenAddModuleDialog('" + d[i].StrDate + "')\">";
                                                            }
                                                            if (d[i].list.length > 0)
                                                                tmp += builderLi(d[i].list);
                                                            tmp += "</li>";
                                                            break;

                                                        case 3:
                                                            tmp += "<li class=\"dateToday\"><a name='today'></a><div class='dateforclick'  onclick=\"clickDayEventList('" + d[i].StrDate + "');\">" + d[i].Day + "</div>";
                                                            if (result.isOwner == true) {
                                                                tmp += "<img src=\"/images/add1.png\" align=\"right\" style=\"cursor:pointer\"  title='Create Event'  onclick=\"OpenAddModuleDialog('" + d[i].StrDate + "')\">";
                                                            }
                                                            if (d[i].list.length > 0)
                                                                tmp += builderLi(d[i].list);
                                                            tmp += "</li>";
                                                            break;

                                                            //no default 
                                                    }
                                                }

                                                if (result.isOwner == false) {
                                                    $('.mainactionBox_left span a').css('visibility', 'hidden');
                                                }
                                                else {
                                                    $('.mainactionBox_left').css('visibility', 'visible');
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
                                            'projectID': $('#<%=ddlProjects.ClientID%>').val()
                                        });

                                        function shopPop() {
                                            jQuery("#tdContext").find("[data-toggle='popover']").popover({
                                                html: true,
                                                trigger: "hover",
                                                delay: { show: 0, hide: 500 },
                                                placement: function (target, source) {
                                                    var popoverObj = this;
                                                    var $day = jQuery(source).closest("ul.monthlyview>li");
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
                                            InitEvents({ "d": calendarDate, "t": 2, "y": y }, calendarMonth);
                                        }

                                        function forwardClick(y) {
                                            InitEvents({ "d": calendarDate, "t": 1, "y": y }, calendarMonth);
                                        }

                                        function refreshParent() {
                                            InitEvents({ "d": calendarDate, "t": 0 }, calendarMonth, true);
                                        }

                                        function gotoToday() {
                                            if (calendarMonth == '<%=DateTime.Now.ToString("MMMM_yyyy")%>')
                                                location.href = "#today"
                                            else {
                                                InitEvents({}, calendarMonth, 2);
                                            }
                                        }
                                    </script>
                                    <td id="tdContext"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <div class="modal fade" id="divTopInviteEventList">
    </div>
    <div class="modal fade" id="divDayInviteEventList" data-refresh="true">
    </div>
    <script type="text/javascript">
        function OpenAddModuleDialog(day) {
            var result = ShowIFrame("/Sunnet/Events/AddEvent.aspx?Date=" + day + "&" + "pid=" + $('#<%=ddlProjects.ClientID%>').val(), 460, 470, true, "Add Schedules");
            if (!result) {
                $('#iBtnSearch').click();
            }
        }

        function OpenEditSchdules(id, canEdit) {
            var result = ShowIFrame("/Sunnet/Events/EditEvent.aspx?ID=" + id + "&c=" + canEdit, 383, 430, true, "Edit Schedules");
            if (result == 0) {
                $('#iBtnSearch').click();
            }
        }

        function clickDayEventList(d) {
            jQuery("#divDayInviteEventList").modal("refresh", "/Sunnet/Events/DayEvents.aspx?date=" + d + "&ProjectID=" + $('#<%=ddlProjects.ClientID%>').val() + "&r=" + Math.random());
        }

        $(function () {

            $('#iBtnSearch').on('click', function (event) {
                //call back to refresh calendar
                event.preventDefault();
                var $tdContent = $('#tdContext')
                $tdContent.css('display', 'none');
                $tdContent.html('');

                InitEvents({
                    'year': $('#<%=ddlYears.ClientID%>').val(),
                    'month': $('#<%=ddlMonths.ClientID%>').val(),
                    'projectID': $('#<%=ddlProjects.ClientID%>').val()
                });
                $tdContent.css('display', 'block');
            });
        });
    </script>

</asp:Content>
