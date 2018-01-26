<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SunnetTicket/Sunnet.master" CodeBehind="New.aspx.cs" Inherits="SunNet.PMNew.PM2014.Ticket.Sunnet.New" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/Sunnet/UsersView.ascx" TagPrefix="custom" TagName="ticketUsersView" %>
<%@ Register Src="~/UserControls/Messager.ascx" TagName="Messager" TagPrefix="custom" %>
<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagName="fileuploadder" TagPrefix="custom" %>
<%@ Import Namespace="SunNet.PMNew.Entity.UserModel" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <style>
        #dvType table td, #dvPriority table td, #<%=dvAccounting.ClientID%> table td {
            padding-right: 10px;
        }

        #dvType, #dvPriority, #<%=dvAccounting.ClientID%> {
            border: 0;
        }

        .saveHiddenBtn {
            visibility: hidden;
        }
        @media (max-width: 992px){
            #body_body_dataSection_txtDesc.inputw2{
                width:455px !important;
            }
            #body_body_dataSection_ddlProject{
                width:461px !important;
            }
        }
    </style>
    <script src="/Scripts/webuploader/webuploader.js"></script>
    <script src="/Scripts/webuploader/uploader.js"></script>
</asp:Content>
<asp:Content ID="data" ContentPlaceHolderID="dataSection" runat="server">
    <div style="width: 696px;" class="form-group-container">
        <custom:Messager ID="Messager1" runat="server" />
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                 <div class="form-group">
                     <label class="col-left-1 lefttext">Project:<span class="noticeRed">*</span></label>
                     <div class="col-right-1 righttext">
                         <asp:DropDownList ID="ddlProject" runat="server" CssClass="selectw2 required" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                         </asp:DropDownList>
                     </div>
                 </div>
             </ContentTemplate>
        </asp:UpdatePanel>
        <div class="form-group">
            <label class="col-left-1 lefttext">Title:<span class="noticeRed">*</span></label><div class="col-right-1 righttext">
                <asp:TextBox runat="server" class="inputw2 required" ID="txtTitle" TabIndex="2"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-1 lefttext">Description:<span class="noticeRed">*</span></label><div class="col-right-1 righttext">
                <textarea id="txtDesc" runat="server" cols="20" class="inputw2 required" rows="6" tabindex="3"></textarea>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">URL:</label><div class="col-right-1 righttext">
                <asp:TextBox runat="server" class="inputw2" ID="txtUrl" TabIndex="4" MaxLength="290"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">Attachments:</label>
            <div class="col-right-1 righttext">
                <div id="thelist" class="uploader-list"></div>
                <h5 id="speed" style="display: none;"></h5>
                <div id="picker">Select Files (Up to 5 files)</div>
                <asp:HiddenField ID="hidUploadFile" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">Type:<span class="noticeRed">*</span></label>
            <div class="col-right-1 righttext" id="dvType">
                <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal" CssClass="rightItem" CellPadding="0" CellSpacing="0">
                    <asp:ListItem Text=" Request" Value="1" Selected="True" TabIndex="6"> </asp:ListItem>
                    <asp:ListItem Text=" Bug" Value="0" TabIndex="7"> </asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext">Priority:<span class="noticeRed">*</span></label>
            <div class="col-right-1 righttext" id="dvPriority">
                <asp:RadioButtonList runat="server" ID="rdoPriority" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0">
                    <asp:ListItem Text=" Low" Value="1" TabIndex="11"></asp:ListItem>
                    <asp:ListItem Text=" Medium" Value="2" Selected="True" TabIndex="12"></asp:ListItem>
                    <asp:ListItem Text=" High" Value="3" TabIndex="13"></asp:ListItem>
                    <asp:ListItem Text=" Emergency" Value="4" TabIndex="14"></asp:ListItem>
                </asp:RadioButtonList>
                <span class="redstar" style="display: none;">**The Emergency Support fee will be based on 1.5 times the regular
                        rate. </span>
            </div>
        </div>

        
           
        <div class="form-group">
             
            <label class="col-left-1 lefttext" runat="server" id="lblAccounting" style="display:none;" >Accounting:<span class="noticeRed">*</span></label>
            <div class="col-right-1 righttext" runat="server" id="dvAccounting" style="display:none;" >
                <asp:RadioButtonList ID="rdoAccounting" runat="server" RepeatDirection="Horizontal" CssClass="righttext" CellPadding="0" CellSpacing="0">
                    <asp:ListItem Text=" Proposal" Value="1"    TabIndex="15"></asp:ListItem>
                    <asp:ListItem Text=" Time" Value="2" TabIndex="16"></asp:ListItem>
                    <asp:ListItem Text=" Not Billable" Value="3" TabIndex="17"></asp:ListItem>
                </asp:RadioButtonList>
                <label class="col-left-1 lefttext" runat="server" id="lblProposal" style="display:none;" >Proposal:<span class="noticeRed">*</span></label>
                <select id="ddlProposal">
                    <option Value="">Please select...</option>
                </select>
                <asp:HiddenField ID="hid_Proposal" runat="server" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-1 lefttext">Estimation:</label>
            <div class="col-right-1 righttext">
                <span class="rightItem">
                    <asp:CheckBox ID="chkEN" runat="server" Text=" Needed" TabIndex="17" />
                </span>
                <label style="display: inline-block; margin-left: 15px;font-weight: bold;" runat="server" id="lblShare">Add to Knowledge Share: </label>
                <div   style="display: inline-block" id="dvShare" runat="server">
                  <asp:CheckBox runat="server" ID="rdoShareKnowlege" Text=" Yes" TabIndex="18" />
                </div>
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-1 lefttext" style="display: none" runat="server" id="lblIsInternal">Internal: </label>
            <div class="col-right-1  righttext" style="display: none" id="dvIsInteral" runat="server">
                <asp:CheckBox runat="server" ID="rdoIsInternal" Text=" Yes" Checked="true" TabIndex="18" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-1 lefttext" runat="server" id="lblSource" style="display: none">Source:</label>
            <div class="col-right-source righttext" runat="server" id="dvSource" style="display: none">
                <asp:DropDownList runat="server" ID="ddlSource" CssClass="selectw3" TabIndex="19"></asp:DropDownList>
            </div>
            <label class="col-left-1 lefttext" style="display: none; margin-left: 20px; width: 51px; padding: 3px 5px 0 0" runat="server" id="lblClientUsers">Client: </label>
            <div class="col-right-source righttext" runat="server" id="Div1">
                <select id="ddlClientUsers" style="display: none;" class="selectw3" tabindex="20" runat="server">
                    <option value="">Please select...</option>
                </select>
            </div>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                   <div class="form-group">
                     <label class="col-left-1 lefttext" runat="server" id="lblRes" style="display: none">Responsible User:</label>
                     <div class="col-right-source righttext" runat="server" id="dvRes" style="display: none">
                         <asp:DropDownList runat="server" ID="ddlRes" CssClass="selectw3" TabIndex="20" AutoPostBack="True"></asp:DropDownList>
                     </div>
                   </div>
             </ContentTemplate>
        </asp:UpdatePanel>
       

        <div id="dvAssignUsers"></div>
        <div class="buttonBox1">
            <img src="/Images/loading.gif" alt="Loading..." title="Loading..." class="loading" style="display: none;" />
            <asp:HiddenField ID="hidSaveType" runat="server" />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="saveBtn1 mainbutton" OnClick="btnSave_Click"
                OnClientClick=" return beforeSubmit2(this,1);" TabIndex="18" />
            <asp:Button ID="btnSubmitAndNew" runat="server" Text="Submit &amp; New" CssClass="saveBtn1 mainbutton"
                OnClientClick=" return beforeSubmit2(this,3);" OnClick="btnSave_Click" TabIndex="19" />
            <input id="btnClear" type="button" value="Clear" class="cancelBtn1 mainbutton" onclick="clearFields();" runat="server" tabindex="20" />
            <asp:HiddenField runat="server" ID="hdTicketUsers" />
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick=" return beforeSubmit(this,0);" CssClass="saveHiddenBtn" />
        </div>
    </div>
    <script type="text/javascript">
        var oldForm = null;
        var projectInfo = null;
        var message = {
            required: "This filed is required!",
            wordCountLimit: "Title should less than 200 character."
        };

        function getUploaderPrefix() {
            return "<%=UserInfo.UserID%>_";
        }

        var uploader;

        var _ajaxEvent = 0;
        var $hidSaveType = $("#<%=hidSaveType.ClientID%>");

        function beforeSubmit2(sender, saveType) {
            $hidSaveType.val(saveType);
            beforeSubmit(document.getElementById("<%=btnSave.ClientID%>"), saveType);
            return false;
        }

        function beforeSubmit(sender, saveType) {
            if (saveType == 0) return true;
            else {
                var $sender = $(sender);

                if ($("form").valid()) {
                    $('.buttonBox1').attr('validated', '1');
                    setSelectedUserToHdField();
                    _ajaxEvent = setTimeout(function () {
                        $sender.hide();
                        $sender.siblings(".loading").show();
                    }, 10);

                    var uploaderStatus = uploader.getStats();
                    var files = uploader.getFiles();

                    if (uploader.getStats().queueNum > 0 && uploader.state == "ready") {
                        $sender.data("clicked", true);
                        uploader.upload();
                        return false;
                    }
                    else
                        $sender.click();

                    if (uploaderStatus.successNum < 1 && uploaderStatus.uploadFailNum > 0) {
                        completed();
                        return false;
                    }
                    return true;
                };
                return false;
            }
        };

        $(function () {
            initUploadControls();
            projectInfo = eval("<%=jsonProjectInfo%>");
            checkIfCanAdd($("#" + "<%=ddlProject.ClientID%>").val(), "#" + "<%=ddlProject.ClientID%>");
            var $emergency = $("#<%=rdoPriority.ClientID%>").find("input:radio:last");
            var $tdToTip = $emergency.closest("td");
            $('<a data-toggle="popover" class="info" title="Emergency" href="###">&nbsp;</a>').popover({
                container: "body",
                placement: "right",
                content: "<span class=\'noticeRed\'>**The Emergency Support fee will be based on 1.5 times the regular rate. </span>",
                trigger: "hover click",
                html: true
            }).appendTo($tdToTip);

            $("#<%= rdoIsInternal.ClientID%>").on("click", function () {
                if ($(this).prop("checked") === true) {
                    <% if (CurRole != "Sales")
                       {  %>
                    //是PM或Client
                    $("#<%= ddlSource.ClientID%>").val("PM");
                    <% }
                       else
                       { %>
                    $("#<%= ddlSource.ClientID%>").val("Sales");
                    <%}%>

                    $("#<%=ddlClientUsers.ClientID%>").hide();
                    $("#<%=lblClientUsers.ClientID%>").hide();
                }
                else {
                    $("#<%= ddlSource.ClientID%>").val("CLIENT");
                    loadClientUsers();
                    $("#<%=ddlClientUsers.ClientID%>").show();
                    $("#<%=lblClientUsers.ClientID%>").show();
                }
            });

            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });

            oldForm = $('form').serialize();
            $('.buttonBox1').on('click', function () { $('.buttonBox1').attr('clicked', '1'); });
            $("#" + "<%=ddlProject.ClientID%>").on("change", function () {
                checkIfCanAdd($(this).val(), this);
                $("#" + "<%=chkEN.ClientID%>").prop("checked", $(this).val() && projectInfo[$(this).val()][0] == "True");
            });

            var project = +($("#<%=ddlProject.ClientID%>").val());
            if (project > 0) {
                $("#<%=txtTitle.ClientID%>").focus();
                fnDisplayUsers();
            } else {
                $("#<%=ddlProject.ClientID%>").focus();
            }

            //加载DropdownlistProposal默认项
           loadDropdownlistProposal();


        });

        $(window).on('beforeunload', function () {
            var isSubmit = $('.buttonBox1').attr('clicked') != '1';
            var hasChangeForm = $('form').serialize() != oldForm;
            var isValidated = $('.buttonBox1').attr('validated') == '1';
            if ((hasChangeForm && isSubmit) || (hasChangeForm && !isValidated)) {
                return '';
            }
        });

        function check() {
            var selectedProject = "#<%=ddlProject.ClientID%>";
            checkIfCanAdd($(selectedProject).val(), selectedProject);
            $("#" + "<%=chkEN.ClientID%>").prop("checked", $(selectedProject).val() && projectInfo[$(selectedProject).val()][0] == "True");
        }
        function clearFields() {
            document.getElementById("<%= ddlProject.ClientID%>").selectedIndex = 0;
            document.getElementById("<%= txtTitle.ClientID%>").value = "";
            document.getElementById("<%= txtUrl.ClientID%>").value = "";
            document.getElementById("<%=  txtDesc.ClientID%>").value = "";
            document.getElementById("<%= chkEN.ClientID%>").checked = false;
            $("#<%=rdoType.ClientID%>_0").prop("checked", true);
            $("#<%=rdoPriority.ClientID%>_1").prop("checked", true);

            $("input[type=\"file\"").each(function (index, item) {
                if (/MSIE/.test(navigator.userAgent)) {
                    $(item).replaceWith($(this).clone(true));
                } else {
                    $(item).val('');
                }
            });
        }

        function loadDropdownlistProposal() {
            var selectedProject = $("#<%=ddlProject.ClientID%>").val();
            if (selectedProject != "") {

                $.ajax({
                    url: '/do/DoGetddlProposal.ashx',
                    data: { projectID: selectedProject },
                    success: function(data) {
                        var data = $.parseJSON(data);
                        if (data.length > 0) {
                            $("#ddlProposal option:not(:first)").remove();
                            var optionsHTML = '';
                            for (var i = 0; i < data.length; i++) {
                                optionsHTML += '<option value="' + data[i].value + '">' + data[i].name + '</option>';
                            }
                            $("#ddlProposal").append(optionsHTML);
                        } else {
                            $("#ddlProposal option:not(:first)").remove();
                        }
                    }
                });
            };
        };

        function ChangeProject() {
            loadDropdownlistProposal();
        }

        function loadClientUsers() {
            var selectedProject = $("#<%=ddlProject.ClientID%>").val();
            if (selectedProject != "") {
                $.ajax({
                    url: '/do/DoGetProjectClientUsers.ashx',
                    data: { projectID: selectedProject, r: Math.random() },
                    success: function (data) {
                        var data = $.parseJSON(data);
                        if (data.length > 0) {
                            $("#<%=ddlClientUsers.ClientID%> option:not(:first)").remove();
                            var optionsHTML = "";
                            for (var j = 0; j < data.length; j++) {
                                if (data.length === 1) {
                                    optionsHTML += "<option value=" + data[j].value + " selected>" + data[j].name + "</option>";
                                }
                                else {
                                    optionsHTML += "<option value=" + data[j].value + ">" + data[j].name + "</option>";
                                }
                            }
                            $("#<%=ddlClientUsers.ClientID%>").append(optionsHTML);
                        }
                        else {
                            $("#<%=ddlClientUsers.ClientID%> option:not(:first)").remove();
                        }
                    }
                });
            }
        }

        function initUploadControls() {
            uploader = SunnetWebUploader.CreateWebUploader({
                fileNumLimit: 5,
                pick: { id: "#picker", multiple: true },
                container: "#thelist",
                uploadbutton: "#ctlBtn",
                submitbutton: $("#<%=btnSave.ClientID%>"),
                targetField: "#<%=hidUploadFile.ClientID%>"
            });
        }

        function checkIfCanAdd(projectId, object) {
            if (projectId && projectInfo[projectId][1] != "True") {
                ShowMessage("This project is closed, if you need to submit new tickets for this project, please contact us at team@sunnet.us.", 2);
                $(object).prop('selectedIndex', 0);
            }
            else {
                $("div[class='alert alert-danger fade hide in']").hide();
            }
        }
    </script>
    <script type="text/template" id="assignUserTemplate">
        <div class="sepline2"></div>

        <div class="assignuserTitle">Assign Users to Ticket:</div>
        {% for ( var i = 0; i < this.length ; i++) { %}
        <div class="form-group">
            <label class="col-left-owassignuser lefttext">{% this[i].Role %}:</label>
            <div class="righttext" style="width: 648px;">
                <ul class="assignUser" id="ulUS" runat="server">
                    {% for ( var j=0; j< this[i].Users.length; j++) { %}
                    <li title="{% this[i].Users[j].UserName %}">{% if (this[i].Users[j].Selected){ %}
                        <input type="checkbox" id="chk_{% this[i].Users[j].Id %}" name="assignuser" data-role="{% this[i].Users[j].role %}" value="{% this[i].Users[j].Id %}" checked="checked" />
                        {% } else { %}
                         <input type="checkbox" id="chk_{% this[i].Users[j].Id %}" name="assignuser" data-role="{% this[i].Users[j].role %}" value="{% this[i].Users[j].Id %}" />
                        {% } %}
                        <label for="chk_{% this[i].Users[j].Id %}">{% this[i].Users[j].UserName %}</label>
                    </li>
                    {% } %}
                </ul>
            </div>
        </div>
        {% } %}  
     
    </script>
    <script type="text/javascript">
        //[{role:"Dev",users:{role:"",id:"",name:""}},...]
        $("#<%=ddlProject.ClientID%>").on("change", function () {
            fnDisplayUsers();
            ChangeProject();
        });
        function fnDisplayUsers() {

            if ($("#<%=ddlProject.ClientID%>").val() !== "") {
                $("#dvAssignUsers").show();
                $.getJSON("/do/dogetprojectUsers.ashx", {
                    pid: $("#<%=ddlProject.ClientID%>").val(),
                }, function (data) {
                    var html = TemplateEngine(GetTemplateHtml("assignUserTemplate"), data);
                    $("#dvAssignUsers").empty();
                    <% if (CurRole != "Sales")
                       {%>
                    $("#dvAssignUsers").append(html);
                    <%}%>

                });
                if ($("#<%=ddlClientUsers.ClientID%>").css("display") != "none") {
                    loadClientUsers();
                }
            }
        }

        function setSelectedUserToHdField() {
            var selectedUsers = "";
            $("#dvAssignUsers").find("input[type=\"checkbox\"]:checked")
                .each(function (index, item) {
                    selectedUsers += $(item).val() + "|" + $(item).attr("data-role") + ",";
                });
            $("#<%=hdTicketUsers.ClientID%>").val(selectedUsers);
        }

        //显示跟隐藏Proposal的下拉框样式跟数据
        $(function () {
            var iSShow = ("<%=CurRole==RolesEnum.PM.ToString()||CurRole==RolesEnum.ADMIN.ToString()||CurRole==RolesEnum.Sales.ToString()%>");
            if (iSShow == "True")
                $("#ddlProposal").addClass("righttext required");
            else
                $("#ddlProposal").addClass("righttext");
            $("#ddlProposal").change(function() {
                $("#<%=hid_Proposal.ClientID%>").val($("#ddlProposal").val());
            });
            $("#<%=rdoAccounting.ClientID%>").find("input:radio").each(function () {
                $(this).click(function () {
                    if ($(this).attr("value") == '1') {
                        $("#<%=hid_Proposal.ClientID%>").val($("#ddlProposal").val());
                        $("#ddlProposal").show();
                        $("#ddlProposal").addClass("required");
                    }
                    else {
                        $("#<%=hid_Proposal.ClientID%>").val("");
                        $("#ddlProposal").removeClass("required");
                        $("#ddlProposal").hide();
                    }
                });
            });
        });
    </script>
</asp:Content>
