<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sunnet/InputPop.Master"
    CodeBehind="AddEvent.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.AddEvent" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Create Event
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
                    <asp:Image runat="server" Width="24" Height="24" ID="imgIcon" align="absmiddle" src="/Images/EventIcon/event_icon_1s.png" />

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
                        <asp:ListItem Text="PM" Value="2" Selected="True"></asp:ListItem>
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
                        <asp:ListItem Text="PM" Value="2" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trRepeat">
                <td align="right">Repeat:
                </td>
                <td>
                    <asp:DropDownList CssClass="selectNormal" ID="selectRepeat" runat="server">
                        <asp:ListItem Text="None" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Every Day" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Every Week" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Every two weeks" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Every Month" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Every Year" Value="6"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trEnd" style="display: none;">
                <td align="right">End:
                </td>
                <td>
                    <asp:DropDownList ID="ddlEnd" CssClass="selectNormal selecteventl" Style="width: 120px;"
                        runat="server">
                        <asp:ListItem Text="After number of times" Value="1"></asp:ListItem>
                        <asp:ListItem Text="On date" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                    <span id="spanEnd1">
                        <asp:TextBox CssClass="inputNormal inpuTime" value="1" Style="width: 40px;" ID="txtTimes"
                            runat="server"></asp:TextBox>
                        Times </span><span id="spanEnd2" style="display: none;">
                            <asp:TextBox CssClass="inputNormal inpuDate" ID="txtEndDate" ReadOnly="true" runat="server"></asp:TextBox>
                        </span>
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
        <table width="100%" cellspacing="0" cellpadding="0" border="0" id="tbInviteFriendView" style="margin-top: 35px;">
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

        jQuery(function () {
            $('.eventiconitem input:radio').click(changeEventIcon);
            $('#<%=imgIcon.ClientID%>').on('click', showEventIcons);
            $('#<%=chkAllDay.ClientID%>').on('click', checkAllday);
            $('#<%=txtFrom.ClientID%>').on('click', { 'isfrom': true }, popCalendar);
            $('#<%=txtTo.ClientID%>').on('click', popCalendar);
            $('#<%=txtEndDate.ClientID%>').on('click', popCalendar);
            $('#<%=selectRepeat.ClientID%>').on('change', repeatChange);
            $('#<%=ddlEnd.ClientID%>').on('change', endChange);
            jQuery("input[name='radioEventIcon']").on('click', setCurrentEventIcon);
            disableDateInput();
            mouseLeaveEventIcons();
        });

        function mouseLeaveEventIcons() {
            var colseEventIcon = 0;
            jQuery("#divEventIcon").on("mouseleave", function () {
                colseEventIcon = setTimeout(function () { jQuery("#divEventIcon").hide(); }, 1000);
            }).on("mouseenter", function () {
                clearTimeout(colseEventIcon);
            });
        }

        function changeEventIcon() {
            var tmpIcon = jQuery(this).val();
            jQuery("#<%=imgIcon.ClientID%>").attr("src", "/Images/EventIcon/event_icon_" + tmpIcon + "s.png");
        }

        function showEventIcons() {
            jQuery("#divEventIcon").css("display", "");
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

        function popCalendar(isfrom) {
            if (isfrom) {
                popUpCalendar(this, this, "mm/dd/yyyy", 2, 1, checkDate);
            }
            else {
                popUpCalendar(this, this, "mm/dd/yyyy", 2, 1);
            }
        }

        function repeatChange() {
            if (jQuery('#<%=selectRepeat.ClientID%>').val() == "1") {
                jQuery("#trEnd").css("display", "none");
            } else {
                jQuery("#trEnd").css("display", "");
                endChange();
            }
        }

        function endChange() {
            if (jQuery("#<%=ddlEnd.ClientID%>").val() == "1") {
                jQuery("#spanEnd1").css("display", "");
                jQuery("#spanEnd2").css("display", "none");
            } else {
                jQuery("#spanEnd1").css("display", "none");
                jQuery("#spanEnd2").css("display", "");
            }
        }

        function setCurrentEventIcon() {
            if ($(this).prop('checked')) {
                $('#<%=Icon.ClientID%>').val($(this).val());
            }
        }

        function disableDateInput() {
            jQuery('#<%=txtFrom.ClientID%>,#<%= txtTo.ClientID%>').attr('readonly', 'readonly');
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
