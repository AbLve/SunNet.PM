<%@ Page Title="Edit Project" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="EditProject.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Projects.EditProject" %>

<%@ Register Src="../../UserControls/ClientMaintenancePlan.ascx" TagName="ClientMaintenancePlan"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selecteitems {
            padding-left: 0px;
            float: left;
            width: 100%;
            margin: 0px;
        }

        .selecteitems2col {
            width: 260px;
        }

        .selecteitems1col {
            width: 130px;
        }

            .selecteitems1col li {
                /*margin-left:1px;*/
            }

        #sunnetusertitle li {
            font-weight: bolder;
            background-color: #bad8f0;
        }

        .selecteitems li {
            width: 118px;
            height: 16px;
            margin-right: 5px;
            padding-top: 2px;
            padding-left: 5px;
            margin-bottom: 5px;
            cursor: pointer;
            float: left;
            list-style: none;
            border: solid 1px #BAD8F0;
            overflow: hidden;
        }

            .selecteitems li.selected {
                background: url('/Icons/29.gif') no-repeat right center;
            }

            .selecteitems li.plus {
                background: url('/Images/plus.png') no-repeat right center;
            }

            .selecteitems li.minus {
                background: url('/Images/minus.png') no-repeat right center;
            }

        .fileform th, .fileform td {
            border-bottom: 1px solid #ddd;
        }
    </style>
    <link href="../../Scripts/Dialog/imgAndCss/zebra_dialog.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Edit Project
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owToptwo">
        Basic Information
    </div>
    <div class="owmainBox" style="width: 800px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr>
                <th width="100">Project Title:<span class="redstar">*</span>
                </th>
                <td width="250">
                    <asp:TextBox ID="txtTitle" Validation="true" length="1-128" CssClass="input200" runat="server"></asp:TextBox>
                </td>
                <th width="100">Project Code:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtProjectCode" CssClass="input200" Validation="true" length="1-64"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>Company:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlCompany" CssClass="select205" runat="server">
                    </asp:DropDownList>
                </td>
                <th class="prority">Status:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlStatus" CssClass="select205" runat="server">
                        <asp:ListItem Text="Open" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Scheduled" Value="2"></asp:ListItem>
                        <asp:ListItem Text="In Process" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Completed" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Cancelled" Value="5"></asp:ListItem> 
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Project Manager:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlPM" CssClass="select205" runat="server">
                    </asp:DropDownList>
                </td>
                <th class="prority">Priority:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:DropDownList ID="ddlPriority" CssClass="select205" runat="server">
                        <asp:ListItem Text="High" Value="1HIGH"></asp:ListItem>
                        <asp:ListItem Text="Medium" Value="2MEDIUM"></asp:ListItem>
                        <asp:ListItem Text="Low" Value="3LOW"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Start Date:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtStartDate" Validation="true" length="8-20" RegType="date" CssClass="input180"
                        onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);' runat="server" Style="width: 173px;"> </asp:TextBox>&nbsp;
                    <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtStartDate.ClientID %>"),document.getElementById("<%=txtStartDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                        align="absmiddle">
                </td>
                <th>End Date:<span class="redstar">*</span>
                </th>
                <td>
                    <asp:TextBox ID="txtEndDate" onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);'
                        Validation="true" length="8-20" RegType="date" CssClass="input180" Style="width: 173px;"
                        runat="server"> </asp:TextBox>&nbsp;
                    <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtEndDate.ClientID %>"),document.getElementById("<%=txtEndDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                        align="absmiddle">
                </td>
            </tr>
            <tr>
                <th class="prority">Approved
                    <br />
                    Options:
                </th>
                <td class="prority">
                    <asp:CheckBox ID="chkBugNeedApprove" Text="Bug:require client approve" runat="server" /><br />
                    <asp:CheckBox ID="chkRequestNeedApprove" Text="Request:require client approve" runat="server" />
                </td>
                <th>Test URL:
                </th>
                <td>
                    <asp:TextBox ID="txtTestUrl" CssClass="input200" Validation="true" RegType="url"
                        runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>Test User Name:
                </th>
                <td>
                    <asp:TextBox ID="txtTestUserName" CssClass="input200" runat="server"> </asp:TextBox>
                </td>
                <th>Test Password:
                </th>
                <td>
                    <asp:TextBox ID="txtTestPassword" CssClass="input200" runat="server"> </asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>Free Hours:<span class="redstar">*</span>
                </th>
                <td class="prority">
                    <asp:TextBox ID="txtFreeHour" CssClass="input200" Validation="true" length="1-3"
                        RegType="number" runat="server"></asp:TextBox>
                    <asp:Literal ID="ltlFreeHour" runat="server"></asp:Literal><br />
                    <asp:Literal ID="ltlFreeHourText" runat="server"></asp:Literal>
                </td>
                <th>Billable:
                </th>
                <td>
                    <asp:RadioButtonList ID="rblBillable" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Text="Yes" Value="true"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="false"> </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th valign="top">Description:<span class="redstar">*</span>
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtDesc" Validation="true" length="1-4000" CssClass="input595" TextMode="MultiLine"
                        Rows="5" runat="server"> </asp:TextBox>
                </td>
            </tr>

            <tr>
                <th align="center" valign="top">Maintenance Plan:<span class="redstar">*</span>
                </th>
                <td colspan="3">
                    <uc1:ClientMaintenancePlan ID="ClientMaintenancePlan1" runat="server" />
                </td>
            </tr>

        </table>
        <div class="btnBoxone" id="savebasicinfo" runat="server">
            <asp:Button ID="btnSave" CssClass="btnone" runat="server" Text="Save" OnClick="btnSave_Click"
                OnClientClick="return Validate();" />
            <input name="button2" id="btnClientCancel" type="button" class="btnone" value="Clear" />
        </div>
    </div>
    <div class="owToptwo" style="width: 800px;">
        <div style="width: 300px; float: left;">
            Sunnet Users<asp:HiddenField ID="hidSelectedSunneters" runat="server" />
        </div>
        <div style="width: 100px; float: right; margin-right: 20px;">
            <asp:Button ID="btnSaveSunnet" CssClass="btnone" runat="server" Text=" Assign " OnClick="btnSaveSunnet_Click" />
        </div>
    </div>
    <div class="owmainBox" style="width: 800px;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="border: none;">
            <tr>
                <td>
                    <ul id="sunnetusertitle" class="selecteitems">
                        <li>Dev</li>
                        <li>Dev</li>
                        <li>Leader</li>
                        <li>PM</li>
                        <li>Qa</li>
                        <li>Sales</li>
                    </ul>
                    <ul id="sunnetusersdev" class="selecteitems selecteitems2col">
                        <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.DEV)%>
                        <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.Contactor)%>
                    </ul>
                    <ul id="sunnetusersleader" class="selecteitems selecteitems1col">
                        <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.Leader)%>
                    </ul>
                    <ul id="sunnetuserspm" class="selecteitems selecteitems1col">
                        <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.PM)%>
                        <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.Supervisor)%>
                    </ul>
                    <ul id="sunnetusersqa" class="selecteitems selecteitems1col">
                        <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.QA) %>
                    </ul>
                    <ul id="sunnetuserssales" class="selecteitems selecteitems1col">
                        <%=GetUsersHtml(SunNet.PMNew.Entity.UserModel.RolesEnum.Sales)%>
                    </ul>
                </td>
            </tr>
        </table>
        <div class="btnBoxone" style="text-align: right; clear: both;">
        </div>
    </div>
    <div class="owToptwo" style="width: 800px;">
        <div style="width: 300px; float: left;">
            Client Users
        </div>
        <div style="width: 100px; float: right; margin-right: 20px;">
            <input type="button" class="btnone" value=" Assign " onclick="AssignClient();" />
        </div>
    </div>
    <div class="owmainBox" style="width: 800px;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
            <tr class="owlistTitle">
                <th width="25%">First Name
                </th>
                <th width="25%">Last Name
                </th>
                <th width="25%">Email
                </th>
                <th width="25%">Phone
                </th>
            </tr>
            <tr runat="server" id="trNoUser" visible="false">
                <td colspan="3" style="color: Red;">No Records
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
    <div class="owToptwo" style="width: 800px;">
        <div style="width: 300px; float: left;">
            Files
        </div>
    </div>
    <div class="owmainBox" style="width: 800px;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
            <tr class="owlistTitle">
                <th width="20">Title
                </th>
                <th width="20">Link
                </th>
                <th width="30">Create On
                </th>
                <th width="30">Create By
                </th>
            </tr>
            <tr runat="server" id="trNoProject" visible="false">
                <td colspan="4" style="color: Red; padding-left: 10px;">No records
                </td>
            </tr>
            <asp:Repeater ID="rptFiles" runat="server">
                <ItemTemplate>
                    <tr class=" <%#Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo"%>">
                        <td>
                            <%#Eval("FileTitle")%>
                        </td>
                        <td>
                            <a href='/Do/DoDownloadFileHandler.ashx?FileID=<%#Eval("FileID") %>&size=<%#Eval("FileSize") %>&tableType=<%#Eval("TableType") %>'
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
        </table>
        <table id="fileform" class="owlistone fileform" runat="server" width="100%" border="0"
            cellspacing="0" cellpadding="5">
            <tr>
                <th width="20"></th>
                <th width="10">Title:
                </th>
                <td width="*">
                    <asp:TextBox ID="txtFileTitle" MaxLength="200" CssClass="input200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th></th>
                <th>File:
                </th>
                <td>
                    <asp:FileUpload ID="fileProject" Width="205" runat="server" />
                </td>
            </tr>
            <tr>
                <th></th>
                <th></th>
                <td>
                    <asp:Button ID="btnSaveFiles" CssClass="btnone" runat="server" Text="Add File" OnClick="btnSaveFiles_Click" />
                </td>
            </tr>
        </table>

        <a id="reload" href="EditProject.aspx?id=<%= QS("id") %>&companyId=<%= QS("companyId") %>" style="display: none"></a>
    </div>

    <div class="owToptwo" style="width: 800px;">
        <div runat="server" style="width: 300px; float: left;">
            <a name="propri">Project Principal</a>
        </div>
        <div style="width: 100px; float: right; margin-right: 20px;">
            <input type="button" name="addprincipal" class="btnone" value="Add " onclick="AddPrincipal('/Sunnet/Projects/AddPrincipal.aspx?id=<%= QS("ID") %>    ',450,470,'Add Principal');" />
        </div>
    </div>
    <div class="owmainBox" style="width: 800px;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="owlistone">
            <tr class="owlistTitle">
                <th width="23%">Module/Function
                </th>
                <th width="23%">PM
                </th>
                <th width="23%">DEV
                </th>
                <th width="23%">Tester
                </th>
                <th width="8%">Action
                </th>
            </tr>
            <tr runat="server" id="trNoPri" visible="false">
                <td colspan="3" style="color: Red; padding-left: 10px;">No Records
                </td>
            </tr>
            <asp:Repeater ID="rptPri" runat="server">
                <ItemTemplate>
                    <tr class="<%#Container.ItemIndex % 2 == 0 ? "owlistrowone" : "owlistrowtwo"%>">
                        <td>
                            <%#Eval("Module") %>
                        </td>
                        <td>
                            <%#Eval("PM") %>
                        </td>
                        <td>
                            <%#Eval("DEV") %>
                        </td>
                        <td>
                            <%#Eval("QA") %>
                        </td>
                        <td>
                            <a href="#" onclick="deletePrincipal('<%#Eval("ID") %>',event)">
                                <img border="0" title="Delete" src="/icons/26.gif"></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <a href="#" id="redirect"></a>
    <script type="text/javascript">

        function ShowIFrame(url, width, height, isModal, title) {
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

        var SunnetUsersjQuery;
        function AssignUser(btnSave, hidSelected, classname) {
            var selectedUsers = jQuery.parseJSON(hidSelected.val());
            for (var i = 0; i < selectedUsers.length; i++) {
                try {
                    jQuery("ul." + classname + ">li[userid='" + selectedUsers[i].id + "']").addClass("selected");
                }
                catch (e)
                { }
            }
            btnSave.click(function () {
                SunnetUsersjQuery = jQuery("ul." + classname + ">li[userid]");
                if (SunnetUsersjQuery.filter(".minus").length <= 0 && SunnetUsersjQuery.filter(".plus").length <= 0) {
                    ShowMessage("you have not made any changes ,there is no need to save!", 0, false, false);
                    return false;
                }
                else {
                    var _tempselectedclients = "";
                    SunnetUsersjQuery.filter(".plus").add(".selected").each(function () {
                        var _selectedItem = jQuery(this);
                        if (_selectedItem.hasClass("minus") == false) {

                            _tempselectedclients = _tempselectedclients + _selectedItem.attr("userid");
                            _tempselectedclients = _tempselectedclients + ",";
                        }
                    });
                    if (_tempselectedclients.length > 0) {
                        hidSelected.val(_tempselectedclients);
                        return true;
                    }
                    else {
                        hidSelected.val("");
                        return false;
                    }

                }
            });
            jQuery("ul." + classname + ">li[userid]").click(function () {
                var _this = jQuery(this);
                if (_this.hasClass("selected")) {
                    if (_this.hasClass("minus")) {
                        _this.removeClass("minus");
                    }
                    else {
                        _this.addClass("minus");
                    }
                }
                else {
                    if (_this.hasClass("plus")) {
                        _this.removeClass("plus");
                    }
                    else {
                        _this.addClass("plus");
                    }
                }
            });
        }

        function AssignClient() {
            var result = ShowIFrame("/Sunnet/Projects/AssignClientUsers.aspx?projectId=<% =QS("id") %>&companyId=<% = QS("companyId") %>&r=" + Math.random(), 480, 430, true, "Assign User");

            if (result == 0) {
                reload.click();
            }
        }


        function AssignSunnet() {

            var hidSelectedSunneters = jQuery("#<%=hidSelectedSunneters.ClientID %>");
            var btnSaveSunnet = jQuery("#<%=btnSaveSunnet.ClientID %>");
            AssignUser(btnSaveSunnet, hidSelectedSunneters, "selecteitems");
        }


        function AddPrincipal(url, width, height, title) {
            $.Zebra_Dialog.popWindow(url, title, width, height, function () {
                refreshToAnchor("propri");
            });
        }
        
        function refreshPrincipals() {
            $.getJSON("DoAddPrincipal.ashx?type=list&projectId=<%= QS("ID") %>"+"&r="+Math.random(), function (data) {
                if(data){
                    ShowMessageAndJumpToAnchor("Add Success!",true,"propri");
                }
            }
            );
        }
        
        function deletePrincipal(id,event) {
            event.preventDefault();
            $.post("DoDeletePrincipal.ashx?id=" + id+"&r="+Math.random(), function (data) {
                if (data == "1") {
                    ShowMessageAndJumpToAnchor('Delete Success!',true,"propri");
                }
                else {
                    ShowInfo(data);
                }
            });
        }

        jQuery(function () {
            AssignSunnet();
        });
    </script>

</asp:Content>
