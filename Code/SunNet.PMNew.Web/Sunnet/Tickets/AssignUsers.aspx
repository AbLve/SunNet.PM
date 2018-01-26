<%@ Page Title="Assign Users to Ticket" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="AssignUsers.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.AssignUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .customWidth
        {
            width: 470px;
        }
        .opendivBox1
        {
            width: 518px;
            height: 300px;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function() {
            $("#btnSave").click(function() {
                var checkboxList = "";
                $("#tbAssignUser input[type=checkbox]:checked").each(function() {
                    var _thistd = $(this);
                    checkboxList += _thistd.attr("id") + ",";
                });
                var tid = getUrlParam('tid');
                //validate
                checkboxList = $.trim(checkboxList);
                if (checkboxList.length < 1) {
                    ShowMessage("Please Select CheckBox!", 0, false, false);
                    return false;
                }
                $("#btnSave").hide();
                $.ajax({
                    type: "post",
                    url: "/Do/DoAssignUserHandler.ashx?r=" + Math.random(),
                    data: {
                        checkboxList: checkboxList,
                        tid: tid,
                        type: $("#<%=ddlRole.ClientID %>").val()
                    },
                    success: function(result) {
                        RefreshParentWindowAfterClose();
                        if (result == "User Assign Successful!") {
                            $("#btnSave").show();
                        }
                        ShowMessage(result, 0, true, false);
                    }
                });
            });

            //clear
            $("#btnClear").click(function() {
                $("#tbAssignUser input[type=checkbox]:checked").each(function() {
                    var _thistd = $(this);
                    _thistd.attr("checked", false);
                });
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="opendivTopone_left">
        Assign Users to Ticket</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox" style="height: 300px;">
        <div class="owlistContainer">
            <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
                <tr>
                    <td width="59">
                        User Type
                    </td>
                    <td>
                        <asp:DropDownList AutoPostBack="true" ID="ddlRole" Width="120px" runat="server" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                            <asp:ListItem Value="-1">Please select...</asp:ListItem>
                            <asp:ListItem Value="2">Dev</asp:ListItem>
                            <asp:ListItem Value="3">QA</asp:ListItem>
                            <asp:ListItem Value="4">Other</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table width="98%" border="0" id="tbAssignUser" align="center" cellpadding="0" cellspacing="0"
                class="owlistone">
                <tr class="owlistTitle">
                    <th>
                        &nbsp;
                    </th>
                    <th>
                        User Name
                    </th>
                    <th style="width: 70px;">
                        User Role
                    </th>
                </tr>
                <tr runat="server" id="trNoTickets" visible="false">
                    <th colspan="3" style="color: Red;">
                        &nbsp; No records
                    </th>
                </tr>
                <asp:Repeater ID="rptAssignUser" runat="server">
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo" %>'>
                            <td>
                                <%# ShowIsExitsInTicketUser(Convert.ToInt32(Eval("UserID")))%>
                            </td>
                            <td>
                                <%# Eval("LastName")%>,
                                <%# Eval("FirstName") %>
                            </td>
                            <td style="text-align: center;">
                                <%# (SunNet.PMNew.Entity.UserModel.RolesEnum)((int)Eval("RoleID"))%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="btnBoxone">
        <input id="btnSave" type="button" value="Save" class="btnone" />
        <input id="btnClear" type="button" value="Clear" class="btnone" />
    </div>
</asp:Content>
