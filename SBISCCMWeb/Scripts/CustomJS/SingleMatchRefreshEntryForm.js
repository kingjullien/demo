$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
$(document).ready(function () {
    $("#matchRefreshForm #txtSrcRecordid").focus();
    $("#matchRefreshForm #btnImportData").attr('disabled', 'disabled');
    if ($.UserRole.toLowerCase() == 'lob') {
        $(".SingleEntryFormMatchFormTab #matchRefreshForm #TagsValue").attr("data-placeholder", "Add Tags (Required)");
        $(".SingleEntryFormMatchFormTab #matchRefreshForm #TagsValue").trigger("chosen:updated");
    }
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {

            if (event.target == this) {
                $("#Tags").val($(this).val());
            }
            if ($.UserRole.toLowerCase() == 'lob') {
                Validation();
            }
        });

    }
    Validation();
});
//Click Import data button and validation for the required fields.
$(document).on('click', '#matchRefreshForm #btnImportData', function () {
    var Tags = $('#matchRefreshForm #Tags').val();
    var isValide = Validation();
    if (!isValide) {
        return false;
    }
    if ($.UserRole.toLowerCase() == 'lob' && Tags == '') {
        $("#matchRefreshForm #spnTags").show();
        return false;
    }
    else {
        $("#matchRefreshForm #spnTags").hide();
    }
});
$(document).on('blur', '#matchRefreshForm #txtSrcRecordid', function (event, node) {
    Validation();
});
$(document).on('blur', '#matchRefreshForm #txtDUNSNumber', function (event, node) {
    Validation();
});
$(document).on('change', '#matchRefreshForm #Country', function () {
    Validation();
});

