$('body').on('click', '#btnAddDUNSregistration', function () {
    var SortBy = $("#RegistrationSortBy").val();
    var SortOrder = $("#RegistrationSortOrder").val();
    var pagevalue = $("#RegistrationPageValue").val();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/MonitorProfile/CreateMonitoringRegistration'
        },
        callbacks: {
            close: function () {
                var url = '/MonitorProfile/IndexMonitoringRegistration/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "HTML",
                    contentType: "application/html",
                    async: false,
                    success: function (data) {
                        $("#divPartialDUNSregistrationData").html(data);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupMonitoringRegistration'
    });
});



// for the pagination event 








function LoaderProgress()
{
    $("#divProgress").show();
}