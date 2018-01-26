<%@ Page Title="Add Project" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="AddProject.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Projects.AddProject" %>

<%@ Register Src="../../UserControls/ClientMaintenancePlan.ascx" TagName="ClientMaintenancePlan"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Add Project
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owToptwo">
        Basic Information</div>
    <div class="owmainBox" style="margin-bottom: 0;">
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th width="100">
                    Project Title:<span class="redstar">*</span>
                </th>
                <td width="250">
                    <asp:TextBox ID="txtTitle" Validation="true" length="1-128" CssClass="input200" runat="server"></asp:TextBox>
                </td>
                <th width="100">
                    Project Code:<span class="redstar">*</span>
                </th>
                <td>
                    <span class="prority">
                        <asp:TextBox ID="txtProjectCode" CssClass="input200" Validation="true" length="1-64"
                            runat="server"></asp:TextBox></span>
                </td>
            </tr>
            <tr>
                <th>
                    Company:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlCompany" CssClass="select205" runat="server">
                    </asp:DropDownList>
                </td>
                <th class="prority">
                    Status:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlStatus" CssClass="select205" runat="server">
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
                    Project Manager:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlPM" CssClass="select205" runat="server">
                    </asp:DropDownList>
                </td>
                <th class="prority">
                    Priority:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlPriority" CssClass="select205" runat="server">
                        <asp:ListItem Text="High" Value="1HIGH"></asp:ListItem>
                        <asp:ListItem Text="Medium" Value="2MEDIUM" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Low" Value="3LOW"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Start Date:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtStartDate" Validation="true" length="8-20" RegType="date" CssClass="input180"
                        onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);' runat="server"> </asp:TextBox>&nbsp;
                    <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtStartDate.ClientID %>"),document.getElementById("<%=txtStartDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                        align="absmiddle">
                </td>
                <th>
                    End Date:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtEndDate" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                        Validation="true" length="8-20" RegType="date" CssClass="input180" runat="server"> </asp:TextBox>&nbsp;
                    <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtEndDate.ClientID %>"),document.getElementById("<%=txtEndDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                        align="absmiddle">
                </td>
            </tr>
            <tr>
                <th class="prority">
                    Approved
                    <br />
                    Options:
                </th>
                <td class="prority">
                    <asp:CheckBox ID="chkBugNeedApprove" Text="Bug:require client aprove" runat="server" /><br />
                    <asp:CheckBox ID="chkRequestNeedApprove" Text="Request:require client aprove" runat="server" />
                </td>
                <th>
                    Test URL:
                </th>
                <td>
                    <asp:TextBox ID="txtTestUrl" CssClass="input200" runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Test User Name:
                </th>
                <td>
                    <asp:TextBox ID="txtTestUserName" CssClass="input200" runat="server"> </asp:TextBox>
                </td>
                <th>
                    Test Password:
                </th>
                <td>
                    <asp:TextBox ID="txtTestPassword" CssClass="input200" runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Free Hours:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:TextBox ID="txtFreeHour" CssClass="input200" Validation="true" length="1-3"
                        RegType="number" runat="server" Text="40"></asp:TextBox>
                </td>
                <th>
                    Billable:
                </th>
                <td>
                    <asp:RadioButtonList ID="rblBillable" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Text="Yes" Value="true"> </asp:ListItem>
                        <asp:ListItem Text="No" Selected="true" Value="false"> </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Description:<span class="redstar">*</span>
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtDesc" Validation="true" length="1-4000" CssClass="input595" TextMode="MultiLine"
                        Rows="5" runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="center" valign="top">
                    Maintenance Plan:<span class="redstar">*</span>
                </th>
                <td colspan="3">
                    <uc1:ClientMaintenancePlan ID="ClientMaintenancePlan1" runat="server" />
                </td>
            </tr>
        </table>
        <div class="btnBoxone">
            <asp:Button ID="btnSave" CssClass="btnone" runat="server" Text="Save" OnClick="btnSave_Click"
                OnClientClick="return Validate();" />
            <input name="button2" id="btnClientCancel" type="button" class="btnone" value="Clear" />
        </div>
    </div>
</asp:Content>
