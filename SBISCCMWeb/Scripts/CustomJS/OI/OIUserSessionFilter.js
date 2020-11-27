function loadOIUserSessionFilter() {
    var ImportProcess = $("#hidenImportProcess").val();
    var cnt = 0;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $("#ImportProcess option").each(function () {
        if (ImportProcess == $(this).attr("value")) {
            cnt++;
        }
    });
    $("select.chzn-select").chosen({
        no_results_text: nothingFound,
        width: "100%",
        search_contains: true
    });
    if (cnt == 0) {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
            message: removeSessionFilter, callback: function () {
                $.ajax({
                    type: "POST",
                    url: "/OIMatchData/DeleteSessionFilter/",
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    success: function (data) {
                        if (data == success) {
                            location.reload();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        });
        $("#ImportProcess").val("");
    }

    $('input:enabled:visible:first').focus();
}

function OnSuccessOIUserSessionFilter(data) {
    if (data.result) {
        $("#OIMatchSessionFilterModal").modal("hide");
        ShowMessageNotification("success", data.message, false);
        location.reload();
    }
}