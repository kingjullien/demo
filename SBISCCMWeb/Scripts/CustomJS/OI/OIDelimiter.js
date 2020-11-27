$("body").on('click', '#btnAddDelimiter', function () {
    var Delimiter = $("#txtDelimiter").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (Delimiter != undefined && Delimiter != "") {
        $("#spnDelimiter").hide();
        callbackCloseDelimiter(Delimiter);
        //$.ajax({
        //    type: "POST",
        //    url: "/OIExportView/SetDelimiter/",
        //    headers: { "__RequestVerificationToken": token },
        //    data: JSON.stringify({ Delimiter: Delimiter }),
        //    dataType: "json",
        //    contentType: "application/json",
        //    cache: false,
        //    async: false,
        //    success: function (data) {
                
        //    },
        //    error: function (xhr, ajaxOptions, thrownError) {
        //    }
        //});
        //return true;
    } else {
        $("#spnDelimiter").show();
        return false;
    }
});
 