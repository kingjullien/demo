function onLoadReviewFilter() {
    var ConfidenceCode = $("#ReviewDataFilterModal #ConfidenceCode").val().split(',');

    $("#MultipleConfidenceCode option").each(function () {
        for (var i = 0; i < ConfidenceCode.length; i++) {
            if ($(this).val() == ConfidenceCode[i]) {
                $(this).attr("selected", "selected");
            }
        }
    });

    $('#MultipleConfidenceCode').multiselect({
        includeSelectAllOption: true
        , maxHeight: 150,
    });
}
$("body").on("click", "#btnReviewDataSearch", function () {
    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
    var pagevalue = $("#pagevalue").val();
    var CountryGroup = $("#CountryGroup").val();
    var Tags = $("#Tags").val();
    if (Tags == undefined) {
        Tags = "";
    }
    var OrderBy = $("#OrderBy").val();
    var Export = false;
    var ConfidenceCode = $('#MultipleConfidenceCode').val();
    if (ConfidenceCode != null) {
        ConfidenceCode = ConfidenceCode.toString();
    } else {
        ConfidenceCode = "ALL";
    }
    var formData = new FormData();
    formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());
    formData.append('TopMatchCandidate', TopMatchCandidate);
    formData.append('pagevalue', pagevalue);
    formData.append('CountryGroup', CountryGroup);
    formData.append('Tags', Tags);
    formData.append('ConfidenceCode', ConfidenceCode);
    formData.append('OrderBy', OrderBy);
    formData.append('Export', Export);
    formData.append('IsLoad', false);
    callBackReViewSearch(formData, false);
});

function ClearReviewData() {
    var formData = new FormData();
    formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());
    callBackReViewSearch(formData, true);
}
