<%@ Page Title="Edit Priority" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="EditPriority.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.EditPriority" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="/Styles/smoothness/jquery-ui-1.9.0.custom.min.css" rel="stylesheet" type="text/css" />--%>

    <script type="text/javascript">
        var oldForm;
        $(function() {
            oldForm = $('#aspnetForm').serialize();
        });
        $(window).on('beforeunload', function() {
            if ($('#aspnetForm').serialize() != oldForm && $('#closePopWindow').attr('clicked') == '1') {
                $('#closePopWindow').attr('clicked', '0')
                return '';
            }
            $('#aspnetForm').attr('clicked', '0')
        });
    </script>

    <link href="/Styles/smoothness/jquery-ui-1.9.0.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        ul.ui-sortable
        {
            list-style: none;
            padding: 0;
            margin: 0 0 0 0;
            font-family: Arial;
            font-size: 11px;
        }
        ul.ui-sortable li.title
        {
            background-image: url("/images/listsub_bg.jpg");
        }
        ul.ui-sortable li.ui-state-odd
        {
            padding: 4px;
            margin: 2px;
            border: 1px solid #81BAE8;
            background-color: #fff;
            overflow: auto;
        }
        ul.ui-sortable li.ui-state-double
        {
            padding: 4px;
            margin: 2px;
            border: 1px solid #81BAE8;
            background-color: #e0ecf9;
            overflow: auto;
        }
        ul.ui-sortable li.ui-state-odd:hover, ul.ui-sortable li.ui-state-double:hover
        {
            background-color: #b6d9ff;
        }
        ul.ui-sortable li.active
        {
            padding: 4px; /*height: 12px;*/
            background-color: #81bae8;
            overflow: auto;
        }
        ul li div
        {
            word-wrap: break-word;
            word-spacing: normal;
        }
        ul li div.first
        {
            width: 19%;
            float: left;
            padding-right: 10px;
        }
        ul li div.second
        {
            width: 52%;
            float: left;
            padding-right: 10px;
        }
        ul li div.third
        {
            width: 6%;
            float: left;
            padding-right: 10px;
        }
        ul li div.forth
        {
            width: 18%;
            float: left;
            padding-right: 10px;
        }
        ul li div.five
        {
            width: 1%;
            float: left;
            padding-right: 10px;
        }
        .ui-state-highlight
        {
            height: 20px;
        }
        .customWidth
        {
            width: 900px;
        }
    </style>

    <script type="text/javascript">
        var oldOrders="";
        var hidNewOrder;
        jQuery(function() {
            jQuery("#data").sortable({
                cursor: "move",
                placeholder: "ui-state-highlight",
                revert: true
            }).children().mousedown(function() {
                jQuery(this).toggleClass("active");
            }).mouseup(function() {
                jQuery(this).toggleClass("active");
            });

            hidNewOrder=jQuery(<%="'#"+hidNewOrder.ClientID+"'" %>);
            jQuery("#data>li").each(function(){
                oldOrders=oldOrders+jQuery(this).attr("ticket");
                oldOrders=oldOrders+",";
            });
        });
        
        function OnSubmit()
        {
            var newOrders="";
            jQuery("#data>li").each(function(){
                newOrders=newOrders+jQuery(this).attr("ticket");
                newOrders=newOrders+",";
            });
            if(oldOrders==newOrders)
            {
                ShowMessage("No changes have been applied.",0,false,false);
                return false;
            }
            else
            {
                hidNewOrder.val(newOrders);
                return true;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Edit Priority
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainrightBoxtwo owmainrightBoxtwoMinH">
        <div class="owmainactionBox">
            <div class="tickettop_left">
                Tickets Priority
            </div>
        </div>
        <table width="95%" border="0" align="center" cellpadding="5" cellspacing="0">
            <tr>
                <th width="10">
                    Projects:
                </th>
                <td width="220">
                    <asp:DropDownList ID="ddlProjects" CssClass="select205" runat="server">
                    </asp:DropDownList>
                    &nbsp
                </td>
                <td width="*">
                    &nbsp;
                    <asp:ImageButton ID="iBtnSearch" runat="server" align="absmiddle" ImageUrl="/images/search_btn.jpg"
                        OnClick="iBtnSearch_Click" /><asp:HiddenField ID="hidNewOrder" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="color: Red;">
                    Tips: drag to reset priority,and you should click Submit Button if you want save
                    the new priority.
                </td>
            </tr>
        </table>
        <div class="btnBoxone">
            <asp:Button ID="btnSave2" CssClass="btnone" runat="server" Text="Save" OnClientClick="return OnSubmit();"
                OnClick="btnSave_Click" />
        </div>
        <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
            <thead>
                <tr class="owlistTitle">
                    <th width="20%">
                        Title
                    </th>
                    <th>
                        Description
                    </th>
                    <th width="6%">
                        Priority
                    </th>
                    <th width="21%">
                        Status
                    </th>
                </tr>
            </thead>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="3" style="color: Red;">
                    &nbsp; No records
                </th>
            </tr>
            <tr>
                <td colspan="4" valign="top">
                    <ul id="data" class="ui-sortable">
                        <asp:Repeater ID="rptTickets" runat="server">
                            <ItemTemplate>
                                <li class="<%#Container.ItemIndex%2==0?"ui-state-odd":"ui-state-double" %>" ticket="<%#Eval("ID") %>">
                                    <div class="first">
                                        [<%#Eval("TicketCode") %>] /
                                        <%#Eval("Title") %></div>
                                    <div class="second">
                                        <%#Eval("Description") %></div>
                                    <div class="third">
                                        <%#Eval("Priority")%></div>
                                    <div class="forth">
                                        <%#Eval("Status")%></div>
                                    </div>
                                    <div class="div">
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <div class="btnBoxone">
        <asp:Button ID="btnSave" CssClass="btnone" runat="server" Text="Save" OnClientClick="return OnSubmit();"
            OnClick="btnSave_Click" />
    </div>
</asp:Content>
