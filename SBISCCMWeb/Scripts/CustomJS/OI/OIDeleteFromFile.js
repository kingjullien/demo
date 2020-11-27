$("body").on('change', '#DeletePurgeFile', function () {
    if ($('#file').val() != "") {
        var formats = $("#allowedFormats").val();
        var fileExtension = formats.split(','); //['xls', 'xlsx'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            var formData = new FormData();
            if ($('#DeletePurgeFile')[0].files[0] != undefined) {
                formData.append('file', $(this)[0].files[0]);
                LoadRejectMapping(formData)
            }
        }
        else {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
            parent.ShowMessageNotification("success", uploadCertainFile + fileExtension.join(', '), true);
            $('#DeletePurgeFile').val("");
        }
    }

});
$("body").on('click', '#btnInsertDeleteData', function () {
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var count = 0;
    var notSelectedCount = 0;
    var totalCount = 0;
    var duplicateselectCount = 0;

    $(".SelectBox").each(function () {
        cat.push($(this).val());
        totalCount = totalCount + 1;
        if ($(this).val() == "0" || $(this).val() == null) {
            notSelectedCount = notSelectedCount + 1;
            $(this).parent().addClass('has-error');
        }
        else {
            $(this).parent().removeClass('has-error');
        }
        OrgColumnName.push($(this).find(":selected").text());
    });

    if (notSelectedCount > 0) {
        return false;
    }

    if (cat.length > 0) {
        for (var i = 0; i < cat.length; i++) {
            for (var j = 0; j <= cat.length; j++) {
                if (i != j && cat[i] == cat[j]) {
                    count = count + 1;
                    if (cat[i] != "0") {
                        var currentvalue = $("#DataColumn-" + i).val();
                        if (parseInt(currentvalue) > 0) {
                            $("#DataColumn-" + i).parent().addClass('has-error');
                        }
                        var currentvalue = $("#DataColumn-" + j).val();
                        if (parseInt(currentvalue) > 0) {
                            $("#DataColumn-" + j).parent().addClass('has-error');
                        }
                    }
                    duplicateselectCount = duplicateselectCount + 1;
                }
            }
        }
    }
    $(".spnExcelColumn").each(function () {
        var ColumnName = $(this).attr("data-val");
        ExcelColumnName.push(ColumnName);
    });



    if ($("#DataColumn-0").val() == 0 || $("#DataColumn-1").val() == 0) {
        return false;
    }
    else if (duplicateselectCount > 0) { return false; }
    else {
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteCompanyData, callback: function (result) {
                if (result) {
                    $.ajax({
                        type: "POST",
                        url: "/OIMatchData/DeleteData",
                        data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName }),
                        dataType: "json",
                        contentType: "application/json",
                        cache: false,
                        success: function (data) {
                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
                            ShowMessageNotification("success", data, false);
                            $("#OIMatchDeleteFromFileModal").modal("hide");
                            callbackRejectPurgeData();
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                }
            }
        });
        return true;
    }
});

function LoadRejectMapping(formData) {
    $.ajax({
        type: "POST",
        url: '/OIMatchData/BindDeleteMapping',
        data: formData,
        dataType: 'html',
        async: false,
        contentType: false,
        processData: false,
        success: function (data) {
            $("#DivPartialBindDeleteMapping").html(data);
            $(".SelectBox").each(function () {

                var fieldName = $(this).parent().parent().find(".spnExcelColumn").attr('data-val');
                var selectedvalue = 0;
                $(".SelectBox option").each(function () {
                    var optionName = $(this).text();
                    if (fieldName == optionName) {
                        selectedvalue = $(this).val();
                    }
                    if (optionName.toLowerCase() == "srcrecordid" || optionName.toLowerCase() == "src recordid" || optionName.toLowerCase() == "accountid") {
                        if (fieldName.toLowerCase() == "srcrecordid") {
                            selectedvalue = $(this).val();
                        }
                    }
                    if (optionName.toLowerCase() == "inputid") {
                        if (fieldName.toLowerCase() == "inputid") {
                            selectedvalue = $(this).val();
                        }
                    }
                });
                $(this).val(selectedvalue);
            });
        }
    });
}