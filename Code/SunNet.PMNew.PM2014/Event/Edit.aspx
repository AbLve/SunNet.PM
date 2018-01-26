<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pop.master" CodeBehind="Edit.aspx.cs" ClientIDMode="Predictable" Inherits="SunNet.PMNew.PM2014.Event.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <style type="text/css">
        .mr250
        {
            margin-right: 250px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
            jQuery(".tipUserName").popover();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Edit Event
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <div class="form-group">
        <label class="col-left-newevent lefttext">Project:<span class="noticeRed">*</span></label>
        <div class="col-right-newevent righttext">
            <asp:Literal ID="litProject" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent lefttext">Title:<span class="noticeRed">*</span></label>
        <div class="col-right-newevent righttext">
            <asp:TextBox CssClass="inputw3 required" Width="217" placeholder="Add Title" ID="txtName" runat="server"></asp:TextBox>
            <asp:Image runat="server" ID="imgIcon" align="absmiddle" />
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
            <asp:TextBox CssClass="inputevent1" ID="txtWhere" placeholder="Add a Place" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent lefttext">Detail:</label>
        <div class="col-right-newevent2 righttext">
            <asp:TextBox TextMode="MultiLine" ID="txtDetails" CssClass="inputevent1" Rows="4" placeholder="Add Event Detail" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="form-group">
        <label class="col-left-newevent lefttext">All-day:</label>
        <div class="col-right-newevent righttext">
            <asp:CheckBox ID="chkAllDay" runat="server" />
        </div>
    </div>
    <div class="form-group" id="div_off" runat="server">
        <label class="col-left-newevent lefttext">Off:</label>
        <div class="col-right-newevent righttext">
            <asp:CheckBox ID="chkOff" runat="server" />
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent lefttext">From:</label>
        <div class="col-right-newevent righttext">
            <asp:TextBox CssClass="inputdate required" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy',minDate:'#F{$dp.$D(\'body_bodySection_TxtFromHide\')}',isShowClear:false});" onblur="minDate()" ID="txtFrom" runat="server">
            </asp:TextBox>
            <asp:TextBox ID="TxtFromHide" CssClass="hide" runat="server"></asp:TextBox>
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
                autocomplete="off" pattern="^[0-2]?[0-9]:[0-5][0-9]\s*((am)|(pm))$"
                Text="1:00pm" Style="width: 95px;" runat="server">
            </asp:TextBox>  
            <div for="<%=txtTo.ClientID %>" class="error" style="display: none;"></div>
            <div for="<%=txtToTime.ClientID %>" class="error" style="display: none;margin-left:120px;"></div>
        </div>
    </div>
    <div class="form-group form-group-topline" >
    </div>


    <div class="form-group">                
        <div class="col-right-newevent2 left">
           Attendees
        </div>
    </div>

    <div class="form-group">
        <label class="col-left-newevent_users lefttext">Project Users:</label>
        <div class="col-right-newevent2 righttext DOMNodeInserted" id="div_form_Users">
            <ul class="assignUser" id="ul_form_users">
                <asp:Repeater ID="rptInviteUser" runat="server">
                    <HeaderTemplate>
                        <li>
                            <input name="chkProjectUser_all" type="checkbox" id='chkProjectUser_all' onclick="chk_projectuser_all();" />
                            <label for="chkProjectUser_all">
                                Select All
                            </label>
                        </li>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <input name="chkProjectUser" type="checkbox" value="<%# Eval("UserID") %>" id='chkU<%# Eval("UserID") %>' <%# (bool)Eval("IsSeleted")? "checked='checked'":"" %> />
                            <label for="chkU<%# Eval("UserID") %>" data-original-title="Card" data-toggle="popover" class="tipUserName" title="" href="###" data-container="body" data-placement="top" data-trigger="hover click"
                                                data-html="true" data-content="
                                               <span class='tipUser_left'>Title:</span><span class='tipUser_right'><%# Eval("Title") %></span>
                                               <span class='tipUser_left'>Company Name:</span><span class='tipUser_right'><%# Eval("CompanyName") %></span>                                               
                                               "> <%# Eval(UserNameDisplayProp)%> 
                            </label>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="litNoUser" runat="server"></asp:Literal>
            </ul>
        </div>
    </div>

    <div class="form-group">
        <label class="col-left-newevent_users lefttext">Other Users:</label>
        <div class="col-right-newevent2 righttext">
            <span class="noticeRed">*</span><input placeholder="First name" style="width: 150px;" id="txtOtherUserFirst">
            <span class="noticeRed">*</span><input placeholder="Last name" style="width: 150px;" id="txtOtherUserLast">
            <input placeholder="Email" style="width: 210px;" id="txtOtherUserEmail">
            <input type="button" onclick="addOtherUser();" value="" class="addBtn" />
        </div>
    </div>
    <div class="form-group" id="div_add_otheruser">
        <input type="hidden" name="otherusers_count" id="otherusers_count" />
        <asp:Repeater ID="rptOtherUser" runat="server">
            <ItemTemplate>
                <div id="div_otheruser_<%# Container.ItemIndex + 1 %>">
                    <label class="col-left-newevent_users lefttext"></label>
                    <div class="col-right-newevent2 righttext">
                        <label class="addUsers">
                            <%# Eval("FirstName") %>
                            <input type="hidden" name="txtOtherUserFirst<%# Container.ItemIndex + 1 %>" value="<%# Eval("FirstName") %>" />
                        </label>
                        <label class="addUsers"><%# Eval("LastName") %><input type="hidden" name="txtOtherUserLast<%# Container.ItemIndex + 1 %>" value=" <%# Eval("LastName") %>" /></label>
                        <label class="addUsers2"><%# Eval("Email") %><input type="hidden" name="txtOtherUserEmail<%# Container.ItemIndex + 1 %>" value=" <%# Eval("Email") %>" /></label>
                        <input type="button" class="delBtn" onclick="removeOtheruser('div_otheruser_<%# Container.ItemIndex + 1 %>    ');"></input>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

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
    </div>

    <asp:HiddenField ID="Icon" runat="server" />
    <script type="text/javascript">
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

        var otheruserId = '<% = rptOtherUser.Items.Count %>';
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
                for(var i = 1;i<= otheruserId;i++)
                {
                    if (jQuery("#txtOtherUserEmail" + i).length >0) {
                        if( jQuery("#txtOtherUserEmail" + i).val().toUpperCase() == tmpUserEmail){
                            alert(tmpUserEmail+ " already exist !");
                            return;
                        }
                    }
                }
            }
            otheruserId++;
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
        }

        function removeOtheruser(id) {
            jQuery("#" + id).remove();
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
            jQuery("input[name='radioEventIcon']").on('click', setCurrentEventIcon);
            //disableDateInput();
            mouseLeaveEventIcons();

            jQuery("#btnDelete1").click(function () {
                jQuery.confirm("Are you sure you want to delete this event?", {
                    yesText: "Delete",
                    yesCallback: function () {
                        jQuery("#<%=btnDelete.ClientID%>").click();
                    },
                    noText: "Cancel"
                });
            });
            jQuery("#btnDelete2").click(function () {
                jQuery.confirm("Are you sure you want to delete this only or delete all in the future?", 
                    [{
                        text:"Delete this only",
                        className:"btn-danger",
                        callback:function(){jQuery("#<%=btnDelete.ClientID%>").click();}
                    },
                {
                    text:"Delete all in the future",
                    className:"btn-danger",
                    callback:function(){jQuery("#<%=btnDeleteAll.ClientID%>").click();}
                },
                {
                    text:"Cancel",
                    className:"btn-default"
                }]);
            });

            jQuery("#btnSave1").click(function () {
                jQuery.confirm("Are you sure you want to update this event?", {
                    yesText: "Update",
                    yesCallback: function () {
                        jQuery("#<%=btnSave.ClientID%>").click();
                    },
                    noText: "Cancel"
                });                             
            });
            jQuery("#btnSave2").click(function () {
                jQuery.confirm("Are you sure you want to update this only or update all in the future?", 
                                [{
                                    text:"Update this only",
                                    className:"btn-danger",
                                    callback:function(){jQuery("#<%=btnSave.ClientID%>").click();}
                                },
                    {
                        text:"Update all in the future",
                        className:"btn-danger",
                        callback:function(){jQuery("#<%=btnSaveAll.ClientID%>").click();}
                    },
                    {
                        text:"Cancel",
                        className:"btn-default"
                    }]);               
            });


            $("#otherusers_count").val(otheruserId);
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
                jQuery("#<%=imgIcon.ClientID%>").attr("src", "/Images/eventicons/event_icon_" + tmpIcon + "s.png");
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

            function setCurrentEventIcon() {
                if ($(this).prop('checked')) {
                    $('#<%=Icon.ClientID%>').val($(this).val());
                }
            }

            function disableDateInput() {
                jQuery('#<%=txtFrom.ClientID%>,#<%= txtTo.ClientID%>').attr('readonly', 'readonly');
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
            }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <% if (!OnlyRead)
       { %>
    <asp:Button CssClass="saveBtn1 mainbutton mr250" Text="Delete" ID="btnDelete" runat="server" OnClick="btnDelete_Click" Style="display: none;" />
    <asp:Button CssClass="saveBtn1 mainbutton mr250" Text="Delete" ID="btnDeleteAll" runat="server" OnClick="btnDeleteAll_Click" Style="display: none;" />
    <asp:Button ID="btnSave" runat="server" CssClass="saveBtn1 mainbutton" Text="Save" 
        OnClick="btnSave_Click"  Style="display: none;" />
    <asp:Button ID="btnSaveAll" runat="server" CssClass="saveBtn1 mainbutton"  Text="Save"
        OnClick="btnSaveAll_Click"  Style="display: none;"/>

    <% if (Times <= 1)
       {%>
    <input type="button" class="saveBtn1 mainbutton" value="Save" id="btnSave1" />
    <%}
       else
       {%>
    <input type="button" class="saveBtn1 mainbutton" value="Save" id="btnSave2"/>
    <%} %>


    <% if (Times <= 1)
       {%>
    <input type="button" class="saveBtn1 mainbutton" value="Delete" id="btnDelete1" />
    <%}
       else
       {%>
    <input type="button" class="saveBtn1 mainbutton" value="Delete" id="btnDelete2" />
    <%} %>
    <input name="btnCancel" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
    <%} %>
</asp:Content>
