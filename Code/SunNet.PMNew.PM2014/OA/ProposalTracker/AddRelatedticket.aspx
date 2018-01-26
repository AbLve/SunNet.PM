<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="AddRelatedticket.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.ProposalTracker.AddRelatedticket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Add Associated Tickets
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <style>
        .inputw3{
            width:250px !important;;
        }
</style>
    <div class="searchItembox">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="50" align="right">Keyword:</td>
                <td>
                    <asp:TextBox ID="txtKeyword" runat="server" queryparam="keyword" CssClass="inputw3" placeholder="Enter Title, Ticket code"></asp:TextBox>

                </td>
                <td>
                    <input type="button" class="searchBtn" id="btnSearch" /></td>
            </tr>
        </table>
    </div>

    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance noclickbind" id="addRelationTickets">
        <thead>
            <tr>
                <th width="8px"></th>
                <th width="120px" class="order  order-desc" default="true" orderby="TicketID">Ticket Code<span class="arrow"></span></th>
                <th width="200px" class="order" orderby="Title">Title<span class="arrow"></span></th>
                <th>Description</th>
            </tr>
        </thead>
        <tbody>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="3" style="color: Red;">&nbsp; No record found.
                </th>
            </tr>
            <asp:Repeater ID="rptTickets" runat="server">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                        <td>
                            <input value='<%# Eval("TicketID")%>' type="checkbox" name="chkTicket" />
                        </td>
                        <td><%# Eval("TicketID") %></td>
                        <td><%# Eval("Title") %></td>
                        <td><%# Eval("Description") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <script type="text/javascript">
        jQuery(function () {
            jQuery('body').on('click', '#addRelationTickets tr td input:checkBox', function (event) {
                event.stopPropagation();
            });
            jQuery('body').on('click', '#addRelationTickets tr td', function (event) {
                var $checkBox = $(this).parent().find('input[type=checkBox]');

                if ($checkBox.prop('checked')) {
                    $checkBox.prop('checked', false);
                }
                else {
                    $checkBox.prop('checked', true);
                }
                event.stopPropagation();
            });
        });

        function clearCheckbox() {
            jQuery("input[name='chkTicket']").each(function () {
                jQuery(this).prop("checked", false);
            });
        }

        function saveTickets() {
            var checkboxList = "";

            jQuery("#addRelationTickets input[type=checkbox]:checked").each(function () {
                var _thischeckbox = jQuery(this);
                checkboxList += _thischeckbox.val() + ",";
            });

            //validate
            checkboxList = $.trim(checkboxList);

            if (checkboxList.length < 1) {
                jQuery.alert("danger", "Please Select CheckBox!",2);
                return false;
            }

            jQuery.ajax({
                type: "post",
                url: "/Do/DoAddRelationWorkRequest.ashx?r=" + Math.random(),
                data: {
                    checkboxList: checkboxList,
                    wid: '<% =QS("ID", 0) %>'
                },
                success: function (result) {
                    top.window.location.href = top.window.location.pathname + top.window.location.search;
                    ClosePopWindow();
                }
            });
        }

    </script>

        <div class="pagebox">
            <webdiyer:AspNetPager ID="anpWaitting" runat="server">
            </webdiyer:AspNetPager>
        </div>
    <div style="clear:both"></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <input name="Input22" type="button" class="saveBtn1 mainbutton" value="Save" onclick="saveTickets()"/>
    <span class="buttonBox1">
        <input name="Input3" type="button" class="cancelBtn1 mainbutton" value="Clear" onclick="clearCheckbox()" />
    </span>
</asp:Content>

