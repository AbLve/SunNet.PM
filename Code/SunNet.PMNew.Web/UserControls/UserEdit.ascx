<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserEdit.ascx.cs" Inherits="SunNet.PMNew.Web.UserControls.UserEdit" %>
<div class="owToptwo">
    <img src="/icons/19.gif" align="absmiddle" />
    Basic Information</div>
<div class="owmainBox" style="padding: 5px 25px;">
    <table width="100%" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th width="80">
                Company:<span class="redstar">*</span>
            </th>
            <td width="250">
                <asp:DropDownList ID="ddlCompany" CssClass="select205" runat="server">
                </asp:DropDownList>
            </td>
            <th width="80">
                Title:
            </th>
            <td width="*">
                <asp:TextBox ID="txtTitle" CssClass="input200" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                First Name:<span class="redstar">*</span>
            </th>
            <td>
                <asp:TextBox ID="txtFirstName" Validation="true" length="1-20" CssClass="input200"
                    runat="server"></asp:TextBox>
            </td>
            <th>
                Last Name:<span class="redstar">*</span>
            </th>
            <td>
                <asp:TextBox ID="txtLastName" Validation="true" length="1-20" CssClass="input200"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                User Name:<span class="redstar">*</span>
            </th>
            <td>
                <asp:TextBox ID="txtUserName" Validation="true" ValidatorTitle="User Name: please enter your email address."
                    RegType="email" length="1-50" CssClass="input200" runat="server"></asp:TextBox>
            </td>
            <th>
                Phone:
            </th>
            <td>
                <asp:TextBox ID="txtPhone" CssClass="input200" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                Skype:
            </th>
            <td>
                <asp:TextBox ID="txtSkype" AutoCompleteType="None" autocomplete="off" CssClass="input200"
                    MaxLength="50" runat="server"></asp:TextBox>
            </td>
            <th>
                Status:
            </th>
            <td>
                <asp:DropDownList ID="ddlStatus" CssClass="select205" runat="server">
                    <asp:ListItem Text="Active" Value="ACTIVE"></asp:ListItem>
                    <asp:ListItem Text="Inactive" Value="INACTIVE"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                Password:<span class="redstar">*</span>
            </th>
            <td>
                <asp:TextBox ID="txtPassword" AutoCompleteType="None" autocomplete="off" TextMode="Password"
                    MaxLength="14" CssClass="input200" runat="server"></asp:TextBox>
            </td>
            <th>
                Confirm:<span class="redstar">*</span>
            </th>
            <td>
                <asp:TextBox ID="txtConfirmPassword" AutoCompleteType="None" autocomplete="off" MaxLength="14"
                    TextMode="Password" CssClass="input200" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
<asp:PlaceHolder ID="phSunnet" runat="server">
    <div class="owToptwo">
        <img src="/icons/19.gif" align="absmiddle" />
        Advance Information</div>
    <div class="owmainBox" style="padding: 5px 25px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th width="80">
                    Role:
                </th>
                <td width="250">
                    <asp:DropDownList ID="ddlRole" CssClass="select205" runat="server">
                    </asp:DropDownList>
                </td>
                <th width="80">
                    Office:<span class="redstar">*</span>
                </th>
                <td width="*">
                    <asp:DropDownList ID="ddlOffice" runat="server" CssClass="select205">
                        <asp:ListItem Text="US" Value="US"></asp:ListItem>
                        <asp:ListItem Text="CN" Value="CN"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Client/Sunnet:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:DropDownList ID="ddlUserType" CssClass="select205" runat="server">
                        <asp:ListItem Text="Sunnet" Value="SUNNET"></asp:ListItem>
                        <asp:ListItem Text="Client" Value="CLIENT"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="phClient" runat="server">
    <div class="owToptwo">
        <img src="/icons/19.gif" align="absmiddle" />
        Advance Information</div>
    <div class="owmainBox" style="padding: 5px 25px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th align="center" colspan="4">
                    Emergency Contact
                </th>
            </tr>
            <tr>
                <th width="80">
                    First Name:<span class="redstar">*</span>
                </th>
                <td width="250">
                    <asp:TextBox ID="txtEFirstName" MaxLength="20" Validation="true" ValidatorTitle="Emergency Contact First Name "
                        length="1-20" CssClass="input200" runat="server"></asp:TextBox>
                </td>
                <th width="80">
                    Last Name:<span class="redstar">*</span>
                </th>
                <td width="*">
                    <asp:TextBox ID="txtELastName" MaxLength="20" Validation="true" ValidatorTitle="Emergency Contact Last Name "
                        length="1-20" CssClass="input200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Email:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtEEmail" Validation="true" RegType="email" CssClass="input200"
                        runat="server"></asp:TextBox>
                </td>
                <th>
                    Phone:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtEPhone" Validation="true" ValidatorTitle="The Emergency Contact Phone field is required."
                        RegType="phone" CssClass="input200" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:PlaceHolder>
<% if (UserToEdit != null)
   { %>
<div class="owToptwo">
    <img src="/icons/19.gif" align="absmiddle" />
    Assigned Projects
    <input type="button" class="btnone" style="margin-left: 20px; width: 112px;" runat="server"
        value="Select Project" id="btnSelectProject" onclick="OpenSelectProjectDialog()" />
</div>
<div class="mainrightBoxtwo">
    <asp:HiddenField ID="hidOrderBy" runat="server" Value="ProjectCode" />
    <asp:HiddenField ID="hidOrderDirection" runat="server" Value="asc" />
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
        <tr class="listsubTitle">
            <th style="width: 15%;" orderby="ProjectCode">
                Project Code
            </th>
            <th style="width: 15%;" orderby="Title">
               Project Title
            </th>
            <th style="width: 10%;" orderby="Priority">
                Company
            </th>
            <th style="width: 10%;" orderby="Billable">
                Project Manager
            </th>
        </tr>
        <tr runat="server" id="trNoRecords" visible="true">
            <th colspan="5" style="color: Red;">
                &nbsp; No records
            </th>
        </tr>
        <asp:Repeater ID="rptProjects" runat="server">
            <ItemTemplate>
                <tr opentype="popwindow" dialogwidth="830" dialogheight="700" dialogtitle="" href="/Sunnet/Projects/EditProject.aspx?id=<%#Eval("ID") %>&companyid=<%#Eval("CompanyID") %>"
                    class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                    <td>
                        <%#Eval("ProjectCode")%>
                    </td>
                    <td>
                        <%#Eval("Title")%>
                    </td>
                    <td>
                        <%#Eval("CompanyName")%>
                    </td>
                    <td>
                        <%#Eval("PMUserName")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>
<div class="mainrightBoxPage">
    <div class="pageBox">
        <webdiyer:AspNetPager ID="anpProjects" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
            DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
            ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
            PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
            NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
            runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
            PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
            LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpProjects_PageChanged">
        </webdiyer:AspNetPager>
    </div>
</div>
<%} %>
<div id="btnForm" runat="server" class="owmainBox">
    <div class="btnBoxone" style="padding: 5px 25px;">
        <asp:Button ID="btnSave" CssClass="btnone" ValidationGroup="Add" CausesValidation="true"
            runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return Validate();" />
        <input id="btnClientCancel" name="button" type="button" class="btnone" value="Clear" />
    </div>
</div>

<script type="text/javascript">
    jQuery(function() {
        jQuery("#<%=txtPhone.ClientID %>").add("#<%=txtEPhone.ClientID %>").mask("(999) 999-9999");
    });

    function OpenSelectProjectDialog() {
        var result = ShowIFrame("/Sunnet/Admin/AssignProjectToUser.aspx?uid=" + '<%=UserToEdit==null? 0:UserToEdit.UserID%>', 620, 560, true, "Assign Project");
        if (result == 0) {
            window.location.reload();
        }
    }

    function ShowIFrame(url, width, height, isModal, title) {
        url = FormatUrl(url);

        var windowStyle = "";
        windowStyle += "dialogWidth=";
        windowStyle += width.toString();
        windowStyle += "px;";

        windowStyle += "dialogHeight=";
        windowStyle += height.toString();
        windowStyle += "px;";
        windowStyle += "dialogLeft:" + ((window.screen.width - width) / 2) + "px;";
        windowStyle += "dialogTop:" + ((window.screen.height - height) / 2) + "px;";
        windowStyle += "center=1;help=no;status=no;scroll=auto;resizable=yes";
        //window.open(url,windowStyle);
        return window.showModalDialog(url, window, windowStyle);
    }

    function FormatUrl(url) {
        if (url.indexOf("?") < 0) {
            url = url + "?";
        }
        else {
            url = url + "&";
        }

        url = url + "r=" + Math.random();
        return url;
    }

    (function SetPagerCssStyle() {
        if ($.browser.mozilla) {
            $('.pageBox table td input:last').css('margin-top', '3px');
        }
        else if ($.browser.msie) {

        }
        $('.pageBox table td').each(function(index, item) {
            console.info(item);
            $(item).css('vertical-align', 'middle');
            console.info(item);
        }).find('img,span,input').each(function(index, item) {
            $(item).css('vertical-align', 'middle');
        }).closest('td').find('input:last').css({ 'margin-left': '5px', 'height': '20px' })
        .closest('td').find('input[type="text"]').css('margin-top', '2px');
    })();
</script>

