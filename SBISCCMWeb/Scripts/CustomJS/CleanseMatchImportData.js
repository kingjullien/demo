function onLoadCleansMatchImportData() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $(".SelectBox").each(function () {
        var fieldName = $(this).closest("tr").find(".spnExcelColumn").attr('data-val');
        var selectedvalue = 0;
        $(".SelectBox option").each(function () {

            var optionName = $(this).text();
            if (fieldName == optionName) {
                selectedvalue = $(this).val();
            }
            if (optionName.toLowerCase() == "confidencecode" || optionName.toLowerCase() == "confidencecodes") {
                if (fieldName.toLowerCase() == "confidencecodes") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "companygrade") {
                if (fieldName.toLowerCase() == "mg_company") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "streetgrade") {
                if (fieldName.toLowerCase() == "mg_streetno") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "streetnamegrade") {
                if (fieldName.toLowerCase() == "mg_streetname") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "citygrade") {
                if (fieldName.toLowerCase() == "mg_city") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "stategrade") {
                if (fieldName.toLowerCase() == "mg_state") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "addressgrade") {
                if (fieldName.toLowerCase() == "mg_pobox") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "phonegrade") {
                if (fieldName.toLowerCase() == "mg_phone") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "zipgrade") {
                if (fieldName.toLowerCase() == "mg_postalcode") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "density") {
                if (fieldName.toLowerCase() == "mg_density") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "uniqueness") {
                if (fieldName.toLowerCase() == "mg_uniqueness") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "sic") {
                if (fieldName.toLowerCase() == "mg_sic") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "companycode") {
                if (fieldName.toLowerCase() == "mdp_company") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "streetcode") {
                if (fieldName.toLowerCase() == "mdp_streetno") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "streetnamecode") {
                if (fieldName.toLowerCase() == "mdp_streetname") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "citycode") {
                if (fieldName.toLowerCase() == "mdp_city") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "statecode") {
                if (fieldName.toLowerCase() == "mdp_state") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "addresscode") {
                if (fieldName.toLowerCase() == "mdp_pobox") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "phonecode") {
                if (fieldName.toLowerCase() == "mdp_phone") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "tags") {
                if (fieldName.toLowerCase() == "tags") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "excludefromautoaccept") {
                if (fieldName.toLowerCase() == "excludefromautoaccept") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "groupname" || optionName.toLowerCase() == "countrygroupname") {
                if (fieldName.toLowerCase() == "countrygroupname") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "zipcode") {
                if (fieldName.toLowerCase() == "mdp_postalcode") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "densitycode") {
                if (fieldName.toLowerCase() == "mdp_density") {
                    selectedvalue = $(this).val();
                }
            }
            if (optionName.toLowerCase() == "uniquenesscode") {
                if (fieldName.toLowerCase() == "mdp_uniqueness") {
                    selectedvalue = $(this).val();
                }
            } if (optionName.toLowerCase() == "siccode") {
                if (fieldName.toLowerCase() == "mdp_sic") {
                    selectedvalue = $(this).val();
                }
            } if (optionName.toLowerCase() == "dunscode") {
                if (fieldName.toLowerCase() == "mdp_duns") {
                    selectedvalue = $(this).val();
                }
            } if (optionName.toLowerCase() == "nationalidcode") {
                if (fieldName.toLowerCase() == "mdp_nationalid") {
                    selectedvalue = $(this).val();
                }
            } if (optionName.toLowerCase() == "urlcode") {
                if (fieldName.toLowerCase() == "mdp_url") {
                    selectedvalue = $(this).val();
                }
            }
        });
        $(this).val(selectedvalue);
        var id = $(this).attr('id');
        var CurrentColumn = $(this).val();
        if (parseInt(CurrentColumn) != 0) {
            $.ajax({
                type: "POST",
                url: "/DNBIdentityResolution/UpdateExamples",
                data: JSON.stringify({ CurrentColumn: CurrentColumn }),
                dataType: "json",
                headers: { "__RequestVerificationToken": token },
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
    });
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {

            if (event.target == this) {
                $("#Tags").val($(this).val());
            }
        });
    }
    if (parseInt($("#DataColumn-0").val()) > 0 && parseInt($("#DataColumn-1").val()) > 0 && parseInt($("#DataColumn-2").val()) > 0 && parseInt($("#DataColumn-3").val()) > 0 && parseInt($("#DataColumn-4").val()) > 0 && parseInt($("#DataColumn-5").val()) > 0 && parseInt($("#DataColumn-6").val()) > 0 && parseInt($("#DataColumn-7").val()) > 0 && parseInt($("#DataColumn-8").val()) > 0 && parseInt($("#DataColumn-9").val()) > 0 && parseInt($("#DataColumn-10").val()) > 0 && parseInt($("#DataColumn-11").val()) > 0 && parseInt($("#DataColumn-12").val()) > 0 && parseInt($("#DataColumn-13").val()) > 0 && parseInt($("#DataColumn-14").val()) > 0 && parseInt($("#DataColumn-15").val()) > 0 && parseInt($("#DataColumn-16").val()) > 0 && parseInt($("#DataColumn-17").val()) > 0 && parseInt($("#DataColumn-18").val()) > 0 && parseInt($("#DataColumn-20").val()) > 0 && parseInt($("#DataColumn-21").val()) > 0) {
        $("#btnInsertData").attr('disabled', false);
    }
    else {
        $("#btnInsertData").attr('disabled', 'disabled');
    }
}
//import data
$("body").on('click', '#btnSubmitImportData', function () {
    var formData = new FormData();
    if ($('#DAndBAutoAcceptanceImportModal #file')[0].files[0] != undefined) {
        formData.append('file', $('#DAndBAutoAcceptanceImportModal #file')[0].files[0]);
        formData.append('header', $('#WithHeader').prop('checked'));
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: "POST",
            url: '/DNBIdentityResolution/ImportData',
            data: formData,
            headers: { "__RequestVerificationToken": token },
            dataType: 'json',
            contentType: false,
            processData: false,
            async: false,
            success: function (response) {
                if (response == "Only formats allowed are :xls,xlsx") {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    ShowMessageNotification("success", response);
                    $('#DAndBAutoAcceptanceImportModal #file').val("");
                    return false;
                }
                if (response.indexOf("already belongs to this file") > -1) {
                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    ShowMessageNotification("success", response);
                    return false;
                }
                if (response.indexOf("Error:") > -1) {

                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                    ShowMessageNotification("success", response);
                    return false;
                }
                if (response == "success") {
                    parent.CloseImportPanel();
                }

            },
            error: function (xhr, status, error) {
            }
        });
    }
    else {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        parent.ShowMessageNotification("success", selectFile, false);
        //bootbox.alert({
        //    title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
        //    message: "Please select file first."
        //});
    }
    return false;
});
// change the value of the Example display in popup when source changes
$("body").on('change', '.SelectBox', function () {
    var id = $(this).attr('id');
    var CurrentColumn = $(this).val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (parseInt($("#DataColumn-0").val()) > 0 && parseInt($("#DataColumn-1").val()) > 0 && parseInt($("#DataColumn-2").val()) > 0 && parseInt($("#DataColumn-3").val()) > 0 && parseInt($("#DataColumn-4").val()) > 0 && parseInt($("#DataColumn-5").val()) > 0 && parseInt($("#DataColumn-6").val()) > 0 && parseInt($("#DataColumn-7").val()) > 0 && parseInt($("#DataColumn-8").val()) > 0 && parseInt($("#DataColumn-9").val()) > 0 && parseInt($("#DataColumn-10").val()) > 0 && parseInt($("#DataColumn-11").val()) > 0 && parseInt($("#DataColumn-12").val()) > 0 && parseInt($("#DataColumn-13").val()) > 0 && parseInt($("#DataColumn-14").val()) > 0 && parseInt($("#DataColumn-15").val()) > 0 && parseInt($("#DataColumn-16").val()) > 0 && parseInt($("#DataColumn-17").val()) > 0 && parseInt($("#DataColumn-18").val()) > 0 && parseInt($("#DataColumn-20").val()) > 0 && parseInt($("#DataColumn-21").val()) > 0) {
        $("#btnInsertData").attr('disabled', false);
    }
    else {
        $("#btnInsertData").attr('disabled', 'disabled');
    }
    if (parseInt(CurrentColumn) != 0) {
        $.ajax({
            type: "POST",
            url: "/DNBIdentityResolution/UpdateExamples",
            data: JSON.stringify({ CurrentColumn: CurrentColumn }),
            dataType: "json",
            contentType: "application/json",
            headers: { "__RequestVerificationToken": token },
            success: function (data) {

                $("#" + id).parent().next().text(data);

            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    } else {
        $("#" + id).parent().next().text('');
    }
});
// Insert data Open popup for Insert Data and make ajax call for match data.
$("body").on('click', '#btnInsertData', function () {
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var count = 0;
    var notSelectedCount = 0;
    var totalCount = 0;
    var duplicateselectCount = 0;
    var Tags = $("#Tags").val();
    var IsTag = $("#IsTag").val();
    var IsCompanyScore = $("#IsCompanyScore").val();
    var CompanyScore = 0;

    var LicenseEnableTags = $("#LicenseEnableTags").val();
    if (Tags == undefined || Tags == "0") {
        Tags = "";
    }

    $(".SelectBox").each(function () {
        cat.push($(this).val());
        totalCount = totalCount + 1;
        if ($(this).val() == "0") {
            notSelectedCount = notSelectedCount + 1;
        }
        if (totalCount == 20) {
            if (IsTag == "False" && LicenseEnableTags.toLowerCase() == "true") {
                OrgColumnName.push("Tags");
            } else {
                OrgColumnName.push($(this).find(":selected").text());
            }
        } else {
            OrgColumnName.push($(this).find(":selected").text());
        }
        $(this).parent().removeClass('has-error');
    });
    if (IsCompanyScore.toLowerCase() == "false") {
        CompanyScore = $("#txtCmpnyScore").val() == "" ? 0 : $("#txtCmpnyScore").val();
        OrgColumnName.push("CompanyScore");
    }

    if (cat.length > 0) {
        for (var i = 0; i < cat.length; i++) {
            for (var j = i; j < cat.length; j++) {
                if (i != j && cat[i] == cat[j]) {
                    count = count + 1;
                    if (cat[i] != "0") {
                        if (IsTag == "False") {
                            var currentvalue = $("#DataColumn-" + i).val();
                            if (parseInt(currentvalue) > 0) {
                                $("#DataColumn-" + i).parent().addClass('has-error');
                            }
                            var currentvalue = $("#DataColumn-" + j).val();
                            if (parseInt(currentvalue) > 0) {
                                $("#DataColumn-" + j).parent().addClass('has-error');
                            }
                        } else {
                            $("#DataColumn-" + i).parent().addClass('has-error');
                            var currentvalue = $("#DataColumn-" + j).val();
                            if (parseInt(currentvalue) > 0) {
                                $("#DataColumn-" + j).parent().addClass('has-error');
                            }
                        }
                        duplicateselectCount = duplicateselectCount + 1;
                    }
                }
            }
        }
    }
    $(".spnExcelColumn").each(function () {
        var ColumnName = $(this).attr("data-val");
        ExcelColumnName.push(ColumnName);
    });

    if (parseInt($("#DataColumn-0").val()) == 0 && parseInt($("#DataColumn-1").val()) == 0 && parseInt($("#DataColumn-2").val()) == 0 && parseInt($("#DataColumn-3").val()) == 0 && parseInt($("#DataColumn-4").val()) == 0 && parseInt($("#DataColumn-5").val()) == 0 && parseInt($("#DataColumn-6").val()) == 0 && parseInt($("#DataColumn-7").val()) == 0 && parseInt($("#DataColumn-8").val()) == 0 && parseInt($("#DataColumn-9").val()) == 0 && parseInt($("#DataColumn-10").val()) == 0 && parseInt($("#DataColumn-11").val()) == 0 && parseInt($("#DataColumn-12").val()) == 0 && parseInt($("#DataColumn-13").val()) == 0 && parseInt($("#DataColumn-14").val()) == 0 && parseInt($("#DataColumn-15").val()) == 0 && parseInt($("#DataColumn-16").val()) == 0 && parseInt($("#DataColumn-17").val()) == 0 && parseInt($("#DataColumn-18").val()) == 0 && parseInt($("#DataColumn-20").val()) == 0 && parseInt($("#DataColumn-21").val()) == 0) {
        return false;
    }
    else if (duplicateselectCount > 0) { return false; }
    else {
        var OrgColumnNameArray = "";
        if (OrgColumnName.length > 0) {
            for (var i = 0; i < OrgColumnName.length; i++) {
                if (OrgColumnNameArray == "") {
                    OrgColumnNameArray = OrgColumnName[i];
                } else {

                    OrgColumnNameArray = OrgColumnNameArray + ',' + OrgColumnName[i];
                }
            }
        }
        var ExcelColumnNameArray = "";
        if (ExcelColumnName.length > 0) {
            for (var i = 0; i < ExcelColumnName.length; i++) {
                if (ExcelColumnNameArray == "") {
                    ExcelColumnNameArray = ExcelColumnName[i];
                } else {
                    ExcelColumnNameArray = ExcelColumnNameArray + ',' + ExcelColumnName[i];
                }
            }
        }
        var displaydata = $(".norecored").is(":visible");
        if (!displaydata) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            var ExistingRecordsCount = 0;
            $.ajax({
                type: "POST",
                url: "/DNBIdentityResolution/GetSecondaryAutoAcceptanceCriteriaGroupCount",
                dataType: "json",
                contentType: "application/json",
                async: false,
                success: function (data) {

                    ExistingRecordsCount = data;
                    // Changes for Converting magnific popup to modal popup
                    $("#DAndBAutoAcceptImportModal").modal("hide");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });

            if (ExistingRecordsCount == 0) {
                $.ajax({
                    type: "POST",
                    url: "/DNBIdentityResolution/CleanseMatchDataMatchAutoAccept",
                    data: JSON.stringify({ OrgColumnName: OrgColumnNameArray, ExcelColumnName: ExcelColumnNameArray, Tags: Tags, IsOverWrite: false, CompanyScore: CompanyScore }),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "json",
                    contentType: "application/json",
                    async: false,
                    cache: false,
                    success: function (data) {
                        // Changes for Converting magnific popup to modal popup
                        $("#DAndBAutoAcceptImportModal").modal("hide");
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                        ShowMessageNotification("success", data);
                        ClosePopupReload();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            } else {
                bootbox.confirm({
                    title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: overwrite, callback: function (result) {
                        if (result) {
                            bootbox.hideAll();
                            bootbox.confirm({
                                title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: overwriteRulesConfirmMessage, callback: function (valid) {
                                    bootbox.hideAll();
                                    if (valid) {
                                        // Changes for Converting magnific popup to modal popup
                                        $.ajax({
                                            type: 'GET',
                                            url: "/DNBIdentityResolution/DeleteComment?CriteriaId=0" + '&ToCall=ImportData&OrgColumnName=' + OrgColumnName + '&ExcelColumnName=' + ExcelColumnName + '&Tags=' + Tags + '&IsOverWrite=' + result + '&CompanyScore=' + CompanyScore,
                                            dataType: 'HTML',
                                            async: false,
                                            success: function (data) {
                                                $("#divProgress").hide();
                                                $("#DeleteAutoAcceptanceModalMain").html(data);
                                                DraggableModalPopup("#DeleteAutoAcceptanceDataModal");
                                            }
                                        });
                                    } else {
                                        $.ajax({
                                            type: "POST",
                                            url: "/DNBIdentityResolution/CleanseMatchDataMatchAutoAccept",
                                            data: JSON.stringify({ OrgColumnName: OrgColumnNameArray, ExcelColumnName: ExcelColumnNameArray, Tags: Tags, IsOverWrite: valid, CompanyScore: CompanyScore }),
                                            headers: { "__RequestVerificationToken": token },
                                            dataType: "json",
                                            contentType: "application/json",
                                            async: false,
                                            cache: false,
                                            success: function (data) {
                                                // Changes for Converting magnific popup to modal popup
                                                $("#DAndBAutoAcceptImportModal").modal("hide");
                                                //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                                                ShowMessageNotification("success", data);
                                                ClosePopupReload();
                                                
                                                bootbox.hideAll();
                                            },
                                            error: function (xhr, ajaxOptions, thrownError) {
                                            }
                                        });
                                    }
                                }
                            });

                        } else {
                            $.ajax({
                                type: "POST",
                                url: "/DNBIdentityResolution/CleanseMatchDataMatchAutoAccept",
                                data: JSON.stringify({ OrgColumnName: OrgColumnNameArray, ExcelColumnName: ExcelColumnNameArray, Tags: Tags, IsOverWrite: result, CompanyScore: CompanyScore }),
                                headers: { "__RequestVerificationToken": token },
                                dataType: "json",
                                contentType: "application/json",
                                async: false,
                                cache: false,
                                success: function (data) {
                                    bootbox.hideAll();
                                    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                                    ShowMessageNotification("success", data);
                                    ClosePopupReload();
                                    
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                }
                            });
                        }

                        return true;
                    }
                });
            }
        } else {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success", "Please Upload Data first.");
            bootbox.hideAll();
        }
    }
    return false;
});

// check file extension if extension valida or not.
$("body").on('change', '#DAndBAutoAcceptanceImportModal #file', function () {
    if ($('#DAndBAutoAcceptanceImportModal #file').val() != "") {
        var fileExtension = ['xls', 'xlsx'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success","Only formats allowed are : " + fileExtension.join(', '));
            
            $('#DAndBAutoAcceptanceImportModal #file').val("");
        }
    }
});
 
$("#txtCmpnyScore").keyup(function (e) {
    var thisvalue = parseInt($(this).val() == "" ? "0" : ($(this).val()));
    if (thisvalue > 100) {
        $("#txtCmpnyScore").val(0);
    }
    else {
        $("#txtCmpnyScore").val(thisvalue);
    }
});
$(document).on('blur', '#txtCmpnyScore', function () {
    var text = $(this).val();
    if (text != "") {
        if (!$.isNumeric(text)) {
            $("#txtCmpnyScore").val(0);
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success", "Only numeric values accepted.");
        }
        else {
            if (parseInt(text == "" ? "0" : text) > 100) {
                $("#txtCmpnyScore").val(0);
            }
            else {
                $("#txtCmpnyScore").val(text);
            }
        }
    }
});