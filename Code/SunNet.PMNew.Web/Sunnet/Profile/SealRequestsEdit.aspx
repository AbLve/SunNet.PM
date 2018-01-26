<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sunnet/popWindow.Master" CodeBehind="SealRequestsEdit.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Profile.SealRequestsEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .listrowone td, .listrowone th {
            padding: 3px 6px 3px 8px;
            background-color: #e0ecf9;
            color: #083583;
        }

        .listrowtwo td, .listrowtwo th {
            padding: 3px 6px 3px 8px;
            background-color: #fff;
            color: #083583;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="sealRequestEditForm" runat="server">
        <iframe id="iframeDownloadFile" style="display: none;"></iframe>
        <div class="owmainBox" style="padding: 0px;">
            <div class="owmainactionBox">
                Basic Informaiton
            </div>
            <table cellpadding="5" cellspacing="0" border="0" style="margin-left: 10px;">
                <tr>
                    <th>Title:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="200" CssClass="input630" Width="350"></asp:TextBox>
                        <asp:HiddenField ID="hdID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th valign="top">Seal:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:CheckBoxList ID="chklistSeal" runat="server">
                        </asp:CheckBoxList>
                        <asp:Literal ID="ltSelas" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr id="trStatus" runat="server">
                    <th>Status:
                    </th>
                    <td>
                        <asp:TextBox ID="txtStatus" runat="server" Enabled="false" CssClass="input630" Width="350"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th valign="top">Description:
                    </th>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="input630"
                            Rows="6" Width="350"></asp:TextBox>
                        <label id="lblDescription" runat="server">
                        </label>
                    </td>
                </tr>
                <tr id="trUploadFiles" runat="server" visible="false">
                    <th>Upload Files:
                    </th>
                    <td>
                        <asp:FileUpload ID="fileupload1" runat="server" /><br />
                        <asp:FileUpload ID="fileupload2" runat="server" /><br />
                        <asp:FileUpload ID="fileupload3" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th valign="top">Files
                    </th>
                    <td>
                        <label id="lblFiles" runat="server">
                        </label>
                    </td>
                </tr>
            </table>
            <div class="btnBoxone">
                <asp:Button ID="btnSave" Text=" Save " CssClass="btnone" runat="server" OnClientClick="return submitForm('Save')"
                    Visible="false" />
                <asp:Button ID="btnSubmit" Text=" Submit " CssClass="btnone" runat="server" Visible="false"
                    OnClientClick="return submitForm('Submit')" />
                <asp:Button ID="btnCancel" runat="server" Text=" Cancel " CssClass="btnone"
                    OnClientClick="return submitForm('Cancel')" Visible="false" />
                <asp:Button ID="btnApproved" runat="server" Text=" Approved " CssClass="btnone" Visible="false"
                    OnClientClick="return submitForm('Approved')" />
                <asp:Button ID="btnDenied" runat="server" Text=" Denied " CssClass="btnone" Visible="false"
                    OnClientClick="return submitForm('Denied')" />
                <asp:Button ID="btnSeal" runat="server" Text=" Sealed " CssClass="btnone" Visible="false"
                    OnClientClick="return submitForm('Sealed')" />
                <asp:Button ID="btnCompleted" runat="server" Text=" Completed " CssClass="btnone"
                    Visible="false" OnClientClick="return submitForm('Completed')" />
            </div>
            <div>
                <div class="owmainactionBox">
                    File
                </div>
                <div class="owlisttopone">
                    <a href="#" onclick="openWin(0,'/Sunnet/Profile/AddFile.aspx?id=<%= QS("ID") %>',450,190,'Add File')">
                        <img src="/icons/37.gif" alt="new/add" align="absmiddle" border="0">
                        Add file</a>
                </div>
                <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone" id="tbFiles">
                    <tbody>
                        <tr class="owlistTitle">
                            <th>Title
                            </th>
                            <th width="25%">File
                            </th>
                            <th width="80px">Created By
                            </th>
                            <th width="80px">Created On
                            </th>
                            <th width="20px">Action
                            </th>
                        </tr>
                        <asp:Repeater ID="rptFiles" runat="server">
                            <ItemTemplate>
                                <tr class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                                    <td>
                                        <%# Eval("Title") %>
                                    </td>
                                    <td>
                                        <a href="../../Do/DownloadSealFile.ashx?ID=<%# Eval("ID") %>" target="iframeDownloadFile">
                                            <%# Eval("Name") %></a>
                                    </td>
                                    <td>
                                        <%# Eval("FirstName") %>
                                    </td>
                                    <td>
                                        <%# Eval("CreateOn","{0:MM/dd/yyyy}")%>
                                    </td>
                                    <td>
                                        <%#ShowDeleteButton((int)Eval("ID"))%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <div>
                <div class="owmainactionBox">
                    Note
                </div>
                <div class="owlisttopone">
                    <a href="#" onclick="openWin(1,'/Sunnet/Profile/AddNote.aspx?id=<%= QS("ID") %>',450,280,'Add Note')">
                        <img src="/icons/37.gif" alt="new/add" align="absmiddle" border="0">
                        Add note</a>
                </div>
                <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone" id="tbNotes">
                    <tbody>
                        <tr class="owlistTitle">
                            <th width="30%">Title
                            </th>
                            <th>Description
                            </th>
                            <th width="80px">Created By
                            </th>
                            <th width="80px">Created On
                            </th>
                        </tr>
                        <asp:Repeater ID="rptNotes" runat="server">
                            <ItemTemplate>
                                <tr class='<%# Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo" %>'>
                                    <td>
                                        <%# Eval("Title") %>
                                    </td>
                                    <td>
                                        <%# Eval("Description") %>
                                    </td>
                                    <td>
                                        <%# Eval("FirstName") %>
                                    </td>
                                    <td>
                                        <%# Eval("CreateOn","{0:MM/dd/yyyy}")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <input type="hidden" runat="server" id="hdChklistKeys" />
    </form>
    <script type="text/javascript">
        function Check() {
            if ($("#<%= txtTitle.ClientID %>").val() == "") {
                ShowInfo("Please entity title.");
                return false;
            }
            return true;
        }

        function deleteFile(id, isRefreshFileList) {
            $.post("/do/deleteSealFile.ashx?id=" + id, function (data) {
                if (data == "OK") {
                    if (isRefreshFileList) {
                        refreshFiles();
                    }
                    else {
                        $("#div" + id).remove();
                    }
                    ShowInfo('Delete Success!');
                }
                else {
                    ShowInfo(data);
                }
            });
        }

        function openWin(type, url, width, height, title) {
            $.Zebra_Dialog.popWindow(url, title, width, height, function () {
                if (type === 0) {
                    refreshFiles();
                }
                else {
                    refreshNotes();
                }
            });
        }

        function refreshFiles() {
            $.getJSON("DoAddFile.ashx?type=list&sealRequestID=<%= QS("ID") %>" + "&" + "r=" + Math.random(), function (data) {
                var resultHTML = constructFileTable(data);
                $("#tbFiles tbody").html(resultHTML);
            }
            );
        }

        function constructFileTable(data) {
            var files = data.files;
            var result = '<tr class="owlistTitle"><th>Title</th><th width="25%">File</th><th width="80px">Created By</th><th width="80px">Created On</th><th width="20px">Action</th></tr>';
            for (var i = 0; i < files.length; i++) {
                result += "<tr class=" + (i % 2 == 0 ? "listrowone" : "listrowtwo") + ">"
                                   + "<td>" + files[i].Title + "</td>"
                                   + "<td><a href='../../Do/DownloadSealFile.ashx?ID=" + files[i].ID + "' target='iframeDownloadFile'>" + files[i].Name + "</a></td>"
                                   + "<td>" + files[i].FirstName + "</td>"
                                   + "<td>" + formatDate(files[i].CreateOn) + "</td>"
                                   + "<td>" + ((files[i].UserID != data.currentUserID || data.NotShowDelete) ? "" : "<a href=\"#\" onclick=\"deleteFile('" + files[i].ID + "',true)\"><img border=\"0\" title=\"Delete\" src=\"/icons/17.gif\"></a>") + "</td>"
                                + "</tr>";
            }
            return result;
        }

        function constructNoteTable(data) {
            var notes = data.notes;
            var result = '<tr class="owlistTitle"><th width="30%">Title</th><th>Description</th><th width="80px">Created By</th><th width="80px">Created On</th></tr>';
            for (var i = 0; i < notes.length; i++) {
                result += "<tr class=" + (i % 2 == 0 ? "listrowone" : "listrowtwo") + ">"
                                   + "<td>" + notes[i].Title + "</td>"
                                   + "<td>" + notes[i].Description + "</td>"
                                   + "<td>" + notes[i].FirstName + "</td>"
                                   + "<td>" + formatDate(notes[i].CreateOn) + "</td>"
                                + "</tr>";
                console.log(i);
                console.log(result);
            }
            return result;
        }

        function formatDate(date) {

            var matchPartial = date.match(/^([0-9]{4})-([0-9]{2})-([0-9]{2})/)[0];
            if (matchPartial) {
                return matchPartial.replace(/^([0-9]{4})-([0-9]{2})-([0-9]{2})/, "$2/$3/$1")
            }
            else {
                return "";
            }
        }

        function refreshNotes() {
            $.getJSON("DoAddNote.ashx?type=list&sealRequestID=<%= QS("ID") %>" + "&" + "r=" + Math.random(), function (data) {
                var resultNotesHTML = constructNoteTable(data);
                $("#tbNotes tbody").html(resultNotesHTML);
            });
        }

        function submitForm(type) {
            if ($("#" + "<%=txtTitle.ClientID%>").val() == "") {
                ShowInfo("Please input title.");
                return false;
            }

            if ($("#" + "<%=chklistSeal.ClientID%>").find("input[type='checkbox']").length > 0 && $("#" + "<%=chklistSeal.ClientID%>").find("input[type='checkbox']:checked").length == 0) {
                ShowInfo("please select a seal.");
                return false;
            }

            $("#" + "<%=txtTitle.ClientID%>").closest("form").ajaxSubmit({
                url: "DoSealRequestEdit.ashx?type=" + type,
                success: function (responseText, statusText, xhr, $form) {
                    if (responseText == "1") {
                        $.Zebra_Dialog.closeCurrent("#" + "<%=txtTitle.ClientID%>");
                    }
                    else if (responseText == "0") {
                        ShowInfo("Operation failed.");
                    }
                    else if (responseText == "2") {
                        ShowInfo("Unauthorized.");
                    }
                    else if (responseText == "-1") {
                        ShowInfo("exception");
                    }
                    else if (responseText == "4") {
                        ShowInfo("Please select seal.");
                    }

                }
            });
            return false;
        }
    </script>
</asp:Content>


