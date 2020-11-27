function closemsg() {
    $(".alert-success").hide();
}
$("#btnConfigCustomAttribute").click(function () {
    var AttributeName = $("#AttributeName").val();

    if (AttributeName != undefined && AttributeName != "") {
        $("#spnAttributeName").hide();
        return true;
    }
    else {
        $("#spnAttributeName").show();
        return false;
    }

});
$(document).ready(function () {
    var displaydata = $(".alert-success").is(":visible");
    if (displaydata) {
        $("#AttributeName").val('');
    }
});

function UpdateAttributeSuccess() {
    var message = $("#Messgae").val();
    bootbox.alert({
        title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
        message: message, callback: function () {
            if (message == "Data created successfully") {
                $('.formCustomeAttribute input[type=text]').val('');
                $(".formCustomeAttribute #AttributeDataTypeCode").val($('option').filter(function () { return $(this).html() == "String"; }).val());
                $("#AttributeName").focus();
            }
        }
    });
}
