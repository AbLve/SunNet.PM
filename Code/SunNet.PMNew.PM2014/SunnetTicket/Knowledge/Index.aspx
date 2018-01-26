<%@ Page Title="Knowledge Share" Language="C#" MasterPageFile="~/SunnetTicket/Sunnet.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.Knowledge.Index" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SunNet.PMNew.Entity.ShareModel" %>
<%@ Import Namespace="SunNet.PMNew.PM2014.Codes" %>
<%@ Register TagPrefix="uc1" TagName="Messager" Src="~/UserControls/Messager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js"></script>
    <script type="text/javascript">
        $(function () {
            $("div.loading").remove();

            
            $("body").on("click", "[data-action='delete']", function () {
                var $this = $(this);
                var options = $.extend({}, { remote: "", key: "id" }, $this.data());
                if (options.remote && options[options.key] > 0) {
                    $.confirm("Confirm to delete?", {
                        yesText: "Delete",
                        yesCallback: function () {
                            jQuery.post(options.remote, options, function (response) {
                                if (response.success) {
                                    $this.closest("tr").remove();
                                } else {
                                    ShowMessage(response.msg, "danger");
                                }
                            }, "json");
                        },
                        noText: "Cancel"
                    });
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <uc1:Messager ID="Messager1" runat="server" />
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="90">Keyword:
                </td>
                <td width="150">
                    <asp:TextBox ID="txtKeyWord" placeholder="Enter Ticket ID, Note" queryparam="keyword" runat="server" CssClass="inputw1"></asp:TextBox>
                </td>
                <td width="80">Share Type:
                </td>
                <td width="155">
                    <asp:DropDownList ID="ddlType" queryparam="type" runat="server" CssClass="selectw1">
                    </asp:DropDownList>
                </td>
                <td width="80">Share User:
                </td>
                <td width="155">
                    <sunnet:ExtendedDropdownList ID="ddlUser" queryparam="user"
                        DataTextField="FirstAndLastName"
                        DataValueField="UserID"
                        DataGroupField="Status"
                        DefaultMode="List"
                        runat="server" CssClass="selectw1">
                    </sunnet:ExtendedDropdownList>
                </td>
            </tr>
            <tr>
                <td>Start Date:
                </td>
                <td>
                    <asp:TextBox ID="txtStartDate" queryparam="start" runat="server" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'})" CssClass="inputdate  inputw1"></asp:TextBox>
                </td>
                <td>End Date:
                </td>
                <td>
                    <asp:TextBox ID="txtEndDate" queryparam="end" runat="server" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'})" CssClass="inputdate inputw1"></asp:TextBox>
                </td>
                <td colspan="2">
                    <input type="button" class="searchBtn" id="btnSearch" />
                    <asp:Button ID="iBtnDownload" CssClass="otherBtn excelBtn" runat="server" Text="&nbsp;" ToolTip="Download Report" OnClick="iBtnDownload_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div style="width:100%;overflow-x:auto;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="120" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="80">Share Type</th>
                <th width="*" class="order" orderby="Note">Note<span class="arrow"></span></th>
                <th width="90">File</th>
                <th width="120">Created By</th>
                <th width="80" class="order order-desc" default="true" orderby="ModifiedOn">Updated<span class="arrow"></span></th>
                <th width="130" class="aligncenter">Action</th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="7" style="color: Red;">&nbsp; No record found
            </th>
        </tr>
        <asp:Repeater ID="rptShare" runat="server" OnItemDataBound="rptShare_ItemDataBound">
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> collapsed' ticket="<%# Eval("TicketID")%>">
                    <td class="action">
                        <a href="/SunnetTicket/Detail.aspx?tid=<%# Eval("TicketID")%>&returnurl=<%= this.ReturnUrl %>"><%# Eval("TicketID")%></a>
                    </td>
                    <td>
                        <%# Eval("TypeEntity.Title")%>
                    </td>
                    <td>
                        <%#Eval("Note")%>
                    </td>
                    <td class="action">
                        
                        <asp:Repeater ID="rptFiles" runat="server">
                            <ItemTemplate>
                                <div>
                                <a href='/do/DoDownloadFileHandler.ashx?FileID=<%#Eval("Key") %>' title='<%#Eval("Value") %>' target='_blank'>Download</a>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                    <td>
                        <%# GetClientUserName(Eval("CreatedBy"))%>
                    </td>
                    <td>
                        <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td class="action aligncenter">
                        <a href="View.aspx?id=<%#Eval("ID") %>" data-target="#modalsmall" data-toggle="modal" title="View">
                            <img src="/Images/icons/view.png" alt="View" />
                        </a>
                        <a href="Edit.aspx?id=<%#Eval("ID") %>" data-target="#modalsmall" data-toggle="modal" title="Edit"
                            class="<%#((int)Eval("CreatedBy"))==UserInfo.ID?"":"hide" %>">
                            <img src="/Images/icons/edit.png" alt="Edit" />
                        </a>
                        <a href='###' title="Delete"
                            data-remote="/Service/Share.ashx"
                            data-action="delete"
                            data-id="<%#Eval("ID") %>"
                            class="<%#((int)Eval("CreatedBy"))==UserInfo.ID?"":"hide" %>">
                            <img src="/Images/icons/delete.png" alt="Del" />
                        </a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpShare" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
