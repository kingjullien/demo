// Display and Hide loader bar for every ajax call
$(document).ajaxStart(function () { $('#divProgress').show(); }).ajaxStop(function () { $('#divProgress').hide(); });

$(document).on('click', '#btnCancel', function () {
    $("#InvestigateModal").modal("hide");
    $("#iResearchInvestigationRecordsTargetedModal").modal("hide");
});

$(document).on('click', '.clsGetStatus', function () {
    var ResearchRequestId = $(this).attr("data-ResearchRequestId");
    $.ajax({
        type: "POST",
        url: "/ResearchInvestigation/GetInvestigationStatus?Parameters=" + ConvertEncrypte(encodeURI(ResearchRequestId)).split("+").join("***"),
        dataType: "JSON",
        contentType: "application/json",
        cache: false,
        success: function (data) {
            if (data.result) {
                LoadResearchInvestigation();
            }
            else {
                ShowMessageNotification("success", data.message, false);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});


function LoadResearchInvestigation() {
    location.href = '/ResearchInvestigation/Index';
}

function FilterResearchInvestigation(SrcRecordId, Status, RequestStartDateTime, RequestendDateTime, KeyWord) {
    var QueryString = "SrcRecordId:" + SrcRecordId + "@#$Status:" + Status + "@#$RequestStartDateTime:" + RequestStartDateTime + "@#$RequestendDateTime:" + RequestendDateTime + "@#$KeyWord:" + KeyWord;
    $.ajax({
        type: 'GET',
        url: '/ResearchInvestigation/FilterIResearchInvestigation?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#divPartialInvestigation").html(data);
            InitiResearchInvestigationDataTable();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$(document).on('change', '#SubTypes', function () {
    if ($(this).val() != null && $(this).val().indexOf("33560") > -1) {
        parent.addHeight600Class(true);
        $(".artDuplicateDuns").show();
        $(".artDuplicateDuns .DUPDUNS").each(function () {
            $(this).attr("disabled", false);
        });
    }
    else {
        parent.addHeight600Class(false);
        $(".artDuplicateDuns").hide();
        $(".artDuplicateDuns .DUPDUNS").each(function () {
            $(this).attr("disabled", true);
        });
    }

    if ($(this).val() != null && $(this).val().indexOf("33769") > -1) {
        $(".divReSearchComment1").show();
    }
    else {
        $(".divReSearchComment1").hide();
    }

    if ($(this).val() != null && $(this).val().indexOf("33770") > -1) {
        $(".divReSearchComment2").show();
    }
    else {
        $(".divReSearchComment2").hide();
    }
});

$(document).on('click', '#btnFilterResearchInvestigation', function () {
    var SrcRecordId = $("#SrcRecordId").val();
    var Status = $("#Status").val();
    var DateRange = $("#RequestStartDateTime").val();
    var KeyWord = $("#Keyword").val();
    var RequestStartDateTime = "";
    var RequestendDateTime = "";
    if (DateRange != undefined && DateRange != "") {
        var DateRangelst = DateRange.split("-");
        RequestStartDateTime = DateRangelst[0];
        RequestendDateTime = DateRangelst[1];
    }
    FilterResearchInvestigation(SrcRecordId, Status, RequestStartDateTime, RequestendDateTime, KeyWord);
});

$(document).on('click', '#InvestigateModal #btnMiniinvestigationSave', function () {
    var cnt = 0;
    var businessName = $('#InvestigateModal #BusinessName').val();
    var street = $('#InvestigateModal #StreetAddress').val();
    var city = $('#InvestigateModal #AddressLocality').val();
    var postal = $('#InvestigateModal #PostalCode').val();
    var country = $('#InvestigateModal #CountryISOAlpha2Code').val();
    var reComment = $('#InvestigateModal #ResearchComments').val();
    var reEmail = $('#InvestigateModal #CustomerRequestorEmail').val();
    if (!businessName) {
        $('#InvestigateModal #spnBusinessName').show();
        cnt++;
    }
    else {
        $('#InvestigateModal #spnBusinessName').hide();
    }

    if (!street) {
        $('#InvestigateModal #spnStreet').show();
        cnt++;
    }
    else {
        $('#InvestigateModal #spnStreet').hide();
    }

    if (!city) {
        $('#InvestigateModal #spnCity').show();
        cnt++;
    }
    else {
        $('#InvestigateModal #spnCity').hide();
    }

    if (!postal) {
        $('#InvestigateModal #spnPostal').show();
        cnt++;
    }
    else {
        $('#InvestigateModal #spnPostal').hide();
    }

    if (!country) {
        $('#InvestigateModal #spnCountry').show();
        cnt++;
    }
    else {
        $('#InvestigateModal #spnCountry').hide();
    }

    if (!country) {
        $('#InvestigateModal #spnCountry').show();
        cnt++;
    }
    else {
        $('#InvestigateModal #spnCountry').hide();
    }

    if (!reComment) {
        $('#InvestigateModal #spnReComments').show();
        cnt++;
    }
    else {
        $('#InvestigateModal #spnReComments').hide();
    }

    if (!reEmail && !isValidEmailAddress(reEmail)) {
        $('#InvestigateModal #spnReEmail').show();
        cnt++;
    }
    else {
        $('#InvestigateModal #spnReEmail').hide();
    }

    if (cnt > 0)
        return false;
    else
        return true;

});

$(document).on('click', '#iResearchInvestigationRecordsTargetedModal #btnSubmitInvestigation', function () {
    var ResearchSubTypes = $.map($("#SubTypes option:selected"), function (e, i) {
        return $(e).text();
    });
    var Duns = $("#iResearchInvestigationRecordsTargetedModal #Duns").val();
    var ResearchComments1 = $("#iResearchInvestigationRecordsTargetedModal #ResearchComments1").val();
    var ResearchComments2 = $("#iResearchInvestigationRecordsTargetedModal #ResearchComments2").val();
    ResearchSubTypes = $("#iResearchInvestigationRecordsTargetedModal #SubTypes").val();
    var DDUNS = [];
    if (ResearchSubTypes.length > 0 && (ResearchSubTypes.indexOf("33560") > -1)) {
        $(".artDuplicateDuns .DUPDUNS").each(function () {
            var currVal = $(this).val();
            if (currVal && currVal != undefined) {
                DDUNS.push(currVal);
            }
        })
    }

    if (DDUNS.length > 0)
        $("#iResearchInvestigationRecordsTargetedModal #DuplicateDuns").val(DDUNS.toString());

    var cnt = 0;
    if (Duns == "") {
        $("#iResearchInvestigationRecordsTargetedModal #spnDuns").show();
        cnt++;
    }
    else {
        $("#iResearchInvestigationRecordsTargetedModal #spnDuns").hide();
    }

    if (ResearchSubTypes.length < 1) {
        $("#iResearchInvestigationRecordsTargetedModal #spnResearchSubTypes").show();
        cnt++;
    }
    else {
        $("#iResearchInvestigationRecordsTargetedModal #ResearchSubTypes").val(ResearchSubTypes);
        $("#iResearchInvestigationRecordsTargetedModal #spnResearchSubTypes").hide();
    }

    if (ResearchSubTypes.length > 0 && ResearchSubTypes.indexOf("33769") > -1) {
        if (ResearchComments1 == "") {
            $("#iResearchInvestigationRecordsTargetedModal #spnResearchComments1").show();
            cnt++;
        }
        else {
            $("#iResearchInvestigationRecordsTargetedModal #spnResearchComments1").hide();
        }
    } else {
        $("#iResearchInvestigationRecordsTargetedModal #spnResearchComments1").hide();
    }

    if (ResearchSubTypes.length > 0 && ResearchSubTypes.indexOf("33770") > -1) {
        if (ResearchComments2 == "") {
            $("#iResearchInvestigationRecordsTargetedModal #spnResearchComments2").show();
            cnt++;
        }
        else {
            $("#iResearchInvestigationRecordsTargetedModal #spnResearchComments2").hide();
        }
    } else {
        $("#iResearchInvestigationRecordsTargetedModal #spnResearchComments2").hide();
    }


    if (cnt > 0) {
        return false;
    }
});

$(document).on("keypress", ".OnlyDigit", function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;
});

function OnSuccessResearchInvestigationTargeted(data) {
    if (data.result) {
        $('#iResearchInvestigationRecordsTargetedModal').modal('hide');
        parent.ShowMessageNotification("success", data.message, false);
    }
    else {
        $(".targetederrMsg").text(data.message);
    }
}

function OnSuccessResearchInvestigation(data) {
    if (data.result) {
        var inpId = $("#InvestigateModalMain #InputId").val();
        $('#InvestigateModal').modal('hide');
        parent.ShowMessageNotification("success", data.message, false);
        if ($("#divStewardshipPortal tr#" + inpId).length > 0) {
            RefreshMatchDataGrid();
            //if ($("#divStewardshipPortal tbody tr.trMatchedItemView").length == 1) {
            //    $("#divStewardshipPortal tr#" + inpId).next().remove();
            //    $("#divStewardshipPortal tr#" + inpId).html('<td colspan="12">' + noRecordsAvailable + '</td>');
            //}
            //else {
            //    $("#divStewardshipPortal tr#" + inpId).next().remove();
            //    $("#divStewardshipPortal tr#" + inpId).remove();
            //}
        }
        else if ($("#dgBanInputData tr#" + inpId).length > 0) {
            $("#SearchPopupModal").modal('hide');
            ReloadWithCurrentPage();
            //if ($("#dgBanInputData tbody tr").length == 1) {
            //    $("#dgBanInputData tr#" + inpId).html('<td colspan="13">' + noRecordsAvailable + '</td>');
            //}
            //else {
            //    $("#dgBanInputData tr#" + inpId).remove();
            //}
        }
    }
    else {
        $(".minierrMsg").text(data.message);
    }
}

$(document).on('click', '.tblMiniReqMultiple .Edit', function () {
    var row = $(this).closest("tr");
    $("td", row).each(function () {
        if ($(this).find("input").length > 0) {
            $(this).find("input").show();
            $(this).find("span").hide();
        }
    });
    row.find(".Update").show();
    row.find(".Cancel").show();
    $(this).hide();
});

$(document).on('click', '.tblMiniReqMultiple .Cancel', function () {
    var row = $(this).closest("tr");
    $("td", row).each(function () {
        if ($(this).find("input").length > 0) {
            var span = $(this).find("span");
            var input = $(this).find("input");
            input.val(span.html());
            span.show();
            input.hide();
        }
    });
    row.find(".Edit").show();
    row.find(".Update").hide();
    $(this).hide();
});

$(document).on("click", ".tblMiniReqMultiple .Update", function () {
    var cnt = 0;
    var row = $(this).closest("tr");
    $("td", row).each(function () {
        if ($(this).find("input").length > 0) {
            var span = $(this).find("span");
            var input = $(this).find("input");
            span.html(input.val());
            span.show();
            input.hide();
            if (!input.hasClass("AddressRegion")) {
                if (input.val() != '' && input.val() != undefined) {
                    $(this).removeClass("cellError");
                }
                else {
                    if (!$(this).hasClass("cellError")) {
                        $(this).addClass("cellError");
                    }
                }
            }
            var rowData = JSON.parse(row.attr("data-val"));
            rowData[input.attr("class")] = input.val();
            row.attr("data-val", JSON.stringify(rowData));
        }
    });

    row.find(".Edit").show();
    row.find(".Cancel").hide();
    $(this).hide();
});

$(document).on("click", "#btnSubmitMultiMiniRequest", function () {
    var cnt = 0;
    var cmnt = $("#rComments").val();
    var Email = $("#reqEmail").val();
    if (cmnt == "" || cmnt == undefined || cmnt == null) {
        cnt++;
        $(".errResearchComments").show();
    }
    else {
        $(".errResearchComments").hide();
    }

    if (Email == "" || Email == undefined || Email == null) {
        cnt++;
        $(".errCustEmail").text(requiredRequestorEmail);
        $(".errCustEmail").show();
    }
    else if (!isValidEmailAddress(Email)) {
        cnt++;
        $(".errCustEmail").text(enterValidEmail);
        $(".errCustEmail").show();
    }
    else {
        $(".errCustEmail").hide();
    }
    if (cnt > 0) {
        return false;
    }
    else {
        var allRecords = [];
        $(".tblMiniReqMultiple tbody tr").each(function () {
            var rowData = JSON.parse($(this).attr("data-val"));
            rowData["ResearchComments"] = cmnt;
            rowData["CustomerRequestorEmail"] = Email;
            allRecords.push(rowData);
        });

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: '/ResearchInvestigation/SubmitMultipleMiniRequests',
            data: JSON.stringify(allRecords),
            dataType: "json",
            cache: false,
            success: function (data) {
                $('#MultiMiniModal').modal('hide');
                ShowMessageNotification("error", data.message, false);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
});

$(document).on("click", "#btnGetAllReSearchStatus", function () {
    $.ajax({
        type: "POST",
        url: "/ResearchInvestigation/GetInvestigationStatusForAll",
        dataType: "JSON",
        contentType: "application/json",
        cache: false,
        success: function (data) {
            if (data.result) {
                LoadResearchInvestigation();
            }
            else {
                ShowMessageNotification("success", data.message, false);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
function isValidResearchComments(emailAddress) {
    var pattern = /^(?!\s*$)[-a-zA-Z0-9_:,.' ']{1,100}$/i;
    return pattern.test(emailAddress);
};
function InitiResearchInvestigationDataTable() {
    InitDataTable(".iResearchInvsetigationTB", [10, 20, 30], false, [5, "desc"]);
}

$(document).on('click', '#btnInvestigationFileUpload', function () {
    $.ajax({
        type: "GET",
        url: "/ResearchInvestigation/FileUploadIndex",
        cache: false,
        success: function (data) {
            $("#InvestigationFileUploadModalMain").html(data);
            DraggableModalPopup("#InvestigationFileUploadModal");
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$(document).on('click', '#btnInvestiogationFilesubmit', function () {
    var formData = new FormData();
    if ($('#InvestigationFile')[0].files[0] != undefined) {
        $(".fileErrorMsg").text("");
        formData.append('file', $('#InvestigationFile')[0].files[0]);
        formData.append('Type', $("#Type").val());
        $.ajax({
            type: "POST",
            url: '/ResearchInvestigation/FileUploadIndex',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response == unableImportBlankFile) {
                    $(".generalMsg").text(response);
                    return false;
                }
                if (response == onlyExcelAllowed) {
                    $(".generalMsg").text(response);
                    return false;
                }
                if (response.indexOf(error) > -1) {
                    $(".generalMsg").text(response);
                    return false;
                }
                if (response.indexOf(alreadyBelongsToFile) > -1) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    $(".generalMsg").text(response);
                    return false;
                }
                $("#InvestigationFileUploadModal").modal('hide');
                $("#InvestigationColumnMappingModalMain").html(response);
                DraggableModalPopup("#InvestigationColumnMappingModal");
            },
            error: function (xhr, status, error) {
            }
        });
    }
    else {
        $(".fileErrorMsg").text(selectFile);
    }
});

$(document).on("change", "#InvestigationFile", function () {
    if ($('#file').val() != "") {
        var formats = "xls,xlsx";
        var fileExtension = formats.split(','); //['xls', 'xlsx', 'csv'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            $(".fileErrorMsg").text(onlyFormatsAllowed + " " + fileExtension.join(', '));
            $('#InvestigationFile').val("");
        }
        else {
            $(".fileErrorMsg").text("");
        }
    }
});

$(document).on('change', 'input[type=radio][name=rdType]', function () {
    $("#Type").val($("input[name='rdType']:checked").val());
    if ($("input[name='rdType']:checked").val() == "Targeted") {
        $(".lnksubtypeinfo").show();
    }
    else {
        $(".lnksubtypeinfo").hide();
    }
});

$(document).on('click', '.investigation_Details', function () {
    var rowData = JSON.parse($(this).attr("data-val"));
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: '/ResearchInvestigation/InvestigationDetails',
        data: JSON.stringify(rowData),
        cache: false,
        dataType: 'HTML',
        success: function (data) {
            $('#InvestigationDetailsModalMain').html(data);
            DraggableModalPopup('#InvestigationDetailsModal');
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
})

$(document).on('click', '#btnExportToCSV', function () {
    $("#frmExportToCSV").submit();
});

$(document).on('click', '.investigation_Challenge', function () {
    var rowData = JSON.parse($(this).attr("data-val"));
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: '/ResearchInvestigation/ChalleangeInvestigation',
        data: JSON.stringify(rowData),
        cache: false,
        dataType: 'HTML',
        success: function (data) {
            $('#InvestigationChallengeModalMain').html(data);
            DraggableModalPopup('#InvestigationChallengeModal');
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
})

$(document).on('click', '#btnChallengeInvestigationSubmit', function () {
    var comment = $("#form_ChallengeInvestigation #comment").val();
    if (comment) {
        $("#form_ChallengeInvestigation #spnReComments").hide();
    }
    else {
        $("#form_ChallengeInvestigation #spnReComments").show();
        return false;
    }
});


function OnSuccessChallengeInvestigation(data) {
    if (data.result) {
        $('.challengeError').text('');
        LoadResearchInvestigation();
        ShowMessageNotification("success", data.message, false);
    }
    else {
        $('.challengeError').text(data.message);
    }
}
