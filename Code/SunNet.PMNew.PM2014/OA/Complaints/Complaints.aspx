<%@ Page Title="Complaints" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="Complaints.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Complaints.Complaints" %>

<%@ Import Namespace="SunNet.PMNew.PM2014.OA.Complaints" %>
<%@ Import Namespace="SunNet.PMNew.Entity.ComplaintModel" %>
<%@ Import Namespace="SunNet.PMNew.Framework.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        @media (max-width:992px) {
        .toploginInfo{
            min-width:auto;
        }
        .topBox{
            min-width:auto;
            height:auto;
        }
        .topBox_logo img{
            width:100%;
        }
        ul.topmenu li {
            width: 80px;
            height: 55px;
            font-size: 12px;
            font-weight: 500;
            padding-top: 15px;
            margin-right: 5px;
        }
        ul.topmenu li .image{
            width: 19px;
            height: 20px;
            background-size: 100% 100% !important;
        }
        .mainleftTd{
            width:193px;
        }
        .leftmenuBox{
            width:180px;
        }
        .mainrightBox {
            min-width: auto;
            padding:10px;
        }
        .footerBox{
            min-width:auto;
        }
        .footerBox_left{
            width:auto;
        }
        .footerBox_right{
            width:auto;
        }
        input, select, textarea{
            width:100px !important;
        }
        .searchBtn{
            width:30px !important;
            margin-left:20px !important;
        }
        .limitwidth tr .inputw1{
            width:170px !important;
        }
        #body_bodySection_ddlAction{
            width:150px !important;
        }
    }
    </style>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="searchSection">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="60px">Keyword:
                </td>
                <td width="140px">
                    <asp:TextBox ID="txtKeyword" placeholder="Enter ID, Title" queryparam="Keyword" runat="server" CssClass="inputw1" Width="130"></asp:TextBox>
                </td>
                <td width="40px">Type:
                </td>
                <td width="140px">
                    <asp:DropDownList ID="ddlType" queryparam="Type" runat="server" CssClass="selectw1" Width="130">
                    </asp:DropDownList>
                </td>
                <td width="40px">Status:
                </td>
                <td width="140px">
                    <asp:DropDownList ID="ddlStatus" queryparam="Status" runat="server" CssClass="selectw3" Width="130">
                    </asp:DropDownList>
                </td>
                <td width="70px">Created By:
                </td>
                <td>
                    <asp:TextBox ID="txtUpdatedBy" placeholder="Enter First name, Last name" queryparam="UpdatedBy" runat="server" CssClass="inputw1" Width="170"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Reason:
                </td>
                <td>
                    <asp:DropDownList ID="ddlReason" queryparam="Reason" runat="server" CssClass="selectw1">
                    </asp:DropDownList>
                </td>
                <td>System:
                </td>
                <td>
                    <asp:DropDownList ID="ddlSystemID" queryparam="SystemID" runat="server" CssClass="selectw1">
                    </asp:DropDownList>
                </td>
                <td colspan="4">App Source:
                    <asp:DropDownList ID="ddlAppSrc" queryparam="AppSrc" runat="server" CssClass="selectw1">
                    </asp:DropDownList>
                
                    <input type="button" class="searchBtn" id="btnSearch" style="margin-left:146px;" />
                </td>
            </tr>
        </table>
        <table border="0" cellspacing="0" cellpadding="0" style="padding-top: 10px;">
            <tbody>
                <tr style="">
                    <td style="padding-right: 20px; color: #999;">
                        <img src="/images/icons/low.png" style="margin-top: -3px;">&nbsp;Low</td>
                    <td style="padding-right: 20px; color: #999;">
                        <img src="/images/icons/medium.png" style="margin-top: -3px;">&nbsp;Medium</td>
                    <td style="padding-right: 20px; color: #999;">
                        <img src="/images/icons/high.png" style="margin-top: -3px;">&nbsp;High</td>
                    <td style="padding-right: 20px; color: #999;">
                        <img src="/images/icons/emergency.png" style="margin-top: -3px;">&nbsp;Emergency</td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div style="width:100%; overflow-x:auto;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="100" class="order" orderby="ComplaintID" default>Complaint ID<span class="arrow"></span></th>
                <th width="80" class="order" orderby="Type">Type<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Reason">Reason<span class="arrow"></span></th>
                <th width="*" class="order" orderby="AdditionalInfo">Additional Info<span class="arrow"></span></th>
                <th width="100" class="order" orderby="SystemName">System<span class="arrow"></span></th>
                <th width="100" class="order" orderby="AppSrc">AppSrc<span class="arrow"></span></th>
                <th width="100" class="order" orderby="Status">Status<span class="arrow"></span></th>
                <th width="100" class="order" orderby="CreatedOn">Created On<span class="arrow"></span></th>
                <th width="100" class="order" orderby="UpdatedOn">Updated On<span class="arrow"></span></th>
                <th width="100" class="order" orderby="UpdatedByName">Updated By<span class="arrow"></span></th>
                <th width="80" class="aligncenter">Action</th>
            </tr>
        </thead>
        <tr runat="server" id="trNoComplaints" visible="false">
            <th colspan="8" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptUsers" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                    <td>
                        <%#Eval("ComplaintID")%>
                    </td>
                    <td>
                        <%#(SunNet.PMNew.Entity.ComplaintModel.Enums.ComplaintTypeEnum)Eval("Type")%>
                    </td>
                    <td>
                        <%#(SunNet.PMNew.Entity.ComplaintModel.Enums.ComplaintReasonEnum)Eval("Reason")%>
                    </td>
                    <td>
                        <%#Eval("AdditionalInfo")%>
                    </td>
                    <td>
                        <%#Eval("SystemName")%>
                    </td>
                    <td>
                        <%#(SunNet.PMNew.Entity.ComplaintModel.Enums.ComplaintAppSrcEnum)Eval("AppSrc") %>
                    </td>
                    <td>
                        <%#(SunNet.PMNew.Entity.ComplaintModel.Enums.ComplaintStatusEnum)Eval("Status")%>
                    </td>
                    <td>
                        <%#Eval("CreatedOn")%>
                    </td>
                    <td>
                        <%#Eval("UpdatedOn")%>
                    </td>
                    <td>
                        <%#Eval("UpdatedByName")%>
                    </td>
                    <td class="action aligncenter">
                        
                        <a href='./ComplaintReview.aspx?ComplaintID=<%#Eval("ComplaintID") %>' data-target="#modalsmall" data-toggle="modal" title="Complaint Review">
                            <img src="/Images/icons/share.png" alt="Knowledge" />
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
        <webdiyer:AspNetPager ID="ComplaintsPage" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>

