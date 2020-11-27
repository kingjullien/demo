$(document).ready(function () {
    var slider = document.getElementById('sldDataSecuritySettings');
    noUiSlider.create(slider, {
        start: [0],
        range: {
            'min': [1],
            'max': [5]
        },
        step: 1
    });

    var sldPresetsval = $("#sldPresets").val();

    if (sldPresetsval != "") {
        document.getElementById('lblDataSecuritySettings').innerHTML = sldPresetsval;
        $("#hdnDataSecuritySettings").val(sldPresetsval);
        slider.noUiSlider.set(sldPresetsval);

    }
    // change event for the slider
    slider.noUiSlider.on('change', function (values, handle) {
        var hdnDataSecuritySettings = document.getElementById('hdnDataSecuritySettings');
        lblDataSecuritySettings.innerHTML = values[handle];
        $("#hdnDataSecuritySettings").val(values[handle]);
        $("#form_SystemSettings").submit();
    });


    $("#isCustomSettings").click(function () {
        $("#form_SystemSettings").submit();
    });
});
// Cancel Click and Reset the data 
$("body").on('click', '#btnCancel', function () {
    $.ajax({
        type: "Get",
        url: "/SystemSettings/Index/",
        dataType: "html",
        contentType: "application/json",
        success: function (data) {
            location.href = "/SystemSettings";
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
