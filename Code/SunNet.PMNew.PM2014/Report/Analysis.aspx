<%@ Page Title="" Language="C#" MasterPageFile="~/Report/Report.Master" AutoEventWireup="true" CodeBehind="Analysis.aspx.cs" Inherits="SunNet.PMNew.PM2014.Report.Analysis" %>

<%@ Import Namespace="SunNet.PMNew.Entity.UserModel" %>
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
                    <asp:DropDownList  ID="ddlProject" queryparam="project" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="60">Source:
                </td>
                <td>
                    <asp:DropDownList ID="ddlSource" queryparam="source" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
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
                <th width="*" class="order order-asc" default="true" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="150"  >Source<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Bug">Bug<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Request">Request<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Issue">Issue<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Change">Change<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Risk">Risk<span class="arrow"></span></th>

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
                        <%# GetEnumName(typeof(RolesEnum),Eval("Source")) %>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?type=bug&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&source=<%# Eval("Source") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%#Eval("Bug")%></a>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?type=request&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&source=<%# Eval("Source") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%# Eval("Request") %></a>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?type=issue&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&source=<%# Eval("Source") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%# Eval("Issue") %></a>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?type=change&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&source=<%# Eval("Source") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%# Eval("Change") %></a>
                    </td>
                    <td>
                        <a href='/Report/SubAnalysis.aspx?type=risk&keyword=<%# QS("keyword") %>&startdate=<%# QS("startdate") %>&enddate=<%# QS("enddate") %>&source=<%# Eval("Source") %>&project=<%# Eval("ProjectId") %>&returnurl=<%# ReturnUrl %>'><%# Eval("Risk") %></a>
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
