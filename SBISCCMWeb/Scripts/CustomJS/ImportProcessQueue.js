// When we click on delete icon in any record in Data Queue Statistics popup
$('body').on('click', '.removeDataFromFile', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var ImportProcessId = $(this).attr("data-ImportProcessId");
    var QueryString = "ImportProcessId:" + ImportProcessId;
    var url = '/Home/RemoveDataFromFile?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: removeDataFromFile, callback: function (result) {
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
                        parent.ShowMessageNotification("success", data, false);
                        $("#ImportProcessDataQueueStatisticsModal").modal("hide");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});

$('body').on('click', '.DownloadDataQueueStatistics', function () {
    $("#frmExportToExcel").submit();
});

function ConvertEncrypte(value) {
    var newvalue = "";
    if (value != '' && value != undefined) {
        $.ajax({
            type: "POST",
            url: "/Home/GetEncryptedString",
            data: JSON.stringify({ strValue: value }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            success: function (data) {
                newvalue = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
    return newvalue;
}