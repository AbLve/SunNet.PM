<%@ Page Title="" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SunNet.PMNew.PM2014.WeekPlan.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    @media (max-width:992px) {
        .toploginInfo{
            min-width:auto;
        }
        .topBox{
            min-width:auto;
            height:auto;
        }
        .topBox_logo img{
            width:100%;
        }
        ul.topmenu li {
            width: 80px;
            height: 55px;
            font-size: 12px;
            font-weight: 500;
            padding-top: 15px;
            margin-right: 5px;
        }
        ul.topmenu li .image{
            width: 19px;
            height: 20px;
            background-size: 100% 100% !important;
        }
        .mainleftTd{
            width:193px;
        }
        .leftmenuBox{
            width:180px;
        }
        .mainrightBox {
            min-width: auto;
            padding:10px;
        }
        .footerBox{
            min-width:auto;
        }
        .footerBox_left{
            width:auto;
        }
        .footerBox_right{
            width:auto;
        }
    }
    </style>
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="30">User:</td>
                <td width="145">
                    <asp:DropDownList ID="ddlUsers" queryparam="user" runat="server" CssClass="selectw1">
                    </asp:DropDownList>
                </td>
                <td width="30">Date:</td>
                <td width="140">
                    <asp:TextBox ID="txtDate" queryparam="date" onclick="WdatePicker({isShowClear:false});"
                        CssClass="inputsearchdate" runat="server"></asp:TextBox>
                </td>
                <td width="30">
                    <input type="button" class="searchBtn" id="btnSearch" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="topbtnbox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="17%">
                            <ul class="listtopBtn">
                                <li>
                                    <div class="listtopBtn_icon">
                                        <a href="WeekPlanEdit.aspx?returnurl=<%=this.ReturnUrl %>">
                                            <img src="/Images/icons/wplan.png" /></a>
                                    </div>
                                    <div class="listtopBtn_text"><a href="WeekPlanEdit.aspx?returnurl=<%=this.ReturnUrl %>">New Plan </a></div>
                                </li>
                            </ul>
                        </td>
                        <td align="center">
                            <table border="0" cellspacing="0" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <td style="width: 40px;">
                                            <asp:ImageButton runat="server" ID="ibtnFrontWeek" ToolTip="Last Week" BorderWidth="0" ImageUrl="/Images/monthleft.png" OnClick="ibtnFrontWeek_Click" />
                                        </td>
                                        <td width="210px" align="center" id="spnMonth">
                                            <asp:Label ID="lblStartDate" runat="server" Style="">06/09/2014</asp:Label>
                                            ~ 
                                            <asp:Label ID="lblEndDate" runat="server">06/15/2014</asp:Label>
                                        </td>
                                        <td style="width: 28px;"></td>
                                        <td style="width: 40px;">
                                            <asp:ImageButton runat="server" ID="ibtnNextWeek" ToolTip="Next Week" BorderWidth="0" ImageUrl="/Images/monthright.png" OnClick="ibtnNextWeek_Click" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td width="17%" align="right">
                            <span class="topbtn toptodayBtn" onclick="javascript:location.href='Index.aspx'">&gt;&gt; Today</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3"></td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%; overflow: auto; min-height: 360px;">
                <table width="140%" border="0" cellspacing="0" cellpadding="0" class="table-advance">
                    <tr class="topweek">
                        <div runat="server" id="dvHead"></div>
                    </tr>
                    <tr runat="server" id="trNoRecords" visible="false">
                        <th colspan="8" style="color: Red;">&nbsp; No record found.
                        </th>
                    </tr>
                    <asp:Repeater ID="rptList" runat="server">
                        <ItemTemplate>
                            <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> <%# int.Parse(Eval("UserID").ToString())==UserInfo.UserID?"":"action"%>'>
                                <td style="color: #0341b3; font-weight: bold;"><%# Eval(UserNameDisplayProp)%></td>
                                <td align="center"><%# 40-(Convert.ToInt32(Eval("MondayEstimate"))+Convert.ToInt32(Eval("TuesdayEstimate"))+Convert.ToInt32(Eval("WednesdayEstimate"))
                                    +Convert.ToInt32(Eval("ThursdayEstimate"))+Convert.ToInt32(Eval("FridayEstimate"))+Convert.ToInt32(Eval("SaturdayEstimate"))+
                                    Convert.ToInt32(Eval("SundayEstimate"))) %></td>
                                <td class="<%=weekDay==1?"wpitemtoday":"" %>">
                                    <%# Eval("Monday")%>
                                    <br />
                                    <%#Eval("Monday").ToString()!=""?"Estimate: "+Eval("MondayEstimate"):"" %>
                                    <br />
                                    <%#Eval("Monday").ToString()!=""?"Remaining: "+(8-(Convert.ToInt32(Eval("MondayEstimate")))):"" %>
                                </td>
                                <td class="<%=weekDay==2?"wpitemtoday":"" %>">
                                    <%# Eval("Tuesday")%>
                                    <br />
                                    <%#Eval("Tuesday").ToString()!=""?"Estimate: "+Eval("TuesdayEstimate"):"" %> 
                                    <br />
                                    <%#Eval("Tuesday").ToString()!=""?"Remaining: "+(8-(Convert.ToInt32(Eval("TuesdayEstimate")))):"" %>
                                </td>
                                <td class="<%=weekDay==3?"wpitemtoday":"" %>">
                                    <%# Eval("Wednesday")%>
                                    <br />
                                    <%#Eval("Wednesday").ToString()!=""?"Estimate: "+Eval("WednesdayEstimate"):"" %>  
                                    <br />
                                    <%#Eval("Wednesday").ToString()!=""?"Remaining: "+(8-(Convert.ToInt32(Eval("WednesdayEstimate")))):"" %>
                                </td>
                                <td class="<%=weekDay==4?"wpitemtoday":"" %>">
                                    <%# Eval("Thursday")%>
                                    <br />
                                    <%#Eval("Thursday").ToString()!=""?"Estimate: "+Eval("ThursdayEstimate"):"" %>   
                                    <br />
                                    <%#Eval("Thursday").ToString()!=""?"Remaining: "+(8-(Convert.ToInt32(Eval("ThursdayEstimate")))):"" %>
                                </td>
                                <td class="<%=weekDay==5?"wpitemtoday":"" %>">
                                    <%# Eval("Friday")%>
                                    <br />
                                    <%#Eval("Friday").ToString()!=""?"Estimate: "+Eval("FridayEstimate"):"" %>  
                                    <br />
                                    <%#Eval("Friday").ToString()!=""?"Remaining: "+(8-(Convert.ToInt32(Eval("FridayEstimate")))):"" %>
                                </td>
                                <td class="<%=weekDay==6?"wpitemtoday":"" %>">
                                    <%# Eval("Saturday")%>
                                    <br />
                                    <%#Eval("Saturday").ToString()!=""?"Estimate: "+Eval("SaturdayEstimate"):"" %> 
                                    <br />
                                    <%#Eval("Saturday").ToString()!=""?"Remaining: "+(8-(Convert.ToInt32(Eval("SaturdayEstimate")))):"" %>
                                </td>
                                <td class="<%=weekDay==0?"wpitemtoday":"" %>">
                                    <%# Eval("Sunday")%>
                                    <br />
                                    <%#Eval("Sunday").ToString()!=""?"Estimate: "+Eval("SundayEstimate"):"" %>   
                                    <br />
                                    <%#Eval("Sunday").ToString()!=""?"Remaining: "+(8-(Convert.ToInt32(Eval("SundayEstimate")))):"" %>
                                </td>
                                <td class="action" style="display: none;">
                                    <%#int.Parse(Eval("UserID").ToString())==UserInfo.UserID?"<a class=\"saveBtn1 mainbutton\" href=\"WeekPlanEdit.aspx?id="+Eval("ID")+"&isEdit=1&returnurl="+ this.ReturnUrl +"\" style=\"display: none;\"></a>":"" %>
                                    <%--<a class="saveBtn1 mainbutton" href="WeekPlanEdit.aspx?id=<%# Eval("ID")%>&returnurl=<%=this.ReturnUrl %>" style="display: none;"></a>--%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
