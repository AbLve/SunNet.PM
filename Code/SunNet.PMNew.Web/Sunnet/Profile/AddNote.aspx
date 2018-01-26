<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/popWindow.Master" AutoEventWireup="true"
    CodeBehind="AddNote.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Profile.AddNote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="addNoteForm" runat="server" action="DoAddNote.ashx">
        <div class="owmainBox">
            <table border="0" cellspacing="0" cellpadding="5">
                <tr>
                    <th>Title:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtNoteTitle" runat="server" MaxLength="500" CssClass="input630" Width="300"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Description<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtNoteDescription" runat="server" TextMode="MultiLine" CssClass="input630"
                            Rows="6" Width="300"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div class="btnBoxone">
            <input type="button" value=" Save " id="Button1" class="btnone" onclick="Check(this);" />
            <asp:HiddenField ID="hdSealRequestID" runat="server" />
        </div>
    </form>
    <script type="text/javascript">
        function Check(elem) {
            if ($("#<%=txtNoteTitle.ClientID %>").val() == "") {
                ShowInfo("Please enter title.");
                return false;
            }
            if ($("#<%=txtNoteDescription.ClientID %>").val() == "") {
                ShowInfo("Please enter description.");
                return false;
            }
            $(elem).closest("form").ajaxSubmit({
                success: function (responseText, statusText, xhr, $form) {
                    if (responseText == 0) {
                        alert("Please specify a file to upload.");
                    }
                    else {
                        $.Zebra_Dialog.closeCurrent(elem);
                    }
                }
                , error: function (context, xhr, e, status) {
                    try {
                        alert('error');
                    }
                    catch (e) {

                    }
                    finally {

                    }
                }

            });
        }
    </script>
</asp:Content>
