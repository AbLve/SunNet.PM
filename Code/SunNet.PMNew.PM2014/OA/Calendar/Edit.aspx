<%@ Page Language="C#"  MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Calendar.Edit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript">

    </script>
    <style>
        ul.timeTable li.onscheduleTime {
            background-color: #f60;
            border: 1px solid #f60;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Add Schedules
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div class="form-group">
      <label class="col-left-schedule lefttext">Title:<span class="noticeRed">*</span></label>
      <div class="col-right-schedule righttext">
          <asp:TextBox ID="txtTitle" runat="server" CssClass="inputschedule"  MaxLength="200" ClientIDMode="Static"></asp:TextBox>
          <asp:HiddenField ID="hdID" runat="server" />
          <asp:HiddenField ID="hdPlanDate" runat="server" />
      </div>
    </div>
	<div class="form-group">
      <label class="col-left-schedule lefttext">Description:<br />
      <span style="display: block;">(<span id="spnCurrentCount">0</span>/<span id="spnSum">1500</span>)</span></label>
      <div class="col-right-schedule righttext">
            <asp:TextBox ID="txtDescription" runat="server" Rows="5" Columns="20" CssClass="inputschedule"
                    TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
      </div>
    </div>
    <div class="form-group">
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td><ul class="timeNum">
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
              <br />
            <ul class="timeTable">
                <asp:Repeater ID="rptPlan" runat="server">
                    <ItemTemplate>
                        <li class="<%# (bool)Eval("IsPlan") ? "onscheduleTime" :"" %>"></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            </td>
          <td>&nbsp;</td>
        </tr>
      </table>
    </div>
	<div class="form-group">
      <label class="col-left-schedule lefttext">Start Time:<span class="noticeRed">*</span></label>
      <div class="col-right-schedule righttext">
        <span class="rightItem">
            <asp:DropDownList ID="ddlStartHours" CssClass="inputtime" runat="server" ClientIDMode="Static">
            </asp:DropDownList> 
            Hours </span>
        <span class="rightItem">
            <asp:DropDownList ID="ddlStartMinute"  CssClass="inputtime" runat="server" ClientIDMode="Static">
                            <asp:ListItem Value="00">00</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                        </asp:DropDownList>
            Minutes </span>
      </div>
    </div>
    
	<div class="form-group">
      <label class="col-left-schedule lefttext">End Time:<span class="noticeRed">*</span></label>
      <div class="col-right-schedule righttext"> <span class="rightItem">
        <asp:DropDownList ID="ddlEndHours" CssClass="inputtime" runat="server" ClientIDMode="Static">
        </asp:DropDownList>
        Hours </span> 
          <span class="rightItem">
        <asp:DropDownList ID="ddlEndMinute" runat="server" ClientIDMode="Static" CssClass="inputtime">
            <asp:ListItem Value="00">00</asp:ListItem>
            <asp:ListItem Value="30">30</asp:ListItem>
        </asp:DropDownList>
          Minutes </span> </div>
    </div>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
	    <div class="form-group">
          <label class="col-left-schedule lefttext">Is Meeting?</label>
          <div class="col-right-schedule righttext" >
                <asp:CheckBox ID="chkMeeting" runat="server"
                    ClientIDMode="Static" />
                <span id="spnMeetingStatus">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; whether to hold the
            conference:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <asp:Button ID="btnOK" runat="server" Text="agree" OnClick="btnOK_Click" Visible="false" />
                <asp:Button ID="btnNo" runat="server" Text="disagree" OnClick="btnNo_Click" Visible="false" />
                <asp:Literal ID="litAgree" runat="server" Visible="false"></asp:Literal>
          </div>
        </div>
	    <div class="form-group" id="divUser" runat="server">
          <label class="col-left-schedule lefttext">Meeting Users:</label>
          <div class="col-right-schedule righttext" style="width:550px">
              <ul class="assignUser">
                <asp:Repeater ID="rtpUser" runat="server" OnItemDataBound="rtpUser_ItemDataBound">
                    <ItemTemplate>
                        <li>
                            <asp:CheckBox runat="server" name="cbUser" id="cbUser" />
                            <asp:Literal runat="server" Visible="false" ID="ltlid" Text='<%#Eval("UserID")%>'></asp:Literal>
                           <%# Eval("FirstAndLastName")%>
                        </li> 
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
          </div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <script type="text/javascript">
        function submitSchedule(type) {
            var ajaxUrl = "/OA/Calendar/DoAddSchedule.ashx"
            
            function submitSchedule(type) {
                var ValidateInput = function () {
                    if ($('#<%=txtTitle.ClientID%>').get(0).value === '') {
                    return false;
                }
                else {
                    return true;
                }
            }

            var submitScheduleFrom = function (id, startTime, endTime) {
                var title = $("#txtTitle").val();
                var description = $("#txtDescription").val();
                var isMeeting = $("#chkMeeting").prop("checked");
                var meetingUserIds = "";
                $('input[name="cbUser"]:checked').each(function () {
                    meetingUserIds = meetingUserIds.replace(/$/, $(this).val() + ",");
                });
                meetingUserIds = meetingUserIds.replace(/\,$/, "");
                $.post(ajaxUrl + "?type=edit"
                    + "&" + "id=" + id
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
                            debugger;
                            ShowInfo("Operation successful.", function () {
                                if (type === "edititNew") {
                                    dialogParam = { type: "new", date: "<%=date%>" };
                                }
                                $.Zebra_Dialog.closeCurrent($("#" + "<%=btnSave.ClientID%>"));
                            });
                        }
                        else {
                            alert(data);
                            ShowInfo(data);
                        }
                    });
            }

                if (ValidateInput()) {//先客户端验证一次
                    var startTime = $("#" + "<%=ddlStartHours.ClientID%>").val() + ":" + $("#" + "<%=ddlStartMinute.ClientID%>").val();
                    var endTime = $("#" + "<%=ddlEndHours.ClientID%>").val() + ":" + $("#" + "<%=ddlEndMinute.ClientID%>").val();
                    var id = $("#" + "<%=hdID.ClientID%>").val();
                    $.get(ajaxUrl + "?type=editvalidate", {
                        id: id,
                        startTime: startTime,
                        endTime: endTime
                    }, function (result) {   //再服务端验证一次
                        if (result == "1") {
                            //提交form信息
                            $(".btnBoxone").css("display", "none");
                            submitScheduleFrom(id, startTime, endTime);
                        }
                        else if (result == "-1") {
                            var message = "The hours of the two schedules conflict."
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
                        submitScheduleFrom(id, startTime, endTime);
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
                            $(this).removeAttr("disabled");
                            this.checked = false;
                        }
                        else if (tag == 'select') {
                            this.selectedIndex = 1;

                        }
                        else if (includeHidden) {
                            if ((includeHidden === true && /hidden/.test(t)) ||
                                 (typeof includeHidden == 'string' && $(this).is(includeHidden))) {
                                if (this.id != "<%=hdID.ClientID%>") {
                                    this.value = '';
                                }
                            }
                        }

                        $("#" + "<%=divUser.ClientID%>").css("display", "none");
                    });
                };
            }

            function deleteSchedule() {
                $.get(ajaxUrl + "?" + "type=delete" + "&id=" + "<%=hdID.Value%>", function (data) {
                    if (data == "1") {
                        $.Zebra_Dialog.closeCurrent($("#" + "<%=btnSave.ClientID%>"));
                    }
                    else {
                        ShowInfo(data);
                    }
                });
            };

            function dealMeeting(type) {
                $("#" + "<%=btnOK.ClientID%>").css("display", "none");
                $("#" + "<%=btnNo.ClientID%>").css("display", "none");
                $.get(ajaxUrl + "?" + "type=" + type + "&id=" + "<%=hdID.Value%>", function (data) {
                    if (data == "1") {
                        //这里要做相应的逻辑处理。
                        if (type == "agree") {
                            $("#spnMeetingStatus").append("agree");
                        }
                        else {
                            $("#spnMeetingStatus").append("disagree");
                        }

                    }
                    else {
                        ShowInfo(data);
                        $("#" + "<%=btnOK.ClientID%>").css("display", "inline");
                        $("#" + "<%=btnNo.ClientID%>").css("display", "inline");
                    }
                });
            }

    $(function () {
            $("#" + "<%=chkMeeting.ClientID%>").on("click", function () {
                        $("#" + "<%=divUser.ClientID%>").toggle();
                    });
            
            var $spnSum = $("#spnSum");
            var $spnCurrentCount = $("#spnCurrentCount");
            var currentLength = $("#" + "<%=txtDescription.ClientID%>").val().length;
                        $spnCurrentCount.text(currentLength);
                        if (currentLength >= 1500) {
                            $spnCurrentCount.css("color", "red");
                            $spnSum.css("color", "red");
                        }

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
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
        <asp:Button ID="btnSave" runat="server" Text="Submit" CssClass="saveBtn1 mainbutton" 
            OnClick="btnSave_Click"  />
        <asp:Button ID="btnSaveAndNew" runat="server" Text="Submit &amp; New" CssClass="saveBtn1 mainbutton"
             OnClick="btnSaveAndNew_Click" />
        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="cancelBtn1 mainbutton" Visible="false" 
             OnClick="btnDelete_Click" />
</asp:Content>