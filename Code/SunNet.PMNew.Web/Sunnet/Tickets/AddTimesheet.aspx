<%@ Page Title="Write Timesheet" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="AddTimesheet.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.AddTimesheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="/Styles/datagrid/icon.css" rel="stylesheet" type="text/css" />--%>
    <link href="/Styles/datagrid/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/datagrid/default/easyui.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .listthree td, .listthree th {
            border: none;
        }

        td > div {
            word-break: normal;
            word-wrap: break-word;
        }

        textarea {
            width: 330px;
            max-width: 330px;
            min-height: 330px;
            min-height: 60px;
            height: auto;
            word-spacing: normal;
            word-break: normal;
            word-wrap: break-word;
            overflow-y: auto;
        }

        td > div.datagrid-cell {
            /*max-height:30px;*/ /*overflow:scroll;*/
        }

        .listtopTitle_leftdate {
            color: Black;
        }

        .button {
            background: none repeat scroll 0 0 transparent;
            border: 1px solid transparent;
            padding-right: 5px;
            color: #444444;
            cursor: pointer;
            display: inline-block;
            font-size: 12px;
            height: 24px;
            outline: medium none;
            text-decoration: none;
        }

            .button span.space {
                background: none repeat scroll 0 0 transparent;
                display: inline-block;
                height: 16px;
                line-height: 16px;
                padding: 4px 0 4px 5px;
            }

            .button span.content {
                padding-left: 20px;
                display: inline-block;
                height: 16px;
                line-height: 16px;
            }

            .button:hover {
                border: 1px solid #7eabcd;
                background: url('/styles/datagrid/default/images/button_plain_hover.png') repeat-x left bottom;
                _padding: 0px 5px 0px 0px;
                -moz-border-radius: 3px;
                -webkit-border-radius: 3px;
                border-radius: 3px;
            }

        .btndelete {
            background: url("/styles/datagrid/icons/edit_remove.png") no-repeat scroll 0 0 transparent;
        }

        .btnsave {
            background: url("/styles/datagrid/icons/filesave.png") no-repeat scroll 0 0 transparent;
        }

        .btncancel {
            background: url("/styles/datagrid/icons/undo.png") no-repeat scroll 0 0 transparent;
        }

        .listrowtwo td {
            padding: 3px 2px 3px 2px;
        }
    </style>

    <script type="text/javascript">
        Date.prototype.format = function (format) {
            var o = {
                "M+": this.getMonth() + 1, //month 
                "d+": this.getDate(),    //day 
                "h+": this.getHours(),   //hour 
                "m+": this.getMinutes(), //minute 
                "s+": this.getSeconds(), //second 
                "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter 
                "S": this.getMilliseconds() //millisecond
            }
            if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length)); for (var k in o) if (new RegExp("(" + k + ")").test(format)) format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            return format;
        }
        function GetDateFromString(date) {
            var _thisday = new Date();
            _thisday.setFullYear(parseInt(date.substr(0, 4)));

            var _thisMonth = date.substr(5, 2);
            if (_thisMonth.indexOf("0") == 0) {
                _thisMonth = _thisMonth.substr(1, 1);
            }
            _thisday.setMonth(parseInt(_thisMonth - 1));

            var _thisDate = date.substr(8, 2);
            if (_thisDate.indexOf("0") == 0) {
                _thisDate = _thisDate.substr(1, 1);
            }
            _thisday.setDate(parseInt(_thisDate));
            return _thisday;
        }
    </script>

    <script type="text/javascript">
        jQuery.extend(jQuery.fn.datagrid.defaults.editors, {
            bools: {
                init: function (container, options) {
                    var input = $('<input type="checkbox" class="datagrid-editable-input">').appendTo(container);
                    return input;
                },
                getValue: function (target) {
                    if ($(target).attr("checked") == "checked") {
                        return true;
                    }
                    return false;
                },
                setValue: function (target, value) {
                    if (value == true || value == "true" || value == "True" || value == "1" || value == 1) {
                        $(target).attr("checked", "checked");
                    }
                    else {
                        $(target).removeAttr("checked");
                    }
                },
                resize: function (target, width) {
                    var input = $(target);
                    if ($.boxModel == true) {
                        input.width(width - (input.outerWidth() - input.width()));
                    } else {
                        input.width(width);
                    }
                }
            }
        });
        function ShowStatus(result, time) {
            var level = 0;
            if (result.Success == false) {
                level = 2;
            }
            CoverAndAlert(result.MessageContent, level, time);
            //            var color = result.Success ? "green" : "red";
            //            jQuery("#status").css("color", color).html("").html(result.MessageContent);
            //            setTimeout(function() { jQuery("#status").html(""); }, time);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Add Timesheet
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainrightBox">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="listtopTitle" style="max-width: 1330px;">
                                <div class="listtopTitle_leftdate">
                                    Timesheet on
                                    <%=SelectedDate.ToString("MM/dd/yyyy") %>&nbsp;&nbsp;&nbsp; Total Hours: <span id="totalhours"
                                        style="color: Red; font-weight: 600;">0</span>
                                </div>
                                <div class="listtopTitle_leftdate">
                                    Import Category:&nbsp;<%--<asp:DropDownList Width="150" ID="ddlCategorys" AutoPostBack="true"
                                        runat="server" OnSelectedIndexChanged="ddlCategorys_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList Width="150" ID="ddlCategorys" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="listtopTitle_leftdate" style="color: Red;" id="status">
                                </div>
                                <div class="listtopTitle_leftdate" style="float: right; min-width: 110px;">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btnone" OnClick="btnSubmit_Click" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="ticketlist" title="TimeSheet" toolbar="#toolbarstatus" pagination="false"
                                    idfield="TimeSheetID" rownumbers="true" fitcolumns="true" singleselect="true">
                                    <thead>
                                        <tr>
                                            <th field='ProjectTitle' width="8">Project
                                            </th>
                                            <th field='ProjectID' hidden="true">ProjectID
                                            </th>
                                            <th field='TicketTitle' width="10">Title
                                            </th>
                                            <th field='TicketID' hidden="true">TicketID
                                            </th>
                                            <th field='TimeSheetID' hidden="true">TimeSheetID
                                            </th>
                                            <th field='IsSubmitted' hidden="true">IsSubmitted
                                            </th>
                                            <th field='TicketCode' width="3">Code
                                            </th>
                                            <th field='TicketDescription' width="18">Description
                                            </th>
                                            <th field='IsMeeting' editor="{type:'bools'}" width="4">Meeting
                                            </th>
                                            <th field='WorkDetail' width="18" editor="{type:'textarea'}">Work detail
                                            </th>
                                            <th field="Hours" width="3" editor="{type:'text'}">Hours (H)
                                            </th>
                                            <th field='Percentage' width="5">PCT (%)
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                                <div id="toolbarstatus">
                                    <a href="javascript:void(0)" class="button " onclick="BtnDelete_Click();"><span class="space">
                                        <span class="content btndelete">Delete</span></span></a> <a href="javascript:void(0)"
                                            class="button" onclick="BtnSave_click();"><span class="space"><span class="content btnsave">Save</span></span></a> <a href="javascript:void(0)" class="button" onclick="javascript:jQuery('#ticketlist').edatagrid('cancelRow')">
                                                <span class="space"><span class="content btncancel">Cancel</span></span></a>
                                </div>
                                <br />
                                <div class="panel datagrid" id="formadd" style="width: auto;">
                                    <div class="panel-header" style="width: auto;">
                                        <div class="panel-title">
                                            Write Timesheet
                                        </div>
                                    </div>
                                    <div class="datagrid-wrap panel-body" style="width: auto;">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listthree">
                                            <tr class="listsubTitle">
                                                <th style="width: 15%">Project
                                                </th>
                                                <th style="width: 15%">Title
                                                </th>
                                                <th style="width: 10%">Code
                                                </th>
                                                <th style="width: 15%">Description
                                                </th>
                                                <th style="width: 5%; padding-left: 0; padding-right: 0;">Meeting
                                                </th>
                                                <th style="width: 20%">Work detail
                                                </th>
                                                <th style="width: 7%; padding-left: 0; padding-right: 0;">Hours(H)
                                                </th>
                                                <th style="width: 7%; padding-left: 0; padding-right: 0;">PCT(%)
                                                </th>
                                                <th style="width: 5%">Action &nbsp;
                                                </th>
                                            </tr>
                                            <tr class="listrowtwo">
                                                <td valign="top">
                                                    <asp:DropDownList ID="ddlProject" CssClass="input98p" runat="server" Style="width: 130px;">
                                                    </asp:DropDownList>
                                                </td>
                                                <td valign="top">
                                                    <select id="ddlTicketTitle" name="select2" class="input98p" style="width: 140px;">
                                                        <option value="0">Please Select</option>
                                                    </select>
                                                </td>
                                                <td valign="top">
                                                    <select id="ddlTicketCode" name="select2" class="input98p" style="width: 96px;">
                                                        <option value="0">Please Select</option>
                                                    </select>
                                                </td>
                                                <td valign="top">
                                                    <div style="max-width: 280px;">
                                                        <textarea id="ticketdesc" style="width: 100%; max-width: 210px; width: 210px; min-width: 210px;"
                                                            name="textarea" rows="5" readonly="readonly" class="input98p"></textarea>
                                                    </div>
                                                </td>
                                                <td align="center" valign="top">
                                                    <input id="ismeeting" type="checkbox" name="checkbox2" value="checkbox" />
                                                </td>
                                                <td valign="top">
                                                    <textarea id="addworkdetail" style='max-width: 210px; width: 210px; min-width: 210px;'
                                                        name="textarea2" rows="5" class="input98p"></textarea>
                                                </td>
                                                <td valign="top">
                                                    <input id="addhours" name="textfield" type="text" style="ime-mode: disabled; padding-left: 6px;" class="input40" />
                                                </td>
                                                <td valign="top">
                                                    <input id="addPercentage" name="textfield" readonly="readonly" type="text" style="ime-mode: disabled;"
                                                        class="input40" />
                                                </td>
                                                <td valign="top">
                                                    <a id="btntimesheet" title="Save item" href="###">
                                                        <img alt="Save" src="/icons/saveBtn1.gif"></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="btnBoxtwo">
            <input id="btnBack" name="Input2" type="button" class="btnone" value="Back" />
        </div>
    </div>

    <script type="text/javascript">
        // data operation
        var ddlCategory;
        var SelectedDate = '<%=SelectedDate.ToString("yyyy-MM-dd") %>';
        var SelectedCategory = 0;
        var GetDataUrl = '/Do/TimeSheet.ashx?type=GetTimeSheetsByDate&date=<%=SelectedDate.ToString("yyyy-MM-dd") %>&category=';
        var SaveDataUrl = '/Do/TimeSheet.ashx?type=add';
        var UpdateDataUrl = '/Do/TimeSheet.ashx?type=update';
        var DeleteDateUrl = '/Do/TimeSheet.ashx?type=delete';
        var DataTable;
        var CurrentTimeSheetsList;

        // add new timesheet
        var ddlTitle;
        var ddlCode;
        var btnAddNewTimesheet;
        var btnSubmit;
        // can edit ,decide by IsSubmitted
        var CanEdited;
        function GetDataUrlString() {
            var _this = ddlCategory.children("option:selected");
            return GetDataUrl + _this.val();
        }
        var ddlProject;
        var TicketsLoaded = {};
        function SetTickets(project, jsonList) {
            try {
                eval("TicketsLoaded.project" + project + "=" + jsonList + ";");
            } catch (e) {
            }
        }
        function GetTickets(project) {
            var result = eval("TicketsLoaded.project" + project);
            return result;
        }

        var WritedHours;
        var EditedHours;
        function UpdateWritedHours(hours) {
            hours = parseFloat(hours);
            WritedHours = parseFloat(WritedHours);
            WritedHours = WritedHours + hours;
        }
        function UpdateTotalHours() {
            var tc = 0;
            for (var i = 0; i < CurrentTimeSheetsList.length; i++) {
                tc = parseFloat(CurrentTimeSheetsList[i].Hours) + tc;
            }
            tc = parseFloat(tc.toFixed(2));
            jQuery("#totalhours").text(tc);
            WritedHours = tc;
        }
        function CheckHasWriteThisTicket(ticketID) {
            for (var i = 0; i < CurrentTimeSheetsList.length; i++) {
                if (CurrentTimeSheetsList[i].TicketID == ticketID) {
                    return true;
                }
            }
            return false;
        }
        function SetSubmitControlVisible() {
            if (CurrentTimeSheetsList.length > 0 && CurrentTimeSheetsList[0].IsSubmitted == true) {
                btnSubmit.remove();
                ddlCategory.attr("disabled", "disabled");
                ddlProject.attr("disabled", "disabled");
                jQuery("#formadd").remove();
                jQuery("#toolbarstatus").remove();
                ShowStatus({ Success: true, MessageContent: "The timesheets has submitted." }, 5000);
            }
        }
        function SetTicketsControls(ticketslist) {
            ddlTitle.html("");
            ddlCode.html("");
            if (ticketslist != undefined && ticketslist != null && ticketslist.length > 0) {
                jQuery.each(ticketslist, function (index, ticket) {
                    if (!CheckHasWriteThisTicket(ticket.ID)) {
                        ddlTitle.append("<option value='" + ticket.ID + "' desc='" + ticket.FullDescription + "' Percentage='" + ticket.Percentage + "'>" + ticket.Title + "</option>");
                        ddlCode.append("<option value='" + ticket.ID + "' desc='" + ticket.FullDescription + "' Percentage='" + ticket.Percentage + "'>" + ticket.TicketCode + "</option>");
                    }
                });
                if (ddlTitle.children().length == 0) {
                    SetDefaultValue();
                }
            }
            else {
                ddlTitle.append("<option value='0' desc=''>Please Select</option>");
                ddlCode.append("<option value='0' desc=''>Please Select</option>");
            }
            ddlTitle.change();
        }
        function SetDefaultValue() {
            ddlTitle.html("").append("<option value='0' desc='Please Slelect'>Please Slelect</option>");
            ddlCode.html("").append("<option value='0' desc='Please Slelect'>Please Slelect</option>");
        }
        function ClearFormControl() {
            ddlProject.children().removeAttr("selected").eq(0).attr("selected", "selected").change();
            jQuery("#formadd textarea").add("#formadd input[type='text']").val("");
        }
        function CheckOverLimitHours(hours) {
            if (hours <= 0) {
                ShowStatus({ Success: false, MessageContent: "Hours must bigger than 0,please rewrite." }, 5000);
                return true;
            }
            else {
                var existsHours = WritedHours;
                hours = parseFloat(hours);
                var totalHours = parseFloat(hours) + existsHours;

                if (totalHours > 24) {
                    ShowStatus({ Success: false, MessageContent: "Total hours bigger than 24,please rewrite." }, 5000);
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        function BtnDelete_Click() {
            var rows_selected = DataTable.edatagrid("getSelected");
            if (rows_selected == undefined || rows_selected == null) {
                ShowStatus({ Success: true, MessageContent: "Please select a row to delete." }, 5000);
                return;
            }
            else {
                jQuery('#ticketlist').edatagrid('destroyRow');
            }
        }
        function BtnSave_click() {
            var rows_selected = DataTable.edatagrid("getSelected");
            if (rows_selected == undefined || rows_selected == null) {
                ShowStatus({ Success: true, MessageContent: "There are no rows need to save." }, 5000);
                return;
            }
            else {
                jQuery('#ticketlist').edatagrid('saveRow');
            }
        }
        jQuery(function () {
            btnSubmit = jQuery("#<%=btnSubmit.ClientID %>");
            DataTable = jQuery("#ticketlist");
            ddlCategory = jQuery("#<%=ddlCategorys.ClientID %>");
            ddlTitle = jQuery("#ddlTicketTitle");
            ddlCode = jQuery("#ddlTicketCode");
            btnAddNewTimesheet = jQuery("#btntimesheet");
            ddlCategory.change(function () {
                DataTable.edatagrid({
                    url: GetDataUrlString()
                });
            });
            DataTable.edatagrid({
                autoRowHeight: true,
                url: GetDataUrlString(),
                saveUrl: SaveDataUrl,
                updateUrl: UpdateDataUrl,
                destroyUrl: DeleteDateUrl,
                onBeforeEdit: function (rowIndex, rowData) {
                    EditedHours = parseFloat(rowData.Hours);
                    WritedHours = WritedHours - EditedHours;
                    if (rowData.SheetDate.indexOf("Date") > 0) {
                        var _sheetDate = eval("new " + rowData.SheetDate.substr(1, rowData.SheetDate.length - 2));
                        _sheetDate = _sheetDate.format("yyyy-MM-dd");
                        rowData.SheetDate = _sheetDate;
                    }
                    if (rowData.IsSubmitted == true) {
                        ShowStatus({ Success: false, MessageContent: "Submitted timesheet can not be edited" }, 5000);
                        return false;
                    }
                },
                onBeforeSubmit: function (rowIndex, rowData) {
                    if (CheckOverLimitHours(rowData.Hours)) {
                        WritedHours = WritedHours + EditedHours;
                        rowData.Hours = EditedHours;
                        //DataTable.edatagrid("reload");
                        //DataTable.edatagrid("updateRow", rowIndex,rowData);
                        return false;
                    }
                    else {
                        return true;
                    }
                },
                onAfterEdit: function (rowIndex, rowData) {
                    var addedid = parseInt(rowData.TimeSheetID);
                    if (addedid <= 0) {
                        //DataTable.edatagrid("deleteRow", rowIndex);
                    }
                    ShowStatus(rowData, 4000);
                    CurrentTimeSheetsList = DataTable.edatagrid('getRows');
                    UpdateTotalHours();
                },
                onLoadSuccess: function (data) {
                    CurrentTimeSheetsList = DataTable.edatagrid('getRows');
                    UpdateTotalHours();
                    SetSubmitControlVisible();
                    DataTable.edatagrid("fixRowHeight");
                },
                onDestroySuccess: function (index, row, message) {
                    var deletedid = parseInt(row.TimeSheetID);
                    ShowStatus(message, 5000);
                    UpdateTotalHours();
                },
                onCancelEdit: function (rowIndex, rowData) {
                    WritedHours = WritedHours + parseFloat(rowData.Hours);
                }
            });

            ddlProject = jQuery("#<%=ddlProject.ClientID %>");
            ddlProject.change(function () {
                var _this = ddlProject.children("option:selected");
                var _projectID = _this.val();
                var loadedJson = GetTickets(_projectID);
                if (loadedJson == undefined || loadedJson == null || loadedJson.length == 0) {
                    jQuery.getJSON(
                            "/Do/TicketTasks.ashx?r=" + Math.random(),
                            {
                                type: "GetTicketsListByProject",
                                SheetDate: SelectedDate,
                                projectid: _projectID,
                                userid: "yes"
                            },
                            function (responseJsonList) {
                                //SetTickets(_projectID, responseJsonList);
                                //alert(responseJsonList.length);
                                SetTicketsControls(responseJsonList);
                            }
                    );
                }
            });

            ddlTitle.change(function () {
                var _this = ddlTitle.children("option:selected");
                ddlCode.children().removeAttr("selected");
                ddlCode.children().eq(_this.index()).attr("selected", true);
                jQuery("#ticketdesc").val(_this.attr("desc"));
                jQuery("#addPercentage").val(_this.attr("Percentage"));
            });
            ddlCode.change(function () {
                var _this = ddlCode.children("option:selected");
                ddlTitle.children().removeAttr("selected");
                ddlTitle.children().eq(_this.index()).attr("selected", true);
                jQuery("#ticketdesc").val(_this.attr("desc"));
                jQuery("#addPercentage").val(_this.attr("Percentage"));
            });

            btnAddNewTimesheet.click(function () {
                var row = {};
                var hours = 0;
                var percent = 0;
                var projectID = parseInt(ddlProject.children("option:selected").eq(0).val());
                var ticketID = parseInt(ddlCode.children("option:selected").eq(0).val());
                try {
                    var _hours = jQuery("#addhours").val();
                    hours = parseFloat(jQuery("#addhours").val());
                    if (_hours.length <= 0 || hours == NaN) {
                        ShowStatus({ Success: false, MessageContent: "Please input a number in Hours field." }, 5000);
                        return false;
                    }
                    else {
                        if (CheckOverLimitHours(hours)) {
                            return false;
                        }
                        else {
                            if (hours <= 0) {
                                ShowStatus({ Success: false, MessageContent: "Please input a number bigger than 0." }, 5000);
                                return false;
                            }
                            else {
                                percent = parseInt(jQuery("#addPercentage").val());
                                var checkNumber = true;
                                checkNumber = (projectID > 0) && (ticketID > 0) && (hours >= 0) && (percent >= 0);
                                if (checkNumber == false) {
                                    ShowStatus({ Success: false, MessageContent: "Check forms input field:Project, Title,Hours, Percentage" }, 5000);
                                    return false;
                                }
                                else {
                                    var _workDetail = jQuery("#addworkdetail").val();
                                    if (_workDetail.length <= 0) {
                                        ShowStatus({ Success: false, MessageContent: "WorkDetail field is required" }, 5000);
                                        return false;
                                    }
                                    else {
                                        row.isNewRecord = true;
                                        row.Hours = hours;
                                        row.IsMeeting = jQuery("#ismeeting").attr("checked") == "checked";
                                        row.IsSubmitted = false;
                                        row.Percentage = percent;
                                        row.ProjectID = projectID;
                                        row.ProjectTitle = ddlProject.children("option:selected").eq(0).text();
                                        row.TicketCode = ddlCode.children("option:selected").eq(0).text();
                                        row.TicketDescription = ddlCode.children("option:selected").eq(0).attr("desc");
                                        row.TicketID = ticketID;
                                        row.TicketTitle = ddlTitle.children("option:selected").eq(0).text();
                                        row.SheetDate = SelectedDate;
                                        row.TimeSheetID = 0;
                                        row.WorkDetail = jQuery("#addworkdetail").val();

                                        DataTable.edatagrid('appendRow', row);
                                        DataTable.edatagrid('beginEdit', CurrentTimeSheetsList.length - 1);
                                        DataTable.edatagrid('endEdit', CurrentTimeSheetsList.length - 1);
                                        DataTable.edatagrid('saveRow');
                                        UpdateTotalHours();
                                        ClearFormControl();
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (e) {
                    ShowStatus({ Success: false, MessageContent: "Check Input:Hours, Percentage" }, 5000);
                    return false;
                }
            });

            jQuery("#btnBack").click(function () {
                window.location.href = "/Sunnet/Tickets/ListTimesheet.aspx";
            });

            btnSubmit.click(function () {
                for (var i = 0; i < CurrentTimeSheetsList.length; i++) {
                    if (CurrentTimeSheetsList[i].TimeSheetID == 0) {
                        ShowStatus({ Success: false, MessageContent: "Please finish your timesheet first." }, 5000);
                        return false;
                    }
                }
                return confirm("Your timesheets cannot be edited once you submit. Proceed to submit?")
            });
        });
    </script>

</asp:Content>
