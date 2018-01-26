jQuery.ajaxSetup({
    cache: false
});
if (typeof console == "undefined" || !console.log) {
    var y = [];
    var j = true;
    window.console = {};
    if (true) {
        console.log = function (a) {
            var b = null, c = false;
            try {
                b = document.getElementsByTagName("body")[0]
            } catch (d) {
                c = true
            }
            if (j)
                try {
                    document.documentElement.doScroll("left")
                } catch (e) {
                    c = true
                }
            if (c)
                y.push(a);
            else {
                for (; y.length;) {
                    var f =
                        y.shift();
                    c = document.createElement("p");
                    c.appendChild(document.createTextNode(f));
                    b.appendChild(c)
                }
                c = document.createElement("p");
                c.appendChild(document.createTextNode(a));
                b.appendChild(c)
            }
        };
        if (j)
            var ea = window.setInterval(function () {
                if (document.readyState == "complete") {
                    for (; y.length;) {
                        var a = y.shift();
                        p = document.createElement("p");
                        p.appendChild(document.createTextNode(a));
                        document.body.appendChild(p)
                    }
                    window.clearTimeout(ea)
                }
            }, 50)
    } else
        console.log = function () {
        }
}
jQuery.extend({
    alert: function (type, message, autoclose) {
        var container = jQuery("#messageContainer").empty();

        var title = {
            success: "OK",
            info: "Info",
            warning: "Warning",
            danger: "Error"
        };
        var autocloseOption = {
            success: 3,
            info: 3,
            warning: 4,
            danger: 5
        };
        autoclose = autoclose || autocloseOption[type];
        /*alert-success alert-info alert-warning alert-danger*/
        var alert = jQuery('<div class="alert alert-dismissable hide">' +
    '<button type="button" class="close" data-dismiss="alert" data-autoclose="3" aria-hidden="true">&nbsp;</button>' +
    '<h3><strong>' + title[type] + '!</strong></h3><p>' + message + '</p></div>').addClass("alert-" + type);
        container.append(alert.removeClass("hide"));
        container.css("left", (document.body.clientWidth - container.width()) / 2 + "px").show();
            alert.slideDown(100);
            if (autoclose) {
                setTimeout(function () { alert.alert("close"); }, autoclose * 1000);
            }
        //if (autoclose == undefined)
        //    autoclose = 4;
        //var $msgContainer;
        //$msgContainer = $("div.alert-" + type);
        //$msgContainer.contents().last().remove(); 
        //$msgContainer.html(message);
        //$msgContainer.addClass("in").removeClass("hide").alert().show();
        //if (autoclose) {
        //    setTimeout(function () { $("div.alert-"+type).hide(); }, autoclose * 1000);
        //}
    },
    confirm: function (message, buttons) {
        var container = jQuery("#messageContainer").empty();
        var confirm = jQuery('<div class="alert alert-danger  alert-dismissable hide">' +
        '<h3><strong>Warning!</strong></h3> <p>' + message +
        '</p></div>');
        container.append(confirm.removeClass("hide"));
        container.css("left", (document.body.clientWidth - container.width()) / 2 + "px").show();
        var $pbtns = jQuery("<p style='text-align: center;padding-top: 10px;'></p>");
        if (buttons.length) {
            for (var i = 0; i < buttons.length; i++) {
                var button = buttons[i];
                jQuery('<button type="button" class="btn">' + button.text + '</button>').addClass(button.className).data("button", button).click(function () {
                    setTimeout(function () { confirm.alert("close"); }, 10);
                    var $this = jQuery(this);
                    var fun = $this.data("button");
                    if (jQuery.isFunction(fun.callback)) { fun.callback(); }
                }).appendTo($pbtns);
                $pbtns.append("&nbsp;&nbsp;");
            }
        }
        else {
            jQuery('<button type="button" class="btn btn-danger">' + buttons.yesText + '</button>').data("button", buttons).click(function () {
                setTimeout(function () { confirm.alert("close"); }, 10);
                var $this = jQuery(this);
                var fun = $this.data("button");
                if (jQuery.isFunction(fun.yesCallback)) { fun.yesCallback(); }
            }).appendTo($pbtns);
            $pbtns.append("&nbsp;&nbsp;");
            jQuery('<button type="button" class="btn btn-default">' + buttons.noText + '</button>').data("button", buttons).click(function () {
                setTimeout(function () { confirm.alert("close"); }, 10);
                var $this = jQuery(this);
                var fun = $this.data("button");
                if (jQuery.isFunction(fun.noCallback)) { fun.noCallback(); }
            }).appendTo($pbtns);
        }
        $pbtns.appendTo(confirm);
        confirm.slideDown(100);
    }
});
jQuery.extend(jQuery.fn, {
    serializeJson: function () {
        var dataArrs = this.serializeArray();
        var data = {};
        for (var i = 0; i < dataArrs.length; i++) {
            data[dataArrs[i].name] = dataArrs[i].value;
        }
        return data;
    }
});
jQuery(document).on("keypress", function (event) {
    if (event.which == 13) {
        var e = jQuery.Event("click");
        jQuery("form:visible:last:not('[nokeypressevent]')").find("input:button,input:submit,button").filter(":not([class*='cancel']):visible:last").trigger(e);
    }
});

(function ($) {
    $.support.placeholder = (function () { return 'placeholder' in document.createElement('input'); })();
})(jQuery);

+(function ($) {
    var PlaceHolder = function (element) {
        this.$element = $(element);
        if (!$.support.placeholder) {
            if (this.$element.attr("placeholder")) {
                this.updateFocusOut();
                this.$element.on("focus", $.proxy(this.updateFocusIn, this));
                this.$element.on("blur", $.proxy(this.updateFocusOut, this));
            }
        }
    }
    PlaceHolder.prototype.updateFocusIn = function () {
        if (!this.$element.val() || this.$element.val() == this.$element.attr("placeholder")) {
            this.$element.val("");
        }
        this.$element.removeClass("placeholder");
    }
    PlaceHolder.prototype.updateFocusOut = function () {
        if (!this.$element.val()) {
            this.$element.val(this.$element.attr("placeholder"));
            this.$element.addClass("placeholder");
        }
    }
    $.fn.placeholder = function () {
        return this.each(function () {
            var $input = $(this);
            var pl = new PlaceHolder($input);
            $input.data("sunnet.placeholder", pl);
        });
    };
    $(function () {
        $("input[placeholder],textarea[placeholder]").placeholder();
    });
})(jQuery);