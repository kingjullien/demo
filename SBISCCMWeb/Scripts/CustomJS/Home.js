// Load Chart for Dashboard page
$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

var ImportIntervalCall;
var ImportSecinterval;
var CleanseIntervalCall;
var CleanseSecinterval;
var EnrichIntervalCall;
var EnrichSecinterval;

function precisionRound(number, precision) {
    var factor = Math.pow(10, precision);
    return Math.round(number * factor) / factor;
}

$(document).ready(function () {
    LoadOnReady();
});

function LoadOnReady() {
    $('[data-toggle="tooltip"]').tooltip();
    //LoadDataQueueStatistics(); 
    BackgroundProcessStatisticsReport();
    LCMCandidateStatisticsReport();
    DataStewardshipStatisticsReport();
}
$(document).on('click', '#ShowAPIUsageReport', function () {
    $.ajax({
        type: 'GET',
        url: "/Home/APIUsageStatisticsReport",
        dataType: 'HTML',
        success: function (data) {
            $("#APIUsageReportModalMain").html(data);
            DraggableModalPopup("#APIUsageReportModal");
        }
    });

});


$(document).on('click', '#btnAPIUsagestatisticsRefresh', function () {
    ReFreshAPIUsageStatistics();
});
function ReFreshAPIUsageStatistics() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/Home/APIUsagestatisticsRefresh',
        data: "",
        dataType: "json",
        contentType: 'application/json;',
        beforeSend: function () {
        },
        success: function (data) {
            var ApiCount = data.ApiCount;
            var YTD = data.YTD;
            var AllCount = data.AllCount;
            var ActualApiCount = data.ActualApiCount;
            var ActualYTD = data.ActualYTD;
            var ActualAllCount = data.ActualAllCount;

            $("#spnApiCount>a").text(ApiCount);
            $("#spnYTD>a").text(YTD);
            $("#spnAllCount>a").text(AllCount);
            $("#titleApiCount").attr("title", ActualApiCount);
            $("#titleYTD").attr("title", ActualYTD);
            $("#titleAllCount").attr("title", ActualAllCount);
        }
    });
}

function LoadDataQueueStatistics() {
    $.ajax({
        type: 'GET',
        url: "/Home/DataQueueStatistics",
        dataType: 'HTML',
        async: true,
        cache: false,
        contentType: 'application/html',
        success: function (data) {
            $("#divPartialDataQueueStatistics").html(data);
            LoadImportProcessChart();
            LoadMatchImportQueue();
        }
    });
}
function loadImportProcessDataQueueStatistics() {
    $.ajax({
        type: 'GET',
        url: "/Home/ImportProcessDataQueueStatistics",
        dataType: 'HTML',
        success: function (data) {
            $("#ImportProcessDataQueueStatisticsModalMain .modal-body").html(data);
            DraggableModalPopup("#ImportProcessDataQueueStatisticsModal");
        }
    });
}
$(document).on('click', '#lnkBtnUnprocessedInput', function () {
    loadImportProcessDataQueueStatistics();
});

$(document).on('click', '#rptShowBackgroundProcessList', function () {
    LoadShowBackgroundProcessList();
});
function LoadShowBackgroundProcessList() {
    $.ajax({
        type: 'GET',
        url: "/Home/BackgroundProcessList",
        dataType: 'HTML',
        success: function (data) {
            $("#BackgroundProcessListModalMain").html(data);
            DraggableModalPopup("#BackgroundProcessListModal");
            //["TableCoulmnName","ServerColumnName","DropDownURL","ISDefault(use lowercase)","Show text/value(use lowercase)","Datatype(If Date)"]
            var colArray = [["TimePeriod", "TimePeriod", "/Home/GetDurationHoursDD", "true", "text"],
            ["StatusType", "StatusType", "/Home/GetStatusType", "false"],
            ["ProcessType", "ProcessType", "/Home/GetETLTypeDD", "false"]
            ];

            //Column array,URL for FilterData, TargetedDiv, DatatableId
            InitFilters(colArray, "/Home/FilterBackGroundProcess", "#BackGroundProcessFilterMain", "#divPartialBackgroundProcess", "");
        }
    });

}

function LoadImportProcessChart() {
    var importProcessChartValue = $("#hdnImportProcessChartValue").val();
    var values = [];
    if (importProcessChartValue != undefined && importProcessChartValue != "") {
        var lstImportProcessChartValue = JSON.parse($("#hdnImportProcessChartValue").val());
        for (var cnt = 0; cnt < lstImportProcessChartValue.length; cnt++) {
            values.push([lstImportProcessChartValue[cnt].EpochDate, lstImportProcessChartValue[cnt].ImportedRecordCount]);
        }
    }
    Highcharts.setOptions({
        lang: {
            thousandsSep: ','
        }
    })
    Highcharts.chart('divRptImportProcess', {
        chart: {
            type: 'spline'
        },
        title: {
            text: 'Import Data'
        },
        xAxis: {
            type: 'datetime',
            dateTimeLabelFormats: { // don't display the dummy year
                month: '%b %Y',
            },
            title: {
                text: 'Date'
            }
        },
        yAxis: {
            title: {
                text: 'Record Count'
            },
            min: 0
        },
        tooltip: {
            crosshairs: false,
            shared: true,
            valueSuffix: ' Records',
        },

        plotOptions: {
            series: {
                marker: {
                    enabled: true
                }
            }
        },

        colors: ['#6CF', '#39F', '#06C', '#036', '#000'],

        series: [{
            name: "Import Data",
            data: values
        }],

        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    plotOptions: {
                        series: {
                            marker: {
                                radius: 2.5
                            }
                        }
                    }
                }
            }]
        }
    });
    $(".highcharts-legend").hide();
}
function hideSeries() {
    $(".highcharts-legend").hide();
    $(".highcharts-legend-item").hide();
    $(".highcharts-button").hide();
    $(".highcharts-grid").hide();
}

function LoadMatchImportQueue() {

    var InputRecordCount_Failed = $("#InputRecordCount_Failed").val();
    var InputRecordCount_AwaitingProcessing = $("#InputRecordCount_AwaitingProcessing").val();
    var InputRecordCount_Processing = $("#InputRecordCount_Processing").val();
    var InputRecordCount_Total = $("#InputRecordCount_Total").val();
    if (InputRecordCount_Total > 0) {
        Highcharts.chart('RptImportQueueCleanMatch', {
            chart: {
                type: 'pie'
            },
            title: {
                text: null
            },
            plotOptions: {
                pie: {
                    innerSize: 100,
                    depth: 45
                }
            },
            series: [{
                name: 'Data',
                data: [
                    ["Failed", InputRecordCount_Failed],
                    ["Awaiting", InputRecordCount_AwaitingProcessing],
                    ["Processing", InputRecordCount_Processing],
                ]
                , size: '100%',
                innerSize: '80%',
                showInLegend: false,
                dataLabels: {
                    enabled: false
                },
                states: {
                    hover: {
                        halo: false
                    }
                }
            }]
        });
    }
}

$(document).on('change', '#EnableCLEANSE_MATCHProcess', function () {
    var currentVal = $(this).prop("checked");
    var Message = "";
    if (currentVal == true) {
        Message = settingPausemsg;
    }
    else {
        Message = settingUnPausemsg;
    }
    var QueryString = "SettingName:" + "PAUSE_CLEANSE_MATCH_ETL" + "@#$SettingValue:" + currentVal;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: Message, callback: function (result) {
            if (result) {
                $('#divProgress').show();
                $.ajax({
                    type: "POST",
                    url: '/Home/PauseDataMethod?Parameters=' + Parameters,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        $('#divProgress').hide();
                        ShowMessageNotification("success", response, false);
                        BackgroundProcessStatisticsReport();
                    },
                    error: function (xhr, status, error) {
                        $('#divProgress').hide();
                    }
                });
            }
            else {
                $('#EnableCLEANSE_MATCHProcess').prop("checked", !currentVal);
            }
        }
    });
});

$(document).on('change', '#EnableENRICHMENTProcess', function () {
    var currentVal = $(this).prop("checked");
    var Message = "";
    if (currentVal == true) {
        Message = settingPausemsg;
    }
    else {
        Message = settingUnPausemsg;
    }
    var QueryString = "SettingName:" + "PAUSE_ENRICHMENT_ETL" + "@#$SettingValue:" + currentVal;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: Message, callback: function (result) {
            if (result) {
                $('#divProgress').show();
                $.ajax({
                    type: "POST",
                    url: '/Home/PauseDataMethod?Parameters=' + Parameters,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        $('#divProgress').hide();
                        ShowMessageNotification("success", response, false);
                        BackgroundProcessStatisticsReport();
                    },
                    error: function (xhr, status, error) {
                        $('#divProgress').hide();
                    }
                });
            }
            else {
                $('#EnableENRICHMENTProcess').prop("checked", !currentVal);
            }
        }
    });
});

$(document).on('change', '#EnablePauseFileImportProcessETL', function () {
    var currentVal = $(this).prop("checked");
    var Message = "";
    if (currentVal == true) {
        Message = settingPausemsg;
    }
    else {
        Message = settingUnPausemsg;
    }
    var QueryString = "SettingName:" + "PAUSE_FILE_IMPORT_PROCESS_ETL" + "@#$SettingValue:" + currentVal;

    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: Message, callback: function (result) {
            if (result) {
                $('#divProgress').show();
                var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                $.ajax({
                    type: "POST",
                    url: '/Home/PauseDataMethod?Parameters=' + Parameters,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        $('#divProgress').hide();
                        ShowMessageNotification("success", response, false);
                        BackgroundProcessStatisticsReport();
                    },
                    error: function (xhr, status, error) {
                        $('#divProgress').hide();
                    }
                });
            }
            else {
                $('#EnablePauseFileImportProcessETL').prop("checked", !currentVal);
            }
        }
    });
});

function BackgroundProcessStatisticsReport() {
    $.ajax({
        type: "GET",
        url: '/Home/BackgroundProcessStatistics',
        dataType: "json",
        success: function (data) {
            ResetAllIntervals();
            $("#LoadBackgroundProcessStatisticsReport").html(data.view);
            startAllIntervals();
        }
    });
}

function LCMCandidateStatisticsReport(QueryString) {
    var url = '/Home/GetLowConfidenceMatchStatistics';
    if (QueryString != "" && QueryString != undefined) {
        url = '/Home/GetLowConfidenceMatchStatistics?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    }
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: url,
        dataType: "json",
        contentType: 'application/json;',
        beforeSend: function () {
        },
        success: function (data) {
            var ResultStatus = jQuery.parseJSON(data.Data1);
            var Result = jQuery.parseJSON(data.Data2);


            for (var i = 0; i < ResultStatus["Table1"].length; i++) {
                for (var j = 0; j < ResultStatus["Table1"][i].length; j++) {
                    ResultStatus["Table1"][i][j] = ResultStatus["Table1"][i][j] == null ? 0 : ResultStatus["Table1"][i][j];
                }
            }

            for (var i = 0; i < Result["Table11"].length; i++) {
                for (var j = 0; j < Result["Table11"][i].length; j++) {
                    Result["Table11"][i][j] = Result["Table11"][i][j] == null ? 0 : Result["Table11"][i][j];
                }
            }
            $("#TCTotal").text(ResultStatus["Table1"][0][0]);
            var value = parseFloat($('#total').text()) + parseFloat($(this).data('amount')) / 100
            $('#total').text(value.toFixed(2));
            $("#TCActive").text(ResultStatus["Table1"][0][4] + " (" + precisionRound((ResultStatus["Table1"][0][8] * 100), 2) + "%)");
            $("#TCHQSL").text(ResultStatus["Table1"][0][6] + " (" + precisionRound((ResultStatus["Table1"][0][10] * 100), 2) + "%)");
            $("#ACTotal").text(ResultStatus["Table1"][0][1] + " (AVG : " + precisionRound(ResultStatus["Table1"][0][2], 2) + ")");
            $("#ACActive").text(ResultStatus["Table1"][0][3] + " (" + precisionRound((ResultStatus["Table1"][0][7] * 100), 2) + "%)");
            $("#ACHQSL").text(ResultStatus["Table1"][0][5] + " (" + precisionRound((ResultStatus["Table1"][0][9] * 100), 2) + "%)");
            $('#divLCMCandidateStatisticsReport').highcharts({
                chart: {
                    type: 'column'
                },
                colors: ["#4475c5", "#ef7e32", "#a6a6a6", "#ffc000"],
                title: {
                    text: ''
                },
                xAxis: {
                    categories: [Result["Table11"][0][0], Result["Table11"][1][0]],
                    crosshair: false
                },
                yAxis: {
                    min: 0,
                    showLastLabel: false,
                    gridLineColor: 'transparent',
                    title: {
                        text: ''
                    },
                    allowDecimals: false,
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.1,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: '9 & 10',
                    data: [Result["Table11"][0][1], Result["Table11"][1][1]]
                }, {
                    name: '7 & 8',
                    data: [Result["Table11"][0][2], Result["Table11"][1][2]]
                }, {
                    name: '5 & 6',
                    data: [Result["Table11"][0][3], Result["Table11"][1][3]]
                }, {
                    name: 'Below 5',
                    data: [Result["Table11"][0][4], Result["Table11"][1][4]]
                }],

            });
        }
    });


    function precisionRound(number, precision) {
        var factor = Math.pow(10, precision);
        return Math.round(number * factor) / factor;
    }
}
function DataStewardshipStatisticsReport(QueryString) {

    var url = '/Home/GetMatchUserCount';
    if (QueryString != "" && QueryString != undefined) {
        url = '/Home/GetMatchUserCount?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    }
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: url,
        data: '',
        dataType: "json",
        beforeSend: function () {
        },
        success: function (data) {

            var dataStatus = jQuery.parseJSON(data.dataStatus);

            var TotalMatch = 0;
            var NameAddressLookupCount = 0;
            var AlternateLookupCount = 0;


            if (dataStatus["Table11"].length > 0) {
                TotalMatch = dataStatus["Table11"][0][0] == null ? 0 : dataStatus["Table11"][0][0];
                NameAddressLookupCount = dataStatus["Table11"][0][1] == null ? 0 : dataStatus["Table11"][0][1];
                AlternateLookupCount = dataStatus["Table11"][0][2] == null ? 0 : dataStatus["Table11"][0][2];
            }


            $("#spnTotalMatch").text(TotalMatch);
            $("#spnNameAddressLkupCnt").text(NameAddressLookupCount);
            $("#spnAlterLkupCnt").text(AlternateLookupCount);

            var MatchUserCount = data.objMatch.lstMatchUser;
            var MatchCodeCount = data.objMatch.lstMatchConfidenceCode;
            var cat = [];
            var objdata = [];
            var maxcount = 0;

            $('#chtMatchUser').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie',
                    width: 240,
                },
                title: {
                    text: matchesByUsers,
                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black',
                    style: {
                        fontFamily: '"Open Sans",Arial, Helvetica, sans-serif',
                        fontSize: '14px',
                    }

                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.key + '</b> <br/>' + matches + ' : ' + this.percentage.toFixed(2) + '% (' + records + ' : ' + this.y + ')';
                    }

                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false,
                            format: '',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black',

                            },

                        }
                    }
                },
                series: [{
                    name: matches,
                    colorByPoint: true,
                    data: MatchUserCount,
                }]
            });

            for (var i = 0; i < MatchCodeCount.length; i++) {
                cat.push(MatchCodeCount[i].x);
                objdata.push(parseInt(MatchCodeCount[i].y));
                if (maxcount < parseInt(MatchCodeCount[i].y)) {
                    maxcount = parseInt(MatchCodeCount[i].y);
                }
            }
            maxcount = maxcount + 2;

            $('#chtMatchConfidenceCode').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: matchesByConfidenceCode,
                    x: -20,//center
                    style: {
                        fontFamily: '"Open Sans",Arial, Helvetica, sans-serif',
                        fontSize: '14px',
                    }
                },
                xAxis: {
                    categories: cat
                },
                yAxis: {
                    min: 0,
                    max: maxcount,
                },
                plotOptions: {
                    column: {
                        pointPadding: 0,
                        borderWidth: 1,
                        borderColor: '#000000'
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: [{
                    name: matches,
                    data: objdata,
                    colorByPoint: true,
                    title: ''
                }]
            });
            hideSeries();
        }
    });
}

$(document).on('click', '#btnLCMStatisticsRefresh', function () {
    LCMCandidateStatisticsReport();
});
$(document).on('click', '#btnDataStewardStatisticsRefresh', function () {
    DataStewardshipStatisticsReport();
});
$(document).on('click', '#btnBackgroundtatisticsRefresh', function () {
    BackgroundProcessStatisticsReport();
});
$(document).on('click', '#btnActiveDataRefresh', function () {
    var buttonName = $("#ActiveStatisticsFilter").text().trim();
    if (buttonName == "Filter Applied") {
        var Tags = "";
        var LOBTags = "";
        if ($("#LicenseEnableTags").val().toLowerCase() == "true") {
            Tags = $("#ActiveStatisticsFilterTags").val();
            LOBTags = $("#LOBTag").val();
        }
        Tags = Tags == "0" ? "" : Tags;
        ReFreshActiveStatisticsFilter(Tags, LOBTags);
    }
    else {
        ReFreshActiveStatisticsFilter("", "");
    }
    $(".chzn-select").trigger("chosen:updated");
});
function ReFreshActiveStatisticsFilter(Tags, LOBTags) {
    if ($.UserRole == "lob") {
        LOBTags = $.UserLOBTag;
    }
    var QueryString = "Tags:" + Tags.split("::").join("&#58&#58") + "@#$LOBTags:" + LOBTags.replace("::", "&#58&#58");
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/Home/ActiveStatisticsFilter',
        data: { Parameters: ConvertEncrypte(encodeURI(QueryString)).split("+").join("***") },
        dataType: "json",
        contentType: 'application/json;',
        beforeSend: function () {
        },
        success: function (data) {
            $(".spnActiveStatus").text(data.ActiveQueueStatus);
            $("#TitleActualMatchCnt").attr("data-original-title", data.InputRecordCount_Total);
            $("#SpnActualMatchCnt a strong").text(data.ActualInputRecordCount_Total);

            $("#TitleActualMatchCntImport").attr("data-original-title", data.FilesAwaitingImportCount);
            $("#SpnActualMatchCntImport a strong").text(data.ActualFilesAwaitingImportCount);

            $("#TitleLCMCnt").attr("data-original-title", data.ActualLCMCount);
            $("#SpnLCMCnt a strong").text(data.LCMCount);

            $("#TitleBIDCnt").attr("data-original-title", data.BadInputRecordCount);
            $("#SpnBIDCnt a strong").text(data.ActualBadInputRecordCount);

            $("#TitleExportCntMatch").attr("data-original-title", data.ActualMatchExportRecordCount);
            $("#SpnExportCntMatch a strong").text(data.MatchExportRecordCount);
            $("#ProcessingQueueCnt").text(data.ProcessingQueueCnt);
            if (data.ProcessingQueueCnt > 0 || data.ProcessingQueueCnt.includes("K") || data.ProcessingQueueCnt.includes("M") || data.ProcessingQueueCnt.includes("B")) {
                $("#ProcessingQueueCnt").remove();
                $(".matchOutput").append("<div class='displayValue' id='ProcessingQueueCnt' title='Processing Queue'>" + data.ProcessingQueueCnt + "</div>");
            }
            else {
                $("#ProcessingQueueCnt").hide();
            }

            $("#TitleExportCntEnrichment").attr("data-original-title", data.ActualEnrichmentExportRecordCount);
            $("#SpnExportCntEnrichment a strong").text(data.EnrichmentExportRecordCount);
            $("#EnrichmentQueueCount").text(data.EnrichmentQueueCount);
            if (data.EnrichmentQueueCount > 0 || data.EnrichmentQueueCount.includes("K") || data.EnrichmentQueueCount.includes("M") || data.EnrichmentQueueCount.includes("B")) {
                $("#EnrichmentQueueCount").remove();
                $(".enrichmentOutput").append("<div class='displayValue' id='EnrichmentQueueCount' title='Processing Queue'>" + data.EnrichmentQueueCount + "</div>");
            }
            else {
                $("#EnrichmentQueueCount").hide();
            }
        }
    });
}

function addZero(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

function setImportInterVal() {
    if ($('#IsImportProcessEnabled').val() == "True") {
        var timerImport = $("#ImportNextExeTimeSpan").val();
        ImportSecinterval = setInterval(function () {
            var timer = timerImport.split(':');
            //by parsing integer, I avoid all extra string processing
            var hour = parseInt(timer[0], 10);
            var minutes = parseInt(timer[1], 10);
            var seconds = parseInt(timer[2], 10);
            if (!(minutes > 0) && !(seconds > 0)) {
                clearInterval(ImportSecinterval);
                $('.spnImportNextTime').text("Running");
                if ($('.spnCleanseNextTime').text() != "Running" && $('.spnEnrichNextTime').text() != "Running")
                    MakeCallForImport();
            }
            else if (minutes > 0 || seconds > 0) {
                --seconds;
                minutes = (seconds < 0) ? --minutes : minutes;
                if (minutes < 0) clearInterval(ImportSecinterval);
                seconds = (seconds < 0) ? 59 : seconds;
                seconds = (seconds < 10) ? '0' + seconds : seconds;
                $('.spnImportNextTime').text('Next execution in ' + addZero(hour) + ':' + addZero(minutes) + ':' + seconds);
                timerImport = hour + ':' + minutes + ':' + seconds;
            }
        }, 1000);
    }
}

function MakeCallForImport() {
    ResetCallIntervals();
    ImportIntervalCall = setInterval(function () {
        $.ajax({
            type: "GET",
            url: '/Home/BackgroundProcessStatistics',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            success: function (data) {
                if (data.ImportNextTime != "00:00:00") {
                    ResetAllIntervals();
                    $("#LoadBackgroundProcessStatisticsReport").html(data.view);
                    startAllIntervals();
                }
            }
        });
        $('#divProgress').hide();
    }, parseInt($("#RealTimeCallInterval").val()) * 1000);
}


function setCleanseInterVal() {
    if ($('#IsCleanseProcessEnabled').val() == "True") {
        var timerCleanse = $("#CleanseNextExeTimeSpan").val();
        CleanseSecinterval = setInterval(function () {
            var timer = timerCleanse.split(':');
            //by parsing integer, I avoid all extra string processing
            var hour = parseInt(timer[0], 10);
            var minutes = parseInt(timer[1], 10);
            var seconds = parseInt(timer[2], 10);
            if (!(minutes > 0) && !(seconds > 0)) {
                clearInterval(CleanseSecinterval);
                $('.spnCleanseNextTime').text("Running");
                if ($('.spnImportNextTime').text() != "Running" && $('.spnEnrichNextTime').text() != "Running")
                    MakeCallForCleanse();
            }
            else if (minutes > 0 || seconds > 0) {
                --seconds;
                minutes = (seconds < 0) ? --minutes : minutes;
                if (minutes < 0) clearInterval(CleanseSecinterval);
                seconds = (seconds < 0) ? 59 : seconds;
                seconds = (seconds < 10) ? '0' + seconds : seconds;
                $('.spnCleanseNextTime').text('Next execution in ' + addZero(hour) + ':' + addZero(minutes) + ':' + seconds);
                timerCleanse = hour + ':' + minutes + ':' + seconds;
            }
        }, 1000);
    }
}

function MakeCallForCleanse() {
    ResetCallIntervals();
    CleanseIntervalCall = setInterval(function () {
        $.ajax({
            type: "GET",
            url: '/Home/BackgroundProcessStatistics',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },

            success: function (data) {
                if (data.CleanseNextTime != "00:00:00") {
                    ResetAllIntervals();
                    $("#LoadBackgroundProcessStatisticsReport").html(data.view);
                    startAllIntervals();
                }
            }
        });
        $('#divProgress').hide();

    }, parseInt($("#RealTimeCallInterval").val()) * 1000);
}

function setEnrichInterVal() {
    if ($('#IsEnrichProcessEnabled').val() == "True") {
        var timerEnrich = $("#EnrichNextExeTimeSpan").val();
        EnrichSecinterval = setInterval(function () {
            var timer = timerEnrich.split(':');
            //by parsing integer, I avoid all extra string processing
            var hour = parseInt(timer[0], 10);
            var minutes = parseInt(timer[1], 10);
            var seconds = parseInt(timer[2], 10);
            if (!(minutes > 0) && !(seconds > 0)) {
                clearInterval(EnrichSecinterval);
                $('.spnEnrichNextTime').text("Running");
                if ($('.spnImportNextTime').text() != "Running" && $('.spnCleanseNextTime').text() != "Running")
                    MakeCallForCleanse();
            }
            else if (minutes > 0 || seconds > 0) {
                --seconds;
                minutes = (seconds < 0) ? --minutes : minutes;
                if (minutes < 0) clearInterval(EnrichSecinterval);
                seconds = (seconds < 0) ? 59 : seconds;
                seconds = (seconds < 10) ? '0' + seconds : seconds;
                $('.spnEnrichNextTime').text('Next execution in ' + addZero(hour) + ':' + addZero(minutes) + ':' + seconds);
                timerEnrich = hour + ':' + minutes + ':' + seconds;
            }
        }, 1000);
    }
}

function MakeCallForEnrich() {
    ResetCallIntervals();
    EnrichIntervalCall = setInterval(function () {
        $.ajax({
            type: "GET",
            url: '/Home/BackgroundProcessStatistics',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            success: function (data) {
                if (data.EnrichNextTime != "00:00:00") {
                    ResetAllIntervals();
                    $("#LoadBackgroundProcessStatisticsReport").html(data.view);
                    startAllIntervals();
                }
            }
        });
        $('#divProgress').hide();

    }, parseInt($("#RealTimeCallInterval").val()) * 1000);
}


function ResetAllIntervals() {
    clearInterval(ImportSecinterval);
    clearInterval(ImportIntervalCall);
    clearInterval(CleanseSecinterval);
    clearInterval(CleanseIntervalCall);
    clearInterval(EnrichSecinterval);
    clearInterval(EnrichIntervalCall);
}

function ResetCallIntervals() {
    clearInterval(ImportIntervalCall);
    clearInterval(CleanseIntervalCall);
    clearInterval(EnrichIntervalCall);
}

function startAllIntervals() {
    setImportInterVal();
    setCleanseInterVal();
    setEnrichInterVal();
}

$(document).on("change", '#FilterByTag', function () {
    var ByTag = $(this).prop('checked');
    if (ByTag == true) {
        $(".spnUsingImportProcess").removeClass('thColor');
        $(".spnUsingTags").addClass('thColor');
    }
    else {
        $(".spnUsingImportProcess").removeClass('thColor');
        $(".spnUsingImportProcess").addClass('thColor');
    }
    var QueryString = "byTag:" + ByTag;
    $.ajax({
        type: 'GET',
        url: "/Home/ImportProcessDataQueueStatistics?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#ImportProcessDataQueueStatisticsModalMain .modal-body").html(data);
            if (ByTag == true && window.matchMedia("(max-width: 1817px)").matches) {
                $("#tbImportProcess").css('display', 'inline-table');
            }
        }
    });
});