<%@ Page Language="C#" MasterPageFile="~/OA/OA.master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="WeekPlanEdit.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.WeekPlan.WeekPlanEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function ()
        {
            $("body").on("hide.bs.modal", ".modal", function ()
            {
                var $this = $(this);
                var frameWindow = $this.find("iframe")[0].contentWindow;
                if (frameWindow.returnValue == null)
                    return;
                else
                    SetTickets(frameWindow && frameWindow.returnValue);
            });
        });
        function SetTickets(str)
        {
            var returnVal = str.split('&');
            var dayName = "";//
            var tickIds = "";
            if (returnVal.length == 2)
            {
                dayName = returnVal[0];
                tickIds = returnVal[1].split('_');
            }
            switch (dayName)
            {
                case "monday":
                    ShowTicket($("#divTicketMonday"), tickIds); break;
                case "tuesday":
                    ShowTicket($("#divTicketTuesday"), tickIds); break;
                case "wednesday":
                    ShowTicket($("#divTicketWednesday"), tickIds); break;
                case "thursday":
                    ShowTicket($("#divTicketThursday"), tickIds); break;
                case "friday":
                    ShowTicket($("#divTicketFriday"), tickIds); break;
                case "saturday":
                    ShowTicket($("#divTicketSaturday"), tickIds); break;
                case "sunday":
                    ShowTicket($("#divTicketSunday"), tickIds); break;
            }
        }

        function DeleteItem(obj)
        {
            $(obj).parent().remove();
        }

        function ShowTicket(dayItem, ticketIds)
        {
            //divTicketMonday
            var i = 0;
            var interalHtml = "";
            for (i = 0; i < ticketIds.length; i++)
            {
                if (ticketIds[i] != "")
                {
                    var linkList = dayItem.find("a");
                    if (linkList.length == 0)
                    {
                        interalHtml += "<span id=\"span" + ticketIds[i] + "\"><a value=" + ticketIds[i] + " id=\"ticket" + ticketIds[i] + "\"  target=\"blank\" href=\"/SunnetTicket/Detail.aspx?tid=" + ticketIds[i] + "&returnurl=/OA/WeekPlan/WeekPlanEdit.aspx\">" + ticketIds[i]
                                           + "</a>  <img id=\"img" + ticketIds[i] + "\" src=\"/Images/icons/close.png\" onclick=\"DeleteItem(this)\"/></span>";
                    }
                    else
                    {
                        for (var n = 0; n < linkList.length; n++)
                        {
                            if ($(linkList[n]).attr("value") == ticketIds[i])
                            {
                                break;
                            }
                            else if (n == linkList.length - 1)
                            {
                                interalHtml += "<span id=\"span" + ticketIds[i] + "\"><a value=" + ticketIds[i] + " id=\"ticket" + ticketIds[i] + "\" target=\"blank\" href=\"/SunnetTicket/Detail.aspx?tid=" + ticketIds[i] + "&returnurl=/OA/WeekPlan/WeekPlanEdit.aspx\">" + ticketIds[i]
                                            + "</a>  <img id=\"img" + ticketIds[i] + "\" src=\"/Images/icons/close.png\" onclick=\"DeleteItem(this)\"/></span>";
                            }
                        }
                    }
                }
            }
            dayItem.append(interalHtml);
        }

        function SetHidenFields()
        {
            SubSetHidenFields("divTicketMonday", $("#<%= mondayTicketIds.ClientID %>"));
            SubSetHidenFields("divTicketTuesday", $("#<%= tuesdayTicketIds.ClientID %>"));
            SubSetHidenFields("divTicketWednesday", $("#<%= wednesdayTicketIds.ClientID %>"));
            SubSetHidenFields("divTicketThursday", $("#<%= thursdayTicketIds.ClientID %>"));
            SubSetHidenFields("divTicketFriday", $("#<%= fridayTicketIds.ClientID %>"));
            SubSetHidenFields("divTicketSaturday", $("#<%= saturdayTicketIds.ClientID %>"));
            SubSetHidenFields("divTicketSunday", $("#<%= sundayTicketIds.ClientID %>"));
            //mondayTicketIds
        }

        function SubSetHidenFields(obj, hideField)
        {
            var str = "";
            $("#" + obj + " a").each(function (index, obj)
            {
                str += $(obj).attr("value") + "_";
            });
            hideField.val(str);
        }

        function GetTickets()
        {
            //SetTickets();
            SetTickets("monday" + "&" + $("#<%= mondayTicketIds.ClientID%>").val());
            SetTickets("tuesday" + "&" + $("#<%= tuesdayTicketIds.ClientID%>").val());
            SetTickets("wednesday" + "&" + $("#<%= wednesdayTicketIds.ClientID%>").val());
            SetTickets("thursday" + "&" + $("#<%= thursdayTicketIds.ClientID%>").val());
            SetTickets("friday" + "&" + $("#<%= fridayTicketIds.ClientID%>").val());
            SetTickets("saturday" + "&" + $("#<%= saturdayTicketIds.ClientID%>").val());
            SetTickets("sunday" + "&" + $("#<%= sundayTicketIds.ClientID%>").val());
        }

        $(function ()
        {
            GetTickets();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <style type="text/css">
        textarea {
            resize: none;
            width: 456px;
            height: 86px;
            max-width: 456px;
            max-height: 86px;
        }

        .col-right-weekplan2 {
            width: 130px;
            padding-top: 3px;
        }

        .col-right-weekplan3 {
            width: 360px;
        }

        .weekplan-con {
            width: 500px;
            min-height: 100px;
            float: left;
            overflow: hidden;
        }

        .col-right-weekplan3 span {
            display: inline-block;
            margin-right: 15px;
            min-width: 55px;
            border-bottom: 1px #ddd dashed;
            line-height: 24px;
            margin-top: 5px;
        }

        .weekplan-t-btn {
            background-color: #036ac9;
            border-radius: 3px;
            color: #ffffff;
            padding: 2px 5px;
        }

            .weekplan-t-btn:hover {
                color: #ffffff;
                background-color: #1e90ff;
            }
    </style>
    <div class="contentTitle titlsealrequest">
        <label runat="server" id="lblTitle"></label>
    </div>
    <div>
        <div class="form-group" style="min-width: 600px;">
            <label class="col-left-weekplan lefttext">Week:<span class="noticeRed"></span></label>
            <div class="col-right-weekplan righttext">
                <span class="rightItem">
                    <label id="lblFrontWeek" runat="server"></label>
                </span>
                <span class="rightItem">
                    <label id="lblNextWeek" runat="server"></label>
                </span>
                <asp:HiddenField ID="hfID" runat="server" />
            </div>
        </div>
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <% if (!IsEdit)
                   {%>
                <div class="form-group">
                    <label class="col-left-weekplan lefttext">Import:<span class="noticeRed"></span></label>
                    <div class="col-right-weekplan1 righttext">
                        <ul class="listtopBtn">
                            <%--<li>
                        <div class="listtopBtn_icon import">
                            <img src="/Images/wimport.png" style="cursor:pointer;" onclick="javascript:$('#tImport').show();$('.import').hide();"/>
                        </div>
                        <div class="listtopBtn_text import" onclick="javascript:$('#tImport').show();$('.import').hide();">Import</div>
                    </li>--%>
                            <li>
                                <asp:DropDownList ID="ddlImport" runat="server" OnSelectedIndexChanged="ddlImport_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </li>
                        </ul>
                    </div>
                </div>
                <%}%>
                <div class="form-group">
                    <label class="col-left-weekplan lefttext">Monday:</label>
                    <div class="col-right-weekplan1 righttext">
                        <asp:TextBox ID="txtMonday" runat="server" TextMode="MultiLine" CssClass="inputweekplan" Rows="4"></asp:TextBox>
                    </div>
                    <div class="weekplan-con">
                        <div class="col-right-weekplan2 righttext">
                            Estimate:
                        <asp:DropDownList ID="ddlMondayEst" runat="server" onchange="javascript:$('[attrFlag=MondayRemaining]').val(24-$(this).val());">
                        </asp:DropDownList>
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            Remaining : 
                        <input type="text" runat="server" id="txtMondayRemaining" attrflag="MondayRemaining" readonly="true" class="inputProfle1" style="width: 20px;" />
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            <a class="listtopBtn_text weekplan-t-btn" style="text-decoration: none" href="/SunnetTicket/TicketSelect.aspx?day=monday" data-target="#modalsmall"
                                data-toggle="modal">Select Tickets</a>
                        </div>
                        <div class="col-right-weekplan3 righttext" id="divTicketMonday">
                        </div>
                        <asp:HiddenField runat="server" ID="mondayTicketIds" />
                    </div>


                </div>
                <div class="form-group">
                    <label class="col-left-weekplan lefttext">Tuesday:</label>
                    <div class="col-right-weekplan1 righttext">
                        <asp:TextBox ID="txtTuesday" runat="server" TextMode="MultiLine" CssClass="inputweekplan" Rows="4"></asp:TextBox>
                    </div>
                    <div class="weekplan-con">
                        <div class="col-right-weekplan2 righttext">
                            Estimate:
                    <asp:DropDownList ID="ddlTuesdayEst" runat="server" onchange="javascript:$('[attrFlag=TuesdayRemaining]').val(24-$(this).val());">
                    </asp:DropDownList>
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            Remaining : 
                    <input type="text" runat="server" id="txtTuesdayRemaining" attrflag="TuesdayRemaining" readonly="true" class="inputProfle1" style="width: 20px;" />
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            <a class="listtopBtn_text weekplan-t-btn" style="text-decoration: none" href="/SunnetTicket/TicketSelect.aspx?day=tuesday" data-target="#modalsmall"
                                data-toggle="modal">Select Tickets</a>
                        </div>
                        <div class="col-right-weekplan3 righttext" id="divTicketTuesday">
                        </div>
                        <asp:HiddenField runat="server" ID="tuesdayTicketIds" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-left-weekplan lefttext">Wednesday:</label>
                    <div class="col-right-weekplan1 righttext">
                        <asp:TextBox ID="txtWednesday" runat="server" TextMode="MultiLine" CssClass="inputweekplan" Rows="4"></asp:TextBox>
                    </div>
                    <div class="weekplan-con">
                        <div class="col-right-weekplan2 righttext">
                            Estimate:
                    <asp:DropDownList ID="ddlWednesdayEst" runat="server" onchange="javascript:$('[attrFlag=WednesdayRemaining]').val(24-$(this).val());">
                    </asp:DropDownList>
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            Remaining : 
                    <input type="text" runat="server" id="txtWednesdayRemaining" attrflag="WednesdayRemaining" readonly="true" class="inputProfle1" style="width: 20px;" />
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            <a class="listtopBtn_text weekplan-t-btn" style="text-decoration: none" href="/SunnetTicket/TicketSelect.aspx?day=wednesday" data-target="#modalsmall"
                                data-toggle="modal">Select Tickets</a>
                        </div>
                        <div class="col-right-weekplan3 righttext" id="divTicketWednesday">
                        </div>
                        <asp:HiddenField runat="server" ID="wednesdayTicketIds" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-left-weekplan lefttext">Thursday:</label>
                    <div class="col-right-weekplan1 righttext">
                        <asp:TextBox ID="txtThursday" runat="server" TextMode="MultiLine" CssClass="inputweekplan" Rows="4"></asp:TextBox>
                    </div>
                    <div class="weekplan-con">
                        <div class="col-right-weekplan2 righttext">
                            Estimate:
                    <asp:DropDownList ID="ddlThursdayEst" runat="server" onchange="javascript:$('[attrFlag=ThursdayRemaining]').val(24-$(this).val());">
                    </asp:DropDownList>
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            Remaining : 
                    <input type="text" runat="server" id="txtThursdayRemaining" attrflag="ThursdayRemaining" readonly="true" class="inputProfle1" style="width: 20px;" />
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            <a class="listtopBtn_text weekplan-t-btn" style="text-decoration: none" href="/SunnetTicket/TicketSelect.aspx?day=thursday" data-target="#modalsmall"
                                data-toggle="modal">Select Tickets</a>
                        </div>
                        <div class="col-right-weekplan3 righttext" id="divTicketThursday">
                        </div>
                        <asp:HiddenField runat="server" ID="thursdayTicketIds" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-left-weekplan lefttext">Friday: </label>
                    <div class="col-right-weekplan1 righttext">
                        <asp:TextBox ID="txtFriday" runat="server" TextMode="MultiLine" CssClass="inputweekplan" Rows="4"></asp:TextBox>
                    </div>
                    <div class="weekplan-con">
                        <div class="col-right-weekplan2 righttext">
                            Estimate:
                    <asp:DropDownList ID="ddlFridayEst" runat="server" onchange="javascript:$('[attrFlag=FridayRemaining]').val(24-$(this).val());">
                    </asp:DropDownList>
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            Remaining : 
                    <input type="text" runat="server" id="txtFridayRemaining" attrflag="FridayRemaining" readonly="true" class="inputProfle1" style="width: 20px;" />
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            <a class="listtopBtn_text weekplan-t-btn" style="text-decoration: none" href="/SunnetTicket/TicketSelect.aspx?day=friday" data-target="#modalsmall"
                                data-toggle="modal">Select Tickets</a>
                        </div>
                        <div class="col-right-weekplan3 righttext" id="divTicketFriday">
                        </div>
                        <asp:HiddenField runat="server" ID="fridayTicketIds" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-left-weekplan lefttext">Saturday: </label>
                    <div class="col-right-weekplan1 righttext">
                        <asp:TextBox ID="txtSaturday" runat="server" TextMode="MultiLine" CssClass="inputweekplan" Rows="4"></asp:TextBox>
                    </div>
                    <div class="weekplan-con">
                        <div class="col-right-weekplan2 righttext">
                            Estimate:
                    <asp:DropDownList ID="ddlSaturdayEst" runat="server" onchange="javascript:$('[attrFlag=SaturdayRemaining]').val(24-$(this).val());">
                    </asp:DropDownList>
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            Remaining : 
                    <input type="text" runat="server" id="txtSaturdayRemaining" attrflag="SaturdayRemaining" readonly="true" class="inputProfle1" style="width: 20px;" />
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            <a class="listtopBtn_text weekplan-t-btn" style="text-decoration: none" href="/SunnetTicket/TicketSelect.aspx?day=saturday" data-target="#modalsmall"
                                data-toggle="modal">Select Tickets</a>
                        </div>
                        <div class="col-right-weekplan3 righttext" id="divTicketSaturday">
                        </div>
                        <asp:HiddenField runat="server" ID="saturdayTicketIds" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-left-weekplan lefttext">Sunday: </label>
                    <div class="col-right-weekplan1 righttext">
                        <asp:TextBox ID="txtSunday" runat="server" TextMode="MultiLine" CssClass="inputweekplan" Rows="4"></asp:TextBox>
                    </div>
                    <div class="weekplan-con">
                        <div class="col-right-weekplan2 righttext">
                            Estimate:
                    <asp:DropDownList ID="ddlSundayEst" runat="server" onchange="javascript:$('[attrFlag=SundayRemaining]').val(24-$(this).val());">
                    </asp:DropDownList>
                        </div>
                        <div class="col-right-weekplan2 righttext">
                            Remaining : 
                    <input type="text" runat="server" id="txtSundayRemaining" attrflag="SundayRemaining" readonly="true" class="inputProfle1" style="width: 20px;" />
                        </div>

                        <div class="col-right-weekplan2 righttext">
                            <a class="listtopBtn_text weekplan-t-btn" style="text-decoration: none" href="/SunnetTicket/TicketSelect.aspx?day=sunday" data-target="#modalsmall"
                                data-toggle="modal">Select Tickets</a>
                        </div>
                        <div class="col-right-weekplan3 righttext" id="divTicketSunday">
                        </div>
                        <asp:HiddenField runat="server" ID="sundayTicketIds" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="buttonBox2">
            <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" ValidationGroup="Add" CausesValidation="true"
                runat="server" Text="Save" OnClientClick="SetHidenFields()" OnClick="btnSave_Click" />
            <input type="button" value=" Back " class="cancelBtn1 mainbutton redirectback" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
