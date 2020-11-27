$("body").on("change", ".clsMatchGrade", function () {
    var p1 = "#", p2 = "#", p3 = "#", p4 = "#", p5 = "#", p7 = "#", p6 = "#";
    if ($("#boolBusinessName").is(":checked")) {
        p1 = "A";
    }
    if ($("#boolStreet").is(":checked")) {
        p2 = "A";
    }
    if ($("#boolStreetName").is(":checked")) {
        p3 = "A";
    }
    if ($("#boolCity").is(":checked")) {
        p4 = "A";
    }
    if ($("#boolState").is(":checked")) {
        p5 = "A";
    }
    if ($("#boolPoBox").is(":checked")) {
        p6 = "A";
    }
    if ($("#boolTelephone").is(":checked")) {
        p7 = "A";
    }


    var matchGrade = p1 + p2 + p3 + p4 + p5 + p6 + p7 + "####";
    if (p1 == "A" && p2 == "A" && p3 == "A" && p4 == "A" && p5 == "A" && p6 == "A" && p7 == "A") {
        $("#boolSelectAll").prop("checked", true);
    }
    else {
        $("#boolSelectAll").prop("checked", false);
    }
    $("#spnMatchGrade").html(matchGrade);
});
   
 //====================================================================================================================

//start  D&B License Keys 
function GetIndexDandBCredentials() {
    $.ajax({
        type: 'GET',
        url: "/DNBLicence/GetDandBCredentials",
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        async: false,
        success: function (data) {
            $("#divPartialDandBCredentials").html(data);
        }, error: function (xhr, ajaxOptions, thrownError) { }
    });
}
function AddUpdateThirdPartyAPICredentials(credId) {
    var url = "/DNBLicence/AddUpdateThirdPartyDandBCredential";
    if (credId != undefined && credId > 0) {
        url = url + "?Parameters=" + ConvertEncrypte(encodeURI(credId)).split("+").join("***");
    }
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#ThirdPartAPICredModalMain").html(data);
            DraggableModalPopup("#ThirdPartAPICredModal");
        }
    });
}
$(document).on("click", ".editThirdPartyAPICredentials", function () {
    var CredentialId = $(this).attr("data-CredentialId");
    if (CredentialId != null && CredentialId != undefined && CredentialId != '') {
        AddUpdateThirdPartyAPICredentials(CredentialId);
    }

});
function onSuccessAddUpdatethirdPartyAPICredentials(data) {
    ShowMessageNotification("success", data.Message);
    if (data.result) {
        $("#ThirdPartAPICredModal").modal("hide");
        GetIndexDandBCredentials();
    }
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
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteThirdPartyAPICred, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/DNBLicence/DeleteThirdPartyAPICredentials?Parameters=" + ConvertEncrypte(encodeURI(CredentialId)).split("+").join("***"),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.Message);
                        if (data.result) {
                            $("#ThirdPartAPICredModal").modal("hide");
                            GetIndexDandBCredentials();
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
$(document).on('click', '#form_setEntilementsForCred .btnSaveEntitlements', function () {
    var selectedval = $("#form_setEntilementsForCred #DnBAPIIdValue").val();
    if (selectedval.length == 0) {
        $("#form_setEntilementsForCred #spnErrEntitlements").show();
        return false;
    }
    else {
        $("#form_setEntilementsForCred #spnErrEntitlements").hide();
        $("#form_setEntilementsForCred #DnBAPIId").val(selectedval.toString());
    }
});
$(document).on('click', '.setEntilementForThirdPartyAPICredentials', function () {
    var CredentialId = $(this).attr("data-CredentialId");
    var APIType = $(this).attr("data-APIType");
    if (CredentialId != null && CredentialId != undefined && CredentialId != '') {
        SetEntitleMentsForCreds(CredentialId, APIType);
    }
});
function SetEntitleMentsForCreds(credId, APIType) {
    var QueryString = "credId:" + credId + "@#$aPIType:" + APIType;
    $.ajax({
        type: 'GET',
        url: "/DNBLicence/SetEntitleMentsForCreds?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#ThirdPartAPICredEntitlementsModalMain").html(data);
            DraggableModalPopup("#ThirdPartAPICredEntitlementsModal");
            LoadEntitlementsMultiSelect();
        }
    });
}
function LoadEntitlementsMultiSelect() {
    var arrfields = $("#form_setEntilementsForCred #lstAPIIds").val().split(',');
    $("#form_setEntilementsForCred #DnBAPIIdValue option").each(function () {
        for (var i = 0; i < arrfields.length; i++) {

            if ($(this).val() == arrfields[i]) {
                $(this).attr("selected", "selected");
            }
        }
    });

    $('#form_setEntilementsForCred #DnBAPIIdValue').bootstrapDualListbox({
        selectorMinimalHeight: 220,
        filterOnValues: false,
        filterTextClear: 'show all',
        moveOnSelect: false
    });
}
function SuccessSetEntitleMentsForCreds(data) {
    if (data.result) {
        $("#ThirdPartAPICredEntitlementsModal").modal('hide');
        ShowMessageNotification("success", data.Message);
    }
    else {
        ShowMessageNotification("success", data.Message);
    }
}

//End  D&B License Keys

//====================================================================================================================

//Start Default Keys For UI
function GetDefaultInteractiveKeys() {
    $.ajax({
        type: 'GET',
        url: "/DNBLicence/DefaultInteractiveKeys",
        dataType: "HTML",
        contentType: "application/html",
        cache: false,
        async: false,
        success: function (data) {
            $("#divPartialDefaultInteractiveKeys").html(data);
        }
    });
}
function OnSuccessDefaultInteractiveKey(data) {
    ShowMessageNotification("success", data.Message);
}
//End Default Keys For UI

//====================================================================================================================

//Start Default Keys For UI For Enrichment
function GetDefaultKeysForEnrichment() {
    $.ajax({
        type: 'GET',
        url: "/DNBLicence/DefaultKeysForEnrichment",
        dataType: "HTML",
        contentType: "application/html",
        cache: false,
        async: false,
        success: function (data) {
            $("#divPartialDefaultKeysForEnrichment").html(data);
            var CredID = $("#CredentialId").val();
            $('#CredID > option').each(function () {
                var thisvalue = this.value;
                if (thisvalue.startsWith(CredID + "@#$")) {
                    $("#CredID").val(this.value);
                }
            });
        }
    });
}
function ValidationForDefaultKeyForEnrichment() {
    var CredentialId = $("#CredID").val();
    var newValue = CredentialId.split("@#$");
    $('#CredentialId').val(newValue[0]);
    var DnBAPIId = $("#DnBAPIId").val();
    var cnt = 0;

    if (CredentialId == '') {
        $("#spnCredName").show();
        cnt++;
    }
    else {
        $("#spnCredName").hide();
    }

    if (DnBAPIId == '') {
        $("#spnAPIType").show();
        cnt++;
    }
    else {
        $("#spnAPIType").hide();
    }

    if (cnt > 0) {
        return false;
    }
    return true;
}
function OnSuccessDefaultInteractiveKeyEnrichment(data) {
    ShowMessageNotification("success", data.Message);
}
$("body").on('change', '#CredID', function () {
    $("#spnCredName").hide();
    $("#spnAPIType").hide();
    var CredentialId = $("#CredID").val();
    var newValue = CredentialId.split("@#$");
    var newAPIValue = CredentialId.split("@#$");
    newValue = newValue[0];
    newAPIValue = newAPIValue[1];
    if (newAPIValue == undefined) {
        newAPIValue = "";
    }
    $.ajax({
        type: 'POST',
        url: "/DNBLicence/GetThirdPartyAPIType?newAPIValue=" + newAPIValue,
        dataType: 'JSON',
        cache: false,
        contentType: 'application/json;',
        success: function (data) {
            var i = 0;
            $("#DnBAPIId option").remove();
            if (data.length == 0) {
                return false;
            }
            if (data.length == 1) {
                if (data[i].Value.length > 0) {
                    for (cnt = 0; cnt < data[i].Value.length; cnt++) {
                        $("#DnBAPIId").append(new Option(data[i].Text, data[i].Value));
                        i++;
                        if (i == 1)
                            break;
                    }
                }
            }
            else {
                if (data[i].Value.length > 0) {
                    for (cnt = 0; cnt < data[i].Value.length; cnt++) {
                        $("#DnBAPIId").append(new Option(data[i].Text, data[i].Value));
                        i++;
                        if (i == 2)
                            break;
                    }
                }
            }
        }
    });
});
//End Default Keys For UI For Enrichment

//====================================================================================================================

//Start Background Process Settings
function GetBackgroundProcessSettings() {
    $.ajax({
        type: 'GET',
        url: "/DNBLicence/BackgroundProcessSettings",
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        async: false,
        success: function (data) {
            $(".divPartialBackgroundProcessSettings").html(data);
        }
    });
}
function OnSuccessBackgroundProcessSettings(data) {
    ShowMessageNotification("success", data.Message);
}
//End Background Process Settings

//====================================================================================================================