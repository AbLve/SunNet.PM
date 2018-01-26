<%@ Page Title="Project-User-TimesheetsDetail" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="ProjectUserTimeSheet.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Reports.ProjectUserTimeSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/smoothness/jquery-ui-1.9.0.custom.min.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/datagrid/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/datagrid/default/easyui.css" rel="stylesheet" type="text/css" />

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
        var DateQueue = "<%=DateQueue %>";
        var UserID = "<%=SelectedUser.ID %>";
        var ProjectID = '<%=Request.Params["project"] %>';
        var Today;
        var GetDataUrl = '/Do/TimeSheet.ashx?type=GetTimeSheetsByDate&category=0&date=';
        function GetDataUrlString(date) {
            var tempUrl = GetDataUrl
            tempUrl = tempUrl + date;
            tempUrl = tempUrl + "&project=";
            tempUrl = tempUrl + ProjectID;
            tempUrl = tempUrl + "&userid=";
            tempUrl = tempUrl + UserID;
            return tempUrl;
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
            SelectedDates.push(date);
            SelectedDate = date;
            //alert("dateLoaded:" + date);
            var table = AddEmptyTable(date);
            DataBind(table, date);
        }
        function LoadThisWeek() {
            LoadWeek(Today);
        }
        function LoadWeek(date) {
            SelectedDates = [];
            TimeSheetsContainer.html("");
            var _thatDay = GetDateFromString(date);
            var _indexweek = _thatDay.getDay();
            for (var i = 1; i <= 7; i++) {
                var _days = -(_indexweek - i);
                var _week = GetDateFromDate(_days, date);
                LoadData(_week.format("yyyy-MM-dd"));
            }
        }

        function AddEmptyTable(date) {
            TimeSheetsContainer.append("<table title='Timesheets:" + date + "' id='timesheet" + date + "' width='95%'  pagination='false' idfield='TimeSheetID' rownumbers='true' fitcolumns='true' singleselect='true'></table> <br/>");
            return "timesheet" + date;
        }
        function DataBind(table, date) {
            jQuery("#" + table).datagrid({
                url: GetDataUrlString(date),
                columns: [
                            [
                            { field: 'ProjectTitle', title: 'Project', width: 10 },
                            { field: 'TicketTitle', title: 'Title', width: 10 },
                            { field: 'TicketCode', title: 'Code', width: 10 },
                            { field: 'WorkDetail', title: 'Work detail', width: 30 },
                            { field: 'Hours', title: 'Hours', width: 10 },
                            { field: 'Percentage', title: 'Percentage', width: 10 },
                            { field: 'SubmittedText', title: 'Submitted', width: 10 }
                            ]
                        ],
                onLoadSuccess: function(data) {
                    GetData();
                }
            });
        }
        var TimeSheetsContainer;
        var DateQueueLength = 0;
        var DataQueueIndex = 0;
        var DataQueueArray = [];
        function GetData() {
            if (DataQueueIndex < DateQueueLength) {
                LoadData(DataQueueArray[DataQueueIndex]);
                DataQueueIndex++;
            }
        }
        jQuery(function() {
            TimeSheetsContainer = jQuery("#timesheets");
            DataQueueArray = DateQueue.split(",");
            DateQueueLength = DataQueueArray.length;
            GetData();
        }); 
    </script>

    <style type="text/css">
        .customWidth
        {
            width: 100%;
            background-color: #EFF5FB;
            min-height: 700px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Projects:&nbsp;<%=SelectedProject.Title%>&nbsp;&nbsp;&nbsp;&nbsp; User:&nbsp;<%=SelectedUser.LastName%>,<%=SelectedUser.FirstName%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainrightBox" style="width: 100%;">
        <br />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td valign="top">
                    <div id="timesheets" style="width: 99%; padding: 0 5px;">
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
