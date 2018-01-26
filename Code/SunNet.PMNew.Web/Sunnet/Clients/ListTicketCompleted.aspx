<%@ Page Title="Completed Tickets" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="ListTicketCompleted.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.ListTicketCompleted" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%=txtKeyWord.ClientID%>").focus();
        });
        function ViewTicketModuleDialog(selectTicketId) {
            var result = ShowIFrame("/Sunnet/Clients/ViewRelatedTicket.aspx?tid=" + selectTicketId + "&is0hsisdse=54156", 870, 620, true, "View Related Ticket");
        }
    </script>

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
    Completed Tickets
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="50">
                Keyword:
            </td>
            <td width="260">
                <asp:TextBox ID="txtKeyWord" runat="server" CssClass="input200"></asp:TextBox>
            </td>
            <td width="40">
                Status:
            </td>
            <td width="160">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select100">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Project:
            </td>
            <td>
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="select205">
                </asp:DropDownList>
            </td>
            <td width="30">
                Type:
            </td>
            <td>
                <asp:DropDownList ID="ddlTicketType" runat="server" CssClass="select100">
                  <asp:ListItem Value="-1">Please select...</asp:ListItem>
                    <asp:ListItem Value="0">Bug</asp:ListItem>
                    <asp:ListItem Value="1">Request</asp:ListItem>
                    <asp:ListItem Value="2">Risk</asp:ListItem>
                    <asp:ListItem Value="3">Issue</asp:ListItem>
                    <asp:ListItem Value="4">Change</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:ImageButton ID="iBtnSearch" runat="server" align="absmiddle" ImageUrl="/images/search_btn.jpg"
                    OnClick="SearchImgBtn_Click" Width="20px" />
            </td>
        </tr>
    </table>
    <div class="mainrightBoxtwo">
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="ModifiedOn" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="desc" />
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th style="width: 85px;" orderby="ProjectTitle">
                    Project
                </th>
                <th orderby="TicketID" style="width: 90px;">
                    Ticket Code
                </th>
                <th orderby="Title">
                    Title
                </th>
                <th style="width: 65px;" orderby="Priority">
                    Priority
                </th>
                <th style="width: 120px;" orderby="Status">
                    Status
                </th>
                <th style="width: 65px;" orderby="CreatedOn">
                    Created
                </th>
                <th style="width: 67px;" orderby="ModifiedOn">
                    Updated
                </th>
                <th style="width: 65px;">
                    Created By
                </th>
                <th style="width: 90px;">
                    Related Tickets
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="10" style="color: Red;">
                    &nbsp; No records
                </th>
            </tr>
            <asp:Repeater ID="rptTicketsList" runat="server">
                <ItemTemplate>
                    <tr opentype="newtab" dialogtitle="" href="/Sunnet/Clients/TicketsDetail.aspx?is0hsisdse=54156&tid=<%#Eval("TicketID") %>"
                        class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                        <td>
                            <%# Eval("ProjectTitle").ToString()%>
                        </td>
                        <td>
                            <%#Eval("TicketCode").ToString()%>
                        </td>
                        <td>
                            <%# Eval("Title").ToString()%>
                        </td>
                        <td>
                            <%# Eval("Priority")%>
                        </td>
                        <td>
                            <%# Eval("Status")%>
                        </td>
                        <td>
                            <%# Eval("CreatedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#GetClientUserName(Convert.ToInt32(Eval("CreatedBy"))) %>
                        </td>
                        <td style="width: 90px;" class="action Related">
                            <%# ShowRelatedByTid(Eval("TicketID").ToString())%>
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
</asp:Content>
