<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="SealsList.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Admin.SealsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainactionBox">
        <span><a href="#" onclick="OpenListPageDialog('/Sunnet/Admin/SealEdit.aspx','Add Seal', 430, 380,'<%=btnRefresh.ClientID%>');">
            <img src="/icons/14.gif" border="0" align="absmiddle" alt="new/add" />
            Add Seal</a></span>
    </div>
    <div class="mainrightBoxtwo">
        <table border="0" cellpadding="0" cellspacing="0" class="listtwo" width="100%">
            <tr class="listsubTitle">
                <th width="180px">Seal Name
                </th>
                <th width="120px">Owner
                </th>
                <th width="120px">Approver
                </th>
                <th>Description
                </th>
                <th width="80px">Status
                </th>
            </tr>
            <asp:Repeater ID="rptSeals" runat="server">
                <ItemTemplate>
                    <tr href="/Sunnet/Admin/SealEdit.aspx?id=<%#Eval("ID") %>" opentype="popwindowNew" dialogwidth="430"
                        dialogheight="380" dialogtitle="Edit Seal" freshbutton="<%#btnRefresh.ClientID %>"
                        class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                        <td>
                            <%#Eval("SealName")%>
                        </td>
                        <td>
                            <%#Eval("OwnerFirstName")%>
                        </td>
                        <td>
                            <%#Eval("ApproverFirstName")%>
                        </td>
                        <td>
                            <%#Eval("Description")%>
                        </td>
                        <td>
                            <%#Eval("Status")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <asp:Button ID="btnRefresh" runat="server" CssClass="btnRefreshHidden" />
    </div>
</asp:Content>
