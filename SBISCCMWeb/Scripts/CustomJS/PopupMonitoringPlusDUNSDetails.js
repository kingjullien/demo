$("body").on("click", "#btnExportDUNS", function () {
    var RegistrationName = $("#RegistrationNamehidden").val();

    $("#RegistrationName").val(RegistrationName);
    $("#frmExportDUNS").submit();

});
$("body").on("click", "#btnAddDUNS", function () {
    var RegistrationsName = $("#RegistrationNamehidden").val();
    var AuthToken = $("#AuthToken").val();
    var QueryString = "RegistrationsName:" + RegistrationsName + "@#$AuthToken:" + AuthToken;
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/DNBMonitoringDirectPlus/AddDUNSMonitoringPlus?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#AddDUNSMonitoringPlusModalMain").html(data);
            DraggableModalPopup("#AddDUNSMonitoringPlusModal");
            loadAddDUNSMonitoringPlus();
        }
    });
});
$("body").on("click", "#btnRemoveDUNS", function () {
    var RegistrationsName = $("#RegistrationNamehidden").val();
    var AuthToken = $("#AuthToken").val();
    var QueryString = "RegistrationsName:" + RegistrationsName + "@#$AuthToken:" + AuthToken;
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/DNBMonitoringDirectPlus/RemoveDUNSMonitoringPlus/" + "?Parameters=" + parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#RemoveDUNSMonitoringPlusModalMain").html(data);
            DraggableModalPopup("#RemoveDUNSMonitoringPlusModal");
            loadRemoveDUNSMonitoringPlus();
        }
    });
});

function myFunction() {
    var DnBDUNSNumber = $("#myInput").val();
    var DUNSDetailsPagevalue = $("#DUNSDetailsPagevalue").val();
    var RegistrationName = $("#RegistrationNamehidden").val();
    var DUNSDetailsSortby = $("#DUNSDetailsSortby").val();
    var DUNSDetailsSortorder = $("#DUNSDetailsSortorder").val();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/DNBMonitoringDirectPlus/MonitoringPlusRegistrationDUNSDetails?DnBDUNSNumber=" + DnBDUNSNumber + "&DUNSDetailsSortby=" + DUNSDetailsSortby + "&DUNSDetailsSortorder=" + DUNSDetailsSortorder + "&DUNSDetailsPagevalue=" + DUNSDetailsPagevalue + "&RegistrationName=" + RegistrationName,
        dataType: 'HTML',
        contentType: 'application/html',
        async: false,
        success: function (data) {
            $("#MonitoringPlusDUNSDetails").html(data);
        }
    });
}

$(document).on("change", "#DUNSPageValue", function () {
    var DnBDUNSNumber = $("#myInput").val();
    var DUNSDetailsPagevalue = $(this).val();
    var RegistrationName = $("#RegistrationNamehidden").val();
    var DUNSDetailsSortby = $("#DUNSDetailsSortby").val();
    var DUNSDetailsSortorder = $("#DUNSDetailsSortorder").val();
    $.ajax({
        type: 'GET',
        cache: false,
        url: "/DNBMonitoringDirectPlus/MonitoringPlusRegistrationDUNSDetails?DnBDUNSNumber=" + DnBDUNSNumber + "&DUNSDetailsSortby=" + DUNSDetailsSortby + "&DUNSDetailsSortorder=" + DUNSDetailsSortorder + "&DUNSDetailsPagevalue=" + DUNSDetailsPagevalue + "&RegistrationName=" + RegistrationName,
        dataType: 'HTML',
        contentType: 'application/html',
        async: false,
        success: function (data) {
            $("#MonitoringPlusDUNSDetails").html(data);
        }
    });
});
function AddDUNSMonitoringPlus(data) {
    ShowMessageNotification("success", data.message);
    if (data.result) {
        $("#AddDUNSMonitoringPlusModal").modal("hide");
    }
}
function RemoveDUNSMonitoringPlus(data) {
    ShowMessageNotification("success", data.message);
    if (data.result) {
        $("#RemoveDUNSMonitoringPlusModal").modal("hide");
    }
}