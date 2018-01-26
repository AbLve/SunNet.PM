<%@ Page Title="" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="EditProposalTracker.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.ProposalTracker.EditProposalTracker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript">
        jQuery(function () {
            // validate signup form on keyup and submit
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });

            jQuery("#tipWorkScope").popover();

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <style>
        .payInput {
            width: 120px;
        }

        .yellow {
            background-color: #FFF2CC;
        }

        .red {
            background-color: #FF5050;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contentTitle titlsealrequest">Basic Information</div>
    <div style="width: 840px;">
        <div class="form-group">
            <label class="col-left-request lefttext">Project:<span class="noticeRed">*</span></label>
            <div class="col-right-request righttext" id="dvDllProject">
                <asp:DropDownList ID="ddlProject" data-msg="Please select project" CssClass="selectrequest" runat="server" min="1">
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-request lefttext">Status:<span class="noticeRed">*</span></label>
            <div class="col-right-request righttext">
                <asp:DropDownList ID="ddlStatus" runat="server" data-msg="Please select status" CssClass="selectproject" min="1">
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-request lefttext">Title:<span class="noticeRed">*</span></label>
            <div class="col-right-request2 righttext">
                <asp:TextBox ID="txtTitle" CssClass="inputrequestds required" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-request lefttext">Description:</label>
            <div class="col-right-request2 righttext">

                <asp:TextBox ID="txtDescription" CssClass="inputrequestds" TextMode="MultiLine"
                    Rows="5" runat="server"> </asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-project lefttext">
                Work Scope:
                <a data-toggle="popover" id="tipWorkScope" class="info" title="Tip" href="###" data-container="body" data-placement="right"
                    data-trigger="hover click" data-html="true" data-content="<span class='noticeRed'>**Please select a file to upload ( *.doc, *.docx, *.pdf). </span>">&nbsp;</a>
            </label>
            <div class="col-right-project righttext">
                <asp:FileUpload ID="fileUpload" Width="205" runat="server" onchange="validationFile()" accept="application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,
                            application/pdf"
                    data-msg="Please select a file to upload ( *.doc, *.docx, *.pdf)" />

                <label id="lblFile" runat="server"></label>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-request lefttext">Proposal Sent To:</label>
            <div class="col-right-request righttext">
                <asp:TextBox ID="txtProposalSentTo" CssClass="inputproject" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-request lefttext">Proposal Sent On: </label>
            <div class="col-right-request righttext">
                <asp:TextBox ID="txtProposalSentOn" CssClass="inputprojectdate date"
                    onclick="WdatePicker({isShowClear:false});" runat="server"> </asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-request lefttext">PO #:</label>
            <div class="col-right-request righttext">
                <asp:TextBox ID="txtPO" CssClass="inputproject" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-right-request righttext">
                <asp:CheckBox runat="server" ID="chkLessThen" Text=" Does the PO total less then the proposal total?" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-request lefttext">Approved By:</label>
            <div class="col-right-request righttext">
                <asp:TextBox ID="txtApprovedBy" CssClass="inputproject" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-request lefttext">Approved On: </label>
            <div class="col-right-request righttext">
                <asp:TextBox ID="txtApprovedOn" CssClass="inputprojectdate date"
                    onclick="WdatePicker({isShowClear:false});" runat="server"> </asp:TextBox>
            </div>
        </div>

        <%--<div class="form-group">
            <label class="col-left-request lefttext">Invoice #:</label>
            <div class="col-right-request righttext">
                <asp:TextBox ID="txtInvoiceNo" CssClass="inputproject" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-request lefttext">Invoice Sent On: </label>
            <div class="col-right-request righttext">
                <asp:TextBox ID="txtInvoiceSentOn" CssClass="inputprojectdate date"
                    onclick="WdatePicker({isShowClear:false});" runat="server"> </asp:TextBox>
            </div>
        </div>--%>

        <div class="buttonBox2">
            <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" ValidationGroup="Add" CausesValidation="true"
                runat="server" Text="Save" OnClick="btnSave_Click" />
            <input type="button" value=" Back " class="cancelBtn1 mainbutton redirectback" />
        </div>
    </div>


    <div class="contentTitle titleeventlist">
        Project Payment
    </div>
    <div style="width:100%;overflow-x:auto;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance tabletimesheet noclickbind">
            <thead>
                <tr>
                    <th width="50">Milestone #</th>
                    <th width="120">Approved</th>
                    <th width="120">ETA Date</th>
                    <th width="120">Invoice #</th>
                    <th width="120">Send Date</th>
                    <th width="120">Due Date</th>
                    <th width="120">Receive Date</th>
                    <th width="80" class="aligncenter">
                        <div style="min-width: 50px;">Action</div>
                    </th>
                </tr>
            </thead>
            <tbody data-bind="foreach: { data: dataset, as: 'invoice' }">
                <tr data-bind="template: { name: 'editInvoice', data: invoice }, css: { 'whiterow': $index() % 2 === 1 }"></tr>
            </tbody>
        </table>
    </div>
    <script type="text/html" id="editInvoice">
        <td>
            <input readonly="true" data-bind="value: Milestone, attr: { id: 'txtMilestone' + Milestone() }" class="inputproject payInput required" />
        </td>
        <td>
            <select data-bind="value: Approved, attr: { id: 'selectApproved' + Milestone() }" name="selectApproved" class="small">
                <option value="0" selected="selected">No</option>
                <option value="1">Yes</option>
            </select>
        </td>
        <td>
            <input data-bind="value: ETADate, attr: { id: 'txtETADate' + Milestone() }" name="txtETADate" onclick="    WdatePicker({ isShowClear: false });" class="inputprojectdate payInput date" />
        </td>
        <td>
            <input data-bind="value: InvoiceNo, attr: { id: 'txtInvoiceNo' + Milestone() }, css: invoiceColor, event: { change: $root.invoiceChange }" name="txtInvoiceNo" class="inputproject payInput" />

        </td>
        <td>
            <input data-bind="value: SendOn, attr: { id: 'txtSendOn' + Milestone(), disabled: disabledSend() }, css: sendOnColor" onchange="sendOnChange(this.value,this.id)" name="txtSendOn" onclick="    WdatePicker({ isShowClear: false });" class="inputprojectdate payInput date" />
            <a data-toggle="popover" data-bind="attr: { id: 'tipSendOn' + Milestone() }, style: { display: sendOnColor == 'red' ? 'inline-block' : 'none' }" name="tipSendOn" class="info" title="Warning" href="###" data-container="body" data-placement="right"
                data-trigger="hover click" data-html="true" data-content="<span class='noticeRed'>This field should be fill in within 3 days after fill in invoice#. Please fill this field as soon as possible. </span>">&nbsp;</a>
        </td>
        <td>
            <input data-bind="value: DueOn, attr: { id: 'txtDueOn' + Milestone(), disabled: disabledSend() }" name="txtDueOn" onclick="    WdatePicker({ isShowClear: false });" class="inputprojectdate payInput date" />
        </td>
        <td>
            <input data-bind="value: ReceiveOn, attr: { id: 'txtReceiveOn' + Milestone(), disabled: disabledReceive() }, css: receiveOnColor" name="txtReceiveOn" onclick="    WdatePicker({ isShowClear: false });" class="inputprojectdate payInput date" />
        </td>
        <td class="aligncenter action">
            <a href="javascript:" data-bind="">
                <img src="/images/icons/save.png" title="Save" data-bind="event: { click: $root.saveInvoice }, visible: $root.showSave" />
                <!-- ko ifnot:$root.showSave() -->
                <img src="/images/icons/failed.png" title="Pending" />
                <!-- /ko -->
            </a>
            <a href="javascript:" data-bind="">
                <img src="/images/icons/newitem.png" title="New" data-bind="event: { click: $root.new }" /></a>
            <a data-toggle="modal" data-target="#modalsmall" data-bind="attr: { href: 'Comment.aspx?ID=' + invoice.id() }">
                <img src="/images/icons/edit.png" title="Comment" data-bind="visible: $root.dataset().length != Milestone() " /></a>
            <a href="javascript:" data-bind="visible: $root.dataset().length == Milestone() ">
                <img src="/images/icons/delete.png" title="Delete" data-bind="event: { click: $root.delInvoice }"></a>

        </td>
    </script>

    <div class="contentTitle titleeventlist">
        Related tickets
    </div>
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <ul class="listtopBtn">
                        <li data-toggle="modal" data-target="#modalsmall" href="AddRelatedticket.aspx?ID=<%=QS("ID") %>">
                            <div class="listtopBtn_icon">
                                <img src="/images/icons/waddrelatedticket.png" />
                            </div>
                            <div class="listtopBtn_text">Add Related Tickets </div>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>

    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="relatedticketTop">
        <tr>
            <td>
                <asp:Literal ID="litProjectName" runat="server"></asp:Literal>
                ( Total Hours: <span class="noticeRed">
                    <asp:Literal ID="litHours" runat="server"></asp:Literal></span>)</td>
        </tr>
    </table>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="70px">Priority</th>
                <th width="70px">Ticket ID</th>
                <th style="width: 30px">&nbsp;
                </th>
                <th>Title</th>
                <th width="100px">Status</th>
                <th width="80px">Created On</th>
                <th width="70px">Updated</th>
                <th width="80px">Created By</th>
                <th width="60px">Option</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptTicketsList" runat="server" OnItemDataBound="rpt_DataBound">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                        <td>
                            <%# Eval("Priority")%>
                        </td>
                        <td>
                            <%# Eval("TicketCode").ToString()%>
                        </td>
                        <td>
                            <%# ShowPriorityImgByDevDate(Eval("DeliveryDate").ToString())%>
                        </td>
                        <td>
                            <%# Eval("Title").ToString()%>
                        </td>
                        <td>
                            <%# Eval("Status").ToString().Replace("_", " ") %>
                        </td>
                        <td>
                            <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}") == "01/01/1753" ? "" : Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%# Eval("CreatedOn", "{0:MM/dd/yyyy}") == "01/01/1753" ? "" : Eval("CreatedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <asp:Literal ID="ltlCreatedByID" runat="server" Visible="false" Text='<%#Eval("CreatedBy")%>'></asp:Literal>
                            <asp:Literal ID="ltlCreatedByName" runat="server"></asp:Literal>
                        </td>
                        <td><a href="javascript:void(0);" onclick="deleteTicket(<%# Eval("TicketID") %>,this)">
                            <img border="0" title="Delete" src="/Images/icons/delete.png"></a></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <div class="contentTitle titleeventlist">
        Documents
    </div>
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <ul class="listtopBtn">
                        <li data-toggle="modal" data-target="#modalsmall" href="AddDocument.aspx?ID=<%=QS("ID") %>">
                            <div class="listtopBtn_icon">
                                <img src="/images/icons/wadddocument.png" />
                            </div>
                            <div class="listtopBtn_text">Add Documents</div>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th>Title</th>
                <th>File</th>
                <th>Tags</th>
                <th width="100px">Date</th>
                <th width="100px" style="text-align: center;">Action</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptDocuments" runat="server">
                <ItemTemplate>
                    <tr class=" <%#Container.ItemIndex % 2 == 0 ? "" : "whiterow"%>">
                        <td>
                            <%# Eval("ThumbPath")%></td>
                        <td><a style="text-decoration: underline" href='/Do/DoDownloadFileHandler.ashx?FileID=<%#Eval("FileID") %>&size=<%#Eval("FileSize") %>'
                            target="_blank"><%#Eval("FileTitle") %></a></td>
                        <td><%# Eval("Tags")%></td>
                        <td><%# Eval("CreatedOn", "{0:MM/dd/yyyy}")%></td>
                        <td style="text-align: center;"><a href="javascript:void(0);" onclick="deleteDocuments('<%# Eval("FileID") %>',this)">
                            <img border="0" title="Delete" src="/Images/icons/delete.png"></a></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div class="contentTitle titleeventlist">Notes</div>
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <ul class="listtopBtn">
                        <li data-toggle="modal" data-target="#modalsmall" href="AddNote.aspx?ID=<%=QS("ID") %>">
                            <div class="listtopBtn_icon">
                                <img src="/images/icons/waddnote.png" />
                            </div>
                            <div class="listtopBtn_text">Add Note</div>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">

        <tbody>
            <asp:Repeater ID="rptNotes" runat="server">
                <ItemTemplate>
                    <tr class=" <%#Container.ItemIndex % 2 == 0 ? "" : "whiterow"%>">
                        <td>
                            <div>
                                <div><strong><%# Eval("Title") %></strong></div>
                                <div>
                                    <%# Eval("Description") %>
                                </div>
                                <div class="subfeedbackDate">
                                    Edited  <%# Eval("ModifyOn","{0:MM/dd/yyyy HH:mm}") %>  by <%# Eval(UserNameDisplayProp) %>
                                </div>
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <script type="text/javascript">
        function Invoice(id, Milestone, InvoiceNo, SendOn, ReceiveOn, DueOn, Approved, ETADate, Color, ColorFor) {
            invoice = this;
            this.id = ko.observable(id);
            this.Milestone = ko.observable(Milestone);
            this.InvoiceNo = ko.observable(InvoiceNo);
            this.SendOn = ko.observable(SendOn);
            this.ReceiveOn = ko.observable(ReceiveOn);
            this.DueOn = ko.observable(DueOn);
            this.Approved = ko.observable(Approved);
            this.ETADate = ko.observable(ETADate);
            this.disabledSend = ko.observable(invoice.InvoiceNo() == "");
            this.disabledReceive = ko.observable(invoice.SendOn() == "");
            this.invoiceColor = getFieldColor("InvoiceNo", InvoiceNo, Color, ColorFor, Milestone);
            this.sendOnColor = getFieldColor("SendOn", SendOn, Color, ColorFor, Milestone);
            this.receiveOnColor = getFieldColor("ReceiveOn", ReceiveOn, Color, ColorFor);
        }

        function getFieldColor(field, value, corlor, colorFor, milestone) {
            if (colorFor == field && value == "" && corlor != "") {
                return corlor;
            }
            else
                return "";

        }
        var proposalId = '<%=QS("ID") %>';
        var projectStatus = ko.observable($("#<%=ddlStatus.ClientID %>").val());
        function invoiceViewModel(options) {
            var self = this;
            this.mileStoneFlag = 0;
            this.savedInvoice = 0;
            this.dataset = ko.observableArray(options.invoices);
            this.getDefaultInvoice = function () {
                var invoice = new Invoice("0", self.mileStoneFlag, "", "", "", "", 0, "", "", "");
                return invoice;
            };
            this.new = function () {
                self.mileStoneFlag++;
                var invoice = self.getDefaultInvoice();
                self.dataset.push(invoice);
            };
            this.invoiceChange = function (invoice) {
                if (invoice.InvoiceNo() == "") {
                    invoice.disabledSend(true);
                }
                else {
                    invoice.disabledSend(false);
                }
            };


            this.delInvoice = function (invoice) {
                if (invoice.id() > 0) {
                    jQuery.confirm("Are you sure you want to delete the invoice?", {
                        yesText: "Delete",
                        yesCallback: function () {
                            jQuery.post("/Service/Invoice.ashx", {
                                action: "delinvoice",
                                invoiceid: invoice.id(),
                                proposalId: proposalId,
                            }, function (response) {
                                if (response.success) {
                                    self.refresh();
                                    ShowMessage("Delete Success.", "success", false, false);
                                    window.location.reload();
                                } else {
                                    ShowMessage(response.msg, "danger");
                                }
                            }, 'json');
                        },
                        noText: "Cancel"
                    });
                }
                else {
                    if (invoice.Milestone() > 1) {
                        self.dataset.remove(invoice);
                        self.mileStoneFlag--;
                    }
                }
            };

            self.showSave= ko.observable(true);
            self.saveInvoice = function (invoice, callback) {
                self.showSave(false);
                var action = invoice.id() > 0 ? "editinvoice" : "addinvoice";
                jQuery.post("/Service/Invoice.ashx", {
                    action: action,
                    id: invoice.id(),
                    proposalId: proposalId,
                    milestone: action == "addinvoice" ? self.savedInvoice + 1 : invoice.Milestone(),
                    invoiceNo: invoice.InvoiceNo(),
                    sendOn: $("#txtSendOn" + invoice.Milestone()).val(),
                    receiveOn: $("#txtReceiveOn" + invoice.Milestone()).val(),
                    dueOn: $("#txtDueOn" + invoice.Milestone()).val(),
                    approved: invoice.Approved(),
                    etaDate: $("#txtETADate" + invoice.Milestone()).val(),
                    projectName: $("#dvDllProject option:selected").text()
                }, function (response) {
                    if (response.success) {
                        if (action == "addinvoice") {
                            invoice.id(response.id);
                            self.refresh();
                            ShowMessage("Add Success.", "success", false, false);
                        }
                        else {
                            self.refresh();
                            ShowMessage("Update Success.", "success", false, false);
                        }
                        window.location.reload();
                    }
                    else {
                        ShowMessage(response.msg, "danger");
                    }
                });
            };

            self.refresh = function () {
                self.dataset.removeAll();
                jQuery.getJSON("/Service/Invoice.ashx", { action: "getproposalinvoices", proposalId: proposalId }, function (invoices) {
                    self.mileStoneFlag = invoices.length;
                    self.savedInvoice = invoices.length;
                    if (invoices && invoices.length) {
                        for (var i = 0; i < invoices.length; i++) {
                            var invoice = invoices[i];
                            invoice.SendOn = formatDate(invoice.SendOn);
                            invoice.ReceiveOn = formatDate(invoice.ReceiveOn);
                            invoice.DueOn = formatDate(invoice.DueOn);
                            invoice.ETADate = formatDate(invoice.ETADate);
                            self.dataset.push(new Invoice(invoice.ID, invoice.Milestone, invoice.InvoiceNo, invoice.SendOn, invoice.ReceiveOn, invoice.DueOn, invoice.Approved, invoice.ETADate, invoice.Color, invoice.ColorFor));
                        }
                    } else {
                        self.new();
                    }
                }).always(function () {
                    
                })
            };
            self.refresh();
        }

        var _invoiceModel;
        jQuery(function () {
            _invoiceModel = new invoiceViewModel({

            });
            ko.applyBindings(_invoiceModel, document.body);

        });
        function sendOnChange(sendOn, sendOnId) {
            var receiveOnId = "txtReceiveOn" + sendOnId.charAt(sendOnId.length - 1);
            if (sendOn == "") {
                $("#" + receiveOnId).attr("disabled", true);
            }
            else {
                $("#" + receiveOnId).attr("disabled", false);
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
    </script>
    <script type="text/javascript">
        function validationFile() {
            jQuery("#hdFileUpolad").val(jQuery("#<%=fileUpload.ClientID %>").val());
        }

        function deleteTicket(id, o) {
            jQuery.confirm("Are you sure you want to delete this ticket? ", {
                yesText: "Delete",
                yesCallback: function () {
                    $.ajax({
                        type: "post",
                        url: "/Do/DoDeleteRelationWorkRequest.ashx?r=" + Math.random(),
                        data: {
                            ticketId: id,
                            wrId: '<%=QS("ID") %>'
                        },
                        success: function (result) {
                        }
                    });
                    jQuery(o).parent().parent().remove();
                },
                noText: "No",
                noCallback: function () { }
            });
        }

        function deleteDocuments(id, o) {
            jQuery.confirm("Are you sure you want to delete this document? ", {
                yesText: "Delete",
                yesCallback: function () {
                    $.ajax({
                        type: "post",
                        url: "/Do/DoDeleteRelationDocument.ashx?r=" + Math.random(),
                        data: {
                            fileId: id,
                            wrId: '<%=QS("ID") %>'
                        },
                        success: function (result) {
                        }
                    });
                    jQuery(o).parent().parent().remove();
                },
                noText: "No",
                noCallback: function () { }
            });
        }

        function statusChange() {
            $("#<%=txtProposalSentTo.ClientID%>").removeClass("required");
            $("#<%=txtProposalSentOn.ClientID%>").removeClass("required");
            $("#<%=txtApprovedBy.ClientID%>").removeClass("required");
            $("#<%=txtApprovedOn.ClientID%>").removeClass("required");
            <%--$("#<%=txtInvoiceNo.ClientID%>").removeClass("required");
            $("#<%=txtInvoiceSentOn.ClientID%>").removeClass("required");--%>
            var status = $("#<%=ddlStatus.ClientID %>").val();
            console.log(status);
            if (status == 3) {
                $("#<%=txtProposalSentTo.ClientID%>").addClass("required");
                $("#<%=txtProposalSentOn.ClientID%>").addClass("required");
                return;
            }
            if (status == 4) {
                $("#<%=txtApprovedBy.ClientID%>").addClass("required");
                $("#<%=txtApprovedOn.ClientID%>").addClass("required");
                return;
            }
            <%--if (status == 6) {
                $("#<%=txtInvoiceNo.ClientID%>").addClass("required");
                $("#<%=txtInvoiceSentOn.ClientID%>").addClass("required");
                return;
            }--%>
        }

    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
