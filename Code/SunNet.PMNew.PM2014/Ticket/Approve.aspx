<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Approve.aspx.cs" MasterPageFile="~/Pop.master" Inherits="SunNet.PMNew.PM2014.Ticket.Approve" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .mainowContent {
            height: 160px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#dvStar img").on("mouseover", function () {
                var score = getCurrentScore(this);
                for (var i = 1; i <= score; i++) {
                    $("#dvStar #star" + i).attr("src", "/Images/icons/star_orange.png");
                }
                for (; i <= 5; i++) {
                    $("#dvStar #star" + i).attr("src", "/Images/icons/star_grey.png");
                }
                setRank(score);
            });

            $("#dvStar img").on("click", function () {
                var score = getCurrentScore(this);
                $("#<%=hdStar.ClientID%>").val(score);
            });

            $("#dvStar img").on("mouseout", function () {
                var clickedScore = $("#<%=hdStar.ClientID%>").val();
                if (clickedScore === "") {
                    for (var i = 1; i <= 5; i++) {
                        $("#dvStar #star" + i).attr("src", "/Images/icons/star_grey.png");
                    }
                    setRank(1);
                }
                else {
                    for (var i = 2; i <= clickedScore; i++) {
                        $("#dvStar #star" + i).attr("src", "/Images/icons/star_orange.png");
                    }
                    for (; i <= 5; i++) {
                        $("#dvStar #star" + i).attr("src", "/Images/icons/star_grey.png");
                    }
                    setRank(clickedScore);
                }

            });
        });


        function sucessCall(parentReloadUrl) {
           layer.msg(
                '<span style="color:#eee;">Operation successful.</span>',
                {
                    time: 5000,
                    shade: [0.3],
                    shadeClose:false,
                    scrollbar: false,
                    offset: '82px'
                },
                function() {
                    window.top.location.href = parentReloadUrl;
                }
            );
            //另一种方法是利用jQuery.sunnet.js方法,注意样式
            //jQuery.alertCustom("success", "Operation successful.", 5, parentReloadUrl);
        }

        function getCurrentScore(elem) {
            return $(elem).attr("id").match(/star([0-9]{1})$/)[1];
        }

        function setRank(score) {
            var rank = null;
            switch (score) {
                case "1":
                    rank = "Poor";
                    break;
                case "2":
                    rank = "Fair";
                    break;
                case "3":
                    rank = "Average";
                    break;
                case "4":
                    rank = "Good";
                    break;
                case "5":
                    rank = "Excellent";
                    break;
                default: rank = "";
                    break;
            }
            $("#lblRank").text(rank);
        }

        function showRating() {
            jQuery('.mainowContent').css("height", 253);
            jQuery("#div_rate").css("display", "none");
            jQuery("#div_Ratings").css("display", "");
            jQuery("#div_Feedback").css("display", "");
        }

        function beforeSubmit() {
            if ($("#<%=helpID.ClientID%>").val() != "1") {
                $("#<%=helpID.ClientID%>").val("1");
                $("#<%=btnSubmit.ClientID%>").click();
                return true;
            }
            if ($("#<%=helpID.ClientID%>").val() == "1") {
                return true;
            }
            return false;
        }

        function beforeSubmitWithConfirm() {
            if ($("#<%=helpID.ClientID%>").val()!="1") {
                jQuery.confirm("Are you sure you want to approve this ticket?", {
                yesText: "Confirm",
                removeWaring:true,
                yesCallback: function () {
                    $("#<%=helpID.ClientID%>").val("1");
                    $("#<%=btnSubmit.ClientID%>").click();
                    return true;
                },
                noText: "Cancel",
                noCallback:function() {
                    return false;
                }
              });
            }
            if ($("#<%=helpID.ClientID%>").val() == "1") {
                return true;
            }
            return false;
        }
    </script>
    
    <style type="text/css">
        #dvStar img {
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="titleSection" runat="server">
    Approve ticket
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodySection" runat="server">
    <div style="font-size: 13px; font-weight: bold; padding-bottom: 20px; color:#333;">
       <asp:Literal runat="server" ID="litHead"></asp:Literal>
    </div>
    <asp:HiddenField runat="server" ID="helpID"/>

    <div class="form-group" id="div_rate">
        
        <div class="col-right-approve left">
            <a class="collapsedtext" onclick="showRating()" href="javascript:void(0);">Rate this ticket</a>
        </div>
    </div>

    <div class="form-group" id="div_Ratings" style="display:none;">
        <label class="col-left-fdapprove lefttext">Ratings:</label>
        <div class="col-right-fdapprove righttext" id="dvStar">
            <img src="/Images/icons/star_grey.png" id="star1">
            <img src="/Images/icons/star_grey.png" id="star2">
            <img src="/Images/icons/star_grey.png" id="star3">
            <img src="/Images/icons/star_grey.png" id="star4">
            <img src="/Images/icons/star_grey.png" id="star5">
            <label id="lblRank"></label>
            <asp:HiddenField ID="hdStar" runat="server" />
        </div>
    </div>

    <div class="form-group" id="div_Feedback" style="display:none;">
        <label class="col-left-fdapprove lefttext">Feedback:</label>
        <div class="col-right-fdapprove righttext">
            <asp:TextBox TextMode="MultiLine" runat="server" ID="txtFeedback" Rows="3" class="inputw4">
            </asp:TextBox>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttonSection" runat="server">
    <asp:Button ID="btnSubmit" runat="server" CssClass="saveBtn1 mainbutton" Text="Submit" OnClientClick="return beforeSubmit();" OnClick="btnSubmit_Click" />
    <input name="btnCancel" type="button" data-dismiss="modal" aria-hidden="true" class="cancelBtn1 mainbutton" value="Cancel" />
</asp:Content>
