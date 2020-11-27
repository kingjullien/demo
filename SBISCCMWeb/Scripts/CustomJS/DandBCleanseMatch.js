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
        url: "/DNBIdentityResolution/GetAutoAcceptanceCriteriaDetailByGroupId?CriteriaGroupId=" + CriteriaGroupId,
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

});
$('body').on('click', '.removecollapsed', function () {
    $(this).addClass("collapsed");
    $(this).removeClass("removecollapsed");
    $(".trAutoAcceptanceItemView").each(function () {
        $(this).remove();
    });
});
var MultiAutoAccept = "";
$("body").on('change', '#MultipleSelect', function () {
    MultiAutoAccept = "";
    if ($(this).prop('checked') == true) {
        $(".SelectedDelete").prop('checked');
        $(".SelectedDelete").each(function () {
            MultiAutoAccept = MultiAutoAccept + $(this).attr('data-val') + ",";
            $(this).prop('checked', true);
        });
    }
    else {
        $(".SelectedDelete").each(function () {
            $(this).prop('checked', false);
        });
    }
    $("#MultiDeleteAutoAccept").val(MultiAutoAccept);
});
$("body").on('change', '.SelectedDelete', function () {
    var CurrentValue = $(this).attr('data-val');
    if ($(this).prop('checked') == true) {
        MultiAutoAccept = MultiAutoAccept + CurrentValue + ",";
    }
    else {
        $('#MultipleSelect').prop('checked', false);
        var lstMultiAutoAccept = MultiAutoAccept.split(',');
        var MultiAutoAccept1 = "";
        for (var i = 0; i < lstMultiAutoAccept.length; i++) {
            if (lstMultiAutoAccept[i] != "")
                if (CurrentValue != lstMultiAutoAccept[i]) {
                    MultiAutoAccept1 = MultiAutoAccept1 + lstMultiAutoAccept[i] + ",";
                }
        }
        MultiAutoAccept = MultiAutoAccept1;
    }
    var TotalCount = 0;
    var SelectedCount = 0;
    $(".SelectedDelete").each(function () {
        TotalCount += 1;
        if ($(this).prop('checked') == true) {
            SelectedCount += 1;
        }
    });
    if (TotalCount == SelectedCount) {
        $("#MultipleSelect").prop('checked', true);
    }
    $("#MultiDeleteAutoAccept").val(MultiAutoAccept);
});
function OnSuccessAutoAcceptance() {
    $('#divProgress').hide();
    $("select.chzn-select").chosen({
        no_results_text: "Oops, nothing found!",
        width: "100%",
        search_contains: true
    });
}
$('body').on('click', '#btnImportData', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/DNBIdentityResolution/ImportData",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#divProgress").hide();
            $("#DAndBAutoAcceptanceImportModalMain").html(data);
            DraggableModalPopup("#DAndBAutoAcceptanceImportModal");
            onLoadCleansMatchImportData();
        }
    });
});
//Close Import panel
function CloseImportPanel() {
    // Changes for Converting magnific popup to modal popup
    $("#DAndBAutoAcceptanceImportModal").modal("hide");
    $.ajax({
        type: 'GET',
        url: "/DNBIdentityResolution/CleanseMatchDataMatchAutoAccept",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#DAndBAutoAcceptImportModalMain").html(data);
            DraggableModalPopup("#DAndBAutoAcceptImportModal");
            onLoadCleansMatchImportData();
        }
    });
}
function isInArray(value, array) {
    return array.indexOf(value) > -1;
}
function RangeValidation(value) {
    var val = value.replace(/[^0-9\.]/g, '');
    return Math.abs(val);
}
//Minimum Confidence Code Override

$('body').on('click', 'table.MinCCOverrideTB tbody tr', function () {
    if ($("table.MinCCOverrideTB tbody tr:first").html().indexOf("No record(s) Found") < 0) {
        if (!$(this).hasClass("current")) {
            $("table.MinCCOverrideTB tbody tr").each(function () {
                $(this).removeClass('current');
            });
            $(this).closest('tr').addClass('current');
            var MinCCID = $(this).attr("data-MinCCID");
            $.ajax({
                type: 'GET',
                cache: false,
                url: "/DNBIdentityResolution/UpsertMinimumConfidenceCodeOverride?Parameters=" + ConvertEncrypte(encodeURI(MinCCID)).split("+").join("***"),
                dataType: 'HTML',
                contentType: 'application/html',
                async: false,
                success: function (data) {
                    $("#divPartialAddUpdateMinimumConfidenceCodeOverride").html(data);

                    //LoadTagsMinCCOverride();
                }
            });
        }
    }
});

//====================================================================================================================

//Start Minimum Confidence Code Override
function LoadIndexMinimumConfidenceCodeOverride() {
    $.ajax({
        type: 'GET',
        url: "/DNBIdentityResolution/IndexMinimumConfidenceCodeOverride",
        dataType: 'html',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#Auto-AcceptOverrite #divPartialMinimumConfidenceCodeOverride").html(data);
            InitMinimumMatchCriteriaDataTable();
            $("table.MinCCOverrideTB tbody tr:first").addClass('current');
            if ($("table.MinCCOverrideTB tbody tr:first").html().indexOf("No record(s) Found") > 0) {
                $("table.MinCCOverrideTB tbody tr:first").removeClass('current');
            }
            var QueryString = $("table.MinCCOverrideTB tbody tr:first").attr("data-MinCCID");
            if (QueryString == undefined) {
                QueryString = 0;
            }
            if (QueryString != null) {
                selectFirstTRMinOverride(QueryString);
            }
            InitMinimumMatchCriteriaDataTable();
            $("#Auto-AcceptOverrite #divPartialAddUpdateMinimumConfidenceCodeOverride").html(data);
        }
    });
}
function selectFirstTRMinOverride(QueryString) {
    InitMinimumMatchCriteriaDataTable();
    $("table.MinCCOverrideTB tbody tr:first").addClass('current');
    if ($("table.MinCCOverrideTB tbody tr:first").html().indexOf("No record(s) Found") > 0) {
        $("table.MinCCOverrideTB tbody tr:first").removeClass('current');
    }
    if (QueryString == undefined) {
        QueryString = $("table.MinCCOverrideTB tbody tr:first").attr("data-MinCCID");
    }
    var url = "/DNBIdentityResolution/UpsertMinimumConfidenceCodeOverride?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        cache: false,
        success: function (data) {
            $("table.MinCCOverrideTB tbody tr:first").addClass('current');
            $("#Auto-AcceptOverrite #divPartialAddUpdateMinimumConfidenceCodeOverride").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
function InitMinimumMatchCriteriaDataTable() {
    InitDataTable(".MinCCOverrideTB", [10, 20, 30], false, [0, "desc"]);
}
function OnSuccessUpsertMinCCOverride(data) {
    ShowMessageNotification("success", data.message);
    if (data.result) {
        LoadIndexMinimumConfidenceCodeOverride();
    }
}
function EnableMinimumCCOverride() {
    $("#frm_MinCCfrm #MinConfidenceCode").prop("disabled", false);
    $("#frm_MinCCfrm #MaxCandidateQty").prop("disabled", false);
    $("#frm_MinCCfrm .MinCCtags").prop("disabled", false);
    $("#btnMinccoverride").show();
    $("#AddMinCCTag").show();
    $("#frm_MinCCfrm #CredentialId").prop("disabled", false);
}
function setBlankMinimumCCOverride() {
    $("#frm_MinCCfrm #Id").val("0");
    $("#frm_MinCCfrm #MinConfidenceCode").val($("#frm_MinCCfrm #MinConfidenceCode option:first").val());
    $("#frm_MinCCfrm #MaxCandidateQty").val($("#frm_MinCCfrm #MaxCandidateQty option:first").val());
    $("#frm_MinCCfrm .MinCCtags").val($("#frm_MinCCfrm .MinCCtags option:first").val());
    $("#frm_MinCCfrm #CredentialId").val($("#frm_MinCCfrm #CredentialId option:first").val());

    $("#frm_MinCCfrm #Tags").val("");
    $(".MinCCtags").trigger("chosen:updated");
    $("#btnMinccoverride").val("Add");
}
$('body').on('click', '#AddMinCCOverride', function () {
    EnableMinimumCCOverride();
    setBlankMinimumCCOverride();
    $("table.MinCCOverrideTB tbody tr").each(function () {
        $(this).removeClass('current');
    });
});
$('body').on('click', '#EditMinCCOverride', function () {
    EnableMinimumCCOverride();
    $(".MinCCtag").trigger("chosen:updated");
});
$('body').on('click', '.DeleteMinCCOverride', function () {
    var id = $(this).attr('id');
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteMinCCOverride, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/DNBIdentityResolution/DeleteMinCCOverride?Parameters=" + ConvertEncrypte(encodeURI(id)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.message);
                        if (data.result) {
                            LoadIndexMinimumConfidenceCodeOverride();
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
$("body").on('keyup', '#frm_MinCCfrm #MaxCandidateQty', function (event) {
    this.value = RangeValidate(this.value, 50)
    if (this.value == "0") {
        this.value = "";
    }
});
function ValidationProcess() {
    var MatchOutput = $("#frm_MinCCfrm #Tag").val();
    var MaximumCandidateResults = $("#frm_MinCCfrm #MaxCandidateQty").val();
    var CredentialId = $("#frm_MinCCfrm #CredentialId").val();
    var cnt = 0;
    if (CredentialId == "") {
        $("#spnCredNames").show();
        cnt++;
    }
    else { $("#spnCredNames").hide(); }
    if (MatchOutput == "") {
        $("#spnTags").show();
        cnt++;
    }
    else { $("#spnTags").hide(); }

    if (MaximumCandidateResults == "0" || MaximumCandidateResults == "") {
        $("#frm_MinCCfrm #spnMaxCandidateQty").show();
        cnt++;
    }
    else { $("#frm_MinCCfrm #spnMaxCandidateQty").hide(); }
    if (cnt > 0) { return false; }
    return true;
}

//End Minimum Confidence Code Override

//====================================================================================================================
$('body').on('click', '#btnSubmitMinimumMatch', function () {
    var MinConfidenceCode = $("#formMinMatchCriteria #minConfidencelevel").val();
    var MaxCandidateQty = $("#formMinMatchCriteria #MaxCandidateQty").val();
    var Id = $("#formMinMatchCriteria #Id").val();
    var CredentialId = $("#formMinMatchCriteria #CredentialId").val();
    var cnt = 0;
    if (CredentialId == "") {
        $("#formMinMatchCriteria #spnCredName").show();
        cnt++;
    }
    else {
        $("#formMinMatchCriteria #spnCredName").hide();
    }
    if (MaxCandidateQty == "0") {
        $("#formMinMatchCriteria #spnMaxCandidateQty").show();
        cnt++;
    }
    else {
        if (MaxCandidateQty >= 1 && MaxCandidateQty <= 50) {
            $("#formMinMatchCriteria #spnMaxCandidateQty").hide();
        }
        else {
            $("#formMinMatchCriteria #spnMaxCandidateQty").show();
            cnt++;
        }
    }
    if (MaxCandidateQty == "0") {
        $("#formMinMatchCriteria #spnMaxCandidateQty").show();
        cnt++;
    }
    else {
        if (MaxCandidateQty >= 1 && MaxCandidateQty <= 50) {
            $("#formMinMatchCriteria #spnMaxCandidateQty").hide();
        }
        else {
            $("#formMinMatchCriteria #spnMaxCandidateQty").show();
            cnt++;
        }
    }
    if (MinConfidenceCode >= 1 && MinConfidenceCode <= 10) {
        $("#formMinMatchCriteria #spnMinConfidenceLevel").hide();
    }
    else {
        $("#formMinMatchCriteria #spnMinConfidenceLevel").show();
        cnt++;
    }
    if (cnt > 0) {
        return false;
    } else {

        var QueryString = "MinConfidenceCode:" + MinConfidenceCode + "@#$MaxCandidateQty:" + MaxCandidateQty + "@#$CredentialName:" + CredentialName + "@#$Id:" + Id + "@#$CredentialId:" + CredentialId;
        var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
        $.ajax({
            type: "POST",
            url: "/DNBIdentityResolution/IndexMinimumMatchCriteria?Parameters=" + Parameters,
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                ShowMessageNotification("success", data.message);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
});
function RangeValidate(value, maxlength) {
    var val = value.replace(/[^0-9\.]/g, '');
    return val > maxlength ? maxlength : Math.abs(val);
}
$("body").on('keyup', '#formMinMatchCriteria #MaxCandidateQty', function (event) {
    this.value = RangeValidation(this.value);
    if (this.value == "0") {
        this.value = "";
    }
});

//END Global Minimum Match Criteria

//====================================================================================================================

//Start Exclusions for Cleanse Match API Calls

function GetIndexExclusionsCleanseMatch() {
    // // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/DNB/ExclusionsCleanseMatch", container: '#divPartialExclusionsCleanseMatch',
        timeout: 50000
    }).done(function () {
        if ($.UserRole.toLowerCase() == "lob") {
            $("#frmCleanseExclusion .chzn-select").each(function () {
                $(this).attr("disabled", true);
            });
            $("#frmCleanseExclusion input[type=checkbox]").each(function () {
                $(this).attr("disabled", true);
            });
            $("#frmCleanseExclusion .OpenTags").each(function () {
                $(this).remove();
            });
        }
        ExclusionLoadTags();
        if ($.UserRole.toLowerCase() == "global") {
            oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active1", "#TagsValue1", "#Tags1", "#TagList1");
            oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active2", "#TagsValue2", "#Tags1", "#TagList2");
            oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active3", "#TagsValue3", "#Tags1", "#TagList3");
            oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active4", "#TagsValue4", "#Tags1", "#TagList4");
            oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active5", "#TagsValue5", "#Tags1", "#TagList5");
        }
    });
}
function oCleanseMatchExclusionsChanges(IdInputEntity, IdTagValue, IdTags, IdTagList) {
    if ($(IdInputEntity).is(':checked')) {
        $(IdTagValue).attr("disabled", false);
        $(IdInputEntity).parent().parent().find(".OpenTags").show();
    }
    else {
        $(IdTagValue).attr("disabled", true);
        $(IdTagValue).val('');
        $(IdTags).val('');
        $(IdTagList).val('');
        $(IdInputEntity).parent().parent().find(".OpenTags").hide();
    }

    $(IdTagValue).trigger("chosen:updated");
}
$("body").on("change", "#oCleanseMatchExclusionsEntity_Active1", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active1", "#TagsValue1", "#Tags1", "#TagList1");
    }
});
$("body").on("change", "#oCleanseMatchExclusionsEntity_Active2", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active2", "#TagsValue2", "#Tags1", "#TagList2");
    }
});
$("body").on("change", "#oCleanseMatchExclusionsEntity_Active3", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active3", "#TagsValue3", "#Tags1", "#TagList3");
    }
});
$("body").on("change", "#oCleanseMatchExclusionsEntity_Active4", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active4", "#TagsValue4", "#Tags1", "#TagList4");
    }
});
$("body").on("change", "#oCleanseMatchExclusionsEntity_Active5", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oCleanseMatchExclusionsChanges("input#oCleanseMatchExclusionsEntity_Active5", "#TagsValue5", "#Tags1", "#TagList5");
    }
});
$('body').on('click', '#btnSubmitExclusion', function () {
    var Active1 = $("#oCleanseMatchExclusionsEntity_Active1").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active1").is(":checked");
    var Tags1 = $("#Tags1").val();
    var Active2 = $("#oCleanseMatchExclusionsEntity_Active2").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active2").is(":checked");
    var Tags2 = $("#Tags2").val();
    var Active3 = $("#oCleanseMatchExclusionsEntity_Active3").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active3").is(":checked");
    var Tags3 = $("#Tags3").val();
    var Active4 = $("#oCleanseMatchExclusionsEntity_Active4").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active4").is(":checked");
    var Tags4 = $("#Tags4").val();
    var Active5 = $("#oCleanseMatchExclusionsEntity_Active5").val() == "undefined" ? "false" : $("#oCleanseMatchExclusionsEntity_Active5").is(":checked");
    var Tags5 = $("#Tags5").val();
    $.post("/DNBIdentityResolution/CleanseMatchExclusions", "ExcludeNonHeadQuarters=" + Active1 + "&Tags1=" + encodeURIComponent(Tags1) + "&ExcludeNonMarketable=" + Active2 + "&Tags2=" + encodeURIComponent(Tags2) + "&ExcludeOutofBusiness=" + Active3 + "&Tags3=" + encodeURIComponent(Tags3) + "&ExcludeUndeliverable=" + Active4 + "&Tags4=" + encodeURIComponent(Tags4) + "&ExcludeUnreachable=" + Active5 + "&Tags5=" + encodeURIComponent(Tags5), function (result) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", dataUpdated);
    });
});

//End Exclusions for Cleanse Match API Calls

//====================================================================================================================

//Start  Additional Auto-Accept Rules 

//load Additional Auto-Accept List
function LoadIndexAutoAcceptance(url) {
    if (url == "") {
        $.pjax({
            url: "/DNB/AutoAcceptance", container: '#divPartialAutoAcceptance',
            timeout: 50000
        }).done(function () {
            InitDNBAutoAcceptanceDataTable();
            AutoAcceptanceEnableDisableRule();
        });
    }
    else {
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/DNB/AutoAcceptance?Parameters=" + url, container: '#divPartialAutoAcceptance',
            timeout: 50000
        }).done(function () {
            InitDNBAutoAcceptanceDataTable();
            AutoAcceptanceEnableDisableRule();
        });
    }
}
//set right click menu on Auto-Accept List
function AutoAcceptanceEnableDisableRule() {
    $(function () {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.contextMenu({
            selector: '.context-menu-one',
            callback: function (key, options) {
            },
            events: {
                show: function (opt) {
                    setTimeout(function () {
                        opt.$menu.find('.context-menu-disabled > span').attr('title');
                    }, 50);
                }
            },
            items: {
                "DisableRule": {
                    name: disableRule, titile: "Disable", callback: function () {
                        //disable rule for from the UI (right - click UI option)
                        var GroupId = $(this).attr("data-GroupId");
                        var Activate = $("#frmExportToExcel #Active").val().toLowerCase() == "true" ? false : true;
                        var QueryString = "GroupId:" + GroupId + "@#$Activate:" + Activate;
                        var url = '/DNBIdentityResolution/DisableRule?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                        bootbox.confirm({
                            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: "Are you sure you want to " + ($("#frmExportToExcel #Active").val().toLowerCase() == "true" ? "disable" : "enable") + " rule ?", callback: function (result) {
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
                                            ShowMessageNotification("success", data.message);
                                            LoadIndexAutoAcceptancePagination();
                                        },
                                        error: function (xhr, ajaxOptions, thrownError) {
                                        }
                                    });
                                }
                            }
                        });
                        return 'context-menu-icon context-menu-icon-quit';
                    }
                }
            }
        });

        $('.context-menu-one').on('click', function (e) {
            return 'context-menu-icon context-menu-icon-quit';
        });

        $('.context-menu-one').on('contextmenu', function (e) {
            if ($("#frmExportToExcel #Active").val().toLowerCase() == "true")
                $('.context-menu-item span').text('Disable rule');
            else
                $('.context-menu-item span').text('Enable rule');
        });
    });
}
//Clear filter of Auto-Accept List
$('body').on('click', '#btnclearFilter', function () {
    LoadIndexAutoAcceptance("");
    $("#ConfidenceCode").val($("#ConfidenceCode option:first").val());
    $("#CountyGroupId").val($("#CountyGroupId option:first").val());
    $(".tagstylebox #Tags").val($(".tagstylebox #Tags option:first").val());
    $("#MatchGrade").val($("#MatchGrade option:first").val());
    $("#MatchGrade").trigger("chosen:updated");
});
function ClosePopupReload() {
    LoadIndexAutoAcceptancePagination();
}
$('body').on('click', '#btnfrmFilter', function () {
    LoadIndexAutoAcceptancePagination();
});
function LoadIndexAutoAcceptancePagination() {
    var pagevalue = 1;
    var SortBy = $("#SortBy").val();
    var SortOrder = $("#SortOrder").val();
    var CountyGroupId = $("#CountyGroupId").val();
    var Tags = $("#frmExportToExcel #Tags").val();
    var Active = $("#frmExportToExcel #AutoAcceptance_Active").prop('checked');
    var QueryString = "pagevalue:" + pagevalue + "@#$sortby:" + SortBy + "@#$sortorder:" + SortOrder + "@#$ConfidenceCode:" + $("#ConfidenceCode").val() + "@#$CountyGroupId:" + CountyGroupId + "@#$Tags:" + Tags + "@#$Active:" + Active;
    LoadIndexAutoAcceptance(ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"));
}
function LoadInsertUpdateAutoAcceptance(UrlValue) {
    var url = "/DNBIdentityResolution/InsertUpdateAutoAcceptance";
    if (UrlValue != undefined && UrlValue != '') {
        url = url + UrlValue;
    }
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'HTML',
        success: function (data) {
            $("#divProgress").hide();
            $("#InsertAutoAcceptanceModalMain").html(data);
            DraggableModalPopup("#InsertUpdateAutoAcceptanceModal");
            OnReadyInsertUpdateAutoAcceptance();
        }
    });
}

$("body").on("click", "#btnAddAAC", function () {
    LoadInsertUpdateAutoAcceptance();
});
$("body").on("click", ".editAutoAcceptance", function () {
    var CriteriaGroupId = $(this).attr("data-id");
    if (CriteriaGroupId != "") {
        LoadInsertUpdateAutoAcceptance("?Parameters=" + CriteriaGroupId);
    }
});
// Delete single Auto Acceptance Record
$("body").on("click", ".deleteAutoAcceptance", function () {
    var currentCls = $(this);
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteAutoAcceptance, callback: function (result) {
            if (result) {
                var CriteriaGroupId = currentCls.attr("data-CriteriaGroupId");
                $.ajax({
                    type: 'GET',
                    url: "/DNBIdentityResolution/DeleteComment?CriteriaGroupId=" + CriteriaGroupId + '&ToCall=DeleteAcceptance',
                    dataType: 'HTML',
                    async: false,
                    success: function (data) {
                        $('#divProgress').hide();
                        $("#DeleteAutoAcceptanceModalMain").html(data);
                        DraggableModalPopup("#DeleteAutoAcceptanceDataModal");
                    }
                });
                return true;
            }
        }
    });
    return false;
});
// Delete multiple Auto Acceptance records
$("body").on('click', '#btnMultipleDeleteAutoAccept', function () {
    var MultipleCriteriaGroupId = $("#MultiDeleteAutoAccept").val();
    MultipleCriteriaGroupId = MultipleCriteriaGroupId.slice(0, -1);
    if (MultipleCriteriaGroupId == "") {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", selectRecordtoDelete);
        return false;
    }
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: multipleDeleteAutoAccept, callback: function (result) {
            if (result) {
                // Changes for Converting magnific popup to modal popup
                $.ajax({
                    type: 'GET',
                    url: '/DNBIdentityResolution/DeleteComment?CriteriaGroupId=' + MultipleCriteriaGroupId + '&ToCall=DeleteAcceptance',
                    dataType: 'HTML',
                    async: false,
                    success: function (data) {
                        $("#divProgress").hide();
                        $("#DeleteAutoAcceptanceModalMain").html(data);
                        DraggableModalPopup("#DeleteAutoAcceptanceDataModal");
                    }
                });
            }
        }
    });
});
//Export Auto Acceptance records
$('body').on('click', '#btnExportToExcel', function () {
    if ($(".AutoAcceptanceTbcenter tbody tr").text().trim() == "No data are available") {
        ShowMessageNotification("success", noRecordsFound);
    }
    else {
        $("#frmExportToExcel #Active").val($("#frmExportToExcel #AutoAcceptance_Active").prop('checked'));
        $("#frmExportToExcel").submit();
    }
});
$('body').on('click', '#btnRunRule', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: runRule, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/DNBIdentityResolution/RunAutoAcceptanceRule/",
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        ShowMessageNotification("success", data.message);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    return false;
});


function OnSuccessUpsertAutoAcceptance(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
    ShowMessageNotification("success", data.message);
    if (data.result) {
        $('#InsertUpdateAutoAcceptanceModal').modal("hide");
        if (data.IsFromReview == false) {
            LoadIndexAutoAcceptance("");
        }
    }
}
//End  Additional Auto-Accept Rules

//====================================================================================================================

//start Auto-Accept Directives

function LoadIndexAutoAcceptDirectives() {
    // // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/DNB/AutoAcceptDirectives", container: '#divPartialAutoAcceptDirectives',
        timeout: 50000
    }).done(function () {
        if ($.UserRole.toLowerCase() == "lob") {
            $("#frmAutoAcceptanceDirective .chzn-select").each(function () {
                $(this).attr("disabled", true);
            });
            $("#frmAutoAcceptanceDirective input[type=checkbox]").each(function () {
                $(this).attr("disabled", true);
            });
            $("#frmAutoAcceptanceDirective .OpenTags").each(function () {
                $(this).remove();
            });
        }
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            IsEnableMatch();
        }
        DirectiveLoadTags();
        if ($.UserRole.toLowerCase() == "global") {
            oAutoAcceptanceDirectives1Changes();
            oAutoAcceptanceDirectives2Changes();
            oAutoAcceptanceDirectives3Changes();
            oAutoAcceptanceDirectives4Changes();
            oAutoAcceptanceDirectives5Changes();
            oAutoAcceptanceDirectives6Changes();
            oAutoAcceptanceDirectives7Changes();
            oAutoAcceptanceDirectives8Changes();
            oAutoAcceptanceDirectives9Changes();
            oAutoAcceptanceDirectives10Changes();
            oAutoAcceptanceDirectives11Changes();
            oAutoAcceptanceDirectives12Changes();
            oAutoAcceptanceDirectives13Changes();
            oAutoAcceptanceDirectives14Changes();
        }
    });
}
function oAutoAcceptanceDirectives1Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active1').is(':checked')) {
        $("#TagsValue6").attr("disabled", false);
        $('input#oAutoAcceptanceDirectivesEntity_Active1').parent().parent().find(".OpenTags").show();
    }
    else {
        $("#TagsValue6").attr("disabled", true);
        $("#TagsValue6").val('');
        $("#Tags6").val('');
        $("#TagList6").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active1').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue6').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives2Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active2').is(':checked')) {
        $("#TagsValue7").attr("disabled", false);
        $('input#oAutoAcceptanceDirectivesEntity_Active2').parent().parent().find(".OpenTags").show();
    }
    else {
        $("#TagsValue7").attr("disabled", true);
        $("#TagsValue7").val('');
        $("#Tags7").val('');
        $("#TagList7").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active2').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue7').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives3Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active3').is(':checked')) {
        $("#TagsValue8").attr("disabled", false);
        $('input#oAutoAcceptanceDirectivesEntity_Active3').parent().parent().find(".OpenTags").show();
    }
    else {
        $("#TagsValue8").attr("disabled", true);
        $("#TagsValue8").val('');
        $("#Tags8").val('');
        $("#TagList8").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active3').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue8').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives4Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active4').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active4').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue9").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active4').parent().parent().find(".OpenTags").show();
        }
    }
    else {
        $("#TagsValue9").attr("disabled", true);
        $("#TagsValue9").val('');
        $("#Tags9").val('');
        $("#TagList9").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active4').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue9').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives5Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active5').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active5').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue10").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active5').parent().parent().find(".OpenTags").show();
        }
    }
    else {
        $("#TagsValue10").attr("disabled", true);
        $("#TagsValue10").val('');
        $("#Tags10").val('');
        $("#TagList10").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active5').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue10').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives6Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active6').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active6').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue11").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active6').parent().parent().find(".OpenTags").show();
        }
    }
    else {
        $("#TagsValue11").attr("disabled", true);
        $("#TagsValue11").val('');
        $("#Tags11").val('');
        $("#TagList11").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active6').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue11').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives7Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active7').is(':checked') && $("#IsLicenseEnableAdvancedMatch").val().toLowerCase() != "false") {
        $(".DegreeOfSeparation").attr("disabled", false);
    }
    else {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            IsEnableMatch();
        }
        else {
            $('.DegreeOfSeparation').attr("disabled", true).prop("checked", false);
            $("#TagsValue13").val("");
            $("#Tags13").val("");
            $("#TagsValue13").attr("disabled", "disabled");
            $("#TagsValue14").val("");
            $("#Tags14").val("");
            $("#TagsValue14").attr("disabled", "disabled");
            $("#TagsValue15").val("");
            $("#Tags15").val("");
            $("#TagsValue15").attr("disabled", "disabled");
            $("#TagsValue16").val("");
            $("#Tags16").val("");
            $("#TagsValue16").attr("disabled", "disabled");
            $("#TagsValue17").val("");
            $("#Tags17").val("");
            $("#TagsValue17").attr("disabled", "disabled");
            $("#TagsValue18").val("");
            $("#Tags18").val("");
            $("#TagsValue18").attr("disabled", "disabled");
            $("#TagsValue19").val("");
            $("#Tags19").val("");
            $("#TagsValue19").attr("disabled", "disabled");
            $(".tag13 .OpenTags").hide();
            $(".tag14 .OpenTags").hide();
            $(".tag15 .OpenTags").hide();
            $(".tag16 .OpenTags").hide();
            $(".tag17 .OpenTags").hide();
            $(".tag18 .OpenTags").hide();
            $(".tag19 .OpenTags").hide();
        }
    }
    $("#TagsValue13").trigger("chosen:updated");
    $("#TagsValue14").trigger("chosen:updated");
    $("#TagsValue15").trigger("chosen:updated");
    $("#TagsValue16").trigger("chosen:updated");
    $("#TagsValue17").trigger("chosen:updated");
    $("#TagsValue18").trigger("chosen:updated");
    $("#TagsValue19").trigger("chosen:updated");
}
function oAutoAcceptanceDirectives8Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active8').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active8').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue13").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active8').parent().parent().find(".OpenTags").show();
        }
    }
    else {
        $("#TagsValue13").attr("disabled", true);
        $("#TagsValue13").val('');
        $("#Tags13").val('');
        $("#TagList13").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active8').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue13').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives9Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active9').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active9').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue14").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active9').parent().parent().find(".OpenTags").show();
        }        
    }
    else {
        $("#TagsValue14").attr("disabled", true);
        $("#TagsValue14").val('');
        $("#Tags14").val('');
        $("#TagList14").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active9').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue14').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives10Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active10').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active10').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue15").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active10').parent().parent().find(".OpenTags").show();
        }
    }
    else {
        $("#TagsValue15").attr("disabled", true);
        $("#TagsValue15").val('');
        $("#Tags15").val('');
        $("#TagList15").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active10').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue15').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives11Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active11').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active11').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue16").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active11').parent().parent().find(".OpenTags").show();
        }
    }
    else {
        $("#TagsValue16").attr("disabled", true);
        $("#TagsValue16").val('');
        $("#Tags16").val('');
        $("#TagList16").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active11').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue16').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives12Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active12').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active12').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue17").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active12').parent().parent().find(".OpenTags").show();
        }
    }
    else {
        $("#TagsValue17").attr("disabled", true);
        $("#TagsValue17").val('');
        $("#Tags17").val('');
        $("#TagList17").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active12').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue17').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives13Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active13').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active13').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue18").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active13').parent().parent().find(".OpenTags").show();
        }
    }
    else {
        $("#TagsValue18").attr("disabled", true);
        $("#TagsValue18").val('');
        $("#Tags18").val('');
        $("#TagList18").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active13').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue18').trigger("chosen:updated");
}
function oAutoAcceptanceDirectives14Changes() {
    if ($('input#oAutoAcceptanceDirectivesEntity_Active14').is(':checked')) {
        if ($("#IsLicenseEnableAdvancedMatch").val().toLowerCase() == "false") {
            $('input#oAutoAcceptanceDirectivesEntity_Active14').parent().parent().find(".OpenTags").hide();
        }
        else {
            $("#TagsValue19").attr("disabled", false);
            $('input#oAutoAcceptanceDirectivesEntity_Active14').parent().parent().find(".OpenTags").show();
        }
    }
    else {
        $("#TagsValue19").attr("disabled", true);
        $("#TagsValue19").val('');
        $("#Tags19").val('');
        $("#TagList19").val('');
        $('input#oAutoAcceptanceDirectivesEntity_Active14').parent().parent().find(".OpenTags").hide();
    }
    $('#TagsValue19').trigger("chosen:updated");
}

$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active1", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives1Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active2", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives2Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active3", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives3Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active4", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives4Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active5", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives5Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active6", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives6Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active7", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives7Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active8", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives8Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active9", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives9Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active10", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives10Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active11", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives11Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active12", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives12Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active13", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives13Changes();
    }
});
$("body").on("change", "#oAutoAcceptanceDirectivesEntity_Active14", function () {
    if ($.UserRole.toLowerCase() == "global") {
        oAutoAcceptanceDirectives14Changes();
    }
});
$("body").on("click", "#btnSubmitDirectives", function () {
    var Active1 = $("#oAutoAcceptanceDirectivesEntity_Active1").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active1").is(":checked");
    var Tags1 = $("#Tags6").val();
    var Active2 = $("#oAutoAcceptanceDirectivesEntity_Active2").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active2").is(":checked");
    var Tags2 = $("#Tags7").val();
    var Active3 = $("#oAutoAcceptanceDirectivesEntity_Active3").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active3").is(":checked");
    var Tags3 = $("#Tags8").val();
    var Active4 = $("#oAutoAcceptanceDirectivesEntity_Active4").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active4").is(":checked");
    var Tags4 = $("#Tags9").val();
    var Active5 = $("#oAutoAcceptanceDirectivesEntity_Active5").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active5").is(":checked");
    var Tags5 = $("#Tags10").val();
    var Active6 = $("#oAutoAcceptanceDirectivesEntity_Active6").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active6").is(":checked");
    var Tags6 = $("#Tags11").val();
    var Active7 = $("#oAutoAcceptanceDirectivesEntity_Active7").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active7").is(":checked");
    //var Tags7 = $("#Tags12").val();
    var Active8 = $("#oAutoAcceptanceDirectivesEntity_Active8").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active8").is(":checked");
    var Tags8 = $("#Tags13").val();
    var Active9 = $("#oAutoAcceptanceDirectivesEntity_Active9").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active9").is(":checked");
    var Tags9 = $("#Tags14").val();
    var Active10 = $("#oAutoAcceptanceDirectivesEntity_Active10").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active10").is(":checked");
    var Tags10 = $("#Tags15").val();
    var Active11 = $("#oAutoAcceptanceDirectivesEntity_Active11").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active11").is(":checked");
    var Tags11 = $("#Tags16").val();
    var Active12 = $("#oAutoAcceptanceDirectivesEntity_Active12").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active12").is(":checked");
    var Tags12 = $("#Tags17").val();
    var Active13 = $("#oAutoAcceptanceDirectivesEntity_Active13").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active13").is(":checked");
    var Tags13 = $("#Tags18").val();
    var Active14 = $("#oAutoAcceptanceDirectivesEntity_Active14").val() == "undefined" ? "false" : $("#oAutoAcceptanceDirectivesEntity_Active14").is(":checked");
    var Tags14 = $("#Tags19").val();
    $.post("/DNBIdentityResolution/AutoAcceptanceDirectives", "&LimitDegreeOfSeparation=" + Active7 + "&AcceptActiveRecordsOnly=" + Active1 + "&Tags1=" + Tags1 + "&PreferHeadquartersRecord=" + Active2 + "&Tags2=" + Tags2 + "&AcceptHeadquartersRecordOnly=" + Active3 + "&Tags3=" + Tags3 + "&AcceptSingleCandidateRecordsOnly=" + Active4 + "&Tags4=" + Tags4 + "&AcceptLinkedRecordOnly=" + Active5 + "&Tags5=" + Tags5 + "&PreferLinkedRecord=" + Active6 + "&Tags6=" + Tags6 + "&RequireDegreeOfSeparation1=" + Active8 + "&Tags8=" + Tags8 + "&RequireDegreeOfSeparation2=" + Active9 + "&Tags9=" + Tags9 + "&RequireDegreeOfSeparation3=" + Active10 + "&Tags10=" + Tags10 + "&RequireDegreeOfSeparation4=" + Active11 + "&Tags11=" + Tags11 + "&RequireDegreeOfSeparation5=" + Active12 + "&Tags12=" + Tags12 + "&RequireDegreeOfSeparation6=" + Active13 + "&Tags13=" + Tags13 + "&RequireDegreeOfSeparation7=" + Active14 + "&Tags14=" + Tags14, function (result) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", result.message);

    });
});
function IsEnableMatch() {
    $('input#oAutoAcceptanceDirectivesEntity_Active4').attr("disabled", true);
    $('input#oAutoAcceptanceDirectivesEntity_Active5').attr("disabled", true);
    $('input#oAutoAcceptanceDirectivesEntity_Active6').attr("disabled", true);
    $('input#oAutoAcceptanceDirectivesEntity_Active8').attr("disabled", true);
    $('input#oAutoAcceptanceDirectivesEntity_Active9').attr("disabled", true);
    $('input#oAutoAcceptanceDirectivesEntity_Active10').attr("disabled", true);
    $('input#oAutoAcceptanceDirectivesEntity_Active11').attr("disabled", true);
    $('input#oAutoAcceptanceDirectivesEntity_Active12').attr("disabled", true);
    $('input#oAutoAcceptanceDirectivesEntity_Active13').attr("disabled", true);
    $('input#oAutoAcceptanceDirectivesEntity_Active14').attr("disabled", true);
    $("#TagsValue9").attr("disabled", "disabled");
    $("#TagsValue10").attr("disabled", "disabled");
    $("#TagsValue11").attr("disabled", "disabled");
    $("#TagsValue12").attr("disabled", "disabled");
    $("#TagsValue13").attr("disabled", "disabled");
    $("#TagsValue14").attr("disabled", "disabled");
    $("#TagsValue15").attr("disabled", "disabled");
    $("#TagsValue16").attr("disabled", "disabled");
    $("#TagsValue17").attr("disabled", "disabled");
    $("#TagsValue18").attr("disabled", "disabled");
    $("#TagsValue19").attr("disabled", "disabled");
}
//End Auto-Accept Directives

//====================================================================================================================
/*Datatable pagination sorting in auto acceptance*/
function InitDNBAutoAcceptanceDataTable() {
    InitDataTable(".AutoAcceptanceTbcenter", [30, 60, 100], false, [1, "asc"]);
}