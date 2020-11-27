$(document).ready(function () {
    $("#left-panel").css("display", "none");
    $("#ribbon").css("display", "none");
    $(".pull-right").css("display", "none");
   
});
function sendEmail() {
    $("#divProgress").show();
    $.ajax({
        type: "POST",
        url: "/Account/SendMailVarificationCode",
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            $("#divProgress").hide();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $("#divProgress").hide();
        }
    });
}