<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeBehind="AddModule.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Modules.AddModule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="form-group-container" style="width: 650px;">
        <div class="form-group">
            <label class="col-left-password lefttext">Parent Module:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlParentModule" CssClass="selectProfle1" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div class="form-group">
            <label class="col-left-password lefttext">Title:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtModuleTitle" CssClass="inputProfle1 required" Validation="true" length="2-50"
                    MaxLength="50" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="form-group">
            <label class="col-left-password lefttext">Path (From Root):<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext" style="width:auto;">
                <asp:TextBox ID="txtModulePath" Validation="true" length="6-500" MaxLength="500"
                    CssClass="inputProfle1" runat="server" style="width:400px;"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-password lefttext">Default Page(Relative):</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtDefaultPage" Validation="true" length="6-500" MaxLength="500"
                    CssClass="inputProfle1" Text="Default.aspx" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="form-group">
            <label class="col-left-password lefttext">Menu Class:</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtClickFunction" Validation="true" length="4-500" MaxLength="500"
                    Text="" CssClass="inputProfle1" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="form-group">
            <label class="col-left-password lefttext">Show In Menu:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:CheckBox ID="chkShow" Checked="true" Text=" Selected stands for the module/page will show in left navigation"
                    runat="server" />
            </div>
        </div>
        <br />
        <div class="form-group">
            <label class="col-left-password lefttext">Priority:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtOrders" Validation="true" RegType="number" MaxLength="3" CssClass="inputProfle1 required"
                    runat="server" Text="1"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="buttonBox1">
            <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" runat="server" Text="Save" OnClick="btnSave_Click"
                OnClientClick="return Validate();" />

            <input type="button" value=" Back " class="cancelBtn1 mainbutton redirectback" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
