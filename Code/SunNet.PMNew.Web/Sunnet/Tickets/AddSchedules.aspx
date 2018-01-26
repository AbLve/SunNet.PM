<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/popWindow.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="AddSchedules.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.AddSchedules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link rel="stylesheet" href="/Styles/jsgantt.css" />--%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" id="frmAddSchedule">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="owmainBox" style="padding: 0px;">
            <table cellpadding="5" cellspacing="0" border="0" style="margin-left: 10px;">
                <tbody>
                    <tr>
                        <th>Title:<span class="redstar">*</span>
                        </th>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="input630" Width="445" MaxLength="200" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th valign="top">Description:
                        <span style="display: block;">(<span id="spnCurrentCount">0</span>/<span id="spnSum">1500</span>)</span>
                        </th>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" Rows="6" Columns="20" CssClass="input630"
                                Width="445" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <ul class="timeNum">
                                <li>08</li>
                                <li></li>
                                <li>09</li>
                                <li></li>
                                <li>10</li>
                                <li></li>
                                <li>11</li>
                                <li></li>
                                <li>12</li>
                                <li></li>
                                <li>13</li>
                                <li></li>
                                <li>14</li>
                                <li></li>
                                <li>15</li>
                                <li></li>
                                <li>16</li>
                                <li></li>
                                <li>17</li>
                                <li></li>
                                <li>18</li>
                                <li></li>
                                <li>19</li>
                                <li></li>
                                <li>20</li>
                                <li></li>
                                <li>21</li>
                                <li></li>
                                <li>22</li>
                                <li></li>
                                <li>23</li>
                                <li></li>
                            </ul>
                            <br>
                            <ul class="timeTable">
                                <asp:Repeater ID="rptPlan" runat="server">
                                    <ItemTemplate>
                                        <li class="<%# (bool)Eval("IsPlan") ? "onscheduleTime" :"" %>"></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <%--<div style="position: relative" class="gantt" id="GanttChartDIV" name="GanttChartDIV">
                        </div>--%>
                        </td>
                    </tr>
                    <tr>
                        <th>Start Time:<span class="redstar">*</span>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlStartHours" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                            Hours &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:DropDownList ID="ddlStartMinute" runat="server" ClientIDMode="Static">
                            <asp:ListItem Value="00">00</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                        </asp:DropDownList>
                            Minutes
                        </td>
                    </tr>
                    <tr>
                        <th>End Time:<span class="redstar">*</span>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlEndHours" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                            Hours &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:DropDownList ID="ddlEndMinute" runat="server" ClientIDMode="Static">
                            <asp:ListItem Value="00">00</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                        </asp:DropDownList>
                            Minutes
                        </td>
                    </tr>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <tr>
                                <th>Is Meeting:
                                </th>
                                <td>
                                    <asp:CheckBox ID="chkMeeting" runat="server"
                                        ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr runat="server" id="trMeetingUsers">
                                <th colspan="2">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="8%" valign="top">Users:
                                            </td>
                                            <td width="25%">
                                                <asp:ListBox ID="lstUsers" runat="server" class="input630" Style="width: 150px;"
                                                    Rows="6" DataTextField="FirstAndLastName" DataValueField="UserID" SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                            <td width="15%" align="center">
                                                <p>
                                                    <asp:Button ID="btnAddUsers" runat="server" Text="&gt;&gt;" />
                                                </p>
                                                <p>
                                                    <asp:Button ID="btnMoveUsers" runat="server" Text="&lt;&lt;" />
                                                </p>
                                            </td>
                                            <td width="20%" align="right" valign="top" style="padding-right: 5px;">Meeting Users:
                                            </td>
                                            <td width="32%">
                                                <asp:ListBox ID="lstMeetingUsers" runat="server" class="input630" Style="width: 150px;"
                                                    Rows="6" DataTextField="FirstAndLastName" DataValueField="UserID" SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </th>
                            </tr>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <tr>
                        <th>&nbsp;
                        </th>
                        <td>&nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnBoxone">
                <asp:Button ID="btnSave" runat="server" Text="Submit" CssClass="btnone" OnClientClick="submitSchedule('submit'); return false; "
                    Height="21" />

                <asp:Button ID="btnSaveAndNew" runat="server" Text="Submit and New" CssClass="btnone"
                    Height="21" Width="140" OnClientClick="submitSchedule('submitNew'); return false;" />
            </div>
        </div>
    </form>

    <script type="text/javascript">
        $("#" + "<%=btnAddUsers.ClientID%>").on("click", function (event) {
            $("#" + "<%=lstUsers.ClientID%>").find("option:selected").prop("selected", false).appendTo($("#" + "<%=lstMeetingUsers.ClientID%>"));
            return false;
        });

        $("#" + "<%=btnMoveUsers.ClientID%>").on("click", function (event) {
            $("#" + "<%=lstMeetingUsers.ClientID%>").find("option:not(option[value='<%=UserInfo.UserID%>']):selected").prop("selected", false).appendTo($("#" + "<%=lstUsers.ClientID%>"));
            return false;
        });

        function submitSchedule(type) {
            var ajaxUrl = "/Sunnet/Tickets/DoAddSchedule.ashx"
            var ValidateInput = function () {
                if ($('#<%=txtTitle.ClientID%>').get(0).value === '') {
                    return false;
                }
                else {
                    return true;
                }
            }

            var submitScheduleFrom = function (date, startTime, endTime) {
                var title = $("#txtTitle").val();
                var description = $("#txtDescription").val();
                var isMeeting = $("#chkMeeting").prop("checked");
                var meetingUserIds = "";
                $("#" + "<%=lstMeetingUsers.ClientID%>" + " option").each(function (index, item) {
                    meetingUserIds = meetingUserIds.replace(/$/, item.value + ",");
                });
                meetingUserIds = meetingUserIds.replace(/\,$/, "");
                $.post(ajaxUrl + "?type=save"
                    + "&" + "Date=" + date
                    + "&" + "startTime=" + startTime
                    + "&" + "endTime=" + endTime,
                    {
                        title: title,
                        description: description,
                        isMeeting: isMeeting,
                        meetingUserIds: meetingUserIds
                    }
                    , function (data) {
                        if (data == "1") {
                            ShowInfo("Operation successful.", function () {
                                if (type === "submit") {
                                    $.Zebra_Dialog.closeCurrent($("#" + "<%=btnSave.ClientID%>"));
                                }
                                else if (type === "submitNew") {
                                    clearInputs($("#" + "<%=btnSave.ClientID%>").closest("form"), true);
                                    $(".btnBoxone").css("display", "block");
                                    $("#" + "<%=trMeetingUsers.ClientID%>").css("display", "none");
                                }
                                else {
                                }
                            });
                    }
                    else {
                        ShowInfo(data);
                    }
                    });
            }

            if (ValidateInput()) {//先客户端验证一次
                var startTime = $("#" + "<%=ddlStartHours.ClientID%>").val() + ":" + $("#" + "<%=ddlStartMinute.ClientID%>").val();
                var endTime = $("#" + "<%=ddlEndHours.ClientID%>").val() + ":" + $("#" + "<%=ddlEndMinute.ClientID%>").val();
                var date = "<%=Date%>";

                $.get(ajaxUrl + "?type=savevalidate" + "&" + "r=" + Math.random(), {
                    Date: date,
                    startTime: startTime,
                    endTime: endTime
                }, function (result) {   //再服务端验证一次
                    if (result == "1") {
                        //提交form信息
                        $(".btnBoxone").css("display", "none");
                        submitScheduleFrom(date, startTime, endTime);
                    }
                    else if (result == "-1") {
                        var message = "The hours of the two schedules conflict. Are you sure you want to continue?."
                        MessageBox.Confirm3(null, message, '', '', conformSchedule);
                    }
                    else {
                        //显示错误消息
                        ShowInfo(result);
                    }

                });
            }
            else {
                ShowMessage("Please enter the title.", 0, false, false);
                $("#<%=txtTitle.ClientID%>").focus();
            }

            var conformSchedule = function (e) {
                if (e === true) {
                    submitScheduleFrom(date, startTime, endTime);
                }
            }


            var clearInputs = function (form, includeHidden) {
                var re = /^(?:color|date|datetime|email|month|number|password|range|search|tel|text|time|url|week)$/i; // 'hidden' is not in this list
                return form.find("input,select,textarea").each(function () {
                    var t = this.type, tag = this.tagName.toLowerCase();
                    if (re.test(t) || tag == 'textarea') {
                        this.value = '';
                    }
                    else if (t == 'checkbox' || t == 'radio') {
                        this.checked = false;
                    }
                    else if (tag == 'select') {
                        this.selectedIndex = 1;
                        if (this.id == "<%=lstMeetingUsers.ClientID%>") {
                            $(this).find("option:not(option[value='<%=UserInfo.UserID%>'])").remove();
                        }
                    }
                    else if (includeHidden) {
                        if ((includeHidden === true && /hidden/.test(t)) ||
                             (typeof includeHidden == 'string' && $(this).is(includeHidden)))
                            this.value = '';
                    }
                });
            };
}

$(function () {
    $("#" + "<%=chkMeeting.ClientID%>").on("click", function () {
        $("#" + "<%=trMeetingUsers.ClientID%>").toggle();
    });
    var $spnSum = $("#spnSum");
    var $spnCurrentCount = $("#spnCurrentCount");
    $spnCurrentCount.text($("#" + "<%=txtDescription.ClientID%>").val().length);
    $("#" + "<%=txtDescription.ClientID%>").on("keyup", function (event) {
        var sum = $spnSum.text();
        var currentCount = $("#" + "<%=txtDescription.ClientID%>").val().length;
        if (currentCount >= sum) {
            $spnSum.css("color", "red");
            $("#" + "<%=txtDescription.ClientID%>").val($("#" + "<%=txtDescription.ClientID%>").val().slice(0, 1500));
            $spnCurrentCount.text(sum).css("color", "red");
            event.stopPropagation();
            event.preventDefault();
        }
        else {
            $spnSum.css("color", "black");
            $spnCurrentCount.css("color", "black").text(currentCount);
        }
    });

});
/* $(function() {
var schedules = eval(' jscheduleResult');
g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'hour');
g.setShowRes(0); // Show/Hide Responsible (0/1)
g.setShowDur(0); // Show/Hide Duration (0/1)
g.setShowComp(0); // Show/Hide % Complete(0/1)
g.setCaptionType('none');  // Set to Show Caption
if (g) {
for (var i = 0; i < schedules.length; i++) {
g.AddTaskItem(new JSGantt.ScheduleItem(schedules[i].id, schedules[i].name, schedules[i].startDate, schedules[i].endDate));
}
g.Draw();
g.DrawDependencies();
}
});*/
    </script>

</asp:Content>
