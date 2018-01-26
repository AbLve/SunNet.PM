<%@ Page Title="" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="WaitingProcessSeal.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Seals.WaitingProcessSeal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                <td width="125px">
                    <asp:DropDownList ID="ddlType" runat="server" queryparam="type" CssClass="select205" Width="120" onchange="OnDdlTypeChange(this)">
                        <asp:ListItem Value="-1">All</asp:ListItem>
                        <asp:ListItem Value="0">Seal</asp:ListItem>
                        <asp:ListItem Value="1">Work Flow</asp:ListItem>
                    </asp:DropDownList>
                </td>

                <td width="30px" class="sealTextCls" style="display:none">Seal:
                </td>
                <td width="125px" class="sealDdlCls" style="display:none">
                    <asp:DropDownList ID="ddlSeal" runat="server" queryparam="seal" CssClass="select205" Width="120" DataValueField="ID"
                        DataTextField="SealName">
                    </asp:DropDownList>
                </td>

                <td width="80px" align="right">Start Date:
                </td>
                <td width="125px">
                    <asp:TextBox ID="txtStartDate" queryparam="start" onclick="WdatePicker({isShowClear:false});"
                        CssClass="input200 inputdate" Style="width: 120px;" runat="server"></asp:TextBox>
                </td>

                <td width="80" align="right">End Date:
                </td>
                <td width="125px">
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
    <% if (UserInfo.Office == "CN" && UserInfo.Role == SunNet.PMNew.Entity.UserModel.RolesEnum.PM)
       { %>
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
                                <div class="listtopBtn_text"><a href="AddSealRequest.aspx?returnurl=<%=this.ReturnUrl %>">New Seal Request</a></div>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <% } %>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="100px" class="order" orderby="ID">Request ID<span class="arrow"></span></th>
                <th width="80px" class="order" orderby="Type">Type<span class="arrow"></span></th>
                <th width="140px" class="order" orderby="Status">Status<span class="arrow"></span></th>
                <th width="200px" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th width="*">Description<span class="arrow"></span></th>
                <th width="140px">Requested<span class="arrow"></span></th>
                <th width="140px" class="order  order-desc" default="true" orderby="RequestedDate">Requested Date<span class="arrow"></span></th>
                <th style="display: none;"></th>
            </tr>
        </thead>
        <tr runat="server" id="trNoRecords" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptSealsRequest" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>R<%# Eval("ID")%>
                    </td>
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