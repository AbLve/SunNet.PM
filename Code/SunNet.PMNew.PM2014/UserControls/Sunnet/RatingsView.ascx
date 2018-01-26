<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RatingsView.ascx.cs" Inherits="SunNet.PMNew.PM2014.UserControls.Ticket.Sunnet.ClientRatingView" %>
<div class="contentTitle titleeventlist">Ratings</div>
<div class="form-group">
    <div class="col-right-fdapprove righttext" id="dvStar">
        <img src="/Images/icons/star_grey.png" id="star1">
        <img src="/Images/icons/star_grey.png" id="star2">
        <img src="/Images/icons/star_grey.png" id="star3">
        <img src="/Images/icons/star_grey.png" id="star4">
        <img src="/Images/icons/star_grey.png" id="star5">
        <label id="lblRank"></label>
    </div>
</div>
<script type="text/ecmascript">
    var currentscore = "<%=Score%>";
    if ($.trim(currentscore) !== "0") {
        setStar(currentscore);
    }

    function setStar(currentScore) {
        if (currentScore === "") {
            for (var i = 1; i <= 5; i++) {
                $("#dvStar #star" + i).attr("src", "/Images/icons/star_grey.png");
            }
            setRank(1);
        }
        else {
            for (var i = 1; i <= currentScore; i++) {
                $("#dvStar #star" + i).attr("src", "/Images/icons/star_orange.png");
            }
            for (; i <= 5; i++) {
                $("#dvStar #star" + i).attr("src", "/Images/icons/star_grey.png");
            }
            setRank(currentScore);
        }
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
</script>
