$("body").on('click', '#btnAddDelimiter', function () {
    var Delimiter = $("#txtDelimiter").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (Delimiter != undefined && Delimiter != "") {
        $("#spnDelimiter").hide();

        $.ajax({
            type: "POST",
            url: "/ExportView/SetDelimiter/",
            headers: { "__RequestVerificationToken": token },
            data: JSON.stringify({ Delimiter: Delimiter }),
            dataType: "json",
            contentType: "application/json",
            cache: false,
            async: false,
            success: function (data) {
                callbackCloseDelimiter(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
        return true;
    } else {
        $("#spnDelimiter").show();
        return false;
    }
});
