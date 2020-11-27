$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});
$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
$(document).on("click", "#clsOrbSingleEntryForm", function () {
    var importMode = $("input[name='rBtnOrbImportDataOption']:checked").val();
    var SingleEntryFrmUrl = "";
    var popupcls = "";
    if (importMode == "Match Refresh") {
        SingleEntryFrmUrl = '/OIData/OISingleEntryMatchRefresh';
        popupcls = 'popupOrbSingleEntryFormMatchRefresh';
    }
    else if (importMode == "Data Import") {
        SingleEntryFrmUrl = '/OIData/OISingleEntryForm';
        popupcls = 'popupOrbSingleEntryForm';
    }
    $.magnificPopup.open({
        preloader: true,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: SingleEntryFrmUrl,
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: popupcls,
    });
});

//// Open File selection popup according to the file type.
$("body").on('click', '.clsFileType', function () {
    var importMode = $("input[name='rBtnOrbImportDataOption']:checked").val();
    var OIMatchRefresh = "";
    var popupcls = "";
    var fileType = $(this).data('filetype');
    var QueryString = "fileType:" + fileType + "@#$importMode:" + importMode;
    if (importMode == "Match Refresh") {
        OIMatchRefresh = '/OIData/OrbImportPanelMatchRefresh?Parameters=' + fileType,
            popupcls = 'popupOIImportPanelMatchRefresh';
    }
    else if (importMode == "Data Import") {
        OIMatchRefresh = '/OIData/OrbImportPanel?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
            popupcls = 'popupOrbImportPanel';
    }
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        height: '200px',
        items: {
            src: OIMatchRefresh,
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupImport'
    });
});

$("#file").change(function () {
    var fileExtension = ['xls', 'xlsx', 'csv', 'txt'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
            message: "Please select file first.",
        });
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
            message: "Only formats allowed are : " + fileExtension.join(', '),
        });
        $('#file').val("");
    }
});
//import data
$("body").on('click', '#btnSubmitImportPanel', function () {
    var formData = new FormData();
    if ($('#file')[0].files[0] != undefined) {

        var delimiter = '';
        var fileType = $("#fileType").val();
        var ImportMode = $("#ImportMode").val();
        var istext = $("#isText").val().toLowerCase();
        if (istext) {
            $('.errorMessage').text('');
            delimiter = $('#txtDelimiter').val();
            if (delimiter == '') {
                $('.errorMessage').text(' Please enter delimiter');
                return;
            }
        }
        formData.append('file', $('#file')[0].files[0]);
        formData.append('header', $('#WithHeader').prop('checked'));
        formData.append('isTSV', $("#isTSV").val().toLowerCase());
        formData.append('delimiter', delimiter);

        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: "POST",
            url: '/OIData/OrbImportPanel',
            data: formData,
            headers: { "__RequestVerificationToken": token },
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                if (response == "success") {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.CloseImportPanel(fileType, ImportMode);
                }
                else if (response == "Only formats allowed are :xls,xlsx,csv,tsv,txt") {
                    var formats = $("#allowedFormats").val();
                    var fileExtension = formats.split(','); //['xls', 'xlsx', 'csv'];
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", "Only formats allowed are : " + fileExtension, false, false);
                    $('#file').val("");
                    return false;
                }
                else if (response == "Unable to import blank file") {
                    parent.ShowMessageNotification("success", response, false);
                    return false;
                }
                else if (response == "File has no rows.") {
                    parent.ShowMessageNotification("success", response, false);
                    return false;
                }
                else {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", response, false, false);
                }
            },
            error: function (xhr, status, error) {
            }
        });
    } else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        parent.ShowMessageNotification("success", "Please select file first.", false, false);
    }
    return false;
});

$("body").on('click', '#btnSubmitImportPanelMatchRefresh', function () {
    var formData = new FormData();
    if ($('#file')[0].files[0] != undefined) {

        var delimiter = '';
        var fileType = $("#fileType").val();
        var istext = $("#isText").val().toLowerCase();
        if (istext) {
            $('.errorMessage').text('');
            delimiter = $('#txtDelimiter').val();
            if (delimiter == '') {
                $('.errorMessage').text(' Please enter delimiter');
                return;
            }
        }
        formData.append('file', $('#file')[0].files[0]);
        formData.append('header', $('#WithHeader').prop('checked'));
        formData.append('isTSV', $("#isTSV").val().toLowerCase());
        formData.append('delimiter', delimiter);

        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: "POST",
            url: '/OIData/OrbImportPanelMatchRefresh',
            data: formData,
            headers: { "__RequestVerificationToken": token },
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                if (response == "success") {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.CloseImportPanelMatchRefresh(fileType);
                }
                else if (response == "Only formats allowed are :xls,xlsx,csv,tsv,txt") {
                    var formats = $("#allowedFormats").val();
                    var fileExtension = formats.split(','); //['xls', 'xlsx', 'csv'];
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", "Only formats allowed are : " + fileExtension, false, false);
                    $('#file').val("");
                    return false;
                }
                else if (response == "File has no rows.") {
                    parent.ShowMessageNotification("success", response, false);
                    return false;
                }
                else {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", response, false, false);
                }
            },
            error: function (xhr, status, error) {
            }
        });
    } else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        parent.ShowMessageNotification("success", "Please select file first.", false, false);
    }
    return false;
});

function CloseImportPanel(fileType, ImportMode) {
    var QueryString = "fileType:" + fileType + "@#$ImportMode:" + ImportMode;
    $.magnificPopup.close();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/OIData/OrbDataMatch?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***")
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupOrbColmnMapping'
    });
}
function CloseImportPanelMatchRefresh(fileType) {
    var QueryString = fileType;
    $.magnificPopup.close();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/OIData/OrbImportPanelDataMatch?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***")
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupOrbColmnMappingMatchRefresh'
    });
}

$("body").on('click', '#btnDataMappingInsertData', function () {
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var count = 0;
    var totalCount = 0;
    var duplicateselectCount = 0;
    var Tags = $("#Tags").val();
    var fileType = $("#fileType").val();
    var url = "";
    url = "/OIData/OrbDataMatch";
    if (Tags == undefined || Tags == "0") {
        Tags = "";
    }

    $(".SelectBox").each(function () {
        cat.push($(this).val());
        totalCount = totalCount + 1;
        if ($(this).hasClass("chzn-select")) {
            OrgColumnName.push("Tags");
        }

        else {
            OrgColumnName.push($(this).find(":selected").text());
        }
        $(this).parent().removeClass('has-error');
    });
    if (cat.length > 0) {
        for (var i = 0; i < cat.length; i++) {
            if (i == 0 || i == 1 || i == 7) {
                if ($("#DataColumn-" + i).val() == 0) {
                    $("#DataColumn-" + i).parent().addClass('has-error');
                }
            }
            for (var j = 0; j <= cat.length; j++) {
                if (i != j && cat[i] == cat[j]) {
                    count = count + 1;
                    var currentvalue = $("#DataColumn-" + i).val();
                    var nextValue = $("#DataColumn-" + j).val();
                    if (parseInt(currentvalue) > 0 || parseInt(nextValue) > 0) {
                        duplicateselectCount = duplicateselectCount + 1;
                        if (parseInt(currentvalue) > 0) {
                            $("#DataColumn-" + i).parent().addClass('has-error');
                        }
                        if (parseInt(nextValue) > 0) {
                            $("#DataColumn-" + j).parent().addClass('has-error');
                        }
                    }
                }
            }
        }
    }
    $(".spnExcelColumn").each(function () {

        var ColumnName = $(this).attr("data-val");
        if (ColumnName == "State (State is required for US)") {
            ColumnName = "State";
        }
        ExcelColumnName.push(ColumnName);
    });

    if ($("#DataColumn-0").val() == 0 || $("#DataColumn-1").val() == 0 || $("#DataColumn-7").val() == 0) {
        return false;
    }
    else if (duplicateselectCount > 0) { return false; }
    else {
        var displaydata = $(".norecored").is(":visible");
        if (!displaydata) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, Tags: Tags, fileType: fileType }),
                headers: { "__RequestVerificationToken": token },
                dataType: "json",
                contentType: "application/json",
                success: function (data) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", data, true);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });

            return true;
        } else {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success", "Please Upload Data first.", false);
        }
    }
    return false;
});

$(document).ready(function () {
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {

            if (event.target == this) {
                $("#Tags").val($(this).val());
            }
        });
    }
    $($('.mainNav ul')[0]).append($('.liCircleUserText'));
    $(".SelectBox").each(function () {
        var fieldName = $(this).parent().parent().find(".spnExcelColumn").attr('data-val');
        var selectedvalue = 0;
        var id = $(this).attr('id');
        $("#" + id + " option").each(function () {
            var optionName = $(this).text();
            if (fieldName == optionName) {
                selectedvalue = $(this).val();
            }
            else if (optionName.toLowerCase() == "srcrecordid" || optionName.toLowerCase() == "src recordid" || optionName.toLowerCase() == "accountid" || optionName.toLowerCase() == "company id" || optionName.toLowerCase() == "companyid") {
                if (fieldName.toLowerCase() == "srcrecordid") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "company" || optionName.toLowerCase() == "companyname" || optionName.toLowerCase() == "organization" || optionName.toLowerCase() == "address1_name") {
                if (fieldName.toLowerCase() == "companyname") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "address" || optionName.toLowerCase() == "address1" || optionName.toLowerCase() == "street line address" || optionName.toLowerCase() == "street line address1" || optionName.toLowerCase() == "address1_line1") {
                if (fieldName.toLowerCase() == "street line address1") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "address2" || optionName.toLowerCase() == "street line address2" || optionName.toLowerCase() == "address1_line2") {
                if (fieldName.toLowerCase() == "street line address2") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "address1_city" || optionName.toLowerCase() == "city") {
                if (fieldName.toLowerCase() == "city") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "state" || optionName.toLowerCase() == "address1_stateorprovince") {
                if (fieldName.toLowerCase() == "state (state is required for us)") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "zipcode" || optionName.toLowerCase() == "postalcode" || optionName.toLowerCase() == "zip" || optionName.toLowerCase() == "address1_postalcode") {
                if (fieldName.toLowerCase() == "postalcode") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "country" || optionName.toLowerCase() == "countrycode" || optionName.toLowerCase() == "countryisoalpha2code" || optionName.toLowerCase() == "address1_country") {
                if (fieldName.toLowerCase() == "countryisoalpha2code" || fieldName.toLowerCase() == "country") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "phoneno" || optionName.toLowerCase() == "phonenbr" || optionName.toLowerCase() == "phone" || optionName.toLowerCase() == "address1_telephone1") {
                if (fieldName.toLowerCase() == "phonenbr") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "tags" || optionName.toLowerCase() == "tag") {
                if (fieldName.toLowerCase() == "tags") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "altaddress" || optionName.toLowerCase() == "altaddress1" || optionName.toLowerCase() == "street line alt. address" || optionName.toLowerCase() == "street line alt. address1" || optionName.toLowerCase() == "address_line1" || optionName.toLowerCase() == "address1_line1") {
                if (fieldName.toLowerCase() == "street line alt. address1") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "altaddress2" || optionName.toLowerCase() == "street line alt. address2") {
                if (fieldName.toLowerCase() == "street line alt. address2") {
                    selectedvalue = $(this).val();
                }
            }
            else if (optionName.toLowerCase() == "orbnum" || optionName.toLowerCase() == "orb num" || optionName.toLowerCase() == "orb number" || optionName.toLowerCase() == "orbnumber" || optionName.toLowerCase() == "orb_num" || optionName.toLowerCase() == "orb_number") {
                if (fieldName.toLowerCase() == "orbnum") {
                    selectedvalue = $(this).val();
                }
            }
        });
        $(this).val(selectedvalue);

        var CurrentColumn = $(this).val();
        SetExampleValue(CurrentColumn, id);



        if ($.UserRole.toLowerCase() == 'lob') {
            $(".chzn-select").attr("data-placeholder", "Add Tags (Required)");
            $(".chzn-select").trigger("chosen:updated");
        }
    });
    $("#btnOIDataMappingInsertData").attr('disabled', 'disabled');
    if ($(".SelectBox").length > 0) {
        ValidSelectedDropDown();
    }

    //open popup after login from salesforce and get call back url
    if (location.hash != "") {
        var fragment = location.hash; // on page load
        isReload = "true";
        $.magnificPopup.open({
            preloader: false,
            closeBtnInside: true,
            type: 'iframe',
            height: '200px',
            items: {
                src: location.protocol + "//" + location.hostname + (location.port ? ':' + location.port : '') + "/OI/Data" + '?' + fragment.replace(/^.*#/, '')
            },
            callbacks: {
                close: function (data) {
                    if (isReload == "true") {
                        window.location = location.protocol + "//" + location.hostname + (location.port ? ':' + location.port : '') + "/OI/Data";
                    }
                }
            },
            closeOnBgClick: false,
            mainClass: 'popupSalesForce'
        });
    }
});

function SetExampleValue(CurrentColumn, id) {
    if (parseInt(CurrentColumn) != 0) {
        $.ajax({
            type: "POST",
            url: "/OIData/UpdateExamples",
            data: JSON.stringify({ CurrentColumn: CurrentColumn }),
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                $("#" + id).parent().next().text(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    } else {
        $("#" + id).parent().next().text('');
    }
}
function ValidSelectedDropDown() {
    var Lobvalidcnt = 0;
    if (parseInt($("#DataColumn-0").val()) > 0) {
    }
    else {
        Lobvalidcnt++;
    }
    if ($.UserRole.toLowerCase() == 'lob') {
        var Tags = $("#Tags").val();
        var IsTag = $("#IsTag").val();
        var LicenseEnableTags = $("#LicenseEnableTags").val();
        if (LicenseEnableTags.toLowerCase() == "true") {
            if (IsTag.toLowerCase() == "false") {
                if (Tags == '' || Tags == '0') {
                    Lobvalidcnt++;
                }
            }
            else {
                    if (parseInt($("#DataColumn-10").val()) > 0) { }
                    else {
                        Lobvalidcnt++;
                    }
            }
        }
    }
    if (Lobvalidcnt > 0) {
        $("#btnDataMappingInsertData").attr('disabled', 'disabled');
        $("#btnInsertLargeData").attr('disabled', 'disabled');
    } else {
        $("#btnDataMappingInsertData").attr('disabled', false);
        $("#btnInsertLargeData").attr('disabled', false);
    }
}
$("body").on('click', '.OpenTags', function () {
    $.magnificPopup.open({
        preloader: true,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/Tags/AddTags/?isAllowLOBTag=' + true
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: 'popupAddTags'
    });
});
function SetTagValue(OptionValue) {
    $.magnificPopup.close();
    var x = document.getElementById("DataColumn");
    var option = document.createElement("option");
    option.text = OptionValue;
    option.value = OptionValue;
    x.add(option);
    $(".chzn-select").trigger("chosen:updated");
}
function ShowMessageNotification(MessageType, Message, isPopup, IsCallBack, FunctionName) {
    if (isPopup) {
        $.magnificPopup.close();
    }
    parent.ShowMessageNotification(MessageType, Message, false, false)
    if (IsCallBack) {
        FunctionName();
    }
}

//ORB Data Match Refresh
$("body").on('click', '#btnOIDataMappingInsertData', function () {
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var count = 0;
    var totalCount = 0;
    var duplicateselectCount = 0;
    var fileType = $("#fileType").val();
    var url = "";
    url = "/OIData/OrbImportPanelDataMatch";

    $(".SelectBox").each(function () {
        cat.push($(this).val());
        totalCount = totalCount + 1;

        $(this).parent().removeClass('has-error');
        if ($(this).hasClass("chzn-select")) {
            OrgColumnName.push("");
        }

        else {
            OrgColumnName.push($(this).find(":selected").text());
        }
    });
    if (cat.length > 0) {
        for (var i = 0; i < cat.length; i++) {
            if (i == 0 || i == 1 || i == 7) {
                if ($("#DataColumn-" + i).val() == 0) {
                    $("#DataColumn-" + i).parent().addClass('has-error');
                }
            }
            for (var j = 0; j <= cat.length; j++) {
                if (i != j && cat[i] == cat[j]) {
                    count = count + 1;
                    var currentvalue = $("#DataColumn-" + i).val();
                    var nextValue = $("#DataColumn-" + j).val();
                    if (parseInt(currentvalue) > 0 || parseInt(nextValue) > 0) {
                        duplicateselectCount = duplicateselectCount + 1;
                        if (parseInt(currentvalue) > 0) {
                            $("#DataColumn-" + i).parent().addClass('has-error');
                        }
                        if (parseInt(nextValue) > 0) {
                            $("#DataColumn-" + j).parent().addClass('has-error');
                        }
                    }
                }
            }
        }
    }
    $(".spnExcelColumn").each(function () {

        var ColumnName = $(this).attr("data-val");
        if (ColumnName == "State (State is required for US)") {
            ColumnName = "State";
        }
        ExcelColumnName.push(ColumnName);
    });

    if ($("#DataColumn-0").val() == 0 || $("#DataColumn-1").val() == 0 || $("#DataColumn-7").val() == 0) {
        return false;
    }
    else if (duplicateselectCount > 0) { return false; }
    else {
        var displaydata = $(".norecored").is(":visible");
        if (!displaydata) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, fileType: fileType }),
                headers: { "__RequestVerificationToken": token },
                dataType: "json",
                contentType: "application/json",
                success: function (data) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", data, true);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });

            return true;
        } else {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success", "Please Upload Data first.", false);
        }
    }
    return false;
});

//Salesforce Integration 
$('body').on('click', '#ClsSalesForceLogin', function () {
    var importMode = $("input[name='rBtnOrbImportDataOption']:checked").val();
    var url = "/OIData/OISalesforceLogin?LoggedIn=false&importMode=" + importMode;
    $(this).attr("href", url);
    $(this).click();
});

function openSalesForceMatchData(fileType, importMode) {
    $.magnificPopup.proto.close("data");
    var QueryString = "fileType:" + fileType + "@#$importMode:" + importMode;
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/OIData/OrbDataMatch?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***")
        },
        callbacks: {
            close: function () {
                backparentSalesforce();
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupOrbColmnMapping'
    });
}
function backparentSalesforce() {
    window.parent.$.magnificPopup.close();
    window.location = location.protocol + "//" + location.hostname + (location.port ? ':' + location.port : '') + "/OI/Data";
}
//end Salesforce Integration 


// Low volume-High volume in ORB data
$('body').on('click', '#IdLowvolumeTab', function () {
    if (!$(this).parent("li").hasClass("active")) {
        LoadLowvolume();
    }
});
function LoadLowvolume() {
    $.ajax({
        type: 'GET',
        url: "/OIData/OILowVolumeIndex",
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#partialDivLowVolume").html(data);
            setTimeout(function () {
                $("#partialDivLowVolume").jarvisWidgets('destroy');
                $("#partialDivLowVolume").jarvisWidgets(
                    {
                        "toggleClass": "fa fa-minus | fa fa-plus"
                    });
            }, 1000);
        }
    });
}

$('body').on('click', '#IdHighvolumeTab', function () {
    if (!$(this).parent("li").hasClass("active")) {
        LoadHighvolume();
    }
});
function LoadHighvolume() {
    $.ajax({
        type: 'GET',
        url: "/OIData/OIHighVolumeIndex",
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#partialDivHighVolume").html(data);
            setTimeout(function () {
                $("#partialDivHighVolume").jarvisWidgets('destroy');
                $("#partialDivHighVolume").jarvisWidgets({
                    "toggleClass": "fa fa-minus | fa fa-plus"
                });
            }, 1000);

        }
    });
}

$("body").on('change', '.pagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#SortBy").val();
    var SortOrder = $("#SortOrder").val();
    var FileType = $("#FileType").val();
    var url = '/OIData/OIHighVolumeIndex?pagevalue=' + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder + "&FileType=" + FileType + "&IsInnerPage=" + true;
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divImportJobListing").html(data);
            pageSetUp();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$("body").on('change', '#DisplayImportFileType', function () {
    var FileType = $(this)[0].value;
    var url = '/OIData/OIHighVolumeIndex?FileType=' + FileType + "&IsInnerPage=" + true;
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divImportJobListing").html(data);
            pageSetUp();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function OnSuccessImportJob() {

    $("#divProgress").hide();
    pageSetUp();
}
// Insert data Open popup for Insert Data and make ajax call for match data.
$("body").on('click', '#btnInsertLargeData', function () {
    var abc = "";
    var cat = [];
    var Tags = $("#Tags").val();
    var IsTag = $("#IsTag").val();
    var LicenseEnableTags = $("#LicenseEnableTags").val();
    var InLanguage = $(".Language").val();
    var fileType = $("#fileType").val();
    var ImportMode = $("#ImportMode").val();
    var ProvidersType = $("#ProvidersType").val();
    var duplicateselectCount = 0;
    var count = 0;
    var IsHeader = $("#IsHeader").val();
    $(".SelectBox").each(function () {
        if ($(this).val() == "0") {
            cat.push("");
        }
        else {
            if ($('option:selected', $(this)).text().toLowerCase() == "select language") {
                cat.push("");
            }
            else {
                cat.push($('option:selected', $(this)).text());
            }
        }
        $(this).parent().removeClass('has-error');
    });

    if (cat.length > 0) {
        for (var i = 0; i < cat.length; i++) {
            if (i == 0 || i == 1 || i == 7) {
                if ($("#DataColumn-" + i).val() == 0) {
                    $("#DataColumn-" + i).parent().addClass('has-error');
                }
            }
            for (var j = 0; j <= cat.length; j++) {
                if (i != j && cat[i] == cat[j]) {
                    count = count + 1;
                    var currentvalue = $("#DataColumn-" + i).val();
                    var nextValue = $("#DataColumn-" + j).val();
                    if (parseInt(currentvalue) > 0 || parseInt(nextValue) > 0) {
                        duplicateselectCount = duplicateselectCount + 1;
                        if (parseInt(currentvalue) > 0) {
                            $("#DataColumn-" + i).parent().addClass('has-error');
                        }
                        if (parseInt(nextValue) > 0) {
                            $("#DataColumn-" + j).parent().addClass('has-error');
                        }
                    }
                }
            }
        }
    }

    if (LicenseEnableTags.toLowerCase() == "false") {
        if (ImportMode == "Match Refresh") {
        }
        else {
            cat.splice(9, 0, "null");
        }
    }
    else {
        if (IsTag.toLowerCase() == "true") {
            Tags = null;
        }
        else {
            if (ImportMode == "Match Refresh") {
            }
            else {
                cat[9] = "";
            }
        }
    }

    if (ImportMode == "Data Import") {
        if ($("#DataColumn-0").val() == 0 || $("#DataColumn-1").val() == 0 || $("#DataColumn-7").val() == 0) {
            return false;
        }
    }
    if (ImportMode == "Match Refresh") {
        if ($("#DataColumn-0").val() == 0) {
            return false;
        }
    }

    if (duplicateselectCount > 0) { return false; }
    else {
        var displaydata = $(".norecored").is(":visible");
        if (!displaydata) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: "POST",
                url: '/OIData/LargeDataMatch',
                data: JSON.stringify({ MappingColumnName: cat.toString(), fileType: fileType, ImportType: ImportMode, Tags: Tags, InLanguage: InLanguage, ProvidersType: ProvidersType, IsHeader: IsHeader }),
                headers: { "__RequestVerificationToken": token },
                dataType: "json",
                contentType: "application/json",
                success: function (data) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", data, true, true, parent.LoadHighImportRequest);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });

            return true;
        } else {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success", "Please Upload Data first.", true);
        }
    }
    return false;

});
function LoadHighImportRequest() {
    var pagevalue = $(".pagevalueChange").val();
    var SortBy = $("#SortBy").val();
    var SortOrder = $("#SortOrder").val();
    var FileType = $("#FileType").val();
    var url = '/OIData/OIHighVolumeIndex?pagevalue=' + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder + "&FileType=" + FileType + "&IsInnerPage=" + true;
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divImportJobListing").html(data);
            pageSetUp();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$("body").on('click', '.clsFileTypes', function () {
    var fileType = $(this).data('filetype');
    var importMode = $("input[name='rBtnOrbImportDataOption']:checked").val();
    var QueryString = "fileType:" + fileType + "@#$importMode:" + importMode;
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        height: '200px',
        items: {
            src: '/OIData/OIImportLargeDataPanel?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: 'popupImportHighVolume'
    });
});

$("body").on('click', '#btnOISubmitLargeData', function () {
    var formData = new FormData();
    var ImportMode = $("#ImportMode").val();
    if ($('#file')[0].files[0] != undefined) {

        var delimiter = '';
        var fileType = $("#fileType").val();
        var istext = $("#isText").val().toLowerCase();
        delimiter = $('#txtDelimiter').val();
        if (istext.toLowerCase() == "true") {
            $('.errorMessage').text('');
            if (delimiter == '') {
                $('.errorMessage').text(' Please enter delimiter');
                return false;
            }
        }
        else {
            delimiter = '';
        }
        formData.append('file', $('#file')[0].files[0]);
        formData.append('header', $('#WithHeader').prop('checked'));
        formData.append('isTSV', $("#isTSV").val().toLowerCase());
        formData.append('delimiter', delimiter);
        formData.append('ImportMode', ImportMode);

        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: "POST",
            url: '/OIData/SaveLargeFile',
            data: formData,
            headers: { "__RequestVerificationToken": token },
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                if (response == "Please select valid delimiter or upload a valid file") {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", response, false);
                    return false;
                }
                if (response == "Only formats allowed are :xls,xlsx,csv,tsv,txt") {
                    var formats = $("#allowedFormats").val();
                    var fileExtension = formats.split(','); //['xls', 'xlsx', 'csv'];
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", "Only formats allowed are : " + fileExtension, false);
                    $('#file').val("");
                    return false;
                }
                if (response.indexOf("already belongs to this file") > -1) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", response, false);
                    return false;
                }
                if (response == "Upload failed due to conflicting column name in file - ImportProcessId. Please rename column.") {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", response, false);
                    return false;
                }
                if (response.indexOf("Error:") > -1) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    parent.ShowMessageNotification("success", response, false);
                    return false;
                }
                if (response == "Unable to import blank file") {
                    parent.ShowMessageNotification("success", response, false);
                    return false;
                }
                if (response == "success") {
                    parent.CloseILargemportPanel(fileType, ImportMode);
                }

            },
            error: function (xhr, status, error) {
            }
        });
    } else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        parent.ShowMessageNotification("success", "Please select file first.", false);
    }
    return false;
});
function CloseILargemportPanel(fileType, ImportMode) {

    var popupclass = "";
    if (ImportMode == "Data Import") {
        popupclass = "popupData";
    }
    if (ImportMode == "Match Refresh") {
        popupclass = "popupOrbColmnMappingMatchRefresh";
    }
    var QueryString = "fileType:" + fileType + "@#$ImportMode:" + ImportMode;
    $.magnificPopup.close();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/OIData/LargeDataMatch?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***")
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: popupclass
    });
}
$("body").on('change', '.SelectBox', function () {
    $("#btnOIDataMappingInsertData").attr('disabled', false);
    ValidSelectedDropDown();
    var CurrentColumn = $(this).val();
    var id = $(this).attr('id');
    SetExampleValue(CurrentColumn, id);
    var ImportMode = $("#ImportMode").val();
    $("#btnInsertLargeData").attr('disabled', false);

    if (ImportMode == "Data Import") {
        if ($("#DataColumn-0").val() == 0 || $("#DataColumn-1").val() == 0 || $("#DataColumn-7").val() == 0) {
            $("#btnInsertLargeData").attr('disabled', 'disabled');
            $("#btnDataMappingInsertData").attr('disabled', 'disabled');
        }
        else if ($("#DataColumn-0").val() == 0 || $("#DataColumn-1").val() == 0 || $("#DataColumn-7").val() == 0) {
            $("#DataColumn-").parent().addClass('has-error');
        }
        else {
            $("#btnInsertLargeData").attr('disabled', false);
            $("#btnDataMappingInsertData").attr('disabled', false);
        }
    }
    if (ImportMode == "Match Refresh") {
        if ($("#DataColumn-0").val() == 0) {
            $("#btnInsertLargeData").attr('disabled', 'disabled');
        }
        else {
            $("#btnInsertLargeData").attr('disabled', false);
        }
    }
});  
