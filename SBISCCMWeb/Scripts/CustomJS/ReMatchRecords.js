$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();

$("body").on('blur', '.CityName', function () {
    if ($(this).val() != "" && $(this).val() != undefined) {
        $("#CityExactMatch").prop("disabled", false);
    } else {
        $("#CityExactMatch").prop("disabled", "disabled");
        $("#CityExactMatch").prop("checked", false);
    }
});
$("body").on('blur', '.StateName', function () {
    if ($(this).val() != "" && $(this).val() != undefined) {
        $("#StateExactMatch").prop("disabled", false);
    } else {
        $("#StateExactMatch").prop("disabled", "disabled");
        $("#StateExactMatch").prop("checked", false);
    }
});
$("body").on('change', '#chkRejectAll', function () {
    if ($(this).prop('checked') == true) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});
$("body").on('change', '#CityExactMatch', function () {
    if ($(this).prop('checked') == true) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});
$("body").on('change', '#StateExactMatch', function () {
    if ($(this).prop('checked') == true) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});

function UpdateReMatchRecords(data) {
    $("body").append(data);
    var message = $("#ReMatchMessage").val();
    $("#ReMatchMessage").remove();
    if (message == "Total 0 records are affected, are you sure you want to continue ?") {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
        ShowMessageNotification("success", noRecords, false, false);
    }
    else if (message.endsWith("are you sure you want to continue ?")) {
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: message, callback: function (result) {
                if (result) {
                    $("#ReMatchRecordsModal").modal("hide");
                    $("#frmRematch #GetCountsOnly").val(false);
                    $("#frmRematch").submit();
                }
            }
        });
    }
    else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("error", message, false, true, callbackRejectPurgeData);
    }
}
