function onLoadOIMatchSearchData() {
    validCompanySearch();
    validEmailSearch();
    validWebDomainSearch();
    validOrbNumSearch();
    validAddressSearch();
    validEINSearch();
    checkAllCheckBox();
}

function CheckIsMaxSearch() {
    if ($(".divleftSearchPanel").children(".panel").length > 9) {
        return false;
    }
    else {
        return true;
    }
}
$.MaxSearchMessage = maximumSearchAllowed;
function checkAllCheckBox() {
    var checkLength = $(".btnSelectUnselectStewOIMatch").length;
    var checkcnt = 0;
    $(".btnSelectUnselectStewOIMatch").each(function () {
        if ($(this).attr("data-value").toLowerCase() == "true") {
            checkcnt++;
        }
    });
    if (checkLength == checkcnt) {
        $(".btnSelectAllStewOIMatch").attr("data-value", true);
        $(".btnSelectAllStewOIMatch").addClass("fa-check-square-o");
        $(".btnSelectAllStewOIMatch").removeClass("fa-square-o");
    }
    else {
        $(".btnSelectAllStewOIMatch").attr("data-value", false);
        $(".btnSelectAllStewOIMatch").addClass("fa-square-o");
        $(".btnSelectAllStewOIMatch").removeClass("fa-check-square-o");
    }
}
$(document).on('click', '.btnDeleteStewOIMatch', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var inputId = $("#lstOICompanyInput_InputId").val();
    var MatchId = $(this).attr("data-MatchId");
    var QueryString = "InputId:" + inputId + "@#$MatchId:" + MatchId;
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteSearch, callback: function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: "/OIMatchData/DeleteStweMatchSearch?Parameters=" + Parameters,
                    headers: { "__RequestVerificationToken": token },
                    dataType: 'HTML',
                    cache: false,
                    contentType: 'application/html;',
                    success: function (data) {
                        $("#divPartialSearchDetails").html(data);
                    }
                });
            }
        }
    });
});

$(document).on('click', '#btnUndoDeleteSearch', function () {
    if (CheckIsMaxSearch()) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        var inputId = $("#lstOICompanyInput_InputId").val();
        var MatchId = $(this).attr("data-MatchId");
        var QueryString = "InputId:" + inputId + "@#$MatchId:" + MatchId;
        var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: restoreDelete, callback: function (result) {
                if (result) {
                    $.ajax({
                        type: 'POST',
                        url: "/OIMatchData/UndoStweMatchSearch?Parameters=" + Parameters,
                        headers: { "__RequestVerificationToken": token },
                        dataType: 'HTML',
                        cache: false,
                        contentType: 'application/html;',
                        success: function (data) {
                            $("#divPartialSearchDetails").html(data);
                        }
                    });
                }
            }
        });
    }
    else {
        parent.ShowMessageNotification("success", $.MaxSearchMessage, false);

    }
});

$(document).on('click', '.btnSelectUnselectStewOIMatch', function () {
    if ($(this).hasClass("fa-check-square-o")) {
        $(this).removeClass("fa-check-square-o");
        $(this).addClass("fa-square-o");
        $(this).attr("data-value", false);
        $(this).attr("title", "Select");
    }
    else {
        $(this).addClass("fa-check-square-o");
        $(this).removeClass("fa-square-o");
        $(this).attr("data-value", true);
        $(this).attr("title", "De-Select");
    }
    checkAllCheckBox()
});
$(document).on('click', '#btnRefreshMatchSearch', function () {
    var selectedMatchSerch = [];
    $(".btnSelectUnselectStewOIMatch").each(function () {
        if ($(this).attr("data-value").toLowerCase() == "true") {
            selectedMatchSerch.push($(this).attr("data-matchid"));
        }

    });
    if (selectedMatchSerch.length > 0) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        var inputId = $("#lstOICompanyInput_InputId").val();
        var MatchId = selectedMatchSerch.toString();
        var QueryString = "InputId:" + inputId + "@#$MatchId:" + MatchId;
        var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        $.ajax({
            type: 'POST',
            url: "/OIMatchData/RefreshMatchSearch?Parameters=" + Parameters,
            headers: { "__RequestVerificationToken": token },
            dataType: 'HTML',
            cache: false,
            contentType: 'application/html;',
            success: function (data) {
                $("#divPartialSearchDetails").html(data);
                checkAllCheckBox();
            }
        });
    }
    else {
        parent.ShowMessageNotification("success", selectSearch, false);
    }

});

function UpdateMatchSearch(data) {
    $("#divPartialSearchDetails").html(data);
}
$(document).on('click', '.btnAssignOrb', function () {
    var inputId = $("#lstOICompanyInput_InputId").val();
    var OrbNum = $(this).attr("data-OrbNum");
    var QueryString = "inputId:" + inputId + "@#$OrbNum:" + OrbNum;
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: assignORB, callback: function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: "/OIMatchData/AssignORBnum?Parameters=" + Parameters,
                    dataType: 'JSON',
                    cache: false,
                    contentType: 'application/json;',
                    success: function (data) {
                        if (data.result == true) {
                            parent.CallbackMatchAssign(data.message);
                        }
                        else {
                            parent.ShowMessageNotification("success", data.message, false);
                        }
                    }
                });
            }
        }
    });
});

$(document).on('click', '.btnOpenMatchMetadata', function () {
    $("#tbOIMetaSearchData tbody tr").each(function () {
        $(this).removeClass("current");
    });
    $(this).closest("tr").addClass("current");

    var InputId = $(this).attr("data-InputId");
    var OrbNum = $(this).attr("data-orb_num");
    var dataNext = $(this).attr("data-next");
    var dataPrev = $(this).attr("data-prev");

    var QueryString = "inputId:" + InputId + "@#$OrbNum:" + OrbNum + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$IsPartial:true";
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: 'GET',
        url: '/OIMatchData/MatchMetadata?Parameters=' + Parameters,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIMatchMetadataModalMain").html(data);
            DraggableModalPopup("#OIMatchMetadataModal");
            onLoadMatchMetaData();
        }
    });
});

$(document).on('click', '#btnViewresolutionMap', function () {
    var selectedMatchSerch = [];
    $(".btnSelectUnselectStewOIMatch").each(function () {
        if ($(this).attr("data-value").toLowerCase() == "true") {
            selectedMatchSerch.push($(this).attr("data-matchid"));
        }
    });
    if (selectedMatchSerch.length > 0) {
        var inputId = $("#lstOICompanyInput_InputId").val();
        var MatchId = selectedMatchSerch.toString();
        var QueryString = "InputId:" + inputId + "@#$MatchId:" + MatchId + "@#$isPartial:true";
        var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        $.ajax({
            type: 'GET',
            url: '/OIMatchData/ViewresolutionMap?Parameters=' + Parameters,
            dataType: 'HTML',
            async: false,
            success: function (data) {
                $("#OIViewResolutionMapModalMain").html(data);
                DraggableModalPopup("#OIViewResolutionMapModal");
            }
        });
    }
    else {
        ShowMessageNotification("success", selectSearch, false);
    }

});
function ValidateSearch() {
    var cnt = 0;
    var InpCompanyName = $("#lstOICompanyInput_InpCompanyName").val();
    if (!InpCompanyName && InpCompanyName == "") {
        cnt++;
        $("#spnInpCompany").show();
    }
    else {
        $("#spnInpCompany").hide();
    }

    if (!CheckIsMaxSearch()) {
        ShowMessageNotification("success", $.MaxSearchMessage, false);
        cnt++;
    }

    if (cnt > 0) {
        return false;
    }
    else {
        return true;
    }
}

//check box for select/De-select all Match Searches List
$(document).on('click', '.btnSelectAllStewOIMatch', function () {

    if ($(this).hasClass("fa-check-square-o")) {
        $(this).removeClass("fa-check-square-o");
        $(this).addClass("fa-square-o");
        $(this).attr("title", "Select all");
        $(".btnSelectUnselectStewOIMatch").each(function () {
            $(this).attr("data-value", false);
            $(this).removeClass("fa-check-square-o");
            $(this).addClass("fa-square-o");
        });
        //$(".divSelectAllSearchMatches span").text("Select All");
    }
    else {
        $(this).addClass("fa-check-square-o");
        $(this).removeClass("fa-square-o");
        $(this).attr("title", "De-Select all");
        $(".btnSelectUnselectStewOIMatch").each(function () {
            $(this).attr("data-value", true);
            $(this).addClass("fa-check-square-o");
            $(this).removeClass("fa-square-o");
        });
    }
});

$(document).on('click', '#btnSearchByCompany', function () {
    if ($(this).hasClass("disabled clsDisable")) {
        return false;
    }
    var formData = new FormData();
    var InpCompanyName = $("#lstOICompanyInput_InpCompanyName").val();
    var InputId = $("#lstOICompanyInput_InputId").val();
    var SrcRecordId = $("#lstOICompanyInput_SrcRecordId").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    formData.append('__RequestVerificationToken', token);
    formData.append('lstOICompanyInput.InpCompanyName', InpCompanyName);
    formData.append('lstOICompanyInput.InputId', InputId);
    formData.append('lstOICompanyInput.SrcRecordId', SrcRecordId);
    SearchOrbMatch(formData);
});

$(document).on('click', '#btnSearchByAddress', function () {
    if ($(this).hasClass("disabled clsDisable")) {
        return false;
    }
    var InputId = $("#lstOICompanyInput_InputId").val();
    var Address = $("#lstOICompanyInput_InpAddress1").val();
    var Address2 = $("#lstOICompanyInput_InpAddress2").val();
    var City = $("#lstOICompanyInput_InpCity").val();
    var State = $("#lstOICompanyInput_InpState").val();
    var Phone = $("#lstOICompanyInput_InpPhoneNbr").val();
    var Zip = $("#lstOICompanyInput_InpPostalCode").val();
    var Country = $("#lstOICompanyInput_InpCountryISOAlpha2Code").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var formData = new FormData();
    formData.append('__RequestVerificationToken', token);
    formData.append('lstOICompanyInput.InputId', InputId);
    formData.append('lstOICompanyInput.InpCompanyName', "");
    formData.append('lstOICompanyInput.InpCountryISOAlpha2Code', Country);
    formData.append('lstOICompanyInput.InpAddress1', Address);
    formData.append('lstOICompanyInput.InpAddress2', Address2);
    formData.append('lstOICompanyInput.InpCity', City);
    formData.append('lstOICompanyInput.InpState', State);
    formData.append('lstOICompanyInput.InpPhoneNbr', Phone);
    formData.append('lstOICompanyInput.InpPostalCode', Zip);
    SearchOrbMatch(formData);
});

$(document).on('click', '#btnSearchByOrbNumber', function () {
    if ($(this).hasClass("disabled clsDisable")) {
        return false;
    }
    var formData = new FormData();
    var InpOrbNum = $("#lstOICompanyInput_InpOrbNum").val();
    var InputId = $("#lstOICompanyInput_InputId").val();
    var SrcRecordId = $("#lstOICompanyInput_SrcRecordId").val();
    var Country = $("#lstOICompanyInput_InpCountryISOAlpha2Code").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    formData.append('__RequestVerificationToken', token);
    formData.append('lstOICompanyInput.InpOrbNum', InpOrbNum);
    //formData.append('lstOICompanyInput.InpCountryISOAlpha2Code', Country);
    formData.append('lstOICompanyInput.InputId', InputId);
    formData.append('lstOICompanyInput.SrcRecordId', SrcRecordId);
    SearchOrbMatch(formData);
});
$(document).on('click', '#btnSearchByEmail', function () {
    if ($(this).hasClass("disabled clsDisable")) {
        return false;
    }
    var formData = new FormData();
    var InpEmail = $("#lstOICompanyInput_InpEmail").val();
    var InputId = $("#lstOICompanyInput_InputId").val();
    var SrcRecordId = $("#lstOICompanyInput_SrcRecordId").val();
    var Country = $("#lstOICompanyInput_InpCountryISOAlpha2Code").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    formData.append('__RequestVerificationToken', token);
    formData.append('lstOICompanyInput.InpEmail', InpEmail);
    formData.append('lstOICompanyInput.InputId', InputId);
    formData.append('lstOICompanyInput.SrcRecordId', SrcRecordId);
    SearchOrbMatch(formData);
});
$(document).on('click', '#btnSearchByWebDomain', function () {
    if ($(this).hasClass("disabled clsDisable")) {
        return false;
    }
    var formData = new FormData();
    var InpWebsite = $("#lstOICompanyInput_InpWebsite").val();
    var InputId = $("#lstOICompanyInput_InputId").val();
    var SrcRecordId = $("#lstOICompanyInput_SrcRecordId").val();
    var Country = $("#lstOICompanyInput_InpCountryISOAlpha2Code").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    formData.append('__RequestVerificationToken', token);
    formData.append('lstOICompanyInput.InpWebsite', InpWebsite);
    formData.append('lstOICompanyInput.InputId', InputId);
    formData.append('lstOICompanyInput.SrcRecordId', SrcRecordId);
    SearchOrbMatch(formData);
});
$(document).on('click', '#btnSearchByEIN', function () {
    if ($(this).hasClass("disabled clsDisable")) {
        return false;
    }
    var formData = new FormData();
    var InpEIN = $("#lstOICompanyInput_InpEIN").val();
    var InputId = $("#lstOICompanyInput_InputId").val();
    var SrcRecordId = $("#lstOICompanyInput_SrcRecordId").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    formData.append('__RequestVerificationToken', token);
    formData.append('lstOICompanyInput.InpEIN', InpEIN);
    formData.append('lstOICompanyInput.InputId', InputId);
    formData.append('lstOICompanyInput.SrcRecordId', SrcRecordId);
    SearchOrbMatch(formData);
});
function SearchOrbMatch(formData) {
    if (CheckIsMaxSearch()) {
        $.ajax({
            type: "POST",
            url: "/OIMatchData/OIMatchNewSearch",
            data: formData,
            dataType: "HTML",
            contentType: false,
            processData: false,
            async: false,
            success: function (data) {
                $("#divPartialSearchDetails").html(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    } else {
        ShowMessageNotification("success", $.MaxSearchMessage, false);
    }
}

$('body').on('blur', '#lstOICompanyInput_InpCompanyName', function () {
    validCompanySearch();
});
$('body').on('blur', '#lstOICompanyInput_InpEIN', function () {
    validEINSearch();
});
$('body').on('blur', '#lstOICompanyInput_InpWebsite', function () {
    validWebDomainSearch();
});

$('body').on('blur', '#lstOICompanyInput_InpEmail', function () {
    validEmailSearch();
});

$('body').on('blur', '#lstOICompanyInput_InpOrbNum', function () {
    validOrbNumSearch();
});

$('body').on('blur', '#lstOICompanyInput_InpAddress1', function () {
    validAddressSearch();
});
$('body').on('blur', '#lstOICompanyInput_InpCity', function () {
    validAddressSearch();
});
$('body').on('blur', '#lstOICompanyInput_InpState', function () {
    validAddressSearch();
});
$('body').on('blur', '#lstOICompanyInput_InpPostalCode', function () {
    validAddressSearch();
});

function validEmailSearch() {
    var InpEmail = $("#lstOICompanyInput_InpEmail").val();
    if (InpEmail == "") {
        $('#btnSearchByEmail').addClass("disabled");
        $('#btnSearchByEmail').addClass("clsDisable");
    } else {
        $('#btnSearchByEmail').removeClass("disabled");
        $('#btnSearchByEmail').removeClass("clsDisable");
    }
}
function validWebDomainSearch() {
    var InpWebsite = $("#lstOICompanyInput_InpWebsite").val();
    if (InpWebsite == "") {
        $('#btnSearchByWebDomain').addClass("disabled");
        $('#btnSearchByWebDomain').addClass("clsDisable");
    } else {
        $('#btnSearchByWebDomain').removeClass("disabled");
        $('#btnSearchByWebDomain').removeClass("clsDisable");
    }
}
function validOrbNumSearch() {
    var InpOrbNum = $('#lstOICompanyInput_InpOrbNum').val();
    if (InpOrbNum == "") {
        $('#btnSearchByOrbNumber').addClass("disabled");
        $('#btnSearchByOrbNumber').addClass("clsDisable");
    } else {
        $('#btnSearchByOrbNumber').removeClass("disabled");
        $('#btnSearchByOrbNumber').removeClass("clsDisable");
    }
}
function validAddressSearch() {
    var Address = $('#lstOICompanyInput_InpAddress1').val().trim();
    var City = $('#lstOICompanyInput_InpCity').val().trim();
    var State = $('#lstOICompanyInput_InpState').val().trim();
    var Zip = $('#lstOICompanyInput_InpPostalCode').val().trim();
    if ((Address != "" && City != "" && State != "") || (Address != "" && Zip != "")) {
        $('#btnSearchByAddress').removeClass("disabled");
        $('#btnSearchByAddress').removeClass("clsDisable");
    } else {
        $('#btnSearchByAddress').addClass("disabled");
        $('#btnSearchByAddress').addClass("clsDisable");
    }
}
function validCompanySearch() {
    var InpCompanyName = $('#lstOICompanyInput_InpCompanyName').val().trim();
    if (InpCompanyName == "") {
        $('#btnSearchByCompany').addClass("disabled");
        $('#btnSearchByCompany').addClass("clsDisable");

    } else {
        $('#btnSearchByCompany').removeClass("disabled");
        $('#btnSearchByCompany').removeClass("clsDisable");
    }
}
function validEINSearch() {
    var InpCompanyName = $('#lstOICompanyInput_InpEIN').val().trim();
    if (InpCompanyName == "") {
        $('#btnSearchByEIN').addClass("disabled");
        $('#btnSearchByEIN').addClass("clsDisable");

    } else {
        $('#btnSearchByEIN').removeClass("disabled");
        $('#btnSearchByEIN').removeClass("clsDisable");
    }
}

$('body').on('click', '#btnBingSearch', function () {
    var Company = $("#lstOICompanyInput_InpCompanyName").val();
    var Address = $("#lstOICompanyInput_InpAddress1").val();
    var City = $("#lstOICompanyInput_InpCity").val();
    var State = $("#lstOICompanyInput_InpState").val();
    var Phone = $("#lstOICompanyInput_InpPhoneNbr").val();
    var PostalCode = $("#lstOICompanyInput_InpPostalCode").val();
    var Country = $("#lstOICompanyInput_InpCountryISOAlpha2Code").val();

    Company = Company == "" ? "" : Company + " ";
    Address = Address == "" ? "" : Address + " ";
    City = City == "" ? "" : City + " ";
    State = State == "" ? "" : State + " ";
    Phone = Phone == "" ? "" : Phone + " ";
    PostalCode = PostalCode == "" ? "" : PostalCode + " ";

    var QueryString = Company + Address + City + State + Phone + PostalCode + Country;
    $.ajax({
        type: 'GET',
        url: '/OIMatchData/OIBingSearch?Parameters=' + parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIBingSearchModalMain").html(data);
            DraggableModalPopup("#OIBingSearchModal");
        }
    });
});
$('body').on('click', '#btnFinetuneResultFilters', function () {
    $.ajax({
        type: 'POST',
        url: "/OIMatchData/GetUserMatchFilter",
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html;',
        success: function (data) {
            $("#divPartialFineTuneResult").html(data);
            $(".divFineTuneResultFilters > .ajax-dropdown").show();
        }
    });
});
$('body').on('click', '#btnAddFineTuneFilter', function () {
    $(".divFineTuneResultFilters > .ajax-dropdown").hide();
    var url = '/OIMatchData/InsertUpdateUserMatchFilter';
    InsertUpdateFineTuneMatchFilter(url);
});

$('body').on('click', '.editOIUserMatchFilter', function () {
    var FilterId = $(this).attr("id");
    var QueryString = "FilterId:" + FilterId;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

    var url = '/OIMatchData/InsertUpdateUserMatchFilter?Parameters=' + Parameters;
    $(".divFineTuneResultFilters > .ajax-dropdown").hide();
    InsertUpdateFineTuneMatchFilter(url);
});
function InsertUpdateFineTuneMatchFilter(url) {
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIFineTuneMatchFilterModalMain").html(data);
            DraggableModalPopup("#OIFineTuneMatchFilterModal");
            onLoadFineTuneMatchFilter();
        }
    });
}

$(document).on("click", ".DeleteOIUserMatchFilter", function () {
    var id = $(this).attr("id");
    if (id == undefined) {
        id = 0;
    }
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecord, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/OIMatchData/DeleteUserMatchFilter?Parameters=" + parent.ConvertEncrypte(encodeURI(id)).split("+").join("***"),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        if (data == dataDeleted) {
                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                            ShowMessageNotification("success", data, false);
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
                return true;
            }
        }
    });
});

$("body").on('click', '.BtnEnableDisableOIAutoAcceptance', function () {
    var id = $(this).attr("id");
    if (id == undefined) {
        id = 0;
    }
    var IsEnable = $(this).is(":checked");
    var QueryString = "FilterId:" + id + "@#$IsEnable:" + IsEnable;

    var message;
    if (IsEnable) {
        message = enableRecordConfirmBox;
    }
    else {
        message = disableRecordConfirmBox;
    }
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: message, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/OIMatchData/EnableDisabledUserMatchFilter?Parameters=" + parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data, false);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
                return true;
            }
        }
    });
});

$(document).on('change', '#EnableMatchFilter', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var IsApply = $(this).is(':checked');
    var inputId = $("#lstOICompanyInput_InputId").val();
    var QueryString = "inputId:" + inputId + "@#$IsApply:" + IsApply;
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: "POST",
        url: "/OIMatchData/SetApplyFilter?Parameters=" + Parameters,
        headers: { "__RequestVerificationToken": token },
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $('#divPartialSearchDetails').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$(document).on('click', '#btnSendtoORB', function () {
    var InputId = $("#lstOICompanyInput_InputId").val();
    var SrcRecordId = $("#lstOICompanyInput_SrcRecordId").val();
    var InpCompanyName = $("#lstOICompanyInput_InpCompanyName").val();
    var InpAddress1 = $("#lstOICompanyInput_InpAddress1").val();
    var InpAddress2 = $("#lstOICompanyInput_InpAddress2").val();
    var InpCity = $("#lstOICompanyInput_InpCity").val();
    var InpState = $("#lstOICompanyInput_InpState").val();
    var InpPostalCode = $("#lstOICompanyInput_InpPostalCode").val();
    var CountryISOAlpha2Code = $("#lstOICompanyInput_InpCountryISOAlpha2Code").val();
    var Phone = $("#lstOICompanyInput_InpPhoneNbr").val();
    var InpWebsite = $("#lstOICompanyInput_InpWebsite").val();
    var InpEmail = $("#lstOICompanyInput_InpEmail").val();
    var InpEIN = $("#lstOICompanyInput_InpEIN").val();
    var InpOrbNum = $("#lstOICompanyInput_InpOrbNum").val();
    var Tags = $("#lstOICompanyInput_Tags").val();

    var QueryString = "SrcRecordId=" + SrcRecordId
        + "&InputId=" + InputId
        + "&CompanyName=" + InpCompanyName
        + "&Address1=" + InpAddress1
        + "&Address2=" + InpAddress2
        + "&City=" + InpCity
        + "&State=" + InpState
        + "&PostalCode=" + InpPostalCode
        + "&CountryISOAlpha2Code=" + CountryISOAlpha2Code
        + "&PhoneNbr=" + Phone
        + "&Website=" + InpWebsite
        + "&Email=" + InpEmail
        + "&EIN=" + InpEIN
        + "&OrbNum=" + InpOrbNum
        + "&Tags=" + Tags;

    QueryString = "SrcRecordId:" + SrcRecordId + "@#$InputId:" + InputId + "@#$CompanyName:" + InpCompanyName + "@#$Address1:" + InpAddress1 + "@#$Address2:" + InpAddress2 + "@#$City:" + InpCity + "@#$State:" + InpState + "@#$PostalCode:" + InpPostalCode + "@#$CountryISOAlpha2Code:" + CountryISOAlpha2Code + "@#$PhoneNbr:" + Phone + "@#$Website:" + InpWebsite + "@#$Email:" + InpEmail + "@#$EIN:" + InpEIN + "@#$OrbNum:" + InpOrbNum + "@#$Tags:" + Tags;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");


    $.ajax({
        type: 'GET',
        url: '/OIMatchData/SendToOrb?Parameters=' + Parameters,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OISendToOrbCompanyInformationModalMain").html(data);
            DraggableModalPopup("#OISendToOrbCompanyInformationModal");
        }
    });
});
$(document).on('click', '#btnGoogleSearch', function () {
    var Url = "https://www.google.com/search?q=";

    var Company = $("#lstOICompanyInput_InpCompanyName").val();
    var Address = $("#lstOICompanyInput_InpAddress1").val();
    var City = $("#lstOICompanyInput_InpCity").val();
    var State = $("#lstOICompanyInput_InpState").val();
    var Phone = $("#lstOICompanyInput_InpPhoneNbr").val();
    var PostalCode = $("#lstOICompanyInput_InpPostalCode").val();
    var Country = $("#lstOICompanyInput_InpCountryISOAlpha2Code").val();

    var cnt = 0;
    if (!Company && Company == "") {
        cnt++;
        $("#spnInpCompany").show();
    }
    else {
        $("#spnInpCompany").hide();
    }
    if (cnt > 0) {
        return false;
    }
    Company = Company == "" ? "" : Company + " ";
    Address = Address == "" ? "" : Address + " ";
    City = City == "" ? "" : City + " ";
    State = State == "" ? "" : State + " ";
    Phone = Phone == "" ? "" : Phone + " ";
    PostalCode = PostalCode == "" ? "" : PostalCode + " ";

    var QueryString = Url + Company + Address + City + State + Phone + PostalCode + Country;
    $(this).attr("href", QueryString);
});


//function for add match as a new company
$(function () {
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
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
                    var orb_num = $(this).attr("Id");
                    var InputId = $(this).attr("data-MatchInput");
                    var QueryString = "orb_num:" + orb_num + "@#$InputId:" + InputId;
                    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

                    //popup size with tag enable disabled
                    $.ajax({
                        type: 'GET',
                        url: '/OIMatchData/OIAddCompanyMatch?Parameters=' + Parameters,
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#OIAddCompanyMatchModalMain").html(data);
                            DraggableModalPopup("#OIAddCompanyMatchModal");
                            $.UserRole = $("#UserRole").val();
                            $.UserLOBTag = $("#UserLOBTag").val(); onLoadOIAddCompany();
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
// Function for encrypting the parameters


$(document).on('click', '#btnDeleteCompanyData', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var InputId = $("#lstOICompanyInput_InputId").val();
    var url = '/OIMatchData/OIDeleteCompanyRecord?Parameters=' + ConvertEncrypte(encodeURI(InputId)).split("+").join("***");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteCompanyData, callback: function (result) {
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
                        if (data == dataDeleted) {
                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                            ShowMessageNotification("success", data, false);
                            $("#OIMatchDataDetailModal").modal("hide");
                            location.reload();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });

});

// MP-770 Add reject data in Match data (ORB)
$(document).on('click', '.btnOIRejectCandidateRecord', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var inputId = $("#lstOICompanyInput_InputId").val();
    var OrbNum = $(this).attr("data-OrbNum");
    var QueryString = "inputId:" + inputId + "@#$OrbNum:" + OrbNum;
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: rejectCandidate, callback: function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: "/OIMatchData/OIRejectCandidateRecord?Parameters=" + Parameters,
                    headers: { "__RequestVerificationToken": token },
                    dataType: 'JSON',
                    cache: false,
                    contentType: 'application/json;',
                    success: function (data) {
                        ShowMessageNotification("success", data.message, false);
                        if (data.result) {
                            $.ajax({
                                type: "POST",
                                url: "/OIMatchData/RefreshStewardshipQueue/",
                                headers: { "__RequestVerificationToken": token },
                                dataType: "json",
                                contentType: "application/json",
                                success: function () {
                                    reloadMatchSearchDetailList();
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                }
                            });
                        }
                    }
                });
            }
        }
    });
});
function reloadMatchSearchDetailList() {
    var inputId = $("#lstOICompanyInput_InputId").val();
    var QueryString = "InputId:" + inputId;
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: 'POST',
        url: "/OIMatchData/ReloadMatchSearchDetailList?Parameters=" + Parameters,
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html;',
        success: function (data) {
            $("#divPartialSearchDetails").html(data);
        }
    });
}

$('body').on('click', '.btnCompany', function () {
    var Company = $.trim($(".companyName").text());
    $("#lstOICompanyInput_InpCompanyName").val(Company);
});
$('body').on('click', '.btnAddress1', function () {
    var Address1 = $.trim($(".primaryAddress").text());
    $("#lstOICompanyInput_InpAddress1").val(Address1);
});
$('body').on('click', '.btnAddress2', function () {
    var Address2 = $.trim($(".secondaryAddress").text());
    $("#lstOICompanyInput_InpAddress2").val(Address2);
});
$('body').on('click', '.btnCity', function () {
    var City = $.trim($(".city").text());
    $("#lstOICompanyInput_InpCity").val(City);
});
$('body').on('click', '.btnState', function () {
    var State = $.trim($(".state").text());
    $("#lstOICompanyInput_InpState").val(State);
});
$('body').on('click', '.btnPostalCode', function () {
    var PostalCode = $.trim($(".postalCode").text());
    $("#lstOICompanyInput_InpPostalCode").val(PostalCode);
});
$('body').on('click', '.btnCountryCode', function () {
    var CountryCode = $.trim($(".countryCode").text());
    $("#lstOICompanyInput_InpCountryISOAlpha2Code").val(CountryCode);
});
$('body').on('click', '.btnPhoneNbr', function () {
    var PhoneNumber = $.trim($(".phoneNumber").text());
    $("#lstOICompanyInput_InpPhoneNbr").val(PhoneNumber);
});
$('body').on('click', '.btnWebsite', function () {
    var Website = $.trim($(".website").text());
    $("#lstOICompanyInput_InpWebsite").val(Website);
});
$('body').on('click', '.btnEmail', function () {
    var Email = $.trim($(".emailAddress").text());
    $("#lstOICompanyInput_InpEmail").val(Email);
});
$('body').on('click', '.btnEIN', function () {
    var EIN = $.trim($(".ein").text());
    $("#lstOICompanyInput_InpEIN").val(EIN);
});
$('body').on('click', '.btnOrbNum', function () {
    var OrbNum = $.trim($(".orbNumber").text());
    $("#lstOICompanyInput_InpOrbNum").val(OrbNum);
});

function OnSuccessSendToOrb(data) {
    if (data.result) {
        $("#OISendToOrbCompanyInformationModal").modal("hide");
    }
    ShowMessageNotification("success", data.Message, false);
}