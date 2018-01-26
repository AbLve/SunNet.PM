<%@ Page Title="" Language="C#" MasterPageFile="~/SunnetTicket/Sunnet.master" AutoEventWireup="true" CodeBehind="InternalDashboard.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.InternalDashboard" %>

<%@ Import Namespace="SunNet.PMNew.Entity.UserModel" %>
<%@ Import Namespace="SunNet.PMNew.Entity.TicketModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .listDiv {
            width: 49%;
            float: left;
            box-sizing: border-box;
            -webkit-box-sizing: border-box;
            margin-bottom: 20px;
        }

            .listDiv tr {
                height: 41px;
            }

        .table-advance tr, .table-advance tr:hover {
            background: #fff;
        }

            .table-advance tr.whiterow:hover {
                background: #fff;
            }

        .mainrightBox {
            border: 0;
            background: #f5f5f5;
            padding-top: 15px;
        }

        .listtopBtn_text {
            background: #fff;
        }

        .topbtnbox {
            min-height: 35px;
        }
    </style>
    <script src="/Scripts/knockout-3.1.0.debug.js"></script>
    <script src="/Scripts/knockout.mapping-latest.debug.js"></script>
    <script type="text/javascript">
        var Roles = {
            PM:<%=(int)RolesEnum.PM%>,
            Leader:<%=(int)RolesEnum.Leader%>,
            DEV:<%=(int)RolesEnum.DEV%>,
            QA:<%=(int)RolesEnum.QA%>
        };
        var PriorityState = {
            Low: <%=(int)PriorityState.Low%>,
            Medium: <%=(int)PriorityState.Medium%>,
            High: <%=(int)PriorityState.High%>,
            Emergency: <%=(int)PriorityState.Emergency%>
        };
        function TicketModel(responsibleUser, ticketId, title, projectId, projectName, priority) {
            var self = this;
            this.responsibleUser = responsibleUser || 0;
            this.ticketId = ticketId || "";
            this.title = "";
            if (title != undefined) {
                title = title + "";
                this.title = title.length > 50 ? title.substring(0, 50) + '...' : title || "";
            }
            this.projectId = projectId || "";
            this.projectName = projectName || "";
            this.priority = "";
            if (priority) {
                switch (priority) {
                    case PriorityState.Low:
                        self.priority = "low";
                        break;
                    case PriorityState.Medium:
                        self.priority = "medium";
                        break;
                    case PriorityState.High:
                        self.priority = "high";
                        break;
                    case PriorityState.Emergency:
                        self.priority = "emergency";
                        break;
                }
            }
        }
        function UserModel(userId, firstName, lastName, ticketCount, tickets, previous, current) {
            var self = this;
            this.userId = userId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.ticketCount = ticketCount;
            this.previous = previous;
            this.current = current;
            this.tickets = ko.observableArray([]);
            for (var i = 0; i < tickets.length; i++) {
                if (tickets[i]) {
                    self.tickets.push(new TicketModel(tickets[i].ResponsibleUserID,
                        tickets[i].TicketID,
                        tickets[i].Title,
                        tickets[i].ProjectID,
                        tickets[i].ProjectName,
                        tickets[i].Priority));
                }
            }
        }
        var currentUserId ='<%=UserInfo.UserID%>';
        function UserTicketViewModel() {
            var self = this;
            this.dataset = ko.observableArray([]);
            this.pmDataset = ko.observableArray([]);
            this.leadDataset = ko.observableArray([]);
            this.devDataset = ko.observableArray([]);
            this.qaDataset = ko.observableArray([]);
            this.currentUserData = ko.observableArray([]);
            this.refresh = function () {
                $.getJSON("/Service/Ticket.ashx", { action: "getUserTickets", currentuserid: currentUserId }, function (data) {
                    self.currentUserData([]);
                    self.dataset([]);
                    self.pmDataset([]);
                    self.leadDataset([]);
                    self.devDataset([]);
                    self.qaDataset([]);
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].UserID == currentUserId) {
                            self.currentUserData.push(new UserModel(data[i].UserID, data[i].FirstName, data[i].LastName, data[i].TicketCount, data[i].Tickets, data[i].Previous, data[i].Current));
                        } else {
                            switch (data[i].RoleID) {
                                case Roles.PM:
                                    self.pmDataset.push(new UserModel(data[i].UserID, data[i].FirstName, data[i].LastName, data[i].TicketCount, data[i].Tickets, data[i].Previous, data[i].Current));
                                    break;
                                case Roles.Leader:
                                    self.leadDataset.push(new UserModel(data[i].UserID, data[i].FirstName, data[i].LastName, data[i].TicketCount, data[i].Tickets, data[i].Previous, data[i].Current));
                                    break;
                                case Roles.DEV:
                                    self.devDataset.push(new UserModel(data[i].UserID, data[i].FirstName, data[i].LastName, data[i].TicketCount, data[i].Tickets, data[i].Previous, data[i].Current));
                                    break;
                                case Roles.QA:
                                    self.qaDataset.push(new UserModel(data[i].UserID, data[i].FirstName, data[i].LastName, data[i].TicketCount, data[i].Tickets, data[i].Previous, data[i].Current));
                                    break;
                            }
                        }
                    }
                    self.pmDataset().forEach(function (item, index) { self.dataset.push(item); });
                    self.leadDataset().forEach(function (item, index) { self.dataset.push(item); });
                    self.devDataset().forEach(function (item, index) { self.dataset.push(item); });
                    self.qaDataset().forEach(function (item, index) { self.dataset.push(item); });
                    self.dataset().forEach(function (item, index) {
                        if (index % 2 == 0 && self.dataset()[index + 1]) {
                            var diff = item.tickets().length - self.dataset()[index + 1].tickets().length;
                            if (diff > 0) {
                                for (var j = 0; j < diff; j++) {
                                    self.dataset()[index + 1].tickets.push(new TicketModel());
                                }
                            } else if (diff < 0) {
                                for (var j = 0; j < -diff; j++) {
                                    self.dataset()[index].tickets.push(new TicketModel());
                                }
                            }
                        }
                    });
                });
            };
            this.needRefresh = ko.observable(false);
            this.needRefresh.subscribe(function (value) {
                if (value) {
                    self.refresh();
                    self.needRefresh(false);
                }
            });
            self.refresh();
        }
        var userTicketModel = null;
        jQuery(function () {
            userTicketModel = new UserTicketViewModel();
            ko.applyBindings(userTicketModel, document.body);
            //jQuery("body").off("hidden.bs.modal", ".modal");
            //$("body").on('hidden.bs.modal', function (e) {
            //    if (userTicketModel.needRefresh) {
            //        userTicketModel.refresh();
            //        userTicketModel.needRefresh = false;
            //    }
            //});
        });
        function closeModal() {
            $("#modalsmall").modal('hide');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="topbtnbox">
        <ul class="listtopBtn">
            <li href="HideUser.aspx" data-target="#modalsmall" data-toggle="modal">
                <div class="listtopBtn_icon">
                    <img src="/images/icons/wnewcategory.png" />
                </div>
                <div class="listtopBtn_text">Hide/Display Users</div>
            </li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <!--ko foreach:{data:currentUserData,as:'userTicket'}-->
    <div class="listDiv" style="width: 100%;background:#fff;padding:10px 15px;">
        <table style="width: 100%" class="table-advance" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <th colspan="4">
                    <a data-bind="attr: { href: '/SunnetTicket/WaitingResponse.aspx?userid=' + userTicket.userId }" target="_blank">
                        <span data-bind="text: 'Waiting for my response (' + userTicket.ticketCount + ')'"></span>
                    </a>
                    <span style="float: right" data-bind="text: 'Total Assigned Tickets : Last Month : (' + userTicket.previous + ')&nbsp;&nbsp;Current : (' + userTicket.current + ')'"></span>
                </th>
            </tr>
            <tr>
                <th style="width: 30%">Project Name</th>
                <th style="width: 15%">Ticket ID</th>
                <th style="width: 50%">Title</th>
                <th style="display: none">Action</th>
            </tr>
            <!--ko foreach:{data:userTicket.tickets,as:'ticket'}-->
            <tr data-bind="attr: { ticket: ticketId }, css: { whiterow: $index() % 2 == 0 }">
                <td>
                    <!-- ko if:ticketId -->
                    <img data-bind="attr: { src: '/images/icons/' + ticket.priority + '.png', title: ticket.priority }" />
                    <span data-bind="text: ticket.projectName"></span>
                    <!-- /ko -->
                </td>
                <td data-bind="text: ticket.ticketId"></td>
                <td data-bind="text: ticket.title"></td>
                <td class="action aligncenter" style="display: none">
                    <!-- ko ifnot:ticketId ==''-->
                    <a class="saveBtn1 mainbutton" data-bind="attr: { href: 'Detail.aspx?tid=' + ticket.ticketId, ticketid: ticket.ticketId }" style="display: none;" target="_blank">
                        <span data-bind="attr: { id: 'spanOpen' + ticket.ticketId }"></span>
                    </a>
                    <!-- /ko -->
                </td>
            </tr>
            <!-- /ko -->
        </table>
    </div>
    <!-- /ko -->
    <!--ko foreach:{data:dataset,as:'userTicket'}-->
    <div class="listDiv" data-bind="style: { marginRight: $index() % 2 == 0 ? '2%' : '0' }" style="background:#fff;padding:10px 15px;">
        <table style="width: 100%" class="table-advance" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <th colspan="4">
                    <a data-bind="attr: { href: '/SunnetTicket/WaitingResponse.aspx?userid=' + userTicket.userId }" target="_blank">
                        <span data-bind="text: userTicket.firstName + ' ' + userTicket.lastName + ' (' + userTicket.ticketCount + ')'"></span>
                    </a>
                    <span style="float: right" data-bind="text: 'Total Assigned Tickets : Last Month : (' + userTicket.previous + ')&nbsp;&nbsp;Current : (' + userTicket.current + ')'"></span>
                </th>
            </tr>
            <tr>
                <th style="width: 30%">Project Name</th>
                <th style="width: 15%">Ticket ID</th>
                <th style="width: 50%">Title</th>
                <th style="display: none">Action</th>
            </tr>
            <!--ko foreach:{data:userTicket.tickets,as:'ticket'}-->
            <tr data-bind="attr: { ticket: ticketId }, css: { whiterow: $index() % 2 == 0 }">
                <td>
                    <!-- ko if:ticketId -->
                    <img data-bind="attr: { src: '/images/icons/' + ticket.priority + '.png', title: ticket.priority }" />
                    <span data-bind="text: ticket.projectName"></span>
                    <!-- /ko -->
                </td>
                <td data-bind="text: ticket.ticketId"></td>
                <td data-bind="text: ticket.title"></td>
                <td class="action aligncenter" style="display: none">
                    <!-- ko ifnot:ticketId ==''-->
                    <a class="saveBtn1 mainbutton" data-bind="attr: { href: 'Detail.aspx?tid=' + ticket.ticketId, ticketid: ticket.ticketId }" style="display: none;" target="_blank">
                        <span data-bind="attr: { id: 'spanOpen' + ticket.ticketId }"></span>
                    </a>
                    <!-- /ko -->
                </td>
            </tr>
            <!-- /ko -->
        </table>
    </div>
    <!-- /ko -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
