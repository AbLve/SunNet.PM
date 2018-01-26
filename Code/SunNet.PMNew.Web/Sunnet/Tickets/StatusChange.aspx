<%@ Page Title="" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="StatusChange.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.StatusChange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .customWidth
        {
            width: 520px;
        }
        .opendivBox1
        {
            height: 200px;
            width: 518px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="opendivTopone_left">
        Reason</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="opendivBox1">
        <table width="90%" border="0" align="center" cellpadding="5" cellspacing="0" class="opendivTable">
            <tr>
                <td align="center" class="opendivText1">
                    Change Status to Testing Failed
                </td>
            </tr>
            <tr>
                <th>
                    Please specify your reason:
                </th>
            </tr>
            <tr>
                <td>
                    <textarea name="textarea2" rows="4" class="input98p"></textarea>
                </td>
            </tr>
        </table>
        <div class="btnBoxone">
            <input id="btnSave" name="btnSave" type="button" class="btnfive" value="Update">
            <input id="btnClear" name="btnCancle" type="button" class="btnfive" value="Clear">
        </div>
    </div>
</asp:Content>
