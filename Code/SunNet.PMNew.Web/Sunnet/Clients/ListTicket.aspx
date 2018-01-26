<%@ Page Title="Ongoing Tickets" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="ListTicket.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.ListTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .starcontainer {
            height: 16px;
            overflow: hidden;
            width: 80px;
        }

        .color_bg {
            background-image: url("/icons/grey_star.png");
            top: 0;
        }

        .color {
            background-image: url("/icons/orange_star.png");
            top: -16px;
            width: 32px;
        }

        .star, .color, .color_bg {
            height: 16px;
            left: 0;
            position: relative;
            width: 80px;
        }

        .star {
            background-repeat: repeat no-repeat;
            top: -32px;
        }

            .star div {
                cursor: pointer;
                display: inline-block;
                float: left;
                height: 16px;
                margin: 0;
                padding: 0;
                width: 16px;
            }

        .Related A {
            display: block;
            float: left;
            margin-right: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Ongoing Tickets
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="50">Keyword:
            </td>
            <td width="260">
                <asp:TextBox ID="txtKeyWord" runat="server" CssClass="input200"></asp:TextBox>
            </td>
            <td width="40">Status:
            </td>
            <td width="160">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select100">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Project:
            </td>
            <td>
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="select205" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td width="30">Type:
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
    <div class="mainactionBox">
        <span><a href="#" onclick="OpenAddModuleDialog('0')">
            <img src="/icons/14.gif" border="0" align="absmiddle" alt="new/add" />New Bug</a></span>
        <span><a href="#" onclick="OpenAddModuleDialog('1')">
            <img src="/icons/09.gif" border="0" align="absmiddle" alt="new/add" />New Request</a></span>
        <%-- <span><a href="#" onclick="OpenAddListDialog()">
            <img src="/icons/07.gif" border="0" align="absmiddle" alt="new/add" />Multiple Add</a></span>--%>
        <span><a href="#" onclick="OpenEditPriorityDialog()">
            <img src="/icons/ico_priority.gif" border="0" align="absmiddle" alt="new/Priority" />Priority</a></span>
    </div>
    <div class="mainrightBoxtwo">
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="ModifiedOn" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="desc" />
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th style="width: 85px;" orderby="ProjectTitle">Project
                </th>
                <th orderby="TicketID" style="width: 90px;">Ticket Code
                </th>
                <th orderby="Title">Title
                </th>
                <th style="width: 65px;" orderby="Priority">Priority
                </th>
                <th style="width: 120px;" orderby="Status">Status
                </th>
                <th style="width: 65px;" orderby="CreatedOn">Created
                </th>
                <th style="width: 67px;" orderby="ModifiedOn">Updated
                </th>
                <th style="width: 65px;">Created By
                </th>
                <th style="width: 90px;">Related Tickets
                </th>
                <th style="width: 100px;">Ratings
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="11" style="color: Red;">&nbsp; No records
                </th>
            </tr>
            <asp:Repeater ID="rptTicketsList" runat="server">
                <ItemTemplate>
                    <tr dialogwidth="880" dialogheight="660" <%# ShowEditTicket(Eval("TicketID"), Eval("Status"))%>
                        class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
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
                        <td <%# ShowActionByFbMsg(FeedBackMessage(Eval("TicketID"))) %>>
                            <%# GetClientStatusNameBySatisfyStatus((int)Eval("Status"), (int)Eval("TicketID"))%>
                            <%# FeedBackMessage(Eval("TicketID"))%>
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
                        <td style="width: 90px;" class="action Related">
                            <%# ShowRelatedByTid(Eval("TicketID").ToString())%>
                        </td>
                        <td class="action">
                            <div class="starcontainer">
                                <div class="color_bg">
                                </div>
                                <div style="width: <%#Convert.ToInt32(Eval("Star").ToString())*16 %>px;" class="color">
                                </div>
                                <div createuser='<%#Eval("CreatedBy") %>' ticketid='<%#Eval("TicketID") %>' scoreid="170"
                                    class="star">
                                    <div title="1 of 5 stars" id="d1">
                                    </div>
                                    <div title="2 of 5 stars" id="d2">
                                    </div>
                                    <div title="3 of 5 stars" id="d3">
                                    </div>
                                    <div title="4 of 5 stars" id="d4">
                                    </div>
                                    <div title="5 of 5 stars" id="d5">
                                    </div>
                                </div>
                            </div>
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

    <script type="text/javascript">
        var oldWidth = 0;
        var starWidth = 16;
        var LoginUserID = '<%=UserInfo.ID %>';
        $(document).ready(function () {
            $("#<%=txtKeyWord.ClientID%>").focus();


            jQuery("div.starcontainer div[createuser='" + LoginUserID + "']").children().hover(
               function () {
                   var _div_Star = $(this);
                   oldWidth = _div_Star.parent().prev().width();
                   _div_Star.parent().prev().width((_div_Star.index() + 1) * starWidth);
               },
               function () {
                   var _div_Star = $(this);
                   _div_Star.parent().prev().width(oldWidth);
               }
            );



            jQuery("div.starcontainer div[createuser='" + LoginUserID + "']").children().click(
                 function () {
                     var _div_Star = jQuery(this);

                     jQuery.getJSON("/Do/TicketStar.ashx?r=" + Math.random(),
                        {
                            type: "UpdateStar",
                            ticketid: _div_Star.parent().attr("ticketid"),
                            star: _div_Star.index() + 1
                        },
                        function (responseData) {
                            if (responseData) {
                                oldWidth = (_div_Star.index() + 1) * starWidth;
                            }
                            else {
                            }
                        }
                    );

                 }
               );

        });

        function ViewTicketModuleDialog(selectTicketId) {
            var result = ShowIFrame("/Sunnet/Clients/ViewRelatedTicket.aspx?tid=" + selectTicketId + "&is0hsisdse=54156", 880, 660, true, "View Related Ticket");
        }
        function OpenAddModuleDialog(bType) {
            var result = ShowIFrame("/Sunnet/Clients/AddTicket.aspx?addType=" + bType, 880, 520, true, "Add Bug/Request Ticket");
            if (!result)
                document.getElementById("<% =iBtnSearch.ClientID %>").click();
    }
    function OpenAddListDialog() {
        var result = ShowIFrame("/Sunnet/Clients/AddTicketList.aspx", 875, 800, true, "Add Ticket List");
        if (result == 0)
            document.getElementById("<% =iBtnSearch.ClientID %>").click();
    }
    function OpenEditPriorityDialog() {
        window.open("/Sunnet/Clients/EditPriority.aspx");
    }
    function OpenTicketDetail(tid, type) {
        if (type == 'f') {
            window.open("/Sunnet/Clients/TicketsDetail.aspx?tid=" + tid + "#Feedback");
        } else {
            window.open("/Sunnet/Clients/TicketsDetail.aspx?tid=" + tid);
        }
    }
    function OpenReplyFeedBackDialog(fid, tid) {
        var result = ShowIFrame("/Sunnet/Clients/AddFeedBacks.aspx?feedbackId=" + fid + "&tid=" + tid, 580, 430, true, "Reply FeedBack");
        if (result == 0) {
            window.location.reload();
        }
    }
    </script>

</asp:Content>
