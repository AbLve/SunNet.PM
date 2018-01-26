<%@ Page Title="My Timesheets " Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="ListTimesheet.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.ListTimesheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/smoothness/jquery-ui-1.9.0.custom.min.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/datagrid/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/datagrid/default/easyui.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        td > div
        {
            word-break: normal;
            word-wrap: break-word;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    My Timesheets
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainrightBox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="78%" valign="top">
                    <div id="tips" runat="server">
                        You have the following timesheet(s) to submit. They are:
                        <br />
                        <asp:Literal ID="ltlUnSubmitted" runat="server"></asp:Literal>
                        <br />
                        <asp:Literal ID="ltlUnfinished" runat="server"></asp:Literal>
                        <br />
                    </div>
                    <div id="timesheets" style="width: 98%;">
                    </div>
                </td>
                <td width="22%" valign="top">
                    <div id="datetimepicker">
                    </div>
                    <div>
                        <br />
                        <span id="txtSelectedDate"></span>
                        <input id="addtimesheet" name="Submit" type="button" class="btnthree" value="Add Timesheet" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        Date.prototype.format = function(format) {
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
    </script>

    <script type="text/javascript">
        var SelectedDates = [];
        var SelectedDate = "";
        var Today = '<%=Today.ToString("yyyy-MM-dd") %>';
        var LimitDate = '<%=SunNet.PMNew.Entity.TimeSheetModel.TimeSheetTicket.LimitDate.ToString("yyyy-MM-dd") %>'
        var GetDataUrl = '/Do/TimeSheet.ashx?type=GetTimeSheetsByWeek&category=0&date=';
        function GetDataUrlString(date) {
            return GetDataUrl + date;
        }

        function GetDateFromString(date) {
            var _thisday = new Date();
            var _thisYear = parseInt(date.substr(0, 4));

            var _thisMonth = date.substr(5, 2);
            if (_thisMonth.indexOf("0") == 0) {
                _thisMonth = _thisMonth.slice(1);
            }
            _thisMonth = parseInt(_thisMonth) - 1;

            var _thisDate = date.substr(8, 2);
            if (_thisDate.indexOf("0") == 0) {
                _thisDate = _thisDate.slice(1);
            }
            _thisDate = parseInt(_thisDate);
            _thisday = new Date(_thisYear, _thisMonth, _thisDate);
            return _thisday;
        }
        function GetDateFromDate(days, date) {
            var _thisday = GetDateFromString(date);
            var result = _thisday;
            result.setDate(_thisday.getDate() + days);
            return result;
        }
        function GetDateFromToday(days) {
            return GetDateFromDate(days, Today);
        }

        function LoadData(date) {
            if (GetDateFromString(date) <= GetDateFromString(LimitDate)) {
                SelectedDates.push(date);
                SelectedDate = date;
                //alert("dateLoaded:" + date);
                var table = AddEmptyTable(date);
                DataBind(table, date);
            }
        }
        function LoadThisWeek() {
            LoadWeek(Today);
        }
        function LoadWeek(date) {
            SelectedDates = [];
            TimeSheetsContainer.html("");
            var _thatDay = GetDateFromString(date);
            var _indexweek = _thatDay.getDay();
            if (_indexweek == 0) {
                _indexweek = 7;
            }
            for (var i = 1; i <= 7; i++) {
                var _days = -(_indexweek - i);
                var _week = GetDateFromDate(_days, date);
                LoadData(_week.format("yyyy-MM-dd"));
            }
        }

        function AddEmptyTable(date) {
            var tableHtml = "<table title='Timesheets : ";
            var formatDate = GetDateFromString(date).format("MM/dd/yyyy");
            tableHtml = tableHtml + formatDate;
            tableHtml = tableHtml + "' id='timesheet";
            tableHtml = tableHtml + date
            tableHtml = tableHtml + "' width='95%'  pagination='false' idfield='TimeSheetID' rownumbers='true' fitcolumns='true' singleselect='true'>";
            tableHtml = tableHtml + "</table> <br/>";
            TimeSheetsContainer.append(tableHtml);
            return "timesheet" + date;
        }
        function DataBind(table, date) {
            jQuery("#" + table).datagrid({
                autoRowHeight: true,
                url: GetDataUrlString(date),
                columns: [
                            [
                            { field: 'ProjectTitle', title: 'Project', width: 15 },
                            { field: 'TicketTitle', title: 'Title', width: 10 },
                            { field: 'TicketCode', title: 'Code', width: 6 },
                            { field: 'WorkDetail', title: 'Work detail', width: 40 },
                            { field: 'Hours', title: 'Hours (H)', width: 6 },
                            { field: 'Percentage', title: 'PCT (%)', width: 6 },
                            { field: 'SubmittedText', title: 'IsSubmitted', width: 10 },
                            { field: 'Action', title: 'Action', width: 6 }
                            ]
                        ],
                onLoadSuccess: function(data) {
                    var totalCount = 0;
                    for (var i = 0; i < data.total; i++) {
                        totalCount = totalCount + parseFloat(data.rows[i].Hours);
                    }
                    //totalCount = 9;
                    var title = jQuery("#" + table).datagrid("options").title;
                    var newtitle;
                    if (totalCount < 8) {
                        newtitle = title + " ( Total Hours: <span style='color:red;'><strong>" + totalCount + "</strong></span> )";
                    }
                    else {
                        newtitle = title + " ( Total Hours: <span><strong>" + totalCount + "</strong></span> )";
                    }
                    jQuery("div.panel-title").each(function() {
                        var _this = jQuery(this);
                        if (_this.text() == title) {
                            _this.html(newtitle);
                            return;
                        }
                    });
                    // merge last cols
                    //                    jQuery("#" + table).datagrid("mergeCells", 
                    //                                                    {
                    //                                                    index: 0,
                    //                                                    field: "Action",
                    //                                                    rowspan: data.total,
                    //                                                    colspan: 0
                    //                                                    }
                    //                                                 );
                }
            });
        }
        var TimeSheetsContainer;
        jQuery(function() {
            TimeSheetsContainer = jQuery("#timesheets");

            jQuery("#datetimepicker").datepicker({
                dateFormat: "yy-mm-dd",
                onSelect: function(date, obj) {
                    //                    SelectedDates.push(date);
                    //                    if (SelectedDates.length >= 2) {
                    //                        if (SelectedDates[SelectedDates.length - 2] != date) {
                    //                            LoadData(date);
                    //                        }
                    //                    }
                    //                    else {
                    //                        LoadWeek(date);
                    //                    }
                    if (GetDateFromString(date) <= GetDateFromString(LimitDate)) {
                        SelectedDate = date;
                        jQuery("#txtSelectedDate").html(GetDateFromString(SelectedDate).format("MM/dd/yyyy"));
                        for (var i = 0; i < SelectedDates.length; i++) {
                            if (SelectedDates[i] == date) {
                                return false;
                            }
                        }
                        LoadWeek(date);
                    }
                }
            });
            jQuery("#addtimesheet").click(function() {
                window.location.href = "AddTimesheet.aspx?date=" + SelectedDate;
            });
            LoadThisWeek();
            SelectedDate = Today;
            jQuery("#txtSelectedDate").html(GetDateFromString(SelectedDate).format("MM/dd/yyyy"));
        }); 
    </script>

</asp:Content>
