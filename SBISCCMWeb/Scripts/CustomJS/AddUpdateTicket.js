// Display and Hide loader bar for every ajax call
$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});

$(document).ready(function () {
    $(".browserimg").each(function () {
        var src = $(this).attr("src");
        if (src == "") {
            $(this).parent().hide();
        }
    });
});
// Mask phome number patent 
jQuery(function ($) {
    $(".maskPhone").mask("(999) 999-9999");
});
// Validate format of Email address
function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
// Upload image or file for ticket and do ajax call for upload image or file on azure as well as database also.
$("body").on('change', '.browserFile', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $('#divProgress').show();
    var formData = new FormData();
    var TicketId = $('#hdnTicketId').val();
    var file = document.getElementById("file").files[0];
    formData.append('file', $('#file')[0].files[0]);
    formData.append('TicketId', TicketId);
    $.ajax({
        type: "POST",
        url: '/Ticket/UploadImage',
        data: formData,
        headers: { "__RequestVerificationToken": token },
        contentType: false,
        processData: false,
        success: function (response) {
            if ((response.indexOf('Only formats allowed are')) >= 0) {
                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                ShowMessageNotification("success",response, false);
                $("#file").val("");
            }
            else if (response == wrongFormat) {
                ShowMessageNotification("success", formatsAllowed, false);
                $('#file').val("");

            }
            else {
                $('#divUploadImage').html(response);
            }

            $(".browserimg").each(function () {
                var src = $(this).attr("src");
                if (src == "") {
                    $(this).parent().hide();
                }
            });
            $('#divProgress').hide();
        },
        error: function (xhr, status, error) {
            $('#divProgress').hide();
        }
    });
});
// remove image or file for ticket and do ajax call for remove image or file on azure as well as database also.
$("body").on('click', '.removeImage', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var ImageName = $(this).attr("data-id");
    var TicketId = $('#hdnTicketId').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: removeImageFile, callback: function (result) {
            if (result) {
                $('#divProgress').show();
                $.ajax({
                    type: "POST",
                    url: '/Ticket/RemoveImage?ImageName=' + ImageName + '&TicketId=' + TicketId,
                    headers: { "__RequestVerificationToken": token },
                    contentType: false,
                    processData: false,
                    success: function (response) {

                        $('#divUploadImage').html(response);
                        $(".browserimg").each(function () {
                            var src = $(this).attr("src");
                            if (src == "") {
                                $(this).parent().hide();
                            }
                        });
                        $('#divProgress').hide();
                    },
                    error: function (xhr, status, error) {
                        $('#divProgress').hide();
                    }
                });
            }
        }
    });

});
// Download uploaded image and do ajax call for download image or file.
$("body").on('click', '.downloadImage', function () {
    var ImageName = $(this).attr("data-id");
    var url = '/Handler/DownloadFile.ashx?ImageName=' + ImageName;
    window.location.href = url;
});

function myFunction() {
    $("#divProgress").show();
    if (!$("#AddUpdateticketFrm").valid()) {
        $("#divProgress").hide();
    }
    
}
