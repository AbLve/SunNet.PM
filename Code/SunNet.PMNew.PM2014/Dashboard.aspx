<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/NoLeftmenu.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SunNet.PMNew.PM2014.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dateToday {
           background-color: #fff4ca !important;   
        }
        table.cdmonthlyviewBox ul.cdmonthlyview li span {
                padding-left:8px;
        }
          table.cdmonthlyviewBox ul.cdmonthlyview li div.cddateforclick {
                padding-left:8px;
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
                    <div class="cdcontentTitle">Tracking Tickets</div>
                </div>
                <div class="cdashboardIconbox">
                    <ul class="cdashboardItem">
                        <a href="/Ticket/New.aspx">
                            <li>
                                <div class="cdashboardIcon">
                                    <img src="/Images/icons/pd_issue.png" />
                                </div>
                                <br />
                                Submit a Ticket</li>
                        </a><a href="/Ticket/Waiting.aspx">
                            <li>
                                <div class="cdashboardIcon">
                                    <% if (Tickets["waitting"] > 0)
                                       { %>
                                    <div class="pdreddot"><%= Tickets["waitting"] %></div>
                                    <% } %>
                                    <img src="/Images/icons/pd_response.png" width="60" height="60" />
                                </div>
                                <br />
                                Waiting for Response<br />
                            </li>
                        </a><a href="/Ticket/Ongoing.aspx">
                            <li>
                                <div class="cdashboardIcon">
                                    <% if (Tickets["ongoing"] > 0)
                                       { %>
                                    <div class="pdreddot"><%= Tickets["ongoing"] %></div>
                                    <% } %>
                                    <img src="/Images/icons/pd_ongoing.png" />
                                </div>
                                <br />
                                Ongoing Tickets<br />
                            </li>
                        </a>
                        <a href="/Ticket/Cancelled.aspx">
                            <li>
                                <div class="cdashboardIcon">
                                    <img src="/Images/icons/pd_cancelled.png" width="60" height="60" border="0" />
                                </div>
                                <br />
                                Cancelled Tickets<br />
                            </li>
                        </a><a href="/Ticket/Draft.aspx">
                            <li>
                                <div class="cdashboardIcon">
                                    <% if (Tickets["drafted"] > 0)
                                       { %>
                                    <div class="pdreddot"><%= Tickets["drafted"] %></div>
                                    <% } %>
                                    <img src="/Images/icons/pd_draft.png" width="60" height="60" border="0" />
                                </div>
                                <br />
                                Drafted Tickets<br />
                            </li>
                        </a><a href="/Ticket/Completed.aspx">
                            <li>
                                <div class="cdashboardIcon">
                                    <img src="/Images/icons/pd_completed.png" width="60" height="60" border="0" />
                                </div>
                                <br />
                                Completed Tickets<br />
                            </li>
                        </a><a href="/Ticket/Report.aspx">
                            <li>
                                <div class="cdashboardIcon">
                                    <img src="/Images/icons/pd_reports.png" />
                                </div>
                                <br />
                                Tickets Report<br />
                            </li>
                        </a>
                    </ul>
                </div>
            </td>
            <td width="20">&nbsp;</td>
            <!-- events -->
            <td width="49%" class="cdmainBox">
                <div class="cdcontentTop">
                    <div class="cdcontentTitle">Upcoming Activities </div>
                    <div class="cdMore"><a href="/Event/Index.aspx" target="targetDashboard">More&gt;&gt;</a></div>
                </div>
                <div class="cdtopbtnbox">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="138">
                                <ul class="listtopBtn">
                                    <li href="/Event/Add.aspx" data-target="#modalsmall" data-toggle="modal">
                                        <div class="listtopBtn_icon">
                                            <img src="/images/icons/wevents.png" />
                                        </div>
                                        <div class="listtopBtn_text" id="dvCreateEvent">Create Event </div>
                                    </li>
                                </ul>
                            </td>
                            <td width="*">
                                <table border="0" align="center" cellpadding="0" cellspacing="0" style="padding-left:110px;">
                                    <tr>
                                        <td align="center" style="width: 40px;">
                                            <a  href="javascript:void(0);">
                                                <img src="/images/monthleft2.png" onclick="backClick(1)" title="Previous Year" /></a></td>
                                        <td align="center" style="width: 40px;">
                                            <a href="javascript:void(0);">
                                                <img src="/images/monthleft.png" onclick="backClick(0)" title="Previous Month" />
                                            </a></td>
                                        <td align="center" id="spnMonth" style="min-width:110px;padding-top:8px;"><%=DateTime.Now.ToString("MMMM yyyy") %></td>
                             <%--           <td align="center" style="width: 28px;"></td>--%>
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
                            <td width="200" align="right" style="padding-right: 7px;"><%--<span class="topbtn cdtopBtn cdtopBtn-active" onclick="gotoToday()" id="spanCalendar">Calendar</span>--%>
                                <span class="topbtn cdtopBtn" onclick="gotoToday()" id="spanList">Today</span></td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hiUserIds" runat="server" />
                <!--edit Calendar-->


                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="cdmonthlyviewBox">
                    <tbody>
                        <tr>
                            <td>

                                <ul data-bind="foreach: getWeekForeach" class="cdmonthlyviewweek">
                                    <li data-bind="text: $data"></li>
                                </ul>
                                <ul class="cdmonthlyview" data-bind='template: { name: selectDayTemplate, foreach: days, as: "arrays" }'></ul>

                                <script type="text/html" id="dayT_base">
                                    <li style="width: 70px; height: 12px">
                                        <img data-bind="attr: { src: Icon }" style='width: 15px; height: 15px;' />
                                        <a data-bind="attr: { href: $root.getEventUrl(DataN), }, text: Title, style: { width: '80px' }" data-target='#modalsmall' data-toggle='modal'></a>
                                    </li>
                                </script>
                                    
                                <script type="text/html" id="dayT_0">
                                    <li class="cddateDisable" data-bind="text: Day">
                                        <ul class="cdmonthlyview cdonscdTicket" data-bind='template: { name: "dayT_base", foreach: list, as: "DataN" }'></ul>
                                    </li>
                                </script>

                                <script type="text/html" id="dayT_1">
                                    <li class='cddateforclick'>
                                        <span data-bind=" text: Day">&nbsp;</span>
                                        <div class='cddateforclick' data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: $root.getUrl(arrays), text: Day }"></div>
                                        <ul class="cdmonthlyview cdonscdTicket" data-bind='template: { name: "dayT_base", foreach: list, as: "DataN" }'></ul>
                                    </li>
                                </script>

                                <script type="text/html" id="dayT_2">
                                    <li>
                                        <div class='cddateforclick' data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: $root.getUrl(arrays) }, text: Day"></div>
                                        <img src="/images/icons/add1.png" data-bind="attr: { href: $root.getCreateUrl(arrays) } " style="cursor: pointer" align="right" title='Create Event' data-toggle="modal" data-target="#modalsmall" />
                                        <ul class="cdmonthlyview cdonscdTicket" data-bind='template: { name: "dayT_base", foreach: list, as: "DataN" }'></ul>
                                        <div data-bind="attr: { href: $root.getCreateUrl(arrays) }, style: { paddingTop: $root.getTop(arrays), 'width': '82px', 'height': $root.getheight(arrays), 'cursor': 'pointer' }" title='Create Event' data-toggle="modal" data-target="#modalsmall"></div>
                                    </li>

                                </script>

                                <script type="text/html" id="dayT_3">
                                    <li class="dateToday"><a name='today'></a>
                                        <div class='cddateforclick' data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: $root.getUrl(arrays) }, text: Day"></div>
                                        <img src="/images/icons/add1.png" data-bind="attr: { href: $root.getCreateUrl(arrays) } " style="cursor: pointer" align="right" title='Create Event' data-toggle="modal" data-target="#modalsmall" />
                                        <ul class="cdmonthlyview cdonscdTicket" data-bind='template: { name: "dayT_base", foreach: list, as: "DataN" }'></ul>
                                        <div data-bind="attr: { href: $root.getCreateUrl(arrays) }, style: { paddingTop: $root.getTop(arrays), 'width': '82px', 'height': $root.getheight(arrays), 'cursor': 'pointer' }" title='Create Event' data-toggle="modal" data-target="#modalsmall"></div>
                                    </li>

                                </script>
                                <script type="text/html" id="dayT_4">
                                    <li class="cddateDisable">
                                        <div class='cddateforclick' data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: $root.getUrl(arrays) }, text: Day"></div>
                                        <img src="/images/icons/add1.png" data-bind="attr: { href: $root.getCreateUrl(arrays) } " style="cursor: pointer" align="right" title='Create Event' data-toggle="modal" data-target="#modalsmall">
                                        <ul class="cdmonthlyview cdonscdTicket" data-bind='template: { name: "dayT_base", foreach: list, as: "DataN" }'></ul>
                                        <div data-bind="attr: { href: $root.getCreateUrl(arrays) }, style: { paddingTop: $root.getTop(arrays), 'width': '82px', 'height': $root.getheight(arrays), 'cursor': 'pointer' }" title='Create Event' data-toggle="modal" data-target="#modalsmall"></div>
                                    </li>

                                </script>

                            </td>
                        </tr>
                    </tbody>
                </table>

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

    <script type="text/javascript">
        var calendarDate;
        var calendarMonth;
        var viewModel = {};
        var userId = "<%=UserInfo.UserID%>";
        var IsPageLoad = true;

        $(function () {
            InitCalendar({ 'userId': userId });
        });

        window.onload = function () {
            $("table.cdmonthlyviewBox td ul.cdmonthlyview li.cddateDisable").each(function (i, x) {
                var th = $(this);
                th.html("<span>" + th.text() + "</span>");
            });
        };

        function InitCalendar(param) {
            if (!param["userId"]) {
                param['userId'] = "<%=UserInfo.UserID%>";
            }
            $.getJSON("/Do/DoGetCalendarList.ashx?timestamp=" + Math.random(), param, function (result) {
                if (IsPageLoad) {
                    viewModel = {
                        getWeekForeach: ['Sun', 'Mon', 'Tues', 'Wed', 'Thurs', 'Fri', 'Sat'],
                        days: ko.observableArray(result.list),

                        selectDayTemplate: function (days) {
                            switch (days.Type) {
                                case 0: return "dayT_0";
                                case 1: return "dayT_1";
                                case 2: return "dayT_2";
                                case 3: return "dayT_3";
                                case 4: return "dayT_4";
                            }
                        },

                        getEventUrl: function (DataN) {
                            if (DataN.IsEdit) {
                                return "/Event/Edit.aspx?ID=" + DataN.ID + "&r=" + Math.random();
                            }
                            else {
                                return "/Event/View.aspx?ID=" + DataN.ID + "&r=" + Math.random();
                            }
                        },

                        getUrl: function (DataN) {
                            return "/Event/DayEvent.aspx?date=" + DataN.StrDate + "&r=" + Math.random() + "&allUser=" + $("#<% = hiUserIds.ClientID%>").val();
                        },
                        getCreateUrl: function (DataN) {
                            return "/Event/Add.aspx?Date=" + DataN.StrDate + "&r=" + Math.random();
                        },

                        getTop: function (day) {
                            return (day.list.length * 40 + 40) + "px";
                        },

                        getheight: function (day) {
                            return (38 - day.list.length * 20 - 20) + "px";
                        }

                    };
                        ko.applyBindings(viewModel);
                        IsPageLoad = false;
                    } else {
                        viewModel.days(result.list);
                    }

                var allData = result.list;
                calendarDate = allData[8].StrDate;
                calendarMonth = allData[8].Month;
                jQuery("#spnMonth").html(allData[8].Month.replace("_", " "));

            });
            }


            function backClick(y) {
                InitCalendar({ "d": calendarDate, "t": 2, "y": y }, calendarMonth);
            }

            function forwardClick(y) {
                InitCalendar({ "d": calendarDate, "t": 1, "y": y }, calendarMonth);
            }

            function gotoToday() {
                if (calendarMonth == '<%=DateTime.Now.ToString("MMMM_yyyy")%>')
                    location.href = "#today";
                else {
                    InitCalendar({ 'userId': userId });
                }
            }


    </script>
    <!--*******************************************************-->
    <!--*******************************************************-->

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
            function getClass(index, expectedValue, expectedClass) {
                if (index % 2 == expectedValue)
                    return expectedClass;
                return "";
            }
            function TopicForumsRedirect() {
                $.post("/forums/login/pm/finish", function (data) {
                });
            }
       

        jQuery(function () {
           
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
        });

    </script>
</asp:Content>
