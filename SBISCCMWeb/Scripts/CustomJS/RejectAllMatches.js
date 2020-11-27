$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
$("body").on('blur', '.CityName', function () {
    if ($(this).val() != "" && $(this).val() != undefined) {
        $("#chkCityExactMatch").prop("disabled", false);
    } else {
        $("#chkCityExactMatch").prop("disabled", "disabled");
        $("#chkCityExactMatch").prop("checked", false);
    }
});
$("body").on('blur', '.StateName', function () {
    if ($(this).val() != "" && $(this).val() != undefined) {
        $("#chkStateExactMatch").prop("disabled", false);
    } else {
        $("#chkStateExactMatch").prop("disabled", "disabled");
        $("#chkStateExactMatch").prop("checked", false);
    }
});
$("body").on('change', '#chkRejectAll', function () {
    if ($(this).prop('checked') == true) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});
$("body").on('change', '#chkCityExactMatch', function () {
    if ($(this).prop('checked') == true) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});
$("body").on('change', '#chkStateExactMatch', function () {
    if ($(this).prop('checked') == true) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});
$("body").on('click', '#btnRejectAll', function () {
    var SrcRecordId = $(".divPurgeRecords #SrcRecordId").val();
    var CompanyName = $(".divPurgeRecords #CompanyName").val();
    var City = $(".divPurgeRecords #City").val();
    var State = $(".divPurgeRecords #State").val();
    var ImportPorcess = $(".divPurgeRecords #ImportPorcess").val();
    var ConfidenceCode = $(".divPurgeRecords #ConfidenceCode").val();
    var CountryISOAlpha2Code = $(".divPurgeRecords #CountryISOAlpha2Code").val();
    var CountryGroupId = $(".divPurgeRecords #CountryGroupId").val();
    var Tag = $(".divPurgeRecords #Tag").val();
    if (Tag == undefined) {
        Tag = "";
    }

    var CityExactMatch = $(".divPurgeRecords #chkCityExactMatch").prop('checked');
    var StateExactMatch = $(".divPurgeRecords #chkStateExactMatch").prop('checked');
    var GetCountsOnly = $("#GetCountsOnly").val();
    var Purge = $("#chkRejectAll").prop('checked');
    var IsMatchData = $("#IsMatchData").val();
    if (Purge == undefined) {
        Purge = false;
    }
    if (CompanyName == undefined) {
        CompanyName = "";
    }
    if (ConfidenceCode == undefined) {
        ConfidenceCode = "0";
    }
    var url = '/StewardshipPortal/RejectAllRecords' + "?SrcRecordId=" + encodeURIComponent(SrcRecordId) + "&CompanyName=" + CompanyName + "&City=" + encodeURIComponent(City) + "&State=" + encodeURIComponent(State) + "&ImportPorcess=" + encodeURIComponent(ImportPorcess) + "&ConfidenceCode=" + ConfidenceCode + "&CountryISOAlpha2Code=" + CountryISOAlpha2Code + "&CountryGroupId=" + CountryGroupId + "&Tag=" + encodeURIComponent(Tag) + "&CityExactMatch=" + CityExactMatch + "&StateExactMatch=" + StateExactMatch + "&GetCountsOnly=" + GetCountsOnly + "&Purge=" + Purge + "&IsMatchData=" + IsMatchData;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        async: false,
        cache: false,
        success: function (data) {

            if (data.result) {
                url = '/StewardshipPortal/RejectAllRecords' + "?SrcRecordId=" + encodeURIComponent(SrcRecordId) + "&CompanyName=" + CompanyName + "&City=" + encodeURIComponent(City) + "&State=" + encodeURIComponent(State) + "&ImportPorcess=" + encodeURIComponent(ImportPorcess) + "&ConfidenceCode=" + ConfidenceCode + "&CountryISOAlpha2Code=" + CountryISOAlpha2Code + "&CountryGroupId=" + CountryGroupId + "&Tag=" + encodeURIComponent(Tag) + "&CityExactMatch=" + CityExactMatch + "&StateExactMatch=" + StateExactMatch + "&GetCountsOnly=false&Purge=" + Purge + "&IsMatchData=" + IsMatchData;
                bootbox.confirm({
                    title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: data.message, callback: function (result) {
                        if (result) {
                            $("#GetCountsOnly").val("false");
                            $.ajax({
                                type: "POST",
                                url: url,
                                dataType: "json",
                                contentType: "application/json; charset=UTF-8",
                                async: false,
                                cache: false,
                                success: function (data) {
                                    ShowMessageNotification("success", data.message, false);
                                    if (data.result) {
                                        $("#divPurgeDataModal").modal('hide');
                                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
                                        CloseRejectAllWindow();
                                    }
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                }
                            });
                        }
                    }
                });
                setTimeout(function () {
                    $(".bootbox-close-button").show();
                    $(".bootbox-close-button").addClass("closeBootbox");
                    $(".bootbox-close-button").removeClass("bootbox-close-button");
                }, 100);
            }
            else {
                ShowMessageNotification("success", data.message, false);
                setTimeout(function () {
                    $(".bootbox-close-button").show();
                    $(".bootbox-close-button").addClass("closeBootbox");
                    $(".bootbox-close-button").removeClass("bootbox-close-button");
                }, 100);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$(document).on('change', '#RecordFileToggle', function () {
    var currVal = $(this).prop('checked');
    if (currVal) {
        $("#divPurgeDataModal .spnUsingRecords").removeClass('thColor');
        $("#divPurgeDataModal .spnFromFile").addClass('thColor');
        $("#divPurgeDataModal #popupConfigData").hide();
        $("#divPurgeDataModal #divPurgeFromFile").show();
    }
    else {
        $("#divPurgeDataModal .spnUsingRecords").addClass('thColor');
        $("#divPurgeDataModal .spnFromFile").removeClass('thColor');
        $("#divPurgeDataModal #popupConfigData").show();
        $("#divPurgeDataModal #divPurgeFromFile").hide();
    }
});