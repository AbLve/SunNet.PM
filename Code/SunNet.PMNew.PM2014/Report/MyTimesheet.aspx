<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyTimesheet.aspx.cs" MasterPageFile="~/Report/Report.Master" 
    Inherits="SunNet.PMNew.PM2014.Report.MyTimesheet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="100" >Keyword:
                </td>
                <td>
                    <asp:TextBox ID="txtKeyword" queryparam="keyword" placeholder="Enter project or ticket title" runat="server" CssClass="inputProfle1"></asp:TextBox>
                </td>
                <td width="80" >Project:
                </td>
                  
                <td>
                    <asp:DropDownList ID="ddlProject" queryparam="project" AutoPostBack="true" CssClass="selectProfle1" runat="server"
                        OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td width="80" >Ticket:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTickets" queryparam="ticket" CssClass="selectProfle1" runat="server">
                    </asp:DropDownList>
                </td>
                 
            </tr>
            <tr>
                <td width="80" >Start Date:
                </td>
                <td>
                     <asp:TextBox CssClass="inputdate inputProfle1" ID="txtStartDate" queryparam="startdate" runat="server"   onclick="WdatePicker();"  onFocus="WdatePicker()"></asp:TextBox>

                    
                </td>
                <td width="60" >End Date:
                </td>
                <td>
                  <asp:TextBox CssClass="inputdate inputProfle1" ID="txtEndDate" queryparam="enddate" runat="server"   onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox> 
                </td>
               
                <td colspan="2">
                    <input type="button" class="searchBtn" id="btnSearch" style="margin-right: 10px;"/> 
                     <asp:Button ID="iBtnDownload" CssClass="otherBtn excelBtn" runat="server" Text="&nbsp;" ToolTip="Download Report" OnClick="iBtnDownload_Click" />
                </td>
                <td>
                   
                </td>

            </tr>
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                 <th width="20"></th>
                <th width="90" class="order" orderby="SheetDate" default>Sheet Date<span class="arrow"></span></th>
                <th width="170" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="*" class="order" orderby="TicketTitle">Ticket Title<span class="arrow"></span></th>
                <th width="90" class="order" orderby="FirstName">User Name<span class="arrow"></span></th>
                <th width="70" class="order" orderby="Hours">Hours<span class="arrow"></span></th>
                <th width="100" class="order" orderby="ModifiedOn">Modified<span class="arrow"></span></th>
                <th width="90" class="order" orderby="IsSubmitted">Submitted<span class="arrow"></span></th>
               
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptReportList" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                     <td class="aligncenter action">
                        <a href='###' timesheet='<%# Eval("TimeSheetID") %>' class='collapsed1'></a>
                    </td>
                    <td>
                        <%#Eval("SheetDate","{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%#Eval("ProjectTitle")%>
                    </td>
                    <td>
                        <%#Eval("TicketTitle")%>
                    </td>
                    <td>
                        <%#GetClientUserName(Eval("UserID")) %>
                    </td>
                    <td>
                        <%#Eval("Hours")%>
                    </td>
                
                    <td>
                        <%#Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%#Convert.ToBoolean(Eval("IsSubmitted").ToString())?"Yes":"No"%>
                    </td>
                    
                </tr>
                     <tr class="sublist"  id='Timesheet<%# Eval("TimeSheetID") %>' style="display: none" >
                    <td colspan="9">
                        <div class="subcontentBox" >
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <td style="width:20px">&nbsp;</td>
                                        <td width="200" valign="top"><strong>Ticket Description:</strong></td>
                                        <td><%# Server.HtmlEncode(Eval("TicketDescription").ToString())%></td>
                                    </tr>
                                    <tr>
                                         <td style="width:20px">&nbsp;</td>
                                        <td width="200" valign="top"><strong>Work Detail:</strong></td>
                                        <td><%#Eval("WorkDetail")%></td>
                                    </tr> 
                                </tbody>
                            </table>
                        </div>
                    </td>

                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="ReportPage" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>

