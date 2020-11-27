//Tab Change Event
$('body').on('click', '#IdRTabCommandUpload', function () {
    $("#IdRTabCommandUpload").addClass("tabBorder");
    $("#IdRTabCommandDownload").removeClass("tabBorder");
    $("#IdRTabCommandDownloadSetup").removeClass("tabBorder");
    $("#RTabCommandDownload").hide();
    $("#RTabCommandDownloadSetup").hide();
    $("#RTabCommandUpload").show();
    if (!$(this).parent("li").hasClass("active")) {
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/Portal/UploadConfiguration", container: '#divPartialCommandMapping',
            timeout: 50000
        }).done(function () {
            InitPortalUploadConfigurationDataTable();
            if ($("table#tbCmndMapping tbody tr").length == 0) {
                $("#EditCommandMapping").attr("disabled", true);
            }
            else {
                $("table#tbCmndMapping tbody tr:first").addClass("current");
            }
        });
    }
});

$('body').on('click', '#IdRTabCommandDownloadSetup', function () {
    $("#IdRTabCommandDownloadSetup").addClass("tabBorder");
    $("#IdRTabCommandUpload").removeClass("tabBorder");
    $("#IdRTabCommandDownload").removeClass("tabBorder");
    $("#RTabCommandUpload").hide();
    $("#RTabCommandDownload").hide();
    $("#RTabCommandDownloadSetup").show();
    $('[data-toggle="popover"]').popover();
    $("#IdRTabCommandUpload").parent("li").removeClass("active");
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/Portal/EXESetup", container: '#divPartialCommandDownloadSetup',
        timeout: 50000
    }).done(function () {            
    });
});

$('body').on('click', '#IdRTabCommandDownload', function () {
    $("#IdRTabCommandDownload").addClass("tabBorder");
    $("#IdRTabCommandDownloadSetup").removeClass("tabBorder");
    $("#IdRTabCommandUpload").removeClass("tabBorder");
    $("#RTabCommandUpload").hide();
    $("#RTabCommandDownloadSetup").hide();
    $("#RTabCommandDownload").show();
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabCommandUpload").parent("li").removeClass("active");
        var url = "/CommandMapping/IndexCommandDownload";
        LoadCommandDownloadList(url);
    }
});

//upload Mapping

$("body").on('click', '#AddCommandMapping', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/CommandMapping/CreateCommandMapping",
        dataType: 'HTML',
        success: function (data) {
            $("#UploadConfigurationModalMain").html(data);
            DraggableModalPopup("#UploadConfigurationModal");
        }
    });
});

$("body").on('click', '.editCommanduploadMapping', function () {
    // Changes for Converting magnific popup to modal popup
    var Id = $(this).attr("id");
    if (Id == undefined) {
        Id = 0;
    }
    $.ajax({
        type: 'GET',
        url: "/CommandMapping/CreateCommandMapping?Parameters=" + ConvertEncrypte(encodeURI(Id)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#UploadConfigurationModalMain").html(data);
            DraggableModalPopup("#UploadConfigurationModal");
        }
    });
});

$('body').on('click', '.deleteCommanduploadMapping', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/CommandMapping/DeleteCommanduploadMapping?Parameters=" + ConvertEncrypte(encodeURI(Parameters)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        ShowMessageNotification("success",data, false);
                        LoadList();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});

$("body").on('change', '.pagevalueChange', function () {
    LoadList();
});

$("body").on('click', 'table#tbCmndMapping tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("table#tbCmndMapping tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
    }
});

function LoadList() {
    $.pjax({
        url: "/Portal/UploadConfiguration", container: '#divPartialCommandMapping',
        timeout: 50000
    }).done(function () {
        InitPortalUploadConfigurationDataTable();
        if ($("table#tbCmndMapping tbody tr").length == 0) {
            $("#EditCommandMapping").attr("disabled", true);
        }
        else {
            $("table#tbCmndMapping tbody tr:first").addClass("current");
        }
    });
}

function OnSuccess() {
    $("#divProgress").hide();
    pageSetUp();
    if ($("table#tbCmndMapping tbody tr").length == 0) {
        $("#EditCommandMapping").attr("disabled", true);
    }
    else {
        $("table#tbCmndMapping tbody tr:first").addClass("current");
    }
}
//End upload Mapping
//End Tab Change Event

//Download Mapping
$("body").on('click', '#AddCommandDownloadMapping', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/CommandMapping/InsertUpdateCommandDownload",
        dataType: 'HTML',
        success: function (data) {
            $("#DownloadConfigurationModalMain").html(data);
            DraggableModalPopup("#DownloadConfigurationModal");
        }
    });
});

$("body").on("click", ".editCommandDownloadMapping", function () {
    // Changes for Converting magnific popup to modal popup
    var Id = $(this).attr("id");
    if (Id == undefined) {
        Id = 0;
    }
    $.ajax({
        type: 'GET',
        url: "/CommandMapping/InsertUpdateCommandDownload?Parameters=" + ConvertEncrypte(encodeURI(Id)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#DownloadConfigurationModalMain").html(data);
            DraggableModalPopup("#DownloadConfigurationModal");
        }
    });
});

$('body').on('click', '.deleteCommandDownloadMapping', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var Parameters = $(this).attr("id");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/CommandMapping/DeleteCommandDownload?Parameters=" + ConvertEncrypte(encodeURI(Parameters)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        ShowMessageNotification("success",data, false);
                        var url = '/CommandMapping/IndexCommandDownload/';
                        LoadCommandDownloadList(url);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});

$("body").on('change', '.pagevalueDownloadChange', function () {
    var url = '/CommandMapping/IndexCommandDownload/';
    LoadCommandDownloadList(url);
});

$("body").on('click', 'table#tbCmndDownloadMapping tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("table#tbCmndDownloadMapping tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
    }
});

function LoadCommandDownloadList(url) {
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/Portal/DownloadConfiguration", container: '#divPartialCommandDownloadList',
        timeout: 50000
    }).done(function () {
        InitPortalDownloadConfigurationDataTable();
        pageSetUp();

        if ($("table#tbCmndDownloadMapping tbody tr").length == 0) {
            $("table#tbCmndDownloadMapping").attr("disabled", true);

        }
        else {
            $("table#tbCmndDownloadMapping tbody tr:first").addClass("current");

        }
    });
}
//End Download Mapping
function InitPortalUploadConfigurationDataTable() {
    InitDataTable("#tbCmndMapping", [10,20,30], false,[0,"desc"]);
}
function InitPortalDownloadConfigurationDataTable() {
    InitDataTable("#tbCmndDownloadMapping", [10, 20, 30], false, [0, "desc"]);
}       