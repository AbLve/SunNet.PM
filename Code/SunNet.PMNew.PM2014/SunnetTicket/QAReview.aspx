<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QAReview.aspx.cs" MasterPageFile="~/Pop.master" Inherits="SunNet.PMNew.PM2014.Ticket.Sunnet.QAReview" %>

<%@ Register Src="~/UserControls/Ticket/FileUploader.ascx" TagName="uploader" TagPrefix="custome" %>

<asp:Content ID="headSection" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#<%=ddlStatus.ClientID%>").on("change", onQADeny);
            onQADeny();

            function onQADeny() {
                if ($("#<%=ddlStatus.ClientID%>").val() == "11" || $("#<%=ddlStatus.ClientID%>").val() == "14") {
                    $("#dvDenyReason").css("display", "block");
                }
                else {
                    $("#dvDenyReason").css("display", "none");
                }
            }
        });
    </script>
</asp:Content>

<asp:Content ID="titleSection" ContentPlaceHolderID="titleSection" runat="server">
    QA Review
</asp:Content>

<asp:Content ID="bodySection" ContentPlaceHolderID="bodySection" runat="server">
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
                        <asp:DropDownList ID="ddlResponsibleUser" runat="server"></asp:DropDownList>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="dvDenyReason">
        <div class="form-group">
            <label class="col-left-owreason lefttext">Description:</label>
            <div class="col-right-owreason righttext">
                <asp:TextBox TextMode="MultiLine" CssClass="inputpmreview" Style="max-width: 600px; max-height: 100px; height: 100px;" Rows="3" runat="server" ID="txtDescription"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-owreason lefttext" style="width: auto !important">Upload a Files:</label>
            <div class="col-right-owreason righttext">
                <custome:uploader runat="server" ID="fileUploader" UploadType="Add" FileUploadCount="1" />
            </div>
        </div>
    </div>
    <div class="sepline2"></div>
    <asp:PlaceHolder ID="phlETA" runat="server" Visible="false">
        <div class="pmreviwDiv1">
            <span class="assignuserTitle">Estimation Hours: </span>
            <span class="rightItem">
                <asp:TextBox ID="txtBoxExtimationHours" runat="server" class="inputnum number" onkeyup="FormatFloatNumber(this,15.2)"></asp:TextBox>
                h </span>
        </div>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ID="buttonSection" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button runat="server" ID="btnSubmit" CssClass="saveBtn1 mainbutton" Text="Save &amp; Close" OnClick="btnSubmit_Click" />
    <input name="Input322" type="button" class="cancelBtn1 mainbutton" data-dismiss="modal" aria-hidden="true" value="Cancel">
</asp:Content>

