$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
$(document).ready(function () {
    $(".dataImport").addClass("thColor");
    $("#dataImportForm #txtSrcRecordid").focus();
    $("#dataImportForm #btnImportData").attr('disabled', 'disabled');

    var Message = $("#dataImportForm #Message").val();
    if (Message != '') {
        jQuery.ajaxSetup({ async: true });
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        parent.ShowMessageNotification("success", Message, Message.startsWith("Import Process Completed Successfully"));
    }
    loadCleansMatchDataTags();
    Validation();
});
//Click Import data button and validation for the required fields.
function loadCleansMatchDataTags() {
    if ($.UserRole.toLowerCase() == 'lob') {
        $(".SingleEntryFormTab #dataImportForm #TagsValue").attr("data-placeholder", "Add Tags (Required)");
        $(".SingleEntryFormTab #dataImportForm #TagsValue").trigger("chosen:updated");
    }
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {

            if (event.target == this) {
                $("#dataImportForm #Tags").val($(this).val());
            }
            if ($.UserRole.toLowerCase() == 'lob') {
                Validation();
            }
        });
    }
}
$(document).on('click', '#dataImportForm #btnImportData', function () {
    var Tags = $('#Tags').val();
    var isValide = Validation();
    if (!isValide) {
        return false;
    }
    if ($.UserRole.toLowerCase() == 'lob' && Tags == '') {
        $("#dataImportForm #spnTags").show();
        return false;
    }
    else {
        $("#dataImportForm #spnTags").hide();
    }
});
function Validation() {
    var ImportType = $('input.rButtonImportData').is(':checked');
    if (ImportType == false) {
        ImportType = "Data Import";
    }
    else if (ImportType == true) {
        ImportType = "Match Refresh";
    }
    if (ImportType == "Match Refresh") {
        var SrcRecordId = $('#matchRefreshForm #txtSrcRecordid').val().trim();
        var DUNSNumber = $('#matchRefreshForm #txtDUNSNumber').val().trim();
        var address = $('#matchRefreshForm #txtState').val();
        var CountryISOAlpha2Code = $('#matchRefreshForm #Country').val();
        var Tags = $('#matchRefreshForm #Tags').val();
        var cnt = 0;

        if (SrcRecordId == '' || SrcRecordId == undefined) {

            cnt++;
        }
        else {
        }
        if (DUNSNumber == '' || DUNSNumber == undefined) {

            cnt++;
        }
        else {
        }
        if (CountryISOAlpha2Code == '' || CountryISOAlpha2Code == undefined) {
            cnt++;
        }
        else {
        }
        if ($.UserRole.toLowerCase() == 'lob' && Tags == '') {
            cnt++;
        }

        if (cnt > 0) {
            $("#matchRefreshForm #btnImportData").attr('disabled', 'disabled');
            return false;
        } else {
            $("#matchRefreshForm #btnImportData").removeAttr('disabled')
            return true;
        }
    }
    else {
        var SrcRecordIdImport = $('#dataImportForm #txtSrcRecordid').val().trim();
        var companyNameImport = $('#dataImportForm #txtCompanyName').val().trim();
        var addressImport = $('#dataImportForm #txtState').val();
        var CountryISOAlpha2CodeImport = $('#Country').val();
        var TagsImport = $('#Tags').val();
        var cntImport = 0;

        if (SrcRecordIdImport == '' || SrcRecordIdImport == undefined) {

            cntImport++;
        }
        else {
        }
        if (CountryISOAlpha2CodeImport == '' || CountryISOAlpha2CodeImport == undefined) {
            cntImport++;
        }
        else {
        }
        if ($.UserRole.toLowerCase() == 'lob') {
            if (TagsImport == '') {
                cntImport++;
            }
            else {
            }
        }
        if (cntImport > 0) {
            $("#dataImportForm #btnImportData").attr('disabled', 'disabled');
            return false;
        } else {
            $("#dataImportForm #btnImportData").removeAttr('disabled')
            return true;
        }
    }
}
$(document).on('blur', '#dataImportForm #txtSrcRecordid', function (event, node) {
    Validation();
});
$(document).on('blur', '#dataImportForm #txtCompanyName', function (event, node) {
    Validation();
});
$(document).on('change', '#dataImportForm #Country', function () {
    Validation();
});
$(document).on('change', 'input[type=checkbox][name=rBtnImportDataOption]', function () {
    var ImportType = $('input.rButtonImportData').is(':checked');
    var IsLicense = $("#IsTagsLicenseAllow").val();
    if (ImportType == false) {
        ImportType = "Data Import";
        $(".dataImport").addClass("thColor");
        $(".matchRefresh").removeClass("thColor");
    }
    else if (ImportType == true) {
        ImportType = "Match Refresh";
        $(".matchRefresh").addClass("thColor");
        $(".dataImport").removeClass("thColor");
    }
    if (ImportType == "Data Import") {
        $.ajax({
            type: "GET",
            url: '/ImportData/SingleFormEntry?IsPartial=' + true,
            dataType: 'html',
            async: false,
            contentType: false,
            processData: false,
            success: function (data) {
                parent.addRemoveDNBSingleEntryPopupClass(false);
                $(".SingleEntryMain").html(data);
                loadCleansMatchDataTags();
            },
        });
    }
    else {
        $.ajax({
            type: "GET",
            url: '/ImportData/SingleMatchRefreshFormEntry',
            dataType: 'html',
            async: false,
            contentType: false,
            processData: false,
            success: function (data) {
                parent.addRemoveDNBSingleEntryPopupClass(true);
                $(".SingleEntryMain").html(data);
            },
        });
    }

});
function matchRefreshFormSuccess(data) {
    $("#SingleEntryFormModal").modal("hide");
    parent.ShowMessageNotification("success", data.message, false);
}