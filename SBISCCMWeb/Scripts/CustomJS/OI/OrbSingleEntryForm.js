$(document).ready(function () {
    $(".dataImport").addClass("thColor");
    LoadSingleEntryTags();
});
$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();

function LoadSingleEntryTags() {
    if ($("#OIDataImport #OrbsingleEntryTagsValue").length > 0) {
        $("#OIDataImport #OrbsingleEntryTagsValue").chosen().change(function (event) {
            if (event.target == this) {
                $("#OIDataImport #OrbSingleEntryTags").val($(this).val());
            }
        });
    }
}

function SetTagValue(OptionValue) {
    $.magnificPopup.close();
    var x = document.getElementById("OrbsingleEntryTagsValue");
    var option = document.createElement("option");
    option.text = OptionValue;
    option.value = OptionValue;
    x.add(option);
    $(".chzn-select").trigger("chosen:updated");
}

//Click Import data button and validation for the required fields.
function OISingleEntryValidation() {
    var Tags = $('#OIDataImport #OrbSingleEntryTags').val();

    if ($.UserRole.toLowerCase() == 'lob' && Tags == '') {
        $("#OIDataImport #spnTags").show();
        return false;
    }
    else {
        $("#OIDataImport #spnTags").hide();
        return true;
    }
}

$(document).on('change', '.OISingleEntryPopup input[type=checkbox][name=rBtnImportDataOption]', function () {
    var ImportType = $('input.rButtonImportData').is(':checked');
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
            url: '/ImportData/OISingleEntryForm?IsPartial=' + true,
            dataType: 'html',
            async: false,
            contentType: false,
            processData: false,
            success: function (data) {
                parent.addRemoveOISingleEntryPopupClass("popupOrbSingleEntryFormMatchRefresh", false);
                $(".OISingleEntryMain").html(data);
                LoadSingleEntryTags();
            },
        });
    }
    else {
        $.ajax({
            type: "GET",
            url: '/ImportData/OISingleEntryMatchRefresh',
            dataType: 'html',
            async: false,
            contentType: false,
            processData: false,
            success: function (data) {
                parent.addRemoveOISingleEntryPopupClass("popupOrbSingleEntryFormMatchRefresh", true);
                $(".OISingleEntryMain").html(data);
            },
        });
    }

});

function OIMatchRefreshSuccess(data) {
    if (data.message.startsWith("Please fill proper information.")) {
        $("#MatchRefreshOrbnumber").show();
        $("#MatchRefreshOrbnumber").text("OrbNumber is Required.");
    }
    else {
        $("#SingleEntryFormModal").modal("hide");
        parent.ShowMessageNotification("success", data.message, data.message.startsWith("Import Process Completed Successfully"));
    }

}


