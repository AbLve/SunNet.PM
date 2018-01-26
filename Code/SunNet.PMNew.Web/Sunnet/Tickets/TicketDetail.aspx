<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketDetail.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.TicketDetail" %>

<%@ Register Src="../../UserControls/FeedBacksList.ascx" TagName="FeedBacksList"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/RelationTicketsList.ascx" TagName="RelationTicketsList"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControls/TicketUsers.ascx" TagName="TicketUsers" TagPrefix="uc3" %>
<%@ Register Src="../../UserControls/AddTicket.ascx" TagName="AddTicket" TagPrefix="uc4" %>
<%@ Register Src="../../UserControls/TaskList.ascx" TagName="TaskList" TagPrefix="uc5" %>
<%@ Register Src="../../UserControls/TicketBaseInfo.ascx" TagName="TicketBaseInfo"
    TagPrefix="uc6" %>
<%@ Register Src="../../UserControls/HistoryList.ascx" TagName="HistoryList" TagPrefix="uc7" %>
<%@ Register Src="../../UserControls/WorkFlow.ascx" TagName="WorkFlow" TagPrefix="uc8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ticket Detail</title>
    <link href="/Styles/openwindow.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/form.css" rel="stylesheet" type="text/css" />

    <script src="/do/js.ashx" type="text/javascript"></script>

    <style type="text/css">
        a:link {
            text-decoration: none;
        }

        .owmainBox1 {
            border: 1px solid #81bae8;
            background-color: #eff5fb;
            padding: 10px;
            overflow: auto;
            margin-bottom: 5px;
        }

        .selecteitems {
            padding-left: 0px;
            float: left;
            width: 100%;
            margin: 0px;
        }

        .selecteitems2col {
            width: 260px;
        }

        .selecteitems1col {
            width: 130px;
        }

            .selecteitems1col li {
                /*margin-left:1px;*/
            }

        #sunnetusertitle li {
            font-weight: bolder;
            background-color: #bad8f0;
        }

        .selecteitems li {
            width: 118px;
            height: 16px;
            margin-right: 5px;
            padding-top: 2px;
            padding-left: 5px;
            margin-bottom: 5px;
            cursor: pointer;
            float: left;
            list-style: none;
            border: solid 1px #BAD8F0;
            overflow: hidden;
        }

            .selecteitems li.selected {
                background: url('/Icons/29.gif') no-repeat right center;
            }

            .selecteitems li.plus {
                background: url('/Images/plus.png') no-repeat right center;
            }

            .selecteitems li.minus {
                background: url('/Images/minus.png') no-repeat right center;
            }
    </style>

    <script type="text/javascript">
        // return value
        var ReturnValueFromModalDialog = -1;
        var ISModalPage = false;
        var IsConfrim = false;
        var GlableStatus = -1;

        function FormatUrl(url) {
            if (url.indexOf("?") < 0) {
                url = url + "?";
            }
            else {
                url = url + "&";
            }

            url = url + "r=" + Math.random();
            return url;
        }
        function ShowIFrame(url, width, height, isModal, title) {
            url = FormatUrl(url);

            var windowStyle = "";
            windowStyle += "dialogWidth=";
            windowStyle += width.toString();
            windowStyle += "px;";

            windowStyle += "dialogHeight=";
            windowStyle += height.toString();
            windowStyle += "px;";
            windowStyle += "dialogLeft:" + ((window.screen.width - width) / 2) + "px;";
            windowStyle += "dialogTop:" + ((window.screen.height - height) / 2) + "px;";
            windowStyle += "center=1;help=no;status=no;scroll=auto;resizable=yes";
            //window.open(url,windowStyle);
            return window.showModalDialog(url, window, windowStyle);
        }
        function CreateCategories(obj) {
            alert("do something here;");
            var _this = jQuery("#" + obj.id).parent();
            _this.css("background-color", "red;");
            return false;
        }
        function UpdateTicketsCount(obj) {
            alert("do something here;");
            var _this = jQuery("#" + obj.id).parent();
            _this.css("background-color", "red;");
            var _ticketMenu = jQuery("#" + obj.id + "_second");
            _ticketMenu.children("div").children("a").each(function () {
                var _currentTypeTickets = jQuery(this);
                _currentTypeTickets.html(_currentTypeTickets.html() + "(" + _currentTypeTickets.index() + ")");
            });
            return false;
        }
        function DefaultFunction(leftMenuLi) {
            jQuery("#" + leftMenuLi.id + "_second").slideToggle(300);
            return false;
        }
        function SetOrders(hidOrderByID, hidOrderDirectioinID, btnSearchID, orderby) {
            var hidOrderby = jQuery("#" + hidOrderByID);
            var hidOrderDirection = jQuery("#" + hidOrderDirectioinID);

            hidOrderby.val(orderby);
            if (hidOrderDirection.val() == undefined || hidOrderDirection.val() == "" | hidOrderDirection.val() == "DESC") {
                hidOrderDirection.val("ASC");
            }
            else {
                hidOrderDirection.val("DESC");
            }
            document.getElementById(btnSearchID).click();
        }
        // page loaded event
        jQuery(function () {
            $("#closePopWindow").click(function () {
                // ShowMsgCommon.js
                CloseCurrent();
                return false;
            });
            // left menu
            jQuery("#leftMenu>div").hide();
            var _selected = jQuery("#leftMenu>li.currentleft:first");
            _selected.siblings("#" + _selected.children("a").attr("id") + "_second").show();

            // reset order
            var _orderby = jQuery("input[id*='hidOrderBy']");
            var _orderbyDirectin = jQuery("input[id*='hidOrderDirection']");
            var _btnsearch = jQuery("input[id*='iBtnSearch']");
            jQuery("th[orderby]").css("cursor", "pointer").click(function () {
                SetOrders(_orderby.attr("id"), _orderbyDirectin.attr("id"), _btnsearch.attr("id"), jQuery(this).attr("orderby"));
            });
            var _direImg = _orderbyDirectin.val() + ".gif";
            var _direImgHtml = "<img src='/images/" + _direImg + "' alt='order'/>";
            jQuery("th[orderby*='" + _orderby.val() + "']").append(_direImgHtml);
        });

        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null)
                return unescape(r[2]);
            return null;
        }


        function OpenAddFeedBackDialog() {
            var result = ShowIFrame("/Sunnet/Tickets/AddFeedBacks.aspx?tid=" + getUrlParam('tid'), 580, 350, true, "Add FeedBack");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenViewTicketDialog(selected) {
            var result = ShowIFrame("/Sunnet/Tickets/ViewRelatedTicket.aspx?tid=" + selected + "&is0hsisdse=54156", 880, 620, true, "View Ticket");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenAddTaskDialog() {
            var result = ShowIFrame("/Sunnet/Tickets/AddTask.aspx?tid=" + getUrlParam('tid'), 620, 270, true, "Add Task");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenTaskDialog(taskid) {
            var result = ShowIFrame("/Sunnet/Tickets/AddTask.aspx?taskid=" + taskid, 620, 310, true, "Task Detail");
            if (result == 0) {
                window.location.reload();
            }
        }
        function UpdateScDate() {
            var result = ShowIFrame("/Sunnet/Tickets/UpdateDate.aspx?tid=" + getUrlParam('tid'), 550, 310, true, "Update Schedule Date");
            if (result == 0) {
                window.location.reload();
            }
        }
        function LookEsDetail(type) {
            var result = ShowIFrame("/Sunnet/Tickets/TicketEsDetail.aspx?tid=" + getUrlParam('tid') + "&isFinal=" + type, 900, 350, true, "View Es Detail");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenAssignUserModuleDialog() {
            var result = ShowIFrame("/Sunnet/Tickets/AssignUsers.aspx?tid=" + getUrlParam('tid'), 480, 430, true, "Assign User");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenTicketTsTimeModuleDialog() {
            var result = ShowIFrame("/Sunnet/Tickets/TicketTsTime.aspx?tid=" + getUrlParam('tid'), 900, 350, true, "Ticket Es Time ");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenAssignUserTsModuleDialog() {
            var result = ShowIFrame("/Sunnet/Tickets/AssignUserTs.aspx?tid=" + getUrlParam('tid'), 530, 190, true, "Assign User");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenReplyFeedBackDialog(fid) {
            var result = ShowIFrame("/Sunnet/Tickets/AddFeedBacks.aspx?feedbackId=" + fid + "&rtype=r" + "&tid=" + getUrlParam('tid'), 580, 430, true, "Reply FeedBack");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenAddRelaionDialog() {
            var result = ShowIFrame("/Sunnet/Tickets/AddRelation.aspx?tid=" + getUrlParam('tid'), 620, 560, true, "Add Relation");
            if (result == 0) {
                window.location.reload();
            }
        }

        function OpenConvertReasonDialog(uType) {
            var result = ShowIFrame("/Sunnet/Tickets/ConvertReason.aspx?tid=" + getUrlParam('tid') + "&uType=" + uType, 530, 490, true, "Assign User");
            if (result == 0) {
                window.location.reload();
            }
        }

        function delRow(t) {
            var vTr = t.parent("td").parent("tr");
            vTr.remove();
        }

        function deleteFdTR(s) {
            var fid = s;

            delRow($("#" + s));

            $.ajax({

                type: "post",

                url: "/Do/DoRemoveFeedBackHandler.ashx?r=" + Math.random(),

                data: {

                    fid: fid
                },
                success: function (result) {
                    MessageBox.Alert3(null, result, function () {
                        window.location.href = window.location.href;
                    });
                }
            });
        }

        function deleteRlationTR(s) {
            var rid = s;
            delRow($("#" + s));
            var tid = getUrlParam('tid');
            $.ajax({
                type: "post",
                data: {
                    tid: tid,
                    rid: rid
                },
                url: "/Do/DoRemoveRelationTicketsHandler.ashx?r=" + Math.random(),
                success: function (result) {
                    MessageBox.Alert3(null, result, function () {
                        window.location.href = window.location.href;
                    });
                }
            });
        }

        function updateTaskStatus(taskid) {
            $.ajax({
                type: "post",
                data: {
                    taskid: taskid
                },
                url: "/Do/DoUpdateTaskStatusHandler.ashx?r=" + Math.random(),
                success: function (result) {
                    MessageBox.Alert3(null, result, function () {
                        window.location.href = window.location.href;
                    });
                }
            });
        }
        // when need to Secondary confirm

        function updateStatus() {
            var tid = getUrlParam('tid');
            $.ajax({
                type: "post",
                data: {
                    tid: tid,
                    statusValue: GlableStatus
                },
                url: "/Do/DoUpdateTicketStatus.ashx?r=" + Math.random(),
                success: function (result) {
                    if (result == "same") {
                        MessageBox.Alert3(null, "Status Are Same With Before!", function () {
                            window.location.reload();
                        });
                    } else if (result == "696") {
                        MessageBox.Alert3(null, "Please assign user before you change status.", null);
                    }
                    else {
                        MessageBox.Alert3(null, result, function () {
                            window.location.reload();
                        });
                    }
                }
            });
        }

        function ConfirmChangeStatus(e) {
            if (e == true) {
                updateStatus();
            }
            else {
                return false;
            }
        }

        //如果是PMReview就先去专门验证一次，验证成功了再提交


        //else if (result == "-2") {
        //MessageBox.Alert3(null, "Are you sure that this ticket does not need a quote approval?", null);
        //}

        function updateStatusConfirm(statusResult, isSureCofirm) {
            GlableStatus = statusResult;
            if (isSureCofirm) {
                if (IsConfrim) {
                    IsConfrim = false;
                    return false;
                }
                MessageBox.Confirm3(null, "Confirm to change Status?", '', '', ConfirmChangeStatus);
            }

            else {
                if (statusResult !== "pReview" && statusResult !== "3") {
                    updateStatus();
                }
                else {
                    pmReview();
                }
            }
        }

        function pmReview() {
            var tid = getUrlParam('tid');
            $.get("/Do/DoUpdateTicketStatus.ashx?r=" + Math.random(),
                {
                    tid: tid,
                    statusValue: "pmReviewMaintenanceValidate"
                }, function (data) {
                    if (data === "-1") {
                        var message = "Are you sure that this ticket does not need a quote approval?";
                        MessageBox.Confirm3(null, message, 'button_yes.gif', 'button_no.gif', function (e) {
                            if (e === true) {
                                updateStatus();
                            }
                        });
                    }
                    else {
                        updateStatus();
                    }
                });
        }


        function NotBugConfirm() {
            if (IsConfrim) {
                IsConfrim = false;
                return false;
            }
            MessageBox.Confirm3(null, "Not Bug?", '', '', ConfirmNotBug);
        }

        function ConfirmNotBug(e) {
            if (e == true) {
                HoursIsCallBack = true;
                GlableStatus = "notBug";
                updateStatus();
                return false;
            }
            else {
                return false;
            }
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="mainTop" style="background-image: url('/images/mainbg.jpg'); min-width: 1002px;">
            <div class="mainTop_left">
                <asp:Literal ID="ltLogo" runat="server"></asp:Literal>
            </div>
            <div class="mainTop_right">
                <span class="mainTop_rightUser">Welcome&nbsp;&nbsp; <strong>
                    <%=string.Format("{0}, {1}", UserInfo.LastName, UserInfo.FirstName)%></strong></span>
                <span class="<%=UserInfo.Role==SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT?"":"hide" %>">
                    <a href="/Sunnet/Clients/Faqs.aspx">FAQ</a>|<a href="/Sunnet/Clients/Survey.aspx">Survey</a>|<a
                        href="/Sunnet/Clients/ContactUs.aspx">Contact us</a></span>
            </div>
            <div style="float: right; margin-right: 15px;">
                <asp:Literal ID="ltSunnetLogo" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="owBoxtwo">
            <div>
                <div class="owTopone">
                    <div class="owTopone_left2" style="font-size: 14px; font-weight: bold; color: #000000; width: 80%; white-space: nowrap; word-break: keep-all; overflow: hidden; height: 18px; text-overflow: ellipsis;">
                        Ticket
                    <asp:Literal ID="lilTicketTitle" runat="server"></asp:Literal>
                    </div>
                    <div class="owTopone_right">
                        <a href="#" id="closePopWindow">
                            <img src="/icons/15.gif" border="0" align="absmiddle" />
                            Close </a>
                    </div>
                </div>
            </div>
            <div runat="server" id="topMenu" visible="false" class="owtopMenu">
                <div class="owtopMenu_left">
                    <ul>
                        <li runat="server" id="line3" class="sepline">
                            <img src="/images/owmenu_sep.gif" />
                        </li>
                        <li id="ng" runat="server"><a href="#" runat="server" id="ngLink" onclick="NotBugConfirm(); return false;">
                            <img src="/icons/14.gif" />
                            Not a bug </a></li>
                        <li class="sepline" id="line4" runat="server">
                            <img src="/images/owmenu_sep.gif" />
                        </li>
                        <li id="cr" runat="server"><a href="#" onclick="OpenConvertReasonDialog('cRequest'); return false;">
                            <img src="/icons/22.gif" />Convert to Request </a></li>
                        <li class="sepline" runat="server" id="line1">
                            <img src="/images/owmenu_sep.gif" />
                        </li>
                        <li runat="server" id="pr"><a href="#" onclick="updateStatusConfirm('pReview', false);return false;">
                            <img src="/icons/20.gif" />
                            PM Reviewed </a></li>
                        <li class="sepline" runat="server" id="line2">
                            <img src="/images/owmenu_sep.gif" />
                        </li>
                    </ul>
                </div>
            </div>
            <div runat="server" id="divtopMeunFill" visible="false" class="owtopMenu">
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="owmainBox_left">
                        <div style="width: 195px;">
                        </div>
                        <ul class="owleftmenu" style="position: fixed;">
                            <li class="currentleft"><a href="#Basic">Basic Information</a> </li>
                            <li><a href="#Feedback">Feedback</a> </li>
                            <li><a href="#Users">Users</a> </li>
                            <li><a href="#Relations">Relations</a> </li>
                            <li><a href="#Tasks">Tasks</a> </li>
                            <li><a href="#Change History">Change History</a> </li>
                            <li><a href="#Workflow">Workflow</a> </li>
                        </ul>
                    </td>
                    <td class="owmainBox_right">
                        <div class="owmainrightBoxtwo">
                            <div>
                                <div class="owmainactionBox">
                                    <div class="tickettop_left">
                                        Description
                                    </div>
                                    <div class="tickettop_right" style="display: none;">
                                        <img src="/icons/12.gif" />
                                        Add ticket to category
                                    <div class="categoryList" id="category" onmouseover="MM_showHideLayers('category', '', 'show')"
                                        onmouseout="MM_showHideLayers('category', '', 'hide')">
                                        <ul class="category">
                                            <li><a href="#">09-27-2012</a> </li>
                                            <li><a href="#">09-27-2012</a> </li>
                                            <li><a href="#">09-27-2012 </a></li>
                                            <li><a href="#">09-27-2012</a> </li>
                                            <li><a href="#">New Category</a> </li>
                                        </ul>
                                    </div>
                                    </div>
                                </div>
                                <uc6:TicketBaseInfo ID="TicketBaseInfo1" runat="server" />
                                <uc4:AddTicket ID="AddTicket1" runat="server" />
                            </div>
                            <div>
                                <div class="owmainactionBox" runat="server">
                                    <div style="float: left; display: inline-block; width: 100px; font-size: 12px; font-weight: bold">
                                        <a name="Feedback">Feedback</a>
                                    </div>

                                </div>
                                <uc1:FeedBacksList ID="FeedBacksList1" runat="server" />
                            </div>
                            <div>
                                <div class="owToptwo">
                                    <div style="width: 300px; float: left;">
                                        <a name="Users">Users</a><asp:HiddenField ID="hidSelectedSunneters" runat="server" />
                                    </div>
                                    <div style="width: 100px; float: right; margin-right: 20px;" runat="server" id="divAssign">
                                        <input type="button" id="btnSaveSunnet" class="btnone" value=" Assign " />
                                    </div>
                                </div>

                                <div class="owmainBox1" style="display: block;">
                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="border: none;">
                                        <tr>
                                            <td>
                                                <ul id="sunnetusertitle" class="selecteitems">
                                                    <li>Dev</li>
                                                    <li>Dev</li>
                                                    <li>Leader</li>
                                                    <li>PM</li>
                                                    <li>Qa</li>
                                                    <li>Sales</li>
                                                </ul>
                                                <ul id="sunnetusersdev" class="selecteitems selecteitems2col">
                                                    <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.DEV)%>
                                                    <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.Contactor)%>
                                                </ul>
                                                <ul id="sunnetusersleader" class="selecteitems selecteitems1col">
                                                    <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.Leader)%>
                                                </ul>
                                                <ul id="sunnetuserspm" class="selecteitems selecteitems1col">
                                                    <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.PM)%>
                                                    <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.Supervisor)%>
                                                </ul>
                                                <ul id="sunnetusersqa" class="selecteitems selecteitems1col">
                                                    <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.QA) %>
                                                </ul>
                                                <ul id="sunnetuserssales" class="selecteitems selecteitems1col">
                                                    <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.Sales)%>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="btnBoxone" style="text-align: right; clear: both;">
                                    </div>
                                </div>

                            </div>
                            <div>
                                <div class="owmainactionBox">
                                    <div style="float: left; display: inline-block; width: 100px; font-size: 12px; font-weight: bold">
                                        <a name="Relations">Related tickets</a>
                                    </div>
                                    <div style="float: left;" runat="server" id="divAddRelation">
                                        <a href="###" onclick="OpenAddRelaionDialog()">
                                            <img src="/icons/28.gif" align="absmiddle" />
                                            Add Associated Tickets for Requirement Change </a>
                                    </div>
                                </div>
                                <uc2:RelationTicketsList ID="RelationTicketsList1" runat="server" />
                            </div>
                            <div>
                                <div class="owmainactionBox">
                                    <div style="float: left; display: inline-block; width: 100px; font-size: 12px; font-weight: bold">
                                        <a name="Tasks">Tasks</a>
                                    </div>
                                    <div runat="server" id="divTask" style="float: left;">
                                        <a href="javascript:void(0);" onclick="OpenAddTaskDialog()">
                                            <img src="/icons/28.gif" align="absmiddle" alt="add task" />Add Task </a>
                                    </div>
                                </div>
                                <uc5:TaskList ID="TaskList1" runat="server" />
                            </div>
                            <div>
                                <div class="owmainactionBox">
                                    <a name="Change History">Change History</a>
                                </div>
                                <uc7:HistoryList ID="HistoryList1" runat="server" />
                            </div>
                            <div>
                                <div class="owmainactionBox">
                                    <a style='cursor: pointer;' onclick="toggleDiv('dvWorkFlow');return false"><span id='wfVisibleText' style='font-size: 14px; color: green;'>+</span></a>
                                    <a name="Workflow">Workflow</a>
                                </div>
                                <div id="dvWorkFlow">
                                    <uc8:WorkFlow ID="WorkFlow1" runat="server" />
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <script type="text/javascript">
        function AssignSunnet() {
            var hidSelectedSunneters = jQuery("#<%=hidSelectedSunneters.ClientID %>");
            var btnSaveSunnet = jQuery("#btnSaveSunnet");
            AssignUser(btnSaveSunnet, hidSelectedSunneters, "selecteitems");
        }

        var SunnetUsersjQuery;
        function AssignUser(btnSave, hidSelected, classname) {
            var selectedUsers = jQuery.parseJSON(hidSelected.val());
            if (selectedUsers) {
                for (var i = 0; i < selectedUsers.length; i++) {
                    try {
                        jQuery("ul." + classname + ">li[userid='" + selectedUsers[i].id + "']").addClass("selected");
                    }
                    catch (e)
                    { }
                }
            }
            btnSave.click(function () {
                SunnetUsersjQuery = jQuery("ul." + classname + ">li[userid]");
                if (SunnetUsersjQuery.filter(".minus").length <= 0 && SunnetUsersjQuery.filter(".plus").length <= 0) {
                    ShowMessage("you have not made any changes ,there is no need to save!", 0, false, false);
                    return false;
                }
                else {
                    var _tempselectedclients = "";
                    SunnetUsersjQuery.filter(".plus").add(".selected").each(function () {
                        var _selectedItem = jQuery(this);
                        if (_selectedItem.hasClass("minus") == false) {

                            _tempselectedclients = _tempselectedclients + _selectedItem.attr("userid") + "|" + _selectedItem.attr("data-role");
                            _tempselectedclients = _tempselectedclients + ",";
                        }
                    });
                    if (_tempselectedclients.length > 0 || SunnetUsersjQuery.filter(".minus").length > 0) {
                        _tempselectedclients = _tempselectedclients.replace(/,$/, "")
                        assignUsers(_tempselectedclients);
                    }
                    else {
                        hidSelected.val("");
                    }

                }
            });
            jQuery("ul." + classname + ">li[userid]").click(function () {
                var _this = jQuery(this);
                if (_this.hasClass("selected")) {
                    if (_this.hasClass("minus")) {
                        _this.removeClass("minus");
                    }
                    else {
                        _this.addClass("minus");
                    }
                }
                else {
                    if (_this.hasClass("plus")) {
                        _this.removeClass("plus");
                    }
                    else {
                        _this.addClass("plus");
                    }
                }
            });
        }

        function assignUsers(selectedUser) {
            var btnAssignuser = jQuery("#btnSaveSunnet")
            btnAssignuser.hide();
            $.ajax({
                type: "post",
                url: "/Do/DoAssignUserHandler.ashx?r=" + Math.random(),
                data: {
                    checkboxList: selectedUser,
                    tid: getUrlParam('tid')
                },
                success: function (result) {

                    if (result == "User Assign Successful!") {
                        btnAssignuser.show();
                    }
                    ShowMessage(result, 0, true, false);

                }
            });
        }

        jQuery(function () {
            AssignSunnet();
            toggleDiv("dvWorkFlow");
            changeUsersLayout();
        });

        function changeUsersLayout() {
            var ulDev = $("#sunnetusersdev");
            var ulPM = $("#sunnetuserspm");
            var devNonVisibleLi = ulDev.find("li[style]");
            var pMNonVisibleLi = ulPM.find("li[style]");
            if (devNonVisibleLi.length && ulDev.find("li").length != devNonVisibleLi.length) {
                devNonVisibleLi.each(function (index, elem) {
                    $(elem).css("visibility", "").css("display", "none");
                });
            }
            if (pMNonVisibleLi.length && ulPM.find("li").length != pMNonVisibleLi.length) {
                pMNonVisibleLi.each(function (index, elem) {
                    $(elem).css("visibility", "").css("display", "none");
                });
            }
        }

        var IsHide = true;
        function toggleDiv(id) {
            $("#" + id).slideToggle("slow", function () {
                if (!IsHide) {
                    $("#wfVisibleText").text("--");
                    IsHide = true;
                } else {
                    IsHide = false;
                    $("#wfVisibleText").text("+");
                }
            });
        }
    </script>
</body>
</html>
