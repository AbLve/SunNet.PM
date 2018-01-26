<%@ Page Title="Add Associated Tickets for Requirement Changes" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="AddRelation.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.AddRelation" %>

<%@ Register Src="../../UserControls/AddRelationTickets.ascx" TagName="AddRelationTickets"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .customWidth
        {
            width: 600px;
            height: 550px;
        }
        .owmainBox
        {
            width: 578px;
            height: 420px;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function() {

            $("#btnSave").click(function() {

                var checkboxList = "";

                $("#addRelationTickets input[type=checkbox]:checked").each(function() {
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

                $.ajax({

                    type: "post",

                    url: "/Do/DoAddRelationTickets.ashx?r=" + Math.random(),

                    data: {
                        checkboxList: checkboxList,
                        tid: tid
                    },
                    success: function(result) {
                        ShowMessage(result, 0, true, true);
                    }
                });

            });

            //clear
            $("#btnClear").click(function() {

                $("#addRelationTickets input[type=checkbox]:checked").each(function() {
                    var _thistd = $(this);
                    _thistd.attr("checked", false);
                });
            });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="owTopone_left1">
        Add Associated Tickets for Requirement Change</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <uc1:AddRelationTickets ID="AddRelationTickets1" runat="server" />
    </div>
    <div class="btnBoxone">
        <input id="btnSave" type="button" value="Save" class="btnone" />
        <input id="btnClear" type="button" value="Clear" class="btnone" />
    </div>
</asp:Content>
