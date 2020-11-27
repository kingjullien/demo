// Display and Hide loader bar for every ajax call
$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
    triggerBubble();
    //setup_dashboard_widgets_desktop();
});
$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

// page load event and set match filter bar , set current matches and match details on page load
$(document).ready(function () {
    $("#divProgress").show();
    var slider = document.getElementById('slider-range');
    noUiSlider.create(slider, {
        start: [0],
        range: {
            'min': [0],
            'max': [10]
        },
        pips: { // Show a scale with the slider
            mode: 'steps',
            density: 10
        },
        step: 1
    });
    $("#slider-range").addClass("form-control");
    slider.noUiSlider.on('change', function (values, handle) {
        var divMinConfidentCodeVal = document.getElementById('divMinConfidentCode');
        divMinConfidentCodeVal.innerHTML = values[handle];
        $("#minConfidentCode").val(values[handle]);
        $('#form_StewardshipPortal').submit();
    });

    $("#divProgress").hide();

    $.ajax({
        type: "GET",
        url: "/UserSessionFilter/popupUserFilterJson/",
        async: false,
        success: function (data) {
            var Data = data.Data;

            //["TableCoulmnName","ServerColumnName","DropDownURL","ISDefault(use lowercase)","Show text/value(use lowercase)","Datatype(If Date)","Default selected values"]
            var defaultColArray = [];
            var notDefaultArray = [];
            defaultColArray.push(["OrderByColumn", "OrderByColumn", "/StewardshipPortal/GetOrderByColumnDD", "true", "value", "onlyselect", (Data.OrderByColumn != null && Data.OrderByColumn != "" ? Data.OrderByColumn : "SrcRecordId")]);
            if (Data.SrcRecordId != null && Data.SrcRecordId != "") {
                defaultColArray.push(["SrcRecordId", "SrcRecordId", "", "", "", "onlytext", (Data.SrcRecordId != null && Data.SrcRecordId != "" ? Data.SrcRecordId : ""), "true"]);
            }
            else {
                notDefaultArray.push(["SrcRecordId", "SrcRecordId", "", "", "", "onlytext", (Data.SrcRecordId != null && Data.SrcRecordId != "" ? Data.SrcRecordId : "")]);
            }


            if (Data.CompanyName != null && Data.CompanyName != "") {
                defaultColArray.push(["CompanyName", "CompanyName", "", "", "", "onlytext", (Data.CompanyName != null && Data.CompanyName != "" ? Data.CompanyName : ""), "true"]);
            }
            else {
                notDefaultArray.push(["CompanyName", "CompanyName", "", "", "", "onlytext", (Data.CompanyName != null && Data.CompanyName != "" ? Data.CompanyName : "")]);
            }

            if (Data.City != null && Data.City != "") {
                defaultColArray.push(["City", "City", "", "", "", "onlytext", (Data.City != null && Data.City != "" ? Data.City : ""), "true"]);
            }
            else {
                notDefaultArray.push(["City", "City", "", "", "", "onlytext", (Data.City != null && Data.City != "" ? Data.City : "")]);
            }

            if (Data.State != null && Data.State != "") {
                defaultColArray.push(["State", "State", "", "", "", "onlytext", (Data.State != null && Data.State != "" ? Data.State : ""), "true"]);
            }
            else {
                notDefaultArray.push(["State", "State", "", "", "", "onlytext", (Data.State != null && Data.State != "" ? Data.State : "")]);
            }

            if (Data.CountryISOAlpha2Code != null && Data.CountryISOAlpha2Code != "") {
                defaultColArray.push(["Country", "Country", "/StewardshipPortal/GetCountryDD", "", "text", "onlyselect", (Data.CountryISOAlpha2Code != null && Data.CountryISOAlpha2Code != "" ? Data.CountryISOAlpha2Code : ""), "true"]);
            }
            else {
                notDefaultArray.push(["Country", "Country", "/StewardshipPortal/GetCountryDD", "", "text", "onlyselect", (Data.CountryISOAlpha2Code != null && Data.CountryISOAlpha2Code != "" ? Data.CountryISOAlpha2Code : "")]);
            }

            if (Data.ImportProcess != null && Data.ImportProcess != "") {
                defaultColArray.push(["ImportProcess", "ImportProcess", "/StewardshipPortal/GetImportProcessDD", "", "text", "onlyselect", (Data.ImportProcess != null && Data.ImportProcess != "" ? Data.ImportProcess : ""), "true"]);
            }
            else {
                notDefaultArray.push(["ImportProcess", "ImportProcess", "/StewardshipPortal/GetImportProcessDD", "", "text", "onlyselect", (Data.ImportProcess != null && Data.ImportProcess != "" ? Data.ImportProcess : "")]);
            }

            if (Data.CountryGroupId != null && Data.CountryGroupId > 0) {
                defaultColArray.push(["CountryGroup", "CountryGroup", "/StewardshipPortal/GetCountryGroupDD", "", "text", "onlyselect", (Data.CountryGroupId != null && Data.CountryGroupId > 0 ? Data.CountryGroupId.toString() : ""), "true"]);
            }
            else {
                notDefaultArray.push(["CountryGroup", "CountryGroup", "/StewardshipPortal/GetCountryGroupDD", "", "text", "onlyselect", (Data.CountryGroupId != null && Data.CountryGroupId > 0 ? Data.CountryGroupId.toString() : "")]);
            }

            if (Data.Tag != null && Data.Tag != "") {
                defaultColArray.push(["Tag", "Tag", "/StewardshipPortal/GetTagsDD", "", "text", "onlyselect", (Data.Tag != null && Data.Tag != "" ? Data.Tag : ""), "true"]);
            }
            else {
                notDefaultArray.push(["Tag", "Tag", "/StewardshipPortal/GetTagsDD", "", "text", "onlyselect", (Data.Tag != null && Data.Tag != "" ? Data.Tag : "")]);
            }



            var colArray = $.merge(defaultColArray, notDefaultArray);
            //[
            //["OrderByColumn", "OrderByColumn", "/StewardshipPortal/GetOrderByColumnDD", "true", "value", "onlytext", (Data.OrderByColumn != null && Data.OrderByColumn != "" ? Data.OrderByColumn : "SrcRecordId")],
            //["SrcRecordId", "SrcRecordId", "", "", "", "onlytext", (Data.SrcRecordId != null && Data.SrcRecordId != "" ? Data.SrcRecordId : "")],
            //["CompanyName", "CompanyName", "", "", "", "onlytext", (Data.CompanyName != null && Data.CompanyName != "" ? Data.CompanyName : "")],
            //["City", "City", "", "", "", "onlytext", (Data.City != null && Data.City != "" ? Data.City : "")],
            //["State", "State", "", "", "", "onlytext", (Data.State != null && Data.State != "" ? Data.State : "")],
            //["Country", "Country", "/StewardshipPortal/GetCountryDD", "", "text", "onlyselect", (Data.CountryISOAlpha2Code != null && Data.CountryISOAlpha2Code != "" ? Data.CountryISOAlpha2Code : "")],
            //["ImportProcess", "ImportProcess", "/StewardshipPortal/GetImportProcessDD", "", "text", "onlyselect", (Data.ImportProcess != null && Data.ImportProcess != "" ? Data.ImportProcess : "")],
            //["CountryGroup", "CountryGroup", "/StewardshipPortal/GetCountryGroupDD", "", "text", "onlyselect", (Data.CountryGroupId != null && Data.CountryGroupId > 0 ? Data.CountryGroupId.toString() : "")],
            //["Tag", "Tag", "/StewardshipPortal/GetTagsDD", "", "text", "onlyselect", (Data.Tag != null && Data.Tag != "" ? Data.Tag : "")]];

            //Column array,URL for FilterData, TargetedDiv, Datatable function
            InitFilters(colArray, "/StewardshipPortal/FilterMatchData", "#MatchDataFilterContainer", "#divStewardshipPortal", "clearAllOtherData()", "equalto");
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });

    $('table.divStewardshipPortal tbody tr').each(function () {
        $(this).find('td:first').attr('title', '');
    });
});

// Comapny clear button event for filter
$("#btnClearCompanyData").click(function () {
    clearCompanyData();
});
// Comapny clear button event for Match filter
$("#btnClearMatchFilter").click(function () {
    clearAllOtherData();
});
// Accept Selected Matches button event for remove class from body and apply scroll for screen
$('body').on('click', '#btnAcceptSelectedMatches', function () {
    $('body').removeClass("nooverflow");
});
// Reject Selected Matches button event for remove class from body and apply scroll for screen
$('body').on('click', '#btnRejectSelectedMatches', function () {
    $('body').removeClass("nooverflow");
    clearAllOtherData();
});
$("#btnClose").click(function () {
    clearAllOtherData();
});
// Clear Filter Data
function clearCompanyData() {
    $("#txtScrRecId").val('');
    $("#txtCompany").val('');
    $("#txtCity").val('');
    $("select#State").prop('selectedIndex', 0);
    $("select#Country").prop('selectedIndex', 0);
}
// Clear or Filter Data for Match Grade Filter on Clear event
function clearAllOtherData() {
    $("#txtScrRecId").val('');
    $("#txtCompany").val('');
    $("#txtCity").val('');
    $("select#State").prop('selectedIndex', 0);
    $("select#Country").prop('selectedIndex', 0);
    $("select#CompanyMatch").prop('selectedIndex', 0);
    $("select#CityName").prop('selectedIndex', 0);
    $("select#StreetNo").prop('selectedIndex', 0);
    $("select#StateName").prop('selectedIndex', 0);
    $("select#StreetName").prop('selectedIndex', 0);
    $("select#PostalCode").prop('selectedIndex', 0);
    $("select#Telephone").prop('selectedIndex', 0);
    $("#txtMatchGrade").val('');
    $(".noUi-origin").css("left", "0");
    $("#divMinConfidentCode").html("0.00");
    $("#minConfidentCode").val("0.00");

    if ($('#SelectTopMatch').prop('checked') == true)
        $('#SelectTopMatch').click();

    //var slider = document.getElementById('slider-range');
    //slider.noUiSlider.set('0.00');
}
// toggle event for Child Data Show  in Match Data Table on "+" icon 
$('body').on('show.bs.collapse', '#divStewardshipPortal .collapse', function () {
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
$('body').on('hide.bs.collapse', '#divStewardshipPortal .collapse', function () {
    $(this).parents('tr').hide();
});

$('body').on('click', '#trig', function () {
    $('#colMain').toggleClass('col-md-9 col-md-12');
    $('#colPush').toggleClass('col-md-3 col-md-0');
});
// On DUNS Number Click on Match Detail Review Popup 
$('body').on('click', '.clsViewMatchedItemDetails', function () {
    $(".currentChildRow").each(function () {
        $(this).removeClass("current");
    });
    $(this).closest(".currentChildRow").addClass("current");
    $(this).closest(".currentChildRow").next().addClass("current");
    //Vijay - New code to use data from parent tr
    var row = $(this).closest('tr');
    var next = row.nextAll().eq(1);
    var prev = row.prevAll().eq(1);
    var tempNext;
    var tempPrev;
    var data = row.attr("data-val");
    var dataNext = "";
    var dataPrev = "";
    if (next.length > 0) {
        tempNext = next.attr("id").split("DUNS");
        dataNext = tempNext[0];
    }

    if (prev.length > 0) {
        tempPrev = prev.attr("id").split("DUNS");
        dataPrev = tempPrev[0];
    }

    var temp = row.attr("id").split("DUNS");
    var DUNS = temp[0];
    var InputId = temp[1];

    var TempCount = $(this).attr("id").split("@#$");
    var Count = TempCount[1];
    //Vijay - New Code ends here

    var QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:AdditionalFields' + '@#$IsPartialView:false';
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/StewardshipPortal/cShowMatchedItesDetailsView?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#MatchDetailModalMain").html(data);
            DraggableModalPopup("#MatchDetailModal");
        }
    });
    //return false;
});
// On Match Select for Company Update Model 
function CallAcceptLCMMatches(SrcId, Seqence) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/StewardshipPortal/AcceptLCMMatches/",
        data: JSON.stringify({ id: SrcId, MatchSeqence: Seqence }),
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function MatchCallAcceptLCMMatches(SrcId, Seqence, token) {
    $.ajax({
        type: "POST",
        url: "/StewardshipPortal/AcceptLCMMatches/",
        data: JSON.stringify({ id: SrcId, MatchSeqence: Seqence }),
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
// On Match Reject for Company Update Model 
function CallRejectLCMMatches(SrcId, Seqence) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/StewardshipPortal/RejectLCMMatches/",
        data: JSON.stringify({ id: SrcId, MatchSeqence: Seqence }),
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function MatchCallRejectLCMMatches(SrcId, Seqence, token) {
    $.ajax({
        type: "POST",
        url: "/StewardshipPortal/RejectLCMMatches/",
        data: JSON.stringify({ id: SrcId, MatchSeqence: Seqence }),
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
// On Match Select Button Green to Black and update Model for this changes
$('body').on('click', '.btnSelected', function () {

    var InputId = $(this).attr("id").replace("InnerMatch", "");// Take Id of the Elements
    var Seqence = $(this).attr("data-val");// Take Sequece of the match.
    $(this).removeClass("btnSelected").addClass("btnUnselected");
    parent.$("#" + InputId + "NotSelected").removeClass("SelectMathced").addClass("NotSelected");

    CallRejectLCMMatches(InputId, Seqence);
});
// On Match Select Button Black to Green and update Model for this changes
$('body').on('click', '.btnUnselected', function () {

    var InputId = $(this).attr("id").replace("InnerMatch", "");// Take Id of the Elements
    var Seqence = $(this).attr("data-val");// Take Sequece of the match.
    parent.$("#" + InputId + "RejectAll").removeClass("parentbtnMatchRejected").addClass("parentbtnUnselected");// Remove Relected Buttton Class for Match Rejectd and Add class for UnSelected and Make red border for the button
    parent.$("#" + InputId + "RejectAll").removeClass("parentbtnIdel").addClass("parentbtnUnselected");// Remove Idel class for the rejected button
    parent.$("#" + InputId + "NotSelected").removeClass("NotSelected").addClass("SelectMathced");// Add Selected icon
    $(this).closest('tr.trMatchedItemView').find(".btnSelected").removeClass("btnSelected").addClass("btnUnselected"); // Remove all selected in matches and make all umwselected matches
    $(this).removeClass("btnUnselected").addClass("btnSelected");// Mark as selected to current button
    CallAcceptLCMMatches(InputId, Seqence); // Server call for the Select match and change the model rebind 
    if ($("#Approve").val() == "False") {
        OpenStewardshipNote(InputId, Seqence, '');// Open Steward ship note.
    }
});
// While Reject button press and remove reject the comapany from the model and button make Red to idel.
$('body').on('click', '.parentbtnMatchRejected', function () {

    $(this).removeClass("parentbtnMatchRejected").addClass("parentbtnUnselected");// Remove Red color and Make red border for reject button
    var id = $(this).attr('id').replace("RejectAll", "");
    CallAcceptLCMMatches(id, 'unrejectall');// Update the Model of the company
});
// While Reject button press and  reject the comapany and button make Red and update model for this changes
$('body').on('click', '.parentbtnIdel', function () {
    var InputId = $(this).attr('id').replace("RejectAll", ""); // Get InputId for the record.
    parent.$("#" + InputId + "NotSelected").removeClass("SelectMathced").addClass("NotSelected"); // remove selected icon form the comapny data 
    parent.$("#" + InputId + "InnerMatch").each(function () {
        $(this).removeClass("btnSelected").addClass("btnUnselected");
    });// Remove all selected matched from the model
    $(this).removeClass("parentbtnIdel").addClass("parentbtnMatchRejected");
    CallAcceptLCMMatches(InputId, 'rejectall');// Update the Model of the company
    if ($("#Approve").val() == "False") {
        //Open stewardship note
        OpenStewardshipNote(InputId, '-1', '');
    }
});
// While Reject button press and  reject the comapany and button make Red and update model for this changes
$('body').on('click', '.parentbtnUnselected', function () {
    var InputId = $(this).attr('id').replace("RejectAll", "");
    parent.$("#" + InputId + "NotSelected").removeClass("SelectMathced").addClass("NotSelected");// remove selected icon form the comapny data 
    parent.$("." + InputId).each(function () {
        $(this).removeClass("btnSelected").addClass("btnUnselected");
    });// Remove all selected matches
    $(this).removeClass("parentbtnUnselected").addClass("parentbtnMatchRejected");
    CallAcceptLCMMatches(InputId, 'rejectall'); // Update the Model of the company
    if ($("#Approve").val() == "False") {
        //Open stewardship note
        OpenStewardshipNote(InputId, '-1', '');
    }
});
// While this event is only fire for open Stewardship Note when user have rights to IsApproval and Enable2state 
function OpenStewardshipNote(id, count, Note) {

    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/StewardshipPortal/OpenSearchData?InputId=' + id + '&count=' + count + '&Notes=' + Note
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupStewardShipNotes'
    });
    return false;
}
// Open Session Filter
$('body').on('click', '.userFilter', function () {
    //popup size with tag enable disabled
    var PopupClassName = "";
    if ($.IsTagsLicenseAllow.toLowerCase() == "true") {
        PopupClassName = 'popupUserSessionFilter'
    }
    else {
        PopupClassName = 'popupUserSessionFilterNoTag'
    }

    var pagevalue = $("#pagevalue").val();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/UserSessionFilter/popupUserFilter/'
        },
        callbacks: {
            close: function () {
                //location.reload();
                window.location.href = "/StewardshipPortal/Index?pagevalue=" + pagevalue;
            }
        },
        closeOnBgClick: false,
        mainClass: PopupClassName
    });
    return false;
});
// Delete Session Filter
$('body').on('click', '.DeleteFilter', function () {

    var pagevalue = $("#pagevalue").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    $(this).find("strong").removeClass("fa fa-plus");
    $(this).find("strong").removeClass("fa fa-minus");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteSessionFilter, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/StewardshipPortal/DeleteSessionFilter/",
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    success: function (data) {
                        if (data == "success") {
                            //location.reload();
                            window.parent.$.magnificPopup.close();
                            //window.location.href = "/StewardshipPortal/Index?pagevalue=" + pagevalue;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    return false;
});

// Page Value Change on Page Value DropDown
$("body").on('change', '.pagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    var url = '/StewardshipPortal/Index/' + "?pagevalue=" + pagevalue;
    $("#divStewardshipPortal").load(url, function () {
        var slider = document.getElementById('slider-range');
        slider.noUiSlider.set(0);
        setup_dashboard_widgets_desktop();
    });
    $('body').removeClass("nooverflow");
});


function pagination() {
    clearAllOtherData();
    setup_dashboard_widgets_desktop();
    $('body').removeClass("nooverflow");
}
function OnSuccess() {
    setup_dashboard_widgets_desktop();
    $('body').removeClass("nooverflow");
}
// On Match Detail Review PopUp Next Button Event
function matchItemNextClick(dunsNumb, InputId) {

    if (dunsNumb != "") {
        var currentid = "#" + dunsNumb + "DUNS" + InputId;
        var nextcurrentid = "#" + dunsNumb + "-2-" + InputId;
        $(currentid).parent().find(".currentChildRow").each(function () {
            $(this).removeClass("current");
        });
        $(currentid).prev().removeClass("current");
        $(currentid).addClass("current");
        $(nextcurrentid).addClass("current");
        //$.magnificPopup.close();
        $InnerTableId = $('#innertable' + InputId);
        $AllTr = $InnerTableId.find('tbody').find('tr');
        var index = parseInt(($(nextcurrentid).index() + 1) / 2) - 1;
        $InnerTableId.find('tbody').scrollTop(($($AllTr[0]).height() + $($AllTr[1]).height()) * index - 50);

        $('#divProgress').hide();
    }
}
// On Match Detail Review PopUp Previous Button Event
function matchItemPrevClick(dunsNumb, InputId) {

    if (dunsNumb != "") {
        var currentid = "#" + dunsNumb + "DUNS" + InputId;
        var nextcurrentid = "#" + dunsNumb + "-2-" + InputId;
        $(currentid).parent().find(".currentChildRow").each(function () {
            $(this).removeClass("current");
        });
        $(currentid).addClass("current");
        $(nextcurrentid).addClass("current");
        //$.magnificPopup.close();
        $InnerTableId = $('#innertable' + InputId);
        $AllTr = $InnerTableId.find('tbody').find('tr');
        var index = parseInt(($(nextcurrentid).index() + 1) / 2) - 1;
        $InnerTableId.find('tbody').scrollTop(($($AllTr[0]).height() + $($AllTr[1]).height()) * index - 50);

    }
}
$("body").on('click', '.sort-header', function () {
    $(this).addClass("headerSortDown");
    $('body').removeClass("nooverflow");
});
//Set Current Selected for Child Records and remove color from previous selected row 
$("body").on('click', '.currentChildRow', function () {
    // Set current row as selected and remove provious row from selection.
    //$('.currentChildRow').each(function () {
    //    $(this).removeClass("current");
    //});
    $(this).parent().parent().find('.currentChildRow').each(function () {
        $(this).removeClass("current");
    });
    $(this).addClass("current");
    var id = "#" + $(this).attr('id');
    if (id.indexOf('-2') > -1) {
        $(id.replace("-2-", "DUNS")).addClass("current");
    } else {
        $(id.replace("DUNS", "-2-")).addClass("current");
    }
});

function backToparent() {
    $.ajax({
        type: "POST",
        url: "/StewardshipPortal/GetFilteredCompanyList/",
        data: '',
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data == "success") {

            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
//Open Stewardship Note.
$("body").on('click', '.StewardNote', function () {
    var id = $(this).attr("data-id");
    var count = $(this).attr("data-count");
    var Note = $(this).attr("data-Note");
    var IsApprovalScreen = $("#Approve").val();
    if (IsApprovalScreen == "True") {
        OpenStewardshipNote(id, count, Note);
    }
});

// On Pagination ask if page isDirty.
$("body").on('click', '.pagination li', function (e) {
    var page = $(this).find('a').html();
    var sortby = $("#sortby").val();
    var sortorder = $("#sortorder").val();
    var pagevalue = $("#pagevalue").val();
    var count = 0;
    if ($('.parentbtnUnselected').length > 0 || $('.parentbtnMatchRejected').length > 0 || $('.SelectMathced').length > 0) {
        count = 1;
    }
    if (count > 0) {
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: unsavedChanges, callback: function (result) {
                if (result) {
                    var slider = document.getElementById('slider-range');
                    slider.noUiSlider.set(0);
                    $.get("/StewardshipPortal/Index/", { page: page, sortby: sortby, sortorder: sortorder, pagevalue: pagevalue }, function (data) {
                        $("#divStewardshipPortal").html(data);
                    });
                }
            }
        });
    } else {
        return true;
    }
    return false;
});

// Open Investigation popup
$('body').on('click', '.InvestigateRecords', function () {
    $.magnificPopup.open({
        items: {
            src: '/InvestigateView/InvestigateRecords'
        },
        type: 'iframe',
        closeOnBgClick: false,
        mainClass: 'popInvestigation',
        callbacks: {
            open: function () {
                setTimeout(function () {
                    if ($('.mfp-iframe').contents().find("html body #contentPopup").length > 0) {
                        $('.mfp-iframe').contents().find("html body #contentPopup").html(data);
                    }
                }, 1500);
            }
        }
    });

    return false;
});
function backToparent2() {
    $.magnificPopup.close();
}


$(function () {
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var Investigation = $("#LicenseEnableInvestigations").val();
    var APIType = $("#APITypeForInvestigation").val();
    var Compliance = $("#LicenseEnableCompliance").val();
    if (Investigation == undefined || Investigation == "False") {
        Investigation = true;
    }

    if (Investigation == "True" && APIType != "DirectPlus") {
        Investigation = true;
    }
    if (Compliance == undefined || Compliance == "False") {
        Compliance = true;
    }
    else {
        Compliance = false;
    }
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
                    var matchRecord = $(this).attr("data-val");
                    $.ajax({
                        type: "POST",
                        url: "/BadInputData/FillMatchData/",
                        data: JSON.stringify({ matchRecord: matchRecord }),
                        headers: { "__RequestVerificationToken": token },
                        dataType: "json",
                        contentType: "application/json",
                        async: false,
                        success: function (data) {
                            //popup size with tag enable disabled
                            var PopupClassName = "";
                            if ($.IsTagsLicenseAllow.toLowerCase() == "true") {
                                PopupClassName = 'popupAddressCompany'
                            }
                            else {
                                PopupClassName = 'popupAddressCompanyNoTag'
                            }
                            // Changes for Converting magnific popup to modal popup
                            $.ajax({
                                type: 'GET',
                                url: "/BadInputData/AddCompany",
                                dataType: 'HTML',
                                async: false,
                                success: function (data) {
                                    $("#divProgress").hide();
                                    $("#SearchDataAddCompanyModalMain").html(data);
                                    DraggableModalPopup("#SearchDataAddCompanyModal");
                                }
                            });
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }

            },

            "AddInvestigation": {
                name: investigateRecord, disabled: Investigation, callback: function () {
                    var matchRecord = $(this).attr("data-val");
                    var dataObj = JSON.parse(matchRecord);
                    var InputId = $(this).attr("data-InputId");
                    var SrcId = $(this).attr("data-SrcId");
                    var Duns = $(this).attr("data-Duns");
                    var Tags = $(this).attr("data-Tags");
                    var Company = dataObj.DnBOrganizationName;
                    var Street = dataObj.DnBStreetAddressLine;
                    var City = dataObj.DnBPrimaryTownName;
                    var PostalCode = dataObj.DnBPostalCode;
                    var Country = dataObj.DnBCountryISOAlpha2Code;
                    var TradeStyle = dataObj.DnBTradeStyleName;
                    var Status = dataObj.DnBOperatingStatus;
                    var QueryString = "InputId:" + InputId + "@#$SrcId:" + SrcId + "@#$Duns:" + Duns + "@#$Tags:" + Tags + "@#$Company:" + Company + "@#$Street:" + Street + "@#$City:" + City + "@#$PostalCode:" + PostalCode + "@#$Country:" + Country + "@#$TradeStyle:" + TradeStyle + "@#$Status:" + Status;
                    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

                    // Changes for Converting magnific popup to modal popup
                    $.ajax({
                        type: 'GET',
                        url: "/ResearchInvestigation/iResearchInvestigationRecordsTargeted?Parameters=" + Parameters,
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#iResearchInvestigationRecordsTargetedModalMain").html(data);
                            DraggableModalPopup("#iResearchInvestigationRecordsTargetedModal");
                            var countSubTypes = $('#SubTypes').has('option').length;
                            if (countSubTypes == 0) {
                                $(".btnShowTargetedInvestigationMsg").show();
                            }
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }

            },

            "BenificiaryDetails": {
                name: benificiaryDetails, disabled: Compliance, callback: function () {
                    var matchRecord = $(this).attr("data-val");
                    var dataObj = JSON.parse(matchRecord);
                    var Duns = $(this).attr("data-Duns");
                    var Country = dataObj.DnBCountryISOAlpha2Code;
                    var formData = new FormData();
                    formData.append('DUNSNumber', Duns);
                    formData.append('Country', Country);
                    formData.append('isModalView', true);
                    $.ajax({
                        type: 'POST',
                        url: "/BeneficialOwnership/SearchBeneficialOwnershipData",
                        data: formData,
                        //dataType: 'HTML',
                        contentType: false,
                        processData: false,
                        async: false,
                        success: function (data) {
                            $("#divBeneficialOwnershipData").html(data);
                            DraggableModalPopup("#BenificiaryDataModal");
                            InitComplianceRightClick();
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


function CloseInvestigationWindow() {
    window.parent.$.magnificPopup.close();
}

$('body').on('click', '.GoogleMapPopUp', function () {
    var matchRecord = $(this).attr("data-val");
    var QueryString = "matchRecord:" + matchRecord;
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/StewardshipPortal/GoogleMapPopUp?Parameters=" + Parameters,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#GoogleMapPopUpModalMain").html(data);
            DraggableModalPopup("#GoogleMapPopUpModal");
        }
    });
    return false;
});
$('body').on('click', '#btnAcceptRejectAll', function () {
    //popup size with tag enable disabled
    var PopupClassName = "";
    if ($.IsTagsLicenseAllow.toLowerCase() == "true") {
        PopupClassName = 'popRejectAll'
    }
    else {
        PopupClassName = 'popRejectAllNoTag'
    }

    $.ajax({
        type: "GET",
        url: '/StewardshipPortal/RejectAllRecords?IsMatchData=' + true,
        cache: false,
        async: false,
        beforeSend: function () {
        },
        success: function (data) {
            $("#divPurgeDataMain").html(data);
            DraggableModalPopup("#divPurgeDataModal");
        }
    });

    $.ajax({
        type: "GET",
        url: '/StewardshipPortal/RejectFromFile?IsPurgeData=' + false,
        cache: false,
        async: false,
        beforeSend: function () {
        },
        success: function (data) {
            $("#divPurgeFromFile").html(data);
        }
    });
});

function CloseRejectAllWindow() {
    window.location.href = "/StewardshipPortal/Index";
}

$('body').on('click', '#btnDefaultPageSize', function () {
    var pagevalue = $("#pagevalue").val();
    var QueryString = "pagevalue:" + pagevalue + "@#$Section:MatchData";

    $.ajax({
        type: "POST",
        url: "/StewardshipPortal/SaveDefaultPageSize?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
            ShowMessageNotification("success", data, false);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$('body').on('click', '#btnAcceptRejectAllFromFile', function () {
    var id = $(this).attr("id");
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/StewardshipPortal/RejectFromFile?IsPurgeData=' + false,
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupRejectPurgeData'
    });
});

$('body').on('click', '#btnPreivewMatchesData', function () {
    var matchRecord = $(this).attr("data-val");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/DNBIdentityResolution/InsertUpdateAutoAcceptance?isAutoAcceptance=" + false,
        dataType: 'HTML',
        success: function (data) {
            $("#ApplyMatchRulesModalMain").html(data);
            DraggableModalPopup("#InsertUpdateAutoAcceptanceModal");
            OnReadyInsertUpdateAutoAcceptance();
        }
    });
});


$(function () {
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var Investigation = $("#LicenseEnableInvestigations").val();
    var EnablePurgeData = $("#LicenseEnablePurgeData").val();
    var APIType = $("#APITypeForInvestigation").val();
    if (Investigation == undefined || Investigation == "False") {
        Investigation = true;
    }
    if (Investigation == "True" && APIType != "DirectPlus") {
        Investigation = true;
    }

    if (EnablePurgeData == undefined || EnablePurgeData == "False") {
        EnablePurgeData = true;
    }
    if (EnablePurgeData == "True") {
        EnablePurgeData = false;
    }
    $.contextMenu({
        selector: '.context-menu-one-purge',
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
            "Purge": {
                name: purgeRecord, disabled: EnablePurgeData, titile: "Purge", callback: function () {
                    //purges a single record from the UI (right - click UI option)
                    var SrcRecordId = $(this).attr("data-SrcRecordId");
                    var InputId = $(this).attr("id");
                    var Queue = "LCM";
                    var token = $('input[name="__RequestVerificationToken"]').val();
                    var QueryString = "InputId:" + InputId + "@#$SrcRecordId:" + SrcRecordId + "@#$Queue:" + Queue;
                    var url = '/StewardshipPortal/StewPurgeSingleRecord?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                    bootbox.confirm({
                        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: purgeRecordMatchData, callback: function (result) {
                            if (result) {
                                $.ajax({
                                    type: "POST",
                                    url: url,
                                    headers: { "__RequestVerificationToken": token },
                                    dataType: "json",
                                    contentType: "application/json; charset=UTF-8",
                                    async: false,
                                    cache: false,
                                    success: function (data) {
                                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                                        ShowMessageNotification("error", data, false, true, RefreshMatchDataGrid);
                                        //location.reload();
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                    }
                                });
                            }
                        }
                    });

                    return 'context-menu-icon context-menu-icon-quit';
                }
            },
            "AddInvestigation": {
                name: investigateRecord, disabled: Investigation, callback: function () {
                    //var matchRecord = $(this).children(".trStewardshipPortal_td10").attr("data-val")
                    var InputId = $(this).children(".trStewardshipPortal_td10").attr("data-InputId");
                    var SrcId = $(this).children(".trStewardshipPortal_td10").attr("data-SrcId");
                    var Company = $(this).children(".trStewardshipPortal_td10").attr("data-Company");
                    var StreetName = $(this).children(".trStewardshipPortal_td10").attr("data-StreetName");
                    var Address = $(this).children(".trStewardshipPortal_td10").attr("data-Address");
                    var City = $(this).children(".trStewardshipPortal_td10").attr("data-City");
                    var PostalCode = $(this).children(".trStewardshipPortal_td10").attr("data-PostalCode");
                    var CountryCode = $(this).children(".trStewardshipPortal_td10").attr("data-CountryCode");
                    var Tags = $(this).children(".trStewardshipPortal_td10").attr("data-Tags");
                    var Phone = $(this).children(".trStewardshipPortal_td10").attr("data-phone");
                    var Website = $(this).children(".trStewardshipPortal_td10").attr("data-website");
                    var QueryString = "InputId:" + InputId + "@#$SrcId:" + SrcId + "@#$Company:" + Company + "@#$StreetName:" + StreetName + "@#$Address:" + Address + "@#$City:" + City + "@#$PostalCode:" + PostalCode + "@#$CountryCode:" + CountryCode + "@#$Tags:" + Tags + "@#$Phone:" + Phone + "@#$Website:" + Website;
                    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

                    // Changes for Converting magnific popup to modal popup
                    $.ajax({
                        type: 'GET',
                        url: "/ResearchInvestigation/iResearchInvestigationRecords?Parameters=" + Parameters,
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#InvestigateModalMain").html(data);
                            DraggableModalPopup("#InvestigateModal");
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }
            },
        }
    });

    $('.context-menu-one-purge').on('click', function (e) {
        return 'context-menu-icon context-menu-icon-quit';
    })
});

$(".userFilter").popover({ trigger: "manual", html: true, animation: false })
    .on("mouseenter", function () {
        var _this = this;
        $(this).popover("show");
        $(".popover").on("mouseleave", function () {
            $(_this).popover('hide');
        });
    }).on("mouseleave", function () {
        var _this = this;
        setTimeout(function () {
            if (!$(".popover:hover").length) {
                $(_this).popover("hide");
            }
        }, 300);
    });


$('body').on('click', '#btnAcceptFromFile', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/StewardshipPortal/AcceptFromFile?IsPurgeData=" + false,
        dataType: 'HTML',
        success: function (data) {
            $("#divProgress").hide();
            $("#AcceptFromFileModalMain").html(data);
            DraggableModalPopup("#AcceptFromFileModal");
        }
    });
});



function callbackRejectPurgeData() {
    location.reload();
}

// Adding Reject Data option in Additional Actions menu in dropdown
$('body').on('click', '#btnRejectDataFromPage', function () {
    var CompanyMatch = $('#CompanyMatch').val();
    var StreetNo = $('#StreetNo').val();
    var StreetName = $('#StreetName').val();
    var CityName = $('#CityName').val();
    var StateName = $('#StateName').val();
    var PostalCode = $('#PostalCode').val();
    var Telephone = $('#Telephone').val();
    var SelectTopMatch = $('#SelectTopMatch').prop('checked');
    var minConfidentCode = $('#minConfidentCode').val();

    // for encrypting the parameters
    var QueryString = "CompanyMatch:" + CompanyMatch + "@#$CityName:" + CityName + "@#$StreetNo:" + StreetNo + "@#$StateName:" + StateName + "@#$StreetName:" + StreetName + "@#$PostalCode:" + PostalCode + "@#$Telephone:" + Telephone + "@#$SelectTopMatch:" + SelectTopMatch + "@#$minConfidentCode:" + minConfidentCode;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

    var token = $('input[name="__RequestVerificationToken"]').val();
    var url = '/StewardshipPortal/RejectDataFromPage?Parameters=' + Parameters;
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: rejectCurrentPageRecords, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: url,
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json; charset=UTF-8",
                    cache: false,
                    success: function (data) {
                        if (data != "") {
                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                            ShowMessageNotification("error", data, false, true, LocationReload);
                        }
                        else if (data == "") {
                            //if no records are there then displays the below message
                            parent.ShowMessageNotification("success", noRecordsFound, false, false);
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});

$('body').on('click', '.table-Matchfixed .Enrichment', function () {
    var row = $(this).closest(".trMatchedItemView").prev().children('.trStewardshipPortal_td10');
    var data = JSON.parse(row.attr("data-val"));

    var DunsNumber = $(this).attr("data-dunsnumber");
    var Company = data.CompanyName;
    var Address = data.Address;
    var City = data.City;
    var State = data.State;
    var Postal = data.PostalCode;
    var CountryCode = data.CountryISOAlpha2Code;
    var SrcId = data.SrcRecordId;
    var Phone = data.PhoneNbr;
    var Country = $(this).attr("data-country");
    var RegNo = data.RegistrationNbr;
    var Website = data.Website

    var QueryString = "DunsNumber:" + DunsNumber + "@#$Company:" + Company + "@#$Address:" + Address + "@#$City:" + City + "@#$State:" + State + "@#$Postal:" + Postal + "@#$CountryCode:" + CountryCode + "@#$SrcId:" + SrcId + "@#$Phone:" + Phone + "@#$Country:" + Country + "@#$RegNo:" + RegNo + "@#$Website:" + Website;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/StewardshipPortal/PreviewEnrichmentData?Parameters=" + Parameters,
        dataType: 'HTML',
        success: function (data) {
            $("#EnrichmentDetailModalMain").html(data);
            DraggableModalPopup("#EnrichmentDetailModal");
        }
    });
});
$(document).on('click', '.InnerGoogleMapPopUp', function () {
    var address = $(this).attr("data-val");
    var QueryString = "Address:" + address + "@#$IsFromSearch:false";
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/SearchData/ValidateGoogleMapPopUp?Parameters=" + Parameters,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#GoogleMapModalMain").html(data);
            DraggableModalPopup("#GoogleMapModal");
        }
    });
});


function addHeight600Class(add) {
    if (add) {
        $(".popupiResearchInvestigationTargeted").addClass("height600");
    }
    else {
        $(".popupiResearchInvestigationTargeted").removeClass("height600");
    }
}


function RefreshMatchDataGrid() {
    var page = $("#divStewardshipPortal #StewPageNumber").val();
    var rowCount = $("#divStewardshipPortal tbody tr").length;
    if (rowCount == 1 && parseInt(page) > 1)
        page = parseInt(page) - 1;
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/StewardshipPortal/Index?page=" + page,
        dataType: "HTML",
        cache: false,
        beforeSend: function () {
        },
        success: function (data) {
            $("#divStewardshipPortal").html(data);
            OnSuccess();
        }
    });
}
