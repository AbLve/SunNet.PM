/**
* jQuery Validation Extension 
*
* Anthor: Dave
* Date: 2013.12.12
*/

$.extend(true, $.validator, {
    defaults: {
        errorClass: "noticeRed2",
        ignore: ":hidden:not(.validate)",
        errorLabelContainer: ".noticeFaile1"
    },

    classRuleSettings: {
        attrRequired: {
            attrRequired: true
        },
        attrChecked: {
            attrChecked: true
        },
        attrEqualTo: {
            attrEqualTo: true
        },
        attrRegex: {
            attrRegex: true
        },
        attrRegexCustom: {
            attrRegexCustom: true
        },
        attrRemote: {
            attrRemote: true
        },
        attrChecked: {
            attrChecked: true
        }
    },

    messages: {
        //ext
        attrRequired: "{0} is required.",
        attrChecked: "{0} is required.",
        attrEqualTo: "{0} don't match.",
        attrRegex: "{0} is valid.",
        attrRegexCustom: "{0}",
        attrRemote: "{0} is exist.",
        attrChecked: "{0}"
    },

    methods: {
        attrRequired: function (value, element, param) {
            switch (element.nodeName.toLowerCase()) {
                case 'select':
                    var val = $(element).val();
                    return val && val.length > 0;
                default:
                    return $.trim(value).length > 0;
            }
        },
        attrChecked: function (value, element) {
            var $elements = $("input[name=" + element.nodeName + "]");
            var result = false;
            for (var i = 0; i < $elements.length; i++) {
                if ($elements[i].checked) {
                    result = true;
                }
            }
            return result;
        },
        attrEqualTo: function (value, element) {
            var $element = $(element);
            var param = $("#" + $element.attr("equalToID"));
            var target = param.unbind(".validate-equalTo").bind("blur.validate-equalTo", function () {
                $element.valid();
            });
            return value == target.val();
        },
        attrRegex: function (value, element) {
            var elementRegex = $(element).attr("regex");
            var reg = new RegExp(elementRegex);
            return reg.test(value);
        },
        attrRegexCustom: function (value, element) {
            var elementRegex = $(element).attr("regex");
            var reg = new RegExp(elementRegex);
            return reg.test(value);
        },
        attrRemote: function (value, element) {
            var $element = $(element);
            var oldValidValue = $element.attr("oldValidValue");
            var oldValidResult = $element.attr("oldValidResult");
            if (oldValidValue == undefined || oldValidValue != value) {
                var url = $element.attr("remoteUrl");
                var key = element.name || element.id;

                var result = false;
                $.ajax({
                    url: url + "?" + key + "=" + value,
                    type: "post",
                    async: false,
                    success: function (data) {
                        result = data == "false";
                        $element.attr("oldValidValue", value);
                        $element.attr("oldValidResult", result);
                    }
                });
                return result;
            }
            return oldValidResult == "true";
        },
        attrChecked: function (value, element) {
            var checked = $(element).prop("checked");
            return checked;
        }
    }
});
