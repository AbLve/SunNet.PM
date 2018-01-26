    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KPIPerformanceReport.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.KPI.KPIPerformanceReport" %>
    
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Content/styles/public.css" rel="stylesheet" />
    <link href="/Content/styles/forms.css" rel="stylesheet" /> 
</head>
<body>
   
   <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
            <thead>
            <tr>
                
                <th width="*">Name<span class="arrow"></span></th>
                <th width="*">Self Grade<span class="arrow"></span></th>
                <th width="100px">PL Grade<span class="arrow"></span></th>
                <th width="100px">PL Comment<span class="arrow"></span></th>
                <th width="100px">Status<span class="arrow"></span></th>
                
            </tr>
          </thead>  
            <asp:Repeater ID="Repeater1" runat="server" DataSourceID="KPICategory">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>
                        <%#Eval("Name")%>
                    </td>
                    <td>
                        <%#Eval("SelfGrade")%>
                    </td>   
                    <td>
                        <%#Eval("PLGrade")%>
                    </td>
                    <td>
                        <%#Eval("PLComment")%>
                    </td>
                    <td>
                        <%#Eval("Status")%>
                    </td>   
                   
                </tr>   
                </ItemTemplate>
            </asp:Repeater>
            
            
            <asp:SqlDataSource ID="KPICategory" runat="server" ConnectionString="<%$ ConnectionStrings:KPICategory %>" SelectCommand="SELECT * FROM [KPICategory]"></asp:SqlDataSource>
            
            
        </table>
    </div>  
   </form>
</body>
</html> 