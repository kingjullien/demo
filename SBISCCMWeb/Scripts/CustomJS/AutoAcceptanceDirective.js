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
    for (var i = 6; i <= 8; i++) {
        var id = "#TagList" + i;
        var ids = "#Tags" + i;

        var TagList = $(id).val().split(',');
        if (TagList != null || TagList != "") {
            $(".clstag" + i + " option").each(function () {
                if (isInArray($(this).val(), TagList)) {
                    $(this).attr("selected", "selected");
                }
            });

            $(ids).val(TagList);
        }
    }
    if ($(".chzn-select.Directive").length > 0) {
        $(".chzn-select.Directive").chosen().change(function (event) {
            var id = "#" + $(this).attr('id').replace('TagsValue', 'Tags');
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

$("#btnSubmitDirectives").click(function () {
    var Active1 = $("#oAutoAcceptanceDirectivesEntity_Active1").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active1").is(":checked");
    var Tags1 = $("#Tags6").val();
    var Active2 = $("#oAutoAcceptanceDirectivesEntity_Active2").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active2").is(":checked");
    var Tags2 = $("#Tags7").val();
    var Active3 = $("#oAutoAcceptanceDirectivesEntity_Active3").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active3").is(":checked");
    var Tags3 = $("#Tags8").val();
    $.post("/CleanseMatchSettings/AutoAcceptanceDirectives", "AcceptActiveRecordsOnly=" + Active1 + "&Tags1=" + Tags1 + "&PreferHeadquartersRecord=" + Active2 + "&Tags2=" + Tags2 + "&AcceptHeadquartersRecordOnly=" + Active3 + "&Tags3=" + Tags3, function (result) {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
            message: "Data updated successfully.",
        });
        
    });
});