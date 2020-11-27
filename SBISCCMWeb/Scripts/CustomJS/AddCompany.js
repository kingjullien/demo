$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();

// Add Company in for the BadInputData and display Messgae.
$("body").on('click', '.AddCompany', function (e) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var SrcId = $("#txtSrcId").val();
    var MatchRecord = $("#btnAddCompany").attr('data-val');
    var OriginalSrcRecordId = $("#OriginalSrcRecordId").val();
    var IsTagsInclusive = $("#IsTagsInclusive").val();


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
        if (UserType.toLowerCase() == "steward" && IsTagsInclusive.toLowerCase()=="true" && UserTags != "" && (Tag != '' && Tag != '0')) {
            var InclusiveTagslst = UserTags.split(',');
            var Tagslst = Tag.split(',');
            var Iscontaintcnt = 0;
            for (var i = 0; i < InclusiveTagslst.length; i++) {
                if ($.inArray(InclusiveTagslst[i], Tagslst) >= 0) {
                    Iscontaintcnt++;
                }
            }
            if (InclusiveTagslst.length == Iscontaintcnt) {
                $("#spnInclusiveTags").hide();
            }
            else {
                $("#spnInclusiveTags").show();
                cnt++;
            }
        }
        else {
            $("#spnInclusiveTags").hide();
        }
    }
    if (cnt > 0) {
        return false;
    }
    else {
        $('#divProgress').show();
        $.ajax({
            type: "POST",
            url: "/BadInputData/AddCompany/",
            data: JSON.stringify({ SrcId: SrcId, MatchRecord: MatchRecord, Tag: Tag, OriginalSrcRecordId: OriginalSrcRecordId }),
            headers: { "__RequestVerificationToken": token },
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                if (data != success) {
                    $('#divProgress').hide();
                    $("#SearchDataAddCompanyModal").modal("hide");
                    $("#SearchPopupModal").modal("hide");
                    $("#MatchDetailModal").modal("hide");
                    $("#ReviewMatchDetailModal").modal("hide");
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                    parent.ShowMessageNotification("success", data, false);
                } else {
                    $('#divProgress').hide();

                    var fn = parent.backToparent;
                    $("#SearchDataAddCompanyModal").modal("hide");
                    $("#SearchPopupModal").modal("hide");
                    $("#MatchDetailModal").modal("hide");
                    $("#ReviewMatchDetailModal").modal("hide");
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", addNewCompany, false, false, fn);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
        e.stopImmediatePropagation();
        return false;
    }
});