$("body").on('change', '#OISalesForceTable', function () {
    $("#btnSubmitSalesforceData").attr("disabled", true);

    var tableName = $(this).val();
    if (tableName != "") {
        $.ajax({
            url: '/OIData/GetTableViewName?TableName=' + tableName,
            type: "json",
            cache: false,
            success: function (response) {
                var appenddata = "<option value=''>Select List View</option>";
                $.each(response, function (Key, value) {
                    appenddata += "<option value = '" + value.Value + " '>" + value.Text + " </option>";
                });
                $("#divResultView").show();
                $('#ResultView').html(appenddata);
            },
            error: function (xhr, status, error) {
            }
        });
    }
    else {
        $("#ResultView").val("");
        $("#divResultView").hide();
    }

});
$("body").on('change', '#ResultView', function () {
    var ListViewName = $(this).val();
    if (ListViewName == "") {
        $("#btnSubmitSalesforceData").attr("disabled", true);
    }
    else {
        $("#btnSubmitSalesforceData").attr("disabled", false);
    }
});
$("body").on('click', '#btnSubmitSalesforceData', function () {
    parent.isReload = "false";
    var ListViewName = $("#ResultView").val();
    var importMode = $("#importMode").val();
    if (ListViewName != "" || ListViewName != null) {
        $("#spnResultView").hide();
        $.ajax({
            url: '/OIData/setSalesForceDataTable?ListViewName=' + encodeURI(ListViewName) + '&ListView=' + $("#ResultView option:selected").text() + "&importMode=" + importMode,
            type: "json",
            cache: false,
            success: function (response) {
                if (response == "Success") {
                    var fileType = "SalesForce";
                    parent.openSalesForceMatchData(fileType, importMode);
                }
                else {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", response, true, false);
                    //bootbox.alert({
                    //    title: "<i class='fa fa-exclamation-triangle' aria-hidden='true'></i> Message",
                    //    message: response,
                    //});
                    $("#btnSubmitSalesforceData").attr("disabled", true);
                }
            },
            error: function (xhr, status, error) {
            }
        });
    }
    else {
        $("#spnResultView").show();
    }
});