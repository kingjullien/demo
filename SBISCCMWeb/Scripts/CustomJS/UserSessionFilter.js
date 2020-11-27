$('body').on('click', '.userFilter', function () {
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/UserSessionFilter/popupUserFilter/'
        },
        callbacks: {
            close: function () {
                location.reload();
            }
        },
        closeOnBgClick: false,
        mainClass: 'popInitiateReturn'
    });
    return false;
});
$(document).ready(function () {
    var ImportProcess = $("#hidenImportProcess").val();
    var cnt = 0;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $("select.chzn-select").chosen({
        no_results_text: "Oops, nothing found!",
        width: "100%",
        search_contains: true
    });
    $("#ImportProcess option").each(function () {
        if (ImportProcess == $(this).attr("value")) {
            cnt++;
        }
    });
    if (cnt == 0) {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
            message: "We have removed your session filter set as ImportProcess, because there is no data for that file.", callback: function () {
                    $.ajax({
                        type: "POST",
                        url: "/StewardshipPortal/DeleteSessionFilter/",
                        data: '',
                        headers: { "__RequestVerificationToken": token },
                        dataType: "json",
                        contentType: "application/json",
                        success: function (data) {
                            if (data == "success") {
                                location.reload();
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
            }
        });
        $("#ImportProcess").val("");
    }

    $('input:enabled:visible:first').focus();
    
});

function SetTagValue(OptionValue) {
    $.magnificPopup.close();
    var x = document.getElementById("TagsValue");
    var option = document.createElement("option");
    option.text = OptionValue;
    option.value = OptionValue;
    x.add(option);
    $(".chzn-select").trigger("chosen:updated");
}
// Delete Session Filter
$('body').on('click', '.DeleteFilter', function () {

    var pagevalue = $("#pagevalue").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    $(this).find("strong").removeClass("fa fa-plus");
    $(this).find("strong").removeClass("fa fa-minus");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteSessionFilter, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/StewardshipPortal/DeleteSessionFilter/",
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    success: function (data) {
                        if (data == "success") {
                            //location.reload();
                            window.parent.$.magnificPopup.close();
                            //window.location.href = "/StewardshipPortal/Index?pagevalue=" + pagevalue;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    return false;
});

function ShowMessageNotification(MessageType, Message, isPopup, IsCallBack, FunctionName) {
    if (isPopup == true) {
        $.magnificPopup.close();
    }
    parent.ShowMessageNotification(MessageType, Message, false, false)
    if (IsCallBack) {
        FunctionName();
    }
}