var regex = new RegExp("^[a-zA-Z0-9-_ ]+$");

$('body').on('click', '.filenameCheck', function () {
    var checksubdomain = $("#IsIncludeSubdomain").prop('checked');
    var checksrcId = $("#isIncludeSecRecordId").prop('checked');
    var checkDateTime = $("#IsIncludeDateTime").prop('checked');
    var newFileName = "";

    var domain = $("#tempdomain").val();
    var SrcRecID = $("#tempSrcRecID").val();
    var SpanTime = $("#tempSpanTime").val();


    if (checksubdomain) {
        newFileName += domain;
    }
    if (checksrcId) {
        newFileName += SrcRecID;
    }
    if (checkDateTime) {
        newFileName += SpanTime;
    }

    $("#zipFileName").val(newFileName.slice(0, -1));
});

$('body').on('click', '#btnSubmitExportName', function () {
    var zipFileName = $.trim($("#zipFileName").val());
    if (zipFileName == "") {
        $("#spnzipfilename").show();
        $("#spnzipvalidfilename").hide();
        return false;
    }
    else {
        if (regex.test(zipFileName)) {
            $("#spnzipfilename").hide();
            $("#spnzipvalidfilename").hide();
        }
        else {
            $("#zipFileName").val('');
            $("#spnzipfilename").hide();
            $("#spnzipvalidfilename").show();
            return false;
        }
    }
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/OIExportView/IsExistExportFileName?Parameters=" + ConvertEncrypte(zipFileName).split("+").join("***"),
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        async: false,
        cache: false,
        success: function (data) {
            if (data.result) {
                ExportSubmitProcess(zipFileName);
            }
            else {
                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                ShowMessageNotification("success", data.message, false);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });

});

$("body").on('keypress', '#zipFileName', function (e) {
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    else {
        if (e.keyCode == 8 || e.keyCode == 37 || e.keyCode == 38 || e.keyCode == 39 || e.keyCode == 40) { return true; }
        else {
            e.preventDefault();
            return false;
        }
    }
});
$("body").on('blur', '#zipFileName', function (e) {
    var str = this.value;
    if (regex.test(str)) {
        return true;
    }
    else {
        this.value = "";
    }
});