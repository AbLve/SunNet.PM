<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditWorkRequest.aspx.cs"
    MasterPageFile="~/Sunnet/InputPop.Master" Inherits="SunNet.PMNew.Web.Sunnet.WorkRequest.EditWorkRequest" %>

<%@ Register Src="../../UserControls/AddWorkRequest.ascx" TagName="AddWorkRequest"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/WorkRequestRelatedTicketsList.ascx" TagName="RelatedTickets"
    TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="owTopone_left1">
        Work Request Detail</div>
    <link href="/Styles/main.master.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        a:link
        {
            text-decoration: none;
        }
    </style>

    <script src="/do/js.ashx" type="text/javascript"></script>

    <script type="text/javascript">
        // return value
        ISModalPage = false;
        jQuery(function() {

            // tr doubleclickclick 
            jQuery("tr[opentype]").hover(
            function() {
                jQuery(this).addClass("listrowthree");
            }
            ,
            function() {
                jQuery(this).removeClass("listrowthree");
            }
            );

            jQuery("body").on("click", "tr[opentype]", function(event) {
                if (event.target.nodeName != "TD") {
                    return true;
                }
                var _this = jQuery(this);
                var url = _this.attr("href");
                var opentype = _this.attr("opentype");
                if (opentype == "popwindow" && url.length > 0) {
                    var title = _this.attr("dialogtitle");
                    var dialogwidth = _this.attr("dialogwidth");
                    var dialogheight = _this.attr("dialogheight");
                    var limitheight = 600;
                    if (dialogheight > limitheight) {
                        dialogheight = limitheight;
                    }
                    var result = ShowIFrame(url,
                                        dialogwidth,
                                        dialogheight,
                                        true,
                                        title);
                    if (result == 0) {
                        var btnSearch = _this.attr("freshbutton");
                        if (btnSearch != undefined && btnSearch != null & btnSearch.length > 0 && document.getElementById(btnSearch)) {
                            document.getElementById(btnSearch).click();
                        }
                        else {
                            window.location.reload();
                        }
                    }
                }
                else if (opentype == "newtab" && url.length > 0) {
                    window.open(url);
                }
            });
            jQuery('#DivDocuments tr td input:checkBox').on('click', function(event) {
                event.stopPropagation();
            }).closest('td').on('click', function(event) {
                var $checkBox = $(this).find('input[type=checkBox]');
                if ($checkBox.prop('checked')) {
                    $checkBox.prop('checked', false);
                }
                else {
                    $checkBox.prop('checked', true);
                }
                event.stopPropagation();
            });

        });

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
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null)
                return unescape(r[2]);
            return null;
        }

        function OpenAddRelaionDialog() {
            var result = ShowIFrame("/Sunnet/WorkRequest/AddRelation.aspx?wid=" + getUrlParam('id'), 620, 560, true, "Add Relation");
            if (result == 0) {
                window.location.reload();
            }
        }
        function AddDocument() {
            var result = ShowIFrame("/Sunnet/WorkRequest/AddDocument.aspx?wid=" + getUrlParam('id'), 620, 230, true, "Add Document");
            if (result == 0) {
                window.location.reload();
            }
        }

        function DeleteDocument() {
            var checkboxList = "";

            $("#DivDocuments input[type=checkbox]:checked").each(function() {
                var _thistd = $(this);
                checkboxList += _thistd.attr("id") + ",";
            });
            var wid = getUrlParam('id');
            //validate
            checkboxList = $.trim(checkboxList);
            if (checkboxList.length < 1) {
                ShowMessage2('Please select a document to delete. ', 0, true, false);
                return false;
            }

            $.ajax({

                type: "post",

                url: "/Do/DoDeleteWorkRequestDocuments.ashx?r=" + Math.random(),

                data: {
                    checkboxList: checkboxList,
                    wid: wid
                },
                success: function(result) {
                    ShowMessage2(result, 0, true, false);
                }
            });

        };


        function AddNote() {
            var result = ShowIFrame("/Sunnet/WorkRequest/AddNote.aspx?wid=" + getUrlParam('id'), 620, 300, true, "Add Note");
            if (result == 0) {
                window.location.reload();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="owBoxtwo">
        <div runat="server" id="divtopMeunFill" visible="false" class="owtopMenu">
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="owmainBox_left">
                    <div style="width: 195px;">
                    </div>
                    <ul class="owleftmenu" style="position: fixed;">
                        <li class="currentleft"><a href="#Basic">Basic Information</a> </li>
                        <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
                           { %>
                        <li><a href="#RelatedTickets">Related Tickets</a> </li>
                        <% }%>
                        <li><a href="#Documents">Documents</a> </li>
                        <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
                           { %>
                        <li><a href="#Notes">Notes</a> </li>
                        <% }%>
                    </ul>
                </td>
                <td class="owmainBox_right">
                    <div class="owmainrightBoxtwo">
                        <div class="owToptwo">
                            <img src="/icons/19.gif" align="absmiddle" /><a name="Basic">Basic Information</a>
                        </div>
                        <div>
                            <uc1:AddWorkRequest ID="AddWorkRequest1" runat="server" />
                        </div>
                        <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
                           { %>
                        <div>
                            <%--<div style="float: left;" runat="server" id="divAddRelation">
                                <a href="###" onclick="OpenAddRelaionDialog()">
                                    <img src="/icons/28.gif" align="absmiddle" />
                                    Add Related Tickets </a>
                            </div>--%>
                            <uc2:RelatedTickets ID="RelationTicketsList1" runat="server" />
                        </div>
                        <% }%>
                        <div>
                            <div class="owmainactionBox">
                                <div style="float: left; width: 500px; font-size: 12px; font-weight: bold">
                                    <a name="Documents">Documents</a> &nbsp;&nbsp;
                                    <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
                                       { %>
                                    <a href="###" onclick="AddDocument()">
                                        <img src="/icons/28.gif" align="absmiddle" />
                                        Add </a><a href="###" onclick="DeleteDocument(); return false;">
                                            <img src="/icons/17.gif" align="absmiddle" />
                                            Delete</a>
                                    <% }%>
                                </div>
                            </div>
                            <div class="owmainBox">
                                <div class="mainrightBoxtwo" id="DivDocuments">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="owlistone">
                                        <tr class="owlistTitle">
                                            <th style="width: 5%;">
                                            </th>
                                            <th orderby="Title" style="width: 32%;">
                                                Title
                                            </th>
                                            <th style="width: 30%;" orderby="Tags">
                                                Tags
                                            </th>
                                            <th style="width: 33%;" orderby="CreatedOn">
                                                Date
                                            </th>
                                        </tr>
                                        <tr runat="server" id="trNoDocuments" visible="false">
                                            <th colspan="5">
                                                &nbsp;
                                            </th>
                                        </tr>
                                        <asp:Repeater ID="rptDocuments" runat="server">
                                            <ItemTemplate>
                                                <tr opentype="popwindow" dialogwidth="620" dialogheight="230" dialogtitle="" href="/Sunnet/WorkRequest/AddDocument.aspx?id=<%#Eval("FileID") %>&wid="
                                                    class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                                                    <td>
                                                        <input id='<%# Eval("FileID")%>' type="checkbox" />
                                                    </td>
                                                    <td>
                                                        <a style="text-decoration: underline" href='/Do/DoDownloadFileHandler.ashx?FileID=<%#Eval("FileID") %>&size=<%#Eval("FileSize") %>'
                                                            target="_blank">
                                                            <%#Eval("FileTitle")%></a>
                                                    </td>
                                                    <td>
                                                        <%# Eval("Tags").ToString()%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("CreatedOn", "{0:MM/dd/yyyy}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <% if (UserInfo.Role != SunNet.PMNew.Entity.UserModel.RolesEnum.CLIENT)
                           { %>
                        <div>
                            <div class="owmainactionBox">
                                <a name="Notes">Notes</a>&nbsp;&nbsp; <a href="###" onclick="AddNote()">
                                    <img src="/icons/28.gif" />
                                    Click here to enter a new note </a>
                            </div>
                            <div class="owmainBox">
                                <div class="mainrightBoxtwo" id="Div3">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="owlistone">
                                        <asp:Repeater ID="rptNotes" runat="server" OnItemDataBound="rptNote_DataBound">
                                            <ItemTemplate>
                                                <tr class=" <%#Container.ItemIndex % 2 == 0 ? "listrowone" : "listrowtwo"%>">
                                                    <td>
                                                        <strong>Title: </strong>
                                                        <%# Eval("Title") %>
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="95%" align="right">
                                                                    Edited
                                                                    <%# Eval("ModifyOn","{0:MM/dd/yyyy HH:mm}") %>
                                                                    by
                                                                    <asp:Literal ID="ltlCreatedByID" runat="server" Visible="false" Text='<%#Eval("ModifyBy")%>'></asp:Literal>
                                                                    <asp:Literal ID="ltlCreatedByName" runat="server"></asp:Literal>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="95%">
                                                                    <span class="owNotegreytext">
                                                                        <%# Eval("Description") %></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <% }%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
