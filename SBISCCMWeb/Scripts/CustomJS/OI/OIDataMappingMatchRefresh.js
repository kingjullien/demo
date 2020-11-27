$(document).ajaxStart(function () { $('#divProgress').show(); }).ajaxStop(function () { $('#divProgress').hide(); });
$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
$(document).ready(function () {
    $(".SelectBox").each(function () {
        var fieldName = $(this).parent().parent().find(".spnExcelColumn").attr('data-val');
        var selectedvalue = 0;
        var id = $(this).attr('id');
        $("#" + id + " option").each(function () {
            var optionName = $(this).text();
            if (fieldName == optionName) {
                selectedvalue = $(this).val();
            }
            else if (optionName.toLowerCase() == "orbnum" || optionName.toLowerCase() == "orb num" || optionName.toLowerCase() == "orb number" || optionName.toLowerCase() == "orbnumber" || optionName.toLowerCase() == "orb_num") {
                if (fieldName.toLowerCase() == "orbnum") {
                    selectedvalue = $(this).val();
                }
            }
        });
        $(this).val(selectedvalue);
        var CurrentColumn = $(this).val();
        SetExampleValue(CurrentColumn, id);
    });
    ValidSelectedDropDown();
    $('#divProgress').hide();
});
function SetExampleValue(CurrentColumn, id) {
    if (parseInt(CurrentColumn) != 0) {
        $.ajax({
            type: "POST",
            url: "/OIData/UpdateExamples",
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
}
$("body").on('change', '.SelectBox', function () {

    var CurrentColumn = $(this).val();
    var id = $(this).attr('id');
    SetExampleValue(CurrentColumn, id);
    $("#btnInsertLargeData").attr('disabled', false);
    ValidSelectedDropDown();
});
function ValidSelectedDropDown() {
    if (parseInt($("#DataColumn-0").val()) > 0) {
        $("#btnOIDataMappingInsertData").attr('disabled', false);
    }
    else {
        $("#btnOIDataMappingInsertData").attr('disabled', 'disabled');
    }
}

//ORB Data Match Refresh
$("body").on('click', '#btnOIDataMappingInsertData', function () {
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var totalCount = 0;
    var fileType = $("#fileType").val();
    var url = "";
    url = "/OIData/OrbImportPanelDataMatch";

    $(".SelectBox").each(function () {
        cat.push($(this).val());
        totalCount = totalCount + 1;

        $(this).parent().removeClass('has-error');
        if ($(this).hasClass("chzn-select")) {
            OrgColumnName.push("");
        }

        else {
            OrgColumnName.push($(this).find(":selected").text());
        }
    });
    $(".spnExcelColumn").each(function () {
        var ColumnName = $(this).attr("data-val");
        ExcelColumnName.push(ColumnName);
    });

    if ($("#DataColumn-0").val() == 0 ) {
        return false;
    }
    else {
        var displaydata = $(".norecored").is(":visible");
        if (!displaydata) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, fileType: fileType }),
                headers: { "__RequestVerificationToken": token },
                dataType: "json",
                contentType: "application/json",
                success: function (data) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", data, true);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });

            return true;
        } else {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success", "Please Upload Data first.", false);
        }
    }
    return false;
});