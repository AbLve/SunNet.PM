<%@ Page Title="List Companys" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="ListCompany.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Companys.ListCompany" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenAddObject() {
            var result = ShowIFrame("AddCompany.aspx",
                            650,
                            310,
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
    List Companies
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="300">
                Keyword:
                <asp:TextBox ID="txtKeyword"  runat="server" CssClass="input200"></asp:TextBox>
            </td>
            <td width="*">
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <span action="new"><a runat="server" id="AddNewObject" onclick="return OpenAddObject();"
            href="###" title="Add Company">
            <img align="absmiddle" border="0" src="/icons/09.gif" />New</a></span><asp:HiddenField
                ID="hidOrderBy" runat="server" Value="CompanyName" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="ASC" />
    </div>
    <div class="mainrightBoxtwo">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th orderby="CompanyName" style="width: 15%;">
                    Company Name
                </th>
                <th orderby="City" style="width: 15%;">
                    City
                </th>
                <th orderby="State" style="width: 10%;">
                    State
                </th>
                <th style="width: 15%;">
                    Phone
                </th>
                <th style="width: 15%;">
                    Fax
                </th>
                <th style="width: 15%;">
                    Project #
                </th>
                <th style="width: 15%;">
                    Client #
                </th>
            </tr>
            <asp:Repeater ID="rptCompanies" runat="server">
                <ItemTemplate>
                    <tr opentype="popwindow" dialogwidth="840" dialogheight="700" dialogtitle="" href="/Sunnet/Companys/EditCompany.aspx?id=<%#Eval("ID") %>"
                        freshbutton="<%#iBtnSearch.ClientID %>" class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                        <td>
                            <%#Eval("CompanyName")%>
                        </td>
                        <td>
                            <%#Eval("City")%>
                        </td>
                        <td>
                            <%#Eval("State")%>
                        </td>
                        <td>
                            <%#Eval("Phone")%>
                        </td>
                        <td>
                            <%#Eval("Fax")%>
                        </td>
                        <td>
                            <%#Eval("ProjectsCount")%>
                        </td>
                        <td>
                            <%#Eval("ClientsCount")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpCompanies" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpCompanies_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>
</asp:Content>
