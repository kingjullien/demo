$(document).ajaxStart(function () { $('#divProgress').show(); }).ajaxStop(function () { $('#divProgress').hide(); });

$(document).ready(function () {
    //["TableCoulmnName","ServerColumnName","DropDownURL","ISDefault(use lowercase)","Show text/value(use lowercase)","Datatype(If Date)"]
    var colArray = [["Files", "Files", "/ImportData/GetFilesDD", "true"],
    ["User", "User", "/ImportData/GetUserList", "false", "text"],
    ["ImportType", "ImportType", "/ImportData/GetImportTypes"],
    ["Status", "Status", "/ImportData/GetAllStatus"],
    ["Message", "Message", "", "", "", "onlytext"],
    ["RequestedDate", "RequestedDate", "/ImportData/GetRequestedDate", "false", "text", "date"],
    ["ErrorMessage", "ErrorMessage", "", "", "", "onlytext"]];

    //Column array,URL for FilterData, TargetedDiv, Datatable function
    InitFilters(colArray, "/ImportData/FilterImportData", "#ImportDataFilterMain", "#ImportFileListing", "InitDataTable('#ImportJobs', [10,20,30], false,[2,'desc'])");
});

$(document).on('click', '#singleForm', function () {
    var SingleEntryFrmUrl = "";
    var popupcls = "";
    var currentprovider = $("#CurrentProvider").val();
    if (currentprovider.toLowerCase() == "oi") {
        SingleEntryFrmUrl = '/ImportData/OISingleEntryForm';
        popupcls = 'popupOrbSingleEntryForm';
    }
    else if (currentprovider.toLowerCase() == "dandb") {
        SingleEntryFrmUrl = "/ImportData/SingleFormEntry";
        popupcls = 'popupEntryForm';
    }
    $.ajax({
        type: 'GET',
        url: SingleEntryFrmUrl,
        dataType: 'HTML',
        success: function (data) {
            $("#SingleEntryFormModalMain").html(data);
            DraggableModalPopup("#SingleEntryFormModal");
        }
    });
});

$(document).on("click", "#btnImportFile", function () {
    $.ajax({
        type: 'GET',
        url: "/ImportData/ImportFileIndex",
        dataType: 'HTML',
        success: function (data) {
            $("#ImportFileIndexModalMain").html(data);
            // reset modal if it isn't visible
            DraggableModalPopup("#ImportFileIndexModal");
        }
    });
});

$(document).on('change', '.pagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#SortBy").val();
    var SortOrder = $("#SortOrder").val();
    var FileType = $("#FileType").val();
    var url = '/ImportData/GetFileImportRequest?pagevalue=' + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder + "&FileType=" + FileType;
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#ImportFileListing").html(data);
            pageSetUp();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$(document).on('change', '#DisplayImportFileType', function () {
    var FileType = $(this)[0].value;
    var url = '/ImportData/GetFileImportRequest?FileType=' + FileType + "&IsInnerPage=" + true;
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#ImportFileListing").html(data);
            pageSetUp();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$(document).on('change', 'input[type=radio][name=FileFormat]', function () {
    $(".file_options .file_type_option").removeClass("bgLightBlue");
    if ($(this).is(":checked")) {
        $(this).parent().parent().addClass("bgLightBlue");
    }
});

$(document).on('change', 'input[type=radio][name=ImportMode]', function () {
    $(".importTypes .file_type_option").removeClass("bgLightBlue");
    if ($(this).is(":checked")) {
        $(this).parent().parent().addClass("bgLightBlue");
    }
});

$(document).on("click", "#ImportFileIndexNext", function () {
    var IsTemplateSelected = $("#IsTemplateSelected").val();
    var FileFormat = $("input[name='FileFormat']:checked").val();
    var ImportMode = $("input[name='ImportMode']:checked").val();
    $.ajax({
        type: "GET",
        url: '/ImportData/UploadFileIndex?FileFormat=' + FileFormat + '&ImportMode=' + ImportMode + '&IsTemplateSelected=' + IsTemplateSelected,
        dataType: 'html',
        async: false,
        contentType: false,
        processData: false,
        success: function (data) {
            showFileUploadStep();
            $("#FileUploadStep").html(data);
        },
    });
});

$(document).on('click', '.uploadfileProceed', function () {
    var formData = new FormData();
    var ImportMode = $("#FileUploadStep #ImportMode").val();
    var sheetName = $("#sheetName").val();
    if (sheetName == undefined)
        sheetName = '';
    var isFinish = "false";
    if ($(this).val().toLowerCase() == "finish")
        isFinish = "true";
    if ($('#file')[0].files[0] != undefined) {
        if ($("#fileType").val() == "EXCEL" && (sheetName == '' || sheetName == undefined)) {
            $(".generalMsg").text(requiredSheetName);
            //parent.ShowMessageNotification("success", "Please select sheet name.", false, false);
            return;
        }
        var delimiter = '';
        var fileType = $("#fileType").val();
        var TemplateId = 0;
        if ($("#TemplateId").val() == undefined || $("#IsTemplateSelected").val() == "false") {
            TemplateId = 0;
        }
        else {
            TemplateId = $("#TemplateId").val().split("::")[0];
        }
        var istext = $("#isText").val().toLowerCase();
        if (istext == "true") {
            $('.errorMessage').text('');
            delimiter = $('#txtDelimiter').val();
            if (delimiter == '') {
                $('.errorMessage').text();
                return;
            }
        }
        var IsTemplateSelected = $("#IsTemplateSelected").val();
        var templateName = $("#templateName").val();
        if (templateName == undefined) {
            templateName = null;
        }
        else if (templateName == "") {
            $("#spnTemplateName").show();
            return;
        }
        else if (templateName != undefined && templateName != "") {
            templateName = $("#templateName").val().trim();
        }
        var token = $('input[name="__RequestVerificationToken"]').val();
        if ($("#fileType").val() == "FIXED") {
            formData.append('file', $('#file')[0].files[0]);
            formData.append('header', $('#WithHeader').prop('checked'));
            formData.append('templateId', TemplateId);
            formData.append('ImportMode', ImportMode);
            formData.append('isFinish', isFinish);
            $.ajax({
                type: "POST",
                url: '/ImportData/UploadFixedFile',
                async: false,
                data: formData,
                headers: { "__RequestVerificationToken": token },
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response == "Only formats allowed is txt") {
                        var formats = $("#allowedFormats").val();
                        var fileExtension = formats.split(','); //['xls', 'xlsx', 'csv'];
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        $(".generalMsg").text(someFormatsAllowed + " " + fileExtension);
                        $('#file').val("");
                        return false;
                    }
                    if (response == uploadFailed) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == errorOccurred) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response.indexOf(error + ":") > -1) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == unableImportBlankFile) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == noRows) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == invalidTempate) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == unicodeFormatNotValid) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (isFinish == isTrue) {
                        parent.addConfirmScreenPopupclass();
                    }
                    else if (ImportMode == dataImport) {
                        parent.addfullheightPopupClass();
                    }
                    else {
                        parent.addMediumScreenPopupclass();
                    }
                    hideFileUploadStep();
                    $("#ImportProcessStep").html(response);
                    return;
                },
                error: function (xhr, status, error) {
                }
            });
        }
        else {
            formData.append('file', $('#file')[0].files[0]);
            formData.append('header', $('#WithHeader').prop('checked'));
            formData.append('isTSV', $("#isTSV").val().toLowerCase());
            formData.append('delimiter', delimiter);
            formData.append('ImportMode', ImportMode);
            formData.append('templateId', TemplateId);
            formData.append('sheetName', sheetName);
            formData.append('isFinish', isFinish);
            formData.append('IsTemplateSelected', IsTemplateSelected);
            formData.append('templateName', templateName);
            $.ajax({
                type: "POST",
                url: '/ImportData/UploadFileIndex',
                data: formData,
                headers: { "__RequestVerificationToken": token },
                //dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response == selectValidDelimiter) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == someFormatsAreAllowed) {
                        var formats = $("#allowedFormats").val();
                        var fileExtension = formats.split(','); //['xls', 'xlsx', 'csv'];
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        $(".generalMsg").text(someFormatsAllowed + " " + fileExtension);
                        $('#file').val("");
                        return false;
                    }
                    if (response.indexOf(alreadyBelongsToFile) > -1) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == uploadFailed) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == errorOccurred) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response.indexOf(error + ":") > -1) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == unableImportBlankFile) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == noRows) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == invalidTempate) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response == unicodeFormatNotValid) {
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (isFinish == isTrue) {
                        parent.addConfirmScreenPopupclass();
                    }
                    else if (ImportMode == dataImport) {
                        parent.addfullheightPopupClass();
                    }
                    else {
                        parent.addMediumScreenPopupclass();
                    }
                    $(".generalMsg").text("");
                    hideFileUploadStep();
                    $("#ImportProcessStep").html(response);
                },
                error: function (xhr, status, error) {
                }
            });
        }
    }
    else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        $(".generalMsg").text(requiredFile);
    }
    return false;
});

$(document).on("change", "#file", function () {
    if ($('#file').val() != "") {
        var formats = $("#allowedFormats").val();
        var fileExtension = formats.split(','); //['xls', 'xlsx', 'csv'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
            ShowMessageNotification("success", someFormatsAllowed + " " + fileExtension.join(', '), false, false);
            $('#file').val("");
        }
        else if ($("#fileType").val() == "EXCEL") {
            var token = $('input[name="__RequestVerificationToken"]').val();
            var formData = new FormData();
            formData.append('file', $('#file')[0].files[0]);
            $('#divProgress').show();
            $.ajax({
                type: "POST",
                url: '/ImportData/GetSheetNames',
                data: formData,
                headers: { "__RequestVerificationToken": token },
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response == someFormatsAreAllowed) {
                        ShowMessageNotification("success", response, false, false);
                        return;
                    }
                    if (response.indexOf(error + ":") > -1) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        $(".generalMsg").text(response);
                        return false;
                    }
                    if (response.length > 0) {
                        $(".sheetSelect").show();
                        $("#sheetName option").remove();
                        for (var cnt = 0; cnt < response.length; cnt++) {
                            $("#sheetName").append(new Option(response[cnt].split("::")[0], response[cnt]));
                        }
                    }
                    $('#divProgress').hide();
                }
            });
        }
    }
    else {
        $(".sheetSelect").hide();
    }
});

$(document).on("click", "#ComfirmDetailsFinish", function () {
    $.ajax({
        type: "POST",
        url: '/ImportData/SaveFileImportRequest',
        dataType: 'json',
        success: function (data) {
            if (data == dataNotImportedToDatabase)
                parent.ShowMessageNotification("success", data, false, false);
            else {
                parent.ShowMessageNotification("success", data, false, false);
                ClosePopupAndReload();
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
})

$(document).on("change", "#TemplateId", function () {
    var data = $(this).val().split("::");
    var Id = data[0];
    var sheetName = data[1];
    var delimeter = data[2];
    var HasHeader = data[3];

    if (parseInt(Id) > 0) {
        $("#UploadFileFinish").attr('disabled', false);
        if (sheetName) {
            $("#sheetName > option").each(function () {
                if (this.value.split('::')[0].toLowerCase() === sheetName.toLowerCase()) {
                    $("#sheetName").val(this.value);
                }
            });
        }

        if (delimeter)
            $("#txtDelimiter").val(delimeter);
        if (HasHeader && HasHeader.toLowerCase() == "true")
            $("#WithHeader").prop("checked", true);
        else
            $("#WithHeader").prop("checked", false);
    }
    else {
        $("#sheetName").val("");
        $("#WithHeader").prop("checked", false);
        $("#UploadFileFinish").attr('disabled', true);
    }
})

$(document).on("click", "#ColumnMetaNext", function () {
    var cnt = 0;
    var ImportMode = $("#ImportMode").val();
    var IsTemplate = $("#IsTemplate").val();
    var templateName = $("#templateName").val();
    var IsTemplateSelected = $("#IsTemplateSelected").val();
    var startindex = [];
    var fieldLength = [];
    var fieldName = [];
    $(".startIndex").each(function () {
        if ($.isNumeric($(this).val())) {
            startindex.push($(this).val());
            $(this).removeClass("custom-Has-error");
        }

        else {
            $(this).addClass("custom-Has-error");
            cnt++;
        }

    });
    $(".fieldLength").each(function () {
        if ($.isNumeric($(this).val())) {
            fieldLength.push($(this).val());
            $(this).removeClass("custom-Has-error");
        }
        else {
            $(this).addClass("custom-Has-error");
            cnt++;
        }
    });

    $(".columnName").each(function () {
        var currentName = $(this).val();
        if (currentName) {
            if (fieldName.length == 0) {
                fieldName.push(currentName);
                $(this).removeClass("custom-Has-error");
            }
            else {
                $.each(fieldName, function (index, value) {
                    if (value.toLowerCase() === currentName.toLowerCase()) {
                        parent.ShowMessageNotification("success", msgColumnname + currentName + msgColumnalreadyexists, false);
                        cnt++;
                        return false;
                    }
                });
                fieldName.push(currentName);
            }
        }
        else {
            $(this).addClass("custom-Has-error");
            cnt++;
        }

    });
    if (cnt === 0) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: "POST",
            url: '/ImportData/FixedFileColumnMapping',
            data: JSON.stringify({ startIndex: startindex, fieldLength: fieldLength, fieldName: fieldName, IsTemplate: IsTemplate, ImportMode: ImportMode, IsTemplateSelected: IsTemplateSelected, templateName: templateName }),
            headers: { "__RequestVerificationToken": token },
            contentType: "application/json",
            processData: false,
            success: function (response) {
                if (response == invalidFieldMapping) {
                    parent.ShowMessageNotification("success", response, false);
                }
                else if (response == errorOccurred) {
                    parent.ShowMessageNotification("success", response, false);
                }
                else {
                    $("#ImportProcessStep").html(response);
                }
            },
            error: function (xhr, status, error) {
            }
        });
    }

})

$(document).on("click", "#btnAddColumn", function () {
    var IsHeader = $('#IsHeader').val();
    var currentId = $('.FixedColumnMeta tr:last').attr('id');
    var startIndex = $("#txtStart" + currentId).val();
    var length = $("#txtLength" + currentId).val();
    var nextStartVal = '';
    if ($.isNumeric(startIndex) && $.isNumeric(length)) {
        nextStartVal = parseInt(startIndex) + parseInt(length);
    }
    currentId = parseInt(currentId) + 1;
    if (IsHeader.toLowerCase() === "true") {
        $(".FixedColumnMeta").append('<tr class="row" id="' + currentId + '"><td class="col-md-3"><input type="text" class="startIndex OnlyDigit" id="txtStart' + currentId + '" value="' + nextStartVal + '"></td> <td class="col-md-3"><input type="text" class="fieldLength OnlyDigit" id="txtLength' + currentId + '" /></td ><td class="col-md-3"><input type="text" class="columnName" id="txtcolumnName' + currentId + '"  />&nbsp;&nbsp;<a href="javascript:void(0);" class="removeColumn" data-id="' + currentId + '"> <i class="fa fa-times-circle"></i></a ></td ><td class="col-md-3"></td></tr > ');
    }
    else {
        $(".FixedColumnMeta").append('<tr class="row" id="' + currentId + '"><td class="col-md-3"><input type="text" class="startIndex OnlyDigit" id="txtStart' + currentId + '" value="' + nextStartVal + '"></td> <td class="col-md-3"><input type="text" class="fieldLength OnlyDigit" id="txtLength' + currentId + '" /></td ><td class="col-md-3"><input type="text" class="columnName" id="txtcolumnName' + currentId + '" value="Column ' + currentId + '" readonly="true" />&nbsp;&nbsp;<a href="javascript:void(0);" class="removeColumn" data-id="' + currentId + '"> <i class="fa fa-times-circle"></i></a ></td ><td class="col-md-3"></td></tr > ');
    }

})

$(document).on("click", ".removeColumn", function () {
    var Id = $(this).attr("data-id");
    var rowid = "#" + Id;
    $(rowid).remove();
})

$(document).on("keypress", ".OnlyDigit", function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;
})

$(document).on('blur', ".startIndex", function () {
    var Id = $(this).attr("id").replace("txtStart", "");
    GetFixedValue(Id);
});

$(document).on('blur', ".fieldLength", function () {
    var Id = $(this).attr("id").replace("txtLength", "");
    GetFixedValue(Id);
});

$(document).on("click", "#UploadFilePrev", function () {
    $.ajax({
        type: "GET",
        url: '/ImportData/ImportFileIndex?IsFromPrev=' + true,
        dataType: 'html',
        async: false,
        contentType: false,
        success: function (data) {
            hideFileUploadStep();
            $("#ImportProcessStep").html(data);
        },
    });
})

$(document).on("click", "#ColumnMappingPrev", function () {
    var fileType = $("#fileType").val();
    var IsTemplateSelected = $("#IsTemplateSelected").val();
    if (fileType == "FIXED") {
        $.ajax({
            type: "GET",
            url: '/ImportData/BackToFixedFileMetaColumn',
            dataType: 'html',
            async: false,
            contentType: false,
            success: function (data) {
                $("#ImportProcessStep").html(data);
            }
        });
    }
    else {
        parent.RemoveAllPopupClass();
        showFileUploadStep();
        if (!IsTemplateSelected) {
            hideTemplate();
        }
    }
});

$(document).on("click", "#ColumnMetaPrev", function () {
    parent.RemoveAllPopupClass();
    showFileUploadStep();
});

$(document).on("click", "#ConfirmDetailsPrev", function () {
    var isFinish = $("#IsFromFinish").val();
    var IsTemplateSelected = $("#IsTemplateSelected").val();
    if (isFinish.toLowerCase() == "true") {
        parent.RemoveAllPopupClass();
        showFileUploadStep();
    }
    else {
        $.ajax({
            type: "GET",
            url: '/ImportData/BackToColumnMapping?IsTemplateSelected=' + IsTemplateSelected,
            dataType: 'html',
            async: false,
            contentType: false,
            success: function (data) {
                if ($("#ImportType").val() == dataImport) {
                    parent.addfullheightPopupClass();
                }
                else {
                    parent.addMediumScreenPopupclass();
                }
                $("#ImportProcessStep").html(data);
            },
        });
    }

})

$(document).on('change', '.SelectBox', function () {
    $.UserRole = $("#UserRole").val();
    $.UserLOBTag = $("#UserLOBTag").val();
    var currentprovider = $("#CurrentProvider").val();
    var ImportMode = $("#ImportMode").val();
    var id = $(this).attr('id');
    var CurrentColumn = $(this).val();
    var Tags = $("#Tags").val();
    var IsTag = $("#IsTag").val();
    var LicenseEnableTags = $("#LicenseEnableTags").val();
    var Lobvalidcnt = 0;
    if (ImportMode == "Data Import") {
        Lobvalidcnt = 0;
        if (parseInt($("#DataColumn-0").val()) > 0 && parseInt($("#DataColumn-7").val()) > 0) {
        }
        else {
            Lobvalidcnt++;
        }
        if ($.UserRole.toLowerCase() == 'lob') {
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
            $("#ColumnMappingFinish").attr('disabled', 'disabled');
            $(".TemplateButtons").attr('disabled', 'disabled');
        } else {
            $("#ColumnMappingFinish").attr('disabled', false);
            $(".TemplateButtons").attr('disabled', false);
        }
    }
    if (ImportMode == "Match Refresh") {
        Lobvalidcnt = 0;
        if (currentprovider.toLowerCase() == "dandb") {
            if (parseInt($("#DataColumn-0").val()) > 0 && parseInt($("#DataColumn-1").val()) && parseInt($("#DataColumn-2").val()) > 0) {
            }
            else {
                Lobvalidcnt++;
            }
        }
        else if (currentprovider.toLowerCase() == "oi") {
            if (parseInt($("#DataColumn-0").val()) > 0) {
            }
            else {
                Lobvalidcnt++;
            }
        }
        if ($.UserRole.toLowerCase() == 'lob') {
            if (LicenseEnableTags.toLowerCase() == "true") {
                if (IsTag.toLowerCase() == "false") {
                    if (Tags == '' || Tags == '0') {
                        Lobvalidcnt++;
                    }
                }
                else {
                    if (parseInt($("#DataColumn-3").val()) > 0) { }
                    else {
                        Lobvalidcnt++;
                    }
                }
            }
        }
        if (Lobvalidcnt > 0) {
            $("#ColumnMappingFinish").attr('disabled', 'disabled');
            $(".TemplateButtons").attr('disabled', 'disabled');
        } else {
            $("#ColumnMappingFinish").attr('disabled', false);
            $(".TemplateButtons").attr('disabled', false);
        }
    }
    if (parseInt(CurrentColumn) != 0) {
        $.ajax({
            type: "POST",
            url: "/ImportData/UpdateExamples",
            data: JSON.stringify({ CurrentColumn: CurrentColumn }),
            dataType: "json",
            contentType: "application/json",
            success: function (data) {

                $("#" + id).parent().parent().next().text(data);

            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    } else {
        $("#" + id).parent().parent().next().text('');
    }
});

$(document).on('click', '#ColumnMappingFinish', function () {
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var count = 0;
    var totalCount = 0;
    var duplicateselectCount = 0;
    var Tags = $("#Tags").val();
    var IsTag = $("#IsTag").val();
    var IsInLanguage = $("#IsInLanguage").val();
    var InLanguage = $(".Language").val();
    var LicenseEnableTags = $("#LicenseEnableTags").val();
    var fileType = $("#fileType").val();
    var url = "";
    var ImportMode = $("#ImportMode").val();
    var url = "/ImportData/ConfirmImportDetails";
    if (Tags == undefined || Tags == "0") {
        Tags = "";
    }
    if (InLanguage == undefined) {
        InLanguage = "";
    }
    $(".SelectBox").each(function () {

        cat.push($(this).val());
        totalCount = totalCount + 1;
        if ($(this).hasClass("chzn-select")) {
            OrgColumnName.push("Tags");
        }
        else if ($(this).hasClass("Language")) {
            OrgColumnName.push("InLanguage");
        }
        else {
            OrgColumnName.push($(this).find(":selected").text());
        }
        $(this).parent().removeClass('has-error');
    });
    if (cat.length > 0) {
        for (var i = 0; i < cat.length; i++) {
            if (i == 0 || i == 7) {
                if ($("#DataColumn-" + i).val() == 0) {
                    $("#DataColumn-" + i).parent().addClass('has-error');
                }
            }
            if (ImportMode == "Match Refresh") {
                if (i == 1) {
                    if ($("#DataColumn-" + i).val() == 0) {
                        $("#DataColumn-" + i).parent().addClass('has-error');
                    }
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

    if ($("#DataColumn-0").val() == 0 || $("#DataColumn-7").val() == 0) {
        return false;
    }
    else if (ImportMode == "Match Refresh" && $("#DataColumn-1").val() == 0) {
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
                data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, Tags: Tags, InLanguage: InLanguage, fileType: fileType }),
                contentType: "application/json",
                headers: { "__RequestVerificationToken": token },
                success: function (data) {
                    parent.addConfirmScreenPopupclass();
                    $("#ImportProcessStep").html(data);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });

            return true;
        } else {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success", msgUploadData, true, false);

        }
    }
    return false;
});

$(document).on('click', '.TemplateButtons', function () {
    var templateName = $("#templateName").val();
    var button = $(this).text().trim();
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var count = 0;
    var totalCount = 0;
    var duplicateselectCount = 0;
    var Tags = $("#Tags").val();
    var IsTag = $("#IsTag").val();
    var IsInLanguage = $("#IsInLanguage").val();
    var InLanguage = $(".Language").val();
    var LicenseEnableTags = $("#LicenseEnableTags").val();
    var fileType = $("#fileType").val();
    var ImportMode = $("#ImportMode").val();
    var url = "";
    url = "/ImportData/SaveTemplate";
    if (Tags == undefined || Tags == "0") {
        Tags = "";
    }
    if (InLanguage == undefined) {
        InLanguage = "";
    }
    $(".SelectBox").each(function () {
        cat.push($(this).val());
        totalCount = totalCount + 1;
        if ($(this).hasClass("chzn-select")) {
            OrgColumnName.push("Tags");
        }
        else if ($(this).hasClass("Language")) {
            OrgColumnName.push("InLanguage");
        }
        else {
            OrgColumnName.push($(this).find(":selected").text());
        }
        $(this).parent().removeClass('has-error');
    });
    if (cat.length > 0) {
        for (var i = 0; i < cat.length; i++) {
            if (i == 0 || i == 7) {
                if ($("#DataColumn-" + i).val() == 0) {
                    $("#DataColumn-" + i).parent().addClass('has-error');
                }
            }
            if (ImportMode == "Match Refresh") {
                if (i == 1) {
                    if ($("#DataColumn-" + i).val() == 0) {
                        $("#DataColumn-" + i).parent().addClass('has-error');
                    }
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
    if (templateName == "") { templateName = "null"; }
    if ($("#DataColumn-0").val() == 0 || $("#DataColumn-7").val() == 0) {
        return false;
    }
    else if (ImportMode == "Match Refresh" && $("#DataColumn-1").val() == 0) {
        return false;
    }
    else if (duplicateselectCount > 0) { return false; }
    else if (templateName != "null") {
        var displayData = $(".norecored").is(":visible");
        if (!displayData) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, Tags: Tags, InLanguage: InLanguage, fileType: fileType, TemplateName: templateName }),
                contentType: "application/json",
                headers: { "__RequestVerificationToken": token },
                success: function (data) {
                    var newTemplateId = data.split("::")[1];
                    var newTemplateName = templateName;
                    var message = data.split("::")[0];
                    $("#ConfigureImportsModal #TemplateId").append('<option selected="selected" value=' + newTemplateId + '>' + newTemplateName + '</option>');                    
                    $("#ConfigureImportsModal .dropdownIcon").html("<i class='fa fa-info-circle fa-lg templateInfoIcon' data-TemplateId='" + newTemplateId + "' style='display: none;'></i>");
                    $("#ConfigureImportsModal .templateInfoIcon").show();
                    $("#ConfigureImportsModal #addTransferDunsTag").hide();
                    $.ajax({
                        type: 'GET',
                        url: "/ImportData/ImportFileTemplatesList",
                        dataType: 'HTML',
                        success: function (data) {
                            SetTemplateListFilter();
                        }
                    });
                    parent.ShowMessageNotification("success", message, false, false);
                    $("#ImportFileIndexModal").modal("hide");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
        else {
            parent.ShowMessageNotification("success", msgUploadData, true, false);
        }
    }
    else {
        var displaydata = $(".norecored").is(":visible");
        if (!displaydata) {
            if (button.toLowerCase() == saveAsTemplateImport) {
                bootbox.dialog({
                    title: enterTemplateName,
                    centerVertical: true,
                    message: "<div style='margin-bottom:-8px;'><input type='text' id='templateNameNew' name='templateNameNew' class='col-md-12 form-control' maxlength='64'><br /><span class='templateNameError error'></span></div>",
                    buttons: {
                        cancel: {
                            label: cancel,
                            className: 'btn-default',
                            callback: function () {

                            }
                        },
                        confirm: {
                            label: save,
                            className: 'btn-primary',
                            callback: function () {
                                var result = $("#templateNameNew").val().trim();
                                if (result != "" && result != undefined) {
                                    var token = $('input[name="__RequestVerificationToken"]').val();
                                    $.ajax({
                                        type: "POST",
                                        url: url,
                                        data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, Tags: Tags, InLanguage: InLanguage, fileType: fileType, TemplateName: result }),
                                        contentType: "application/json",
                                        headers: { "__RequestVerificationToken": token },
                                        success: function (data) {
                                            var message = data.split("::")[0];
                                            parent.ShowMessageNotification("success", message, false, false);
                                        },
                                        error: function (xhr, ajaxOptions, thrownError) {
                                        }
                                    });
                                }
                                else {
                                    $(".templateNameError").text(enterTemplateName);
                                    //parent.ShowMessageNotification("success", "Please Enter Template Name", false, false);
                                    return false;
                                }
                            }
                        }
                    }
                });
            }
            else {
                var token = $('input[name="__RequestVerificationToken"]').val();
                $.ajax({
                    type: "POST",
                    url: url,
                    data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, Tags: Tags, InLanguage: InLanguage, fileType: fileType }),
                    contentType: "application/json",
                    headers: { "__RequestVerificationToken": token },
                    success: function (data) {
                        parent.ShowMessageNotification("success", data, false, false);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });

                return true;
            }

        } else {
            parent.ShowMessageNotification("success", msgUploadData, true, false);
        }
    }
    return false;
});
$(document).on('click', '#btnSEDataImport', function () {
    var SingleEntryFrmUrl = "";
    var popupcls = "";
    var currentprovider = $("#CurrentProvider").val();
    if (currentprovider.toLowerCase() == "oi") {
        SingleEntryFrmUrl = '/ImportData/OISingleEntryForm';
        popupcls = 'popupOrbSingleEntryForm';
    }
    else if (currentprovider.toLowerCase() == "dandb") {
        SingleEntryFrmUrl = "/ImportData/SingleFormEntry";
        popupcls = 'popupEntryForm';
    }

    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: SingleEntryFrmUrl
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: popupcls
    });
});

$(document).on('click', '#btnSEMatchRefresh', function () {
    var SingleEntryFrmUrl = "";
    var popupcls = "";
    var PopupClassName = "";
    var currentprovider = $("#CurrentProvider").val();
    if (currentprovider.toLowerCase() === "oi") {
        SingleEntryFrmUrl = '/ImportData/OISingleEntryMatchRefresh';
        PopupClassName = 'popupOrbSingleEntryFormMatchRefresh';
    }
    else if (currentprovider.toLowerCase() === "dandb") {
        if ($.IsTagsLicenseAllow.toLowerCase() === "true") {
            PopupClassName = 'popupEntryMatchrefreshForm'
        }
        else {
            PopupClassName = 'popupEntryMatchrefreshFormNoTag'
        }
        SingleEntryFrmUrl = "/ImportData/SingleMatchRefreshFormEntry";
    }

    popupcls = PopupClassName;
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: SingleEntryFrmUrl
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: popupcls
    });
})

$(document).on('click', '.importFileStats', function () {
    var FileId = $(this).attr("data-ImportId");
    $.ajax({
        type: "GET",
        url: '/ImportData/GetFileDetails?Parameters=' + ConvertEncrypte(encodeURI(FileId)).split("+").join("***"),
        success: function (data) {
            if (data.result) {
                $("#ImportFileStatsDetailsMain").html(data.message);
                DraggableModalPopup("#ImportFileStatsModal");
            }
            else {
                ShowMessageNotification("error", data.message, false);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

function OnSuccessImportJob() {
    $("#divProgress").hide();
    pageSetUp();
}

function GetFixedValue(Id) {
    var IsHeader = $("#IsHeader").val();
    var startIndex = $("#txtStart" + Id).val();
    var fieldLength = $("#txtLength" + Id).val();
    var fieldName = $("#txtcolumnName" + Id).val();
    if ($.isNumeric(startIndex) && $.isNumeric(fieldLength)) {
        if (IsHeader.toLowerCase() == "true") {
            $.ajax({
                type: "POST",
                url: "/ImportData/UpdateFixedColumnNames",
                data: JSON.stringify({ startIndex: startIndex, fieldLength: fieldLength, fieldName: fieldName }),
                dataType: "json",
                async: true,
                contentType: "application/json",
                success: function (data) {
                    $("#txtcolumnName" + Id).val(data);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }

        $.ajax({
            type: "POST",
            url: "/ImportData/UpdateFixedExamples",
            data: JSON.stringify({ startIndex: startIndex, fieldLength: fieldLength, fieldName: fieldName }),
            dataType: "json",
            async: true,
            contentType: "application/json",
            success: function (data) {
                $(".FixedColumnMeta").find("#" + Id).find('td:last').text(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });

    } else {
        $(".FixedColumnMeta").find("#" + Id).find('td:last').text('');
    }

}

function addfullheightPopupClass() {
    $(".popupImportData").removeClass("mediumHeight confirmHeight");
    $(".popupImportData").addClass("fullheight");
}

function addConfirmScreenPopupclass() {
    $(".popupImportData").removeClass("fullheight");
    $(".popupImportData").removeClass("mediumHeight");
    $(".popupImportData").addClass("confirmHeight");
}

function addMediumScreenPopupclass() {
    $(".popupImportData").removeClass("fullheight");
    $(".popupImportData").removeClass("confirmHeight");
    $(".popupImportData").addClass("mediumHeight");
}

function RemoveAllPopupClass() {
    $(".popupImportData").removeClass("fullheight");
    $(".popupImportData").removeClass("mediumHeight");
    $(".popupImportData").removeClass("confirmHeight");
}

function hideFileUploadStep() {
    $("#FileUploadStep").hide();
    $("#ImportProcessStep").show();
}

function showFileUploadStep() {
    $("#ImportProcessStep").hide();
    $("#ImportProcessStep").html('');
    $("#FileUploadStep").show();
}
function hideTemplate() {
    $(".templateSelect").hide();
}
function ClosePopupAndReload() {
    $("#ImportFileIndexModal").modal("hide");
    location.reload();
}
function addRemoveOISingleEntryPopupClass(className, IsAdd) {
    if (IsAdd) {
        $(".popupOrbSingleEntryForm").addClass(className);
    }
    else {
        $(".popupOrbSingleEntryForm").removeClass(className);
    }

}
function addRemoveDNBSingleEntryPopupClass(IsAdd) {
    var IsLicense = $("#IsTagsLicenseAllow").val();
    if (IsAdd) {
        $(".popupEntryForm").addClass(IsLicense.toLowerCase() === "true" ? "popupEntryMatchrefreshForm" : "popupEntryMatchrefreshFormNoTag");
    }
    else {
        $(".popupEntryForm").removeClass(IsLicense.toLowerCase() === "true" ? "popupEntryMatchrefreshForm" : "popupEntryMatchrefreshFormNoTag");
    }

}
$(document).on('click', '#rptShowBackgroundProcessList', function () {
    LoadShowBackgroundProcessList();
});
function LoadShowBackgroundProcessList() {
    $.ajax({
        type: 'GET',
        url: "/Home/BackgroundProcessList",
        dataType: 'HTML',
        success: function (data) {
            $("#BackgroundProcessListModalMain").html(data);
            DraggableModalPopup("#BackgroundProcessListModal");
            //$("#BackgroundProcessListModal").modal("show");

            //["TableCoulmnName","ServerColumnName","DropDownURL","ISDefault:IsMultiselect(use lowercase)","Show text/value(use lowercase)","Datatype(If Date)","Default selected values"]
            var colArray = [["TimePeriod", "TimePeriod", "/Home/GetDurationHoursDD", "true", "text"],
            ["ProcessType", "ProcessType", "/Home/GetETLTypeDD", "", "value", "text", "IMPORT", "true"],
            ["StatusType", "StatusType", "/Home/GetStatusType", "false"]
            ];

            //Column array,URL for FilterData, FilterParentDiv,TargetedDiv, DatatableId
            InitFilters(colArray, "/Home/FilterBackGroundProcess", "#BackGroundProcessFilterMain", "#divPartialBackgroundProcess", "");
        }
    });


}
function CleansMatchSinngleEnrtyFormSuccess(data) {
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#SingleEntryFormModal").modal("hide");
    }
}
function OICleansMatchSinngleEnrtyFormSuccess(data) {
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#SingleEntryFormModal").modal("hide");
    }
}

$(document).on('click', '#btnGetImportTemplatesList', function () {
    $.ajax({
        type: 'GET',
        url: "/ImportData/ImportFileTemplatesList",
        dataType: 'HTML',
        success: function (data) {
            $("#ImportFileTemplatesModalMain").html(data);
            DraggableModalPopup("#ImportFileTemplatesModal");
            SetTemplateListFilter();
        }
    });
});

$('body').on('click', '.deleteTemplate', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var TemplateId = $(this).attr('data-TemplateId');
    var TemplateName = $(this).attr('data-TemplateName');
    var QueryString = "TemplateId:" + TemplateId + "@#$TemplateName:" + TemplateName;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    var url = '/ImportData/RemoveTemplateData?Parameters=' + Parameters;
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: removeTemplate, callback: function (result) {
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
                        SetTemplateListFilter();
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                        parent.ShowMessageNotification("success", data, false);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});

function LoadTemplateAgain(url) {
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#divPartialImportTemplate").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$(document).on("click", ".clsTemplateDetails", function () {
    var templateId = $(this).attr("data-TemplateId");
    LoadTemplateDetails(templateId);
});
$(document).on("click", ".dropdownIcon", function () {
    var templateId = $(".templateInfoIcon").attr("data-TemplateId");
    LoadTemplateDetails(templateId);
});
// User Story 105-File Import Configuration settings page - Client Portal
function addNewTemplateFromImport() {
    $.ajax({
        type: 'GET',
        url: "/ImportData/ImportFileIndex?IsTemplateSelected=" + false,
        dataType: 'HTML',
        success: function (data) {
            $("#ImportFileIndexModalMain").html(data);
            // reset modal if it isn't visible
            DraggableModalPopup("#ImportFileIndexModal");

        }
    });
}
function InsertUpdateConfigureImportsFromImport(Id, TemplateId, TemplateName) {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/Portal/InsertUpdateConfigureImports?Parameters=" + ConvertEncrypte(encodeURI(Id)).split("+").join("***") + "&IsFromImportData=" + true + "&TemplateId=" + TemplateId + "&TemplateName=" + TemplateName,
        dataType: 'HTML',
        success: function (data) {
            $("#ConfigureImportsModalMain").html(data);
            DraggableModalPopup("#ConfigureImportsModal");
            $("#PostLoadAction").trigger("change");
        }
    });
}
function sumbitConfigureImport() {
    var ConfigurationName = $("#ConfigurationName").val().trim();
    var ExternalDataStoreId = $("#ExternalDataStoreId").val();
    var SourceFolderPath = $("#SourceFolderPath").val().trim();
    var FileNamePattern = $("#FileNamePattern").val().trim();
    var PostLoadAction = $("#PostLoadAction").val();
    var PostLoadActionParameters = $("#PostLoadActionParameters").val().trim();
    var TemplateId = $("#TemplateId").val();
    var cnt = 0;
    if (ConfigurationName == "") {
        $("#spnConfigurationName").show();
        cnt++;
    }
    else {
        $("#spnConfigurationName").hide();
    }
    if (ExternalDataStoreId == "") {
        $("#spnExternalDataStoreId").show();
        cnt++;
    }
    else {
        $("#spnExternalDataStoreId").hide();
    }
    if (SourceFolderPath == "") {
        $("#spnSourceFolderPath").show();
        cnt++;
    }
    else {
        $("#spnSourceFolderPath").hide();
    }
    if (FileNamePattern == "") {
        $("#spnFileNamePattern").show();
        cnt++;
    }
    else {
        $("#spnFileNamePattern").hide();
    }
    if (PostLoadAction == "ARCHIVE" || PostLoadAction == "RENAME") {
        if (PostLoadActionParameters == "") {
            $("#spnPostLoadActionParameters").show();
            cnt++;
        }
        else {
            $("#spnPostLoadActionParameters").hide();
        }
    }
    else if (PostLoadAction == "DELETE" || PostLoadAction == "DONOTHING") {
        $("#spnPostLoadActionParameters").hide();
        $("#PostLoadActionParameters").val('');
    }
    if (TemplateId == 0) {
        $("#spnTemplateId").show();
        cnt++;
    }
    else {
        $("#spnTemplateId").hide();
    }
    if (cnt > 0) {
        return false;
    }
}
$("body").on('change', '#PostLoadAction', function () {
    var PostLoadAction = $("#PostLoadAction").val();
    if (PostLoadAction == "ARCHIVE") {
        $("#PostLoadActionParameters").attr("readonly", false);
        $(".setCheckBoxForPostLoadAction").show();
        $(".txtAppendUTCTime").show();
        $('#PostLoadActionParameters').attr('placeholder', ArchivePath);
    }
    else if (PostLoadAction == "RENAME") {
        $("#PostLoadActionParameters").attr("readonly", false);
        $(".setCheckBoxForPostLoadAction").show();
        $(".txtAppendUTCTime").show();
        $('#PostLoadActionParameters').attr('placeholder', NewFileExtension);
    }
    else {
        $("#PostLoadActionParameters").attr("readonly", true);
        $("#spnPostLoadActionParameters").hide();
        $(".setCheckBoxForPostLoadAction").hide();
        $(".txtAppendUTCTime").hide();
        $("#PostLoadActionParameters").val('');
        $('#PostLoadActionParameters').attr('placeholder', PostLoadActionParameters);
    }
});
function UpdateConfigureImportsSuccess(data) {
    // Changes for Converting magnific popup to modal popup
    if (data.result == true) {
        $("#ConfigureImportsModal").modal("hide");
    }
    ShowMessageNotification("success", data.message, false);
    var IsFromImportData = $("#IsFromImportData").val();
    if (IsFromImportData == "true") {
        $.ajax({
            type: 'GET',
            url: "/ImportData/ImportFileTemplatesList",
            dataType: 'HTML',
            success: function (data) {
                SetTemplateListFilter();
            }
        });
    }
    else if (data.result && IsFromImportData == "False") {
        LoadConfigureImports();
    }
}
//END TASK User Story 105-File Import Configuration settings page - Client Portal

function LoadTemplateDetails(TemplateId) {
    var QueryString = "TemplateId:" + TemplateId;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: 'GET',
        url: "/ImportData/GetTemplateDetails?Parameters=" + Parameters,
        dataType: 'HTML',
        success: function (data) {
            $("#TemplateDetailsModalMain").html(data);
            DraggableModalPopup("#TemplateDetailsModal");
        }
    });
}


function SetTemplateListFilter() {
    //["TableCoulmnName","ServerColumnName","DropDownURL","ISDefault(use lowercase)","Show text/value(use lowercase)","Datatype(If Date)"]
    var colArray = [["Format", "Format", "/ImportData/GetFormat", "true"],
    ["TemplateName", "TemplateName", "/ImportData/GetTemplateName"],
    ["ImportType", "ImportType", "/ImportData/GetImportTypes"]];

    //Column array,URL for FilterData, TargetedDiv, Datatable function
    InitFilters(colArray, "/ImportData/FilterImportDataTemplateList", "#ImportDataTemplateListFilter", "#divPartialImportTemplate", "InitDataTable('#tblImportFileTemplate', [10, 20, 30], false, [0, 'desc'])");
}