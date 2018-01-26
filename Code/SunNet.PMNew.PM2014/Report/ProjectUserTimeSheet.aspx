<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Report/Report.Master"
    CodeBehind="ProjectUserTimeSheet.aspx.cs" Inherits="SunNet.PMNew.PM2014.Report.ProjectUserTimeSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/My97DatePicker/PM_WdatePicke.js" type="text/javascript"> </script>
    <style type="text/css">
        .customWidth {
            width: 100%;
            background-color: #EFF5FB;
            min-height: 700px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    User:
    <asp:Literal runat="server" ID="litUserName"></asp:Literal>
    <span style="width: 100px">&nbsp;</span>
    Total Hours:
    <asp:Literal runat="server" ID="litTotalhours"></asp:Literal> 

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <asp:Repeater ID="rptDateView" runat="server" OnItemDataBound="rptDateView_ItemDataBound">
        <HeaderTemplate>
          
        </HeaderTemplate>
        <ItemTemplate>
               <div class="timesheetbox3">
          <table border="0" width="100%" cellpadding="0" cellspacing="0"  >
            <tr>
                <td class="timesheetTitle1">
                        Timesheet Date: <%#Eval("SheetDate","{0:MM/dd/yyyy}")%>
                        <asp:HiddenField runat="server" ID="hiddenDate" Value='<%#Eval("SheetDate","{0:MM/dd/yyyy}")%>'/>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Repeater runat="server" ID="subDetail">
                        <HeaderTemplate>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="table-advance tabletimesheet" >
                                <thead>
                                    <tr>
                                        
                                        <th width="15%" >Project<span class="arrow"></span></th>
                                        <th width="15%" >Title<span class="arrow"></span></th>
                                        <th width="10%" >Ticket ID<span class="arrow"></span></th>
                                        <th width="30%" >Work detail<span class="arrow"></span></th>
                                        <th width="10%" >Hours<span class="arrow"></span></th>
                                    
                                        <th width="10%" >Submitted<span class="arrow"></span></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <!-- collapsed expanded -->
                            <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                                <td>
                                    <%#Eval("Project")%>
                                </td>
                                <td>
                                    <%#Eval("Title")%>
                                </td>
                                <td>
                                    <%#Eval("Code")%>
                                </td>
                                <td>
                                    <%#Eval("WorkDetail")%>
                                </td>
                                   <td>
                                    <%#Eval("Hours")%>
                                </td>
                               
                                   <td>
                                    <%#Eval("SubmittedText")%>
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
        </ItemTemplate>
        <FooterTemplate>
           
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
     <div style="width: 100%; text-align: center">
                <input name="button2" tabindex="10" id="btnCancel" type="button" class="redirectback backBtn mainbutton" value="Back">
    </div>
</asp:Content>
