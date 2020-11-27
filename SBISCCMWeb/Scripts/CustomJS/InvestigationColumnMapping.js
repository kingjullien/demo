$(document).on('click', '#btnInvestigationFileSubmit', function () {
    var type = $("#InvestigationColumnMappingModal #Type").val();
    var cat = [];
    var OrgColumnName = [];
    var ExcelColumnName = [];
    var count = 0;
    var duplicateselectCount = 0;
    var cnt = 0;
    var totalCount = 0;
    $("#InvestigationColumnMappingModal .SelectBox").each(function () {
        cat.push($(this).val());
        totalCount = totalCount + 1;
        OrgColumnName.push($(this).find(":selected").text());
        $(this).parent().removeClass('has-error');
    });

    if (type.toLowerCase() == "mini") {
        if (cat.length > 0) {
            for (var i = 0; i < cat.length; i++) {
                if (i != 4) {
                    if ($("#InvestigationColumnMappingModal #DataColumn-" + i).val() == 0) {
                        $("#InvestigationColumnMappingModal #DataColumn-" + i).parent().addClass('has-error');
                        cnt++;
                    }
                }
                for (var j = 0; j <= cat.length; j++) {
                    if (i != j && cat[i] == cat[j]) {
                        count = count + 1;
                        var currentvalue = $("#InvestigationColumnMappingModal #DataColumn-" + i).val();
                        var nextValue = $("#InvestigationColumnMappingModal #DataColumn-" + j).val();
                        if (parseInt(currentvalue) > 0 || parseInt(nextValue) > 0) {
                            duplicateselectCount = duplicateselectCount + 1;
                            if (parseInt(currentvalue) > 0) {
                                $("#InvestigationColumnMappingModal #DataColumn-" + i).parent().addClass('has-error');
                            }
                            if (parseInt(nextValue) > 0) {
                                $("#InvestigationColumnMappingModal #DataColumn-" + j).parent().addClass('has-error');
                            }
                        }
                    }
                }
            }
        }
        var cmnt = $("#InvestigationColumnMappingModal #rComments").val();
        var Email = $("#InvestigationColumnMappingModal #reqEmail").val();
        if (cmnt == "" || cmnt == undefined || cmnt == null || (!isValidResearchComments(cmnt))) {
            cnt++;
            $("#InvestigationColumnMappingModal .errResearchComments").show();
        }
        else {
            $("#InvestigationColumnMappingModal .errResearchComments").hide();
        }

        if (Email == "" || Email == undefined || Email == null) {
            cnt++;
            $("#InvestigationColumnMappingModal .errCustEmail").text("Requestor Email is required.");
            $("#InvestigationColumnMappingModal .errCustEmail").show();
        }
        else if (!isValidEmailAddress(Email)) {
            cnt++;
            $("#InvestigationColumnMappingModal .errCustEmail").text("Please enter valid email address.");
            $("#InvestigationColumnMappingModal .errCustEmail").show();
        }
        else {
            $("#InvestigationColumnMappingModal .errCustEmail").hide();
        }
        $("#InvestigationColumnMappingModal .spnExcelColumn").each(function () {
            var ColumnName = $(this).attr("data-val");
            ExcelColumnName.push(ColumnName);
        });
        if (cnt > 0) { return false; }
        else if (duplicateselectCount > 0) { return false; }
        else {
            $.ajax({
                type: "POST",
                url: "/ResearchInvestigation/SubmitColumnMapping",
                data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, Email: Email, Comment: cmnt }),
                contentType: "application/json",
                success: function (data) {
                    $("#InvestigationColumnMappingModal").modal('hide');
                    ShowMessageNotification("success", data, false);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
    }
    else {
        var reqFields = [0, 1, 3, 4, 6, 7];
        if (cat.length > 0) {
            for (var a = 0; a < cat.length; a++) {
                if (reqFields.indexOf(a) > -1) {
                    if ($("#InvestigationColumnMappingModal #DataColumn-" + a).val() == 0) {
                        $("#InvestigationColumnMappingModal #DataColumn-" + a).parent().addClass('has-error');
                        cnt++;
                    }
                }
                for (var b = 0; b <= cat.length; b++) {
                    if (a != b && cat[a] == cat[b]) {
                        count = count + 1;
                        var Tarcurrentvalue = $("#InvestigationColumnMappingModal #DataColumn-" + a).val();
                        var TarnextValue = $("#InvestigationColumnMappingModal #DataColumn-" + b).val();
                        if (parseInt(Tarcurrentvalue) > 0 || parseInt(TarnextValue) > 0) {
                            duplicateselectCount = duplicateselectCount + 1;
                            if (parseInt(Tarcurrentvalue) > 0) {
                                $("#InvestigationColumnMappingModal #DataColumn-" + a).parent().addClass('has-error');
                            }
                            if (parseInt(TarnextValue) > 0) {
                                $("#InvestigationColumnMappingModal #DataColumn-" + b).parent().addClass('has-error');
                            }
                        }
                    }
                }
            }
        }

        var TarEmail = $("#InvestigationColumnMappingModal #reqEmail").val();
        if (TarEmail == "" || TarEmail == undefined || TarEmail == null) {
            cnt++;
            $("#InvestigationColumnMappingModal .errCustEmail").text("Requestor Email is required.");
            $("#InvestigationColumnMappingModal .errCustEmail").show();
        }
        else if (!isValidEmailAddress(TarEmail)) {
            cnt++;
            $("#InvestigationColumnMappingModal .errCustEmail").text("Please enter valid email address.");
            $("#InvestigationColumnMappingModal .errCustEmail").show();
        }
        else {
            $("#InvestigationColumnMappingModal .errCustEmail").hide();
        }

        $("#InvestigationColumnMappingModal .spnExcelColumn").each(function () {
            var ColumnName = $(this).attr("data-val");
            ExcelColumnName.push(ColumnName);
        });

        if (cnt > 0) { return false; }
        else if (duplicateselectCount > 0) { return false; }
        else {
            $.ajax({
                type: "POST",
                url: "/ResearchInvestigation/SubmitColumnMappingTargeted",
                data: JSON.stringify({ OrgColumnName: OrgColumnName, ExcelColumnName: ExcelColumnName, Email: TarEmail }),
                contentType: "application/json",
                success: function (data) {
                    $("#InvestigationColumnMappingModal").modal('hide');
                    ShowMessageNotification("success", data, false);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
    }

});

$(document).on('change', '#InvestigationColumnMappingModal .SelectBox', function () {
    var id = $(this).attr('id');
    var CurrentColumn = $(this).val();
    if (parseInt(CurrentColumn) != 0) {
        $.ajax({
            type: "POST",
            url: "/ResearchInvestigation/UpdateExamples",
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
})