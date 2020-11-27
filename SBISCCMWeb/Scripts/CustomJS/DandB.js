//tab events
$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
$(document).ready(function () {
    if (!$('input#EnrichmentAlwaysRefresh').is(':checked')) {
        $("#ENRICHMENT_STALE_NBR_DAYS").attr("disabled", false);
    }
    else {
        $('#ENRICHMENT_STALE_NBR_DAYS').prop('readonly', true);
    }
    if ($.UserType.toLowerCase() == "steward") {
        var IsActiveDirectPlus = $("#IsActiveDirectPlus").val();
        if (IsActiveDirectPlus.toLowerCase() == "true") {
            LoadMonitoringDirectPlus();
        }
    }
    else {
        LoadTransferDunsTag();
        EnableDisableAutoEnrichTag();
    }
    if ($.UserRole.toLowerCase() == "lob") {
        $("#LicenceTab select").attr("disabled", true);
        $("#LicenceTab input").attr("disabled", true);
        $("#Licence input[type=radio]").attr("disabled", false);
    }
});
$('body').on('click', '#IdFeatureTab', function () {
    if (!$(this).parent("li").hasClass("active")) {
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/DNB/Feature", container: '#divPartialDandBFeature',
            timeout: 50000
        }).done(function () {
            LoadTransferDunsTag();
            EnableDisableAutoEnrichTag();
            SetAutoRefreshEnrichment();
        });
    }
});
$('body').on('click', '#IdLicenceTab', function () {
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/DNB/License", container: '#divPartialDandBCredentials',
        timeout: 50000
    }).done(function () {
        LoadLicenceTab();
    });
});
function LoadLicenceTab() {
    if (!$(this).parent("li").hasClass("active")) {
        GetIndexDandBCredentials();
        GetBackgroundProcessSettings();
        GetDefaultInteractiveKeys();
        GetDefaultKeysForEnrichment();
        if ($("#UserRole").val().toLowerCase() == "lob") {
            $("#LicenceTab select").attr("disabled", true);
            $("#LicenceTab input").attr("disabled", true);
            $("#Licence input[type=radio]").attr("disabled", false);
        }
    }
}
$('body').on('click', '#IdIdentityResolutionTab', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabMinimumMatchCriteria").parent("li").addClass("active");
        $("#CleanseMatchTab li").each(function () {
            $(this).removeClass("active");
        });
        $("#CleanseMatchTabcontent").children("div").each(function () {
            $(this).removeClass("active");
        });
        $("#CleanseMatchTab li:first").addClass("active");
        var TabName = $('#CleanseMatchTab li:first').find("a").attr("href");
        $(TabName).addClass("active");
        $("#IdRTabExclusionsCleanseMatch").removeClass("active");
        $("#IdRTabAutoAcceptance").removeClass("active");
        $("#IdRTabAutoAcceptDirectives").removeClass("active");
        $("#IdRTabMultiPassConfig").removeClass("active");
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/DNB/MinimumMatchCriteria", container: '#divPartialMinimumMatchCriteria',
            timeout: 50000
        }).done(function () {
            $("#RTabMinimumMatchCriteria").addClass("active");
            if ($.UserRole.toLowerCase() == "lob") {
                $("#form_MinimumMatchCriteria #Settings_4__SettingValue").attr("disabled", true);
                $('#form_MinimumMatchCriteria #divMinimumConfidenceLevel').addClass("CustomeSliderDisable");
            }
            // Set Slide bar value and Update method call .
            var sliderminConfLevel = document.getElementById('divMinimumConfidenceLevel');
            noUiSlider.create(sliderminConfLevel, {
                start: [0],
                range: {
                    'min': [0],
                    'max': [10]
                },
                step: 1
            });
            var MIN_CONFIDENCE_CODE = $("#MinConfidenceCode").val();
            if (MIN_CONFIDENCE_CODE != "") {
                document.getElementById('divminConfidencelevel').innerHTML = MIN_CONFIDENCE_CODE;
                $("#minConfidencelevel").val(MIN_CONFIDENCE_CODE);
                sliderminConfLevel.noUiSlider.set(MIN_CONFIDENCE_CODE);
            }
            $("#divMinimumConfidenceLevel").addClass("form-control");
            sliderminConfLevel.noUiSlider.on('change', function (values, handle) {
                var divminConfidencelevel = document.getElementById('divminConfidencelevel');
                var key = $("#MinConfidenceCodes").val();
                divminConfidencelevel.innerHTML = parseInt(values[handle]);
                $("#minConfidencelevel").val(parseInt(values[handle]));
            });
            //end slider
            LoadIdentityResolutionTab();
        });
    }
});
function LoadIdentityResolutionTab() {    
    if ($("#LicenseEnableTags").val().toLowerCase() == "true") {
        LoadIndexMinimumConfidenceCodeOverride();
    }
}
$(document).on('click', '#IdDataEnrichmentTab', function () {
    //if (!$(this).parent("li").hasClass("active")) {
    $(".enrichTabs>li").removeClass("active");
    $(".enrichContentDiv>div").removeClass("active");
    $(".enrichTabs>li:first").addClass("active");
    $(".enrichContentDiv>div:first").addClass("active");
    $("#IdRTabEnrichment").parent("li").addClass("active");
    $("#IdRTabDataBlockGroups").removeClass("active");
    $("#IdRTabThirdPartyEnrichment").removeClass("active");
    LoadDataEnrichment();
    //}
});
$('body').on('click', '#IdMonitoringTab', function () {
    $.pjax({
        url: "/DNB/MonitoringProfile", container: '#divPartialMonitoringDirectTwo',
        timeout: 50000
    }).done(function () {
        if (!$(this).parent("li").hasClass("active")) {
            $("#Mornitoring20Credential").val("0");
            $("#divPartialMonitoringTabs").html("");
        }
    });
});
$('body').on('click', '#IdRTabMinimumMatchCriteria', function () {
    if (!$(this).parent("li").hasClass("active")) {
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/DNB/MinimumMatchCriteria", container: '#divPartialMinimumMatchCriteria',
            timeout: 50000
        }).done(function () {
            LoadIdentityResolutionTab();
            // Set Slide bar value and Update method call .
            var sliderminConfLevel = document.getElementById('divMinimumConfidenceLevel');
            noUiSlider.create(sliderminConfLevel, {
                start: [0],
                range: {
                    'min': [0],
                    'max': [10]
                },
                step: 1
            });
            var MIN_CONFIDENCE_CODE = $("#MinConfidenceCode").val();
            if (MIN_CONFIDENCE_CODE != "") {
                document.getElementById('divminConfidencelevel').innerHTML = MIN_CONFIDENCE_CODE;
                $("#minConfidencelevel").val(MIN_CONFIDENCE_CODE);
                sliderminConfLevel.noUiSlider.set(MIN_CONFIDENCE_CODE);
            }
            $("#divMinimumConfidenceLevel").addClass("form-control");
            sliderminConfLevel.noUiSlider.on('change', function (values, handle) {
                var divminConfidencelevel = document.getElementById('divminConfidencelevel');
                var key = $("#MinConfidenceCodes").val();
                divminConfidencelevel.innerHTML = parseInt(values[handle]);
                $("#minConfidencelevel").val(parseInt(values[handle]));
            });
            //end slider
        });
    }
});
$('body').on('click', '#IdRTabExclusionsCleanseMatch', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabMinimumMatchCriteria").parent("li").removeClass("active");
        GetIndexExclusionsCleanseMatch();

    }
});
$('body').on('click', '#IdRTabAutoAcceptance', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabMinimumMatchCriteria").parent("li").removeClass("active");
        LoadIndexAutoAcceptance("");
    }
});
$('body').on('click', '#IdRTabAutoAcceptDirectives', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabMinimumMatchCriteria").parent("li").removeClass("active");
        LoadIndexAutoAcceptDirectives();
    }
});
$('body').on('click', '#IdRTabMultiPassConfig', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabMinimumMatchCriteria").parent("li").removeClass("active");
        LoadIndexMultiPassConfig();
    }
});
$('body').on('click', '#IdRTabMonitoring', function () {
    if (!$(this).parent("li").hasClass("active")) {
        LoadMonitoring();
    }
});
$('body').on('click', '#IdRTabUserPreference', function () {
    if (!$(this).parent("li").hasClass("active")) {
        LoadUserPrefrence();
    }
});
$('body').on('click', "#IdMonitoringDirectPlusTab", function () {
    if (!$(this).parent("li").hasClass("active")) {
        LoadMonitoringDirectPlus();
    }
});
$('body').on('click', '#IdRTabNotificationProfile', function () {
    if (!$(this).parent("li").hasClass("active")) {
        LoadNotificationProfile();
    }
});
$('body').on('click', '#IdRTabDUNSRegistration', function () {
    if (!$(this).parent("li").hasClass("active")) {
        onloadDUNSregistration();
    }
});
function ExclusionLoadTags() {
    for (var i = 1; i <= 5; i++) {
        var ids = "#Tags" + i;
        var TagList = $("#TagList" + i).val().split(',');

        if (TagList != null || TagList != "") {
            $(".clstag" + i + " option").each(function () {
                if (isInArray($(this).val(), TagList)) {
                    $(this).attr("selected", "selected");
                }
            });
            $(ids).val(TagList);
        }
    }
    if ($(".chzn-select.Exclusion").length > 0) {
        $(".chzn-select.Exclusion").chosen().change(function (event) {

            var id = "#" + $(this).attr('id').replace('TagsValue', 'Tags');
            if (event.target == this) {
                var oldval = $(id).val();
                if (oldval != undefined) {
                    oldval = oldval + $(this).val();
                } else {
                    oldval = $(this).val();
                }
                $(id).val($(this).val());
            }
        });
    }
    $(".chzn-select").trigger("chosen:updated");
}
function DirectiveLoadTags() {
    for (var i = 6; i <= 19; i++) {
        var id = "#TagList" + i;
        var ids = "#Tags" + i;

        var TagList = $(id).val().split(',');
        if (TagList != null || TagList != "") {
            $(".clstag" + i + " option").each(function () {
                if (isInArray($(this).val(), TagList)) {
                    $(this).attr("selected", "selected");
                }
            });

            $(ids).val(TagList);
        }
    }
    if ($(".chzn-select.Directive").length > 0) {
        $(".chzn-select.Directive").chosen().change(function (event) {
            var id = "#" + $(this).attr('id').replace('TagsValue', 'Tags');
            if (event.target == this) {
                var oldval = $(id).val();
                if (oldval != undefined) {
                    oldval = oldval + $(this).val();
                } else {
                    oldval = $(this).val();
                }
                $(id).val($(this).val());
            }
        });
    }
    $(".chzn-select").trigger("chosen:updated");
}
$(document).on("keypress", ".OnlyDigit", function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});
$("body").on('blur', '.OnlyDigit', function () {
    var text = $(this).val();
    if (text != "") {
        if (!$.isNumeric(text)) {
            $(this).val("");
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success", allowNumbers);
        }
    }
});
$('body').on('change', '#RegistrationsName', function () {
    var referenceName = $(this).val();
    LoadMonitoringPlusRegistrationDetail(referenceName);
});
//New Process settings for transfer duns enrichment MP-507
function LoadTransferDunsTag() {
    var TagList = $("#TransferDunsTagList").val();
    if (TagList == undefined) {
        TagList = "";
    }
    TagList = TagList.split(',');
    if (TagList != null || TagList != "") {
        $("#TransferDunsTagValue option").each(function () {
            for (var i = 0; i < TagList.length; i++) {
                if ($(this).val() == TagList[i]) {
                    $(this).attr("selected", "selected");
                }
            }
        });
        $("#TRANSFER_DUNS_AUTO_ENRICH_TAG").val(TagList);
    }
    if ($("#TransferDunsTagValue").length > 0) {
        $("#TransferDunsTagValue").chosen().change(function (event) {
            if (event.target == this) {
                $("#TRANSFER_DUNS_AUTO_ENRICH_TAG").val($(this).val());
            }
        });
    }
}
$('body').on('click', '#IdRTabEnrichment', function () {
    if (!$(this).parent("li").hasClass("active")) {
        LoadDataEnrichment();
        $("#IdRTabEnrichment").parent("li").addClass("active");
    }
});
$(document).on('click', '#IdRTabDataBlockGroups', function () {
    //if (!$(this).parent("li").hasClass("active")) {
    $("#IdRTabEnrichment").parent("li").removeClass("active");
    LoadDataBlockGroups();
    //}
});
function InitMinimumMatchCriteriaDataTable() {
    InitDataTable(".MinCCOverrideTB", [10, 20, 30], false, [0, "desc"]);
}
$(document).on('change', '#AutoAcceptance_Active', function () {
    var currentVal = $(this).prop('checked');
    //$("#frmExportToExcel #Active").val(currentVal);
});
//====================================================================================================================
//Start Reset System data
//Rest System data
$('body').on('click', '#btnResetSystemData', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: '/DandB/ResetSystemData?isResetConfig=false',
        dataType: 'HTML',
        success: function (data) {
            $("#ResetSystemDataModalMain").html(data);
            DraggableModalPopup("#ResetSystemDataModal");
        }
    });
});
function ResetCallback() {
    $("#ResetSystemDataModal").modal("hide");
    location.reload();
}

//End Reset System data
//====================================================================================================================

//Start DNBFeature 
$(document).on('change', '#TRANSFER_DUNS_AUTO_ENRICH', function () {
    EnableDisableAutoEnrichTag();
});
function EnableDisableAutoEnrichTag() {
    if (!$('input#TRANSFER_DUNS_AUTO_ENRICH').is(':checked')) {
        $("#addTransferDunsTag").hide();
        $("#TransferDunsTagValue").attr("disabled", "disabled");
    }
    else {
        $("#addTransferDunsTag").show();
        $("#TransferDunsTagValue").attr("disabled", false);
    }
    $("#TransferDunsTagValue").trigger("chosen:updated");
}

// MP-920 UI changes
$(document).on('change', '#EnrichmentAlwaysRefresh', function () {
    SetAutoRefreshEnrichment();
});
$('body').on('blur', '#ENRICHMENT_STALE_NBR_DAYS', function () {
    var EnrichmentPeriodDays = $('#ENRICHMENT_STALE_NBR_DAYS').val();
    if (EnrichmentPeriodDays < 1 || EnrichmentPeriodDays > 30) {
        if (EnrichmentPeriodDays == -1) {
            return false;
        }
        else {
            $('#ENRICHMENT_STALE_NBR_DAYS').val("");
            return false;
        }
    }
    else {
        $('.EnrichmentPeriodDaysError').attr("hidden", "hidden");
        return true;
    }
});
function SetAutoRefreshEnrichment() {
    if (!$('input#EnrichmentAlwaysRefresh').is(':checked')) {
        $('#ENRICHMENT_STALE_NBR_DAYS').val(7);
        $('#ENRICHMENT_STALE_NBR_DAYS').prop('readonly', false);
    }
    else {
        $('#ENRICHMENT_STALE_NBR_DAYS').val(-1);
        $('#ENRICHMENT_STALE_NBR_DAYS').prop('readonly', true);
    }
}
//De-Duplicate Data button added
$('body').on('click', '#btnDuplicateData', function () {
    // Changes for Converting magnific popup to modal popup
    //popup size with tag enable disabled
    var PopupClassName = "";
    if ($.IsTagsLicenseAllow.toLowerCase() == "true") {
        PopupClassName = 'popDeDuplicate'
    }
    else {
        PopupClassName = 'popDeDuplicateNoTag'
    }
    $.ajax({
        type: 'GET',
        url: "/DNBFeature/DuplicateData",
        dataType: 'HTML',
        success: function (data) {
            $("#DeDuplicateDataModalMain").html(data);
            DraggableModalPopup("#DeDuplicateDataModal");
        }
    });
});

//MP-523 Provide ability to de-duplicate data in Match/No-Match and Export queues
//De-Duplicate Data added in dropdown and on submitting below event is called
$("body").on('click', '#btnDeDuplicateData', function () {
    var Tag = $("#divDeDuplicate #Tag").val();
    var LOBTag = $("#divDeDuplicate #LOBTag").val();
    var CountryCode = $("#divDeDuplicate #CountryCode").val();
    var CountryGroupId = $("#divDeDuplicate #CountryGroupId").val();

    if (Tag == undefined || Tag == "" || Tag == null) {
        Tag = "";
    }

    if (LOBTag == undefined || LOBTag == "") {
        LOBTag = "";
    }
    var token = $('input[name="__RequestVerificationToken"]').val();

    // D&B - Provide Confirmation Button on De-Duplicate Data Filter (MP-721)
    // D&B - When user upload file which does not have country code get upload from Low Volume but from High volume error appear (MP-732)
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: purgeDuplicatemsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/DNBFeature/DuplicateData",
                    data: JSON.stringify({ Tag: Tag, LOBTag: LOBTag, CountryCode: CountryCode, CountryGroupId: CountryGroupId }),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.message);
                        $("#DeDuplicateDataModal").modal("hide");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    return true;
});


// Added Cached Data settings in feature tab
$('body').on('click', '.cachedDataSettings', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/DNBFeature/CachedDataSettings",
        dataType: 'HTML',
        success: function (data) {
            $("#CachedDataModalMain").html(data);
            DraggableModalPopup("#CachedDataModal");
        }
    });
});
//End DNBFeature

//====================================================================================================================

//Start Monitoring Direct Plus
function LoadMonitoringPlusRegistrationDetail(referenceName) {
    var url = '/DNBMonitoringDirectPlus/GetMonitoringPlusRegistrationDetail/' + "?Parameters=" + ConvertEncrypte(encodeURI(referenceName)).split("+").join("***");
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $("#partialMonitoringPlusRegistration").html(data);
            MonitoringTagLoad();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function LoadMonitoringDirectPlus() {
    // // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/DNB/MonitoringDirectPlus", container: '#divPartialMonitoringDirectPlus',
        timeout: 50000
    }).done(function (data) {
        if (data != null && data != "" && data != undefined && data.indexOf("No record(s) Found") < 0) {
            $("table.MonitoringPlusRegistrationTb tbody tr:first").addClass("current");
        }
        MonitoringTagLoad();
    });

}
function MonitoringTagLoad() {
    if ($(".formMonitoringDnBPlus #MonitoringRegistrationTagList").val() != undefined && $(".formMonitoringDnBPlus #MonitoringRegistrationTagList").val() != null) {
        var TagList = $(".formMonitoringDnBPlus #MonitoringRegistrationTagList").val().split(',');
        if (TagList != null || TagList != "") {
            $(".chzn-select.monitoringDirectPlusTag option").each(function () {
                for (var i = 0; i < TagList.length; i++) {
                    if ($(this).val() == TagList[i]) {
                        $(this).attr("selected", "selected");
                    }
                }
            });
            $(".formMonitoringDnBPlus #messages_registration_Tags").val(TagList);
        }
        if ($(".chzn-select.monitoringDirectPlusTag").length > 0) {
            $(".chzn-select.monitoringDirectPlusTag").chosen().change(function (event) {
                if (event.target == this) {
                    $(".formMonitoringDnBPlus #messages_registration_Tags").val($(this).val());
                }
            });
        }
    }
}

$("body").on("click", "#editMonitoringPlusRegistration", function () {

    $('#messages_registration_reference').attr("disabled", false);
    $('#messages_registration_productId').attr("disabled", false);
    $('#messages_registration_versionId').attr("disabled", false);
    $('#messages_registration_email').attr("disabled", false);
    $('#messages_registration_fileTransferProfile').attr("disabled", false);
    $('#messages_registration_description').attr("disabled", false);
    $('#messages_registration_deliveryTrigger').attr("disabled", false);
    $('#messages_registration_deliveryFrequency').attr("disabled", false);
    $('#messages_dunsCount').attr("disabled", false);
    $('#messages_registration_seedData').attr("disabled", false);
    $('#messages_notificationsSuppressed').attr("disabled", false);
    $('#UpdateMonitoringRegistrationDetail').show();
    $('.tagstyleMonitoring').show();
    $('#TagsValueMonitoringDnBplus').attr("disabled", false);
    $(".chzn-select.monitoringDirectPlusTag").trigger("chosen:updated");
});

$("body").on("click", "#btnSyncDUNS", function () {
    $.ajax({
        type: "POST",
        url: "/DNBMonitoringDirectPlus/SyncDUNS",
        dataType: "json",
        contentType: "application/json",
        async: false,
        success: function (data) {
            if (data.result) {
                ShowMessageNotification("success", data.message);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$("body").on("click", ".SuppUnsuppDUNS", function () {
    var referenceName = $(this).attr("id");
    var isSuprressed = $(this).attr("data-isSuprressed");
    var AuthToken = $(this).attr("data-AuthToken");
    var message;
    if (isSuprressed == "True") {
        message = unSuppDUNS;
    }
    else {
        message = suppDUNS;
    }
    var QueryString = "referenceName:" + referenceName + "@#$isSuprressed:" + isSuprressed + "@#$AuthToken:" + AuthToken;
    $.ajax({
        type: "POST",
        url: "/DNBMonitoringDirectPlus/SuppUnsuppDUNS",
        data: JSON.stringify({
            Parameters: ConvertEncrypte(encodeURI(QueryString)).split("+").join("***")
        }),
        dataType: "json",
        contentType: "application/json",
        async: false,
        success: function (data) {
            ShowMessageNotification("success", data.message);
            if (data.result) {
                LoadMonitoringDirectPlus();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
$("body").on('click', '.ViewDUNSDetails', function () {
    // Changes for Converting magnific popup to modal popup
    var RegistrationsName = $(this).attr("data-RegistrationsName");
    var AuthToken = $(this).attr("data-AuthToken");
    var QueryString = "RegistrationsName:" + RegistrationsName + "@#$AuthToken:" + AuthToken + "@#$IsFromMainPage:true";
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/DNBMonitoringDirectPlus/MonitoringPlusRegistrationDUNSDetails?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#MonitoringDirectPlusDUNSDetailModalMain").html(data);
            DraggableModalPopup("#MonitoringDirectPlusDUNSDetailModal");
        }
    });
});

function UpdateMonitoringRegistrationSuccess(data) {
    ShowMessageNotification("success", data.message);
    if (data.result) {
        LoadMonitoringDirectPlus();
    }
}

$('body').on('click', 'table.MonitoringPlusRegistrationTb tbody tr', function () {
    if (!$(this).hasClass("current")) {
        if ($(this).html().indexOf("No record(s) Found") > 0) {
            return false;
        }
        $(".MonitoringPlusRegistrationTb tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var QueryString = $(this).attr("data-MonitoringProfilePlusRegistrationID");
        LoadMonitoringPlusRegistrationDetail(QueryString);

    }
});
//End Monitoring Direct Plus

//====================================================================================================================


