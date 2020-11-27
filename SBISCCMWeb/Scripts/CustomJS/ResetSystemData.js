$(document).on("click", "#btnResetDataSubmit", function () {
    var txtDomain = $("#txtDomain").val();
    var isResetConfig = $("#isResetConfig").val();
    if (txtDomain.toLowerCase() == 'yes') {
        $("#spnDomainName").hide();
        $("#spnDomainValidation").hide();
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: resetData, callback: function (result) {
                if (result) {
                    $.post("/DandB/ResetAllSystemData/", { isResetConfig: isResetConfig }, function (data) {
                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
                            message: data,
                            callback: function () {
                                ResetCallback();
                            }
                        });

                    });
                }

            }
        });
    }
    else {
        $("#btnResetDataSubmit").attr("Disabled", true);
        return false;
    }
});
$(document).on('change', '#txtDomain', function (event, node) {
    var StrAns = $(this).val();
    if (StrAns.toLowerCase() == "yes") {
        $("#btnResetDataSubmit").attr("Disabled", false);
    }
    else
    {
        $("#btnResetDataSubmit").attr("Disabled", true);
    }
});