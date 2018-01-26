<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DevReview.aspx.cs" MasterPageFile="~/Pop.master" Inherits="SunNet.PMNew.PM2014.Ticket.Sunnet.DevReview" %>

<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagName="uploader" TagPrefix="custome" %>
<%@ Register Src="~/UserControls/Sunnet/UsersView.ascx" TagName="userView" TagPrefix="custom" %>

<asp:Content ID="headSection" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="titleSection" ContentPlaceHolderID="titleSection" runat="server">
    DEV Review
</asp:Content>

<asp:Content ID="bodySection" ContentPlaceHolderID="bodySection" runat="server">
    <script type="text/javascript">
        console.log(1);
        var responsibleUserId = "<%=_ticketEntity.ResponsibleUser %>";
        $(function () {
            $("#<%=dvUserView.ClientID%>").find("input[type=\"checkbox\"]").on("click", function () {
                var $ddlResponsibleUser = $("#<%=ddlResponsibleUser.ClientID%>");
                if ($(this).prop("checked")) {
                    $ddlResponsibleUser.append("<option value=\"" + $(this).closest("span").attr("data-id") + "\">" + $(this).closest("span").find("label").text() + "</option>");
                }
                else {
                    $ddlResponsibleUser.find("option[value=\"" + $(this).closest("span").attr("data-id") + "\"]").remove();
                }
            });
            setDefaultEstUsers();
        });

        function setDefaultEstUsers() {
            //如果已经分配了人，那要把这些人加到ddlEstUser的选择项里.
            //并且默认第一个人的值到隐藏域中。
            var selectedEstUser = $("#<%=dvUserView.ClientID%>").find("input[type=\"checkbox\"]:checked");
            var $ddlResponsibleUser = $("#<%=ddlResponsibleUser.ClientID%>");
            var i = 0;
            selectedEstUser.each(function (index, item) {
                console.log(index);
                if (!$(item).closest("span").parent("li").attr("data-iscreate")) {
                    var dataSpan = $(item).closest("span");

                    if (responsibleUserId == dataSpan.attr("data-id"))
                        $ddlResponsibleUser.append("<option value=\"" + dataSpan.attr("data-id") + "\" selected='selected'>" + dataSpan.find("label").text() + "</option>");
                    else
                        $ddlResponsibleUser.append("<option value=\"" + dataSpan.attr("data-id") + "\">" + dataSpan.find("label").text() + "</option>");
                    i++;
                }
            });
        }
        function Saverdo() {
            var responseUser = $("#<%=ddlResponsibleUser.ClientID%>").val();
            if (responseUser == "0") {
                alert("Please select a responsible user.");
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
                <td>
                    <div style="font-size: 13px; font-weight: bold; padding-bottom: 20px;">
                        <asp:Literal runat="server" ID="litHead"></asp:Literal>
                    </div>

                </td>
            </tr>
            <tr>
                <td>
                    <div class="assignuserTitle" style="width: 50%; float: left;">
                        Change Status to:
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="selectw3"></asp:DropDownList>
                    </div>
                    <div class="assignuserTitle" style="width: 50%; float: left;">
                        Responsible User<i style="color: red;"> * </i>:
             <select runat="server" id="ddlResponsibleUser" class="selectw3">
             </select>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:PlaceHolder ID="phlEstimation" runat="server" Visible="false">
        <div class="sepline2"></div>
        <div class="pmreviwDiv1" id="dvEstimationSectoin" runat="server">
            <span class="assignuserTitle">Estimation Hours: </span><span class="rightItem">
                <asp:TextBox ID="txtBoxExtimationHours" runat="server" onkeyup="FormatFloatNumber(this,15.2)" class="inputnum inputw1 number"></asp:TextBox>
                h </span>
        </div>
    </asp:PlaceHolder>
    <div runat="server" id="dvUserView">
        <div class="sepline2"></div>
        <custom:userView runat="server" ID="userViews" />
    </div>
</asp:Content>

<asp:Content ID="buttonSection" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button runat="server" ID="btnSubmit" CssClass="saveBtn1 mainbutton" Text="Save &amp; Close" OnClientClick=" return Saverdo()" OnClick="btnSubmit_Click" />
    <input name="Input322" type="button" class="cancelBtn1 mainbutton" data-dismiss="modal" aria-hidden="true" value="Cancel">
</asp:Content>
