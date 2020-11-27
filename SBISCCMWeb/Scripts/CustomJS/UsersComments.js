$('body').on('click', '#btnUsersComments', function () {
    var comment = $("#Comment").val();
    if (comment == "" || comment == undefined) {
        $("#spnComment").show();
        return false;
    }
});