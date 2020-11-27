$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();

$(document).ready(function () {
    $("#LOBTag").val($("#HiddenLOBTag").val());
    if ($("#LicenseEnableTags").val().toLowerCase() == "true") {
        LoadTags();
    }
});
function LoadTags() {
    if ($.UserRole.toLowerCase() == "lob") {
        $("#LOBTag").val($.UserLOBTag);
        $("#LOBTag").attr("disabled", true);
    }
    var TagList = $("#ActiveStatisticsFilterTagList").val().split(',');
    if (TagList != null || TagList != "") {
        $(".chzn-select option").each(function () {
            for (var i = 0; i < TagList.length; i++) {
                if ($(this).val() == TagList[i]) {
                    $(this).attr("selected", "selected");
                }
            }
        });
        $("#ActiveStatisticsFilterTags").val(TagList);
    }
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {

            if (event.target == this) {
                $("#ActiveStatisticsFilterTags").val($(this).val());
            }
        });
    }
}
$('body').on('click', '.DownloadDataQueueStatistics', function () {
    $("#IsDownload").val(true);
    $("#frmDataQueueStatistics").submit();
});

function btnFilter() {
    $("#IsDownload").val(false);
    return true;
}

function ConvertEncrypte(value) {
    var newvalue = "";
    if (value != '' && value != undefined) {
        $.ajax({
            type: "POST",
            url: "/Home/GetEncryptedString",
            data: JSON.stringify({ strValue: value }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            success: function (data) {
                newvalue = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
    return newvalue;
}
function ShowMessageNotification(MessageType, Message, isPopup, IsCallBack, FunctionName) {
    if (isPopup == true) {
        $.magnificPopup.close();
    }
    parent.ShowMessageNotification(MessageType, Message, false, false);
    if (IsCallBack) {
        FunctionName();
    }
}