$(document).ready(function () {    
    var header = $("#header");
    $("#header").remove();
    header.prependTo("#login");
    $.ajax({
        type: 'GET',
        url: '/Home/ValidateEULA',
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#ValidateEULAModalMain").html(data);
            $("#ValidateEULAModal").show();
        }
    });
});
function backToParentFromEULA(response) {
    $("#ValidateEULAModal").modal("hide");
    if (response.result) {
        window.location = "/Home/Index";
    }
    else
    {
        window.location = "/Account/Login";
    }
}