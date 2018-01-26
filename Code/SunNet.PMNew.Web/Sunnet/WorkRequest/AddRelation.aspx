<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddRelation.aspx.cs" MasterPageFile="~/Sunnet/InputPop.Master"
Inherits="SunNet.PMNew.Web.Sunnet.WorkRequest.AddRelation" %>


<%@ Register Src="../../UserControls/AddWorkRequestRelation.ascx" TagName="AddRelationTickets"
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="owTopone_left1">
        Add Associated Tickets</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox" >
        <uc1:AddRelationTickets ID="AddRelationTickets1" runat="server" />
    </div>
    <div class="btnBoxone">
        <input id="btnSave" type="button" value="Save" onclick="SaveRelationTickets(); return false;"
            class="btnone" />
        <input id="btnClear" type="button" value="Clear" onclick="clearCheckBox(); return false;"
            class="btnone" />
    </div>

    <script type="text/javascript">

        function SaveRelationTickets() {
            var checkboxList = "";

            $("#addRelationTickets input[type=checkbox]:checked").each(function() {
                var _thistd = $(this);
                checkboxList += _thistd.attr("id") + ",";
            });
            var wid = getUrlParam('wid');
            //validate
            checkboxList = $.trim(checkboxList);
            if (checkboxList.length < 1) {
                ShowMessage("Please Select CheckBox!", 0, false, false);
                return false;
            }

            $.ajax({

                type: "post",

                url: "/Do/DoAddRelationWorkRequest.ashx?r=" + Math.random(),

                data: {
                    checkboxList: checkboxList,
                    wid: wid
                },
                success: function(result) {
                    ShowMessage(result, 0, true, true);
                }
            });

        };


        //clear
        function clearCheckBox() {
            $("#addRelationTickets input[type=checkbox]:checked").each(function() {
                var _thistd = $(this);
                _thistd.attr("checked", false);
            });
        }
    </script>

</asp:Content>
