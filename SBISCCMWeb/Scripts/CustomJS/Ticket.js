$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});
// Manage add event for the Ticket
$('body').on('click', '#btnAddTicket', function () {
    $.ajax({
        type: "GET",
        url: "/Ticket/Create/",
        data: '',
        dataType: "html",
        contentType: "application/json",
        success: function (data) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
// for the pagination event 
$("body").on('change', '.pagevalueChange', function () {

    var pagevalue = $(this)[0].value;
    var SortBy = $("#TicketSortBy").val();
    var SortOrder = $("#TicketSortOrder").val();
    var url = '/Ticket/Index/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache:false,
        success: function (data) {
            $("#divTicketPortal").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function OnSuccess() {
}
// Close Ticket 
$("body").on('click', '.TicketClose', function () {
    var id = $(this).attr('id');
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: closeTicket, callback: function (result) {
            if (result) {
                var pageNo = 0;
                var pageSize = $(".pagevalueChange")[0].value;
                $(".pagination li").each(function () {
                    if ($(this).hasClass("active")) {
                        pageNo = $(this).children().first().html();
                    }
                })
                $('#divProgress').show();
                $.ajax({
                    type: "POST",
                    url: "/Ticket/CloseTicket/",
                    data: JSON.stringify({ id: id, pageNo: pageNo, pageSize: pageSize }),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "html",
                    contentType: "application/json",
                    success: function (data) {
                        $("#divTicketPortal").html(data);
                        $('#divProgress').hide();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $('#divProgress').hide();
                    }
                });
            }
        }
    });
});

// Edit Ticket
$("body").on('click', '.editTicket', function () {
    var id = $(this).attr("data-val");

    var url = "/Ticket/Edit?Parameters=" + ConvertEncrypte(encodeURI(id)).split("+").join("***");
    window.location.href = url;

});