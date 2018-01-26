﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddObjective.aspx.cs" Inherits="SunNet.PMNew.PM2014.Admin.KPI.AddObjective" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<<title></title>
     <link href="/Content/styles/bootstrap_1.css" rel="stylesheet"/>
    <link href="/Content/styles/public.css" rel="stylesheet"/>
    <link href="/Content/styles/forms.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="form-group">
        <label class="col-left-profile lefttext">Goal<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtGoalName" CssClass="inputProfle1 required" MaxLength="50" runat="server"></asp:TextBox>
            </div>
    </div>
        <div class="form-group">
        <label class="col-left-profile lefttext">Description<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtDescription" CssClass="inputProfle1 required" MaxLength="50" runat="server"></asp:TextBox>
            </div>
    </div>
    <div class="form-group">
        <label class="col-left-profile lefttext">Percentage<span class="noticeRed">*</span></label>
            <div class="col-right-profile1 righttext">
                <asp:TextBox ID="txtPercentage" CssClass="inputProfle1 required" MaxLength="50" runat="server"></asp:TextBox>
            </div>
    </div>
    <div class="form-group">
        <label class="col-left-profile lefttext">Status:</label>
            <div class="col-right-profile1 righttext">
              <asp:DropDownList ID="ddlCategoryStatus" CssClass="selectProfle1" runat="server">
                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
               </asp:DropDownList>
            </div>


    </div>
    <div class="buttonBox3">        
        
        <input name="btnCancel" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
    </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" OnSelecting="SqlDataSource1_Selecting"></asp:SqlDataSource>
    </form>
</body>
</html>