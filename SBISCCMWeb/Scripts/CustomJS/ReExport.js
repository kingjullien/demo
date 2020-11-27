$("body").on('click', '#btnUnFlagAll', function () {
    var SrcRecordId = $("#SrcRecordId").val();
    var SrcRecordIdList = $("#SrcRecordIdList").val();
    var ImportProcess = $("#ImportProcess").val();
    var CountryGroupId = $("#CountryGroupId").val();
    //var Tags = $("#Tags").val();
    var Tags = $("#TagsValue").val();
    var Recordtype = $('input[name=Recordtype]:checked').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (Tags == undefined) {
        Tags = "";
    }
    var url = '/ExportView/ReExportFile' + "?SrcRecordId=" + encodeURIComponent(SrcRecordId) + "&SrcRecordIdList=" + SrcRecordIdList + "&ImportProcess=" + encodeURIComponent(ImportProcess) + "&CountryGroupId=" + encodeURIComponent(CountryGroupId) + "&Tags=" + encodeURIComponent(Tags) + "&Recordtype=" + Recordtype + "&GetCountsOnly=true";
    $.ajax({
        type: "POST",
        url: url,
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        async: false,
        cache: false,
        success: function (data) {
            if (data == dataRejected) {
                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                ShowMessageNotification("success", data, false);
                $("#ReExportFileModal").modal("hide");
            } else {
                if (data == noRecordsAffected) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                    ShowMessageNotification("success", noRecordsFound, false);
                    setTimeout(function () {
                        $(".bootbox-close-button").show();
                        $(".bootbox-close-button").addClass("closeBootbox");
                        $(".bootbox-close-button").removeClass("bootbox-close-button");
                    }, 100);
                    $("#ReExportFileModal").modal("hide");
                }
                else {
                    var url = '/ExportView/ReExportFile' + "?SrcRecordId=" + encodeURIComponent(SrcRecordId) + "&SrcRecordIdList=" + SrcRecordIdList + "&ImportProcess=" + encodeURIComponent(ImportProcess) + "&CountryGroupId=" + encodeURIComponent(CountryGroupId) + "&Tags=" + encodeURIComponent(Tags) + "&Recordtype=" + Recordtype + "&GetCountsOnly=false";
                    bootbox.confirm({
                        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: data, callback: function (result) {
                            if (result) {
                                $.ajax({
                                    type: "POST",
                                    url: url,
                                    headers: { "__RequestVerificationToken": token },
                                    dataType: "json",
                                    contentType: "application/json; charset=UTF-8",
                                    async: false,
                                    cache: false,
                                    success: function (data) {
                                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                                            ShowMessageNotification("success", data, false);
                                            callbackReExportData();
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                    }
                                });
                            }
                        }
                    });
                    setTimeout(function () {
                        $(".bootbox-close-button").show();
                        $(".bootbox-close-button").addClass("closeBootbox");
                        $(".bootbox-close-button").removeClass("bootbox-close-button");
                    }, 100);
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});