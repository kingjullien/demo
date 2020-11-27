$("body").on("click", "#form_CachedDataSettings #btnSubmitCacheData", function () {
    var data = $("#form_CachedDataSettings").serialize();
    var clientCode = $(this).attr("data-ClientCode");
    updateText(clientCode);
    $.post("/DNBFeature/CachedDataSettings/", data).done(function (data) {
    });
});
function updateText(clientCode) {
    var ClientCode = clientCode;
    var GetMissingDataFromProvider = $('#btnMissing').prop('checked');
    var QueryString = "ClientCode:" + ClientCode + "@#$GetMissingDataFromProvider:" + GetMissingDataFromProvider;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: 'POST',
        url: "/DNBFeature/UpdateMissingDataProvider?Parameters=" + Parameters,
        dataType: 'JSON',
        cache: false,
        contentType: 'application/json;',
        success: function (data) {
            ShowMessageNotification("success", data.message);
            $("#CachedDataModal").modal("hide");
        }
    });
}
$("body").on('click', '#btnDownloadData', function () {
    // Changes for Converting magnific popup to modal popup
    var clientCode = $(this).attr("data-ClientCode");
    $.ajax({
        type: 'GET',
        url: "/DNBFeature/CacheDataPopup",
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html',
        data: { Parameters: ConvertEncrypte(encodeURI(clientCode)).split("+").join("***") },
        success: function (data) {
            $("#divDownloadData").html(data);
            DraggableModalPopup("#myModalDownloadCache");
        }
    });
});

$("body").on('click', '#btnDeleteCleansematchdata', function () {
    // Changes for Converting magnific popup to modal popup
    var clientCode = $(this).attr("data-ClientCode");
    $.ajax({
        type: 'GET',
        url: "/DNBFeature/DeleteCleanseMatchData",
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html',
        data: { Parameters: ConvertEncrypte(encodeURI(clientCode)).split("+").join("***") },
        success: function (data) {
            $("#divCacheCleanseMatchData").html(data);
            DraggableModalPopup("#myModalCleanseMatch");
        }
    });
});
function OnSuccessDeleteCleanseMatchData(data) {
    $("#CachedDataModal").modal("hide");
    $("#myModalCleanseMatch").modal("hide");
    ShowMessageNotification("success", data.message);
    if (data.result) {
        callbackCloseMagnific();
    }
}

$("body").on('click', '#btnDeleteEnrichmentdata', function () {
    // Changes for Converting magnific popup to modal popup
    var clientCode = $(this).attr("data-ClientCode");
    $.ajax({
        type: 'GET',
        url: "/DNBFeature/DeleteCachedEnrichmentData",
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html',
        data: { Parameters: ConvertEncrypte(encodeURI(clientCode)).split("+").join("***") },
        success: function (data) {
            $("#divCacheEnrichmentData").html(data);
            DraggableModalPopup("#myModalEnrichment");
        }
    });
});

function OnSuccessDeleteEnrichmentData(data) {
    $("#CachedDataModal").modal("hide");
    $("#myModalEnrichment").modal("hide");
    ShowMessageNotification("success", data.message);
    if (data.result) {
        callbackCloseMagnific();
    }
}
function callbackCloseMagnific() {
    $("#myModal").modal("hide");
}
