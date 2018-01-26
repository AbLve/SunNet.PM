<%@ Page Title="" Language="C#" MasterPageFile="~/Invoice/Invoice.master" AutoEventWireup="true" CodeBehind="TSDetail.aspx.cs" Inherits="SunNet.PMNew.PM2014.Invoice.TM.TSDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <h3>View timesheet and Add Invoice</h3>
    <asp:Label ID="Label1" runat="server" Text="Label" Style="margin-left: 85%"><b>Total Hours: </b></asp:Label><u>
        <asp:Label ID="lblTotalHours" runat="server" Text="Label"></asp:Label>
    </u>
    <div class="sepline2"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="dataSection" runat="server">
    <asp:HiddenField ID="hidProject" runat="server" />
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th width="1%">
                    <input id="chkAll" type="checkbox" name="chkAll" checked="checked" /></th>
                <th width="14%" class="order" orderby="SheetDate">Sheet Date<span class="arrow"></span></th>
                <th width="14%" class="order" orderby="CompanyName">Company<span class="arrow"></span></th>
                <th width="14%" class="order" orderby="ProjectTitle">Project Name<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="TicketID">Ticket ID<span class="arrow"></span></th>
                <th width="14%" class="order" orderby="TicketTitle">Ticket Title<span class="arrow"></span></th>
                <th width="14%" class="order" orderby="FirstName">User Name<span class="arrow"></span></th>
                <th width="10%" class="order" orderby="Hours">Hours<span class="arrow"></span></th>
            </tr>
        </thead>
        <tr runat="server" id="trNoTickets" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No record found
            </th>
        </tr>
        <asp:Repeater ID="rptTicketsList" runat="server">
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>' onclick="clickCheckBox(this)">
                    <td>
                        <input id="<%# Eval("ID")%>" name="chk" onclick="clickCheckBox(this)" type="checkbox" value="<%# Eval("ID")%>" checked="checked" />
                    </td>
                    <td><%# Eval("SheetDate","{0:MM/dd/yyyy}")%></td>
                    <td><%# Eval("CompanyName")%></td>
                    <td><%# Eval("ProjectTitle")%></td>
                    <td><%# Eval("TicketID")%></td>
                    <td><%# Eval("TicketTitle")%></td>
                    <td><%# Eval("UserName")%> </td>
                    <td><%# Eval("hours")%> </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pagerSection" runat="server">
    <div style="text-align: center; padding-top: 15px;">
        <input type="button" value="Continue to Add Invoice #" onclick="saveClick()" class="saveBtn1 mainbutton" style="margin-right: 10px;" />
        <a id="newInvoice" href="#" data-target="#modalsmall" data-toggle="modal"></a>
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="cancelBtn1 mainbutton" OnClick="btnCancel_Click" />
    </div>

    <script type="text/javascript">
        $(function () {
            $("#form1").prop("id", "form2");
            var chk = document.getElementById('chkAll');
            $("#chkAll").change(function () {
                var chks = document.getElementsByName("chk");
                if (chk.checked) {
                    for (var i = 0; i < chks.length; i++) {
                        chks[i].checked = true;
                    }
                } else {
                    for (var i = 0; i < chks.length; i++) {
                        chks[i].checked = false;
                    }
                }
            })
        })

        function saveClick() {
            var chks = document.getElementsByName("chk");
            var timeSheetIDs = "";
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].checked == true)
                    timeSheetIDs = timeSheetIDs + chks[i].value + ",";
            }
            if (timeSheetIDs == "") {
                alert("Please select at least one")
                return false;
            }
            timeSheetIDs = timeSheetIDs.substr(0, timeSheetIDs.length - 1);
            setCookie('timeTsheetIDs', timeSheetIDs);
            $("#newInvoice").attr('href', 'New.aspx?projectid=' + $("#<%=hidProject.ClientID%>").val());
            $("#newInvoice").click();
        }
        function setCookie(name, value) {
            var Days = 30;
            var exp = new Date();
            exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
            document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
        }
        function clickCheckBox(chk) {
            var chkAll = document.getElementById('chkAll');
            var ischecked = false;
            if (chk.tagName.toLowerCase() == "input") {
                ischecked = $(chk).prop("checked");
                if (ischecked == true) {
                    $(chk).prop("checked", false)
                    chkAll.checked = false;
                }
                else
                    $(chk).prop("checked", true);
            }
            else {
                ischecked = $(chk).find("input").prop("checked");
                if (ischecked == true) {
                    $(chk).find("input").prop("checked", false)
                    chkAll.checked = false;
                }
                else
                    $(chk).find("input").prop("checked", true);
            }

        }
    </script>
</asp:Content>
