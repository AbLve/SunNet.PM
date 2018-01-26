<%@ Page Title="" Language="C#" MasterPageFile="~/SunnetTicket/Sunnet.master" AutoEventWireup="true" CodeBehind="MyCategory.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.MyCategory" %>

<%@ Import Namespace="SunNet.PMNew.PM2014.Codes" %>

<%@ Register Src="../UserControls/Messager.ascx" TagName="Messager" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        jQuery(function () {
            jQuery("#<%=ddlCategories.ClientID%>").change(function () {
                var $this = jQuery(this);
                window.location.href = location.pathname + "?category=" + $this.val();
            });
            var CategoryID = '<%= CurrentModel.GID%>';
            jQuery("a[action='removefromcategory'][ticket]").click(function () {
                var $this = $(this);
                jQuery.post("/Service/CateGory.ashx",
                    {
                        action: "removefromcategory",
                        ticketid: $this.attr("ticket"),
                        cagetory: CategoryID
                    }, function (responseData) {
                        if (responseData) {
                            $this.closest("tr").remove();
                        }
                    }, "json");
            });
        });

        function clickButton(buttonId) {
            jQuery("#" + buttonId).click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <uc1:Messager ID="Messager1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="topbtnbox">
        <asp:Button ID="btnEmpty" runat="server" Text="" CssClass="hide" OnClick="btnEmpty_Click" />
        <asp:Button ID="btnDelete" runat="server" Text="" CssClass="hide" OnClick="btnDelete_Click" />
        <ul class="listtopBtn">
            <asp:PlaceHolder ID="phlEmpty" runat="server">
                <li onclick="clickButton('<%=btnEmpty.ClientID %>')">
                    <div class="listtopBtn_icon">
                        <img src="/Images/icons/wtrashempty.png" />
                    </div>
                    <div class="listtopBtn_text">Empty Current Category</div>
                </li>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phlDelete" runat="server">
                <li onclick="clickButton('<%=btnDelete.ClientID %>')">
                    <div class="listtopBtn_icon">
                        <img src="/images/icons/wdelete.png" />
                    </div>
                    <div class="listtopBtn_text">Destroy Current Category</div>
                </li>
            </asp:PlaceHolder>
            <li href="NewCategory.aspx" data-target="#modalsmall" data-toggle="modal">
                <div class="listtopBtn_icon">
                    <img src="/images/icons/wnewcategory.png" />
                </div>
                <div class="listtopBtn_text">New Category</div>
            </li>
            <li style="border:none;">
                <asp:DropDownList ID="ddlCategories" CssClass="selectw1" runat="server" Height="32" style="padding:3px 2px;"></asp:DropDownList>
            </li>
        </ul>
    </div>
    <div style="width:100%;overflow-x:auto;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="30" class="aligncenter"></th>
                <th width="120" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="80" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="90" class="order" orderby="Priority">Priority<span class="arrow"></span></th>
                <th width="120" class="order" orderby="Status">Status<span class="arrow"></span></th>
                <th width="80" class="order order-desc" default="true" orderby="ModifiedOn">Updated<span class="arrow"></span></th>
                <th width="130" class="aligncenter">Action</th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="8" style="color: Red;">&nbsp; No record found
            </th>
        </tr>
        <asp:Repeater ID="rptTicketsList" runat="server">
            <ItemTemplate>
                <script type="text/html" id='ticket<%# Eval("TicketID")%>Description'><%# Server.HtmlEncode(Eval("FullDescription").ToString()) %></script>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> collapsed' ticket="<%# Eval("TicketID")%>">
                    <td class="aligncenter action">
                        <%# FeedBackButtonOrExpanded(Container.DataItem, this.ReturnUrl)%>
                    </td>
                    <td>
                        <%# Eval("ProjectTitle").ToString()%>
                    </td>
                    <td>
                        <%#Eval("TicketID").ToString()%>
                    </td>
                    <td>
                        <%# Eval("Title").ToString()%>
                    </td>
                    <td>
                        <%# Eval("Priority")%>
                    </td>
                    <td>
                        <%# GetStatus(Eval("Status"))%>
                    </td>
                    <td>
                        <%# Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td class="action aligncenter">
                        <a class="saveBtn1 mainbutton" href="Detail.aspx?tid=<%# Eval("TicketID")%>&returnurl=<%=this.ReturnUrl %>" style="display: none;" target="_blank" ticketId='<%# Eval("TicketID")%>'><span id='spanOpen<%# Eval("TicketID")%>'></span></a>
                        <%# GetActionHTML(Eval("ProjectID"),Eval("Status"),Eval("TicketID"),Eval("IsEstimates"),Eval("EsUserID"),Eval("CreatedBy"),(int)Eval("ConfirmEstmateUserId")) %>
                        <a href='###' action="removefromcategory" title="Remove from category" ticket='<%#Eval("TicketID") %>'>
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
</asp:Content>
