// Country Group
$('body').on('click', '#btnConfigCountryGroupRight', function (e) {
    var selectedOpts = $('#objCountryGroup_AddSelectedCountry option:selected');
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", nothingToMove, false);
        e.preventDefault();
    }
    $('#objCountryGroup_RemoveSelectedCountry').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('body').on('click', '#btnConfigCountryAllRight', function (e) {
    var selectedOpts = $('#objCountryGroup_AddSelectedCountry option');
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", nothingToMove, false);
        e.preventDefault();
    }
    $('#objCountryGroup_RemoveSelectedCountry').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('body').on('click', '#btnConfigCountryGroupLeft', function (e) {
    var selectedOpts = $('#objCountryGroup_RemoveSelectedCountry option:selected');
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", nothingToMove, false);
        e.preventDefault();
    }
    $('#objCountryGroup_AddSelectedCountry').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('body').on('click', '#btnConfigCountryGroupAllLeft', function (e) {
    var selectedOpts = $('#objCountryGroup_RemoveSelectedCountry option');
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", nothingToMove, false);
        e.preventDefault();
    }
    $('#objCountryGroup_AddSelectedCountry').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('body').on('click', '#btnConfigCountryGroup', function (e) {
    var value = "";
    $('#objCountryGroup_RemoveSelectedCountry option').each(function () {
        value = value + $(this).val() + ",";
    });
    $("#objCountryGroup_ISOAlpha2Codes").val(value);
});
function UpdateCountryGroup(data) {
    $("#PortalCountryGroupModal").modal("hide");
    var CountryGroupName = $(".CountryGroupName").val();
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    parent.ShowMessageNotification("success", data.message, false);
    CallbackCountryGroup(CountryGroupName);
}
function UpdateCountryGroupPopup(data) {
    $("#PortalCountryGroupModal").modal("hide");
    var CountryGroupName = $(".CountryGroupName").val();
    var CountryGroupId = data.CountryId;
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    parent.ShowMessageNotification("success", data.message, false);
    CallbackCountryGroupPopup(CountryGroupName, CountryGroupId);
}

$('body').on('click', '.btnConfigCountryGroup', function () {
    var OptionCount = $('select#objCountryGroup_RemoveSelectedCountry option').length;
    var GroupName = $("#objCountryGroup_GroupName").val().trim();
    var count = 0;
    if (GroupName == '') {
        $("#spnGroupName").show();
        count++;
    }
    else {
        $("#spnGroupName").hide();
    }
    if (OptionCount == 0) {
        $("#spnOptionValue").show();
        count++;
    }
    else {
        $("#spnOptionValue").hide();
    }
    if (count > 0) {
        return false;
    }
});