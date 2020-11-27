$(document).on('click', '.btnSelectUnselectStewOIMatch', function () {
    if ($(this).hasClass("fa-circle-thin")) {
        $(this).removeClass("fa-circle-thin");
        $(this).addClass("fa-circle");
        $(this).attr("data-value", true);
    }
    else {
        $(this).addClass("fa-circle-thin");
        $(this).removeClass("fa-circle");
        $(this).attr("data-value", false);
    }
});
$(document).on('click', '#btnRefreshMatchSearch', function () {
    var selectedMatchSerch = [];
    $(".btnSelectUnselectStewOIMatch").each(function () {
        if ($(this).attr("data-value").toLowerCase() == "true") {
            selectedMatchSerch.push($(this).attr("data-matchid"));
        }
    });
    if (selectedMatchSerch.length > 0) {
        var inputId = $("#lstOICompanyInput_InputId").val();
        var MatchId = selectedMatchSerch.toString();
        var QueryString = "InputId:" + inputId + "@#$MatchId:" + MatchId;
        var Parameters = parent.parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        $.ajax({
            type: 'POST',
            url: "/OIMatchData/ViewresolutionMap?Parameters=" + Parameters,
            dataType: 'HTML',
            cache: false,
            contentType: 'application/html;',
            success: function (data) {
                $(".DivPartialResolutionMap").html(data);
            }
        });
    }
    else {
        parent.ShowMessageNotification("success", selectSearch, false);
    }

});
$(document).on('click', '.btnAssignOrbNumber', function () {
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
