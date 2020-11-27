$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});
$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});
// Save the Auto Acceptance Record



$('body').on('click', '#btnRunRule', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "This will auto-accept matches currently in the Match Data Queue based on these match criteria.", callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/AutoAcceptance/RunAutoAcceptanceRule/",
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    success: function (data) {
                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                            message: "Matches accepted based on match criteria"
                        });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    return false;
});
function backToparent() {

    window.parent.$.magnificPopup.close();
    window.location = "/AutoAcceptance/Index";
};
$('body').on('click', '#btnExportToExcel', function () {
    $("#frmExportToExcel").submit();
});

$('body').on('click', '#btnclearFilter', function () {
    $("#frmfilter").submit();
});

//Close Import panel
function CloseImportPanel() {
    $.magnificPopup.close();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/AutoAcceptance/CleanseMatchDataMatch/'
        },
        callbacks: {
            close: function () {
                $.ajax({
                    type: "GET",
                    url: "/AutoAcceptance/Index/",
                    dataType: "HTML",
                    contentType: "application/html",
                    cache: false,
                    data: {},
                    success: function (data) {
                        $("#divPartialAutoAcceptance").html(data);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupAutoAcceptanceData'
    });
}

$("body").on('change', '.pagevalueChangeAutoAcceptanceCriteria', function () {

    var pagevalue = $(this)[0].value;
    var SortBy = $("#SortBy").val();
    var SortOrder = $("#SortOrder").val();
    var ConfidenceCode = $("#ConfidenceCode").val();
    var MatchGrade = $("#MatchGrade").val();
    var CountyGroupId = $("#CountyGroupId").val();
    var Tags = $("#Tags").val();
    var url = '/AutoAcceptance/Index' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder + "&ConfidenceCode=" + ConfidenceCode + "&MatchGrade=" + encodeURIComponent(MatchGrade) + "&CountyGroupId=" + CountyGroupId + "&Tags=" + Tags;
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $("#divPartialAutoAcceptance").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

function OnSuccess() {
    $('#divProgress').hide();
};






function ReloadForm() {
    window.parent.$.magnificPopup.close();
    var pagevalue = $(".pagevalueChangeAutoAcceptanceCriteria")[0].value;
    var SortBy = $("#SortBy").val();
    var SortOrder = $("#SortOrder").val();
    var ConfidenceCode = $("#ConfidenceCode").val();
    var MatchGrade = $("#MatchGrade").val();
    var Tags = $("#Tags").val();
    var CountyGroupId = $("#CountyGroupId").val();
    var url = '/AutoAcceptance/Index' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder + "&ConfidenceCode=" + ConfidenceCode + "&CountyGroupId=" + CountyGroupId + "&MatchGrade=" + encodeURIComponent(MatchGrade) + "&Tags=" + Tags;
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divPartialAutoAcceptance").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

function ClosePopupReload() {
    window.parent.$.magnificPopup.close();
    var url = '/AutoAcceptance/Index';
    window.location.href = url;
}


// toggle event for Child Data Show  in Match Data Table on "+" icon 
$('body').on('show.bs.collapse', '.collapse', function () {
    $('.partialrow').each(function () {
        $(this).removeClass("current");
    });
    $(".panel-collapse.in").collapse('hide');
    $('.trAutoAcceptanceItemView').each(function () {
        $(this).hide();
    });
    $(this).parents('tr').show();
    $(this).parents('tr').prev().addClass("current");
});
// toggle event for Child Data Hide in Match Data Table on "-" icon 
$('body').on('hide.bs.collapse', '.collapse', function () {
    $(this).parents('tr').hide();

});
$('body').on('click', '.collapsed', function () {
    var curntclass = $(this);
    $(".removecollapsed").each(function () {
        $(this).removeClass("removecollapsed");
        $(this).addClass("collapsed");
    });
    $(curntclass).addClass("removecollapsed");
    $(curntclass).removeClass("collapsed");

    var CriteriaGroupId = $(this).attr("data-CriteriaGroupId");
    $.ajax({
        type: "GET",
        url: "/AutoAcceptance/GetAutoAcceptanceCriteriaDetailByGroupId?CriteriaGroupId=" + CriteriaGroupId,
        dataType: "HTML",
        contentType: "application/html",
        cache: false,
        success: function (data) {
            $(".trAutoAcceptanceItemView").each(function () {
                $(this).remove();
            });
            $(curntclass).closest('TR').after(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    //$.magnificPopup.open({
    //    preloader: false,
    //    closeBtnInside: true,
    //    type: 'iframe',
    //    items: {
    //        src: '/AutoAcceptance/GetAutoAcceptanceCriteriaDetailByGroupId?CriteriaGroupId=' + CriteriaGroupId,
    //    },
    //    callbacks: {
    //        close: function () {
    //        }
    //    },
    //    closeOnBgClick: false,
    //    mainClass: 'popupAutoAcceptanceCriteriaDetail'
    //});
   
});
$('body').on('click', '.removecollapsed', function () {
    //$(".removecollapsed").each(function () {
    //    $(this).addClass("collapsed");
    //    $(this).removeClass("removecollapsed");
    //});
    $(this).addClass("collapsed");
    $(this).removeClass("removecollapsed");
    $(".trAutoAcceptanceItemView").each(function () {
        $(this).remove();
    });
});