<%@ Page Title="" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="SealRequests.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Seals.SealRequests" %>

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
            width:80px !important;
        }
        .searchBtn{
            width:28px !important;
        }
        .limitwidth tr .inputdate{
            width:80px !important;
        }
        #body_body_searchSection_txtKeyword{
            width:80px !important;
        }
        .searchBtn{
            margin-left:0 !important;
        }
    }
    </style>
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="30px">Keyword:
                </td>
                <td width="150px">
                    <asp:TextBox ID="txtKeyword" runat="server" queryparam="keyword" CssClass="input200" Width="170" placeholder="Enter Title"></asp:TextBox>
                </td>

                <td width="30px">Type:
                </td>
                <td width="130px">
                    <asp:DropDownList ID="ddlType" runat="server" queryparam="type" CssClass="select205" Width="120" onchange="OnDdlTypeChange(this)">
                        <asp:ListItem Value="-1">All</asp:ListItem>
                        <asp:ListItem Value="0">Seal</asp:ListItem>
                        <asp:ListItem Value="1">Work Flow</asp:ListItem>
                    </asp:DropDownList>
                </td>

                <td width="30px" class="sealTextCls" style="display:none">Seal:
                </td>
                <td width="130px" class="sealDdlCls" style="display:none">
                    <asp:DropDownList ID="ddlSeal" runat="server" queryparam="seal" CssClass="select205" Width="120" DataValueField="ID"
                        DataTextField="SealName">
                    </asp:DropDownList>
                </td>

                <td width="50">Status:
                </td>
                <td width="130px">
                    <asp:DropDownList ID="ddlStatus" runat="server" queryparam="status" CssClass="select205" Width="120">
                        <asp:ListItem Value="-88">ALL</asp:ListItem>
                        <asp:ListItem Value="-1">Denied</asp:ListItem>
                        <asp:ListItem Value="0">Canceled</asp:ListItem>
                        <asp:ListItem Value="1">Draft</asp:ListItem>
                        <asp:ListItem Value="2">Submitted</asp:ListItem>
                        <asp:ListItem Value="3">Pending Approval</asp:ListItem>
                        <asp:ListItem Value="4">Approved</asp:ListItem>
                        <asp:ListItem Value="5">Pending Process</asp:ListItem>
                        <asp:ListItem Value="6">Processed</asp:ListItem>
                        <asp:ListItem Value="7">Completed</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="80px" align="right">Start Date:
                </td>
                <td width="130px">
                    <asp:TextBox ID="txtStartDate" queryparam="start" onclick="WdatePicker({isShowClear:false});"
                        CssClass="input200 inputdate" Style="width: 120px;" runat="server"></asp:TextBox>
                </td>

                <td width="80" align="right">End Date:
                </td>
                <td width="130px">
                    <asp:TextBox ID="txtEndDate" queryparam="end" onclick="WdatePicker({isShowClear:false});"
                        CssClass="input200 inputdate" Style="width: 120px;" runat="server"></asp:TextBox>
                </td>
                
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        function OnDdlTypeChange(obj) {
            if ($("#<%= ddlType.ClientID%>").val() == 0) {
                $(obj).parent().siblings(".sealTextCls").show();
                $(obj).parent().siblings(".sealDdlCls").show();
            } else {
                $(obj).parent().siblings(".sealTextCls").hide();
                $(obj).parent().siblings(".sealDdlCls").hide();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
  <%--  <% if (UserInfo.Role == SunNet.PMNew.Entity.UserModel.RolesEnum.PM)
       { %>--%>
    <div class="topbtnbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td>
                        <ul class="listtopBtn">
                            <li>
                                <div class="listtopBtn_icon">
                                    <img src="/Images/icons/newsealrequest.png">
                                </div>
                                <div class="listtopBtn_text"><a href="AddSealRequest.aspx?returnurl=<%=this.ReturnUrl %>">New Work Flow</a></div>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
<%--    <% } %>--%>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="80px" class="order" orderby="ID">Request ID<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="Type">Type<span class="arrow"></span></th>
                <th width="140px" class="order" orderby="Status">Status<span class="arrow"></span></th>
                <th width="160px" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="*">Description<span class="arrow"></span></th>
                <th width="140px">Requested<span class="arrow"></span></th>
                <th width="140px" class="order  order-desc" default="true" orderby="RequestedDate">Requested Date<span class="arrow"></span></th>
                <th style="display: none;"></th>
            </tr>
        </thead>
        <tr runat="server" id="trNoRecords" visible="false">
            <th colspan="7" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptSealsRequest" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>R<%# Eval("ID")%></td>
                    <td>
                        <%#(int)Eval("Type")==0 ? "Seal" : "Work Flow" %>
                    </td>
                    <td>
                        <%#GetStautsHTML(Eval("Status")) %>
                    </td>
                    <td>
                        <%#Eval("Title")%>
                    </td>
                    <td style="word-break:break-all">
                        <%# (string)Eval("Description")%>
                    </td>
                    <td>
                        <%#Eval(UserNameDisplayProp)%>
                    </td>
                    <td>
                        <%#Eval("RequestedDate","{0:MM/dd/yyyy}")%>
                    </td>
                    <td class="action" style="display: none;">
                        <a class="saveBtn1 mainbutton" href="EditSealRequest.aspx?ID=<%# Eval("ID")%>&returnurl=<%=this.ReturnUrl %>" style="display: none;"></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div class="pagebox">
        <webdiyer:AspNetPager ID="anpWaitting" runat="server">
        </webdiyer:AspNetPager>
    </div>
</asp:Content>

