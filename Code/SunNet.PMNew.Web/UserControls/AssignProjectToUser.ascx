<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssignProjectToUser.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.AssignProjectToUser" %>

<script type="text/javascript">
    jQuery(function() {
    jQuery('#assignProjectToUser tr td input:checkBox').on('click', function(event) {
            event.stopPropagation();
        }).closest('td').on('click', function(event) {
            var $checkBox = $(this).find('input[type=checkBox]');
            if ($checkBox.prop('checked')) {
                $checkBox.prop('checked', false);
            }
            else {
                $checkBox.prop('checked', true);
            }
            event.stopPropagation();
        });
    });
</script>

<div class="owlistContainer" style="min-height: 370px;">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="59">
                &nbsp; Keyword
            </td>
            <td>
                <asp:TextBox ID="txtKeyWord" runat="server" CssClass="input200"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" align="absmiddle" runat="server" ImageUrl="~/Images/search_btn.jpg"
                    OnClick="btnSearch_Click" />
                <asp:HiddenField ID="hidOrderBy" runat="server" Value="ProjectCode" />
                <asp:HiddenField ID="hidOrderDirection" runat="server" Value="asc" />
            </td>
        </tr>
    </table>
    <table width="95%" id="assignProjectToUser" border="0" align="center" cellpadding="0"
        cellspacing="0" class="owlistone">
        <tr class="owlistTitle">
            <th>
            </th>
            <th style="width: 22%;" orderby="ProjectCode">
                Project Code
            </th>
            <th style="width: 25%;" orderby="Title">
               Project Title
            </th>
            <th style="width: 20%;" orderby="Priority">
                Company
            </th>
            <th style="width: 20%;" orderby="Billable">
                Project Manager
            </th>
        </tr>
        <tr runat="server" id="trNoRecords" visible="true">
            <th colspan="5" style="color: Red;">
                &nbsp; No records
            </th>
        </tr>
        <asp:Repeater ID="rptProjects" runat="server">
            <ItemTemplate>
                <tr class="<%#Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo"%>">
                    <td>
                        <input id='<%# Eval("ProjectID")%>' type="checkbox" />
                    </td>
                    <td>
                        <%#Eval("ProjectCode")%>
                    </td>
                    <td>
                        <%#Eval("Title")%>
                    </td>
                    <td>
                        <%#Eval("CompanyName")%>
                    </td>
                    <td>
                        <%#Eval("PMUserName")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>
<div class="pageBox">
    <webdiyer:AspNetPager ID="anpProjects" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
        DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
        ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
        PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
        NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
        runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
        PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
        LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpProjects_PageChanged">
    </webdiyer:AspNetPager>
</div>
