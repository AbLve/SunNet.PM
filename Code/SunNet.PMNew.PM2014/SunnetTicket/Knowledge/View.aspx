<%@ Page Title="" Language="C#" MasterPageFile="~/Pop.master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="SunNet.PMNew.PM2014.SunnetTicket.Knowledge.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        label.col-md-1,.col-md-1{ width: 60px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    View Knowledge Share Ticket - <%= Current.TicketID %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
     <div class="form-group">
        <label class="col-md-1 lefttext">Ticket:</label>
        <div class="col-md-3 righttext">
            <div class="ticket-header">
                <%=Ticket.ID %>,&nbsp;<%=Ticket.Title %>
            </div>
        </div>
    </div>
    <div class="form-group">
        <label class="col-md-1 lefttext">Note:<span class="noticeRed">*</span></label>
        <div class="col-md-3 righttext">
            <% =Current.Note %>
        </div>
    </div>
    <div class="form-group">
        <label class="col-md-1 lefttext">Type:<span class="noticeRed">*</span></label>
        <div class="col-md-3 righttext">
            <% =Current.TypeEntity.Title %>
        </div>
    </div>
    <div class="form-group DOMNodeInserted">
        <label class="col-md-1 lefttext">File:</label>
        <div class="col-md-3 righttext">
            <asp:Repeater ID="rptFiles" runat="server" OnItemDataBound="rptFiles_ItemDataBound">
                <ItemTemplate>
                    <asp:HiddenField ID="hidFileTemplate" Value="<a href='/do/DoDownloadFileHandler.ashx?FileID={FileID}' title='Download' target='_blank'>{FileTitle}</a>" runat="server" />
                    <asp:Literal ID="ltlFiles" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <input name="Input322" type="button" class="backBtn mainbutton" data-dismiss="modal" aria-hidden="true" value="Close">
</asp:Content>
