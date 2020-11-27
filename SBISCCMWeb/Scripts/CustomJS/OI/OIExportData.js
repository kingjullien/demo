// Display and Hide loader bar for every ajax call
$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});
$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
$(document).ready(function () {
    $("#MultiSelectOptions option").each(function () {
        if ($(this).val() == "MatchOutput" || $(this).val() == "Enrichment") {
            $(this).attr("selected", true)
        }
    });
    $('#MultiSelectOptions').multiselect({
        includeSelectAllOption: true,
        numberDisplayed: 6,
        allSelectedText: "",
        nonSelectedText: selectOutputQueue,

    });
    if ($.UserRole.toLowerCase() == "lob") {
        $("#LOBTag").val($.UserLOBTag);
        $("#LOBTag").attr("disabled", true);
    }
    $("select.chzn-select").chosen({
        no_results_text: nothingFound,
        width: "100%",
        search_contains: true
    });
    InitPortalOIExportCompayData();
});
$("body").on("click", "#btnOIExportData", function () {

    ValidationProcess();
});
function ValidationProcess() {
    var Excel = $("#rdExcel").prop("checked");
    var CSV = $("#rdCSV").prop("checked");
    var TSV = $("#rdTSV").prop("checked");
    var Text = $("#rdText").prop("checked");
    var SrcRecID = $("#txtSrcRecID").val().trim();
    var QueryString = SrcRecID;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    countProcess = 0;
    if ($("#MultiSelectOptions option:selected").length == 0) {
        countProcess++;
    }
    if (countProcess == 1) {
        $("#spnProcess").css("display", "block");

    } else {
        $("#spnProcess").css("display", "none");
    }
    var countFormat = 0;
    if (Excel == false) {
        countFormat++;
    }
    if (CSV == false) {
        countFormat++;
    }
    if (TSV == false) {
        countFormat++;
    }
    if (Text == false) {
        countFormat++;
    }
    if (countFormat == 4) {
        $("#spnFormat").css("display", "block");

    } else {
        $("#spnFormat").css("display", "none");
    }
    if (countProcess == 1 || countFormat == 4) {
        return false;
    }
    if (Text) {
        var Delimiter = $("#Delimiter").val().trim();
        if (Delimiter == "") {
            OpenDelimiter();
            return false;
        }
    }

    var SrcRecIdExactMatch = $("#SrcRecIdExactMatch").prop("checked");
    var LOBTag = $("#LOBTag").val();
    var Tag = $("#Tags").val();
    var ImportPorcess = $("#Input").val();
    var output = $("#MultiSelectOptions option:selected").map(function () { return this.text }).get().join(', ');
    output = output.replace("Enrichment", "Firmographics");
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
    Message += "<tr><td><strong><span>" + selectedOutput + "</span></strong></td><td>&nbsp:&nbsp</td><td> " + output + "</td><tr>";
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
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: Message, callback: function (result) {
            if (result) {

                $.ajax({
                    type: 'GET',
                    url: '/OIExportView/ExportFileName?Parameters=' + Parameters,
                    dataType: 'HTML',
                    async: false,
                    success: function (data) {
                        $("#ExportFileNameModalMain").html(data);
                        DraggableModalPopup("#ExportFileNameModal");
                    }
                });
            }
        }
    });
}

function ExportSubmitProcess(filename) {
    $("#ExportFileNameModal").modal("hide");
    var selected = $("#MultiSelectOptions option");
    selected.each(function () {
        var checkedItem = "#" + $(this).val();
        $(checkedItem).val(this.selected);
    });
    $("#FilePath").val(filename);
    $("#OIExportDatafrm").submit();
}
$("body").on('click', '.OpenDelimiter', function () {
    OpenDelimiter();
});
function OpenDelimiter() {
    var Delimiter = $("#Delimiter").val();
    $.ajax({
        type: 'GET',
        url: '/OIExportView/Delimiter?Parameters=' + ConvertEncrypte(encodeURI(Delimiter)).split("+").join("***"),
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#CustomExportDelimiterModalMain").html(data);
            DraggableModalPopup("#CustomExportDelimiterModal");
        }
    });
}
function callbackCloseDelimiter(data) {
    $("#CustomExportDelimiterModal").modal("hide");
    $("#Delimiter").val(data);
}
$("body").on('click', 'input:radio[name=Format]', function () {
    if ($(this).val().trim().toLowerCase() != "delimiter") {
        $("#Delimiter").val("");
    }
});

$("body").on('click', '.deleteFile', function () {
    var FileType = $("#FileType").val();
    var urlIndex = '/OI/ExportView?FileType=' + FileType;

    var id = $(this).attr('id');
    var url = '/OIExportView/Delete?Id=' + id;
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: DeleteExportRecord, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    contentType: "application/json",
                    async: false,
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.message, false);
                        if (data.result) {
                            RefreshExportList(urlIndex);
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) { }
                });
            }
        }
    });


});

function RefreshExportList(urlIndex) {
    $.ajax({
        type: "GET",
        url: urlIndex,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divExportJobListing").html(data);
            InitPortalOIExportCompayData();
            pageSetUp();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    InitPortalOIExportCompayData();
}
function OnSuccess() {
    $("#divProgress").hide();
    pageSetUp();
    var UserType = $("#UserType").val();
    if ($.UserRole.toLowerCase() == "lob" || UserType.toLowerCase() == "steward") {
        jQuery("#DisplayFileType option:contains('All Files')").remove();
        jQuery("#MonitoringDisplayFileType option:contains('All Files')").remove();
    }
}
$("body").on('change', '.DisplayFileType', function () {
    var FileType = $(this)[0].value;
    var url = '/OI/ExportView?FileType=' + FileType;
    RefreshExportList(url);
});

$("body").on('click', '.CancelFile', function () {
    var FileType = $("#FileType").val();
    var urlIndex = '/OI/ExportView?FileType=' + FileType;
    var id = $(this).attr('id');
    CancelExportREquest(id, urlIndex);
});
function CancelExportREquest(id, urlIndex) {
    var url = '/OIExportView/CancelExportProcess?Id=' + id;
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: ConfirmExportRequest, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    contentType: "application/json",
                    async: false,
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.message, false);
                        if (data.result) {
                            RefreshExportList(urlIndex);
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        },
    });
}
function UpdateOIExportCompany(data) {
    if (data.result) {
        var FileType = $("#FileType").val();
        var urlIndex = '/OI/ExportView?FileType=' + FileType;
        RefreshExportList(urlIndex);
    }
    ShowMessageNotification("success", data.message, false);
}
function InitPortalOIExportCompayData() {
    InitDataTable("#ExportJobs", [10, 20, 30], false, [1, "desc"]);
}