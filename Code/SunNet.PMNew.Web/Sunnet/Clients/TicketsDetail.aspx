<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketsDetail.aspx.cs"
    Inherits="SunNet.PMNew.Web.Sunnet.Clients.TicketsDetail" %>

<%@ Register Src="../../UserControls/FeedBacksList.ascx" TagName="FeedBacksList"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/RelationTicketsList.ascx" TagName="RelationTicketsList"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControls/AddTicket.ascx" TagName="AddTicket" TagPrefix="uc3" %>
<%@ Register Src="../../UserControls/TicketUsers.ascx" TagName="TicketUsers" TagPrefix="uc4" %>
<%@ Register Src="../../UserControls/HistoryList.ascx" TagName="HistoryList" TagPrefix="uc5" %>
<%@ Register Src="../../UserControls/ClientTicketBaseInfo.ascx" TagName="ClientTicketBaseInfo"
    TagPrefix="uc6" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ticket Detail</title>
    <link href="/Styles/openwindow.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/public.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/form.css" rel="stylesheet" type="text/css" />

    <script src="/do/js.ashx" type="text/javascript"></script>

    <style type="text/css">
        a:link {
            text-decoration: none;
        }

        #tbWorkflow {
            list-style-type: none;
            margin: 0px;
        }

            #tbWorkflow li {
                float: left;
                overflow-x: hidden;
                min-width: 110px;
            }
    </style>

    <script type="text/javascript">
        // return value
        var ReturnValueFromModalDialog = -1;
        var ISModalPage = false;
        var IsConfrim = false;

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
                window.opener = self;
                window.close();
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
            if (r != null) return unescape(r[2]); return null;
        }

        function OpenAddFeedBackDialog() {
            var result = ShowIFrame("/Sunnet/Clients/AddFeedBacks.aspx?tid=" + getUrlParam('tid'), 580, 350, true, "Add Note");
            if (result == 0)
                window.location.reload();
        }

        function OpenViewTicketDialog(selected) {
            var result = ShowIFrame("/Sunnet/Clients/ViewRelatedTicket.aspx?tid=" + selected + "&is0hsisdse=54156", 880, 660, true, "View Ticket");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenReplyFeedBackDialog(fid, ticketId) {
            var result = ShowIFrame("/Sunnet/Clients/AddFeedBacks.aspx?feedbackId=" + fid + "&tid=" + ticketId, 580, 400, true, "Reply FeedBack");
            if (result == 0) {
                window.location.reload();
            }
        }
        function OpenAddRelaionDialog() {
            var result = ShowIFrame("/Sunnet/Clients/AddRelation.aspx?tid=" + getUrlParam('tid'), 620, 560, true, "Add Relation");
            if (result == 0) {
                window.location.reload();
            }
        }

        function updateStatus(s) {
            var tid = getUrlParam('tid');
            $.ajax({
                type: "post",
                data: {
                    tid: tid,
                    statusValue: s
                },
                url: "/Do/DoUpdateTicketStatus.ashx?r=" + Math.random(),
                success: function (result) {
                    if (result == "same")
                        ShowMessage('Status Are Same With Before!', 0, false, false);
                    else
                        MessageBox.Alert3(null, result, function () {
                            window.location.reload();
                        });
                }
            });
        }


        function delRow(t) {
            var vTr = t.parent("td").parent("tr");
            vTr.remove();
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
                    ShowMessage(result, 0, true, false);
                }
            });
        }

        function StatusChangeConfirm(type) {
            if (type == 'deny') {
                var result = ShowIFrame("/Sunnet/Clients/DenyTicket.aspx?tid=" + getUrlParam('tid') + "&deny=1", 580, 350, true, "Add Reason");
                if (result == 0) {
                    window.location.reload();
                }
                return;
            }

            if (IsConfrim) {
                IsConfrim = false;
                return false;
            }
            if (type == 'app') {
                MessageBox.Confirm3(null, "Continue to approve the ticket?", '', '', ConfirmApprove);

            } else {
                MessageBox.Confirm3(null, "Status will be changed to Deny.", '', '', ConfirmDeny);
            }
        }

        function ConfirmApprove(e) {
            if (e == true) {
                IsConfrim = true;
                updateStatus('approve');
                return false;
            }
            else {
                return false;
            }
        }
        function ConfirmDeny(e) {
            if (e == true) {
                IsConfrim = true;
                updateStatus('deny');
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
                            Close</a>
                    </div>
                </div>
            </div>
            <div runat="server" id="divTopMenu" visible="false" class="owtopMenu">
                <div class="owtopMenu_left">
                    <ul>
                        <li class="sepline">
                            <img src="/images/owmenu_sep.gif" /></li>
                        <li><a href="#" onclick="StatusChangeConfirm('app');return false;">
                            <img src="/icons/20.gif" />
                            <span>Approve</span> </a></li>
                        <li class="sepline">
                            <img src="/images/owmenu_sep.gif" /></li>
                        <li><a href="#" onclick="StatusChangeConfirm('deny');return false;">
                            <img src="/icons/deny.gif" />
                            <span>Deny</span> </a></li>
                        <li class="sepline">
                            <img src="/images/owmenu_sep.gif" /></li>
                    </ul>
                </div>
            </div>
            <div runat="server" id="divTopMenuFill" visible="false" class="owtopMenu">
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="owmainBox_left">
                        <div style="width: 195px;">
                        </div>
                        <ul class="owleftmenu" style="position: fixed; left: 2px;">
                            <li class="currentleft"><a href="#Basic">Basic Information</a> </li>
                            <li><a href="#Feedback">Notes</a></li>
                            <li><a href="#Relations">Relations</a></li>
                            <li><a href="#Change History">Change History</a></li>
                        </ul>
                    </td>
                    <td class="owmainBox_right">
                        <div class="owmainrightBoxtwo">
                            <div>
                                <uc6:ClientTicketBaseInfo ID="ClientTicketBaseInfo1" runat="server" />
                                <uc3:AddTicket ID="AddTicket1" runat="server" />
                                <div class="btnBoxoneClient" id="divBtnSave" runat="server" visible="false">
                                    <input class="btnoneClient" type="button" value="Save" satus="save" action="update"
                                        name="button" />
                                </div>
                            </div>
                            <div>
                                <div class="owmainactionBox">
                                    <div style="float: left; display: inline-block; width: 100px; font-size: 12px; font-weight: bold">
                                        <a name="Feedback">Notes</a>
                                    </div>
                                </div>
                                <uc1:FeedBacksList ID="FeedBacksList1" runat="server" Title="Notes Detail" />
                            </div>
                            <div>
                                <div class="owmainactionBox">
                                    <div style="float: left; display: inline-block; width: 100px; font-size: 12px; font-weight: bold">
                                        <a name="Relations">Related tickets</a>
                                    </div>
                                    <div style="float: left;" runat="server" id="divAddRelation">
                                        <a href="#Relations" id="addRelation" onclick="OpenAddRelaionDialog()">
                                            <img src="/icons/28.gif" align="absmiddle" />
                                            Add Associated Tickets for Requirement Change</a>
                                    </div>
                                </div>
                                <uc2:RelationTicketsList ID="RelationTicketsList1" runat="server" />
                            </div>
                            <div>
                                <div class="owmainactionBox">
                                    <a name="Change History">Change History</a>
                                </div>
                                <uc5:HistoryList ID="HistoryList1" runat="server" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
