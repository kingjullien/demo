$("#txtCompany").keypress(function () {
    var APItype = $("#APItype").val();
    if (APItype == "DirectPlus") {
        LoadTypeAhead();
    }
});

$('body').on('change', '#Country', function () {
    var country = $(this).val();
    changeLanguage(country);
    var APItype = $("#APItype").val();
    if (APItype == "DirectPlus") {
        LoadTypeAhead();
    }
});

function LoadTypeAhead() {
    if ($(".TypeAheadToggle").is(':checked')) {
        var defaultCountryCode = $('#Country').val();
        if (defaultCountryCode != undefined && defaultCountryCode != "") {
            var listOfCompany = [];
            var dataSrc = function (request, response) {
                listOfCompany = [];
                ShowLoadingImageCompany();
                setTimeout(function () {
                    $.ajax({
                        type: "GET",
                        url: "/SearchData/SearchDataCompanyNameTypeAhead",
                        data: { "paramater": request.term, "defaultCountryCode": defaultCountryCode },
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            HideLoadingImageCompany();
                            var JSONResponse = JSON.parse(data);
                            if (JSONResponse.error != undefined && JSONResponse.error.errorMessage != null) {
                                //ShowMessageNotification("success", JSONResponse.error.errorMessage, true);
                                return false;
                            }
                            else {
                                response($.map(JSONResponse.searchCandidates, function (value, key) {
                                    return {
                                        label: value.organization.primaryName + (value.organization.primaryAddress.streetAddress != undefined ? ', ' + value.organization.primaryAddress.streetAddress.line1 : '') + (value.organization.primaryAddress.addressLocality != undefined ? ", " + value.organization.primaryAddress.addressLocality.name : ''),
                                        value: value.organization.primaryName,
                                        duns: value.organization.duns,
                                        address: (value.organization.primaryAddress.streetAddress != undefined ? ', ' + value.organization.primaryAddress.streetAddress.line1 : '') + (value.organization.primaryAddress.addressLocality != undefined ? ", " + value.organization.primaryAddress.addressLocality.name : ''),
                                        address1: (value.organization.primaryAddress.streetAddress != undefined ? value.organization.primaryAddress.streetAddress.line1 : ''),
                                        city: value.organization.primaryAddress.addressLocality != undefined ? value.organization.primaryAddress.addressLocality.name : '',
                                        state: value.organization.primaryAddress.addressRegion != undefined ? value.organization.primaryAddress.addressRegion.name : '',
                                        country: value.organization.primaryAddress.addressCountry.isoAlpha2Code != undefined ? value.organization.primaryAddress.addressCountry.isoAlpha2Code : '',
                                        fullDetail: value
                                    };
                                }));
                            }
                        }
                    });
                }, 1000);
            };
        }
        $("#txtCompany").autocomplete({
            source: dataSrc,
            minLength: 3,
            select: function (event, ui) {
                FinalResponse = JSON.stringify(ui.item);
                $("#form_SearchBeneficialOwnershipData #txtCompany").val(ui.item.value);
                $("#form_SearchBeneficialOwnershipData #txtAddress").val(ui.item.address1);
                $("#form_SearchBeneficialOwnershipData #txtCity").val(ui.item.city);
                $("#form_SearchBeneficialOwnershipData #txtState").val(ui.item.state);
                $("#form_SearchBeneficialOwnershipData #Country").val(ui.item.country);

                $("form_SearchBeneficialOwnershipData #txtZip").val("");
                $("form_SearchBeneficialOwnershipData #txtPhone").val("");

                $('.divSearchOption #btnSearchData').focus();
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
    }
}

function changeLanguage(country) {
    $("#Language").find("option").remove();
    $("#Language").append('<option value=>Select Language</option>');
    if (country.toLowerCase() == "us") {
        $("#Language").append('<option value=en-US>English</option>');
        $("#Language").val("en-US");
    }
    else if (country.toLowerCase() == "jp") {
        $("#Language").append('<option value=en-US>English</option>');
        $("#Language").append('<option value=ja-JP>Japanese</option>');
        $("#Language").val("ja-JP");
    }
    else if (country.toLowerCase() == "tw") {
        $("#Language").append('<option value=en-US>English</option>');
        $("#Language").append('<option value=zh-hant-TW>Traditional Chinese</option>');
        $("#Language").val("zh-hant-TW");
    }
    else if (country.toLowerCase() == "kr") {
        $("#Language").append('<option value=en-US>English</option>');
        $("#Language").append('<option value=ko-hang-KR>Hangul</option>');
        $("#Language").val("ko-hang-KR");
    }
    else if (country.toLowerCase() == "cn") {
        $("#Language").append('<option value=en-US>English</option>');
        $("#Language").append('<option value=zh-hans-CN>Simplified Chinese</option>');
        $("#Language").val("zh-hans-CN");
    }
    else {
        $("#Language").append('<option value=en-US>English</option>');
        $("#Language").append('<option value=ja-JP>Japanese</option>');
        $("#Language").append('<option value=zh-hans-CN>Simplified Chinese</option>');
        $("#Language").append('<option value=zh-hant-TW>Traditional Chinese</option>');
        $("#Language").append('<option value=ko-hang-KR>Hangul</option>');
        $("#Language").val("en-US");
    }
}

$(document).on('change', '.TypeAheadToggle', function () {
    if (!$(".TypeAheadToggle").is(':checked')) {
        $(".lblTypeAhead").removeClass("thColor");
        if ($("#txtCompany").autocomplete("instance") != undefined) {
            $("#txtCompany").autocomplete("disable");
        }
    }
    else {
        $(".lblTypeAhead").addClass("thColor");
        if ($("#txtCompany").autocomplete("instance") != undefined) {
            $("#txtCompany").autocomplete("enable");
        }
    }
});

function ShowLoadingImageCompany() {
    $("#imgCompanyLoad").show();
}

function HideLoadingImageCompany() {
    $("#imgCompanyLoad").hide();
}