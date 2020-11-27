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

    var firstmatchid = "#" + $('#example_wrapper table.inlinetable').attr('id');
    var firstmatchDetailid = "#" + $('#example_wrapper_Detail table.MatchedItem').attr('id');
    $(firstmatchid).css("cssText", "display: block !important;");
    $(firstmatchDetailid).css("cssText", "display: block !important;");

    if (firstmatchDetailid != "#undefined") {
        var ClassList = $(firstmatchDetailid)[0].classList;
        for (var i = 0 ; i < ClassList.length ; i++) {
            if (ClassList[i].indexOf("Detail-") > -1) {
                var listofclass = "." + ClassList[i];
                if ($(listofclass).length > 1) {
                    $(".widget-toolbar").css("display", "block");
                }
                else {
                    $(".widget-toolbar").css("display", "none");
                }
            }
        }
    }
    else {
        $(".widget-toolbar").css("display", "none");
    }
    $("#divProgress").hide();
    //setup_dashboard_widgets_desktop();
});

// Clear all  Company filter
$("#btnClearCompanyData").click(function () {
    clearCompanyData();
});
// Clear all Match Filter
$("#btnClearMatchFilter").click(function () {
    clearAllOtherData();
});
$('body').on('click', '#btnAcceptSelectedMatches', function () {
    $('body').removeClass("nooverflow");
});

// On Reject button click clear all Filter
$('body').on('click', '#btnRejectSelectedMatches', function () {
    $('body').removeClass("nooverflow");
    clearAllOtherData();
});
$("#btnClose").click(function () {
    clearAllOtherData();
});

function clearCompanyData() {
    $("#txtScrRecId").val('');
    $("#txtCompany").val('');
    $("#txtCity").val('');
    $("select#State").prop('selectedIndex', 0);
    $("select#Country").prop('selectedIndex', 0);
}
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
// Collapse - expand effect for Grid View panel
$('body').on('show.bs.collapse', '.collapse', function () {
    $(".panel-collapse.in").collapse('hide');
    $('.trMatchedItemView').each(function () {
        $(this).hide();
    });
    $(this).parents('tr').show();
});
$('body').on('hide.bs.collapse', '.collapse', function () {
    $(this).parents('tr').hide();
});

$('body').on('click', '#trig', function () {
    $('#colMain').toggleClass('col-md-9 col-md-12');
    $('#colPush').toggleClass('col-md-3 col-md-0');
});

// On Match Details click event on match select and update in entity
$('body').on('click', '.clsViewMatchedItemDetails', function () {
    var data = $(this).attr("data-val");
    $.ajax({
        type: "POST",
        url: "/ApproveMatchData/cShowMatchedItesDetailsView/",
        data: { id: data, childButtonId: this.id },
        success: function (data) {
            $.magnificPopup.open({
                items: {
                    src: '/ApproveMatchData/GetLayoutPopUp'
                },
                type: 'iframe',
                closeOnBgClick: false,
                mainClass: 'popInitiateReturn',
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
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    return false;
});

//update matched in entity of company
function CallAcceptLCMMatches(SrcId, Seqence) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/ApproveMatchData/AcceptLCMMatches/",
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
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/ApproveMatchData/AcceptLCMMatches/",
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
        url: "/ApproveMatchData/RejectLCMMatches/",
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
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/ApproveMatchData/RejectLCMMatches/",
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
// Manage Color code and update parent color code for match data
// Green to black and remove all selected matches for select one for green
$('body').on('click', '.btnSelected', function () {
    var InputId = $(this).attr("id").replace("InnerMatch", "");// Take Id of the Elements
    var Seqence = $(this).attr("data-val");// Take Sequece of the match.
    parent.$("#" + InputId + "RejectAll").removeClass("parentbtnMatchRejected").addClass("parentbtnUnselected");// Remove Relected Buttton Class for Match Rejectd and Add class for UnSelected and Make red border for the button
    parent.$("#" + InputId + "RejectAll").removeClass("parentbtnIdel").addClass("parentbtnUnselected");// Remove Idel class for the rejected button
    parent.$("#" + InputId + "NotSelected").removeClass("SelectMathced").addClass("NotSelected");// Add Selected icon
    $(this).removeClass("btnSelected").addClass("btnUnselected");
    CallRejectLCMMatches(InputId, Seqence);
});

// Click event for black to green color for matches section
$('body').on('click', '.btnUnselected', function () {
    var InputId = $(this).attr("id").replace("InnerMatch", "");// Take Id of the Elements
    var Seqence = $(this).attr("data-val");// Take Sequece of the match.
    parent.$("#" + InputId + "RejectAll").removeClass("parentbtnMatchRejected").addClass("parentbtnUnselected");// Remove Relected Buttton Class for Match Rejectd and Add class for UnSelected and Make red border for the button
    parent.$("#" + InputId + "RejectAll").removeClass("parentbtnIdel").addClass("parentbtnUnselected");// Remove Idel class for the rejected button
    parent.$("#" + InputId + "NotSelected").removeClass("NotSelected").addClass("SelectMathced");// Add Selected icon
    parent.$("." + InputId).each(function () {
        $(this).removeClass("btnSelected").addClass("btnUnselected");
    });// Remove all selected matches
    $(this).removeClass("btnUnselected").addClass("btnSelected");// Mark as selected to current button
    CallAcceptLCMMatches(InputId, Seqence);// Update the Model of the company
    if ($("#Approve").val() == "False") {
        OpenStewardshipNote(InputId, Seqence, '');// Open Steward ship note.
    }
});

//Match Data Color Chnage for Parent
$('body').on('click', '.parentbtnMatchRejected', function () {
    $(this).removeClass("parentbtnMatchRejected").addClass("parentbtnUnselected");// Remove Red color and Make red border for reject button
    var id = $(this).attr('id').replace("RejectAll", "");
    CallAcceptLCMMatches(id, 'unrejectall');// Update the Model of the company
});

$('body').on('click', '.parentbtnIdel', function () {
    var InputId = $(this).attr("id").replace("RejectAll", "");// Take Id of the Elements
    var Seqence = $(this).attr("data-val");// Take Sequece of the match.
    parent.$("." + InputId).each(function () {
        $(this).removeClass("btnSelected").addClass("btnUnselected");
    });// Remove all selected matches
    $(this).removeClass("parentbtnIdel").addClass("parentbtnMatchRejected");
    CallAcceptLCMMatches(InputId, 'rejectall');// Update the Model of the company
    if ($("#Approve").val() == "False") {
        //Open stewardship note
        OpenStewardshipNote(InputId, '-1', '');
    }
});

$('body').on('click', '.parentbtnUnselected', function () {
    var InputId = $(this).attr('id').replace("RejectAll", "");// Take Id of the Elements
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
            src: '/ApproveMatchData/OpenSearchData?InputId=' + id + '&count=' + count + '&Notes=' + Note
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
//Open Session Filter Popup
$('body').on('click', '.userFilter', function () {
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/UserSessionFilter/popupUserFilter/'
        },
        callbacks: {
            close: function () {
                location.reload();
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupUserSessionFilter'
    });
    return false;
});

// Delete Session Filter Delete Popup
$('body').on('click', '.DeleteFilter', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to delete session filter?", callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/ApproveMatchData/DeleteSessionFilter/",
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    success: function (data) {
                        if (data == "success") {
                            location.reload();
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

//On click on Match data row open child matches in next panel
$('body').on('click', '.panelviewRow', function () {
   
    var data = $(this).attr("data-inputId");
    $('.current').each(function () {
        $(this).removeClass("current");
    });
    $(this).addClass("current");
    $(".inlinetable").attr("style", "display: none !important");
    var id = "#tr" + data;
    $(id).css("cssText", "display: block !important;");
    $(id).children("tbody").children('tr:first').addClass("current");
    $(id).children("tbody").children('tr:first').next().addClass("current");
    var firstmatchDetailid = id + " tr.OpenDetails";
    var DUNSNO = $(firstmatchDetailid).attr("data-val");

    $(".MatchedItem").each(function () {
        $(this).attr("style", "display: none !important");
    });

    var DUNSNOID = "#Detail-" + DUNSNO.replace("\"", "").replace("\"", "");
    $(DUNSNOID).css("display", "block");
});
// On click on match data main panel and display match detail
$('body').on('click', '.detailOpen', function () {
    var data = $(this).attr("data-val");
    $(".MatchedItem").attr("style", "display: none !important");
    var id = ".Detail-" + data.replace("\"", "").replace("\"", "");
    id = id.replace(" ", "");
    var firstmatchDetailid = "#" + $(id).attr('id');
    //$(firstmatchDetailid).css("display", "block");
    $(firstmatchDetailid).css("cssText", "display: block !important;");
    if ($(id).length > 1) {
        $(".widget-toolbar").css("display", "block");
    }
    else {
        $(".widget-toolbar").css("display", "none");
    }
    $(".detail-Prev").parent().addClass("disabled");
    $(".detail-Next").parent().removeClass("disabled");
});

// On click  of matches and display match details
$('body').on('click', '.OpenDetails', function () {
    $(".OpenDetails").removeClass("current");
    $(this).addClass("current");
    var classList = $(this)[0].classList;
    var j = 0
    for (var i = 0; i < classList.length; i++) {
        if (classList[i] == "border-top-none") {
            $(this).prev().addClass("current");
            j++;
        }
    }
    if (j == 0) {
        $(this).next().addClass("current");
    }

    //$(this).next().addClass("current");
    var data = $(this).attr("data-val");
    $(".MatchedItem").attr("style", "display: none !important");
    var id = "#Detail-" + data.replace("\"", "").replace("\"", "");
    //$(id).css("display", "block");
    $(id).css("cssText", "display: block !important;");
    if ($(this).next().next().length == 0) {
        $(".detail-Next").parent().addClass("disabled");
    }
    else {
        $(".detail-Next").parent().removeClass("disabled");
    }
    if ($(this).prev().prev().length == 0) {

        $(".detail-Prev").parent().addClass("disabled");
    } else {
        $(".detail-Prev").parent().removeClass("disabled");
    }
});

var currentclass;
// set Previous button event and display previous match details record
$('body').on('click', '.detail-Prev', function () {
    if (!$(this).parent().hasClass("disabled")) {
        // Match Item loop for all Match Details View
        $('.MatchedItem').each(function () {
            // find current display view
            if ($(this).css("display") == "block") {
                var InputId = $(this).attr("data-InputId");// Get InputId for this records
                $(this).css("display", "none"); // display none current node
                // display block next node if exists
                if ($(this).prev().attr("data-InputId") == InputId) {
                    $(this).prev().css("display", "block");
                }
                // if next data s not from same inputid than disable next button
                if ($(this).next().attr("data-InputId") != InputId) {
                    $(".detail-Next").parent().removeClass("disabled");
                }
                // if prev button disable than remove disable class
                if ($(this).prev().prev().attr("data-InputId") != InputId) {
                    $(".detail-Prev").parent().addClass("disabled");
                }
                return false;
            }
        });
        // add current class to prev record and make it selected
        var currentRows = $('.OpenDetails.current');
        for (var i = 0; i < currentRows.length; i++) {
            $(currentRows[i]).prev().prev().addClass('current');
            $(currentRows[i]).removeClass('current');
        }
    }
});

// set Next button event and display Next match details record
$('body').on('click', '.detail-Next', function () {

    if (!$(this).parent().hasClass("disabled")) {
        // Match Item loop for all Match Details View
        $('.MatchedItem').each(function () {
            // find current display view
            if ($(this).css("display") == "block") {
                var InputId = $(this).attr("data-InputId");// Get InputId for this records
                $(this).css("display", "none");// display none current node
                // display block next node if exists
                if ($(this).next().attr("data-InputId") == InputId) {
                    $(this).next().css("display", "block");
                }
                // if prev button disable than remove disable class
                if ($(this).prev().attr("data-InputId") != InputId) {
                    $(".detail-Prev").parent().removeClass("disabled");
                }
                // if next data s not from same inputid than disable next button
                if ($(this).next().next().attr("data-InputId") != InputId) {
                    $(".detail-Next").parent().addClass("disabled");
                }
                return false;
            }
        });
        // add current class to next record and make it selected
        var currentRows = $('.OpenDetails.current');
        for (var i = 0; i < currentRows.length; i++) {
            $(currentRows[i]).next().next().addClass('current');
            $(currentRows[i]).removeClass('current');
        }
    }
});

//Load First Match Child and Match Item detail load at Pagination time
function pagination() {
    clearAllOtherData();
    $('body').removeClass("nooverflow");
    if ($(".noContain").html() === undefined) {
        var firstmatchid = "#" + $('#example_wrapper table.inlinetable').attr('id');
        var firstmatchDetailid = "#" + $('#example_wrapper_Detail table.MatchedItem').attr('id');
        // $(firstmatchid).css("display", "block");
        $(firstmatchid).css("cssText", "display: block !important;");
        //$(firstmatchDetailid).css("display", "block");
        $(firstmatchDetailid).css("cssText", "display: block !important;");
        var ClassList = $(firstmatchDetailid)[0].classList;
        for (var i = 0 ; i < ClassList.length ; i++) {
            if (ClassList[i].indexOf("Detail-") > -1) {
                var listofclass = "." + ClassList[i];
                if ($(listofclass).length > 1) {
                    $(".widget-toolbar").css("display", "block");
                }
                else {
                    $(".widget-toolbar").css("display", "none");
                }
            }
        }
    }
    else {
        $(".MatchesItem").hide();
        $(".MatchDetails").hide();
    }
    setup_dashboard_widgets_desktop();

};
// Page Value Change on Page Value DropDown
$("body").on('change', '.pagevalueChange', function () {
   
    var pagevalue = $(this)[0].value;
    $('body').removeClass("nooverflow");
    var url = '/ApproveMatchData/Index/' + "?pagevalue=" + pagevalue;
    $("#divStewardshipPortal").load(url, function () {
        if ($(".noContain").html() === undefined) {
            var firstmatchid = "#" + $('#example_wrapper table.inlinetable').attr('id');
            var firstmatchDetailid = "#" + $('#example_wrapper_Detail table.MatchedItem').attr('id');

            // $(firstmatchid).css("display", "block");
            $(firstmatchid).css("cssText", "display: block !important;");
            //$(firstmatchDetailid).css("display", "block");
            $(firstmatchDetailid).css("cssText", "display: block !important;");
            var ClassList = $(firstmatchDetailid)[0].classList;
            for (var i = 0 ; i < ClassList.length ; i++) {
                if (ClassList[i].indexOf("Detail-") > -1) {
                    var listofclass = "." + ClassList[i];
                    if ($(listofclass).length > 1) {
                        $(".widget-toolbar").css("display", "block");
                    }
                    else {
                        $(".widget-toolbar").css("display", "none");
                    }
                }
            }
        }
        else {
            $(".MatchesItem").hide();
            $(".MatchDetails").hide();
        }


        setup_dashboard_widgets_desktop();

        var slider = document.getElementById('slider-range');
        slider.noUiSlider.set(0);
        //setup_dashboard_widgets_desktop();
    });
   
});

// On Page Load First Match Child and Match Item detail load
function LoadChildandDetail() {
    $('#divProgress').show();
    $('body').removeClass("nooverflow");
    if ($(".noContain").html() === undefined) {
        var firstmatchid = "#" + $('#example_wrapper table.inlinetable').attr('id');
        var firstmatchDetailid = "#" + $('#example_wrapper_Detail table.MatchedItem').attr('id');
        // $(firstmatchid).css("display", "block");
        $(firstmatchid).css("cssText", "display: block !important;");
        //$(firstmatchDetailid).css("display", "block");
        $(firstmatchDetailid).css("cssText", "display: block !important;");
        var ClassList = $(firstmatchDetailid)[0].classList;
        for (var i = 0 ; i < ClassList.length ; i++) {
            if (ClassList[i].indexOf("Detail-") > -1) {
                var listofclass = "." + ClassList[i];
                if ($(listofclass).length > 1) {
                    $(".widget-toolbar").css("display", "block");
                }
                else {
                    $(".widget-toolbar").css("display", "none");
                }
            }
        }

    }
    else {
        $(".MatchesItem").hide();
        $(".MatchDetails").hide();
    }
    $('#divProgress').hide();
    setup_dashboard_widgets_desktop();
}
// Click of Panel 
$("body").on('click', '.panelview', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/ApproveMatchData/SetUserLayoutPreference/",
        data: JSON.stringify({ userLayout: "PANEL" }),
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            $(".set-panel").hide();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });

});
$(document).ready(function () {
    $(window).bind("beforeunload", function (event) {
        //alert('Good Bye !!!');
    });
});
function backToparent() {
    $.magnificPopup.close();
    $.ajax({
        type: "POST",
        url: "/ApproveMatchData/GetFilteredCompanyList/",
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
    var IsApprovalScreen = $("#IsApprovalScreen").val();
    if (IsApprovalScreen == "True") {
        OpenStewardshipNote(id, count, Note);
    }
});
// On Window Close Fire Event
//$(window).unload(function () {

//    $.ajax({
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        url: '/StewardshipPortal/StewUserActivityCloseWindow',
//        data: '',
//        dataType: "HTML",
//        beforeSend: function () {
//        },
//        success: function (data) {
//            location.reload();
//        }
//    });
//});

// On Pagination ask if page isDirty.
$("body").on('click', '.dataTables_paginate .pagination', function (e) {
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
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "You have unsaved changes. Are you sure you want to change page and discard the changes?", callback: function (result) {
                if (result) {
                    var slider = document.getElementById('slider-range');
                    slider.noUiSlider.set(0);
                    $.ajax({
                        type: "GET",
                        url: "/ApproveMatchData/Index/",
                        data: { page: page, sortby: sortby, sortorder: sortorder, pagevalue: pagevalue },
                        dataType: "HTML",
                        contentType: "application/html",
                        success: function (data) {
                            $("#divStewardshipPortal").html(data);
                            LoadChildandDetail();
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                }
            }
        });
    } else {
        return true;
    }
    return false;
});
//  Open Custom attribute window
$("body").on('click', '.trStewardshipPortal_td2', function ()
{   
    var id = $(this).attr('id');
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/ApproveMatchData/popupCompanyAttribute?Parameters=' + id
        },
        callbacks: {
            close: function () {
               
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupCompanyAttribute'
    });
    return false;
});


$(function () {
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var Investigation = $("#LicenseEnableInvestigations").val();
    if (Investigation == undefined || Investigation == "False") {
        Investigation = true;
    }

    if (Investigation == "True") {
        Investigation = false;
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
                name: "Add Match as Company", callback: function () {
                    var matchRecord = $(this).attr("data-value");

                    $.ajax({
                        type: "POST",
                        url: "/BadInputData/FillMatchData/",
                        data: JSON.stringify({ matchRecord: matchRecord }),
                        headers: { "__RequestVerificationToken": token },
                        dataType: "json",
                        contentType: "application/json",
                        async: false,
                        success: function (data) {
                            if (data == "success") {
                                $.magnificPopup.open({
                                    preloader: false,
                                    closeBtnInside: true,
                                    type: 'iframe',
                                    items: {
                                        src: '/BadInputData/AddCompany'
                                    },
                                    callbacks: {
                                        close: function () {

                                        }
                                    },
                                    closeOnBgClick: false,
                                    mainClass: 'popupAddressCompany'
                                });
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }
            },
            "AddInvestigation": {
                name: "Investigate Record", disabled: Investigation, callback: function () {
                    var matchRecord = $(this).attr("data-value");

                    $.ajax({
                        type: "POST",
                        url: "/InvestigateView/SetValue/",
                        data: JSON.stringify({ IsCompany: false, data: matchRecord }),
                        headers: { "__RequestVerificationToken": token },
                        dataType: "json",
                        contentType: "application/json",
                        async: false,
                        success: function (data) {
                            if (data == "success") {

                                $.magnificPopup.open({
                                    preloader: false,
                                    closeBtnInside: true,
                                    type: 'iframe',
                                    items: {
                                        src: '/InvestigateView/InvestigateRecords'
                                    },
                                    callbacks: {
                                        close: function () {

                                        }
                                    },
                                    closeOnBgClick: false,
                                    mainClass: 'popupInvestigateRecord'

                                });
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

$(function () {
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var Investigation = $("#LicenseEnableInvestigations").val();
    if (Investigation == undefined || Investigation == "False") {
        Investigation = true;
    }

    if (Investigation == "True") {
        Investigation = false;
    }
    $.contextMenu({
        selector: '.context-menu-one-match',
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
            "AddInvestigation": {
                name: "Investigate Record", disabled: Investigation, callback: function () {
                    var matchRecord = $(this).attr("data-val");
                  
                    $.ajax({
                        type: "POST",
                        url: "/InvestigateView/SetValue/",
                        data: JSON.stringify({ IsCompany: true, data: matchRecord }),
                        headers: { "__RequestVerificationToken": token },
                        dataType: "json",
                        contentType: "application/json",
                        async: false,
                        success: function (data) {
                            if (data == "success") {
                  
                                $.magnificPopup.open({
                                    preloader: false,
                                    closeBtnInside: true,
                                    type: 'iframe',
                                    items: {
                                        src: '/InvestigateView/InvestigateRecords'
                                    },
                                    callbacks: {
                                        close: function () {

                                        }
                                    },
                                    closeOnBgClick: false,
                                    mainClass: 'popupInvestigateRecord'

                                });
                            }

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }

            },

        }

    });

    $('.context-menu-one-match').on('click', function (e) {
        return 'context-menu-icon context-menu-icon-quit';
    })
});
function backToparent2() {
    $.magnificPopup.close();
}
$('body').on('click', '.GoogleMapPopUp', function () {
    var matchRecord = $(this).attr("data-val");
    $.magnificPopup.open({
        items: {
            src: '/StewardshipPortal/GoogleMapPopUp?id=' + matchRecord,
        },
        type: 'iframe',
        closeOnBgClick: false,
        mainClass: 'popInvestigation',
        callbacks: {
          
        }
    });

    return false;
});