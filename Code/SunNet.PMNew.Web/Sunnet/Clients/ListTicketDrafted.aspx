<%@ Page Title="Drafted Tickets" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="ListTicketDrafted.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.ListTicketDrafted" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var deleteTicketid = 0;

        function DeleteConfirm(s) {
            deleteTicketid = s;
            MessageBox.Confirm3(null, "Confirm to Detele?", '', '', ConfirmDeleteCallBack);
            return false;
        }
        function ConfirmDeleteCallBack(e) {
            if (e == true) {
                if (deleteTicketid > 0) {
                    deleteImg(deleteTicketid);
                    deleteTicketid = 0;
                }
            }
        }
        function delRow(t) {
            var vTr = t.parent("td").parent("tr");
            vTr.remove();
        };
        function deleteImg(tid) {
            delRow($("#" + tid));
            $.ajax({
                type: "post",
                url: "/Do/DoRemoveDraftTicketHandler.ashx?r=" + Math.random(),
                data: {
                    tid: tid
                },
                success: function(result) {
                    ShowMessage(result, 0, true, false);
                }
            });
        }
        $(document).ready(function() {
            $("#<%=txtKeyWord.ClientID%>").focus();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Drafted Tickets
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="50">
                Keyword:
            </td>
            <td width="260">
                <asp:TextBox ID="txtKeyWord" runat="server" CssClass="input200"></asp:TextBox>
            </td>
            <td width="40">
                Status:
            </td>
            <td width="160">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select100">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Project:
            </td>
            <td>
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="select205">
                </asp:DropDownList>
            </td>
            <td width="30">
                Type:
            </td>
            <td>
                <asp:DropDownList ID="ddlTicketType" runat="server" CssClass="select100">
                  <asp:ListItem Value="-1">Please select...</asp:ListItem>
                    <asp:ListItem Value="0">Bug</asp:ListItem>
                    <asp:ListItem Value="1">Request</asp:ListItem>
                    <asp:ListItem Value="2">Risk</asp:ListItem>
                    <asp:ListItem Value="3">Issue</asp:ListItem>
                    <asp:ListItem Value="4">Change</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:ImageButton ID="iBtnSearch" runat="server" align="absmiddle" ImageUrl="/images/search_btn.jpg"
                    OnClick="SearchImgBtn_Click" Width="20px" />
            </td>
        </tr>
    </table>
    <div class="mainrightBoxtwo">
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="ModifiedOn" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="desc" />
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th style="width: 85px;" orderby="ProjectTitle">
                    Project
                </th>
                <th orderby="TicketID" style="width: 90px;">
                    Ticket Code
                </th>
                <th orderby="Title">
                    Title
                </th>
                <th style="width: 65px;" orderby="Priority">
                    Priority
                </th>
                <th style="width: 60px;" orderby="Status">
                    Status
                </th>
                <th style="width: 65px;" orderby="CreatedOn">
                    Created
                </th>
                <th style="width: 67px;" orderby="ModifiedOn">
                    Updated
                </th>
                <th style="width: 65px;">
                    Created By
                </th>
                <th style="width: 60px;">
                    Action
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="10" style="color: Red;">
                    &nbsp; No records
                </th>
            </tr>
            <asp:Repeater ID="rptTicketsList" runat="server">
                <ItemTemplate>
                    <tr opentype="popwindow" dialogwidth="830" dialogheight="645" dialogtitle="" href="/Sunnet/Clients/EditTicket.aspx?tid=<%#Eval("TicketID") %>"
                        freshbutton="<%#iBtnSearch.ClientID %>" class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                        <td>
                            <%# Eval("ProjectTitle").ToString()%>
                        </td>
                        <td>
                            <%#Eval("TicketCode").ToString()%>
                        </td>
                        <td>
                            <%# Eval("Title").ToString()%>
                        </td>
                        <td>
                            <%# Eval("Priority")%>
                        </td>
                        <td>
                            <%# Eval("Status")%>
                        </td>
                        <td>
                            <%# Eval("CreatedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#GetClientUserName(Convert.ToInt32(Eval("CreatedBy"))) %>
                        </td>
                        <td class="action">
                            <%# ShowDeleteImg(Eval("TicketID").ToString())%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
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
    </div>
</asp:Content>
