<%@ Page Title="" Language="C#" MasterPageFile="~/Invoice/Invoice.master" AutoEventWireup="true" CodeBehind="ToDoList.aspx.cs" Inherits="SunNet.PMNew.PM2014.Invoice.ToDo.ToDoList" %>

<%@ Import Namespace="SunNet.PMNew.Entity.InvoiceModel.Enums" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .listCount {
            color: #cc0000;
        }

        .searchItembox {
            margin-top: 20px;
        }

        * {
            margin: 0;
            padding: 0;
            list-style: none;
        }

        .tab {
            width: 100%;
            height: 40px;
            border-bottom: 1px solid #ccc;
        }

            .tab li {
                float: left;
                border-radius: 5px 5px 0 0;
                line-height: 38px;
                padding: 0 12px;
                background: #fff;
                cursor: pointer;
                margin-right: 2px;
                border: 1px solid transparent;
            }

                .tab li:hover {
                    background: #eee;
                }

                .tab li.cur {
                    border: 1px solid #ccc;
                    line-height: 39px;
                    border-bottom: 1px solid #fff;
                }

                    .tab li.cur:hover {
                        background: #fff;
                    }

        .content div {
            display: none;
        }

            .content div.in {
                display: block;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <ul id="invoiceTab" class="tab">
        <li class="cur">Proposal (<asp:Label ID="proposalInvoiceCount" CssClass="listCount" runat="server" Text=""></asp:Label>)
        </li>
        <li>Time Material (<span class="listCount" data-bind="text: companycount"></span>)
            
        </li>
    </ul>
    <div class="content">
        <div id="" class="in searchItembox">
            <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="60px">Keyword:
                    </td>
                    <td width="300px">
                        <asp:TextBox ID="txtKeyword" runat="server" queryparam="keyword" CssClass="input200" placeholder="Enter Title, Invoice#" Width="200"></asp:TextBox>
                    </td>
                    <td width="60px">Project:
                    </td>
                    <td width="300px">
                        <sunnet:ExtendedDropdownList ID="ddlProject" queryparam="project"
                            DataTextField="Title"
                            DataValueField="ProjectID"
                            DataGroupField="Status"
                            DefaultMode="List" runat="server" CssClass="selectw1" Width="260">
                        </sunnet:ExtendedDropdownList>
                    </td>
                    <td>
                        <input type="button" class="searchBtn" id="btnSearch" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="" class="searchItembox">
            <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="60px">Company:
                    </td>
                    <td width="300px">
                        <asp:DropDownList ID="tm_ddlCompany" CssClass="select150" runat="server" Width="200">
                        </asp:DropDownList>
                    </td>
                    <td width="60px">Project:
                    </td>
                    <td width="300px">
                        <sunnet:ExtendedDropdownList ID="tm_ddlProject"
                            DataTextField="Title"
                            DataValueField="ProjectID"
                            DataGroupField="Status"
                            DefaultMode="List" runat="server" CssClass="selectw1" Width="260">
                        </sunnet:ExtendedDropdownList>
                    </td>
                    <td>
                        <input type="button" data-bind="click: searchCompany" class="searchBtn" id="btnSearchCompany" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="content">
        <div class="in">
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
                <thead>
                    <tr>
                        <th width="20%" class="order order-desc" default="true" orderby="ProjectTitle">Project<span class="arrow"></span></th>
                        <th width="20%" class="order" orderby="ProposalTrackerTitle">Proposal Title<span class="arrow"></span></th>
                        <th width="10%">PO #<span class="arrow"></span></th>
                        <th width="10%">Milestone<span class="arrow"></span></th>
                        <th width="10%">Invoice #<span class="arrow"></span></th>
                        <th width="15%" class="arrow">Invoice Status<span class="arrow"></span></th>
                        <th width="10%" class="aligncenter">Action</th>
                    </tr>
                </thead>
                <tr runat="server" id="trNoProposalInvoice" visible="false">
                    <th colspan="7" style="color: Red;">&nbsp; No record found
                    </th>
                </tr>
                <asp:Repeater ID="rptInvoiceList" runat="server">
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                            <td><%# Eval("ProjectTitle")%></td>
                            <td><%# Eval("ProposalTrackerTitle")%></td>
                            <td><%# Eval("PONo")%></td>
                            <td><%# Eval("Milestone")%></td>
                            <td><%# Eval("InvoiceNo")%></td>
                            <td><%# ((InvoiceStatus)Eval("Status")).ToText()%></td>
                            <td style="text-align: center;">
                                <a style="display: <%#(InvoiceStatus)Eval("Status")==InvoiceStatus.Missing_Invoice?"":"none"%>" href="Proposal/DetailInvoice.aspx?proposalId=<%#Eval("ProposalTrackerID") %>&returnUrl=/Invoice/ToDoList.aspx">Add Invoice</a>
                                <a style="display: <%#(InvoiceStatus)Eval("Status")==InvoiceStatus.Awaiting_Send?"":"none"%>" href="Proposal/DetailInvoice.aspx?proposalId=<%#Eval("ProposalTrackerID") %>&returnUrl=/Invoice/ToDoList.aspx">View</a>
                                <a style="display: <%#(InvoiceStatus)Eval("Status")==InvoiceStatus.Awaiting_Send?"":"none"%>" href="Proposal/DetailInvoice.aspx?proposalId=<%#Eval("ProposalTrackerID") %>&returnUrl=/Invoice/ToDoList.aspx">Edit</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
                <thead>
                    <tr>
                        <th width="20%">Company<span class="arrow"></span></th>
                        <th width="20%">Project<span class="arrow"></span></th>
                        <th width="10%">Action<span class="arrow"></span></th>
                    </tr>
                </thead>
                <tr runat="server" id="trNoCompany" visible="false">
                    <th colspan="3" style="color: Red;">&nbsp; No record found
                    </th>
                </tr>
                <tbody data-bind="foreach: { data: companylist, as: 'company' }">
                    <tr data-bind="css: $index() % 2 == 0 ? 'whiterow' : ''">
                        <td>
                            <label data-bind="text: company.CompanyName, attr: { for: 'company' + company.CompanyId }"></label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <input type="checkbox" data-bind="attr: { id: 'company' + company.CompanyId, value: company.CompanyId }" onchange="selectAll(this)" checked="checked" />&nbsp;&nbsp;
                                <label data-bind="attr: { for: 'company' + company.CompanyId }">All</label>
                        </td>
                        <td data-bind="foreach: { data: company.Projects, as: 'project' }">
                            <p>
                                <input type="checkbox" data-bind="attr: { id: 'project' + project.ProjectId, value: project.ProjectId, companyid: company.CompanyId }" onchange="checkSelectAll(this)" checked="checked" />&nbsp;&nbsp;
                            <label data-bind="attr: { for: 'project' + project.ProjectId }, text: project.ProjectTitle"></label>
                            </p>
                        </td>
                        <td>
                            <input style="padding: 6px 12px" class="saveBtn1 mainbutton" type="button" data-bind="click: $root.viewTSDetail" value="View detail to add invoice" /></td>
                    </tr>
                </tbody>
                <%--<asp:Repeater ID="rptCompanyList" runat="server" OnItemDataBound="rptCompanyList_ItemDataBound">
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                            <td>
                                <label for="company<%# Eval("ComID") %>"><%# Eval("CompanyName") %></label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <input id="company<%# Eval("ComID") %>" type="checkbox" value="<%# Eval("ComID") %>" onchange="selectAll(this)" checked="checked" />&nbsp;&nbsp;
                                <label for="company<%# Eval("ComID") %>">All</label>
                                <asp:HiddenField ID="hfid" runat="server" Value=' <%#Eval("ComID")%>' />
                            </td>
                            <td>
                                <asp:Repeater ID="rptProjectList" runat="server">
                                    <ItemTemplate>
                                        <p>
                                            <input id="project<%# Eval("ProjectID") %>" type="checkbox" value="<%# Eval("ProjectID") %>" companyid="<%# Eval("CompanyID") %>" onchange="checkSelectAll(this)" checked="checked" />&nbsp;&nbsp;
                                                    <label for="project<%# Eval("ProjectID") %>"><%#Eval("Title") %></label>
                                        </p>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                            <td>
                                <input type="button" value="View detail to add invoice" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>--%>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        var companymodel;
        $(function () {
            $('ul.tab li').click(function (event) {
                $(this).addClass('cur').siblings('li').removeClass('cur');
                var i = $(this).index();
                $('div.content').each(function (e) {
                    $(this).find('div').eq(i).addClass('in').siblings('div').removeClass('in');
                });
            });
            if ('<%=tabIndex%>' == '1') {
                $('ul.tab li')[1].click();
            }

            companymodel = new CompanyModel();
            ko.applyBindings(companymodel, document.body);
        });

        function selectAll(e) {
            var companyId = $(e).val();
            var chkState = $(e).prop('checked');
            $("input[companyid=" + companyId + "]").each(function () {
                $(this).prop('checked', chkState);
            });
        }
        function checkSelectAll(e) {
            var companyId = $(e).attr('companyid');
            var chkState = $(e).prop('checked');
            if (chkState) {
                var otherChk = $("input[companyid=" + companyId + "]").not("input:checked");
                if (!otherChk.length) {
                    $("#company" + companyId).prop('checked', true);
                }
            } else {
                $("#company" + companyId).prop('checked', false);
            }
        }

        function CompanyModel() {
            var self = this;
            self.companylist = ko.observableArray([]);
            self.companycount = ko.observable(0);
            self.searchCompany = function () {
                var companyId = $("[id$=tm_ddlCompany]").val() == "" ? 0 : $("[id$=tm_ddlCompany]").val();
                var projectId = $("[id$=tm_ddlProject]").val() == "" ? 0 : $("[id$=tm_ddlProject]").val();
                jQuery.getJSON("/Service/Invoice.ashx", { action: "getcompanies", companyId: companyId, projectId: projectId }, function (companies) {
                    self.companylist(companies);
                    self.companycount(companies.length);
                });
            };
            self.viewTSDetail = function (company) {
                var projectIds = "";
                var chks = $("input[companyid=" + company.CompanyId + "]:checked");
                if (chks.length) {
                    chks.each(function (i) {
                        projectIds = projectIds + $(this).val() + ",";
                    });
                    projectIds = projectIds.substr(0, projectIds.length - 1);
                    location.href = "TM/TSDetail.aspx?projectIds=" + projectIds + "&companyId=" + company.CompanyId;
                }
            };
            self.searchCompany();
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
