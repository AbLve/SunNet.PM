<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="ComplaintReview.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Complaints.ComplaintReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        label.col-md-1, .col-md-1 {
            width: 60px;
        }

        .form-group {
            display: block;
        }

        .overflow {
            width: 380px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis; /*以省略号替代截除部分*/
        }

        .menu-left-l {
            border-bottom: 1px #ddd dashed;
            padding: 0px 0px;
            margin: 5px;
            margin-left: 0px;
            width: 250px;
        }

        .menu-left-l1 {
            border-bottom: 1px #ddd dashed;
            padding: 0px 0px;
            margin: 8px;
            margin-left: 3px;
            width: 505px;
        }

        .tableTitle {
            color: #0341b3;
            font-size: 14px;
            border-bottom: 1px solid #ddd;
            padding-bottom: 5px;
            font-weight: bold;
            text-transform: uppercase;
        }
    </style>
    <script src="/Scripts/webuploader/webuploader.js"></script>
    <script src="/Scripts/webuploader/uploader.js"></script>
    <script type="text/javascript" src="/Scripts/mediaPlayer/flowplayer-3.2.6.min.js"></script>

    <script type="text/javascript">
        var uploader;

        jQuery(function () {
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });

            //$.ajax({
            //    url: "OA/Complaints/ComplaintReview.aspx/GetComplaintItem",
            //    type: "get",
            //    dataType: "json",
            //    success: function (data, textStatus) {
            //        if (data.Type == "Photo") {
            //            $("#comImg")[0].src = data.Path;
            //            $("#comImg").show();
            //        } else if (data.type == "Video") {
            //            setPlayerVideo(data.Path, "videoPlay");
            //            $("#videoPlay").show();
            //        }
            //    }
            //});

        });

        var _ajaxEvent = 0;
        function beforeSubmit(sender) {

            //if ($("#ddlAction").val() == 0) {
            //    alert("Please select an action.");
            //    return false;
            //}

            var $sender = $(sender);
            function completed() {
                clearTimeout(_ajaxEvent);
                $sender.show();
                $sender.siblings(".loading").hide();
            }
            if ($("form").valid()) {
                _ajaxEvent = setTimeout(function () {
                    $sender.hide();
                    $sender.siblings(".loading").show();
                }, 10);
                var uploaderStatus = uploader.getStats();
                if (uploaderStatus.queueNum > 0 && uploader.state == "ready") {
                    $sender.data("clicked", true);
                    uploader.upload();
                    return false;
                }
                if (uploaderStatus.successNum < 1 && uploaderStatus.uploadFailNum > 0) {
                    completed();
                    return false;
                }
                return true;
            }
            return false;
        }

        function showMore() {
            $("#divshowmore").css("display", "none");
            $("#divcmplhistory").css("display","");
            var h = window.innerHeight;
            window.resizeTo(window.innerWidth, window.innerHeight);
            $(window).scrollTop(12);
        }

        function setPlayerVideo(src) {
            var s2 = "/Scripts/mediaPlayer/flowplayer-3.2.7.swf";
            var s = " <center>  <a href='" + src + "' ";
            s += "       style='display:block;width:410px;height:300px;'  ";
            s += "            id='playerSunnet'>";
            s += "        </a></center> ";
            s += "      <script language='JavaScript'> ";
            s += "          flowplayer('playerSunnet', '" + s2 + "'); ";
            s += "     <";
            s += "/" + "script>	";

            $("#<%=comVideo.ClientID%>").html(s);
            $("#<%=comVideo.ClientID%>").show();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Complaint -
    <asp:Literal ID="ltlComplaintID" runat="server"></asp:Literal>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">

    <table>
        <tr>
            <td>
                <div class="form-group menu-left-l">
                    <label class="lefttext">Type:</label>
                    <div class="righttext"><%=(SunNet.PMNew.Entity.ComplaintModel.Enums.ComplaintTypeEnum)cmplEntity.Type %></div>
                </div>
            </td>
            <td>
                <div class="form-group menu-left-l">
                    <label class="lefttext">Reason:</label>
                    <div class="righttext"><%=(SunNet.PMNew.Entity.ComplaintModel.Enums.ComplaintReasonEnum)cmplEntity.Reason %></div>
                </div>
            </td>
        </tr>

        <tr>
            <td>
                <div class="form-group menu-left-l">
                    <label class="lefttext">System:</label>
                    <div class="righttext"><%=cmplEntity.SystemName %></div>
                </div>
            </td>
            <td>
                <div class="form-group menu-left-l">
                    <label class="lefttext">App Source:</label>
                    <div class="righttext"><%=(SunNet.PMNew.Entity.ComplaintModel.Enums.ComplaintAppSrcEnum)cmplEntity.AppSrc %></div>
                </div>
            </td>
        </tr>

        <tr>
            <td>
                <div class="form-group menu-left-l">
                    <label class="lefttext">Created On:</label>
                    <div class="righttext"><%=cmplEntity.CreatedOn %></div>
                </div>
            </td>
        </tr>
    </table>

    <div class="form-group menu-left-l1">
        <label class="lefttext" style="width: 20%">Additional Info:</label>
        <div class="righttext" style="width: 75%"><%=cmplEntity.AdditionalInfo %></div>
    </div>

    <div class="tableTitle titleeventlist" style="width: 510px">Details </div>

    <asp:Image ID="comImg" runat="server" ImageUrl="<%# comItem.Path %>" alt="Image" style="display: none;"/>

    <div id="comVideo" runat="server" style="display: none;" />

    <table id="comUser" runat="server" style="display: none;">
        <tr>
            <td>
                <div class="form-group menu-left-l">
                    <label class="lefttext">User Name:</label>
                    <div class="righttext"><%=comItem.UserName %></div>
                </div>
            </td>
            <td>
                <div class="form-group menu-left-l">
                    <label class="lefttext">User Email:</label>
                    <div class="righttext"><%=comItem.UserEmail %></div>
                </div>
            </td>
        </tr>
    </table>

    <table id="comGroup" runat="server" style="display: none;">
        <tr>
            <td>
                <div class="form-group menu-left-l">
                    <label class="lefttext">User Name:</label>
                    <div class="righttext"><%=comItem.UserName %></div>
                </div>
            </td>
            <td>
                <div class="form-group menu-left-l">
                    <label class="lefttext">Group Name:</label>
                    <div class="righttext"><%=comItem.GroupName %></div>
                </div>
            </td>
        </tr>
    </table>
    
    <div id="comPost" runat="server" style="display: none;">
        <div class="form-group menu-left-l1">
            <label class="lefttext">User Name:</label>
            <div class="righttext"><%=comItem.UserName %></div>
        </div>

        <div class="form-group menu-left-l1">
            <label class="lefttext">Message:</label>
            <div class="righttext" style="width: 85%"><%=comItem.Message %></div>
        </div>
    </div>

    <br />

    <div class="form-group">
        <label class="col-md-1 lefttext">Notes:<span class="noticeRed">*</span></label>
        <div class="col-md-3 righttext">
            <asp:TextBox TextMode="MultiLine" CssClass="inputpmreview required" Style="width: 430px; max-width: 430px; max-height: 60px; height: 100px;" Rows="3" runat="server" ID="txtComments"></asp:TextBox>
        </div>
    </div>


    <label class="lefttext">Action: &nbsp&nbsp&nbsp</label>
    <td>
        <asp:DropDownList ID="ddlAction" queryparam="NotUse" runat="server">
            <asp:ListItem Text="(Please select)" Value="0"></asp:ListItem>
            <asp:ListItem Text="Delete" Value="DELETE"></asp:ListItem>
            <asp:ListItem Text="Approve but not delete" Value="APPROVEBUTNOTDEL"></asp:ListItem>
            <asp:ListItem Text="Deny" Value="DENY"></asp:ListItem>
        </asp:DropDownList>
    </td>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="saveBtn1 mainbutton"
        OnClientClick="return beforeSubmit(this);" OnClick="btnOK_Click" />
    <input name="Input322" type="button" class="cancelBtn1 mainbutton" data-dismiss="modal" aria-hidden="true" value="Cancel">

    </br>

    <div class="fdshowmore" id="divshowmore" onclick="showMore();">&gt;&gt; <strong>Show More</strong></div>

    <div id="divcmplhistory" style="display: none;">
        <div class="tableTitle titleeventlist">Update History </div>

        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="table-advance">
            <thead>
                <tr>
                    <th class="order" orderby="ModifiedByName">Modified By
                    </th>
                    <th class="order" orderby="Action">Action
                    </th>
                    <th class="order" orderby="ModifiedOn">Modified On
                    </th>
                    <th class="order" orderby="Comments">Comments
                    </th>
                </tr>
            </thead>

            <tbody>
                <tr runat="server" id="trNoComments" visible="false">
                    <td colspan="4">No record found.
                    </td>
                </tr>
                <asp:Repeater ID="rptComplaintHistoryList" runat="server">
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                            <td>
                                <%# Eval("ModifiedByName")%>
                            </td>
                            <td>
                                <%# Eval("Action")%>
                            </td>
                            <td>
                                <%# Eval("ModifiedOn")%>
                            </td>
                            <td>
                                <%# Eval("Comments")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div id="divcmplhistory" style="display: none;">
</asp:Content>
