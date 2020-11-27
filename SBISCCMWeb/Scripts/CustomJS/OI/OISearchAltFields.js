$(document).on("click", "#SearchAltFields", function () {
    var SearchName = $('input[name=rdSearchAltField]:checked').val();
    var cnt = 0;
    if (SearchName == "OrbNum") {
        var Country = $('#OrBNumCountry').val();
        var OrbNum = $("#txtOrBNumber").val();
        if (OrbNum == "") {
            $("#spnOrbNumber").show();
            cnt++;
        }
        else {
            $("#spnOrbNumber").hide();
        }
        if (cnt > 0) {
            return false;
        }
        parent.SearchAltOrb(OrbNum, Country);
    }
    else if (SearchName == "Domain") {
        var Domain = $("#txtOIDomain").val();
        if (Domain == "") {
            $("#spnOIDomain").show();
            cnt++;
        }
        else {
            $("#spnOIDomain").hide();
        }
        if (cnt > 0) {
            return false;
        }
        parent.SearchAltDomain(Domain);
    }
    else if (SearchName == "Email") {
        var Email = $("#txtOIEmail").val();
        if (Email == "") {
            $("#spnOIEmail").show();
            cnt++;
        }
        else {
            $("#spnOIEmail").hide();
        }
        if (cnt > 0) {
            return false;
        }
        parent.SearchAltEmail(Email);
    }
    else if (SearchName == "EIN") {
        var EIN = $("#txtOIEIN").val();
        if (EIN == "") {
            $("#spnOIEIN").show();
            cnt++;
        }
        else {
            $("#spnOIEIN").hide();
        }
        if (cnt > 0) {
            return false;
        }
        parent.SearchAltEIN(EIN);
    }
    $("#OISearchByAltfieldsModal").modal("hide");
});

$('body').on('change', 'input[name=rdSearchAltField]', function () {
    $("#txtOrBNumber").val("");
    $("#txtOIDomain").val("");
    $("#txtOIEmail").val("");
    $("#txtOIEIN").val("");
    $("#spnOrbNumber").hide();
    $("#spnOIDomain").hide();
    $("#spnOIEmail").hide();
    $("#spnOIEIN").hide();
    $("#spnOrBNumCountry").hide();
    $("#OrBNumCountry").val("");
    $("#DomainCountry").val("");
    $("#EmailCountry").val("");
    $("#EINCountry").val("");

    var SearchName = $('input[name=rdSearchAltField]:checked').val();
    if (SearchName == "OrbNum") {
        $(".divSearchOrbNumber").show();
        $(".divSearchOIDomain").hide();
        $(".divSearchOIEmail").hide();
        $(".divSearchOIEIN").hide();
    }
    else if (SearchName == "Domain") {
        $(".divSearchOrbNumber").hide();
        $(".divSearchOIDomain").show();
        $(".divSearchOIEmail").hide();
        $(".divSearchOIEIN").hide();
    }
    else if (SearchName == "Email") {
        $(".divSearchOrbNumber").hide();
        $(".divSearchOIDomain").hide();
        $(".divSearchOIEmail").show();
        $(".divSearchOIEIN").hide();
    }
    else if (SearchName == "EIN") {
        $(".divSearchOrbNumber").hide();
        $(".divSearchOIDomain").hide();
        $(".divSearchOIEmail").hide();
        $(".divSearchOIEIN").show();
    }
});