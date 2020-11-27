function SuccessOrbDeleteData(data) {
    if (data.result == false) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
        ShowMessageNotification("success", noRecords, false);
    }
    else if (data.result == true && data.IsRecordDeleted == false) {
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: data.message, callback: function (result) {
                if (result) {
                    $("#fromDeleteData #GetCountOnly").val(false);
                    $("#fromDeleteData").submit();
                }
            }
        });
    }
    else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", data.message, false);
        $("#OIMatchDeleteCompanyDataModal").modal("hide");
        callbackRejectPurgeData();
    }
}