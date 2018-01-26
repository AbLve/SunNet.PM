<%@ Page Title="" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="Date.aspx.cs" Inherits="SunNet.PMNew.PM2014.Timesheet.Date" %>

<%@ Import Namespace="SunNet.PMNew.PM2014.Timesheet" %>

<%@ Register Src="/UserControls/Messager.ascx" TagName="Messager" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .mainrightBox {
            min-width: 1080px;
        }

        .errorTr td {
            background: #ffecec;
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
    }
    </style>
    <script src="/Scripts/knockout-3.1.0.debug.js"></script>
    <script src="/Scripts/knockout.mapping-latest.debug.js"></script>
    <script type="text/javascript">
        function Ticket(obj) {
            this.title = obj && obj.title || "<%= DefaulSelectText%>";
            this.description =  obj && obj.description || "";
            this.id =  obj && obj.id || "<%= DefaulSelectText%>";
            this.pct =  obj && obj.pct || 0;
        }
        function Timesheet(project,ticket,id,hours,workDetail,isEdit,submitted) {
            if (!project.tickets) project.tickets = [];
            this.id = id;
            this.project = project;
            this.ticket = ko.observable(ticket);
            var d = new Date();
            var date = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
            if (new Date('<%=SelectedDate.Date%>') <= new Date(date)) {
                if (hours == 0)hours = null;
            }
            this.hours = ko.observable(hours);
            this.workDetail = workDetail;
            this.isEdit = ko.observable(isEdit);
            this.submitted = submitted || false;
            this.valid = ko.observable(true);
        }
        function timesheetViewModel(options) {
            var self = this;
            this.closeTimeout = 3;

            this.ShowByAjax = ko.observable(true);
            this.date = options.date;
            this.dataset = ko.observableArray(options.timesheets);
            
            this.getObjectById = function(source,id,idfield) {
                var target = 0;
                if (!idfield) idfield = "id";
                if (source && source.length) {
                    for (var i = 0; i < source.length; i++) {
                        var val = source[i][idfield];
                        if (jQuery.isFunction(val))
                            val = val();
                        if (val == id) {
                            target = source[i];
                            break;
                        }
                    }
                }
                return target;
            }
            this.loadTickets = function(project,timesheet) {
                if (!project.loaded()) {
                    jQuery.getJSON("/Service/Timesheet.ashx", {
                        action: "getticketslistbyproject",
                        date: self.date,
                        project: project.id
                    }, function(tickets) {
                        if (tickets && tickets.length) {
                            for (var i = 0; i < tickets.length; i++) {
                                if (!self.getObjectById(project.tickets(),tickets[i].id)) {
                                    project.tickets.push(tickets[i]);
                                }
                            }
                        }
                        project.loaded(true);
                    });
                }
            };
            this.getProject= function(id) {
                var p = self.getObjectById(self.projects(), id);
                if (!p){
                    p = ko.observable(self.projects()[0]);
                }
                else {
                    p = ko.observable(p);
                }
                if (p().tickets().length < 1) {
                    p().tickets.push(self.getDefaultTicket());
                }
                p.subscribe(function(project) {
                    if (project.tickets().length < 1) {
                        project.tickets.push(self.getDefaultTicket());
                    }
                    if(!project.loaded()){
                        if (project.id() > 0) {
                            self.loadTickets(project);
                        } else{
                            //project.tickets.push(self.getDefaultTicket());
                            project.loaded(true);
                        }
                    }
                });
                return p;
            }

            this.getDefaultTicket = function() {
                return new Ticket();
            };
            this.getDefaultTimesheet = function() {
                var p = self.getProject();
                var t = self.getDefaultTicket();
                var ts =  new Timesheet(p, t, 0, 0, "", true,false);
                return ts;
            };
            var map = {
                create: function(item) {
                    item.data.loaded = false;
                    return ko.mapping.fromJS(item.data);
                }
            };
            var _ps = ko.mapping.fromJS(options.projects,map);
            this.projects =ko.observableArray(_ps());
            
            this.categories = ko.observableArray(options.categories);
            this.selectedCategory = ko.observable(this.categories()[0]);
            this.selectedCategory.subscribe(function(category) {
                self.refresh(true);
            });

            this.beginEdit = function(timesheet) {
                if(timesheet.isEdit()==false){
                    self.loadTickets(timesheet.project(), timesheet);
                    timesheet.isEdit(true); 
                }
            }
            this.validateTimesheet = function(timesheet) {
                if (timesheet.project().id()<=0) {
                    ShowMessage("Project is required.", "danger",self.closeTimeout);
                    return false;
                }
                if (isNaN(+(timesheet.ticket().id)) || +(timesheet.ticket().id) <= 0 ) {
                    ShowMessage("Ticket is required.", "danger",self.closeTimeout);
                    return false;
                }
                if (timesheet.hours() <= 0) {
                    ShowMessage("Hours must more than 0, please rewrite.", "danger",self.closeTimeout);
                    return false;
                }
                if (timesheet.hours() > 24 || self.totalHours()>24) {
                    ShowMessage("Hours must less than 24, please rewrite.", "danger",self.closeTimeout);
                    return false;
                }
                if (!timesheet.workDetail.trim()) {
                    ShowMessage("Work Detail required.", "danger",self.closeTimeout);
                    return false;
                }
                return true;
            };
            this.saveTimesheet = function(timesheet, callback) {
                if (self.validateTimesheet(timesheet) === false) {
                    timesheet.valid(false);
                    return false;
                }
                var type = timesheet.id > 0 ? "updatetimesheet" : "addtimesheet";
                jQuery.post("/Service/timesheet.ashx", {
                    action: type,
                    WorkDetail: timesheet.workDetail,
                    Hours: timesheet.hours,
                    Percentage: timesheet.ticket().pct,
                    IsMeeting: false,
                    IsSubmitted: false,
                    ProjectID: timesheet.project().id,
                    TicketID: timesheet.ticket().id,
                    TimeSheetID: timesheet.id,
                    SheetDate: self.date
                }, function(response) {
                    if (response.success) {
                        if (jQuery.isFunction(callback)) {
                            timesheet.isEdit(false);
                            callback();
                        } else {
                            timesheet.id = response.id;
                            timesheet.isEdit(false);
                        }
                        self.newIfNo();
                    } else {
                        if (response.length==0) {
                            ShowMessage("Login timed out", "danger", self.closeTimeout);
                            window.top.location.href = '/Login.aspx?returnurl='+"<%=Server.UrlEncode(Request.RawUrl)%>";
                            return false;
                        }
                        ShowMessage(response.msg, "danger", self.closeTimeout);
                    }
                }, 'json');
                return false;
            };
            this.newIfNo = function() {
                var exists = false;
                jQuery.each(self.dataset(), function(i, timesheet) {
                    if (timesheet.id < 1 && timesheet.isEdit()==true) {
                        exists = true;
                        return false;
                    }
                });
                if (exists) {
                } else {
                    this.new();
                }
            };
            this.new = function() {
                var ts = self.getDefaultTimesheet();
                self.dataset.push(ts);
            };
            this.deleteTimesheet = function(timesheet) {
                if (timesheet.id > 0) {
                    jQuery.confirm("Are you sure you want to delete the timesheet?", {
                        yesText:"Delete",
                        yesCallback: function() {
                            jQuery.post("/Service/timesheet.ashx", {
                                action: "deletetimesheet",
                                id: timesheet.id,
                                SheetDate: self.date
                            }, function(response) {
                                if (response.success) {
                                    self.dataset.remove(timesheet);
                                    if (timesheet.isEdit()) {
                                        self.newIfNo();
                                    }
                                } else {
                                    ShowMessage(response.msg, "danger");
                                }
                            }, 'json');
                        },
                        noText:"Cancel"
                    });
                } else {
                    self.dataset.remove(timesheet);
                    self.newIfNo();
                }
            }
            this.submit = function() {
                var $btnSubmit = jQuery("#<%=btnSubmit.ClientID%>");
                if (this.totalHours() > 24) {
                    ShowMessage("Total hours can not bigger than 24, please rewrite.", "danger",self.closeTimeout);
                    return false;
                }
                if ((this.totalHours()+<%= totalQWeeklyHours %> ) > 40) {
                    ShowMessage("Your weekly work time is over 40 hours, please contact your manager immediately.", "danger",self.closeTimeout);
                    return false;
                }
                <%--var interval = "<%=System.Configuration.ConfigurationManager.AppSettings["TimesheetHoursUserID"]%>";
                var userID = "<%=UserInfo.ID%>";
                var list = interval.toString().split(',');
                for (var i = 0; i < list.length; i++) 
                {
                    if(parseInt(userID) == parseInt(list[i]))
                    {
                        var weekHours = 0;
                        $.getJSON("/Service/Timesheet.ashx",{
                            action: "getWeekHours",
                            userID: userID
                        },function(data){
                            alert(data);
                            weekHours = data;
                        });
                        if(weekHours > 40)
                        {
                            ShowMessage("Your weekly work time is over 40 hours, please contact your manager immediately.", "danger",self.closeTimeout);
                            return false;
                        }
                    }
                }--%>
                var countinue = true;
                jQuery.each(self.dataset(), function(i, timesheet) {
                    if(timesheet.isEdit() == true
                        && timesheet.project().id()>0
                        && timesheet.ticket().id>0
                        ){
                        self.saveTimesheet(timesheet, function() {
                            self.submit();
                        });
                        countinue = false;
                        return false;
                    }
                });

                if (this.totalHours() < 8) {
                    jQuery.confirm("Total hours less than 8 hours, your timesheets cannot be edited once you submit. Proceed to submit?",
                    {
                        yesText: "Submit",
                        yesCallback: function() {
                            $btnSubmit.click();
                        },
                        noText: "Cancel",
                        noCallback: function() {}
                    });
                } else {
                    if (countinue) {
                        jQuery.confirm("Your timesheets cannot be edited once you submit. Proceed to submit?",
                        {
                            yesText: "Submit",
                            yesCallback: function() {
                                $btnSubmit.click();
                            },
                            noText: "Cancel",
                            noCallback: function() {}
                        });
                    }
                }
            }
            this.submitted = ko.computed(function() {
                return self.dataset().length && self.dataset()[0].submitted;
            }, self);

            this.totalHours = ko.computed(function() {
                var hours = 0;
                for (var i = 0; i < self.dataset().length; i++) {
                    hours += +self.dataset()[i].hours();
                }
                return Math.round(hours*100)/100;
            }, self);
            
            self.refresh = function(clear) {
                jQuery.getJSON("/Service/timesheet.ashx", {
                    action: "gettimesheetsbydate",
                    date: self.date,
                    category:self.selectedCategory().id
                }, function(timesheets) {
                    if (clear) {
                        self.dataset.removeAll();
                    }
                    if (timesheets && timesheets.length) {
                        for (var i = 0; i < timesheets.length; i++) {
                            var ts = timesheets[i];
                            var _p = self.getProject(ts.project.id);
                            var _ticket = new Ticket(ts.ticket);
                            _p().tickets.push(_ticket);
                            self.dataset.push(new Timesheet(_p,_ticket,ts.id, ts.hours, ts.workDetail, false,ts.submitted));
                        }
                    }
                }).complete(function() {
                    // default add mode or edit item
                    if(!self.submitted()) {
                        
                        self.new();
                    }
                });
            }
            jQuery.ajaxSetup({
                beforeSend : function() {
                    self.ShowByAjax(false);
                },
                complete: function() {
                    self.ShowByAjax(true);
                }
            });
            this.showDate=function(item){
                return "MoveTimeSheet.aspx?tmid="+item.id;
            }


            // load added timesheets
            self.refresh();
        }

        var vm;
        jQuery(function() {
            var d = new Date();
            var date = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
            if (new Date('<%=SelectedDate.Date%>') > new Date(date)) {
                $(".btnrightalign").hide();
            }
            vm = new timesheetViewModel({
                date:'<%=this.SelectedDate.ToString("MM/dd/yyyy")%>',
                projects:<%= ProjectJson%>,
                categories:<%= CategoryJson%>
                });

            ko.applyBindings(vm,document.body);

            jQuery("#tipNotice").popover();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <script type="text/html" id="editable">
        <td>
            <select name="select" data-bind="options: $root.projects,
    optionsText: 'title',
    value: project"
                class="small">
            </select>
        </td>
        <td>
            <select name="select" class="small" data-bind="options: project().tickets,
    optionsText: 'title',
    value: ticket">
            </select></td>
        <td>
            <select name="select3" class="small" data-bind="options: project().tickets,
    optionsText: 'id',
    value: ticket">
            </select></td>
        <td data-bind="text:ticket().description" style="word-break: break-all;"></td>

        <td>
            <textarea name="textarea2" rows="5" class="inputpmreview middle" data-bind="value:timesheet.workDetail"></textarea></td>
        <td class="aligncenter">
            <input name="textfield" type="text" class="inputnum" style="width: 30px;" data-bind="value:timesheet.hours" />
        </td>
        <td class="aligncenter action">
            <a href="javascript:" data-bind="event:{click:$root.saveTimesheet},visible:$root.ShowByAjax">
                <img src="/images/icons/save.png" title="Save" /></a>
            <a href="javascript:" data-bind="event:{click:$root.new},if:timesheet.id<1">
                <img src="/images/icons/newitem.png" title="New" /></a>
            <a href="javascript:" data-bind="event:{click:$root.deleteTimesheet}">
                <img src="/images/icons/delete.png" title="Delete"></a>
        </td>
    </script>
    <script type="text/html" id="viewtimesheet">
        <td data-bind="text: timesheet.project().title"></td>
        <td data-bind="text: timesheet.ticket().title"></td>
        <td data-bind="text: timesheet.ticket().id"></td>
        <td data-bind="text: timesheet.ticket().description" style="word-break: break-all;"></td>
        <td data-bind="text: timesheet.workDetail"></td>
        <td data-bind="text: timesheet.hours" class="aligncenter"></td>
        <td class="aligncenter action">
            <%--<a href="javascript:" data-bind="attr:{href:$root.showDate(timesheet)}" data-target='#modalsmall' data-toggle='modal'>
                <img src="/images/icons/waiting_on.png" title="Move to another day" />
            </a>--%>
            <a href="javascript:" data-bind="event:{click:$root.beginEdit},ifnot:$root.submitted">
                <img src="/images/icons/edit.png" title="Edit" />
            </a>
            <a href="javascript:" data-bind="event:{click:$root.deleteTimesheet},ifnot:$root.submitted">
                <img src="/images/icons/delete.png" title="Delete">
            </a>
        </td>
    </script>
    <div class="timesheetbox1">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="35%" class="timesheetTitle1">Timesheet: <%=SelectedDate.ToString("MM/dd/yyyy") %>  (Total Hours: <span class="fdNotice" data-bind="text:totalHours"></span>)

                </td>
                <td width="*">Import Category:&nbsp;<select name="select" class="middle"
                    data-bind="options: $root.categories,
    optionsText: 'category',
    value: $root.selectedCategory,
    enable:$root.submitted()==false">
                </select>
                </td>
                <td width="15%">
                    <asp:CheckBox ID="chkNoticeHR" Text=" Notice HR" runat="server" /><a data-original-title="Notice HR" data-toggle="popover" id="tipNotice" class="info" title="" href="###" data-container="body" data-placement="right" data-trigger="hover click" data-html="true" data-content="If you have forgot to write timesheets someday, HR wants to know you have submit timesheets on date: <%=this.SelectedDate.ToString("MM/dd/yyyy")%>. Thank you!">&nbsp;</a>
                </td>
                <td width="13%">
                    <ul class="listtopBtn">
                        <asp:Button ID="btnSubmit" runat="server" Text="" OnClick="btnSubmit_Click" Style="display: none;" />
                        <li class="btnrightalign" data-bind="click:$root.submit,ifnot:$root.submitted">
                            <div class="listtopBtn_icon">
                                <img src="/images/icons/wsubmit.png" />
                            </div>
                            <div class="listtopBtn_text">Submit</div>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    <div style="width:100%;overflow-x:auto;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance tabletimesheet noclickbind">
        <thead>
            <tr>
                <th width="120">Project</th>
                <th width="120">Title</th>
                <th width="50">ID</th>
                <th width="250">
                    <div style="min-width: 100px;">Description</div>
                </th>

                <th width="*">
                    <div style="min-width: 300px;">Work Detail</div>
                </th>
                <th width="10" class="aligncenter">Hours</th>
                <th width="80" class="aligncenter">
                    <div style="min-width: 50px;">Action</div>
                </th>
            </tr>
        </thead>
        <tbody data-bind="foreach: { data: dataset, as: 'timesheet' }">
            <!-- ko if:timesheet.isEdit -->
            <tr data-bind="template:{ name: 'editable', data: timesheet },css:{'whiterow' : $index() % 2 === 1,writetimesheet:true,'errorTr':!valid()}"></tr>
            <!-- /ko -->
            <!-- ko ifnot:timesheet.isEdit -->
            <tr data-bind="template:{ name: 'viewtimesheet', data: timesheet},css:{'whiterow' : $index() % 2 === 1}"></tr>
            <!-- /ko -->
        </tbody>
    </table>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
