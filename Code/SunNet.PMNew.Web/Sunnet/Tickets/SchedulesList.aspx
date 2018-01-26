<%@ Page Title="Schedule List View" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="SchedulesList.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.SchedulesList" %>

<%@ Register Src="../../UserControls/OpenICalendar.ascx" TagName="OpenICalendar"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Related A
        {
            display: block;
            float: left;
            margin-right: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Schedule List View
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="300">
                Keyword:
                <asp:TextBox ID="txtKeyword"  CssClass="input200"
                    runat="server"></asp:TextBox>
            </td>
            <td width="220">
                Status:
                <asp:DropDownList ID="ddlStatus" CssClass="select205" runat="server" Width="140">
                </asp:DropDownList>
            </td>
            <td width="200">
                Users:
                <asp:DropDownList ID="ddlUsers" CssClass="select150" runat="server">
                </asp:DropDownList>
            </td>
            <td width="*">
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <div class="mainactionBox_left">
            <span class="cld">
                <img src="/icons/10.gif" border="0" align="absmiddle" />
                List View</span> <span><a href="SchedulesYearly.aspx">
                    <img src="/icons/11.gif" border="0" align="absmiddle" />
                    Yearly View</a></span><span> <a href="SchedulesMonthly.aspx">
                        <img src="/icons/31.gif" border="0" align="absmiddle" />
                        Monthly View</a></span>
            <uc1:OpenICalendar ID="OpenICalendar1" runat="server" />
        </div>
        <div class="mainactionBox_right">
        </div>
    </div>
    <div class="mainrightBoxtwo">
        <table id="dataTickets" width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th width="8">
                    &nbsp;
                </th>
                <th width="90">
                    Project
                </th>
                <th width="70">
                    Ticket Code
                </th>
                <th width="*">
                    Title
                </th>
                <th width="140">
                    Status
                </th>
                <th width="60">
                    Due Date
                </th>
                <th width="40">
                    Action
                </th>
                <th width="100">
                    Related Tickets
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="8" style="color: Red;">
                    &nbsp; No records
                </th>
            </tr>
            <asp:Repeater ID="rptTickets" runat="server">
                <ItemTemplate>
                    <tr opentype="newtab" dialogtitle="" href="/Sunnet/Tickets/TicketDetail.aspx?tid=<%#Eval("ID") %>"
                        type="tickets" class="<%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>"
                        id='<%#Eval("ID") %>'>
                        <td class="action" type="showtasks" ticketid='<%#Eval("ID") %>' style="cursor: pointer;"
                            onclick="AddTasks(this);return false;">
                            <img src="/icons/04.gif">
                        </td>
                        <td>
                            <%#Eval("ProjectTitle")%>
                        </td>
                        <td>
                            <%#Eval("TicketCode")%>
                        </td>
                        <td>
                            <%#Eval("Title") %>
                        </td>
                        <td>
                            <%#Enum.Parse(typeof(SunNet.PMNew.Entity.TicketModel.TicketsState),Eval("Status").ToString()).ToString().Replace("_"," ")%>
                        </td>
                        <td>
                            <%#Eval("DeliveryDate", "{0:MM/dd/yyyy}").ToString() == "01/01/1753" 
                                ? "" : Eval("DeliveryDate", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td class="action">
                            <a href="###" title="Add To Category " action="addtocategory" ticketid="<%#Eval("ID") %>"
                                onclick="chooseCategoryDiv(event,this);return false;">
                                <img src="/icons/12.gif" alt="Add To Category " /></a>
                        </td>
                        <td class="Related">
                            <%#GetRelatedTickets(Eval("ID"),Eval("ProjectID")) %>
                        </td>
                    </tr>
                    <tr type="tasks" style="display: none;">
                        <td colspan="9" class="listrownblank">
                            <table id='tasks<%#Eval("ID") %>' width="100%" border="0" cellpadding="0" cellspacing="0"
                                class="subList">
                                <tr class="listrowfour">
                                    <td width="100%">
                                        Tasks<%--&nbsp;<a href="###"><img src="/icons/ico_refresh.gif" /></a>--%>
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
            <webdiyer:AspNetPager ID="anpTickets" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpTickets_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>

    <script src="../../Scripts/addtocategory.js" type="text/javascript"></script>

    <script type="text/javascript">

        function OpenTicketDetail(tid) {
            window.open("/Sunnet/Tickets/TicketDetail.aspx?tid=" + tid);
        }

        function ViewTicketModuleDialog(selectTicketId) {
            var result = ShowIFrame("/Sunnet/Tickets/ViewRelatedTicket.aspx?tid=" + selectTicketId, 870, 620, true, "View Related Ticket");
            if (result == 0) {
                window.href = window.href;
            }
        }

        var TasksLoaded = [];

        var GetDataTimeOut = 0;
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

        var trtemplate = "<tr class='{ClassName}'><td>{Title}<font color='red'>{Complete}</font>>{Description}</td></tr>";
        function GetObjsToHtml(objs) {
            if (objs == undefined || objs == null || objs.length == 0) {
                var classname = GetClassName(0);
                var thisHtml = trtemplate.replace("{ClassName}", classname)
                                        .replace("{Title}", "NoRecords")
                                        .replace("{Description}", "0 task")
                                        .replace("{Complete}", "");
                return thisHtml;
            }
            var html = "";
            for (var i = 0; i < objs.length; i++) {
                var classname = GetClassName(i);
                var thisHtml = trtemplate.replace("{ClassName}", classname)
                                        .replace("{Title}", objs[i].Title)
                                        .replace("{Description}", objs[i].Description)
                                        .replace("{Complete}", objs[i].IsCompleted == true ? "[Completed]" : "");
                html = html + thisHtml;
            }
            return html;
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
                            function(taskList) {
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
                                                                  .replace("{TaskID}", '')
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
    </script>

</asp:Content>
