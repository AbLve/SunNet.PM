<%@ Page Title="Move to " Language="C#" MasterPageFile="~/Sunnet/InputPop.Master"
    AutoEventWireup="true" CodeBehind="MoveObjects.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Documents.MoveObjects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #leftContainerFilesMenu
        {
            border: solid 2px #80BEF1;
            padding-top: 10px;
            border-radius: 5px;
            width: 100%;
            min-height: 200px;
        }
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
        #leftContainerFilesMenu div
        {
            height: 20px;
            cursor: pointer; /*position:relative;*/
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
            min-height: 25px;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    Move To:Please select a directory
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="leftContainerFilesMenu" class="owmainrightBoxtwo">
    </div>
    <asp:HiddenField ID="hidSelectedDirectory" runat="server" />
    <div class="btnBoxone">
        <asp:Button ID="btnSave" CssClass="btnone" ValidationGroup="Add" CausesValidation="true"
            runat="server" Text="Save" OnClick="btnSave_Click" />
    </div>

    <script type="text/javascript">
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
                        LeftContainer.append(html);
                        LoadDirectory(1, 1);
                    }
                    else {
                        _leftContainer.append(html);
                    }
                    GetDataTimes = GetDataTimes + 1;
                    ToggleLeftMenu(directory);
                    if (lockDirectory) {
                        setTimeout(function() { ToggleLeftMenu(lockDirectory); }, 50);
                    }
                });
        }
        function LoadDirectory(directory, lockDirectory) {
            LoadLeftMenu(directory, lockDirectory);
        }
        var GetDataTimes = 0;
        jQuery(function() {
            LeftContainer = jQuery("#leftContainerFilesMenu");
            LoadDirectory(0, 1);
            var hidSelected = jQuery("#<%=hidSelectedDirectory.ClientID %>");
            jQuery("#<%=btnSave.ClientID %>").click(function() {
                var selectedDires = jQuery("#leftContainerFilesMenu li.currentdirectory");
                if (selectedDires.length > 0) {
                    ValueToReturnOnClose = selectedDires.eq(0).attr("objectid");
                    hidSelected.val(ValueToReturnOnClose);
                }
                else {
                    ShowMessage("Please select a directory", 0, false, false);
                    return false;
                }
                return true;
            });
        });
    </script>

</asp:Content>
