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
$('body').on('show.bs.collapse', '.collapse', function () {


    $('.partialrow').each(function () {
        $(this).removeClass("current");
    });
    $(".panel-collapse.in").collapse('hide');
    $('.trMatchedItemView').each(function () {
        $(this).hide();

    });
    $(this).parents('tr').show();
    $(this).parents('tr').prev().addClass("current");
});
// toggle event for Child Data Hide in Match Data Table on "-" icon 
$('body').on('hide.bs.collapse', '.collapse', function () {
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
    $(this).closest(".currentChildRow").next().addClass("current")

    var data = $(this).attr("data-val");
    var InputId = $(this).attr("data-Id");
    var DUNS = $(this).attr("data-DUNS");
    var dataNext = $(this).attr("data-next");
    var dataPrev = $(this).attr("data-prev");
    var Count = $(this).attr("data-Seqence");
    var QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:AdditionalFields' + '@#$IsPartialView:false';
    $.magnificPopup.open({
        items: {
            src: '/ApproveMatchData/cShowMatchedItesDetailsView?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***")
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

    return false;
});
// On Match Select for Company Update Model 
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
function MatchCallRejectLCMMatches(SrcId,Seqence, token) {
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
// On Match Select Button Green to Black and update Model for this changes
$('body').on('click', '.btnSelected', function () {
    
    var InputId = $(this).attr("id").replace("InnerMatch", "");// Take Id of the Elements
    var Seqence = $(this).attr("data-val");// Take Sequece of the match.
    $(this).removeClass("btnSelected").addClass("btnUnselected");
    parent.$("#" + InputId + "NotSelected").removeClass("SelectMathced").addClass("NotSelected");
   
    CallRejectLCMMatches(InputId , Seqence);
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
// Open Session Filter
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
// Delete Session Filter
$('body').on('click', '.DeleteFilter', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $(this).find("strong").removeClass("fa fa-plus");
    $(this).find("strong").removeClass("fa fa-minus");
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
// Page Value Change on Page Value DropDown
$("body").on('change', '.pagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    var url = '/ApproveMatchData/Index/' + "?pagevalue=" + pagevalue;
    $("#divStewardshipPortal").load(url, function () {
        var slider = document.getElementById('slider-range');
        slider.noUiSlider.set(0);
        setup_dashboard_widgets_desktop();
    });

    $('body').removeClass("nooverflow");
});
// Change View like Panel to Grid and Grid to Panel
$("body").on('click', '.panelview', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/ApproveMatchData/SetUserLayoutPreference/",
        data: JSON.stringify({ userLayout: "GRID" }),
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
        var currentid = "#" + dunsNumb + "DUNS"+ InputId;
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
        var index = parseInt(($(nextcurrentid).index() + 1) / 2) -1;
        $InnerTableId.find('tbody').scrollTop(($($AllTr[0]).height() + $($AllTr[1]).height()) * index - 50);

        $('#divProgress').hide();
        // $('tr[id=' + InputId + ']').next('tr').find('.clsViewMatchedItemDetails:contains("' + dunsNumb + '")').click();
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
        var index = parseInt(($(nextcurrentid).index() + 1) / 2) -1;
        $InnerTableId.find('tbody').scrollTop(($($AllTr[0]).height() + $($AllTr[1]).height()) * index - 50);
      
        //$('tr[id=' + InputId + ']').next('tr').find('.clsViewMatchedItemDetails:contains("' + dunsNumb + '")').click();
        
    }
}
$("body").on('click', '.sort-header', function () {
    $(this).addClass("headerSortDown");
    $('body').removeClass("nooverflow");
});
//Set Current Selected for Child Records and remove color from previous selected row 
$("body").on('click', '.currentChildRow', function () {
  // Set current row as selected and remove provious row from selection.
    $('.currentChildRow').each(function () {
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
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "You have unsaved changes. Are you sure you want to change page and discard the changes?", callback: function (result) {
                if (result) {
                    var slider = document.getElementById('slider-range');
                    slider.noUiSlider.set(0);
                    $.get("/ApproveMatchData/Index/", { page: page, sortby: sortby, sortorder: sortorder, pagevalue: pagevalue }, function (data) {
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
//  Open Custom attribute window
$("body").on('click', '.trStewardshipPortal_td2', function () {
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

// Open Right click popup and Manage according ajax call.
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
                name: "Add match as a new company", callback: function () {
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
                    var matchRecord = $(this).attr("data-val");

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

            },

        }

    });

    $('.context-menu-one').on('click', function (e) {
        return 'context-menu-icon context-menu-icon-quit';
    })
});
// Open Popup for the Investigation and Set value in Investigation window
$(function () {
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var Investigation = $("#LicenseEnableInvestigations").val();
    if (Investigation == undefined || Investigation =="False")
    {
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
                name: "Investigate Record", disabled: Investigation,titile:"Investigation", callback: function () {
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

function CloseInvestigationWindow()
{
    window.parent.$.magnificPopup.close();
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