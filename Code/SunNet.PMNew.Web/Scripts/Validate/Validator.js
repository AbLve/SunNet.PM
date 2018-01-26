


var validatorResult = true;
var messages = "";
function Validate() {
    validatorResult = true;
    messages = "";
    jQuery("input:text,input:password").each(function() {
        var _this = jQuery(this);
        var _check = _this.attr("Validation");
        var _tempResult = true;
        if (_check == true || _check == "true") {
            var _value = _this.val();
            var _title = _this.parents("td").prev().text();
            if (_title) {
                _title = _title.replace("*", "");
                _title = _title.replace(":", "");
                _title = "[" + _title + "] ";
            }
            var _validatorTitle = "";
            if (_this.attr("ValidatorTitle") && (_this.attr("ValidatorTitle").length > 0)) {
                _validatorTitle = _this.attr("ValidatorTitle") + "<br/>";
            }
            var _regType = _this.attr("RegType");
            var _validator = eval("Validator." + _regType);
            var _msg = "";
            if (_this.attr("length") != undefined && _this.attr("length").length > 3) {
                var lens = _this.attr("length").split("-");
                var min = parseInt(lens[0]);
                var max = parseInt(lens[1]);

                if (_value.length < min || _value.length > max) {
                    _msg = _title + " length must between " + min.toString() + " and " + max.toString();
                    if (_validator && _validator.sample) {
                        _msg = _msg + "<br/>";
                        _msg = _msg + _title + " 's format:" + _validator.sample;
                    }
                    _msg = _msg + "<br/>";
                    _tempResult = false;
                }
                else if (_regType != undefined && _regType.length > 0) {
                    if (Validator.CheckInput(_value, _regType) == false) {
                        _tempResult = false;
                        _msg = _title + " format wrong!  Sample:" + _validator.sample;
                        _msg = _msg + "<br/>";
                    }
                }
            }
            else if (_regType != undefined && _regType.length > 0) {
                if (Validator.CheckInput(_value, _regType) == false) {
                    _tempResult = false;
                    _msg = _title + " format wrong!  Sample:" + _validator.sample;
                    _msg = _msg + "<br/>";
                }
            }
            if (!_tempResult) {
                validatorResult = false;
                if (_validatorTitle.length > 0) {
                    messages = messages + _validatorTitle;
                }
                else {
                    messages = messages + _msg;
                }
            }
        }
    });
    if (validatorResult == false) {
        ShowMessage(messages, 2, false, false);
    }
    return validatorResult;
}