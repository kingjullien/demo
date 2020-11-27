function onLoadMatchMetaData() {
    setRankId();
}
function prevClick(objThis) {
    var inputId = $("#lstOICompanyInput_InputId").val();
    var OrbNum = $(objThis).attr("data-val");
    var dataNext = $(".dnbOrbNumber").attr("data-val");// $(".matchdetail-Next").attr("data-val");
    var dataPrev = $(objThis).attr("data-prev");
    if ($(objThis).parent().hasClass("disabled")) {
        return false;
    }
    else {
        var QueryString = "inputId:" + inputId + "@#$OrbNum:" + OrbNum + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$IsPartial:false";
        var Parameters = parent.parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        var url = '/OIMatchData/MatchMetadata?Parameters=' + Parameters;
        OIMatchMetaDataDetail(url);
    }
}
function nextClick(objThis) {
    var inputId = $("#lstOICompanyInput_InputId").val();
    var OrbNum = $(objThis).attr("data-val");
    var dataNext = $(objThis).attr("data-Next");
    var dataPrev = $(".dnbOrbNumber").attr("data-val");
    if ($(objThis).parent().hasClass("disabled")) {
        return false;
    }
    else {
        var QueryString = "inputId:" + inputId + "@#$OrbNum:" + OrbNum + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$IsPartial:false";
        var Parameters = parent.parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        var url = '/OIMatchData/MatchMetadata?Parameters=' + Parameters;
        OIMatchMetaDataDetail(url);
    }
}
function OIMatchMetaDataDetail(url) {
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'HTML',
        async: false,
        cache: false,
        contentType: 'application/html',
        success: function (data) {
            $("#ParialDivMatchMetaDataDetails").html(data);
            setRankId();
        }
    });
}
function setRankId() {
    //set Rank and current Selected row in parent Table
    parent.$("#tbOIMetaSearchData tr").removeClass("current");
    var itemId = $(".dnbOrbNumber").attr("data-val");
    parent.$("#" + itemId).addClass("current");
    var Rank = parent.$("#" + itemId).find(".RankId").text();
    $("#RankId").text(Rank);
            //end set Rank and current Selected row in parent Table
}
$(document).on('click', '.btnAssignOrn', function () {
    var inputId = $("#lstOICompanyInput_InputId").val();
    var OrbNum = $(this).attr("data-orbnum");
    var QueryString = "inputId:" + inputId + "@#$OrbNum:" + OrbNum;
    var Parameters = parent.parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: assignORB, callback: function (result) {
            if (result) {
                $.ajax({
                    type: 'POST',
                    url: "/OIMatchData/AssignORBnum?Parameters=" + Parameters,
                    dataType: 'JSON',
                    cache: false,
                    contentType: 'application/json;',
                    success: function (data) {
                        if (data.result == true) {
                            parent.parent.CallbackMatchAssign(data.message);
                        }
                        else {
                            parent.parent.ShowMessageNotification("success", data.message, false);
                        }
                    }
                });
            }
        }
    });
});