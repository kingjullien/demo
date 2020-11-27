// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyUp Event)
$('input[type=text]').keyup(function (e) {

    var strtext = $(this).val();
    var strvalidation = new RegExp('<[^>]*>');
    if (strvalidation.test(strtext)) {
        $(this).val('');
    }

});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for Change Event)
$('input[type=text]').on('change', function (event, node) {
    var strtext = $(this).val();
    var strvalidation = new RegExp('<[^>]*>');
    if (strvalidation.test(strtext)) {
        $(this).val('');
    }
});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyPress Event)
$('input[type=text]').keypress(function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode != 47) && (keyCode != 60) && (keyCode != 62));
    return ret;

});


// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for textarea KeyUp Event)
$('textarea').keyup(function (e) {

    var strtext = $(this).val();
    var strvalidation = new RegExp('<[^>]*>');
    if (strvalidation.test(strtext)) {
        $(this).val('');
    }

});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for textarea Change Event)
$('textarea').on('change', function (event, node) {
    var strtext = $(this).val();
    var strvalidation = new RegExp('<[^>]*>');
    if (strvalidation.test(strtext)) {
        $(this).val('');
    }
});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for textarea KeyPress Event)
$('textarea').keypress(function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode != 47) && (keyCode != 60) && (keyCode != 62));
    return ret;

});

//$(document).ready(function () {
//    $('input:enabled:visible:first').focus();
//    $('textarea:enabled:visible:first').focus();
//});
function isNumber(evt) {
   
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
function AjaxPostWithJsonObjectCall(url, data, callback) {
    jQuery.ajax({
        url: url,
        async: true,
        type: "POST",
        contentType: 'application/json',
        dataType: 'json',
        data: data,
        cache: false,
        success: callback,
        error: function (xhr, status, err) {
           
            if (xhr.status == 400) {
                ShowValidationMessage(xhr.responseText, 'Error');
            }
            else if (xhr.status == 404) {
                alert('Invalid Path', 'Error');
            } else {           
            }
        },
    });
}

function AjaxGetCall(url, param, callback) {
    jQuery.ajax({
        url: url + "?" + param,
        async: true,
        type: "GET",
        dataType: "JSON",
        cache: false,
        success: callback,
        error: function (e) {
            
        },
    });
}


$(".OnlyDigit").keypress(function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});
$("body").on('blur', '.OnlyDigit', function () {
    var text = $(this).val();
    if (text != "") {
        if (!$.isNumeric(text)) {
            $(this).val("");
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success","Only numeric values accepted.", true);
        }
    }
});
