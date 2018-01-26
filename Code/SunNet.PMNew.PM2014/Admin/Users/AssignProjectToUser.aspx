<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignProjectToUser.aspx.cs"
    MasterPageFile="~/Pop.master"
    Inherits="SunNet.PMNew.PM2014.Admin.Users.AssignProjectToUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #body_body_dataSection_ticketBasicInfo_rdoPriority {
            border: 0;
        }
    </style>

    <script src="/Scripts/Validate/regex.js"></script>
    <script src="/Scripts/Validate/Validator.js"></script>
    <script src="/Scripts/jquery.maskedinput-1.3.1.min.js"></script>
    <script type="text/javascript">
        jQuery(function ()
        {
            jQuery('#assignProjectToUser tr td input:checkBox').on('click', function (event)
            {
                event.stopPropagation();
            }).closest('td').on('click', function (event)
            {
                var $checkBox = $(this).find('input[type=checkBox]');
                if ($checkBox.prop('checked'))
                {
                    $checkBox.prop('checked', false);
                }
                else
                {
                    $checkBox.prop('checked', true);
                }
                event.stopPropagation();
            });
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Assign Projects
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <table border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td  style="padding-right: 5px"  >Keyword:
            </td>
            <td style="padding-right: 5px">
                <asp:TextBox ID="txtKeyword" queryparam="keyword" runat="server" Width="250px" CssClass="inputw1"></asp:TextBox>
            </td>
            <td>
                <input type="button" class="searchBtn" id="btnSearch" />
            </td>

        </tr>
    </table>
   <div  >
        <table width="100%" border="0" id="assignProjectToUser" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr><th></th>
                <th width="200" >Project Code</th>
                <th width="*" >Project Title</th>
                <th width="200">Company</th>
                <th width="200"> Project Manager</th>
      
            </tr>
        </thead>
        <tr runat="server" id="trNoProjects" visible="false">
            <th colspan="9" style="color: Red;">&nbsp; No records
            </th>
        </tr>
        <asp:Repeater ID="rptProjects" runat="server">
            <ItemTemplate>
                <!-- collapsed expanded -->
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %> '>
                    <td>
                         <input id='<%# Eval("ProjectID")%>' type="checkbox" />
                    </td>
                    <td>
                        <%#Eval("ProjectCode")%>
                    </td>
                    <td>
                        <%#Eval("Title")%>
                    </td>
                    <td>
                        <%#Eval("CompanyName")%>
                    </td>
                    <td>
                        <%#Eval("PMFirstName") + " " + Eval("PMLastName") %>
                    </td> 
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
      
    </div>
     <div class="pagebox">
        <webdiyer:AspNetPager ID="ProjectPage" runat="server">
        </webdiyer:AspNetPager>
    </div>
      
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">

    <div class="buttonBox3">
        <input type="button" ID="btnSave" value=" Save " class="saveBtn1 mainbutton"   onclick="assignProjectToUser(); return false;"/>
        <input name="btnCancel" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
    </div>
     <script type="text/javascript">

         function assignProjectToUser()
         {
             var checkboxList = "";

             $('#assignProjectToUser input[type=checkbox]:checked').each(function ()
             {
                 var _thistd = $(this);
                 checkboxList += _thistd.attr("id") + ",";
             });
             var uid = '<%=QS("uid",0)%>';
            //validate
            checkboxList = $.trim(checkboxList);
            if (checkboxList.length < 1)
            {
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
                success: function (result)
                {
                    //ShowMessage(result, 0, true, true);
                    urlParams.close = 1;
                    ClosePopWindow();
                }
            });
        };
        //clear
        function clearCheckBox()
        {
            $('#assignProjectToUser input[type=checkbox]:checked').each(function ()
            {
                var _thistd = $(this);
                _thistd.attr("checked", false);
            });
        }

        (function SetPagerCssStyle()
        {
            if ($.browser.mozilla)
            {
                $('.pageBox table td input:last').css('margin-top', '3px');
            }
            else if ($.browser.msie)
            {

            }
            $('.pageBox table td').each(function (index, item)
            {
                $(item).css('vertical-align', 'middle');

            }).find('img,span,input').each(function (index, item)
            {

                $(item).css('vertical-align', 'middle');
            }).closest('td').find('input:last').css({ 'margin-left': '5px', 'height': '20px' })
        .closest('td').find('input[type="text"]').css('margin-top', '2px');
        })();
    </script>
</asp:Content>

