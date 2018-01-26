<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeBehind="AddProject.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Projects.AddProject" %>

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
    <style>
        @media (max-width: 992px){
            .col-right-project {
                width: 270px;
            }
            .col-right-project2{
                width:600px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contentTitle titlsealrequest">Basic Information</div>
    <div class="limitwidth" style="width:890px;">
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
            <asp:DropDownList ID="ddlCompany" data-msg="Please select company" CssClass="selectproject" runat="server" min="1">
            </asp:DropDownList>
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
                <asp:ListItem Text="86'd" Value="6"></asp:ListItem>
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
Options:</label>
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
            <asp:TextBox ID="txtTestPassword" CssClass="inputpr oject" runat="server"> </asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-project lefttext">Free Hours:<span class="noticeRed">*</span></label>
        <div class="col-right-project righttext">
            <asp:TextBox ID="txtFreeHour" CssClass="inputproject required number" MaxLength="3" runat="server" Text="40"></asp:TextBox>
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
        <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" ValidationGroup="Add" CausesValidation="true" runat="server" Text="Save" OnClick="btnSave_Click" />
        <input type="button" value=" Back " class="cancelBtn1 mainbutton redirectback" />
    </div>
        </div>
</asp:Content>