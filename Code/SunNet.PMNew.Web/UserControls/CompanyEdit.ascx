<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyEdit.ascx.cs"
    Inherits="SunNet.PMNew.Web.UserControls.CompanyEdit" %>
<div class="owmainrightBoxtwo">
    <div>
        <div class="owmainactionBox">
            <div class="tickettop_left">
                General Information
            </div>
        </div>
        <br />
        <table width="95%" border="0" align="center" cellpadding="5" cellspacing="0">
            <tr>
                <th width="50">
                    Company:<span class="redstar">*</span>
                </th>
                <td width="250">
                    <asp:TextBox ID="txtCompanyName" Validation="true" length="1-200" runat="server"
                        TabIndex="1" CssClass="input200"></asp:TextBox>
                </td>
                <td width="80">
                    &nbsp;
                </td>
                <th width="70">
                    Address 1:<span class="redstar">*</span>
                </th>
                <td width="250">
                    <asp:TextBox ID="txtAddress1"   TabIndex="5"  Validation="true" length="1-500" runat="server" CssClass="input200"></asp:TextBox>
                </td>
                <td width="*">
                </td>
            </tr>
            <tr>
                <th>
                    Phone:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtPhone" Validation="true"   TabIndex="2"  length="1-20" runat="server" CssClass="input200"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <th>
                    Address 2:
                </th>
                <td>
                    <asp:TextBox ID="txtAddress2" runat="server"   TabIndex="6"  CssClass="input200"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <th>
                    Fax:
                </th>
                <td>
                    <asp:TextBox ID="txtFax" Validation="true" RegType="phone"   TabIndex="3"  runat="server" CssClass="input200"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <th>
                    City:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtCity" Validation="true" length="1-100" runat="server"   TabIndex="7"  CssClass="input200"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <th>
                    Website:
                </th>
                <td>
                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="input200"   TabIndex="4" ></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <th>
                    State:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:DropDownList runat="server" CssClass="select205" ID="ddlState"    TabIndex="8" >
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
                </td>
                <td>
                </td>
            </tr>
            <tr style="display: none;">
                <th colspan="2">
                    Assigned System Url: <a href="#">client.sunnet.us</a>
                </th>
                <td>
                    &nbsp;
                </td>
                <th>
                    <%--Zip Code:--%>
                </th>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <div class="btnBoxone" id="savebasicinfo" runat="server">
            <asp:Button ID="btnSave" CssClass="btnone" runat="server" Text="Save" OnClick="btnSave_Click"
                OnClientClick="return Validate();" />
            <input name="button2" id="btnClientCancel" type="button" class="btnone" value="Cancel" />
        </div>
    </div>
    <div>
        <div class="owmainactionBox">
            Logo<asp:Literal ID="ltlNoLogo" runat="server"></asp:Literal></div>
        <table id="logoform" runat="server" width="95%" border="0" align="center" cellpadding="5"
            cellspacing="0">
            <tr>
                <th width="20">
                    Logo:
                </th>
                <td width="250">
                    <asp:FileUpload ID="fileLogo" runat="server" />&nbsp;
                </td>
                <td width="100">
                    <asp:Button ID="btnUpload" CssClass="btnone" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                </td>
                <td width="*">
                    <asp:ImageButton ID="iBtnDeleteLogo" ToolTip="Delete Logo" ImageUrl="/icons/34.gif"
                        runat="server" value="Delete Logo" OnClick="iBtnDeleteLogo_Click" />
                </td>
            </tr>
        </table>
        <div class="logoBox">
            <asp:Image ID="imgLogo" runat="server" /></div>
    </div>
    <asp:PlaceHolder ID="phFiles" runat="server">
        <div class="owmainactionBox">
            Files</div>
        <div class="owmainBox">
            <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
                <tr class="owlistTitle">
                    <th width="20">
                        Title
                    </th>
                    <th width="20">
                        Link
                    </th>
                    <th width="30">
                        Create On
                    </th>
                    <th width="30">
                        Create By
                    </th>
                </tr>
                <tr runat="server" id="trNoFiles" visible="false">
                    <td colspan="4" style="color: Red; padding-left: 10px;">
                        No records
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
                                <%#BaseWebsitePage.GetClientUserName(Convert.ToInt32(Eval("CreatedBy").ToString()))%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table id="fileform" runat="server" width="95%" border="0" cellspacing="0" cellpadding="5">
                <tr>
                    <td width="20">
                    </td>
                    <th width="10">
                        Title:
                    </th>
                    <td width="*">
                        <asp:TextBox ID="txtFileTitle" MaxLength="200" CssClass="input200" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <th>
                        File:
                    </th>
                    <td>
                        <asp:FileUpload ID="fileProject" runat="server" Width="205" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <th>
                    </th>
                    <td>
                        <asp:Button ID="btnSaveFiles" CssClass="btnone" runat="server" Text="Add File" OnClick="btnSaveFiles_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:PlaceHolder>
    <div>
        <div class="owmainactionBox">
            Users
        </div>
        <div class="owmainBox">
            <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
                <tr class="owlistTitle">
                    <th width="25%">
                        First Name
                    </th>
                    <th width="25%">
                        Last Name
                    </th>
                    <th width="25%">
                        Email
                    </th>
                    <th width="25%">
                        Phone
                    </th>
                </tr>
                <tr runat="server" id="trNoUser" visible="false">
                    <td colspan="4" style="color: Red;">
                        No Records
                    </td>
                </tr>
                <asp:Repeater ID="rptUsers" runat="server">
                    <ItemTemplate>
                        <tr class="<%#Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo"%>">
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
            </table>
        </div>
    </div>
    <div>
        <div class="owmainactionBox">
            Projects
        </div>
        <div class="owmainBox">
            <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
                <tr class="owlistTitle">
                    <th width="30">
                        Project
                    </th>
                    <th width="50">
                        Description
                    </th>
                    <th width="20">
                        Created By
                    </th>
                </tr>
                <tr runat="server" id="trNoProject" visible="false">
                    <td colspan="3" style="color: Red;">
                        No records
                    </td>
                </tr>
                <asp:Repeater ID="rptProjects" runat="server">
                    <ItemTemplate>
                        <tr class="<%#Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo"%>">
                            <td>
                                <%#Eval("Title") %>
                            </td>
                            <td>
                                <%#Eval("Description") %>
                            </td>
                            <td>
                                <%#BaseWebsitePage.GetClientUserName(Convert.ToInt32(Eval("CreatedBy").ToString()))%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
</div>

<script type="text/javascript">
    jQuery(function() {
        jQuery("#<%=txtPhone.ClientID %>").add("#<%=txtFax.ClientID %>").mask("(999) 999-9999");
    });
</script>

