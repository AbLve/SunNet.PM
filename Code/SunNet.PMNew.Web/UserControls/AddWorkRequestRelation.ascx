<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddWorkRequestRelation.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.AddWorkRequestRelation" %>

<script type="text/javascript">
    jQuery(function() {
        jQuery('#addRelationTickets tr td input:checkBox').on('click', function(event) {
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
                    OnClick="ImageButton1_Click" />
                <asp:HiddenField ID="hidOrderBy" runat="server" Value="TicketID" />
                <asp:HiddenField ID="hidOrderDirection" runat="server" Value="ASC" />
            </td>
        </tr>
    </table>
    <table width="95%" id="addRelationTickets" border="0" align="center" cellpadding="0"
        cellspacing="0" class="owlistone">
        <tr class="owlistTitle">
            <th style="width: 7%">
            </th>
            <th orderby="TicketCode" style="width: 23%">
                Ticket Code
            </th>
            <th orderby="Title" style="width: 30%">
                Title
            </th>
            <th orderby="Description" style="width: 40%">
                Description
            </th>
        </tr>
        <tr runat="server" id="trNoTickets" visible="true">
            <th colspan="5" style="color: Red;">
                &nbsp; No records
            </th>
        </tr>
        <asp:Repeater ID="rptRelationTicketsList" runat="server">
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo" %>'>
                    <td>
                        <input id='<%# Eval("TicketID")%>' type="checkbox" />
                    </td>
                    <td>
                        <%#Eval("TicketCode").ToString()%>
                    </td>
                    <td>
                        <%#  Server.HtmlEncode(Eval("Title").ToString())%>
                    </td>
                    <td>
                        <%# Eval("Description").ToString().Length >= 100? Server.HtmlEncode(Eval("Description").ToString()) + "..." : Server.HtmlEncode(Eval("Description").ToString())%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>
<div class="pageBox">
    <webdiyer:AspNetPager ID="anpUsers" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
        DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
        ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
        PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
        NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" runat="server"
        AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
        PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
        LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpUsers_PageChanged"
        PageSize="20">
    </webdiyer:AspNetPager>
</div>
