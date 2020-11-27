$('body').on('click', '.DownloadOIDataQueueStatistics', function () {
    $("#IsDownload").val(true);
    $("#frmOIDataQueueStatistics").submit();
});
function onLoadDataQueueStatistics() {
    InitDataTable(".tbAPIUsagGrid", [10, 20, 30], false, [0, "desc"]);
}