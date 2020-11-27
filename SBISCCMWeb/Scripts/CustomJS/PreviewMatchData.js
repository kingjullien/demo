$(document).ready(function () {
    $('table#accordion tbody tr').each(function () {
        $(this).find('td:last').attr('title', '');
        $(this).find('td:nth-last-of-type(2)').attr('title', '');

    });
    //["TableCoulmnName","ServerColumnName","DropDownURL","ISDefault(use lowercase)","Show text/value(use lowercase)","Datatype(If Date)"]
    var colArray = [["AcceptedBy", "AcceptedBy", "/PreviewMatchData/GetAutoAcceptDD", "true", "text"],
    ["SrcRecordId", "SrcRecordId", "", "", "", "onlytext"],
    ["SourceRecordIdExactMatch", "SourceRecordIdExactMatch", "/PreviewMatchData/GetIsExactMatchDD", "false", "value", "onlyselect"],
    ["ImportProcess", "ImportProcess", "/PreviewMatchData/GetImportProcessDD", "false", "value", "onlyselect"],
    ["ConfidenceCode", "ConfidenceCode", "/PreviewMatchData/GetConfidenceCodeDD", "false"],
    ["Tag", "Tag", "/PreviewMatchData/GetTagsDD", "false", "text", "onlyselect"],
    ["LOBTag", "LOBTag", "/PreviewMatchData/GetLOBTagsDD", "false", "value", "onlyselect"]
    ];

    //Column array,URL for FilterData, TargetedDiv, Datatable function
    InitFilters(colArray, "/PreviewMatchData/FilterPreviewMatchData", "#PreviewMatchDataFilterContainer", "#divGrid", "", "equalto");
});
function HideShowMenu(e, tabMenuId) {

    var c = $(e).children().attr("class");
    if (c == "fa fa-minus") {
        $(e).children().removeClass();
        $(e).children().addClass("fa fa-plus");
        $("#" + tabMenuId).hide();

    }
    else {
        $(e).children().removeClass();
        $(e).children().addClass("fa fa-minus");
        $("#" + tabMenuId).show();
    }

}

function HideShowMenuMatchOutPut(e, tabMenuId) {

    var c = $(e).children().attr("class");
    if (c == "fa fa-minus") {
        $(e).children().removeClass();
        $(e).children().addClass("fa fa-plus");
        $("#" + tabMenuId).hide();

    }
    else {
        $(e).children().removeClass();
        $(e).children().addClass("fa fa-minus");
        $("#" + tabMenuId).show();
    }
}

$(function () {
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
$('body').on('click', '.btnDCP', function (item) {

    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'inline',
        items: {
            src: $('.divDcpIndex').html(),
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: 'popupMain'
    });

    return false;
});

function BindTab() {
    $('.tabs').each(function (i, el) {
        $(el).tabs();
    });
}

$(document).on('click', '.PreviewMatchDunsPopup', function () {
    var DunsNumber = $(this).attr("data-dunsnumber");
    var QueryString = "DunsNumber:" + DunsNumber;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

    $.ajax({
        type: 'GET',
        url: '/PreviewMatchData/ViewDetails?Parameters=' + Parameters,
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#PreviewMatchViewDetailsModalMain").html(data);
            DraggableModalPopup("#PreviewMatchViewDetailsModal");
        }
    });
});
