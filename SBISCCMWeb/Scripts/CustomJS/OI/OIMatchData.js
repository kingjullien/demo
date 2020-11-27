$(document).on("change", ".pagevalueChange", function () {
    var IncludeWithCandidates = $(".FilterIncludeWithCandidates").is(":checked");
    var IncludeWithoutCandidates = $(".FilterIncludeWithoutCandidates").is(":checked");

    var OIMatchsortPagevalue = $(this).val();
    var OIMatchSortby = $("#OIMatchSortby").val();
    var OIMatchsortorder = $("#OIMatchsortorder").val();
    var url = "/OI/MatchData?OIMatchPage=&OIMatchSortby=" + OIMatchSortby + "&OIMatchsortorder=" + OIMatchsortorder + "&OIMatchsortPagevalue=" + OIMatchsortPagevalue + "&IncludeWithoutCandidates=" + IncludeWithoutCandidates + "&IncludeWithCandidates=" + IncludeWithCandidates;
    LoadMatchData(url);
});

$(document).on("change", ".FilterMatchesAllCandidate", function () {
    var isChecked = $(this).is(":checked");
    var IncludeWithCandidates = isChecked;
    var IncludeWithoutCandidates = isChecked;
    var OIMatchsortPagevalue = $(".pagevalueChange").val();
    var OIMatchSortby = $("#OIMatchSortby").val();
    var OIMatchsortorder = $("#OIMatchsortorder").val();
    var url = "/OI/MatchData?OIMatchPage=&OIMatchSortby=" + OIMatchSortby + "&OIMatchsortorder=" + OIMatchsortorder + "&OIMatchsortPagevalue=" + OIMatchsortPagevalue + "&IncludeWithoutCandidates=" + IncludeWithoutCandidates + "&IncludeWithCandidates=" + IncludeWithCandidates;
    LoadMatchData(url);

});
$(document).on("change", ".FilterIncludeWithCandidates", function () {
    var IncludeWithCandidates = $(".FilterIncludeWithCandidates").is(":checked");
    var IncludeWithoutCandidates = $(".FilterIncludeWithoutCandidates").is(":checked");
    var OIMatchsortPagevalue = $(".pagevalueChange").val();
    var OIMatchSortby = $("#OIMatchSortby").val();
    var OIMatchsortorder = $("#OIMatchsortorder").val();
    var url = "/OI/MatchData?OIMatchPage=&OIMatchSortby=" + OIMatchSortby + "&OIMatchsortorder=" + OIMatchsortorder + "&OIMatchsortPagevalue=" + OIMatchsortPagevalue + "&IncludeWithoutCandidates=" + IncludeWithoutCandidates + "&IncludeWithCandidates=" + IncludeWithCandidates;
    LoadMatchData(url);
});
$(document).on("change", ".FilterIncludeWithoutCandidates", function () {
    var IncludeWithCandidates = $(".FilterIncludeWithCandidates").is(":checked");
    var IncludeWithoutCandidates = $(".FilterIncludeWithoutCandidates").is(":checked");
    var OIMatchsortPagevalue = $(".pagevalueChange").val();
    var OIMatchSortby = $("#OIMatchSortby").val();
    var OIMatchsortorder = $("#OIMatchsortorder").val();
    var url = "/OI/MatchData?OIMatchPage=&OIMatchSortby=" + OIMatchSortby + "&OIMatchsortorder=" + OIMatchsortorder + "&OIMatchsortPagevalue=" + OIMatchsortPagevalue + "&IncludeWithoutCandidates=" + IncludeWithoutCandidates + "&IncludeWithCandidates=" + IncludeWithCandidates;
    LoadMatchData(url);
});

// On click event on clicking AcceptFromFile
$('body').on('click', '#btnAcceptFromFile', function () {

    $.ajax({
        type: 'GET',
        url: '/OIMatchData/AcceptFromFile',
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIMatchDataAcceptFromFileModalMain").html(data);
            DraggableModalPopup("#OIMatchDataAcceptFromFileModal");
            LoadOIMatchAcceptFromFile();
        }
    });
});

function LoadMatchData(url) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: url,
        dataType: 'HTML',
        contentType: 'application/html',
        async: false,
        success: function (data) {
            $("#partialDivOICompanyMatchdata").html(data);
        }
    });
}
// Open Session Filter
$('body').on('click', '.OIuserFilter', function () {
    $.ajax({
        type: 'GET',
        url: '/OIUserSessionFilter/popupUserFilter/',
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIMatchSessionFilterModalMain").html(data);
            DraggableModalPopup("#OIMatchSessionFilterModal");
        }
    });
});
// Delete Session Filter
$('body').on('click', '.DeleteFilter', function () {

    var pagevalue = $("#pagevalue").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    $(this).find("strong").removeClass("fa fa-plus");
    $(this).find("strong").removeClass("fa fa-minus");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteSessionFilter, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/OIMatchData/DeleteSessionFilter/",
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    success: function (data) {
                        if (data == success) {
                            $("#OIMatchSessionFilterModal").modal("hide");
                            location.reload();
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
 
$('body').on('click', '.openOIMatchdetailPopup', function () {
    var Inputlid = $(this).attr("data-inputId");
    $.ajax({
        type: 'GET',
        url: '/OIMatchData/MatchDataDetail/?Parameters=' + ConvertEncrypte(encodeURI(Inputlid)).split("+").join("***"),
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIMatchDataDetailModalMain").html(data);
            DraggableModalPopup("#OIMatchDataDetailModal");
            onLoadOIMatchSearchData();
        }
    });
});
function CallbackMatchAssign(data) {
    $("#OIMatchDataDetailModal").modal("hide");
    bootbox.alert({
        title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
        message: data, callback: function () {
            window.location.reload();
        }
    });
}

function ResertUndoMatchId() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/OIMatchData/ResertUndoMatchId/",
        async: false,
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function callbackRejectPurgeData() {
    location.reload();
}
  
// On click event on clicking DeleteDataFromFiles
$('body').on('click', '#btnDeleteDataFromFiles', function () {
    $.ajax({
        type: 'GET',
        url: '/OIMatchData/DeleteFromFile',
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIMatchDeleteFromFileModalMain").html(data);
            DraggableModalPopup("#OIMatchDeleteFromFileModal");
        }
    });
});

// On click event on clicking ExportToExcel
$('body').on('click', '#btnExportToExcel', function () {

    $.ajax({
        type: 'GET',
        url: '/OIMatchData/ExportRecordToExcel',
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIMatchExportRecordToExcelModalMain").html(data);
            DraggableModalPopup("#OIMatchExportRecordToExcelModal");
            loadMatchExportToExcel();
        }
    });
});

$(function () {
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.contextMenu({
        selector: '.context-menu-one-delete',
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
                name: lblDeleteCompanyData, titile: "Purge", callback: function () {
                    //deletes a single record from the UI (right - click UI option)
                    var SrcRecordId = $(this).attr("data-SrcRecordId");
                    var InputId = $(this).attr("id");
                    var City = $(this).attr("data-City");
                    var State = $(this).attr("data-State");
                    var QueryString = "InputId:" + InputId + "@#$SrcRecordId:" + SrcRecordId + "@#$City:" + City + "@#$State:" + State
                    var url = '/OIMatchData/DeleteCompanyRecord?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                    bootbox.confirm({
                        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteCompanyData, callback: function (result) {
                            if (result) {
                                $.ajax({
                                    type: "POST",
                                    url: url,
                                    dataType: "json",
                                    contentType: "application/json; charset=UTF-8",
                                    async: false,
                                    cache: false,
                                    success: function (data) {
                                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                                            ShowMessageNotification("error", data, false, true, LocationReload);
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
            "Export": {
                name: exporttoExcel, titile: "Export", callback: function () {
                    //exports to excel from the UI (right - click UI option)
                    var InputId = $(this).attr("id");

                    $('<form action="/OIMatchData/ExportToExcel"><input type="hidden" name="InputId" value="' + InputId + '"> </form>').appendTo('body').submit();

                    return 'context-menu-icon context-menu-icon-quit';
                }
            }
        }
    });

    $('.context-menu-one-purge').on('click', function (e) {
        return 'context-menu-icon context-menu-icon-quit';
    });
});



// On click event on clicking Delete Data from Addtional Action 
$('body').on('click', '#btnDeleteData', function () {
    $.ajax({
        type: 'GET',
        url: '/OIMatchData/OIDeleteCompanyData',
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIMatchDeleteCompanyDataModalMain").html(data);
            DraggableModalPopup("#OIMatchDeleteCompanyDataModal");
        }
    });
});
$('body').on('click', '#btnRefreshStewardshipQueue', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/OIMatchData/RefreshStewardshipQueue/",
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data == success) {
                location.reload();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

function loadMatchExportToExcel() {
    $("select.chzn-select").chosen({
        no_results_text: nothingFound,
        width: "100%",
        search_contains: true
    });
}