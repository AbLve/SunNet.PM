<%@ Page Title="Estimation Time" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="TicketTsTime.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.TicketTsTime" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .customWidth
        {
            width: 850px;
        }
        .opendivBox1
        {
            width: 860px;
            height: 400px;
        }
        .bg2
        {
            background: #fff;
        }
        .bg1
        {
            background: #e0ecf9;
        }
        .bg3
        {
            background: #81bae8;
        }
    </style>

    <script type="text/javascript">
        var TableForm = jQuery("#esTbl");
        var JsonString = '';
        var ISModalPage = false;
        var FirstUpdate = false;
        var isPm = '<%=HdIsPm.Value %>';
        var HasRowCount = 0;

        var tid = getUrlParam('tid');
        $(document).ready(function() {
            FillDataTable();
            RefreshParentWindowAfterClose();
            $("#esTbl input[validate='true'] ").live("keydown", function(event) {
                var keyCode = event.which;
                if (keyCode == 46 || keyCode == 8 || keyCode == 9 || keyCode == 190 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105) || keyCode == 110) {
                    return true;
                } else { return false }
            }).live("blur", function() {

                var _this = jQuery(this);

                var _thisTr = _this.parent().parent().children().children().filter("input[name !='txtTotal']")
                                                                           .filter("input[name !='txtTime']")
                                                                           .filter("input[name !='txtRemark']");

                var totalValue = 0;
                _thisTr.each(
                    function() {
                        var _thisInput = jQuery(this);
                        if (_thisInput.val().length > 0) {
                            totalValue = totalValue + parseFloat(_thisInput.val());
                        }
                    }
                )

                _this.parent().parent().children().children().filter("input[name ='txtTotal']").val(totalValue);

                if (_this.val().length < 1) {
                    _this.val(0);
                }
            });

            SetRowBgColor();
        });
        function ClickRow(e) {
            SetRowBgColor();
            $(e).addClass("bg3");
            $(e).attr("selected", "1");
        }
        // this.style.backgroundColor = ['#e0ecf9', '#fff'][i % 2];
        function UpdateValue(e) {
            $("#esTbl tr img").remove();
            var setValueTrs = $("#esTbl tr");

            var firstRowHtml = setValueTrs.eq(0).html();
            var secondRowHtml = setValueTrs.eq(1).html();
            $("#esTbl>tbody").empty().append("<tr>" + firstRowHtml + "</tr>").append("<tr>" + secondRowHtml + "</tr>");
            HasRowCount = HasRowCount - setValueTrs.length + 2;

            var setValueTr = $("#esTbl tr")[1];
            setValueTr.children[0].children[0].value = $.trim(e.children[0].innerHTML);
            setValueTr.children[1].children[0].value = $.trim(e.children[1].innerHTML);
            setValueTr.children[2].children[0].value = $.trim(e.children[2].innerHTML);
            setValueTr.children[3].children[0].value = $.trim(e.children[3].innerHTML);
            setValueTr.children[4].children[0].value = $.trim(e.children[4].innerHTML);
            setValueTr.children[5].children[0].value = $.trim(e.children[5].innerHTML);
            setValueTr.children[6].children[0].value = $.trim(e.children[6].innerHTML);
            setValueTr.children[7].children[0].value = $.trim(e.children[7].innerHTML);

            $("#btnClear").css("display", "");
        }

        function SetRowBgColor() {
            $("#esTable tr").each(function() { $(this).removeAttr("selected"); $(this).removeClass("bg3"); })
        }

        function SaveHours() {

            var trtemplate = '{EsID:{EsID},TicketID:{TicketID},Week:"{Week}",QaAdjust:{QaAdjust},DevAdjust:{DevAdjust},GrapTime:{GrapTime},DocTime:{DocTime},TrainingTime:{TrainingTime},TotalTimes:{TotalTimes},EsByUserId:{EsByUserId},CreatedTime:"{CreatedTime}",Remark:"{Remark}",IsPM:{IsPM}}';

            var type = getUrlParam('uType');
            var EsId = 0;

            if ($("#esTable tr[selected='selected']")[0] != null || $("#esTable tr[selected='selected']")[0] != undefined) {
                FirstUpdate = true;
                EsId = $("#esTable tr[selected='selected']").attr('id');
            }

            var trInput = $('#esTbl tr input[validate="true"]');
            var checkResult = true;
            trInput.each(function() {
                var _currentInput = $(this);
                if (_currentInput.val().length < 1) {
                    ShowMessage("Please Input value!", 0, false, false);
                    _currentInput.focus();
                    checkResult = false;
                    return false;
                }
            });
            if (checkResult) {
                var tr = $('#esTbl tr');

                if (tr.length > 0) {

                    for (var i = 1; i < tr.length; i++) {

                        var thisHtml = trtemplate.replace("{TicketID}", tid)
                                      .replace("{EsID}", 0)
                                      .replace("{Week}", tr[i].children[0].children[0].value)
                                      .replace("{QaAdjust}", tr[i].children[1].children[0].value)
                                      .replace("{DevAdjust}", tr[i].children[2].children[0].value)
                                      .replace("{GrapTime}", tr[i].children[3].children[0].value)
                                      .replace("{DocTime}", tr[i].children[4].children[0].value)
                                      .replace("{TrainingTime}", tr[i].children[5].children[0].value)
                                      .replace("{TotalTimes}", tr[i].children[6].children[0].value)
                                      .replace("{EsByUserId}", '0')
                                      .replace("{CreatedTime}", '1753/01/01')
                                      .replace("{Remark}", tr[i].children[7].children[0].value)
                                      .replace("{IsPM}", isPm)
                        JsonString = JsonString + thisHtml + ",";
                    }
                    JsonString = "[" + JsonString.substring(0, JsonString.length - 1) + "]";
                }

                $.ajax({
                    type: "post",

                    url: "/Do/DoTicketEsTimeUpdateHandler.ashx?r=" + Math.random(),

                    data: {
                        tid: tid,
                        type: isPm,
                        JsonString: JsonString,
                        EsId: EsId,
                        FirstUpdate: FirstUpdate
                    },
                    success: function(result) {
                        if ($.trim(result) != $.trim('Update Es Time Fail!') && $.trim(result) != $.trim('Add Es Time Fail!')) {
                            FillDataTable();
                        }
                        ShowMessage(result, 0, false, false);
                        JsonString = '';
                        SetValueForWeek();
                        var setValueTrs = $("#esTbl tr");
                        var firstRowHtml = setValueTrs.eq(0).html();
                        var secondRowHtml = setValueTrs.eq(1).html();
                        $("#esTbl>tbody").empty().append("<tr>" + firstRowHtml + "</tr>").append("<tr>" + secondRowHtml + "</tr>");

                    }
                });
            }
        }
        function FillDataTable() {

            $.ajax({

                type: "get",

                url: "/Do/DoGetListEsStringHandler.ashx?su=" + Math.random(10000),

                data: {
                    tid: tid
                },
                success: function(result) {
                    $("#data").empty();
                    HasRowCount = parseInt(result.substring(0, 2));
                    $("#data").append(result.substring(2));
                    HasRowCount++;
                    ClearHours();
                }

            });
        }
        //clear
        function ClearHours() {
            //set value
            $('#esTbl input').val('');
            SetValueForWeek();
            $("#esTable tr").removeAttr("selected");
            $("#btnClear").css("display", "none");
        }
        function SetValueForWeek() {
            var setValueTrs = $("#esTbl tr")[1];
            setValueTrs.children[0].children[0].value = "Week" + HasRowCount;
            setValueTrs.children[8].innerHTML = "<img  src='/icons/37.gif' alt='add'  onclick = 'addEs(); return false;' />";
        }
    </script>

    <style type="text/css">
        .tabStyle td
        {
            text-align: center;
        }
        .tabStyle th
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="opendivTopone_left">
        Estimating Time
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HdIsPm" runat="server" />
    <div class="owmainBox">
        <div class="owlistContainer">
            <div id="data">
            </div>
            <div id="div" style="height: 20px;">
            </div>
            <table id="esTbl" width="90%" border="1" align="center" cellpadding="5" cellspacing="0"
                class="tabStyle">
                <tr class="owlistTitle">
                    <th style="width: 40px;">
                        Week
                    </th>
                    <th style="width: 40px;">
                        <%= HdIsPm.Value == "true" ? "PM-QA" : "QA"%>
                    </th>
                    <th style="width: 40px;">
                        <%= HdIsPm.Value == "true" ? "PM-DEV" : "Dev"%>
                    </th>
                    <th style="width: 60px;">
                        Grap Time
                    </th>
                    <th style="width: 60px;">
                        Doc Time
                    </th>
                    <th style="width: 80px;">
                        Training Time
                    </th>
                    <th style="width: 60px;">
                        Total
                    </th>
                    <th style="width: 110px;">
                        Remark
                    </th>
                    <th style="width: 60px;">
                        Action
                    </th>
                </tr>
                <tr class="trtemp">
                    <td>
                        <input value="Week1" disabled="disabled" name="txtTime" type="text" class="input630"
                            tabindex="1" style="width: 40px;" />
                    </td>
                    <td>
                        <input validate="true" name="txtQa" type="text" class="input630" tabindex="2" style="width: 40px;" />
                    </td>
                    <td>
                        <input validate="true" name="txtDev" type="text" class="input630" tabindex="3" style="width: 40px;" />
                    </td>
                    <td>
                        <input validate="true" name="txtGrap" type="text" class="input630" tabindex="4" style="width: 40px;" />
                    </td>
                    <td>
                        <input validate="true" name="txtDoc" type="text" class="input630" tabindex="5" style="width: 40px;" />
                    </td>
                    <td>
                        <input validate="true" name="txtTraining" type="text" class="input630" tabindex="6"
                            style="width: 40px;" />
                    </td>
                    <td>
                        <input disabled="disabled" name="txtTotal" type="text" class="input630" tabindex="7"
                            style="width: 40px;" />
                    </td>
                    <td style="text-align: left">
                        <input name="txtRemark" type="text" class="input630" tabindex="8" style="width: 110px;" />
                    </td>
                    <td>
                        <img src="/icons/37.gif" alt="add" onclick="addEs(); return false;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="btnBoxone">
        <input id="btnSave" type="button" value="Save" class="btnfive" onclick='SaveHours();return false;' />
        <input id="btnClear" type="button" value="Clear" onclick='ClearHours();return false;'
            class="btnfive" style="display: none;" />
    </div>

    <script type="text/javascript">

        function addEs() {
            var _len = 0;
            if (HasRowCount > 0) {
                HasRowCount++;
                _len = HasRowCount;
            } else {
                HasRowCount = 2;
                _len = HasRowCount;
            }
            $('#esTbl').append(
                        '<tr  class="trtemp">' +
                                '<td> <input disabled="disabled" value ="Week' + _len + '" name="txtTime" type="text" class="input630" style="width: 40px;" /></td>' +
                                '<td> <input validate="true"  name="txtQa"  type="text" class="input630" style="width: 40px;" /></td>' +
                                '<td> <input validate="true"  name="txtDev" type="text" class="input630" style="width: 40px;" /></td>' +
                                '<td> <input validate="true"  name="txtGrap"type="text" class="input630" style="width: 40px;" /></td>' +
                                '<td> <input validate="true"  name="txtDoc" type="text" class="input630" style="width: 40px;" /></td>' +
                                '<td> <input validate="true"  name="txtTraining" type="text" class="input630" style="width: 40px;" /></td>' +
                                '<td> <input disabled="disabled"  name="txtTotal" type="text" class="input630" style="width: 40px;" /></td>' +
                                '<td> <input name="txtRemark" type="text" class="input630" style="width: 110px;" /></td>' +
                                '<td>  <img  src="/icons/38.gif" alt="remove" id=' + _len + ' onclick = "removeEs(' + _len + '); return false;" /></td>' +
                       '</tr>'
                   );
        }
        function removeEs(removeID) {
            var vTr = $("#" + removeID).parent("td").parent("tr");
            vTr.remove();
            HasRowCount--;
        }
    </script>

</asp:Content>
