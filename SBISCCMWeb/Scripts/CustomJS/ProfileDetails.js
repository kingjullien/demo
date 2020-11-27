$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {

    $('#divProgress').hide();
});
$(document).ready(function () {
    $(".updateSecurityAnswer").prop("disabled", true);
    var ImagePath = $("#Imagepath").val();
    if ("@Model.Imagepath" == '') {
        $("#btRemoveImage").hide();
    }
    //set Cookie for the Validation 
    function createCookie(name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else var expires = "";
        document.cookie = name + "=" + value + expires + "; path=/";
    }
    //Remove Cookie
    function readCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    function eraseCookie(name) {
        createCookie(name, "", -1);
    }


});
// Upload Image.
$("body").on('click', '#btnSubmit', function () {

    if ($('#file').val() != "") {
        var ImgPath = $("#Imagepath").val();
        var userName = $("#LoginId").val();
        var userId = $("#UserId").val();
        var formData = new FormData();
        var file = document.getElementById("file").files[0];

        formData.append('file', $('#file')[0].files[0]);
        formData.append("UserId", userId);
        formData.append("UserName", userName);
        formData.append("Imgpath", ImgPath);
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: "POST",
            url: '/Home/UploadImage',
            data: formData,
            headers: { "__RequestVerificationToken": token },
            dataType: 'json',
            contentType: false,
            processData: false,
            async: false,
            success: function (response) {
                if (response == failer) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
                    ShowMessageNotification("success", removeImage, false);
                    window.location = "/Home/ProfileDetails";
                    
                }
                if (response == success) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
                    ShowMessageNotification("error", uploadFile, false, true, LocationReload);
                    //window.location = "/Home/ProfileDetails";
                    
                }
                if (response == wrongFormat) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
                    ShowMessageNotification("success", allowedFormats, false);
                    window.location = "/Home/ProfileDetails"; 
                    
                    $('#file').val("");
                }
            },
            error: function (xhr, status, error) {
            }
        });
    } else {
        ShowMessageNotification("success", specifyFile, false);
        
    }
});
// Validate file type for the specific file type are allowed
$("#file").change(function () {

    if ($('#file').val() != "") {
        var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            ShowMessageNotification("success", allowedFormatsOnly + fileExtension.join(', '), false);
            
            $('#file').val("");
        }
    }
});
// Open Rest password popup
$('.ResetPassword').click(function () {

    var userId = $("#UserId").val();
    var EmailAddress = $("#EmailAddress").val();

    var QueryString = "UserId:" + userId + "@#$EmailAddress:" + EmailAddress;
    $.ajax({
        type: 'GET',
        url: '/Home/ResetPassword?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#ResetPasswordModalMain").html(data);
            DraggableModalPopup("#ResetPasswordModal");
        }
    });
    return false;
});
function backToparent() {
    $("#ResetPasswordModal").modal("hide");
    location.href = "/Account/LogOff";
}
//when click on Remove Image  method will call and remove image
$("body").on('click', '#btRemoveImage', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: removeImage, callback: function (result) {
            if (result) {
                var ImgPath = $("#Imagepath").val();
                var userName = $("#LoginId").val();
                var userId = $("#UserId").val();
                var formData = new FormData();
                formData.append("UserId", userId);
                formData.append("UserName", userName);
                formData.append("Imgpath", ImgPath);
                $.ajax({
                    type: "POST",
                    url: '/Home/RemoveImage',
                    data: formData,
                    headers: { "__RequestVerificationToken": token },
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    async: false,
                    success: function (response) { },
                    error: function (xhr, status, error) { }
                });
                window.location.href = window.location.href;
            }
        }
    });
});
$("body").on('focus', '#SecurityAnswer', function () {
    $("#SecurityAnswer").val('');
    $(".updateSecurityAnswer").prop("disabled", false);
});

// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyUp Event)
$("#SecurityAnswer").keyup(function (e) {

    var strtext = $(this).val();
    var strvalidation = new RegExp('<[^>]*>');
    if (strvalidation.test(strtext)) {
        $(this).val('');
    }

});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for Chane Event)
$("#SecurityAnswer").on('change', function (event, node) {
    var strtext = $(this).val();
    var strvalidation = new RegExp('<[^>]*>');
    if (strvalidation.test(strtext)) {
        $(this).val('');
    }
});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyPress Event)
$("#SecurityAnswer").keypress(function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode != 47) && (keyCode != 60) && (keyCode != 62));
    return ret;

});
$("body").on('focus', '#SecurityAnswer', function () {
    $("#SecurityAnswer").attr('readonly', false);
});
