$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
$(document).ready(function () {
    $('[data-toggle="popover"]').popover();   
    $('#MultiSelectOptions').multiselect({
        includeSelectAllOption: true,
        numberDisplayed: 6,
        allSelectedText: "",
        nonSelectedText: selectOutputQueue,

    });

    var UserType = $("#UserType").val();
    var StwerdTags = $("#StwerdTags").val();

    // $("#btnExportData").hide();
    var ExportTimeInterval = $("#ExportTimeInterval").val();

    var timeinterval = ExportTimeInterval * 60 * 1000;
    $('#divProgress').hide();
    setInterval(function () {
        //ExportListAutoRefresh();
    }, timeinterval);

        if ($.UserRole != undefined && ($.UserRole.toLowerCase() == "lob")) {
            $("#LOBTag").val($.UserLOBTag);
            $("#LOBTag").attr("disabled", true);
        }
    if (UserType.toLowerCase() == "steward" && StwerdTags != "") {
        var StwTags = StwerdTags.split(",");
        $("#ExportDatafrm #Tag option").each(function () {
            var isDelete = true;
            for (var i = 0; i < StwTags.length; i++) {
                if ($(this).val() == StwTags[i]) {
                    isDelete = false;
                }
            }
            if (isDelete) {
                $("#ExportDatafrm #Tag option[value='" + $(this).val() + "']").remove();
            }
        });
    }

        if ($.UserRole != undefined && ($.UserRole.toLowerCase() == "lob" || UserType.toLowerCase() == "steward")) {
            jQuery("#DisplayFileType option:contains('All Files')").remove();
            jQuery("#MonitoringDisplayFileType option:contains('All Files')").remove();
        }
    $("select.chzn-select").chosen({
        no_results_text: "Oops, nothing found!",
        width: "100%",
        search_contains: true
    });
    InitPortalExportCompayData();
});
var dialog;
//export Match Output data
//export Monitoring Notifications data
$('body').on('click', '.MonitoringclsFileType', function () {

    var MonitoringOutputcount = $("#MonitoringOutputcount").val();
    if (MonitoringOutputcount != 0) {
        var MonitoringDataExport = false;
        var ApiName = $("#MonitoringApiName").val();
        var FileType = $(this).attr("data-filetype");
        var token = $('input[name="__RequestVerificationToken"]').val();
        setTimeout(function () {
            $(".bootbox-close-button").show();
            $(".bootbox-close-button").addClass("closeBootbox");
            $(".bootbox-close-button").removeClass("bootbox-close-button");
        }, 100);
        dialog = bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: markDataAsExported, callback: function (result) {
                MonitoringDataExport = result;
                if (result) {
                    $(".MonitoringOutputcnt").attr("data-original-title", "0");
                    $("#MonitoringOutputcnt").html(0);
                    $("#MonitoringOutputcount").val("0");
                }
                var Parameters = "MonitoringDataExport:" + MonitoringDataExport + "@#$ApiName:" + ApiName + "@#$FileType:" + FileType;
                var Encrypted = ConvertEncrypte(encodeURI(Parameters)).split("+").join("***");
                $("#frmMonitoring").children().remove();
                $('<input>').attr({ type: 'hidden', name: "Parameters", value: Encrypted }).appendTo('#frmMonitoring');
                $("#frmMonitoring").submit();
            }

        });
    }
    else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", "Data not found.", false);
    }
});
function ShowCloseBtn() {
    setTimeout(function () {
        $(".bootbox-close-button").show();
        $(".bootbox-close-button").addClass("closeBootbox");
        $(".bootbox-close-button").removeClass("bootbox-close-button");
    }, 3000);


}
$('body').on('change', '.closeBootbox', function () { dialog.modal('hide'); });
 
$("body").on("change", "#chkMatchOutput", function () {
    if ($(this).prop("checked")) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});
$("body").on("change", "#chkEnrichment", function () {
    if ($(this).prop("checked")) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});
$("body").on("change", "#chkActiveDataQueue", function () {
    if ($(this).prop("checked")) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});
$("body").on("change", "#rdMarkAsExported", function () {
    if ($(this).prop("checked")) {
        $(this).attr("value", "true");
    } else {
        $(this).attr("value", "false");
    }
});
$("body").on("click", "#btnExportData", function () {
    ValidationProcess();
});
function ValidationProcess() {
    var cntErr = 0;
    var OutputOptions = $("#MultiSelectOptions").val();
    var Text = $("#rdText").prop("checked");
    if (OutputOptions != undefined && OutputOptions != null && OutputOptions.length > 0) {
        $("#spnProcess").hide();
    }
    else {
        $("#spnProcess").show();
        cntErr++;
    }
    if ($("input:radio[name='Format']:checked").length > 0) {
        $("#spnFormat").css("display", "none");
    }
    else {
        $("#spnFormat").css("display", "block");
        cntErr++;
    }
    if (cntErr > 0) {
        return false;
    }
    if (Text) {
        var Delimiter = $("#Delimiter").val().trim();
        if (Delimiter == "") {
            OpenDelimiter();
            return false;
        }
    }
    var SrcRecID = $("#txtSrcRecID").val().trim();
    var SrcRecIdExactMatch = $("#SrcRecIdExactMatch").prop("checked");
    var LOBTag = $("#LOBTag").val();
    var Tag = $("#Tags").val();
    var ImportPorcess = $("#Input").val();
    var output = $("#MultiSelectOptions option:selected").map(function () { return this.text }).get().join(', ');
    var outputFormat = $('input[name=Format]:checked').val();
    var MarkAsExported = $('#MarkAsExported').prop('checked');
    var Note = noFilterSelected;
    var Message = "<table class='ExportConfirmBox'>";
    if (SrcRecID != "") {
        Message += "<tr><td><strong><span>" + srcRecordId + "</span></strong></td><td>&nbsp:&nbsp</td><td> <span style='word-wrap: anywhere;'>" + SrcRecID + "</span> <strong><span>" + isExactMatch + "</span></strong> : " + SrcRecIdExactMatch + "</td><tr>";
    }
    if (LOBTag != undefined && LOBTag != "") {
        Message += "<tr><td><strong><span>" + lobTag + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + LOBTag + "</td><tr>";
    }
    if (LOBTag != undefined && Tag != "") {
        Message += "<tr><td><strong><span>" + tag + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + Tag + "</td><tr>";
    }
    if (ImportPorcess != "") {
        Message += "<tr><td><strong><span>" + importPorcess + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + ImportPorcess + "</td><tr>";
    }

    Message += "<tr><td><strong><span>" + selectedOutput + "</span ></strong ></td > <td>&nbsp:&nbsp</td> <td> " + output + "</td> <tr>";
    Message += "<tr><td><strong><span>" + outputFormat + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + outputFormat + "</td><tr>";
    if (Text) {
        Message += "<tr><td><strong><span>" + delimiterValue + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + Delimiter + "</td><tr>";
    }
    Message += "<tr><td><strong><span>" + markAsExported + "</span></strong></td><td>&nbsp:&nbsp</td><td>" + MarkAsExported + "</td><tr>";
    if (SrcRecID == "" && (LOBTag == "" || LOBTag == undefined) && (Tag == "" || Tag == undefined) && ImportPorcess == "") {
        Message += "<tr><td></tr></td><tr><td colspan='5'><span class='error'>" + Note + "</span></td></tr>";
    }
    Message += "</table>";
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: Message, buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn btn-primary btnYes'
            },
            cancel: {
                label: 'No',
                className: 'btn btn-default'
            }
        }, callback: function (result) {
            if (result) {
                $('.btnYes').prop('disabled', true);
                var Parameters = ConvertEncrypte(encodeURI(SrcRecID)).split("+").join("***");
                $.ajax({
                    type: 'GET',
                    url: '/ExportView/ExportFileName?Parameters=' + Parameters,
                    dataType: 'HTML',
                    async: false,
                    success: function (data) {
                        $("#ExportFileNameModalMain").html(data);
                        DraggableModalPopup("#ExportFileNameModal");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
}
function OnSuccess() {

    $("#divProgress").hide();
    $('[data-toggle="popover"]').popover();  
    var UserType = $("#UserType").val();
        if ($.UserRole != undefined && ($.UserRole.toLowerCase() == "lob" || UserType.toLowerCase() == "steward")) {
            jQuery("#DisplayFileType option:contains('All Files')").remove();
            jQuery("#MonitoringDisplayFileType option:contains('All Files')").remove();
        }
    }
$("body").on('change', '.DisplayFileType', function () {
    var FileType = $(this)[0].value;
    var QueryString = "FileType:" + FileType;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: "GET",
        url: "/ExportView/LoadExportStatus?Parameters=" + Parameters,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divExportJobListing").html(data);
            $('[data-toggle="popover"]').popover();
            $('table#ExportJobs tbody tr:first').addClass('current');
            InitPortalExportCompayData();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
$("body").on('click', '.deleteFile', function () {
    var FileType = $("#FileType").val();
    var urlIndex = '/ExportView/Index?FileType=' + FileType;

    var id = $(this).attr('id');
    var url = '/ExportView/Delete?Id=' + id;
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecord, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    contentType: "application/json",
                    async: false,
                    cache: false,
                    success: function (data) {
                            RefreshExportList(urlIndex);
                    },
                    error: function (xhr, ajaxOptions, thrownError) { }
                });
            }
        }
    });


});
$("body").on('click', '.OpenDelimiter', function () {
    OpenDelimiter();
});
function OpenDelimiter() {
    var Delimiter = $("#Delimiter").val();
    $.ajax({
        type: 'GET',
        url: '/ExportView/Delimiter?Parameters=' + ConvertEncrypte(encodeURI(Delimiter)).split("+").join("***"),
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#CustomExportDelimiterModalMain").html(data);
            DraggableModalPopup("#CustomExportDelimiterModal");
        }
    });
}
function SetDownloadValue() {

    var id = $("#lnkDownload").attr('data-id');
    $.ajax({
        type: "POST",
        url: "/ExportView/SetDownloadValue/",
        data: JSON.stringify({ Id: id }),
        dataType: "json",
        contentType: "application/json",
        cache: false,
        async: false,
        success: function (data) {
            if (data == "Success") {
                return true;
            }
            return false;
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function ExportListAutoRefresh() {
    var FileType = $("#FileType").val();
    var url = '/ExportView/Index?FileType=' + FileType;
    RefreshExportList(url);
}
function ExportSubmitProcess(filename) {
    var selected = $("#MultiSelectOptions option");
    selected.each(function () {
        var checkedItem = "#" + $(this).val();
        $(checkedItem).val(this.selected);
    });
    $("#FilePath").val(filename);
    $("#ExportDatafrm").submit();
}
$("body").on('click', '#btnReExportData', function () {

    $.ajax({
        type: 'GET',
        url: '/ExportView/ReExportFile',
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#ReExportFileModalMain").html(data);
            DraggableModalPopup("#ReExportFileModal");
        }
    });
});
$("body").on('click', '#btnMonitoringNotificationsExportData', function () {
    var MarkAsExported = $('#MarkAsMonitoringExported').prop('checked');
    var IsHeader = $('#MonitoringIsHeader').prop('checked');
    var FileFormat = $('input[name=FileMonitoringFormat]:checked').val();
    var APIName = $("#MonitoringApiName").val();
    var Delimiter = $("#Delimiter").val();
    var MonitoringHasTextQualifierToAll = $("#MonitoringHasTextQualifierToAll").prop('checked');
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (FileFormat.toLowerCase() == "delimiter") {
        if (Delimiter == "") {
            OpenDelimiter();
            return false;
        }
    }

    var Message = "<table class='ExportConfirmBox'>";
    Message += "<tr><td><strong><span>" + apiLayer + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + APIName + "</td><tr>";
    Message += "<tr><td><strong><span>" + outputFormat + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + FileFormat + "</td><tr>";

    if (FileFormat.toLowerCase() == "delimiter") {
        Message += "<tr><td><strong><span>" + delimiterValue + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + Delimiter + "</td><tr>";
    }
    Message += "<tr><td><strong><span>" + markAsExported + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + MarkAsExported + "</td><tr>";
    Message += "</table>";
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: Message, callback: function (result) {
            if (result) {
                Parameters = "MarkAsExported:" + MarkAsExported + "@#$FileFormat:" + FileFormat + "@#$APIName:" + APIName + "@#$Delimiter:" + Delimiter + "@#$IsHeader:" + IsHeader + "@#$MonitoringHasTextQualifierToAll:" + MonitoringHasTextQualifierToAll;
                $.ajax({
                    type: "POST",
                    url: "/ExportView/ExportToMonitoring/",
                    headers: { "__RequestVerificationToken": token },
                    data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(Parameters)).split("+").join("***") }),
                    dataType: "json",
                    contentType: "application/json",
                    cache: false,
                    async: false,
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        ShowMessageNotification("success", data, false);
                        if (data.indexOf('data request has been submitted successfully') > 0) {
                            loadMonitoringList();
                            InitPortalExportMonitoringData();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }

                });
            }
        }
    });

});
$("body").on('change', '#MonitoringDisplayFileType', function () {
    var FileType = $(this)[0].value;
    var QueryString = "FileType:" + FileType;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: "GET",
        url: "/ExportView/LoadMonitoringExportStatus?Parameters=" + Parameters,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divMonitoringExportJobListing").html(data);
            InitPortalExportMonitoringData();
            pageSetUp();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function callbackReExportData() {
    $("#ReExportFileModal").modal("hide");
    location.reload();
}
function callbackCloseDelimiter(data) {
    $("#CustomExportDelimiterModal").modal("hide");
    $("#Delimiter").val(data);
}
$("body").on('click', 'input:radio[name=FileFormat]', function () {
    if ($(this).val().trim().toLowerCase() != "delimiter") {
        $("#Delimiter").val("");
    }
});
$("body").on('click', 'input:radio[name=FileMonitoringFormat]', function () {
    if ($(this).val().trim().toLowerCase() != "delimiter") {
        $("#Delimiter").val("");
    }
});
$("body").on('click', '.CancelFile', function () {
    var FileType = $("#FileType").val();
    var urlIndex = '/ExportView/Index?FileType=' + FileType;
    var id = $(this).attr('id');
    CancelExportREquest(id, urlIndex, true, FileType);
});
$("body").on('click', '.CancelMoniroringFile', function () {
    var FileType = $("#MonitoringFileType").val();
    var url = '/ExportView/MonitoringExportindex?MonitoringFileType=' + FileType;
    var id = $(this).attr('id');
    CancelExportREquest(id, url, false, FileType);
});

function CancelExportREquest(id, urlIndex, isExport, FileType) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var url = '/ExportView/CancelExportProcess?Id=' + id;
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: cancelExportDataRequest, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: url,
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    async: false,
                    cache: false,
                    success: function (data) {
                        if (data == success) {
                            if (isExport) {
                                RefreshExportList(urlIndex);
                            }
                            else {
                                RefreshExportMonitoringList(urlIndex);
                            }
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                    }
                });
            }
        },
    });
}

function RefreshExportList(urlIndex) {
      loadCompanyDataList();
      InitPortalExportCompayData();
}
function RefreshExportMonitoringList(urlIndex) {
    loadMonitoringList();
    InitPortalExportMonitoringData();
}
function UpdateExportCompany(data) {
    if (data.result) {
        var FileType = $("#FileType").val();
        var urlIndex = '/ExportView/Index?FileType=' + FileType;
        RefreshExportList(urlIndex);
    }
    ShowMessageNotification("success", data.message, false);

}

function UpdateMonitoringExportCompany(data) {
    if (data.result) {
        var FileType = $("#MonitoringFileType").val();
        var urlIndex = '/ExportView/MonitoringExportindex?MonitoringFileType=' + FileType;
        RefreshExportMonitoringList(urlIndex);
    }
    ShowMessageNotification("success", data.message, false);
}
$('body').on('click', '#IdExportDataTab', function () {
    if (!$(this).parent("li").hasClass("active")) {
        loadCompanyDataList();
    }
});
$('body').on('click', '#IdMonitoringExportDataTab', function () {
    if (!$(this).parent("li").hasClass("active")) {
        loadMonitoringList();
    }
});
function loadCompanyDataList() {
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/Export/CompanyData", container: '#ExportDataTab',
        timeout: 50000
    }).done(function () {
        $('table#ExportJobs tbody tr:first').addClass('current');
        $('#MultiSelectOptions').multiselect({
            includeSelectAllOption: true,
            numberDisplayed: 6,
            allSelectedText: "",
            nonSelectedText: selectOutputQueue
        });
        InitPortalExportCompayData();
        OnSuccess();
    });
}
function loadMonitoringList() {   
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/Export/MonitoringData", container: '#MonitoringExportDataTab',
        timeout: 50000
    }).done(function () {
        $('table#ExportMonitoringJobs tbody tr:first').addClass('current');
        InitPortalExportMonitoringData();
        OnSuccess();
    });
}
function InitPortalExportCompayData() {
    $.fn.dataTable.moment('MM/DD/YYYY HH:mm:ss');
    InitDataTable("#ExportJobs", [10, 20, 30], false, [1, "desc"]);
}
function InitPortalExportMonitoringData() {
    InitDataTable("#ExportMonitoringJobs", [10, 20, 30], false, [1, "desc"]);
}