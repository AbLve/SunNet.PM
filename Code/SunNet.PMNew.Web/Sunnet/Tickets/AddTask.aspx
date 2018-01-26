<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="AddTask.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.AddTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        //get url para
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        function baseValidate(title, descr) {
            if (title.length < 1) {
                ShowMessage("Please Input title.", 0, false, false);
                $("#<%=txtTitle.ClientID%>").focus();
                return false;
            } else if (descr.length < 1) {
                ShowMessage("Please Input description.", 0, false, false);
                $("#<%=txtDesc.ClientID%>").focus();
                return false;
            }
            return true;
        }
        //declare
        var tid;
        var pid;
        var title;
        var descr;
        $(document).ready(function() {
            //save
            $("#<%=btnSave.ClientID%>").click(function() {
                //set value
                tid = getUrlParam('tid');
                title = $("#<%=txtTitle.ClientID%>").val();
                descr = $("#<%=txtDesc.ClientID%>").val();


                //validate
                title = $.trim(title);
                descr = $.trim(descr);
                if (baseValidate(title, descr) == false) {
                    return false;
                }
                $.ajax({

                    type: "post",

                    url: "/Do/DoAddTaskHandler.ashx?r=" + Math.random(),

                    data: {
                        tid: tid,
                        title: title,
                        descr: descr
                    },
                    success: function(result) {
                        ShowMessage(result, 0, true, true);
                    }
                });
            });

            //clear
            $("#<%=btnClear.ClientID%>").click(function() {

                $("#<%=txtTitle.ClientID%>").val("");
                $("#<%=txtDesc.ClientID%>").val("");
            });

        });
    </script>

    <style type="text/css">
        .customWidth
        {
            width: 600px;
        }
        .owmainBox
        {
            padding: 9px;
            width: 580px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Add New Task
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <table border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th>
                    Title:<span class="redstar">*</span>
                </th>
                <td>
                    <input id="txtTitle" runat="server" type="text" class="input450" />
                </td>
            </tr>
            <tr id="IsComplete" runat="server" visible="false">
                <th valign="top">
                    IsComplete:
                </th>
                <td>
                    <input id="IsCompleteCK" runat="server" type="checkbox" />
                </td>
            </tr>
            <tr id="Complete" runat="server" visible="false">
                <th>
                    Complete Date:
                </th>
                <td>
                    <asp:TextBox ID="txtComplete" Validation="true" length="8-20" RegType="date" CssClass="input180"
                        runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Description:<span class="redstar">*</span>
                </th>
                <td>
                    <textarea id="txtDesc" runat="server" cols="15" class="input450" rows="5"></textarea>
                </td>
            </tr>
        </table>
    </div>
    <div class="btnBoxone">
        <input id="btnSave" type="button" runat="server" action="save" value="Submit" visible="false"
            class="btnone" />
        <input id="btnClear" type="button" runat="server" value="Clear" class="btnone" visible="false" />
    </div>
</asp:Content>
