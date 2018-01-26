<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sunnet/InputPop.Master"
    CodeBehind="EditEvent.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.EditEvent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        td {
            font-family: 'Open Sans', sans-serif;
            color: #04466a;
            font-size: 13px;
        }

        .txtDetail {
            width: 200px;
            padding-left: 3px;
            padding-right: 3px;
        }

        input, select, textarea {
            color: #333;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <asp:Literal runat="server" ID="ltrlControlTitle">Edit Event</asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainowConbox" style="min-height: 50px;">
        <table width="98%" class="owTable1" border="0" cellpadding="0" cellspacing="0">
            <tr align="right">
                <td align="right">Project:
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlProjects" runat="server" class="select205">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="20%" align="right">Name:
                </td>
                <td>
                    <asp:TextBox CssClass="inputNormal" confirm='ex.Birthday Party' ID="txtName" runat="server"></asp:TextBox>
                    <asp:Image runat="server" Width="24" Height="24" ID="imgIcon" align="absmiddle" ImageUrl="/Images/EventIcon/event_icon_1s.png" />
                    <div class="eventiconBox" style="display: none;" id="divEventIcon">
                        <ul class="eventiconitem">
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="1" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_1.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="2" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_2.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="3" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_3.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="4" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_4.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="5" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_5.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="6" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_6.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="7" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_7.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="8" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_8.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="9" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_9.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="10" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_10.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="11" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_11.png"></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input type="radio" value="12" name="radioEventIcon" onclick="clickRadioIcon(this)">
                                </div>
                                <img src="/Images/EventIcon/event_icon_12.png"></li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr align="right">
                <td align="right">Detail:
                </td>
                <td align="left">
                    <asp:TextBox TextMode="MultiLine" ID="txtDetails" CssClass="txtDetail" confirm='Add Event Detail?' runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">Where:
                </td>
                <td>
                    <asp:TextBox CssClass="inputNormal" ID="txtWhere" confirm='Add a Place?' runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">all-day:
                </td>
                <td>
                    <asp:CheckBox ID="chkAllDay" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="right">From:
                </td>
                <td>
                    <asp:TextBox CssClass="inputNormal inpuDate" ID="txtFrom" runat="server"></asp:TextBox>
                    <asp:TextBox CssClass="inputNormal" ID="txtFromTime" onblur="maskEventFromTime(this);"
                        value="12:00" Style="width: 55px;" runat="server"></asp:TextBox>
                    <asp:DropDownList CssClass="selectNormal" ID="selectFromType" Style="width: 55px;"
                        runat="server">
                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">To:
                </td>
                <td>
                    <asp:TextBox CssClass="inputNormal inpuDate" ID="txtTo" runat="server"></asp:TextBox>
                    <asp:TextBox CssClass="inputNormal" ID="txtToTime" onblur="maskEventFromTime(this);"
                        value="01:00" Style="width: 55px;" runat="server"></asp:TextBox>
                    <asp:DropDownList CssClass="selectNormal" ID="selectToType" Style="width: 55px;"
                        runat="server">
                        <asp:ListItem Text="AM" Value="1"></asp:ListItem>
                        <asp:ListItem Text="PM" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">Alert:
                </td>
                <td>
                    <asp:DropDownList ID="ddlAlert" CssClass="selectNormal" runat="server">
                        <asp:ListItem Text="None" Value="1"></asp:ListItem>
                        <asp:ListItem Text="five minutes before" Value="2"></asp:ListItem>
                        <asp:ListItem Text="fifteen minutes before" Value="3"></asp:ListItem>
                        <asp:ListItem Text="thirty minutes before" Value="4"></asp:ListItem>
                        <asp:ListItem Text="one hour before" Value="5"></asp:ListItem>
                        <asp:ListItem Text="two hours before" Value="6"></asp:ListItem>
                        <asp:ListItem Text="one day before" Value="7"></asp:ListItem>
                        <asp:ListItem Text="two days before" Value="8"></asp:ListItem>
                        <asp:ListItem Text="On date of" Value="9"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" id="tbInviteFriendView">
            <tbody>
                <tr>
                    <td colspan="2">
                        <div class="owBtnbox1">
                            <asp:Button Text="Save" CssClass="owBtn1" ID="btnSave" runat="server" OnClick="btnSave_Click" />
                            <input name="button222" type="button" class="owcancelBtn1" onclick="cancelAddEvent()"
                                value="Cancel" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" id="tbDeleteEvent"
            style="display: none; margin-top: 35px;">
            <tbody>
                <tr id="trBtns" runat="server">
                    <td>
                        <div class="owBtnbox1">
                            <asp:Button CssClass="owcancelBtn1" Text="Delete" ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
                        </div>
                    </td>
                    <td>
                        <div class="owBtnbox1">
                            <asp:Button Text="Save" CssClass="owBtn1" ID="Button1" runat="server" OnClick="btnSave_Click" />
                            <input
                                name="button22" type="button" class="owcancelBtn1" onclick="cancelAddEvent()"
                                value="Cancel" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="Icon" runat="server" />
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

        function checkDate(t, v) {
            console.log(t);
            console.log(v);
            var tmpFrom = jQuery("#<%=txtFrom.ClientID%>").val();
            var tmpTo = jQuery("#<%=txtTo.ClientID%>").val();
            var fromDate, toDate;
            var arr;
            if (t == 1) {
                arr = v.split("/");
                fromDate = new Date(arr[2], arr[0] - 1, arr[1]);
                if (tmpTo.indexOf("/") > -1) {
                    arr = tmpTo.split("/");
                    toDate = new Date(arr[2], arr[0] - 1, arr[1]);
                }
                if (fromDate > toDate)
                    jQuery("#<%=txtTo.ClientID%>").val(fromDate.format("MM/dd/yyyy"));
            }
        }

        function clickIcon() {
            jQuery("#divEventIcon").css("display", "");
        }

        function clickRadioIcon(o) {
            var tmpIcon = jQuery(o).val();
            jQuery("#<%=imgIcon.ClientID%>").attr("src", "/Images/EventIcon/event_icon_" + tmpIcon + "s.png");
        }


        function maskEventFromTime(o) {
            var reg = /^[0-2]?[0-9]:[0-5][0-9]?$/;
            var v = jQuery(o).val();
            if (reg.test(v)) {
                var date = new Date(2007, 3, 30, v.split(':')[0], v.split(':')[1], 10);
                if (date) {
                    var v1 = date.format("hh:mm");
                    var v1h = v1.split(':')[0];
                    var v1m = v1.split(':')[1];
                    if (v1h == "0" || v1h == "00")
                        v1h = "12";
                    else if (v1h > 12)
                        v1h = v1h - 12;
                    jQuery(o).val(v1h + ":" + v1m);
                }
                else
                    jQuery(o).val("12:00");
            }
            else jQuery(o).val("12:00");
        }

        function maskEventToTime(o) {
            var reg = /^[0-2]?[0-9]:[0-5][0-9]?$/;
            var v = jQuery(o).val();
            if (reg.test(v)) {
                var date = new Date(2007, 3, 30, v.split(':')[0], v.split(':')[1], 10);
                if (date) {
                    var v1 = date.format("hh:mm");
                    var v1h = v1.split(':')[0];
                    var v1m = v1.split(':')[1];
                    if (v1h == "0" || v1h == "00")
                        v1h = "12";
                    else if (v1h > 12)
                        v1h = v1h - 12;
                    jQuery(o).val(v1h + ":" + v1m);
                }
                else
                    jQuery(o).val("1:00");
            }
            else jQuery(o).val("1:00");
        }

        var closeEventContainer = 0;
        var colseEventIcon = 0;
        jQuery(function () {
            jQuery("#divPrivacy").on("mouseleave", function () {
                closeEventContainer = setTimeout(function () { jQuery("#divPrivacy").hide(); }, 100);
            }).on("mouseenter", function () {
                clearTimeout(closeEventContainer);
            });
            jQuery("#divEventIcon").on("mouseleave", function () {
                colseEventIcon = setTimeout(function () { jQuery("#divEventIcon").hide(); }, 100);
            }).on("mouseenter", function () {
                clearTimeout(colseEventIcon);
            });
        });

        var editEventId = 0;
        var newFriendView = true;
        var inviteFriendRefresh = false;
        function inviteFriendView(id, refresh) {
            if (id) { //edit
                if (refresh)
                    inviteFriendRefresh = true;
                jQuery("#divInviteFriend").modal("reload", "/Events/Events/Invitefriend/" + id);
            }
            else {
                if (newFriendView) {
                    jQuery("#divInviteFriend").modal("reload", "/Events/Events/Invitefriend");
                }
                else {
                    jQuery("#divInviteFriend").modal("show");
                }
            }
        }

        function saveInviteFriendView(id) {
            if (id) {
                var ids = "";
                jQuery("#divInviteFriend").find("input[name='chkUserID']:checked").each(function () {
                    ids += jQuery(this).val() + ",";
                });
                jQuery.post("/Events/Events/SaveInvitefriend", { "user": ids, "eventId": id }, function () {
                    jQuery("#divInviteFriend").modal('hide');
                    if (inviteFriendRefresh)
                        refreshParent();
                });

            }
            else {
                newFriendView = false;
                jQuery("#divInviteFriend").modal('hide');
            }
        }

        function cancelInviteFriendView() {
            newFriendView = true;
            jQuery("#divInviteFriend").modal('hide');
        }

        function deleteEvent() {
            jQuery.confirm("Are you sure you want to delete this event?",
            {
                yesText: "Delete",
                noText: "Cancel",
                yesCallback: function () {
                    jQuery.post("/Events/Events/DeleteEvent/" + editEventId, {}, function () {
                        jQuery("#createEvent").modal('hide');
                        removeViewEvent(editEventId);
                    });
                },
                noCallback: function () { }
            });
        }

        function createEvents(date) {
            jQuery("#divPopTitle").text("Create New Event");
            editEventId = 0;
            newFriendView = true;
            jQuery("#txtName").val();
            jQuery("#txtDetails").val()
            jQuery("#txtWhere").val();
            jQuery("#txtFrom").val(date);
            jQuery("#txtTo").val(date);
            jQuery("#txtFromTime").val("12:00");
            jQuery("#selectFromType").val("2");
            jQuery("#txtToTime").val("1:00");
            jQuery("#selectToType").val("2");
            jQuery("#txtEndDate").val(date);
            jQuery("#divPrivacyValue").text("Family");
            jQuery("input[name='chkPrivacy']").each(function () {
                if (jQuery(this).attr("text") == "Family") {
                    jQuery(this).prop("checked", true);
                }
                else
                    jQuery(this).prop("checked", false);
            });

            jQuery("#trRepeat").css("display", "");
            jQuery("#tbInviteFriendView").css("display", "");
            jQuery("#tbDeleteEvent").css("display", "none");
            jQuery("#createEvent").modal('show');
        }

        function editEvent(id) {
            $.getJSON("/Events/Events/GetEvent/" + id, {}, function (data) {
                jQuery("#divPopTitle").text("Edit Event Info");
                jQuery("#trRepeat").css("display", "none");
                jQuery("#txtName").val(data.Name);
                jQuery("input[name='radioEventIcon']").each(function () {
                    if (jQuery(this).val() == data.Icon) {
                        jQuery(this).prop("checked", true);
                        jQuery(this).click();
                        return;
                    }
                });
                jQuery("#txtDetails").val(data.Details)
                jQuery("#txtWhere").val(data.Where);
                jQuery("#txtFrom").val(data.FromDayString.substring(0, 10));
                jQuery("#txtTo").val(data.ToDayString.substring(0, 10));
                if (data.AllDay) {
                    jQuery("#chkAllDay").prop("checked", true);
                    checkAllday();
                } else {
                    jQuery("#txtFromTime").val(data.FromTime);
                    jQuery("#selectFromType").val(data.FromTimeType);
                    jQuery("#txtToTime").val(data.ToTime);
                    jQuery("#selectToType").val(data.ToTimeType);
                }
                jQuery("#selectAlert").val(data.Alert);

                switch (data.Privacy) {
                    case 1:
                        jQuery("#chkOnlyme").prop("checked", true);
                        break;
                    case 2:
                        jQuery("#chkPublic").prop("checked", true);
                        break;
                    case 3:
                        if (data.GroupID != "") {
                            var tmpids = data.GroupID.split(',');
                            for (var i = 0; i < tmpids.length; i++) {
                                jQuery("#chkPrivacy" + tmpids[i]).prop("checked", true);
                            }
                        }
                        if (data.HasInvite)
                            jQuery("#chkInviteOnly11").prop("checked", true);
                        break;
                    case 4:
                        jQuery("#chkInviteOnly11").prop("checked", true);
                        break;
                }
                var tmpV = "";
                jQuery("input[name='chkPrivacy']:checked").each(function () {
                    tmpV += jQuery(this).attr("text") + ", ";
                });
                if (tmpV != "")
                    jQuery("#divPrivacyValue").text(tmpV.substring(0, tmpV.length - 2));

                jQuery("#tbInviteFriendView").css("display", "none");
                jQuery("#tbDeleteEvent").css("display", "");
                editEventId = id;
                jQuery("#createEvent").modal('show');
            });
        }

        function cancelAddEvent(r) {
            CloseCurrent(); //close 
            if (r)
                refreshParent();
        }

        function refreshParent() {
            window.returnValue = 0;
            if (ISModalPage !== undefined && ISModalPage === false) {
                window.location.href = window.location.href;
            }
        }

        jQuery(function () {
            jQuery("#createEvent").on("hidden.bs.modal", function (e) {
                jQuery("#txtName").val("ex.Birthday Party");
                jQuery("#txtDetails").val("Add Event Detail");
                jQuery("#txtWhere").val("Add a Place?");
                jQuery("#chkAllDay").prop("checked", false);
                checkAllday();
                repeatChange();
                jQuery("#ddlEnd").val("2");
                endChange();
                jQuery("#selectAlert").val("1");
                jQuery("#txtTimes").val("1");
                onlymeClick();
                jQuery("#divPrivacy").hide();
                //关闭后的事件
            }).on("hide.bs.modal", function (e) {
                return true;  //关闭之间的事件
            });


            if (jQuery("#chkAllDay").prop("checked")) {
                checkAllday();
            }

            $('#<%=imgIcon.ClientID%>').on('click', clickIcon);
            $('#<%=chkAllDay.ClientID%>').on('click', checkAllday);
            $('#<%=txtFrom.ClientID%>').on('click', { 'isfrom': true }, popCalendar);
            $('#<%=txtTo.ClientID%>').on('click', popCalendar);
            jQuery("#tbDeleteEvent").css("display", "");
            jQuery("#tbInviteFriendView").css("display", "none");
            jQuery("input[name='radioEventIcon']").on('click', function () {
                if ($(this).prop('checked')) {
                    $('#<%=Icon.ClientID%>').val($(this).val());
                }
            });
            jQuery('#<%=txtFrom.ClientID%>,#<%= txtTo.ClientID%>').attr('readonly', 'readonly');
        });

        function popCalendar(isfrom) {
            if (isfrom) {
                popUpCalendar(this, this, "mm/dd/yyyy", 2, 1, checkDate);
            }
            else {
                popUpCalendar(this, this, "mm/dd/yyyy", 2, 1);
            }
        }

        function checkAllday() {
            if (jQuery("#<%=chkAllDay.ClientID%>").prop("checked")) {
                jQuery("#<%=txtFromTime.ClientID%>").hide();
                jQuery("#<%=selectFromType.ClientID%>").hide();
                jQuery("#<%=txtToTime.ClientID%>").hide();
                jQuery("#<%=selectToType.ClientID%>").hide();
            } else {
                jQuery("#<%=txtFromTime.ClientID%>").show();
                jQuery("#<%=selectFromType.ClientID%>").show();
                jQuery("#<%=txtToTime.ClientID%>").show();
                jQuery("#<%=selectToType.ClientID%>").show();
            }
        }

        function privacyClick() {
            jQuery("#divPrivacy").toggle();
        }

        function onlymeClick() {
            if (jQuery("#chkOnlyme").prop("checked")) {
                jQuery("input[name='chkPrivacy']").prop("checked", false);
                jQuery("#chkOnlyme").prop("checked", true)
                jQuery("#divPrivacyValue").text("Only me");
            }
            else {
                jQuery("#divPrivacyValue").text("");
            }
        }

        function publicClick() {
            if ($("#chkPublic").prop("checked")) {
                $("input[name='chkPrivacy']").prop("checked", false);
                $("#chkPublic").prop("checked", true)
                jQuery("#divPrivacyValue").text("All friends");
            }
            else
                jQuery("#divPrivacyValue").text("");
        }

        function groupsClick() {
            $("#chkOnlyme").prop("checked", false)
            $("#chkPublic").prop("checked", false)
            var tmpV = "";
            jQuery("input[name='chkPrivacy']:checked").each(function () {
                tmpV += jQuery(this).attr("text") + ", ";
            });
            if (tmpV == "")
                jQuery("#divPrivacyValue").text("All friends");
            else {
                jQuery("#divPrivacyValue").text(tmpV.substring(0, tmpV.length - 2));
            }
        }

        //confirm input
        $("body").on("keyup", "input:text[confirm],textarea[confirm]", function (e) {
            var element = $(this);
            var confirm = element.attr("confirm");
            if (element.val() == confirm && !(e.ctrlKey && e.which == 90)) {
                element.data("inputEqual", true);
            } else {
                element.data("inputEqual", false);
            }
        });
        $("body").on("focus", "input:text[confirm],textarea[confirm]", function (e) {
            var element = $(this);
            var confirm = element.attr("confirm");
            if (!element.data("inputEqual")) {
                if (element.hasClass("input-confirm") && element.val() != confirm) {
                    element.removeClass("input-confirm");
                }
                if (element.hasClass("input-confirm") || element.val() == element.attr("confirm")) {
                    element.removeClass("input-confirm").val("");
                }
            }
        });
        $("body").on("blur", "input:text[confirm],textarea[confirm]", function (e) {
            var element = $(this);
            if (element.val() == "" && !element.data("inputEqual")) {
                element.addClass("input-confirm").val(element.attr("confirm"));
            }
        });
        $("input:text[confirm],textarea[confirm]").each(function (e) {
            var element = $(this);
            var confirm = element.attr("confirm");
            if (!element.val().length || (element.val().length && element.val() == confirm)) {
                element.addClass("input-confirm").val(confirm);
                element.data("inputEqual", false);
            }
        });

    </script>
</asp:Content>
