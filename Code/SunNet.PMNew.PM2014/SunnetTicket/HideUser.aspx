<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="HideUser.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.HideUser" %>

<%@ Import Namespace="SunNet.PMNew.Entity.UserModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/knockout-3.1.0.debug.js"></script>
    <script src="/Scripts/knockout.mapping-latest.debug.js"></script>
    <script type="text/javascript">
        var Roles = {
            PM:<%=(int)RolesEnum.PM%>,
            Leader:<%=(int)RolesEnum.Leader%>,
            DEV:<%=(int)RolesEnum.DEV%>,
            QA:<%=(int)RolesEnum.QA%>
        };
        function UserModel(RoleID, RoleName, UserID, FirstName, LastName, IsHide) {
            this.roleId = RoleID || 0;
            this.roleName = RoleName || "";
            this.userId = UserID || "";
            this.firstName = FirstName || "";
            this.lastName = LastName || "";
            this.isHide = ko.observable(IsHide) || ko.observable(false);
        }

        var currentUserId = '<%=UserInfo.UserID%>';
        function UserViewModel() {
            var self = this;
            this.pmDataset = ko.observableArray([]);
            this.leadDataset = ko.observableArray([]);
            this.devDataset = ko.observableArray([]);
            this.qaDataset = ko.observableArray([]);
            this.currentUserData = ko.observableArray([]);
            this.saveHideUsers = function () {
                var chks = new Array();
                $("input[name='chkPmUser']:not(:checked)").each(function () {
                    chks.push($(this).attr('userid'));
                });
                $("input[name='chkLeaderUser']:not(:checked)").each(function () {
                    chks.push($(this).attr('userid'));
                });
                $("input[name='chkDevUser']:not(:checked)").each(function () {
                    chks.push($(this).attr('userid'));
                });
                $("input[name='chkQaUser']:not(:checked)").each(function () {
                    chks.push($(this).attr('userid'));
                });
                var hideUserIds = chks.join();
                $.post("/Service/User.ashx", { action: "saveHideUser", currentuserid: currentUserId, hideUserIds: hideUserIds }, function (isSuccess) {
                    console.log(isSuccess);
                    if (isSuccess == "1") {
                        parent.userTicketModel.needRefresh(true);
                        parent.closeModal();
                    }
                }, 'json');
            };
            this.selectAll = function (role) {
                var checked = $(arguments[2].target).prop('checked');
                switch (role) {
                    case Roles.PM:
                        self.pmDataset().forEach(function (item, index) { item.isHide(!checked) });
                        $("input[name='chkPmUser']").prop("checked", checked);
                        break;
                    case Roles.Leader:
                        self.leadDataset().forEach(function (item, index) { item.isHide(!checked) });
                        $("input[name='chkLeaderUser']").prop("checked", checked);
                        break;
                    case Roles.DEV:
                        self.devDataset().forEach(function (item, index) { item.isHide(!checked) });
                        $("input[name='chkDevUser']").prop("checked", checked);
                        break;
                    case Roles.QA:
                        self.qaDataset().forEach(function (item, index) { item.isHide(!checked) });
                        $("input[name='chkQaUser']").prop("checked", checked);
                        break;
                };
            };
            this.checkAll = function (user) {
                var role = user.roleId;
                var checked = $(arguments[1].target).prop('checked');
                var selectAllChk;
                var notChecked = 0;
                switch (role) {
                    case Roles.PM:
                        selectAllChk = $("#chkPm");
                        notChecked = $("input[name='chkPmUser']:not(:checked)").length;
                        break;
                    case Roles.Leader:
                        selectAllChk = $("#chkLeader");
                        notChecked = $("input[name='chkLeaderUser']:not(:checked)").length;
                        break;
                    case Roles.DEV:
                        selectAllChk = $("#chkDev");
                        notChecked = $("input[name='chkDevUser']:not(:checked)").length;
                        break;
                    case Roles.QA:
                        selectAllChk = $("#chkQa");
                        notChecked = $("input[name='chkQaUser']:not(:checked)").length;
                        break;
                };
                if (!checked) {
                    selectAllChk.prop('checked', false);
                } else if (notChecked == 0) {
                    selectAllChk.prop('checked', true);
                }
            };
            this.refresh = function () {
                $.getJSON("/Service/User.ashx", { action: "getdashboardusers", currentuserid: currentUserId }, function (data) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].UserID == currentUserId) {
                            continue;
                        } else {
                            switch (data[i].RoleID) {
                                case Roles.PM:
                                    self.pmDataset.push(new UserModel(data[i].RoleID, data[i].RoleName, data[i].UserID, data[i].FirstName, data[i].LastName, data[i].IsHide));
                                    break;
                                case Roles.Leader:
                                    self.leadDataset.push(new UserModel(data[i].RoleID, data[i].RoleName, data[i].UserID, data[i].FirstName, data[i].LastName, data[i].IsHide));
                                    break;
                                case Roles.DEV:
                                    self.devDataset.push(new UserModel(data[i].RoleID, data[i].RoleName, data[i].UserID, data[i].FirstName, data[i].LastName, data[i].IsHide));
                                    break;
                                case Roles.QA:
                                    self.qaDataset.push(new UserModel(data[i].RoleID, data[i].RoleName, data[i].UserID, data[i].FirstName, data[i].LastName, data[i].IsHide));
                                    break;
                            }
                        }
                    }
                    initDisplay();
                });
            };
            self.refresh();
        }
        var userViewModel = null;
        jQuery(function () {
            console.log(1);
            userViewModel = new UserViewModel();
            ko.applyBindings(userViewModel, document.body);
        });
        function initDisplay() {
            if ($("input[name='chkPmUser']:not(:checked)").length == 0) {
                $("#chkPm").prop('checked', true);
            } else {
                $("#chkPm").prop('checked', false);
            }
            if ($("input[name='chkLeaderUser']:not(:checked)").length == 0) {
                $("#chkLeader").prop('checked', true);
            } else {
                $("#chkLeader").prop('checked', false);
            }
            if ($("input[name='chkDevUser']:not(:checked)").length == 0) {
                $("#chkDev").prop('checked', true);
            } else {
                $("#chkDev").prop('checked', false);
            }
            if ($("input[name='chkQaUser']:not(:checked)").length == 0) {
                $("#chkQa").prop('checked', true);
            } else {
                $("#chkQa").prop('checked', false);
            }
            window.top.$("#" + urlParams["parentmodal"]).trigger("loaded.bs.modal", null);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Hide/Display Users
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <div class="form-group" runat="server" id="pmUsers">
        <label class="col-left-owassignuser lefttext">
            <input id="chkPm" type="checkbox" name="chkUser"
                data-bind="event: { change: selectAll.bind($data, Roles.PM) }" />
            PM:
        </label>
        <div class="col-right-owassignuser righttext" id="pMUsers">
            <ul class="assignUser" data-bind="foreach: { data: pmDataset, as: 'user' }">
                <li>
                    <span data-role="1" data-id="1" data-us="1">
                        <input type="checkbox" name="chkPmUser" data-bind="attr: { id: 'chk' + userId, userid: userId, checked: !isHide() }, event: { change: $root.checkAll }" />
                        <label style="padding-left: 0px; padding-right: 0px;" data-bind="attr: { for: 'chk' + userId }, text: firstName + ' ' + lastName ">
                        </label>
                    </span>
                </li>
            </ul>
        </div>
    </div>
    <div class="form-group" id="leaderUsers">
        <label class="col-left-owassignuser lefttext">
            <input id="chkLeader" type="checkbox" name="chkUser"
                data-bind="event: { change: selectAll.bind($data, Roles.Leader) }" />
            Leader:</label>
        <div class="col-right-owassignuser righttext">
            <ul class="assignUser" data-bind="foreach: { data: leadDataset, as: 'user' }">
                <li>
                    <span data-role="1" data-id="1" data-us="1">
                        <input type="checkbox" name="chkLeaderUser" data-bind="attr: { id: 'chk' + userId, userid: userId, checked: !isHide() }, event: { change: $root.checkAll }" />
                        <label style="padding-left: 0px; padding-right: 0px;" data-bind="attr: { for: 'chk' + userId }, text: firstName + ' ' + lastName ">
                        </label>
                    </span>
                </li>
            </ul>
        </div>
    </div>
    <div class="form-group" id="devUsers">
        <label class="col-left-owassignuser lefttext">
            <input id="chkDev" type="checkbox" name="chkUser"
                data-bind="event: { change: selectAll.bind($data, Roles.DEV) }" />
            DEV:</label>
        <div class="col-right-owassignuser righttext">
            <ul class="assignUser" data-bind="foreach: { data: devDataset, as: 'user' }">
                <li>
                    <span data-role="1" data-id="1" data-us="1">
                        <input type="checkbox" name="chkDevUser" data-bind="attr: { id: 'chk' + userId, userid: userId, checked: !isHide() }, event: { change: $root.checkAll }" />
                        <label style="padding-left: 0px; padding-right: 0px;" data-bind="attr: { for: 'chk' + userId }, text: firstName + ' ' + lastName ">
                        </label>
                    </span>
                </li>
            </ul>
        </div>
    </div>
    <div class="form-group" id="qaUsers">
        <label class="col-left-owassignuser lefttext">
            <input id="chkQa" type="checkbox" name="chkUser"
                data-bind="event: { change: selectAll.bind($data, Roles.QA) }" />
            QA:</label>
        <div class="col-right-owassignuser righttext">
            <ul class="assignUser" data-bind="foreach: { data: qaDataset, as: 'user' }">
                <li>
                    <span data-role="1" data-id="1" data-us="1">
                        <input type="checkbox" name="chkQaUser" data-bind="attr: { id: 'chk' + userId, userid: userId, checked: !isHide() }, event: { change: $root.checkAll }" />
                        <label style="padding-left: 0px; padding-right: 0px;" data-bind="attr: { for: 'chk' + userId }, text: firstName + ' ' + lastName ">
                        </label>
                    </span>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <input type="button" id="btnSave" class="saveBtn1 mainbutton" data-bind="click: saveHideUsers" tabindex="19" value="Save" />
    <input id="btnClear" type="button" value="Cancel" class="cancelBtn1 mainbutton" data-dismiss="modal" runat="server" tabindex="20" />
</asp:Content>
