function LoadIndexMultiPassConfig() {
    $.pjax({
        url: "/DNB/MultiPass", container: '#divPartialMultiPassConfigList',
        timeout: 50000
    }).done(function () {
        InitMPMConfigurationDataTable();
    })
}

$(document).on("click", "#btnAddMultiPassConfig", function () {
    $.ajax({
        type: 'GET',
        url: '/MultiPass/AddMultiPassConfig',
        dataType: 'HTML',
        success: function (data) {
            $("#AddMultiPassConfigModalMain").html(data);
            DraggableModalPopup("#AddMultiPassConfigModal");
        }
    });
})

$(document).on("click", "#AddMultiPassTagNext", function () {
    var tag = $('#divMultiPassTagSelection #Tags').val();
    if (tag) {
        $("#divMultiPassTagSelection #spnTags").hide();
        var QueryString = "tag:" + tag;
        $.ajax({
            type: 'GET',
            url: '/MultiPass/CreateMultiPassGroup?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
            dataType: 'HTML',
            success: function (data) {
                $("#divMultiPassGroupCreation").html(data);
                $("#divMultiPassTagSelection").closest('article').hide();
                $("#divMultiPassGroupCreation").show();
            }
        });
    }
    else {
        $("#divMultiPassTagSelection #spnTags").show();
    }
})

$(document).on("click", "#btnAddMultiPassGroup", function () {
    var cnt = $(".MultiPassGroupControls").children().last().attr("id");
    if (cnt == undefined)
        cnt = -1;
    var newId = parseInt(cnt) + 1;
    $(".MultiPassGroupControls").append('<div class="groupControls" id="' + newId + '"><div class="col-md-4 padding-top-10"><input type="text" class="form-control txtVerificationGroupName" placeholder = "' + verificationGroupName + '" id="VerificationGroupName' + newId + '"></div><div class="col-md-7 padding-top-10"><select class="verificationLookup form-control" multiple="true" id="VerifiationLookup' + newId + '"></select></div><div class="col-md-1 margin-top-15"><span class="deleteMultiPassgrpControls"><i class="fa fa-trash"></i></span></div></div>');
    $("#VerifiationLookupMaster > option").each(function () {
        $('#VerifiationLookup' + newId).append($("<option></option>").attr("value", this.value).text(this.text));
    });
    $('#VerifiationLookup' + newId).multiselect({
        includeSelectAllOption: true,
        nonSelectedText: selectLookups,
        maxHeight: 300,
        numberDisplayed: 3,
        buttonClass: 'btn LookupMultiSelectbtn' + newId,
    });
})


$(document).on("click", ".deleteMultiPassgrpControls", function () {
    $(this).parent().parent().remove();
})

$(document).on('change', 'input[type=radio][name=matchlast]', function () {
    var category = $(this).filter(':checked').val();
    if (category == "yes") {
        $("#btnAddMultiPassGroup").show();
        $(".MultiPassGroupControls").show();
    }
    else {
        $("#btnAddMultiPassGroup").hide();
        $(".MultiPassGroupControls").hide();
        $(".multipassgroupcreationerr").text('');
    }
});

$(document).on("click", "#AddMultiPassVerificationGroupNext", function () {
    var category = $('input[type=radio][name=matchlast]').filter(':checked').val();
    var tag = $("#TagInGroupStep").val();
    var multiPassObj = [];
    var i = 0;
    var cnt = 0;
    var groupLookupError = false;
    if (category == "yes") {
        $(".MultiPassGroupControls").children().each(function () {
            var currId = $(this).attr("id");
            var groupName = $("#VerificationGroupName" + currId).val().replaceAll(':', '').replaceAll('|','');
            var grpLookups = $("#VerifiationLookup" + currId).val();

            
            if (groupName && groupName.trim()) {
                $("#VerificationGroupName" + currId).removeClass('custom-has-error');
            }
            else {
                $("#VerificationGroupName" + currId).addClass('custom-has-error');
                cnt++;
            }

            if (grpLookups.length > 1) {
                $(".LookupMultiSelectbtn" + currId).removeClass('custom-has-error');
            }
            else {
                $(".LookupMultiSelectbtn" + currId).addClass('custom-has-error');
                groupLookupError = true;
                cnt++;
            }

            if (!cnt > 0) {
                multiPassObj.push({
                    "Category": category,
                    "Tag": tag,
                    "ProviderCode": "106001",
                    "VerificationGroupName": groupName,
                    "VerifiationLookup": grpLookups.toString()
                });
            }
            i++;
        });

        if (cnt > 0) {
            if (groupLookupError)
                $('.multipassgroupcreationerr').text(lookUpValidationmsg);
            return false;
        }
        else {
            var NameArr = multiPassObj.map(function (item) { return item.VerificationGroupName.toLowerCase() });
            var isNameDuplicate = NameArr.some(function (item, idx) {
                return NameArr.indexOf(item.toLowerCase()) != idx
            });

            var LookupArr = multiPassObj.map(function (item) { return item.VerifiationLookup });
            var isLookupDuplicate = LookupArr.some(function (item, idx) {
                return LookupArr.indexOf(item) != idx
            });

            if (isNameDuplicate) {
                $('.multipassgroupcreationerr').text(groupNameValidationmsg);
                return false;
            }
            else if (isLookupDuplicate) {
                $('.multipassgroupcreationerr').text(groupConfigurationValidationmsg);
                return false;
            }
            else {
                $('.multipassgroupcreationerr').text('');
            }

            if (multiPassObj.length < 1) {
                $('.multipassgroupcreationerr').text(multipassGroupValidationmsg);
            }
            else {
                $('.multipassgroupcreationerr').text('');
            }
        }
    }
    else {
        multiPassObj.push({
            "Category": category,
            "Tag": tag,
            "ProviderCode": "106001",
            "VerificationGroupName": "",
            "VerifiationLookup": ""
        });
    }
    if (multiPassObj.length > 0) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: '/MultiPass/SaveMultiPassGroups',
            data: JSON.stringify(multiPassObj),
            beforeSend: function () {
            },
            success: function (data) {
                if (data.result == undefined) {
                    $("#divMultiPassPrecedence").html(data);
                    $("#divMultiPassTagSelection").closest('article').hide();
                    $("#divMultiPassGroupCreation").hide();
                    $("#divMultiPassPrecedence").show();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
})

$(document).on("click", "#MultiPassPrecendenceFinish", function () {
    var tag = $("#TagInPrecedenceStep").val();
    var isCompalsory = $("#IsPrecedenceComplusory").val();
    var providerCode = 106001;
    var objPrecedence = [];
    var selectedPrecedence = [];

    $('#SelectedPrecendence option').each(function () {
        selectedPrecedence.push($(this).val());
    });
    
    for (var i = 0; i < selectedPrecedence.length; i++) {
        objPrecedence.push({
            "Tag": tag,
            "ProviderCode": "106001",
            "Steps": selectedPrecedence[i].split("::")[0],
        });
    }


    if (isCompalsory.toLowerCase() == "true") {
        var cnt = 0;
        $("#AvialPrecendence option").each(function () {
            if ($(this).val().split("::")[1].toLowerCase() == "true") {
                cnt++;
            }
        })
        if (cnt > 0) {
            $('.precendenceErrormsg').show();
            return false;
        }
        else {
            $('.precendenceErrormsg').hide();
        }
    }

    if (objPrecedence.length > 0) {
        $('.precendenceErrormsg').hide();
    }
    else if (isCompalsory.toLowerCase() == "true") {
        $('.precendenceErrormsg').show();
        return false;
    }
    else if (isCompalsory.toLowerCase() == "false") {
        objPrecedence.push({
            "Tag": tag,
            "ProviderCode": "106001",
            "Steps": "",
        });
    }
    if (objPrecedence.length > 0) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: '/MultiPass/SaveMultiPassPrecedence',
            data: JSON.stringify(objPrecedence),
            beforeSend: function () {
            },
            success: function (data) {
                $("#AddMultiPassConfigModal").modal('hide');
                ShowMessageNotification("success", data.message);
                LoadIndexMultiPassConfig();
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
   
})


$(document).on("click", ".editMPMConfiguration", function () {
    var tag = $(this).attr("data-tag");
    var provider = $(this).attr("data-provider");
    var QueryString = "tag:" + tag + "@#$Provider:" + provider;
    $.ajax({
        type: 'GET',
        url: '/MultiPass/UpdateMultiPassGroup?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#AddMultiPassConfigModalMain").html(data);
            DraggableModalPopup("#AddMultiPassConfigModal");
        }
    });
})


$(document).on("click", ".deleteMPMConfiguration", function () {
    var tag = $(this).attr("data-tag");
    var provider = $(this).attr("data-provider");
    var QueryString = "tag:" + tag + "@#$Provider:" + provider;
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: multipleDeleteAutoAccept, callback: function (result) {
            if (result) {
                $.ajax({
                    type: 'GET',
                    url: '/MultiPass/DeleteMultiPassConfiguration?Parameters=' + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
                    dataType: "JSON",
                    success: function (data) {
                        ShowMessageNotification("success", data.message);
                        LoadIndexMultiPassConfig();
                    }
                });
            }
        }
    });
    return false;
})

$(document).on("click", "#AddMultiPassVerificationGroupPrev", function () {
    $("#divMultiPassTagSelection").closest('article').show();
    $("#divMultiPassGroupCreation").hide();
    $("#divMultiPassPrecedence").hide();
})


$(document).on("click", "#MultiPassPrecendencePrev", function () {
    $("#divMultiPassTagSelection").closest('article').hide();
    $("#divMultiPassGroupCreation").show();
    $("#divMultiPassPrecedence").hide();
})

$(document).on('click', '#btnPrecedenceRight', function (e) {
    var selectedOpts = $('#AvialPrecendence option:selected');
    $(this).attr("disabled", true);
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", nothingToMove);

        e.preventDefault();
    }
    $('#SelectedPrecendence').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$(document).on('click', '#btnPrecedenceAllRight', function (e) {
    var selectedOpts = $('#AvialPrecendence option');
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", nothingToMove);
        e.preventDefault();
    }
    $('#SelectedPrecendence').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});
$(document).on('click', '#btnPrecedenceLeft', function (e) {
    var selectedOpts = $('#SelectedPrecendence option:selected');
    $(this).attr("disabled", true);
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", nothingToMove);
        e.preventDefault();
    }
    $('#AvialPrecendence').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});
$(document).on('click', '#btnPrecedenceAllLeft', function (e) {
    var selectedOpts = $('#SelectedPrecendence option');
    if (selectedOpts.length == 0) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
        ShowMessageNotification("success", nothingToMove);
        e.preventDefault();
    }
    $('#AvialPrecendence').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$(document).on('click', '#AvialPrecendence option', function () {
    if ($("#btnPrecedenceRight").attr("disabled") == "disabled" || $("#btnPrecedenceRight").attr("disabled") == undefined) {
        $("#btnPrecedenceRight").attr("disabled", false);
    }
    else {
        $("#btnPrecedenceRight").attr("disabled", true);
    }
})

$(document).on('click', '#SelectedPrecendence option', function () {
    if ($("#btnPrecedenceLeft").attr("disabled") == "disabled" || $("#btnPrecedenceLeft").attr("disabled") == undefined) {
        $("#btnPrecedenceLeft").attr("disabled", false);
    }
    else {
        $("#btnPrecedenceLeft").attr("disabled", true);
    }
})

function InitMPMConfigurationDataTable() {
    InitDataTable("#tblMPMConfiguration", [10, 20, 30], false, []);
}