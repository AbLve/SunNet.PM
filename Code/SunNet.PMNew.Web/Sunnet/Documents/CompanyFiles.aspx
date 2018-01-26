<%@ Page Title="Company Files" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="CompanyFiles.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Documents.CompanyFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Company Files 
    <asp:Literal ID="ltlWhosCompany" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td runat="server" width="50" id="tdCompanyLabel">
                Company:
            </td>
            <td width="200" runat="server" id="tdCompanyDdl">
                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="select150">
                </asp:DropDownList>
            </td>
            <td width="50">
                Keyword:
            </td>
            <td width="200">
                <asp:TextBox ID="txtKeyword"  CssClass="input200"
                    runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />
            </td>
        </tr>
    </table>
    <div class="mainrightBoxtwo">
        <table id="dataTickets" width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th width="10%">
                    Company
                </th>
                <th width="30%">
                    File Name
                </th>
                <th width="10%">
                    Create On
                </th>
                <th width="10%">
                    Create By
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="4" style="color: Red;">
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
