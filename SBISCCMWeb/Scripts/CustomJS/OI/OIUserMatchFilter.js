function onLoadFineTuneMatchFilter() {
    $(".OIAutoAcceptanceMatchGrade select").each(function () {
        var id = $(this).attr("id").replace("Value", "");
        var hiddenValue = "#" + id;
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
    var FilterId = $("#FilterId").val();
    if (FilterId && FilterId == "0") {
        ResetValues();
    }
    else {
        var ConfidenceCodeMin = $("#ConfidenceCodeMin").val();
        var ConfidenceCodeMax = $("#ConfidenceCodeMax").val();
        $('#ConfidenceCodeMax').empty();
        for (var i = parseInt(ConfidenceCodeMin); i <= 100; i++) {
            $("#ConfidenceCodeMax").append(new Option(i, i));
        }
        if (parseInt(ConfidenceCodeMin) < parseInt(ConfidenceCodeMax)) {
            $("#ConfidenceCodeMax").val(ConfidenceCodeMax);
        }
    }
    BuildMultiSelect();
}
function ResetValues() {
    $('#MG_CompanyValue').val("#");
    $('#MDP_CompanyValue').val("##");
    $('#MG_StreetNoValue').val("#");
    $('#MG_StreetNameValue').val("#");
    $('#MG_CityValue').val("#");
    $('#MG_StateValue').val("#");
    $('#MG_PostalCodeValue').val("#");
    $('#MG_PhoneValue').val("#");
    $('#MG_WebdomainValue').val("#");
    $('#MDP_WebdomainValue').val("##");
    $('#MG_CountryValue').val("#");
    $('#MG_EINValue').val("#");
    $('#MG_Company').val("#");
    $('#MDP_Company').val("##");
    $('#MG_StreetNo').val("#");
    $('#MG_StreetName').val("#");
    $('#MG_City').val("#");
    $('#MG_State').val("#");
    $('#MG_PostalCode').val("#");
    $('#MG_Phone').val("#");
    $('#MG_Webdomain').val("#");
    $('#MDP_Webdomain').val("##");
    $('#MG_Country').val("#");
    $('#MG_EIN').val("#");
    $('#MG_Company').val("#");
    $('#MDP_Company').val("##");
    $('#MG_StreetNo').val("#");
    $('#MG_StreetName').val("#");
    $('#MG_City').val("#");
    $('#MG_State').val("#");
    $('#MG_PostalCode').val("#");
    $('#MG_Phone').val("#");
    $('#MG_Webdomain').val("#");
    $('#MDP_Webdomain').val("##");
    $('#MG_Country').val("#");
    $('#MG_EIN').val("#");
    $("#AcceptActiveOnly").prop("checked", false);
    $("#ExcludeFromAutoAccept").prop("checked", false);
    $('#Score_Company').val("0");
    $('#Score_StreetName').val("0");
    $("#ConfidenceCodeMin").val("80");
    $("#ConfidenceCodeMax").val("100");
    $("#ExcludedLable").removeClass("Excluded");
}
function BuildMultiSelect() {
    $('#MG_CompanyValue').multiselect({
        nonSelectedText: 'Select Company Grade',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MDP_CompanyValue').multiselect({
        nonSelectedText: 'Select Company Code',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_StreetNoValue').multiselect({
        nonSelectedText: 'Select Street Grade',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_StreetNameValue').multiselect({
        nonSelectedText: 'Select Street Name Grade',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_CityValue').multiselect({
        nonSelectedText: 'Select City Grade',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_StateValue').multiselect({
        nonSelectedText: 'Select State Grade',
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_PostalCodeValue').multiselect({
        nonSelectedText: 'Select Postal Code Grade',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_PhoneValue').multiselect({
        nonSelectedText: 'Select Phone Grade',
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_WebdomainValue').multiselect({
        nonSelectedText: 'Select Web Domain Grade',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MDP_WebdomainValue').multiselect({
        nonSelectedText: 'Select Web Domain Code',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_CountryValue').multiselect({
        nonSelectedText: 'Select Country Grade',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_EINValue').multiselect({
        nonSelectedText: 'Select EIN Grade',
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });

}
function onchangeMultiSelect(option, checked) {
    var id = option[0].parentElement.id;
    var ControllerId = "#" + option[0].parentElement.id;
    var chkValue = "";
    if (option[0].value == "#") {
        $(ControllerId).val("#");
        $(ControllerId).multiselect("refresh");
        if (checked) {
            $(ControllerId).val("#");
        }
        else {
            $(ControllerId).val("");
        }
        $(ControllerId).multiselect("refresh");
    }
    else {
        $(ControllerId).parent().parent().find("li").each(function () {
            if (($(this).find("input").attr("value") == "#" || $(this).find("input").attr("value") == "##")) {
                $(this).find("input").prop("checked", false);
                var v = jQuery.grep($(ControllerId).val(), function (value) {
                    return value != "#";
                });
                $(ControllerId).val(v);
                $(ControllerId).multiselect("refresh");

            }
        });
    }

    var idValue = $(ControllerId).val();
    var setId = id.replace("Value", "");
    if (idValue == null) {
        $("#" + setId).val("");
    }
    else {
        $("#" + setId).val(idValue.toString());
    }
}
function VAlidateInputes() {
    var count = 0;
    $(".customValidation").each(function () {
        if ($(this).val() == "" || $(this).val() == undefined) {
            var item = "#" + $(this).attr('id') + "Value";
            $(item).next("div").find("button").addClass("custom-has-error");
            count += 1;

        } else {
            var item = "#" + $(this).attr('id') + "Value";
            $(item).next("div").find("button").removeClass("custom-has-error");
        }
    });
    var ConfidenceCodeMin = $("#ConfidenceCodeMin").val();
    var ConfidenceCodeMax = $("#ConfidenceCodeMax").val();
    if (parseInt(ConfidenceCodeMin) > parseInt(ConfidenceCodeMax)) {
        ShowMessageNotification("success", maxValue, false);
        return false;
    }

    if (count > 0) {
        return false;
    }
    else {
        return true;
    }
}
function SuccessInsertUpdateFineTuneMatchFilter(data) {
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#OIFineTuneMatchFilterModal").modal("hide");
    }
}
$('body').on('click', '#Exclude', function () {
    var box = $(this).prop("checked");
    if (box) {
        $("#ExcludedLable").addClass("Excluded");
    }
    else {
        $("#ExcludedLable").removeClass("Excluded");
    }
});
$(document).on("change", "#ConfidenceCodeMin", function () {
    var thisValue = $(this).val();
    $('#ConfidenceCodeMax').empty();
    for (var i = parseInt(thisValue); i <= 100; i++) {
        $("#ConfidenceCodeMax").append(new Option(i, i));
    }
});