<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Projects.Projects" %>
<%@ Import Namespace="SunNet.PMNew.Entity.ProjectModel" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="60px">Company:
                </td>
                <td width="">
                    <asp:DropDownList ID="ddlCompany" CssClass="select150" queryparam="company" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="60px">Keyword:
                </td>
                <td>
                    <asp:TextBox ID="txtKeyword" runat="server" queryparam="keywork" CssClass="input200" placeholder="Enter Title, Project Code"></asp:TextBox>
                </td>
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td>
                        <ul class="listtopBtn">
                            <li>
                                <div class="listtopBtn_icon">
                                    <img src="/Images/icons/newproject.png">
                                </div>
                                <div class="listtopBtn_text"><a href="AddProject.aspx?returnurl=<%=this.ReturnUrl %>">New Project</a></div>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="160px" class="order" orderby="ProjectCode">Project Code<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Description">Description<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="Priority">Priority<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="CreatedOn">Created On<span class="arrow"></span></th>
                <th width="100px" class="order  order-desc" default="true" orderby="ModifiedOn">Modified On<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="EndDate">End Date<span class="arrow"></span></th>
                <th width="110px"class="order" orderby="ModifiedOn">Modified By<span class="arrow"></span></th>
                <th width="70px" class="order" orderby="Status">Status<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="Billable">Over Free Hour<span class="arrow"></span></th>
                <th style="display: none;"></th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptProjects" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>
                        <%# Eval("ProjectCode")%>
                    </td>
                    <td>
                        <%# Eval("Title")%>
                    </td>
                    <td>
                        <%# Eval("Description") %>
                    </td>
                    <td>
                        <%#Eval("Priority").ToString().Substring(1)%>
                    </td>
                     <td>
                        <%#Eval("CreatedOn","{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%#Eval("ModifiedOn","{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%#Eval("EndDate","{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%#GetClientUserName(Convert.ToInt32(Eval("ModifiedBy").ToString()))%>
                    </td>
                    <td>
                        <%#Eval("Status").ToString().ToEnum<ProjectStatus>().ToText()%>
                    </td>
                    <td>
                        <%#Convert.ToBoolean(Eval("IsOverFreeTime").ToString()) ?
                                "<font color='red'><b>Yes</b></font>" : "No"%>
                    </td>
                   
                    <td class="action" style="display: none;">
                        <a class="saveBtn1 mainbutton" href="EditProject.aspx?ID=<%# Eval("ID")%>&returnurl=<%=this.ReturnUrl %>" style="display: none;"></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpWaitting" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>


