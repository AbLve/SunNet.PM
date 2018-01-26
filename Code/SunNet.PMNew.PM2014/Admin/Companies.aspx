<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.master" AutoEventWireup="true" CodeBehind="Companies.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.Companies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>

                <td width="60" align="right">Keyword:
                </td>
                <td>
                    <asp:TextBox placeholder="Enter CompanyName" ID="txtKeyword" queryparam="keyword" runat="server" Width="250px" CssClass="inputw1"></asp:TextBox>
                </td>
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" />
                </td>

            </tr>
        </table>
    </div>
    <% if (CheckRoleCanAccessPage("/Admin/NewCompany.aspx"))
       { %>
    <div class="topbtnbox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <span>
                        <ul class="listtopBtn" href="/Admin/NewCompany.aspx" data-target="#modalsmall" data-toggle="modal">
                            <li>
                                <div class="listtopBtn_icon">
                                    <img src="/Images/icons/newcompany.png">
                                </div>
                               <div class="listtopBtn_text">New Company</div> 
                            </li>
                        </ul>
                    </span>
                </td>
            </tr>
        </table>
    </div>
    <% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="*" class="order" orderby="CompanyName" default>Company Name<span class="arrow"></span></th>
                <th width="150" class="order" orderby="City">City<span class="arrow"></span></th>
                <th width="70" class="order" orderby="State">State<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Phone">Phone<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Fax">Fax<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Website">Website<span class="arrow"></span></th>
                <th width="80">Project #<span class="arrow"></span></th>
                <th width="80">Client #<span class="arrow"></span></th>
                <th width="20" style="display: none" class="aligncenter">Action</th>
                </tr>
        </thead>    
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptCompanyList" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                    <td>
                        <%# Eval("CompanyName").ToString()%>
                    </td>
                    <td>
                        <%#Eval("City").ToString()%>
                    </td>
                    <td>
                        <%#Eval("State").ToString()%>
                    </td>
                    <td>
                        <%# Eval("Phone").ToString()%>
                    </td>
                    <td>
                        <%# Eval("Fax")%>
                    </td>
                    <td>
                        <%# Eval("Website")%>
                    </td>
                    <td>
                        <%# Eval("ProjectsCount")%>
                    </td>
                    <td>
                        <%#Eval("ClientsCount")%>   
                    </td>
                    <td class="action aligncenter" style="display: none">
                        <a href='CompanyDetail.aspx?id=<%# Eval("ComId")%>&returnurl=<%# this.ReturnUrl %>'>
                            <img src="/Images/icons/edit.png" title="View"></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="CompanyPage" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
