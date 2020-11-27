$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();

$(document).ready(function () {
    var isAllowLOBTag = $("#isAllowLOBTag").val();

    if (isAllowLOBTag.toLowerCase() == "false") {
        $("#TagTypeCode option[value='601006@#$LOB']").remove();
    }
    if ($.UserRole.toLowerCase() == "lob") {
        $("#TagTypeCode option[value='601006@#$LOB']").remove();
        $("#LOBTag").val($.UserLOBTag);
        $("#LOBTag").attr("disabled", true);
    }
});

$("body").on('keypress', '#txtTags', function (e) {
    if (e.which == 13) {
        SaveTags();
        return false;
    }
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var result = ((keyCode >= 48 && keyCode <= 57) || keyCode == 45 || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || keyCode == 95);
    if (result == true && e.which == 13) {
        SaveTags();
        return false;
    }
    if (result == false) {
        $("#spnIsValidTag").show();
    }
    else if (result == true) {
        $("#spnIsValidTag").hide();
    }
    return result;
});

function SaveTags() {
    var cntError = 0;
    if ($('#txtTags').val() == "") {
        $("#spnSearchText").show();
        cntError++;
    } else {
        $("#spnSearchText").hide();
    }

    if ($("#TagTypeCode").val() == "") {
        $("#spnTag").show();
        cntError++;
    } else {
        $("#spnTag").hide();
    }

    if (cntError > 0) {
        return false;
    }
    else {
        return true;
    }
}
$('#txtTags').on('change', function (event, node) {
    if ($(this).val() != "") {
        $("#spnSearchText").hide();
    }
    var tagsval = $(this).val();
    var tagval = $("#TagTypeCode").val().split('@#$')[1];
    if (tagval == undefined) {
        tagval = "";
    }
    var TypeCode = tagval;
    var newval = "[" + TypeCode + "::" + tagsval + "]";
    $("#txttag").text(newval);
});
$("body").on('change', '#TagTypeCode', function () {
    var isAllowLOBTag = $("#isAllowLOBTag").val();
    if ($("#TagTypeCode").val() != "") {
        $("#spnTag").hide();
    }
    var tagval = $("#TagTypeCode").val().split('@#$')[1];
    if (tagval == undefined) {
        tagval = "";
    }
    var tagsval = $("#txtTags").val();
    var TypeCode = tagval;
    var newval = "[" + TypeCode + "::" + tagsval + "]";
    $("#txttag").text(newval);

    if (tagval.toLowerCase() == "lob") {
        $("#LOBTag").val("");
        $("#LOBTag option:selected").text("");
        $(".clsLobtag").hide();
    }
    else {
        $(".clsLobtag").show();
    }

});

$('#txtTags').on('keyup', function (event, node) {

    if ($(this).val() != "") {
        $("#spnSearchText").hide();
    }
    var tagsval = $(this).val();
    var tagval = $("#TagTypeCode").val().split('@#$')[1];
    if (tagval == undefined) {
        tagval = "";
    }
    var TypeCode = tagval;
    var newval = "[" + TypeCode + "::" + tagsval + "]";
    $("#txttag").text(newval);
});

function OnSuccessAddTags(data) {
    $("#divProgress").hide();
    $("#AddTagsModal").modal("hide");
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        SetTagValue(data.tagValue);
    }
}