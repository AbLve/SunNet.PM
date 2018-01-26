<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="TicketSelect.aspx.cs"
    Inherits="SunNet.PMNew.PM2014.Ticket.Sunnet.TicketSelect" %>

<%@ Import Namespace="SunNet.PMNew.PM2014.Codes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <script type="text/javascript">
        var returnValStr = "";
        $(function () {
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div",
                onkeyup: false,
                onfocusout: false
            });

            $("#btnAssign").click(function () {
                $("input[name='chkTicketId']:checked").each(function (index, obj) { returnValStr = returnValStr + obj.value + "_"; });
                returnValStr = getQueryString("day") + "&" + returnValStr;
                window.returnValue = returnValStr;
                ClosePopWindow();
            });
        });
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="titleSection" runat="server">
    Select Ticket
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodySection" runat="server">
    <style type="text/css">
        textarea {
            resize: none;
            width: 456px;
            height: 86px;
            max-width: 456px;
            max-height: 86px;
        }

        .col-right-weekplan2 {
            width: 130px;
            padding-top: 3px;
        }

        .col-right-weekplan3 {
            width: 360px;
        }

        .weekplan-con {
            width: 500px;
            min-height: 100px;
            float: left;
            overflow: hidden;
        }

        .col-right-weekplan3 span {
            display: inline-block;
            margin-right: 15px;
            min-width: 55px;
            border-bottom: 1px #ddd dashed;
            line-height: 24px;
            margin-top: 5px;
        }

        .weekplan-t-btn {
            background-color: #036ac9;
            border-radius: 3px;
            color: #ffffff;
            padding: 2px 5px;
        }

            .weekplan-t-btn:hover {
                color: #ffffff;
                background-color: #1e90ff;
            }
    </style>
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="60">Keyword:
                </td>
                <td width="150">
                    <asp:TextBox ID="txtKeyWord" placeholder="Enter ID, Title" queryparam="keyword" runat="server" CssClass="inputw1"></asp:TextBox>
                </td>
                <td width="40">Project:
                </td>
                <td width="155">
                    <sunnet:ExtendedDropdownList ID="ddlProject" queryparam="project"
                        DataTextField="Title"
                        DataValueField="ProjectID"
                        DataGroupField="Status"
                        DefaultMode="List" runat="server" CssClass="selectw1">
                    </sunnet:ExtendedDropdownList>
                </td>
                <td width="90">Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" queryparam="status" runat="server" CssClass="selectw3">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="60">Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTicketType" queryparam="tickettype" runat="server" CssClass="selectw1">
                        <asp:ListItem Value="-1">ALL</asp:ListItem>
                        <asp:ListItem Value="0">Bug</asp:ListItem>
                        <asp:ListItem Value="1">Request</asp:ListItem>
                        <asp:ListItem Value="2">Risk</asp:ListItem>
                        <asp:ListItem Value="3">Issue</asp:ListItem>
                        <asp:ListItem Value="4">Change</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" />
                </td>
                <td></td>
                <td></td>
                <td>
                    <a class="listtopBtn_text weekplan-t-btn"
                        style="text-decoration: none; cursor: pointer; float: right" id="btnAssign">Assign Tickets</a>
                    <%-- <input type="button" class="add-tBtn" id="btnAssign" />--%>
                </td>
            </tr>
        </table>
    </div>

    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="120" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="80" class="order order-desc" default="true" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="*" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="80">Created By</th>
                <th width="130" class="aligncenter">Action</th>
            </tr>

        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="5" style="color: Red;">&nbsp; No record found
            </th>
        </tr>
        <asp:Repeater ID="rptTicketsList" runat="server">
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> collapsed' ticket="<%# Eval("TicketID")%>">
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
                        <%# GetClientUserName(Eval("CreatedBy"))%>
                    </td>
                    <td class="action aligncenter">

                        <input type="checkbox" name="chkTicketId" id='T<%#Eval("TicketID").ToString()%>' value='<%#Eval("TicketID").ToString()%>' />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpOngoing" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>
