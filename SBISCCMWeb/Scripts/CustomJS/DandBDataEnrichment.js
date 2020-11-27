//====================================================================================================================

//Start Data Block Group
function InitDandBDataBlocksDataTable() {
    InitDataTable(".dataBlockGroupsTB", [10, 20, 30], false, [0, "desc"]);
}
function setVersionLevel() {
    var DataBlocksIds = $("#DataBlocksIds").val();
    if (DataBlocksIds != undefined && DataBlocksIds != "") {
        var lstDataBlocksIds = DataBlocksIds.split(",");
        $(".selectVersion").each(function () {
            var datablockid = $(this).attr('data-datablockid');
            if (jQuery.inArray(datablockid, lstDataBlocksIds) > -1) {
                $(this).addClass("active");
            }
        });
    }
}
function LoadDataBlockGroups() {
    // // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/DNB/DataBlocks", container: '#divPartialDataBlockGroup',
        timeout: 50000
    }).done(function (data) {
        var obj = JSON.parse(data);
        $("#divPartialDataBlockGroup").html(obj.indexDataBlockGroup);
        $(".dataBlockGroupsTB tbody tr:first").addClass("current");
        $("#divPartialUpsertDataBlockGroup").html(obj.upsertDataBlockGroup);
        InitDandBDataBlocksDataTable();
        setVersionLevel();
    });
}
function selectFirstTRDataBlocks() {

    $("table.dataBlockGroupsTB tr").each(function () {
        $(this).removeClass('current');
    });
    $("table.dataBlockGroupsTB tbody tr:first").addClass("current");
    var QueryString = $("table.dataBlockGroupsTB tbody tr:first").attr("data-DataBlockGroupId");
    if (QueryString == undefined) {
        QueryString = 0;
    }
    if ($("table.dataBlockGroupsTB tbody tr:first").html().indexOf("No record(s) Found") > 0) {
        $("table.dataBlockGroupsTB tbody tr:first").removeClass("current");
    }
    var url = "/DNBDataEnrichment/UpsertDataBlockGroup?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        cache: false,
        success: function (data) {            
            $("#divPartialUpsertDataBlockGroup").html(data);
            setVersionLevel();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
$('body').on('click', '.deleteDataBlockGroup', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();

    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteDataBlockRecord, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/DNBDataEnrichment/DeleteDataBlockGroups?Parameters=" + ConvertEncrypte(encodeURI(Parameters)).split("+").join("***"),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.message);
                        if (data.result) {
                            LoadDataBlockGroups();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
                return true;
            }
        }
    });
    return false;
});
function UpdateUpsertDataBlockGroup() {
    LoadDataBlockGroups();
}
function EnableDataBlockGroup() {
    $("#DataBlockGroupName").prop("disabled", false);
    $("#CustomerReference").prop("disabled", false);
    $("#TradeUp").prop("disabled", false);
    $(".selectVersion").prop("disabled", false);
    $("#btnDataBlockGroup").show();
    $(".LevelBlockbox").removeClass("disabledLevelBox")
}
function SetBlankValueDataBlockGroup() {
    $("#DataBlockGroupName").val("");
    $("#CustomerReference").val("");
    $("#TradeUp").val("");
    $(".selectVersion").removeClass("active");
    $("#DataBlocks").val("");
    $("#DataBlocksIds").val("");
    $("#DataBlockGroupId").val("0");
    $("#spnAPIUrl").text("");
}
$(document).on('click', '#addDataBlockGroup', function () {
    EnableDataBlockGroup();
    SetBlankValueDataBlockGroup();
});
$(document).on('click', '#editDataBlockGroup', function () {
    EnableDataBlockGroup();
});
function ValidateDataBlockGroup() {
    var DataBlockGroupName = $("#DataBlockGroupName").val();
    var lstDataBlockId = [];
    var lstProductCode = [];
    var cnt = 0;
    if (DataBlockGroupName != undefined && DataBlockGroupName.trim() != "") {
        $("#spnDataBlockGroupName").hide();
    }
    else {
        $("#spnDataBlockGroupName").show();
        cnt++;
    }
    $(".selectVersion.active").each(function () {
        lstDataBlockId.push($(this).attr("data-DataBlockId"))
        lstProductCode.push($(this).attr("data-ProductCode"))
    });
    if (lstDataBlockId.length > 0 && lstProductCode.length > 0) {
        $("#DataBlocksIds").val(lstDataBlockId.toString());
        $("#DataBlocks").val(lstProductCode.toString());
        $("#spnDataBlocks").hide();
    }
    else {
        $("#spnDataBlocks").show();
        cnt++;
    }

    if (cnt > 0) {
        return false;
    }
    else {
        return true;
    }
}
$(document).on('click', '.selectVersion', function () {
    var selector = $(this);
    if (selector.hasClass("active")) {
        selector.removeClass("active");
    }
    else {
        selector.siblings().removeClass("active");
        selector.addClass("active");
    }
    SetAPIUrl();
});
$(document).on('change', '#CustomerReference', function () { SetAPIUrl(); });
$(document).on('change', '#TradeUp', function () { SetAPIUrl(); });
function SetAPIUrl() {
    var lstProductCode = [];
    var CustomerReference = $("#CustomerReference").val();
    var TradeUp = $("#TradeUp").val();

    $(".selectVersion.active").each(function () {
        lstProductCode.push($(this).attr("data-ProductCode"));
    });

    var apiUrl = "https://plus.dnb.com/v1/data/duns/{dunsNumber}?blockIDs=" + lstProductCode.toString();
    if (CustomerReference != undefined && CustomerReference != "") {
        apiUrl += "&CustomerReference=" + CustomerReference;
    }
    if (TradeUp != undefined && TradeUp != "") {
        apiUrl += "&TradeUp=" + TradeUp;
    }
    $("#spnAPIUrl").text(apiUrl);

}
function loadUpsertDataBlockGroup(DataBlockGroupId) {
    $.ajax({
        type: 'GET',
        url: "/DNBDataEnrichment/UpsertDataBlockGroup?Parameters=" + ConvertEncrypte(encodeURI(DataBlockGroupId)).split("+").join("***"),
        dataType: 'html',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#divPartialUpsertDataBlockGroup").html(data);
            setVersionLevel();
        }
    });
}
$("body").on('click', 'table.dataBlockGroupsTB tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("table.dataBlockGroupsTB tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var DataBlockGroupId = $(this).attr("data-DataBlockGroupId");
        loadUpsertDataBlockGroup(DataBlockGroupId);
    }
});

//End Data Block Group

//====================================================================================================================

//Start Data Enrichment

function LoadDataEnrichment() {
    // // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/DNB/DataEnrichment", container: '#divPartialDataEnrichmentSettings',
        timeout: 50000
    }).done(function () {
        InitDandBDataEnrichmentDataTable();
        $("table.APIEnrichmentTB tbody tr:first").addClass("current");
        var id = $("table.APIEnrichmentTB tbody tr:first").attr("data-apigroupid");
        if (id != undefined && id != null && id != "") {
        }
        else {
            id = null;
        }
        $.ajax({
            type: 'GET',
            url: "/DNBDataEnrichment/InsertUpdateDataEnrichment?Parameters=" + id,
            dataType: 'HTML',
            contentType: 'application/html',
            cache: false,
            success: function (data) {
                $("#divPartialAddUdateDataEnrichmentSettings").html(data);
                if ($.UserRole.toLowerCase() == 'lob') {
                    $("#frm_DnBEnrichment .chzn-select").attr("data-placeholder", "Add Tags (Required)");
                    $("#frm_DnBEnrichment .chzn-select").trigger("chosen:updated");
                }
                if ($("table.APIEnrichmentTB tbody tr").length == 0) {
                    $("#editAPIEnrichment").attr("disabled", true);
                }
                LoadTagsDnBApiGroupTag();
            }
        });
    });
}
function InitDandBDataEnrichmentDataTable() {
    InitDataTable(".APIEnrichmentTB", [10, 20, 30], false, [0, "desc"]);
}
$('body').on('click', '.deleteEnrichment', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: multipleDeleteAutoAccept, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/DNBDataEnrichment/DeleteAPIGroup?Parameters=" + Parameters,
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        ShowMessageNotification("success", data.message);
                        if (data.result) {
                            onChangeDataEnrichment();
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
$('body').on('click', '#editAPIEnrichment', function () {
    enableAPIEnrichmentFields();
    $("#btnDnBApiGrp").val(update);
});
$('body').on('click', '#AddAPIEnrichment', function () {
    $("#spnCredName").hide();
    $("table.APIEnrichmentTB tbody tr").each(function () {
        $(this).removeClass('current');
    });
    enableAPIEnrichmentFields();
    $("#APIGroupName").val("");
    $("#CountryGroupId").val("");
    $("#frm_DnBEnrichment #CredentialId").val("0");

    $(".DnBApiGroupTag").val('0');
    $(".DnBApiGroupTag").trigger("chosen:updated");

    $("#APIGroupId").val("0");
    $("#CountryGroupId").val("-1");

    var selectedOpts = $('#RemoveAPIIds option');
    $('#APIIds').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    $("#frm_DnBEnrichment #Tags").val("");
    $("#btnDnBApiGrp").val(add);
    $("#frm_DnBEnrichment #Tags").val("");
    $("#tmpName").val("");
    $("#APIType").val("DirectPlus");
    $("#frm_DnBEnrichment #CredentialId").val($("#frm_DnBEnrichment #CredentialId option:first").val());
    fillAPIType();

});
function fillAPIType() {
    var APItype = $("#APIType").val();
    var CredId = 0;
    var QueryString = "APIType:" + APItype + "@#$CredId:" + CredId;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/DNBDataEnrichment/GetDnBAPIList?Parameters=" + Parameters,
        dataType: "JSON",
        headers: { "__RequestVerificationToken": token },
        contentType: "application/json",
        success: function (data) {
            $("#APIIds option").remove();
            $("#frm_DnBEnrichment #CredentialId option").remove();
            $("#RemoveAPIIds option").remove();
            if (data.data.length > 0) {
                for (var cnt = 0; cnt < data.data.length; cnt++) {
                    $("#APIIds").append(new Option(data.data[cnt].APIName, data.data[cnt].DnBAPIId));
                }
            }
            if (data.data2.length > 0) {
                for (cnt = 0; cnt < data.data2.length; cnt++) {
                    $("#frm_DnBEnrichment #CredentialId").append(new Option(data.data2[cnt].CredentialName, data.data2[cnt].CredentialId));
                }
            }
        }, error: function (xhr, ajaxOptions, thrownError) { }

    });
}
$(document).on('change', '#frm_DnBEnrichment #CredentialId', function () {
    var APItype = $("#frm_DnBEnrichment #APIType").val();
    var CredId = $(this).val();
    var QueryString = "APIType:" + APItype + "@#$CredId:" + CredId;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/DNBDataEnrichment/GetDnBAPIList?Parameters=" + Parameters,
        dataType: "JSON",
        headers: { "__RequestVerificationToken": token },
        contentType: "application/json",
        success: function (data) {
            $("#frm_DnBEnrichment #APIIds option").remove();
            $("#frm_DnBEnrichment #RemoveAPIIds option").remove();
            if (data.data.length > 0) {
                for (var cnt = 0; cnt < data.data.length; cnt++) {
                    $("#frm_DnBEnrichment #APIIds").append(new Option(data.data[cnt].APIName, data.data[cnt].DnBAPIId));
                }
            }
        }, error: function (xhr, ajaxOptions, thrownError) { }

    });
});

function UpdateDnbGroupAPI(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
    ShowMessageNotification("success", data.message);
    if (data.result) {
        $("#APIGroupName").prop("disabled", true);
        $(".DnBApiGroupTag").prop("disabled", true);
        $(".DnBApiGroupTag").trigger("chosen:updated");
        $("#APIIds").prop("disabled", true);
        $("#RemoveAPIIds").prop("disabled", true);
        $("#btnDnBApiGrpRight").prop("disabled", true);
        $("#btnDnBApiGrpAllRight").prop("disabled", true);
        $("#btnDnBApiGrpLeft").prop("disabled", true);
        $("#btnDnBApiGrpAllLeft").prop("disabled", true);
        $("#btnDnBApiGrp").hide();
        $("#frm_DnBEnrichment .OpenTags").hide();
        onChangeDataEnrichment();
    }
}

function LoadTagsDnBApiGroupTag() {
    if ($("#frm_DnBEnrichment #TagList").length > 0) {
        var TagList = $("#frm_DnBEnrichment #TagList").val().split(',');
        if (TagList != null || TagList != "") {
            $(".DnBApiGroupTag option").each(function () {
                for (var i = 0; i < TagList.length; i++) {
                    if ($(this).val() == TagList[i]) {
                        $(this).attr("selected", "selected");
                    }
                }
            });
            $("#frm_DnBEnrichment #Tags").val(TagList);
        }
    }
    if ($(".DnBApiGroupTag").length > 0) {
        $(".DnBApiGroupTag").chosen().change(function (event) {
            if (event.target == this) {
                $("#frm_DnBEnrichment #Tags").val($(this).val());
            }
        });
    }
    $(".DnBApiGroupTag").trigger("chosen:updated");

}
$(document).on('change', '#APIType', function () {
    fillAPIType();
});
$('body').on('click', '#btnDnBApiGrpRight', function (e) {
    var selectedOpts = $('#APIIds option:selected');

    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", nothingToMove);

        e.preventDefault();
    }
    $('#RemoveAPIIds').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});
$('body').on('click', '#btnDnBApiGrpAllRight', function (e) {
    var selectedOpts = $('#APIIds option');
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", nothingToMove);
        e.preventDefault();
    }
    $('#RemoveAPIIds').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});
$('body').on('click', '#btnDnBApiGrpLeft', function (e) {
    var selectedOpts = $('#RemoveAPIIds option:selected');
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", nothingToMove);
        e.preventDefault();
    }
    $('#APIIds').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});
$('body').on('click', '#btnDnBApiGrpAllLeft', function (e) {
    var selectedOpts = $('#RemoveAPIIds option');
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", nothingToMove);
        e.preventDefault();
    }
    $('#APIIds').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});
$('body').on('click', '.btnDnBApiGrp', function () {
    var OptionCount = $('select#RemoveAPIIds option').length;
    var GroupName = $("#APIGroupName").val().trim();
    var CredentialId = $("#frm_DnBEnrichment #CredentialId").val();
    var count = 0;
    var Tags = $("#frm_DnBEnrichment #Tags").val();
    if (GroupName == '') {
        $("#spnGroupName").show();
        count++;
    }
    else {
        $("#spnGroupName").hide();
    }
    if (CredentialId == 0) {
        $("#frm_DnBEnrichment #spnCredName").show();
        count++;
    }
    else {
        $("#frm_DnBEnrichment #spnCredName").hide();
    }
    if (OptionCount == 0) {
        $("#spnOptionValue").show();
        count++;
    }
    else {
        $("#spnOptionValue").hide();
    }
    if ($.UserRole.toLowerCase() == 'lob') {
        if (Tags == '' || Tags == 0) {
            count++;
            $("#frm_DnBEnrichment #spnTags").show();
        }
        else {
            $("#frm_DnBEnrichment #spnTags").hide();
        }
    }
    if (count > 0) {
        return false;
    }
    var value = "";
    $('#RemoveAPIIds option').each(function () {
        value = value + $(this).val() + ",";
    });
    $("#DnbAPIIds").val(value);
});
$('body').on('click', 'table.APIEnrichmentTB tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("table.APIEnrichmentTB tbody tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var GroupId = $(this).attr("data-APIGroupId");
        $.ajax({
            type: 'GET',
            cache: false,
            url: "/DNBDataEnrichment/InsertUpdateDataEnrichment?Parameters=" + GroupId,
            dataType: 'HTML',
            contentType: 'application/html',
            async: false,
            success: function (data) {
                $("#divPartialAddUdateDataEnrichmentSettings").html(data);
                LoadTagsDnBApiGroupTag();
            }
        });
    }
});
function enableAPIEnrichmentFields() {
    $("#frm_DnBEnrichment #CredentialId").prop("disabled", false);
    $("#APIGroupName").prop("disabled", false);
    $("#CountryGroupId").prop("disabled", false);
    $(".DnBApiGroupTag").prop("disabled", false);
    $(".DnBApiGroupTag").trigger("chosen:updated");
    $("#APIType").prop("disabled", false);
    $("#frm_DnBEnrichment #CredentialId").prop("disabled", false);
    $("#APIIds").prop("disabled", false);
    $("#btnDnBApiGrpRight").prop("disabled", false);
    $("#btnDnBApiGrpAllRight").prop("disabled", false);
    $("#btnDnBApiGrpLeft").prop("disabled", false);
    $("#btnDnBApiGrpAllLeft").prop("disabled", false);
    $("#RemoveAPIIds").prop("disabled", false);
    $("#btnDnBApiGrp").show();
    $("#frm_DnBEnrichment .OpenTags").show();
}
//End Data Enrichment

//====================================================================================================================

//start ThirdParty Enrichment
$('body').on('click', '#IdRTabThirdPartyEnrichment', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabEnrichment").parent("li").removeClass("active");
        GetThirdPartyEnrichments();
    }
});
$(document).on('change', '#ThirdPartyEnrichProvider', function () {
    var currVal = $(this).val().split("::");
    if (currVal.length > 0) {
        GetThirdPartyAPICredentialsByProvider(currVal[0]);
        if (currVal.length > 1)
            GetFieldsForThirdPartyEnrichment(currVal[1]);
        else {
            $('#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue').find('option').remove().end();
            $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").multiselect('refresh');
        }
    }
    else {
        $('.enrichCredentialName #EnrichCredential').find('option').remove().end().append('<option value="">--Select--</option>').val('');
        $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").empty();
        $('#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue').multiselect('rebuild');
    }
});
$(document).on('click', '#AddThirdPartyEnrichment', function () {
    $("#articleAddupdateThirdPartyEnrichment #editThirdPartyEnrichment").prop("disabled", true);
    $("table.ThirdPartyEnrichmentTB tbody tr").each(function () {
        $(this).removeClass('current');
    });
    $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichProvider").val("");
    $("#frm_ThirdPartyEnrichment #EnrichCredential").val("");
    $("#frm_ThirdPartyEnrichment #EnrichmentId").val("0");
    $("#frm_ThirdPartyEnrichment #EnrichmentDescription").val("");
    $("#frm_ThirdPartyEnrichment #CountryGroupId").val("-1");
    $(".ThirdPartyEnrichmentTag").val('0');
    $(".ThirdPartyEnrichmentTag").trigger("chosen:updated");
    $("#frm_ThirdPartyEnrichment #Tags").val("");
    $("#frm_ThirdPartyEnrichment #EnrichmentFields").val("");
    $('#ThirdPartyEnrichmentFieldsValue').multiselect("enable");
    $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").empty();
    $('#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue').multiselect('rebuild');
    //$("#frm_ThirdPartyEnrichment #PeriodicRefreshIntervalDays").val("");
    $('#frm_ThirdPartyEnrichment #EnablePeriodicRefresh').removeAttr('checked');
    enableThirdPartyEnrichmentFields();
    $("#frm_ThirdPartyEnrichment #btnThirdPartyEnrichment").val(add);
});
$(document).on('click', '#editThirdPartyEnrichment', function () {
    enableThirdPartyEnrichmentFields();
    $('#ThirdPartyEnrichmentFieldsValue').multiselect("enable");
    $("#frm_ThirdPartyEnrichment #btnThirdPartyEnrichment").val(update);
});
$(document).on('click', 'table.ThirdPartyEnrichmentTB tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("#articleAddupdateThirdPartyEnrichment #editThirdPartyEnrichment").prop("disabled", false);
        $("table.ThirdPartyEnrichmentTB tbody tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var enrichId = $(this).attr("data-EnrichId");
        GetThirdPartyEnrichmentsByEnrichId(enrichId);
    }
});
$(document).on('click', '#frm_ThirdPartyEnrichment #btnThirdPartyEnrichment', function () {
    var cnt = 0;
    var ThirdPartyEnrichProvider = $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichProvider").val();
    var EnrichCredential = $("#frm_ThirdPartyEnrichment #EnrichCredential").val();
    var EnrichmentDescription = $("#frm_ThirdPartyEnrichment #EnrichmentDescription").val();
    var ThirdPartyEnrichmentFieldsValue = $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").val();
    if (!ThirdPartyEnrichProvider) {
        cnt++;
        $("#spnErrroThirdPartyEnrichProvider").show();
    } else {
        $("#spnErrroThirdPartyEnrichProvider").hide();
    }
    if (!EnrichCredential) {
        cnt++;
        $("#spnErrroThirdPartyEnrichCred").show();
    } else {
        $("#spnErrroThirdPartyEnrichCred").hide();
    }
    if (!EnrichmentDescription) {
        cnt++;
        $("#spnErrEnrichmentDescription").show();
    } else {
        $("#spnErrEnrichmentDescription").hide();
    }
    if (ThirdPartyEnrichmentFieldsValue.length < 1) {
        cnt++;
        $("#spnErrThirdPartyEnrichmentFieldsValue").show();
    } else {
        $("#spnErrThirdPartyEnrichmentFieldsValue").hide();
    }
    if (cnt > 0) {
        return false;
    }
    $("#frm_ThirdPartyEnrichment #ThirdPartyProvider").val(ThirdPartyEnrichProvider.split("::")[0]);
    $("#frm_ThirdPartyEnrichment #CredentialId").val(EnrichCredential);
    $("#frm_ThirdPartyEnrichment #EnrichmentFields").val(ThirdPartyEnrichmentFieldsValue.toString());
    
});
$(document).on('click', '.ThirdPartyEnrichmentTB .deleteThirdPartyEnrichment', function () {
    var enrichId = $(this).attr('id');
    if (enrichId != undefined && enrichId != null && enrichId != "") {
    }
    else {
        enrichId = null;
    }
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteDataBlockRecord, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/DNBDataEnrichment/DeleteThirdPartyEnrichments?Parameters=" + enrichId,
                    dataType: "JSON",
                    contentType: "application/json",
                    async: false,
                    cache: false,
                    success: function (data) {
                        ShowMessageNotification("success", data.message, false);
                        if (data.result) {
                            GetThirdPartyEnrichments();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
                return true;
            }
        }
    });
});
function GetThirdPartyAPICredentialsByProvider(provider) {
    $.ajax({
        type: 'GET',
        url: "/DNBDataEnrichment/GetThirdPartyAPICredentials?Parameters=" + ConvertEncrypte(encodeURI(provider)).split("+").join("***"),
        dataType: 'json',
        contentType: 'application/json',
        cache: false,
        async: false,
        success: function (data) {
            $('.enrichCredentialName #EnrichCredential').find('option').remove().end().append('<option value="">--Select--</option>').val('');
            for (var j = 0; j < data.length; j++) {
                $(".enrichCredentialName #EnrichCredential").append($("<option></option>").attr("value", data[j].CredentialId).text(data[j].CredentialName));
            }
        }
    });
}

function GetFieldsForThirdPartyEnrichment(provider) {
    $.ajax({
        type: 'GET',
        url: "/DNBDataEnrichment/GetFieldsForThirdPartyEnrichment?Parameters=" + ConvertEncrypte(encodeURI(provider)).split("+").join("***"),
        dataType: 'json',
        contentType: 'application/json',
        cache: false,
        async: false,
        success: function (data) {
            $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").empty();
            for (var j = 0; j < data.length; j++) {
                $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").append($("<option></option>").attr("value", data[j].Value).text(data[j].Text));
            }
            
            $('#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue').multiselect('rebuild');
        }
    });
}


function GetThirdPartyEnrichments() {
    // // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/DNB/ThirdPartyEnrichment", container: '#divPartialDataThirdPartyEnrichment',
        timeout: 50000
    }).done(function () {
        if ($("table.ThirdPartyEnrichmentTB").length > 0) {
            InitDandBThirdPartyEnrichmentDataTable();
            $("table.ThirdPartyEnrichmentTB tbody tr:first").addClass('current');
            var enrichId = $("table.ThirdPartyEnrichmentTB tbody tr:first").attr("data-EnrichId");
            GetThirdPartyEnrichmentsByEnrichId(enrichId);
        }
        else {
            $("#divPartialDataThirdPartyEnrichmentAddUpdate").html('');
        }
    });
}
function GetThirdPartyEnrichmentsByEnrichId(enrichId) {
    if (enrichId != undefined && enrichId != null && enrichId != "") {
    }
    else {
        enrichId = null;
    }

    $.ajax({
        type: 'GET',
        cache: false,
        url: "/DNBDataEnrichment/UpsertThirdPartyEnrichments?Parameters=" + enrichId,
        dataType: 'HTML',
        contentType: 'application/html',
        success: function (data) {
            $("#divPartialDataThirdPartyEnrichmentAddUpdate").html(data);
        }
    });
}
function enableThirdPartyEnrichmentFields() {
    $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichProvider").prop("disabled", false);
    $("#frm_ThirdPartyEnrichment #EnrichCredential").prop("disabled", false);
    $("#frm_ThirdPartyEnrichment #EnrichmentDescription").prop("disabled", false);
    $("#frm_ThirdPartyEnrichment #CountryGroupId").prop("disabled", false);
    $("#frm_ThirdPartyEnrichment .ThirdPartyEnrichmentTag").prop("disabled", false);
    $("#frm_ThirdPartyEnrichment .ThirdPartyEnrichmentTag").trigger("chosen:updated");
    $("#frm_ThirdPartyEnrichment #EnablePeriodicRefresh").prop("disabled", false);
    $("#frm_ThirdPartyEnrichment #PeriodicRefreshIntervalDays").prop("disabled", false);
    $("#frm_ThirdPartyEnrichment #btnThirdPartyEnrichment").show();
    $("#frm_DnBEnrichment .OpenTags").show();
}
function InitDandBThirdPartyEnrichmentDataTable() {
    InitDataTable(".ThirdPartyEnrichmentTB", [10, 20, 30], false, [0, "desc"]);
}
function LoadTagsThirdPartyEnrichmentTags() {
    if ($("#frm_ThirdPartyEnrichment #TagList").length > 0) {
        var TagList = $("#frm_ThirdPartyEnrichment #TagList").val().split(',');
        if (TagList != null || TagList != "") {
            $(".ThirdPartyEnrichmentTag option").each(function () {
                for (var i = 0; i < TagList.length; i++) {
                    if ($(this).val() == TagList[i]) {
                        $(this).attr("selected", "selected");
                    }
                }
            });
            $("#frm_ThirdPartyEnrichment #Tags").val(TagList);
        }
    }
    if ($(".ThirdPartyEnrichmentTag").length > 0) {
        $(".ThirdPartyEnrichmentTag").chosen().change(function (event) {
            if (event.target == this) {
                $("#frm_ThirdPartyEnrichment #Tags").val($(this).val());
            }
        });
    }
    $(".ThirdPartyEnrichmentTag").trigger("chosen:updated");
}
function LoadThirdPartyEnrichmentFieldsValue() {
    $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").multiselect({
        includeSelectAllOption: true,
        nonSelectedText: 'Select Fields',
        numberDisplayed: 3,
        maxHeight: 200,
        enableCaseInsensitiveFiltering: true,
        onChange: function (option, checked, select) {
            var currVals = $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").val();
            if (currVals.length > 0)
                $("#frm_ThirdPartyEnrichment #EnrichmentFields").val(currVals.toString());
        }
    });

    var arrfields = $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsList").val().split(',');
    if (arrfields.length > 0) {
        $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").val(arrfields);
        $("#frm_ThirdPartyEnrichment #ThirdPartyEnrichmentFieldsValue").multiselect('refresh');
    }
}
function SuccessAddUpdateThirdPartyEnrichment(data) {
    ShowMessageNotification("success", data.message);
    if (data.result) {
        GetThirdPartyEnrichments();
    }
}

//End ThirdParty Enrichment
//====================================================================================================================

function onChangeDataEnrichment() {
    $.ajax({
        type: 'GET',
        cache: false,
        url: '/DNB/DataEnrichment/',
        dataType: 'HTML',
        contentType: 'application/html',
        success: function (data) {
            $("#divPartialDataEnrichmentSettings").html(data);
            InitDandBDataEnrichmentDataTable();
            $("table.APIEnrichmentTB tbody tr:first").addClass("current");
            var id = $("table.APIEnrichmentTB tbody tr:first").attr("data-apigroupid");
            $.ajax({
                type: 'GET',
                url: "/DNBDataEnrichment/InsertUpdateDataEnrichment?Parameters=" + id,
                dataType: 'HTML',
                contentType: 'application/html',
                cache: false,
                success: function (data) {
                    $("#divPartialAddUdateDataEnrichmentSettings").html(data);
                    InitDandBDataEnrichmentDataTable();
                    if ($.UserRole.toLowerCase() == 'lob') {
                        $("#frm_DnBEnrichment .chzn-select").attr("data-placeholder", "Add Tags (Required)");
                        $("#frm_DnBEnrichment .chzn-select").trigger("chosen:updated");
                    }
                    if ($("table.APIEnrichmentTB tbody tr").length == 0) {
                        $("#editAPIEnrichment").attr("disabled", true);
                    }
                    LoadTagsDnBApiGroupTag();
                }
            });
        }
    });
}