$(document).ready(function () {
    var ColumnMapping = $("#ColumnMapping").val();
    if (ColumnMapping != undefined && ColumnMapping != "") {
        var ColumnMappingValue = ColumnMapping.split(',');

        var i = 0;
        $(".SelectBoxCommandLine").each(function () {
            var LicenseEnableTags = $("#LicenseEnableTags").val();
            if (i === 10 && LicenseEnableTags.toLowerCase() == "false") {
                i++;
            }
            if (ColumnMappingValue[i] == "") {
                $(this).val("0");
            }

            else {
                $("#" + $(this).attr('id') + " option").filter(function (index) { return $(this).text() === ColumnMappingValue[i]; }).attr('selected', 'selected');
            }
            i++;
        });

        var TagList = $("#TagList").val();
        if (TagList != undefined && TagList != "") {
            TagList = TagList.split(',');
        }
        if (TagList != null || TagList != "") {
            $(".chzn-select option").each(function () {
                for (var i = 0; i < TagList.length; i++) {
                    if ($(this).val() == TagList[i]) {
                        $(this).attr("selected", "selected");
                    }
                }
            });
            $("#Tags").val(TagList);
        }
    }
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {
            if (event.target == this) {
                $("#Tags").val($(this).val());
            }
        });
    }
});

$('body').on('change', '#IdFile', function () {
    var CustmDelimtr = $("#Formatvalue").val();
    var FileFormat = $('input[name=FileFormatCommandLine]:checked').val();
    if (FileFormat.toLowerCase() == "delimiter") {
        if ($("#Formatvalue").val() == "") {
            $('#IdFile').val("");
            $("#Formatvalue").parent().addClass('has-error');
            return false;
        }
    } else {
        CustmDelimtr = "";
    }
    if ($('#file').val() != "") {
        var formats = $("#allowedFormats").val();
        var fileExtension = formats.split(','); //['xls', 'xlsx', 'csv'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            parent.ShowMessageNotification("success", fileFormatsAllowed + fileExtension.join(', '), false);
            //bootbox.alert({
            //    title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
            //    message: "Only formats allowed are : " + fileExtension.join(', '),
            //});
            $('#IdFile').val("");
        }
        else {
            if (File)
                var formData = new FormData();
            var ImportType = $("#ImportType").val();
            if ($('#IdFile')[0].files[0] != undefined) {
                formData.append('file', $(this)[0].files[0]);
                formData.append('ImportType', ImportType);
                formData.append('CustmDelimtr', CustmDelimtr);
                LoadCommandMapping(formData);
            }
        }
    }
    rbtnChangeEvent();
});

$('body').on('change', '.rbtnFile', function () {
    rbtnChangeEvent();
    ResetMapping();
})

function rbtnChangeEvent() {
    var FileFormat = $('input[name=FileFormatCommandLine]:checked').val();
    if (FileFormat.toLowerCase() == "excel") {
        $("#allowedFormats").val("xls,xlsx");
        $("#IdFile").attr("accept", ".xls,.xlsx");
        $(".divFrmtValue").hide();
    }
    else if (FileFormat.toLowerCase() == "csv") {
        $("#allowedFormats").val("csv");
        $("#IdFile").attr("accept", ".csv");
        $(".divFrmtValue").hide();
    }
    else if (FileFormat.toLowerCase() == "tsv" || FileFormat.toLowerCase() == "delimiter") {
        $("#allowedFormats").val("txt");
        $("#IdFile").attr("accept", ".txt");
        if (FileFormat.toLowerCase() == "delimiter") {
            $(".divFrmtValue").show();
        }
        else {
            $(".divFrmtValue").hide();
        }
    }
}

function sumbitCommandMapping() {
    var OriginalColumns = [];
    var cat = [];
    var Tags = $("#Tags").val();
    var IsTag = $("#IsTag").val();
    var LicenseEnableTags = $("#LicenseEnableTags").val();
    var IsInLanguage = $("#IsInLanguage").val();
    var InLanguage = $(".Language option:selected").val();
    var ImportMode = $("#ImportType").val();
    var duplicateselectCount = 0;
    var count = 0;
    var ConfigurationName = $("#ConfigurationName").val();
    var IdFile = $("#IdFile").val();
    var FileFormat = $('input[name=FileFormatCommandLine]:checked').val();
    if (FileFormat.toLowerCase() == "delimiter") {
        if ($("#Formatvalue").val() == "") {
            $('#IdFile').val("");
            $("#Formatvalue").parent().addClass('has-error');
            return false;
        }
    }

    $(".SelectBoxCommandLine").each(function () {
        cat.push($(this).val());
        $(this).parent().removeClass('has-error');
    });
    if (cat.length > 0) {
        for (var i = 0; i < cat.length; i++) {


            if (ImportMode.toLowerCase() == "data import") {
                if (i == 0 || i == 1 || i == 7) {
                    if ($("#DataColumn-" + i).val() == "0" || $("#DataColumn-" + i).val() == null) {
                        $("#DataColumn-" + i).parent().addClass('has-error');
                        duplicateselectCount = duplicateselectCount + 1;
                    }
                }
            }
            if (ImportMode.toLowerCase() == "match refresh") {
                if (i == 0 || i == 1 || i == 2) {
                    if ($("#DataColumn-" + i).val() == "0" || $("#DataColumn-" + i).val() == null) {
                        $("#DataColumn-" + i).parent().addClass('has-error');
                        duplicateselectCount = duplicateselectCount + 1;
                    }
                }
            }
            if (ImportMode.toLowerCase() == "orb data import") {
                if (i == 0 || i == 1 || i == 7) {
                    if ($("#DataColumn-" + i).val() == "0" || $("#DataColumn-" + i).val() == null) {
                        $("#DataColumn-" + i).parent().addClass('has-error');
                        duplicateselectCount = duplicateselectCount + 1;
                    }
                }
            }
            //added orb enrichment only dropdown in upload configuration
            if (ImportMode.toLowerCase() == "orb match refresh") {
                if (i == 0) {
                    if ($("#DataColumn-" + i).val() == "0" || $("#DataColumn-" + i).val() == null) {
                        $("#DataColumn-" + i).parent().addClass('has-error');
                        duplicateselectCount = duplicateselectCount + 1;
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

    if (ConfigurationName == "") {
        $("#spnUConfigName").show();
        duplicateselectCount++
    } else {
        $("#spnUConfigName").hide();
    }

    if (duplicateselectCount > 0) {
        return false;
    }
    else {
        cat = [];
        $(".SelectBoxCommandLine").each(function () {

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
        if (IsInLanguage.toLowerCase() == "false") {
            if (ImportMode == "Data Import") {
                $("#InLanguage").val(InLanguage);
                cat[9] = "";
            }
        }
        if (LicenseEnableTags.toLowerCase() == "true") {
            if (IsTag.toLowerCase() == "false") {
                if (ImportMode == "Match Refresh") {
                    cat[4] = "";
                }
                //else {
                //    cat[10] = "";
                //}
            }
        }
        else {
            if (ImportMode == "Match Refresh") {
                cat.splice(4, 0, "");
            }
            //else {
            //    cat.splice(10, 0, "");
            //}
        }

        if (IsInLanguage.toLowerCase() == "false") {
            if (ImportMode == "Orb Data Import") {
                $("#InLanguage").val(InLanguage);
                cat[9] = "";
            }
        }


        //added orb enrichment only dropdown in upload configuration
        if (IsInLanguage.toLowerCase() == "false") {
            if (ImportMode == "Orb Match Refresh") {
                $("#InLanguage").val(InLanguage);
                cat[1];
            }
        }

        $("#ColumnMapping").val(cat.toString());


        $("#DataColumn-0 option").each(function () {
            OriginalColumns.push($(this).text())
        })
        $("#OriginalColumns").val(OriginalColumns.toString());

        // Changes for Converting magnific popup to modal popup
        $("#UploadConfigurationModal").modal("hide");
        return true;
    }
    return false;

}

$("body").on('click', '#AddCommandLine', function () {
    $.ajax({
        type: "GET",
        url: '/CommandMapping/CreateCommandMapping',
        dataType: 'html',
        success: function (data) {
            $("#divPartialCommandMapping").html(data);
        }
    });
});

function InsertCommandSuccess() {
    // Changes for Converting magnific popup to modal popup
    $.pjax({
        url: "/Portal/UploadConfiguration", container: '#divPartialCommandMapping',
        timeout: 50000
    }).done(function () {
        InitPortalUploadConfigurationDataTable();
        pageSetUp();
        //var Id = 0;
        if ($("table#tbCmndMapping tbody tr").length == 0) {
            $("table#tbCmndMapping").attr("disabled", true);
            //LoadInsertUpdateCommandDownload(Id);
        }
        else {
            $("table#tbCmndMapping tbody tr:first").addClass("current");
            //Id = $('table#tbCmndDownloadMapping tbody tr:first').attr("data-CmndDownloadId");
            //LoadInsertUpdateCommandDownload(Id);
        }
    });
}

$('body').on('change', '#ImportType', function () {
    var formData = new FormData();
    var ImportType = $("#ImportType").val();
    var CustmDelimtr = $("#Formatvalue").val();
    formData.append('ImportType', ImportType);
    formData.append('CustmDelimtr', CustmDelimtr);
    LoadCommandMapping(formData);
    rbtnChangeEvent();
    $("#IdFile").val("");
});

//REsert Colomn Mapping on changes file or import data type
function ResetMapping() {
    var FileFormat = $('input[name=FileFormatCommandLine]:checked').val();
    //$("#TbColumnMapping>.tbdataMatch").find("tr").remove();
    $('#IdFile').val("");
    if (FileFormat.toLowerCase() != "delimiter") {
        $("#Formatvalue").val("|");
    }
    //$("#MappingDiv").hide();
    $("Tags").val("");
    $(".SelectBoxCommandLine").each(function () {
        $(this).find("option").remove()
    });

}

function ResetFields() {
    $('#ConfigurationName').val('');
    $('#ImportType').val('Data Import');
    $('#ColumnMapping').val('');
    $('#InLanguage').val('');
    $('#Tags').val('');
    $('#Id').val('0');
    $("#IsDefault").attr("checked", false)
}

function LoadCommandMapping(formData) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: '/CommandMapping/BindColumnMapping',
        headers: { "__RequestVerificationToken": token },
        data: formData,
        async: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.result == false) {
                $('#spnInvalidColumns').text(data.message);
                $('#spnInvalidColumns').show();
            }
            else {
                $('#spnInvalidColumns').hide();
                $("#DivPartialBindColumnMapping").html(data);
                var ImportMode = $("#ImportType").val();
                $(".SelectBoxCommandLine").each(function () {
                    if (ImportMode.toLowerCase() == "data import") {
                        var fieldName = $(this).closest("tr").find(".spnExcelColumn").attr('data-val');
                        var selectedvalue = 0;
                        $(".SelectBoxCommandLine option").each(function () {

                            var optionName = $(this).text();
                            if (fieldName == optionName) {
                                selectedvalue = $(this).val();
                            }
                            if (optionName.toLowerCase() == "country" || optionName.toLowerCase() == "countrycode" || optionName.toLowerCase() == "countryisoalpha2code" || optionName.toLowerCase() == "address1_country") {
                                if (fieldName.toLowerCase() == "countryisoalpha2code" || fieldName.toLowerCase() == "country") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "zipcode" || optionName.toLowerCase() == "postalcode" || optionName.toLowerCase() == "zip" || optionName.toLowerCase() == "address1_postalcode") {
                                if (fieldName.toLowerCase() == "postalcode") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "phoneno" || optionName.toLowerCase() == "phonenbr" || optionName.toLowerCase() == "phone" || optionName.toLowerCase() == "address1_telephone1") {
                                if (fieldName.toLowerCase() == "phonenbr") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "company" || optionName.toLowerCase() == "companyname" || optionName.toLowerCase() == "organization" || optionName.toLowerCase() == "address1_name") {
                                if (fieldName.toLowerCase() == "companyname") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "srcrecordid" || optionName.toLowerCase() == "src recordid" || optionName.toLowerCase() == "accountid") {
                                if (fieldName.toLowerCase() == "srcrecordid") {
                                    selectedvalue = $(this).val();
                                }
                            }

                            if (optionName.toLowerCase() == "language" || optionName.toLowerCase() == "language values" || optionName.toLowerCase() == "languagevalues" || optionName.toLowerCase() == "language code" || optionName.toLowerCase() == "languagecode") {

                                if (fieldName.toLowerCase() == "inlanguage") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "address" || optionName.toLowerCase() == "street line address1" || optionName.toLowerCase() == "address1_line1") {

                                if (fieldName.toLowerCase() == "street line address1") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "address1" || optionName.toLowerCase() == "street line address2" || optionName.toLowerCase() == "address1_line2") {

                                if (fieldName.toLowerCase() == "street line address2") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "altaddress" || optionName.toLowerCase() == "street line alt. address1" || optionName.toLowerCase() == "address2_line1") {

                                if (fieldName.toLowerCase() == "street line alt. address1") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "altaddress1" || optionName.toLowerCase() == "street line alt. address2") {

                                if (fieldName.toLowerCase() == "street line alt. address2") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "state" || optionName.toLowerCase() == "address1_stateorprovince") {
                                if (fieldName.toLowerCase() == "state (state is required for us)") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "address1_city") {
                                if (fieldName.toLowerCase() == "city") {
                                    selectedvalue = $(this).val();
                                }
                            }

                            if (optionName.toLowerCase() == "tags" || optionName.toLowerCase() == "tag") {
                                if (fieldName.toLowerCase() == "tags") {
                                    selectedvalue = $(this).val();
                                }
                            }
                        });
                        $(this).val(selectedvalue);
                    }
                    if (ImportMode == "Match Refresh") {
                        var fieldName = $(this).closest("tr").find(".spnExcelColumn").attr('data-val');
                        var selectedvalue = 0;
                        $(".SelectBoxCommandLine option").each(function () {
                            var optionName = $(this).text();
                            if (fieldName == optionName) {
                                selectedvalue = $(this).val();
                            }
                            if (optionName.toLowerCase() == "country" || optionName.toLowerCase() == "countrycode" || optionName.toLowerCase() == "countryisoalpha2code" || optionName.toLowerCase() == "address1_country") {
                                if (fieldName.toLowerCase() == "countryisoalpha2code" || fieldName.toLowerCase() == "country") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "srcrecordid" || optionName.toLowerCase() == "src recordid" || optionName.toLowerCase() == "accountid") {
                                if (fieldName.toLowerCase() == "srcrecordid") {
                                    selectedvalue = $(this).val();
                                }
                            }
                            if (optionName.toLowerCase() == "tags" || optionName.toLowerCase() == "tag") {
                                if (fieldName.toLowerCase() == "tags") {
                                    selectedvalue = $(this).val();
                                }
                            }
                        });
                        $(this).val(selectedvalue);
                    }
                    if (ImportMode == "Orb Data Import") {
                        var fieldName = $(this).closest("tr").find(".spnExcelColumn").attr('data-val');
                        var selectedvalue = 0;
                        $(".SelectBoxCommandLine option").each(function () {
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
                            else if (optionName.toLowerCase() == "orbnum" || optionName.toLowerCase() == "orb num" || optionName.toLowerCase() == "orb number" || optionName.toLowerCase() == "orbnumber") {
                                if (fieldName.toLowerCase() == "orbnum") {
                                    selectedvalue = $(this).val();
                                }
                            }
                        });
                        $(this).val(selectedvalue);
                    }
                });

                if ($(".chzn-select").length > 0) {
                    $(".chzn-select").chosen().change(function (event) {
                        if (event.target == this) {
                            $("#Tags").val($(this).val());
                        }
                    });
                }
            }
        }
    });
}



$(document).on('click', "#Previous", function () {
    var id = $(".AAAA").children(".tab-pane.active").attr("id");
    if (id == "tab2") {
        $(".AAAA").children(".tab-pane").each(function () {
            $(this).removeClass("active");
        });
        var PrevId = $("#" + id).prev().attr("id");
        $("#" + PrevId).addClass("active");
        //$("#Next").parent().removeClass("disabled")
        $("#Next").parent().show();
        if ($("#" + PrevId).is(':first-child')) {
            //$("#Previous").parent().addClass("disabled")
            $("#Previous").parent().hide();
        }
    }
});

$(document).on('click', "#Next", function () {
    var Id = $("#Id").val();
    var ConfigurationName = $("#ConfigurationName").val();
    var IdFile = $("#IdFile").val();
    var duplicateselectCount = 0;
    if (ConfigurationName == "") {
        $("#spnUConfigName").show();
        duplicateselectCount++
    } else {
        $("#spnUConfigName").hide();
    }
    if (IdFile == "" && Id == 0) {
        $("#spnfile").show();
        duplicateselectCount++
    } else {
        $("#spnfile").hide();
    }
    if (duplicateselectCount > 0) {
        return false;
    }

    var id = $(".AAAA").children(".tab-pane.active").attr("id");
    if (id == "tab1") {
        $(".AAAA").children(".tab-pane").each(function () {
            $(this).removeClass("active");
        });
        var NextId = $("#" + id).next().attr("id");
        $("#" + NextId).addClass("active");
        $("#Previous").parent().show();
        if ($("#" + NextId).is(':last-child')) {
            $("#Next").parent().hide();
        }
    }
});