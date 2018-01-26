<%@ Page Title="" Language="C#" MasterPageFile="~/Report/Report.Master" AutoEventWireup="true" CodeBehind="Ratings.aspx.cs" Inherits="SunNet.PMNew.PM2014.Report.Ratings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
         <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="70">Keyword:
                </td>
                <td>
                    <asp:TextBox ID="txtKeyword" placeholder="Enter project or ticket title"  queryparam="keyword" runat="server" CssClass="inputProfle1"></asp:TextBox>
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
                <td >
                     <asp:DropDownList ID="ddlTickets" queryparam="ticket" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td></td>

            </tr>
            <tr>
                <td>Start Date:
                </td>
                <td>
                    <asp:TextBox CssClass="inputdate inputProfle1" ID="txtStartDate" queryparam="startdate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
                </td>
                <td>End Date:
                </td>
                <td>
                    <asp:TextBox CssClass="inputdate inputProfle1" ID="txtEndDate" queryparam="enddate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
                </td>
                <td>Type:</td>
                <td>
                      <asp:DropDownList ID="ddlType" queryparam="type" CssClass="selectProfle1" runat="server">
                        <asp:ListItem Text="All" Value="All"> </asp:ListItem>
                        <asp:ListItem Text="Request" Value="Request"> </asp:ListItem>
                        <asp:ListItem Text="Bug" Value="Bug"> </asp:ListItem>
                        <asp:ListItem Text="Change" Value="Change"> </asp:ListItem>
                        <asp:ListItem Text="Risk" Value="Risk"> </asp:ListItem>
                        <asp:ListItem Text="Issue" Value="Issue"> </asp:ListItem>
                    </asp:DropDownList>
                </td>

                <td colspan="3">
                    <input type="button" class="searchBtn" id="btnSearch" style="margin-right: 10px;" />
                    <asp:Button ID="iBtnDownload" CssClass="otherBtn excelBtn" runat="server" Text="&nbsp;" ToolTip="Download Report" OnClick="iBtnDownload_Click" />

                </td>

            </tr>
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="*" class="order" default="true" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="150" class="order" orderby="TicketType">Type<span class="arrow"></span></th>
                <th width="100" >None Star<span class="arrow"></span></th>
                <th width="100" >1 Star<span class="arrow"></span></th>
                <th width="100" >2 Stars<span class="arrow"></span></th>
                <th width="100" >3 Stars<span class="arrow"></span></th>
                <th width="100" >4 Stars<span class="arrow"></span></th>
                <th width="100" >5 Stars<span class="arrow"></span></th>
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
                    <td>
                        <%#Eval("ProjectTitle")%>
                    </td>
                    <td>
                        <%# Eval("TicketType") %>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?star=0&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&type=<%# Eval("TicketType") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%#Eval("[4]")%></a>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?star=1&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&type=<%# Eval("TicketType") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%# Eval("[5]") %></a>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?star=2&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&type=<%# Eval("TicketType") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%# Eval("[6]") %></a>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?star=3&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&type=<%# Eval("TicketType") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%# Eval("[7]") %></a>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?star=4&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&type=<%# Eval("TicketType") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%# Eval("[8]") %></a>
                    </td>
                     <td>
                        <a href='/Report/SubAnalysis.aspx?star=5&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&type=<%# Eval("TicketType") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%# Eval("[9]") %></a>
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