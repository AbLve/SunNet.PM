<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/OA/OA.master"
    CodeBehind="PtoUserTime.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Pto.PtoUserTime" %>
<%@ Register Src="~/UserControls/OA/YearsUserCon.ascx" TagName="Yearsddl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <style type="text/css">
        .customWidth {
            width: 100%;
            background-color: #EFF5FB;
            min-height: 700px;
        }
    </style>
    <script>
        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        function yearchange(e) {
            var firstyear = GetQueryString('year');
            location.href = location.href.replace(firstyear,$(e).val());
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    User:
    <asp:Literal runat="server" ID="litUserName"></asp:Literal>
    <span style="width: 100px">&nbsp;</span>
    Year:
    <uc1:Yearsddl ID="Yearsddl1" runat="server" />
    <span style="width: 100px">&nbsp;</span>
    Total Hours:
    <asp:Literal runat="server" ID="litTotalhours"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <div class="timesheetbox3">
        <table border="0" width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Repeater runat="server" ID="ptoDetail">
                        <HeaderTemplate>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="table-advance tabletimesheet">
                                <thead>
                                    <tr>
                                        <th width="15%">Project<span class="arrow"></span></th>
                                        <th width="20%">Title<span class="arrow"></span></th>
                                        <th width="30%">Detail<span class="arrow"></span></th>
                                        <th width="15%">Begin Time<span class="arrow"></span></th>
                                        <th width="15%">End Time<span class="arrow"></span></th>
                                        <th width="15%">Hours<span class="arrow"></span></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <!-- collapsed expanded -->
                            <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                                <td>
                                    <%#Eval("Title")%>
                                </td>
                                <td>
                                    <%#Eval("Name")%>
                                </td>
                                <td>
                                    <%#Eval("Details")%>
                                </td>
                                <td>
                                    <%#Eval("FromDay")%>
                                </td>
                                <td>
                                    <%#Eval("ToDay")%>
                                </td>
                                <td>
                                    <%#Eval("Hours")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div style="width: 100%; text-align: center">
        <input name="button2" tabindex="10" id="btnCancel" type="button" class="redirectback backBtn mainbutton" value="Back">
    </div>
</asp:Content>
