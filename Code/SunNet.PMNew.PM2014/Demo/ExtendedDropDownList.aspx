<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExtendedDropDownList.aspx.cs" Inherits="SunNet.PMNew.PM2014.Demo.ExtendedDropDownList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Expanded DropDownList Demo</title>
    <style type="text/css">
        optgroup {
            color: red;
        }
        optgroup.ACTIVE {
            color: green;
        }
        option {
            color: blue;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Demo 1:<sunnet:ExtendedDropdownList ID="ExtendedDropdownList1" DefaultMode="Form" runat="server" OptionGroupValues="Active" DataTextField="Text" DataValueField="Value">
            </sunnet:ExtendedDropdownList>
            selected:<asp:Literal ID="Literal1" runat="server"></asp:Literal>
            <br />
            Group by role:<sunnet:ExtendedDropdownList ID="ddlUsers"
                DataTextField="FirstAndLastName"
                DataValueField="UserID"
                DataGroupField="Role"
                DefaultMode="List" runat='server'>
            </sunnet:ExtendedDropdownList>
            selected:<asp:Literal ID="Literal2" runat="server"></asp:Literal>
            <br />
            Group by status:<sunnet:ExtendedDropdownList ID="ddlUsers2"
                DataTextField="FirstAndLastName"
                DataValueField="UserID"
                DataGroupField="Status"
                DefaultMode="Form"
                DefaultSelectText="User is required"
                EnableTheming="false"
                runat='server'>
            </sunnet:ExtendedDropdownList>
            selected:<asp:Literal ID="Literal3" runat="server"></asp:Literal>
            <br />
            Group by status(only one optgroup):<sunnet:ExtendedDropdownList
                DataTextField="FirstAndLastName"
                DataValueField="UserID"
                DataGroupField="Status"
                DefaultMode="List" ID="ddlUsers3" runat='server'>
            </sunnet:ExtendedDropdownList>
            selected:<asp:Literal ID="Literal4" runat="server"></asp:Literal>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
