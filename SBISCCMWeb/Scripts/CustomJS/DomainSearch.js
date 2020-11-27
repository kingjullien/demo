$("body").on('click', '#btnSearchDomain', function () {
    var searchType = $('input[name=DominorEmailSearch]:checked').val();
    var SrcRecId = $("#SrcRecId").val();
    var InputId = $("#InputId").val();

    if (searchType == "duns") {
        var DUNSNO = $("#txtDUNSNo").val();
        var Country = $("#Country").val();
        if (DUNSNO == "") {
            $("#spnDUNS").show();
            return false;
        } else {
            $("#spnDUNS").hide();
        }
        parent.updateSearchData(DUNSNO, Country, SrcRecId, InputId);
    }
    if (searchType == "Domain" || searchType == "Email") {
        var searchValue = $("#txtDominorEmail").val();
        var IsCleanSearch = $("#IsCleanSearch").val();
        var Country = $("#Country").val();
        if (searchValue == "") {
            $("#spnDominorEmail").show();
            return false;
        }
        parent.DomainOrEmailSearchData(searchValue, searchType, IsCleanSearch, Country, SrcRecId, InputId);
    }
    if (searchType == "Registration Number") {
        var RegistrationNoValue = $("#txtRegistrationNo").val();
        var IsCleanSearch = $("#IsCleanSearch").val();
        var Country = $("#CountryRegistration").val();
        if (RegistrationNoValue == "" && Country == "") {
            $("#spnRegistrationNumber").show();
            $("#spnRegistrationCountry").show();
            return false;
        } else {
            $("#spnRegistrationNumber").hide()
            $("#spnRegistrationCountry").hide()
        }
        if (RegistrationNoValue == "") {
            $("#spnRegistrationNumber").show();
            return false;
        }
        else {
            $("#spnRegistrationNumber").hide()
        }
        if (Country == "") {
            $("#spnRegistrationCountry").show();
            return false;
        } else {
            $("#spnRegistrationCountry").hide()
        }
        parent.RegistrationNumberSearchData(RegistrationNoValue, searchType, IsCleanSearch, Country, SrcRecId, InputId);
    }
    $("#SearchByAltFieldsModal").modal("hide");
});

$('body').on('change', '.rButtonDominorEmailSearch', function () {

    var value = $(this).val();
    $("#spnDUNS").hide();
    $("#spnDominorEmail").hide();
    $("#spnRegistrationCountry").hide();
    $("#spnRegistrationNumber").hide();
    $("#Country").val('');
    if (value == "duns") {
        $(".searchDUNS").show();
        $(".searchDomainOrEmail").hide();
        $(".searchRegistrationNumber").hide();
        $("#txtDominorEmail").val('');
        $("#txtRegistrationNo").val('');
    }
    if (value == "Domain" || value == "Email") {
        $(".searchDomainOrEmail").show();
        $(".searchDUNS").hide();
        $(".searchRegistrationNumber").hide();
        $("#txtDUNSNo").val('');
        $("#txtRegistrationNo").val('');
        if (value == "Email") {
            $("#txtDominorEmail").val('');
            $("#txtDominorEmail").attr("placeholder", searchByEmail);
        }
        else {
            $("#txtDominorEmail").attr("placeholder", searchByDomain);
        }
    }
    if (value == "Registration Number") {
        $(".searchDomainOrEmail").hide();
        $(".searchDUNS").hide();
        $(".searchRegistrationNumber").show();
        $("#txtDUNSNo").val('');
        $("#txtDominorEmail").val('');
    }

});


// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyUp Event)
$('#txtDUNSNo').keyup(function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;
});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for Chane Event)
$('#txtDUNSNo').on('change', function (e, node) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyPress Event)
$('#txtDUNSNo').keypress(function (e) {

    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});
//only accepted numeric values
$("body").on('blur', '#txtDUNSNo', function () {
    var text = $(this).val();
    if (text != "") {
        if (!$.isNumeric(text)) {
            $(this).val("");
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success", onlyNumbersAllowed, true);
            
        }
    }
});