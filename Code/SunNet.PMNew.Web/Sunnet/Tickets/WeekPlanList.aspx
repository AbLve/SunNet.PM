<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="WeekPlanList.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.WeekPlanList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .weekplanTable
        {
            border: 1px solid #99bbe8;
            margin: 10px auto 0;
        }
        .weekplanTable td
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding: 4px 5px;
            border-bottom: 1px dotted #ccc;
            border-right: 1px dotted #ccc;
            color: #333;
        }
        tr.weekplanTitle td
        {
            background-image: url(/images/weekplan_topbg.gif);
            border-bottom: 1px solid #99bbe8;
            font-weight: bold;
            color: #00285d;
        }
        tr.weekplanw td
        {
            background-image: url(/images/weekplan_wbg.gif);
            color: #222;
            text-align: center;
            border-bottom: 1px solid #ccc;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    List Week Plan
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="59">
                User:
            </td>
            <td width="260">
                <asp:DropDownList ID="ddlUsers" runat="server" CssClass="select205" Width="150">
                </asp:DropDownList>
            </td>
            <td width="59">
                Start Date:
            </td>
            <td width="260">
                <asp:TextBox ID="txtStartDate" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                    Style="width: 175px;" CssClass="input200" runat="server"></asp:TextBox>
                <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtStartDate.ClientID %>"),document.getElementById("<%=txtStartDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                    align="absmiddle">
            </td>
            <td width="59">
                End Date:
            </td>
            <td width="260">
                <asp:TextBox ID="txtEndDate" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                    Style="width: 175px;" CssClass="input200" runat="server"></asp:TextBox>
                <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtEndDate.ClientID %>"),document.getElementById("<%=txtEndDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                    align="absmiddle">
            </td>
            <td>
                <asp:ImageButton ID="iBtnSearch" runat="server" align="absmiddle" ImageUrl="/images/search_btn.jpg"
                    OnClick="iBtnSearch_Click" Width="20px" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <span><a href="#" onclick="OpenAddModuleDialog()">
            <img src="/icons/14.gif" border="0" align="absmiddle" alt="new/add" />
            Add Week Plan</a></span>
    </div>
    <div class="mainrightBoxtwo">
        <label id="lblNoResult" runat="server" visible="false">
        </label>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="weekplanTable">
                    <tr class="weekplanTitle">
                        <td colspan="6">
                            <b>
                                <%# Eval("FirstName")%></b> &nbsp;&nbsp;&nbsp;&nbsp; (<%# Eval("WeekDay","{0:MM/dd/yyyy}")%>
                            ----
                            <%# Eval("WeekDayEnd", "{0:MM/dd/yyyy}")%>) &nbsp;&nbsp;&nbsp;&nbsp;<b>Modified by:
                            </b>
                            <%# Eval("EditFirstName", "{0:MM/dd/yyyy}")%>
                            &nbsp;&nbsp;&nbsp;&nbsp;<b> Modified on:</b>
                            <%# Eval("UpdateDate", "{0:MM/dd/yyyy}")%>
                        </td>
                    </tr>
                    <tr class="weekplanw">
                        <%#string.IsNullOrEmpty(Eval("Sunday").ToString()) ? "" : "<td>Sunday</td>"%>
                        <td width="19%">
                            Monday
                        </td>
                        <td width="19%">
                            Tuesday
                        </td>
                        <td width="19%">
                            Wednesday
                        </td>
                        <td width="19%">
                            Thursday
                        </td>
                        <%#string.IsNullOrEmpty(Eval("Sunday").ToString()) ? "<td>Friday</td>" : ""%>
                        <td width="5%">
                            Action
                        </td>
                    </tr>
                    <tr>
                        <%#Eval("SundayColumn")%>
                        <td>
                            <%# Eval("Monday")%>
                            <br />
                            Estimate:
                            <%#Eval("MondayEstimate")%>
                            H
                        </td>
                        <td>
                            <%# Eval("Tuesday")%>
                            <br />
                            Estimate:
                            <%#Eval("TuesdayEstimate")%>
                            H
                        </td>
                        <td>
                            <%# Eval("Wednesday")%>
                            <br />
                            Estimate:
                            <%#Eval("WednesdayEstimate")%>
                            H
                        </td>
                        <td>
                            <%# Eval("Thursday")%>
                            <br />
                            Estimate:
                            <%#Eval("ThursdayEstimate")%>
                            H
                        </td>
  
                        <%#Eval("FridayColumnInFirstRow")%>
                        <td rowspan="3">
                            <%# ShowEdit(Eval("ID"), Eval("UserID"))%>
                        </td>
                    </tr>
                    <tr>
                        <%#Eval("WeekenDayHTML")%>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="aspNetPager1" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" runat="server"
                AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="aspNetPager1_PageChanged"
                PageSize="20">
            </webdiyer:AspNetPager>
        </div>
    </div>

    <script src="../../Scripts/addtocategory.js" type="text/javascript"></script>

    <script type="text/javascript">

        function OpenAddModuleDialog() {
            var result = ShowIFrame("/Sunnet/Tickets/WeekPlanEdit.aspx", 880, 740, true, "Add Week Plan");
            if (!result) {
                window.location.reload();
            }
        }

        function OpenPlanDetail(id) {
            var result = ShowIFrame("/Sunnet/Tickets/WeekPlanEdit.aspx?id=" + id, 880, 740, true, "Edit Week Plan");
            if (result == 0) {
                window.location.reload();
            }
        }
       
    </script>

</asp:Content>
