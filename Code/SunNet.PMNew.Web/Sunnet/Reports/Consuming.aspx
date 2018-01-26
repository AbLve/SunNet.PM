<%@ Page Title="Time Consuming Report" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="Consuming.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Reports.Consuming" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenDetailDialog(project, user) {
            var StartDate = '<%=StartDate.ToString("yyyy-MM-dd") %>';
            var EndDate = '<%=EndDate.ToString("yyyy-MM-dd")  %>';
            var url = "/Sunnet/Reports/ProjectUserTimeSheet.aspx?project=" + project + "&user=" + user + "&startdate=" + StartDate + "&enddate=" + EndDate;
            window.open(url);
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Time Consuming Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <th width="70">
                Project:
            </th>
            <td width="240">
                <asp:DropDownList ID="ddlProject" CssClass="select205" runat="server">
                </asp:DropDownList>
            </td>
            <th width="59">
                User:
            </th>
            <td width="240">
                <asp:DropDownList ID="ddlUsers" CssClass="select205" runat="server">
                </asp:DropDownList>
            </td>
            <td width="*">
            </td>
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
            <td>
                <asp:ImageButton ID="iBtnSearch" ToolTip="Search" ImageUrl="/images/search_btn.jpg"
                    runat="server" OnClick="iBtnSearch_Click" />&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="iBtnDownload"
                        ToolTip="DownLoad Report" ImageUrl="/images/ex.gif" runat="server" OnClick="iBtnDownload_Click" />
                <asp:HiddenField ID="hidOrderBy" runat="server" Value="All" />
                <asp:HiddenField ID="hidOrderDirection" runat="server" Value="ASC" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <asp:ImageButton ID="btnDetailView" ToolTip="DetailView" ImageUrl="~\Icons\08.gif"
            runat="server" OnClick="btnDetailView_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="btnHoursView" ToolTip="HoursView" ImageUrl="~\Images\report.png"
            runat="server" OnClick="btnHoursView_Click" />&nbsp;&nbsp;&nbsp;
    </div>
    <div class="mainrightBoxtwo">
        <asp:Repeater ID="rptDetailView" runat="server">
            <HeaderTemplate>
                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="listtwo">
                    <tr class="listsubTitle">
                        <th orderby="ProjectTitle" style="width: 30%;">
                            Project Title
                        </th>
                        <th orderby="FirstName" style="width: 20%;">
                            First Name
                        </th>
                        <th orderby="LastName" style="width: 20%;">
                            Last Name
                        </th>
                        <th orderby="Hours" style="width: 10%;">
                            Hours(H)
                        </th>
                        <th view="hidethis" style="width: 20%;">
                            Action
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                    <td>
                        <%#Eval("Title")%>
                    </td>
                    <td>
                        <%#Eval("FirstName")%>
                    </td>
                    <td>
                        <%#Eval("LastName")%>
                    </td>
                    <td>
                        <%#Eval("Hours")%>
                    </td>
                    <td view="hidethis">
                        <a title="view" href="javascript:void(0);" onclick="OpenDetailDialog(<%#Eval("ProjectID") %>,<%#Eval("UserID") %>)">
                            <img src="/icons/18.gif" alt="view" />
                        </a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptHoursView" runat="server">
            <HeaderTemplate>
                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="listtwo">
                    <tr class="listsubTitle">
                        <th orderby="ProjectTitle" style="width: 71%;">
                            Project Title
                        </th>
                        <th orderby="Hours" style="width: 29%;">
                            Hours(H)
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                    <td>
                        <%#Eval("Title")%>
                    </td>
                    <td>
                        <%#Eval("Hours")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
