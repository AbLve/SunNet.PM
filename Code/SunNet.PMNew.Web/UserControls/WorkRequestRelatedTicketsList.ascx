<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkRequestRelatedTicketsList.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.WorkRequestRelatedTicketsList" %>
<br />

<script type="text/javascript">
        // return value
        var projectId = <%=this.ProjectID %>;
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
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null)
                return unescape(r[2]);
            return null;
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
        function OpenAddRelaionDialog() {
            var result = ShowIFrame("/Sunnet/WorkRequest/AddRelation.aspx?wid=" + getUrlParam('id')+"&pid="+ projectId, 620, 560, true, "Add Relation");
            if (result == 0) {
                window.location.reload();
            }
        }
        
        function Delete() {
            var checkboxList = "";

            $("#mainBox input[type=checkbox]:checked").each(function() {
                var _thistd = $(this);
                checkboxList += _thistd.attr("id") + ",";
                _thistd.attr('checked','');
            });
            var wid = getUrlParam('id');
            //validate
            checkboxList = $.trim(checkboxList);
            if (checkboxList.length < 1) {
                ShowMessage("Please select a ticket to delete. ", 0, false, false);
                return false;
            }

            $.ajax({
                type: "post",
                url: "/Do/DoDeleteRelationWorkRequest.ashx?r=" + Math.random(),
                data: {
                    checkboxList: checkboxList,
                    wid: wid
                },
                success: function(result) {
                    ShowMessage2(result, 0, true, false);
                }
            });

        };
        jQuery(function(){
             jQuery('#relatedTickets tr td input:checkBox').on('click', function(event) {
                event.stopPropagation();
            }).closest('td').on('click', function(event) {
                var $checkBox = $(this).find('input[type=checkBox]');
                if ($checkBox.prop('checked')) {
                    $checkBox.prop('checked', false);
                }
                else {
                    $checkBox.prop('checked', true);
                }
                event.stopPropagation();
            });
            });
            

</script>

<div class="owmainactionBox">
    <div style="float: left; width: 500px; font-size: 12px; font-weight: bold">
        <a name="RelatedTickets">Related tickets</a> &nbsp;&nbsp; <a href="###" onclick="OpenAddRelaionDialog()">
            <img src="/icons/28.gif" />
            Add Related Tickets </a><a href="###" onclick="Delete(); return false;">
                <img src="/icons/17.gif" />
                Delete</a>
    </div>
</div>
<div class="owmainBox">
    <div class="onlistBox" style="padding-left: 200px">
        <img src="/icons/03.gif" align="absmiddle" />
        <span class="onlistText">Due Date is today</span>
        <img src="/icons/02.gif" align="absmiddle" />
        <span class="onlistText">Due day is 3 days before today</span>
        <img src="/icons/01.gif" align="absmiddle" />
        <span class="onlistText">Passed Due Date</span> <strong>Priority:</strong> The Smaller
        the value, the higher the priority.</div>
    <div class="mainactionBox">
        <div style="float: left">
            <asp:Label ID="lblProjectName" runat="server" Style="padding-left: 60px" Font-Size="11px"></asp:Label>
        </div>
        <div style="float: right">
            <asp:Label ID="lblHour" runat="server" Style="padding-right: 60px" Font-Size="15px"></asp:Label>
        </div>
    </div>
    <div class="mainrightBoxtwo" id="mainBox">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="owlistone"
            id="relatedTickets">
            <tr class="owlistTitle">
                <th style="width: 40px">
                    &nbsp;
                </th>
                <th style="width: 55px;" orderby="Priority">
                    Priority
                </th>
                <th orderby="TicketType, TicketID" style="width: 80px;">
                    Ticket Code
                </th>
                <th style="width: 30px">
                    &nbsp;
                </th>
                <th orderby="Title">
                    Title
                </th>
                <th style="width: 130px;" orderby="Status">
                    Status
                </th>
                <th style="width: 80px;" orderby="CreatedOn">
                    Created On
                </th>
                <th style="width: 80px;" orderby="ModifiedOn">
                    Updated
                </th>
                <th style="width: 80px;" orderby="CreatedBy">
                    Created By
                </th>
                <th style="width: 80px;" orderby="DeliveryDate">
                    Due Date
                </th>
            </tr>
            <asp:Repeater ID="rptTicketsList" runat="server" OnItemDataBound="rpt_DataBound">
                <ItemTemplate>
                    <tr opentype="newtab" dialogtitle="" href="/Sunnet/Tickets/TicketDetail.aspx?tid=<%#Eval("TicketID") %>"
                        class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                        <td>
                            <input id='<%# Eval("TicketID")%>' type="checkbox" />
                        </td>
                        <td>
                            <%# Eval("Priority")%>
                        </td>
                        <td>
                            <%# Eval("TicketCode").ToString()%>
                        </td>
                        <td>
                            <%# ShowPriorityImgByDevDate(Eval("DeliveryDate").ToString())%>
                        </td>
                        <td>
                            <%# Eval("Title").ToString()%>
                        </td>
                        <td>
                            <%# Eval("Status").ToString().Replace("_", " ") %>
                        </td>
                        <td>
                            <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}") == "01/01/1753" ? "" : Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%# Eval("CreatedOn", "{0:MM/dd/yyyy}") == "01/01/1753" ? "" : Eval("CreatedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <asp:Literal ID="ltlCreatedByID" runat="server" Visible="false" Text='<%#Eval("CreatedBy")%>'></asp:Literal>
                            <asp:Literal ID="ltlCreatedByName" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <%# Eval("DeliveryDate", "{0:MM/dd/yyyy}") == "01/01/1753" ? "" : Eval("DeliveryDate", "{0:MM/dd/yyyy}")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
</div>
