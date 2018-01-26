<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewSunneter.aspx.cs"
    MasterPageFile="~/Admin/admin.master"
    Inherits="SunNet.PMNew.PM2014.Admin.Users.NewSunneter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #body_body_dataSection_ticketBasicInfo_rdoPriority {
            border: 0;
        }
    </style>
    <script src="/Scripts/jquery.maskedinput-1.3.1.min.js"></script>
    <script type="text/javascript">
        function clearNoNum(obj) {
            obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符  
            obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的  
            obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
            obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');//只能输入两个小数  
            if (obj.value.indexOf(".") < 0 && obj.value != "") {//以上已经过滤，此处控制的是如果没有小数点，首位不能为类似于 01、02的金额 
                obj.value = parseFloat(obj.value);
            }
        }
        jQuery(function () {
            jQuery("#<%=txtPhone.ClientID %>").mask("(999) 999-9999");
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });

            jQuery("#tipNotice").popover();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contentTitle titleprofile">Basic Information </div>
    <div class="form-group-container">
        <div class="form-group">
            <label class="col-left-profile lefttext">Company:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlCompany" CssClass="selectProfle1 required" runat="server">
                </asp:DropDownList>
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
                <asp:TextBox ID="txtUserName" ValidatorTitle="User Name: please enter your email address."
                    RegType="email" MaxLength="50" CssClass="inputProfle1 required email" runat="server"></asp:TextBox>
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
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Password:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtPassword" AutoCompleteType="None" autocomplete="off" TextMode="Password"
                    MaxLength="14" CssClass="inputProfle1 password required" runat="server"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext">Confirm:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtConfirmPassword" AutoCompleteType="None" autocomplete="off" MaxLength="14"
                    TextMode="Password" CssClass="inputProfle1 required password" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="    seplineOne"></div>
    <div class="contentTitle titleprofile">
        Advance Information
    </div>
    <div class="form-group-container">
        <div class="form-group">
            <label class="col-left-profile lefttext">Role:</label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlRole" CssClass="selectProfle1" runat="server"></asp:DropDownList>
            </div>
            <label class="col-left-profile lefttext">Office:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlOffice" runat="server" CssClass="selectProfle1 required">
                    <asp:ListItem Text="US" Value="US"></asp:ListItem>
                    <asp:ListItem Text="CN" Value="CN"></asp:ListItem> 
                    <asp:ListItem Text="Administration Office" Value="AO"></asp:ListItem>
                    <asp:ListItem Text="Department 1" Value="D1"></asp:ListItem>
                    <asp:ListItem Text="Department 2" Value="D2"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Client/Sunnet:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList ID="ddlUserType" CssClass="selectProfle1" runat="server">
                    <asp:ListItem Text="Sunnet" Value="SUNNET"></asp:ListItem>
                    <asp:ListItem Text="Client" Value="CLIENT"></asp:ListItem>
                </asp:DropDownList>
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
            <span>From: </span><input type="text" value="" id="txtBeginTimeFirst" class="inputtime" data-itemwidth="60" data-width="100" autocomplete="off" pattern="^[0-2]?[0-9]:[0-5][0-9]\s*(( AM)|( PM))$" data-options="12:00 AM,12:30 AM,1:00 AM,1:30 AM,2:00 AM,2:30 AM,3:00 AM,3:30 AM,4:00 AM,4:30 AM,5:00 AM,5:30 AM,6:00 AM
                ,6:30 AM,7:00 AM,7:30 AM,8:00 AM,8:30 AM,9:00 AM,9:30 AM,10:00 AM,10:30 AM,11:00 AM,11:30 AM,12:00 PM,12:30 PM,1:00 PM,1:30 PM,2:00 PM,2:30 PM,3:00 PM
                ,3:30 PM,4:00 PM,4:30 PM,5:00 PM,5:30 PM,6:00 PM,6:30 PM,7:00 PM,7:30 PM,8:00 PM,8:30 PM,9:00 PM,9:30 PM,10:00 PM,10:30 PM,11:00 PM,11:30 PM" data-filter="false" data-height="150" data-autocomplete="true" style="width: 95px;">
            <span>To: </span><input type="text" value="" id="txtEndTimeFirst" class="inputtime" data-itemwidth="60" data-width="100" autocomplete="off" pattern="^[0-2]?[0-9]:[0-5][0-9]\s*(( AM)|( PM))$" data-options="12:00 AM,12:30 AM,1:00 AM,1:30 AM,2:00 AM,2:30 AM,3:00 AM,3:30 AM,4:00 AM,4:30 AM,5:00 AM,5:30 AM,6:00 AM
                ,6:30 AM,7:00 AM,7:30 AM,8:00 AM,8:30 AM,9:00 AM,9:30 AM,10:00 AM,10:30 AM,11:00 AM,11:30 AM,12:00 PM,12:30 PM,1:00 PM,1:30 PM,2:00 PM,2:30 PM,3:00 PM
                ,3:30 PM,4:00 PM,4:30 PM,5:00 PM,5:30 PM,6:00 PM,6:30 PM,7:00 PM,7:30 PM,8:00 PM,8:30 PM,9:00 PM,9:30 PM,10:00 PM,10:30 PM,11:00 PM,11:30 PM" data-filter="false" data-height="150" data-autocomplete="true" style="width: 95px;">
            <input type="button" onclick="addOtherWorkInterval();" value="" class="addBtn" title="Add Work Interval" />
            </div>
             <label class="col-left-profile lefttext" style="width:115px;margin-left:15px">PTO Hours/Years:</label>
            <div class="col-left-profile righttext" style="text-align:right">
                <asp:TextBox runat="server" ID="PTOhours" style="width:50px;" onkeyup="clearNoNum(this)" Text="40"></asp:TextBox>
            </div>
        </div>
        <div class="form-group" id="div_add_workinterval">
            <input type="hidden" name="workinterval_count" id="workinterval_count"/>
        </div>
         <script type="text/html" id="temp_work_Interval">
            {% if (this) { %}               
              <div id="div_workinterval_{%this.ID%}">
                  <label class="col-left-newevent_users lefttext"></label>
                  <div class="col-right-newevent2 righttext">
                      <label class="addUsers">{% this.BeginTime %}
                          <input type="hidden" name="txtBeginTimeFirst{%this.ID%}" value="{% this.BeginTime %}" />
                      </label>
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
    <script type="text/javascript">
        var otheruserId = 1;
        function addOtherWorkInterval() {
            var fromTimeStr = "07/14/2017" + " " + ($("#txtBeginTimeFirst").val());
            var fromTime = new Date("" + fromTimeStr + "");
            var toTimeStr = "07/14/2017" + " " + ($("#txtEndTimeFirst").val());
            var toTime = new Date("" + toTimeStr + "");
            if (toTime < fromTime) {
                alert("Please select correct From Time and To Time!");
                jQuery("#txtBeginTimeFirst").val("");
                jQuery("#txtEndTimeFirst").val("");
                 return false;
            }


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
        }
    </script>
</asp:Content>
