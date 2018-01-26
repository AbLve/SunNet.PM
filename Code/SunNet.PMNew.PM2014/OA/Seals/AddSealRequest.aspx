<%@ Page Title="" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="AddSealRequest.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Seals.AddSealRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        jQuery(function () {
            // validate signup form on keyup and submit
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div"
            });
        });

        function TypeChange(type) {
            switch (type) {
                case "Seal":
                    $("#SealList").show();
                    //$("#NextApprover").hide();
                    $("input[name = 'chkSeals']").addClass("required");
                    break;
                case "WorkOrder":
                    $("#SealList").hide();
                    //$("#NextApprover").show();
                    $("input[name = 'chkSeals']").removeClass("required");
                    break;
            }
        };
        function OnDdlActionChange() {
            $.ajax({
                url: "/Do/DoGetWorkflowNextPeople.ashx",
                type: "get",
                data: {
                    Action: $("#<%= ddlAction.ClientID%>").val(),
                    RequestType: $("#<%= ddlType.ClientID %>").val(),
                    RequestID: "0"
                },
                dataType: "json"
            }).success(function (message) {
                var userList = message.list;
                if (userList.length > 0) {
                    $("#<%= ddlResponUsers.ClientID %>").parent().show();
                } else {
                    $("#<%= ddlResponUsers.ClientID %>").parent().hide();
                }

                $("#<%= ddlResponUsers.ClientID %>").empty();
                $.each(userList, function (index, element) {
                    $("#<%= ddlResponUsers.ClientID %>").append($('<option></option>').val(element["UserID"]).html(element["FirstAndLastName"]));
                });

            });
            };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <style>
    </style>
    <div class="contentTitle titlsealrequest">Basic Information</div>
    <div class="limitwidth" style="width: 890px;">
        <div class="form-group">
            <label class="col-left-sealrequest lefttext">Title:<span class="noticeRed">*</span></label>
            <div class="col-right-sealrequest2 righttext">
                <asp:TextBox ID="txtTitle" runat="server" MaxLength="128" CssClass="inputsealrequest required"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-sealrequest lefttext">Type:<span class="noticeRed">*</span></label>
            <asp:DropDownList ID="ddlType" runat="server" CssClass="select205" Width="120" onchange="TypeChange(this.value)">
                <asp:ListItem Value="Select">Please select</asp:ListItem>
                <asp:ListItem Value="Seal">Seal</asp:ListItem>
                <asp:ListItem Value="WorkOrder">Work Flow</asp:ListItem>
            </asp:DropDownList>
        </div>


        <div class="form-group" id="SealList" style="display: none">
            <label class="col-left-sealrequest lefttext">Seal:<span class="noticeRed">*</span></label>
            <div class="col-right-sealrequest2 righttext">
                <ul class="seals">
                    <asp:Repeater ID="rptSeals" runat="server">
                        <ItemTemplate>
                            <li>
                                <input name="chkSeals" type="checkbox" data-msg="Please select seal." value="<%# Eval("ID") %>" id="chkSeal<%# Eval("ID") %>" />
                                <label for="chkSeal<%# Eval("ID") %>"><%# Eval("SealName") %></label></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div for="chkSeals" class="error" style="display: none;">Please select seal.</div>
            </div>
        </div>
        <br />
        <div class="form-group">
            <label class="col-left-sealrequest lefttext">Description:</label>
            <div class="col-right-sealrequest2 righttext">
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="inputsealrequest"
                    Rows="6" Width="350"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-sealrequest lefttext">Upload File:</label>
            <div class="col-right-sealrequest2 righttext">
                <asp:FileUpload ID="fileupload1" runat="server" /><br />
                <asp:FileUpload ID="fileupload2" runat="server" /><br />
                <asp:FileUpload ID="fileupload3" runat="server" />
            </div>
        </div>
        <br />
        <div>
            <div class="contentTitle titlsealrequest">Action</div>
            <br />
            <div class="form-group">
                <label class="col-left-sealrequest lefttext">Action:<span class="noticeRed">*</span></label>
                <asp:DropDownList ID="ddlAction" runat="server" onchange="OnDdlActionChange()"></asp:DropDownList>
            </div>
            <br />
            <div class="form-group" style="display: none">
                <label id="responUserText" class="col-left-sealrequest lefttext">Next Users:<span class="noticeRed">*</span></label>
                <asp:DropDownList ID="ddlResponUsers" name="ddlResponUsers" runat="server"></asp:DropDownList>
                <a style="display: none">All items in the list are responsible users.</a>
                <asp:HiddenField ID="HiddenFieldUsers" runat="server" />
            </div>
            <br />
            <div class="buttonBox2">
                <asp:Button ID="btnSave" CssClass="saveBtn1 mainbutton" ValidationGroup="Add" CausesValidation="true" runat="server" Text="OK" OnClientClick="return btnOK_ClientClick()" OnClick="btnSave_Click" />
                <input type="button" value="Back" class="backBtn mainbutton redirectback" />
            </div>
        </div>
        <script type="text/javascript">
            function btnOK_ClientClick() {
                $("#<%= HiddenFieldUsers.ClientID %>")[0].value = $("#<%= ddlResponUsers.ClientID %>").val()
                return true;
            }
        </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
