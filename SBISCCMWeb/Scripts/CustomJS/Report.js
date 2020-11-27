$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
    //setup_dashboard_widgets_desktop();
});
var DataStewdStatisticsByCCchart;
var APIUsagesCurntMonthCntChart;
var APIUsagesCurntYearCntChart;
var APIUsagesLineTotalCntChart;
$(document).ready(function () {
    //$("#content").addClass("imgmaindiv")
    setTimeout(function () { chartDataQueue(); DataQueueDashboard(); }, 500);
});


$('body').on('click', '#divDataQueue', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#reportTitle").html($(this).attr("data-title"));
        setTimeout(function () {
            chartDataQueue();
            DataQueueDashboard();
        }, 500);
    }
});
function chartDataQueue() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/ReportsList/GetDataQueue',
        data: '',
        async: false,
        dataType: "json",
        beforeSend: function () {
        },
        success: function (data) {
            var cat = [];
            var objdata = [];
            var maxcount = 0;
            for (var i = 0; i < data.length; i++) {
                cat.push(data[i].x);
                objdata.push(parseInt(data[i].y));
                if (maxcount < parseInt(data[i].y)) {
                    maxcount = parseInt(data[i].y);
                }
            }
            $('#partialDivRptDataQueueDashboard').highcharts({
                chart: {
                    type: 'bar',
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: cat,
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: noOfRecords
                    }
                },
                legend: {
                    reversed: true,
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                        }
                    }
                },
                series: [{
                    name: 'NUMBER OF RECORDS',
                    data: objdata,
                    colorByPoint: true,
                }],
                colors: ['#111', '#fd625e', '#fd625e', '#01b8aa', '#1aadce', '#b6960b', '#f28f43', '#77a1e5', '#c42525']
            });
            hideSeries();
        }
    });
}

function DataQueueDashboard() {

    $.ajax({
        type: 'GET',
        url: '/ReportsList/DataQueueDashboard/',
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html',
        async: false,
        success: function (data) {
            $("#partialDataQueuetitle").html(data);
        }
    });

}



function ChartDataStewardshipStatisticsByUser(userGroup) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/ReportsList/GetDataStewardshipStatisticsByUser',
        data: { Parameters: ConvertEncrypte(userGroup).split("+").join("***") },
        async: false,
        dataType: "json",
        beforeSend: function () {
        },
        success: function (data) {
            var cat = [];
            var objdata = [];
            var maxcount = 0;
            var color = [];
            for (var i = 0; i < data.length; i++) {
                cat.push(data[i].x);
                if (data[i].userGroup == "System Matched") {
                    color.push("#7cb5ec");
                }
                if (data[i].userGroup == "User Matched") {
                    color.push("#434348");
                }
                objdata.push(parseInt(data[i].y));
                if (maxcount < parseInt(data[i].y)) {
                    maxcount = parseInt(data[i].y);
                }
            }
            $('#partialDivRptDataStewdStatisticsByUser').highcharts({
                chart: {
                    type: 'bar'
                },
                title: {
                    text: topRowsByUser,
                    align: "left",

                },
                xAxis: {
                    categories: cat,
                },
                yAxis: {

                    allowDecimals: false,
                },
                legend: {
                    reversed: true,
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                series: [{
                    name: 'Total Matched Rows',
                    data: objdata, colorByPoint: true,
                }],
                colors: color,
            });
            $("#partialDivRptDataStewdStatisticsByUser .highcharts-legend").hide();
        }
    });
}
function ChartDataStewardshipStatisticsBycc(userGroup) {

    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/ReportsList/GetDataStewardshipStatisticsBycc',
        data: { Parameters: ConvertEncrypte(userGroup).split("+").join("***") },
        async: false,
        dataType: "json",
        beforeSend: function () {
        },
        success: function (data) {
            var cc = [];
            var objdata = [];
            var GroupName = [];
            for (var i = 0; i < data.length; i++) {
                cc.push(data[i].x);
                GroupName.push(data[i].userGroup);
            }
            var uniqueGroupName = GroupName.filter(function (itm, i, GroupName) {
                return i == GroupName.indexOf(itm);
            });
            var uniquecc = cc.filter(function (itm, i, cc) {
                return i == cc.indexOf(itm);
            });
            uniquecc = uniquecc.sort(function (a, b) { return a - b });

            for (var i = 0; i < uniqueGroupName.length; i++) {
                var value = [];
                for (var j = 0; j < uniquecc.length; j++) {
                    var filteredData = data.filter(function (entry) {
                        return (entry.x == uniquecc[j] && entry.userGroup == uniqueGroupName[i]);
                    });
                    if (filteredData.length > 0) {
                        value.push(filteredData[0].y)
                    }
                    else {
                        value.push(0);
                    }
                }
                item = {}
                item["name"] = uniqueGroupName[i];
                item["data"] = value;
                if (uniqueGroupName[i] == "System Matched") {
                    item["color"] = "#7cb5ec";
                }
                if (uniqueGroupName[i] == "User Matched") {
                    item["color"] = "#434348";
                }
                objdata.push(item);
            }
            DataStewdStatisticsByCCchart = Highcharts.chart('partialDivRptDataStewdStatisticsByCC', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: topRowsByCC,
                    align: "left",
                },
                xAxis: {
                    categories: uniquecc
                },
                yAxis: {
                    allowDecimals: false,
                },
                tooltip: {
                    headerFormat: 'Confidance Code: {point.x}<br/>',
                    pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                },
                plotOptions: {
                    column: {
                        stacking: 'normal',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                if (this.y > 0)
                                    return this.y;
                            }
                        }
                    }
                },
                legend: {
                    align: 'right',
                    x: 0,
                    verticalAlign: 'top',
                    y: 10,
                    floating: true,
                },
                series: objdata,
            });
        }
    });
}


$('body').on('click', '#divDataStewrdStatistics', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#reportTitle").html($(this).attr("data-title"));
        setTimeout(function () {
            ChartDataStewardshipStatisticsByUser('');
            ChartDataStewardshipStatisticsBycc('');
        }, 500);
    }
});
$('body').on('click', '#divAPIUsage', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#reportTitle").html($(this).attr("data-title"));
        setTimeout(function () {
            APIUsageGrid();
            ChartAPIUsagesCurntMonthCnt();
            ChartAPIUsagesCurntYearCnt();
            ChartAPIUsagesLineTotalCnt();
        }, 500);
    }
});
function APIUsageGrid(isDashboard) {

    $.ajax({
        type: 'GET',
        url: '/ReportsList/GetAPIUsageGrid/',
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html',
        success: function (data) {
            $("#partialAPIUsageGrid").html(data);
            if (APIUsageGrid)
            {
                $(".DashboardAPIUsageGrid .APIreportGrid table thead").addClass("thead_cust");
            }
        }
    });

}
function ChartAPIUsagesCurntMonthCnt() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/ReportsList/GetCurrntMonthCnt',
        data: '',
        dataType: "json",
        success: function (data) {
            var cat = [];
            var objdata = [];
            var maxcount = 0;
            for (var i = 0; i < data.length; i++) {
                cat.push(data[i].x);
                objdata.push(parseInt(data[i].y));
                if (maxcount < parseInt(data[i].y)) {
                    maxcount = parseInt(data[i].y);
                }
            }
            APIUsagesCurntMonthCntChart = Highcharts.chart('partialCurntMonthCnt', {
                chart: {
                    type: 'column',
                },
                title: {
                    text: currentMonthCount,
                },
                subtitle: {
                    text: currentMonthCountByAPI
                },
                xAxis: {
                    categories: cat,
                    labels: {
                        useHTML: true,
                        formatter: function () {
                            return cat[this.value];
                        }
                    }
                },
                yAxis: {
                    allowDecimals: false,
                    min: 0,
                    max: maxcount,
                },
                tooltip: {
                    headerFormat: '<span style="font-size: 10px"> API Type: {point.key} </span><br/>',
                    pointFormat: '{series.name}: <b>{point.y}</b>'
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: [{
                    name: 'Current Month Count',
                    data: objdata,
                    title: ''
                }]
            });
            hideSeries();
        }
    });
}
function ChartAPIUsagesCurntYearCnt() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/ReportsList/GetCurrntYearCnt',
        data: '',
        dataType: "json",
        success: function (data) {

            APIUsagesCurntYearCntChart = Highcharts.chart('partialCurntYearCnt', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: currentYearCount
                },
                subtitle: {
                    text: currentYearCountByDnBAPIName
                },
                tooltip: {
                    headerFormat: '<span style="font-size: 10px"> DnBAPIName {point.key} </span><br/>',
                    pointFormat: '{series.name}: ({point.y}) <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                fontSize: 9,
                                bold: false,
                            }
                        }
                    }
                },
                series: [{
                    name: 'Current Year Count',
                    colorByPoint: true,
                    data: data
                }]
            });
        }
    });
}
function ChartAPIUsagesLineTotalCnt() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/ReportsList/GetTotalYearsCnt',
        data: '',
        dataType: "json",
        success: function (data) {
            var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            var objdata = [];
            var cat = [];
            var maxcount = 0;
            for (var i = 0; i < data.length; i++) {
                var mon = parseInt(data[i].Month) - 1;
                cat.push(monthNames[mon] + " " + data[i].year);
                objdata.push(parseInt(data[i].TotalCalls));

            }
            APIUsagesLineTotalCntChart = Highcharts.chart('partialDivLinechart', {
                chart: {
                    type: 'line'
                },
                title: {
                    text: totalCountByMonth
                },
                xAxis: {
                    categories: cat
                },
                series: [{
                    name: 'Total Count',
                    data: objdata
                }]
            });
            hideSeries();
        }
    });
}



function hideSeries() {
    $(".highcharts-legend").hide();
    $(".highcharts-legend-item").hide();
    $(".highcharts-button").hide();
    $(".highcharts-grid").hide()
}

$("body").on("change", "#UserGroup", function () {
    var userGroup = $(this).val();
    ChartDataStewardshipStatisticsByUser(userGroup);
    ChartDataStewardshipStatisticsBycc(userGroup);
});


//Format all thousands numbers with comma separator
Highcharts.setOptions({
    lang: {
        thousandsSep: ','
    }
});