<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PMReview.aspx.cs" MasterPageFile="~/Pop.master" Inherits="SunNet.PMNew.PM2014.Ticket.Sunnet.PMReview" %>
<%@ Import Namespace="SunNet.PMNew.Entity.TicketModel" %>

<%@ Register Src="~/UserControls/Sunnet/UsersView.ascx" TagPrefix="custom" TagName="ticketUsersView" %>
<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagPrefix="custom" TagName="uploader" %>

<asp:Content ID="headSection" ContentPlaceHolderID="head" runat="server">
    <style>
        #accounting table td {
            padding-right: 10px;
        }
          .error {
            color: #f00;
            border: 1px solid #f00;
            background-color: #ffecec;
          }
    </style>
    <script type="text/javascript">
        var denyStatus = "16";
        var assignUserValidateMsg = " Please assign a ticket user first.";
        var isEstimation = "<%=ticketsEntity.IsEstimates%>";
        var accounting = "<%=(Int32)ticketsEntity.Accounting%>";
        var ticketStatus = "<%= (int)ticketsEntity.Status %>";
        var esTimationUserId = "<%=ticketsEntity.EsUserID%>";
        var confirmEstmateUserId = "<%=ticketsEntity.ConfirmEstmateUserId%>";
        var responsibleUserId = "<%=ticketsEntity.ResponsibleUser %>";

        $(function () {
            //显示Accounting
            var rdoInput = $("#<%=rdoAccounting.ClientID%>").find("input");
            for (var i = 0; i < rdoInput.length; i++) {
                if ($(rdoInput[i]).val() == accounting)
                    $(rdoInput[i]).prop("checked", true);
            }

            $("#<%=ddlStatus.ClientID%>").on("change", function () { onChangeStatus(); });
            onChangeStatus();
            if ($("#<%=rdoEstimationYes.ClientID%>").prop("checked") == true) {
                $("#dvAssignEstUser").css("display", "block");
                if (!$("#<%=ddlEstUser.ClientID%>").attr("min") != null) {
                    $("#<%=ddlEstUser.ClientID%>").attr("min", "1").attr("data-msg", assignUserValidateMsg);
                }
            }
            $("#<%=rdoEstimationYes.ClientID%>").on("click", function () {
                if ($(this).prop("checked") == true) {
                    $("#dvAssignEstUser").css("display", "block");
                    if (!$("#<%=ddlEstUser.ClientID%>").attr("min") != null) {
                        $("#<%=ddlEstUser.ClientID%>").attr("min", "1").attr("data-msg", assignUserValidateMsg);
                    }
                    $("#<%=rdoEstimationNo.ClientID%>").prop("checked", false);
                }
                else {
                    $("#dvAssignEstUser").css("display", "none");
                    $("#<%=ddlEstUser.ClientID%>").removeAttr("min").removeAttr("data-msg");
                    $("#<%=rdoEstimationNo.ClientID%>").prop("checked", true);
                }
            });

            $("#<%=rdoEstimationNo.ClientID%>").on("click", function () {
                if ($(this).prop("checked") == true) {
                    $("#<%=rdoEstimationYes.ClientID%>").prop("checked", false);
                    $("#dvAssignEstUser").css("display", "none");
                    $("#<%=ddlEstUser.ClientID%>").removeAttr("min").removeAttr("data-msg");

                    if ($("#<%=ddlStatus.ClientID%>").val() == "<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.Waiting_For_Estimation%>") {
                        $("#<%=ddlStatus.ClientID%>").val("<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.PM_Reviewed%>")
                    }
                }
                else {
                    $("#<%=rdoEstimationYes.ClientID%>").prop("checked", true);
                    $("#dvAssignEstUser").css("display", "block");
                    if (!$("#<%=ddlEstUser.ClientID%>").attr("min") != null) {
                        $("#<%=ddlEstUser.ClientID%>").attr("min", "1").attr("data-msg", assignUserValidateMsg);
                    }

                }
            });

            if ($("#<%=ddlStatus.ClientID%>").val() == denyStatus) {
                $("#<%=dvDenySection.ClientID%>").css("display", "block");
            }

            if ($("#<%=rdoRequestYes.ClientID%>").prop("checked") == true) {
                $("#dvChangetoRequestDesc").css("display", "block");
            }

            $("#<%=rdoRequestYes.ClientID%>").on("click", function () {
                if ($(this).prop("checked") == true) {
                    $("#dvChangetoRequestDesc").css("display", "block");
                    $("#<%=rdoReuqestNo.ClientID%>").prop("checked", false);
                }
                else {
                    $("#dvChangetoRequestDesc").css("display", "none");
                    $("#<%=rdoReuqestNo.ClientID%>").prop("checked", true);
                }
            });
            $("#<%=rdoReuqestNo.ClientID%>").on("click", function () {
                if ($(this).prop("checked") == true) {
                    $("#<%=rdoRequestYes.ClientID%>").prop("checked", false);
                    $("#dvChangetoRequestDesc").css("display", "none");
                }
                else {
                    $("#<%=rdoRequestYes.ClientID%>").prop("checked", true);
                    $("#dvChangetoRequestDesc").css("display", "block");
                }
            });

            // validate signup form on keyup and submit
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
            });



            $("#dvUsers").find("input[type=\"checkbox\"]").on("click", function () {
                var ddlEstUser = $("#<%=ddlEstUser.ClientID%>");
                var $ddlConfirmEstmateUserId = $("#<%=ddlConfirmEstmateUserId.ClientID%>");
                var $ddlResponsibleUser = $("#<%=ddlResponsibleUser.ClientID%>");
                if ($(this).prop("checked")) {
                    if (ddlEstUser.find("option").length == 1 && ddlEstUser.find("option[value=\"-1\"]").length > 0) {
                        ddlEstUser.find("option").remove();
                    }
                    if ($(this).closest("span").attr("data-us"))
                        $ddlConfirmEstmateUserId.prepend("<option value=\"" + $(this).closest("span").attr("data-id") + "\" checked='checked'>" + $(this).closest("span").find("label").text() + "</option>");

                    ddlEstUser.append("<option value=\"" + $(this).closest("span").attr("data-id") + "\">" + $(this).closest("span").find("label").text() + "</option>");
                    $("#<%=hdAssignUserID.ClientID%>").val($(this).closest("span").attr("data-id"));
                    $ddlResponsibleUser.prepend("<option value=\"" + $(this).closest("span").attr("data-id") + "\">" + $(this).closest("span").find("label").text() + "</option>");
                }
                else {
                    if (ddlEstUser.find("option").length == 1 && ddlEstUser.find("option[value=\"-1\"]").length == 0) {
                        ddlEstUser.append("<option value=\"-1\">Please select...</option>");
                        $("#<%=hdAssignUserID.ClientID%>").val("");
                    }
                    ddlEstUser.find("option[value=\"" + $(this).closest("span").attr("data-id") + "\"]").remove();
                    $ddlConfirmEstmateUserId.find("option[value=\"" + $(this).closest("span").attr("data-id") + "\"]").remove();
                    $ddlResponsibleUser.find("option[value=\"" + $(this).closest("span").attr("data-id") + "\"]").remove();
                }
            });

            $("#<%=ddlEstUser.ClientID%>").on("change", function () {
                $("#<%=hdAssignUserID.ClientID%>").val($(this).val());
            });

            setDefaultEstUsers();
        });

        function setDefaultEstUsers() {
            //如果已经分配了人，那要把这些人加到ddlEstUser的选择项里.
            //并且默认第一个人的值到隐藏域中。
            var selectedEstUser = $("#dvUsers").find("input[type=\"checkbox\"]:checked");
            var ddlEstUser = $("#<%=ddlEstUser.ClientID%>");
            var $ddlResponsibleUser = $("#<%=ddlResponsibleUser.ClientID%>");
            var i = 0;
            selectedEstUser.each(function (index, item) {
                if (!$(item).closest("span").parent("li").attr("data-iscreate")) {
                    var dataSpan = $(item).closest("span");
                    if (esTimationUserId == dataSpan.attr("data-id"))
                        ddlEstUser.append("<option value=\"" + dataSpan.attr("data-id") + "\" selected='selected'>" + dataSpan.find("label").text() + "</option>");
                    else
                        ddlEstUser.append("<option value=\"" + dataSpan.attr("data-id") + "\">" + dataSpan.find("label").text() + "</option>");
                    if (i == 0) {
                        $("#<%=hdAssignUserID.ClientID%>").val($(selectedEstUser.get(0)).closest("span").attr("data-id"));
                    }

                    if (responsibleUserId == dataSpan.attr("data-id"))
                        $ddlResponsibleUser.prepend("<option value=\"" + dataSpan.attr("data-id") + "\" selected='selected'>" + dataSpan.find("label").text() + "</option>");
                    else
                        $ddlResponsibleUser.prepend("<option value=\"" + dataSpan.attr("data-id") + "\">" + dataSpan.find("label").text() + "</option>");

                    i++;
                }
            });

            var $ddlConfirmEstmateUserId = $("#<%=ddlConfirmEstmateUserId.ClientID%>");
            $("#divUSUsers").find("input[type='checkbox']:checked").each(function () {

                if (confirmEstmateUserId == $(this).closest("span").attr("data-id"))
                    $ddlConfirmEstmateUserId.prepend("<option value=\"" + $(this).closest("span").attr("data-id") + "\" Selected='True'>" + $(this).closest("span").find("label").text() + "</option>");
                else
                    $ddlConfirmEstmateUserId.prepend("<option value=\"" + $(this).closest("span").attr("data-id") + "\" Selected='True'>" + $(this).closest("span").find("label").text() + "</option>");
            });

            if (ddlEstUser.find("option").length > 1 && ddlEstUser.find("option[value=\"-1\"]").length > 0) {
                ddlEstUser.find("option[value=\"-1\"]").remove();
            }

        }

        function isNeedEstmation() {
            return isEstimation === "True" ||
                ($("#<%=rdoEstimationYes.ClientID%>").prop("checked"));
        }

        function onChangeStatus() {
            onReviewEstimation();
            onReviewDeny();
            setConvertoRequest();
            setBacktoStatus();
            onReviewEstimationApprover();
        }

        function onReviewEstimation() {
            var selectedStatus = $("#<%=ddlStatus.ClientID%>").val();

            //这里处理的是Estimation的显示控制， 如果是PMReviewd 
            //如果是从 >= Estimation_Approved 状态回到  PMReviewd 状态的，则不显示
            if (selectedStatus === "<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.PM_Reviewed%>"
                && ticketStatus < "<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.Estimation_Approved %>") {
                //显示分配用户
                //dvEstimation
                $("#<%=dvEstimation.ClientID%>").css("display", "block");
                if (isEstimation === "True") {
                    $("#<%=rdoEstimationYes.ClientID%>").prop("checked", true);
                    $("#dvAssignEstUser").css("display", "block");
                    if (!$("#<%=ddlEstUser.ClientID%>").attr("min") != null) {      //添加AssignEstUser的验证
                        $("#<%=ddlEstUser.ClientID%>").attr("min", "1").attr("data-msg", assignUserValidateMsg);
                    }
                }
                else {
                    $("#<%=rdoEstimationYes.ClientID%>").prop("checked", false);
                    $("#dvAssignEstUser").css("display", "none");
                    $("#<%=ddlEstUser.ClientID%>").removeAttr("min").removeAttr("data-msg");//删除AssignEstUser的验证
                }


            }
            else {  //否则就不显示分配用户 且删除分配用户的验证
                $("#<%=dvEstimation.ClientID%>").css("display", "none");
                $("#<%=ddlEstUser.ClientID%>").removeAttr("min").removeAttr("data-msg");
            }
        }

        function onReviewDeny() {
            var selectedStatus = $("#<%=ddlStatus.ClientID%>").val();
            if (selectedStatus == denyStatus) {   //如果当前选择的是deny状态 就显示denyReason否则就关闭
                $("#<%=dvDenySection.ClientID%>").css("display", "block");
            }
            else {
                $("#<%=dvDenySection.ClientID%>").css("display", "none");
            }
        }

        function onReviewEstimationApprover() {
            var $divEstimationApprover = $("#divEstimationApprover");
            $divEstimationApprover.css("display", "none");
            var selectedStatus = $("#<%=ddlStatus.ClientID%>").val();
            $("#divEstimationApproved1").css("display", "none");
            $("#divEstimationApproved2").css("display", "none");
            if (selectedStatus === "<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.Waiting_Confirm %>") {
                $divEstimationApprover.css("display", "inlineblock");
                $("#<% =ddlConfirmEstmateUserId.ClientID%>").attr("disabled", false);
            }
            else if (selectedStatus === "<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.Estimation_Approved %>") {
                $divEstimationApprover.css("display", "inlineblock");
                $("#<% =ddlConfirmEstmateUserId.ClientID%>").attr("disabled", "disabled");
                $("#divEstimationApproved1").css("display", "block");
                $("#divEstimationApproved2").css("display", "block");
            }
        }


        function setBacktoStatus() {//设置确认时间
            var selectedStatus = $("#<%=ddlStatus.ClientID%>").val();
            //如果需要估时(这里要判断是原来的Ticket是否是需要估时，与你radioButton无关)，并且已经在估时进行中要退回到PMReview状态或之前的估时状态时
            $("#<%=dvEstimationUser.ClientID%>").css("display", "none");
            $("#<%=estimationUser.ClientID%>").css("display", "none");
            $("#<%=dvInitialTime.ClientID%>").css("display", "none");
            $("#<%=dvFinalTime.ClientID%>").css("display", "none");

            if (selectedStatus != "-1") {
                if (isEstimation) {
                    if (selectedStatus === "<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.Waiting_Confirm%>"
                        || selectedStatus === "<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.Estimation_Approved%>"
                    ) {
                        $("#<%=dvEstimationUser.ClientID%>").css("display", "block");
                        $("#<%=estimationUser.ClientID%>").css("display", "inlineblock");
                    $("#<%=dvInitialTime.ClientID%>").css("display", "inlineblock");
                    $("#<%=dvFinalTime.ClientID%>").css("display", "inlineblock");
                    }
                }
            }
        }

        function setConvertoRequest() {
            var selectedStatus = $("#<%=ddlStatus.ClientID%>").val();
            //在PM_Review时及以在Sales确认估时开始开发以前都可以把Bug类型的Ticket转为Request类型。
            if (selectedStatus >= "<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.PM_Reviewed %>" &&
                status <= "<%=(int)SunNet.PMNew.Entity.TicketModel.TicketsState.Estimation_Approved%>" &&
                "<%=ticketsEntity.TicketType == SunNet.PMNew.Entity.TicketModel.TicketsType.Bug%>" === "True") {
                $("#<%=dvChangeRequest.ClientID%>").css("display", "block");
            }
            else {
                $("#<%=dvChangeRequest.ClientID%>").css("display", "none");
            }
        }

    </script>
</asp:Content>

<asp:Content ID="titleSection" ContentPlaceHolderID="titleSection" runat="server">
    PM Review
    
</asp:Content>

<asp:Content ID="bodySection" ContentPlaceHolderID="bodySection" runat="server">
    <div style="font-size: 13px; font-weight: bold; padding-bottom: 20px;">
        <asp:Literal runat="server" ID="litHead"></asp:Literal>
        <asp:HiddenField ID="HiddenField_TicketCreateId" Value="-1" runat="server" />
    </div>
    

    <div class="assignuserTitle">Assign User to Ticket:</div>
    <div id="dvUsers">
        <custom:ticketUsersView runat="server" ID="ticketUsersView" />
    </div>

    <div class="sepline2"></div>
    <div class="assignuserTitle">

        <div class="assignuserTitle">Assign Responsible User:</div>
        <div class="form-group">
            <span class="col-left-1 lefttext" style="width: auto !important; font-weight: bold; text-align: right;">Change Status to :</span>
        <asp:DropDownList runat="server" ID="ddlStatus" CssClass="selectw3">
            <asp:ListItem Value="-1" Text="not change"></asp:ListItem>
            <asp:ListItem Value="2" Text="Cancelled"></asp:ListItem>
            <asp:ListItem Value="3" Text="PM Reviewed"></asp:ListItem>
            <%--            <asp:ListItem Value="4" Text="Waiting For Estimation"></asp:ListItem>--%>
            <%--            <asp:ListItem Value="5" Text="PM Verify Estimation"></asp:ListItem>--%>
            <asp:ListItem Value="6" Text="Waiting Confirm"></asp:ListItem>
            <%--          <asp:ListItem Value="7" Text="Denied"></asp:ListItem>--%>
            <asp:ListItem Value="8" Text="Estimation Approved"></asp:ListItem>
            <asp:ListItem Value="9" Text="Developing"></asp:ListItem>
            <%--            <asp:ListItem Value="10" Text="Testing On Local"></asp:ListItem>
            <asp:ListItem Value="11" Text="Tested Fail On Local"></asp:ListItem>
            <asp:ListItem Value="12" Text="Tested Success On Local"></asp:ListItem>
            <asp:ListItem Value="13" Text="Testing On Client"></asp:ListItem>
            <asp:ListItem Value="14" Text="Tested Fail On Client"></asp:ListItem>
            <asp:ListItem Value="15" Text="Tested Success On Client"></asp:ListItem>--%>
            <asp:ListItem Value="16" Text="PM Deny"></asp:ListItem>
            <asp:ListItem Value="17" Text="Ready For Review"></asp:ListItem>
            <asp:ListItem Value="18" Text="Not Approved"></asp:ListItem>
            <asp:ListItem Value="19" Text="Completed"></asp:ListItem>
            <asp:ListItem Value="20" Text="Internal Cancel"></asp:ListItem>
        </asp:DropDownList>
        </div>
        <div class="form-group" id="divEstimationApprover">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            Estimation Approver:
             <select runat="server" id="ddlConfirmEstmateUserId" class="selectw3">
             </select>
        </div>
    </div>


    <div class="assignuserTitle" id="divEstimationApproved1">
        <div class="form-group">
            Proprosal Name:&nbsp;&nbsp;
      <asp:TextBox class="inputw2" runat="server" ID="txtProprosalName" Width="200" MaxLength="100">
      </asp:TextBox>

        </div>
        <div class="form-group" id="div1">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            Work Plan:
            <asp:TextBox class="inputw2" runat="server" ID="txtWorkPlanName" Width="200" MaxLength="100">
            </asp:TextBox>
        </div>
    </div>

    <div class="assignuserTitle" id="divEstimationApproved2">
        <div class="form-group">
            Work Scope:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           <asp:TextBox class="inputw2" runat="server" ID="txtWorkScope" Width="200" MaxLength="100">
           </asp:TextBox>
        </div>
        <div class="form-group" id="div2">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            Invoice #:&nbsp;&nbsp;
               <asp:TextBox class="inputw2" runat="server" ID="txtInvoice" Width="200" MaxLength="100">
               </asp:TextBox>
        </div>
    </div>






    <div class="assignuserTitle">
        <div class="form-group">
            <span class="col-left-1 lefttext" style="width: auto !important; font-weight: bold; text-align: right;">Responsible User :</span>
             <select runat="server" id="ddlResponsibleUser" class="selectw3">
             </select>
        </div>
    </div>


    <div id="dvDenySection" style="display: none;" runat="server">
        <div class="form-group">
            <label class="col-left-owreason lefttext">Description:</label>
            <div class="col-right-owreason righttext">
                <asp:TextBox TextMode="MultiLine" runat="server" ID="txtBoxDenyReason" Rows="4" CssClass="inputpmreview"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-owreason lefttext">Upload a Files:</label>
            <div class="col-right-owreason righttext">
                <custom:uploader runat="server" ID="uploader" UploadType="Add" FileUploadCount="1" />
            </div>
        </div>
    </div>
    <div id="dvEstimation" runat="server">
        <div class="sepline2"></div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td>
                        <div class="pmreviwDiv1">
                            <span class="assignuserTitle">Estimation: </span><span class="rightItem">
                                <asp:RadioButton runat="server" ID="rdoEstimationYes" GroupName="radioEstimation" Text=" Yes" />
                            </span><span class="rightItem">
                                <asp:RadioButton runat="server" ID="rdoEstimationNo" GroupName="radioEstimation" Text=" No" />
                            </span>
                        </div>
                        <div class="assignuserTitle" id="dvAssignEstUser" style="display: none;">
                            Assign User to Estimation:
                            <select runat="server" id="ddlEstUser" class="selectw3">
                                <option value="-1">Please select...</option>
                            </select>
                            <asp:HiddenField runat="server" ID="hdAssignUserID" />
                        </div>


                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div runat="server" id="dvEstimationUser">
        <div class="sepline2"></div>
        <div class="form-group" runat="server" id="estimationUser" style="display: none;">
            <label class="lefttext">Estimation User: </label>
            <div class="righttext" style="width: 173px;">
                <asp:Literal runat="server" ID="ltrlEstimationUser"></asp:Literal>
            </div>
        </div>
        <div class="form-group" runat="server" id="dvInitialTime" style="display: none;">
            <label class="lefttext">Initial Time: </label>
            <div class="righttext" style="width: 130px;">
                <asp:TextBox type="text" class="inputnum required number" onkeyup="FormatFloatNumber(this,15.2)" Text="0" runat="server" ID="txtInitialTime" Width="40">
                </asp:TextBox>
            </div>
        </div>
        <div class="form-group" runat="server" id="dvFinalTime" style="display: none;">
            <label class="lefttext">Final Time: </label>
            <div class="righttext" style="width: 130px;">
                <asp:TextBox type="text" class="inputnum required number" onkeyup="FormatFloatNumber(this,15.2)" Text="0" runat="server" ID="txtBoxExtimationHours" Width="40">
                </asp:TextBox>
            </div>
        </div>
    </div>

    <div class="sepline2"></div>
    <div runat="server" id="dvChangeRequest">
        <div class="pmreviwDiv1">
            <span class="assignuserTitle">Change to Request: </span>
            <span class="rightItem">
                <asp:RadioButton runat="server" ID="rdoRequestYes" Text=" Yes" />
            </span><span class="rightItem">
                <asp:RadioButton runat="server" ID="rdoReuqestNo" Text=" No" Checked="true" />
            </span>
        </div>
        <div id="dvChangetoRequestDesc" style="display: none;">
            <div class="form-group">
                <label class="col-left-owreason lefttext">Description:</label>
                <div class="col-right-owreason righttext">
                    <asp:TextBox TextMode="MultiLine" runat="server" ID="txtBoxConvertDescr" Rows="4" CssClass="inputpmreview"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div>
        <div class="assignuserTitle">Assign Client:</div>
        <div class="form-group" runat="server" id="dvClient">
            <span class="col-left-1 lefttext" style="width: auto; font-weight: bold; text-align: right;">Client : </span>
            <asp:DropDownList runat="server" ID="ddlClient" CssClass="selectw3 pmreviwDiv1">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <span class="col-left-1 lefttext" style="width: auto; font-weight: bold;text-align: right;">Accounting<i style="color: red;"> * </i> : </span>
            <div id="accounting" class="col-right-1 righttext">
                <asp:RadioButtonList ID="rdoAccounting" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" CssClass="righttext">
                    <asp:ListItem Text=" Proposal" Value="1"></asp:ListItem>
                    <asp:ListItem Text=" Time" Value="2"></asp:ListItem>
                    <asp:ListItem Text=" Not Billable" Value="3"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:DropDownList ID="ddl_Proposal" runat="server" CssClass="righttext"></asp:DropDownList>
                <label runat="server" cssclass="righttext" id="lbl_error" style="display: none; color: red;">Proposal is required.</label>
                <asp:HiddenField ID="hid_Proposal" runat="server" />
            </div>
        </div>
        <div class="sepline2"></div>
    </div>
</asp:Content>

<asp:Content ID="buttonSection" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button runat="server" ID="btnSave" Text="Save &amp; Close" CssClass="saveBtn1 mainbutton" OnClick="btnSave_Click" OnClientClick=" return Saverdo()" />
    <input name="Input322" type="button" class="cancelBtn1 mainbutton" data-dismiss="modal" aria-hidden="true" value="Cancel">


    <script type="text/javascript">
        function Saverdo() {
            var selectedList = $("#<%=rdoAccounting.ClientID%>").find("input:radio:checked");
            if (selectedList.length == 0) {
                alert("Accounting cannot be null.");
                return false;
            }
            else if ($("#<%=rdoAccounting.ClientID%>").find("input:radio:checked").attr("value") == '1') {
                if ($("#<%=ddl_Proposal.ClientID%>").val() == "") {
                    $("#<%=lbl_error.ClientID%>").show();
                    $("#<%=ddl_Proposal.ClientID%>").addClass("error");
                    return false;
                }
            }
            else {
                return true;
            }
        }
        $(function () {

            if ($("#<%=hid_Proposal.ClientID%>").val() == "")
                $("#<%=hid_Proposal.ClientID%>").val($("#<%=ddl_Proposal.ClientID%>").val());


            if ($("#<%=rdoAccounting.ClientID%>").find("input:radio:checked").attr("value") == '1') {
                $("#<%=ddl_Proposal.ClientID%>").show();
            //$("#<%=ddl_Proposal.ClientID%>").addClass("error");

            }
            else {
                $("#<%=ddl_Proposal.ClientID%>").hide();
            }


            $("#<%=rdoAccounting.ClientID%>").find("input:radio").each(function () {
                $(this).click(function () {
                    if ($(this).attr("value") == '1') {
                        $("#<%=hid_Proposal.ClientID%>").val($("#<%=ddl_Proposal.ClientID%>").val());
                        $("#<%=ddl_Proposal.ClientID%>").show();
                    //$("#<%=ddl_Proposal.ClientID%>").addClass("required");
                    }
                    else {
                    //$("#<%=ddl_Proposal.ClientID%>").removeClass("required");
                        $("#<%=ddl_Proposal.ClientID%>").hide();
                        $("#<%=lbl_error.ClientID%>").hide();
                        $("#<%=ddl_Proposal.ClientID%>").removeClass("error");
                    }

                });
            });
            $("#<%=ddl_Proposal.ClientID%>").change(function() {
                $("#<%=hid_Proposal.ClientID%>").val($("#<%=ddl_Proposal.ClientID%>").val());
                $("#<%=lbl_error.ClientID%>").hide();
            });

            $("#<%=ddlStatus.ClientID%>").change(function () {
                if ($(this).val() ==<%= (int)TicketsState.Ready_For_Review%>) {
                    var ticketCreaterId = <%=HiddenField_TicketCreateId.Value%>;
                    $("#<%=ddlResponsibleUser.ClientID%>").val(ticketCreaterId);
                }
            });
        });
    </script>
</asp:Content>

