$('body').on('click', '#btnSearchDUNS', function () {
   
    var DUNSNO = $("#txtDUNSNo").val();
    var Country = $("#Country").val();

    if (DUNSNO == "") {
        $("#spnDUNS").show();
        return false;
    } else {
        $("#spnDUNS").hide();
    }
    parent.updateSearchData(DUNSNO, Country);
});

// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyUp Event)
$('input[type=text]').keyup(function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;
});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for Chane Event)
$('input[type=text]').on('change', function (e, node) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyPress Event)
$('input[type=text]').keypress(function (e) {

    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});
//only accepted numeric values
$("body").on('blur', 'input[type=text]', function () {
    var text = $(this).val();
    if (text != "") {
        if (!$.isNumeric(text)) {
            $(this).val("");
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success","Only numeric values accepted.", true);
            
        }
    }
});

