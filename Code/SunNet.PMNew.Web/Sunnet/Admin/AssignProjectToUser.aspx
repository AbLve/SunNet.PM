<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignProjectToUser.aspx.cs"
    MasterPageFile="~/Sunnet/InputPop.Master" Inherits="SunNet.PMNew.Web.Sunnet.Admin.AssignProjectToUser" %>

<%@ Register Src="../../UserControls/AssignProjectToUser.ascx" TagName="assignProjectToUser"
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
        Assign Projects</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owmainBox">
        <uc1:assignProjectToUser ID="assignProjectToUser"  runat="server" />
    </div>
    <div class="btnBoxone">
        <input id="btnSave" type="button" value="Save" onclick="assignProjectToUser(); return false;"
            class="btnone" />
        <input id="btnClear" type="button" value="Clear" onclick="clearCheckBox(); return false;"
            class="btnone" />
    </div>

    <script type="text/javascript">

        function assignProjectToUser() {
            var checkboxList = "";

            $('#assignProjectToUser input[type=checkbox]:checked').each(function() {
                var _thistd = $(this);
                checkboxList += _thistd.attr("id") + ",";
            });
            var uid = '<%=UserToEdit.UserID%>';
            //validate
            checkboxList = $.trim(checkboxList);
            if (checkboxList.length < 1) {
                ShowMessage("Please Select CheckBox!", 0, false, false);
                return false;
            }
            $.ajax({
                type: "post",
                url: "/Do/DoAssignProjectsToUser.ashx?r=" + Math.random(),
                data: {
                    checkboxList: checkboxList,
                    uid: uid,
                    isClient: '<%=isClient%>'
                },
                success: function(result) {
                    ShowMessage(result, 0, true, true);
                }
            });
        };
        //clear
        function clearCheckBox() {
            $('#assignProjectToUser input[type=checkbox]:checked').each(function() {
                var _thistd = $(this);
                _thistd.attr("checked", false);
            });
        }

        (function SetPagerCssStyle() {
            if ($.browser.mozilla) {
                $('.pageBox table td input:last').css('margin-top', '3px');
            }
            else if ($.browser.msie) {

            }
            $('.pageBox table td').each(function(index, item) {
            $(item).css('vertical-align', 'middle');

        }).find('img,span,input').each(function(index, item) {
            
                $(item).css('vertical-align', 'middle');
            }).closest('td').find('input:last').css({ 'margin-left': '5px', 'height': '20px' })
        .closest('td').find('input[type="text"]').css('margin-top', '2px');
        })();
    </script>

</asp:Content>
