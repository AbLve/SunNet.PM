<%@ Page Title="" Language="C#" MasterPageFile="~/OA/OA.master" AutoEventWireup="true" CodeBehind="EditSealRequest.aspx.cs" Inherits="SunNet.PMNew.PM2014.OA.Seals.EditSealRequest" EnableEventValidation="false"  %>

<%@ Import Namespace="SunNet.PMNew.Entity.SealModel" %>
<%@ Import Namespace="SunNet.PMNew.Entity.SealModel.Enum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $.validator.addMethod('noSelectNone', function (value, element) {
            return this.optional(element) || (value != -1);
        }, "Please select Action！");

        jQuery(function () {
            // validate signup form on keyup and submit
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $("form").validate({
                errorElement: "div",
                rules: {
                    "<%= ddlAction.UniqueID%>": {
                        noSelectNone: true
                    },
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchSection" runat="server">
    <style>
        .col-right-sealrequest2 input{
            width:350px !important;
        }
    </style>
    <iframe id="iframeDownloadFile" style="display: none;"></iframe>
    <div class="contentTitle titlsealrequest">Basic Information</div>
    <div class="limitwidth">
        <div class="form-group">
            <label class="col-left-sealrequest lefttext">Title:<span class="noticeRed">*</span></label>
            <div class="col-right-sealrequest2 righttext">
                <asp:TextBox ID="txtTitle" runat="server" MaxLength="128" CssClass="inputsealrequest required"></asp:TextBox>
                <asp:HiddenField ID="hdID" runat="server" />
            </div>
        </div>
        <div class="form-group" id="SealList">
            <label class="col-left-sealrequest lefttext">Seal:<span class="noticeRed">*</span></label>
            <div class="col-right-sealrequest2 righttext">
                <ul style="list-style: none; padding: 0; margin: 0">
                    <asp:Repeater ID="rptSeals" runat="server">
                        <ItemTemplate>
                            <li>
                                <input <%= Disable?"disabled='disabled'":"" %> name="chkSeals" class="required" data-msg="Please select seal." type="checkbox" value="<%# Eval("ID") %>" id='chkSeal<%# Eval("ID") %>' <%# (bool)Eval("Checked")? "checked='checked'":"" %> />
                                <label for="chkSeal<%# Eval("ID") %>"><%# Eval("SealName") %></label>
                                <literal> ( Approver: <%# Eval("ApproverFirstName") %> <%# Eval("ApproverLastName") %>; </literal>
                                <literal> Owner: <%# Eval("OwnerFirstName") %> <%# Eval("OwnerLastName") %> )</literal>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div for="chkSeals" class="error" style="display: none;">Please select seal.</div>
                <asp:Literal ID="ltSelas" runat="server"></asp:Literal>
            </div>
        </div>

        <br />

        <div class="form-group">
            <label class="col-left-sealrequest lefttext">Creator:</label>
            <div class="col-right-sealrequest2 righttext">
                <asp:Label ID="lblCreator" runat="server"></asp:Label>
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-sealrequest lefttext">Status:</label>
            <div class="col-right-sealrequest2 righttext">
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-sealrequest lefttext">Description:</label>
            <div class="col-right-sealrequest2 righttext">
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="inputProfle1"
                    Rows="6" Width="350"></asp:TextBox>
                <label id="lblDescription" runat="server" style="word-break: break-all">
                </label>
            </div>
        </div>

        <div class="form-group" id="divUploadFiles1" runat="server">
            <label class="col-left-sealrequest lefttext">Upload File:</label>
            <div class="col-right-sealrequest2 righttext">
                <asp:FileUpload ID="fileupload1" runat="server" /><br />
                <asp:FileUpload ID="fileupload2" runat="server" /><br />
                <asp:FileUpload ID="fileupload3" runat="server" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-sealrequest lefttext">Files:</label>
            <div class="col-right-sealrequest2 righttext">
                <label id="lblFiles" runat="server">
                </label>
            </div>
        </div>

    </div>



    <%--Work flow History--%>
    <br />
    <div class="contentTitle titlsealrequest">Work Flow History</div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance" id="tableWorkflowHis">
        <thead>
            <tr>
                <th width="80px" class="order">User<span class="arrow"></span></th>
                <th width="80px" class="order">Action<span class="arrow"></span></th>
                <th width="80px" class="order">Time<span class="arrow"></span></th>
                <th width="120px" class="order">Comment<span class="arrow"></span></th>
                <th width="120px" class="order">Files<span class="arrow"></span></th>
            </tr>
        </thead>
        <tr runat="server" id="trNoRecords" visible="false">
            <th colspan="4" style="color: Red;">&nbsp; No record found.
            </th>
        </tr>
        <asp:Repeater ID="rptWorkflowHistory" runat="server" OnItemDataBound="rptWorkflowHistory_ItemDataBound">
            <ItemTemplate>
                <tr class='<%# Container.ItemIndex % 2 == 0 ? "whiterow" : "" %>'>
                    <td>
                        <%# Eval("ProcessedByName")%>
                    </td>
                    <td>
                        <asp:Label ID="lblAction" runat="server" Text='<%# ((WorkflowAction)Eval("Action")).WorkflowActionToText() %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblProcessedTime" runat="server" Text='<%# Eval("ProcessedTime") %>'></asp:Label>
                    </td>
                    <td style="word-break: break-all">
                        <%#Eval("Comment")%>
                    </td>
                    <td>
                        <asp:Repeater ID="rptWorkflowHistoryFiles" runat="server">
                            <ItemTemplate>
                                <a href="../../Do/DownloadSealFile.ashx?ID=<%# Eval("ID") %>" target="_blank">
                                    <%# Eval("Name") %><br />
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <br />


    <br />

    <div id="divAction" runat="server" style="display: none">
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

        <div class="form-group" id="divComments" runat="server">
            <label class="col-left-sealrequest lefttext">Comments:</label>
            <div class="col-right-sealrequest2 righttext">
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="inputProfle1 "
                    Rows="3" Width="350"></asp:TextBox>
            </div>
        </div>

        <br />

        <div class="form-group" id="divUploadFiles2" runat="server">
            <label class="col-left-sealrequest lefttext">Upload File:</label>
            <div class="col-right-sealrequest2 righttext">
                <asp:FileUpload ID="fileupload4" runat="server" /><br />
                <asp:FileUpload ID="fileupload5" runat="server" /><br />
                <asp:FileUpload ID="fileupload6" runat="server" />
            </div>
        </div>

        <div class="buttonBox2">
            <asp:Button ID="btnOK" CssClass="saveBtn1 mainbutton" CausesValidation="true"
                runat="server" Text="OK" OnClientClick="return btnOK_ClientClick();" OnClick="btnOK_Click" />
        </div>
    </div>

    <div class="contentTitle titleeventlist">Files</div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance" id="tableDeprecatedFiles">
        <thead>
            <tr>
                <th width="240">Title</th>
                <th>File</th>
                <th width="80" class="aligncenter">Created By </th>
                <th width="80" class="aligncenter">Created On </th>
                <th width="80" class="aligncenter">Action</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptFiles" runat="server">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                        <td><%# Eval("Title") %></td>
                        <td><a href="../../Do/DownloadSealFile.ashx?ID=<%# Eval("ID") %>" target="_blank">
                            <%# Eval("Name") %></a>
                        </td>
                        <td class="aligncenter"><%# Eval("FirstName") %> </td>
                        <td class="aligncenter"><%# Eval("CreateOn","{0:MM/dd/yyyy}")%></td>
                        <td class="aligncenter"><%#ShowDeleteButton((int)Eval("ID"))%></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <div class="contentTitle titleeventlist">Notes</div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance" id="tableDeprecatedNotes">
        <thead>
            <tr>
                <th width="240">Title</th>
                <th>Description </th>
                <th width="80" class="aligncenter">Created By </th>
                <th width="80" class="aligncenter">Created On </th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptNotes" runat="server">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "whiterow" %>'>
                        <td><%# Eval("Title") %></td>
                        <td>
                            <%# Eval("Description") %></td>
                        <td class="aligncenter"><%# Eval("FirstName") %> </td>
                        <td class="aligncenter"><%# Eval("CreateOn","{0:MM/dd/yyyy}")%></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <script type="text/javascript">
        function deleteFile(id, list, o) {
            jQuery.confirm("Are you sure you want to delete this file? ", {
                yesText: "Delete",
                yesCallback: function () {
                    $.ajax({
                        type: "post",
                        url: "/Do/deleteSealFile.ashx?r=" + Math.random(),
                        data: {
                            id: id
                        },
                        success: function (result) {
                        }
                    });
                    if (list) {
                        jQuery(o).parent().parent().remove();
                    }
                    else
                        $("#lif" + id).remove();
                },
                noText: "No",
                noCallback: function () { }
            });
        };

        $(function () {

            // Description
            var $lblDes = $("#<%= lblDescription.ClientID %>");
            if ($lblDes.siblings().size() == 0 && $lblDes.text().length == 0) {
                $lblDes.parent().parent().hide();
            }

            // lable Files
            if ($("#<%= lblFiles.ClientID %>").text().length == 0) {
                $("#<%= lblFiles.ClientID %>").parent().parent().hide();
            }

            //Seals
            if ("<%= sealRequestEntity.Type %>" != "0") {
                $("#SealList").hide();
                $("input[name = 'chkSeals']").removeClass("required");
            }

            $("input[name = 'chkSeals']").each(function () {
                if (!$(this).prop("checked")) {
                    $(this).parent().hide();
                }
            });

            if ("<%= sealRequestEntity.Status %>" != "2") {
                $("#<%= ddlAction.ClientID %>").val("Approve");    
                OnDdlActionChange();
            }
            // table Workflowhistory
            if ($("#tableWorkflowHis").find("tr").length == 1) {
                $("#tableWorkflowHis").hide();
                $("#tableWorkflowHis").prev().hide();
                $("#tableWorkflowHis").prev().prev().hide();
                $("#tableWorkflowHis").next().hide();
            }

            // table Files
            if ($("#tableDeprecatedFiles").find("tr").length == 1) {
                $("#tableDeprecatedFiles").hide();
                $("#tableDeprecatedFiles").prev().hide();
            }

            // table Notes
            if ($("#tableDeprecatedNotes").find("tr").length == 1) {
                $("#tableDeprecatedNotes").hide();
                $("#tableDeprecatedNotes").prev().hide();
            }

        });

        function OnDdlActionChange() {
            $.ajax({
                url: "/Do/DoGetWorkflowNextPeople.ashx",
                type: "get",
                data: {
                    Action: $("#<%= ddlAction.ClientID%>").val(),
                    RequestType: "<%= sealRequestEntity.Type %>" == "0" ? "Seal" : "Workflow",
                    RequestID: "<%= sealRequestEntity.ID %>"
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

        function btnOK_ClientClick() {
            $("#<%= HiddenFieldUsers.ClientID %>")[0].value = $("#<%= ddlResponUsers.ClientID %>").val()
            $("#<%= btnOK.ClientID%>").hide();;
            setTimeout(function () { $("#<%= btnOK.ClientID%>").show(); }, 4000);
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="pagerSection" runat="server">
</asp:Content>
