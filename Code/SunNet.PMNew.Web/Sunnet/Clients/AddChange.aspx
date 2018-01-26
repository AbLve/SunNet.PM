<%@ Page Title=" Submit a Change" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
  ValidateRequest="false"   CodeBehind="AddChange.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Clients.AddChange" %>


<%@ Register Src="~/UserControls/UploadFile.ascx" TagName="UploadFile" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            margin: 0px;
        }
    </style>

    <script type="text/javascript">
        var oldForm;
        var projectAndEstimateRelation = null;
        $(function() {
            oldForm = $('#aspnetForm').serialize();
            $('.btnBoxone').on('click',
            function() {
                $('.btnBoxone').attr('clicked', '1');
            }
            );
            projectAndEstimateRelation = eval("<%=jsonProjectAndEstimate%>");
            $("#" + "<%=ddlProject.ClientID%>").on("change", function () {
                $("#" + "<%=chkEN.ClientID%>").prop("checked", projectAndEstimateRelation[$(this).val()] == "True");
            });

        });
        $(window).on('beforeunload', function() {
            var isSubmit = $('.btnBoxone').attr('clicked') != '1';
            var hasChangeForm = $('#aspnetForm').serialize() != oldForm;
            var isValidated = $('.btnBoxone').attr('validated') == '1';
            if ((hasChangeForm && isSubmit) || (hasChangeForm && !isValidated)) {
                return '';
            }
        });
        
         
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Submit a Change
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owToptwo">
        <asp:HiddenField ID="hdhideDev" runat="server" />
        <asp:Literal ID="lilhideDev" runat="server"></asp:Literal>
        <img src="/icons/19.gif" align="absmiddle">
        <a name="Basic">Basic Information</a></div>
    <asp:Label ID="MsgInfo" runat="server" CssClass="msg" Text=""></asp:Label>
    <asp:HiddenField ID="hdIsSunnet" runat="server" />
    <div class="owmainBox" style="background-color: #EFF5FB;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <iframe id="iframeDownloadFile" style="display: none;"></iframe>
        <span class="redstar">* Indicates required fields</span>
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <td width="120">
                </td>
                <td width="280">
                </td>
                <td width="120">
                </td>
                <td width="*">
                </td>
            </tr>
            <tr>
                <th>
                    Project:<span class="redstar">*</span>
                </th>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlProject" runat="server" CssClass="select635" 
                                 Width="635">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <th>
                    Type:<span class="redstar">*</span>
                </th>
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddlType" Width="78">
                        <asp:ListItem Text="Bug" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Request" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="Risk" Value="2"> </asp:ListItem>
                        <asp:ListItem Text="Issue" Value="3"> </asp:ListItem>
                        <asp:ListItem Text="Change" Selected="True" Value="4"> </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
             <tr runat="server" id="trSource" visible="false">
                <th>Source:
                </th>
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddlSource">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Priority:<span class="redstar">*</span>
                </th>
                <td class="prority" colspan="3">
                    <asp:RadioButtonList runat="server" ID="radioPriority" RepeatDirection="Horizontal">
                        <asp:ListItem Text=" Low" Value="1"></asp:ListItem>
                        <asp:ListItem Text=" Medium" Value="2"></asp:ListItem>
                        <asp:ListItem Text=" High" Value="3"></asp:ListItem>
                        <asp:ListItem Text=" Emergency" Value="4"></asp:ListItem>
                    </asp:RadioButtonList>
                    <span class="redstar">**The Emergency Support fee will be based on 1.5 times the regular
                        rate. </span>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="chkEN" runat="server" Text=" Estimation needed" />
                </td>
            </tr>
            <tr>
                <th>
                    Title:<span class="redstar">*</span>
                </th>
                <td colspan="3">
                    <input id="txtTitle" runat="server" type="text" class="input630" style="width: 630px"
                        maxlength="99" />
                </td>
            </tr>
            <tr>
                <th>
                    URL:
                </th>
                <td colspan="3">
                    <input id="txtUrl" runat="server" type="text" class="input630" style="width: 630px" />
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Description:<span class="redstar">*</span>
                </th>
                <td colspan="3">
                    <textarea id="txtDesc" runat="server" cols="20" class="input630" rows="6" style="width: 630px"></textarea>
                </td>
            </tr>
            <tr>
                <th>
                    Screen Capture:
                </th>
                <td colspan="3">
                    <asp:FileUpload ID="fileupload" runat="server" />
                </td>
            </tr>
        </table>
        <div class="btnBoxone">
            <asp:Button ID="btnSaveAsDraft" runat="server" Text="Save as Draft" CssClass="btnone"
                OnClick="btnSaveAsDraft_Click" OnClientClick="return baseValidate();" Width="160" />
            <asp:Button ID="btnSave" runat="server" Text=" Submit" CssClass="btnone" OnClick="btnSave_Click"
                OnClientClick="return baseValidate();" />
            <asp:Button ID="btnSaveAndNew" runat="server" Text="Submit and New" CssClass="btnone"
                OnClientClick="return baseValidate();" OnClick="btnSaveAndNew_Click" Width="160" />
            <input id="btnClear" type="button" value="Clear" class="btnone" />
        </div>
    </div>

    <script type="text/javascript">

        $("#btnClear").click(function() {

            $("input[type=radio]:eq(0)").attr("checked", 'checked');
            $("#<%=ddlProject.ClientID%>").val("value", 0);
            $("#<%=chkEN.ClientID%>").attr('checked', false);
            $("#<%=txtTitle.ClientID%>").val("");
            $("#<%=txtDesc.ClientID%>").val("");
            $("#<%=txtUrl.ClientID%>").val("");
            $("#demolist<%=this.ID %> li").remove();
        });


        function baseValidate(url, title, descr) {
            var result = false;
            if ($("#<%=ddlProject.ClientID%>").get(0).value == "") {
                ShowMessage("Please select a project.", 0, false, false);
                $("#<%=ddlProject.ClientID%>").focus();
                result = false;
            }
            else if ($.trim($("#<%=txtTitle.ClientID%>").val()) == "") {
                ShowMessage("Please enter the title.", 0, false, false);
                $("#<%=txtTitle.ClientID%>").focus();
                result = false;
            } else if ($.trim($("#<%=txtDesc.ClientID%>").val()) == "") {
                ShowMessage("Please Input description.", 0, false, false);
                $("#<%=txtDesc.ClientID%>").focus();
                result = false;
            } else if ($.trim($("#<%=txtTitle.ClientID%>").val()).length > 200) {
                ShowMessage("Title should less than 200 character.", 0, false, false);
                $("#<%=txtTitle.ClientID%>").focus();
                result = false;
            }
            else {
                result = true;
            }
            if (result) {
                $('.btnBoxone').attr('validated', '1');
            }
            else {
                $('.btnBoxone').attr('validated', '0');
            }

            return result;
        }
    </script>

</asp:Content>
