<%@ Page Title="" Language="C#" MasterPageFile="~/Invoice/Invoice.master" AutoEventWireup="true" CodeBehind="DetailInvoice.aspx.cs" Inherits="SunNet.PMNew.PM2014.Invoice.Proposal.DetailInvoice" %>

<%@ Import Namespace="SunNet.PMNew.Entity.InvoiceModel.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div id="" class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="60px">Company:</td>
                <td width="300px">
                    <asp:Label ID="lblCompany" runat="server" Text=""></asp:Label>
                </td>
                <td width="60px">Proposal:</td>
                <td width="300px">
                    <asp:Label ID="lblProposal" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="60px">Project:</td>
                <td width="300px" id="tdProjectName">
                    <asp:Label ID="lblProject" runat="server" Text=""></asp:Label>
                </td>
                <td width="60px">PO #:</td>
                <td width="300px">
                    <asp:Label ID="lblPO" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">

    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance tabletimesheet noclickbind">
        <thead>
            <tr>
                <th style="width: 3%"></th>
                <th style="width: 8%">Milestone #</th>
                <th style="width: 5%">Approved</th>
                <th style="width: 8%">ETA Date</th>
                <th style="width: 10%">Invoice Status</th>
                <th style="width: 15%">Invoice #</th>
                <th style="width: 15%">Send On</th>
                <th style="width: 8%">Due On</th>
                <th style="width: 8%">Receive On</th>
                <th class="aligncenter" style="width: 10%">
                    <div>Note</div>
                </th>
            </tr>
        </thead>
        <tbody data-bind="foreach: { data: dataset, as: 'invoice' }">
            <tr data-bind="template: { name: 'editInvoice', data: invoice }, css: { 'whiterow': $index() % 2 === 1 }"></tr>
        </tbody>
    </table>
    <div class="buttonBox2">
        <input type="hidden" name="invoices" id="hidInvoice" />
        <input type="button" name="btnSave" value="Save" id="btnSave" onclick="SaveInvoice();" class="saveBtn1 mainbutton">
        <input type="button" value=" Close " class="cancelBtn1 mainbutton" onclick="javascript: location.href='<%=Request.QueryString["returnUrl"]%>';">
    </div>
    <script type="text/html" id="editInvoice">
        <td>
            <a data-bind="click: $root.InvoiceWaive, visible: (invoice.Status() == 3 || invoice.Status() == 4 || invoice.Status() == 5) ? true : false">Waive</a>
        </td>
        <td>
            <span data-bind="text: Milestone"></span>
        </td>
        <td>
            <span data-bind="text: (Approved() ? 'Yes' : 'No')"></span>
        </td>
        <td>
            <span data-bind="text: ETADate"></span>
        </td>
        <td>
            <span data-bind="text: StatusText"></span>
        </td>
        <td>
            <input style="width: 90%" data-bind="value: InvoiceNo, attr: { id: 'txtInvoiceNo' + Milestone(), disabled: disabledWaive() }, event: { change: $root.invoiceChange }" />
        </td>
        <td>
            <input style="width: 80%" data-bind="value: SendOn, attr: { id: 'txtSendOn' + Milestone(), disabled: disabledSend() }" onchange="sendDateChange(this.value,this.id)" name="txtSendOn" onclick="    WdatePicker({ isShowClear: false });" class=" payInput date" />
            <a data-toggle="popover" data-bind="attr: { id: 'tipSendOn' + Milestone }" name="tipSendOn" class="info" title="Warning" href="###" data-container="body" data-placement="right"
                data-trigger="hover click" data-html="true" data-content="<span class='noticeRed'>This field should be fill in within 3 days after fill in invoice#. Please fill this field as soon as possible. </span>">&nbsp;</a>
        </td>
        <td>
            <input data-bind="value: DueOn, attr: { id: 'txtDueOn' + Milestone(), disabled: disabledSend() }" onchange="" name="txtDueOn" onclick="    WdatePicker({ isShowClear: false });" class="payInput date" />
        </td>
        <td>
            <input data-bind="value: ReceiveOn, attr: { id: 'txtReceiveOn' + Milestone(), disabled: disabledReceive() }" onchange="" name="txtReceiveOn" onclick="    WdatePicker({ isShowClear: false });" class="payInput date" />
        </td>
        <td>
            <input style="width: 90%" data-bind="value: Notes, attr: { id: 'txtNotes' + Milestone(), disabled: disabledWaive() }" name="txtNotes" />
        </td>
    </script>
    <script type="text/javascript">
        var invoicemodel;
        $(function () {
            invoicemodel = new InvoiceModel();
            ko.applyBindings(invoicemodel);
        });

        var proposalid = '<%=QS("proposalId",0) %>';
        function Invoice(id, status, statusText, milestone, approved,etaDate, invoiceNo, sendOn, dueOn, receiveOn, notes) {
            //_invoice = this;
            this.ID = ko.observable(id);
            this.Status = ko.observable(status);
            this.StatusText = ko.observable(statusText);
            this.Milestone = ko.observable(milestone);
            this.Approved = ko.observable(approved);
            this.ETADate = ko.observable(etaDate);
            this.disabledWaive = ko.observable(status == "<%=(int)InvoiceStatus.Waive%>");
            this.disabledSend = ko.observable(invoiceNo == "" || !approved || status == "<%=(int)InvoiceStatus.Waive%>");
            this.disabledReceive = ko.observable(sendOn == "" || !approved || status == "<%=(int)InvoiceStatus.Waive%>");
            this.InvoiceNo = ko.observable(invoiceNo);
            this.SendOn = ko.observable(sendOn);
            this.DueOn = ko.observable(dueOn);
            this.ReceiveOn = ko.observable(receiveOn);
            this.Notes = ko.observable(notes);
        }
        function InvoiceModel() {
            var self = this;

            this.dataset = ko.observableArray([]);
            this.currentMilestone = ko.observable(0);
            this.refreshData = function () {
                self.dataset.removeAll();
                jQuery.getJSON("/Service/Invoice.ashx", { action: "getproposalinvoices", proposalid: proposalid }, function (invoices) {
                    if (invoices && invoices.length) {
                        for (var i = 0; i < invoices.length; i++) {
                            var invoice = invoices[i];
                            invoice.SendOn = formatDate(invoice.SendOn);
                            invoice.ReceiveOn = formatDate(invoice.ReceiveOn);
                            invoice.DueOn = formatDate(invoice.DueOn);
                            invoice.ETADate = formatDate(invoice.ETADate);
                            self.dataset.push(new Invoice(invoice.ID, invoice.Status, invoice.StatusText, invoice.Milestone, invoice.Approved, invoice.ETADate, invoice.InvoiceNo, invoice.SendOn,
                                invoice.DueOn, invoice.ReceiveOn, invoice.Notes));
                        }
                    }
                }).always(function () {
                    jQuery("[name=tipSendOn]").popover();
                });
            };
            this.InvoiceWaive = function (item) {
                item.Status("<%=InvoiceStatus.Waive%>");
                //item.ReceiveOn("<%=DateTime.Now.ToString("MM/dd/yyyy")%>");
            };
            this.invoiceChange = function (item) {
                if (item.InvoiceNo() == "" || !item.Approved()) {
                    item.SendOn("");
                    item.DueOn("");
                    item.ReceiveOn("");
                    item.disabledReceive(true);
                    item.disabledSend(true);
                }
                else {
                    item.disabledSend(false);
                }
            };

            this.refreshData();
        }

        function sendDateChange(sendDate, sendDateId) {
            var receiveDateId = "txtReceiveOn" + sendDateId.charAt(sendDateId.length - 1);
            if (sendDate == "") {
                $("#" + receiveDateId).attr("disabled", true);
                $("#" + receiveDateId).val('');
            }
            else {
                $("#" + receiveDateId).attr("disabled", false);
            }
        }

        function formatDate(dateStr) {
            var fmtDate = "";
            if (dateStr && dateStr.length) {
                dateStr = dateStr.substring(0, 10);
                var dateArr = dateStr.split('-');
                if (dateArr.length == 3) {
                    fmtDate = dateArr[1] + '/' + dateArr[2] + '/' + dateArr[0];
                }
            }
            return fmtDate;
        }

        function SaveInvoice() {
            debugger
            var list_Invoices = [];
            for (var i = 0; i < invoicemodel.dataset().length; i++) {
                var invoiceCurrent = invoicemodel.dataset()[i];
                var status = invoiceCurrent.Status();
                if (invoiceCurrent.Status() != "<%=InvoiceStatus.Waive%>") {
                    if (invoiceCurrent.ReceiveOn() != "" && invoiceCurrent.ReceiveOn() != undefined) {
                        status = "<%=InvoiceStatus.Payment_Received%>";
                    }
                    else if (invoiceCurrent.SendOn() != "" && invoiceCurrent.SendOn() != undefined) {
                        status = "<%=InvoiceStatus.Awaiting_Payment%>";
                    }
                    else if (invoiceCurrent.Approved()) {
                        status = "<%=InvoiceStatus.Awaiting_Send%>";
                    }
                }
                var invoice = new InvoiceResult(invoiceCurrent.ID(), status, invoiceCurrent.Milestone(),
                invoiceCurrent.Approved(), invoiceCurrent.ETADate(), invoiceCurrent.InvoiceNo(),
                invoiceCurrent.SendOn(), invoiceCurrent.DueOn(), invoiceCurrent.ReceiveOn(), invoiceCurrent.Notes());
                list_Invoices.push(invoice);
                if(invoiceCurrent.ReceiveOn != null && invoiceCurrent.ReceiveOn != undefined)
                {
                    jQuery.post("/Service/Invoice.ashx", {
                        action: "sendEmail",
                        id: invoiceCurrent.ID(),
                        proposalId: '<%=Request.QueryString["proposalId"]%>',
                        projectName: $("#tdProjectName").children().text(),
                        milestone: invoiceCurrent.Milestone(),
                        invoiceNo: invoiceCurrent.InvoiceNo(),
                        sendOn: invoiceCurrent.SendOn(),
                        receiveOn: invoiceCurrent.ReceiveOn(),
                        dueOn: invoiceCurrent.DueOn(),
                        approved: invoiceCurrent.Approved(),
                        etaDate: invoiceCurrent.ETADate(),
                        proposalTitle: $("#<%=lblProposal.ClientID %>").html()
                    }, function () {

                    }, 'json');
                }
            }
        var formStr = JSON.stringify(list_Invoices);
        $("#hidInvoice").val(formStr);
        $("#form1").submit();
    }

        function InvoiceResult(id, status, milestone, approved, etaDate, invoiceNo, sendOn, dueOn, receiveOn, notes) {
    this.ID = id;
    this.ProposalId = '<%=QS("proposalId",0) %>';
            this.Status = status;
            this.Milestone = milestone;
            this.Approved = approved;
            this.ETADate = etaDate;
            this.InvoiceNo = invoiceNo;
            this.SendOn = sendOn;
            this.DueOn = dueOn;
            this.ReceiveOn = receiveOn;
            this.Notes = notes;
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
