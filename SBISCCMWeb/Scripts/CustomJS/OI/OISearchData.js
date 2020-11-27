function ValidationOISearch() {
    var cnt = 0;
    var CompanyName = $("#CompanyName").val();
    var Country = $("#Country").val();

    if (CompanyName.trim() == "") {
        cnt++;
        $("#spnCompany").show();
    } else {
        $("#spnCompany").hide();
    }

    if (Country.trim() == "") {
        cnt++;
        $("#spnCountry").show();
    } else {
        $("#spnCountry").hide();
    }


    if (cnt > 0) {
        return false;
    }
    else {
        return true;
    }

}

$(document).on('click', '#btnOIAltFieldsSearch', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/OISearchData/OISearchAltfields",
        dataType: 'HTML',
        success: function (data) {
            $("#OISearchByAltfieldsModalMain").html(data);
            DraggableModalPopup("#OISearchByAltfieldsModal");
        }
    });
});


function SearchAltOrb(OrbNum, Country) {
    $('#divProgress').show();
    var QueryString = "OrbNum:" + OrbNum + "@#$Country:" + Country;
    var Url = "/OISearchData/OIAltSearchOrbNumber?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    var token = $('input[name="__RequestVerificationToken"]').val();
    CallSearch(Url);
}
function SearchAltDomain(Domain) {
    $('#divProgress').show();
    var QueryString = "Domain:" + Domain;
    var Url = "/OISearchData/OIAltSearchDomain?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    CallSearch(Url);
}
function SearchAltEmail(Email) {
    $('#divProgress').show();
    var QueryString = "Email:" + Email;
    var Url = "/OISearchData/OIAltSearchEmail?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    CallSearch(Url);
}
function SearchAltEIN(EIN) {
    $('#divProgress').show();
    var QueryString = "EIN:" + EIN;
    var Url = "/OISearchData/OIAltSearchEIN?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    CallSearch(Url);
}
function CallSearch(Url) {
    $.ajax({
        type: "POST",
        url: Url,
        success: function (data) {
            $('#divPartialOISearchData').html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    setTimeout(function () { $('#divProgress').hide(); }, 10000);
}
$(function () {
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
        },
        events: {
            show: function (opt) {
                setTimeout(function () {
                    opt.$menu.find('.context-menu-disabled > span').attr('title', txtActivateFeature);
                }, 50);
            }
        },
        items: {
            "AddCompany": {
                name: addMatchAsCompany, callback: function () {
                    var orb_num = $(this).attr("Id");
                    var MatchString = $(this).attr("data-val");
                    var QueryString = "orb_num:" + orb_num + "@#$MatchString:" + MatchString;
                    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                    $.ajax({
                        type: "POST",
                        url: "/OISearchData/FillMatchData",
                        //data: { orb_num: orb_num, MatchString: MatchString },
                        data: JSON.stringify({ orb_num: orb_num, MatchString: MatchString }),
                        headers: { "__RequestVerificationToken": token },
                        dataType: "json",
                        contentType: "application/json",
                        async: false,
                        success: function (data) {
                            if (data.result) {
                                $("#OISearchDataAddCompanyModalMain").html(data.htmlData);
                                DraggableModalPopup("#OISearchDataAddCompanyModal");
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }
            }
        }
    });

    $('.context-menu-one').on('click', function (e) {
        return 'context-menu-icon context-menu-icon-quit';
    })
});
