<%@ Page Title="Update Schedule Date" Language="C#" MasterPageFile="~/Sunnet/InputPop.Master" AutoEventWireup="true"
    CodeBehind="UpdateDate.aspx.cs" Inherits="SunNet.PMNew.Web.Sunnet.Tickets.UpdateDate" %>

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

    <script type="text/javascript">
        //get url para
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        $(document).ready(function() {

            //clear
            $("#btnSave").click(function() {

                var start = $("#<%=txtStartDate.ClientID%>").val();
                var end = $("#<%=txtEndDate.ClientID%>").val();
                $.ajax({

                    type: "post",

                    url: "/Do/DoUpdateDate.ashx?r=" + Math.random(),

                    data: {
                        tid: getUrlParam('tid'),
                        start: start,
                        end: end
                    },
                    success: function(result) {
                        ShowMessage(result, 0, true, true);
                    }
                });


            });

            //clear
            $("#btnClear").click(function() {

                $("#<%=txtStartDate.ClientID%>").val("");
                $("#<%=txtEndDate.ClientID%>").val("");

            });
        });      
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTitle" runat="server">
    <div class="opendivTopone_left">
        Update Schedule Date</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="opendivBox1">
        <table width="90%" border="0" align="center" cellpadding="5" cellspacing="0" class="opendivTable">
            <tr>
                <th width="28%">
                    Schedule Start Date:
                </th>
                <td>
                    <asp:TextBox ID="txtStartDate" Validation="true" length="8-20" RegType="date" CssClass="input180"
                        onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);' runat="server"> </asp:TextBox>&nbsp;
                    <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtStartDate.ClientID %>"),document.getElementById("<%=txtStartDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                        align="absmiddle">
                </td>
            </tr>
            <tr>
                <th>
                    Schedule Due Date:
                </th>
                <td>
                    <asp:TextBox ID="txtEndDate" Validation="true" length="8-20" RegType="date" CssClass="input180"
                        onfocus='popUpCalendar(this,this, "mm/dd/yyyy", 0, 0);' runat="server"> </asp:TextBox>&nbsp;
                    <img src="/icons/30.gif" onclick='javascript:popUpCalendar(document.getElementById("<%=txtEndDate.ClientID %>"),document.getElementById("<%=txtEndDate.ClientID %>"), "mm/dd/yyyy", 0, 0);'
                        align="absmiddle">
                </td>
            </tr>
        </table>
        <div class="btnBoxone">
            <input id="btnSave" name="btnSave" type="button" class="btnfive" value="Update">
            <input id="btnClear" name="btnCancle" type="button" class="btnfive" value="Clear">
        </div>
    </div>
</asp:Content>
