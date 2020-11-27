$(function () {
    // set current record is selected match or not and set text according to this from model.
    $('.mfp-iframe').contents().find("html body #contentPopup .btnUnselectedMatchDetailReview,html body #contentPopup .btnSelectedMatchDetailReview").each(function () {

        var parentBtnclass = $('div#' + this.id.replace(' ', '')).attr('class');
        $(this).removeAttr('class').addClass(parentBtnclass);
        if (parentBtnclass.indexOf("btnSelectedMatchDetailReview") > -1) {
            $(this).parent().find(".displaylbl").text("Selected");
        }
    });
    // make current match select button green as well parent grid change button color black to green and update model according this changes
    $('body').on('click', '.btnUnselectedMatchDetailReview', function () {

        var InputId = $(".CustomAttribute").attr("data-val");// Get InputId for the Records
        var CombineId = $(".dnbDunsNumber").attr("data-val") + "DUNS" + InputId;// Get DUNS number of the this records
        var Seqence = parseInt($(".Seq").attr("data-val")); //Get Sequece of the records.
        parent.$("#" + InputId + "RejectAll").removeClass("parentbtnMatchRejected").addClass("parentbtnUnselected");// Remove Class form the Rejected button and add class
        parent.$("#" + InputId + "RejectAll").removeClass("parentbtnIdel").addClass("parentbtnUnselected");// Remove Class form the Rejected button and add class
        parent.$("#" + InputId + "NotSelected").removeClass("NotSelected").addClass("SelectMathced");// Remove selected mark in company record.
        parent.$("." + InputId).each(function () {
            $(this).removeClass("btnSelected").addClass("btnUnselected");
        });// Remove all selected matches
        parent.$("#" + CombineId).find("#" + InputId + "InnerMatch").removeClass("btnUnselected").addClass("btnSelected")// Match mark as selected.        
        $(this).removeClass("btnUnselectedMatchDetailReview").addClass("btnSelectedMatchDetailReview");// Current records mark as selected.
        var token = $('input[name="__RequestVerificationToken"]').val();
        parent.MatchCallAcceptLCMMatches(InputId, Seqence, token);// Update Model and rebind the data

        $(this).parent().find(".displaylbl").text("Selected");// Change the text of label of the current button
        if ($("#Approve").val() == "False") {
            // Open Stewardship note 
            var href = "/StewardshipPortal/OpenSearchData?InputId=" + InputId + "&count=" + Seqence + "&Notes=";
            $.magnificPopup.open({
                closeBtnInside: true,
                type: 'iframe',
                items: {
                    src: href
                },
                callbacks: {
                    close: function () {
                    }
                },
                closeOnBgClick: false,
                mainClass: 'popNotes'
            });
            return false;
        }
        return false;
    });

});

$('body').on('click', '.btnSelectedMatchDetailReview', function () {

    var InputId = $(".CustomAttribute").attr("data-val");// Get InputId for the Records
    var Seqence = parseInt($(".Seq").attr("data-val"));//Get Sequece of the records.
    parent.$("." + InputId).each(function () {
        $(this).removeClass("btnSelected").addClass("btnUnselected");
    });// Remove all selected matches
    $(this).removeClass("btnSelectedMatchDetailReview").addClass("btnUnselectedMatchDetailReview");// Current records mark as selected.
    parent.$("#" + InputId + "NotSelected").removeClass("SelectMathced").addClass("NotSelected");// Remove selected mark in company record.
    $(this).parent().find(".displaylbl").text("Select");// Change the text of label of the current button
    var token = $('input[name="__RequestVerificationToken"]').val();
    parent.MatchCallRejectLCMMatches(InputId, Seqence, token);// Update model 
});
// For Popup close
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
// Next button event for Match Detail Review Popup
function nextClick(objThis) {

    //parent.matchItemNextClick($(objThis).data('val'),$(objThis).attr('data-Id'));
    $("#divProgress").show();
    var url = "";
    var QueryString = "";
    var IsSearchData = $("#IsSearchData").val();
    if ($(objThis).parent().attr('class') != "disabled") {

        var InputId = $(objThis).attr("data-Id");
        var SrcId = $(objThis).attr("data-SrcId");
        var DUNS = $(objThis).data('val');
        var dataNext = $(objThis).attr("data-Next");
        var dataPrev = $(".dnbDunsNumber").attr("data-val");
        if (dataNext == undefined) {
            dataNext = "";
        }
        if (dataPrev == undefined || dataPrev == "") {
            dataPrev = $(".dnbDunsNumber").attr("data-val");
        }
        var Count = $(".Seq").attr("data-val");

        if (IsSearchData == "StewardshipPortal") {
            QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:AdditionalFields' + '@#$IsPartialView:true';
            url = '/StewardshipPortal/cShowMatchedItesDetailsView' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        } else if (IsSearchData == "SearchData") {
            QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:null' + '@#$IsPartialView:true';
            url = '/SearchData/cShowMatchedItesDetailsView' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");;
        }
        if (IsSearchData == "BadInputData") {
            QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:AdditionalFields' + '@#$IsPartialView:true' + "@#$SrcId:" + SrcId;
            url = '/BadInputData/cShowMatchedItesDetailsView' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        }
        if (IsSearchData == "ApproveMatch") {
            QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:AdditionalFields' + '@#$IsPartialView:true';
            url = '/ApproveMatchData/cShowMatchedItesDetailsView' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        }
        $.ajax({
            type: 'GET',
            url: url,
            dataType: 'HTML',
            async: false,
            cache: false,
            contentType: 'application/html',
            data: {
                pagevalue: $("#pagevalue").val()
            },
            success: function (data) {
                $("#MatchedcontentPopup").html(data);
            }
        });
        if ($("#form_SearchBeneficialOwnershipData").length < 1)
            parent.matchItemNextClick($(objThis).data('val'), $(objThis).attr('data-Id'));

    }
    //parent.matchItemNextClick($(objThis).data('val'), $(objThis).attr('data-Id'));
    $("#divProgress").hide();
}
// Previous button event for Match Detail Review Popup
function prevClick(objThis) {
    //parent.matchItemPrevClick($(objThis).data('val'), $(objThis).attr('data-Id'));
    $("#divProgress").show();
    var url = "";
    var QueryString = "";
    var IsSearchData = $("#IsSearchData").val();

    // New code
    if ($(objThis).parent().attr('class') != "disabled") {
        var InputId = $(objThis).attr("data-Id");
        var SrcId = $(objThis).attr("data-SrcId");
        var DUNS = $(objThis).data('val');
        var dataNext = $(".dnbDunsNumber").attr("data-val");// $(".matchdetail-Next").attr("data-val");
        var dataPrev = $(objThis).attr("data-prev")
        if (dataNext == undefined || dataNext == "") {
            dataNext = $(".dnbDunsNumber").attr("data-val");
        }
        if (dataPrev == undefined) { dataPrev = ""; }
        var Count = $(".Seq").attr("data-val");

        if (IsSearchData == "StewardshipPortal") {
            QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:AdditionalFields' + '@#$IsPartialView:true';
            url = '/StewardshipPortal/cShowMatchedItesDetailsView' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        }
        else if (IsSearchData == "SearchData") {
            QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:null' + '@#$IsPartialView:true';
            url = '/SearchData/cShowMatchedItesDetailsView' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        }
        else if (IsSearchData == "BadInputData") {
            QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:AdditionalFields' + '@#$IsPartialView:true' + "@#$SrcId:" + SrcId;
            url = '/BadInputData/cShowMatchedItesDetailsView' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        }
        else if (IsSearchData == "ApproveMatch") {
            QueryString = "id:" + InputId + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + Count + '@#$type:AdditionalFields' + '@#$IsPartialView:true';
            url = '/ApproveMatchData/cShowMatchedItesDetailsView' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        }
        $.ajax({
            type: 'GET',
            url: url,
            dataType: 'HTML',
            async: false,
            cache: false,
            contentType: 'application/html',
            data: {
                pagevalue: $("#pagevalue").val()
            },
            success: function (data) {
                $("#MatchedcontentPopup").html(data);
            }
        });
        if ($("#form_SearchBeneficialOwnershipData").length < 1)
            parent.matchItemPrevClick($(objThis).data('val'), $(objThis).attr('data-Id'));
    }
    //parent.matchItemPrevClick($(objThis).data('val'), $(objThis).attr('data-Id'));
    $("#divProgress").hide();
}
// Add Company Popup open 
$('body').on('click', '.btnAddCompany', function (e) {
    var thisBtn = this;
    $(thisBtn).attr("disabled", "disabled");
    setTimeout(function () {
        $(thisBtn).removeAttr("disabled", false);
    }, 1500);

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
});

function ConvertEncrypte(value) {
    var newvalue = "";
    if (value != '' && value != undefined) {
        $.ajax({
            type: "POST",
            url: "/Home/GetEncryptedString",
            data: JSON.stringify({ strValue: value }),
            dataType: "json",
            contentType: "application/json",
            async: false,
            success: function (data) {
                newvalue = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
    return newvalue;
}

$("body").on("click", "#MatchDetailModal #lnkCollapse", function () {
    var IsExpand = false;
    if (!$("#MatchDetailModal #lnkCollapse i").hasClass("fa-minus")) {
        IsExpand = true;
    }
    $.ajax({
        type: "GET",
        url: '/StewardshipPortal/SetSession',
        contentType: "application/json; charset=utf-8",
        data: { IsExpand: IsExpand },
        dataType: "json",
        success: function () { }
    });
});

$("body").on("hidden.bs.collapse shown.bs.collapse", ".additional-main", function () {
    $(this).find(".more-less").toggleClass('fa-plus fa-minus');

});

function backToparent2() {
    $.magnificPopup.close();
}

$('body').on('click', '.clsEnrichment', function () {


    var row = $('#MatchDetailModal #btnAddCompany');
    var data = JSON.parse(row.attr("data-val"));

    var DunsNumber = $(this).attr("data-dunsnumber");
    var Company = $(this).attr("data-company");
    var Address = $(this).attr("data-street") ? $(this).attr("data-street") + " " + $(this).attr("data-address") : $(this).attr("data-address");
    var City = $(this).attr("data-city");
    var State = $(this).attr("data-state");
    var Postal = $(this).attr("data-postal");
    var CountryCode = "";
    var SrcId = $(this).attr("data-srcId");
    var Phone = $(this).attr("data-phone");
    var Country = $(this).attr("data-country");

    var QueryString = "DunsNumber:" + DunsNumber + "@#$Company:" + Company + "@#$Address:" + Address + "@#$City:" + City + "@#$State:" + State + "@#$Postal:" + Postal + "@#$CountryCode:" + CountryCode + "@#$SrcId:" + SrcId + "@#$Phone:" + Phone + "@#$Country:" + Country;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: 'GET',
        url: "/StewardshipPortal/PreviewEnrichmentData?Parameters=" + Parameters,
        dataType: 'HTML',
        success: function (data) {
            $("#CleanSearchEnrichmentDetailModalMain").html(data);
            DraggableModalPopup("#EnrichmentDetailModal");
        }
    });
});

//Preview Enrichment Input data show hide

$(document).on('click', '#lnkShowHideInputDetails', function () {
    var flag = $(this).attr("data-flag");
    if (flag === "hide") {
        $(this).text(hideInputDetails);
        $(this).attr("data-flag", "show");
        $('#articleEnrichmentInputData').show();
    }
    else {
        $(this).text(showInputDetails);
        $(this).attr("data-flag", "hide");
        $('#articleEnrichmentInputData').hide();
    }
})