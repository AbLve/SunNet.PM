<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/popWindow.Master" AutoEventWireup="true"
    CodeBehind="AddFile.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Profile.AddFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="addFileForm" runat="server" action="DoAddFile.ashx">
        <div class="owmainBox">
            <table border="0" cellspacing="0" cellpadding="5">
                <tr>
                    <th>Title:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtFileTitle" runat="server" MaxLength="500" CssClass="input630" Width="300"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Upload Files:<span class="redstar">*</span>
                    </th>
                    <td>
                        <asp:FileUpload ID="fileupload1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="btnBoxone">
            <input type="button" value=" Save " id="btnSave" class="btnone" onclick="Check(this);" />
            <asp:HiddenField ID="hdSealRequestID" runat="server" />
        </div>
    </form>
    <script type="text/javascript">
        function Check(elem) {
            if ($("#<% =txtFileTitle.ClientID %>").val() == "") {
                alert("Please enter title.");
                return false;
            }
            $(elem).closest("form").ajaxSubmit({
                success: function (responseText, statusText, xhr, $form) {
                    if (responseText == 0) {
                        alert("Please specify a file to upload.");
                    }
                    else if (responseText == 3) {
                        alert("The max length of the upload file exceeded.");
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
            return false;
        }


    </script>


</asp:Content>
