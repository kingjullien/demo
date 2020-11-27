$('body').on('click', '#btnAddNotificationProfile', function () {
    var SortBy = $("#NotificationSortBy").val();
    var SortOrder = $("#NotificationSortOrder").val();
    var pagevalue = $("#NotificationPageValue").val();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/MonitorProfile/CreateNotificationProfile'
        },
        callbacks: {
            close: function () {
                var url = '/MonitorProfile/IndexNotificationProfile/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "HTML",
                    contentType: "application/html",
                    async: false,
                    success: function (data) {
                        $("#divPartialNotificationProfileData").html(data);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupAddNotification'
    });
});

$('body').on('click', '.EditNotificationProfile', function () {
    var SortBy = $("#NotificationSortBy").val();
    var SortOrder = $("#NotificationSortOrder").val();
    var pagevalue = $("#NotificationPageValue").val();
    var id = $(this).attr("id");
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/MonitorProfile/CreateNotificationProfile?Parameters=' + ConvertEncrypte(encodeURI(id)).split("+").join("***"),
        },
        callbacks: {
            close: function () {
                var url = '/MonitorProfile/IndexNotificationProfile/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "HTML",
                    contentType: "application/html",
                    async: false,
                    success: function (data) {
                        $("#divPartialNotificationProfileData").html(data);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupAddNotification'
    });
});

// for the pagination event 





function LoaderProgress() {
    $("#divProgress").show();
}