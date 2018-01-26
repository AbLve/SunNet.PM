<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddTicket.ascx.cs" Inherits="SunNet.PMNew.Web.AddTicket" %>
<%@ Register Src="UploadFile.ascx" TagName="UploadFile" TagPrefix="uc1" %>

<script type="text/javascript">
    var IsHide = false;
    function hideDiv() {
        $(".owmainBox").slideToggle("slow", CallbackToChangeText);
    }
    function CallbackToChangeText() {
        if (!IsHide) {
            $("#VisibleText").text("+");
            IsHide = true;
        } else {
            IsHide = false;
            $("#VisibleText").text("--");
        }
    }
    //get url para
    function getUrlParam(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    function AssignUser() {
        // alert(jQuery("#avaUser").find("option:selected").text());
        jQuery("#avaUser").find("option:selected").appendTo($("#ticketUser"));
    }
    function RemoveAssignUser() {
        jQuery("#ticketUser").find("option:selected").appendTo($("#avaUser"));
        // jQuery(this).appendTo($("#avaUser"));
    }
    function baseValidate(url, title, descr) {

        if ($("#<%=ddlProject.ClientID%>").get(0).value == "") {
            ShowMessage("Please select a project.", 0, false, false);
            $("#<%=ddlProject.ClientID%>").focus();
            return false;
        }
        else if ($("#<%=ddlTicketType.ClientID%>").get(0).selectedIndex <= 0) {
            ShowMessage("Please select ticket type.", 0, false, false);
            $("#<%=ddlTicketType.ClientID%>").focus();
            return false;
        }
        else if (title.length < 1) {
            ShowMessage("Please enter the title.", 0, false, false);
            $("#<%=txtTitle.ClientID%>").focus();
            return false;
        } else if (descr.length < 1) {
            ShowMessage("Please Input description.", 0, false, false);
            $("#<%=txtDesc.ClientID%>").focus();
            return false;
        } else if (title.length > 100) {
            ShowMessage("Title should less than 200 character.", 0, false, false);
            $("#<%=txtTitle.ClientID%>").focus();
            return false;
        }
    return true;
}
//declare
var satus;
var pId;
var tType;
var rblPri;
var ckbEn;
var url;
var title;
var descr;
var StartDate;
var DeliveryDate;
var imageList;
var imageSizeList;
var tid;
var isSunnet;
$(document).ready(function () {

    isSunnet = $("#<%=hdIsSunnet.ClientID%>").val();
    //save
    $("input[action='save']").click(function () {
        //set value
        pId = $("#<%=ddlProject.ClientID%>").val();
            tType = $("#<%=ddlTicketType.ClientID%>").val();
            rblPri = $("input[type=radio]:checked").val();
            ckbEn = $("#<%=ckbEN.ClientID%>").attr("checked");
            url = $("#<%=txtUrl.ClientID%>").val();
            title = $("#<%=txtTitle.ClientID%>").val();
            descr = $("#<%=txtDesc.ClientID%>").val();

            StartDate = $("#<%=txtStartDate.ClientID%>").val();
            DeliveryDate = $("#<%=txtDeliveryDate.ClientID%>").val();
            imageList = "";
            imageSizeList = "";
            satus = $(this).attr('satus');
            $('li font').each(function () {
                imageSizeList += $(this).text() + ",";
            })

            $('li b').each(function () {
                imageList += $(this).text() + ",";
            })
            var userlist = '';
            $("#ticketUser option").each(function () {
                userlist += $(this).attr("id") + ",";
            })
            //validate
            url = $.trim(url);
            title = $.trim(title);
            descr = $.trim(descr);
            if (baseValidate(url, title, descr) == false) {
                return false;
            }
            $("input[action='save']").hide(); //hide
            $.ajax({

                type: "post",

                url: "/Do/DoAddTicketHandler.ashx",

                data: {
                    pId: pId,
                    tType: tType,
                    pty: rblPri,
                    ckbEn: ckbEn,
                    title: title,
                    url: url,
                    descr: descr,
                    StartDate: StartDate,
                    DeliveryDate: DeliveryDate,
                    imageList: imageList,
                    imageSizeList: imageSizeList,
                    satus: satus,
                    isSunnet: isSunnet,
                    userlist: userlist
                },
                success: function (result) {
                    if (result != "The ticket has been added.") {
                        $("input[action='save']").show();
                    }
                    ShowMessage(result, 0, true, true);
                }
            });
        });
    //update
    $("input[action='update']").click(function () {
        //set value
        pId = $("#<%=ddlProject.ClientID%>").val();
            tType = $("#<%=ddlTicketType.ClientID%>").val();
            rblPri = $("input[type=radio]:checked").val();
            ckbEn = $("#<%=ckbEN.ClientID%>").attr("checked");
            url = $("#<%=txtUrl.ClientID%>").val();
            title = $("#<%=txtTitle.ClientID%>").val();
            descr = $("#<%=txtDesc.ClientID%>").val();
            StartDate = $("#<%=txtStartDate.ClientID%>").val();
            DeliveryDate = $("#<%=txtDeliveryDate.ClientID%>").val();

            isSunnet = $("#<%=hdIsSunnet.ClientID%>").val();
            imageList = "";
            imageSizeList = "";
            satus = $(this).attr('satus');
            $('li font').each(function () {
                imageSizeList += $(this).text() + ",";
            })

            $('li b').each(function () {
                imageList += $(this).text() + ",";
            })

            //validate
            url = $.trim(url);
            title = $.trim(title);
            descr = $.trim(descr);
            tid = getUrlParam('tid');
            if (baseValidate(url, title, descr) == false) {
                return false;
            }
            $.ajax({

                type: "post",

                url: "/Do/DoEditTicketHandler.ashx",

                data: {
                    tid: tid,
                    pId: pId,
                    tType: tType,
                    pty: rblPri,
                    ckbEn: ckbEn,
                    title: title,
                    url: url,
                    descr: descr,
                    StartDate: StartDate,
                    DeliveryDate: DeliveryDate,
                    imageList: imageList,
                    isSunnet: isSunnet,
                    imageSizeList: imageSizeList,
                    satus: satus
                },
                success: function (result) {
                    ShowMessage(result, 0, true, true);
                }
            });
        });

    //clear
    $("#btnClear").click(function () {

        $("input[type=radio]:eq(0)").attr("checked", 'checked');
        $("#<%=ddlProject.ClientID%>").val("value", 0);
            if (getUrlParam("addType") == null || getUrlParam("addType") == undefined) {
                $("#<%=ddlTicketType.ClientID%>").val("value", 'please select..');
            }
            $("#<%=ckbEN.ClientID%>").attr('checked', false);
            $("#<%=txtTitle.ClientID%>").val("");
            $("#<%=txtDesc.ClientID%>").val("");
            $("#<%=txtStartDate.ClientID%>").val("");
            $("#<%=txtDeliveryDate.ClientID%>").val("");
            $("#<%=txtUrl.ClientID%>").val("");
            $("#demolist<%=this.ID %> li").remove();

        });

    $("#<%=ddlProject.ClientID %>").change(function () {
        pId = parseInt(jQuery("#<%=ddlProject.ClientID%>").val());
            tType = parseInt(jQuery("#<%=ddlTicketType.ClientID%>").val());
            if (parseInt(jQuery("#<%=ddlProject.ClientID%>").val()) > 0 && parseInt(jQuery("#<%=ddlTicketType.ClientID%>").val()) >= 0) {
                $.ajax({

                    type: "post",

                    url: "/Do/DoUpdateEnStatus.ashx",

                    data: {
                        tType: tType,
                        pId: pId
                    },
                    success: function (result) {
                        if (result == "true") {
                            $("#<%=ckbEN.ClientID%>").attr('checked', true);
                        } else {
                            $("#<%=ckbEN.ClientID%>").attr('checked', false);
                        }
                    }
                });
            };
        });

    $("#<%=ddlTicketType.ClientID %>").change(function () {
        pId = parseInt(jQuery("#<%=ddlProject.ClientID%>").val());
            tType = parseInt(jQuery("#<%=ddlTicketType.ClientID%>").val());
            if (parseInt(jQuery("#<%=ddlProject.ClientID%>").val()) > 0 && parseInt(jQuery("#<%=ddlTicketType.ClientID%>").val()) >= 0) {
                $.ajax({

                    type: "post",

                    url: "/Do/DoUpdateEnStatus.ashx",

                    data: {
                        tType: tType,
                        pId: pId
                    },
                    success: function (result) {
                        if (result == "true") {
                            $("#<%=ckbEN.ClientID%>").attr('checked', true);
                        } else {
                            $("#<%=ckbEN.ClientID%>").attr('checked', false);
                        }
                    }
                });
            };

        });
});

    //pm or leader change Internal ticket's status
    function funChangeStatus() {
        MessageBox.Confirm3(null, "Confirm to change status?", '', '', ConfirmInternalStatus);
    }
    function ConfirmInternalStatus(e) {
        if (e) {
            $.ajax({
                type: "post",
                url: "/Do/DoInternalCancel.ashx",
                data: {
                    pId: '<% =ProjectId %>',
                    tId: '<% =TicketID %>',
                    status: $("#<% = selectStatus.ClientID %>").val()
                },
                success: function (result) {
                    if (result == "1") {
                        window.location.href = window.location.href;
                    } else if (result == "2") {
                        MessageBox.Alert3(null, "ticket associated personnel operate it!", null);
                    }
                    else
                        MessageBox.Alert3(null, "error.", null);
                }
            });
        }
    }
</script>

<div class="owToptwo">
    <asp:HiddenField ID="hdhideDev" runat="server" />
    <asp:Literal ID="lilhideDev" runat="server"></asp:Literal>
    <img src="/icons/19.gif" align="absmiddle">
    <a name="Basic">Basic Information</a>
</div>
<asp:Label ID="MsgInfo" runat="server" CssClass="msg" Text=""></asp:Label>
<asp:HiddenField ID="hdIsSunnet" runat="server" />
<div class="owmainBox">
    <iframe id="iframeDownloadFile" style="display: none;"></iframe>
    <span class="redstar">* Indicates required fields</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <select id="selectStatus" runat="server" class="input630" style="width: 160px;" visible="false">
        <option value="2">Cancelled</option>
        <option value="3">PM Reviewed</option>
        <option value="4">Waiting For Estimation</option>
        <option value="5">PM Verify Estimation</option>
        <option value="6">Waiting Sales Confirm</option>
        <option value="7">Estimation Fail</option>
        <option value="8">Estimation Approved</option>
        <option value="9">Developing</option>
        <option value="10">Testing On Local</option>
        <option value="11">Tested Fail On Local</option>
        <option value="12">Tested Success On Local</option>
        <option value="13">Testing On Client</option>
        <option value="14">Tested Fail On Client</option>
        <option value="15">Tested Success On Client</option>
        <option value="16">PM Deny</option>
        <option value="17">Ready For Review</option>
        <option value="18">Not Approved</option>
        <option value="19">Completed</option>
        <option value="20">Internal Cancel</option>
    </select>
    <input type="button" id="btnChangeInternalStatus" runat="server" value="Change" onclick="funChangeStatus()"
        class="btnone" visible="false" />
    <table width="100%" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <td width="120"></td>
            <td width="280"></td>
            <td width="120"></td>
            <td width="*"></td>
        </tr>
        <tr>
            <th>Project:<span class="redstar">*</span>
            </th>
            <td colspan="3">
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="select635" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>Type:<span class="redstar">*</span>
            </th>
            <td colspan="3">
                <select id="ddlTicketType" runat="server">
                    <option value="-1">Please Select...</option>
                    <option value="0">Bug</option>
                    <option value="1">Request</option>
                    <option value="2">Risk</option>
                    <option value="3">Issue</option>
                    <option value="4">Change</option>
                </select>
            </td>
        </tr>
        <tr>
            <th valign="top">Priority:<span class="redstar">*</span>
            </th>
            <td class="prority" colspan="3">
                <input type="radio" id="radioP1" name="radioPriority" runat="server" value="1" />
                Low
                <input type="radio" id="radioP2" name="radioPriority" runat="server" value="2" />
                Medium
                <input type="radio" id="radioP3" name="radioPriority" runat="server" value="3" />
                High
                <input type="radio" id="radioP4" name="radioPriority" runat="server" value="4" />
                Emergency
                <span class="redstar">**The Emergency Support fee will be based on 1.5 times the regular
                    rate. </span>
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <input id="ckbEN" runat="server" type="checkbox" value="" />
                <label for="<%=ckbEN.ClientID %>" style="color: Blue;">Estimation needed</label>
            </td>
        </tr>
        <tr>
            <th>Title:<span class="redstar">*</span>
            </th>
            <td colspan="3">
                <input id="txtTitle" runat="server" type="text" class="input630" />
            </td>
        </tr>
        <tr id="trStartDate" runat="server" visible="false">
            <th>Schedule Start Date:
            </th>
            <td>
                <asp:TextBox ID="txtStartDate" Validation="true" length="8-20" RegType="date" CssClass="input180"
                    onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);' runat="server"> </asp:TextBox>&nbsp;
                <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtStartDate.ClientID %>"),document.getElementById("<%=txtStartDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                    align="absmiddle" runat="server" id="startDateJs" alt="strat date" />
            </td>
            <th>Schedule End Date:
            </th>
            <td>
                <asp:TextBox ID="txtDeliveryDate" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                    Validation="true" length="8-20" RegType="date" CssClass="input180" runat="server"> </asp:TextBox>&nbsp;
                <img src="/icons/30.gif" align="absmiddle" runat="server" id="devDateJs" alt="end date" />
            </td>
        </tr>
        <tr>
            <th>URL:
            </th>
            <td colspan="3">
                <input id="txtUrl" runat="server" type="text" class="input630" />
            </td>
        </tr>
        <tr>
            <th valign="top">Description:<span class="redstar">*</span>
            </th>
            <td colspan="3">
                <textarea id="txtDesc" runat="server" cols="20" class="input630" rows="6"></textarea>
            </td>
        </tr>
        <tr>
            <th>Screen Capture:
            </th>
            <td colspan="3">
                <asp:Literal ID="lilImgList" runat="server"></asp:Literal>
                <uc1:UploadFile ID="UploadFile1" runat="server" />
            </td>
        </tr>
        <tr runat="server" id="trAssignUser" visible="false">
            <th>Ticket Users
            </th>
            <td>
                <select id="avaUser" multiple="multiple" style="height: 100px; width: 150px;">
                    <asp:Literal ID="lilUserList" runat="server"></asp:Literal>
                </select>
            </td>
            <td>
                <input type="button" onclick="AssignUser(); return false;" value=">>" class="btnone" />
                <input type="button" onclick="RemoveAssignUser(); return false;" value="<<" style="margin-top: 10px;"
                    class="btnone" />
            </td>
            <td>
                <select id="ticketUser" multiple="multiple" style="height: 100px; width: 150px; margin-left: 70px;">
                </select>
            </td>
        </tr>
    </table>
</div>
