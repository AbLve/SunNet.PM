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
                b = document.getElementsByTagName("body")[0];
            } catch (d) {
                c = true;
            }
            if (j)
                try {
                    document.documentElement.doScroll("left");
                } catch (e) {
                    c = true;
                }
            if (c)
                y.push(a);
            else {
                for (; y.length;) {
                    var f =
                        y.shift();
                    c = document.createElement("p");
                    c.appendChild(document.createTextNode(f));
                    b.appendChild(c);
                }
                c = document.createElement("p");
                c.appendChild(document.createTextNode(a));
                b.appendChild(c);
            }
        };
        if (j)
            var ea = window.setInterval(function () {
                if (document.readyState == "complete") {
                    for (; y.length;) {
                        var a = y.shift();
                        p = document.createElement("p");
                        p.appendChild(document.createTextNode(a));
                        document.body.appendChild(p);
                    }
                    window.clearTimeout(ea);
                }
            }, 50);
    } else
        console.log = function () {
        };
}

function getMessageContainer() {
    var $container = jQuery("#messageContainer");
    if (!$container.length) {
        var html = "<div id='messageContainer' style='display: none; position: fixed; z-index: 2000; top: 90px;'></div>";
        $container = $(html).appendTo("body");
    }
    return $container;
}

(function ($) {
    $.support.placeholder = (function () { return 'placeholder' in document.createElement('input'); })();
})(jQuery);
jQuery.extend({
    alert: function (type, message, autoclose, buttons) {
        if (typeof (autoclose) == "object") {
            buttons = autoclose;
            autoclose = 0;
        }
        var container = getMessageContainer().empty();

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
    },
    alertCustom: function (type, message, autoclose, parentReloadUrl, buttons) {
        if (typeof (autoclose) == "object") {
            buttons = autoclose;
            autoclose = 0;
        }
        var container = getMessageContainer().empty();

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
        var alert = jQuery('<div class="alert alert-dismissable hide" style=" filter: Alpha(opacity=9);-moz-opacity:0.9;opacity:0.9;">' +
    '<button type="button" class="close" data-dismiss="alert" data-autoclose="3" aria-hidden="true">&nbsp;</button>' +
    '<h3><strong>' + title[type] + '!</strong></h3><p>' + message + '</p></div>').addClass("alert-" + type);
        container.append(alert.removeClass("hide"));
        container.css("left", (document.body.clientWidth - container.width()) / 2 + "px").show();
        alert.slideDown(100);
        if (autoclose) {
            setTimeout(function () {
                alert.alert("close");
                if (parentReloadUrl) {
                    window.top.location.href = parentReloadUrl;
                }
            }, autoclose * 1000);
        }
    },
    confirm: function (message, buttons) {
        var container = getMessageContainer().empty();
        var confirm = jQuery('<div class="alert alert-danger  alert-dismissable hide">' +
        '<h3><strong>Warning!</strong></h3> <p>' + message +
        '</p></div>');
        if (!buttons.length && buttons.removeWaring) {
            confirm = jQuery('<div class="alert alert-danger  alert-dismissable hide">' +
        '<p>' + message +
        '</p></div>');
        }
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
//jQuery(document).on("keypress", function (event) {
//    if (event.which == 13) {
//        var e = jQuery.Event("click");
//        jQuery("form:visible:last:not('[nokeypressevent]')").find("input:button,input:submit,button").filter(":not([class*='cancel']):visible:last").trigger(e);
//    }
//});

// ReSharper disable once WrongExpressionStatement
+function ($) {
    "use strict";
    var AutoComplete = function (element, options) {
        var self = this;
        this.$element = $(element);
        this.options = options;
        this.$hidden = $(options.hidden);
        this.hideEvent = 0;
        this.showEvent = 0;
        this.isShow = false;
        this.items = [];
        this.$container = jQuery("<div style='position: absolute;z-index:999999;' data-autocomplete='true' class='autocomplete'></div>").appendTo("body");
        this.$container.on("click", "li", function () {
            //console.log("click", this);
            var text = $(this).text();
            var value = self.options.input == "text" ? $(this).text() : $(this).attr("value");
            self.$element.val(text);
            self.$hidden.val(value);
            self.hide();
            return false;
        }).on("scroll", function () {
            //console.log("scroll", this);
            clearTimeout(self.hideEvent);
        }).on("mouseover", function () {
            clearTimeout(self.hideEvent);
        }).on("mouseleave", function () {
            self.hideEvent = setTimeout(function () {
                self.hide();
            }, 500);
        });
    }
    AutoComplete.prototype.hide = function () {
        this.$container.hide();
        this.isShow = false;
    };
    AutoComplete.prototype.getPosition = function () {
        var rect = this.$element[0].getBoundingClientRect();
        return {
            x: rect.left + parseInt(this.$element.css("margin-left").replace("px", "")),
            y: rect.top + this.$element.outerHeight() + $(document).scrollTop()
        };
    }
    AutoComplete.prototype.getQueryParams = function () {
        var params = {};
        params[this.options.paramname] = this.$element.val();
        var queryParams = this.options.params.split(',');
        var $parent = $(this.options.parent);
        if (!$parent.length) $parent = this.closest("form");
        if (!$parent.length) $parent = $("body");
        for (var i = 0; i < queryParams.length; i++) {
            var paraInput = $parent.find(queryParams[i]);
            params[paraInput.attr("data-queryParamName") || paraInput.attr("name")] = paraInput.val();
        }
        return params;
    }
    AutoComplete.prototype.getItemsFromControl = function () {
        var items = [];
        var keyword = this.$element.val();
        if (this.options.options) {
            var arr = this.options.options.split(",");
            for (var i = 0; i < arr.length; i++) {
                var tmp = {};
                tmp[this.options.text] = arr[i];
                tmp[this.options.value] = i;
                if (this.options.filter === false) {
                    items.push(tmp);
                }
                else if (this.options.filter === true && arr[i].indexOf(keyword) >= 0) {
                    items.push(tmp);
                }
                else if ($.isFunction(window[this.options.filter])) {
                    if (window[this.options.filter].call(window, tmp)) {
                        items.push(tmp);
                    }
                }
            }
        }
        return items;
    }
    AutoComplete.prototype.fillData = function (datas) {
        var position = this.getPosition();
        var template = "<ul>{% for(var i=0;i<this.length;i++){ %}<li  data-autocomplete='true'  value='{% this[i]['" + this.options.value + "'] %}'>{% " +
            "this[i]['" + this.options.text + "'].trim() %}</li>{% } %}</ul>";
        if (!datas) datas = [];
        var html = TemplateEngine(template, datas);
        var width = this.options.width || this.$element.width();
        this.$container.empty();
        if (datas.length && html && html.length > 10) {
            this.$container.html(html)
                .css("top", position.y)
                .css("left", position.x)
                .width(width);
            if (this.options.height) {
                this.$container.height(this.options.height);
            }
            if (this.options.itemwidth) {
                this.$container.find("li").width(this.options.itemwidth);

            }
            this.$container.show();
        } 
    }
    AutoComplete.prototype.selectItem = function (item) {
        var self = this;
        if (!self.$element.data("optionsArray")) {
            if (self.$element.data("options")){
                self.$element.data("optionsArray", self.$element.data("options").split(","));
            }
        }
        var selected = item || $.inArray(self.$element.val(), self.$element.data("optionsArray"));
        var index = self.$container.find("li").removeClass("active").filter("li[value='" + selected + "']").addClass("active").index();
        self.$container.scrollTop(self.$container.find("li").outerHeight() * index);
    }
    AutoComplete.prototype.getItems = function (reload) {
        var self = this;
        if (this.options.options) {
            self.fillData(self.getItemsFromControl());
        } else if (reload) {
            $.getJSON(this.options.remote, this.getQueryParams(), function(items) {
                self.fillData(items);
                self.selectItem();
            });
        } else {
            self.$container.show();
        }
        self.selectItem();
    }
    AutoComplete.DEFAULTS = {
        templete: "",
        text: "text",
        value: "value",
        hidden: "",
        input: "text",
        remote: '',
        paramname: "keyword",
        params: "",
        parent: "body",
        delay: 200,
        options: "",
        filter: false
    }
    $.fn.autoComplete = function (option) {
        return this.each(function () {
            var $this = $(this);
            var data = $this.data('sunnet.autoComplete');
            var options = $.extend({}, AutoComplete.DEFAULTS, $this.data(), typeof option == 'object' && option);
            if (!data) {
                $this.data("sunnet.autoComplete", data = new AutoComplete($this, options));
                data.getItems();
            }
            else if (typeof option == "object") {
                $.extend(data.options, option);
            }
            else if (typeof option == "string") {
                data[option]();
            }
        });
    }
    $(document).on("click", "input:text[data-autocomplete='true']", function () {
        $(this).autoComplete("getItems");
        return false;
    }).on("keyup", "input:text[data-autocomplete='true']", function () {
        var self = $(this).data("sunnet.autoComplete");
        self.getItems(true);
        self.$hidden.val("");
    }).on("focus", "input:text[data-autocomplete='true']", function () {
        $(this)[0].select();
        var self = $(this).data("sunnet.autoComplete");
        if (!self) {
            $(this).autoComplete();
            self = $(this).data("sunnet.autoComplete");
        }
        self.getItems(true);
    }).on("blur", "input:text[data-autocomplete='true']", function () {
        //console.log("blur", this);
        var self = $(this).data("sunnet.autoComplete");
        self.hideEvent = setTimeout(function () {
            self.hide();
        }, 800);
    }).on("mouseover", "input:text[data-autocomplete='true']", function () {
        var self = $(this).data("sunnet.autoComplete");
        if (self) {
            clearTimeout(self.hideEvent);
        }
    }).on("mouseleave", "input:text[data-autocomplete='true']", function () {
        var self = $(this).data("sunnet.autoComplete");
        if (self) {
            self.hideEvent = setTimeout(function () {
                self.hide();
            }, 500);
        }
    });

}(jQuery);


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

+(function ($) {
    function CalendarDate(year, month, date, iscurrentmonth, options) {
        this.year = year;
        this.month = month + 1;
        this.date = date;
        this.isCurrentMonth = iscurrentmonth;
        this.dateObj = NewDate(this.month + "/" + this.date + "/" + this.year);
        this.options = options;
    }
    CalendarDate.prototype.toString = function (format) {
        return this.dateObj.Format(format);
    }
    CalendarDate.prototype.getHtml = function () {
        return "<li class='" + (this.isCurrentMonth ? this.options.thisMonthDayClass : this.options.notThisMonthClass) + " " +
            (this.toString("yyyy-MM-dd") == this.options.defaultDate ? this.options.todayClass : " ") +
            "' data-date='" + this.month + "/" + this.date + "/" + this.year + "'>" + this.date + "</li>";
    }

    var Calendar = function (element, options) {
        this.$element = $(element);
        this.options = options;
        this.year = 0;
        this.month = 0;
        this.date = 0;
        this.days = [];
    };
    Calendar.prototype.init = function () {
        var date;
        if (this.options.defaultDate)
            date = NewDate(this.options.defaultDate);
        else
            date = new Date();
        this.year = date.getFullYear();
        this.month = date.getMonth();
        this.date = date.getDate();

        this.refresh();
        this.bindEvents();
    };
    Calendar.prototype.refresh = function () {
        this.days = [];

        this.fillPrevMonthDays();
        this.fillThisMonthDays();
        this.fillNextMonthDays();

        this.fillCalendar();
    }
    Calendar.prototype.formatMonth = function () {
        return Calendar.monthMap[this.options.monthFormat][this.month];
    }
    Calendar.prototype.getLastDayOfMonth = function (year, month) {
        year = year || this.year;
        month = month || this.month;
        if (month >= 11) {
            year++;
            month = 0;
        } else {
            month++;
        }
        var lastDate = NewDate(year, month, 0);
        return lastDate;
    }
    Calendar.prototype.getHeader = function () {
        return "<table width='100%' border='0' cellspacing='0' cellpadding='0' class='tmcalendar'>" +
             "<tr>" +
                 "<td width='16'>&nbsp;</td>" +
                 "<td width='9' class='prevmonth'>" +
                 this.options.prevMonth +
                 "</td>" +
                 "<td align='center'>" + this.formatMonth() + "  " + this.year + "</td>" +
                 "<td width='9' class='nextmonth'>" + this.options.nextMonth +
                 "</td>" +
                 "<td width='16'>&nbsp;</td>" +
             "</tr>" +
             "</table>";
    };
    Calendar.prototype.getWeekHtml = function () {
        return "<div><ul class='" + this.options.weekClass + "'><li>Sun</li><li>Mon</li><li>Tue</li><li>Wed</li><li>Thu</li><li>Fri</li><li>Sat</li></ul></div>";
    }
    Calendar.prototype.fillPrevMonthDays = function () {
        var firstDate = NewDate(this.year, this.month, 1);
        var prevMonthDays = firstDate.getDay();
        if (this.options.firstDayMonday) {
            prevMonthDays = prevMonthDays == 0 ? 7 : prevMonthDays;
        }
        for (var i = 0; i < prevMonthDays; i++) {
            var tempDay = NewDate(this.year, this.month, 1);
            tempDay.setDate(i + 1 - prevMonthDays);
            this.days.push(new CalendarDate(tempDay.getFullYear(), tempDay.getMonth(), tempDay.getDate(), false, this.options));
        }
    }
    Calendar.prototype.fillThisMonthDays = function () {
        var lastDate = this.getLastDayOfMonth();
        for (var i = 1; i <= lastDate.getDate() ; i++) {
            this.days.push(new CalendarDate(this.year, this.month, i, true, this.options));
        }
    }
    Calendar.prototype.fillNextMonthDays = function () {
        var lastDate = this.getLastDayOfMonth();
        var nextMonthDays = 6 - lastDate.getDay();
        for (var i = 1; i <= nextMonthDays; i++) {
            var tempDay = this.getLastDayOfMonth();
            tempDay.setDate(tempDay.getDate() + i);
            this.days.push(new CalendarDate(tempDay.getFullYear(), tempDay.getMonth(), tempDay.getDate(), false, this.options));
        }
    }
    Calendar.prototype.fillCalendar = function () {
        this.$element.empty();
        this.$element.append(this.getHeader()).append(this.getWeekHtml());
        var htmls = "<div><ul class='" + this.options.daysClass + "'>";
        for (var i = 0; i < this.days.length; i++) {
            htmls += this.days[i].getHtml();
        }
        this.$element.append(htmls);
    };
    Calendar.prototype.bindEvents = function () {
        if (this.eventsBinded) return false;
        var self = this;
        this.$element.on("click", ".prevmonth", function () {
            if (self.month == 0) {
                self.year--;
                self.month = 11;
            } else {
                self.month--;
            }
            self.refresh();
        });
        this.$element.on("click", ".nextmonth", function () {
            if (self.month == 11) {
                self.year++;
                self.month = 0;
            } else {
                self.month++;
            }
            self.refresh();
        });
        this.$element.on("click", "." + this.options.thisMonthDayClass, function () {
            var $day = $(this);
            self.$element.find("." + self.options.todayClass).removeClass(self.options.todayClass);
            $day.addClass(self.options.todayClass);
            var dateObj = NewDate($day.data("date"));
            self.date = dateObj.getDate();
            var e = $.Event('choosen.sunnet.calendar', {
                date: dateObj
            });
            self.$element.trigger(e);
        });
        this.eventsBinded = true;
    }
    Calendar.monthMap = {
        number: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
        "short": ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
        full: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
    };
    Calendar.weekMap = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    Calendar.Defaults = {
        dayTemplate: "",
        headerClass: "",
        weekClass: "",
        daysClass: "",
        thisMonthDayClass: "",
        notThisMonthClass: "",
        todayClass: "",
        monthFormat: "short",
        firstDayMonday: false,
        prevMonth: "<",
        nextMonth: ">",
        defaultDate: null
    }
    $.fn.calendar = function (option) {
        var $this = $(this);
        var data = $this.data('sunnet.calendar');
        var options = $.extend({}, Calendar.Defaults, typeof option == 'object' && option);
        if (!data) {
            data = new Calendar(this, options);
            data.init();
            $this.data('sunnet.calendar', data);
        }
    }
})(jQuery);