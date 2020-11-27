$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    BackgroundProcessStatisticsReport();
});
$(document).on('click', '#ShowOIAPIUsageTable', function () {
    $.ajax({
        type: 'GET',
        url: '/OIHome/OIAPIUsageStatisticsGrid/',
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#OIAPIUsageStatisticsGridModalMain").html(data);
            DraggableModalPopup("#OIAPIUsageStatisticsGridModal");
            onLoadDataQueueStatistics();
        }
    });
});
function BackgroundProcessStatisticsReport() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/OIHome/OIBackgroundProcessStatistics',
        data: '',
        dataType: "json",
        beforeSend: function () {
        },
        success: function (data) {
            var Result = jQuery.parseJSON(data.Data1);
            var ResultStatus = jQuery.parseJSON(data.Data2);

            var MatchEnrichProcess = ResultStatus.Table11[0][1];
            var color = "";
            if (MatchEnrichProcess.toLowerCase() == "scheduled") {
                color = "peru";
            }
            else if (MatchEnrichProcess.toLowerCase() == "process paused") {
                color = "red";
            }
            else if (MatchEnrichProcess.toLowerCase() == "running") {
                color = "green";
            }
            $(".spnOICleanmatch").text(MatchEnrichProcess);
            $(".spnOICleanmatch").css("color", color);
            var OICleansMatchNbrProcesses = 0;
            var OICleansMatchNbrCompleted = 0;
            var OICleansMatchNbrRunning = 0;
            var OICleansMatchNbrFailed = 0;
            var OICleansMatchNbrSuccessful = 0;
            var OICleansMatchLastProcessCompletedMinutes = 0;
            if (Result.Table1.length > 0) {
                OICleansMatchNbrProcesses = Result.Table1[1][0];
                OICleansMatchNbrCompleted = Result.Table1[1][1];
                OICleansMatchNbrRunning = Result.Table1[1][2];
                OICleansMatchNbrFailed = Result.Table1[1][3];
                OICleansMatchNbrSuccessful = Result.Table1[1][4];
                OICleansMatchLastProcessCompletedMinutes = Result.Table1[1][5];
            }
            var OICleanData = Result.Table1.length > 0 ? [[running, OICleansMatchNbrRunning], [successful, OICleansMatchNbrSuccessful], [failed, OICleansMatchNbrFailed], { name: noRecordsAvailable, y: 0.1, dataLabels: { enabled: false, }, }] : [{ name: noRecordsAvailable, y: 0.1, dataLabels: { enabled: false, }, }];
            //CLEANSE MATCH Background Process Statistics

            $('#chtOICleanMatchBackgroundProcess').highcharts({
                colors: ["#d5d5d5", "#6fd64b", "#fe642e"],
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 0,
                    plotShadow: false,
                    height:200
                },
                title: {
                    text: '<span style="font-size: 9px">' + lastProcess + '<br><span style="font-size: 9px">' + completed  + '</span><br><span style="font-size: 9px">' +minute + '</span><br>' + '<span style="font-size: 12px">' + OICleansMatchLastProcessCompletedMinutes + '</span>',
                    align: 'center',
                    verticalAlign: 'middle',
                    y: 0,
                    style: {
                        fontFamily: '"Open Sans",Arial, Helvetica, sans-serif',
                        textTransform: 'uppercase'
                    }
                },
                subtitle: {
                    text: cleanseMatch,
                    y: 12,
                    style: {
                        fontFamily: '"Open Sans",Arial, Helvetica, sans-serif',
                        paddingBottom: 0,
                        
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: {point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        dataLabels: {
                            enabled: true,
                            distance: -10,
                            style: {
                                fontWeight: 'normal',
                                color: 'black',
                                fontSize: '9px',
                                plotBorderColor: 'black',
                                plotBorderWidth: 0,
                                fontFamily: '"Open Sans",Arial, Helvetica, sans-serif',
                            }
                        },
                        startAngle: -90,
                        endAngle: 90,
                        size: '130%'
                    }
                },
                series: [{
                    type: 'pie',
                    name: executions,
                    innerSize: '60%',
                    shadow: false,
                    data: OICleanData
                }]
            });
        }
    });
}

$(document).on('click', '#btnOIActiveDataRefresh', function () {
    ReFreshOIActiveStatisticsFilter();
});
function ReFreshOIActiveStatisticsFilter() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/OIHome/OIActiveStatisticsFilter',
        dataType: "json",
        beforeSend: function () {
        },
        success: function (data) {
            $("#SpnOIActualMatchCnt a strong").text(data.InputRecordCount);
            $("#SpnOIUnMatchedCnt a strong").text(data.UnMatchRecordCount);
            $("#SpnMatchOutputCnt a strong").text(data.MatchedOutputQueueCount);
            $("#SpnFirmographicsCnt a strong").text(data.FirmographicsExportQueueCount);
            $("#SpnCompanyTreeCnt a strong").text(data.CorporateTreeExportQueueCount);

            $("#TitleOIActualMatchCnt").attr("title", data.FormatInputRecordCount);
            $("#TitleOIUnMatchedCnt").attr("title", data.FormatUnMatchRecordCount);
            $("#TitleMatchOutputCnt").attr("title", data.FormatMatchedOutputQueueCount);
            $("#TitleFirmographicsCnt").attr("title", data.FormatFirmographicsExportQueueCount);
            $("#TitleCompanyTreeCnt").attr("title", data.FormatCorporateTreeExportQueueCount);

            $("#TitleOIActualMatchCnt").attr("data-original-title", data.FormatInputRecordCount);
            $("#TitleOIUnMatchedCnt").attr("data-original-title", data.FormatUnMatchRecordCount);
            $("#TitleMatchOutputCnt").attr("data-original-title", data.FormatMatchedOutputQueueCount);
            $("#TitleFirmographicsCnt").attr("data-original-title", data.FormatFirmographicsExportQueueCount);
            $("#TitleCompanyTreeCnt").attr("data-original-title", data.FormatCorporateTreeExportQueueCount);
        }
    });
}

$(document).on('click', '#btnBackgroundtatisticsRefresh', function () {
    BackgroundProcessStatisticsReport();
});
$(document).on('click', '#btnOIAPIUsagestatisticsRefresh', function () {
    ReFreshOIAPIUsagestatistics();
});
function ReFreshOIAPIUsagestatistics() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/OIHome/OIAPIUsagestatisticsReFresh',
        dataType: "json",
        beforeSend: function () {
        },
        success: function (data) {
            $("#spnOIMonthCnt a strong").text(data.FormatMonthCount);
            $("#spnOIYTD a strong").text(data.FormatYTDCount);
            $("#spnOIAllCount a strong").text(data.FormatAllAPIUsageCount);
            $("#spnLastOneDay a strong").text(data.FormatHourlyAPIUsageCount);
           
            $("#titleOIMonthCnt").attr("title", data.MonthCount);
            $("#titleOIYTD").attr("title", data.YTDCount);
            $("#titleOIAllCount").attr("title", data.AllAPIUsageCount);
            $("#titleLastOneDay").attr("title", data.HourlyAPIUsageCount);
           
            $("#titleOIMonthCnt").attr("data-original-title", data.MonthCount);
            $("#titleOIYTD").attr("data-original-title", data.YTDCount);
            $("#titleOIAllCount").attr("data-original-title", data.AllAPIUsageCount);
            $("#titleLastOneDay").attr("data-original-title", data.HourlyAPIUsageCount);
        }
    });
}