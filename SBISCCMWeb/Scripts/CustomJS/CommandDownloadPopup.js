$(document).ready(function () {    
    if ($("#ProviderType").val() == "Orb") {
        $("#APILayer").hide();
    }
    EnableDisableFilePrefix();
    setIsAppendExample();
});

$(document).on('change', '#ProviderType', function () {
    $("#MultiSelectOptions").empty();

    if ($("#ProviderType").val() == "DandB") {
        $("#MultiSelectOptions").append(new Option("Match Output", "DownloadMatchOutput"));
        $("#MultiSelectOptions").append(new Option("Enrichment Output", "DownloadEnrichmentOutput"));
        $("#MultiSelectOptions").append(new Option("Transferred Duns", "DownloadTransferDUNS"));
        $("#MultiSelectOptions").append(new Option("Active Data Queue", "DownloadActiveDataQueue"));
        $("#MultiSelectOptions").append(new Option("Monitoring Notification", "DownloadMonitoringUpdates"));
        $("#MultiSelectOptions").append(new Option("Low Confidence Queue", "DownloadLCMQueue"));
        $("#MultiSelectOptions").append(new Option("No Match Queue", "DownloadNoMatchQueue"));
    }
    else if ($("#ProviderType").val() == "Orb") {
        $("#APILayer").hide();
        $("#spnAPILayre").hide();
        $("#MultiSelectOptions").append(new Option("Match Output", "DownloadMatchOutput"));
        $("#MultiSelectOptions").append(new Option("Firmographics", "DownloadEnrichmentOutput"));
        $("#MultiSelectOptions").append(new Option("Company Tree", "DownloadCompanyTree"));
        $("#MultiSelectOptions").append(new Option("Active Data Queue", "DownloadActiveDataQueue"));
    }
    else {
        $("#APILayer").hide();
        $("#spnAPILayre").hide();
    }
    
    $('#MultiSelectOptions').multiselect('rebuild');
    //$("#MultiSelectOptions").multiselect("destroy").multiselect({
    //    includeSelectAllOption: true,
    //    nonSelectedText: 'Select Output Type'
    //});
    //$('#').multiselect('refresh');
});

function sumbitCommandDownloadMapping() {
    var cnt = 0;
    var ConfigurationName = $(".DownloadConfigName").val();
    var FileFormat = $('input[name=DownloadFormat]:checked').val();
    var ProviderType = $("#ProviderType").val();
    if (ProviderType == "") {
        $("#spnProviderType").show();
        cnt++;
    } else {
        $("#spnProviderType").hide();
    }


    if (ConfigurationName == "") {
        $("#spnUConfigNameDownload").show();
        cnt++;
    } else {
        $("#spnUConfigNameDownload").hide();
    }
    if (FileFormat.toLowerCase() == "delimiter") {
        if ($(".FormatdownloadValue").val() == "") {

            $(".FormatdownloadValue").parent().addClass('has-error');
            cnt++;
        }
    }

    if ($("#MultiSelectOptions").val() == "" || $("#MultiSelectOptions").val() == undefined) {
        $("#spnOutputType").show();
        cnt++;
    } else {
        $("#spnOutputType").hide();
    }
    
    var selected = $("#MultiSelectOptions option");
    selected.each(function () {
        var checkedItem = "#" + $(this).val();
        $(checkedItem).val(this.selected);
    });

    if (ProviderType && ProviderType.toLowerCase() == "dandb") {
        if ($("#DownloadMonitoringUpdates").val().toLowerCase() == "true") {
            if ($(".divmonitoringAPILayre #APILayer").val() == "") {
                $("#spnAPILayre").show();
                cnt++;
            }
            else {
                $("#spnAPILayre").hide();
            }
        }
        else {
            $("#spnAPILayre").hide();
        }
    }

    if (cnt > 0) {
        return false;
    }
}

$('body').on('change', '.rbtnDownloadFile', function () {
    var FileFormat = $('input[name=DownloadFormat]:checked').val();
    if (FileFormat.toLowerCase() == "delimiter") {
        $(".divFrmtDownloadValue").show();
        $("#Formatvalue").val("|");
    }
    else {
        $(".divFrmtDownloadValue").hide();
    }
});

function CommandDownloadUpdateSuccess(data) {
    var pagevalue = $(".pagevalueDownloadChange").val();
    var SortBy = $("#SortBy").val();
    var SortOrder = $("#SortOrder").val();
    $("body").append(data);
    var message = $("#CommadDownloadMessage").val();
    $("#CommadDownloadMessage").remove();

    // Changes for Converting magnific popup to modal popup
    $("#DownloadConfigurationModal").modal("hide");   
    LoadCommandDownloadList();
}
function LoadCommandDownloadList() {
    $.pjax({
        url: "/Portal/DownloadConfiguration", container: '#divPartialCommandDownloadList',
        timeout: 50000
    }).done(function () {
        InitPortalDownloadConfigurationDataTable();
        pageSetUp();
        if ($(".popover").length > 0)
            $(".popover").hide();
        //var Id = 0;
        if ($("table#tbCmndDownloadMapping tbody tr").length == 0) {
            $("table#tbCmndDownloadMapping").attr("disabled", true);
            //LoadInsertUpdateCommandDownload(Id);
        }
        else {
            $("table#tbCmndDownloadMapping tbody tr:first").addClass("current");
            //Id = $('table#tbCmndDownloadMapping tbody tr:first').attr("data-CmndDownloadId");
            //LoadInsertUpdateCommandDownload(Id);
        }
    });
}

$("body").on('change', 'input:checkbox[value="DownloadMonitoringUpdates"]', function () {
    if ($(this).prop('checked') == true) {
        $("#APILayer").show();
        $(".divmonitoringAPILayre").show();
    }
    else {
        $(".divmonitoringAPILayre #APILayer").val("");
        $(".divmonitoringAPILayre").hide();
        $("#spnAPILayre").hide();
    }
});

$("body").on('change', 'input:checkbox[value="multiselect-all"]', function () {
    var ProviderType = $("#ProviderType").val();
    if (ProviderType && ProviderType.toLowerCase() == "dandb") {
        if ($(this).prop('checked') == true) {
            // MP-776 Issue while updating download configuration from provider ORB to D&B integration gateway and made changes where required
            $("#APILayer").show();
            $(".divmonitoringAPILayre").show();
        }
        else {
            $(".divmonitoringAPILayre #APILayer").val("");
            $(".divmonitoringAPILayre").hide();
            $("#spnAPILayre").hide();
        }
    }
});

$("body").on('change', '#IsAppendDateTime', function () {
    EnableDisableFilePrefix();
    setIsAppendExample();
});
function EnableDisableFilePrefix() {
    var IsAppendDateTime = $('#IsAppendDateTime').prop('checked');
    if (!IsAppendDateTime) {
        $("#DateTimeFileFormat").attr('disabled', true);
        $("#IsFilePrefix").attr('disabled', true);
    }
    else {
        $("#DateTimeFileFormat").attr('disabled', false);
        $("#IsFilePrefix").attr('disabled', false);
    }
}

$("body").on('change', 'input[name=DownloadFormat]', function () {
    setIsAppendExample();
});

$("body").on('blur', '#FilePrefix', function () {
    setIsAppendExample();
});

$("body").on('change', '#IsFilePrefix', function () {
    setIsAppendExample();
});

$("body").on('change', '#DateTimeFileFormat', function () {
    if ($('#IsAppendDateTime').prop('checked')) {
        setIsAppendExample();
    }
});


function setIsAppendExample() {

    var FileFormat = $('input[name=DownloadFormat]:checked').val();
    var IsAppendDateTime = $('#IsAppendDateTime').prop('checked');
    var DatetimeFormat = $('#DateTimeFileFormat').val();
    var IsFilePrefix = $('#IsFilePrefix').prop('checked');
    var FilePrefix = $('#FilePrefix').val();
    if (FileFormat != undefined) {
        if (FileFormat.toLowerCase() == "excel") {
            fileExtension = ".xlsx";
        }
        else if (FileFormat.toLowerCase() == "csv") {
            fileExtension = ".csv";
        }
        else if (FileFormat.toLowerCase() == "tsv" || FileFormat.toLowerCase() == "delimiter") {
            fileExtension = ".txt";
        }

        var QueryString = "IsAppendDateTime:" + IsAppendDateTime + "@#$DatetimeFormat:" + DatetimeFormat + "@#$IsFilePrefix:" + IsFilePrefix + "@#$FilePrefix:" + FilePrefix + "@#$fileExtension:" + fileExtension;
        var token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            type: "POST",
            url: "/CommandMapping/GetDateTimeFormat?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
            headers: { "__RequestVerificationToken": token },
            dataType: "JSON",
            contentType: "application/json",
            cache: false,
            success: function (data) {
                $("#TextExample").text(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
}

function ConvertEncrypte(value) {
    var newvalue = "";
    if (value != '' && value != undefined) {
        $.ajax({
            type: "POST",
            url: "/Home/GetEncryptedString",
            data: JSON.stringify({
                strValue: value
            }),
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

function resetValueCommadLineDownload() {
    $("#Id").val('');
    $("#ConfigurationName").val('');
    $("#LOBTag").val('');
    $("#Tag").val('');
    $("#Formatvalue").val('|');
    $('#MultiSelectOptions').multiselect("clearSelection");
    $("#MarkAsExported").prop("checked", false);
    $("#IsDefault").prop("checked", false);
    $("#IsAppendDateTime").prop("checked", false);
    $(".rbtnDownloadFile").prop("checked", false);
    $("#IsAppendDateTime").prop("checked", true);
    $(".rbtnDownloadFile[value='Excel']").prop("checked", true);
    $("#DateTimeFileFormat").val($("#DateTimeFileFormat option:first").val());
    $("#FilePrefix").val('');
    $("#IsFilePrefix").prop("checked", false);
}
