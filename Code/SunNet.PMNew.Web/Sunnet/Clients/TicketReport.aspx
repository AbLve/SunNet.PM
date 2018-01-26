<%@ Page Title="Ticket Report" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="TicketReport.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.TicketReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .faqsText {
            padding: 5px 10px 5px 25px;
            display: none;
            position: absolute;
            left: 0;
            right: 0;
            background-color: #f1f0f0;
            width: 500px;
        }

            .faqsText p {
                border-bottom: 1px solid #ddd;
            }

        .mainactionBox1 {
            border-top: 1px solid #81bae8;
            border-left: 1px solid #81bae8;
            border-right: 1px solid #81bae8;
            background-image: url(/images/action_bg.jpg);
            padding: 5px;
            color: #083583;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Ticket Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <th width="40">&nbsp;Project:
            </th>
            <td width="170">
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="select205" Width="150">
                </asp:DropDownList>
            </td>
            <th width="30">Type:
            </th>
            <td width="100">
                <asp:DropDownList ID="ddlTicketType" runat="server" CssClass="select205" Width="80">
                    <asp:ListItem Value="-1">ALL</asp:ListItem>
                    <asp:ListItem Value="0">Bug</asp:ListItem>
                    <asp:ListItem Value="1">Request</asp:ListItem>
                    <asp:ListItem Value="2">Risk</asp:ListItem>
                    <asp:ListItem Value="3">Issue</asp:ListItem>
                    <asp:ListItem Value="4">Change</asp:ListItem>
                </asp:DropDownList>
            </td>
            <th width="40">Status:
            </th>
            <td width="160">
                <asp:DropDownList ID="ddlStatus" CssClass="select205" runat="server" Width="140">
                </asp:DropDownList>
            </td>
            <th width="40">Keyword:
            </th>
            <td width="220">
                <asp:TextBox ID="txtKeywords" CssClass="input200"
                    runat="server"></asp:TextBox>
            </td>
            <td width="*">
                <asp:ImageButton ID="iBtnSearch" ToolTip="Search" ImageUrl="/images/search_btn.jpg"
                    runat="server" OnClick="iBtnSearch_Click" />&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="iBtnDownload" ToolTip="DownLoad Report" ImageUrl="/images/ex.gif"
                    runat="server" OnClick="iBtnDownload_Click" />
            </td>
        </tr>
    </table>
    <div class="mainactionBox1" style="position: relative; overflow; none;">
        <span><a href="#" onclick="TicketStatus();">What is the meaning of Status?</a> </span>
        <div class="faqsText" id="div7">
            <p>
                <strong class="faqsspantext1">Draft </strong>- <u>Draft</u> stands for that you
                have just write a draft,it has not entered into the system.
            </p>
            <p>
                <strong class="faqsspantext1">Submitted </strong>- <u>Submitted tickets</u> have
                been successfully entered into the system, but have not yet been reviewed by your
                Project Manager.
            </p>
            <p>
                <strong class="faqsspantext1">In progress </strong>- <u>In Progress tickets</u>
                have been reviewed by your Project Manager and are currently under review or in
                the development stage.
            </p>
            <p>
                <strong class="faqsspantext1">Estimating </strong>- <u>Estimating</u> tickets are
                estimating.
            </p>
            <p>
                <strong class="faqsspantext1">Estimation Fail </strong>- <u>Estimation Fail</u>
                tickets were denied by saler.
            </p>
            <p>
                <strong class="faqsspantext1">Waiting FeedBack </strong>- <u>Waiting FeedBack</u>
                tickets need your review and feedback before we can proceed.
            </p>
            <p>
                <strong class="faqsspantext1">WaitingVerify </strong>- <u>WaitingVerify</u> tickets
                have already been published to your production/live site and need your review and
                approval.
            </p>
            <p>
                <strong class="faqsspantext1">Not Approved </strong>- <u>Not Approved</u> ticket
                has been released on product server ,but you didn't approve.
            </p>
            <p>
                <strong class="faqsspantext1">Completed </strong>- <u>Completed</u> tickets have
                been approved and published to your production/live site. These tickets are considered
                closed.
            </p>
            <p>
                <strong class="faqsspantext1">Cancelled</strong> - <u>Cancelled</u> tickets are
                tickets that are no longer being worked on but continue to be stored in the client
                portal.
            </p>
        </div>
        <asp:HiddenField ID="hidOrderBy" runat="server" Value="ProjectTitle" />
        <asp:HiddenField ID="hidOrderDirection" runat="server" Value="ASC" />
    </div>
    <div class="mainrightBoxtwo">
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th orderby="ProjectTitle" style="width: 10%;">Project Title
                </th>
                <th orderby="TicketType" style="width: 5%;">Type
                </th>
                <th orderby="TicketCode" style="width: 25%;">Ticket Code/Ticket Title
                </th>
                <th orderby="CreatedOn" style="width: 15%;">Created
                </th>
                <th orderby="ModifiedOn" style="width: 15%;">Updated
                </th>
                <th orderby="Status" style="width: 10%;">Status
                </th>
                <th orderby="Priority" style="width: 10%;">Priority
                </th>
                <th orderby="CreatedBy" style="width: 15%;">Created By
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="8" style="color: Red;">&nbsp; No records
                </th>
            </tr>
            <asp:Repeater ID="rptTicketsReport" runat="server">
                <ItemTemplate>
                    <tr <%# ShowEditTicket(Eval("TicketID"), Eval("Status"))%> class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                        <td>
                            <%#Eval("ProjectTitle")%>
                        </td>
                        <td>
                            <%#Eval("TicketType")%>
                        </td>
                        <td>[<%#Eval("TicketCode")%>]<%#Eval("Title")%>
                        </td>
                        <td>
                            <%#Eval("CreatedOn","{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#Eval("ModifiedOn", "{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#GetClientStatusNameBySatisfyStatus((int)Eval("Status"),Eval("TicketID"))%>
                        </td>
                        <td>
                            <%#Eval("Priority")%>
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
            <webdiyer:AspNetPager ID="anpTicketReport" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpTicketReport_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>

    <script type="text/javascript">
        function TicketStatus() {
            jQuery("#div7").slideToggle("fast");
        }
        jQuery(function () {
            jQuery("#div7").mouseleave(function () {
                jQuery(this).slideUp("fast");
            });
        });
    </script>

</asp:Content>
