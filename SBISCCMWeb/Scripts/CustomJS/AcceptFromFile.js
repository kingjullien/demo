// Display and Hide loader bar for every ajax call
$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();

    //setup_dashboard_widgets_desktop();
});
$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

$("body").on('click', '#btnInsertAcceptData', function () {
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
    if ($("#DataColumn-0").val() == 0 || $("#DataColumn-1").val() == 0 || $("#DataColumn-2").val() == 0) {
        $("#btnInsertAcceptData").attr('disabled', 'disabled');
        return false;
    }
    else if (duplicateselectCount > 0) { $("#btnInsertAcceptData").attr('disabled', 'disabled'); return false; }
    else {
        $("#btnInsertAcceptData").attr('disabled', false);
        //var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: "POST",
            url: "/StewardshipPortal/AcceptDataFile",
            data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName }),
            //headers: { "__RequestVerificationToken": token },
            dataType: "json",
            contentType: "application/json",
            cache: false,
            success: function (data) {
                $("#AcceptFromFileModal").modal("hide");
                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                parent.ShowMessageNotification("error", data, false, true, parent.callbackRejectPurgeData);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });

        return true;
    }
});

function LoadAcceptMapping(formData) {
    $.ajax({
        type: "POST",
        url: '/StewardshipPortal/BindAcceptFileMapping',
        data: formData,
        dataType: 'html',
        async: false,
        contentType: false,
        processData: false,
        success: function (data) {
            $("#DivPartialBindAcceptMapping").html(data);
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
                    if (optionName.toLowerCase() == "dnbdunsnumber" || optionName.toLowerCase() == "dunsnumber") {
                        if (fieldName.toLowerCase() == "dnbdunsnumber") {
                            selectedvalue = $(this).val();
                        }
                    }
                });
                $(this).val(selectedvalue);
                var cnt = 0;
                if (parseInt($("#DataColumn-0").val()) > 0 && parseInt($("#DataColumn-1").val()) > 0 && parseInt($("#DataColumn-2").val()) > 0) {
                }
                else {
                    cnt++;
                }
                if (cnt > 0) {
                    $("#btnInsertAcceptData").attr('disabled', 'disabled');
                } else {
                    $("#btnInsertAcceptData").attr('disabled', false);
                }
            });
        }
    });
}

$("body").on('change', '.SelectBox', function () {
    var cnt = 0;
    if (parseInt($("#DataColumn-0").val()) > 0 && parseInt($("#DataColumn-1").val()) > 0 && parseInt($("#DataColumn-2").val()) > 0) {
    }
    else {
        cnt++;
    }
    if (cnt > 0) {
        $("#btnInsertAcceptData").attr('disabled', 'disabled');
    } else {
        $("#btnInsertAcceptData").attr('disabled', false);
    }
});