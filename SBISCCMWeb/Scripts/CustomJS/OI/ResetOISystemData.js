$(document).on("click", "#btnOIResetDataSubmit", function () {
    var txtDomain = $("#txtDomain").val();
    if (txtDomain.toLowerCase() == 'yes') {
        $("#spnDomainName").hide();
        $("#spnDomainValidation").hide();
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: resetOIData, callback: function (result) {
                if (result) {
                    $.post("/OISetting/ResetOISystemsData/", function (data) {
                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
                            message: data,
                            callback: function () {
                                parent.ResetCallback();
                            }
                        });

                    });
                }

            }
        });
    }
    else {
        $("#btnOIResetDataSubmit").attr("Disabled", true);
        return false;
    }
});
$('#txtDomain').on('change', function (event, node) {
    var StrAns = $(this).val();
    if (StrAns.toLowerCase() == "yes") {
        $("#btnOIResetDataSubmit").attr("Disabled", false);
    }
    else {
        $("#btnOIResetDataSubmit").attr("Disabled", true);
    }
});