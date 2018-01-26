<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientUserDetail.aspx.cs"
    MasterPageFile="~/Admin/admin.master"
    Inherits="SunNet.PMNew.PM2014.Admin.Users.ClientUserDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #body_body_dataSection_ticketBasicInfo_rdoPriority {
            border: 0;
        }
    </style>


    <script src="/Scripts/jquery.maskedinput-1.3.1.min.js"></script>
    <script type="text/javascript">
        jQuery(function () {
            jQuery("#<%=txtPhone.ClientID %>").add("#<%= txtEPhone.ClientID %>").mask("(999) 999-9999");
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
        });

        function BackToList() {
            this.location.href = "/Admin/Users.aspx";
        }
        function SelectProject() {
            var uid = urlParams["id"];
            if (uid)
                RedirectBack("/Admin/Users/AssignProjectToUser.aspx?uid=" + uid);
        }

        function doDeleteProject(elem, projectID) {
            $.ajax({
                url: '/do/DoDeleteUserFromProject.ashx',
                type: 'post',
                data: { 'projectID': projectID, 'userToEdit': '<%=userToEditID.ToString()%>' }
            }).success(function (message) {
                if (message === "1") {
                    $(elem).closest("tr").remove();
                }
            });

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contentTitle titleprofile">Basic Information </div>
    <div class="form-group-container">
        <div class="form-group">
            <label class="col-left-profile lefttext">Company:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlCompany" Enabled="False" CssClass="selectProfle1" runat="server">
                </asp:DropDownList>
                <asp:Literal runat="server" ID="litCompanyName"></asp:Literal>
            </div>
            <label class="col-left-profile lefttext">Title:</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtTitle" CssClass="inputProfle1" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">First Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtFirstName" MaxLength="20" CssClass="inputProfle1 required"
                    runat="server"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext">Last Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtLastName" MaxLength="20" CssClass="inputProfle1 required"
                    runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">User Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtUserName" Validation="true" ValidatorTitle="User Name: please enter your email address."
                    RegType="email" MaxLength="50" CssClass="inputProfle1 email required" runat="server"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext">Phone:</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtPhone" CssClass="inputProfle1" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Skype:</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtSkype" CssClass="inputProfle1" length="1-50" runat="server"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext">Status:</label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlStatus" CssClass="selectProfle1" runat="server">
                    <asp:ListItem Text="Active" Value="ACTIVE"></asp:ListItem>
                    <asp:ListItem Text="Inactive" Value="INACTIVE"></asp:ListItem>
                </asp:DropDownList>
                <asp:Literal runat="server" ID="LitStatus"></asp:Literal>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Password:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtPassword" AutoCompleteType="None" autocomplete="off" TextMode="Password"
                    length="1-14" CssClass="inputProfle1 password" runat="server"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext">Confirm:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtConfirmPassword" AutoCompleteType="None" autocomplete="off" MaxLength="14"
                    TextMode="Password" CssClass="inputProfle1 password" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="seplineOne"></div>
    <div class="contentTitle titleprofile">
         Emergency Contact
    </div>
    <div class="form-group-container">
        <div class="form-group">
            <label class="col-left-profile lefttext">First Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtEFirstName" MaxLength="20" Validation="true" ValidatorTitle="Emergency Contact First Name "
                    length="1-20" CssClass="inputProfle1 required" runat="server"></asp:TextBox>
            </div>

            <label class="col-left-profile lefttext">Last Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtELastName" MaxLength="20" Validation="true" ValidatorTitle="Emergency Contact Last Name "
                    length="1-20" CssClass="inputProfle1 required" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Email:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtEEmail" Validation="true" RegType="email" CssClass="inputProfle1 email required" runat="server"></asp:TextBox>
            </div>

            <label class="col-left-profile lefttext">Phone:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtEPhone" Validation="true" ValidatorTitle="The Emergency Contact Phone field is required."
                    RegType="phone" CssClass="inputProfle1 required" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="buttonBox3">
            <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" ValidationGroup="Add" CausesValidation="true" runat="server" Text="Save" OnClick="btnSave_Click" />
            <input name="button2" tabindex="10" id="btnCancel" type="button" class="redirectback backBtn mainbutton" value="Back" />
        </div>
    </div>
    <div class="seplineOne"></div>
    <div class="contentTitle titleprofile">
        Assigned Projects
    <input name="button2" tabindex="10" id="btnSelectProject " href="/Admin/Users/AssignProjectToUser.aspx?uid=<%= QS("id",0) %>" data-target="#modalsmall" data-toggle="modal" type="button" class="cancelBtn1 mainbutton" value="Select Project" />

    </div>
    <div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
            <thead>
                <tr>
                    <th width="200">Project Code</th>
                    <th width="*">Project Title</th>
                    <th width="200">Company</th>
                    <th width="200">Project Manager</th>
                    <th width="40">Action</th>

                </tr>
            </thead>
            <tr runat="server" id="trNoProjects" visible="false">
                <th colspan="9" style="color: Red;">&nbsp; No record found.
                </th>
            </tr>
            <asp:Repeater ID="rptProjects" runat="server">
                <ItemTemplate>
                    <!-- collapsed expanded -->
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                        <td>
                            <%#Eval("ProjectCode")%>
                        </td>
                        <td>
                            <%#Eval("Title")%>
                        </td>
                        <td>
                            <%#Eval("CompanyName")%>
                        </td>
                        <td>
                            <%#Eval("PMFirstName") + " " + Eval("PMLastName") %>
                        </td>
                        <td>
                            <a onclick="doDeleteProject(this,'<%#Eval("ProjectID")%>')" title="Delete" href="###">
                                <img alt="Delete" src="/Images/icons/delete.png"></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

    </div>
    <div class="pagebox">
        <webdiyer:AspNetPager ID="ProjectPage" runat="server">
        </webdiyer:AspNetPager>
    </div>

</asp:Content>
