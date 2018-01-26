<%@ Page Language="C#"
    MasterPageFile="~/Report/Report.Master"
    AutoEventWireup="true" CodeBehind="Timesheet.aspx.cs" Inherits="SunNet.PMNew.PM2014.Report.Timesheet" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript">
        function CancelSubmit(sheetdate, userid, username) {
            var msg = "The timesheets Date";
            msg = msg + "(" + sheetdate + ") User(" + username + ") ";
            msg = msg + " will be cancelled, Proceed? ";
            var p = jQuery.confirm(msg, {
                yesText: "Yes",
                noText: "No",
                yesCallback: function () {
                    jQuery.getJSON("/service/Timesheet.ashx?r=" + Math.random(),
                        {
                            action: "cancelsubmit",
                            sheetdate: sheetdate,
                            userid: userid
                        },
                        function (responseData) {

                            if (responseData.success == true || responseData.success == "true") {
                                document.getElementById("btnSearch").click();
                            }
                        }
                    );

                },
                noCallback: function () { }
            });
            return false;
        }

        function checkContion() {
            if ($("#<%=txtStartDate.ClientID%>").val() == "") {
                alert("Please enter Start Date.");
                return false;
            }
            if ($("#<%=txtEndDate.ClientID%>").val() == "") {
                alert("Please enter End Date.");
                return false;
            }
            $("#<%=iBtncheck.ClientID%>").attr("disabled", "disabled");

            jQuery.getJSON("/service/CheckTimesheet.ashx?r=" + Math.random(),
                        {
                            startDate: $("#<%=txtStartDate.ClientID%>").val(),
                            endDate: $("#<%=txtEndDate.ClientID%>").val()
                        },
                        function (responseData) {
                            $("#<%=iBtncheck.ClientID%>").attr("disabled", false);

                            if (responseData.length > 0) {
                                var tmpList = "The following user(s) did not submit the timesheet:<br>";
                                for (var i = 0 ; i < responseData.length; i++) {
                                    if (i % 4 == 0)
                                        tmpList += "<br>";
                                    else
                                        tmpList += responseData[i].TimesheetDate + "&nbsp;&nbsp;:&nbsp;&nbsp;" + responseData[i].FirstName + " " + responseData[i].LastName + "&nbsp;&nbsp;;&nbsp; ";
                                }
                                tmpList += "<br><br>Are you sure you want to export the timesheet?";
                                jQuery.confirm(tmpList, {
                                    yesText: "Yes",
                                    noText: "No",
                                    yesCallback: function () {
                                        $("#<%=iBtnDownload.ClientID%>").click();
                                    },
                                    noCallback: function () { }
                                });
                            }
                            else {
                                $("#<%=iBtnDownload.ClientID%>").click();
                            }
                        }
                    );

                        return false;
                    }
    </script>

    <style type="text/css">
        .visible_btn {
            visibility: hidden;
            float:left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="70">Keyword:
                </td>
                <td>
                    <asp:TextBox ID="txtKeyword" placeholder="Enter project or ticket id  or ticket title" queryparam="keyword" runat="server" CssClass="inputProfle1"></asp:TextBox>
                </td>
                <td width="80">Company:
                </td>
                <td>
                    <asp:DropDownList ID="ddlCompanies" queryparam="Company" AutoPostBack="true" CssClass="selectProfle1" runat="server" OnSelectedIndexChanged="ddlCompanies_SelectedIndexChanged1">
                    </asp:DropDownList>
                </td>
                <td width="80">Project:
                </td>
                <td>
                    <asp:DropDownList ID="ddlProject" queryparam="project" AutoPostBack="true" CssClass="selectProfle1" runat="server"
                        OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td width="60">Ticket:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTickets" queryparam="ticket" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Start Date:
                </td>
                <td>
                    <asp:TextBox Enabled="true" CssClass="inputdate inputProfle1" ID="txtStartDate" queryparam="startdate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
                </td>
                <td>End Date:
                </td>
                <td>
                    <asp:TextBox CssClass="inputdate inputProfle1" ID="txtEndDate" queryparam="enddate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
                </td>
                <td>User:
                </td>
                <td>
                    <asp:DropDownList ID="ddlUsers" queryparam="user" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
                
                <td >Accounting:
                </td>
                <td colspan="6">
                    <asp:DropDownList ID="dpAccounting" queryparam="accounting" CssClass="rightItem" runat="server">
                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                        <asp:ListItem Text=" Proposal" Value="1"></asp:ListItem>
                        <asp:ListItem Text=" Time" Value="2"></asp:ListItem>
                        <asp:ListItem Text=" Not Billable" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td colspan="7">
                    </td>
                <td colspan="2">
                    <input type="button" class="searchBtn" id="btnSearch" style="margin-right: 10px;" />
                    <asp:Button ID="iBtncheck" CssClass="otherBtn excelBtn" runat="server" Text="&nbsp;" ToolTip="Download Report" OnClientClick="return checkContion();" />
                    <asp:Button ID="iBtnDownload" OnClick="iBtnDownload_Click" runat="server" CssClass="visible_btn "/>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="20"></th>
                <th width="90" class="order" orderby="SheetDate" default>Sheet Date<span class="arrow"></span></th>
                <th width="150" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="90" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="150" class="order" orderby="TicketTitle">Ticket Title<span class="arrow"></span></th>
                <th width="90" class="order" orderby="FirstName">User Name<span class="arrow"></span></th>
                <th width="70" class="order" orderby="Hours">Hours<span class="arrow"></span></th>

                <th width="100" class="order" orderby="ModifiedOn">Modified<span class="arrow"></span></th>
                <th width="90" class="order" orderby="IsSubmitted">Submitted<span class="arrow"></span></th>
                <th width="90" class="order" orderby="Accounting">Accounting<span class="arrow"></span></th>
                <th width="40" class="aligncenter">Action</th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptReportList" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                    <td class="aligncenter action">
                        <a href='###' timesheet='<%# Eval("TimeSheetID") %>' class='collapsed1'></a>
                    </td>
                    <td>
                        <%#Eval("SheetDate","{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%#Eval("ProjectTitle")%>
                    </td>
                    <td>
                        <%#Eval("TicketID")%>
                    </td>
                    <td>
                        <%#Eval("TicketTitle")%>
                    </td>
                    <td>
                        <%#GetClientUserName(Eval("UserID")) %>
                    </td>
                    <td>
                        <%#Eval("Hours")%>
                    </td>

                    <td>
                        <%#Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%#Convert.ToBoolean(Eval("IsSubmitted").ToString())?"Yes":"No"%>
                    </td>
                    <td>
                        <%#Convert.ToBoolean(Eval("Accounting").ToString()=="0")?" ":(Convert.ToBoolean(Eval("Accounting").ToString()=="Not_Billable")?"Not Billable":Eval("Accounting"))%>
                    </td>
                    <td>

                        <a class=" <%#Convert.ToBoolean(Eval("IsSubmitted").ToString())?"":"hide"%>" sheetdate="<%#Eval("SheetDate","{0:MM/dd/yyyy}")%>" userid="<%#Eval("UserID") %>"
                            onclick="javascript:return CancelSubmit('<%#Eval("SheetDate","{0:MM/dd/yyyy}")%>','<%#Eval("UserID") %>',' <%#Eval("LastName") %>,<%#Eval("FirstName") %>');"
                            href="###" title="Cancel Submit" action="cancel">
                            <img alt="Cancel Submit" src="/Images/icons/deny.png" />
                        </a>
                    </td>
                </tr>
                <tr class="sublist" id='Timesheet<%# Eval("TimeSheetID") %>' style="display: none">
                    <td colspan="9">
                        <div class="subcontentBox">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <td style="width: 20px">&nbsp;</td>
                                        <td width="200" valign="top"><strong>Ticket Description:</strong></td>
                                        <td><%# Server.HtmlEncode(Eval("TicketDescription").ToString())%></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20px">&nbsp;</td>
                                        <td width="200" valign="top"><strong>Work Detail:</strong></td>
                                        <td><%#Eval("WorkDetail")%></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>

                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="ReportPage" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
