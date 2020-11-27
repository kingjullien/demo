function closemsg() {
    $(".alert-success").hide();
}
$('body').on('click', '.btnDnBApiGrp', function () {
    var OptionCount = $('select#objDnbGroupAPI_RemoveAPIIds option').length;
    var GroupName = $("#objDnbGroupAPI_APIGroupName").val();
    var count = 0;
    if (GroupName == '') {
        $("#spnGroupName").show();
        count++;
    }
    else {
        $("#spnGroupName").hide();
    }
    if (OptionCount == 0) {
        $("#spnOptionValue").show();
        count++;
    }
    else {
        $("#spnOptionValue").hide();
    }
    if (count > 0) {
        return false;
    }
   
});


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
$(document).ready(function () {
    LoadTags();    
});
function SetTagValue(OptionValue) {
    $.magnificPopup.close();
    var x = document.getElementById("TagsValue");
    var option = document.createElement("option");
    option.text = OptionValue;
    option.value = OptionValue;
    x.add(option);
    $(".chzn-select").trigger("chosen:updated");
}

function ConfigSuccess()
{
    var message = $("#ConfigMessage").val();
    if ($.trim(message) != '') {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
            message: message
        });
    }
    LoadTags();
}

function LoadTags()
{
    var TagList = $("#TagList").val().split(',');
    if (TagList != null || TagList != "") {
        $(".chzn-select option").each(function () {
            for (var i = 0; i < TagList.length; i++) {
                if ($(this).val() == TagList[i]) {
                    $(this).attr("selected", "selected");
                }
            }
        });
        $("#Tags").val(TagList);
    }
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {

            if (event.target == this) {
                $("#Tags").val($(this).val());
            }
        });
    }
}
