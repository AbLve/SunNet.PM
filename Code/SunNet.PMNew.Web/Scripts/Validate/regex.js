var Validator =
{
    chinese: { pattern: "[\u4e00-\u9fa5]", sample: "abcABC132" },
    doubleByte: { pattern: "[^\x00-\xff]", sample: "abcABC132" },

    username: { pattern: "^[a-zA-Z][a-zA-Z0-9_]{4,19}$", sample: "abcABC132" },
    password: { pattern: "^[a-zA-Z]\\w{4,20}$", sample: "abcABC132" },
    email: { pattern: "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\.\\w+([-.]\\w+)*", sample: "admin@sunnet.us" },
    url: { pattern: "[a-zA-z]+://[^\s]*", sample: "http://www.google.com" },
    number: { pattern: "^[0-9]*$", sample: "789" },
    date: { pattern: "(\\d{2}(-|\/)\\d{2}(-|\/)\\d{4})|\\d{4}(-|\/)\\d{2}(-|\/)\\d{2}", sample: "12/31/2012" },
    phone: { pattern: "(\(\\d{3,4}\)|\\d{3,4}-)?((\\d{7,8})|(\\d{3}-\\d{4}))", sample: "123-456-7890" },
    mobile: { pattern: "(\\d{11})|(\\d{1}-\\d{3}-\\d{3}-\\d{4})", sample: "1-234-567-8901" },
    CheckChinese: function(value) {
        var _reg = new RegExp(this.chinese.pattern);
        return _reg.text(value);
    },
    CheckDoubleByte: function(value) {
        var _reg = new RegExp(this.doubleByte.pattern);
        return _reg.text(value);
    },
    CheckInput: function(value, regType) {
        var _validatorReg = eval("this." + regType);
        var _reg = new RegExp(_validatorReg.pattern);
        value = value.replace(" ", "");
        if (value == undefined || value == null || value.length <= 0) {
            return true;
        }
        if (_reg.test(value)) {
            return true;
        }
        return false;
    }
}
/*chinesePhone: "(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}",chineseMobile: "\d{11}",*/