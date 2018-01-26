<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeBehind="EditProject.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Projects.EditProject" %>


<%@ Register Src="/UserControls/Admin/ClientMaintenancePlan.ascx" TagName="ClientMaintenancePlan"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript">
        jQuery(function () {
            // validate signup form on keyup and submit
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contentTitle titlsealrequest">Basic Information</div>
    <div class="limitwidth" style="width: 890px;">
        <div class="form-group">
            <label class="col-left-project lefttext">Project Title:<span class="noticeRed">*</span></label>
            <div class="col-right-project righttext">
                <asp:TextBox ID="txtTitle" MaxLength="128" CssClass="inputproject required" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Project Code:<span class="noticeRed">*</span></label>
            <div class="col-right-project righttext">
                <asp:TextBox ID="txtProjectCode" CssClass="inputproject required" Validation="true" MaxLength="64"
                    runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Company:<span class="noticeRed">*</span></label>
            <div class="col-right-project righttext">
                <label id="lblCompany" runat="server"></label>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Status:<span class="noticeRed">*</span></label>
            <div class="col-right-project righttext">
                <asp:DropDownList ID="ddlStatus" CssClass="selectproject" runat="server">
                    <asp:ListItem Text="Open" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Scheduled" Value="2"></asp:ListItem>
                    <asp:ListItem Text="In Progress" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Cancelled" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Other" Value="6"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Project Manager:<span class="noticeRed">*</span></label>
            <div class="col-right-project righttext">
                <asp:DropDownList ID="ddlPM" CssClass="selectproject" runat="server" data-msg="Please select project manager" min="0">
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Priority:<span class="noticeRed">*</span></label>
            <div class="col-right-project righttext">
                <asp:DropDownList ID="ddlPriority" CssClass="selectproject" runat="server">
                    <asp:ListItem Text="High" Value="1HIGH"></asp:ListItem>
                    <asp:ListItem Text="Medium" Value="2MEDIUM" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Low" Value="3LOW"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Start Date:<span class="noticeRed">*</span></label>
            <div class="col-right-project righttext">

                <asp:TextBox ID="txtStartDate" CssClass="inputprojectdate required date"
                    onclick="WdatePicker({isShowClear:false});" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">End Date:</label>
            <div class="col-right-project righttext">
                <asp:TextBox ID="txtEndDate" onclick="WdatePicker({isShowClear:false});"
                    CssClass="inputprojectdate date" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">
                Approved
Options:
            </label>
            <div class="col-right-project2 righttext">
                <span class="rightItem">
                    <asp:CheckBox ID="chkBugNeedApprove" Text=" Bug: requires client approval" runat="server" /></span>
                <span class="rightItem">
                    <asp:CheckBox ID="chkRequestNeedApprove" Text=" Request: requires client approval" runat="server" /></span>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Test URL: </label>
            <div class="col-right-project righttext">
                <asp:TextBox ID="txtTestUrl" CssClass="inputproject" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Test User Name: </label>
            <div class="col-right-project righttext">
                <asp:TextBox ID="txtTestUserName" CssClass="inputproject" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Test Password: </label>
            <div class="col-right-project righttext">
                <asp:TextBox ID="txtTestPassword" CssClass="inputproject" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Free Hours:<span class="noticeRed">*</span></label>
            <div class="col-right-project righttext">
                <asp:TextBox ID="txtFreeHour" CssClass="inputproject required number" MaxLength="3" runat="server" Text="40"></asp:TextBox>
                <asp:Literal ID="ltlFreeHour" runat="server"></asp:Literal><br />
                <asp:Literal ID="ltlFreeHourText" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Billable:</label>
            <div class="col-right-project righttext">

                <asp:RadioButtonList ID="rblBillable" RepeatDirection="Horizontal" runat="server">
                    <asp:ListItem Text=" Yes" Value="true"> </asp:ListItem>
                    <asp:ListItem Text=" No" Selected="true" Value="false"> </asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Description:<span class="noticeRed">*</span></label>
            <div class="col-right-project2 righttext">
                <asp:TextBox ID="txtDesc" CssClass="inputprojectds required" TextMode="MultiLine"
                    Rows="5" runat="server"> </asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-project lefttext">Maintenance Plan:</label>
            <div class="col-right-project2 righttext">
                <uc1:ClientMaintenancePlan ID="ClientMaintenancePlan1" runat="server" />
            </div>
        </div>
        <div class="buttonBox2">
            <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" ValidationGroup="Add" CausesValidation="true"
                runat="server" Text="Save" OnClick="btnSave_Click" />
            <input type="button" value=" Back " class="cancelBtn1 mainbutton redirectback" />
        </div>
    </div>

    <div class="contentTitle titleeventlist">Sunnet Users</div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <div class="form-group">
                    <label class="col-left-assignuser lefttext">DEV:</label>
                    <div class="col-right-assignuser righttext">
                        <ul class="assignUser">
                            <asp:Repeater ID="rptDev" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <input name="Input" type="checkbox" value="<%# Eval("ID") %>" id="chkDev<%# Eval("ID") %>" <%# (bool)Eval("Selected") ?"checked='checked'":"" %>
                                            onclick='assignSunnet(<%# Eval("ID") %>,<%# Eval("ProjectID") %>,this)' />
                                        <label for="chkDev<%# Eval("ID") %>">
                                            <%# Eval(UserNameDisplayProp) %>
                                        </label>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-left-assignuser lefttext">QA:</label>
                    <div class="col-right-assignuser righttext">
                        <ul class="assignUser">
                            <asp:Repeater ID="rptQA" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <input name="Input" type="checkbox" value="<%# Eval("ID") %>" <%# (bool)Eval("Selected") ?"checked='checked'":"" %>
                                            id="chkQA<%# Eval("ID") %>" onclick='assignSunnet(<%# Eval("ID") %>,<%# Eval("ProjectID") %>,this)' />
                                        <label for="chkQA<%# Eval("ID") %>">
                                            <%# Eval(UserNameDisplayProp) %>
                                        </label>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-left-assignuser lefttext">US:</label>
                    <div class="col-right-assignuser righttext">
                        <ul class="assignUser">
                            <asp:Repeater ID="rptUS" runat="server">
                                <ItemTemplate>
                                    <li >
                                        <input name="Input" type="checkbox" value="<%# Eval("ID") %>" <%# (bool)Eval("Selected") ?"checked='checked'":"" %>
                                            id="chkUS<%# Eval("ID") %>" onclick='assignSunnet(<%# Eval("ID") %>,<%# Eval("ProjectID") %>,this)' />
                                        <label for="chkUS<%# Eval("ID") %>">
                                            <%# Eval(UserNameDisplayProp) %>
                                        </label>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <div class="contentTitle titleeventlist">Client Users</div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <div class="form-group">
                    <label class="col-left-assignuser lefttext"></label>
                    <div class="col-right-assignuser righttext">
                        <ul class="assignUser">
                            <asp:Repeater ID="rptClient" runat="server">
                                <ItemTemplate>
                                    <li >
                                        <input name="Input" type="checkbox" value="<%# Eval("ID") %>" id='chkClient<%# Eval("ID") %>' <%# (bool)Eval("Selected") ?"checked='checked'":"" %>
                                            onclick='assignClient(<%# Eval("ID") %>,<%# Eval("ProjectID") %>,this)' />
                                        <label for="chkClient<%# Eval("ID") %>">
                                            <%# Eval(UserNameDisplayProp) %>
                                        </label>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <div class="contentTitle titleeventlist">
        Project Files
    </div>
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <ul class="listtopBtn">
                        <li>
                            <div class="listtopBtn_icon">
                                <img src="/images/icons/waddfile.png" />
                            </div>
                            <div class="listtopBtn_text"><a data-toggle="modal" data-target="#modalsmall" href="AddProjectFile.aspx?ID=<%=QS("ID") %>">Add File</a></div>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="220px">Title</th>
                <th>Link</th>
                <th class="aligncenter" width="140px">Created On </th>
                <th class="aligncenter" width="140px">Create By </th>
            </tr>
        </thead>
        <tbody>
            <tr runat="server" id="trNoProject" visible="false">
                <td colspan="4" style="color: Red; padding-left: 10px;">No records
                </td>
            </tr>
            <asp:Repeater ID="rptFiles" runat="server">
                <ItemTemplate>
                    <tr class=" <%#Container.ItemIndex % 2 == 0 ? "" : "whiterow"%>">
                        <td>
                            <%#Eval("FileTitle")%>
                        </td>
                        <td>
                            <a href='/Do/DoDownloadFileHandler.ashx?FileID=<%#Eval("FileID") %>&size=<%#Eval("FileSize") %>&tableType=<%#Eval("TableType") %>'
                                target="_blank">
                                <%#Eval("FileTitle")%></a>
                        </td>
                        <td class="aligncenter">
                            <%#Eval("CreatedOn")%>
                        </td>
                        <td class="aligncenter">
                            <%#GetClientUserName(Convert.ToInt32(Eval("CreatedBy").ToString()))%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>


    <div class="contentTitle titleeventlist">
        Project Principals
    </div>
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <ul class="listtopBtn">
                        <li>
                            <div class="listtopBtn_icon">
                                <img src="/images/icons/waddprincipal.png" />
                            </div>
                            <div class="listtopBtn_text"><a data-toggle="modal" data-target="#modalsmall" href="AddProjectPrincipal.aspx?ID=<%=QS("ID") %>">Add Principal</a></div>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th>Module/Function </th>
                <th>PM</th>
                <th>DEV</th>
                <th>Tester</th>
                <th class="aligncenter">Action</th>
            </tr>
        </thead>
        <tbody>
            <tr runat="server" id="trNoPri" visible="false">
                <td colspan="3" style="color: Red; padding-left: 10px;">No records
                </td>
            </tr>
            <asp:Repeater ID="rptPrincipals" runat="server">
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex % 2 == 0 ? "" : "whiterow"%>">
                        <td>
                            <%# ((string)Eval("Module")).Replace("\r\n","<br>") %>
                        </td>
                        <td>
                            <%# ((string)Eval("PM")).Replace("\r\n","<br>") %>
                        </td>
                        <td>
                            <%# ((string)Eval("DEV")).Replace("\r\n","<br>") %>
                        </td>
                        <td>
                            <%# ((string)Eval("QA")).Replace("\r\n","<br>") %>
                        </td>
                        <td class="aligncenter"><a data-toggle="modal" data-target="#modalsmall" href="EditProjectPrincipal.aspx?ID=<%# Eval("ID")%>">Edit</a> </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>

        </tbody>
    </table>

    <script type="text/javascript">
        function assignSunnet(userId,projectid,o)
        {
            $.ajax({
                type: "post",
                url: "/Do/DoAssentUserToProject.ashx?r=" + Math.random(),
                data: {
                    type: jQuery(o).prop("checked") ?  "add" : "del",
                    projectId:projectid,
                    UserId: userId,
                    client : "sunnet" 
                }
            });
        }

        function assignClient(userId,projectid,o)
        {
            $.ajax({
                type: "post",
                url: "/Do/DoAssentUserToProject.ashx?r=" + Math.random(),
                data: {
                    type: jQuery(o).prop("checked") ?  "add" : "del",
                    projectId:projectid,
                    UserId: userId,
                    client :  "client"
                }
            });
        }

    </script>
</asp:Content>
