$(document).on('change', '#divPurgeFromFile #RejectPurgeFile', function () {
    if ($('#file').val() != "") {
        var formats = $("#divPurgeFromFile #allowedFormats").val();
        var fileExtension = formats.split(','); //['xls', 'xlsx'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
            parent.ShowMessageNotification("success", formatsAllowed + fileExtension.join(', '), true);
            
            $('#divPurgeFromFile #RejectPurgeFile').val("");
        }
        else {
            var formData = new FormData();
            if ($('#divPurgeFromFile #RejectPurgeFile')[0].files[0] != undefined) {
                formData.append('file', $(this)[0].files[0]);
                LoadRejectMapping(formData)
            }
        }
    }
})

$(document).on('click', '#btnInsertRejectData', function () {
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var count = 0;
    var notSelectedCount = 0;
    var totalCount = 0;
    var duplicateselectCount = 0;
    var IsPurgedata = $('#divPurgeFromFile #IsPurgeData').val();
    if (IsPurgedata.toLocaleLowerCase() == "false") {
        IsPurgedata = $("#divPurgeFromFile #chkRejectAll").prop("checked")
    }
    $("#divPurgeFromFile .SelectBox").each(function () {
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
                        var currentvalue = $("#divPurgeFromFile #DataColumn-" + i).val();
                        if (parseInt(currentvalue) > 0) {
                            $("#divPurgeFromFile #DataColumn-" + i).parent().addClass('has-error');
                        }
                        var currentvalue = $("#divPurgeFromFile #DataColumn-" + j).val();
                        if (parseInt(currentvalue) > 0) {
                            $("#divPurgeFromFile #DataColumn-" + j).parent().addClass('has-error');
                        }
                    }
                    duplicateselectCount = duplicateselectCount + 1;
                }
            }
        }
    }
    $("#divPurgeFromFile .spnExcelColumn").each(function () {
        var ColumnName = $(this).attr("data-val");
        ExcelColumnName.push(ColumnName);
    });



    if ($("#divPurgeFromFile #DataColumn-0").val() == 0 || $("#divPurgeFromFile #DataColumn-1").val() == 0) {
        return false;
    }
    else if (duplicateselectCount > 0) { return false; }
    else {

        //var token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            type: "POST",
            url: "/StewardshipPortal/RejectData",
            data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, IsPurgeData: IsPurgedata }),
            //headers: { "__RequestVerificationToken": token },
            dataType: "json",
            contentType: "application/json",
            cache: false,
            success: function (data) {
                $("#divPurgeDataModal").modal('hide');
                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
                parent.ShowMessageNotification("error", data, false, true, parent.callbackRejectPurgeData);
                
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });

        return true;
    }





});

function LoadRejectMapping(formData) {
    //var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: '/StewardshipPortal/BindRejectMapping',
        //headers: { "__RequestVerificationToken": token },
        data: formData,
        dataType: 'html',
        async: false,
        contentType: false,
        processData: false,
        success: function (data) {
            $("#DivPartialBindRejectMapping").html(data);
            $("#divPurgeFromFile .SelectBox").each(function () {

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