$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});
function LoadTags() {
    if ($("#OIAutoAcceptancneTagsList").length > 0) {
        var TagList = $("#OIAutoAcceptancneTagsList").val().split(',');
        if (TagList != null || TagList != "") {
            $("#TagsValue option").each(function () {
                for (var i = 0; i < TagList.length; i++) {
                    if ($(this).val() == TagList[i]) {
                        $(this).attr("selected", "selected");
                    }
                }
            });
            $("#Tags").val(TagList);
        }
    }
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {
            if (event.target.id == "TagsValue") {
                $("#Tags").val($(this).val());
            }
        });
    }
}
function onchangeMultiSelect(option, checked) { 
    var id = option[0].parentElement.id;
    var ControllerId = "#" + option[0].parentElement.id;
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
function BuildMultiSelect() {
    $('#MG_CompanyValue').multiselect({
        nonSelectedText: selectCompanyGrade,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MDP_CompanyValue').multiselect({
        nonSelectedText: selectCompanyCode,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_StreetNoValue').multiselect({
        nonSelectedText: selectStreetGrade,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_StreetNameValue').multiselect({
        nonSelectedText: selectStreetNameGrade,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_CityValue').multiselect({
        nonSelectedText: selectCityGrade,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_StateValue').multiselect({
        nonSelectedText: selectStateGrade,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_PostalCodeValue').multiselect({
        nonSelectedText: selectPostalCodeGrade,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_PhoneValue').multiselect({
        nonSelectedText: selectPhoneGrade,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_WebdomainValue').multiselect({
        nonSelectedText: selectWebDomainGrade,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MDP_WebdomainValue').multiselect({
        nonSelectedText: selectWebDomainCode,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_CountryValue').multiselect({
        nonSelectedText: selectCountryGrade,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $('#MG_EINValue').multiselect({
        nonSelectedText: selectEINGrade,
        numberDisplayed: 2,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });

}
function SuccessInsertUpdate(data) {
    var url = '/OISetting/IndexOIAutoAcceptance/';
    ShowMessageNotification("success", data.message, false, false, LoadOIAutoAcceptance(url));
    if (data.result) {
        $("#AddOIAutoAcceptanceModal").modal("hide");
    }
}


$(document).on("change", "#ConfidenceCodeMin", function () {
    var thisValue = $(this).val();
    $('#ConfidenceCodeMax').empty();
    for (var i = parseInt(thisValue); i <= 100; i++) {
        $("#ConfidenceCodeMax").append(new Option(i, i));
    }
});
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
        parent.ShowMessageNotification("success", confidenceCodeMinMaxValue, false);
        return false;
    }
    if (count > 0) {
        return false;
    }
    else {
        return true;
    }
}
function CallbackCountryGroup(CountryGroupName) {
    $.magnificPopup.close();
    $("#CountryGroupId").append(new Option(CountryGroupName.split("@#$")[0], CountryGroupName.split("@#$")[1]));
}
$('body').on('click', '#ExcludeFromAutoAccept', function () {

    var box = $(this).prop("checked");
    if (box) {
        $("#ExcludedLable").addClass("Excluded");
    }
    else {
        $("#ExcludedLable").removeClass("Excluded");
    }
});
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
    $("#PreferLinkedRecord").prop("checked", false);
    $("#AcceptActiveOnly").prop("checked", false);
    $("#ExcludeFromAutoAccept").prop("checked", false);
    $('#Score_Company').val("0");
    $('#Score_StreetName').val("0");
    $("#ConfidenceCodeMin").val("80");
    $("#ConfidenceCodeMax").val("100");
    $("#CountryGroupId").val($("#CountryGroupId option:first").val());
    $("#Tags").val("");
    $("#ExcludedLable").removeClass("Excluded");
}
function RefreshMultiselect() {
    $('#MG_CompanyValue').multiselect("refresh");
    $('#MDP_CompanyValue').multiselect("refresh");
    $('#MG_StreetNoValue').multiselect("refresh");
    $('#MG_StreetNameValue').multiselect("refresh");
    $('#MG_CityValue').multiselect("refresh");
    $('#MG_StateValue').multiselect("refresh");
    $('#MG_PostalCodeValue').multiselect("refresh");
    $('#MG_PhoneValue').multiselect("refresh");
    $('#MG_WebdomainValue').multiselect("refresh");
    $('#MDP_WebdomainValue').multiselect("refresh");
    $('#MG_CountryValue').multiselect("refresh");
    $('#MG_EINValue').multiselect("refresh");
}