<%@ Page Title="" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="AddProposalTracker.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.ProposalTracker.AddProposalTracker" %>

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
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contentTitle titlsealrequest">Basic Information</div>
    <div style="width: 840px;">
        <div class="form-group">
            <label class="col-left-request lefttext">Project:<span class="noticeRed">*</span></label>
            <div class="col-right-request righttext">
                <asp:DropDownList ID="ddlProject" data-msg="Please select project" CssClass="selectrequest" runat="server" min="1">
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-request lefttext">Status:<span class="noticeRed">*</span></label>
            <div class="col-right-request righttext">
                <asp:DropDownList ID="ddlStatus" runat="server" data-msg="Please select status" CssClass="selectproject" min="1">
                    <asp:ListItem Text="Awaiting ETA" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Awaiting Proposal" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Awaiting Approval/PO" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Awaiting Development" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Awaiting Sending Invoice" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Awaiting Payment" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Paid/Completed " Value="7"></asp:ListItem>
                    <asp:ListItem Text="On Hold" Value="8"></asp:ListItem>
                    <asp:ListItem Text="Not Approved" Value="9"></asp:ListItem>
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
                <input type="text" style="display: none;" id="hdFileUpolad" name="hdFileUpolad" />
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
    <script type="text/javascript">
        function validationFile() {
            jQuery("#hdFileUpolad").val(jQuery("#<%=fileUpload.ClientID %>").val());
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
