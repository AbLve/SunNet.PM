/*
ASP.NET Comet JavaScript
*/

//
//  A AspNetComet object that represents a client
//  subscribed to a comet channel on the ASP.NET Server
//
//  handler:        The Path to the handler
//  privateToken:   The Client's private token
//  alias:          An alias for the channel
//
function AspNetComet(handler, privateToken, alias) {
    this.handler = handler;
    this.successHandlers = new Array();
    this.failureHandlers = new Array();
    this.timeoutHandlers = new Array();
    this.lastMessageId = 0;
    this.privateToken = privateToken;
    this.alias = alias;
    this.enabled = true;
}

//
//  get an instance of the XML HTTP Request object
//  that is browser specific
//
AspNetComet.prototype.getXMLHttpRequest =
function AspNetComet_getXMLHttpRequest() {
    if (window.XMLHttpRequest) {
        return new XMLHttpRequest()
    }
    else {
        if (window.ActiveXObject) {
            // ...otherwise, use the ActiveX control for IE5.x and IE6
            return new ActiveXObject("Microsoft.XMLHTTP");
        }
    }
}

//
//  Add a success handler, called when the comet call succeeds with a message
//
//  func:   The function that will be called
//
AspNetComet.prototype.addSuccessHandler =
function AspNetComet_addSuccessHandler(func) {
    this.successHandlers[this.successHandlers.length] = func;
}

//
//  Add a failure handler, called when the comet call fails
//
//  func:   The function that will be called
//
AspNetComet.prototype.addFailureHandler =
function AspNetComet_addFailureHandler(func) {
    this.failureHandlers[this.failureHandlers.length] = func;
}

//
//  Add a timeout handler, called when the comet connection returns with no messages
//
//  func:   The function that will be called
//
AspNetComet.prototype.addTimeoutHandler =
function AspNetComet_addTimeoutHandler(func) {
    this.timeoutHandlers[this.timeoutHandlers.length] = func;
}

//
//  Call all the sucess handlers
//
//  privateToken:   The private token of the client
//  alias:          The alias of the channel
//  message:        The message received from the channel
//
AspNetComet.prototype.callSuccessHandlers =
function AspNetComet_callSuccessHandlers(privateToken, alias, message) {
    for (var i = 0; i < this.successHandlers.length; i++) {
        this.successHandlers[i](privateToken, alias, message);
    }
}

//
//  Call all the failure handlers
//
//  privateToken:   The private token of the client
//  alias:          The alias of the channel
//  error:          The error message received from the server
//
AspNetComet.prototype.callFailureHandlers =
function AspNetComet_callFailureHandlers(privateToken, alias, error) {
    for (var i = 0; i < this.failureHandlers.length; i++) {
        this.failureHandlers[i](privateToken, alias, error);
    }
}

//
//  Call all the timeout handlers
//
//  privateToken:   The private token of the client
//  alias:          The alias of the channel
//
AspNetComet.prototype.callTimeoutHandlers =
function AspNetComet_callTimeoutHandlers(privateToken, alias) {
    for (var i = 0; i < this.timeoutHandlers.length; i++) {
        this.timeoutHandlers[i](privateToken, alias);
    }
}

//
//  unsubscribe from the channel (basically stop the request connecting to the channel after it returns)
//
AspNetComet.prototype.unsubscribe =
function AspNetComet_unsubscribe() {
    this.enabled = false;
}

//
//  subscribe to the channel, and start the comet mechanism
//  
AspNetComet.prototype.subscribe =
function AspNetComet_subscribe() {
    var aspNetComet = this;

    //  get our object that is going to perform the request
    var waitRequest = this.getXMLHttpRequest();

    //  indicate we are enabled
    this.enabled = true;

    waitRequest.onreadystatechange = function () {
        //
        //  validate the ready state, we are looking for "4" ready
        if (waitRequest.readyState == "4") {
            //  and a status code of 200 "OK"
            if (waitRequest.status == "200") {
                //  finished, success or not?
                var result;
                if (waitRequest.responseText == "")
                    result = null;
                else
                    result = JSON.parse(waitRequest.responseText);

                if (result == null || result.length == 0) {
                    //  failure
                    aspNetComet.callFailureHandlers(aspNetComet.privateToken, aspNetComet.alias, null);
                }
                else {
                    //  we have a message but we need to inspect
                    //  to see if this is a failure
                    var message = result[0];


                    switch (message.name) {
                        case "aspNetComet.error":
                            //  yes we do this is a failure
                            //  failure
                            aspNetComet.callFailureHandlers(aspNetComet.privateToken, aspNetComet.alias, message);
                            break;
                        case "aspNetComet.timeout":
                            //  its a timeout, so lets continue with this
                            aspNetComet.callTimeoutHandlers(aspNetComet.privateToken, aspNetComet.alias);
                            //  and attach back to the handler
                            if (aspNetComet.enabled) {
                                //  continue if we are enabled
                                aspNetComet.subscribe();
                            }
                            break;
                        default:
                            //  else, lets go for it, iterate through the
                            //  returned messages and call the success handlers
                            for (var i = 0; i < result.length; i++) {
                                var message = result[i];
                                //  get the last messageId
                                aspNetComet.lastMessageId = message.mid;
                                //  and now lets call the success handler
                                aspNetComet.callSuccessHandlers(aspNetComet.privateToken, aspNetComet.alias, message);
                            }

                            //  attach back up to the handler
                            if (aspNetComet.enabled) {
                                //  continue!
                                aspNetComet.subscribe();
                            }
                            break;
                    }
                }
            }
        }
    }

    //
    //  open the post request to the handler
    waitRequest.open("GET", this.handler + "?privateToken=" + this.privateToken + "&lastMessageId=" + this.lastMessageId + "&t=" + Math.random(), true);
    //  and set the request header indicating we are posting form data
    waitRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    //  setup the private token and last message id, these are needed to identify what state we
    //  are actually interested in
    waitRequest.send();
}

if (!this.JSON) {

    // Create a JSON object only if one does not already exist. We create the
    // object in a closure to avoid global variables.

    JSON = function () {

        function f(n) {    // Format integers to have at least two digits.
            return n < 10 ? '0' + n : n;
        }

        Date.prototype.toJSON = function () {

            // Eventually, this method will be based on the date.toISOString method.

            return this.getUTCFullYear() + '-' +
                 f(this.getUTCMonth() + 1) + '-' +
                 f(this.getUTCDate()) + 'T' +
                 f(this.getUTCHours()) + ':' +
                 f(this.getUTCMinutes()) + ':' +
                 f(this.getUTCSeconds()) + 'Z';
        };


        var escapeable = /["\\\x00-\x1f\x7f-\x9f]/g,
            gap,
            indent,
            meta = {    // table of character substitutions
                '\b': '\\b',
                '\t': '\\t',
                '\n': '\\n',
                '\f': '\\f',
                '\r': '\\r',
                '"': '\\"',
                '\\': '\\\\'
            },
            rep;


        function quote(string) {

            // If the string contains no control characters, no quote characters, and no
            // backslash characters, then we can safely slap some quotes around it.
            // Otherwise we must also replace the offending characters with safe escape
            // sequences.

            return escapeable.test(string) ?
                '"' + string.replace(escapeable, function (a) {
                    var c = meta[a];
                    if (typeof c === 'string') {
                        return c;
                    }
                    c = a.charCodeAt();
                    return '\\u00' + Math.floor(c / 16).toString(16) +
                                               (c % 16).toString(16);
                }) + '"' :
                '"' + string + '"';
        }


        function str(key, holder) {

            // Produce a string from holder[key].

            var i,          // The loop counter.
                k,          // The member key.
                v,          // The member value.
                length,
                mind = gap,
                partial,
                value = holder[key];

            // If the value has a toJSON method, call it to obtain a replacement value.

            if (value && typeof value === 'object' &&
                    typeof value.toJSON === 'function') {
                value = value.toJSON(key);
            }

            // If we were called with a replacer function, then call the replacer to
            // obtain a replacement value.

            if (typeof rep === 'function') {
                value = rep.call(holder, key, value);
            }

            // What happens next depends on the value's type.

            switch (typeof value) {
                case 'string':
                    return quote(value);

                case 'number':

                    // JSON numbers must be finite. Encode non-finite numbers as null.

                    return isFinite(value) ? String(value) : 'null';

                case 'boolean':
                case 'null':

                    // If the value is a boolean or null, convert it to a string. Note:
                    // typeof null does not produce 'null'. The case is included here in
                    // the remote chance that this gets fixed someday.

                    return String(value);

                    // If the type is 'object', we might be dealing with an object or an array or
                    // null.

                case 'object':

                    // Due to a specification blunder in ECMAScript, typeof null is 'object',
                    // so watch out for that case.

                    if (!value) {
                        return 'null';
                    }

                    // Make an array to hold the partial results of stringifying this object value.

                    gap += indent;
                    partial = [];

                    // If the object has a dontEnum length property, we'll treat it as an array.

                    if (typeof value.length === 'number' &&
                            !(value.propertyIsEnumerable('length'))) {

                        // The object is an array. Stringify every element. Use null as a placeholder
                        // for non-JSON values.

                        length = value.length;
                        for (i = 0; i < length; i += 1) {
                            partial[i] = str(i, value) || 'null';
                        }

                        // Join all of the elements together, separated with commas, and wrap them in
                        // brackets.

                        v = partial.length === 0 ? '[]' :
                            gap ? '[\n' + gap + partial.join(',\n' + gap) +
                                      '\n' + mind + ']' :
                                  '[' + partial.join(',') + ']';
                        gap = mind;
                        return v;
                    }

                    // If the replacer is an array, use it to select the members to be stringified.

                    if (typeof rep === 'object') {
                        length = rep.length;
                        for (i = 0; i < length; i += 1) {
                            k = rep[i];
                            if (typeof k === 'string') {
                                v = str(k, value, rep);
                                if (v) {
                                    partial.push(quote(k) + (gap ? ': ' : ':') + v);
                                }
                            }
                        }
                    } else {

                        // Otherwise, iterate through all of the keys in the object.

                        for (k in value) {
                            v = str(k, value, rep);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }

                    // Join all of the member texts together, separated with commas,
                    // and wrap them in braces.

                    v = partial.length === 0 ? '{}' :
                        gap ? '{\n' + gap + partial.join(',\n' + gap) +
                                  '\n' + mind + '}' :
                              '{' + partial.join(',') + '}';
                    gap = mind;
                    return v;
            }
        }


        // Return the JSON object containing the stringify, parse, and quote methods.

        return {
            stringify: function (value, replacer, space) {

                // The stringify method takes a value and an optional replacer, and an optional
                // space parameter, and returns a JSON text. The replacer can be a function
                // that can replace values, or an array of strings that will select the keys.
                // A default replacer method can be provided. Use of the space parameter can
                // produce text that is more easily readable.

                var i;
                gap = '';
                indent = '';
                if (space) {

                    // If the space parameter is a number, make an indent string containing that
                    // many spaces.

                    if (typeof space === 'number') {
                        for (i = 0; i < space; i += 1) {
                            indent += ' ';
                        }

                        // If the space parameter is a string, it will be used as the indent string.

                    } else if (typeof space === 'string') {
                        indent = space;
                    }
                }

                // If there is no replacer parameter, use the default replacer.

                if (!replacer) {
                    rep = function (key, value) {
                        if (!Object.hasOwnProperty.call(this, key)) {
                            return undefined;
                        }
                        return value;
                    };

                    // The replacer can be a function or an array. Otherwise, throw an error.

                } else if (typeof replacer === 'function' ||
                        (typeof replacer === 'object' &&
                         typeof replacer.length === 'number')) {
                    rep = replacer;
                } else {
                    throw new Error('JSON.stringify');
                }

                // Make a fake root object containing our value under the key of ''.
                // Return the result of stringifying the value.

                return str('', { '': value });
            },


            parse: function (text, reviver) {

                // The parse method takes a text and an optional reviver function, and returns
                // a JavaScript value if the text is a valid JSON text.

                var j;

                function walk(holder, key) {

                    // The walk method is used to recursively walk the resulting structure so
                    // that modifications can be made.

                    var k, v, value = holder[key];
                    if (value && typeof value === 'object') {
                        for (k in value) {
                            if (Object.hasOwnProperty.call(value, k)) {
                                v = walk(value, k);
                                if (v !== undefined) {
                                    value[k] = v;
                                } else {
                                    delete value[k];
                                }
                            }
                        }
                    }
                    return reviver.call(holder, key, value);
                }


                // Parsing happens in three stages. In the first stage, we run the text against
                // regular expressions that look for non-JSON patterns. We are especially
                // concerned with '()' and 'new' because they can cause invocation, and '='
                // because it can cause mutation. But just to be safe, we want to reject all
                // unexpected forms.

                // We split the first stage into 4 regexp operations in order to work around
                // crippling inefficiencies in IE's and Safari's regexp engines. First we
                // replace all backslash pairs with '@' (a non-JSON character). Second, we
                // replace all simple value tokens with ']' characters. Third, we delete all
                // open brackets that follow a colon or comma or that begin the text. Finally,
                // we look to see that the remaining characters are only whitespace or ']' or
                // ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

                if (/^[\],:{}\s]*$/.test(text.replace(/\\["\\\/bfnrtu]/g, '@').
replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').
replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

                    // In the second stage we use the eval function to compile the text into a
                    // JavaScript structure. The '{' operator is subject to a syntactic ambiguity
                    // in JavaScript: it can begin a block or an object literal. We wrap the text
                    // in parens to eliminate the ambiguity.

                    j = eval('(' + text + ')');

                    // In the optional third stage, we recursively walk the new structure, passing
                    // each name/value pair to a reviver function for possible transformation.

                    return typeof reviver === 'function' ?
                        walk({ '': j }, '') : j;
                }

                // If the text is not JSON parseable, then a SyntaxError is thrown.

                throw new SyntaxError('JSON.parse');
            },

            quote: quote
        };
    }();
}
var NOTI = {
    userID: 0,
    getMessageUrl: "/Channels/Poll.ashx",
    defaultChannel: null,
    showMost: 5,
    messageList: [],
    getMessage: function (userID) {
        if (NOTI.defaultChannel == null) {
            NOTI.defaultChannel = new AspNetComet(NOTI.getMessageUrl, userID, "defaultChannel");
            //defaultChannel.addTimeoutHandler(TimeoutHandler);
            //defaultChannel.addFailureHandler(FailureHandler);
            NOTI.defaultChannel.addSuccessHandler(NOTI.SuccessHandler);
            NOTI.defaultChannel.subscribe();
        }
    },
    getMessageByID: function (messageID) {
        var m = 0;
        for (var i = 0; i < NOTI.messageList.length; i++) {
            if (NOTI.messageList[i].mid == messageID) {
                m = NOTI.messageList[i];
                break;
            }
        }
        return m;
    },
    removeMessage: function (messageID) {
        var m = 0;
        for (var i = 0; i < NOTI.messageList.length; i++) {
            if (NOTI.messageList[i].mid == messageID) {
                m = i;
                break;
            }
        }
        NOTI.messageList = NOTI.messageList.slice(0, m).concat(NOTI.messageList.slice(m + 1));
        if (NOTI.messageList.length) {
            NOTI.$noticeCount.html(" ( " + NOTI.messageList.length + " )");
        }
        else {
            NOTI.$noticeCount.html("");
            jQuery("#unreadnotice").show();
            NOTI.$noticeCount.removeClass("notice_Highlight");
        }
    },
    getNextMessage: function (lastmessageID) {
        var next = 0;
        for (var i = 0; i < NOTI.messageList.length; i++) {
            if (NOTI.messageList[i].mid == lastmessageID) {
                if (i < NOTI.messageList.length - 1) {
                    next = NOTI.messageList[i + 1];
                }
                break;
            }
        }
        return next;
    },
    SuccessHandler: function (privateToken, alias, message) {
        if (message.content.type == "User") {
            NOTI.messageList.unshift(message);
            if (NOTI.$noticeContainer.find(".remote").length > NOTI.showMost - 1) {
                NOTI.$noticeContainer.find(".remote:last").remove();
                NOTI.$viewallnotices.html("View all ( <span class='notice_Highlight'>" + (NOTI.messageList.length - NOTI.showMost) + "</span> more)");
            }
            var html = NOTI.getNotificationHtml(message);
            jQuery("#unreadnotice").hide();
            jQuery(html).insertBefore("#unreadnotice");

            if (!message.content.read) {
                var nowCount = NOTI.$noticeCount.text().replace(/\(|\)/g, "") || 0;
                NOTI.$noticeCount.addClass("notice_Highlight").html(" ( " + (parseInt(nowCount) + 1) + " )");
            }
        }
    },
    getNotificationButtons: function (message) {
        var html = "<div style = 'display:inline-block;margin:5px 0px;'>";
        if (message.content.actions) {
            message.content.buttons = jQuery.parseJSON(message.content.actions.replace(/\\/g, ''));
        }
        if (message.content.buttons && message.content.buttons.length) {
            for (var i = 0; i < message.content.buttons.length; i++) {
                var btn = message.content.buttons[i];
                html = html + "<span index='" + i + "' class='eventbtn3 action'>" + btn.value + "</span>";
            }
        }
        html = html + "</div>";
        return html;
    },
    getNotificationHtml: function (message) {
        var notification = message.content;
        var html = "<li class='remote' id='message" + message.mid + "' message='" + message.mid + "' notiid='" + message.content.id + "'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td width='50'>" +
                "<a href='/users/user/mainpage/" + notification.sender + "'>" +
                "      <img src='" + notification.image + "' width='50' height='50' title='"
                            + notification.firstname + " " + notification.lastname + "'/>" +
                "  </a></td>" +
                "  <td>" +
                (notification.link ?
                "<a href='javascript:void(0);' class='message' action='goto'>" + notification.message + "</a>" :
                "<span>" + notification.message + "</span>" + this.getNotificationButtons(message)) +
                "      <div class='noticeTitme'>" +
                "      <img src='/images/icons/calendar_day.png' />" + notification.datetime + "</div>" +
                "  </td>" +
                "  <td width='15'>" +
                "  <div class='noticeAction' action='read'>" +
                "      <img alt='mark' title='Mark as read' src='/images/icons/read_notice.png' /></div>" +

                "  </td>" +
                "  </tr>" +
                "  </table>" +
                "  </li>";
        /*"  <div class='noticeAction' action='close'>" +
        "      <img alt='remove' title='Delete' src='/images/icons/close_notice.png' /></div>" +*/
        return html;
    },
    Mark: function (removeID, notiID, callback) {
        var nextMessage = NOTI.getNextMessage(NOTI.$noticeBox.find("li.remote:last").attr("message"));

        NOTI.removeMessage(removeID);
        NOTI.$noticeBox.find("#message" + removeID).remove();

        if (nextMessage) {
            var html = NOTI.getNotificationHtml(nextMessage);
            jQuery(html).insertBefore("#unreadnotice");
        }
        if (NOTI.messageList.length) {
            NOTI.$noticeCount.html(" ( " + NOTI.messageList.length + " )");
        }
        else {
            NOTI.$noticeCount.html("");
            jQuery("#unreadnotice").show();
            NOTI.$noticeCount.removeClass("notice_Highlight");
        }
        if (NOTI.messageList.length > NOTI.showMost) {
            NOTI.$viewallnotices.html("View all ( <span class='notice_Highlight'>" + (NOTI.messageList.length - NOTI.showMost) + "</span> more)");
        }
        else {
            NOTI.$viewallnotices.text("View all");
        }
        jQuery.get("/Notification/Index/Mark", { messageIndex: removeID, notificationID: notiID });
    },
    MarkAndRedirect: function (removeID, notiID) {
        var message = NOTI.getMessageByID(removeID);
        if (message.link) {
            jQuery.get("/Notification/Index/Mark", { messageIndex: removeID, notificationID: notiID }, function () {
                location.href = message.content.link;
            });
        }
        else {
            location.href = "/Notification/Index/";
        }
    },
    postBack: function (button, noti, removeID, notiID) {
        var url = button.url.replace(/{Sender}/g, noti.content.sender).replace(/{Recipient}/g, NOTI.userID);
        jQuery.ajax({
            type: button.type,
            url: url,
            data: button.data || {},
            success: function (response) {
                if (response == "ok") {
                    jQuery.alert("success", button.success.replace(/{username}/g, noti.content.firstname));
                }
                else {
                    jQuery.alert("danger", "Operation failed.");
                }
                NOTI.$noticeContainer.find("li[notiid='" + notiID + "']").remove();
                NOTI.Mark(removeID, notiID);
            }
        });
    },
    $noticeCount: null,
    $noticeBox: null,
    $noticeContainer: null,
    $viewallnotices: null,
    hideNoticeContainerEvent: 0,
    init: function (userID) {
        NOTI.userID = userID;
        NOTI.$noticeCount = jQuery("#notificationsCount");
        NOTI.$noticeContainer = jQuery("#notificationsContainer");
        NOTI.$noticeBox = jQuery("#noticeBox");
        NOTI.$viewallnotices = jQuery("#viewallnotices");
        NOTI.getMessage(userID);

        NOTI.$noticeBox.hide();
        NOTI.$noticeBox.mouseleave(function () {
            NOTI.hideNoticeContainerEvent = setTimeout(function () { NOTI.$noticeBox.hide(); }, 1000);
        }).mouseenter(function () {
            clearTimeout(NOTI.hideNoticeContainerEvent);
        });
        jQuery("#noticeTitle").click(function () {
            NOTI.$noticeBox.slideDown(200);
        });

        jQuery("#closeNoticeContainer").click(function () {
            NOTI.$noticeBox.hide();
        });

        jQuery("body").on("click", "div.noticeAction,.remote .message", function (event) {
            var $this = jQuery(this);
            var removeID = $this.closest("li.remote").attr("message");
            var notiID = $this.closest("li.remote").attr("notiid");
            if ($this.hasClass("noticeAction")) {
                NOTI.Mark(removeID, notiID);
            }
            if ($this.hasClass("message")) {
                NOTI.MarkAndRedirect(removeID, notiID);
            }
        }).on("click", "div.noticeAction,.remote .action", function (event) {
            var $this = jQuery(this);
            var removeID = $this.closest("li.remote").attr("message");
            var notiID = $this.closest("li.remote").attr("notiid");
            var message = NOTI.getMessageByID(removeID);
            var btn = message && message.content && message.content.buttons && message.content.buttons.length && message.content.buttons[$this.attr("index")];
            NOTI.postBack(btn, message, removeID, notiID);
        });
    }
}