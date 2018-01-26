<%@ Page Title="" Language="C#" MasterPageFile="Event.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SunNet.PMNew.PM2014.Event.Index" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">


</asp:Content>

    <asp:Content ID="Content1" runat="server" ContentPlaceHolderID="searchSection">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td width="30" align="right">Year:</td>
                    <td>
                        <asp:DropDownList ID="ddlYears" runat="server" CssClass="selectw1" Width="120px">
                            <asp:ListItem Value="1800">1800</asp:ListItem>
                            <asp:ListItem Value="2013">2013</asp:ListItem>
                            <asp:ListItem Value="2014">2014</asp:ListItem>
                            <asp:ListItem Value="2015">2015</asp:ListItem>
                            <asp:ListItem Value="2016">2016</asp:ListItem>
                            <asp:ListItem Value="2017">2017</asp:ListItem>
                            <asp:ListItem Value="2018">2018</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="55" align="right">Month:</td>
                    <td>
                        <asp:DropDownList ID="ddlMonths" runat="server" CssClass="selectw1" Width="120px">
                            <asp:ListItem Value="1">January</asp:ListItem>
                            <asp:ListItem Value="2">February</asp:ListItem>
                            <asp:ListItem Value="3">March</asp:ListItem>
                            <asp:ListItem Value="4">April</asp:ListItem>
                            <asp:ListItem Value="5">May</asp:ListItem>
                            <asp:ListItem Value="6">June</asp:ListItem>
                            <asp:ListItem Value="7">July</asp:ListItem>
                            <asp:ListItem Value="8">August</asp:ListItem>
                            <asp:ListItem Value="9">September</asp:ListItem>
                            <asp:ListItem Value="10">October</asp:ListItem>
                            <asp:ListItem Value="11">November</asp:ListItem>
                            <asp:ListItem Value="12">December</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="60" align="right">Project:</td>
                    <td>
                        <asp:DropDownList ID="ddlProjects" runat="server" class="selectw1">
                        </asp:DropDownList>
                    </td>
                    <td width="55" align="right">User:</td>
                    <td>
                        <asp:DropDownList ID="ddlUser" runat="server" class="selectw1" DataValueField="ID" DataTextField="FirstAndLastName">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hiUserIds" runat="server"/>
                    </td>
                    <td width="30">
                        <input name="iBtnSearch" type="submit" class="searchBtn" value=" " id="iBtnSearch"></td>
                    <td width="30">

                        <a href="/do/iCalFeed.ashx" class="exportBtn" target="_blank">&nbsp;</a>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td width="140">
                        <ul class="listtopBtn">
                            <li href="Add.aspx" data-target="#modalsmall" data-toggle="modal">
                                <div class="listtopBtn_icon">
                                    <img src="/images/icons/wevents.png" />
                                </div>
                                <div class="listtopBtn_text">Create Event </div>
                            </li>
                        </ul>
                    </td>
                    <td align="center">
                        <table border="0" cellspacing="0" cellpadding="0">
                            <tbody>
                                <tr>
                                    <td align="center" style="width: 40px;">
                                        <a href="javascript:void(0);">
                                            <img src="/images/monthleft2.png" onclick="backClick(1)" title="Previous Year" /></a></td>
                                    <td align="center" style="width: 40px;">
                                        <a href="javascript:void(0);">
                                            <img src="/images/monthleft.png" onclick="backClick(0)" title="Previous Month" />
                                        </a></td>
                                    <td align="center"id="spnMonth"><%=DateTitle%></td>
                                    <td style="/*width: 28px;*/"></td>
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
                            </tbody>
                        </table>
                    </td>
                    <td width="100" align="right"><span class="topbtn toptodayBtn" onclick="gotoToday();">&gt;&gt; Today</span></td>
                </tr>
            </tbody>
        </table>
    </div>

        
<style type="text/css">
    .profitWarning {
        color: red;
    }

    .majorHighlight {
        color: blue;
    }
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
        .mainrightBox{
            min-width:auto;
            padding:10px;
        }
        .searchItembox table{
            width:100%;
        }
        .selectw1 {
            width: 100px !important;
        }
        .searchItembox td {
            padding: 0px;
        }
        ul.monthlyviewweek,ul.monthlyview{
            width:100%;
        }
        ul.monthlyview li,ul.monthlyviewweek li{
            width:14%;
        }
        .footerBox{
            min-width:auto;
        }
        .footerBox_left,.footerBox_right{
            width:auto;
        }
    }
</style>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="dataSection">
    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="monthlyviewBox">
        <tbody> 
            <tr>
                <td>
                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="monthlyviewBox">
                        <tbody> 
                            <tr>
                                <td>
                                    <ul class="monthlyviewweek">
                                        <li>Sunday</li>
                                        <li>Monday</li>
                                        <li>Tuesday</li>
                                        <li>Wednesday</li>
                                        <li>Thursday</li>
                                        <li>Friday</li>
                                        <li>Saturday</li>
                                        
                                    </ul>
                                    <script type="text/html" id="day_template_Items">
                                        <!-- ko if: $index() < 3 -->
                                        <li data-bind="attr: { 'id': $root.createEventId(item) }">
                                                 <img data-bind="attr: { src: Icon }" style='width: 15px; height: 15px; vertical-align: text-bottom;' />
                                            <a data-bind="attr: { href: $root.getEventUrl(item) }, text: Title" data-target='#modalsmall' data-toggle='modal'></a></li>
                                        <!-- /ko -->
                                        <!-- ko if: $index() == 3 -->   
                                        <li><a style="cursor: pointer; color: #15c" data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: $root.getUrl(d) }">More</a></li>
                                        <!-- /ko -->
                                    </script>
                                    <script type="text/html" id="day_template_0">
                                        <li class="dateDisable" data-bind="text: Day">
                                            <ul class="onscdTicket" data-bind='template: { name: "day_template_Items", foreach: list, as: "item" }'>
                                                <li>More</li>
                                            </ul>
                                        </li>
                                    </script>
                                    <script type="text/html" id="day_template_1">
                                        <li>
                                            <div class='dateforclick' data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: $root.getUrl(d) }, text: Day"></div>
                                            <ul class="onscdTicket" data-bind='template: { name: "day_template_Items", foreach: list, as: "item" }'>
                                            </ul>
                                        </li>
                                    </script>
                                    <script type="text/html" id="day_template_2">
                                        <li>    
                                            <div class='dateforclick' data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: $root.getUrl(d) }, text: Day"></div>
                                            <!-- ko if:$root.isOwner -->
                                            <img src="/images/icons/add1.png" data-bind="attr: { href: $root.getCreateUrl(d) } " style="cursor: pointer" align="right" title='Create Event' data-toggle="modal" data-target="#modalsmall">
                                            <!-- /ko -->
                                            <ul class="onscdTicket" data-bind='template: { name: "day_template_Items", foreach: list, as: "item" }'>
                                            </ul>
                                            <div data-bind="attr: { href: $root.getCreateUrl(d) }, style: { paddingTop: $root.getTop(d), 'width': '150px', 'height': $root.getheight(d) }" data-target="#modalsmall" data-toggle="modal"></div>
                                        </li>
                                    </script>
                                    <script type="text/html" id="day_template_3">
                                        <li class="dateToday"><a name='today'></a>
                                            <div class='dateforclick' data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: $root.getUrl(d) }, text: Day"></div>
                                            <!-- ko if:$root.isOwner -->
                                            <img src="/images/icons/add1.png" data-bind="attr: { href: $root.getCreateUrl(d) } " style="cursor: pointer" align="right" title='Create Event' data-toggle="modal" data-target="#modalsmall">
                                            <!-- /ko -->
                                            <ul class="onscdTicket" data-bind='template: { name: "day_template_Items", foreach: list, as: "item" }'>
                                            </ul>
                                            <div data-bind="attr: { href: $root.getCreateUrl(d) }, style: { paddingTop: $root.getTop(d), 'width': '150px', 'height': $root.getheight(d) }" data-target="#modalsmall" data-toggle="modal"></div>
                                        </li>
                                    </script>
                                    <script type="text/html" id="day_template_4">
                                        <li class="dateDisable">
                                            <div class='dateforclick' data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: $root.getUrl(d) }, text: Day"></div>
                                            <!-- ko if:$root.isOwner -->
                                            <img src="/images/icons/add1.png" data-bind="attr: { href: $root.getCreateUrl(d) } " style="cursor: pointer" align="right" title='Create Event' data-toggle="modal" data-target="#modalsmall">
                                            <!-- /ko -->
                                            <ul class="onscdTicket" data-bind='template: { name: "day_template_Items", foreach: list, as: "item" }'>
                                            </ul>
                                            <div data-bind="attr: { href: $root.getCreateUrl(d) }, style: { paddingTop: $root.getTop(d), 'width': '150px', 'height': $root.getheight(d) }" data-target="#modalsmall" data-toggle="modal"></div>
                                        </li>
                                    </script>
                                    <ul class="monthlyview" data-bind='template: { name: dayTemplate, foreach: days, as: "d" }'>
                                    </ul>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>

    <script type="text/javascript">
        var months;

        $(function () {
            $.ajaxSetup({
                beforeSend: function () {
                    doubleClick = false;
                },
                complete: function () {
                    doubleClick = true;
                }
            });

            InitEvents({
                'projectID': $('#<%=ddlProjects.ClientID%>').val(),
                'userId': $('#<%=ddlUser.ClientID%>').val()
            });

            $('#iBtnSearch').on('click', function (event) {
                //call back to refresh calendar
                event.preventDefault();

                InitDefault();
            });

            jQuery("body").off("hidden.bs.modal", ".modal");
            jQuery("body").on("hidden.bs.modal", ".modal", function () {
                var $ifame = $(this).find("iframe");
                if ($ifame.attr("src") && $ifame[0].contentWindow.urlParams.close) {
                    InitDefault();
                }
            });

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


        var calendarDate;
        var calendarMonth;
        var doubleClick = true;
        var IsPageLoad = true;
        var viewModel = {};

        function InitDefault() {
            InitEvents({
                'year': $('#<%=ddlYears.ClientID%>').val(),
                'month': $('#<%=ddlMonths.ClientID%>').val(),
                'projectID': $('#<%=ddlProjects.ClientID%>').val()
            });
        }

        function InitEvents(para, oldId, r) {
            if (!para["userId"]) {
                para['userId'] = $('#<%=ddlUser.ClientID%>').val();
            }
            para['allUser'] = $("#<% = hiUserIds.ClientID%>").val();
            if (!doubleClick) return;
            $.getJSON("/Do/DoGetCalendarList.ashx?" + Math.random(), para, function (result) {
                if (IsPageLoad) {
                    viewModel = {
                        getUrl: function (day) {
                            return "DayEvent.aspx?date=" + day.StrDate + "&ProjectID="
                                + $('#<%=ddlProjects.ClientID%>').val()
                                + "&userId=" + $('#<%=ddlUser.ClientID%>').val()
                                + "&allUser=" + $("#<% = hiUserIds.ClientID%>").val()
                                + "&r=" + Math.random();
                        },
                        getCreateUrl: function (day) {
                            return "Add.aspx?Date=" + day.StrDate + "&pid=" + $('#<%=ddlProjects.ClientID%>').val() + "&r=" + Math.random();
                        },

                        days: ko.observableArray(result.list),

                        dayTemplate: function (day) {
                            switch (day.Type) {
                                case 0:
                                    return "day_template_0";
                                case 1:
                                    return "day_template_1";
                                case 2:
                                    return "day_template_2";
                                case 3:
                                    return "day_template_3";
                                case 4:
                                    return "day_template_4";
                            }
                        },

                        createEventId: function (item) {
                            return "liCalendarEvent" + item.ID;
                        },

                        getEventUrl: function (item) {
                            return "Edit.aspx?ID=" + item.ID + "&r=" + Math.random();
                        },

                        getTop: function (day) {
                            return (day.list.length * 20 + 20) + "px";
                        },

                        getheight: function (day) {
                            return (105 - day.list.length * 20 - 20) + "px";
                        },

                        ShowMore: function (index) {
                            return index() >= 3;
                        },

                        NoShowMore: function (index) {
                            return index() < 3;
                        },

                        isOwner: ko.observable(result.isOwner)
                    };

                        ko.applyBindings(viewModel);
                        IsPageLoad = false;
                    }
                    else {
                        viewModel.days(result.list);
                        viewModel.isOwner(result.isOwner);
                    }

                var d = result.list;
                calendarDate = d[8].StrDate;
                //var full_month = d[8].Month.split("_");
                //var formatStr = handleMonthFormat(full_month[0]) + " " + full_month[1];
                //$("#spnMonth").text(formatStr);
                jQuery("#spnMonth").html(d[8].Month.replace("_", " "));
                calendarMonth = d[8].Month;

                if (result.isOwner == false) {
                    $('.mainactionBox_left span a').css('visibility', 'hidden');
                }
                else {
                    $('.mainactionBox_left').css('visibility', 'visible');
                }

                if (r) {
                    if (r == 2) //today
                        location.href = "#today";
                }

                $('#<%=ddlYears.ClientID%>').val(d[5].StrDate.substring(6));
                if ($("#<%=ddlYears.ClientID%>").get(0).selectedIndex < 0) {
                    $("#<%=ddlYears.ClientID%>").append("<option value='" + d[5].StrDate.substring(6) + "'>" + d[5].StrDate.substring(6) + "</option>");
                    $('#<%=ddlYears.ClientID%>').val(d[5].StrDate.substring(6));
                }
                $('#<%=ddlMonths.ClientID%>').val(parseInt(d[10].StrDate.substring(0, 2)));
            });
        }


        function backClick(y) {
            InitEvents({ "d": calendarDate, "t": 2, "y": y }, calendarMonth);
        }

        function forwardClick(y) {
            InitEvents({ "d": calendarDate, "t": 1, "y": y }, calendarMonth);
        }

        function gotoToday() {
            if (calendarMonth == '<%=DateTime.Now.ToString("MMMM_yyyy")%>')
                location.href = "#today"
            else {
                InitEvents({
                    'ProjectID': $('#<%=ddlProjects.ClientID%>').val(),
                    'userId': $('#<%=ddlUser.ClientID%>').val()
                }, calendarMonth, 2);
            }
        }

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

        window.onload = function () {
            var ele = $("table.monthlyviewBox td ul.monthlyview li.dateDisable");
            if (ele.size() > 0) {
                ele.each(function (i, x) {
                    var th = $(this);
                    var thTxt = parseInt(th.text());
                    th.html("<div>" + thTxt + "</div>");
                });
            }
        };
    </script>
</asp:Content>





