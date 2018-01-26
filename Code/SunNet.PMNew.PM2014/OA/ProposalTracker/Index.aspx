<%@ Page Title="" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.ProposalTracker.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function navigateToTimesheetReport(event, id) {
            var event = event || window.event;
            window.location = "/Report/TimeSheet.aspx?WID=" + id;
            event.stopPropagation();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
            <script src="/Scripts/My97DatePicker/PM_WdatePicke.js"></script>
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>

                <td width="60px">Keyword:
                </td>
                <td>
                    <asp:TextBox ID="txtKeyword" runat="server" queryparam="keywork" CssClass="input200" Width="220" placeholder="Enter Title, Invoice No"></asp:TextBox>
                </td>
                <td width="80px">Project:
                </td>
                <td width="180px">
                    <asp:DropDownList ID="ddlProject" CssClass="select150" Width="130" queryparam="project" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="60px">Status:</td>
                <td width="200px">
                    <asp:DropDownList ID="ddlStatus" CssClass="select150" queryparam="status" runat="server">
                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Awaiting ETA" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Awaiting Proposal" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Awaiting Approval/PO" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Awaiting Development" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Awaiting Sending Invoice" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Awaiting Payment" Value="6"></asp:ListItem>
                        <asp:ListItem Text="Paid/Completed " Value="7"></asp:ListItem>
                        <asp:ListItem Text="On Hold" Value="8"></asp:ListItem>
                        <asp:ListItem Text="Not Approved" Value="9"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="60px">Company:</td>
                <td width="200px">
                     <asp:DropDownList ID="ddlCompany" CssClass="select150" Width="200" queryparam="companyId" runat="server">
                    </asp:DropDownList>
                </td>

                <%--<td width="60px">Payment</td>
                <td>
                    <asp:DropDownList ID="ddlPayment" CssClass="select150" queryparam="payment" runat="server">
                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Quote Approval" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Invoiced" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Paid" Value="3"></asp:ListItem>
                    </asp:DropDownList></td>--%>



                
            </tr>
            <tr>
                <td width="80px">Start Date:
                </td>
                <td>
                    <asp:TextBox Width="220" ID="txtStartDate" queryparam="start" runat="server" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'})" CssClass="inputdate  inputw1"></asp:TextBox>
                </td>
                <td>End Date:
                </td>
                <td>
                    <asp:TextBox     ID="txtEndDate" queryparam="end" runat="server" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'})" CssClass="inputdate inputw1"></asp:TextBox>
                </td>
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td>
                        <ul class="listtopBtn">
                            <li>
                                <div class="listtopBtn_icon">
                                    <a href="AddProposalTracker.aspx?returnurl=<%=this.ReturnUrl %>">
                                        <img src="/Images/icons/newproject.png"></a>
                                </div>
                                <div class="listtopBtn_text"><a href="AddProposalTracker.aspx?returnurl=<%=this.ReturnUrl %>">New Proposal</a></div>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div style="width:100%;overflow-x:auto;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="150px" class="order order-desc" default="true" orderby="NK.ProjectName" style="word-break: break-all;">Project<span class="arrow"></span></th>
                <th width="120px" class="order" orderby="NK.Title">Title<span class="arrow"></span></th>
                <th width="120px" class="order" orderby="NK.CompanyName">Company<span class="arrow"></span></th>
                <th width="50px">File<span class="arrow"></span></th>
                <th width="70px" class="order" orderby="NK.Status">Status<span class="arrow"></span></th>
                <th width="100px" class="order" orderby="NK.InvoiceNo">Invoice<span class="arrow"></span></th>
                <th width="60px">Hours<span class="arrow"></span></th>
                <th width="60px" class="order" orderby="NK.CreatedOn">Created On<span class="arrow"></span></th>
                <%-- <th width="120px" class="order" orderby="w.CreatedOn">Created On<span class="arrow"></span></th>--%>
                <th style="display: none;"></th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptWR" runat="server" OnItemDataBound="rptWR_ItemDataBound">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>
                        <%#Eval("ProjectName")%>
                    </td>
                    <td>
                        <%#Eval("Title")%>
                    </td>
                     <td>
                        <%#Eval("CompanyName")%>
                    </td>
                    <td class="action">
                        <%# BuilderDown((string)Eval("WorkScope"),(int)Eval("ProposalTrackerID"),(string)Eval("WorkScopeDisplayName")) %>
                    </td>
                    <td>
                        <%#((SunNet.PMNew.Entity.ProposalTrackerModel.Enums.ProposalTrackerStatusEnum)Eval("Status")).ToString().Replace("_"," ")%>
                    </td>
                    <td>
                        <%#Eval("InvoiceNo")%>
                    </td>
                    <td>
                        <asp:Literal ID="lblHours" runat="server"></asp:Literal>
                        <asp:Literal ID="lblWID" runat="server" Visible="false" Text='<%#Eval("ProposalTrackerID")%>'></asp:Literal>
                    </td>
                     <td>
                        <%# Eval("CreatedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td class="action" style="display: none;">
                        <a class="saveBtn1 mainbutton" href="EditProposalTracker.aspx?ID=<%# Eval("ProposalTrackerID")%>&returnurl=<%=this.ReturnUrl %>" style="display: none;"></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    </div>


    <iframe id="iframeDownloadFile" style="padding: 0; margin: 0; width: 0; height: 0; display: none;"></iframe>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpWaitting" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>

