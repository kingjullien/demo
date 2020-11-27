$(document).ready(function () {
    $("#divProgress").hide();
    $(".PurgeMultipleRecords").each(function () {
        $(this).attr('checked', false);
    });

    $.ajax({
        type: "GET",
        url: "/UserSessionFilter/popupUserFilterJson/",
        async: false,
        success: function (data) {
            var Data = data.Data;

            //["TableCoulmnName","ServerColumnName","DropDownURL","ISDefault(use lowercase)","Show text/value(use lowercase)","Datatype(If Date)","Default selected values"]
            var defaultColArray = [];
            var notDefaultArray = [];
            defaultColArray.push(["OrderByColumn", "OrderByColumn", "/StewardshipPortal/GetOrderByColumnDD", "true", "value", "onlyselect", (Data.OrderByColumn != null && Data.OrderByColumn != "" ? Data.OrderByColumn : "SrcRecordId")]);
            if (Data.SrcRecordId != null && Data.SrcRecordId != "") {
                defaultColArray.push(["SrcRecordId", "SrcRecordId", "", "", "", "onlytext", (Data.SrcRecordId != null && Data.SrcRecordId != "" ? Data.SrcRecordId : ""), "true"]);
            }
            else {
                notDefaultArray.push(["SrcRecordId", "SrcRecordId", "", "", "", "onlytext", (Data.SrcRecordId != null && Data.SrcRecordId != "" ? Data.SrcRecordId : "")]);
            }

            if (Data.CompanyName != null && Data.CompanyName != "") {
                defaultColArray.push(["CompanyName", "CompanyName", "", "", "", "onlytext", (Data.CompanyName != null && Data.CompanyName != "" ? Data.CompanyName : ""), "true"]);
            }
            else {
                notDefaultArray.push(["CompanyName", "CompanyName", "", "", "", "onlytext", (Data.CompanyName != null && Data.CompanyName != "" ? Data.CompanyName : "")]);
            }

            if (Data.City != null && Data.City != "") {
                defaultColArray.push(["City", "City", "", "", "", "onlytext", (Data.City != null && Data.City != "" ? Data.City : ""), "true"]);
            }
            else {
                notDefaultArray.push(["City", "City", "", "", "", "onlytext", (Data.City != null && Data.City != "" ? Data.City : "")]);
            }

            if (Data.State != null && Data.State != "") {
                defaultColArray.push(["State", "State", "", "", "", "onlytext", (Data.State != null && Data.State != "" ? Data.State : ""), "true"]);
            }
            else {
                notDefaultArray.push(["State", "State", "", "", "", "onlytext", (Data.State != null && Data.State != "" ? Data.State : "")]);
            }

            if (Data.CountryISOAlpha2Code != null && Data.CountryISOAlpha2Code != "") {
                defaultColArray.push(["Country", "Country", "/StewardshipPortal/GetCountryDD", "", "text", "onlyselect", (Data.CountryISOAlpha2Code != null && Data.CountryISOAlpha2Code != "" ? Data.CountryISOAlpha2Code : ""), "true"]);
            }
            else {
                notDefaultArray.push(["Country", "Country", "/StewardshipPortal/GetCountryDD", "", "text", "onlyselect", (Data.CountryISOAlpha2Code != null && Data.CountryISOAlpha2Code != "" ? Data.CountryISOAlpha2Code : "")]);
            }

            if (Data.ImportProcess != null && Data.ImportProcess != "") {
                defaultColArray.push(["ImportProcess", "ImportProcess", "/StewardshipPortal/GetImportProcessDD", "", "text", "onlyselect", (Data.ImportProcess != null && Data.ImportProcess != "" ? Data.ImportProcess : ""), "true"]);
            }
            else {
                notDefaultArray.push(["ImportProcess", "ImportProcess", "/StewardshipPortal/GetImportProcessDD", "", "text", "onlyselect", (Data.ImportProcess != null && Data.ImportProcess != "" ? Data.ImportProcess : "")]);
            }

            if (Data.CountryGroupId != null && Data.CountryGroupId > 0) {
                defaultColArray.push(["CountryGroup", "CountryGroup", "/StewardshipPortal/GetCountryGroupDD", "", "text", "onlyselect", (Data.CountryGroupId != null && Data.CountryGroupId > 0 ? Data.CountryGroupId.toString() : ""), "true"]);
            }
            else {
                notDefaultArray.push(["CountryGroup", "CountryGroup", "/StewardshipPortal/GetCountryGroupDD", "", "text", "onlyselect", (Data.CountryGroupId != null && Data.CountryGroupId > 0 ? Data.CountryGroupId.toString() : "")]);
            }

            if (Data.Tag != null && Data.Tag != "") {
                defaultColArray.push(["Tag", "Tag", "/StewardshipPortal/GetTagsDD", "", "text", "onlyselect", (Data.Tag != null && Data.Tag != "" ? Data.Tag : ""), "true"]);
            }
            else {
                notDefaultArray.push(["Tag", "Tag", "/StewardshipPortal/GetTagsDD", "", "text", "onlyselect", (Data.Tag != null && Data.Tag != "" ? Data.Tag : "")]);
            }

            if (Data.ErrorCode != null && Data.ErrorCode != "") {
                defaultColArray.push(["ErrorCode", "ErrorCode", "", "", "", "onlytext", (Data.ErrorCode != null && Data.ErrorCode != "" ? Data.ErrorCode : ""), "true"]);
            }
            else {
                notDefaultArray.push(["ErrorCode", "ErrorCode", "", "", "", "onlytext", (Data.ErrorCode != null && Data.ErrorCode != "" ? Data.ErrorCode : "")]);
            }

            var colArray = $.merge(defaultColArray, notDefaultArray);

            //Column array,URL for FilterData, TargetedDiv, Datatable function
            InitFilters(colArray, "/BadInputData/FilterCleanData", "#CleanDataFilterContainer", "#divBadinputData", "", "equalto");
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });

    changeLanguageHeader();
    $('table#dgBanInputData tbody tr').each(function () {
        $(this).find('td:last').attr('title', '');
    });
});

$("#btnClearCompanyData").click(function () {
    $("#txtScrRecId").val('');
    $("#txtCompany").val('');
    $("#txtCity").val('');
    $("select#State").prop('selectedIndex', 0);
    $("select#Country").prop('selectedIndex', 0);
});
$('body').on('click', '#trig', function () {
    $('#colMain').toggleClass('col-md-9 col-md-12');
    $('#colPush').toggleClass('col-md-3 col-md-0');
});

// On Clean Match Table Search Button Click
$('body').on('click', '.rejectSearch', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var str = $(this).attr("id").split("_");
    id = str[1];
    var lang = $("#hiddenInLang_" + id).val();
    var data = $(this).attr("data-val");
    var pagevalue = $("#pageValue").val();
    var page = $("#pageNo").val();
    var sortBy = $("#SortBy").val();
    var sortOrder = $("#SortOrderNo").val();
    var Parameters = ConvertEncrypte(encodeURI(lang)).split("+").join("***");
    $.ajax({
        type: "POST",
        url: "/BadInputData/FillTempData/",
        data: JSON.stringify({ id: data }),
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data == "success") {
                // Changes for Converting magnific popup to modal popup
                $.ajax({
                    type: 'GET',
                    url: "/BadInputData/SearchData?Parameters=" + Parameters,
                    dataType: 'HTML',
                    //async: false,
                    success: function (data) {
                        $("#divProgress").hide();
                        $("#SearchPopupModalMain").html(data);
                        DraggableModalPopup("#SearchPopupModal");
                    }
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });

    //return false;
});
// On Clean Match Table Reject Button Click
$('body').on('click', '.reject', function () {

    var str = $(this).attr("id").split("_");
    id = str[1];

    var Company = $("input[name='Company_" + id.replace("#", "") + "']").val();// $("#Company_" + id).val();
    var Address = $("input[name='Address_" + id.replace("#", "") + "']").val();//$("#Address_" + id).val();
    var City = $("input[name='City_" + id.replace("#", "") + "']").val();//$("#City_" + id).val();
    var State = $("input[name='State_" + id.replace("#", "") + "']").val();//$("#State_" + id).val();
    var PostalCode = $("input[name='PostalCode_" + id.replace("#", "") + "']").val();//$("#PostalCode_" + id).val();
    //var CountryISOAlpha2Code = $("input[name='CountryISOAlpha2Code_" + id.replace("#", "") + "']").val();//$("#CountryISOAlpha2Code_" + id).val();
    var CountryISOAlpha2Code = $("#CountryISOAlpha2Code_" + id).val();
    var PhoneNbr = $("input[name='PhoneNbr_" + id.replace("#", "") + "']").val();// $("#PhoneNbr_" + id).val();

    var spanCompany = $("#spanCompany_" + id).val();
    var spanAddress = $("#spanAddress_" + id).val();
    var spanCity = $("#spanCity_" + id).val();
    var spanState = $("#spanState_" + id).val();
    var spanPostalCode = $("#spanPostalCode_" + id).val();
    var spanCountryISOAlpha2Code = $("#spanCountryISOAlpha2Code_" + id).val();
    var spanPhoneNbr = $("#spanPhoneNbr_" + id).val();

    if (id != "") {

        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: '@Url.Action("UpdateBadInputData")',
            data: { "SrcRecId": id, "company": Company, "address": Address, "city": City, "state": State, "postalCode": PostalCode, "countryISOAlpha2Code": CountryISOAlpha2Code, "phoneNbr": PhoneNbr, "isRejected": true },
            dataType: "html",
            beforeSend: function () {
            },
            success: function (data) {
                if (data.result == true) {
                    $("#form_BadinputData").submit();
                }
            }
        });
    }

});

// Session Filter Button Click
$('body').on('click', '.userFilter', function () {
    var pagevalue = $("#pageValue").val();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/UserSessionFilter/popupUserFilter/'
        },
        callbacks: {
            close: function () {
                window.location.href = "/BadInputData/Index?pagevalue=" + pagevalue;
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupUserSessionFilter'
    });
    //return false;
});
// Delete Session Filter Button Click
$('body').on('click', '.DeleteFilter', function () {
    var pagevalue = $("#pageValue").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    $(this).find("strong").removeClass("fa fa-plus");
    $(this).find("strong").removeClass("fa fa-minus");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteSessionFilter, callback: function (result) {
            if (result) {

                $.ajax({
                    type: "POST",
                    url: "/StewardshipPortal/DeleteSessionFilter/",
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    success: function (data) {
                        if (data == "success") {
                            window.location.href = "/BadInputData/Index?pagevalue=" + pagevalue;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    return false;
});

var isReloadPopup = false;
function loadWebGrid(data) {
    var pagevalue = $("#pageValue").val();
    var page = $("#pageNo").val();
    var sortBy = $("#SortBy").val();
    var sortOrder = $("#SortOrderNo").val();
    var rowCount = $("#dgBanInputData tbody tr").length;
    if (rowCount == 1 && parseInt(page) > 1)
        page = parseInt(page) - 1;
    RefreshCleanDataGrid(page, sortBy, sortOrder, pagevalue);
}
function UpdateChildRecord() {
    isReloadPopup = true;
}

// Pagination for Clean Match Data on Dropdown change
$("body").on('change', '.pagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/BadInputData/Index',
        data: { "pagevalue": pagevalue },
        dataType: "HTML",
        cache: false,
        beforeSend: function () {
        },
        success: function (data) {
            $("#divBadinputData").html(data);
            setup_dashboard_widgets_desktop();
            OnSuccess();
        }
    });
});

// Set jarwish controll for Expand and collapse and Full screen functioanlity
function backToparent() {
    // setup_dashboard_widgets_desktop(); 
}

function backToparent2() {
    $.magnificPopup.close();
}

//Index page js
var isLoad = true;
$(document).ready(function () {
    OnSuccess();
    isLoad = false;
});
function generateAjaxActionLink(title, actionName, page, sortBy, sortOrder, pageValue) {
    var link = decodeURI(globalAjaxLink);
    link = link.replace('{title}', title).replace('{actionName}', actionName).replace('{page}', page).replace('{sortby}', sortBy).replace('{sortorder}', (sortOrder == 1 ? 2 : 1)).replace('{pageValue}', pageValue);
    return link;
}
// On Success method call and set action link for the set pagination link
function OnSuccess() {

    var sortOrder = Number($('#SortOrderNo').val());
    var pageNo = Number($('#pageNo').val());
    var pageValue = Number($('#pageValue').val());

    if (!isLoad) {
        setup_dashboard_widgets_desktop();
        changeLanguageHeader();
    }
    $('#dgBanInputData th').each(function () {

        if ($.trim(this.innerHTML) == "Src Id") {
            this.innerHTML = generateAjaxActionLink("Src Id", "GetFilteredCompanyList", pageNo, 1, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "Company") {
            this.innerHTML = generateAjaxActionLink("Company", "GetFilteredCompanyList", pageNo, 2, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "Address") {
            this.innerHTML = generateAjaxActionLink("Address", "GetFilteredCompanyList", pageNo, 3, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "City") {
            this.innerHTML = generateAjaxActionLink("City", "GetFilteredCompanyList", pageNo, 4, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "State") {
            this.innerHTML = generateAjaxActionLink("State", "GetFilteredCompanyList", pageNo, 5, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "Zip") {
            this.innerHTML = generateAjaxActionLink("Zip", "GetFilteredCompanyList", pageNo, 6, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "Country") {
            this.innerHTML = generateAjaxActionLink("Country", "GetFilteredCompanyList", pageNo, 7, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "Phone") {
            this.innerHTML = generateAjaxActionLink("Phone", "GetFilteredCompanyList", pageNo, 8, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "Error Code") {
            this.innerHTML = generateAjaxActionLink("Error Code", "GetFilteredCompanyList", pageNo, 9, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "Error Description") {
            this.innerHTML = generateAjaxActionLink("Error Description", "GetFilteredCompanyList", pageNo, 10, sortOrder, pageValue);
        }
        if ($.trim(this.innerHTML) == "Language") {
            this.innerHTML = generateAjaxActionLink("Language", "GetFilteredCompanyList", pageNo, 11, sortOrder, pageValue);
        }
    });
    $('body').removeClass("nooverflow");

}
function changeLanguageHeader() {
    $(".trStewardshipPortal_td11").each(function () {
        var id = $(this).children("span").children("span").attr("id").split("_")[1];
        var CountryISOAlpha2Code = $("#spanCountryISOAlpha2Code_" + id).html();
        var spanid = "#spaninLanguage_" + id;
        var dropId = "#inLanguage_" + id;
        var hiddenInLang = "#hiddenInLang_" + id;
        var hiddenInLangValue = $(hiddenInLang).val();
        var spanvalue = $(spanid).html();
        changeLanguage(CountryISOAlpha2Code, id);
        var dropdowntext = $(dropId).find("option[value='" + hiddenInLangValue + "']").text();
        if (spanvalue != "") {
            $(spanid).html(dropdowntext);
        }
        else {
            $(spanid).html("");
        }

        $(dropId).val($(hiddenInLang).val());
    });
}
$("body").on('change', '.txtCntryCode', function () {
    var id = $(this).attr("id").split("_")[1];
    var country = $(this).val();
    var inLanguage = "#inLanguage_" + id;
    changeLanguage(country, id)
});

function changeLanguage(country, id) {
    $("#inLanguage_" + id).find("option").remove();
    $("#inLanguage_" + id).append('<option value=>Select Language</option>');
    if (country.toLowerCase() == "us") {
        $("#inLanguage_" + id).append('<option value=en-US>English</option>');
        $("#inLanguage_" + id).val("en-US");
    }
    else if (country.toLowerCase() == "jp") {
        $("#inLanguage_" + id).append('<option value=en-US>English</option>');
        $("#inLanguage_" + id).append('<option value=ja-JP>Japanese</option>');
        $("#inLanguage_" + id).val("ja-JP");
    }
    else if (country.toLowerCase() == "tw") {
        $("#inLanguage_" + id).append('<option value=en-US>English</option>');
        $("#inLanguage_" + id).append('<option value=zh-hant-TW>Traditional Chinese</option>');
        $("#inLanguage_" + id).val("zh-hant-TW");
    }
    else if (country.toLowerCase() == "kr") {
        $("#inLanguage_" + id).append('<option value=en-US>English</option>');
        $("#inLanguage_" + id).append('<option value=ko-hang-KR>Hangul</option>');
        $("#inLanguage_" + id).val("ko-hang-KR");
    }
    else if (country.toLowerCase() == "cn") {
        $("#inLanguage_" + id).append('<option value=en-US>English</option>');
        $("#inLanguage_" + id).append('<option value=zh-hans-CN>Simplified Chinese</option>');
        $("#inLanguage_" + id).val("zh-hans-CN");
    }
    else {
        $("#inLanguage_" + id).append('<option value=en-US>English</option>');
        $("#inLanguage_" + id).append('<option value=ja-JP>Japanese</option>');
        $("#inLanguage_" + id).append('<option value=zh-hans-CN>Simplified Chinese</option>');
        $("#inLanguage_" + id).append('<option value=zh-hant-TW>Traditional Chinese</option>');
        $("#inLanguage_" + id).append('<option value=ko-hang-KR>Hangul</option>');
        $("#inLanguage_" + id).val("en-US");
    }
}

$('body').on('click', '#btnAcceptRejectAll', function () {
    //popup size with tag enable disabled
    var PopupClassName = "";
    if ($.IsTagsLicenseAllow.toLowerCase() == "true") {
        PopupClassName = 'popRejectAllBadData'
    }
    else {
        PopupClassName = 'popRejectAllBadDataNoTag'
    }
    $.ajax({
        type: "GET",
        url: '/StewardshipPortal/RejectAllRecords?IsMatchData=' + false,
        cache: false,
        async: false,
        beforeSend: function () {
        },
        success: function (data) {
            $("#divPurgeDataMain").html(data);
            DraggableModalPopup("#divPurgeDataModal");
        }
    });

    $.ajax({
        type: "GET",
        url: '/StewardshipPortal/RejectFromFile?IsPurgeData=' + true,
        cache: false,
        beforeSend: function () {
        },
        success: function (data) {
            $("#divPurgeFromFile").html(data);
            $("#divProgress").hide();
        }
    });


});

function CloseRejectAllWindow() {
    var pagevalue = $("#pageValue").val();
    var page = $("#pageNo").val();
    var sortBy = $("#SortBy").val();
    var sortOrder = $("#SortOrderNo").val();
    var rowCount = $("#dgBanInputData tbody tr").length;
    if (rowCount == 1 && parseInt(page) > 1)
        page = parseInt(page) - 1;
    RefreshCleanDataGrid(page, sortBy, sortOrder, pagevalue);
    //window.location.href = "/BadInputData/Index?page=" + page + "&sortby=" + sortBy + "&sortorder=" + sortOrder + "&pagevalue=" + pagevalue;
}
$('body').on('click', '#btnDefaultPageSize', function () {
    var pagevalue = $("#pageValue").val();
    var QueryString = "pagevalue:" + pagevalue + "@#$Section:BadInputData";

    $.ajax({
        type: "POST",
        url: "/StewardshipPortal/SaveDefaultPageSize?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: "json",
        contentType: "application/json",
        cache: false,
        success: function (data) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success", updateSettings, false);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

function ReloadWithCurrentPage() {
    var pagevalue = $("#divBadinputData #pageValue").val();
    var page = $("#divBadinputData #pageNo").val();
    var sortBy = $("#divBadinputData #SortBy").val();
    var sortOrder = $("#divBadinputData #SortOrderNo").val();
    var rowCount = $("#dgBanInputData tbody tr").length;
    if (rowCount == 1 && parseInt(page) > 1)
        page = parseInt(page) - 1;
    RefreshCleanDataGrid(page, sortBy, sortOrder, pagevalue);
    //location.href = location.protocol + "//" + location.hostname + (location.port ? ':' + location.port : '') + "/BadInputData/Index?page=" + page + "&sortby=" + sortBy + "&sortorder=" + sortOrder + "&pagevalue=" + pagevalue;
}


//purges a single record from the UI (right - click UI option)
$(function () {
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var EnablePurgeData = $("#LicenseEnablePurgeData").val();

    if (EnablePurgeData == undefined || EnablePurgeData == "False") {
        EnablePurgeData = true;
    }
    if (EnablePurgeData == "True") {
        EnablePurgeData = false;
    }
    $.contextMenu({
        selector: '.context-menu-one-purge',
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
            "Purge": {
                name: purgeRecord, disabled: EnablePurgeData, titile: "Purge", callback: function () {
                    //purges a single record from the UI (right - click UI option)
                    var SrcRecordId = $(this).children().find(".hidenSrcRecordId").attr("id");
                    var InputId = $(this).children().find(".hidenInputId").attr("id");
                    var Queue = "BID";
                    var QueryString = "InputId:" + InputId + "@#$SrcRecordId:" + SrcRecordId + "@#$Queue:" + Queue;
                    var url = '/StewardshipPortal/StewPurgeSingleRecord?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                    bootbox.confirm({
                        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: purgeRecordCleanData, callback: function (result) {
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
                                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                                        ShowMessageNotification("error", data, false, true, ReloadWithCurrentPage);
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                    }
                                });
                            }
                        }
                    });

                    return 'context-menu-icon context-menu-icon-quit';
                }
            },
            "ReMatch": {
                name: rematchRecord, disabled: false, titile: "ReMatch", callback: function () {
                    //re-matches a single record from the UI (right - click UI option)
                    var SrcRecordId = $(this).children().find(".hidenSrcRecordId").attr("id");
                    var QueryString = "SrcRecordId:" + SrcRecordId;
                    var url = '/BadInputData/ReMatchRecord?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                    bootbox.confirm({
                        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: rematchRecordCleanData, callback: function (result) {
                            if (result) {
                                $.ajax({
                                    type: "POST",
                                    url: url,
                                    headers: { "__RequestVerificationToken": token },
                                    dataType: "json",
                                    contentType: "application/json; charset=UTF-8",
                                    cache: false,
                                    success: function (data) {
                                        if (data != "") {
                                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                                            ShowMessageNotification("error", data, false, true, ReloadWithCurrentPage);
                                        }
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                    }
                                });
                            }
                        }
                    });

                    return 'context-menu-icon context-menu-icon-quit';
                }
            },
            "FilterByErrorCode": {
                name: filterByErrorCode, disabled: false, titile: "Filter By Error Code", callback: function () {
                    var errorCode = $(this).attr("data-errorCode");
                    $("#CleanDataFilterContainer #btnAddFilter").click();
                    var isExists = false;
                    var oldFId = "";
                    $('#CleanDataFilterContainer .FilterContainer').children().not('.divFilterAddButton').each(function () {
                        if ($(this).attr("style") != "display: none;") {
                            var id = $(this).attr("filter-id");
                            var field = $("#CleanDataFilterContainer #filterColumn" + id).val();
                            if (field == "ErrorCode") {
                                oldFId = id;
                                isExists = true;
                            }
                        }
                    });
                    if (isExists) {
                        $("#CleanDataFilterContainer #filterColumn" + oldFId).val("ErrorCode");
                        $("#CleanDataFilterContainer input#filterValErrorCode" + oldFId).val(errorCode);
                    }
                    else {
                        var Fid = $('#CleanDataFilterContainer .FilterContainer').children().not('.divFilterAddButton').last().attr("filter-id");
                        $("#CleanDataFilterContainer #filterColumn" + Fid).val("ErrorCode");
                        $("#CleanDataFilterContainer input#filterValErrorCode" + Fid).val(errorCode);
                    }
                    DoFiltering("#CleanDataFilterContainer");
                    return 'context-menu-icon context-menu-icon-quit';
                }
            },
        }
    });

    $('.context-menu-one-purge').on('click', function (e) {
        return 'context-menu-icon context-menu-icon-quit';
    })
});

$(".userFilter").popover({ trigger: "manual", html: true, animation: false })
    .on("mouseenter", function () {
        var _this = this;
        $(this).popover("show");
        $(".popover").on("mouseleave", function () {
            $(_this).popover('hide');
        });
    }).on("mouseleave", function () {
        var _this = this;
        setTimeout(function () {
            if (!$(".popover:hover").length) {
                $(_this).popover("hide");
            }
        }, 300);
    });

$('body').on('click', '#btnAcceptRejectAllFromFile', function () {
    var id = $(this).attr("id");
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/StewardshipPortal/RejectFromFile?IsPurgeData=' + true,
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupPurgeData'
    });
});


function callbackRejectPurgeData() {
    var pagevalue = $("#pageValue").val();
    var page = $("#pageNo").val();
    var sortBy = $("#SortBy").val();
    var sortOrder = $("#SortOrderNo").val();
    var rowCount = $("#dgBanInputData tbody tr").length;
    if (rowCount == 1 && parseInt(page) > 1)
        page = parseInt(page) - 1;
    RefreshCleanDataGrid(page, sortBy, sortOrder, pagevalue);
    //location.href = location.protocol + "//" + location.hostname + (location.port ? ':' + location.port : '') + "/BadInputData/Index?page=" + page + "&sortby=" + sortBy + "&sortorder=" + sortOrder + "&pagevalue=" + pagevalue;
}


//Implement re-match queue (MP-14)
$('body').on('click', '#btnReMatchRecords', function () {
    //popup size with tag enable disabled
    var PopupClassName = "";
    if ($.IsTagsLicenseAllow.toLowerCase() == "true") {
        PopupClassName = 'popReMatch'
    }
    else {
        PopupClassName = 'popReMatchNoTag'
    }
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/BadInputData/ReMatchRecords",
        dataType: 'HTML',
        success: function (data) {
            $("#divProgress").hide();
            $("#ReMatchRecordsModalMain").html(data);
            DraggableModalPopup("#ReMatchRecordsModal");
        }
    });
});

$('body').on('click', '#btnPurgeDataFromPage', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var inputId = [];
    var srcId = [];
    $(".PurgeMultipleRecords").each(function () {
        if ($(this).prop("checked")) {
            var inputIdColumn = $(this).attr("data-InputId");
            inputId.push(inputIdColumn);
        }
    });
    $(".PurgeMultipleRecords").each(function () {
        if ($(this).prop("checked")) {
            var srcIdColumn = $(this).attr("data-SrcRecordId");
            srcId.push(srcIdColumn);
        }
    });
    if (inputId == '' || srcId == '') {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", selectRecordToPurge, false);
        return false;
    }
    else {
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: purgeRecords, callback: function (result) {
                if (result) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/BadInputData/PurgeMultipleRecords',
                        //data: { "Ids": inputId, "SrcIds": srcId },
                        headers: { "__RequestVerificationToken": token },
                        data: JSON.stringify({ Ids: inputId, SrcIds: srcId }),
                        dataType: "json",
                        cache: false,
                        beforeSend: function () {
                        },
                        success: function (data) {
                            if (data != "") {
                                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                                ShowMessageNotification("error", data, false, true, LocationReload);
                            }

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                }
            }
        });
    }
});

var MultiAutoAccept = "";
$("body").on('change', '#MultipleSelect', function () {
    MultiAutoAccept = "";
    if ($(this).prop('checked') == true) {
        $(".ShowWhenUnchecked").hide();
        $(".ShowWhenchecked").show();
        $(".PurgeMultipleRecords").prop('checked');
        $(".PurgeMultipleRecords").each(function () {
            MultiAutoAccept = MultiAutoAccept + $(this).attr("data-SrcRecordId") + "," + $(this).attr("data-InputId") + ",";
            $(this).prop('checked', true);
        });
    }
    else {
        $(".ShowWhenchecked").hide();
        $(".ShowWhenUnchecked").show();
        $(".PurgeMultipleRecords").each(function () {
            $(this).prop('checked', false);
        });
    }
});

$("body").on('change', '.PurgeMultipleRecords', function () {
    var CurrentValue = $(this).attr("data-SrcRecordId") + "," + $(this).attr("data-InputId");
    if ($(this).prop('checked') == true) {
        MultiAutoAccept = MultiAutoAccept + CurrentValue + ",";
    }
    else {
        $('#MultipleSelect').prop('checked', false);
        var lstMultiAutoAccept = MultiAutoAccept.split(',');
        var MultiAutoAccept1 = "";
        for (var i = 0; i < lstMultiAutoAccept.length; i++) {
            if (lstMultiAutoAccept[i] != "")
                if (CurrentValue != lstMultiAutoAccept[i]) {
                    MultiAutoAccept1 = MultiAutoAccept1 + lstMultiAutoAccept[i] + ",";
                }
        }
        MultiAutoAccept = MultiAutoAccept1;
    }
    var TotalCount = 0;
    var SelectedCount = 0;
    $(".PurgeMultipleRecords").each(function () {
        TotalCount += 1;
        if ($(this).prop('checked') == true) {
            SelectedCount += 1;
        }
    });
    if (SelectedCount > 0) {
        $(".ShowWhenUnchecked").hide();
        $(".ShowWhenchecked").show();
    }
    else {
        $(".ShowWhenchecked").hide();
        $(".ShowWhenUnchecked").show();
    }
    if (TotalCount == SelectedCount) {
        $("#MultipleSelect").prop('checked', true);
    }
});


$('body').on('click', '#btnReMatchRecordsFromPage', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var srcId = [];
    $(".PurgeMultipleRecords").each(function () {
        if ($(this).prop("checked")) {
            var srcIdColumn = $(this).attr("data-SrcRecordId");
            srcId.push(srcIdColumn);
        }
    });
    var length = srcId.length;
    if (srcId == '') {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", selectRecordToReMatch, false);
        return false;
    }
    else {
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: total + " " + length + " " + rematchRecords, callback: function (result) {
                if (result) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/BadInputData/ReMatchMultipleRecords',
                        headers: { "__RequestVerificationToken": token },
                        data: JSON.stringify({ SrcIds: srcId }),
                        dataType: "json",
                        cache: false,
                        beforeSend: function () {
                        },
                        success: function (data) {
                            if (data != "" && data.endsWith("rows sent for ReMatch.")) {
                                data = "Data ReMatch Request Completed Successfully. " + length + " rows sent for ReMatch.";
                                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                                ShowMessageNotification("error", data, false, true, LocationReload);
                            }
                            else {
                                ShowMessageNotification("error", data, false, true, LocationReload);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                }
            }
        });
    }


});

$("body").on("click", "#btnInvestigateFromPage", function () {
    var recordData = [];
    $(".PurgeMultipleRecords").each(function () {
        if ($(this).prop("checked")) {
            var recordDataRow = JSON.parse($(this).closest("tr").attr("data-val"));
            recordData.push({
                "InputId": recordDataRow.InputId,
                "SrcRecordId": recordDataRow.SrcRecordId,
                "BusinessName": recordDataRow.CompanyName,
                "StreetAddress": recordDataRow.Address,
                "AddressLocality": recordDataRow.City,
                "AddressRegion": recordDataRow.State,
                "PostalCode": recordDataRow.PostalCode,
                "CountryISOAlpha2Code": recordDataRow.CountryISOAlpha2Code,
                "Tags": recordDataRow.Tags
            });
        }
    });
    if (recordData.length > 0) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: '/ResearchInvestigation/iResearchInvestigationRecordsMultiple',
            data: JSON.stringify(recordData),
            cache: false,
            beforeSend: function () {
            },
            success: function (data) {
                $('#divMultiMiniModalMain').html(data);
                DraggableModalPopup('#MultiMiniModal');
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
    else {
        ShowMessageNotification("success", selectRecotdToInvestigate, false);
        return false;
    }
})


function RefreshCleanDataGrid(page, sortBy, sortOrder, pagevalue) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/BadInputData/Index',
        data: { "page": page, "sortby": sortBy, "sortorder": sortOrder, "pagevalue": pagevalue },
        dataType: "HTML",
        cache: false,
        beforeSend: function () {
        },
        success: function (data) {
            $("#divBadinputData").html(data);
            setup_dashboard_widgets_desktop();
            OnSuccess();
        }
    });
}
