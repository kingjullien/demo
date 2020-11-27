$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();

function closemsg(val) {
    val.parentNode.hidden = true;
}
function MatchGradeValueChnage() {
    $("#form_windowAAC .chzn-select").val('').trigger("chosen:updated");
    var MatchGradeText = $("#MatchGrade option:selected").text();
    var ConfidenceCode = $("#ConfidenceCode").val();
    var CriteriaGroupId = $("#CriteriaGroupId").val();
    if (CriteriaGroupId == "0" && (ConfidenceCode == "0" || ConfidenceCode == null)) {
        $("#ConfidenceCodeValue").val("");
        $("#CompanyGradeValue").val($("#CompanyGradeValue option:first").val());
        $("#StreetGradeValue").val($("#StreetGradeValue option:first").val());
        $("#StreetNameGradeValue").val($("#StreetNameGradeValue option:first").val());
        $("#CityGradeValue").val($("#CityGradeValue option:first").val());
        $("#StateGradeValue").val($("#StateGradeValue option:first").val());
        $("#AddressGradeValue").val($("#AddressGradeValue option:first").val());
        $("#PhoneGradeValue").val($("#PhoneGradeValue option:first").val());
        $("#ZipGradeValue").val($("#ZipGradeValue option:first").val());
        $("#DensityValue").val($("#DensityValue option:first").val());
        $("#UniquenessValue").val($("#UniquenessValue option:first").val());
        $("#SICValue").val($("#SICValue option:first").val());

        $("#ConfidenceCode").val("");
        $("#CompanyGrade").val($("#CompanyGradeValue option:first").val());
        $("#StreetGrade").val($("#StreetGradeValue option:first").val());
        $("#StreetNameGrade").val($("#StreetNameGradeValue option:first").val());
        $("#CityGrade").val($("#CityGradeValue option:first").val());
        $("#StateGrade").val($("#StateGradeValue option:first").val());
        $("#AddressGrade").val($("#AddressGradeValue option:first").val());
        $("#PhoneGrade").val($("#PhoneGradeValue option:first").val());
        $("#ZipGrade").val($("#ZipGradeValue option:first").val());
        $("#Density").val($("#DensityValue option:first").val());
        $("#Uniqueness").val($("#UniquenessValue option:first").val());
        $("#SIC").val($("#SICValue option:first").val());


        $("#CompanyCodeValue").val($("#CompanyCodeValue option:first").val());
        $("#StreetCodeValue").val($("#StreetCodeValue option:first").val());
        $("#StreetNameCodeValue").val($("#StreetNameCodeValue option:first").val());
        $("#CityCodeValue").val($("#CityCodeValue option:first").val());
        $("#StateCodeValue").val($("#StateCodeValue option:first").val());
        $("#AddressCodeValue").val($("#AddressCodeValue option:first").val());
        $("#PhoneCodeValue").val($("#PhoneCodeValue option:first").val());
        $("#ZipCodeValue").val($("#ZipCodeValue option:first").val());
        $("#DensityCodeValue").val($("#DensityCodeValue option:first").val());
        $("#UniquenessCodeValue").val($("#UniquenessCodeValue option:first").val());
        $("#SICCodeValue").val($("#SICCodeValue option:first").val());

        $("#CompanyCode").val($("#CompanyCodeValue option:first").val());
        $("#StreetCode").val($("#StreetCodeValue option:first").val());
        $("#StreetName").val($("#StreetNameCodeValue option:first").val());
        $("#CityCode").val($("#CityCodeValue option:first").val());
        $("#StateCode").val($("#StateCodeValue option:first").val());
        $("#AddressCode").val($("#AddressCodeValue option:first").val());
        $("#PhoneCode").val($("#PhoneCodeValue option:first").val());
        $("#ZipCode").val($("#ZipCodeValue option:first").val());
        $("#DensityCode").val($("#DensityCodeValue option:first").val());
        $("#UniquenessCode").val($("#UniquenessCodeValue option:first").val());
        $("#SICCode").val($("#SICCodeValue option:first").val());

        $("#ExcludeFromAutoAccept").prop("checked", false);
        $(".ExcludedLable").removeClass("Excluded");
        $("#SingleCandidateMatchOnly").prop("checked", false);
        $("#RequiredDegreeSeparationOnly").prop("checked", false);
        $("#GroupId").val($("#GroupId option:first").val());
        $("#Tags").val("");
        $("#CompanyScore").val('0');
        RefreshMultiSelect();


    }
    if (MatchGradeText.length > 0 && ConfidenceCode > 0) {
        $("#ConfidenceCodeValue").val(ConfidenceCode);
        $("#CompanyGradeValue").val(MatchGradeText.length > 0 ? MatchGradeText.substring(0, 1) : "Z");
        $("#StreetGradeValue").val(MatchGradeText.length > 1 ? MatchGradeText.substring(1, 2) : "Z");
        $("#StreetNameGradeValue").val(MatchGradeText.length > 2 ? MatchGradeText.substring(2, 3) : "Z");
        $("#CityGradeValue").val(MatchGradeText.length > 3 ? MatchGradeText.substring(3, 4) : "Z");
        $("#StateGradeValue").val(MatchGradeText.length > 4 ? MatchGradeText.substring(4, 5) : "Z");
        $("#AddressGradeValue").val(MatchGradeText.length > 5 ? MatchGradeText.substring(5, 6) : "Z");
        $("#PhoneGradeValue").val(MatchGradeText.length > 6 ? MatchGradeText.substring(6, 7) : "Z");
        $("#ZipGradeValue").val(MatchGradeText.length > 7 ? MatchGradeText.substring(7, 8) : "Z");
        $("#DensityValue").val(MatchGradeText.length > 8 ? MatchGradeText.substring(8, 9) : "Z");
        $("#UniquenessValue").val(MatchGradeText.length > 9 ? MatchGradeText.substring(9, 10) : "Z");
        $("#SICValue").val(MatchGradeText.length > 10 ? MatchGradeText.substring(10, 11) : "Z");


        $("#CompanyCodeValue").val($("#CompanyCodeValue option:first").val());
        $("#StreetCodeValue").val($("#StreetCodeValue option:first").val());
        $("#StreetNameCodeValue").val($("#StreetNameCodeValue option:first").val());
        $("#CityCodeValue").val($("#CityCodeValue option:first").val());
        $("#StateCodeValue").val($("#StateCodeValue option:first").val());
        $("#AddressCodeValue").val($("#AddressCodeValue option:first").val());
        $("#PhoneCodeValue").val($("#PhoneCodeValue option:first").val());
        $("#ZipCodeValue").val($("#ZipCodeValue option:first").val());
        $("#DensityCodeValue").val($("#DensityCodeValue option:first").val());
        $("#UniquenessCodeValue").val($("#UniquenessCodeValue option:first").val());
        $("#SICCodeValue").val($("#SICCodeValue option:first").val());

        $("#CompanyCode").val($("#CompanyCodeValue option:first").val());
        $("#StreetCode").val($("#StreetCodeValue option:first").val());
        $("#StreetName").val($("#StreetNameCodeValue option:first").val());
        $("#CityCode").val($("#CityCodeValue option:first").val());
        $("#StateCode").val($("#StateCodeValue option:first").val());
        $("#AddressCode").val($("#AddressCodeValue option:first").val());
        $("#PhoneCode").val($("#PhoneCodeValue option:first").val());
        $("#ZipCode").val($("#ZipCodeValue option:first").val());
        $("#DensityCode").val($("#DensityCodeValue option:first").val());
        $("#UniquenessCode").val($("#UniquenessCodeValue option:first").val());
        $("#SICCode").val($("#SICCodeValue option:first").val());

        $("#ConfidenceCode").val(ConfidenceCode);
        $("#CompanyGrade").val(MatchGradeText.length > 0 ? MatchGradeText.substring(0, 1) : "Z");
        $("#StreetGrade").val(MatchGradeText.length > 1 ? MatchGradeText.substring(1, 2) : "Z");
        $("#StreetNameGrade").val(MatchGradeText.length > 2 ? MatchGradeText.substring(2, 3) : "Z");
        $("#CityGrade").val(MatchGradeText.length > 3 ? MatchGradeText.substring(3, 4) : "Z");
        $("#StateGrade").val(MatchGradeText.length > 4 ? MatchGradeText.substring(4, 5) : "Z");
        $("#AddressGrade").val(MatchGradeText.length > 5 ? MatchGradeText.substring(5, 6) : "Z");
        $("#PhoneGrade").val(MatchGradeText.length > 6 ? MatchGradeText.substring(6, 7) : "Z");
        $("#ZipGrade").val(MatchGradeText.length > 7 ? MatchGradeText.substring(7, 8) : "Z");
        $("#Density").val(MatchGradeText.length > 8 ? MatchGradeText.substring(8, 9) : "Z");
        $("#Uniqueness").val(MatchGradeText.length > 9 ? MatchGradeText.substring(9, 10) : "Z");
        $("#SIC").val(MatchGradeText.length > 10 ? MatchGradeText.substring(10, 11) : "Z");
        RefreshMultiSelect();

    }
}

$('body').on('click', '#ExcludeFromAutoAccept', function () {

    var box = $(this).prop("checked");
    if (box) {
        $(".ExcludedLable").addClass("Excluded");
    }
    else {
        $(".ExcludedLable").removeClass("Excluded");
    }
});

function OnSuccess() {
    $('#divProgress').hide();
}

function LoadTags() {
    if ($("#AutoAcceptancneTagsList").length > 0) {
        var TagList = $("#AutoAcceptancneTagsList").val().split(',');
        if (TagList != null || TagList != "") {
            $("#TagsValue option").each(function () {
                for (var i = 0; i < TagList.length; i++) {
                    if ($(this).val() == TagList[i]) {
                        $(this).attr("selected", "selected");
                    }
                }
            });
            $("#InsertUpdateAutoAcceptanceModal #form_windowAAC #Tags").val(TagList);
        }
    }
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {
            if (event.target.id == "TagsValue") {
                $("#InsertUpdateAutoAcceptanceModal #form_windowAAC #Tags").val($(this).val());
            }
            if (event.target.id == "ConfidenceCodeValue") {
                $("#InsertUpdateAutoAcceptanceModal #form_windowAAC #ConfidenceCode").val($(this).val());
            }
        });
    }
}
$("body").on('click', '#btnAutoAcceptanceSubmit', function () {
    var count = 0;
    var GroupId = $("#GroupId").val();
    var tag = $("#Tags").val();
    $(".customValidation").each(function () {
        if ($(this).val() == "" || $(this).val() == undefined) {
            var item = "#" + $(this).attr('id') + "Value";
            $(item).next("div").find("button").addClass("custom-has-error")
            count += 1;

        } else {
            var item = "#" + $(this).attr('id') + "Value";
            $(item).next("div").find("button").removeClass("custom-has-error")
        }
    });
    if (GroupId == undefined) {
        $("#spnGroupId").show();
        count += 1;
    }
    else {
        $("#spnGroupId").hide();
    }
    if ($("#IsLicenseEnableTags").val().toLowerCase() == "true") {
        if ($.UserRole.toLowerCase() == 'lob') {
            if (tag == undefined || tag == '') {
                $("#spnTags").show();
                count += 1;
            }
            else {
                $("#spnTags").hide();
            }

        }
    }
    
    if (count > 0) {
        return false;
    }
    $('#divProgress').show();

});
function BuildMultiSelect() {
    showNonSelectedText();
}
function RefreshMultiSelect() {
    $("#ConfidenceCodeValue").multiselect("destroy").multiselect({
        includeSelectAllOption: true,
        nonSelectedText: selectConfidenceCode
    });
    $("#CompanyGradeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectCompanyGrade
    });
    $("#StreetGradeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectStreetGrade
    });
    $("#StreetNameGradeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectStreetNameGrade
    });
    $("#CityGradeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectCityGrade
    });
    $("#StateGradeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectStateGrade
    });
    $("#AddressGradeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectAddressGrade
    });
    $("#PhoneGradeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectPhoneGrade
    });
    $("#ZipGradeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectZipGrade
    });
    $("#DensityValue").multiselect("destroy").multiselect({
        nonSelectedText: selectDensity
    });
    $("#UniquenessValue").multiselect("destroy").multiselect({
        nonSelectedText: selectUniqueness
    });
    $("#SICValue").multiselect("destroy").multiselect({
        nonSelectedText: selectSIC
    });
    $("#CompanyCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectCompanyCode,
        maxHeight: 200
    });
    $("#StreetCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectStreetCode
    });
    $("#StreetNameCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectStreetNameCode
    });
    $("#CityCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectCityCode
    });
    $("#StateCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectStateCode
    });
    $("#AddressCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectAddressCode
    });
    $("#PhoneCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectPhoneCode
    });
    $("#ZipCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectZipCode
    });
    $("#DensityCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectDensityCode
    });
    $("#UniquenessCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectUniquenessCode
    });
    $("#SICCodeValue").multiselect("destroy").multiselect({
        nonSelectedText: selectSICCode
    });
}

$("body").on('change', '#MatchGradeComponentCount', function () {
    MatchGradeComponentChange();
});
function MatchGradeComponentChange() {
    MatchDataCriteriaChanges(true);
    var MatchDataCriteria = $("#MatchDataCriteria").val();
    var MatchValue = $("#MatchGradeComponentCount").val();

    if (MatchValue == "0" && (MatchDataCriteria == "DUNS Number Lookup" && MatchDataCriteria == "National ID Lookup" && MatchDataCriteria == "Telephone Number Lookup" && MatchDataCriteria == "Domain Lookup")) {
        $('#ZipGradeValue').multiselect("enable");
        $('#DensityValue').multiselect("enable");
        $('#UniquenessValue').multiselect("enable");
        $('#SICValue').multiselect("enable");
    }
    else if (MatchValue == "7") {
        $("#ZipGradeValue").val($("#ZipGradeValue option:first").val());
        $("#ZipGradeValue").multiselect("destroy").multiselect({
            includeSelectAllOption: false,
            nonSelectedText: selectZipGrade
        });
        $('#ZipGradeValue').multiselect("disable");


        $("#DensityValue").val($("#DensityValue option:first").val());
        $("#DensityValue").multiselect("destroy").multiselect({
            includeSelectAllOption: false,
            nonSelectedText: selectDensity,
        });
        $('#DensityValue').multiselect("disable");

        $("#UniquenessValue").val($("#UniquenessValue option:first").val());
        $("#UniquenessValue").multiselect("destroy").multiselect({
            includeSelectAllOption: false,
            nonSelectedText: selectUniqueness,
        });
        $('#UniquenessValue').multiselect("disable");


        $("#SICValue").val($("#SICValue option:first").val());
        $("#SICValue").multiselect("destroy").multiselect({
            includeSelectAllOption: false,
            nonSelectedText: selectSIC
        });
        $('#SICValue').multiselect("disable");
    }
    else if (MatchValue == "11" && (MatchDataCriteria == "DUNS Number Lookup" && MatchDataCriteria == "National ID Lookup" && MatchDataCriteria == "Telephone Number Lookup" && MatchDataCriteria == "Domain Lookup")) {
        $('#ZipGradeValue').multiselect("enable");
        $('#DensityValue').multiselect("enable");
        $('#UniquenessValue').multiselect("enable");
        $('#SICValue').multiselect("enable");
    }
}


$("body").on('click', '#btnAutoAcceptancePreview', function () {
    var count = 0;
    var GroupId = $("#GroupId").val();
    var Tags = $("#Tags").val();
    $(".customValidation").each(function () {
        if ($(this).val() == "" || $(this).val() == undefined) {
            var item = "#" + $(this).attr('id') + "Value";
            $(item).next("div").find("button").addClass("custom-has-error")
            count += 1;

        } else {
            var item = "#" + $(this).attr('id') + "Value";
            $(item).next("div").find("button").removeClass("custom-has-error")
        }
    });
    if (GroupId == undefined) {
        $("#spnGroupId").show();
        count += 1;
    }
    else {
        $("#spnGroupId").hide();
    }
    if ($("#UserRole").val().toLowerCase() == 'lob') {
        if (Tags == undefined || Tags == '') {
            $("#spnTags").show();
            count += 1;
        }
        else {
            $("#spnTags").hide();
        }
    }

    if (count > 0) {
        return false;
    }

    var objAutoSetting = $('#form_windowAAC').serialize();
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'POST',
        url: "/DNBIdentityResolution/PreviewAutoAcceptance",
        dataType: 'HTML',
        data: objAutoSetting,
        success: function (data) {
            $("#PreviewAutoAcceptanceModalMain").html(data);
            DraggableModalPopup("#PreviewAutoAcceptanceModal");
        }
    });
});

function ClosePreviewPopUp() {
    $("#PreviewAutoAcceptanceModal").modal("hide");
    $("#InsertUpdateAutoAcceptanceModal").modal("hide");
    var isRefresh = $("#isAutoAcceptance").val();
    if (isRefresh.toLowerCase() == "false") {
        $("#PreviewAutoAcceptanceModal").modal("hide");
        $("#InsertUpdateAutoAcceptanceModal").modal("hide");
    }
    window.ReloadPage();
}
$(document).on('click', '.btnPreviewApplyAutoAcceptance', function () {
    $.ajax({
        type: 'GET',
        url: "/DNBIdentityResolution/PreviewApplyAutoAcceptance" ,
        success: function (data) {
            bootbox.alert({
                title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
                message: data.data, callback: function () {
                    ClosePreviewPopUp();
                }
            });
        }
    });
});

$(document).on('change', '#CompanyGradeValue', function () {
    //var IsLicenseEnableAdvancedMatch = $("#IsLicenseEnableAdvancedMatch").val();
    //if (IsLicenseEnableAdvancedMatch.toLowerCase() == "true") {
    var thisValue = $(this).val();
    getScoreDropdown(thisValue);
    //}
});

function getScoreDropdown(thisValue) {

    var startScore = 0;
    var EndScore = 100;
    if (thisValue != null) {
        if (jQuery.inArray("Z", thisValue) !== -1) {
            EndScore = 0;
            $("#CompanyScore").attr("disabled", "disabled")
        }
        else if ((jQuery.inArray("F", thisValue) !== -1 && jQuery.inArray("B", thisValue) !== -1 && jQuery.inArray("A", thisValue) !== -1) || jQuery.inArray("#", thisValue) !== -1) {
            startScore = 0;
            EndScore = 100;
        }
        else if ((jQuery.inArray("B", thisValue) !== -1 && jQuery.inArray("A", thisValue) !== -1)) {
            startScore = 37;
            EndScore = 100;
        }
        else if ((jQuery.inArray("B", thisValue) !== -1 && jQuery.inArray("F", thisValue) !== -1)) {
            startScore = 0;
            EndScore = 80;
        }
        else if ((jQuery.inArray("A", thisValue) !== -1 && jQuery.inArray("F", thisValue) !== -1)) {
            startScore = 0;
            EndScore = 36;
        }

        else if (jQuery.inArray("F", thisValue) !== -1) {
            EndScore = 36;
        }
        else if (jQuery.inArray("B", thisValue) !== -1) {
            startScore = 37;
            EndScore = 80;
        }
        else if (jQuery.inArray("A", thisValue) !== -1) {
            startScore = 81;
        }
    }
    var IsLicenseEnableAdvancedMatch = $("#IsLicenseEnableAdvancedMatch").val();
    if (jQuery.inArray("Z", thisValue) == -1 && IsLicenseEnableAdvancedMatch.toLowerCase() == "true") {
        $("#CompanyScore").attr("disabled", false)
    }

    $('#CompanyScore').empty();
    var $select = $("#CompanyScore");
    for (i = startScore; i <= EndScore; i++) {
        $select.append($('<option></option>').val(i).html(i))
    }
}

$(document).on('change', '#ZipGradeValue', function () { MatchgreadValueChange() });

$(document).on('change', '#DensityValue', function () { MatchgreadValueChange() });

$(document).on('change', '#UniquenessValue', function () { MatchgreadValueChange() });

$(document).on('change', '#SICValue', function () { MatchgreadValueChange() });

function MatchgreadValueChange() {
    $('#MatchGradeComponentCount').val('11');
}

function OnReadyInsertUpdateAutoAcceptance() {
    $(".AutoAcceptanceMatchGrade select").each(function () {
        var id = $(this).attr("id").replace("Value", "");
        var hiddenList = "#" + id + "List";
        var ddVale = "#" + id + "Value";
        var valueofList = $(hiddenList).val();
        var lstValue = valueofList.split(',');
        $(ddVale + "  option").each(function () {
            for (var i = 0; i < lstValue.length; i++) {
                if ($(this).val() == lstValue[i]) {
                    $(this).attr("selected", "selected");
                }
            }
        });

    });
    if ($("#UserRole").val().toLowerCase() == 'lob') {
        $(".chzn-select").attr("data-placeholder", "Add Tags (Required)");
        $(".chzn-select").trigger("chosen:updated");
    }
    BuildMultiSelect();
    LoadTags();
    MatchGradeComponentChange();
}
$("body").on('change', '#MatchDataCriteria', function () {
    MatchDataCriteriaChanges(false);
});
function MatchDataCriteriaChanges(isEdit) {
    var MatchDataCriteria = $("#MatchDataCriteria").val();
    var MatchGradeComponentCount = $("#MatchGradeComponentCount").val();
    var ConfidenceCode;
    if (MatchDataCriteria == "DUNS Number Lookup" || MatchDataCriteria == "National ID Lookup" || MatchDataCriteria == "Telephone Number Lookup") {
        $('#ConfidenceCodeValue').multiselect('clearSelection');
        ConfidenceCode = $('#ConfidenceCodeValue').val(10);
        ConfidenceCode = $('#ConfidenceCodeValue').val();
        $('#ConfidenceCodeValue').multiselect('select', ConfidenceCode);
        $('#ConfidenceCodeValue').multiselect("disable");
        IsDefaultMatchGradeAndCode();
        DisableAllMultiselectDropdowns('disable');
    }
    else if (MatchDataCriteria == "Domain Lookup") {
        $('#ConfidenceCodeValue').multiselect('clearSelection');
        ConfidenceCode = $('#ConfidenceCodeValue').val();
        ConfidenceCode = ['8', '9', '10'];
        $('#ConfidenceCodeValue').multiselect('select', ConfidenceCode);
        $('#ConfidenceCodeValue').multiselect("disable");
        IsDefaultMatchGradeAndCode();
        DisableAllMultiselectDropdowns('disable');
    }
    else {
        if (!isEdit) {
            ConfidenceCode = "";
            $('#ConfidenceCodeValue').multiselect('select', ConfidenceCode);
            $('#ConfidenceCodeValue').multiselect('clearSelection');
        }
        else {
            ConfidenceCode = $('#ConfidenceCodeValue').val();
            $('#ConfidenceCodeValue').multiselect('select', ConfidenceCode);
        }
        $('#ConfidenceCodeValue').multiselect("enable");
        DisableAllMultiselectDropdowns('enable');
        if (MatchGradeComponentCount == 7) {
            $("#ZipGradeValue").multiselect("disable");
            $("#DensityValue").multiselect("disable");
            $("#UniquenessValue").multiselect("disable");
            $("#SICValue").multiselect("disable");
        }
    }

}

function DisableAllMultiselectDropdowns(dropdownSelectValue) {
    $('#CompanyGradeValue').multiselect(dropdownSelectValue);
    $('#CompanyCodeValue').multiselect(dropdownSelectValue);
    $('#StreetGradeValue').multiselect(dropdownSelectValue);
    $('#StreetCodeValue').multiselect(dropdownSelectValue);
    $('#StreetNameGradeValue').multiselect(dropdownSelectValue);
    $('#StreetNameCodeValue').multiselect(dropdownSelectValue);
    $('#CityGradeValue').multiselect(dropdownSelectValue);
    $('#CityCodeValue').multiselect(dropdownSelectValue);
    $('#StateGradeValue').multiselect(dropdownSelectValue);
    $('#StateCodeValue').multiselect(dropdownSelectValue);
    $('#AddressGradeValue').multiselect(dropdownSelectValue);
    $('#AddressCodeValue').multiselect(dropdownSelectValue);
    $('#PhoneGradeValue').multiselect(dropdownSelectValue);
    $('#PhoneCodeValue').multiselect(dropdownSelectValue);
    $('#ZipGradeValue').multiselect(dropdownSelectValue);
    $('#ZipCodeValue').multiselect(dropdownSelectValue);
    $('#DensityValue').multiselect(dropdownSelectValue);
    $('#DensityCodeValue').multiselect(dropdownSelectValue);
    $('#UniquenessValue').multiselect(dropdownSelectValue);
    $('#UniquenessCodeValue').multiselect(dropdownSelectValue);
    $('#SICValue').multiselect(dropdownSelectValue);
    $('#SICCodeValue').multiselect(dropdownSelectValue);
}
function IsDefaultMatchGradeAndCode() {
    $("#CompanyGradeValue").val($("#CompanyGradeValue option:first").val());
    $("#CompanyGradeValue").multiselect("destroy");
    $("#CompanyCodeValue").val($("#CompanyCodeValue option:first").val());
    $("#CompanyCodeValue").multiselect("destroy");
    $("#StreetGradeValue").val($("#StreetGradeValue option:first").val());
    $("#StreetGradeValue").multiselect("destroy");
    $("#StreetCodeValue").val($("#StreetCodeValue option:first").val());
    $("#StreetCodeValue").multiselect("destroy");
    $("#StreetNameGradeValue").val($("#StreetNameGradeValue option:first").val());
    $("#StreetNameGradeValue").multiselect("destroy");
    $("#StreetNameCodeValue").val($("#StreetNameCodeValue option:first").val());
    $("#StreetNameCodeValue").multiselect("destroy");
    $("#CityGradeValue").val($("#CityGradeValue option:first").val());
    $("#CityGradeValue").multiselect("destroy");
    $("#CityCodeValue").val($("#CityCodeValue option:first").val());
    $("#CityCodeValue").multiselect("destroy");
    $("#StateGradeValue").val($("#StateGradeValue option:first").val());
    $("#StateGradeValue").multiselect("destroy");
    $("#StateCodeValue").val($("#StateCodeValue option:first").val());
    $("#StateCodeValue").multiselect("destroy");
    $("#AddressGradeValue").val($("#AddressGradeValue option:first").val());
    $("#AddressGradeValue").multiselect("destroy");
    $("#AddressCodeValue").val($("#AddressCodeValue option:first").val());
    $("#AddressCodeValue").multiselect("destroy");
    $("#PhoneGradeValue").val($("#PhoneGradeValue option:first").val());
    $("#PhoneGradeValue").multiselect("destroy");
    $("#PhoneCodeValue").val($("#PhoneCodeValue option:first").val());
    $("#PhoneCodeValue").multiselect("destroy");
    $("#ZipGradeValue").val($("#ZipGradeValue option:first").val());
    $("#ZipGradeValue").multiselect("destroy");
    $("#ZipCodeValue").val($("#ZipCodeValue option:first").val());
    $("#ZipCodeValue").multiselect("destroy");
    $("#DensityValue").val($("#DensityValue option:first").val());
    $("#DensityValue").multiselect("destroy");
    $("#DensityCodeValue").val($("#DensityCodeValue option:first").val());
    $("#DensityCodeValue").multiselect("destroy");
    $("#UniquenessValue").val($("#UniquenessValue option:first").val());
    $("#UniquenessValue").multiselect("destroy");
    $("#UniquenessCodeValue").val($("#UniquenessCodeValue option:first").val());
    $("#UniquenessCodeValue").multiselect("destroy");
    $("#SICValue").val($("#SICValue option:first").val());
    $("#SICValue").multiselect("destroy");
    $("#SICCodeValue").val($("#SICCodeValue option:first").val());
    $("#SICCodeValue").multiselect("destroy");
    showNonSelectedText();
}
function showNonSelectedText() {
    $('#ConfidenceCodeValue').multiselect({
        includeSelectAllOption: true,
        nonSelectedText: selectConfidenceCode
    });
    $('#CompanyGradeValue').multiselect({
        nonSelectedText: selectCompanyGrade
    });
    $('#CompanyCodeValue').multiselect({
        nonSelectedText: selectCompanyCode
    });
    $('#StreetGradeValue').multiselect({
        nonSelectedText: selectStreetGrade
    });
    $('#StreetCodeValue').multiselect({
        nonSelectedText: selectStreetCode
    });
    $('#StreetNameGradeValue').multiselect({
        nonSelectedText: selectStreetNameGrade
    });
    $('#StreetNameCodeValue').multiselect({
        nonSelectedText: selectStreetNameCode
    });
    $('#CityGradeValue').multiselect({
        nonSelectedText: selectCityGrade
    });
    $('#CityCodeValue').multiselect({
        nonSelectedText: selectCityCode
    });
    $('#StateGradeValue').multiselect({
        nonSelectedText: selectStateGrade
    });
    $('#StateCodeValue').multiselect({
        nonSelectedText: selectStateCode
    });
    $('#AddressGradeValue').multiselect({
        nonSelectedText: selectAddressGrade
    });
    $('#AddressCodeValue').multiselect({
        nonSelectedText: selectAddressCode
    });
    $('#PhoneGradeValue').multiselect({
        nonSelectedText: selectPhoneGrade
    });
    $('#PhoneCodeValue').multiselect({
        nonSelectedText: selectPhoneCode
    });
    $('#ZipGradeValue').multiselect({
        nonSelectedText: selectZipGrade
    });
    $('#ZipCodeValue').multiselect({
        nonSelectedText: selectZipCode
    });
    $('#DensityValue').multiselect({
        nonSelectedText: selectDensity
    });
    $('#DensityCodeValue').multiselect({
        nonSelectedText: selectDensityCode
    });
    $('#UniquenessValue').multiselect({
        nonSelectedText: selectUniqueness
    });
    $('#UniquenessCodeValue').multiselect({
        nonSelectedText: selectUniquenessCode
    });
    $('#SICValue').multiselect({
        nonSelectedText: selectSIC
    });
    $('#SICCodeValue').multiselect({
        nonSelectedText: selectSICCode
    });
}