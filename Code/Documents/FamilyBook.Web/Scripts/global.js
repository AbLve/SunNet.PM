Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(),    //day 
        "h+": this.getHours(),   //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter 
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length)); for (var k in o) if (new RegExp("(" + k + ")").test(format)) format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

$(function () {
    //Show color when enter data.
    $(".ipt").blur(function () {
        $(this).removeClass("inputon");
    }).focus(function () {
        $(this).addClass("inputon");
    });

    //focus on first element
    var visible_form = $("form:visible");
    if (visible_form && visible_form.length) {
        var first_form = visible_form[0];
        if (first_form.length) {
            $(first_form[0]).focus();
        }
    }
    /*
    $("input:text.counter,textarea.counter").each(function (e) {
    var element = $(this);
    var length = element.attr("maxlength");
    if (length && !isNaN(parseInt(length))) {
    element.after("<label for='" + (this.id ? this.id : this.name) + "' style='font-size:14px; color: #999; font-family: Arial,Helvetica,sans-serif; font-size: 12px; margin-left: 1px;'>" + this.value.length + "/" + length + "</label>");
    }
    });
    $("body").on("keyup", "input:text.counter,textarea.counter", function (e) {
    var length = $(this).attr("maxlength");
    if (length && !isNaN(parseInt(length))) {
    $(this).next("label[for='" + (this.id ? this.id : this.name) + "']").text(this.value.length + "/" + length);
    }
    });
    */
    jQuery(document).on("focus", "input:text.counter,textarea.counter", function () {
        var $element = $(this);
        if (!$element.data("counter_event")) {
            var length = $element.attr("maxlength");
            if (length && !isNaN(parseInt(length))) {
                $element.after("<label for='" + (this.id ? this.id : this.name) + "' countfor='" + (this.id ? this.id : this.name) + "' style='font-size:14px; color: #999; font-family: Arial,Helvetica,sans-serif; font-size: 12px; margin-left: 1px;'>" + this.value.length + "/" + length + "</label>");
            }
            $element.data("counter_event", true);
        }
        $element.nextAll("label[countfor='" + (this.id ? this.id : this.name) + "']").text(this.value.length + "/" + $element.attr("maxlength"));
    });
    $(document).on("keyup", "input:text.counter,textarea.counter", function (e) {
        var length = $(this).attr("maxlength");
        if (length && !isNaN(parseInt(length))) {
            $(this).next("label[countfor='" + (this.id ? this.id : this.name) + "']").text(this.value.length + "/" + length);
        }
    });

    //confirm input
    $("body").on("keyup", "input:text[confirm],textarea[confirm]", function (e) {
        var element = $(this);
        var confirm = element.attr("confirm");
        if (element.val() == confirm && !(e.ctrlKey && e.which == 90)) {
            element.data("inputEqual", true);
        } else {
            element.data("inputEqual", false);
        }
    });
    $("body").on("focus", "input:text[confirm],textarea[confirm]", function (e) {
        var element = $(this);
        var confirm = element.attr("confirm");
        if (!element.data("inputEqual")) {
            if (element.hasClass("input-confirm") && element.val() != confirm) {
                element.removeClass("input-confirm");
            }
            if (element.hasClass("input-confirm") || element.val() == element.attr("confirm")) {
                element.removeClass("input-confirm").val("");
            }
        }
    });
    $("body").on("blur", "input:text[confirm],textarea[confirm]", function (e) {
        var element = $(this);
        if (element.val() == "" && !element.data("inputEqual")) {
            element.addClass("input-confirm").val(element.attr("confirm"));
        }
    });
    $("input:text[confirm],textarea[confirm]").each(function (e) {
        var element = $(this);
        var confirm = element.attr("confirm");
        if (!element.val().length || (element.val().length && element.val() == confirm)) {
            element.addClass("input-confirm").val(confirm);
            element.data("inputEqual", false);
        }
    });

    //Press up or down change select
    $("body").on("keyup", "select", function (e) {
        if (e.keyCode == 38 || e.keyCode == 40) {
            $(this).change();
        }
    });

    //https font
    WebFontConfig = {
        google: { families: ['Open+Sans::latin'] }
    };
    (function () {
        var wf = document.createElement('script');
        wf.src = ('https:' == document.location.protocol ? 'https' : 'http') +
          '://ajax.googleapis.com/ajax/libs/webfont/1/webfont.js';
        wf.type = 'text/javascript';
        wf.async = 'true';
        var s = document.getElementsByTagName('script')[0];
        s.parentNode.insertBefore(wf, s);
    })();
});

//Close modals by id.
function CloseModal(id) {
    //Close modal.
    jQuery("#" + id).modal("hide");
}

//Show or hide result panel.
function SubmitResult(formID, submitResult, url) {
    var time = 2000;
    $(".ipt").removeClass("inputon");
    if (submitResult) {
        $("#" + formID + " .noticeSuccess").show('slow');
        setTimeout(function () { $("#" + formID + " .noticeSuccess").hide('slow'); if (url) { location = url; } }, time);
    } else {
        $("#" + formID + " .noticeFaile").show('slow');
        setTimeout(function () { $("#" + formID + " .noticeFaile").hide('slow'); }, time);
    }
}
function StringBuilder() {
    this.data = Array('');
}
StringBuilder.prototype.append = function () {
    this.data.push(arguments[0]);
    return this;
}
StringBuilder.prototype.toString = function () {
    return this.data.join('');
}