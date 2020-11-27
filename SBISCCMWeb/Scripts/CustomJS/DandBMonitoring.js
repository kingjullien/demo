$(document).ready(function () {
    var LevelValue = $("#Level").val();
    if (LevelValue != undefined && LevelValue == "Level1") {
        $(".rButton[value='Level1']").attr("checked", true);

    } else if (LevelValue != undefined && LevelValue == "Level2") {

        $(".rButton[value='Level2']").attr("checked", true);
    } else if (LevelValue == undefined || LevelValue == "") {
        $(".rButton[value='Level1']").attr("checked", true);

    }
});
//====================================================================================================================

//Start Monitoring Tab
$(document).on('change', '#Mornitoring20Credential', function () {
    var Mornitoring20Credential = $(this).val();
    if (Mornitoring20Credential == undefined || Mornitoring20Credential == "0" || Mornitoring20Credential == "") {
        $("#Mornitoring20Credential").val("0");
        $("#divPartialMonitoringTabs").html("");
        $("#Mornitoring20Credential").attr("disabled", false);
        return false;
    }
    else {
        $.ajax({
            type: 'GET',
            url: "/DNBMonitoring/IndexLoadMornitoringTabs?Parameters=" + ConvertEncrypte(encodeURI(Mornitoring20Credential)).split("+").join("***"),
            dataType: 'HTML',
            contentType: 'application/html',
            cache: false,
            async: false,
            success: function (data) {
                $("#divPartialMonitoringTabs").html(data);
                var monitorID = $("table.MonitoringTb tbody tr:first").attr("data-MonitoringProfileID");
                selectFirstTR();
            },
            error: function (e, er, err) {
            }
        });
    }
});
function LoadMonitoring() {
    $.ajax({
        type: 'GET',
        url: "/DNBMonitoring/IndexMonitoringProfile",
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#divPartialMonitoringData").html(data);
            if ($("table.MonitoringTb tbody tr:first").html().indexOf("No record(s) Found") < 0) {
                $("table.MonitoringTb tbody tr:first").addClass("current");
            }
            OnSuccessMonitoringProfile();
        }
    });
}
$('body').on('click', '.CreateMonitoring', function () {
    var ProfileName = $("#profileName").val().trim();
    var Description = $("#description").val().trim();
    var ProductId = $("#Product").val();
    var ProductCode = $("#Product option:selected").text();
    var BusinessLevel = $("input:radio[name='BusinessLevel']:checked").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var ProfileId = $("#ProfileId").val();
    var MonitoringProfileID = $("#MonitoringProfileID").val();
    var CustomerReferenceText = $("#CustomerReferenceText").val().trim();
    var count = 0;
    if (ProfileName == '' || ProfileName == undefined || ProfileName == null) {
        count++;
        $("#spnName").show();
    }
    else {
        $("#spnName").hide();
    }
    if (Description == '' || Description == undefined || Description == null) {
        count++;
        $("#spnDescription").show();
    }
    else {
        $("#spnDescription").hide();
    }
    if (ProductId == '' || ProductId == undefined || ProductId == null) {
        count++;
        $("#spnProduct").show();
    }
    else {
        $("#spnProduct").hide();
    }


    if (count > 0) { return false; }

    if (MonitoringProfileID == undefined || MonitoringProfileID == '' || MonitoringProfileID == null) {
        MonitoringProfileID = 0;
    }
    var QueryString = "ProfileName:" + ProfileName + "@#$Description:" + Description + "@#$ProductId:" + ProductId + "@#$BusinessLevel:" + BusinessLevel + "@#$ProductCode:" + ProductCode + "@#$MonitoringProfileID:" + MonitoringProfileID + "@#$CustomerReferenceText:" + CustomerReferenceText;
    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/CreateMonitorProfile",
        data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(QueryString)).split("+").join("***") }),
        dataType: "json",
        contentType: "application/json",
        headers: { "__RequestVerificationToken": token },
        cache: false,
        success: function (data) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success", data.message);
            UpdateSetMonitorProfile(data);
            if (data.result) {
                LoadMonitoring();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function UpdateSetMonitorProfile(data) {
    if ((data.message.indexOf("profile created successfully") > 0) || (data.message.indexOf("profile updated successfully") > 0)) {
        $("#profileName").prop("disabled", true);
        $("#description").prop("disabled", true);
        $("#Product").prop("disabled", true);
        $("#chkMonitoringChange").prop("disabled", true);
        $(".rButton").prop("disabled", true);
        $("#btnCreateMonitoringProfile").prop("disabled", true);
        $("#CustomerReferenceText").prop("disabled", true);
        $(".CreateMonitoring").hide();
        $("#btnCancel").hide();


        var pagevalue = $(".pagevalueChangeMonitorProfile").val();
        var SortBy = $("#MonitoringSortBy").val();
        var SortOrder = $("#MonitoringSortOrder").val();
        var Page = $("#MonitoringPageNo").val();
        var url = '/DandB/IndexMonitoringProfile/' + "?page=" + Page + "&pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
        $.ajax({
            type: "POST",
            url: url,
            dataType: "HTML",
            contentType: "application/html",
            cache: false,
            success: function (data) {
                $("#divPartialMonitoringData").html(data);
                selectFirstTR();
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
}
$('body').on('click', '#btnCreateMonitoringProfile', function () {
    var ProfileName = $("#profileName").val();
    var Description = $("#description").val();
    var ProductId = $("#Product").val();
    var ProductCode = $("#Product option:selected").text();
    var BusinessLevel = $("input:radio[name='BusinessLevel']:checked").val();
    var cnt = 0;
    if (ProfileName == "" || ProfileName == undefined) {
        $("#spnName").show();
        cnt++;
    }
    else {
        $("#spnName").hide();
    }
    if (Description == "" || Description == undefined) {
        $("#spnDescription").show();
        cnt++;
    }
    else {
        $("#spnDescription").hide();
    }
    if (ProductId == "" || ProductId == undefined) {
        $("#spnProduct").show();
        cnt++;
    }
    else {
        $("#spnProduct").hide();
    }
    if (cnt > 0) {
        return false;
    }
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/SetMonitoringData",
        data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(ProductId)).split("+").join("***"), ProductCode: ConvertEncrypte(encodeURI(ProductCode)).split("+").join("***"), BusinessLevel: ConvertEncrypte(encodeURI(BusinessLevel)).split("+").join("***") }),
        cache: false,
        dataType: "json",
        headers: { "__RequestVerificationToken": token },
        contentType: "application/json",
        success: function (data) {
            if (data.result) {
                // Changes for Converting magnific popup to modal popup
                $.ajax({
                    type: 'GET',
                    url: "/DNBMonitoring/BusinessElement?Parameters="+ ConvertEncrypte(encodeURI("IsFromMainPage:true")).split("+").join("***"),
                    dataType: 'HTML',
                    success: function (data) {
                        $("#BusinessElementModalMain").html(data);
                        DraggableModalPopup("#BusinessElementModal");
                    }
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
$('body').on('change', '#chkMonitoringChange', function () {
    var IsCheck = $(this).prop('checked');
    if (IsCheck == false) {
        $("#btnCreateMonitoringProfile").removeAttr("disabled");
    } else {
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteElements, callback: function (result) {
                if (result) {
                    var ProfileId = $("#ProfileId").val();
                    if (ProfileId == undefined || ProfileId == '') {
                        ProfileId = '';
                    }
                    $("#btnCreateMonitoringProfile").attr("disabled", "disabled");
                    $.ajax({
                        type: "GET",
                        url: "/DNBMonitoring/RemoveCondition?Parameters=" + ConvertEncrypte(encodeURI(ProfileId)).split("+").join("***"),
                        dataType: "html",
                        contentType: "application/html",
                        cache: false,
                        success: function (data) {

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    })
                }
                else {
                    $("#chkMonitoringChange").prop('checked', false);
                }
            },
        });
    }
});
$('body').on('click', '.DeleteMonitorProfile', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var QueryString = $(this).attr("id");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteMonitorProfile, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/DNBMonitoring/DeleteProfile?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        ShowMessageNotification("success", data.message);
                        if (data.result) {
                            LoadMonitoring();
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
$('body').on('click', '#btnCancel', function () {
    ClearMonitoringProfile();
    selectFirstTR();
});
$('body').on('click', '.syncProfileList', function () {
    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/GetProfileList",
        dataType: "JSON",
        cache: false,
        contentType: "application/json",
        success: function (data) {
            if (data.result) {
                LoadMonitoring();
            }
            else {
                ShowMessageNotification("success", data.message);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
$('body').on('click', '.syncUserPrefrence', function () {
    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/GetUserPrefrenceList",
        dataType: "JSON",
        contentType: "application/json",
        cache: false,
        success: function (data) {
            var url = '/DNBMonitoring/IndexUserPreference';
            LoadUserPreference(url);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function OnSuccessMonitoringProfile() {
    selectFirstTR();
}
function ClearMonitoringProfile() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/CancelMonitoringProfile",
        dataType: "json",
        headers: { "__RequestVerificationToken": token },
        contentType: "application/json",
        async: false,
        success: function (data) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    $("#MonitoringProfileId").val("0");
    $("#profileName").val("");
    $("#description").val("");
    $("#Product").val($("#Product option:first").val());
    $("#chkMonitoringChange").prop('checked', true);
    $("#btnCreateMonitoringProfile").attr("disabled", "disabled");
    $(".rButton[value='Level1']").attr("checked", true);
    $("#CustomerReferenceText").val("");

    $("#spnName").hide();
    $("#spnDescription").hide();
    $("#spnProduct").hide();
    $("#spnLevel").hide();

    $(".CreateMonitoring").html(createMonitoringProfile);
}
function enableMonitorProfileFields() {
    $("#profileName").prop("disabled", false);
    $("#description").prop("disabled", false);
    $("#Product").prop("disabled", false);
    $("#chkMonitoringChange").prop("disabled", false);
    $(".rButton").prop("disabled", false);
    $("#btnCreateMonitoringProfile").prop("disabled", false);
    $("#CustomerReferenceText").prop("disabled", false);
    $(".CreateMonitoring").show();
    $("#btnCancel").show();
}
$('body').on('click', '#EditMonitorProfile', function () {
    enableMonitorProfileFields();
    if ($("#chkMonitoringChange").prop('checked')) {
        $("#btnCreateMonitoringProfile").prop("disabled", true);
    }
});
$('body').on('click', '#AddMonitorProfile', function () {
    ClearMonitoringProfile();
    enableMonitorProfileFields();
    $("monitoringFrm #ProfileId").val('0');
    $("#monitoringFrm #MonitoringProfileID").val('0');
    $("#btnCreateMonitoringProfile").prop("disabled", true);
    $(".MonitoringTb tr").each(function () {
        $(this).removeClass('current');
    });
});
$('body').on('click', 'table.MonitoringTb tbody tr', function () {
    if (!$(this).hasClass("current")) {
        if ($(this).html().indexOf("No record(s) Found") > 0) {
            return false;
        }
        $(".MonitoringTb tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var QueryString = $(this).attr("data-MonitoringProfileID");
        var url = "/DNBMonitoring/CreateMonitorProfile?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        $.ajax({
            type: "GET",
            url: url,
            dataType: "HTML",
            contentType: "application/html",
            async: false,
            cache: false,
            success: function (data) {
                $("#divPartialAddUpdateMonitoringData").html(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
});
function selectFirstTR() {
    $("table.MonitoringTb tr").each(function () {
        $(this).removeClass('current');
    });
    $("table.MonitoringTb tbody tr:first").addClass("current");
    var QueryString = $("table.MonitoringTb tbody tr:first").attr("data-MonitoringProfileID");
    if (QueryString == undefined) {
        QueryString = 0;
    }
    if ($("table.MonitoringTb tbody tr:first").html().indexOf("No record(s) Found") > 0) {
        $("table.MonitoringTb tbody tr:first").removeClass("current");
    }
    var url = "/DNBMonitoring/CreateMonitorProfile?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        cache: false,
        success: function (data) {
            $("#divPartialAddUpdateMonitoringData").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

//End Monitoring Tab
//====================================================================================================================

//Start User Preference

function LoadUserPrefrence() {
    var url = "/DNBMonitoring/IndexUserPreference";
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'JSON',
        contentType: 'application/json',
        cache: false,
        success: function (data) {
            $("#divPartialUserPrefrenceData").html(data.indexUserPrefrnc);
            $("#divPartialAddUpdateUserPreference").html(data.upsertUserPrefrnc);
        }, error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function OnSuccessUserPreference(data) {
    ShowMessageNotification("success", data.message);
    if (data.result) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        LoadUserPrefrence();
    }
}
$('body').on('click', '.DeleteUserPrefrence', function () {
    var QueryString = $(this).attr("id");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecord, callback: function (result) {
            if (result) {
                $("#divProgress").show();
                var token = $('input[name="__RequestVerificationToken"]').val();
                $.ajax({
                    type: "POST",
                    url: "/DNBMonitoring/DeleteUserPrefrence?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.message);
                        if (data.result) {
                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                            LoadUserPrefrence();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    $('#divProgress').hide();
    return false;
});
function enableUserPreferenceFields() {
    $("#PreferenceName").prop("disabled", false);
    $("#PreferenceDescription").prop("disabled", false);
    $("#PreferenceValue").prop("disabled", false);
    $("#DefaultPreference").prop("disabled", false);
    $("#btnUserPreference").show();
}
$('body').on('click', '#EditUserPrefrence', function () {
    enableUserPreferenceFields();
});
$('body').on('click', '#AddUserPreference', function () {
    $(".UserPreferenceTb tr").each(function () {
        $(this).removeClass('current');
    });
    enableUserPreferenceFields();
    $("#PreferenceName").val("");
    $("#PreferenceDescription").val("");
    $("#PreferenceValue").val("dnbnotifications@matchbookservices.com");
    $("#frmUserPreference #PreferenceID").val("0");
    $("#btnUserPreference").val(createUserPreference);
    $("#DefaultPreference").prop("checked", false);

});
function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}
$("body").on('click', '#btnUserPreference', function () {
    var count = 0;
    if ($.trim($("#PreferenceName").val()) == '' || $("#PreferenceName").val() == undefined || $("#PreferenceName").val() == null) {
        count++;
        $("#spnPreferenceName").show();
    }
    else {
        $("#spnPreferenceName").hide();
    }
    if ($.trim($("#PreferenceDescription").val()) == '' || $("#PreferenceDescription").val() == undefined || $("#PreferenceDescription").val() == null) {
        count++;
        $("#spnDesc").show();
    }
    else {
        $("#spnDesc").hide();
    }
    if ($.trim($("#PreferenceValue").val()) == '' || $("#PreferenceValue").val() == undefined || $("#PreferenceValue").val() == null) {
        count++;
        $("#spnEmailRequired").show();
    }
    else {
        $("#spnEmailRequired").hide();
    }
    if (!validateEmail($("#PreferenceValue").val())) {
        count++;
        $("#spnEmail").show();
    }
    else {
        $("#spnEmail").hide();
    }
    if (count > 0)
        return false;
    else
        return true;
});
function LoadAddUpdateUserPreference(id) {
    var url = '/DNBMonitoring/AddUpdateUserPreference?Parameters=' + ConvertEncrypte(encodeURI(id)).split("+").join("***");
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divPartialAddUpdateUserPreference").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
$('body').on('click', 'table.UserPreferenceTb tbody tr', function () {
    if (!$(this).hasClass("current")) {
        if ($(this).html().indexOf("No record(s) Found") > 0) {
            return false;
        }
        $(".UserPreferenceTb tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var id = $(this).attr("data-PreferenceID");
        LoadAddUpdateUserPreference(id);
    }
});

//End User Preference

//====================================================================================================================

//Start Notification Profile 

function LoadNotificationProfile() {
    var url = "/DNBMonitoring/IndexNotificationProfile";
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'JSON',
        contentType: 'application/json',
        cache: false,
        success: function (data) {
            $("#divPartialNotificationProfileData").html(data.indexNotificationProfile);
            $("#divPartialAddUpdateNotificationProfileData").html(data.upsertNotificationProfile);
        }, error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function OnSuccessUpsertNotificationProfile(data) {
    ShowMessageNotification("success", data.message);
    if (data.result) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        LoadNotificationProfile();
    }
}
function enableNotificationProfileFields() {
    $("#NotificationProfileName").prop("disabled", false);
    $("#NotificationProfileDescription").prop("disabled", false);
    $("#DeliveryChannelUserPreferenceName").prop("disabled", false);
    $("#DeliveryFrequency").prop("disabled", false);
    $("#StopDeliveryIndicator").prop("disabled", false);
    $("#CompressedProductIndicator").prop("disabled", false);
    $("#InquiryReferenceText").prop("disabled", false);
    $("#NotificationProfileID").prop("disabled", false);
    $("#btnNotificationProfile").show();
}
$('body').on('click', '#EditNotificationProfile', function () {
    enableNotificationProfileFields();
});
$('body').on('click', '#AddNotificationProfile', function () {
    enableNotificationProfileFields();
    $(".NotificationProfileTb tr").each(function () {
        $(this).removeClass('current');
    });
    $("#NotificationProfileName").val("");
    $("#NotificationProfileDescription").val("");
    $("#DeliveryChannelUserPreferenceName").val($("#DeliveryChannelUserPreferenceName option:first").val());
    $("#DeliveryFrequency").val($("#DeliveryFrequency option:first").val());
    $("#StopDeliveryIndicator").prop("checked", false);
    $("#CompressedProductIndicator").prop("checked", false);
    $("#InquiryReferenceText").val("");
    $("#NotificationProfileID").val("0");

    $("#btnNotificationProfile").val(createNotificationProfile);
});
$('body').on('click', '.DeleteNotificationProfile', function () {
    var pagevalue = $("#NotificationPageValue").val();
    var SortBy = $("#NotificationSortBy").val();
    var SortOrder = $("#NotificationSortOrder").val();
    var page = $("#NotificationyPageNo").val();

    var token = $('input[name="__RequestVerificationToken"]').val();
    var QueryString = $(this).attr("id");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecord, callback: function (result) {
            if (result) {
                $("#divProgress").show();
                $.ajax({
                    type: "POST",
                    url: "/DNBMonitoring/DeleteNotificationProfile?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                    headers: { "__RequestVerificationToken": token },
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.message);
                        if (data.result) {
                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                            LoadNotificationProfile();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    $('#divProgress').hide();
    return false;
});
function LoadAddUpdateNotificationProfile(id) {
    var url = '/DNBMonitoring/AddUpdateNotificationProfile?Parameters=' + ConvertEncrypte(encodeURI(id)).split("+").join("***");
    $.ajax({
        beforeSend: function () { $('#divProgress').show(); },
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divPartialAddUpdateNotificationProfileData").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
$('body').on('click', '.syncNotification', function () {
    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/GetNotificationList",
        dataType: "JSON",
        contentType: "application/json",
        cache: false,
        success: function (data) {
            LoadNotificationProfile();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
$("body").on('click', '#btnNotificationProfile', function () {
    var count = 0;
    if ($.trim($("#NotificationProfileName").val()) == '' || $("#NotificationProfileName").val() == undefined || $("#NotificationProfileName").val() == null) {
        count++;
        $("#spnNotificationName").show();
    }
    else {
        $("#spnNotificationName").hide();
    }

    if ($.trim($("#NotificationProfileDescription").val()) == '' || $("#NotificationProfileDescription").val() == undefined || $("#NotificationProfileDescription").val() == null) {
        count++;
        $("#spnspnNotificationDesc").show();
    }
    else {
        $("#spnspnNotificationDesc").hide();
    }

    if ($.trim($("#InquiryReferenceText").val()) == '' || $("#InquiryReferenceText").val() == undefined || $("#InquiryReferenceText").val() == null) {
        count++;
        $("#spnReferenceText").show();
    }
    else {
        $("#spnReferenceText").hide();
    }

    if ($("#DeliveryFrequency option:selected").val() == "-1") {
        count++;
        $("#spnDeliveryFrequency").show();
    }
    else {
        $("#spnDeliveryFrequency").hide();
    }
    if ($("#DeliveryChannelUserPreferenceName option:selected").val() == "") {
        count++;
        $("#spnDeliveryChannelUserPreferenceName").show();
    }
    else {
        $("#spnDeliveryChannelUserPreferenceName").hide();
    }
    if (count > 0)
        return false;
    else
        return true;
});
$('body').on('click', 'table.NotificationProfileTb tbody tr', function () {
    if (!$(this).hasClass("current")) {
        if ($(this).html().indexOf("No record(s) Found") > 0) {
            return false;
        }
        $(".NotificationProfileTb tbody tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var id = $(this).attr("data-NotificationProfileID");
        LoadAddUpdateNotificationProfile(id);

    }
});

//End Notification Profile

//====================================================================================================================

//Start DUNS Registration

function TagsMonitoringRegistration() {
    if ($("#frmMonitoringRegistration #TagList").length > 0) {
        var TagList = $("#frmMonitoringRegistration #TagList").val().split(',');
        if (TagList != null || TagList != "") {
            $(".TagsValueMonitoringRegistration option").each(function () {
                for (var i = 0; i < TagList.length; i++) {
                    if ($(this).val() == TagList[i]) {
                        $(this).attr("selected", "selected");
                    }
                }
            });
            $("#frmMonitoringRegistration #Tags").val(TagList);
        }
    }
    if ($(".TagsValueMonitoringRegistration").length > 0) {
        $(".TagsValueMonitoringRegistration").chosen().change(function (event) {
            if (event.target == this) {
                $("#frmMonitoringRegistration #Tags").val($(this).val());
            }
        });
    }
}
function onloadDUNSregistration() {
    var url = "/DNBMonitoring/IndexMonitoringRegistration";
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json',
        contentType: 'application/json',
        cache: false,
        success: function (data) {
            $("#divPartialDUNSregistrationData").html(data.indexMonitoringRegistrations);
            $("#divPartialAddUpdateDUNSRegistration").html(data.upsertMonitoringRegistrations);
            TagsMonitoringRegistration();
            if ($("table.MonitoringRegistrationTb tbody tr:first").html().indexOf("No record(s) Found") < 0) {
                $("table.MonitoringRegistrationTb tbody tr:first").addClass("current");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function LoadUpsertDUNSregistration(id) {
    if (id == undefined || id == null || id == "") {
        id = 0;
    }
    var url = '/DNBMonitoring/AddUpdateDUNSregistration?Parameters=' + ConvertEncrypte(encodeURI(id)).split("+").join("***");
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $("#divPartialAddUpdateDUNSRegistration").html(data);
            if ($("table.MonitoringRegistrationTb tbody tr:first").html().indexOf("No record(s) Found") > 0) {
                $("#editDUNSRegistration").attr("disabled", true);
            }
            TagsMonitoringRegistration();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
$('body').on('click', '.DeleteMonitoringRegistration', function () {
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecord, callback: function (result) {
            if (result) {
                var token = $('input[name="__RequestVerificationToken"]').val();
                var QueryString = $(this).attr("id");
                $.ajax({
                    type: "POST",
                    url: "/DNBMonitoring/DeleteMonitoringRegistration?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                    headers: { "__RequestVerificationToken": token },
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.message);
                        if (data.result) {
                            onloadDUNSregistration();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    $('#divProgress').hide();
    return false;
});
$("body").on('click', 'table.MonitoringRegistrationTb tbody tr', function () {
    if (!$(this).hasClass("current")) {
        if ($(this).html().indexOf("No record(s) Found") > 0) {
            return false;
        }
        $(".MonitoringRegistrationTb tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var id = $(this).attr("data-MonitoringRegistrationId");
        LoadUpsertDUNSregistration(id);
    }
});
$("body").on('click', '#btnMonitorRegistration', function () {
    var count = 0;
    if ($("#MonitoringProfileId").val() == "-1" || $("#MonitoringProfileId").val() == undefined) {
        count++;
        $("#spnMonitoringProfileId").show();
    }
    else {
        $("#spnMonitoringProfileId").hide();
    }
    if ($("#NotificationProfileId").val() == "-1" || $("#NotificationProfileId").val() == undefined) {
        count++;
        $("#spnNotificationProfileId").show();
    }
    else {
        $("#spnNotificationProfileId").hide();
    }
    if (count > 0)
        return false;
    else
        return true;
});
$('body').on('click', '#editDUNSRegistration', function () {
    enableMonitoringRegistrationFields();
});
$('body').on('click', '#AddDUNSRegistration', function () {
    $(".MonitoringRegistrationTb tr").each(function () {
        $(this).removeClass('current');
    });
    enableMonitoringRegistrationFields();
    $("#MonitoringProfileId").val($("#MonitoringProfileId option:first").val());
    $("#NotificationProfileId").val($("#NotificationProfileId option:first").val());
    $("#TradeUpIndicator").prop("checked", false);
    $("#AutoRenewalIndicator").prop("checked", false);
    $("#SubjectCategory").val($("#SubjectCategory option:first").val());
    $(".CustomerReferenceTextNotification").val("");
    $("#BillingEndorsementText").val("");
    $("#MonitoringRegistrationId").val("0");
    $("#btnMonitorRegistration").show();
    $(".TagsValueMonitoringRegistration").val('0');
    $(".TagsValueMonitoringRegistration").trigger("chosen:updated");
    $("#btnMonitorRegistration").val(createDUNSRegistration);
    $("#frmMonitoringRegistration #Tags").val("");

});
function enableMonitoringRegistrationFields() {
    $("#MonitoringProfileId").prop("disabled", false);
    $("#NotificationProfileId").prop("disabled", false);
    $("#TradeUpIndicator").prop("disabled", false);
    $("#AutoRenewalIndicator").prop("disabled", false);
    $("#SubjectCategory").prop("disabled", false);
    $(".CustomerReferenceTextNotification").prop("disabled", false);
    $("#BillingEndorsementText").prop("disabled", false);
    $(".TagsValueMonitoringRegistration").prop("disabled", false);
    $(".TagsValueMonitoringRegistration").trigger("chosen:updated");
    $("#btnMonitorRegistration").show();
    $("#frmMonitoringRegistration .OpenTags").show();
}
function UpdateDUNSRegistration(data) {
    ShowMessageNotification("success", data.message);
    if (data.result) {
        onloadDUNSregistration();
    }
}

//End DUNS Registration

//====================================================================================================================