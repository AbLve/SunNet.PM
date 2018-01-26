<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pop.master" CodeBehind="View.aspx.cs" Inherits="SunNet.PMNew.PM2014.Event.View" %>

<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    View Event
</asp:Content>
<asp:Content ID="body" ContentPlaceHolderID="bodySection" runat="server">
    <div class="form-group">
        <label class="col-left-newevent-view lefttext">Project:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:Literal runat="server" ID="ltrlProject"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent-view lefttext">Name:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:Image runat="server" ID="imgIcon" ImageAlign="AbsMiddle" />
            <asp:Literal runat="server" ID="ltrlName"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent-view lefttext">Where:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:Literal runat="server" ID="ltrlWhere"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent-view lefttext">Detail:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:Literal runat="server" ID="ltrlDetail"></asp:Literal>
        </div>
    </div>

    <div class="form-group">
        <label class="col-left-newevent-view lefttext">All-day:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:CheckBox runat="server" ID="chkAllDay" />
        </div>
    </div>
    <div class="form-group" id="div_off" runat="server">
        <label class="col-left-newevent-view lefttext">Off:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:CheckBox ID="chkOff" runat="server" Enabled="false" />
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent-view lefttext">From:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:Literal runat="server" ID="ltrlFrom"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent-view lefttext">To:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:Literal runat="server" ID="ltrlTo"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent-view lefttext">Alert:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:Literal runat="server" ID="ltrlAlert"></asp:Literal>
        </div>
    </div>
    <div class="form-group">
        <label class="col-left-newevent-view lefttext">Created By:</label>
        <div class="col-right-newevent-view righttext-view">
            <asp:Literal runat="server" ID="litCreated"></asp:Literal>
        </div>
    </div>

    <div class="form-group">
        <label class="col-left-newevent-view lefttext">Attendees:</label>
        <div class="col-right-newevent-view righttext-view">
            <ul class="righttext-view-name">
                <asp:Repeater ID="rptInvite" runat="server">
                    <ItemTemplate>
                        <li><%# Eval(UserNameDisplayProp) %></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>

        </div>
    </div>
</asp:Content>
