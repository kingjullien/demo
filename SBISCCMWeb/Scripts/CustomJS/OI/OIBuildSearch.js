$(document).ready(function () {
    $('#MultiSelectOptionsEmployees').multiselect({
        nonSelectedText: selectEmployees,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }

    });
    $('#MultiSelectOptionRevenue').multiselect({
        nonSelectedText: selectRevenue,
        onChange: function (option, checked) {
            onchangeMultiSelect(option, checked);
        }
    });
    $("select.chzn-select").chosen({
        no_results_text: nothingFound,
        width: "100%",
        search_contains: true
    });
});

$("body").on('click', '.btnOISearchHistory', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/OIBuildList/GetOISearchHistory",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#GetOISearchHistoryModalMain").html(data);
            DraggableModalPopup("#GetOISearchHistoryModal");
            InitDataTable(".MatchedItems", [5, 10, 15], false, ['0', 'asc']);
        }
    });

    return false;
});

$("body").on('change', '.pagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    $('#divProgress').show();
    var url = '/OIBuildList/Index?' + "pagevalue=" + pagevalue;
    $("#divBuildList").load(url, function () {
        BindTab();
        $('#divProgress').hide();
    });
});

$('body').on('show.bs.collapse', '.collapsed', function () {
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
$('body').on('hide.bs.collapse', '.collapsed', function () {
    $(this).parents('tr').hide();
});


function onchangeMultiSelect(option, checked) {
    var ControllerId = "#" + option[0].parentElement.id;
    if (ControllerId.toLowerCase() == "#multiselectoptionrevenue") {
        $("#request_fields_revenue").val($(ControllerId).val());
    }
    if (ControllerId.toLowerCase() == "#multiselectoptionsemployees") {
        $("#request_fields_employees").val($(ControllerId).val());
    }
}

function OIExportToExcel() {
    if ($(".divOIBuildSearch").length > 0) {
        var strurl = "/OIBuildList/ExportToExcel";
        window.location.href = strurl;
    }
    else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", noRecordsFound, false);
    }
}
function OnSearch(data) {
    $("#divGrid").html(data);
    BindTab();
}
function BindTab() {
    $('.tabs').each(function (i, el) {
        $(el).tabs();
    });
}
function ClearOIDataBuildList() {
    $("#request_fields_industry").val($("#request_fields_industry option:first").val());
    $("#request_fields_industry").trigger("chosen:updated");
    $("#request_fields_entity_type").val('');
    $("#request_fields_category").val($("#request_fields_category option:first").val());
    $("#request_fields_category").trigger("chosen:updated");
    $("#MultiSelectOptionsEmployees").multiselect("clearSelection");
    $("#MultiSelectOptionsEmployees").multiselect('refresh');
    $("#MultiSelectOptionRevenue").multiselect("clearSelection");
    $("#MultiSelectOptionRevenue").multiselect('refresh');
    $('input[type="radio"]').prop('checked', true);
    $("#request_fields_orb_num").val('');
    $("#request_fields_parent_orb_num").val('');
    $("#request_fields_ultimate_parent_orb_num").val('');
    $("#request_fields_address1").val('');
    $("#request_fields_city").val('');
    $("#request_fields_state").val('');
    $("#request_fields_zip").val('');
    $("#request_fields_country").val('');
    $("#request_fields_techs").val($("#request_fields_techs option:first").val());
    $("#request_fields_techs").trigger("chosen:updated");
    $("#request_fields_tech_categories").val($("#request_fields_tech_categories option:first").val());
    $("#request_fields_tech_categories").trigger("chosen:updated");
    $("#request_fields_naics_codes").val($("#request_fields_naics_codes option:first").val());
    $("#request_fields_naics_codes").trigger("chosen:updated");
    $("#request_fields_sic_codes").val($("#request_fields_sic_codes option:first").val());
    $("#request_fields_sic_codes").trigger("chosen:updated");
    $("#request_fields_cik").val('');
    $("#request_fields_ticker").val($("#request_fields_ticker option:first").val());
    $("#request_fields_ticker").trigger("chosen:updated");
    $("#request_fields_cusip").val('');
    $("#request_fields_include").val('');
    $("#request_fields_exchange").val('');
    $("#request_fields_importance_score").val('');
    $("#request_fields_rankings").val($("#request_fields_rankings option:first").val());
    $("#request_fields_rankings").trigger("chosen:updated");
    $("#divGrid").html('');
    $("#divGrid").html('<table class=""><td colspan="10" class="noContain">' + noDataAreAvailable + '</td></table>');
}
function ClosePopup(id) {
    window.parent.$(".loaderMain").show();
    window.parent.$(".loaderMain").css("z-index", "9999999");
    $("#GetOISearchHistoryModal").modal("hide");
    $.ajax({
        type: "POST",
        url: '/OIBuildList/ViewHistory',
        data: JSON.stringify({ Id: id }),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",       
        processData: false,
        async: false,
        success: function (response) {
            if (response.Success) {
                var reqJson = jQuery.parseJSON(response.Object[0].RequestJson);
                var resJson = JSON.stringify(JSON.parse(response.Object[0].ResponseJson), null, '\t');
                if (reqJson != null) {
                    window.parent.$("#request_fields_industry").val(reqJson.industry);
                    window.parent.$("#request_fields_industry").trigger("chosen:updated");
                    window.parent.$("#request_fields_entity_type").val(reqJson.entity_type);
                    window.parent.$("#request_fields_category").val(reqJson.category);
                    window.parent.$("#request_fields_category").trigger("chosen:updated");
                    if (reqJson.employees.length > 0) {
                        var lstValueEmployees = reqJson.employees[0].split(',');
                        window.parent.$("#MultiSelectOptionsEmployees option").each(function () {
                            for (var i = 0; i < lstValueEmployees.length; i++) {
                                if ($(this).val() == lstValueEmployees[i]) {
                                    $(this).attr("selected", "selected");
                                }
                            }
                        });
                    }
                    window.parent.$("#MultiSelectOptionsEmployees").multiselect('refresh');
                    if (reqJson.revenue.length > 0) {
                        var lstValueRevenue = reqJson.revenue[0].split(',');
                        window.parent.$("#MultiSelectOptionRevenue option").each(function () {
                            for (var i = 0; i < lstValueRevenue.length; i++) {
                                if ($(this).val() == lstValueRevenue[i]) {
                                    $(this).attr("selected", "selected");
                                }
                            }
                        });
                    }
                    window.parent.$("#MultiSelectOptionRevenue").multiselect('refresh');
                    if (reqJson.show_full_profile == true) {
                        window.parent.$("#request_fields_show_full_profile").prop('checked', true);
                    }
                    else if (reqJson.show_full_profile == false) {
                        window.parent.$("#request_fields_show_full_profile").prop('checked', false);
                    }
                    window.parent.$("#request_fields_orb_num").val(reqJson.orb_num);
                    window.parent.$("#request_fields_parent_orb_num").val(reqJson.parent_orb_num);
                    window.parent.$("#request_fields_ultimate_parent_orb_num").val(reqJson.ultimate_parent_orb_num);
                    window.parent.$("#request_fields_address1").val(reqJson.address1);
                    window.parent.$("#request_fields_city").val(reqJson.city);
                    window.parent.$("#request_fields_state").val(reqJson.state);
                    window.parent.$("#request_fields_zip").val(reqJson.zip);
                    window.parent.$("#request_fields_country").val(reqJson.country);
                    window.parent.$("#request_fields_techs").val(reqJson.techs);
                    window.parent.$("#request_fields_techs").trigger("chosen:updated");
                    window.parent.$("#request_fields_tech_categories").val(reqJson.tech_categories);
                    window.parent.$("#request_fields_tech_categories").trigger("chosen:updated");
                    window.parent.$("#request_fields_naics_codes").val(reqJson.naics_codes);
                    window.parent.$("#request_fields_naics_codes").trigger("chosen:updated");
                    window.parent.$("#request_fields_sic_codes").val(reqJson.sic_codes);
                    window.parent.$("#request_fields_sic_codes").trigger("chosen:updated");
                    window.parent.$("#request_fields_cik").val(reqJson.cik);
                    window.parent.$("#request_fields_ticker").val(reqJson.ticker);
                    window.parent.$("#request_fields_ticker").trigger("chosen:updated");
                    window.parent.$("#request_fields_cusip").val(reqJson.cusip);
                    window.parent.$("#request_fields_include").val(reqJson.include);
                    window.parent.$("#request_fields_exchange").val(reqJson.exchange);
                    window.parent.$("#request_fields_importance_score").val(reqJson.importance_score);
                    window.parent.$("#request_fields_rankings").val(reqJson.rankings);
                    window.parent.$("#request_fields_rankings").trigger("chosen:updated");

                    if (response.ResponseString != '' || response.ResponseString != null) {
                        window.parent.$("#divGrid").html(response.ResponseString);
                        BindTab();
                    }
                    else {
                        window.parent.$("#divGrid").html('<table class=""><td colspan="10" class="noContain">' + noDataAreAvailable + '</td></table>');
                        if (window.parent.$(".searchResult ul.nav-pills li a i").attr("class") == "fa fa-plus") {
                            window.parent.$(".searchResult ul.nav-pills li a").click();
                        }
                    }
                    window.parent.$.magnificPopup.close();
                }
            }
        }, error: function (xhr, ajaxOptions, thrownError) {
        }

    });
}