$(document).ready(function () {
    var header = $("#header");
    $("#header").remove()
    header.prependTo("#login");
    var RememberMe = $("#RememberMe").val();
    if (RememberMe == "True") {
        $("#RememberMachineDetails").prop('checked', true);
        $("#RememberMachineDetails").val(true);
    }

});

$("body").on('change', '#RememberMachineDetails', function () {
    if ($(this).is(":checked")) {
        var returnVal = $(this).is(":checked");
        $(this).attr("checked", returnVal);
        $(this).attr("value", returnVal);
    }
});
$("body").on('focus', '#SecurityAnswer', function () {
    $("#SecurityAnswer").attr('readonly', false);
});