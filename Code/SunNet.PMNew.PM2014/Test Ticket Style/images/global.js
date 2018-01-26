function FormatNumber(sender, len) {
    var source = sender.value;

    var number = "0123456789.-";

    var output = "";
    for (var i = 0; i < source.length; i++) {
        if (number.indexOf(source.charAt(i)) > -1) {
            output = output + source.charAt(i);
        }
    }

    if (output.length > len) {
        output = output.substr(0, len);
    }

    sender.value = output;
}

//Format inputed number string
//sender:TextBox DOM element
//len: integral part length
//precision: decimal part length
function FormatFloatNumber(sender, len, precision) {
    var source = sender.value;
    var number = "0123456789.";
    var output = "";
    for (var i = 0; i < source.length; i++) {
        if (number.indexOf(source.charAt(i)) > -1) {
            output = output + source.charAt(i);
        }
    }
    if (output.length > len) {
        output = output.substr(0, len);
    }
    var tmp = "";
    if (output.indexOf(".") > 0) {
        if (precision)
            tmp = output.substr(0, output.indexOf(".") + precision + 1);
        else
            tmp = output.substr(0, output.indexOf(".") + 3);
    }
    else
        tmp = output;
    sender.value = tmp;
}

/*
* 2014-5-7,
* 5/7/2014,
* 2014,4,7
*/
function NewDate(year, month, date) {
    var str;
    if (arguments.length == 1)
    { str = year; }
    if (str && str.indexOf("-") > 0) {
        str = str.split('-');
        year = +str[0];
        month = +str[1] - 1;
        date = +str[2];

    } else if (str && str.indexOf("/") > 0) {
        str = str.split('/');
        year = +str[2];
        month = +str[0] - 1;
        date = +str[1];
    }
    var finalDate = new Date();
    //finalDate.setUTCFullYear(year, month, date);
    //finalDate.setUTCHours(0, 0, 0, 0);
    finalDate.setFullYear(year);
    finalDate.setMonth(month);
    finalDate.setDate(date);
    finalDate.setHours(0);
    finalDate.setMinutes(0);
    finalDate.setSeconds(0);
    return finalDate;
};

Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+": this.getHours(), //小时 
        "h+": this.getHours() > 12 ? this.getHours() - 12 : this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
var NewTime = function (date) {
    var items = /(\d{1,2}):?(\d{1,2})\s*((am)|(pm))?/.exec(date);
    this.hour = 0;
    this.minute = 0;
    if (items && items.length >= 4) {
        this.hour = +items[1];
        if (this.hour > 23)
            this.hour = 23;
        this.minute = +items[2];
        if (items[3].toLowerCase() == "pm") {
            this.hour += 12;
        }
    }
}
NewTime.prototype.toString = function () {
    var h = this.hour;
    var aorp = "am";
    if (this.hour > 12) {
        h = this.hour - 12;
        aorp = "pm";
    }
    var m = this.minute;
    if (this.minute > 0 && this.minute < 30) {
        m = 30;
    }
    if (this.minute > 30 && this.minute < 60) {
        h = h + 1;
        m = 0;
    }
    if (m < 10) {
        m = "0" + m;
    }
    return h + ":" + m + aorp;
};
function ShowMessage(msg, level, closeInSeconds) {
    // level:success,info,warning,danger
    // info ,warning 不可用
    if (level !== "success") {
        level = "danger";
    }
    var $msgContainer;
    $msgContainer = $(".mainrightBox div.alert-" + level).add(".loginBoxn1 div.alert-" + level);
    $msgContainer.contents().each(function (index, element) {
        var $this = $(element);
        var keepElement = $this.is("a") || $this.is("img");
        if (!keepElement) {
            $this.remove();
        }
    });
    $msgContainer.append(msg);
    $msgContainer.addClass("in").alert().show();

    if (closeInSeconds && +closeInSeconds > 0) {
        setTimeout(function () { $msgContainer.alert("close"); }, closeInSeconds * 1000);
    }

    window.scrollTo(0, 0);
}

function GetTemplateHtml(templateID) {
    if (templateID in window) {
        return window[templateID].innerHTML;
    }
    return "";
}

function TemplateEngine(html, options) {
    var re = /{%([^%>]+)?%}/g, reExp = /(^( )?(if|for|else|switch|case|break|{|}))(.*)?/g, code = 'var r=[];\n', cursor = 0;
    var add = function (line, js) {
        js ? (code += line.match(reExp) ? line + '\n' : 'r.push(' + line + ');\n') :
            (code += line != '' ? 'r.push("' + line.replace(/"/g, '\\"') + '");\n' : '');
        return add;
    }
    while (match = re.exec(html)) {
        add(html.slice(cursor, match.index))(match[1], true);
        cursor = match.index + match[0].length;
    }
    add(html.substr(cursor, html.length - cursor));
    code += 'return r.join("");';
    return new Function(code.replace(/[\r\t\n]/g, '')).apply(options);
}

function RedirectBack(urlIfNoReturnUrl) {
    var url = urlParams.returnurl;
    if (!url && urlIfNoReturnUrl) {
        url = urlIfNoReturnUrl;
    }
    if (!url) {
        url = "/Default.aspx";
    }
    url = decodeURIComponent(url);
    if (url.indexOf("?") < 0) {
        url += "?";
    } else if (!/[\?|\&]$/.test(url)) {
        url += "&";
    }
    if (!window["objectArgumentName"]) {
        window["objectArgumentName"] = "tid";
    }
    url = url.replace(/isBack=\d{1,10}/g, "");
    url += "isBack=" + urlParams[window["objectArgumentName"]];
    location.href = url;
}

var urlParams;

(window.onpopstate = function () {
    // 处理URL参数
    var match,
        pl = /\+/g,
        search = /([^&=]+)=?([^&]*)/g,
        decode = function (s) {
            return decodeURIComponent(s.replace(pl, " "));
        },
        query = window.location.search.substring(1);
    urlParams = {};

    while (match = search.exec(query)) {
        try {
            urlParams[decode(match[1])] = decode(match[2]);
        } catch (e) {
            console.log(e);
        }
    }
    if (urlParams.returnurl) {
        urlParams.returnurl = encodeURIComponent(urlParams.returnurl);
    }
})();

function appendUrlQueryString(params) {
    var url = location.pathname + "?";
    for (var name in urlParams) {
        url += "&";
        url += name;
        url += "=";
        url += urlParams[name];
    }
    return url;
}

$(function () {
    $.ajaxSetup({
        cache: false
    });
    var $body = $("body");

    // back from last page
    if (urlParams.isBack && !isNaN(+(urlParams.isBack)) && +(urlParams.isBack) > 0) {
        if (!window["objectArgumentName"]) {
            window["objectArgumentName"] = "ticket";
        }
        var $target = $("tr[" + window["objectArgumentName"] + "='" + urlParams.isBack + "']");
        $target.addClass("back");

        if ($target.length == 0) {
            $target = $($("#accordion").find("a[href='#panel_" + window["objectArgumentName"] + urlParams.isBack + "']").attr("href"));
            if ($target.length) {
                $("#accordion").find(".in").collapse('hide');
                $target.collapse('show');
            }
        }
        delete urlParams.isBack;
    }

    if (!urlParams.order || !urlParams.sort) {
        var $defaultOrder = $("th.order[default]");
        urlParams.order = $defaultOrder.attr("orderby");
        urlParams.sort = $defaultOrder.hasClass("order-asc") ? "asc" : "desc";
    }
    $body.on("click", "input:button,input:submit,button", function () {
        $("input[placeholder],textarea[placeholder]").each(function (index, element) {
            var $this = $(element);
            if ($this.val() == $this.attr("placeholder")) {
                $this.val("");
            }
        });
    });
    // URL search
    $body.on("click", "#btnSearch", function () {
        delete urlParams.success;
        var $searchTable = $(this).closest("table");
        var queryString = [];
        $searchTable.find("input[queryparam],select[queryparam]").each(function (index, element) {
            var $search = $(element);
            urlParams[$search.attr("queryparam")] = $search.val();
        });
        urlParams.page = 1;
        location.href = appendUrlQueryString();
    });

    // default order prop
    if (urlParams.order) {
        $("th.order").removeClass("order-asc").removeClass("order-desc");
        $("th.order[orderby='" + urlParams.order + "']").addClass("order-" + (urlParams.sort || "asc"));
    } else {
        urlParams.sort = "asc";
    }

    // order
    $body.on("click", "th.order", function () {
        var $header = $(this);
        if (urlParams.order && urlParams.order == $header.attr("orderby")) {
            urlParams.sort = urlParams.sort == "asc" ? "desc" : "asc";
        } else {
            urlParams.sort = "desc";
        }
        urlParams.order = $header.attr("orderby");
        location.href = appendUrlQueryString();
    });

    // show success message
    if (urlParams.success && isPostBack == "False") {
        $("div.alert-success").addClass("in").alert().show();
        setTimeout(function () {
            $("div.alert-success").alert("close");
        }, 2000);

    }
    $body.on("closed.bs.alert", ".alert", function () {
        if (urlParams.close) {
            ClosePopWindow();
        } else if (urlParams["parentmodal"]) {
            setTimeout(function () {
                window.top.$("#" + urlParams["parentmodal"]).trigger("loaded.bs.modal", null);
            }, 0);
        }
    });
    var maxPopHieght = 0;
    // pop window height
    $body.on("loaded.bs.modal", ".modal", function () {
        var $modal = $(this),
            $iframe = $modal.find("iframe"),
            $alertSuccess = $iframe[0].contentWindow.$("div.alert-success"),
            $alertDanger = $iframe[0].contentWindow.$("div.alert-danger");

        maxPopHieght = $(window).innerHeight()
            - +($modal.find(".modal-dialog").css("margin-top").replace("px", ""))
             - +($modal.find(".modal-dialog").css("padding-top").replace("px", ""))
             - 100;
        var iframeWidth = $iframe[0].contentWindow.$(".mainowBox").outerWidth(),
            iframeHeight = $iframe[0].contentWindow.$("form").outerHeight(),
            iframeHeaderHeight = $iframe[0].contentWindow.$("div.mainowBoxtop").outerHeight() +
            ($alertSuccess.is(":visible") ? $alertSuccess.outerHeight() : 0) +
            ($alertDanger.is(":visible") ? $alertDanger.outerHeight() : 0),
            modalHeight = iframeHeight > maxPopHieght ? maxPopHieght : iframeHeight;
        $modal.find(".modal-content,.modal-dialog")
                    .width(iframeWidth)
                    .height(modalHeight)
                    .find(".modal-body").height(modalHeight);
        if (iframeHeight > maxPopHieght) {
            var $iframeBody = $iframe[0].contentWindow.$(".mainowContent");
            var maxHeight = maxPopHieght - iframeHeaderHeight
                - +($iframeBody.css("margin-top").replace("px", ""))
                - +($iframeBody.css("margin-bottom").replace("px", ""))
                - +($iframeBody.css("padding-top").replace("px", ""))
                - +($iframeBody.css("padding-bottom").replace("px", ""));
            $iframeBody.css("max-height", maxHeight);
        }
    }).on("hidden.bs.modal", ".modal", function () {
        var $this = $(this);
        var $ifame = $this.find("iframe");
        if ($ifame.attr("src") && $ifame[0].contentWindow.urlParams.close) {
            window.location.href = window.location.pathname + window.location.search;
        }
        $ifame.attr("src", "/PopEmpty.aspx");
        return true;
    });

    // ticket : expanded collapsed feedback
    $body.on("click", "td.action a.collapsed,td.action a.expanded", function () {
        var $this = $(this);
        var $tr = $this.closest("tr");
        var ticketid = $tr.attr("ticket");
        var $detail = $("#ticketDetail" + ticketid);
        if ($this.hasClass("collapsed")) {
            if ($detail.length) {
                $detail.show();
            } else {
                var data = {
                    description: GetTemplateHtml("ticket" + ticketid + "Description")
                };
                $.getJSON("/service/Ticket.ashx", {
                    action: "getFeedbacks",
                    ticketID: ticketid,
                    count: 2
                }, function (feedbacks) {
                    data.feedback = feedbacks;
                    var html = TemplateEngine(GetTemplateHtml("ticketExpend"), data);
                    $detail = $(html).attr("id", "ticketDetail" + ticketid).insertAfter($tr);
                }).error(function (xhr, status, errorText) {
                    data.feedback = [
                    {
                        text: "Get feedback error: " + errorText
                    }];
                    var html = TemplateEngine(GetTemplateHtml("ticketExpend"), data);
                    $detail = $(html).attr("id", "ticketDetail" + ticketid).insertAfter($tr);
                });
            }
            if ($this.hasClass("message")) {
                $.getJSON("/service/Ticket.ashx", {
                    action: "clearfeedbackmessage",
                    ticketID: ticketid
                });
            }
        }
        else {
            $detail.hide();
        }
        $tr.find("a.message").removeClass("message").empty();
        $tr.toggleClass("collapsed").toggleClass("expanded").toggleClass("onclick");
        $this.toggleClass("collapsed").toggleClass("expanded");
    });

    $body.on("click", "td.action a.collapsed1,td.action a.expanded1", function () {
        var $this = $(this);
        var $tr = $this.closest("tr");
        var id = $this.attr("timesheet");
        var $detail = $("#Timesheet" + id);
        if ($this.hasClass("collapsed1")) {
            if ($detail.length) {
                $detail.show();
            }
        }
        else {
            $detail.hide();
        }
        $tr.find("a.message").removeClass("message").empty();
        $tr.toggleClass("collapsed1").toggleClass("expanded1").toggleClass("onclick");
        $this.toggleClass("collapsed1").toggleClass("expanded1");
    });

    // redirectback
    $body.on("click", ".redirectback", function () {
        RedirectBack();
    });

    // enter search
    $body.on("keypress", "input:text,select", function (event) {
        if (event.which == 13) {
            if ($("#btnSearch").length) {
                $("#btnSearch").trigger("click");
                event.preventDefault();
                return false;
            }
        }
    });

    // row click function
    $body.on("click", "table.table-advance:not(.noclickbind)>tbody>tr>td:not(.action)", function (event) {
        var $tdClicked = $(this);
        var $tr = $tdClicked.closest("tr");
        var $action = $tr.find(".action:last");
        var $a = $action.find("a[href]:first");
        if ($a.length) {
            var modal = $a.data();
            if (modal && modal.target) {
                $a.trigger("click");
            } else {
                var $spanOpen = $("#spanOpen" + $a.attr("ticketId"));
                var $imageOpen = $("#imageOpen" + $a.attr("ticketId"));
                if ($spanOpen.length)
                    $spanOpen.click();
                else if ($imageOpen.length)
                    $imageOpen.click();
                else
                    window.location.href = $a.attr("href");
            }
            return false;
        }
    });

    // show loading layer when submit
    $body.on("submit", "form", function () {
        setTimeout(function () {
            $(".loading").show();
        }, 500);
    });

    // category
    var $categorycontainer = jQuery("#categorycontainer");
    $categorycontainer.mouseout(function () {
        hideCategoryContainer = setTimeout(function () {
            $categorycontainer.slideUp(100);
        }, 300);
    }).mouseover(function () {
        clearTimeout(hideCategoryContainer);
    });
    var hideCategoryContainer = 0;
    var selectedTicket = 0;
    jQuery("body").on("click", "a[action='calladdtocategory']", function (event) {
        clearTimeout(hideCategoryContainer);
        var $this = jQuery(this);
        selectedTicket = +($(this).attr("ticket"));
        var _thisx = event.pageX - ($categorycontainer.outerWidth() - $this.outerWidth()) / 2;
        var _thisy = event.pageY + 10;

        $categorycontainer.css({
            position: "absolute",
            "top": _thisy,
            "left": _thisx,
            "z-index": 999
        }).show();

    }).on("click", "a[action='addtocategory']", function (event) {
        var $this = jQuery(this);
        if ($this.attr("ticket"))
            selectedTicket = +($(this).attr("ticket"));
        if (selectedTicket > 0) {
            jQuery.post("/Service/CateGory.ashx",
                 {
                     action: "addtocategory",
                     ticketid: selectedTicket,
                     cagetory: $this.attr("category")
                 },
                function (responseData) { },
                "json");
        }
        $categorycontainer.slideUp(100);
    });
    /*category end*/


    /*pager all */
    jQuery("div.pager").find("select:last").find("option:last").text("All");

    //Press up or down change select
    $body.on("keyup", "select", function (e) {
        if (e.keyCode == 38 || e.keyCode == 40) {
            $(this).change();
        }
    });


    // working status
    $body.on("click", '[data-remote="workingon"] a[ticket]', function () {
        var $this = $(this);
        var ticket = $this.attr("ticket");
        var action = $this.data("action");
        var $statusBar = $("[data-workingstatus='" + ticket + "']");
        $.post("/Service/TicketUser.ashx", {
            action: action,
            ticket: ticket
        }, function (response) {
            if (response.success) {
                $statusBar.find("span.text").text($this.text());
            } else {
                ShowMessage(response.msg, "danger");
            }
        }, "json");
    });
});

