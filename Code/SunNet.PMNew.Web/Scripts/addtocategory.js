var divChooseCategory;
var currentTicketClicked = 0;
var closeCategoriesDiv = null;
function chooseCategoryDiv(event, o) {
    if (divChooseCategory == null)
        divChooseCategory = jQuery("#addtocategory");
    var _this = jQuery(o)
    var _thisx = event.pageX + 10;
    var _thisy = event.pageY;
    divChooseCategory.css("margin-top", _thisy).css("margin-left", _thisx).css("z-index", 999).show();
    currentTicketClicked = _this.attr("ticketid");
}
function addtoCategory(o) {
    jQuery.ajax({
        type: "POST",
        url: "/Do/CateGory.ashx?r=" + Math.random(),
        data: {
            type: "addtocategory",
            ticketid: currentTicketClicked,
            cagetoryid: jQuery(o).attr("categoryid")
        },
        success: function (responseData) {
            if (responseData == true || responseData == "true") {
                divChooseCategory.slideUp(100);
                //                CoverAndAlert("Operation successful.",0, 3000);
            }
            else {
                //                CoverAndAlert("Operation fail.",2, 3000);
            }
        }
    });
}
// Collected ticket
// CollectedTicketsDirectory comes from Main.Master line 516
function CollecteTicket(ticketid) {
    jQuery.getJSON(
        "/Do/Directory.ashx?r=" + Math.random(),
        {
            type: "UpdateObject",
            objectid: ticketid,
            directoryid: CollectedTicketsDirectory,
            objecttype: 'Ticket'
        },
        function (responseData) {
            var level = 0;
            if (responseData.Success == false) {
                level = 2;
            }
            ShowMessage(responseData.MessageContent, level, false, false);
        });
}
jQuery(function () {
    divChooseCategory = jQuery("#addtocategory");
    jQuery("a[action='removefromcategory']").click(function () {
        jQuery.ajax({
            type: "POST",
            url: "/Do/CateGory.ashx?r=" + Math.random(),
            data: {
                type: "removefromcategory",
                ticketid: jQuery(this).attr("ticketid"),
                cagetoryid: CategoryID
            },
            success: function (responseData) {
                if (responseData == true || responseData == "true") {
                    window.location.reload();
                }
            }
        });
    });

    divChooseCategory.mouseover(function () {
        clearTimeout(closeCategoriesDiv);
    }).mouseleave(function () {
        closeCategoriesDiv = setTimeout(function () { divChooseCategory.hide(); }, 500);
    });
});