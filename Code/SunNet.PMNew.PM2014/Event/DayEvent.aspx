<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pop.master" CodeBehind="DayEvent.aspx.cs" Inherits="SunNet.PMNew.PM2014.Event.DayEvent" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .mainowBox {
            background-position: center top;
            background-repeat: repeat-x;
            box-shadow: 0 0 10px #939393;
            padding: 5px;
            width: 420px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    <%=title %>
</asp:Content>
<asp:Content ID="body" ContentPlaceHolderID="bodySection" runat="server">
    <% if (dayEvents.Count() == 0)
       {%>
    <div class="ownothingText">
        Nothing was scheduled for this day.
    </div>
    <% }%>
    <% else
       {%>
    <%  foreach (SunNet.PMNew.Entity.EventModel.EventEntity entity in dayEvents)
        { %>

    <div class="form-group">

        <%if (entity.AllDay)
          {  %>
        <label class="col-left-newevent lefttext">All-day</label>
        <% }
          else
          { %>
        <% if (entity.FromTimeType == 1)
           { %>
        <label class="col-left-newevent lefttext"><%=entity.FromTime%> AM</label>
        <% }
           else
           { %>
        <label class="col-left-newevent lefttext"><%=entity.FromTime%> PM</label>
        <%}
          } %>


        <div class="col-right-newevent righttext">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="28" valign="top"><a href="#">
                        <img src="<%=entity.IconPath %>"></a></td>
                    <td><a href="Edit.aspx?ID=<%=entity.ID%>&c=true"><%=entity.Name %></a></td>
                </tr>
            </table>
        </div>
        <%} %>
        <% } %>
    </div>

</asp:Content>
