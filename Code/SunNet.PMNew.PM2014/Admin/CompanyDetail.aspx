<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyDetail.aspx.cs"
    MasterPageFile="~/Admin/admin.master"
    Inherits="SunNet.PMNew.PM2014.Admin.CompanyDetail" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #body_body_dataSection_ticketBasicInfo_rdoPriority {
            border: 0;
        }
    </style>

    <script src="/Scripts/jquery.maskedinput-1.3.1.min.js"></script>
    <script type="text/javascript">
        jQuery(function () {
            var $btnSave = jQuery("#<%=btnSave.ClientID%>");
            var $btnSaveFile = jQuery("#<%=btnSaveFiles.ClientID%>");
            var validateFile = false;
            jQuery("#<%=txtPhone.ClientID %>").add("#<%=txtFax.ClientID %>").mask("(999) 999-9999");
            jQuery.extend(jQuery.validator.messages, defaultMessageProvider);
            $btnSave.click(function () {
                validateFile = false;
            });
            $btnSaveFile.click(function () {
                validateFile = true;
            });
            $("form").validate({
                errorElement: "div",
                rules: {
                    "<%=fileProject.UniqueID%>": {
                        required: function () {
                            return validateFile;
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="dataSection" runat="server">
    <div class="contentTitle titleprofile">General Information </div>
    <div class="form-group-container">
        <div class="form-group">
            <label class="col-left-profile lefttext">Company:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtCompanyName" MaxLength="200" runat="server"
                    TabIndex="1" CssClass="inputProfle1 required"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext">Address 1:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtAddress1" TabIndex="2" MaxLength="500" runat="server" CssClass="inputProfle1 required"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label class="col-left-profile lefttext">Phone:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtPhone" TabIndex="3" MaxLength="20" runat="server" CssClass="inputProfle1 phone required"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext">Address 2:</label>
            <div class="col-right-profile1 righttext">

                <asp:TextBox ID="txtAddress2" runat="server" TabIndex="4" CssClass="inputProfle1"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Fax:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">

                <asp:TextBox ID="txtFax" ValidatorTitle="The Fax number field cannot be left blank."
                    RegType="fax" MaxLength="20" TabIndex="5" runat="server" CssClass="inputProfle1 phone required"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext">City:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtCity" Validation="true" MaxLength="100" runat="server" TabIndex="6" CssClass="inputProfle1 required"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label class="col-left-profile lefttext">Website:</label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtWebsite" runat="server" CssClass="inputProfle1" TabIndex="7"></asp:TextBox>
            </div>
            <label class="col-left-profile lefttext">State:<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:DropDownList runat="server" CssClass="selectProfle1 required" ID="ddlState" TabIndex="8">
                    <asp:ListItem Value="0">Please Select</asp:ListItem>
                    <asp:ListItem Value="1">AK</asp:ListItem>
                    <asp:ListItem Value="2">AL</asp:ListItem>
                    <asp:ListItem Value="3">AR</asp:ListItem>
                    <asp:ListItem Value="4">AS</asp:ListItem>
                    <asp:ListItem Value="5">AZ</asp:ListItem>
                    <asp:ListItem Value="6">CA</asp:ListItem>
                    <asp:ListItem Value="7">CO</asp:ListItem>
                    <asp:ListItem Value="8">CT</asp:ListItem>
                    <asp:ListItem Value="9">DC</asp:ListItem>
                    <asp:ListItem Value="10">DE</asp:ListItem>
                    <asp:ListItem Value="11">FL</asp:ListItem>
                    <asp:ListItem Value="12">GA</asp:ListItem>
                    <asp:ListItem Value="13">GU</asp:ListItem>
                    <asp:ListItem Value="14">HI</asp:ListItem>
                    <asp:ListItem Value="15">IA</asp:ListItem>
                    <asp:ListItem Value="16">ID</asp:ListItem>
                    <asp:ListItem Value="17">IL</asp:ListItem>
                    <asp:ListItem Value="18">IN</asp:ListItem>
                    <asp:ListItem Value="19">KS</asp:ListItem>
                    <asp:ListItem Value="20">KY</asp:ListItem>
                    <asp:ListItem Value="21">LA</asp:ListItem>
                    <asp:ListItem Value="22">MA</asp:ListItem>
                    <asp:ListItem Value="23">MD</asp:ListItem>
                    <asp:ListItem Value="24">ME</asp:ListItem>
                    <asp:ListItem Value="25">MI</asp:ListItem>
                    <asp:ListItem Value="26">MN</asp:ListItem>
                    <asp:ListItem Value="27">MO</asp:ListItem>
                    <asp:ListItem Value="28">MS</asp:ListItem>
                    <asp:ListItem Value="29">MT</asp:ListItem>
                    <asp:ListItem Value="30">NC</asp:ListItem>
                    <asp:ListItem Value="31">ND</asp:ListItem>
                    <asp:ListItem Value="32">NE</asp:ListItem>
                    <asp:ListItem Value="33">NH</asp:ListItem>
                    <asp:ListItem Value="34">NJ</asp:ListItem>
                    <asp:ListItem Value="35">NM</asp:ListItem>
                    <asp:ListItem Value="36">NV</asp:ListItem>
                    <asp:ListItem Value="37">NY</asp:ListItem>
                    <asp:ListItem Value="38">OH</asp:ListItem>
                    <asp:ListItem Value="39">OK</asp:ListItem>
                    <asp:ListItem Value="40">OR</asp:ListItem>
                    <asp:ListItem Value="41">PA</asp:ListItem>
                    <asp:ListItem Value="42">PR</asp:ListItem>
                    <asp:ListItem Value="43">RI</asp:ListItem>
                    <asp:ListItem Value="44">SC</asp:ListItem>
                    <asp:ListItem Value="45">SD</asp:ListItem>
                    <asp:ListItem Value="46">TN</asp:ListItem>
                    <asp:ListItem Value="47">TX</asp:ListItem>
                    <asp:ListItem Value="48">UT</asp:ListItem>
                    <asp:ListItem Value="49">VA</asp:ListItem>
                    <asp:ListItem Value="50">VI</asp:ListItem>
                    <asp:ListItem Value="51">VT</asp:ListItem>
                    <asp:ListItem Value="52">WA</asp:ListItem>
                    <asp:ListItem Value="53">WI</asp:ListItem>
                    <asp:ListItem Value="54">WV</asp:ListItem>
                    <asp:ListItem Value="55">WY</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="buttonBox3">

            <asp:Button ID="btnSave" TabIndex="9" CssClass="saveBtn1 mainbutton" runat="server" Text="Save" OnClick="btnSave_Click"
                OnClientClick="return Validate();" />
            <input name="button2" tabindex="10" id="btnCancel" type="button" class="backBtn mainbutton redirectback" value="Back" />
        </div>
    </div>
    <div class="seplineOne"></div>
    <div class="contentTitle titleprofile" style="display: none">
        Logo <span class="noticeRed2">
            <asp:Literal ID="ltlNoLogo" runat="server"></asp:Literal></span>
    </div>
    <div class="form-group" style="display: none">

        <div class="col-right-1 righttext">
            <asp:FileUpload ID="fileLogo" runat="server" CssClass="inlineBtn1" />&nbsp;&nbsp;
            <asp:Button ID="btnUpload" CssClass="inlineBtn1" runat="server" Text="Upload" OnClick="btnUpload_Click" />&nbsp;&nbsp;
            <asp:ImageButton ID="iBtnDeleteLogo" ToolTip="Delete Logo" ImageUrl="/Images/icons/deletelogo.gif"
                runat="server" value="Delete Logo" OnClick="iBtnDeleteLogo_Click" CssClass="inlineBtn1" />
        </div>
    </div>
    <div class="companylogo" style="display: none">
        <asp:Image ID="imgLogo" Width="230" Height="77" runat="server" />
    </div>
    <div class="contentTitle titleeventlist">Files</div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th>Title</th>
                <th>Link</th>
                <th>Create On</th>
                <th>Create By</th>
            </tr>
        </thead>
        <tbody>
            <tr runat="server" id="trNoFiles" visible="false">
                <td colspan="4" style="color: Red; padding-left: 10px;">No record found.
                </td>
            </tr>
            <asp:Repeater ID="rptFiles" runat="server">
                <ItemTemplate>
                    <tr class=" <%#Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo"%>">
                        <td>
                            <%#Eval("FileTitle")%>
                        </td>
                        <td>
                            <a href='/Do/DoDownloadFileHandler.ashx?FileID=<%#Eval("FileID") %>&size=<%#Eval("FileSize") %>'
                                target="_blank">
                                <%#Eval("FileTitle")%></a>
                        </td>
                        <td>
                            <%#Eval("CreatedOn")%>
                        </td>
                        <td>
                            <%#GetClientUserName(Convert.ToInt32(Eval("CreatedBy").ToString()))%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <table id="fileform" runat="server" width="95%" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <td width="20"></td>
            <th width="10">Title:
            </th>
            <td width="220">
                <asp:TextBox ID="txtFileTitle" MaxLength="200" CssClass="input200" runat="server"></asp:TextBox>
            </td>
            <td width="10">File:<span class="noticeRed">*</span>
            </td>
            <td>
                <asp:FileUpload ID="fileProject" runat="server" Width="300" CssClass="" />
            </td>
        </tr>
        <tr>
            <td></td>
            <th></th>
            <td>
                <asp:Button ID="btnSaveFiles" CssClass="inlineBtn1" runat="server" Text="Add File" OnClick="btnSaveFiles_Click" />

            </td>
        </tr>
    </table>
    <div class="contentTitle titleeventlist">Clients</div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th>First Name</th>
                <th>Last Name </th>
                <th>Email</th>
                <th>Phone</th>
            </tr>
        </thead>
        <tbody>
            <tr runat="server" id="trNoUser" visible="false">
                <td colspan="4" style="color: Red;">No record found.
                </td>
            </tr>
            <asp:Repeater ID="rptUsers" runat="server">
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex % 2 == 0 ? "" : "whiterow"%>">
                        <td>
                            <%#Eval("FirstName") %>
                        </td>
                        <td>
                            <%#Eval("LastName") %>
                        </td>
                        <td>
                            <a href="mailto:<%#Eval("UserName") %>">
                                <%#Eval("UserName") %></a>
                        </td>
                        <td>
                            <%#Eval("Phone") %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <div class="contentTitle titleeventlist">Projects</div>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-advance">
        <thead>
            <tr>
                <th>Project</th>
                <th>Description </th>
                <th>Created by </th>
            </tr>
        </thead>
        <tbody>
            <tr runat="server" id="trNoProject" visible="false">
                <td colspan="3" style="color: Red;">No record found.
                </td>
            </tr>
            <asp:Repeater ID="rptProjects" runat="server">
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex % 2 == 0 ? "" : "whiterow"%>">
                        <td>
                            <%#Eval("Title") %>
                        </td>
                        <td>
                            <%#Eval("Description") %>
                        </td>
                        <td>
                            <%#GetClientUserName(Convert.ToInt32(Eval("CreatedBy").ToString()))%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</asp:Content>
