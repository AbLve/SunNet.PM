<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DayEvents.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Events.DayEvents" %>

<!DOCTYPE html>
<html>
<head>
    <style type="text/css">
        .mainowBox {
            background-color: #FFFFFF;
            background-image: url("/Images/signbox_bg.png");
            background-position: center top;
            background-repeat: repeat-x;
            border: 1px solid #CCCCCC;
            border-radius: 5px;
            box-shadow: 0 0 10px #939393;
            margin: 0 auto;
            padding: 5px;
            width: 420px;
        }

        .mainowBoxtop {
            height: 25px;
            padding: 10px 0 5px 10px;
        }

        .mainowConbox {
            font-size: 13px;
            height: auto;
            margin: 15px 0;
            min-height: 200px;
            overflow: hidden;
            padding: 0 5px 0 15px;
            width: auto;
        }

        .mainowBoxtop_title {
            color: #04466A;
            float: left;
            font-size: 16px;
            font-weight: bold;
            width: 70%;
        }

        .mainowBoxtop_close {
            cursor: pointer;
            float: right;
            padding-right: 15px;
            width: 24px;
        }

        .alleventTable {
            margin-bottom: 10px;
        }

        .alleventDate {
            color: #666666;
            padding-top: 3px;
            vertical-align: top;
        }

        .eventlistTitle2 {
            font-size: 13px;
            line-height: 18px;
            padding: 5px 8px 0;
        }

        .eventlistBox2 {
            border: 1px solid #DDDDDD;
            border-radius: 5px;
            color: #666666;
            overflow: auto;
        }
    </style>
</head>
<body>
    <div class="modal-dialog">
        <div class="modal-content" style="width: 100%;">
            <div class="mainowBox" style="width: 100%;">
                <div class="mainowBoxtop" style="width: 100%;">
                    <div class="mainowBoxtop_title">
                        <%=title %>
                    </div>
                    <div class="mainowBoxtop_close" data-dismiss="modal" aria-hidden="true" title="Close">
                        <img src="/Images/close.png" alt="&times;" />
                    </div>
                </div>
                <div class="mainowConbox" style="min-height: 80px;">
                    <% if (dayEvents.Count() == 0)
                       {%>
                    <div class="mainowConbox" style="min-height: 80px;">
                        <div class="ownothingText">
                            Nothing was scheduled for this day.
                        </div>
                    </div>
                    <% }%>
                    <% else
                       {%>
                    <%  foreach (SunNet.PMNew.Entity.EventModel.EventEntity entity in dayEvents)
                        { %>
                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="alleventTable">
                        <tbody>
                            <tr>
                                <%if (entity.AllDay)
                                  {  %>

                                <td width="21%" align="right" class="alleventDate">All-day<br>
                                    <img width="24" height="24" style="width: 24px; height: 24px;" src="<%=entity.IconPath %>">
                                </td>
                                <% }
                                  else
                                  { %>
                                <% if (entity.FromTimeType == 1)
                                   { %>
                                <td width="21%" align="right" class="alleventDate"><%=entity.FromTime%> AM<br>
                                    <img width="24" height="24" style="width: 24px; height: 24px;" src="<%=entity.IconPath %>">
                                </td>
                                <% }
                                   else
                                   { %>
                                <td width="21%" align="right" class="alleventDate"><%=entity.FromTime%> PM<br>
                                    <img width="24" height="24" style="width: 24px; height: 24px;" src="<%=entity.IconPath%>">
                                </td>
                                <%}
                                  } %>
                                <td width="79%" class="itemcontent">
                                    <div class="eventlistBox2">
                                        <div class="eventlistTitle2">
                                            <% if (isEdit)
                                               { %>
                                            <a href="javascript:void(0)" onclick="OpenEditSchdules(<%=entity.ID%>,1)"><%=entity.Name %></a>
                                            <%}
                                               else
                                               {%>
                                            <a href="javascript:void(0)" onclick="OpenEditSchdules(<%=entity.ID%>,2)"><%=entity.Name %></a>
                                            <%} %>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                    </table>
                    <%} %>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function OpenEditSchdules(id, canEdit) {
            var result = ShowIFrame("/Sunnet/Events/EditEvent.aspx?ID=" + id + "&c=" + canEdit, 382, 409, true, "Edit Schedules");
            if (result == 0) {
                document.getElementById('ctl00_ContentPlaceHolder1_iBtnSearch').click();
            }
        }

    </script>
</body>
</html>
