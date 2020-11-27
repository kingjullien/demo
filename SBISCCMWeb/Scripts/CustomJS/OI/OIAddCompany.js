// Add Company in for the OISearchData and display Message.
$("body").on('click', '.AddCompany', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var SrcId = $("#txtSrcId").val();
    var OriginalSrcRecordId = $("#OriginalSrcRecordId").val();
    var MatchUrl = $("#MatchUrl").val();
    var ResponseJson = $("#ResponeJson").val();
    var orb_num = $("#ORBNumber").val();

    var UserType = $("#UserType").val();
    var UserTags = $("#UserTags").val();

    var Tag = '';
    var cnt = 0;
    if ($("#Tags").length > 0) {
        Tag = $("#Tags").val();
    }
    if (SrcId != null && SrcId != "" && $.trim(SrcId).length != 0) {
        $("#spnSrcId").hide();
    }
    else {
        $("#spnSrcId").show();
        cnt++;
    }
    if ($("#UserRole").val().toLowerCase() == 'lob' || (UserType.toLowerCase() == "steward" && UserTags != "")) {
        if (Tag == '' || Tag == "0") {
            cnt++;
            $("#spnTags").show();
        }
        else {
            $("#spnTags").hide();
        }
    }
    if (cnt > 0) {
        return false;
    }
    else {
        $('#divProgress').show();
        $.ajax({
            type: "POST",
            url: "/OISearchData/AddCompany/",
            data: JSON.stringify({ SrcId: SrcId, orb_num: OriginalSrcRecordId, Tag: Tag, MatchUrl: MatchUrl, ResponseJson: ResponseJson }),
            headers: { "__RequestVerificationToken": token },
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                if (data != success) {
                    $('#divProgress').hide();
                    $("#OISearchDataAddCompanyModal").modal("hide");
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                    parent.ShowMessageNotification("success", data, false);
                } else {
                    $('#divProgress').hide();

                    var fn = parent.backToparent;
                    $("#OISearchDataAddCompanyModal").modal("hide");
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", addNewCompany, false, true, fn);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
});

//For OIMatchData
$("body").on('click', '.AddMatchCompany', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var SrcId = $("#txtSrcId").val();
    var orb_num = $("#btnAddCompanyMatch").attr('data-val');
    var OriginalSrcRecordId = $("#OriginalSrcRecordId").val();
    var InputId = $(this).attr("data-MatchInput");

    var UserType = $("#UserType").val();
    var UserTags = $("#UserTags").val();

    var Tag = '';
    var cnt = 0;
    if ($("#Tags").length > 0) {
        Tag = $("#Tags").val();
    }
    if (SrcId != null && SrcId != "" && $.trim(SrcId).length != 0) {
        $("#spnSrcId").hide();
    }
    else {
        $("#spnSrcId").show();
        cnt++;
    }
    if ($.UserRole.toLowerCase() == 'lob' || (UserType.toLowerCase() == "steward" && UserTags != "")) {
        if (Tag == '' || Tag == "0") {
            cnt++;
            $("#spnTags").show();
        }
        else {
            $("#spnTags").hide();
        }
    }
    if (cnt > 0) {
        return false;
    }
    else {
        $('#divProgress').show();
        $.ajax({
            type: "POST",
            url: "/OIMatchData/OIAddCompanyMatch/",
            data: JSON.stringify({ SrcId: SrcId, InputId: InputId, orb_num: OriginalSrcRecordId, Tag: Tag }),
            headers: { "__RequestVerificationToken": token },
            dataType: "json",
            contentType: "application/json",
            success: function (data) {

                if (data != "success") {
                    $('#divProgress').hide();
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                    ShowMessageNotification("success", data, false);
                } else {
                    $('#divProgress').hide();
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    ShowMessageNotification("success", addNewCompany, false);
                    $("#OIAddCompanyMatchModal").modal("hide");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
});

function onLoadOIAddCompany() {
    $(".chzn-select").chosen();
}
