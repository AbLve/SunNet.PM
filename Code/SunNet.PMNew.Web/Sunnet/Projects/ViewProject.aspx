<%@ Page Title="Project Info" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="ViewProject.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Projects.ViewProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selecteitems
        {
        }
        .selecteitems li
        {
            width: 140px;
            height: 16px;
            margin-right: 5px;
            padding-top: 2px;
            padding-left: 5px;
            margin-bottom: 5px;
            cursor: pointer;
            float: left;
            list-style: none;
            border: solid 1px #BAD8F0;
            overflow: hidden;
        }
        .selecteitems li.selected
        {
            background: url('/Icons/29.gif') no-repeat right center;
        }
        .selecteitems li.plus
        {
            background: url('/Images/plus.png') no-repeat right center;
        }
        .selecteitems li.minus
        {
            background: url('/Images/minus.png') no-repeat right center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    View Project
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owToptwo">
        Basic Information</div>
    <div class="owmainBox">
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th width="100">
                    Project Title:
                </th>
                <td width="250">
                    <asp:TextBox ID="txtTitle" Enabled="false" Validation="true" length="1-128" CssClass="input200"
                        runat="server"></asp:TextBox>
                </td>
                <th width="100">
                    Project Code:
                </th>
                <td>
                    <span class="prority">
                        <asp:TextBox ID="txtProjectCode" Enabled="false" CssClass="input200" Validation="true"
                            length="1-64" runat="server"></asp:TextBox>
                        <asp:CheckBox ID="chkBillable" Enabled="false" Text="Billable" runat="server" /></span>
                </td>
            </tr>
            <tr>
                <th>
                    Company:
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlCompany" Enabled="false" CssClass="select205" runat="server">
                    </asp:DropDownList>
                </td>
                <th class="prority">
                    Status:
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlStatus" Enabled="false" CssClass="select205" runat="server">
                        <asp:ListItem Text="Open" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Scheduled" Value="2"></asp:ListItem>
                        <asp:ListItem Text="In Process" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Cancelled" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Project Manager:
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlPM" Enabled="false" CssClass="select205" runat="server">
                    </asp:DropDownList>
                </td>
                <th class="prority">
                    Priority:
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlPriority" Enabled="false" CssClass="select205" runat="server">
                        <asp:ListItem Text="High" Value="1HIGH"></asp:ListItem>
                        <asp:ListItem Text="Medium" Value="2MEDIUM"></asp:ListItem>
                        <asp:ListItem Text="Low" Value="3LOW"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Start Date:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox Enabled="false" ID="txtStartDate" Validation="true" length="8-20" RegType="date"
                        CssClass="input200" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);' runat="server"> </asp:TextBox>&nbsp;
                </td>
                <th>
                    End Date:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtEndDate" Enabled="false" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                        Validation="true" length="8-20" RegType="date" CssClass="input200" runat="server"> </asp:TextBox>&nbsp;
                </td>
            </tr>
            <tr>
                <th class="prority">
                    Approved
                    <br />
                    Options:
                </th>
                <td class="prority">
                    <asp:CheckBox ID="chkBugNeedApprove" Enabled="false" Text="BugNeedApproved" runat="server" /><br />
                    <asp:CheckBox ID="chkRequestNeedApprove" Enabled="false" Text="RequestNeedApproved"
                        runat="server" />
                </td>
                <th>
                    Test URL:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtTestUrl" Validation="true" Enabled="false" length="1-250" CssClass="input200"
                        runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Test User Name:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtTestUserName" Validation="true" Enabled="false" length="1-50"
                        CssClass="input200" runat="server"> </asp:TextBox>
                </td>
                <th>
                    Test Password:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtTestPassword" Validation="true" Enabled="false" length="1-50"
                        CssClass="input200" runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    FreeHour:
                </th>
                <td class="prority">
                    <asp:TextBox ID="txtFreeHour" CssClass="input200" Validation="true" Enabled="false"
                        length="1-3" RegType="number" runat="server"></asp:TextBox><asp:Literal ID="ltlFreeHour"
                            runat="server"></asp:Literal>
                </td>
                <th>
                    Over FreeHour:
                </th>
                <td>
                    <asp:CheckBox ID="chkIsOverFreeTime" Enabled="false" Checked="false" runat="server"
                        Text="Selected stands for has over freetime" />
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Description:
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtDesc" Validation="true" length="1-4000" Enabled="false" CssClass="input595"
                        TextMode="MultiLine" Rows="5" runat="server"> </asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div class="owToptwo">
        Files</div>
    <div class="owmainBox">
        <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
            <tr class="owlistTitle">
                <th width="20">
                    Title
                </th>
                <th width="20">
                    Link
                </th>
                <th width="30">
                    Create On
                </th>
                <th width="30">
                    Create By
                </th>
            </tr>
            <tr runat="server" id="trNoProject" visible="false">
                <td colspan="4" style="color: Red; padding-left: 10px;">
                    No records
                </td>
            </tr>
            <asp:Repeater ID="rptFiles" runat="server">
                <ItemTemplate>
                    <tr class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                        <td>
                            <%#Eval("FileTitle")%>
                        </td>
                        <td>
                            <a href='<%#Eval("FilePath") %>' target="_blank">
                                <%#Eval("ThumbPath")%></a>
                        </td>
                        <td>
                            <%#Eval("CreatedOn")%>
                        </td>
                        <td>
                            <%#Eval("FirstName")%>
                            <%#Eval("LastName")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
</asp:Content>
