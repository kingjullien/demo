$("body").on('click', '.OpenTags', function () {
    $.magnificPopup.open({
        preloader: true,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/Data/AddTags/'
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: 'popupAddTags'
    });
});
function isInArray(value, array) {
    return array.indexOf(value) > -1;
}
$(document).ready(function () {
    for (var i = 1; i <= 5; i++) {
        var ids = "#Tags" + i;
        var TagList = $("#TagList" + i).val().split(',');
        if (TagList != null || TagList != "") {
            $(".clstag" + i + " option").each(function () {
                if (isInArray($(this).val(), TagList)) {
                     $(this).attr("selected", "selected");
                }
            });
            $(ids).val(TagList);
        }
    }
    if ($(".chzn-select.Exclusion").length > 0) {
        $(".chzn-select.Exclusion").chosen().change(function (event) {
           
            var id ="#"+ $(this).attr('id').replace('TagsValue', 'Tags');
            if (event.target == this) {
                var oldval = $(id).val();
                if (oldval != undefined) {
                    oldval = oldval + $(this).val();
                } else {
                    oldval = $(this).val();
                }
                $(id).val($(this).val());
            }
        });
    }
});

$("#btnSubmitExclusion").click(function () {
    var Active1 = $("#oCleanseMatchExclusionsEntity_Active1").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active1").is(":checked");
    var Tags1 = $("#Tags1").val();
    var Active2 = $("#oCleanseMatchExclusionsEntity_Active2").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active2").is(":checked");
    var Tags2 = $("#Tags2").val();
    var Active3 = $("#oCleanseMatchExclusionsEntity_Active3").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active3").is(":checked");
    var Tags3 = $("#Tags3").val();
    var Active4 = $("#oCleanseMatchExclusionsEntity_Active4").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active4").is(":checked");
    var Tags4 = $("#Tags4").val();
    var Active5 = $("#oCleanseMatchExclusionsEntity_Active5").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active5").is(":checked");
    var Tags5 = $("#Tags5").val();
    $.post("/CleanseMatchSettings/CleanseMatchExclusions", "ExcludeNonHeadQuarters=" + Active1 + "&Tags1=" + encodeURIComponent(Tags1) + "&ExcludeNonMarketable=" + Active2 + "&Tags2=" + encodeURIComponent(Tags2) + "&ExcludeOutofBusiness=" + Active3 + "&Tags3=" + encodeURIComponent(Tags3) + "&ExcludeUndeliverable=" + Active4 + "&Tags4=" + encodeURIComponent(Tags4) + "&ExcludeUnreachable=" + Active5 + "&Tags5=" + encodeURIComponent(Tags5), function (result) {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
            message: "Data updated successfully."  });
    });
});