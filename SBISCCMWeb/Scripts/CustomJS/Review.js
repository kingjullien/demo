// Display and Hide loader bar for every ajax call and set width on top scroll for Scroll Review Data Table and also set Filter for Review Table Data
$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
    $("#divp").width($("#Review").width());
    setTimeout(function () {
        $("#divp").width($("#Review").width());
    }, 2000);
});
$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
// On Page load search button , Export button disable and set selected index 0 for dropdown

$(document).ready(function () {
    setTimeout(function () {
        $(".noData").show('fadeIn', {}, 500)
    }, 2000);
    //["TableCoulmnName","ServerColumnName","DropDownURL","ISDefault(use lowercase)","Show text/value(use lowercase)","Datatype(If Date)"]
    var colArray = [["OrderByColumn", "OrderByColumn", "/Review/GetOrderByColumnDD", "true", "text"],
    ["TopMatchCandidate", "TopMatchCandidate", "/Review/GetTopMatchCandidateDD", "false", "value", "onlyselect"],
    ["NumberOfRecordsPerPage", "NumberOfRecordsPerPage", "/Review/GetNumberOfRecordsDD", "false", "text", "onlyselect"],
    ["CountryGroup", "CountryGroup", "/Review/GetCountryGroupDD", "false", "text", "onlyselect"],
    ["ConfidenceCode", "ConfidenceCode", "/Review/GetConfidenceCodeDD", "false"],
    ["Tag", "Tag", "/Review/GetTagsDD", "false", "value", "onlyselect"]
    ];

    //Column array,URL for FilterData, TargetedDiv, Datatable function
    InitFilters(colArray, "/Review/FilterRreviewMatchCandidates", "#ReviewMatchCandidatesFilterContainer", "#divReview", "", "equalto");
});

// Search Button Enable from All four dorpdown chnage event
$("body").on('change', '.selectbox', function () {
    var Country = $("select#CountryGroup").val(); // $("select#CountryGroup").prop('selectedIndex');
    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
    if (Country > 0) {
        $("#btnSearchReviewData").removeClass('disabled');
        $("#btnExportData").removeClass('disabled');
    } else {
        $("#btnSearchReviewData").addClass('disabled');
        $("#btnExportData").addClass('disabled');
    }
});

// Exprot button click
$("body").on('click', '#btnExportData', function () {

    $('#form_ReviewData').submit();
});
// Run Auto Accept button click.
$("body").on('click', '#btnRunAutoAcceptRule', function () {
    $('#Export').val("false");
    var Country = $("select#CountryGroup").val(); // $("select#CountryGroup").prop('selectedIndex');
    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
    var token = $('input[name="__RequestVerificationToken"]').val();

    var QueryString = "TopMatchCandidate:" + TopMatchCandidate + "@#$CountryGroup:" + Country;

    if (TopMatchCandidate == true && Country != undefined) {
        $.ajax({
            type: "POST",
            url: "/Review/RunAutoAcceptRules/",
            data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(QueryString)).split("+").join("***") }),
            headers: { "__RequestVerificationToken": token },
            dataType: "html",
            contentType: "application/json",
            success: function (data) {
                $("#divReview").html(data);
                setup_dashboard_widgets_desktop();
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });

    }
    else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", "Matches accepted based on match criteria", false);

    }
    return false;
});
// Search button click
$("body").on('click', '#btnSearchReviewData', function (e) {
    FillData();
});
// First Candidate check event
$("body").on('change', '#FirstCandidate', function (e) {
    $('#Count').val(0);
    $(".context-menu-one").each(function () {
        $(this).hide();
    });
    $("#Review tbody").html("<tr>" +
        "<td class='noContain'>No data are available</td>" +
        "<td class='hidetd noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "<td class='noContain'></td>" +
        "</tr>")
});
// For Sorting check data is available or not
$("body").on('click', '.ActionLink', function (e) {

    var displaydata = $(".noContain").is(":visible");
    if (!displaydata) {
        return true;
    } else {
        return false;
    }

});
// Fill Data in tabel while search 
function FillData() {
    var Country = $("select#CountryGroup").val(); // $("select#CountryGroup").prop('selectedValue');
    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
    
    $.ajax({
        type: "POST",
        url: "/Review/Index/",
        data: JSON.stringify({ TopMatchCandidate: TopMatchCandidate, CountryGroup: Country }),
        dataType: "html",
        contentType: "application/json",
        success: function (data) {

            $("#divReview").html(data);
            $("#divp").width($("#Review").width());
            setTimeout(function () {
                $("#divp").width($("#Review").width());
            }, 2000);
            setup_dashboard_widgets_desktop();
            var classcount = $(".context-menu-one").length;
            $('#Count').val(classcount);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
// Right click event on data which display in Review Data Table 
$(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var EnableCreateAutoAcceptRules = $("#EnableCreateAutoAcceptRules").val();
    var UserType = $("#UserType").val();
    if (EnableCreateAutoAcceptRules == undefined || EnableCreateAutoAcceptRules == "False" || UserType == "STEWARD") {
        EnableCreateAutoAcceptRules = true;
    }

    if (EnableCreateAutoAcceptRules == "True") {
        EnableCreateAutoAcceptRules = false;
    }

    $.contextMenu({
        selector: '.context-menu-one',
        //trigger:'none',
        callback: function (key, options) {
        },
        items: {
            "AcceptMatches": {
                name: acceptSelectedMatches,
                callback: function () {
                    var Country = $(this).attr("data-Country");
                    var TopMatchCandidate = $("#filterValTopMatchCandidate2").val();
                    if (TopMatchCandidate == undefined) {
                        TopMatchCandidate = false;
                    }
                    var SrcRecordId = '';
                    var DUNS = '';
                    if ($("#Review tr.current").length > 1) {
                        $("#Review tr").each(function () {
                            if ($(this).hasClass("current")) {
                                SrcRecordId = SrcRecordId + $(this).attr("data-inputid") + ',';
                                DUNS = DUNS + $(this).attr("data-DUNS");
                            }
                        });
                    }
                    else {
                        SrcRecordId = $(this).attr("data-inputid");
                        DUNS = $(this).attr("data-DUNS");
                    }
                    var QueryString = "id:" + SrcRecordId + "@#$TopMatchCandidate:" + TopMatchCandidate + "@#$CountryGroup:" + Country + "@#$duns:" + DUNS;
                    //Get TopMatchCandidate value from session 'Helper.oReviewDataFilter'
                    var CloseQueryString = "TopMatchCandidate:" + true + "@#$CountryGroup:" + Country;
                    $.ajax({
                        type: "POST",
                        url: "/Review/Accepted_Item/",
                        data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(QueryString)).split("+").join("***") }),
                        headers: { "__RequestVerificationToken": token },
                        dataType: "html",
                        contentType: "application/json",
                        success: function (data) {
                            location.href = '/Review?Parameters=' + ConvertEncrypte(encodeURI(CloseQueryString)).split("+").join("***");
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                }

            },
            "AddCCAuto-acceptance": {
                name: addCC, callback: function () {
                    var Country = $(this).attr("data-Country");
                    var ConfidenceCode = $(this).attr("data-ConfidenceCode");
                    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
                    var Tags = $(this).attr("data-Tags");
                    var QueryString = "ConfidanceCode:" + ConfidenceCode + "@#$Country:" + Country + "@#$Tags:" + Tags.replace(new RegExp("::", 'g'), "@@");
                    //Get TopMatchCandidate value from session 'Helper.oReviewDataFilter'
                    $.ajax({
                        type: 'GET',
                        url: "/Review/CC_Item?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#ReviewCC_ItemModalMain").html(data);
                            DraggableModalPopup("#InsertUpdateAutoAcceptanceModal");
                            OnReadyInsertUpdateAutoAcceptance();
                        }
                    });
                }, disabled: function (key, opt) {
                    if (!EnableCreateAutoAcceptRules) {
                        if ($("#Review").find(".current").length > 1) {
                            return true;
                        }
                        return false;
                    } else { return true; }
                }
            },
            "AddCCMGAuto-acceptance": {
                name: addCCandMG, callback: function () {

                    var Country = $(this).attr("data-Country");
                    var ConfidenceCode = $(this).attr("data-ConfidenceCode");
                    var DnBMatchGradeText = $(this).attr("data-DnBMatchGradeText");
                    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
                    var Tags = $(this).attr("data-Tags");

                    var QueryString = "ConfidanceCode:" + ConfidenceCode + "@#$Country:" + Country + "@#$DnBMatchGradeText:" + DnBMatchGradeText + "@#$Tags:" + Tags.replace(new RegExp("::", 'g'), "@@");
                    //Get TopMatchCandidate value from session 'Helper.oReviewDataFilter'
                    var CloseQueryString = "TopMatchCandidate:" + true + "@#$CountryGroup:" + Country;
                    $.ajax({
                        type: 'GET',
                        url: "/Review/CCMG_Item?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#ReviewCC_ItemModalMain").html(data);
                            DraggableModalPopup("#InsertUpdateAutoAcceptanceModal");
                            OnReadyInsertUpdateAutoAcceptance();
                        }
                    });
                }, disabled: function (key, opt) {
                    if (!EnableCreateAutoAcceptRules) {
                        if ($("#Review").find(".current").length > 1) {
                            return true;
                        }
                        return false;
                    } else { return true; }
                }
            },
            "AddCCMGMDPAuto-acceptance": {
                name: addCCandMGandMDP, disabled: EnableCreateAutoAcceptRules, callback: function () {

                    var Country = $(this).attr("data-Country");
                    var ConfidenceCode = $(this).attr("data-ConfidenceCode");
                    var DnBMatchGradeText = $(this).attr("data-DnBMatchGradeText");
                    var DnBMatchDataProfileText = $(this).attr("data-DnBMatchDataProfileText");
                    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
                    var Tags = $(this).attr("data-Tags");
                    var QueryString = "ConfidanceCode:" + ConfidenceCode + "@#$Country:" + Country + "@#$DnBMatchGradeText:" + DnBMatchGradeText + "@#$DnBMatchDataProfileText:" + DnBMatchDataProfileText + "@#$Tags:" + Tags.replace(new RegExp("::", 'g'), "@@");
                    //Get TopMatchCandidate value from session 'Helper.oReviewDataFilter'
                    var CloseQueryString = "TopMatchCandidate:" + true + "@#$CountryGroup:" + Country;
                    $.ajax({
                        type: 'GET',
                        url: "/Review/CCMGMDP_Item?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#ReviewCC_ItemModalMain").html(data);
                            DraggableModalPopup("#InsertUpdateAutoAcceptanceModal");
                            OnReadyInsertUpdateAutoAcceptance();
                        }
                    });
                }, disabled: function (key, opt) {
                    if (!EnableCreateAutoAcceptRules) {
                        if ($("#Review").find(".current").length > 1) {
                            return true;
                        }
                        return false;
                    } else { return true; }
                }
            },
            "ExcludeCCMGAuto-acceptance": {
                name: excludeCCandMG, callback: function () {
                    var Country = $(this).attr("data-Country");
                    var ConfidenceCode = $(this).attr("data-ConfidenceCode");
                    var DnBMatchGradeText = $(this).attr("data-DnBMatchGradeText");
                    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
                    var Tags = $(this).attr("data-Tags");
                    var QueryString = "ConfidanceCode:" + ConfidenceCode + "@#$Country:" + Country + "@#$DnBMatchGradeText:" + DnBMatchGradeText + "@#$Tags:" + Tags.replace(new RegExp("::", 'g'), "@@");
                    //Get TopMatchCandidate value from session 'Helper.oReviewDataFilter'
                    var CloseQueryString = "TopMatchCandidate:" + true + "@#$CountryGroup:" + Country;
                    $.ajax({
                        type: 'GET',
                        url: "/Review/ExCCMG_Item?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#ReviewCC_ItemModalMain").html(data);
                            DraggableModalPopup("#InsertUpdateAutoAcceptanceModal");
                            OnReadyInsertUpdateAutoAcceptance();
                        }
                    });
                }, disabled: function (key, opt) {
                    if (!EnableCreateAutoAcceptRules) {
                        if ($("#Review").find(".current").length > 1) {
                            return true;
                        }
                        return false;
                    } else { return true; }
                }
            },
            "ExcludeCCMGMDPAuto-acceptance": {
                name: excludeCCandMGandMDP, callback: function () {
                    var Country = $(this).attr("data-Country");
                    var ConfidenceCode = $(this).attr("data-ConfidenceCode");
                    var DnBMatchGradeText = $(this).attr("data-DnBMatchGradeText");
                    var DnBMatchDataProfileText = $(this).attr("data-DnBMatchDataProfileText");
                    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
                    var Tags = $(this).attr("data-Tags");
                    var QueryString = "ConfidanceCode:" + ConfidenceCode + "@#$Country:" + Country + "@#$DnBMatchGradeText:" + DnBMatchGradeText + "@#$DnBMatchDataProfileText:" + DnBMatchDataProfileText + "@#$Tags:" + Tags.replace(new RegExp("::", 'g'), "@@");
                    //Get TopMatchCandidate value from session 'Helper.oReviewDataFilter'
                    var CloseQueryString = "TopMatchCandidate:" + true + "@#$CountryGroup:" + Country;
                    $.ajax({
                        type: 'GET',
                        url: "/Review/ExCCMGMDP_Item?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#ReviewCC_ItemModalMain").html(data);
                            DraggableModalPopup("#InsertUpdateAutoAcceptanceModal");
                            OnReadyInsertUpdateAutoAcceptance();
                        }
                    });

                }, disabled: function (key, opt) {
                    if (!EnableCreateAutoAcceptRules) {
                        if ($("#Review").find(".current").length > 1) {
                            return true;
                        }
                        return false;
                    } else { return true; }
                }
            },
            "ShowMatchDetail": {
                name: showMatchDetail, callback: function () {
                    $('#divProgress').show();

                    var id = $(this).attr("data-val");
                    var SrcRecordId = $(this).attr("data-SrcRecordId");
                    var InputId = $(this).attr("data-inputid");
                    var next = $(this).attr("data-next");
                    var prev = $(this).attr("data-prev");
                    var Country = $(this).attr("data-Country");
                    var DUNS = $(this).attr("data-DUNS");
                    var OrderBy = $("#OrderBy").val();
                    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
                    //Get TopMatchCandidate value from session 'Helper.oReviewDataFilter'
                    var QueryString = "id:" + InputId + "@#$TopMatchCandidate:" + true + "@#$CountryGroup:" + Country + "@#$dataNext:" + next + "@#$dataPrev:" + prev + "@#$DUNS:" + DUNS + "@#$OrderBy:" + OrderBy + '@#$IsPartialView:false'

                    $.ajax({
                        type: 'GET',
                        url: "/Review/MatchDetailView_Item?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#ReviewMatchDetailModalMain").html(data);
                            DraggableModalPopup("#ReviewMatchDetailModal");
                        }
                    });
                    $('#divProgress').hide();
                    return 'context-menu-icon context-menu-icon-quit';
                }, disabled: function (key, opt) {

                    // this references the trigger element
                    return $("#Review").find(".current").length > 1 ? true : false;
                }
            },
        }
    });

    $('.context-menu-one').on('click', function (e) {

    })
});
// Next Button event for Match Data Review Popup
function matchItemNextClick(dunsNumb, InputId) {
    if (dunsNumb != "") {
        $(".context-menu-one").each(function () {
            $(this).removeClass("current");
        });
        var currentid = "#" + dunsNumb;
        $(currentid).addClass("current");
    }
}
// Previous Button event for Match Data Review Popup
function matchItemPrevClick(dunsNumb, InputId) {
    if (dunsNumb != "") {
        $(".context-menu-one").each(function () {
            $(this).removeClass("current");
        });
        var currentid = "#" + dunsNumb;
        $(currentid).addClass("current");
    }
}
// Open Match Detail Review Popup on Right click of data 
$('body').on('click', '.clsViewMatchedItemDetails', function () {

    var SrcRecordId = $(this).attr("data-SrcRecordId");
    var InputId = $(this).attr("data-inputid");
    var next = $(this).attr("data-next");
    var prev = $(this).attr("data-prev");
    var Country = $(this).attr("data-Country");
    var DUNS = $(this).attr("data-DUNS");
    var TopMatchCandidate = $('#TopMatchCandidate').prop('checked');
    var QueryString = "id:" + InputId + "@#$TopMatchCandidate:" + TopMatchCandidate + "@#$CountryGroup:" + Country + "@#$childButtonId:" + SrcRecordId + "@#$dataNext:" + next + "@#$dataPrev:" + prev + "@#$DUNS:" + DUNS
    $.magnificPopup.open({
        items: {
            src: "/Review/MatchDetailView_Item?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***")
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
// Set current row select and set color for current row and remove color from previous selected row.
$('body').on('click', '.context-menu-one', function (evt) {
    if (evt.ctrlKey) {
        if ($(this).hasClass("current")) { $(this).removeClass("current"); }
        else { $(this).addClass("current"); }
    } else {
        $(".context-menu-one").each(function () {
            $(this).removeClass("current");
        });
        $(this).addClass("current");
    }
});
// Top Scroll and bottom scroll work together event.
var arescrolling = 0;
function scroller(from, to) {

    if (arescrolling) return;
    arescrolling = 1;

    document.getElementById(to).scrollLeft = document.getElementById(from).scrollLeft;
    arescrolling = 0;
}
function OnSuccess() {
    setup_dashboard_widgets_desktop();
    $('body').removeClass("nooverflow");
}


// for the pagination event 
$("body").on('change', '.pagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#SortBy").val();
    var SortOrder = $("#SortOrder").val();
    var url = '/Review/Index/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $("#divTicketPortal").load(url, function () {

    });
});

// Add Company Popup open 
$('body').on('click', '.btnAddCompany', function () {

    var token = $('input[name="__RequestVerificationToken"]').val();
    var matchRecord = $(this).attr("data-val");

    $.ajax({
        type: "POST",
        url: "/BadInputData/FillMatchData/",
        data: JSON.stringify({ matchRecord: matchRecord }),
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            // Changes for Converting magnific popup to modal popup
            $.ajax({
                type: 'GET',
                url: "/BadInputData/AddCompany",
                dataType: 'HTML',
                async: false,
                success: function (data) {
                    $("#divProgress").hide();
                    $("#CleanSearchDataAddCompanyModalMain").html(data);
                    DraggableModalPopup("#SearchDataAddCompanyModal");
                }
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    return false;
});