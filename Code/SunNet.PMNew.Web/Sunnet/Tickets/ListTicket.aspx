<%@ Page Title="All Tickets" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="ListTicket.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.ListTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Related A {
            display: block;
            float: left;
            margin-right: 3px;
        }

        .searchContentBox {
            margin: 5px 15px;
            overflow: hidden;
            height: auto;
        }

        .searchrowitem {
            margin-left: 10px;
            margin-top: 5px;
            white-space: nowrap;
            float: left;
            width: 330px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    All Tickets List
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="searchContentBox">
        <div class="searchrowitem">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td width="100">Keyword:
                        </td>
                        <td width="230">
                            <asp:TextBox ID="txtKeyWord" runat="server" CssClass="input200"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="searchrowitem">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td width="100">Status:
                        </td>
                        <td width="230">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select205">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="searchrowitem">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td width="100">Type:
                        </td>
                        <td width="230">
                            <asp:DropDownList ID="ddlTicketType" runat="server" CssClass="select205">
                                <asp:ListItem Value="">Please select...</asp:ListItem>
                                <asp:ListItem Value="Bug">Bug</asp:ListItem>
                                <asp:ListItem Value="Request">Request</asp:ListItem>
                                <asp:ListItem Value="Risk">Risk</asp:ListItem>
                                <asp:ListItem Value="Issue">Issue</asp:ListItem>
                                <asp:ListItem Value="Change">Change</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="searchrowitem">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td width="100">Project:
                        </td>
                        <td width="230">
                            <asp:DropDownList ID="ddlProject" runat="server" CssClass="select205" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="searchrowitem">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td width="100">Assigned User:
                        </td>
                        <td width="230">
                            <asp:DropDownList ID="ddlAssUser" runat="server" CssClass="select205">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="searchrowitem" runat="server" id="dvCompany">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td width="100">Company:
                        </td>
                        <td width="230">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="select205">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="searchrowitem">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td width="100">Priority:
                        </td>
                        <td width="230">
                            <asp:DropDownList ID="ddlClientPriority" runat="server" CssClass="select205">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="searchrowitem" style="width: 30px;">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td>
                            <asp:ImageButton ID="iBtnSearch" runat="server" align="absmiddle" ImageUrl="/images/search_btn.jpg"
                                OnClick="SearchImgBtn_Click" Width="20px" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="mainactionBox" runat="server" id="AddNewTicket" visible="false">
        <span><a href="#" onclick="OpenAddModuleDialog()">
            <img src="/icons/14.gif" border="0" align="absmiddle" alt="new/add" />
            Add Internal Ticket</a></span>
    </div>
    <div class="mainrightBoxtwo">
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="ModifiedOn" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="DESC" />
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th style="width: 8px">&nbsp;
                </th>
                <th style="width: 120px;" orderby="ProjectTitle">Project
                </th>
                <th orderby="TicketID" style="width: 90px;">Ticket Code
                </th>
                <th orderby="Title">Title
                </th>
                <th style="width: 60px;" orderby="Priority">Priority
                </th>
                <th style="width: 115px;" orderby="Status">Status
                </th>
                <th style="width: 65px;" orderby="ModifiedOn">Updated
                </th>
                <th style="width: 60px;">Is Internal
                </th>
                <th style="width: 40px;">Action
                </th>
                <th style="width: 90px;">Related Tickets
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="10" style="color: Red;">&nbsp; No records
                </th>
            </tr>
            <asp:Repeater ID="rptTicketsList" runat="server">
                <ItemTemplate>
                    <tr opentype="newtab" dialogtitle="" href="/Sunnet/Tickets/TicketDetail.aspx?tid=<%#Eval("TicketID") %>"
                        class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                        <td class="action" onclick="AddTasks(this);return false;" type="showtasks" ticketid='<%#Eval("ID") %>'
                            style="cursor: pointer;">
                            <img src="/icons/04.gif" alt='task'>
                        </td>
                        <td>
                            <%# Eval("ProjectTitle").ToString()%>
                        </td>
                        <td>
                            <%# Eval("TicketCode").ToString()%>
                        </td>
                        <td>
                            <%# Eval("Title").ToString()%>
                        </td>
                        <td>
                            <%# Eval("Priority")%>
                        </td>
                        <td <%# ShowActionByFbMsg(FeedBackMessage(Eval("TicketID"))) %>>
                            <%# ChangeStatus(Eval("Status"), (int)Eval("TicketID"))%>
                            <%# FeedBackMessage(Eval("TicketID"))%>
                        </td>
                        <td>
                            <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%# Eval("IsInternal").ToString().ToLower() == "true" ? "Yes" : "No"%>
                        </td>
                        <td class="action">
                            <a href="###" title="Add To Category " action="addtocategory" ticketid="<%#Eval("TicketID") %>"
                                onclick="chooseCategoryDiv(event,this);return false;">
                                <img src="/icons/12.gif" alt="Add To Category " /></a> <a href="###" title="add this ticket to Collect Tickets package"
                                    action="addtocategory" ticketid="<%#Eval("TicketID") %>" onclick="CollecteTicket(<%#Eval("TicketID") %>);return false;">
                                    <img src="/icons/24.gif" alt="Collecte this ticket" /></a>
                        </td>
                        <td style="width: 90px;" class="action Related">
                            <%# ShowRelatedByTid(Eval("TicketID").ToString())%>
                        </td>
                    </tr>
                    <tr type="tasks" style="display: none;">
                        <td colspan="13" class="listrownblank">
                            <table id='tasks<%#Eval("TicketID") %>' width="100%" border="0" cellpadding="0" cellspacing="0"
                                class="subList">
                                <tr class="listrowfour">
                                    <td width="100%">Tasks
                                    </td>
                                </tr>
                                <tr class="listrowsix load">
                                    <td>
                                        <img alt="loading" src="/Images/loading16_blue.gif" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpUsers" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" runat="server"
                AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpUsers_PageChanged"
                PageSize="20">
            </webdiyer:AspNetPager>
        </div>
    </div>

    <script src="../../Scripts/addtocategory.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#<%=txtKeyWord.ClientID%>").focus();
            showCount("leftMenu10");
        });

        var TasksLoaded = [];

        function SetTasksLoaded(ticketid, loaded) {
            eval("TasksLoaded.ticket" + ticketid + "=" + loaded.toString() + ";");
        }

        function GetTasksLoaded(ticketid) {
            var result = eval("TasksLoaded.ticket" + ticketid);
            if (result == undefined || result == null) {
                return false;
            }
            return result;
        }

        function GetClassName(index) {
            if (index % 2 == 0) {
                return "listrowsix";
            }
            return "listrowfive";
        }

        function AddTasks(o) {
            var _this = jQuery(o);
            _this.parent().toggleClass("listrowthree").next().slideToggle(100);
            var _ticketid = _this.attr("ticketid");
            var _loaded = GetTasksLoaded(_ticketid);
            if (_loaded == false) {
                var trtemplate = "<tr class='{ClassName}'><td>{Title}<font color='red'>{Complete}</font>{Description}</td></tr>";
                jQuery.getJSON(
                            "/Do/DoGetTaskList.ashx?r=" + Math.random(),
                            {
                                type: "GetTasksByTicket",
                                ticketid: _ticketid
                            },
                            function (taskList) {
                                var html = "";
                                if (taskList.length > 0) {
                                    for (var i = 0; i < taskList.length; i++) {

                                        var classname = GetClassName(i);
                                        var thisHtml = trtemplate.replace("{ClassName}", classname)
                                                            .replace("{Title}", taskList[i].Title + '--->')
                                                            .replace("{Description}", taskList[i].Description)
                                                            .replace("{Complete}", taskList[i].IsCompleted == true ? "[Completed]" : "");
                                        html = html + thisHtml;

                                    }
                                } else {
                                    var classname = GetClassName(1);
                                    var thisHtml = trtemplate.replace("{ClassName}", classname)
                                                                .replace("{Title}", "<span style='color:red;'> &nbsp; No records!</span>")
                                                                .replace("{Description}", "")
                                                                .replace("{Complete}", "");
                                    html = html + thisHtml;
                                }
                                jQuery("#tasks" + _ticketid + " tr").remove(".load");
                                jQuery("#tasks" + _ticketid).append(html);
                                SetTasksLoaded(_ticketid, true);
                            }
                  );
            }
        }


        function ViewTicketModuleDialog(selectTicketId) {
            ShowIFrame("/Sunnet/Tickets/ViewRelatedTicket.aspx?tid=" + selectTicketId + "&is0hsisdse=54156", 870, 620, true, "View Related Ticket");
        }

        function OpenEditModuleDialog(selectTicketId) {
            ShowIFrame("/Sunnet/Tickets/EditTicket.aspx?tid=" + selectTicketId, 878, 645, true, "Edit Ticket");
        }
        function OpenAddModuleDialog() {
            var result = ShowIFrame("/Sunnet/Tickets/AddTicket.aspx", 880, 740, true, "Add Internal Ticket");
            if (!result) {
                window.location.reload();
            }
        }
        function OpenReplyFeedBackDialog(fid, tid) {
            var result = ShowIFrame("/Sunnet/Tickets/AddFeedBacks.aspx?feedbackId=" + fid + "&rtype=r" + "&tid=" + tid, 580, 430, true, "Reply FeedBack");
            if (result == 0) {
                $("#feedback" + tid).remove();
            }
        }
        function OpenTicketDetail(tid, type) {
            if (type == 'f') {
                window.open("/Sunnet/Tickets/TicketDetail.aspx?tid=" + tid + "#Feedback");
            } else {
                window.open("/Sunnet/Tickets/TicketDetail.aspx?tid=" + tid);
            }
        }

    </script>

</asp:Content>
