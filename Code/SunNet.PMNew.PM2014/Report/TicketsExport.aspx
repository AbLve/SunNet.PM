<%@ Page Title="" Language="C#" MasterPageFile="~/Report/Report.Master" AutoEventWireup="true" CodeBehind="TicketsExport.aspx.cs"
    Inherits="SunNet.PMNew.PM2014.Report.TicketsExport" %>

<%@ Import Namespace="SunNet.PMNew.PM2014.Codes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
        <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="80px">Keyword:
                </td>
                <td width="150px">
                    <asp:TextBox ID="txtKeyWord" placeholder="Enter ID, Title" queryparam="keyword" runat="server" CssClass="inputw1" Width="130"></asp:TextBox>
                </td>
                <td width="80px">Project:
                </td>
                <td width="150px">
                    <sunnet:ExtendedDropdownList ID="ddlProject" queryparam="project"
                        DataTextField="Title"
                        DataValueField="ProjectID"
                        DataGroupField="Status"
                        DefaultMode="List" runat="server" CssClass="selectw1" Width="130">
                    </sunnet:ExtendedDropdownList>
                </td>
                <td width="40px">Type:
                </td>
                <td width="150px">
                    <asp:DropDownList ID="ddlTicketType" queryparam="tickettype" runat="server" CssClass="selectw1">
                        <asp:ListItem Value="-1">ALL</asp:ListItem>
                        <asp:ListItem Value="0">Bug</asp:ListItem>
                        <asp:ListItem Value="1">Request</asp:ListItem>
                        <asp:ListItem Value="2">Risk</asp:ListItem>
                        <asp:ListItem Value="3">Issue</asp:ListItem>
                        <asp:ListItem Value="4">Change</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                 <td>Start Date:
                </td>
                <td>
                    <asp:TextBox CssClass="inputdate " Width="130" ID="txtStartDate" queryparam="startdate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
                </td>
                <td>End Date:
                </td>
                <td>
                    <asp:TextBox CssClass="inputdate "  Width="130" ID="txtEndDate" queryparam="enddate" runat="server" onclick="WdatePicker();" onFocus="WdatePicker()"></asp:TextBox>
                </td>
                <td colspan="2" style="text-align: left">  
                    <input type="button" class="searchBtn" id="btnSearch"  />
                     <asp:Button ID="iBtnDownload" CssClass="otherBtn excelBtn" runat="server" Text="&nbsp;" ToolTip="Download Report" OnClick="iBtnDownload_Click" />
                </td>
            </tr>
        </table>
       
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    
       
        <asp:Repeater ID="rptTicketsList" runat="server">
            <HeaderTemplate>
              <table border="0" width="100%"  cellpadding="0" cellspacing="0" class="table-advance">
                <thead>
            <tr>
                <th width="120" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="70px" class="order" orderby="TicketID" default="true">Ticket ID<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="90px" class="order" orderby="TicketType">Type<span class="arrow"></span></th>
                <th width="100px" class="order" orderby="Status">Status<span class="arrow"></span></th>
                <th width="100px" class="order" orderby="CreatedOn">Created On<span class="arrow"></span></th>  
                <th width="100px" class="order" orderby="ModifiedOn">Updated On<span class="arrow"></span></th>  
            </tr>

        </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <script type="text/html" id='ticket<%# Eval("TicketID")%>Description'><%# Server.HtmlEncode(Eval("FullDescription").ToString()) %></script>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> collapsed' ticket="<%# Eval("TicketID")%>">
                   
                    <td>
                        <%# Eval("ProjectTitle").ToString()%>
                    </td>
                    <td>
                        <%# " "+Eval("TicketID").ToString()+" " %>
                    </td>
                    <td>
                        <%# Eval("Title").ToString()%>
                    </td>
                     <td>
                        <%# Eval("TicketType").ToString()%>
                    </td>
                    <td>
                        <%# GetStatus(Eval("Status"))%>
                    </td>
                     <td>
                        <%# Eval("CreatedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                  
                   
                </tr>
            </ItemTemplate>
            <FooterTemplate>    </table></FooterTemplate>
        </asp:Repeater>
        <table>
            <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found
            </th>
        </tr>
        </table>
    
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpOngoing" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
