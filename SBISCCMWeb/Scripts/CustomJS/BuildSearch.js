$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $("body").keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            SearchData();
        }
    });

    if ($(".EnhanceFilter ul.nav-pills li a i").attr("class") == "fa fa-minus") {
        $(".EnhanceFilter ul.nav-pills li a").click();
    }


    if ($(".searchResult ul.nav-pills li a i").attr("class") == "fa fa-plus") {
        $(".searchResult ul.nav-pills li a").click();
        $("#divGrid").html('');
        $("#divGrid").html('<table class=""><td colspan="10" class="noContain">' + noDataAreAvailable + '</td></table>');
    }

    $('body').on('show.bs.collapse', '.collapse', function () {
        $('.partialrow').each(function () {
            $(this).removeClass("current");
        });
        $(".panel-collapse.in").collapse('hide');
        $('.trMatchedItemView').each(function () {
            $('.panel-heading a').addClass("collapsed");
            $(this).hide();
        });
        $(this).parents('tr').show();
        $(this).parents('tr').prev().addClass("current");
    });
    // toggle event for Child Data Hide in Match Data Table on "-" icon
    $('body').on('hide.bs.collapse', '.collapse', function () {
        $(this).parents('tr').hide();
    });
});

function SearchData() {
    $("#Request_locationRadius_lat").css("border", "1px solid #ccc");
    $("#Request_locationRadius_lon").css("border", "1px solid #ccc");
    $("#Request_locationRadius_radius").css("border", "1px solid #ccc");
    $("#Request_locationRadius_unit").css("border", "1px solid #ccc");

    $("#spSelectMaxMinEmp").hide();
    $("#spRequiredStandAloneParam").hide();
    $("#spSearchTermMinLength").hide();
    $("#spAddressMinLength").hide();

    var request = {
        'searchTerm': $("#Request_searchTerm").val(),
        'countryISOAlpha2Code': $("#Country").val(),
        'duns': $("#Request_duns").val(),
        'isOutOfBusiness': $("#IsOutOfBusiness").val(),
        'isMarketable': $("#IsMarketable").val(),
        'isMailUndeliverable': $("#IsMailUndeliverable").val(),
        'usSicV4': $("#txtusSicV4").val().trim() != '' ? $("#txtusSicV4").val().split(',') : null,
        'yearlyRevenue': {
            'minimumValue': $("#MinimumValueFinanceInfo").val(),
            'maximumValue': $("#MaximumValueFinanceInfo").val()
        },
        'isTelephoneDisconnected': $("#IsTelephoneDisconnected").val(),
        'telephoneNumber': $("#TelephoneNumber").val(),
        'domain': $("#Domain").val(),
        'registrationNumbers': $("#txtRegNo").val().trim() != '' ? $("#txtRegNo").val().split(',') : null,
        'businessEntityType': null,
        'addressLocality': $("#Request_addressLocality").val(),
        'addressRegion': $("#Request_addressRegion").val(),
        'streetAddressLine1': $("#Request_streetAddressLine1").val(),
        'postalCode': $("#Request_postalCode").val(),
        'locationRadius': {
            'lat': $("#Request_locationRadius_lat").val(),
            'lon': $("#Request_locationRadius_lon").val(),
            'radius': $("#Request_locationRadius_radius").val(),
            'unit': $("#Request_locationRadius_unit").val(),
        },
        'primaryName': $("#Request_primaryName").val(),
        'tradeStyleName': $("#TradeStyleName").val(),
        'tickerSymbol': $("#TickerSymbol").val(),
        'familytreeRolesPlayed': null,
        'globalUltimateFamilyTreeMembersCount': {
            'minimumValue': $("#MinimumValueLinkage").val(),
            'maximumValue': $("#MaximumValueLinkage").val()
        },
        'numberOfEmployees': {
            'informationScope': $("#Request_numberOfEmployees_informationScope").val(),
            'minimumValue': $("#Request_numberOfEmployees_minimumValue").val(),
            'maximumValue': $("#Request_numberOfEmployees_maximumValue").val()
        }
    };

    var IsValid = true;


    checkValidation();

    if ($("#Request_searchTerm").val() == '' && $("#Request_duns").val() == ''
        && $("#TradeStyleName").val() == ''
        && $("#TickerSymbol").val() == ''
        && $("#TelephoneNumber").val() == ''
        && $("#Domain").val() == ''
        && $("#Request_streetAddressLine1").val() == ''
        && $("#Request_primaryName").val() == ''
        && $("#txtusSicV4").val() == ''
        && $("#txtRegNo").val() == ''
        && ($("#Request_locationRadius_lat").val() == '' && $("#Request_locationRadius_lon").val() == '' && $("#Request_locationRadius_radius").val() == '' && $("#Request_locationRadius_unit").val() == '')) {
        var str = standaloneParameter1 + " <strong> " + standaloneParameter2 + " </strong> " + standaloneParameter3;
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", str, false);

        return;
    }

    if ($("#TradeStyleName").val() != '') {
        if ($("#TradeStyleName").val().length < 2) {
            $("#spTradeStyleLength").show();
            IsValid = false;
        }
    }
    else {
        $("#spTradeStyleLength").hide();
    }


    if ($("#TelephoneNumber").val() != '') {
        if ($("#TelephoneNumber").val().length < 5) {
            $("#spTelephoneNumber").show();
            IsValid = false;
        }
    }
    else {
        $("#spTelephoneNumber").hide();
    }


    if ($("#Request_searchTerm").val() != '') {
        if ($("#Request_searchTerm").val().length < 2) {
            $("#spSearchTermMinLength").show();
            IsValid = false;
        }
    }
    else {
        $("#spSearchTermMinLength").hide();
    }

    if ($("#Request_streetAddressLine1").val() != '') {
        if ($("#Request_streetAddressLine1").val().length < 3) {
            $("#spAddressMinLength").show();
            IsValid = false;
        }
    }
    if ($("#Domain").val() != '') {
        if ($("#Domain").val().length < 4) {
            $("#spDomain").show();
            IsValid = false;
        }
    }

    if ($("#TradeStyleName").val() != '') {
        if ($("#TradeStyleName").val().length < 2) {
            $("#spTradeStyleLength").show();
            IsValid = false;
        }
    }


    if ($("#MinimumValueFinanceInfo").val() != '') {
        if ($("#MinimumValueFinanceInfo").val() < 1 || $("#MinimumValueFinanceInfo").val() > 100000000000000) {
            $("#spMinimumValueFinanceInfoLength").show();
            IsValid = false;
        }
        else {
            $("#spMinimumValueFinanceInfoLength").hide();
        }
    } else {
        $("#spMinimumValueFinanceInfoLength").hide();
    }
    if ($("#MaximumValueFinanceInfo").val() != '') {
        if ($("#MaximumValueFinanceInfo").val() < 1 || $("#MaximumValueFinanceInfo").val() > 100000000000000) {
            $("#spMaximumValueFinanceInfoLength").show();
            IsValid = false;
        } else {
            $("#spMaximumValueFinanceInfoLength").hide();
        }
    } else {
        $("#spMaximumValueFinanceInfoLength").hide();
    }
    if ($("#Request_numberOfEmployees_minimumValue").val() != '') {
        if ($("#Request_numberOfEmployees_minimumValue").val() < 1 || $("#Request_numberOfEmployees_minimumValue").val() > 999999) {
            $("#spMinEmployeeLength").show();
            IsValid = false;
        } else {
            $("#spMinEmployeeLength").hide();
        }
    } else {
        $("#spMinEmployeeLength").hide();
    }
    if ($("#Request_numberOfEmployees_maximumValue").val() != '') {
        if ($("#Request_numberOfEmployees_maximumValue").val() < 1 || $("#Request_numberOfEmployees_maximumValue").val() > 999999) {
            $("#spMaxEmployeeLength").show();
            IsValid = false;
        } else {
            $("#spMaxEmployeeLength").hide();
        }
    } else {
        $("#spMaxEmployeeLength").hide();
    }
    if ($("#MinimumValueLinkage").val() != '') {
        if ($("#MinimumValueLinkage").val() < 1 || $("#MinimumValueLinkage").val() > 999999) {
            $("#spMinValueLinkage").show();
            IsValid = false;
        } else {
            $("#spMinValueLinkage").hide();
        }
    } else {
        $("#spMinValueLinkage").hide();
    }
    if ($("#MaximumValueLinkage").val() != '') {
        if ($("#MaximumValueLinkage").val() < 1 || $("#MaximumValueLinkage").val() > 999999) {
            $("#spMaxValueLinkage").show();
            IsValid = false;
        } else {
            $("#spMaxValueLinkage").hide();
        }
    } else {
        $("#spMaxValueLinkage").hide();
    }


    if (request.locationRadius.lat != '' || request.locationRadius.lon != '' || request.locationRadius.radius != '' || request.locationRadius.unit != '') {

        if (request.locationRadius.lat == '') {
            $("#Request_locationRadius_lat").css("border", "1px solid red");
            IsValid = false;
        }

        if (request.locationRadius.lon == '') {
            $("#Request_locationRadius_lon").css("border", "1px solid red");
            IsValid = false;
        }

        if (request.locationRadius.radius == '') {
            $("#Request_locationRadius_radius").css("border", "1px solid red");
            IsValid = false;
        }

        if (request.locationRadius.unit == '') {
            $("#Request_locationRadius_unit").css("border", "1px solid red");
            IsValid = false;
        }
    }

    if (request.numberOfEmployees != null) {
        if (request.numberOfEmployees.informationScope != null && request.numberOfEmployees.informationScope != '') {
            if (request.numberOfEmployees.minimumValue == '' && request.numberOfEmployees.maximumValue == '') {
                $("#spSelectMaxMinEmp").html("Please enter Min or Max Employee.");
                $("#spSelectMaxMinEmp").show();
                IsValid = false;
            }
        }

        if ((request.numberOfEmployees.minimumValue != '' || request.numberOfEmployees.maximumValue != '') && (request.numberOfEmployees.informationScope == '')) {
            $("#spSelectMaxMinEmp").html("Please select Information Scope.");
            $("#spSelectMaxMinEmp").show();
            IsValid = false;
        }
    }

    if (IsValid) {

        var search = {
            'Request': request,
            'NoOfRecored': $("#NoOfRecored").val()
        };

        $("#Request_locationRadius_lat").css("border", "1px solid #ccc");
        $("#Request_locationRadius_lon").css("border", "1px solid #ccc");
        $("#Request_locationRadius_radius").css("border", "1px solid #ccc");
        $("#Request_locationRadius_unit").css("border", "1px solid #ccc");

        $("#spSelectMaxMinEmp").hide();
        $("#spRequiredStandAloneParam").hide();
        $("#divGrid").html('');
        $("#spSearchTermMinLength").hide();
        $("#spAddressMinLength").hide();
        $("#spTradeStyleLength").hide();
        $("#spMaximumValueFinanceInfoLength").hide();
        $("#spMinimumValueFinanceInfoLength").hide();
        $("#spMinEmployeeLength").hide();
        $("#spMaxEmployeeLength").hide();
        $("#spMinValueLinkage").hide();
        $("#spMaxValueLinkage").hide();
        $("#spDomain").hide();
        $("#spTelephoneNumber").hide();

        AjaxPostWithJsonObjectCall("/BuildList/Search/", JSON.stringify(search), function (response) {
            if (response.Success) {
                $("#divGrid").html('');
                $("#divGrid").html(response.ResponseString);
                if ($(".searchResult ul.nav-pills li a i").attr("class") == "fa fa-plus") {
                    $(".searchResult ul.nav-pills li a").click();
                }
                BindTab();
            }
            else {
                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                ShowMessageNotification("success", response.ResponseString, false);

                $("#divGrid").html('<table class=""><td colspan="10" class="noContain">' + noDataAreAvailable + '</td></table>');
                if ($(".searchResult ul.nav-pills li a i").attr("class") == "fa fa-plus") {
                    $(".searchResult ul.nav-pills li a").click();
                }
            }
        });
    }
}

function Export() {
    if ($(".divBuildSearch").length > 0) {
        var strurl = "/BuildList/ExportExcel";
        window.location.href = strurl;
    }
    else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", noDataFound, false);
    }
}

$('body').on('click', '.btnSearchHistory', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/BuildList/GetSearchHistory",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#GetSearchHistoryModalMain").html(data);
            DraggableModalPopup("#GetSearchHistoryModal");
            InitDataTable(".MatchedItems", [5, 10, 15], false, ['0', 'asc']);
        }
    });
    return false;
});


function ClosePopp(id) {
    window.parent.$(".loaderMain").show()
    window.parent.$(".loaderMain").css("z-index", "9999999");
    $("#GetSearchHistoryModal").modal("hide");
    AjaxPostWithJsonObjectCall("/BuildList/ViewHistory", JSON.stringify({ Id: id }), function (response) {
        window.parent.$(".loaderMain").hide()
        if (response.Success) {
            var reqJson = jQuery.parseJSON(response.Object[0].RequestJson);
            var resJson = JSON.stringify(JSON.parse(response.Object[0].ResponseJson), null, '\t');
            if (reqJson != null) {
                window.parent.$("#Request_searchTerm").val(reqJson.Request.searchTerm);
                window.parent.$("#Country").val(reqJson.Request.countryISOAlpha2Code);
                window.parent.$("#Request_duns").val(reqJson.Request.duns);
                window.parent.$("#Request_addressLocality").val(reqJson.Request.addressLocality);
                window.parent.$("#Request_addressRegion").val(reqJson.Request.addressRegion);
                window.parent.$("#Request_streetAddressLine1").val(reqJson.Request.streetAddressLine1);
                window.parent.$("#Request_postalCode").val(reqJson.Request.postalCode);
                window.parent.$("#TradeStyleName").val(reqJson.Request.tradeStyleName);
                window.parent.$("#TickerSymbol").val(reqJson.Request.tickerSymbol);
                if (reqJson.Request.isOutOfBusiness != null && (reqJson.Request.isOutOfBusiness)) {
                    window.parent.$("#IsOutOfBusiness").val("true");
                }
                else if (reqJson.Request.isOutOfBusiness != null && (!reqJson.Request.isOutOfBusiness)) {
                    window.parent.$("#IsOutOfBusiness").val("false");
                }
                else {
                    window.parent.$("#IsOutOfBusiness").val("null");
                }

                if (reqJson.Request.isMarketable != null && (reqJson.Request.isMarketable)) {
                    window.parent.$("#IsMarketable").val("true");
                }
                else if (reqJson.Request.isMarketable != null && (!reqJson.Request.isMarketable)) {
                    window.parent.$("#IsMarketable").val("false");
                }
                else {
                    window.parent.$("#IsMarketable").val("null");
                }

                if (reqJson.Request.isMailUndeliverable != null && (reqJson.Request.isMailUndeliverable)) {
                    window.parent.$("#IsMailUndeliverable").val("true");
                }
                else if (reqJson.Request.isMailUndeliverable != null && (!reqJson.Request.isMailUndeliverable)) {
                    window.parent.$("#IsMailUndeliverable").val("false");
                }
                else {
                    window.parent.$("#IsMailUndeliverable").val("null");
                }

                if (reqJson.Request.isTelephoneDisconnected != null && (reqJson.Request.isTelephoneDisconnected)) {
                    window.parent.$("#IsTelephoneDisconnected").val("true");
                }
                else if (reqJson.Request.isTelephoneDisconnected != null && (!reqJson.Request.isTelephoneDisconnected)) {
                    window.parent.$("#IsTelephoneDisconnected").val("false");
                }
                else {
                    window.parent.$("#IsTelephoneDisconnected").val("null");
                }

                if (reqJson.Request.yearlyRevenue != null) {
                    window.parent.$("#MinimumValueFinanceInfo").val(reqJson.Request.yearlyRevenue.minimumValue);
                    window.parent.$("#MaximumValueFinanceInfo").val(reqJson.Request.yearlyRevenue.maximumValue);
                }
                window.parent.$("#TelephoneNumber").val(reqJson.Request.telephoneNumber);
                window.parent.$("#Domain").val(reqJson.Request.domain);
                if (reqJson.Request.globalUltimateFamilyTreeMembersCount != null) {
                    window.parent.$("#MinimumValueLinkage").val(reqJson.Request.globalUltimateFamilyTreeMembersCount.minimumValue);
                    window.parent.$("#MaximumValueLinkage").val(reqJson.Request.globalUltimateFamilyTreeMembersCount.maximumValue);
                }

                if (reqJson.Request.locationRadius != null) {
                    window.parent.$("#Request_locationRadius_lat").val(reqJson.Request.locationRadius.lat);
                    window.parent.$("#Request_locationRadius_lon").val(reqJson.Request.locationRadius.lon);
                    window.parent.$("#Request_locationRadius_radius").val(reqJson.Request.locationRadius.radius);
                    window.parent.$("#Request_locationRadius_unit").val(reqJson.Request.locationRadius.unit);
                }
                window.parent.$("#Request_primaryName").val(reqJson.Request.primaryName);
                if (reqJson.Request.numberOfEmployees != null) {
                    window.parent.$("#Request_numberOfEmployees_informationScope").val(reqJson.Request.numberOfEmployees.informationScope);
                    window.parent.$("#Request_numberOfEmployees_minimumValue").val(reqJson.Request.numberOfEmployees.minimumValue);
                    window.parent.$("#Request_numberOfEmployees_maximumValue").val(reqJson.Request.numberOfEmployees.maximumValue);
                }
                window.parent.$("#NoOfRecored").val(reqJson.NoOfRecored)


                window.parent.$("#divGrid").html('');
                if (response.ResponseString != '' || response.ResponseString != null) {
                    window.parent.$("#divGrid").html(response.ResponseString);
                    BindTab();
                }
                else {
                    window.parent.$("#divGrid").html('<table class=""><td colspan="10" class="noContain"> ' + noDataAreAvailable + ' </td></table>');
                    if (window.parent.$(".searchResult ul.nav-pills li a i").attr("class") == "fa fa-plus") {
                        window.parent.$(".searchResult ul.nav-pills li a").click();
                    }
                }

                if (reqJson.Request.registrationNumbers != null && reqJson.Request.registrationNumbers != '') {
                    $("#txtRegNo").val(reqJson.Request.registrationNumbers.toString());
                }
                else {
                    $("#txtRegNo").val('');
                }

                if (reqJson.Request.usSicV4 != null && reqJson.Request.usSicV4 != '') {
                    $("#txtusSicV4").val(reqJson.Request.usSicV4.toString());
                }
                else {
                    $("#txtusSicV4").val('');
                }

                window.parent.$.magnificPopup.close();
            }
        } else {
        }
    });
}

$("body").on('change', '.pagevalueChangeHistory', function () {
    var pagevalue = $(this)[0].value;
    window.parent.$('#divProgress').show();
    var url = '/BuildList/HistoryIndex/' + "?pagevalue=" + pagevalue;
    window.parent.$("#divHistory").load(url, function () {
        window.parent.$('#divProgress').hide();
    });
});

$("body").on('change', '.pagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    $('#divProgress').show();
    var url = '/BuildList/Index/' + "?pagevalue=" + pagevalue;
    $("#divBuildList").load(url, function () {
        BindTab();
        $('#divProgress').hide();
    });
});

function OnSuccess() {
    BindTab();
}

function BindTab() {
    $('.tabs').each(function (i, el) {
        $(el).tabs();
    });
}
//-------------------------------------------------------------------
//                      Add usSicV4
//-------------------------------------------------------------------
$('body').on('click', '#btnAddusSicv4', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/BuildList/AddusSicv4",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#AddusSicv4ModalMain").html(data);
            DraggableModalPopup("#AddusSicv4Modal");
        }
    });
    return false;
});

function AddusSicV4() {
    var iframe = $('#AddusSicv4Modal').contents();
    if (iframe.find("#txtPopupusSicV4").val() != '' && $.trim(iframe.find("#txtPopupusSicV4").val()).length > 0) {
        var tblusSicV4 = iframe.find("#tblusSicV4");

        if (iframe.find("#hdnusSicV4RowIndex").val() != '') {
            $(tblusSicV4).find("tbody").find("tr").eq(parseInt(iframe.find("#hdnusSicV4RowIndex").val())).html('<td>' + iframe.find("#txtPopupusSicV4").val() + '</td><td><a class="Edit" onclick="window.parent.EditusSicV4(this)" >Edit</a></td><td><a class="Delete" onclick="window.parent.DeleteusSicV4(this)" >Delete</a></td>')
        }
        else {
            $(tblusSicV4).find("tbody").append('<tr><td>' + iframe.find("#txtPopupusSicV4").val() + '</td><td><a class="Edit" onclick="window.parent.EditusSicV4(this)" >Edit</a></td><td><a class="Delete" onclick="window.parent.DeleteusSicV4(this)" >Delete</a></td></tr>');
        }
    }
    iframe.find("#hdnusSicV4RowIndex").val('');
    iframe.find("#txtPopupusSicV4").focus();
    iframe.find("#txtPopupusSicV4").val('');
}

function EditusSicV4(e) {
    var iframe = $('#AddusSicv4Modal').contents();
    iframe.find("#txtPopupusSicV4").val($(e).parent().parent().find("td").eq(0).text());
    iframe.find("#hdnusSicV4RowIndex").val(parseInt($(e).parent().parent().parent().children().index($(e).parent().parent())));
    iframe.find("#txtPopupusSicV4").focus();
}


function DeleteusSicV4(e) {
    var iframe = $('#AddusSicv4Modal').contents();
    iframe.find("#hdnusSicV4RowIndex").val('');
    iframe.find("#txtPopupusSicV4").val('');
    $(e).parent().parent().remove();
}

function CloseusSicV4Popup() {
    var usSicV4 = '';
    var iframe = $('#AddusSicv4Modal').contents();
    var tblusSicV4 = iframe.find("#tblusSicV4");
    $(tblusSicV4).find("tbody tr").each(function (i, el) {
        if (usSicV4.trim() != '') {
            usSicV4 += "," + $(el).find("td").eq(0).text().trim();
        }
        else {
            usSicV4 = $(el).find("td").eq(0).text().trim();
        }
    });

    $("#txtusSicV4").val(usSicV4);
}


//-------------------------------------------------------------------
//                      Add Registration no
//-------------------------------------------------------------------
$('body').on('click', '#btnAddRegNo', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/BuildList/AddRegistration",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#AddRegistrationNumberModalMain").html(data);
            DraggableModalPopup("#AddRegistrationNumberModal");
        }
    });
    return false;
});

function AddRegNo() {
    var iframe = $('#AddRegistrationNumberModal').contents();
    if (iframe.find("#txtPopupRegNo").val() != '' && $.trim(iframe.find("#txtPopupRegNo").val()).length > 0) {
        var tblRegNo = iframe.find("#tblRegNo");
        if (iframe.find("#hdnRegNoRowIndex").val() != '') {
            $(tblRegNo).find("tbody").find("tr").eq(parseInt(iframe.find("#hdnRegNoRowIndex").val())).html('<td>' + iframe.find("#txtPopupRegNo").val() + '</td><td><a class="Edit" onclick="window.parent.EditRegNo(this)" >Edit</a></td><td><a class="Delete" onclick="window.parent.DeleteRegNo(this)" >Delete</a></td>')
        }
        else {
            $(tblRegNo).find("tbody").append('<tr><td>' + iframe.find("#txtPopupRegNo").val() + '</td><td><a class="Edit" onclick="window.parent.EditRegNo(this)" >Edit</a></td><td><a class="Delete" onclick="window.parent.DeleteRegNo(this)" >Delete</a></td></tr>');
        }
    }
    iframe.find("#txtPopupRegNo").val('');
    iframe.find("#txtPopupRegNo").focus();
    iframe.find("#hdnRegNoRowIndex").val('');
}

function EditRegNo(e) {
    var iframe = $('#AddRegistrationNumberModal').contents();
    iframe.find("#txtPopupRegNo").val($(e).parent().parent().find("td").eq(0).text());
    iframe.find("#hdnRegNoRowIndex").val(parseInt($(e).parent().parent().parent().children().index($(e).parent().parent())));
    iframe.find("#txtPopupRegNo").focus();
}

function DeleteRegNo(e) {
    var iframe = $('#AddRegistrationNumberModal').contents();
    iframe.find("#hdnRegNoRowIndex").val('');
    iframe.find("#txtPopupRegNo").val('');
    $(e).parent().parent().remove();
}

function CloseRegNoPopup() {
    var regno = '';
    var iframe = $('#AddRegistrationNumberModal').contents();
    var tblRegNo = iframe.find("#tblRegNo");
    $(tblRegNo).find("tbody tr").each(function (i, el) {
        if (regno.trim() != '') {
            regno += "," + $(el).find("td").eq(0).text().trim();
        }
        else {
            regno = $(el).find("td").eq(0).text().trim();
        }
    });
    $("#txtRegNo").val(regno);
}


function ClearRegNo() {
    $("#txtRegNo").val('');
}

function ValidateLocation() {
    if ($("#Request_locationRadius_lat").val().trim() != '' ||
        $("#Request_locationRadius_lon").val().trim() != '' ||
        $("#Request_locationRadius_radius").val().trim() != '' ||
        $("#Request_locationRadius_unit").val().trim() != ''
    ) {
        if ($("#Request_locationRadius_lat").val().trim() == '') {
            $("#Request_locationRadius_lat").css("border", "1px solid red");
        }
        else {
            $("#Request_locationRadius_lat").css("border", "1px solid #ccc");
        }

        if ($("#Request_locationRadius_lon").val().trim() == '') {
            $("#Request_locationRadius_lon").css("border", "1px solid red");
        }
        else {
            $("#Request_locationRadius_lon").css("border", "1px solid #ccc");
        }

        if ($("#Request_locationRadius_radius").val().trim() == '') {
            $("#Request_locationRadius_radius").css("border", "1px solid red");
        }
        else {
            $("#Request_locationRadius_radius").css("border", "1px solid #ccc");
        }

        if ($("#Request_locationRadius_unit").val().trim() == '') {
            $("#Request_locationRadius_unit").css("border", "1px solid red");
        }
        else {
            $("#Request_locationRadius_unit").css("border", "1px solid #ccc");
        }
    }
    else {
        $("#Request_locationRadius_lat").css("border", "1px solid #ccc");
        $("#Request_locationRadius_lon").css("border", "1px solid #ccc");
        $("#Request_locationRadius_radius").css("border", "1px solid #ccc");
        $("#Request_locationRadius_unit").css("border", "1px solid #ccc");
    }
}

function codeAddress() {
    if ($("#Request_streetAddressLine1").val() != '' ||
        $("#Request_addressLocality").val() != '' ||
        $("#Request_addressRegion").val() != '' ||
        $("#Country").val() != '' ||
        $("#Request_postalCode").val() != ''
    ) {

        if ($("#Request_streetAddressLine1").val().length >= 3) {
            $("#spAddressMinLength").hide();
            var strAddress = '';
            if ($("#Request_streetAddressLine1").val() != '') {
                strAddress += $("#Request_streetAddressLine1").val().trim() + " ";
            }
            if ($("#Request_addressLocality").val() != '') {
                strAddress += $("#Request_addressLocality").val().trim() + " ";
            }
            if ($("#Request_addressRegion").val() != '') {
                strAddress += $("#Request_addressRegion").val().trim() + " ";
            }
            if ($("#Request_postalCode").val() != '') {
                strAddress += $("#Request_postalCode").val().trim() + " ";
            }
            if ($("#Country").val() != '') {
                strAddress += $("#Country").val().trim();
            }
            geocoder = new google.maps.Geocoder();

            var address = strAddress;
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    $("#Request_locationRadius_lat").val(results[0].geometry.location.lat());
                    $("#Request_locationRadius_lon").val(results[0].geometry.location.lng());
                }
                else {
                    $("#Request_streetAddressLine1").val('');

                    $("#Request_addressLocality").val('');
                    $("#Request_addressRegion").val('');
                    $("#Request_postalCode").val('');
                }
            });
        }
        else {
            $("#spAddressMinLength").show();
        }
    }
    else {
        $("#Request_streetAddressLine1").val('');

        $("#Request_addressLocality").val('');
        $("#Request_addressRegion").val('');
        $("#Request_postalCode").val('');
    }
}
$("body").on('blur', '#Request_duns', function () {
    var text = $(this).val();
    if (text != "") {
        if (!$.isNumeric(text)) {
            $(this).val("");
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success", allowOnlyNumbers, false);
        }
    }
});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyUp Event)
$('#Request_duns').keyup(function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;
});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for Chane Event)
$('#Request_duns').on('change', function (e, node) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});
// Regex validation for the HTMl tag from the Client Side and validate text contain do not post with any html code or tag(special for KeyPress Event)
$('#Request_duns').keypress(function (e) {

    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});

$(".OnlyDigit").keypress(function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});
$("body").on('blur', '.OnlyDigit', function () {
    var text = $(this).val();
    if (text != "") {
        if (!$.isNumeric(text)) {
            $(this).val("");
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success", allowOnlyNumbers, false);
        }
    }
});

function checkValidation()
{
    if ($("#MinimumValueFinanceInfo").val() != '') {
        if ($("#MinimumValueFinanceInfo").val() > 1 || $("#MinimumValueFinanceInfo").val() < 100000000000000) {
            $("#spMinimumValueFinanceInfoLength").hide();
        }
    } else {
        $("#spMinimumValueFinanceInfoLength").hide();
    }
    if ($("#MaximumValueFinanceInfo").val() != '') {
        if ($("#MaximumValueFinanceInfo").val() > 1 || $("#MaximumValueFinanceInfo").val() < 100000000000000) {
        }
    } else {
        $("#spMaximumValueFinanceInfoLength").hide();
    }
    if ($("#Request_numberOfEmployees_minimumValue").val() != '') {
        if ($("#Request_numberOfEmployees_minimumValue").val() > 1 || $("#Request_numberOfEmployees_minimumValue").val() < 999999) {
        }
    } else {
        $("#spMinEmployeeLength").hide();
    }
    if ($("#Request_numberOfEmployees_maximumValue").val() != '') {
        if ($("#Request_numberOfEmployees_maximumValue").val() > 1 || $("#Request_numberOfEmployees_maximumValue").val() < 999999) {
        }
    } else {
        $("#spMaxEmployeeLength").hide();
    }
    if ($("#MinimumValueLinkage").val() != '') {
        if ($("#MinimumValueLinkage").val() > 1 || $("#MinimumValueLinkage").val() < 999999) {
        }
    } else {
        $("#spMinValueLinkage").hide();
    }
    if ($("#MaximumValueLinkage").val() != '') {
        if ($("#MaximumValueLinkage").val() > 1 || $("#MaximumValueLinkage").val() < 999999) {
        }
    } else {
        $("#spMaxValueLinkage").hide();
    }
}