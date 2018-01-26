<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KPICategories.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.KPI.KPICategories" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <link href="/Content/styles/bootstrap_1.css" rel="stylesheet" />
    <link href="/Content/styles/public.css" rel="stylesheet" />
    <link href="/Content/styles/forms.css" rel="stylesheet" />
    
    
    
</head>
<body>
    <div class="topbtnbox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <span>
                        <ul class="listtopBtn" href="/Admin/KPI/AddCategory.aspx" data-target="#modalsmall" data-toggle="modal">
                            <li>
                                <div class="listtopBtn_icon">
                                    <img src="/Images/icons/add1.png">
                                </div>
                                <a class="listtopBtn_text" href="AddCategory.aspx">AddCategory</a>
                            </li>
                        </ul>
                    </span>
                </td>
            </tr>
        </table>
    </div>
    <form id="form1" runat="server">
    <div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
       <thead>
            <tr>
                <th width="100px">ID<span class="arrow"></span></th>
                <th width="*">Name<span class="arrow"></span></th>
                <th width="200px">Status<span class="arrow"></span></th>
            </tr>     
        </thead>
        
        <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1">
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>
                        <%#Eval("ID")%>
                    </td>
                    <td>
                        <%#Eval("Name")%>
                    </td>   
                    <td>x
                        <%#Eval("Status")%>
                    </td>
                    <td class="aligncenter action">
                        <a class="listtopBtn_text" style="text-decoration: none; display: none;" href="AddCategory.aspx?id=<%# Eval("ID") %>" data-target="#modalsmall" data-toggle="modal">Add</a>

                        <a href="AddCategory.aspx?id=<%#Eval("ID") %>&returnurl=<%=this.ReturnUrl %>" title="PerformanceReport">Add</a>
                    </td>
                </tr>   
            </ItemTemplate>     
        </asp:Repeater>
                    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KPICategoriesConnection %>" SelectCommand="SELECT * FROM [KPICategory]"></asp:SqlDataSource>
        
    </table>
    </div>
    </form>
    </body>
</html> 