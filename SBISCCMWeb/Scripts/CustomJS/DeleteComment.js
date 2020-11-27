$('body').on('click', '.btnCancel', function () {
    $("#DeleteAutoAcceptanceDataModal").modal("hide");
});
$('body').on('click', '#btnDeleteAutoAcceptance', function () {
    var ToCall = $("#ToCall").val();
    var CommentId = $("#CommentId").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (CommentId != undefined && CommentId != '') {
        $("#spnComment").hide();
        var CriteriaGroupId = $("#CriteriaGroupId").val();
        if (ToCall == "DeleteAcceptance") {
            $.ajax({
                type: "POST",
                url: "/DNBIdentityResolution/DeleteAcceptance?CriteriaGroupId=" + CriteriaGroupId + '&CommentId=' + CommentId,
                data: '',
                headers: { "__RequestVerificationToken": token },
                dataType: "json",
                contentType: "application/json",
                cache: false,
                success: function (data) {
                    // Changes for Converting magnific popup to modal popup
                    $("#DeleteAutoAcceptanceDataModal").modal("hide");
                    $('#divProgress').hide();
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    ShowMessageNotification("success", data.message);
                    LoadIndexAutoAcceptance("");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
        if (ToCall == "ImportData") {
            var OrgColumnName = $("#OrgColumnName").val();
            var ExcelColumnName = $("#ExcelColumnName").val();
            var Tags = $("#Tags").val();
            var IsOverWrite = $("#IsOverWrite").val();
            var CompanyScore = $("#CompanyScore").val();

            $.ajax({
                type: "POST",
                url: "/DNBIdentityResolution/CleanseMatchDataMatchAutoAccept",
                data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, Tags: Tags, IsOverWrite: IsOverWrite, CommentId: CommentId, CompanyScore: CompanyScore }),
                headers: { "__RequestVerificationToken": token },
                dataType: "json",
                contentType: "application/json",
                async: false,
                cache: false,
                success: function (data) {
                    // Changes for Converting magnific popup to modal popup
                    $("#DeleteAutoAcceptanceDataModal").modal("hide");
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    ShowMessageNotification("success", data);
                    LoadIndexAutoAcceptancePagination();

                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
        else {
            $("#DeleteAutoAcceptanceDataModal").modal("hide");
            //return false;
        }
    } else {
        $("#spnComment").show();
        return false;
    }
});