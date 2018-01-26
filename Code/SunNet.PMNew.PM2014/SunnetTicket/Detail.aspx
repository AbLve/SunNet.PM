﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SunnetTicket/Ticket.master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.Detail" %>

<%@ Register Src="~/UserControls/Ticket/Progress.ascx" TagPrefix="custom" TagName="progress" %>
<%@ Register Src="~/UserControls/Ticket/FeedbackList.ascx" TagPrefix="custom" TagName="feedbacks" %>
<%@ Register Src="~/UserControls/Ticket/TicketBasicInfoView.ascx" TagPrefix="custom" TagName="TicketBasicInfo" %>
<%@ Register Src="~/UserControls/Sunnet/ChangeHistory.ascx" TagPrefix="custom" TagName="ChangeHistory" %>
<%@ Register Src="~/UserControls/Messager.ascx" TagName="Messager" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagPrefix="custom" TagName="fileUpload" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #body_body_dataSection_ticketBasicInfo_rdoPriority {
            border: 0;
        }

        .form-group {
            display: block;
        }

        .col-right-1.righttext {
            width: auto;
            max-width: 900px;
        }

        .mainTable {
            border-radius: 5px;
            background: #ffffff;
            border: 1px solid #dddddd;
            table-layout: initial;
        }

        .ProcessBox {
            padding: 20px 0px 10px 0px;
            height: 20px;
            width: 100%;
        }

        .ProcessBox-td {
            padding: 20px;
        }

        .mainleftTd {
            width: 100%;
            padding: 20px 0;
            margin-top:20px;
        }

        .leftmenuBox {
            height: auto;
            min-width: 100%;
            min-height: 350px;
            border: 0px;
        }

        .mainrightBox {
            min-width: 250px;
            border-radius: 0px;
            padding: 0px;
            min-height: 350px;
            overflow: hidden;
            position: relative;
            border: 0px;
            border-top: 1px solid #6ca632;
            border-left:1px solid #ccc;
            margin-top:25px;
        }

        .form-group .col-lab-con {
            display: inline-block;
            width: 47%;
            border-bottom: 1px dashed #ddd;
            margin: 10px;
            margin-left: 0px;
        }

        .form-group .col-lab-con {
            border-bottom: 1px dashed #ddd;
        }

        .form-group .form-group-status {
            font-weight: bold;
            color: #fff;
            height: 42px;
            line-height: 42px;
        }

            .form-group .form-group-status .col-left-status {
                margin-right: 5px;
                color:#333;
                font-size:22px;
            }

            .form-group .form-group-status span {
                color:red;
                font-size:22px;
                font-weight:500;
            }

            .form-group .form-group-status .btn_ReviewE {
                color: #fff;
                background-color: #6CA632;
                border-radius: 5px;
                margin: 5px 0px 5px 20px;
                line-height: 32px;
                padding: 0 10px;
                display: inline-block;
                text-decoration: none;
                cursor: pointer;
                width:auto !important;
                vertical-align:middle;
                float:right;
                margin-right:180px;
            }

                .form-group .form-group-status .btn_ReviewE:hover {
                    color: #fff;
                }
                .form-group .form-group-status .btn_ReviewE span{
                    color:#fff;
                    font-size:13px;
                }

        .righttext_description {
            overflow-y: auto;
            margin-left: 115px;
            margin-top: -20px;
        }

        .titleeventlist {
            margin: 0px;
            padding: 15px 10px 10px 0px;
            color: #333;
            text-transform: none;
            font-size: 16px;
            color: #6ca632;
        }

            .titleeventlist .titleeventlist_icons {
                margin-right: 5px;
                margin-left: 10px;
            }

            .titleeventlist .titleeventlist_input {
                font-size: 14px;
                font-weight: normal;
                float: right;
                color: #333;
            }

        .fdcontentBox1 {
            background-color: #fff;
        }

        .chat_peoples {
            overflow: hidden;
            padding: 15px 10px 10px;
            border-bottom: 1px #ddd dashed;
        }

            .chat_peoples .chat_peoples_s {
                margin-bottom: 10px;
                display: inline-block;
                color: #999;
            }

            .chat_peoples ul.assignUser li {
                width: 30%;
                margin-bottom: 8px;
            }

        .feedbackBox {
            background-color: #f5f5f5;
            padding: 0 10px;
        }

            .feedbackBox #dvShowEarlier {
                background: none;
                margin: 5px 0px;
            }

                .feedbackBox #dvShowEarlier a {
                    color: #666;
                    padding: 3px 10px;
                    border-radius: 15px;
                    background-color: #e3e2e2;
                    text-decoration: none;
                }

                    .feedbackBox #dvShowEarlier a:hover {
                        color: #333;
                        background-color: #ddd;
                    }

            .feedbackBox .requestfrom {
                text-align: center;
                padding: 5px;
                margin: 5px 0px;
                cursor: pointer;
            }

                .feedbackBox .requestfrom a {
                    color: #eee;
                    padding: 3px 10px;
                    border-radius: 15px;
                    background-color: #F60;
                    text-decoration: none;
                }

                    .feedbackBox .requestfrom a:hover {
                        color: #ffffff;
                        background-color: #F60;
                    }

        .chat_input_box {
            padding: 20px 10px;
            text-align: center;
        }

        .chat_btn_upload {
            background-color: #d0762d;
            padding: 20px 11px;
            border-radius: 30px;
            cursor: pointer;
        }

            .chat_btn_upload:hover {
                background-color: #c16b25;
            }

        .chat_btn_push {
            background-color: #6ca632;
            padding: 20px 11px;
            border-radius: 30px;
            cursor: pointer;
        }

            .chat_btn_push:hover {
                background-color: #609629;
            }

        .chat_btns_u {
            text-align: right;
        }

        .chat_btns_p {
            text-align: left;
        }

        .inputFeedback {
            height: 35px;
            width: 60%;
            padding: 0px;
            vertical-align: middle;
            /*border-radius: 10px;*/
            padding: 10px;
            margin: 0 10px;
        }

        .form-group-btns {
            overflow: inherit;
            margin-top: 15px;
            margin-right: 10px;
            padding-bottom: 10px;
            margin-bottom: 10px;
        }

        .footerBox {
            width: auto;
            min-width: 0;
        }

        .topBox {
            min-width: 1220px;
        }

        .toploginInfo {
            min-width: 700px;
        }

        .fdUser {
            padding-right: 10px;
        }

        .otherbox .fdUser > span {
            width: auto;
            line-height: 26px;
        }

        .fdDate {
            line-height: 22px;
        }

        ul.fdItembox li {
            width: 96%;
        }

        .fdarrowbox {
            left: 35px;
            margin-left: 0px;
            margin-top: 12px;
            position: absolute;
        }

        .myselfbox .fdarrowbox {
            float: none;
            right: 35px;
            margin-left: 0px;
            margin-top: 10px;
            position: absolute;
        }

        .col-right-col2 {
            width: 150px;
        }

        .btn_showRight {
            display: none;
        }

        .btn_disRight {
            display: none;
        }
        .TicketDetails{
            position:absolute;
            left:0px;
            top:-8px;
            font-size:16px;
            font-weight:700;
            border-top:1px solid #ccc;
            width:100%;
        }
        .TicketDetails span{
            position:absolute;
            left:0px;
            top:-10px;
            font-size:16px;
            font-weight:700;
            background:#fff;
            padding-right:10px;
        }

        @media (max-width: 1420px) {
            .chat_peoples ul.assignUser li {
                width: 45%;
            }
            .form-group .form-group-status .btn_ReviewE{
                margin-right:120px;
            }
        }

        @media (max-width: 1320px) {
            #fbScroll {
                height: 260px !important
            }
            ul.ticketProcess li.roundicon{
                width:15px;
                height:15px;
            }
            ul.ticketProcess li.ongoing,ul.ticketProcess li.completed,ul.ticketProcess li.nofinish{
                background-size:100% 100%;
            }
            ul.ticketProcess li.processtext{
                margin-top:-13px;
            }
        }

        @media (max-width: 1250px) {
            ul.ticketProcess li.processtext{
                font-size:11px;
                padding:0;
            }
            .form-group .form-group-status .btn_ReviewE{
                margin-right:80px;
            }
        }
        @media (max-width: 870px) {
            .form-group .form-group-status .btn_ReviewE{
                margin-right:20px;
            }
        }

        .feedback_waiting {
            border-radius: 5px;
        }

        .menu-left-l {
            border-bottom: 1px #ddd dashed;
            padding: 0px 0px;
            margin: 5px;
            margin-left: 0px;
            width: 245px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="mainTable">
        <tbody>
            <tr>
                <td class="ProcessBox-td" width="60%" style="vertical-align:top;">
                    <div class="form-group" style="overflow: visible">
                                    <div class="form-group-status">
                                        <label class="col-left-status">Ticket Status:</label>
                                        <span><%=GetDisplayStatus() %></span>

                                        <% 
                                            if (ReviewUrl != "")
                                            { %>
                                        <input type="button" href="<%= ReviewUrl %>" data-toggle="modal" data-target="#modalsmall" value="<%= ReviewName %>" class="btn_ReviewE" />
                                        <% 
                                            }
                                            if (isReadyForReview)
                                            {
                                        %>
                                        <input type="button" href="<%= UrlApprove %>" data-target="#modalsmall" data-toggle="modal" value="Approve" class="btn_ReviewE" />
                                        <input type="button" href="<%= UrlDeny %>" data-target="#modalsmall" data-toggle="modal" value="Deny" class="btn_ReviewE" />
                                        <%
                                            }
                                        %>

                                        <asp:PlaceHolder ID="phlWorkingOn" runat="server">
                                            <div class="btn-group" data-remote="workingon">
                                                <button type="button" style="margin-left:5px;" class="btn_ReviewE mainbutton dropdown-toggle" data-workingstatus="<%=ticketEntity.TicketID %>" data-toggle="dropdown">
                                                    <span class="text">
                                                        <asp:Literal ID="ltlStatus" runat="server"></asp:Literal></span>
                                                    <span class="caret"></span>
                                                </button>
                                                <ul class="dropdown-menu" role="menu">
                                                    <li><a href="javascript:;" ticket="<%=ticketEntity.TicketID %>" data-action="setworkingon">WorkingOn</a></li>
                                                    <li><a href="javascript:;" ticket="<%=ticketEntity.TicketID %>" data-action="setworkingcomplete">Completed</a></li>
                                                    <li><a href="javascript:;" ticket="<%=ticketEntity.TicketID %>" data-action="setworkingcancelled">Cancelled</a></li>
                                                    <li class="divider"></li>
                                                    <li><a href="javascript:;" ticket="<%=ticketEntity.TicketID %>" data-action="setworkingonnone">None</a></li>
                                                </ul>
                                            </div>
                                        </asp:PlaceHolder>

                                    </div>
                                </div>
                    <custom:progress runat="server" ID="progress" />
                    <div class="mainleftTd" style="position:relative">
                        <div style="position:relative;margin-top:-10px;margin-bottom:10px;">
                            <div class="TicketDetails"><span>Ticket Details</span></div>
                        </div>
                        <div class="mainleftBox">
                        <div class="limitwidth">
                            <div class="form-group-container" style="width: 100%;">
                                <div class="alert alert-success fade hide">
                                    <img src="/Images/icons/success.png">Operation successful.
                                </div>
                                <div class="alert alert-danger fade hide">
                                    <img src="/Images/icons/failed.png">
                                    <a class="close" data-dismiss="alert" href="https://pm.sunnet.us/demo/page_ticket2.html###" aria-hidden="true">×</a> Operation failed.
                                </div>

                                

                                <div class="form-group">
                                    <label class="col-left-1 lefttext">Ticket ID:</label>
                                    <div class="col-right-1 righttext">
                                        <%=ticketEntity.TicketID %>
                                    </div>
                                    <div class="col-right-1 righttext" style="padding-left:5px;">
                                    <% if (ticketEntity.Status == SunNet.PMNew.Entity.TicketModel.TicketsState.Completed)
                                        { %>
                                            <div class="starcontainer">
                                                <div class="color_bg">
                                                </div>
                                                <div style="width: <%=Convert.ToInt32(ticketEntity.Star)*16 %>px;" class="color">
                                                </div>
                                                <div class="star">
                                                    <div title="Poor">
                                                    </div>
                                                    <div title="Fair">
                                                    </div>
                                                    <div title="Average">
                                                    </div>
                                                    <div title="Good">
                                                    </div>
                                                    <div title="Excellent">
                                                    </div>
                                                </div>
                                            </div>
                                    <%} %>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-left-1 lefttext">Title:</label>
                                    <div class="col-right-1 righttext">
                                        <%=ticketEntity.Title %>
                                    </div>
                                </div>

                                <% if (ticketEntity.FullDescription.Length > 0)
                                   { %>
                                <div class="form-group">
                                    <label class="col-left-1 lefttext">Description:</label>
                                    <div class="col-right-1 righttext">
                                        <%=Server.HtmlEncode(ticketEntity.FullDescription).Replace("\r\n","<br>")%>
                                    </div>
                                </div>
                                <% } %>


                                <% if (ticketEntity.URL.Length > 0)
                                   { %>
                                <div class="form-group">
                                    <label class="col-left-1 lefttext">URL:</label>
                                    <div class="col-right-1 righttext nowrap">
                                        <a href="<%=ticketEntity.URL %>" target="_blank"><%=ticketEntity.URL %></a>
                                    </div>
                                </div>
                                <% } %>


                                <div class="form-group" id="attachDiv">
                                    <label class="col-left-1 lefttext">Attachments:</label>
                                    <div class="col-right-1 righttext" id="attachFiles">
                                        <custom:fileUpload runat="server" ID="fileUpload" UploadType="View" />
                                    </div>
                                </div>

                                <div style="border-bottom: 1px #ddd solid; padding-bottom: 10px;"></div>
                                <br />
                            </div>

                            <div class="fdshowmore" id="dvshowmore" onclick="showMore();">&gt;&gt; <strong>Show More</strong></div>
                            <div id="divmore" style="display: none;">
                                <div class="form-group">
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Project:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.ProjectTitle %></div>
                                    </div>
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Type:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.TicketType.ToText() %> </div>
                                    </div>

                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Priority:</label>
                                        <div class="col-right-col2 righttext"><%=GetPriority((int)ticketEntity.Priority) %> </div>
                                    </div>

                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Accounting:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.Accounting.ToText() %></div>
                                    </div>
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Internal:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.IsInternal ? "Yes" : "No" %></div>
                                    </div>
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Source:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.Source.ToText() %></div>
                                    </div>
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Created By:</label>
                                        <% if (ticketEntity.IsInternal)
                                           {%>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.CreatedUserEntity.FirstAndLastName %></div>
                                        <%}
                                           else
                                           { %>
                                        <%  if (UserInfo.Role == SunNet.PMNew.Entity.UserModel.RolesEnum.PM || UserInfo.Role == SunNet.PMNew.Entity.UserModel.RolesEnum.Sales)
                                            { %>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.CreatedUserEntity.FirstAndLastName %></div>
                                        <%}
                                            else
                                            { %>
                                        <div class="col-right-col2 righttext">CLIENT</div>
                                        <%} %>
                                        <%} %>
                                    </div>
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Created Date:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.CreatedOn.ToString("MM/dd/yyyy") %></div>
                                    </div>
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Proprosal Name:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.ProprosalName %></div>
                                    </div>
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Work Plan:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.WorkPlanName%></div>
                                    </div>
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Work Scope:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.WorkScope %></div>
                                    </div>
                                    <div class="col-lab-con">
                                        <label class="col-left-1 lefttext">Invoice #:</label>
                                        <div class="col-right-col2 righttext"><%=ticketEntity.Invoice %></div>
                                    </div>
                                </div>
                                <custom:ChangeHistory runat="server" ID="changeHistory" />
                            </div>

                            <script type="text/javascript">
                                function showMore() {
                                    $("#dvshowmore").css("display", "none");
                                    $("#divmore").toggle("fast");
                                }

                                $(function () {
                                    if ($("#attachFiles").is(':has(ul)') == false) {
                                        $("#attachFiles").parent().hide();
                                    }
                                })
                            </script>

                        </div>
                    </div>
                    </div>
                </td>
                <td class="mainrightTd" width="40%" style="vertical-align:top;">
                    <div class="mainrightBox">
                        <div class="limitwidth">
                            <div class="form-group-container" style="width: 100%;">
                                <uc1:Messager ID="Messager1" runat="server" />
                                <custom:feedbacks runat="server" ID="feedbacks" />
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>

    <div class="footerBox">
        <div class="footerBox_left">
            <a href="/About/Faqs.aspx?returnurl=%2fSunnetTicket%2fDashboard.aspx">FAQ</a> <span>|</span>
            <a href="/About/Survey.aspx?returnurl=%2fSunnetTicket%2fDashboard.aspx">Survey</a><span>|</span>
            <a href="/About/ContactUs.aspx?returnurl=%2fSunnetTicket%2fDashboard.aspx">Contact Us</a>
        </div>
        <div class="footerBox_right">Copyright © <%=DateTime.Now.Year %> <a href="http://www.sunnet.us" target="_blank">SunNet Solutions</a>. </div>
    </div>

</asp:Content>
