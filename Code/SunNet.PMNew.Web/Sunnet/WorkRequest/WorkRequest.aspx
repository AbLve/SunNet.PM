<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkRequest.aspx.cs" MasterPageFile="~/Sunnet/Main.Master"
    Inherits="SunNet.PMNew.Web.Sunnet.WorkRequest.WorkRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenAddObject() {
            var result = ShowIFrame("AddWorkRequest.aspx",
                            790,
                            410,
                            true,
                            "");
            if (result == 0) {
                window.location.reload();
            }
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Work Request
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="270">
                Keyword:
                <asp:TextBox ID="txtKeyword" runat="server" CssClass="input200"></asp:TextBox>
            </td>
            <td width="270">
                Project:
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="select205">
                </asp:DropDownList>
            </td>
            <td >
                Status:
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select205">
                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Proposal Submitted" Value="1"></asp:ListItem>
                    <asp:ListItem Text="In Process" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Approval" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Cancel" Value="5"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td >
                Payment:
                <asp:DropDownList ID="ddlPayment" runat="server" CssClass="select205">
                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Quote Approval" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Invoiced" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Paid" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td >
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />
            </td>
            <td>
            </td>
        </tr>
    </table>
    <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
       { %>
    <div class="mainactionBox">
        <span action="new"><a runat="server" id="AddNewObject" onclick="return OpenAddObject();"
            href="###" title="New Work Request">
            <img align="absmiddle" border="0" src="/icons/09.gif" />New</a></span>
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="RequestNo" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="desc" />
    </div>
    <% }%>
    <div class="mainrightBoxtwo">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th style="width: 4%;">
                </th>
                <th style="width: 15%;" orderby="RequestNo">
                    Request #
                </th>
                <th style="width: 10%;" orderby="p.Title">
                    Project
                </th>
                <th style="width: 15%;" orderby="w.Title">
                    Title
                </th>
                <th style="width: 4%;" orderby="WorkScopeDisplayName">
                    Work Scpoe
                </th>
                <th style="width: 10%;" orderby="w.Status">
                    Status
                </th>
                <th style="width: 9%;" orderby="Payment">
                    Payment
                </th>
                <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
                   { %>
                <th style="width: 9%;" orderby="InvoiceNo">
                    Invoice #
                </th>
                <th style="width: 8%;">
                    Total Hours
                </th>
                <% }%>
                <th style="width: 7%;" orderby="DueDate">
                    Due Date
                </th>
                <th style="width: 8%;" orderby="w.CreatedOn">
                    Created On
                </th>
            </tr>
            <asp:Repeater ID="rptWR" runat="server" OnItemDataBound="rptWR_ItemDataBound">
                <ItemTemplate>
                    <tr href="/Sunnet/WorkRequest/EditWorkRequest.aspx?id=<%#Eval("WorkRequestID") %>"
                        opentype="newtab" class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                        <td style="text-align: center;">
                            <%# ShowPriorityImgByDevDate(Eval("DueDate"))%>
                        </td>
                        <td>
                            <%#Eval("RequestNo")%>
                        </td>
                        <td>
                            <%#Eval("ProjectName")%>
                        </td>
                        <td>
                            <%#Eval("Title")%>
                        </td>
                        <td style="text-align: center;">
                          
                            <a href="###" title='<%#Eval("WorkScopeDisplayName")%>'>
                                <img src="/icons/download.png" alt='<%#Eval("WorkScopeDisplayName")%>' onclick="downloadFile('Download.aspx?FileName=<%#Eval("WorkScopeDisplayName")%>&FilePath=<%#Eval("WorkScope")%>')" />
                            </a>
                        </td>
                        <td>
                            <%#((SunNet.PMNew.Entity.WorkRequestModel.Enums.WorkRequestStatusEnum)Eval("Status")).ToString().Replace("_"," ")%>
                        </td>
                        <td>
                            <%# ((SunNet.PMNew.Entity.WorkRequestModel.Enums.PaymentEnum)Eval("Payment")).ToString().Replace("_", " ").Replace("0", " ")%>
                        </td>
                        <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
                           { %>
                        <td>
                            <%#Eval("InvoiceNo")%>
                        </td>
                        <td>
                            <asp:Literal ID="lblHours" runat="server"></asp:Literal>
                            <asp:Literal ID="lblWID" runat="server" Visible="false" Text='<%#Eval("WorkRequestID")%>'></asp:Literal>
                        </td>
                        <% }%>
                        <td>
                            <%# Eval("DueDate", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%# Eval("CreatedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpWorkRequest" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                CurrentPageIndex="1" runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpWorkRequest_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>
    <iframe id="fileDownload" style="padding: 0; margin: 0; width: 0; height: 0;"></iframe>

    <script type="text/javascript">
        function downloadFile(downloadUrl) {
            document.getElementById('fileDownload').src = downloadUrl;
        }
    </script>

</asp:Content>
