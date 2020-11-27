// make current match select button green as well parent grid change button color black to green and update model according this changes
$('body').on('click', '.btnUnselected', function () {
    $('#divProgress').show();
    $('.btnSelected').each(function () {
        $(this).removeClass("btnSelected").addClass("btnUnselected");
    });
    $(this).removeClass("btnUnselected").addClass("btnSelected");
    var this1 = $(this);
    var dataval = $(this).attr("data-val");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: assignDUNS, callback: function (result) {
            if (result) {
                var id = $("#SrcId").attr("data-inputid");
                var Company = $("#txtCompany").val();
                var Address = $("#txtAddress").val();
                var City = $("#form_SearchData #txtCity").val();
                var State = $("#txtState").val();
                var PostalCode = $("#txtZip").val();
                var CountryISOAlpha2Code = $("select#Country").val();
                var PhoneNbr = $("#txtPhone").val();
                $.ajax({
                    type: "POST",
                    url: '/BadInputData/AcceptBIDMatch/',
                    //data: { SrcRecordId: id, Match: dataval},
                    data: { InputId: id, Match: dataval, "CompanyName": Company, "address": Address, "city": City, "state": State, "postalCode": PostalCode, "countryISOAlpha2Code": CountryISOAlpha2Code, "phoneNbr": PhoneNbr },
                    dataType: "json",
                    beforeSend: function () {
                    },
                    success: function (data) {
                        $("#SearchPopupModal").modal("hide");
                        var pagevalue = parent.$("#pageValue").val();
                        var page = parent.$("#pageNo").val();
                        var sortBy = parent.$("#SortBy").val();
                        var sortOrder = parent.$("#SortOrderNo").val();
                        var rowCount = parent.$("#dgBanInputData tbody tr").length;
                        if (rowCount == 1 && parseInt(page) > 1)
                            page = parseInt(page) - 1;
                        parent.UpdateChildRecord();
                        $('#divProgress').hide();
                        RefreshCleanDataGrid(page, sortBy, sortOrder, pagevalue);
                    }
                });
            }
            else {
                this1.removeClass("btnSelected").addClass("btnUnselected");
            }
        }
    });
    $('#divProgress').hide();
});
$('body').on('click', '.btnSelected', function () {
    $(this).removeClass("btnSelected").addClass("btnUnselected");
});

function backToparent() {
    parent.UpdateChildRecord();
}
// Open match detail review popup
$('body').on('click', '.clsViewMatchedItemDetails', function () {
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
    var SrcId = row.attr("data-SrcId");
    //Vijay - New Code ends here

    //var QueryString = "id:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + '@#$type:AdditionalFields';
    var QueryString = "id:" + 0 + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:0@#$type:AdditionalFields" + "@#$IsPartialView:false" + "@#$SrcId:" + SrcId;
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/BadInputData/cShowMatchedItesDetailsView?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#MatchDetailModalMain").html(data);
            DraggableModalPopup("#MatchDetailModal");
        }
    });
    return false;
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

$('body').on('click', '#btnSearchData', function (e) {
    var Company = $("#txtCompany").val();
    var Country = $("#Country").val();
    var cnt = 0;

    if (Company == "" || Company == " ") {
        $("#spnCompany").show();
        $(".GoogleMapPopUp").hide();
        cnt++;
    }
    else {
        $("#spnCompany").hide();
    }
    if (Country == "") {
        $("#spnCountry").show();
        cnt++;
    }
    else {
        $("#spnCountry").hide();
    }

    if (cnt > 0) {
        $(".customTable.inlinetable.searchCleanData tbody td").remove();
        $(".customTable.inlinetable.searchCleanData tbody").html("<tr><td class='noContain' colspan='11'>No data are available</td><tr>");
        return false;
    }
    $('#divProgress').show();
});
// Update Data from update button click and refresh main grid also
$('body').on('click', '.btnUpdateData', function (e) {

    var id = $("#SrcId").attr("data-inputid");
    var Company = $("#txtCompany").val().trim();
    var Address = $("#txtAddress").val();
    var Address2 = $("#txtAddress2").val();
    var City = $("#form_SearchData #txtCity").val();
    var State = $("#txtState").val();
    var PostalCode = $("#txtZip").val();
    var CountryISOAlpha2Code = $("select#Country").val();
    var PhoneNbr = $("#txtPhone").val();
    var inLanguage = $("#Language").val();
    var cnt = 0;
    if (Company == "") {
        $("#spnCompany").show();
        cnt++;
    }
    else {
        $("#spnCompany").hide();
    }

    if (CountryISOAlpha2Code == "") {
        $("#spnCountry").show();
        cnt++;
    }
    else {
        $("#spnCountry").hide();
    }
    if (cnt > 0) {
        return false;
    }
    else {
        if (id != "") {
            $.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: '/BadInputData/UpdateCompany',
                data: { "InputId": id, "company": Company, "address": Address, "city": City, "state": State, "postalCode": PostalCode, "countryISOAlpha2Code": CountryISOAlpha2Code, "phoneNbr": PhoneNbr, "isRejected": false, "inLanguage": inLanguage, "Address2": Address2 },
                dataType: "Html",
                beforeSend: function () {
                },
                success: function (data) {
                    parent.loadWebGrid(data);
                    parent.OnSuccess();
                    parent.changeLanguageHeader();
                    $("#SearchPopupModal").modal("hide");
                }
            });
        }
    }
});
// Search Data from option and load grid .
$('body').on('click', '.searchChild', function () {
    $(".searchChild").each(function () {
        $(this).removeClass("current");
    });
    var id = "#" + $(this).attr('id');
    if (id.indexOf('-2') > -1) {
        $(id).addClass("current");
        var previd = id.replace("-2", "");
        $(previd).addClass("current");
    } else {
        var nextrowid = id + "-2";
        $(id).addClass("current");
        $(nextrowid).addClass("current");
    }
});
function ReloadSuccess() {
    $('#divProgress').hide();
    dynamicSearchDataHeight();
}
// Resize Match Detail Review Popup accroding to window size
$(window).resize(function () {
    dynamicSearchDataHeight();
});
// Resize Match Detail Review Popup accroding to window size
function dynamicSearchDataHeight() {
    //var bodyHeight = $(window.parent).height();
    //var srtSearchDataGridHeight = $('.srtSearchDataGrid').height();
    //$('.searchpopup').height(bodyHeight - srtSearchDataGridHeight - 180);

}

function updateSearchData(DUNSNO, Country, SrcRecId, InputId) {
    $('#divProgress').show();
    var QueryString = "DUNSNO:" + DUNSNO + '@#$FromPage:BadInputData' + "@#$SrcRecId:" + SrcRecId + "@#$InputId:" + InputId;
    $.ajax({
        type: "POST",
        //url: "/SearchData/UpdateSearchData?DUNSNO=" + DUNSNO + '&FromPage=BadInputData' + "&SrcRecId=" + SrcRecId + "&InputId=" + InputId,
        url: "/SearchData/UpdateSearchData?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        success: function (data) {
            $('#divProgress').show()
            $('#divSearchData').html(data);
            setTimeout(function () { $('#divProgress').hide(); }, 20000);
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });

}
// Open Investigation Record popup
$('body').on('click', '.btnInvestigation', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var Company = $("#txtCompany").val();
    var CountryCode = $("#Country").val();
    var Address = $("#txtAddress").val();
    var City = $("#form_SearchData #txtCity").val();
    var State = $("#txtState").val();
    var PhoneNbr = $("#txtPhone").val();
    var PostalCode = $("#txtZip").val();
    var SrcId = $("#SrcRecId").val();
    var InputId = $(this).attr("data-InputId");
    var Tags = $(this).attr("data-Tags");
    var StreetName = $(this).attr("data-StreetName");

    var QueryString = "InputId:" + InputId + "@#$SrcId:" + SrcId + "@#$Company:" + Company + "@#$StreetName:" + State + "@#$Address:" + Address + "@#$City:" + City + "@#$PostalCode:" + PostalCode + "@#$CountryCode:" + CountryCode + "@#$Tags:" + Tags + "@#$Phone:" + PhoneNbr;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

    // Changes for Converting magnific popup to modal popup	
    $.ajax({
        type: 'GET',
        url: "/ResearchInvestigation/iResearchInvestigationRecords?Parameters=" + Parameters,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#InvestigateModalMain").html(data);
            DraggableModalPopup("#InvestigateModal");
        }
    });
});
function toggleIcon(e) {
    $(e.target).prev().find(".more-less").toggleClass('fa-plus fa-minus');
}
$('.additional-main').on('hidden.bs.collapse', toggleIcon);
$('.additional-main').on('shown.bs.collapse', toggleIcon);


$("body").on("click", "#SearchPopupModal #lnkCollapse", function () {

    var IsExpandCleanData = false;
    if (!$("#SearchPopupModal #lnkCollapse i").hasClass("fa-minus")) {
        IsExpandCleanData = true;
    }

    $.ajax({
        type: "GET",
        url: '/BadInputData/SetSession',
        contentType: "application/json; charset=utf-8",
        data: { IsExpandCleanData: IsExpandCleanData },
        dataType: "json",
        success: function () { },
        global: false
    });

});
$("body").on("click", ".AltCompany", function () {
    var companyName = $(this).attr("data-company");
    var txtcompanyName = $("#txtCompany").val();
    var tempvalue = "";
    tempvalue = companyName;
    $("#spnAltCompanyName").html(txtcompanyName);
    $(this).attr("data-company", txtcompanyName);
    $("#txtCompany").val(tempvalue);
    $("#txtCompany").attr("value", tempvalue);
});
$("body").on("click", ".AltAddress", function () {
    var tempValue = "";
    var Address = $(this).attr("data-Address");
    var txtAddress = $("#txtAddress").val();
    tempvalue = Address;
    $("#spnAltAddress").html(txtAddress);
    $(this).attr("data-address", txtAddress);
    $("#txtAddress").val(tempvalue);
    $("#txtAddress").attr("value", tempvalue);

    tempValue = "";
    var City = $(this).attr("data-city");
    var txtCity = $("#form_SearchData #txtCity").val();
    tempvalue = City;
    $("#spnAltCity").html(txtCity);
    $(this).attr("data-city", txtCity);
    $("#form_SearchData #txtCity").val(tempvalue);
    $("#form_SearchData #txtCity").attr("value", tempvalue);

    tempValue = "";
    var State = $(this).attr("data-state");
    var txtState = $("#txtState").val();
    tempValue = State;
    $("#spnAltState").html(txtState);
    $(this).attr("data-state", txtState);
    $("#txtState").val(tempValue);
    $("#txtState").attr("value", tempvalue);

    tempValue = "";
    var PostalCode = $(this).attr("data-postalcode");
    var txtPostalCode = $("#txtZip").val();
    tempValue = PostalCode;
    $("#spnAltPostalCode").html(txtPostalCode);
    $(this).attr("data-postalcode", txtPostalCode);
    $("#txtZip").val(tempValue);
    $("#txtZip").attr("value", tempvalue);


    tempValue = "";
    var Address2 = $(this).attr("data-address2");
    var txtAddress2 = $("#txtAddress2").val();
    tempValue = Address2;
    $("#spnAltAddress2").html(txtAddress2);
    $(this).attr("data-address2", txtAddress2);
    $("#txtAddress2").val(tempValue);
    $("#txtAddress2").attr("value", tempvalue);

    SearchByAddressValidation();

});


function changeLanguage(country, IsLanguageFlag) {
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
    if (IsLanguageFlag != undefined) {
        $("#Language").val(IsLanguageFlag);
    }
}


// Open popup for the Search Domin or Email
$('body').on('click', '.btnDomainOrEmailSearch', function () {
    var email = $("#HiddenEmail").val();
    var domain = $("#HiddenWebsite").val();
    var SrcRecId = $("#SrcRecId").val();
    var InputId = $("#InputId").val();
    var searchvalue = "";
    var type = "";
    if ((email != "" && domain != "") || (email == "" && domain == "")) {
        type = "domain";
        searchvalue = domain;
    }
    else if (email != "") {
        type = "email";
        searchvalue = email;
    }
    else if (domain != "") {
        type = "domain";
        searchvalue = domain;
    }
    var QueryString = "type:" + type + "@#$searchvalue:" + searchvalue + "@#$SrcRecId:" + SrcRecId + "@#$InputId:" + InputId;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

    $(".clsAdditionActionFilter .input-group-btn").removeClass('open');
    // Changes for Converting magnific popup to modal popup	
    $.ajax({
        type: 'GET',
        url: "/SearchData/DomainOrEmailPopup?Parameters=" + Parameters + "&IsCleanSearch=true",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#SearchByAltFieldsModalMain").html(data);
            DraggableModalPopup("#SearchByAltFieldsModal");
        }
    });
    return false;
});

function DomainOrEmailSearchData(searchValue, searchType, IsCleanSearch, Country, SrcRecId, InputId) {
    $('#divProgress').show();
    var QueryString = "searchValue:" + searchValue + "@#$searchType:" + searchType + "@#$IsCleanSearch:" + IsCleanSearch + "@#$Country:" + Country + "@#$SrcRecId:" + SrcRecId + "@#$InputId:" + InputId;
    $.ajax({
        type: "POST",
        url: "/SearchData/DomainOrEmailPopup?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        success: function (data) {
            $('#divSearchData').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    setTimeout(function () { $('#divProgress').hide(); }, 10000);
}

//Bing search 
$('body').on('click', '#btnBingSearch', function () {
    var Company = $("#txtCompany").val();
    var Address = $("#txtAddress").val();
    var City = $("#form_SearchData #txtCity").val();
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
    $(".clsAdditionActionFilter .input-group-btn").removeClass('open');
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

function RegistrationNumberSearchData(RegistrationNoValue, searchType, IsCleanSearch, Country, SrcRecId, InputId) {
    $('#divProgress').show();
    var QueryString = "RegistrationNoValue:" + RegistrationNoValue + "@#$searchType:" + searchType + "@#$IsCleanSearch:" + IsCleanSearch + "@#$Country:" + Country + "@#$SrcRecId:" + SrcRecId + "@#$InputId:" + InputId;
    $.ajax({
        type: "POST",
        url: "/SearchData/RegistrationNumberPopup?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        success: function (data) {
            $('#divSearchData').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    setTimeout(function () { $('#divProgress').hide(); }, 10000);
}

$('body').on('click', '#btnSearchByCompany', function (e) {
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
    if (Country == "") {
        $("#spnCountry").show();
        cnt++;
    }
    else {
        $("#spnCountry").hide();
    }
    //var cnt = 0;
    var formData = new FormData();
    formData.append('Company', Company);
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
        $('#divProgress').show();
        $.ajax({
            type: "POST",
            url: "/BadInputData/SearchData",
            data: formData,
            dataType: "HTML",
            contentType: false,
            processData: false,
            async: false,
            success: function (data) {
                $('#divSearchData').html(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });

    }
});

$('body').on('click', '#btnSearchByAddress', function (e) {
    var Country = $("#Country").val();
    var Address = $("#txtAddress").val();
    var Address2 = $("#txtAddress2").val();
    var City = $("#form_SearchData #txtCity").val();
    var State = $("#txtState").val();
    var Phone = $("#txtPhone").val();
    var Zip = $("#txtZip").val();
    var Company = $("#txtCompany").val();
    var ExcludeNonHeadQuarters = $("#ExcludeNonHeadQuarters").prop("checked");
    var ExcludeNonMarketable = $("#ExcludeNonMarketable").prop("checked");
    var ExcludeOutofBusiness = $("#ExcludeOutofBusiness").prop("checked");
    var ExcludeUndeliverable = $("#ExcludeUndeliverable").prop("checked");
    var ExcludeUnreachable = $("#ExcludeUnreachable").prop("checked");

    var token = $('input[name="__RequestVerificationToken"]').val();

    //var cnt = 0;
    var formData = new FormData();
    formData.append('Company', "");
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

    $.ajax({
        type: "POST",
        url: "/BadInputData/SearchData",
        data: formData,
        dataType: "HTML",
        contentType: false,
        processData: false,
        async: false,
        success: function (data) {
            $('#divSearchData').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });


});

$('body').on('click', '#btnPurgeData', function (e) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var SrcRecordId = $('#SrcRecId').val();
    var InputId = $('#InputId').val();
    var Queue = "BID";
    var QueryString = "InputId:" + InputId + "@#$SrcRecordId:" + SrcRecordId + "@#$Queue:" + Queue;
    var url = '/StewardshipPortal/StewPurgeSingleRecord?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: cleansearchpurgeRecord, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: url,
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json; charset=UTF-8",
                    async: false,
                    cache: false,
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                        parent.ShowMessageNotification("success", data, true, true, parent.callbackRejectPurgeData);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});

$('body').on('blur', '#txtCompany', function () {
    var Company = $('#txtCompany').val().trim();
    if (Company == "") {
        //$('#btnSearchByCompany').attr("class", "disabled clsDisable");
        $('#btnSearchByCompany').hide();
        $('#btnSearchByCompanydisabled').show();
    } else {
        $('#btnSearchByCompany').show();
        $('#btnSearchByCompanydisabled').hide();
        //$('#btnSearchByCompany').attr("class", "");
    }
});

$('body').on('blur', '#txtAddress', function () {
    SearchByAddressValidation();
});


$('body').on('blur', '#txtCity', function () {
    SearchByAddressValidation();
});


$('body').on('blur', '#txtState', function () {
    SearchByAddressValidation();
});

$('body').on('blur', '#txtZip', function () {
    SearchByAddressValidation();
});

function SearchByAddressValidation() {
    var Address = $('#txtAddress').val().trim();
    var City = $('#form_SearchData #txtCity').val().trim();
    var State = $('#txtState').val().trim();
    var Zip = $('#txtZip').val().trim();
    if (Address != "" || Zip != "") {
        $("#btnSearchByAddressdisabled").hide();
        $("#btnSearchByAddress").show();
        //$('#btnSearchByAddress').attr("class", "");
    } else {
        $("#btnSearchByAddressdisabled").show();
        $("#btnSearchByAddress").hide();
        //$('#btnSearchByAddress').attr("class", "disabled clsDisable");
    }
}


$('li.dropdown.mega-dropdown a').on('click', function (event) {
    $(this).parent().toggleClass('open');
});
$(document).on('click', '#btnValidateInputAddress', function () {
    var Address = $("#txtAddress").val();
    var Address2 = $("#txtAddress2").val();
    var City = $("#txtCity").val();
    var State = $("#txtState").val();
    var PostalCode = $("#txtZip").val();
    var CountryISOAlpha2Code = $("select#Country").val();
    var fullAddress = "";
    if (Address != undefined && Address != "") {
        fullAddress += Address;
    }
    if (Address2 != undefined && Address2 != "") {
        fullAddress += "," + Address2;
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
function updateValidateInputMapAddress(address, city, state, PostalCode, Country) {
    if (address != undefined && address != '') {
        $("#form_SearchData #txtAddress").val(address);
    }
    if (city != undefined && city != '') {
        $("#form_SearchData #txtCity").val(city);
    }
    if (state != undefined && state != '') {
        $("#form_SearchData #txtState").val(state);
    }
    if (PostalCode != undefined && PostalCode != '') {
        $("#form_SearchData #txtZip").val(PostalCode);
    }
    if (Country != undefined && Country != '') {
        $("select#Country").val(Country);
    }
}

$('body').on('click', '#btnGoogleSearch', function () {
    var Company = $("#txtCompany").val();
    var Address = $("#txtAddress").val();
    var City = $("#form_SearchData #txtCity").val();
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

// TYPEAHEAD FUNCTIONALITY IN SEARCH DATA
function ShowLoadingImageCompany() {
    $("#imgCompanyLoadForCleanData").show();
}

function HideLoadingImageCompany() {
    $("#imgCompanyLoadForCleanData").hide();
}
function LoadTypeAhead() {
    if ($(".TypeAheadToggle").is(':checked')) {
        var defaultCountryCode = $('#Country').val();
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

        $('.ui-front').addClass("cleanSearchTypeAhead");

        $("#txtCompany").autocomplete({
            source: dataSrc,
            minLength: 3,
            select: function (event, ui) {
                FinalResponse = JSON.stringify(ui.item);

                $(".cleanSearchPopup #txtCompany").val(ui.item.value);
                $(".cleanSearchPopup #txtAddress").val(ui.item.address1);
                $(".cleanSearchPopup #txtCity").val(ui.item.city);
                $(".cleanSearchPopup #txtState").val(ui.item.state);
                $(".cleanSearchPopup #Country").val(ui.item.country);

                $(".cleanSearchPopup #txtZip").val("");
                $(".cleanSearchPopup #txtPhone").val("");
                $('#btnSearchData').focus();
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