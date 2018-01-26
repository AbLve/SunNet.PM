<%@ Page Title="Calendar" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Calendar.Index" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="searchSection">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td width="30" align="right">Year:</td>
                    <td>
                        <asp:DropDownList ID="ddlYears" runat="server" CssClass="selectw1" Width="120px">
                            <asp:ListItem Value="2013">2013</asp:ListItem>
                            <asp:ListItem Value="2014">2014</asp:ListItem>
                            <asp:ListItem Value="2015">2015</asp:ListItem>
                            <asp:ListItem Value="2016">2016</asp:ListItem>
                            <asp:ListItem Value="2017">2017</asp:ListItem>
                            <asp:ListItem Value="2018">2018</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="55" align="right">Month:</td>
                    <td>
                        <asp:DropDownList ID="ddlMonths" runat="server" CssClass="selectw1" Width="120px">
                            <asp:ListItem Value="1">1</asp:ListItem>
                            <asp:ListItem Value="2">2</asp:ListItem>
                            <asp:ListItem Value="3">3</asp:ListItem>
                            <asp:ListItem Value="4">4</asp:ListItem>
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="6">6</asp:ListItem>
                            <asp:ListItem Value="7">7</asp:ListItem>
                            <asp:ListItem Value="8">8</asp:ListItem>
                            <asp:ListItem Value="9">9</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="11">11</asp:ListItem>
                            <asp:ListItem Value="12">12</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="60" align="right">Users:</td>
                    <td>
                        <asp:DropDownList ID="ddlUsers" runat="server" class="selectw1">
                        </asp:DropDownList>
                    </td>
                    <td width="30">
                        
                        <asp:ImageButton ID="iBtnSearch"  runat="server"  CssClass="searchBtn" AlternateText=" "
                            OnClick="iBtnSearch_Click" /></td>
                </tr>
            </tbody>
        </table>
    </div>
    
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td width="140">
                        <ul class="listtopBtn">
                            <li>
                                <div class="listtopBtn_icon">
                                    <img src="/images/waddschedule.png" />
                                </div>
                                <div class="listtopBtn_text" href="Add.aspx" data-target="#modalsmall" data-toggle="modal">Add Schedules </div>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="dataSection">
    
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="monthlyviewBox">
            <tr>
                <td>
                    <ul class="monthlyviewweek">
                        <li>Sunday</li>
                        <li>Monday</li>
                        <li>Tuesday</li>
                        <li>Wednesday</li>
                        <li>Thursday</li>
                        <li>Friday</li>
                        <li>Saturday</li>
                    </ul>
                    <ul class="monthlyview">
                        <asp:Repeater ID="rptDays" runat="server" OnItemDataBound="rptDays_ItemDataBound">
                            <ItemTemplate>
                                <li class="<%#Eval("Css")%>">
                                    <%#Eval("AddUrl")%>
                                    <asp:Repeater ID="rptSchedules" runat="server">
                                        <HeaderTemplate>
                                            <ul class="onscdTicket">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li><a href="Edit.aspx?ID=<%#Eval("ID")%>&target=<%= ddlUsers.SelectedValue %>" data-target="#modalsmall" data-toggle="modal" 
                                                title="Time: <%#Eval("StartTime")%> - <%#Eval("EndTime")%> ">
                                                <%#Eval("Title") %></a> </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </td>
            </tr>
        </table>
    
    <script type="text/javascript">
        var dialogParam;

        function OpenEditSchdules(id, targetUserId) {
            $.Zebra_Dialog.popWindow("/Sunnet/Tickets/EditSchedules.aspx?ID=" + id + "&target=" + targetUserId + "&r=" + Math.random(), "Edit Schedules", 580, 500, function () {
                if (dialogParam && dialogParam.type === "new") {
                    OpenAddModuleDialog(dialogParam.date);
                    dialogParam = null;
                }
                else {
                    $("#" + "<%=iBtnSearch.ClientID%>").click();
                }
            });
        }
    </script>
</asp:Content>
