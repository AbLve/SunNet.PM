//Remove all leading and trailing white-space characters
String.prototype.Trim = function() {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}



//Remove all trailing white-space characters
String.prototype.RTrim = function() {
    return this.replace(/(\s*$)/g, "");
}

function Trim(source) {
    return source.replace(/(^\s*)|(\s*$)/g, "");
}

function LTrim(source) {
    return source.replace(/(^\s*)/g, "");
}

function RTrim(source) {
    return source.replace(/(\s*$)/g, "");
}



//validate email format
String.prototype.IsEmail = function() {
    var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return reg.test(this);
}

//Return DOM object according to its id
function $(id) {
    var reValue;

    var obj = document.getElementById(id);
    switch (obj.type) {
        case "text":
            reValue = obj.value;
            break;
        case "textarea":
            reValue = obj.value;
            break;
        case "select-one":
            reValue = obj.options[obj.selectedIndex].value;
            break;
        case "checkbox":
            reValue = obj.checked;
            break;
        case "radio":
            reValue = obj.checked;
            break;
        default:
            reValue = "";
            break;
    }

    return reValue;
}

function $T(id) {
    var reValue;
    var obj = document.getElementById(id);
    if (obj.type == "select-one") {
        reValue = obj.options[obj.selectedIndex].text;
    }
    return reValue;
}

function $X(id) {
    var returnValue;
    var obj = document.getElementById(id);
    if (obj.type == "select-one") {
        returnValue = $T(id);
    }
    else {
        returnValue = $(id);
    }
    return returnValue;
}

function $F(id) {
    return document.getElementById(id);
}
//Format & Filter inputed phone string
//return xxx-xxx-xxxx
function FormatPhone(sender) {
    var source = sender.value;

    var number = "0123456789";

    var output = "";
    for (var i = 0; i < source.length; i++) {
        if (number.indexOf(source.charAt(i)) > -1) {
            output = output + source.charAt(i);
            if (output.length == 3 || output.length == 7) {
                output = output + "-";
            }
        }
    }

    while (output.charAt(output.length - 1) == "-") {
        output = output.substr(0, output.length - 1);
    }

    if (output.length > 12) {
        output = output.substr(0, 12);
    }

    sender.value = output;
}

function IsEmail(source) {
    var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return reg.test(source);
}

function IsNumber(source) {
    var reg = /^[\+-]?\d+(\.\d+)?$/;
    return reg.test(source);
}

//Format inputed number string
//sender:TextBox DOM element
//len: number length
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
    var ch = source.charAt(0);
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
    if (output.indexOf(".") > 0)
        output = output + "0000000000";
    else
        output = output + ".0000000000";

    tmp = output.substr(0, output.indexOf(".") + parseInt(precision) + 1);
    if (ch == "+" || ch == "-") {
        tmp = ch + tmp;
    }
    sender.value = tmp;
    TrimEnd(sender, ".");
}

function ValidateDecimal(ctlID, precision, minValue, maxValue, title) {
    var sender = document.getElementById(ctlID);
    var v = sender.value.Trim();
    if (IsNumber(v) == false) {
        alert("Invalid " + title + " value.");

        sender.focus();
        sender.select();
        return false;
    }

    if (parseFloat(v) < parseFloat(minValue) || parseFloat(v) > maxValue) {
        sender.select();
        sender.focus();
        alert("The " + title + " value number is between " + minValue + " and " + maxValue + ".");
        return false;
    }

    FormatFloatNumber(sender, 20, precision);
    FormatFloatNumber(sender, 20, precision);
}

//Removes all trailing occurrences of a set of characters specified 
//in an array from the current String object
function TrimEnd(sender, ch) {
    var source = sender.value;
    if (source.charAt(source.length - 1) == ch)
        sender.value = source.substr(0, source.length - 1);
    else
        sender.value = source;
}

function SetMaxLength(obj, len) {
    var source = obj.value;
    if (source.length > len) {
        obj.value = source.substring(0, len);
        return true;
    }
    return false;
}

function AlertAndFalse(obj, Msg) {
    alert(Msg);
    obj.focus();
    return false;
}

function GetBrowseType() {
    if (window.ActiveXObject)
        return "IE";
    else if (document.getBoxObjectFor)
        return "FF";
    else if (window.MessageEvent && !document.getBoxObjectFor)
        return "CHROME";
    else if (window.opera)
        return "OPERA";
    else if (window.openDatabase)
        return "SAFARI";

    return "";
}

function TextBoxEnter(txtID, buttonID, func) {
    var sender;
    if (txtID != null)
        sender = document.getElementById(txtID)
    else
        sender = document.body;

    sender.onkeypress = function(e) {
        if (event.keyCode == 13) {
            event.keyCode = 9;
            event.returnValue = false;
            if (func != null) {
                if (func() == false) return;
            }

            if (GetBrowseType() == "FF") {
                var tmpEvent = document.createEvent("MouseEvents");
                tmpEvent.initEvent("click", true, true);
                document.getElementById(buttonID).dispatchEvent(tmpEvent);
            }
            else {
                document.getElementById(buttonID).click();
            }
        }
    }
}

function DefaultFocus(ctlID) {
    document.getElementById(ctlID).focus();
}

function openWin(url, w, h) {
    if (w == null || h == null) {
        w = 900;
        h = 600;
    }
    var top = (window.screen.availHeight - 30 - h) / 2;
    var left = (window.screen.availWidth - 10 - w) / 2;

    var t = new Date().getTime().toString();
    if (url.indexOf("?") > -1) {
        url = url + "&t=" + t;
    }
    else {
        url = url + "?t=" + t;
    }
    window.open(url, "", "height=" + h + ",width=" + w + ",top=" + top + ",left=" + left + ",toolbar=no,menubar=no,scrollbars=yes,resizable=yes,location=no,status=yes");
}

function openWinModel(url, args, w, h) {
    if (w == null || h == null) {
        w = 900;
        h = 600;
    }
    if (args == null) {
        args = window;
    }

    var top = (window.screen.availHeight - 30 - h) / 2;
    var left = (window.screen.availWidth - 10 - w) / 2;
    var t = new Date().getTime().toString();
    if (url.indexOf("?") > -1) {
        url = url + "&t=" + t;
    }
    else {
        url = url + "?t=" + t;
    }
    var ret = window.showModalDialog(url, args, "dialogHeight:" + h + "px;dialogWidth:" + w + "px;dialogTop:" + top + "px;dialogLeft:" + left + "px;toolbar:no;menubar:no;scrollbars:no;resizable:yes;location:no;status:no;");
    return ret;
}

function EventX(e) {
    if (!e) e = window.Event;
    var x = e.pageX || (e.clientX ? e.clientX + document.documentElement.scrollLeft : 0);

    return x;
}

function EventY(e) {
    if (!e) e = window.Event;
    var y = e.pageY || (e.clientY ? e.clientY + document.documentElement.scrollTop : 0);

    return y;
}

function IsLatitude(source) {
    var reg = /^-?([1-8]?[1-9]|[1-9]0)\.{1}\d{1,6}/;
    return reg.test(source);
}

function IsLongitude(source) {
    var reg = /^-?([1]?[1-7][1-9]|[1]?[1-8][0]|[1-9]?[0-9])\.{1}\d{1,6}/;
    return reg.test(source);
}

function checknum(source) {
    var reg = /^[\+-]?\d+(\.\d+)?$/;

    return reg.test(source);
}

String.prototype.startwith = function(str) {
    if (str == null || str == "" || this.length == 0 || str.length > this.length)
        return false;
    if (this.substr(0, str.length) == str)
        return true;
    else
        return false;
    return true;
}

function RequestQuery(url) {
    var currentUrl = window.location.href;
    var reg = new RegExp("(^|&)" + url + "=([^&]*)(&|$)");
    var r = currentUrl.substr(currentUrl.indexOf("\?") + 1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}
function Replace(source, chr, rep) {
    source = "" + source;
    var s = "";
    if (source.length == 0) return "";
    for (var i = 0; i < source.length; i++) {
        if (source.charAt(i) == chr) {
            s = s + rep;
            continue;
        }
        s = s + source.charAt(i);
    }

    return s;
}

function ShowOrHide(objName, self) {
    if ($F(objName).style.display == 'none') {
        $F(objName).style.display = '';
        self.src = '/App_Themes/Default/images/Icon5.gif';
    }
    else {
        $F(objName).style.display = 'none';
        self.src = '/App_Themes/Default/images/Icon4.gif';
    }

}

function contains(string, substr, isIgnoreCase) {
    if (isIgnoreCase) {
        string = string.toLowerCase();
        substr = substr.toLowerCase();
    }
    var startChar = substr.substring(0, 1);
    var strLen = substr.length;
    for (var j = 0; j < string.length - strLen + 1; j++) {
        if (string.charAt(j) == startChar) {
            if (string.substring(j, j + strLen) == substr) {
                return true;
            }
        }
    }
    return false;
}


function checkNumber(e) {
    var key = window.event ? e.keyCode : e.which;
    var keychar = String.fromCharCode(key);
    reg = /\d/;
    var result = reg.test(keychar);
    if (!result) {
        return false;
    }
    else {
        return true;
    }
}


var oldForm;
$(function() {
    oldForm = $('form').serialize();
});
$(window).on('beforeunload', function() {
    if ($('#form').serialize() != oldForm) {
        return '';
    }
});