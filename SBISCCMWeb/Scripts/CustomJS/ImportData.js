// Insert data Open popup for Insert Data and make ajax call for match data.
$("body").on('click', '#btnSubmit', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var formData = new FormData();
    if ($('#file')[0].files[0] != undefined) {
        formData.append('file', $('#file')[0].files[0]);
        formData.append('header', $('#WithHeader').prop('checked'));
        $.ajax({
            type: "POST",
            url: '@Url.Action("SaveFile", "Data")',
            data: formData,
            headers: { "__RequestVerificationToken": token },
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                $.magnificPopup.open({
                    preloader: false,
                    closeBtnInside: true,
                    type: 'iframe',
                    items: {
                        src: '/Data/DataMatch/'
                    },
                    callbacks: {
                        close: function () {
                        }
                    },
                    closeOnBgClick: false,
                    mainClass: 'popupData'
                });
            },
            error: function (xhr, status, error) {
            }
        });
    } else {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
            message: requiredFile,
        });
    }
    return false;
});
// check file extension if extension valida or not.
$("#file").change(function () {
    var fileExtension = ['xls', 'xlsx', 'csv'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
            message: "Please select file first.",
        });
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
            message: "Only formats allowed are : " + fileExtension.join(', '),
        });
        $('#file').val("");
    }
});