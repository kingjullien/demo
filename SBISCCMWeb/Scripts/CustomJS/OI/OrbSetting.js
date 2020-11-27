//Start License
$('body').on('click', '#IdOrbLicenceTab', function () {
    var divOrbLicense = document.getElementById("OrbLicenceTab");
    divOrbLicense.style.display = "block";
    LoadOrbLicense();
    $("#IdOrbIdentityResolutionTab").parent("li").removeClass("active");
});
function SuccessOrbLicense(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#OrbCredentialsMessgae").remove();
        LoadOrbLicense();
    }
}
function LoadOrbLicense() {
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/OI/Setting/License", container: '#divPartialOrbLicense',
            timeout: 50000
        }).done(function () {
        });
}
function LoadOrbBackground() {
    var url = '/OISetting/OIBackgroundProcessSettings/'
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#divPartialOrbBackgroundProcessSettings").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function SuccessOrbBackground(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#OrbBackgroundMessgae").remove();
        LoadOrbBackground();
    }
}

function CheckBackgroundValidation() {  
    var cnt = 0
    var ORB_BATCH_SIZE = $("#ORB_BATCH_SIZE").val();
    var ORB_BATCH_WAITTIME_SECS = $("#ORB_BATCH_WAITTIME_SECS").val();
    var ORB_MAX_PARALLEL_THREADS = $("#ORB_MAX_PARALLEL_THREADS").val();
    $("#PAUSE_ORB_BATCHMATCH_ETL").val($("#PAUSE_ORB_BATCHMATCH_ETL").is(":checked"));
    $("#ORB_ENABLE_CORPORATE_TREE_ENRICHMENT").val($("#ORB_ENABLE_CORPORATE_TREE_ENRICHMENT").is(":checked"));
    if (ORB_BATCH_SIZE.trim() == "") {
        $("#SpnBatchsize").show();
        cnt++;
    }
    else {
        $("#SpnBatchsize").hide();
    }
    if (ORB_BATCH_WAITTIME_SECS.trim() == "") {
        $("#SpnBatchWaittimeSecs").show();
        cnt++;
    }
    else {
        $("#SpnBatchWaittimeSecs").hide();
    }
    if (ORB_MAX_PARALLEL_THREADS.trim() == "") {
        $("#SpnMaxParallelThreads").show();
        cnt++;
    }
    else {
        $("#SpnMaxParallelThreads").hide();
    }

    if (cnt > 0) {
        return false;
    }
    else {
        return true;
    }
}
function CheckCredentialValidation() {
    var cnt = 0;
    var ORB_API_KEY = $("#ORB_API_KEY").val();

    if (ORB_API_KEY.trim() == "") {
        $("#spnOrbApiKey").show();
        cnt++;
    }
    else {
        $("#spnOrbApiKey").hide();
    }
    if (cnt > 0) {
        return false;
    }
    else {
        return true;
    }
}

function LoadDataImportDuplicateResolutionTags() {
    if ($("#DATA_IMPORT_DUPLICATE_RESOLUTIONTagList").val() != undefined) {
        var TagList = $("#DATA_IMPORT_DUPLICATE_RESOLUTIONTagList").val().split(',');
        if (TagList != null || TagList != "") {
            $("#DataImportDuplicateResolutionTagValue option").each(function () {
                for (var i = 0; i < TagList.length; i++) {
                    if ($(this).val() == TagList[i]) {
                        $(this).attr("selected", "selected");
                    }
                }
            });
            $("#ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS").val(TagList);
        }
        if ($("#DataImportDuplicateResolutionTagValue").length > 0) {
            $("#DataImportDuplicateResolutionTagValue").chosen().change(function (event) {
                if (event.target == this) {
                    $("#ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS").val($(this).val());
                }
            });
        }
    }
}

$(document).ready(function () {
    var pageURL = $(location).attr("href");
    if (pageURL.includes("License")) {
        var divOrbLicense = document.getElementById("OrbLicenceTab");
        divOrbLicense.style.display = "block";
    }
    LoadDataImportDuplicateResolutionTags();
});

function SuccessOrbImportDataHandling(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#OrbDataImportHandlingMessgae").remove();
        LoadOrbDataImportHandlingMessgae();
    }
}
function LoadOrbDataImportHandlingMessgae() {
    var url = '/OISetting/IndexDataImportHandling/'
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#divPartialDataImportHandling").html(data);
            LoadDataImportDuplicateResolutionTags();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

//end License

//Start Auto-Acceptance
$(document).on('click', '#IdOrbIdentityResolutionTab', function () {
    //if (!$(this).parent("li").hasClass("active")) {
    $("#IdOrbLicenceTab").parent("li").removeClass("active");
    LoadOIAutoAcceptance();
    //}
});
function LoadOIAutoAcceptance() {
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/OI/Setting/IdentityResolution", container: '#divPartialOIAutoAcceptance',
        timeout: 50000
    }).done(function () {
        $('table.OIAutoAcceptance tbody tr:first').addClass('current');
    });
}
$(document).on("click", "#btnAddOIAutoAcceptance", function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/OISetting/InsertUpdateOIAutoAcceptance/",
        dataType: 'HTML',
        success: function (data) {
            $("#AddOIAutoAcceptanceModalMain").html(data);
            DraggableModalPopup("#AddOIAutoAcceptanceModal");
        }
    });
});
$(document).on("click", ".editOIAutoAcceptance", function () {
    var RuleId = $(this).attr("data-id");
    var QueryString = "RuleId:" + RuleId;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/OISetting/InsertUpdateOIAutoAcceptance?Parameters=" + Parameters,
        dataType: 'HTML',        
        success: function (data) {
            $("#AddOIAutoAcceptanceModalMain").html(data);
            DraggableModalPopup("#AddOIAutoAcceptanceModal");
        }
    });
});
$(document).on("click", ".DeleteOIAutoAcceptance", function () {
    var id = $(this).attr("id");
    if (id == undefined) {
        id = 0;
    }
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteAutoAcceptancemsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/OISetting/DeleteAutoAcceptance?Parameters=" + ConvertEncrypte(encodeURI(id)).split("+").join("***"),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                            ShowMessageNotification("success", data, false);
                            LoadOIAutoAcceptance();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
                return true;
            }
        }
    });
});

//End Auto-Acceptance

$('body').on('click', '#btnResetOISystemData', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: '/OISetting/ResetOISystemData',
        dataType: 'HTML',
        success: function (data) {
            $("#OIResetSystemDataModalMain").html(data);
            DraggableModalPopup("#OIResetSystemDataModal");
        }
    });
    return false;
});
function ResetCallback() {
    $("#OIResetSystemDataModal").modal("hide");
    location.reload();
}
$(document).on("click", "#AddOrbThirdPartyAPICredentials", function () {
    AddUpdateThirdPartyOrBAPICredentials(0);
});
$(document).on("click", ".editThirdPartyAPICredentials", function () {
    var CredentialId = $(this).attr("data-CredentialId");
    if (CredentialId != null && CredentialId != undefined && CredentialId != '') {
        AddUpdateThirdPartyOrBAPICredentials(CredentialId);
    }
});

function AddUpdateThirdPartyOrBAPICredentials(credId) {
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/OISetting/AddUpdateThirdPartyOrbCredential?Parameters=' + ConvertEncrypte(encodeURI(credId)).split("+").join("***")
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupAddOrbThirdPartyAPICredentials'
    });
}

$(document).on("click", ".deleteThirdPartyAPICredentials", function () {
    var CredentialId = $(this).attr("data-CredentialId");
    if (CredentialId != null && CredentialId != undefined && CredentialId != '') {
        deleteThirdPartyAPICredentials(CredentialId);
    }

});

function deleteThirdPartyAPICredentials(CredentialId) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteThirdPartyAPICredentials, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/OISetting/DeleteThirdPartyAPICredentials?Parameters=" + ConvertEncrypte(encodeURI(CredentialId)).split("+").join("***"),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        if (data.toLowerCase() == dataDeleted) {
                            GetIndexOrbCredentials();
                            ShowMessageNotification("success", data, false);
                        }
                        else {
                            ShowMessageNotification("success", data, false);
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
                return true;
            }
        }
    });
}

function callbackthirdPartyOrbAPICredentials(message) {
    ShowMessageNotification("success", message, false);
    GetIndexOrbCredentials();
} 

function GetIndexOrbCredentials() {
    $.ajax({
        type: 'GET',
        url: "/OISetting/IndexThirdPartyAPICredentials",
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        async: false,
        success: function (data) {
            $("#divPartialThirdPartyAPICredentials").html(data);
        },
        error: function (e, er, err) {
        }
    });
}

$(document).on('keypress', '#ORB_BATCH_SIZE', function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var result = (keyCode >= 48 && keyCode <= 57);
    return result;
});

$(document).on('keypress', '#ORB_BATCH_WAITTIME_SECS', function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var result = (keyCode >= 48 && keyCode <= 57);
    return result;
});

$(document).on('keypress', '#ORB_MAX_PARALLEL_THREADS', function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var result = (keyCode >= 48 && keyCode <= 57);
    return result;
});