<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KPIObjectives.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.KPI.KPIObjectives" %>

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
                <th width="*">Goal<span class="arrow"></span></th>
                <th width="*">Description<span class="arrow"></span></th>
                <th width="100px">Percentage<span class="arrow"></span></th>
                <th width="50px">Status<span class="arrow"></span></th>
                <th width="100px">CategoryID<span class="arrow"></span></th>
                <th width="200px">Role<span class="arrow"></span></th>
            </tr>
          </thead>  
            <asp:Repeater ID="Repeater1" runat="server" DataSourceID="KPIObjView">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>
                        <%#Eval("Goal")%>
                    </td>
                    <td>
                        <%#Eval("Description")%>
                    </td>   
                    <td>
                        <%#Eval("Percentage")%>
                    </td>
                    <td>
                        <%#Eval("Status")%>
                    </td>
                    <td>
                        <%#Eval("Category ID")%>
                    </td>   
                    <td>
                        <%#Eval("Role")%>
                    </td>
                    </tr>   
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="KPIObjView" runat="server" ConnectionString="<%$ ConnectionStrings:KPIObjView %>" SelectCommand="SELECT * FROM [KPIOBjectivesView]"></asp:SqlDataSource>
        </table>
    </div>
    </form>
</body>
</html>