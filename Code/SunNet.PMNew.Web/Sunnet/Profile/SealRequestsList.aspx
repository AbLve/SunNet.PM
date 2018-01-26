<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="SealRequestsList.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Profile.SealRequestsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Seal Request List
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="35">Seal:
            </td>
            <td width="200">
                <asp:DropDownList ID="ddlSeal" runat="server" CssClass="select205" Width="150" DataValueField="ID"
                    DataTextField="SealName">
                </asp:DropDownList>
            </td>
            <td width="40">Status:
            </td>
            <td width="200">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select205" Width="150">
                    <asp:ListItem Value="-1">ALL</asp:ListItem>
                    <asp:ListItem Value="0">Cancel</asp:ListItem>
                    <asp:ListItem Value="1">Open</asp:ListItem>
                    <asp:ListItem Value="2">Submit</asp:ListItem>
                    <asp:ListItem Value="3">Approved</asp:ListItem>
                    <asp:ListItem Value="4">Denied</asp:ListItem>
                    <asp:ListItem Value="5">Sealed</asp:ListItem>
                    <asp:ListItem Value="6">Complete</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td width="59">Start Date:
            </td>
            <td width="220">
                <asp:TextBox ID="txtStartDate" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                    Style="width: 150px;" CssClass="input200" runat="server"></asp:TextBox>
                <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtStartDate.ClientID %>"),document.getElementById("<%=txtStartDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                    align="absmiddle">
            </td>
            <td width="59">End Date:
            </td>
            <td width="220">
                <asp:TextBox ID="txtEndDate" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                    Style="width: 150px;" CssClass="input200" runat="server"></asp:TextBox>
                <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtEndDate.ClientID %>"),document.getElementById("<%=txtEndDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                    align="absmiddle">
            </td>
            <td>
                <asp:ImageButton ID="iBtnSearch" runat="server" align="absmiddle" ImageUrl="/images/search_btn.jpg"
                    OnClick="iBtnSearch_Click" Width="20px" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <% if (UserInfo.Office == "CN" && UserInfo.Role == SunNet.PMNew.Entity.UserModel.RolesEnum.PM)
           { %>
        <span><a href="#" onclick="OpenListPageDialog('/Sunnet/Profile/SealRequestsEdit.aspx','Add Seal Request',520, 380,'<%=iBtnSearch.ClientID%>');">
            <img src="/icons/14.gif" border="0" align="absmiddle" alt="new/add" />
            Add Seal Request</a></span>
        <% } %>
    </div>
    <div class="mainrightBoxtwo">
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="RequestedDate" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="DESC" />
        <table border="0" cellpadding="0" cellspacing="0" class="listtwo" width="100%">
            <tr class="listsubTitle">
                <th width="95px" orderby="ID">Request Code
                </th>
                <th width="65px" orderby="Status">Status
                </th>
                <th width="200px" orderby="Title">Title
                </th>
                <th>Description
                </th>
                <th width="90px" orderby="RequestedFirstName">Requested
                </th>
                <th width="110px" orderby="RequestedDate">Requested Date
                </th>
            </tr>
            <tr id="trNoRecords" runat="server">
                <td colspan="9">
                    <span style='color: Red;'>&nbsp; No records</span>
                </td>
            </tr>
            <asp:Repeater ID="rptSealsRequest" runat="server">
                <ItemTemplate>
                    <tr href="/Sunnet/Profile/SealRequestsEdit.aspx?id=<%#Eval("ID") %>" opentype="popwindowNew"
                        dialogwidth="700" dialogheight="500" dialogtitle="Edit Seal Request" class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'
                        freshbutton="<%#iBtnSearch.ClientID %>">
                        <td>R<%#Eval("ID")%>
                        </td>
                        <td>
                            <%#GetStautsHTML(Eval("Status")) %>
                        </td>
                        <td>
                            <%#Eval("Title")%>
                        </td>
                        <td>
                            <%# (string)Eval("Description")%>
                        </td>
                        <td>
                            <%#Eval("RequestedFirstName")%>
                        </td>
                        <td>
                            <%#Eval("RequestedDate","{0:MM/dd/yyyy}")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="aspNetPager1" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" runat="server"
                AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="aspNetPager1_PageChanged"
                PageSize="20">
            </webdiyer:AspNetPager>
        </div>
    </div>
</asp:Content>
