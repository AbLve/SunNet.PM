<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/Main.Master" AutoEventWireup="true"
    CodeBehind="SunnetFilesListView.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Documents.SunnetFilesListView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        ul
        {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
        }
        .currentpath
        {
            width: 350px;
            height: auto;
            padding: 5px;
            border: solid 1px #FFEEA9;
            background: #EAEAEA;
            border-radius: 5px;
            cursor: pointer;
            position: relative;
        }
        .currentpath .title
        {
            font-weight: normal;
        }
        .currentpath .content
        {
            font-weight: bold;
            font-size: larger;
        }
        #directories
        {
            border: solid 1px #FFEEA9;
            border-top: solid 1px #000000;
            width: 360px;
            height: auto;
            position: absolute;
            background: #BAD8F0;
            display: none;
            left: -1px;
            margin-top: 5px;
            border-radius: 0 0 5px 5px;
        }
        .currentpath:hover
        {
            border-radius: 5px 5px 0 0;
        }
        .currentpath:hover #directories
        {
            display: block;
        }
        #directories ul
        {
            list-style-type: none;
            width: 100%;
            margin: 0px;
            padding: 0px;
        }
        #directories li
        {
            height: auto;
            margin: 0px;
            padding: 0px;
        }
        #directories div
        {
            min-height: 15px;
            border: solid 1px #BAD8F0;
            padding: 2px 0px;
        }
        #directories > ul > li > div
        {
            padding-left: 5px;
        }
        #directories > ul > li > ul > li > div
        {
            padding-left: 50px;
        }
        #directories div:hover
        {
            border: solid 1px #ffffff;
        }
        #directories > ul > li ul
        {
            display: none;
        }
        #directories > ul > li:hover, #directories > ul > li.on
        {
            background: #e0ecf9;
        }
        #directories > ul > li:hover > ul, #directories > ul > li.on > ul
        {
            display: block;
        }
        #directories > ul li:hover > ul > li:hover, #directories > ul li.on > ul > li.on
        {
            background: #ffffff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    SunnetFiles
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="97%" border="0" align="center" cellpadding="0" cellspacing="0" class="searchBox">
        <tr>
            <td width="400">
                <div class="currentpath">
                    <span class="title">Current</span> : <span class="content" id="spanPath">All Directory
                        > All</span>
                    <div id="directories">
                        <ul style="margin-top: 5px; padding: 5px 0px;">
                            <li>
                                <div deep="1" direid="1">
                                    All Directory
                                </div>
                            </li>
                            <asp:Repeater ID="rptFirstDire" runat="server" DataMember="ID" OnItemDataBound="rptFirstDire_ItemDataBound">
                                <ItemTemplate>
                                    <li>
                                        <div direid='<%#Eval("ID") %>' deep="1">
                                            <%#Eval("Title")%>
                                        </div>
                                        <asp:Repeater ID="rptSecondDire" runat="server">
                                            <HeaderTemplate>
                                                <ul>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <li>
                                                    <div direid='<%#Eval("ID") %>' deep="2">
                                                        <%#Eval("Title")%>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </ul>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
                <asp:HiddenField ID="hidCurrentDirectory" Value="1-0" runat="server" />
            </td>
            <td width="20">
                ID:
            </td>
            <td width="80">
                <asp:TextBox ID="txtID" CssClass="input200" ToolTip="Unique ID exists in the list of data "
                    Width="50" runat="server"></asp:TextBox>
            </td>
            <td width="50">
                Keyword:
            </td>
            <td width="200">
                <asp:TextBox ID="txtKeyword" CssClass="input180" runat="server"></asp:TextBox>
            </td>
            <td width="40">
                <asp:ImageButton ID="iBtnSearch" ImageUrl="/images/search_btn.jpg" runat="server"
                    OnClick="iBtnSearch_Click" />
            </td>
            <td width="*">
            </td>
        </tr>
    </table>
    <div class="mainactionBox">
        <input type="button" id="btnNewDirectory" class="btnfive" value="New Directory" onclick="return btnNewDirectory_onclick()" />&nbsp;
        <input type="button" id="btnUploadFile" class="btnfive" value="Upload File" onclick="return btnUploadFile_onclick()" />
    </div>
    <div class="mainrightBoxtwo">
        <table id="dataFile" width="100%" border="0" cellpadding="0" cellspacing="0" class="listtwo">
            <tr class="listsubTitle">
                <th width="40">
                    ID
                </th>
                <th width="50">
                    Source
                </th>
                <th width="300">
                    File Name / Ticket Title
                </th>
                <th width="*">
                    Description
                </th>
                <th width="100">
                    Updated On
                </th>
                <th width="80">
                    Updated By
                </th>
                <th width="100">
                    Actoin
                </th>
            </tr>
            <tr runat="server" id="trNoTickets" visible="false">
                <th colspan="7" style="color: Red;">
                    &nbsp; No records
                </th>
            </tr>
            <asp:Repeater ID="rptFiles" runat="server">
                <ItemTemplate>
                    <tr opentype="newtab" href="<%# Eval("Type").ToString().Equals("Ticket") ? "/Sunnet/Tickets/TicketDetail.aspx?tid=" + Eval("ObjectID") : ""%><%# Eval("Type").ToString().Equals("File") ? "/Do/DoDownloadFileHandler.ashx?FileID=" + Eval("ObjectID") : ""%>"
                        type="tickets" class="<%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>"
                        id='<%#Eval("ID") %>'>
                        <td>
                            <%#Eval("ID") %>
                        </td>
                        <td>
                            <%# Eval("Type")%>
                        </td>
                        <td>
                            <%#Eval("Title")%>
                        </td>
                        <td>
                            <%# Eval("Type").ToString().Equals("Ticket") ? "" :Eval("Description")%>
                        </td>
                        <td>
                            <%#Eval("CreatedOn","{0:MM/dd/yyyy}")%>
                        </td>
                        <td>
                            <%#GetClientUserName(Convert.ToInt32(Eval("CreatedBy"))) %>
                        </td>
                        <td isadmin="<%#UserInfo.Role==SunNet.PMNew.Entity.UserModel.RolesEnum.ADMIN %>"
                            userid="<%#UserInfo.ID %>" createuser='<%#Eval("CreatedBy") %>'>
                            <a href="javascript:void(0);" arguments="<%#Eval("Type")+"-"+Eval("ID")+"" %>" url="/Sunnet/Documents/MoveObjects.aspx?objects="
                                action="move">
                                <img src="/icons/12.gif" alt="Move" title="Move" /></a> &nbsp; <a arguments="<%#Eval("Type")+"-"+Eval("ID")+"-"+Eval("ObjectID") %>"
                                    action="delete" href="javascript:void(0);">
                                    <img src="/icons/35.gif" alt="Delete" title="Delete" /></a> &nbsp; <a type="<%#Eval("Type")%>"
                                        url="/Sunnet/Documents/EditObject.aspx?id=<%#Eval("ObjectID") %>&direid=<%#Eval("ID") %>&type=<%#Eval("Type") %>"
                                        action="edit" href="javascript:void(0);">
                                        <img src="/icons/05.gif" alt="Edit" title="Edit" /></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="mainrightBoxPage">
        <div class="pageBox">
            <webdiyer:AspNetPager ID="anpFiles" ShowCustomInfoSection="Left" CustomInfoSectionWidth="5%"
                DisabledButtonImageNameExtension="x" ButtonImageNameExtension="b" HorizontalAlign="Right"
                ShowPageIndexBox="Always" InvalidPageIndexErrorMessage="Invalid page index."
                PageIndexOutOfRangeErrorMessage="Page index out of range." ImagePath="/Images/"
                NumericButtonType="Image" SubmitButtonImageUrl="/Images/go.gif" PageSize="20"
                runat="server" AlwaysShow="true" LayoutType="Table" FirstPageText='<img src="/icons/firstb.gif">'
                PrevPageText='<img src="/icons/prevb.gif">' NextPageText='<img src="/icons/nextb.gif">'
                LastPageText='<img src="/icons/lastb.gif">' OnPageChanged="anpFiles_PageChanged">
            </webdiyer:AspNetPager>
        </div>
    </div>

    <script type="text/javascript">
        var hidCurrentDirectory;
        var iBtnSearch;
        var txtID;
        var txtKeyword;
        var ISCreateDirectoryRoles = "<%=ISCreateDirectoryRoles %>";
        function CheckCreateRoles(first, second) {
            if (ISCreateDirectoryRoles == "true" || ISCreateDirectoryRoles == "True") {
                if (second == "0") {
                    return true;
                }
            }
            return false;
        }
        function GetCurrentDirectoryID(uploadfile) {
            var dires = hidCurrentDirectory.val().split("-");
            var current = 1;
            if (dires.length == 2) {
                current = dires[0];
            }
            if (uploadfile) {
                if (dires[1] != "0") {
                    current = dires[1];
                }
            }
            return current;
        }
        jQuery(function() {
            iBtnSearch = document.getElementById("<%=iBtnSearch.ClientID %>");
            txtID = document.getElementById("<%=txtID.ClientID %>");
            txtKeyword = document.getElementById("<%=txtKeyword.ClientID %>");
            var btnNewDirectory = jQuery("#btnNewDirectory");
            if (ISCreateDirectoryRoles != "True") {
                btnNewDirectory.remove();
            }
            hidCurrentDirectory = jQuery("#<%=hidCurrentDirectory.ClientID %>");
            var txtPath = jQuery("#spanPath");

            jQuery("#directories div").click(function(event) {
                jQuery("#directories>ul li").removeClass("on");
                txtPath.text("");
                var _this = jQuery(this);
                var _targetLi;
                if (_this.attr("deep") == 1) {
                    var result = (_this.attr("direid") != GetCurrentDirectoryID(false)) || (GetCurrentDirectoryID(true) != _this.attr("direid"));

                    _targetLi = _this.parent().addClass("on");
                    txtPath.text(_this.text().trim() + " > All");
                    hidCurrentDirectory.val(_this.attr("direid") + "-0");
                    if (result) {
                        txtID.value = "";
                        txtKeyword.value = "";
                        iBtnSearch.click();
                    }
                }
                else if (_this.attr("deep") == "2") {
                    var result = _this.attr("direid") != GetCurrentDirectoryID(true);
                    _targetLi = _this.parent().addClass("on");
                    var topLiDiv = _targetLi.parent().parent().addClass("on").children("div").eq(0);
                    txtPath.text(topLiDiv.text().trim() + " > " + _this.text());
                    hidCurrentDirectory.val(topLiDiv.attr("direid") + "-" + _this.attr("direid"));
                    if (result) {
                        txtID.value = "";
                        txtKeyword.value = "";
                        iBtnSearch.click();
                    }
                }
            });

            var dires = hidCurrentDirectory.val().split("-");
            if (dires.length == 2) {
                if (dires[1] == "0") {
                    jQuery("#directories>ul>li>div[direid='" + dires[0] + "']").click().parent().addClass("on");
                }
                else if (dires[1] != "0") {
                    jQuery("#directories>ul>li>ul>li>div[direid='" + dires[1] + "']").click().parent().addClass("on");
                }
            }

            jQuery("#dataFile a[action='move']").click(function() {
                var _this = jQuery(this);
                var url = _this.attr("url") + _this.attr("arguments");
                var target = ShowIFrame(url, 500, 400, true, "Move to");
                if (target == undefined || target == 0) {
                    iBtnSearch.click();
                }
            });
            jQuery("#dataFile a[action='delete']").click(function() {
                var _this = jQuery(this);
                MessageBox.Confirm3(null, "Are you sure?", '', '', function() {
                    var arguments = _this.attr("arguments");
                    jQuery.getJSON(
                        "/Do/Directory.ashx?r=" + Math.random(),
                        {
                            type: "DeleteObjects",
                            objects: arguments
                        },
                        function(responseData) {
                            if (responseData.Success) {
                                _this.parent().parent().remove();
                            }
                        });
                });
            });
            jQuery("#dataFile a[action='edit'][type='Ticket']").remove();
            jQuery("#dataFile a[action='edit']").click(function() {
                var _this = jQuery(this);
                var url = _this.attr("url");
                var target = ShowIFrame(url, 500, 400, true, "Edit Directory | Edit File");
                if (target == undefined || target == 0) {
                    iBtnSearch.click();
                }
            });

            jQuery("#dataFile td[isadmin]").each(function(index, item) {
                var _this = jQuery(item);
                if (_this.attr("isadmin") != "True" && _this.attr("createuser") != _this.attr("userid")) {
                    _this.empty();
                }
            });
        });
        function btnNewDirectory_onclick() {
            var result = ShowIFrame("/Sunnet/Documents/NewDirectory.aspx?newdirectory=true&id=" + GetCurrentDirectoryID(), 500, 300, true, "New Directory | Upload File");
            if (result == undefined || result == 0) {
                iBtnSearch.click();
            }
        }

        function btnUploadFile_onclick() {
            var result = ShowIFrame("/Sunnet/Documents/NewDirectory.aspx?id=" + GetCurrentDirectoryID(true), 500, 300, true, "New Directory | Upload File");
            if (result == undefined || result == 0) {
                iBtnSearch.click();
            }
        }

    </script>

</asp:Content>
