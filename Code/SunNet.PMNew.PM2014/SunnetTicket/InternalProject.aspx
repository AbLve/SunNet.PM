<%@ Page Title="" Language="C#" MasterPageFile="~/SunnetTicket/Sunnet.master" AutoEventWireup="true" CodeBehind="InternalProject.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.InternalProject" %>

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
            background: #eee;
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
        var PriorityState = {
            Low: <%=(int)PriorityState.Low%>,
            Medium: <%=(int)PriorityState.Medium%>,
            High: <%=(int)PriorityState.High%>,
            Emergency: <%=(int)PriorityState.Emergency%>
        };
        function TicketModel(ticketId, projectId, statusText, title, responsibleUserId, priority) {
            var self = this;
            this.ticketId = ticketId || "";
            this.projectId = projectId || "";
            this.statusText = statusText || "";
            this.title = "";
            if (title != undefined) {
                title = title + "";
                this.title = title.length > 50 ? title.substring(0, 50) + '...' : title || "";
            }
            this.responsibleUserId = responsibleUserId || 0;
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
        function ProjectModel(projectId, projectCode, projectName, projectStatus, pmId, companyId, companyName, ongoingTickets, tickets) {
            var self = this;
            this.projectId = projectId || "";
            this.projectCode = projectCode || "";
            this.projectName = projectName || "";
            this.projectStatus = projectStatus || "";
            this.pmId = pmId || 0;
            this.highlight = false;
            if (this.pmId == currentUserId) {
                self.highlight = true;
            }
            this.companyId = companyId || 0;
            this.companyName = companyName || "";
            this.ongoingTickets = ongoingTickets || 0;
            this.tickets = ko.observableArray([]);
            for (var i = 0; i < tickets.length; i++) {
                if (tickets[i]) {
                    self.tickets.push(new TicketModel(
                        tickets[i].TicketID,
                        tickets[i].ProjectID,
                        tickets[i].StatusText,
                        tickets[i].Title,
                        tickets[i].ResponsibleUserID,
                        tickets[i].Priority));
                }
            }
        }

        var currentUserId = '<%=UserInfo.UserID%>';
        function ProjectViewModel() {
            var self = this;
            this.otherDataset = ko.observableArray([]);
            this.noTicketDataset = ko.observableArray([]);
            this.dataset = ko.observableArray([]);
            this.refresh = function () {
                $.getJSON("/Service/Project.ashx", { action: "getinternalproject", userid: currentUserId }, function (data) {
                    self.otherDataset([]);
                    self.dataset([]);
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].Tickets.length == 0) {
                            self.noTicketDataset.push(new ProjectModel(data[i].ProjectID, data[i].ProjectCode, data[i].ProjectName, data[i].ProjectStatus,
                                data[i].PMID, data[i].CompanyID, data[i].CompanyName, data[i].OngoingTickets, data[i].Tickets));
                        } else {
                            self.otherDataset.push(new ProjectModel(data[i].ProjectID, data[i].ProjectCode, data[i].ProjectName, data[i].ProjectStatus,
                                data[i].PMID, data[i].CompanyID, data[i].CompanyName, data[i].OngoingTickets, data[i].Tickets));
                        }
                    }
                    self.otherDataset().forEach(function (item, index) { self.dataset.push(item); });
                    self.noTicketDataset().forEach(function (item, index) { self.dataset.push(item); });
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
            self.refresh();
        }
        var projectViewModel = null;
        $(function () {
            projectViewModel = new ProjectViewModel();
            ko.applyBindings(projectViewModel, document.body);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <!--ko foreach:{data:dataset,as:'project'}-->
    <div class="listDiv" data-bind="style: { marginRight: $index() % 2 == 0 ? '2%' : '0' }">
        <table style="width: 100%" class="table-advance" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <th colspan="4" class="aligncenter">
                    <span data-bind="text: project.projectName + ' (' + project.ongoingTickets + ' Ongoing Tickets)', style: { color: highlight ? '#CC0000' : '#000' }"></span>
                </th>
            </tr>
            <tr>
                <th style="width: 15%">Ticket ID</th>
                <th style="width: 20%">Status</th>
                <th style="width: 50%">Title</th>
                <th style="display: none">Action</th>
            </tr>
            <!--ko foreach:{data:project.tickets,as:'ticket'}-->
            <tr data-bind="attr: { ticket: ticketId }, css: { whiterow: $index() % 2 == 0 }">
                <td>
                    <!-- ko if:ticketId -->
                    <img data-bind="attr: { src: '/images/icons/' + ticket.priority + '.png', title: ticket.priority }" />
                    <span data-bind="text: ticket.ticketId"></span>
                    <!-- /ko -->
                </td>
                <td data-bind="text: ticket.statusText"></td>
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
