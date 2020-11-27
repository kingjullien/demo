// Display and Hide loader bar for every ajax call
//$(document).ajaxStart(function () { $('#divProgress').show(); }).ajaxStop(function () { $('#divProgress').hide(); });

$(document).ready(function () {
    $(".content").removeClass("overflow-hidden");
    $('#MultiSelectOptions').multiselect({
        includeSelectAllOption: true,
        numberDisplayed: 3,
        allSelectedText: "",
        nonSelectedText: 'Select ETL Type',

    });
    setInterval(function () {
        var dt = new Date();
        $("#currentUTCTime").text((currentUTCTime + " : " + addTime(dt.getUTCMonth() + 1) + "/" + addTime(dt.getUTCDate()) + "/" + dt.getUTCFullYear() + " " + addTime(dt.getUTCHours()) + ":" + addTime(dt.getUTCMinutes()) + ":" + addTime(dt.getUTCSeconds()) + ""));
    }, 1000);
});

function addTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}
$('body').on('click', '.ExecutionCollapsed', function () {
    var curntclass = $(this);
    if ($(curntclass).hasClass("removecollapsed")) {
        $(".removecollapsed").each(function () {
            $(this).removeClass("removecollapsed");
            $(this).addClass("collapsed");
        });

        $(curntclass).removeClass("removecollapsed");
        $(curntclass).addClass("collapsed");
        $(".trBackgroundProcessExecutionDetailItemView").each(function () {
            $(this).remove();
        });
    }
    else if ($(curntclass).hasClass("collapsed")) {
        $(".removecollapsed").each(function () {
            $(this).removeClass("removecollapsed");
            $(this).addClass("collapsed");
        });
        $(this).addClass("removecollapsed");
        $(this).removeClass("collapsed");
        var ExecutionId = $(curntclass).attr("data-ExecutionId");
        $.ajax({
            type: "GET",
            url: "/Home/BackgroundProcessExecutionDetail?ExecutionId=" + ExecutionId,
            dataType: "HTML",
            contentType: "application/html",
            cache: false,
            success: function (data) {
                $(".trBackgroundProcessExecutionDetailItemView").each(function () {
                    $(this).remove();
                });
                $(curntclass).closest('TR').after(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
});

function ConvertEncrypte(value) {
    var newvalue = "";
    if (value != '' && value != undefined) {
        $.ajax({
            type: "POST",
            url: "/Home/GetEncryptedString",
            data: JSON.stringify({ strValue: value }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            success: function (data) {
                newvalue = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
    return newvalue;
}