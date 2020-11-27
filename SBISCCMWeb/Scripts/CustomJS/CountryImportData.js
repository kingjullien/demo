$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});

//import data
$("body").on('click', '#btnSubmitImportData', function () {
    var formData = new FormData();
    if ($('#file')[0].files[0] != undefined) {
        formData.append('file', $('#file')[0].files[0]);
        formData.append('header', $('#WithHeader').prop('checked'));
        var token = $('input[name="__RequestVerificationToken"]').val();
        try {
            $.ajax({
                type: "POST",
                url: '/Portal/CountryImportData',
                data: formData,
                headers: { "__RequestVerificationToken": token },
                dataType: 'json',
                contentType: false,
                processData: false,
                async: false,
                success: function (response) {
                    if (response == "Only formats allowed are :xls,xlsx") {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        parent.ShowMessageNotification("success",response, false);
                        //bootbox.alert({
                        //    title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                        //    message: response
                        //});
                        $('#file').val("");
                        return false;
                    }
                    if (response.indexOf("already belongs to this file") > -1) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        parent.ShowMessageNotification("success",response, false);

                        return false;
                    }
                    if (response.indexOf("Error:") > -1) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        parent.ShowMessageNotification("success",response, false);

                        return false;
                    }
                    if (response == "success") {
                        parent.CloseImportPanel();
                    }

                },
                error: function (xhr, status, error) {
                }
            });
        }
        catch (e) { }
    } else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        parent.ShowMessageNotification("success", selectFile, false);
        return false;

    }
    return false;
});


// change the value of the Example display in popup when source changes
$("body").on('change', '.SelectBoxCommandLine', function () {
    var id = $(this).attr('id');
    var CurrentColumn = $(this).val();

    if (parseInt($("#DataColumn-0").val()) > 0 && parseInt($("#DataColumn-1").val())) {
        $("#btnInsertData").attr('disabled', false);
    }
    else {
        $("#btnInsertData").attr('disabled', 'disabled');
    }
    if (parseInt(CurrentColumn) != 0) {
        $.ajax({
            type: "POST",
            url: "/Portal/UpdateExamples",
            data: JSON.stringify({ CurrentColumn: CurrentColumn }),
            dataType: "json",
            contentType: "application/json",
            success: function (data) {

                $("#" + id).parent().next().text(data);

            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    } else {
        $("#" + id).parent().next().text('');
    }
});

// Insert data Open popup for Insert Data and make ajax call for match data.
$("body").on('click', '#btnInsertData', function () {
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var count = 0;
    var notSelectedCount = 0;
    var totalCount = 0;
    var duplicateselectCount = 0;
    
    $(".SelectBoxCommandLine").each(function () {
        cat.push($(this).val());
        totalCount = totalCount + 1;
        if ($(this).val() == "0") {
            notSelectedCount = notSelectedCount + 1;
        }
        if (totalCount == 2) {
            OrgColumnName.push($(this).find(":selected").text());
        } else {
            OrgColumnName.push($(this).find(":selected").text());
        }
        $(this).parent().removeClass('has-error');
    });

    if (cat.length > 0) {
        for (var i = 0 ; i < cat.length; i++) {
            for (var j = i; j <= cat.length; j++) {
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
        var displaydata = $(".norecored").is(":visible");
        if (!displaydata) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            bootbox.confirm({
                title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: overwrite, callback: function (result) {
                    try {
                        $.ajax({
                            type: "POST",
                            url: "/Portal/CountryDataMatch",
                            data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, IsOverWrite: result }),
                            headers: { "__RequestVerificationToken": token },
                            dataType: "json",
                            contentType: "application/json",
                            cache: false,
                            success: function (data) {
                                // Changes for Converting magnific popup to modal popup
                                $("#CountryGroupImportModal").modal("hide");
                                bootbox.hideAll();
                                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                                ShowMessageNotification("success",data, false);

                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                            }
                        });
                    } catch (e) { }
                    return true;
                }
            });
        } else {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success", uploadData, true);

        }
    }
    return false;
});



$("#file").change(function () {
    if ($('#file').val() != "") {
        var fileExtension = ['xls', 'xlsx'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success", allowedFormats + fileExtension.join(', '), false);

            $('#file').val("");
        }
    }
});