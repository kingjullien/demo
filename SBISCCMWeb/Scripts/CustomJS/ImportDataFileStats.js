$(document).ready(function () {
    if ($("#ImportFileStatsChart").length > 0) {
        var QueryString = "ImportProcessId:" + $("#FileStatsImportProcessId").val();
        var url = '/Home/GetMatchUserCount';
        if (QueryString != "" && QueryString != undefined) {
            url = '/Home/GetMatchUserCount?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        }
        $.ajax({
            type: "GET",
            //contentType: "application/json; charset=utf-8",
            url: url,
            data: '',
            dataType: "json",
            beforeSend: function () {
            },
            success: function (data) {
                var dataStatus = jQuery.parseJSON(data.dataStatus);

                var MatchUserCount = data.objMatch.lstMatchUser;
                var TotalMatch = 0;
                var NameAddressLookupCount = 0;
                var AlternateLookupCount = 0;


                if (dataStatus["Table11"].length > 0) {
                    TotalMatch = dataStatus["Table11"][0][0] == null ? 0 : dataStatus["Table11"][0][0];
                    NameAddressLookupCount = dataStatus["Table11"][0][1] == null ? 0 : dataStatus["Table11"][0][1];
                    AlternateLookupCount = dataStatus["Table11"][0][2] == null ? 0 : dataStatus["Table11"][0][2];
                }

                $("#spnNameAddressLkupCnt").text(addCommas(NameAddressLookupCount));
                $("#spnAlterLkupCnt").text(addCommas(AlternateLookupCount));

                $('#ImportFileStatsChart').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie',
                        width: 200,
                        height: 250
                    },
                    title: {
                        text: matchesbyUser,
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black',
                        style: {
                            fontFamily: '"Open Sans",Arial, Helvetica, sans-serif',
                            fontSize: '14px',
                        },
                        margin: 0
                    },
                    tooltip: {
                        formatter: function () {
                            return '<b>' + this.key + '</b> <br/>' + matches +' : ' + this.percentage.toFixed(2) + '% (' + records + ': ' + this.y + ')';
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

                            },
                            size: "100%"
                        }
                    },
                    series: [{
                        name: 'Matches',
                        colorByPoint: true,
                        data: MatchUserCount,
                    }]
                });
            }
        });
    }
});


