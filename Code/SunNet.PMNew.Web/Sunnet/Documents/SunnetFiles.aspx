<%@ Page Title="Sunnet Files & Collected Tickets" Language="C#" MasterPageFile="~/Sunnet/Main.Master"
    AutoEventWireup="true" CodeBehind="SunnetFiles.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Documents.SunnetFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Styles/tree/jquery.checkboxtree.min.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/smoothness/jquery-ui-1.9.0.custom.min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #leftContainerFilesMenu > ul > li > div
        {
            padding-left: 20px;
        }
        #leftContainerFilesMenu > ul > li > ul > li > div
        {
            padding-left: 40px;
        }
        #leftContainerFilesMenu > ul > li > ul > li > ul > li > div
        {
            padding-left: 60px;
        }
        #leftContainerFilesMenu > ul > li > ul > li > ul > li > ul > li > div
        {
            padding-left: 80px;
        }
        #leftContainerFilesMenu > ul > li > ul > li > ul > li > ul > li > ul > li > div
        {
            padding-left: 100px;
        }
        .btnfive
        {
            display: none;
        }
        #leftContainerFilesMenu
        {
            border: solid 2px #80BEF1;
            padding-top: 10px;
            border-radius: 5px;
            width: 150px;
        }
        #leftContainerFilesMenu div
        {
            height: 20px;
            cursor: pointer; /*position:relative;*/
            border: solid 1px #bad8f0;
        }
        #leftContainerFilesMenu div:hover
        {
            border: solid 1px #FFEEA9;
        }
        #leftContainerFilesMenu ul
        {
            list-style-type: none;
            width: 100%;
            margin: 0px;
            padding: 0px;
        }
        #leftContainerFilesMenu ul li
        {
            height: auto;
            min-height: 20px;
            margin: 0px;
            padding: 0px;
        }
        #leftContainerFilesMenu ul li span
        {
            /*position: absolute;*/ /*left: -15px;
            top: 0px;*/
            cursor: pointer; /*display: block;*/
            width: 16px;
            height: 12px;
            display: inline-block;
        }
        #leftContainerFilesMenu ul li.collapsed ul
        {
            display: none;
        }
        #leftContainerFilesMenu ul li.expanded span
        {
            background-image: url('/images/downArrow.gif');
        }
        #leftContainerFilesMenu ul li.collapsed span
        {
            /* display: block;*/
            background-image: url('/images/rightArrow.gif');
        }
        #leftContainerFilesMenu li.currentdirectory > div
        {
            font-weight: bold;
            background-color: #80BEF1;
        }
        #rightObject
        {
            border: solid 2px #80BEF1;
            border-radius: 5px; /*position: relative;*/
            overflow: hidden;
        }
        #directorypath
        {
            width: 105%;
            margin: -5px 0px 0px -5px;
            padding: 9px 4px 4px 9px;
            height: 20px;
            background-color: White;
            border-radius: 4px 4px 0 0;
            font-size: larger;
            font-weight: bold;
        }
        #rightObject ul
        {
            list-style-type: none;
            margin-top: 10px;
            margin-left: 10px;
            padding: 5px;
        }
        #rightObject li
        {
            float: left;
            width: 64px;
            display: block;
            height: 80px;
            border: solid 2px #80BEF1;
            padding: 3px;
            margin-right: 3px;
            margin-bottom: 3px;
            position: relative;
            cursor: pointer;
            background-color: #E0ECF8;
        }
        #rightObject li:hover, #rightObject li.on
        {
            background-color: #35B7E4;
            border: solid 2px #FFEEA9;
        }
        #rightObject li:hover div.content, #rightObject li.on div.content
        {
            border-top: solid 1px #FFEEA9;
        }
        #rightObject li:hover div.action, #rightObject li:hover div.select, #rightObject li.on div.action, #rightObject li.on div.select
        {
            display: block;
        }
        #rightObject li div.select span
        {
            width: 100%;
            height: 17px;
            widows: 100%;
            cursor: default;
            display: inline-block;
        }
        #rightObject li:hover div.select > span
        {
            background: url("/images/checked-100.png") no-repeat scroll 0 0 transparent;
        }
        #rightObject li.on div.select > span
        {
            background: url("/images/checked-100.png") no-repeat scroll 0px -17px transparent;
        }
        #rightObject li div
        {
            float: left;
        }
        #rightObject li div.icon
        {
            width: 64px;
            height: 64px;
        }
        #rightObject li div.icon img
        {
            width: 64px;
            height: 64px;
        }
        #rightObject li div.content
        {
            width: 64px;
            position: absolute;
            left: 3px;
            bottom: 3px;
            border-top: solid 1px #80BEF1;
        }
        #rightObject li div.action
        {
            display: none;
            width: 100%;
            height: 50px;
            position: absolute;
            left: 0px;
            top: 20px;
            z-index: 999;
        }
        #rightObject li div.select
        {
            display: none;
            width: 100%;
            height: 20px;
            position: absolute;
            left: 0px;
            top: 0px;
            z-index: 999;
            overflow: hidden;
        }
        #rightObject li div.content input
        {
            display: none;
        }
        #rightObject li div.content span
        {
            width: 64px;
            display: inline-block;
            height: 15px;
            overflow: hidden;
            word-wrap: normal;
            word-break: break-all;
        }
    </style>

    <script type="text/javascript">
        var CanCreateObject = <%=CheckRoleCanAccessPage("/Sunnet/Documents/NewDirectory.aspx").ToString().ToLower()%>;
        var ISCreateDirectoryRoles = <%=ISCreateDirectoryRoles.ToString().ToLower() %>;
    </script>

    <script type="text/javascript">
        function ShowMessageSunnet(message, level, time) {
            ShowMessage(message, level, false, false);
        }
        function CoverPage() {
            jQuery("#coverDiv").show();
        }
        function ShowPage() {
            jQuery("#coverDiv").hide();
        }
        function GetPathText() {
            var _thisLi = jQuery("#leftContainerFilesMenu li.currentdirectory").eq(0);
            var path = _thisLi.children("div").eq(0).text();
            _thisLi.parents("li").each(function() {
                var _this = jQuery(this);
                path = _this.children("div").eq(0).text() + " > " + path;
            });
            return path;
        }
        function SetCurrentPath() {
            var txtPath = jQuery("#directorypath");
            txtPath.html("").html("Current : " + GetPathText());
        }
        function GetLeftLiHtml(directory) {
            if (directory == undefined || directory == null || directory.Type != "Directory") {
                return "";
            }
            else {
                var li = "<ul><li id='directory";
                li = li + directory.ID;
                li = li + "' type='" + directory.Type + "'";
                li = li + " objectid='" + directory.ID + "'";
                li = li + " style=\"background-image:url('" + directory.Logo + "')\" class='collapsed' ";
                li = li + ">";
                li = li + "<div onclick='ToggleLeftMenu(" + directory.ID + ",true)'>";
                li = li + '<span onclick="ToggleLeftMenu(' + directory.ID + ')"></span>';
                li = li + directory.Title;
                li = li + "</div>";
                li = li + "</li></ul>";
                return li;
            }
        }
        function GetEmptyRightObject(directory) {
            if (CanCreateObject) {
                var content = '<li objectid="' + directory + '" type="empty" >';
                content = content + '   <div class="icon">';
                content = content + '       <img alt="" src="/images/filetype/empty.png" />';
                content = content + '   </div>';
                content = content + '   <div class="content">';
                content = content + '       <span title="Add directory Or upload file">New</span>';
                content = content + '   </div>';
                content = content + '   <div class="action"  title="Add directory Or upload file">';
                content = content + '   </div>';
                content = content + '</li>';
                return content;
            }
            else {
                return "";
            }
        }
        function GetContentHtml(object) {
            var content = '<li objectid="' + object.ObjectID + '" direObjID="' + object.ID + '" type="' + object.Type + '" >';
            content = content + '   <div class="icon">';
            content = content + '       <img alt="" src="/images/filetype/' + object.Logo + '" />';
            content = content + '   </div>';
            content = content + '   <div class="content">';
            content = content + '       <span title="' + object.Title + '">' + object.Title + '</span>';
            content = content + '   </div>';
            content = content + '   <div class="select" title="Select">';
            content = content + '   <span></span>';
            content = content + '   </div>';
            if (object.Type == "Directory") {
                content = content + '   <div class="action"  title="' + object.Description + '">';
            }
            else if (object.Type == "Ticket") {
                content = content + '   <div class="action"  title="Open ticket">';
            }
            else if (object.Type == "File") {
                content = content + '   <div class="action"  title="Download file">';
            }
            else {
                content = content + '   <div class="action">';
            }
            content = content + '   </div>';
            content = content + '</li>';
            // onclick="SelectObject(this);" onclick="OpenObject(\'' + object.Type + '\',\'' + object.ID + '\')"
            return content;
        }
        // left menus
        function GetDirectoriesHtml(directories) {
            if (directories == undefined || directories == null || directories.length == 0) {
                return "";
            }
            else {
                var html = "";
                for (var i = 0; i < directories.length; i++) {
                    if (directories[i].Type == "Directory") {
                        html = html + GetLeftLiHtml(directories[i]);
                    }
                }
                html = html + "";
                return html;
            }
        }

        function GetContents(parent, directories) {
            if (directories == undefined || directories == null) {
                return "";
            }
            else {
                var ulid = "rdirectory" + parent;
                var html = "<ul id='" + ulid + "' directoryid='" + parent + "'>";
                for (var i = 0; i < directories.length; i++) {
                    html = html + GetContentHtml(directories[i]);
                }
                html = html + GetEmptyRightObject(parent);
                html = html + "</ul>";
                return html;
            }
        }
        function RefreshDirectory(directory) {
            var ulid = "rdirectory" + directory;
            jQuery("#" + ulid).remove();
            if (ToggleDirectoryObjects(directory) == false) {
                LoadRightContent(directory);
            }
            SetCurrentPath();
            //            LeftContainer.empty();
            //            LoadDirectory(0, directory);
        }
        function ToggleDirectoryObjects(directory) {
            RightContainer.children("ul").children("li").removeClass("on");
            var ulid = "rdirectory" + directory;
            var dobjs = jQuery("#" + ulid);
            if (dobjs.length > 0) {
                RightContainer.children("ul").removeClass("current").hide();
                dobjs.show().addClass("current");
                return true;
            }
            else {
                return false;
            }
            SetCurrentPath();
        }
        function OpenObject(type, objectid) {
            if (type == "Directory") {
                LoadDirectory(objectid);
            }
            else if (type == "Ticket") {
                window.open("/Sunnet/Tickets/TicketDetail.aspx?tid=" + objectid);
            }
            else if (type == "File") {
                jQuery("#download").attr("href", "/Do/DoDownloadFileHandler.ashx?FileID=" + objectid);
                document.getElementById("download").click();
            }
            else if (type == "empty") {
                var objectAdded = ShowIFrame("/Sunnet/Documents/NewDirectory.aspx?both=true&id=" + objectid, 500, 300, true, "New Directory | Upload File");
                if (objectAdded) {
                    if (objectAdded == 0) {
                        setTimeout(function() { ToggleLeftMenu(objectid, true); }, 500);
                    }
                }
                else {
                    setTimeout(function() { ToggleLeftMenu(objectid, true); }, 500);
                }
            }
            else
            { }
        }
        function ToggleLeftMenu(directory, reload) {
            jQuery("#leftContainerFilesMenu li").removeClass("expanded").addClass("collapsed").removeClass("currentdirectory");
            var _leftContainer = jQuery("#directory" + directory);
            if (reload) {
                _leftContainer.children("ul").remove();
                LoadLeftMenu(directory, directory);
            }
            else {
                _leftContainer.removeClass("collapsed").addClass("expanded").addClass("currentdirectory").parents("li").removeClass("collapsed").addClass("expanded");
            }
            //            if (ToggleDirectoryObjects(directory) == false) {
            //                LoadRightContent(directory);
            //            }
            RefreshDirectory(directory);
            SetCurrentPath();
        }
        function LoadLeftMenu(directory, lockDirectory) {
            var _leftContainer = jQuery("#directory" + directory);
            jQuery.getJSON(
                "/Do/Directory.ashx?r=" + Math.random(),
                {
                    type: "GetObjects",
                    parentID: directory
                },
                function(directories) {
                    var html = GetDirectoriesHtml(directories);
                    if (GetDataTimes == 0 || directory == 0 || directory == "0") {
                        GetDataTimes = GetDataTimes + 1;
                        LeftContainer.append(html);
                        LoadDirectory(1, 1);
                    }
                    else {
                        GetDataTimes = GetDataTimes + 1;
                        _leftContainer.append(html);
                        ToggleLeftMenu(directory);
                        if (lockDirectory) {
                            setTimeout(function() { ToggleLeftMenu(lockDirectory); }, 50);
                        }
                        SetCurrentPath();
                    }
                });
        }
        function LoadRightContent(directory) {
            var ulid = "rdirectory" + directory;
            var _targetRightUl = jQuery("#" + ulid);
            if (_targetRightUl.length > 0) {
                _targetRightUl.remove();
            }
            CoverPage();
            jQuery.getJSON(
                "/Do/Directory.ashx?r=" + Math.random(),
                {
                    type: "GetObjects",
                    parentID: directory
                },
                function(directories) {
                    var html = GetContents(directory, directories);
                    RightContainer.append(html);
                    ToggleDirectoryObjects(directory);
                    ShowPage();
                });
        }
        function LoadDirectory(directory, lockDirectory) {
            LoadLeftMenu(directory, lockDirectory);
        }
        var GetDataTimes = 0;
        var LeftContainer;
        var RightContainer;
        var ClickedSelected;


        function btnOpen_onclick() {
            var selectedLi = jQuery("#rightObject>ul.current>li.on");
            if (selectedLi.length > 1) {
                ShowMessageSunnet("You can only open just one directory or objects one time. ", 1, 2000);
                return false;
            }
            if (ClickedSelected && ClickedSelected.length > 0) {
                var type = ClickedSelected.attr("type");
                var objectid = ClickedSelected.attr("objectid");
                OpenObject(type, objectid);
                ClickedSelected = null;
            }
            else {
                ShowMessageSunnet("Please select a directory | file | ticket first", 1, 3000);
            }
        }

        function btnEdit_onclick() {
            var selectedLi = jQuery("#rightObject>ul.current>li.on");
            if (selectedLi.length > 1) {
                ShowMessageSunnet("You can only edit just one directory or objects one time. ", 1, 2000);
                return false;
            }
            if (ClickedSelected && ClickedSelected.length > 0) {
                var type = ClickedSelected.attr("type");
                var objectid = ClickedSelected.attr("objectid");
                var direObjID = ClickedSelected.attr("direObjID");
                var parentID = ClickedSelected.parent().attr("directoryid");
                if (type == "Directory") {
                    var objectAdded = ShowIFrame("/Sunnet/Documents/EditObject.aspx?id=" + objectid + "&type=" + type, 500, 300, true, "Edit Directory | Edit File");
                    setTimeout(function() { ToggleLeftMenu(parentID, true); ClickedSelected = null; }, 500);
                }
                else if (type == "File") {
                    var objectAdded = ShowIFrame("/Sunnet/Documents/EditObject.aspx?id=" + objectid + "&direid=" + direObjID + "&type=" + type, 500, 300, true, "Edit Directory | Edit File");
                    setTimeout(function() { ToggleLeftMenu(parentID, true); ClickedSelected = null; }, 500);
                }
                else {
                    ShowMessageSunnet("Ticket can not be edited", 1, 3000);
                }
            }
            else {
                ShowMessageSunnet("Please select an object to edit", 1, 3000);
            }
        }
        function btnDelete_onclick() {
            var selectedLi = jQuery("#rightObject>ul.current>li.on");
            if (selectedLi.length > 0) {
                var parasToDelete = "";
                var parentULRight = jQuery("#rightObject>ul.current:first").attr("id").substr(10);
                selectedLi.each(function() {
                    var _thisObject = jQuery(this);
                    parasToDelete = parasToDelete + _thisObject.attr("type");
                    parasToDelete = parasToDelete + "-";
                    parasToDelete = parasToDelete + _thisObject.attr("direObjID");
                    parasToDelete = parasToDelete + "-";
                    parasToDelete = parasToDelete + _thisObject.attr("objectid");
                    parasToDelete = parasToDelete + ",";
                });
                jQuery.getJSON(
                "/Do/Directory.ashx?r=" + Math.random(),
                {
                    type: "DeleteObjects",
                    objects: parasToDelete
                },
                function(responseData) {
                    if (responseData.Success) {
                        setTimeout(function() { RefreshDirectory(parentULRight); }, 500);
                    }
                });
            }
            else {
                ShowMessageSunnet("Please select a directory | file | ticket directory to delete", 1, 3000);
            }
        }
        function btnRefresh_onclick() {
            ClickedSelected = jQuery("#leftContainerFilesMenu li.currentdirectory").eq(0);
            if (ClickedSelected && ClickedSelected.length > 0) {
                var type = ClickedSelected.attr("type");
                var objectid = ClickedSelected.attr("objectid");
                RefreshDirectory(objectid);
            }
        }
        function btnMove_onclick() {
            var selectedLi = jQuery("#rightObject>ul.current>li.on");
            if (selectedLi.length < 1) {
                ShowMessageSunnet("Please select a directory | file | ticket to move", 1, 3000);
            }
            else {
                var _sourceParent = selectedLi.parent().attr("directoryid");
                window.parasToMove = "";
                selectedLi.each(function() {
                    var _thisObject = jQuery(this);
                    parasToMove = parasToMove + _thisObject.attr("type");
                    parasToMove = parasToMove + "-";
                    parasToMove = parasToMove + _thisObject.attr("direObjID");
                    parasToMove = parasToMove + ",";
                });
                var target = ShowIFrame("/Sunnet/Documents/MoveObjects.aspx?objects=" + window.parasToMove, 500, 400, true, "Move to");
                window.parasToMove = "";
                ToggleLeftMenu(_sourceParent);
                if (target && target > 0) {
                    ToggleLeftMenu(target);
                }
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Sunnet Files
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mainactionBox" style="padding-left: 260px;">
        <a href="###" target="_blank" id="download"></a>
        <input id="btnOpen" type="button" class="btnfive everyone" value="Open" onclick="return btnOpen_onclick()" /><input
            id="btnEdit" class="btnfive" type="button" value="Edit" onclick="return btnEdit_onclick()" /><input
                id="btnMove" class="btnfive" type="button" value="Move" onclick="return btnMove_onclick()" /><input
                    id="btnDelete" class="btnfive" type="button" value="Delete" onclick="return btnDelete_onclick()" />
        <input id="btnRefresh" class="btnfive" type="button" value="Refresh" onclick="return btnRefresh_onclick()" /></div>
    <table cellpadding="2" cellspacing="4" style="width: 100%;">
        <tr>
            <td width="20%" id="leftContainerFilesMenu" valign="top">
            </td>
            <td width="80%" id="rightObject" valign="top">
                <div id="directorypath">
                </div>
                <div id="coverDiv" style="width: 100%; height: 100%; display: none;">
                    <img src="/Images/loading100.gif" />
                </div>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        function OpenObjectFromLi(selectLi) {
            var objectid = selectLi.attr("direobjid");
            var type = selectLi.attr("type");
            if (type != "Directory") {
                objectid = selectLi.attr("objectid");
            }
            OpenObject(type, objectid);
        }
        jQuery(function() {
            LeftContainer = jQuery("#leftContainerFilesMenu");
            RightContainer = jQuery("#rightObject");
            CoverPage();
            LoadDirectory(0);
            jQuery("div.select").live("click", function(event) {
                var _checkBox = jQuery(this).children();
                if (_checkBox.attr("checked")) {
                    _checkBox.removeAttr("checked");
                }
                else {
                    _checkBox.attr("checked", "checked");
                }
                // clear items checked by click
                if (jQuery("div.action> input:checked").length == 1 && _checkBox.attr("checked")) {
                    RightContainer.children().children().removeClass("on");
                }
                var _clickedLi = _checkBox.parents("li");
                if (_checkBox.attr("checked")) {
                    _clickedLi.addClass("on");
                }
                else {
                    _clickedLi.removeClass("on");
                }
                if (_clickedLi.hasClass("on")) {
                    ClickedSelected = _clickedLi;
                }
                else {
                    var selectedLi = jQuery("#rightObject>ul.current>li.on");
                    ClickedSelected = selectedLi.eq(0);
                }
                return false;
            });

            jQuery("#rightObject>ul>li>div.action").live("click", function() {
                var _this = jQuery(this);
                var _currentLi = _this.parent();
                OpenObjectFromLi(_currentLi);
                return false;
            });
            jQuery("#rightObject>ul>li>div.icon").live("click", function() {
                var _this = jQuery(this);
                var _currentLi = _this.parent();
                OpenObjectFromLi(_currentLi);
                return false;
            });
            jQuery("#rightObject>ul>li>div.content").live("click", function() {
                var _this = jQuery(this);
                var _currentLi = _this.parent();
                OpenObjectFromLi(_currentLi);
                return false;
            });
            if (ISCreateDirectoryRoles) {
                jQuery("input.btnfive").show();
            }
            else {
                jQuery("input.btnfive:not(.everyone)").remove();
                jQuery("input.btnfive").show();
            }
        });
    </script>

</asp:Content>
