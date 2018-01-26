<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketTimesheet.aspx.cs" 
      MasterPageFile="~/Pop.master"
    Inherits="SunNet.PMNew.PM2014.Report.TicketTimesheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    <asp:Literal runat="server" ID="litHead"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="100" class="order" orderby="SheetDate" default>Sheet Date<span class="arrow"></span></th>
                <th width="100" class="order" orderby="FirstName">User Name<span class="arrow"></span></th>
                <th width="80"  >Role<span class="arrow"></span></th>
                <th width="120" class="order" orderby="Hours">Hours<span class="arrow"></span></th>
                <th width="100" class="order" orderby="ModifiedOn">Modified On<span class="arrow"></span></th>
                <th width="100" class="order" orderby="IsSubmitted">Submitted<span class="arrow"></span></th>
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
                    <td>
                        <%#Eval("SheetDate","{0:MM/dd/yyyy}")%>
                    </td>
                   
                    <td>
                        <%#GetClientUserName(Eval("UserID")) %>
                    </td>
                    <td>
                          <%#Eval("RoleName")%>
                   
                    </td>
                    <td>
                           <%#Eval("Hours")%>
                    </td>
                    <td>
                        <%#Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td>
                        <%#Convert.ToBoolean(Eval("IsSubmitted").ToString())?"yes":"no"%>
                    </td>
                  
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
     <div class="pagebox">
        <webdiyer:AspNetPager ID="ReportPage" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
   
</asp:Content>