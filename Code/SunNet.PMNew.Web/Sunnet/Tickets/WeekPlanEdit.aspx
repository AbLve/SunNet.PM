<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="WeekPlanEdit.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.WeekPlanEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        select, textarea {
            margin-left: 5px;
            vertical-align: middle;
        }

        label {
            margin-left: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <label id="lblTitle" runat="server">
    </label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <table border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th>Week:
                </th>
                <td>
                    <label id="lblStart" runat="server">
                    </label>
                    ------
                    <label id="lblEnd" runat="server">
                    </label>
                    <asp:HiddenField ID="hfID" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <label id="lblAddPlan" runat="server">
                    </label>
                </td>
            </tr>
            <% if (!IsEdit)
               {%>
            <tr>
                <th>Import:
                </th>
                <td>
                    <input type="button" class="btnone" value=" Import " id="btnImport" onclick="showImport(0);" />
                    <table id="tImport" style="display: none;">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlImport" runat="server">
                                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnOK" Text=" Confirm " CssClass="btnone" runat="server" OnClientClick="return CheckImport()"
                                    OnClick="btnOK_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <input type="button" class="btnone" value=" Cancel " id="btnCancel" onclick="showImport(1);" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <% } %>
            <tr>
                <th>Sunday:
                </th>
                <td>
                    <asp:TextBox ID="txtSunday" runat="server" TextMode="MultiLine" CssClass="input630"
                        Rows="6"></asp:TextBox>
                    <label>Estimate: </label>
                    <asp:DropDownList ID="ddlSundayEst" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Monday:
                </th>
                <td>
                    <asp:TextBox ID="txtMonday" runat="server" TextMode="MultiLine" CssClass="input630"
                        Rows="6"></asp:TextBox>
                    <label>Estimate: </label>
                    <asp:DropDownList ID="ddlMondayEst" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Tuesday:
                </th>
                <td>
                    <asp:TextBox ID="txtTuesday" runat="server" TextMode="MultiLine" CssClass="input630"
                        Rows="6"></asp:TextBox>
                    <label>Estimate: </label>
                    <asp:DropDownList ID="ddlTuesdayEst" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Wednesday:
                </th>
                <td>
                    <asp:TextBox ID="txtWednesday" runat="server" TextMode="MultiLine" CssClass="input630"
                        Rows="6"></asp:TextBox>
                    <label>Estimate: </label>
                    <asp:DropDownList ID="ddlWednesdayEst" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Thursday:
                </th>
                <td>
                    <asp:TextBox ID="txtThursday" runat="server" TextMode="MultiLine" CssClass="input630"
                        Rows="6"></asp:TextBox>
                    <label>Estimate: </label>
                    <asp:DropDownList ID="ddlThursdayEst" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Friday:
                </th>
                <td>
                    <asp:TextBox ID="txtFriday" runat="server" TextMode="MultiLine" CssClass="input630"
                        Rows="6"></asp:TextBox>
                    <label>Estimate: </label>
                    <asp:DropDownList ID="ddlFridayEst" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Saturday:
                </th>
                <td>
                    <asp:TextBox ID="txtSaturday" runat="server" TextMode="MultiLine" CssClass="input630"
                        Rows="6"></asp:TextBox>
                    <label>Estimate: </label>
                    <asp:DropDownList ID="ddlSaturdayEst" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div class="btnBoxone">
        <asp:Button ID="btnSave" Text=" Save " CssClass="btnone" runat="server" OnClientClick="return Check()"
            OnClick="btnSave_Click" />
    </div>

    <script type="text/javascript">
        function Check() {
            if ($("#<%= txtSunday.ClientID %>").val() == "" && $("#<%= txtMonday.ClientID %>").val() == "" && $("#<%= txtTuesday.ClientID %>").val() == ""
            && $("#<%= txtWednesday.ClientID %>").val() == "" && $("#<%= txtThursday.ClientID %>").val() == "" && $("#<%= txtFriday.ClientID %>").val() == ""
            && $("#<%= txtSaturday.ClientID %>").val() == "") {
                alert("Please entity week plan.");
                return false;
            }
            $("#" + "<%=btnSave.ClientID%>").css("display", "none");
            return true;
        }

        function showImport(v) {
            if (v == 0) {
                $("#tImport").css("display", "");
                $("#btnImport").css("display", "none");
            }
            else if (v == 1) {
                $("#tImport").css("display", "none");
                $("#btnImport").css("display", "");
            }
        }

        function CheckImport() {
            if ($("#<% =ddlImport.ClientID %>").val() == 0) {
                alert("Please select plan");
                return false;
            }
        }
    </script>

</asp:Content>
