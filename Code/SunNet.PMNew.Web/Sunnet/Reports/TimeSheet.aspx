<%@ Page Title="Timesheet Report" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="TimeSheet.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Reports.TimeSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function CancelSubmit(sheetdate, userid, username) {
            var msg = "The timesheets Date";
            msg = msg + "(" + sheetdate + ") User(" + username + ") ";
            msg = msg + " will be submitted to the pm, Proceed? ";
            var p = MessageBox.Confirm3(null, msg, "", "",
            function(result) {
                if (result || result == "true") {
                    jQuery.getJSON("/Do/Timesheet.ashx?r=" + Math.random(),
                                {
                                    type: "cancelsubmit",
                                    sheetdate: sheetdate,
                                    userid: userid
                                },
                                function(responseData) {
                                    MessageBox.Alert3(null, responseData.MessageContent, function() {
                                        if (responseData.Success == true || responseData.Success == "true") {
                                            document.getElementById("<%=iBtnSearch.ClientID %>").click();
                                        }
                                    });

                                }
                    );
                }
            }
            );

            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Timesheet Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <th width="70">
                        Keyword:
                    </th>
                    <td width="240">
                        <asp:TextBox ID="txtKeyword" CssClass="input200" runat="server"></asp:TextBox>
                    </td>
                    <th width="59">
                        Project:
                    </th>
                    <td width="240">
                        <asp:DropDownList ID="ddlProject" AutoPostBack="true" CssClass="select205" runat="server"
                            OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <th width="40">
                        Ticket:
                    </th>
                    <td width="240">
                        <asp:DropDownList ID="ddlTickets" CssClass="select205" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="*">
                    </td>
                </ContentTemplate>
            </asp:UpdatePanel>
        </tr>
        <tr>
            <th>
                Start Date:
            </th>
            <td>
                <asp:TextBox ID="txtStartDate" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                    Style="width: 175px;" CssClass="input200" runat="server"></asp:TextBox>
                <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtStartDate.ClientID %>"),document.getElementById("<%=txtStartDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                    align="absmiddle">
            </td>
            <th>
                End Date:
            </th>
            <td>
                <asp:TextBox ID="txtEndDate" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                    Style="width: 175px;" CssClass="input200" runat="server"></asp:TextBox>
                <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtEndDate.ClientID %>"),document.getElementById("<%=txtEndDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                    align="absmiddle">
            </td>
            <th>
                User:
            </th>
            <td>
                <asp:DropDownList ID="ddlUsers" CssClass="select205" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />&nbsp;&nbsp;
                <asp:ImageButton ID="iBtnDownload" ImageUrl="/images/ex.gif" runat="server" OnClick="iBtnDownload_Click" />
                <asp:HiddenField ID="hidOrderBy" runat="server" Value="SheetDate" />
                <asp:HiddenField ID="hidOrderDirection" runat="server" Value="DESC" />
            </td>
        </tr>
    </table>
    <div class="mainrightBoxtwo">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th orderby="SheetDate" style="width: 15%;">
                    SheetDate
                </th>
                <th orderby="ProjectTitle" style="width: 10%;">
                    Project Title
                </th>
                <th orderby="TicketTitle" style="width: 15%;">
                    Ticket Title
                </th>
                <th orderby="FirstName" style="width: 10%;">
                    UserName
                </th>
                <th orderby="Hours" style="width: 10%;">
                    Hours(H)
                </th>
                <th orderby="Percentage" style="width: 10%;">
                    Percentage(%)
                </th>
                <th orderby="ModifiedOn" style="width: 10%;">
                    Modified On
                </th>
                <th orderby="IsSubmitted" style="width: 10%;">
                    Submitted
                </th>
                <th style="width: 10%;">
                    Action
                </th>
            </tr>
            <asp:Repeater ID="rptTimesheet" runat="server">
                <ItemTemplate>
                    <tr class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                        <td>
                            <%#Eval("SheetDate","{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#Eval("ProjectTitle")%>
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
                            <%#Eval("Percentage")%>
                        </td>
                        <td>
                            <%#Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#Convert.ToBoolean(Eval("IsSubmitted").ToString())?"yes":"no"%>
                        </td>
                        <td>
                       
                            <a class=" <%#Convert.ToBoolean(Eval("IsSubmitted").ToString())?"":"hide"%>" sheetdate="<%#Eval("SheetDate","{0:MM/dd/yyyy}")%>" userid="<%#Eval("UserID") %>"
                                onclick="javascript:return CancelSubmit('<%#Eval("SheetDate","{0:yyyy-MM-dd}")%>','<%#Eval("UserID") %>',' <%#Eval("LastName") %>,<%#Eval("FirstName") %>');"
                                href="###" title="Cancel Submit" action="cancel">
                                <img alt="Cancel Submit" src="/icons/17.gif" />
                            </a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpTimesheet" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpTimesheet_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>
</asp:Content>
