<%@ Page Title="Document Center" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="FilesList.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Documents.FilesList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenEditObject(id) {
            var url = "/Sunnet/Projects/ViewProject.aspx?id=" + id;
            var result = ShowIFrame(url,
                            880,
                            700,
                            true,
                            "");
            if (result == 0) {
                window.location.reload();
            }
            return false;
        }

        function OpenTicketDetail(selectTicketId) {
            window.open("/Sunnet/<%=TicketUrl %>?tid=" + selectTicketId);
        }
        function OpenTicketDetailFeedBack(selectTicketId) {
            window.open("/Sunnet/<%=TicketUrl %>?tid=" + selectTicketId + "#FeedBack");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Ticket files - include feedback files
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <td runat="server" width="50" id="tdCompanyLabel">
                        Company:
                    </td>
                    <td width="200" runat="server" id="tdCompanyDdl">
                        <asp:DropDownList ID="ddlCompany" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged"
                            runat="server" CssClass="select150">
                        </asp:DropDownList>
                    </td>
                    <td width="50">
                        Project:
                    </td>
                    <td width="200">
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="select150">
                        </asp:DropDownList>
                    </td>
                </ContentTemplate>
            </asp:UpdatePanel>
            <td width="50">
                Keyword:
            </td>
            <td width="200">
                <asp:TextBox ID="txtKeyword" CssClass="input200" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />
            </td>
        </tr>
    </table>
    <div class="mainrightBoxtwo">
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="FileTitle" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="ASC" />
        <table id="dataTickets" width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th width="150;">
                    Company
                </th>
                <th width="150;">
                    Project
                </th>
                <th width="50;" title="Ticket Code">
                    Code
                </th>
                <th width="*;" orderby="TicketTitle">
                    Ticket Title
                </th>
                <th width="150;">
                    Feedback
                </th>
                <th width="150;" orderby="FileTitle">
                    File Name
                </th>
                <th width="80;"  orderby="CreatedOn">
                    Create On
                </th>
                <th width="80;">
                    Create By
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="9" style="color: Red;">
                    &nbsp; No records
                </th>
            </tr>
            <asp:Repeater ID="rptTickets" runat="server">
                <ItemTemplate>
                    <tr type="tickets" class="<%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>"
                        id='<%#Eval("ID") %>'>
                        <td>
                            <%# Eval("CompanyName")%>
                        </td>
                        <td class='<%#Eval("SourceType").ToString() == ((int)SunNet.PMNew.Entity.FileModel.FileSourceType.Project).ToString()? "strong" : ""%>'>
                            <%# GetHtml(SunNet.PMNew.Entity.FileModel.FileSourceType.Project, Eval("ProjectTitle"),Eval("ProjectId"))%>
                        </td>
                        <td class='<%#Eval("SourceType").ToString() == ((int)SunNet.PMNew.Entity.FileModel.FileSourceType.Ticket).ToString()? "strong" : ""%>'>
                            <%#GetHtml(SunNet.PMNew.Entity.FileModel.FileSourceType.Ticket,  Eval("TicketCode").ToString(), Eval("TicketId"))%>
                        </td>
                        <td class='<%#Eval("SourceType").ToString() == ((int)SunNet.PMNew.Entity.FileModel.FileSourceType.Ticket).ToString()? "strong" : ""%>'>
                            <%#GetHtml(SunNet.PMNew.Entity.FileModel.FileSourceType.Ticket,  Eval("TicketTitle").ToString(), Eval("TicketId"))%>
                        </td>
                        <td class='<%#Eval("SourceType").ToString() == ((int)SunNet.PMNew.Entity.FileModel.FileSourceType.FeedBack).ToString()? "strong" : ""%>'>
                            <%#GetHtml(SunNet.PMNew.Entity.FileModel.FileSourceType.FeedBack, Eval("FeedBackTitle"), Eval("TicketId"))%>
                        </td>
                        <td>
                            <a href='/Do/DoDownloadFileHandler.ashx?FileID=<%#Eval("FileID") %>&size=<%#Eval("FileSize") %>'
                                target="_blank">
                                <%#Eval("FileTitle")%></a>
                        </td>
                        <td>
                            <%#Eval("CreatedOn","{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#GetClientUserName(Convert.ToInt32(Eval("CreatedBy"))) %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpFiles" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpFiles_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>
</asp:Content>
