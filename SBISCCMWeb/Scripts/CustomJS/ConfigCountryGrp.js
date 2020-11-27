function closemsg() {
    $(".alert-success").hide();

}
// manage the show hide of the Group name and Option Value if Coutry Group
$('body').on('click', '.btnConfigCountryGroup', function () {

    var OptionCount = $('select#objCountryGroup_RemoveSelectedCountry option').length;
    var GroupName = $("#objCountryGroup_GroupName").val();
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

function CountryGrpSuccess() {
    var message = $("#CountryGrpMessage").val();
    if ($.trim(message) != '') {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
            message: message
        });
    }
}

