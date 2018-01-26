<%@ Page Title="My Timesheet" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Timesheet.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    @media (max-width:992px) {
        .toploginInfo{
            min-width:auto;
        }
        .topBox{
            min-width:auto;
            height:auto;
        }
        .topBox_logo img{
            width:100%;
        }
        ul.topmenu li {
            width: 80px;
            height: 55px;
            font-size: 12px;
            font-weight: 500;
            padding-top: 15px;
            margin-right: 5px;
        }
        ul.topmenu li .image{
            width: 19px;
            height: 20px;
            background-size: 100% 100% !important;
        }
        .mainleftTd{
            width:193px;
        }
        .leftmenuBox{
            width:180px;
        }
        .mainrightBox {
            min-width: auto;
            padding:10px;
        }
        .footerBox{
            min-width:auto;
        }
        .footerBox_left{
            width:auto;
        }
        .footerBox_right{
            width:auto;
        }
        select.middle{
            width:160px;
        }
        .tabletimesheet th{
            width:50px !important;
        }
        .tabletimesheet th.w-d{
            width:auto !important;
        }
        .table-advance tbody td{
            word-break:break-all;
        }
    }
    </style>
    <script type="text/javascript">
        var SelectedDates = [];
        var SelectedDate = "";
        var Today = '<%=Today.ToString("yyyy-MM-dd") %>';
        var LimitDate = '<%=SunNet.PMNew.Entity.TimeSheetModel.TimeSheetTicket.LimitDate.ToString("yyyy-MM-dd") %>';
        var EditableDate = '<%=EditableDate.ToString("yyyy-MM-dd") %>';
        var GetDataUrl = '/Service/TimeSheet.ashx';
        var waitingList = [];
        var loadedList = {};

        var isAjax = false;
        function StartLoadData() {
            if (!waitingList.length) {
                isAjax = false;
                return;
            }
            var date = waitingList[0];
            if (loadedList[date]) {
                AppendHtml(loadedList[date]);
                waitingList = waitingList.slice(1);
            }
            else if (!isAjax) {
                isAjax = true;
                jQuery.getJSON(GetDataUrl, {
                    action: "gettimesheetsbyweek",
                    date: date
                }, function (dateTimesheets) {
                    if (!dateTimesheets || dateTimesheets.length == 0) {
                        dateTimesheets = {
                            date: date,
                            totalHours: 0,
                            timesheets: [{
                                project: { title: "" },
                                ticket: { title: "", id: "" },
                                workDetail: "",
                                submitted: false,
                                hours: 0
                            }]
                        };
                    }
                    loadedList[date] = dateTimesheets;
                    AppendHtml(dateTimesheets);
                    isAjax = false;
                    waitingList = waitingList.slice(1);
                    StartLoadData();
                });
            }
        }

        function AppendHtml(data) {
            $timesheetsContainer.append(TemplateEngine(GetTemplateHtml("temp_mytimesheet_week"), data));
        }

        function GetDateFromString(date) {
            var _thisday = new Date();
            var _thisYear = parseInt(date.substr(0, 4));

            var _thisMonth = date.substr(5, 2);
            if (_thisMonth.indexOf("0") == 0) {
                _thisMonth = _thisMonth.slice(1);
            }
            _thisMonth = parseInt(_thisMonth) - 1;

            var _thisDate = date.substr(8, 2);
            if (_thisDate.indexOf("0") == 0) {
                _thisDate = _thisDate.slice(1);
            }
            _thisDate = parseInt(_thisDate);
            _thisday = new Date(_thisYear, _thisMonth, _thisDate);
            return _thisday;
        }
        function GetDateFromDate(days, date) {
            var _thisday = GetDateFromString(date);
            var result = _thisday;
            result.setDate(_thisday.getDate() + days);
            return result;
        }

        function LoadData(date) {
            if (GetDateFromString(date) <= GetDateFromString(LimitDate)) {
                SelectedDates.push(date);
                SelectedDate = date;
                waitingList.push(date);
                StartLoadData();
            }
        }
        function LoadThisWeek() {
            LoadWeek(Today);
        }
        function LoadWeek(date) {
            $timesheetsContainer.find(".timesheetbox3").remove();
            $timesheetsContainer.find("table").remove();
            SelectedDates = [];
            var _thatDay = GetDateFromString(date);
            var _indexweek = _thatDay.getDay();
            if (_indexweek == 0) {
                _indexweek = 7;
            }
            for (var i = 1; i <= 7; i++) {
                var _days = -(_indexweek - i);
                var _week = GetDateFromDate(_days, date);
                LoadData(_week.Format("yyyy-MM-dd"));
            }
        }

        function CanEdit(date) {
            return GetDateFromString(date) >= GetDateFromString(EditableDate);
        }

        var $timesheetsContainer;
        jQuery(function () {
            $timesheetsContainer = jQuery("#timesheetsContainer");
            LoadThisWeek();
            SelectedDate = Today;

            jQuery("#calendarContainer").calendar({
                headerClass: "tmcalendar",
                weekClass: "tmcalendarWeek",
                daysClass: "tmcalendarDay",
                notThisMonthClass: "nothismonthy",
                thisMonthDayClass: "thismonth",
                todayClass: "ontoday",
                monthFormat: "short",
                firstDayMonday: false,
                prevMonth: "<div class='tmcalendarArrow'><img src='/images/monthleft.png'></div>",
                nextMonth: "<div class='tmcalendarArrow'><img src='/images/monthright.png'></div>",
                defaultDate: Today
            });
            jQuery("body").on("choosen.sunnet.calendar", "#calendarContainer", function (event) {
                var choosenDate = event.date.Format("yyyy-MM-dd");
                LoadWeek(choosenDate);
            });
        });
        function getClass(index, expectedValue, expectedClass) {
            if (index % 2 == expectedValue)
                return expectedClass;
            return "";
        }
        function showMove(hours) {
            return Number(hours) > 0;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <script type="text/html" id="temp_mytimesheet_week">
        <div class="timesheetbox3">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="timesheetTitle1">Timesheet: {% GetDateFromString(this.date).Format("MM/dd/yyyy") %} (Total Hours: <span class="{% this.totalHours < 8 ? 'fdNotice' : '' %}">{% this.totalHours %}</span>)</td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance tabletimesheet">
            <thead>
                <tr>
                    <th width="120">Project</th>
                    <th width="150">Title</th>
                    <th width="50">ID</th>
                    <th width="*" class="w-d">Work Detail</th>
                    <th width="40" class="aligncenter">Hours</th>
                    <th width="60" class="aligncenter">Submitted</th>
                    <th width="60" class="aligncenter">Action</th>
                </tr>
            </thead>
            <tbody>
                {% if (this && this.timesheets && this.timesheets.length) { %}
                    {% for(var i = 0; i < this.timesheets.length ; i++) {%}
                    <tr class="{% getClass(i,1,'whiterow') %}">
                        <td>{% this.timesheets[i].project.title %}</td>
                        <td>{% this.timesheets[i].ticket.title %}</td>
                        <td>{% this.timesheets[i].ticket.id %}</td>
                        <td>{% this.timesheets[i].workDetail %}</td>
                        <td class="aligncenter">{% this.timesheets[i].hours %}</td>
                        <td class="aligncenter">{% this.timesheets[i].submitted ? "Yes" :"No" %}</td>
                        <td class="aligncenter">{% if (this.timesheets[i].submitted) { %}
                            <img src="/images/icons/userselected.png" alt="Submitted" title="Submitted">
                            {% } else { %}
                                {% if (CanEdit(this.date)) { %}
                                <a href="Date.aspx?date={% this.date %}&edit={% this.timesheets[i].id %}">
                                    <img src="/images/icons/edit.png" title="Edit" />
                                </a>
                            <%--{% if(showMove(this.timesheets[i].hours)) { %}
                            <a href="MoveTimeSheet.aspx?tmid={% this.timesheets[i].id %}"
                                data-target='#modalsmall' data-toggle='modal'>
                                <img src="/images/icons/waiting_on.png" title="Move to another day" />
                            </a>
                            {% } %}--%>
                            {% } else { %}
                                <img src="/images/icons/passtime.png" title="Too late" />
                            {% } %}
                            {% } %}
                        </td>
                    </tr>
                {% } %}
                {% } %}
            </tbody>
        </table>
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top" id="timesheetsContainer">
                <div style="word-break: break-all;">
                    <asp:PlaceHolder ID="phlUncompleted" runat="server" Visible="false">You have the following timesheet(s) to submit. They are:
                                <br />
                        <asp:Literal ID="ltlUnfinished" runat="server"></asp:Literal>
                        <br />
                        <asp:Literal ID="ltlUnSubmitted" runat="server"></asp:Literal>
                    </asp:PlaceHolder>
                </div>
            </td>
            <td width="20">&nbsp;</td>
            <td width="185" valign="top">
                <table border="0" cellspacing="0" cellpadding="0" class="calendarTable">
                    <tr>
                        <td>
                            <div class="tmcalendarBox" id="calendarContainer">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
