<%@ Page Title="Convert Reason" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="ConvertReason.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.ConvertReason" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .customWidth
        {
            width: 520px;
        }
        .opendivBox1
        {
            width: 518px;
            height: 200px;
        }
        .faqsText
        {
            padding: 5px 10px 5px 25px;
            display: none;
            position: absolute;
            left: 269px;
            top: 86px;
            background-color: #BCE9F7;
            width: 200px;
            height: 110px;
        }
        .faqsText p
        {
            border-bottom: 1px solid #ddd;
        }
    </style>

    <script type="text/javascript">
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        $(document).ready(function() {
            $("#txtDesc").focus();
            jQuery("#help").mouseover(function() {
                jQuery("#div7").slideToggle("fast");
            });
            jQuery("#help").mouseleave(function() {
                jQuery("#div7").slideUp("fast");
            });
            $("#btnSave").click(function() {
                var desc = $("#<%=txtDesc.ClientID%>").val();
                var tid = getUrlParam('tid');
                var ckIsSave = $('input[type="radio"][name="rd"]:checked').val();
                var action = getUrlParam('uType');
                $.ajax({

                    type: "post",

                    url: "/Do/DoConverReasonHandler.ashx?r=" + Math.random(),

                    data: {
                        desc: desc,
                        tid: tid,
                        ckIsSave: ckIsSave,
                        action: action
                    },
                    success: function(result) {
                        ShowMessage(result, 0, true, true);
                    }
                });

            });

            //clear
            $("#btnClear").click(function() {
                $("#<%=txtDesc.ClientID%>").val('');
                $("#defaultRD").attr("checked", "checked");
            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="opendivTopone_left">
        Convert Reason
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <div class="owlistContainer">
            <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
                <tr runat="server" class="owlistrowone" id="trCk" visible="false">
                    <td style="width: 120px;">
                        Keep the original Bug
                    </td>
                    <td colspan="2">
                        <input id="defaultRD" name="rd" type="radio" checked="checked" value="yes" />Yes
                        <input name="rd" style="margin-right: 5px;" type="radio" value="no" />No <span class="redstar">
                            *</span><a href="#" id="help"><img src="/icons/icn-help.png" alt="help" /></a>
                        <div class="faqsText" id="div7">
                            <p>
                                <strong class="faqsspantext1">Yes </strong>- <u>Yes</u> This ticket will be kept
                                as bug partially, a new ticket will be created as Request.
                            </p>
                            <p>
                                <strong class="faqsspantext1">No </strong>- <u>No</u> This ticket will be cancelled,
                                a new ticket will be created as Request.
                            </p>
                        </div>
                    </td>
                </tr>
                <tr class="owlistrowtwo">
                    <td colspan="3">
                        <span>Original Description:</span>
                    </td>
                </tr>
                <tr class="owlistrowtwo">
                    <td colspan="3">
                        <textarea id="txtOriginalDesc" runat="server" class="input98p" rows="4" name="textarea2"
                            style="width: 100%; height: 120px;" readonly="readonly">
                        </textarea>
                    </td>
                </tr>
                <tr class="owlistrowtwo">
                    <td colspan="3">
                        <span>Please specify your reason:</span>
                    </td>
                </tr>
                <tr class="owlistrowtwo">
                    <td colspan="3">
                        <textarea id="txtDesc" runat="server" class="input98p" rows="4" name="textarea2"
                            style="width: 100%; height: 120px;"></textarea>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="btnBoxone">
        <input id="btnSave" type="button" value="Save" class="btnone" />
        <input id="btnClear" type="button" value="Clear" class="btnone" />
    </div>
</asp:Content>
