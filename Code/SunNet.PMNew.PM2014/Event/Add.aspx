<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pop.master" CodeBehind="Add.aspx.cs" ClientIDMode="Predictable" Inherits="SunNet.PMNew.PM2014.Event.Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript">
        $(function () {
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div",
                onkeyup: false,
                onfocusout: false
            });

            $("#body_bodySection_txtTimes").on("change", function (e) {
                var th = $(this);
                var thVal = parseInt(th.val());
                if (isNaN(thVal) || (thVal <= 0 || thVal >= 1001)) {
                    alert("Please enter a number between 1 and 1000.");
                    th.val(1);
                    return false;
                } else {
                    th.val(thVal);
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Create Event
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <style>
                .lefttext {
                    width: 62px !important;
                }

                .inputevent1 {
                    width: 610px !important;
                }
            </style>
            <div class="form-group">
                <label class="col-left-newevent lefttext">Project:<span class="noticeRed">*</span></label>
                <div class="col-right-newevent righttext">

                    <asp:DropDownList ID="ddlProjects" runat="server" class="selectw4 required">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <asp:TextBox runat="server" ID="userName" style="display:none"></asp:TextBox>
                <label class="col-left-newevent lefttext">Title:<span class="noticeRed">*</span></label>
                <div class="col-right-newevent righttext">
                    <asp:TextBox CssClass="inputw3 required" Width="217" ID="txtName" runat="server" placeholder="Add Title"></asp:TextBox>
                    <asp:Image runat="server" ID="imgIcon" align="absmiddle" src="/images/eventicons/event_icon_8s.png" />
                    <div class="eventiconBox" style="display: none;" id="divEventIcon">
                        <ul class="eventiconitem">
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="1" />
                                </div>
                                <img src="/images/eventicons/event_icon_1.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="2" />
                                </div>
                                <img src="/images/eventicons/event_icon_2.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="3" />
                                </div>
                                <img src="/images/eventicons/event_icon_3.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="4" />
                                </div>
                                <img src="/images/eventicons/event_icon_4.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="5" />
                                </div>
                                <img src="/images/eventicons/event_icon_5.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="6" />
                                </div>
                                <img src="/images/eventicons/event_icon_6.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="7" />
                                </div>
                                <img src="/images/eventicons/event_icon_7.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="8" />
                                </div>
                                <img src="/images/eventicons/event_icon_8.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="9" />
                                </div>
                                <img src="/images/eventicons/event_icon_9.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="10" />
                                </div>
                                <img src="/images/eventicons/event_icon_10.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="11" />
                                </div>
                                <img src="/images/eventicons/event_icon_11.png" /></li>
                            <li>
                                <div class="radiobutBox1">
                                    <input name="radioEventIcon" type="radio" value="12" />
                                </div>
                                <img src="/images/eventicons/event_icon_12.png" /></li>
                        </ul>
                    </div>
                    <div for="<%=txtName.ClientID %>" class="error" style="display: none;"></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form-group" id="dvProjectRequired" style="display: none;">
        <label class="col-left-newevent lefttext"></label>
        <div class="col-right-newevent righttext">
            <div class="noticeRed2" id="dvProjectContent">
                This field is required.
            </div>
        </div>
    </div>
    <div class="form-group" id="dvTitleRequired" style="display: none;">
        <label class="col-left-newevent lefttext"></label>
        <div class="col-right-newevent righttext noticeRed2">
            <div class="noticeRed2">
                This field is required.
            </div>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent lefttext">Where:</label>
        <div class="col-right-newevent2 righttext">
            <asp:TextBox CssClass="inputevent1" ID="txtWhere" placeholder="Add a location" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent lefttext">Detail:</label>
        <div class="col-right-newevent2 righttext">
            <asp:TextBox TextMode="MultiLine" ID="txtDetails" CssClass="inputevent1" placeholder="Add Event Detail" Rows="4" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="form-group">
        <label class="col-left-newevent lefttext">All-day:</label>
        <div class="col-right-newevent righttext">
            <asp:CheckBox ID="chkAllDay" runat="server" />
        </div>       
    </div>
    <div class="form-group" style="visibility:hidden" id="div_off">
        <label class="col-left-newevent lefttext">Off:</label>
        <div class="col-right-newevent righttext">
            <asp:CheckBox ID="chkOff" runat="server" Enabled="false"/>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent lefttext">From:</label>
        <div class="col-right-newevent righttext">
            <asp:TextBox CssClass="inputdate required" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy',isShowClear:false});" ID="txtFrom" onblur="minDate()" runat="server">
            </asp:TextBox>
            <asp:TextBox CssClass="inputtime" ID="txtFromTime"
                data-autocomplete="true" data-options="12:00am,12:30am,1:00am,1:30am,2:00am,2:30am,3:00am,3:30am,4:00am,4:30am,5:00am,5:30am,6:00am
                ,6:30am,7:00am,7:30am,8:00am,8:30am,9:00am,9:30am,10:00am,10:30am,11:00am,11:30am,12:00pm,12:30pm,1:00pm,1:30pm,2:00pm,2:30pm,3:00pm
                ,3:30pm,4:00pm,4:30pm,5:00pm,5:30pm,6:00pm,6:30pm,7:00pm,7:30pm,8:00pm,8:30pm,9:00pm,9:30pm,10:00pm,10:30pm,11:00pm,11:30pm"
                data-width="100" data-itemwidth="50" data-height="150" data-filter="false"
                autocomplete="off" pattern="^[0-2]?[0-9]:[0-5][0-9]\s*((am)|(pm))$" 
                Text="8:00am" Style="width: 95px;" runat="server">
            </asp:TextBox>
            <div for="<%=txtFrom.ClientID %>" class="error" style="display: none;"></div>
            <div for="<%=txtFromTime.ClientID %>" class="error" style="display: none;margin-left:120px;"></div>
        </div>
    </div>

    <div class="form-group">
        <label class="col-left-newevent lefttext">Alert:</label>
        <div class="col-right-newevent righttext">
            <asp:DropDownList ID="ddlAlert" CssClass="selectw4" runat="server">
                <asp:ListItem Text="None" Value="1"></asp:ListItem>
                <asp:ListItem Text="five minutes before" Value="2"></asp:ListItem>
                <asp:ListItem Text="fifteen minutes before" Value="3"></asp:ListItem>
                <asp:ListItem Text="thirty minutes before" Value="4"></asp:ListItem>
                <asp:ListItem Text="one hour before" Value="5"></asp:ListItem>
                <asp:ListItem Text="two hours before" Value="6"></asp:ListItem>
                <asp:ListItem Text="one day before" Value="7"></asp:ListItem>
                <asp:ListItem Text="two days before" Value="8"></asp:ListItem>
                <asp:ListItem Text="On date of" Value="9"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent lefttext">To:</label>
        <div class="col-right-newevent righttext">
            <asp:TextBox CssClass="inputdate required" ID="txtTo" runat="server" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy',minDate:'#F{$dp.$D(\'body_bodySection_txtFrom\')}',isShowClear:false});">
            </asp:TextBox>
            <asp:TextBox CssClass="inputtime" ID="txtToTime"
                data-autocomplete="true"  data-options="12:00am,12:30am,1:00am,1:30am,2:00am,2:30am,3:00am,3:30am,4:00am,4:30am,5:00am,5:30am,6:00am
                ,6:30am,7:00am,7:30am,8:00am,8:30am,9:00am,9:30am,10:00am,10:30am,11:00am,11:30am,12:00pm,12:30pm,1:00pm,1:30pm,2:00pm,2:30pm,3:00pm
                ,3:30pm,4:00pm,4:30pm,5:00pm,5:30pm,6:00pm,6:30pm,7:00pm,7:30pm,8:00pm,8:30pm,9:00pm,9:30pm,10:00pm,10:30pm,11:00pm,11:30pm"
                data-width="100" data-itemwidth="50" data-height="150" data-filter="FilterToTime" 
                autocomplete="off"  pattern="^[0-2]?[0-9]:[0-5][0-9]\s*((am)|(pm))$" 
                Text="8:30am" Style="width: 95px;" runat="server">
            </asp:TextBox>           
            <div for="<%=txtTo.ClientID %>" class="error" style="display: none;"></div>
            <div for="<%=txtToTime.ClientID %>" class="error" style="display: none;margin-left:120px;"></div>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent lefttext">Repeat:</label>
        <div class="col-right-newevent righttext">
            <asp:DropDownList CssClass="selectw4" ID="selectRepeat" runat="server">
                <asp:ListItem Text="None" Value="1"></asp:ListItem>
                <asp:ListItem Text="Every Day" Value="2"></asp:ListItem>
                <asp:ListItem Text="Every Week" Value="3"></asp:ListItem>
                <asp:ListItem Text="Every Two Weeks" Value="4"></asp:ListItem>
                <asp:ListItem Text="Every Month" Value="5"></asp:ListItem>
                <asp:ListItem Text="Every Month's First Friday" Value="7"></asp:ListItem>
                <asp:ListItem Text="Every Year" Value="6"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group" id="trEnd" style="display: none;">
        <label class="col-left-newevent lefttext">End:</label>
        <div class="col-right-newevent2 righttext">
            <asp:DropDownList ID="ddlEnd" CssClass="selectend" runat="server">
                <asp:ListItem Text="After number of times" Value="1"></asp:ListItem>
                <asp:ListItem Text="On date" Value="2"></asp:ListItem>
            </asp:DropDownList>
            <span id="spanEnd1">
                <asp:TextBox CssClass="inputnum" value="1" Style="width: 40px;" ID="txtTimes"
                    runat="server">
                </asp:TextBox>
                Times
            </span>
            <span id="spanEnd2" style="display: none;">
                <asp:TextBox CssClass="inputdate" Width="86" ID="txtEndDate" runat="server" onFocus="WdatePicker({isShowClear:true,minDate:'#F{$dp.$D(\'body_bodySection_txtFrom\',{d:+1})}'});">
                </asp:TextBox>
            </span>
        </div>
    </div>
    
    <div class="form-group form-group-topline" >
    </div>
    
    <div class="form-group">                
        <div class="col-right-newevent2 left">
           Attendees
        </div>
    </div>
    
    <div class="form-group" id="div_Project_users">
        <label class="col-left-newevent_users lefttext">Project Users:</label>
        <div class="col-right-newevent2 righttext DOMNodeInserted" id="div_form_Users">
           
        </div>
    </div>
     <script type="text/html" id="temp_form_Users">
                <ul class="assignUser" id="ul_form_users">
                    {% if (this && this.length) { %}   
                            <li>
                                        <input name="chkProjectUser_all" type="checkbox"  id='chkProjectUser_all'  onclick="chk_projectuser_all();" />
                                        <label for="chkProjectUser_all">
                                            Select All
                                        </label>
                            </li>        
                       {% for(var i = 0; i < this.length ; i++) {%}
                                    <li>
                                        <input name="chkProjectUser" type="checkbox" value="{% this[i].UserID %}" id='chkU{% this[i].UserID %}' />
                                        <label for="chkU{% this[i].UserID %}" data-original-title="Card" data-toggle="popover" class="tipUserName" title="" href="###" data-container="body" data-placement="top" data-trigger="hover click" 
                                                data-html="true" data-content="<span class='tipUser_left'>Title:</span><span class='tipUser_right'>{% this[i].Title%}</span>
                                               <span class='tipUser_left'>Company Name:</span><span class='tipUser_right'>{% this[i].CompanyName%}</span>"> 
                                            {% this[i].<%=UserNameDisplayProp %> %}
                                        </label>
                                    </li>
                    {% } %}                
                {% } else { %}
                     <li>No Users</li>
                    {% } %} 
                </ul>
            </script>


    <div class="form-group" id="div_Other_users">
        <label class="col-left-newevent_users lefttext">Other Users:</label>
        <div class="col-right-newevent2 righttext">
            <span class="noticeRed">*</span><input placeholder="First name" style="width: 150px;" id="txtOtherUserFirst">
            <span class="noticeRed">*</span><input placeholder="Last name" style="width: 150px;" id="txtOtherUserLast">
            <input placeholder="Email" style="width: 210px;" id="txtOtherUserEmail">
            <input type="button" onclick="addOtherUser();" value="" class="addBtn" />
        </div>
    </div>
    <div class="form-group" id="div_add_otheruser">
        <input type="hidden" name="otherusers_count" id="otherusers_count"/>
      
    </div>
      <script type="text/html" id="temp_other_Users">
            {% if (this) { %}               
              <div id="div_otheruser_{%this.ID%}">
                  <label class="col-left-newevent_users lefttext"></label>
                  <div class="col-right-newevent2 righttext">
                      <label class="addUsers">{% this.FirstName %}
                          <input type="hidden" name="txtOtherUserFirst{%this.ID%}" value="{% this.FirstName %}" />
                      </label>
                      <label class="addUsers">{% this.LastName%}<input type="hidden" name="txtOtherUserLast{%this.ID%}" value="{% this.LastName %}" /></label>
                      <label class="addUsers2">{% this.Email%}<input type="hidden" id="txtOtherUserEmail{%this.ID%}" name="txtOtherUserEmail{%this.ID%}" value="{% this.Email %}" /></label>
                      <input type="button" class="delBtn" onclick="removeOtheruser('div_otheruser_{%this.ID%}');"></input>                      
                  </div>
              </div>
            {%}%}
        </script>

    <asp:HiddenField ID="Icon" runat="server" />
    <script type="text/javascript">
        var HRProjectID = '<%=HRProjectID%>';
        function FilterToTime(item) {
            var $start = $("#<%=txtFromTime.ClientID%>");
            var start = $start.val();
            if (!$start.data("optionsArray"))
                $start.data("optionsArray", $start.data("options").split(","));
            var startIndex = $.inArray(start, $start.data("optionsArray"));
            if (startIndex < 0) {
                var startTime = new NewTime(start);
                startIndex = $.inArray(startTime.toString(), $start.data("optionsArray"));
            }
            var startDate = NewDate($("#<%=txtFrom.ClientID%>").val());
            var endDate = NewDate($("#<%=txtTo.ClientID%>").val());
            var dateSpan = endDate - startDate;
            if (start) {
                if (dateSpan < 0)
                    return false;
                else if (dateSpan < 1000) {
                    return +item.value >= startIndex;
                } else {
                    return true;
                }
            }
            return false;
        }

        function chk_projectuser_all() {
            if (jQuery("#chkProjectUser_all").prop("checked")) {
                $("input[name='chkProjectUser']").prop("checked", true);
            }
            else
                $("input[name='chkProjectUser']").prop("checked", false);
        }

        var otheruserId = 1;
        function addOtherUser() {
            var tmpUserFirst = jQuery("#txtOtherUserFirst").val();
            if (tmpUserFirst == "") {
                alert("Please enter first name.");
                return;
            }
            var tmpUserLast = jQuery("#txtOtherUserLast").val();
            if (tmpUserLast == "") {
                alert("Please enter last name.");
                return;
            }
            var tmpUserEmail = jQuery("#txtOtherUserEmail").val();
            if (tmpUserEmail != "") {
                if (!/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(tmpUserEmail)) {
                    alert("Your Email Address is incorrectly formatted.");
                    return;
                }
                tmpUserEmail = tmpUserEmail.toUpperCase();
                for (var i = 1; i <= otheruserId; i++) {
                    if (jQuery("#txtOtherUserEmail" + i).length > 0) {
                        if (jQuery("#txtOtherUserEmail" + i).val().toUpperCase() == tmpUserEmail) {
                            alert(tmpUserEmail + " already exist !");
                            return;
                        }
                    }
                }
            }
            var otheruser = {
                ID: otheruserId,
                FirstName: tmpUserFirst,
                LastName: tmpUserLast,
                Email: tmpUserEmail
            };
            jQuery("#txtOtherUserFirst").val("");
            jQuery("#txtOtherUserLast").val("");
            jQuery("#txtOtherUserEmail").val("");
            var html = TemplateEngine(GetTemplateHtml("temp_other_Users"), otheruser);

            $("#div_add_otheruser").append(html);
            $("#otherusers_count").val(otheruserId);
            otheruserId++;
        }

        function removeOtheruser(id) {
            jQuery("#" + id).remove();
        }

        function binderUser() {
            if (jQuery("#<% =ddlProjects.ClientID%>").val() == "") {
                jQuery("#div_form_Users").empty();
                var html = TemplateEngine(GetTemplateHtml("temp_form_Users"), {});
                $("#div_form_Users").append(html);

                $("#div_off").attr("style", "visibility:hidden");
                $("#<% =chkOff.ClientID%>").attr("enabled", "false");
                $("#<% =chkOff.ClientID%>").removeAttr("checked");
            }
            else {
                if (jQuery("#<% =ddlProjects.ClientID%>").find("option:selected").text() == "0_PTO") {
                    var txtName = $("#<% =userName.ClientID%>").val() + " off";
                    $("#<% =txtName.ClientID%>").val(txtName);
                } else {
                    $("#<% =txtName.ClientID%>").val(null);
                    $("#<% =txtName.ClientID%>").attr("disabled", false);
                }
                jQuery.getJSON("/do/event/GetUsersByProjectId.ashx",
                    { projectId: jQuery("#<% =ddlProjects.ClientID%>").val() },
                    function (data) {
                        jQuery("#div_form_Users").empty();
                        var html = TemplateEngine(GetTemplateHtml("temp_form_Users"), data);
                        $("#div_form_Users").append(html);
                        jQuery(".tipUserName").popover();
                    });

                if ($("#<% =ddlProjects.ClientID%>").val() == HRProjectID) {
                    $("#div_off").removeAttr("style");
                    $("#<% =chkOff.ClientID%>").removeAttr("enabled");
                    $("#<% =chkOff.ClientID%>").removeAttr("disabled");
                }
                else {
                    $("#div_off").attr("style", "visibility:hidden");
                    $("#<% =chkOff.ClientID%>").attr("enabled", "false");
                    $("#<% =chkOff.ClientID%>").removeAttr("checked");
                }
            }
        }

        function maskEventFromTime(o) {
            var reg = /^[0-2]?[0-9]:[0-5][0-9]?$/;
            var v = jQuery(o).val();
            if (reg.test(v)) {
                var date = new Date(2007, 3, 30, v.split(':')[0], v.split(':')[1], 10);
                if (date) {
                    var v1 = date.Format("hh:mm");
                    var v1h = v1.split(':')[0];
                    var v1m = v1.split(':')[1];
                    if (v1h == "0" || v1h == "00")
                        v1h = "12";
                    else if (v1h > 12)
                        v1h = v1h - 12;
                    jQuery(o).val(v1h + ":" + v1m);
                }
                else
                    jQuery(o).val("12:00");
            }
            else jQuery(o).val("12:00");
        }

        function maskEventToTime(o) {
            var reg = /^[0-2]?[0-9]:[0-5][0-9]?$/;
            var v = jQuery(o).val();
            if (reg.test(v)) {
                var date = new Date(2007, 3, 30, v.split(':')[0], v.split(':')[1], 10);
                if (date) {
                    var v1 = date.format("hh:mm");
                    var v1h = v1.split(':')[0];
                    var v1m = v1.split(':')[1];
                    if (v1h == "0" || v1h == "00")
                        v1h = "12";
                    else if (v1h > 12)
                        v1h = v1h - 12;
                    jQuery(o).val(v1h + ":" + v1m);
                }
                else
                    jQuery(o).val("1:00");
            }
            else jQuery(o).val("1:00");
        }

        jQuery(function () {
            $('.eventiconitem input:radio').click(changeEventIcon);
            $('#<%=imgIcon.ClientID%>').on('click', showEventIcons);
            $('#<%=chkAllDay.ClientID%>').on('click', checkAllday);
            $('#<%=selectRepeat.ClientID%>').on('change', repeatChange);
            $('#<%=ddlEnd.ClientID%>').on('change', endChange);
            $("#<%=txtFrom.ClientID%>").on("change", function () {
                if (jQuery("#spanEnd2").css("display") !== "none") {
                    changeEndDate();
                }
            });
            jQuery("input[name='radioEventIcon']").on('click', setCurrentEventIcon);
            mouseLeaveEventIcons();
            jQuery("#<% =ddlProjects.ClientID%>").on("change", function () { binderUser(); });
            jQuery("#<% =ddlProjects.ClientID%>").change();
        });



        function mouseLeaveEventIcons() {
            var colseEventIcon = 0;
            jQuery("#divEventIcon").on("mouseleave", function () {
                colseEventIcon = setTimeout(function () { jQuery("#divEventIcon").hide(); }, 1000);
            }).on("mouseenter", function () {
                clearTimeout(colseEventIcon);
            });
        }

        function changeEventIcon() {
            var tmpIcon = jQuery(this).val();
            jQuery("#<%=imgIcon.ClientID%>").attr("src", "/Images/EventIcons/event_icon_" + tmpIcon + "s.png");
        }

        function showEventIcons() {
            jQuery("#divEventIcon").css("display", "");
        }

        function checkAllday() {
            if (jQuery("#<%=chkAllDay.ClientID%>").prop("checked")) {
                jQuery("#<%=txtFromTime.ClientID%>").hide();
                jQuery("#<%=txtToTime.ClientID%>").hide();
            } else {
                jQuery("#<%=txtFromTime.ClientID%>").show();
                jQuery("#<%=txtToTime.ClientID%>").show();
            }
        }



        function repeatChange() {
            if (jQuery('#<%=selectRepeat.ClientID%>').val() == "1") {
                jQuery("#trEnd").css("display", "none");
            } else {
                jQuery("#trEnd").css("display", "");
                endChange();
            }
        }

        function endChange() {
            if (jQuery("#<%=ddlEnd.ClientID%>").val() == "1") {
                jQuery("#spanEnd1").css("display", "");
                jQuery("#spanEnd2").css("display", "none");
            } else {
                jQuery("#spanEnd1").css("display", "none");
                changeEndDate();
            }
        }

        function changeEndDate() {
            var fromDate = new Date(jQuery("#<%=txtFrom.ClientID%>").val());
            fromDate.setDate(fromDate.getDate() + 1);
            jQuery("#spanEnd2").css("display", "").find("input[type=\"text\"]").val(fromDate.Format("MM/dd/yyyy"));
        }

        function setCurrentEventIcon() {
            if ($(this).prop('checked')) {
                $('#<%=Icon.ClientID%>').val($(this).val());
            }
        }

        function cancelAddEvent(r) {
            CloseCurrent(); //close 
            if (r)
                refreshParent();
        }

        function refreshParent() {
            window.returnValue = 0;
            if (ISModalPage !== undefined && ISModalPage === false) {
                window.location.href = window.location.href;
            }
        }

        //confirm input
        $("body").on("keyup", "input:text[confirm],textarea[confirm]", function (e) {
            var element = $(this);
            var confirm = element.attr("confirm");
            if (element.val() == confirm && !(e.ctrlKey && e.which == 90)) {
                element.data("inputEqual", true);
            } else {
                element.data("inputEqual", false);
            }
        });

        $("body").on("focus", "input:text[confirm],textarea[confirm]", function (e) {
            var element = $(this);
            var confirm = element.attr("confirm");
            if (!element.data("inputEqual")) {
                if (element.hasClass("input-confirm") && element.val() != confirm) {
                    element.removeClass("input-confirm");
                }
                if (element.hasClass("input-confirm") || element.val() == element.attr("confirm")) {
                    element.removeClass("input-confirm").val("");
                }
            }
        });

        $("body").on("blur", "input:text[confirm],textarea[confirm]", function (e) {
            var element = $(this);
            if (element.val() == "" && !element.data("inputEqual")) {
                element.addClass("input-confirm").val(element.attr("confirm"));
            }
        });

        $("input:text[confirm],textarea[confirm]").each(function (e) {
            var element = $(this);
            var confirm = element.attr("confirm");
            if (!element.val().length || (element.val().length && element.val() == confirm)) {
                element.addClass("input-confirm").val(confirm);
                element.data("inputEqual", false);
            }
        });

        function minDate() {
            if ($("#<% =txtTo.ClientID %>").val() == "") {
                $("#<% =txtTo.ClientID %>").val($("#<% =txtFrom.ClientID %>").val());
            }
            else {
                var sDate = new Date($("#<% =txtFrom.ClientID %>").val().replace(/\-/g, "\/"));
                var eDate = new Date($("#<% =txtTo.ClientID %>").val().replace(/\-/g, "\/"));
                if (sDate > eDate) {
                    $("#<% =txtTo.ClientID %>").val($("#<% =txtFrom.ClientID %>").val());
                }
            }
            if ($("#<% =txtEndDate.ClientID%>").val() != "") {
                var sDate = new Date($("#<% =txtFrom.ClientID %>").val().replace(/\-/g, "\/"));
                var eDate = new Date($("#<% =txtEndDate.ClientID %>").val().replace(/\-/g, "\/"));
                if (sDate > eDate) {
                    $("#<% =txtEndDate.ClientID %>").val($("#<% =txtFrom.ClientID %>").val());
                }
            }
        }
        function checkTime() {
              if ($("#<%=chkAllDay.ClientID%>")[0].checked == true) {
                $("#<%=btnSubmit.ClientID%>").click();
            }
            else {
                var fromTimeStr = $("#<%=txtFrom.ClientID%>").val() + " " + ($("#<%=txtFromTime.ClientID%>").val().replace("am", " am").replace("pm", " pm"));
                var fromTime = new Date("" + fromTimeStr + "");
                var toTimeStr = $("#<%=txtTo.ClientID%>").val() + " " + ($("#<%=txtToTime.ClientID%>").val().replace("am", " am").replace("pm", " pm"));
                var toTime = new Date("" + toTimeStr + "");
                if (toTime > fromTime) {
                    $("#<%=btnSubmit.ClientID%>").click();
                }
                else {
                    alert("Please select correct From Time and To Time!");
                }
            }
        }
        function beforeSavefn() {
           if ('<%=NotEnoughPTOHour%>' === '<%=true%>' && $("#<%=ddlProjects.ClientID%> option:selected").text().indexOf('PTO') > -1) {
                jQuery.confirm("You do not have enough PTO credit for this off. Further extra off will result salary deduction. Please only use your remaining PTO hours and talk to your manager for the extra off", {
                    yesText: "Continue",
                    yesCallback: checkTime,
                    noText: "Cancel"
                });
            } else {
               checkTime();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button ID="btnSubmit" runat="server" CssClass="saveBtn1 mainbutton" Text="Save" OnClick="btnSave_Click" Style="display: none;" />
    <input type="button" class="saveBtn1 mainbutton" onclick="beforeSavefn()" value="Save" />
    <input name="btnCancel" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
</asp:Content>
