<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Seals.aspx.cs"
    MasterPageFile="~/Admin/admin.master"
     Inherits="SunNet.PMNew.PM2014.Admin.Seals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <div class="topbtnbox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr> 
                <td> 
                     <ul class="listtopBtn" href="/Admin/NewSeal.aspx" data-target="#modalsmall" data-toggle="modal">
                            <li>
                                <div class="listtopBtn_icon">
                                    <img src="/images/icons/newseal.png" />
                                </div>
                                <div class="listtopBtn_text" >New Seal</div>
                            </li>
                        </ul> 
                </td>
            </tr>
        </table>
    </div>
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                
                <th width="180" >Seal Name<span class="arrow"></span></th>
                <th width="120" >Owner<span class="arrow"></span></th>
                <th width="120" >Approver<span class="arrow"></span></th>
                <th width="*" >Description<span class="arrow"></span></th>
                <th width="80" >Status<span class="arrow"></span></th> 
                <th width="80" style="display: none" class="aligncenter">Action</th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptSealList" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                    
                    <td>
                        <%# Eval("SealName").ToString()%>
                    </td>
                    <td>
                        <%#Eval("OwnerFirstName").ToString()%>
                    </td>
                    <td>
                        <%#Eval("ApproverFirstName").ToString()%>
                    </td>
                    <td>
                        <%# Eval("Description").ToString()%>
                    </td>
                    <td>
                        <%# Eval("Status")%>
                    </td>
                    <td class="action aligncenter" style="display: none">
                        <a href='SealDetail.aspx?id=<%# Eval("ID")%>' data-target="#modalsmall" data-toggle="modal">
                            <img src="/Images/icons/edit.png" title="View"></a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    
</asp:Content>