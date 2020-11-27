$('body').on('click', '#btnBingsearchSubmit', function () {
    var searchValue = $("#searchValue").val();
    if (searchValue == "") {
        $("#searchValue").parent().addClass("has-error");
        return false;
    }
    else {
        $("#searchValue").parent().removeClass("has-error");
    }
});
function OnSuccess() {
    $('#divProgress').hide();
}