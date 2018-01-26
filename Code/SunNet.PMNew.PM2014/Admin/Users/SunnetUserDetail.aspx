<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SunnetUserDetail.aspx.cs"
    MasterPageFile="~/Admin/admin.master"
    Inherits="SunNet.PMNew.PM2014.Admin.Users.SunnetUserDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #body_body_dataSection_ticketBasicInfo_rdoPriority {
            border: 0;
        }
    </style>
    <script src="/Scripts/jquery.maskedinput-1.3.1.min.js"></script>
    <script type="text/javascript">
        jQuery(function () {
            jQuery("#<%=txtPhone.ClientID %>").mask("(999) 999-9999");
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });

            jQuery("#tipNotice").popover();
        });
        function BackToList() {
            this.location.href = "/Admin/Users/Users.aspx";
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
        function clearNoNum(obj) {
            obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符  
            obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的  
            obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
            obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');//只能输入两个小数  
            if (obj.value.indexOf(".") < 0 && obj.value != "") {//以上已经过滤，此处控制的是如果没有小数点，首位不能为类似于 01、02的金额 
                obj.value = parseFloat(obj.value);
            }
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
                <asp:TextBox ID="txtLastName" Validation="true" MaxLength="20" CssClass="inputProfle1 required"
                    runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">User Name:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtUserName" ValidatorTitle="User Name: please enter your email address."
                    MaxLength="50" CssClass="inputProfle1 required email" runat="server"></asp:TextBox>
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
                    MaxLength="14" CssClass="inputProfle1 password" runat="server"></asp:TextBox>
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
        Advance Information
    </div>
    <div class="form-group-container">
        <div class="form-group">
            <label class="col-left-profile lefttext">Role:</label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlRole" CssClass="selectProfle1" runat="server"></asp:DropDownList>
                <asp:Literal runat="server" ID="LitRole"></asp:Literal>
            </div>
            <label class="col-left-profile lefttext">Office:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlOffice" runat="server" CssClass="selectProfle1">
                    <asp:ListItem Text="US" Value="US"></asp:ListItem>
                    <asp:ListItem Text="CN" Value="CN"></asp:ListItem>
                    <asp:ListItem Text="Administration Office" Value="AO"></asp:ListItem>
                    <asp:ListItem Text="Department 1" Value="D1"></asp:ListItem>
                    <asp:ListItem Text="Department 2" Value="D2"></asp:ListItem>
                </asp:DropDownList>
                <asp:Literal runat="server" ID="LitOffice"></asp:Literal>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Client/Sunnet:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlUserType" CssClass="selectProfle1" runat="server">
                    <asp:ListItem Text="Sunnet" Value="SUNNET"></asp:ListItem>
                    <asp:ListItem Text="Client" Value="CLIENT"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;   &nbsp;&nbsp;
               <asp:Literal runat="server" ID="LitClient"></asp:Literal>
            </div>
            <label class="col-left-profile lefttext">Daily Notice:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:CheckBox ID="chkNotice" runat="server" />&nbsp;<a data-original-title="Daily Notice" data-toggle="popover" id="tipNotice" class="info" title="" href="###" data-container="body" data-placement="right" data-trigger="hover click" data-html="true" data-content="Checked means the user's timesheet report will sent to  administrator daily.">&nbsp;</a>
            </div>

        </div>
    </div>
    <div class="seplineOne"></div>
    <div class="contentTitle titleprofile">Work Schedule</div>
    <div class="form-group-container">
        <div class="form-group">
            <label class="col-left-newevent_users lefttext">Work Interval:</label>
            <div class="col-right-newevent1 righttext">
                <span class="">From: </span>
                <input type="text" value="" id="txtBeginTimeFirst" class="inputtime" data-itemwidth="60" data-width="100" autocomplete="off" pattern="^[0-2]?[0-9]:[0-5][0-9]\s*(( AM)|( PM))$" data-options="12:00 AM,12:30 AM,1:00 AM,1:30 AM,2:00 AM,2:30 AM,3:00 AM,3:30 AM,4:00 AM,4:30 AM,5:00 AM,5:30 AM,6:00 AM
                ,6:30 AM,7:00 AM,7:30 AM,8:00 AM,8:30 AM,9:00 AM,9:30 AM,10:00 AM,10:30 AM,11:00 AM,11:30 AM,12:00 PM,12:30 PM,1:00 PM,1:30 PM,2:00 PM,2:30 PM,3:00 PM
                ,3:30 PM,4:00 PM,4:30 PM,5:00 PM,5:30 PM,6:00 PM,6:30 PM,7:00 PM,7:30 PM,8:00 PM,8:30 PM,9:00 PM,9:30 PM,10:00 PM,10:30 PM,11:00 PM,11:30 PM"
                    data-filter="false" data-height="150" data-autocomplete="true" style="width: 95px;">
                <span class="">To:</span><input type="text" value="" id="txtEndTimeFirst" class="inputtime" data-itemwidth="60" data-width="100" autocomplete="off" pattern="^[0-2]?[0-9]:[0-5][0-9]\s*(( AM)|( PM))$" data-options="12:00 AM,12:30 AM,1:00 AM,1:30 AM,2:00 AM,2:30 AM,3:00 AM,3:30 AM,4:00 AM,4:30 AM,5:00 AM,5:30 AM,6:00 AM
                ,6:30 AM,7:00 AM,7:30 AM,8:00 AM,8:30 AM,9:00 AM,9:30 AM,10:00 AM,10:30 AM,11:00 AM,11:30 AM,12:00 PM,12:30 PM,1:00 PM,1:30 PM,2:00 PM,2:30 PM,3:00 PM
                ,3:30 PM,4:00 PM,4:30 PM,5:00 PM,5:30 PM,6:00 PM,6:30 PM,7:00 PM,7:30 PM,8:00 PM,8:30 PM,9:00 PM,9:30 PM,10:00 PM,10:30 PM,11:00 PM,11:30 PM"
                    data-filter="false" data-height="150" data-autocomplete="true" style="width: 95px;">
                <input type="button" onclick="addOtherWorkInterval();" value="" class="addBtn" title="Add Work Interval" />
            </div>
            <label class="col-left-profile lefttext" style="width: auto; margin-left: 25px">PTO Hours/Years:</label>
            <div class="col-left-profile righttext" style="text-align: right">
                <asp:TextBox runat="server" ID="PTOhours" Style="width: 50px;" onkeyup="clearNoNum(this)"></asp:TextBox>
            </div>
        </div>
        <div class="form-group" id="div_add_workinterval">
            <input type="hidden" name="workinterval_count" id="workinterval_count" />
            <asp:Repeater ID="rptOtherUser" runat="server">
                <ItemTemplate>
                    <div id="div_workinterval_<%# Container.ItemIndex + 1 %>">
                        <label class="col-left-newevent_users lefttext"></label>
                        <div class="col-right-newevent2 righttext">
                            <label class="addUsers"><%# Eval("FromTime") %><input type="hidden" name="txtBeginTimeFirst<%# Container.ItemIndex + 1 %>" value="<%# Eval("FromTime") %>" /></label>
                            <label class="addUsers"><%# Eval("ToTime") %><input type="hidden" name="txtEndTimeFirst<%# Container.ItemIndex + 1 %>" value=" <%# Eval("ToTime") %>" /></label>
                            <input type="button" class="delBtn" onclick="removeWorkinterval('div_workinterval_<%#Container.ItemIndex + 1%>    ');"></input>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>
        <script type="text/html" id="temp_work_Interval">
            {% if (this) { %}               
              <div id="div_workinterval_{%this.ID%}">
                  <label class="col-left-newevent_users lefttext"></label>
                  <div class="col-right-newevent2 righttext">
                      <label class="addUsers">{% this.BeginTime %}<input type="hidden" name="txtBeginTimeFirst{%this.ID%}" value="{% this.BeginTime %}" /></label>
                      <label class="addUsers">{% this.EndTime%}<input type="hidden" name="txtEndTimeFirst{%this.ID%}" value="{% this.EndTime %}" /></label>
                      <input type="button" class="delBtn" onclick="removeWorkinterval('div_workinterval_{%this.ID%}');"></input>
                  </div>
              </div>
            {%}%}
        </script>
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
    <script type="text/javascript">
        var otheruserId = <% = rptOtherUser.Items.Count+1 %>;
        function addOtherWorkInterval() {
            var tmpBeginTime = jQuery("#txtBeginTimeFirst").val();
            if (tmpBeginTime == "") {
                alert("Please enter Begin Time.");
                return;
            }
            var tmpEndTime = jQuery("#txtEndTimeFirst").val();
            if (tmpEndTime == "") {
                alert("Please enter End Time.");
                return;
            }
            var otheruser = {
                ID: otheruserId,
                BeginTime: tmpBeginTime,
                EndTime: tmpEndTime
            };
            jQuery("#txtBeginTimeFirst").val("");
            jQuery("#txtEndTimeFirst").val("");
            var html = TemplateEngine(GetTemplateHtml("temp_work_Interval"), otheruser);

            $("#div_add_workinterval").append(html);
            $("#workinterval_count").val(otheruserId);
            otheruserId++;
        }

        function removeWorkinterval(id) {
            jQuery("#" + id).remove();
            $("#workinterval_count").val($("#workinterval_count").val()-1);
        }

        $(function() {
            $("#workinterval_count").val(otheruserId-1);
        });
    </script>
</asp:Content>
