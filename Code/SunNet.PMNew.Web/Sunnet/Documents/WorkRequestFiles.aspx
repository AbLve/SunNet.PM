<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkRequestFiles.aspx.cs" MasterPageFile="~/Sunnet/Main.Master"
 Inherits="SunNet.PMNew.Web.Sunnet.Documents.WorkRequestFiles" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenEditObject(id) {
            var url = "/Sunnet/Projects/ViewProject.aspx?id=" + id;
            var result = ShowIFrame(url,
                            880,
                            700,
                            true,
                            "");
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Work Request Files
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="50">
                Tag:
            </td>
            <td width="200">
                <asp:TextBox ID="txtTag"  CssClass="input200"
                    runat="server"></asp:TextBox>
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
                <th width="30%"  orderby="FileTitle">
                    Title 
                </th>
                <th width="20%" orderby="Tags">
                    Tags
                </th>
                <th width="50%" orderby="CreatedOn">
                    Date
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="5" style="color: Red;">
                    &nbsp; No records
                </th>
            </tr>
            <asp:Repeater ID="rpt" runat="server">
                <ItemTemplate>
                    <tr  dialogwidth="620" dialogheight="560" dialogtitle="" href="/Sunnet/WorkRequest/AddDocument.aspx?id=<%#Eval("FileID") %>&wid="
                        class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                        <td>
                            <a href='/Do/DoDownloadFileHandler.ashx?FileID=<%#Eval("FileID") %>&size=<%#Eval("FileSize") %>'
                                target="_blank">
                                <%#Eval("FileTitle")%></a>
                        </td>
                        <td>
                            <%# Eval("Tags").ToString()%>
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
           <webdiyer:AspNetPager ID="anpWorkRequestFiles" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%" 
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20" CurrentPageIndex="1"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpWorkRequestFiles_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>
</asp:Content>