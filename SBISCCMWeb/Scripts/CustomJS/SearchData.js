String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};
// Display and Hide loader bar for every ajax call
$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    // setup_dashboard_widgets_desktop();
    $('#divProgress').hide();
});

$(document).ready(function () {
    var country = $("#hdnCountry").val();
    $("select#Country").val(encodeURI(country));
    $("select#Language").val("en-US");

    var ddcountry = $("#Country").val();
    changeLanguage(ddcountry);
    SetSearchByCompanyValidation();
    SetSearchByAddressValidation();
});
// open match detail review popup
$('body').on('click', '.clsViewMatchedItemDetails', function () {
    $(".currentRow").each(function () {
        $(this).removeClass("current");
    });
    $(this).parent().parent().parent().addClass("current");
    $(this).parent().parent().parent().next().addClass("current");
    //Vijay - New code to use data from parent tr
    var row = $(this).closest('tr');
    var next = row.nextAll().eq(1);
    var prev = row.prevAll().eq(1);

    var data = row.attr("data-val");
    var dataNext = "";
    var dataPrev = "";
    if (next.length > 0) {
        dataNext = next.attr("id");
    }

    if (prev.length > 0) {
        dataPrev = prev.attr("id");
    }

    var DUNS = row.attr("id");
    //Vijay - New Code ends here

    //var QueryString = 'id:' + DUNS + '@#$childButtonId:' + this.id + '@#$dataNext:' + dataNext + '@#$dataPrev:' + dataPrev + '@#$IsPartialView:false';
    var QueryString = "id:" + 0 + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + this.id + '@#$type:null' + '@#$IsPartialView:false';

    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/SearchData/cShowMatchedItesDetailsView?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#MatchDetailModalMain").html(data);
            DraggableModalPopup("#MatchDetailModal");
        }
    });
});
// Next button event for Match Detail Review Popup
function matchItemNextClick(dunsNumb) {
    if (dunsNumb != "") {

        var currentid = "#" + dunsNumb;
        var nextcurrentid = "#" + dunsNumb + "-2";
        $(currentid).prev().removeClass("current");
        $(currentid).prev().prev().removeClass("current");
        $(currentid).addClass("current");
        $(nextcurrentid).addClass("current");
    }
}
// Previous button event for Match Detail Review Popup
function matchItemPrevClick(dunsNumb) {
    if (dunsNumb != "") {

        var currentid = "#" + dunsNumb;
        var nextcurrentid = "#" + dunsNumb + "-2";
        $(nextcurrentid).next().removeClass("current");
        $(nextcurrentid).next().next().removeClass("current");
        $(currentid).addClass("current");
        $(nextcurrentid).addClass("current");
    }
}
// Set current row as selected and set color and remove previous row color.
$('body').on('click', '.currentRow', function () {

    $(".currentRow").each(function () {
        $(this).removeClass("current");
    });
    $(this).addClass("current");

    var id = "#" + $(this).attr('id');
    if (id.indexOf('-2') > -1) {
        id = id.substr(0, id.length - 2);
        $(id).addClass("current");
    } else {
        id = id + "-2";
        $(id).addClass("current");
    }
});
function ReloadSuccess() {
    setup_dashboard_widgets_desktop();
}
// on Search Button click validate .
$('body').on('click', '#btnSearchData', function () {
    var CompanyName = $("#txtCompany").val();
    var Country = $("select#Country").prop('selectedIndex');
    var cnt = 0;
    if (CompanyName == '') {
        $("#spnCompany").show();
        $(".GoogleMapPopUp").hide();
        cnt++;
    } else {
        $("#spnCompany").hide();
    }
    if (Country == 0) {
        $("#spnCountry").show();
        cnt++;
    }
    else {
        $("#spnCountry").hide();
    }
    if (cnt > 0) {
        $("#divSearchData table").remove();
        $("#divSearchData table").html("<p>No data are available</p>");
        return false;
    }

});
// Open popup for the Seach Duns number
$('body').on('click', '.btnSearchDUNS', function () {
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/SearchData/DUNSPopup'
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: 'popupSearchDUNS'
    });

    return false;
});
// Update the Search data 
function updateSearchData(DUNSNO) {
    var QueryString = "DUNSNO:" + DUNSNO + '@#$FromPage:SearchData' + "@#$SrcRecId:" + "" + "@#$InputId:" + "";
    $('#divProgress').show();
    $.ajax({
        type: "POST",
        url: "/SearchData/UpdateSearchData?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        success: function (data) {
            $('#divSearchData').html(data);
            // Changes for Converting magnific popup to modal popup
            $("#SearchByAltFieldsModal").modal("hide");
            resetValue();
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }

    });

    setTimeout(function () { $('#divProgress').hide(); }, 10000);
}
function DomainOrEmailSearchData(searchValue, searchType, IsCleanSearch, Country) {
    $('#divProgress').show();
    var QueryString = "searchValue:" + searchValue + "@#$searchType:" + searchType + "@#$IsCleanSearch:" + IsCleanSearch + "@#$Country:" + Country;
    $.ajax({
        type: "POST",
        url: "/SearchData/DomainOrEmailPopup?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        success: function (data) {
            $('#divSearchData').html(data);
            // Changes for Converting magnific popup to modal popup
            $('#SearchByAltFieldsModal').modal("hide");
            resetValue();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    setTimeout(function () { $('#divProgress').hide(); }, 10000);
}


function RegistrationNumberSearchData(RegistrationNoValue, searchType, IsCleanSearch, Country, SrcRecId, InputId) {
    $('#divProgress').show();
    var QueryString = "RegistrationNoValue:" + RegistrationNoValue + "@#$searchType:" + searchType + "@#$IsCleanSearch:" + IsCleanSearch + "@#$Country:" + Country + "@#$SrcRecId:" + SrcRecId + "@#$InputId:" + InputId;
    $.ajax({
        type: "POST",
        url: "/SearchData/RegistrationNumberPopup?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        success: function (data) {
            $('#divSearchData').html(data);
            // Changes for Converting magnific popup to modal popup
            $("#SearchByAltFieldsModal").modal("hide");
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    setTimeout(function () { $('#divProgress').hide(); }, 10000);
}

// Manage content menu and fill the matchdata and also set the value for the Investigation.
$(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var txtActivateFeature = $('#ActivateFeature').val();
    var Investigation = $("#LicenseEnableInvestigations").val();
    var APIType = $("#APITypeForInvestigation").val();
    var Compliance = $("#LicenseEnableCompliance").val();
    if (Investigation == undefined || Investigation == "False") {
        Investigation = true;
    }

    if (Investigation == "True" && APIType != "DirectPlus") {
        Investigation = true;
    }
    if (Compliance == undefined || Compliance == "False") {
        Compliance = true;
    }
    else {
        Compliance = false;
    }
    $.contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
        },
        events: {
            show: function (opt) {
                setTimeout(function () {
                    opt.$menu.find('.context-menu-disabled > span').attr('title', txtActivateFeature);
                }, 50);
            }
        },
        items: {
            "AddCompany": {
                name: addMatchAsCompany, callback: function () { 
                    var matchRecord = $(this).attr("data-val");

                    $.ajax({
                        type: "POST",
                        url: "/BadInputData/FillMatchData/",
                        data: JSON.stringify({ matchRecord: matchRecord }),
                        headers: { "__RequestVerificationToken": token },
                        dataType: "json",
                        contentType: "application/json",
                        async: false,
                        success: function (data) {
                            //popup size with tag enable disabled
                            var PopupClassName = "";
                            if ($.IsTagsLicenseAllow.toLowerCase() == "true") {
                                PopupClassName = 'popupAddressCompany'
                            }
                            else {
                                PopupClassName = 'popupAddressCompanyNoTag'
                            }
                            // Changes for Converting magnific popup to modal popup
                            $.ajax({
                                type: 'GET',
                                url: "/BadInputData/AddCompany",
                                dataType: 'HTML',
                                async: false,
                                success: function (data) {
                                    $("#divProgress").hide();
                                    $("#SearchDataAddCompanyModalMain").html(data);
                                    DraggableModalPopup("#SearchDataAddCompanyModal");
                                }
                            });
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }
            },
            "AddInvestigation": {
                name: investigateRecord, disabled: Investigation, callback: function () {
                    var matchRecord = $(this).attr("data-val");
                    var dataObj = JSON.parse(matchRecord);
                    var InputId = ""; /*$(this).attr("data-InputId");*/
                    var SrcId = "" /*$(this).attr("data-SrcId");*/
                    var Duns = $(this).attr("data-Duns");
                    var Tags = $(this).attr("data-Tags");
                    var Company = dataObj.DnBOrganizationName;
                    var Street = dataObj.DnBStreetAddressLine;
                    var City = dataObj.DnBPrimaryTownName;
                    var PostalCode = dataObj.DnBPostalCode;
                    var Country = dataObj.DnBCountryISOAlpha2Code;
                    var TradeStyle = dataObj.DnBTradeStyleName;
                    var Status = dataObj.DnBOperatingStatus;
                    var QueryString = "InputId:" + InputId + "@#$SrcId:" + SrcId + "@#$Duns:" + Duns + "@#$Tags:" + Tags + "@#$Company:" + Company + "@#$Street:" + Street + "@#$City:" + City + "@#$PostalCode:" + PostalCode + "@#$Country:" + Country + "@#$TradeStyle:" + TradeStyle + "@#$Status:" + Status;
                    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

                    // Changes for Converting magnific popup to modal popup
                    $.ajax({
                        type: 'GET',
                        url: "/ResearchInvestigation/iResearchInvestigationRecordsTargeted?Parameters=" + Parameters,
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#iResearchInvestigationRecordsTargetedModalMain").html(data);
                            DraggableModalPopup("#iResearchInvestigationRecordsTargetedModal");
                            var countSubTypes = $('#SubTypes').has('option').length;
                            if (countSubTypes == 0) {
                                $(".btnShowTargetedInvestigationMsg").show();
                            }
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }

            },
            "BenificiaryDetails": {
                name: benificiaryDetails, disabled: Compliance, callback: function () {
                    var matchRecord = $(this).attr("data-val");
                    var dataObj = JSON.parse(matchRecord);
                    var Duns = $(this).attr("data-Duns");
                    var Country = dataObj.DnBCountryISOAlpha2Code;
                    var formData = new FormData();
                    formData.append('DUNSNumber', Duns);
                    formData.append('Country', Country);
                    formData.append('isModalView', true);
                    // Changes for Converting magnific popup to modal popup
                    $.ajax({
                        type: 'POST',
                        url: "/BeneficialOwnership/SearchBeneficialOwnershipData",
                        data: formData,
                        //dataType: 'HTML',
                        contentType: false,
                        processData: false,
                        async: false,
                        success: function (data) {
                            $("#divBeneficialOwnershipData").html(data);
                            DraggableModalPopup("#BenificiaryDataModal");
                            InitComplianceRightClick();
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }
            }
        }
    });

    $('.context-menu-one').on('click', function (e) {
        return 'context-menu-icon context-menu-icon-quit';
    })
});


//Bing search 
$('body').on('click', '#btnBingSearch', function () {
    var Company = $("#txtCompany").val();
    var Address = $("#txtAddress").val();
    var City = $("#txtCity").val();
    var State = $("#txtState").val();
    var Phone = $("#txtPhone").val();
    var PostalCode = $("#txtZip").val();
    var Country = $("#Country").val();

    Company = Company == "" ? "" : Company + " ";
    Address = Address == "" ? "" : Address + " ";
    City = City == "" ? "" : City + " ";
    State = State == "" ? "" : State + " ";
    Phone = Phone == "" ? "" : Phone + " ";
    PostalCode = PostalCode == "" ? "" : PostalCode + " ";

    var QueryString = Company + Address + City + State + Phone + PostalCode + Country;
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/SearchData/BingSearch?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#divProgress").hide();
            $("#BingSearchModalMain").html(data);
            DraggableModalPopup("#BingSearchModal");
        }
    });
});



// Open popup for the Search Domin or Email
$('body').on('click', '.btnDomainOrEmailSearch', function () {
    $(".searchByAltFields").removeClass("open");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/SearchData/DomainOrEmailPopup?IsCleanSearch=" + false,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#SearchByAltFieldsModalMain").html(data);
            DraggableModalPopup("#SearchByAltFieldsModal");
        }
    });
    return false;
});



$('body').on('change', '#Country', function () {
    var country = $(this).val();
    changeLanguage(country);
    var APItype = $("#APItype").val();
    if (APItype == "DirectPlus") {
        LoadTypeAhead();
    }
});

$('body').on('click', '.GoogleMapPopUp', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/SearchData/GoogleMapPopUp",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#GoogleMapPopUpModalMain").html(data);
            DraggableModalPopup("#GoogleMapPopUpModal");
        }
    });
    return false;
});

function updateValidateInputMapAddress(address, city, state, PostalCode, Country) {
    if (address != undefined && address != '') {
        $("#txtAddress").val(address);
    }
    if (city != undefined && city != '') {
        $("#txtCity").val(city);
    }
    if (state != undefined && state != '') {
        $("#txtState").val(state);
    }
    if (PostalCode != undefined && PostalCode != '') {
        $("#txtZip").val(PostalCode);
    }
    if (Country != undefined && Country != '') {
        $("select#Country").val(Country);
    }
}


$('body').on('click', '.Enrichment', function () {
    var DunsNumber = $(this).attr("data-dunsnumber");
    var SrcId = "";
    var Company = $("#txtCompany").val().trim();
    var Address = $("#txtAddress").val() ? $("#txtAddress").val() + " " + $("#txtAddress2").val() : $("#txtAddress2").val();
    var City = $("#txtCity").val();
    var State = $("#txtState").val();
    var Postal = $("#txtZip").val();
    var CountryCode = $("select#Country").val();
    var Phone = $("#txtPhone").val();
    var Country = $(this).attr("data-country");
    var RegNo = $("#SearchedRegNum").val();
    var Website = $("#SearchedWebsite").val();

    var QueryString = "DunsNumber:" + DunsNumber + "@#$Company:" + Company + "@#$Address:" + Address + "@#$City:" + City + "@#$State:" + State + "@#$Postal:" + Postal + "@#$CountryCode:" + CountryCode + "@#$SrcId:" + SrcId + "@#$Phone:" + Phone + "@#$Country:" + Country + "@#$RegNo:" + RegNo + "@#$Website:" + Website;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/StewardshipPortal/PreviewEnrichmentData?Parameters=" + Parameters,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#EnrichmentDetailModalMain").html(data);
            DraggableModalPopup("#EnrichmentDetailModal");
        }
    });
});

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
function resetValue() {
    $('#txtCompany').val('');
    $('#txtAddress').val('');
    $('#txtAddress2').val('');
    $('#txtCity').val('');
    $('#txtState').val('');
    $('#txtPhone').val('');
    $('#txtZip').val('');
    $('#Country').val('US');
    $('#Language').val('en-US');
    $('#ExcludeNonHeadQuarters').prop('checked', false);
    $('#ExcludeNonMarketable').prop('checked', false);
    $('#ExcludeOutofBusiness').prop('checked', false);
    $('#ExcludeUndeliverable').prop('checked', false);
    $('#ExcludeUnreachable').prop('checked', false);
    $("#spnCompany").hide();
    $("#spnCountry").hide();
}
function backToparent() {
    $.magnificPopup.close();
}


$('body').on('click', '#btnGoogleSearch', function () {
    var Company = $("#txtCompany").val();
    var Address = $("#txtAddress").val();
    var City = $("#txtCity").val();
    var State = $("#txtState").val();
    var Phone = $("#txtPhone").val();
    var PostalCode = $("#txtZip").val();
    var Country = $("#Country").val();

    Company = Company == "" ? "" : Company + " ";
    Address = Address == "" ? "" : Address + " ";
    City = City == "" ? "" : City + " ";
    State = State == "" ? "" : State + " ";
    Phone = Phone == "" ? "" : Phone + " ";
    PostalCode = PostalCode == "" ? "" : PostalCode + " ";

    var QueryString = Company + Address + City + State + Phone + PostalCode + Country;
    var GoogleUrl = "https://www.google.com/search?q=Establishment : " + QueryString;
    window.open(GoogleUrl);
});

$(document).on('click', '#btnValidateInputAddress', function () {
    var Address = $("#txtAddress").val();
    var City = $("#txtCity").val();
    var State = $("#txtState").val();
    var PostalCode = $("#txtZip").val();
    var CountryISOAlpha2Code = $("select#Country").val();
    var fullAddress = "";
    if (Address != undefined && Address != "") {
        fullAddress += Address;
    }
    if (City != undefined && City != "") {
        fullAddress += "," + City;
    }
    if (State != undefined && State != "") {
        fullAddress += "," + State;
    }
    if (PostalCode != undefined && PostalCode != "") {
        fullAddress += "," + PostalCode;
    }
    if (CountryISOAlpha2Code != undefined && CountryISOAlpha2Code != "") {
        fullAddress += "," + CountryISOAlpha2Code;
    }

    var QueryString = "Address:" + fullAddress + "@#$IsFromSearch:true";
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/SearchData/ValidateGoogleMapPopUp?Parameters=" + Parameters,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#GoogleMapModalMain").html(data);
            DraggableModalPopup("#GoogleMapModal");
        }
    });
});


$('body').on('click', '#btnSearchByAddress', function (e) {
    if ($(this).hasClass("disabled")) {
        return false;
    }
    var Country = $("#Country").val();
    var Address = $("#txtAddress").val();
    var Address2 = $("#txtAddress2").val();
    var City = $("#txtCity").val();
    var State = $("#txtState").val();
    var Phone = $("#txtPhone").val();
    var Zip = $("#txtZip").val();
    var ExcludeNonHeadQuarters = $("#ExcludeNonHeadQuarters").prop("checked");
    var ExcludeNonMarketable = $("#ExcludeNonMarketable").prop("checked");
    var ExcludeOutofBusiness = $("#ExcludeOutofBusiness").prop("checked");
    var ExcludeUndeliverable = $("#ExcludeUndeliverable").prop("checked");
    var ExcludeUnreachable = $("#ExcludeUnreachable").prop("checked");

    var token = $('input[name="__RequestVerificationToken"]').val();

    var formData = new FormData();
    formData.append('CompanyName', "");
    formData.append('Country', Country);
    formData.append('Address', Address);
    formData.append('Address2', Address2);
    formData.append('City', City);
    formData.append('State', State);
    formData.append('Phone', Phone);
    formData.append('Zip', Zip);

    formData.append('ExcludeNonHeadQuarters', ExcludeNonHeadQuarters);
    formData.append('ExcludeNonMarketable', ExcludeNonMarketable);
    formData.append('ExcludeOutofBusiness', ExcludeOutofBusiness);
    formData.append('ExcludeUndeliverable', ExcludeUndeliverable);
    formData.append('ExcludeUnreachable', ExcludeUnreachable);
    formData.append('__RequestVerificationToken', token);
    formData.append('btnSearchData', "SearchByAddress");

    SearchData(formData);
});

$('body').on('click', '#btnSearchByCompany', function (e) {
    if ($(this).hasClass("disabled")) {
        return false;
    }
    var Country = $("#Country").val();
    var Company = $("#txtCompany").val();

    var ExcludeNonHeadQuarters = $("#ExcludeNonHeadQuarters").prop("checked");
    var ExcludeNonMarketable = $("#ExcludeNonMarketable").prop("checked");
    var ExcludeOutofBusiness = $("#ExcludeOutofBusiness").prop("checked");
    var ExcludeUndeliverable = $("#ExcludeUndeliverable").prop("checked");
    var ExcludeUnreachable = $("#ExcludeUnreachable").prop("checked");

    var token = $('input[name="__RequestVerificationToken"]').val();
    var cnt = 0;
    if (Company == "") {
        $("#spnCompany").show();
        cnt++;
    }
    else {
        $("#spnCompany").hide();
    }
    var formData = new FormData();
    formData.append('CompanyName', Company);
    formData.append('Country', Country);
    formData.append('ExcludeNonHeadQuarters', ExcludeNonHeadQuarters);
    formData.append('ExcludeNonMarketable', ExcludeNonMarketable);
    formData.append('ExcludeOutofBusiness', ExcludeOutofBusiness);
    formData.append('ExcludeUndeliverable', ExcludeUndeliverable);
    formData.append('ExcludeUnreachable', ExcludeUnreachable);


    formData.append('__RequestVerificationToken', token);
    formData.append('btnSearchData', "SearchByCompany");
    if (cnt > 0) {
        $(".customTable.inlinetable.searchCleanData tbody td").remove();
        $(".customTable.inlinetable.searchCleanData tbody").html("<tr><td class='noContain' colspan='11'>No data are available</td><tr>");
        return false;
    }
    else {
        SearchData(formData);
    }
});
function SearchData(formData) {
    $('#divProgress').show();
    $.ajax({
        type: "POST",
        url: "/SearchData/Index",
        data: formData,
        dataType: "HTML",
        contentType: false,
        processData: false,
        async: false,
        success: function (data) {
            $('#divSearchData').html(data);
            setup_dashboard_widgets_desktop();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}


$('body').on('blur', '#txtCompany', function () {
    SetSearchByCompanyValidation();
});
function SetSearchByCompanyValidation() {
    var Company = $('#txtCompany').val().trim();
    if (Company == "") {
        $('#btnSearchByCompany').attr("class", "disabled clsDisable");
    } else {
        $('#btnSearchByCompany').attr("class", "");
    }
}




$('body').on('blur', '#txtAddress,#txtCity,#txtState,#txtZip', function () {
    SetSearchByAddressValidation();
});

function SetSearchByAddressValidation() {
    var Address = $('#txtAddress').val().trim();
    var City = $('#txtCity').val().trim();
    var State = $('#txtState').val().trim();
    var Zip = $('#txtZip').val().trim();
    if (Address != "" || Zip != "") {
        $('#btnSearchByAddress').attr("class", "");
    } else {
        $('#btnSearchByAddress').attr("class", "disabled clsDisable");
    }
}



// TYPEAHEAD FUNCTIONALITY IN SEARCH DATA
function ShowLoadingImageCompany() {
    $("#imgCompanyLoad").show();
}

function HideLoadingImageCompany() {
    $("#imgCompanyLoad").hide();
}
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
                $(".divSearchOption #txtCompany").val(ui.item.value);
                $(".divSearchOption #txtAddress").val(ui.item.address1);
                $(".divSearchOption #txtCity").val(ui.item.city);
                $(".divSearchOption #txtState").val(ui.item.state);
                $(".divSearchOption #Country").val(ui.item.country);

                $(".divSearchOption #txtZip").val("");
                $(".divSearchOption #txtPhone").val("");

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

$("#txtCompany").keypress(function () {
    var APItype = $("#APItype").val();
    if (APItype == "DirectPlus") {
        LoadTypeAhead();
    }
});


function addHeight600Class(add) {
    if (add) {
        $(".popupiResearchInvestigationTargeted").addClass("height600");
    }
    else {
        $(".popupiResearchInvestigationTargeted").removeClass("height600");
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