<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsersView.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.Sunnet.UsersView" %>

    <div class="form-group" runat="server" id="dvUS">
        <label class="col-left-owassignuser lefttext" >US:</label>
        <div class="col-right-owassignuser righttext" id="divUSUsers">
            <ul class="assignUser" id="ulUS" runat="server">
            </ul>
        </div>
    </div>
    <div class="form-group" id="devUsers">
        <label class="col-left-owassignuser lefttext">DEV:</label>
        <div class="col-right-owassignuser righttext">
            <ul class="assignUser" id="ulDev" runat="server">
            </ul>
        </div>
    </div>
    <div class="form-group" id="qsUsers">
        <label class="col-left-owassignuser lefttext">QA:</label>
        <div class="col-right-owassignuser righttext">
            <ul class="assignUser" id="ulQA" runat="server">
            </ul>
        </div>
    </div>
