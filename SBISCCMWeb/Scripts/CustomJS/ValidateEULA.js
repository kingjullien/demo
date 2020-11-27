$('body').on('click', '#chkAccept', function () {
    var IsCheck = $(this).prop('checked');
    if (IsCheck == false) {
        $("#btnEULA").attr("disabled", "disabled");
    } else {
        $("#btnEULA").removeAttr("disabled");
    }
});
$('body').on('click', '.EULA', function () {

    var token = $('input[name="__RequestVerificationToken"]').val();
    var formData = new FormData();
    formData.append("IsAccept", $("#chkAccept").prop('checked'));
    $.ajax({
        type: "POST",
        url: '/Home/ValidateEULA',
        data: formData,
        headers: { "__RequestVerificationToken": token },
        dataType: 'json',
        contentType: false,
        processData: false,
        async: false,
        success: function (response) {
            backToParentFromEULA(response);
        },
        error: function (xhr, status, error) {
        }
    });
});