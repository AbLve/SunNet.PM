var SNF = {};
window.SNF = SNF;

window.SNF.PureRequester = null;
window.SNF.ServerResponse = null;
window.SNF.MessageCount = 0;
window.SNF.ErrorMessageCss = "ErrorTip";
window.SNF.FieldNameTag = "FieldName";
window.SNF.ErrorMessageContainerType = "span";
window.SNF.ValidateFieldName = "";
window.SNF.SubmitButtonID = null;
window.SNF.GetEntityUrl = null;
window.SNF.ValidateEntityUrl = null;

window.SNF.ConstructUserInput = null;

window.SNF.Config = function (submitButtonId, ajaxHandlerUrl, constructEntity) {
    window.SNF.GetEntityUrl = ajaxHandlerUrl + "?Action=GetPureRequester";
    window.SNF.ValidateEntityUrl = ajaxHandlerUrl + "?Action=Validate";
    window.SNF.SubmitButtonID = submitButtonId;
    window.SNF.ConstructUserInput = constructEntity;

    window.SNF.GetPureRequester();
}
window.SNF.ValidateField = function (obj, fieldName) {
    window.SNF.ValidateFieldName = fieldName;
    window.SNF.ServerResponse = null;
    window.SNF.MessageCount = 0;

    window.SNF.ConstructUserInput();
    window.SNF.Send2Server();
}

window.SNF.GetPureRequester = function () {
    $.ajax({
        url: window.SNF.GetEntityUrl,
        success: function (result) {
            window.SNF.PureRequester = $.parseJSON(result);
        },
        async: false
    }
            );
}
window.SNF.Send2Server = function () {
    $.ajax({
        type: 'POST',
        url: window.SNF.ValidateEntityUrl,
        data: { entity: JSON.stringify(window.SNF.PureRequester) },
        success: function (result) {
            window.SNF.ServerResponse = $.parseJSON(result);
            window.SNF.DisplayMessage();
            window.SNF.AdjustSubmitButton();
        },
        async: true
    }
                   );
}

window.SNF.DisplayMessage = function () {
    if (window.SNF.ValidateFieldName == null || window.SNF.ValidateFieldName == "") {
        $(window.SNF.ErrorMessageContainerType + "[" + window.SNF.FieldNameTag + "]").each(function () {
            $(this).html("");
            $(this).removeClass(window.SNF.ErrorMessageCss);
        });

        $(window.SNF.ErrorMessageContainerType + "[" + window.SNF.FieldNameTag + "]").each(function () {
            var fieldName = $(this).attr(window.SNF.FieldNameTag);
            var msg = window.SNF.GetMessageByFieldName(fieldName);
            if (msg != "") {
                $(this).html(msg);
                $(this).addClass(window.SNF.ErrorMessageCss);
            }
        });
    }
    else {
        $(window.SNF.ErrorMessageContainerType + "[" + window.SNF.FieldNameTag + "]").each(function () {
            var fieldName = $(this).attr(window.SNF.FieldNameTag);
            if (window.SNF.ValidateFieldName == fieldName) {
                var msg = window.SNF.GetMessageByFieldName(fieldName);
                if (msg != "") {
                    $(this).html(msg);
                    $(this).addClass(window.SNF.ErrorMessageCss);
                }
                else {
                    $(this).html("");
                    $(this).removeClass(window.SNF.ErrorMessageCss);
                }
            }
        });
    }

    $(window.SNF.ErrorMessageContainerType + "[" + window.SNF.FieldNameTag + "]").each(function () {
        var fieldName = $(this).attr(window.SNF.FieldNameTag);
        var msg = window.SNF.GetMessageByFieldName(fieldName);
        if (msg != "") {
            window.SNF.MessageCount++;
        }
    });
}

window.SNF.GetMessageByFieldName = function (fieldName) {
    var msgs = "";
    for (var i = 0; i < window.SNF.ServerResponse.length; i++) {
        if (window.SNF.ServerResponse[i].Key == fieldName) {
            msgs = window.SNF.ServerResponse[i].Message + "\r\n";
        }
    }
    return msgs;
}

window.SNF.AdjustSubmitButton = function () {
    if (window.SNF.SubmitButtonID == null)
        return;
    if (window.SNF.MessageCount > 0)
        $("#" + window.SNF.SubmitButtonID).attr("disabled", true);
    else
        $("#" + window.SNF.SubmitButtonID).removeAttr("disabled");
}