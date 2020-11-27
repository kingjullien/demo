$(function () {
    // set current record is selected match or not and set text according to this from model.
    $('.mfp-iframe').contents().find("html body #contentPopup .btnUnselected,html body #contentPopup .btnSelected").each(function () {

        var parentBtnclass = $('div#' + this.id.replace(' ', '')).attr('class');
        $(this).removeAttr('class').addClass(parentBtnclass);
        if (parentBtnclass.indexOf("btnSelected") > -1) {
            $(this).parent().find(".displaylbl").text("Selected");
        }
    });

    $('.mfp-iframe').contents().find("html body #contentPopup .btnUnselected, html body #contentPopup .btnSelected").click(function () {
        // make current match select button green as well parent grid change button color black to green and update model according this changes
        if ($(this)[0].classList[2] == "btnUnselected") {

            var SrcId = $(this)[0].classList[0];
            $('div#' + this.id).parent().parent().parent().parent().parent().parent().parent().prev('tr').find("." + SrcId).removeClass("parentbtnMatchRejected").addClass("parentbtnUnselected");
            $('div#' + this.id).parent().parent().parent().parent().parent().parent().parent().prev('tr').find("." + SrcId).removeClass("parentbtnIdel").addClass("parentbtnUnselected");
            $('div#' + this.id).parent().parent().parent().parent().parent().parent().parent().prev('tr').find("." + SrcId).parent().prev().removeClass("NotSelected").addClass("SelectMathced");
            $('.' + SrcId + '.btnSelected').each(function () {
                $(this).removeClass("btnSelected").addClass("btnUnselected");
            });
            $(this).removeClass("btnUnselected").addClass("btnSelected");
            $('div#' + this.id.replace(' ', '')).removeClass("btnUnselected").addClass("btnSelected");
            CallAcceptLCMMatches(this.id);
            $(this).parent().find(".displaylbl").text("Selected");

        }
        else if ($(this)[0].classList[2] == "btnSelected") {
            // make current match select button black as well parent grid change button color green to black and update model according this changes
            var SrcId = $(this)[0].classList[0];
            $('.' + SrcId + '.btnSelected').each(function () {
                $(this).removeClass("btnSelected").addClass("btnUnselected");
            });
            $(this).removeClass("btnSelected").addClass("btnUnselected");
            $('div#' + this.id).removeClass("btnSelected").addClass("btnUnselected");
            var newId = "#" + SrcId;
            $(".trStewardshipPortal_td12").find(newId).parent().prev().removeClass("SelectMathced").addClass("NotSelected");
            $(this).parent().find(".displaylbl").text("Select");
            CallAcceptLCMMatches(this.id);
        }
    });
});
// Next button event for Match Detail Review Popup
function nextClick(objThis) {
    //parent.matchItemNextClick($(objThis).data('val'), $(objThis).attr('data-Id'));
    if ($(objThis).parent().attr('class') != "disabled") {
        
        var InputId = $(objThis).attr("data-val");
        var DUNS = $(objThis).attr('data-nextduns');
        var dataNext = $(objThis).attr("data-next");
        var dataPrev = $(objThis).attr("data-Id");
        var TopMatchCandidate = $("#TopMatchCandidate").val();
        var CountryGroup = $("#CountryGroup").val();
        var OrderBy = $("#OrderBy").val();
        if (dataNext == undefined)
        {
            dataNext = "";
        }
        if (dataPrev == undefined)
        {
            dataPrev = "";
        }
        
        var QueryString = "id:" + InputId + "@#$TopMatchCandidate:" + TopMatchCandidate + "@#$CountryGroup:" + CountryGroup + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$DUNS:" + DUNS + "@#$OrderBy:" + OrderBy + '@#$IsPartialView:true';
        var url = '/Review/MatchDetailView_Item' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
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
       
        parent.matchItemNextClick($(objThis).data('val'), $(objThis).attr('data-Id'));

    }
}
// Previous button event for Match Detail Review Popup
function prevClick(objThis) {
    if ($(objThis).parent().attr('class') != "disabled") {
       
        var InputId = $(objThis).attr("data-val");
        var DUNS = $(objThis).attr('data-prevduns');
        var dataNext =$(objThis).attr("data-Id"); 
        var dataPrev = $(objThis).attr("data-prev");
        var TopMatchCandidate = $("#TopMatchCandidate").val();
        var CountryGroup = $("#CountryGroup").val();
        var OrderBy = $("#OrderBy").val();
        if (dataNext == undefined) {
            dataNext = "";
        }
        if (dataPrev == undefined) {
            dataPrev = "";
        }

        var QueryString = "id:" + InputId + "@#$TopMatchCandidate:" + TopMatchCandidate + "@#$CountryGroup:" + CountryGroup + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$DUNS:" + DUNS + "@#$OrderBy:" + OrderBy + '@#$IsPartialView:true';
        var url = '/Review/MatchDetailView_Item' + '?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
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

        parent.matchItemNextClick($(objThis).data('val'), $(objThis).attr('data-Id'));

    }
}
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
    return false;
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
