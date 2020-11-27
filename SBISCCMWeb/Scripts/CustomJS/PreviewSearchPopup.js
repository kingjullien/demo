function SearchPreviewMatchData() {
    $("#MultipleConfidenceCode").multiselect("refresh");

    var value = $('#MultipleConfidenceCode').val();
    if (value != null) {
        value = value.toString();
    } 
    //$('#ConfidenceCode').val(value);
    var search = {
        'SrcRecordId': $("#txtSrcRecID").val(),
        'IsExactMatch': $("#IsExactMatch").is(":checked"),
        'LobTag': $("#LobTag").val(),
        'Tag': $("#Tag").val(),
        'ImportProcess': $("#ImportProcess").val(),
        'ConfidenceCode': value,
        'AcceptedBy': $('#AcceptedBy').val(),
        'IsClear': false
    };

    parent.CallbackSearchPreviewMatchData(search, false);
 
}