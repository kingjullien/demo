// Set style on page load 
$(document).ready(function () {
    $(".mfp-iframe-scaler").css("height", "70");
    $(".mfp-iframe").css("height", "70");
    setTimeout(function () { $('#divProgress').hide(); }, 2000);

});
// Email validation for user
function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
// Add user and make ajax call to open add user popup
$('body').on('click', '#btnAddUsers', function () {
    var pagevalue = $(".pagevalueChange").val();
    var SortBy = $("#UserSortBy").val();
    var SortOrder = $("#UserSortOrder").val();
    var url = '/ConfigurationData/Index/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/ConfigurationData/popupConfigData/'
        },
        callbacks: {
            open: function () {
                $.magnificPopup.instance.close = function () {
                    // Do whatever else you need to do here
                    $.ajax({
                        type: 'GET',
                        url: url,
                        dataType: 'HTML',
                        contentType: 'application/html',
                        data: { pagevalue: $("#pagevalue").val() },
                        success: function (data) {
                            $("#divUser").html(data);
                        }
                    });
                    // Call the original close method to close the popup
                    $.magnificPopup.proto.close.call(this);
                };
            }
        },
        closeOnBgClick: false,
        mainClass: 'popUserInitiateReturn'
    });
    return false;
});
// edit user and make ajax call to open edit user popup
$('body').on('click', '.editUser', function () {
    var pagevalue = $(".pagevalueChange").val();
    var SortBy = $("#UserSortBy").val();
    var SortOrder = $("#UserSortOrder").val();
    var url = '/ConfigurationData/Index/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;

    var UserId = this.id;
    if (UserId != "") {
        $.magnificPopup.open({
            preloader: false,
            closeBtnInside: true,
            type: 'iframe',
            callbacks: {
                open: function () {
                    $.magnificPopup.instance.close = function () {
                        // Do whatever else you need to do here
                        $.ajax({
                            type: 'GET',
                            url: url,
                            dataType: 'HTML',
                            contentType: 'application/html',
                            data: { pagevalue: $("#pagevalue").val() },
                            success: function (data) {
                                $("#divUser").html(data);
                            }
                        });
                        // Call the original close method to close the popup
                        $.magnificPopup.proto.close.call(this);
                    };
                }
            },
            items: {
                src: "/ConfigurationData/popupConfigData?Parameters=" + UserId
            },
            closeOnBgClick: false,
            mainClass: 'popUserInitiateReturn',
        });
    }
    return false;
});
// add Country Group and make ajax call to open Country Group popup
$('body').on('click', '#btnAddCountryGroup', function () {
    var pagevalue = $("#CntryGrpPageValue").val();
    var SortBy = $("#CountrySortBy").val();
    var SortOrder = $("#CountrySortOrder").val();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/ConfigurationData/popupConfigCountryGrp/'
        },
        callbacks: {
            close: function () {
                $.ajax({
                    type: 'GET',
                    url: "/ConfigurationData/indexCountryGrp?Countrypagevalue=" + pagevalue + "&Countrysortby=" + SortBy + "&Countrysortorder=" + SortOrder,
                    dataType: 'HTML',
                    cache: false,
                    contentType: 'application/html',
                    async: false,
                    success: function (data) {
                        $("#divCountry").html(data);
                    }
                });
            }
        },
        closeOnBgClick: false,
        mainClass: 'popInitiateReturn popCountryGroup'
    });
    return false;
});
// edit Country Group and make ajax call to open Country Group popup
$('body').on('click', '.editCountryGroup', function () {
    var GroupId = this.id;
    var pagevalue = $("#CntryGrpPageValue").val();
    var SortBy = $("#CountrySortBy").val();
    var SortOrder = $("#CountrySortOrder").val();
    if (GroupId != "") {
        $.magnificPopup.open({
            preloader: false,
            closeBtnInside: true,
            type: 'iframe',
            callbacks: {
                close: function () {
                    $.ajax({
                        type: 'GET',
                        url: "/ConfigurationData/indexCountryGrp?Countrypagevalue=" + pagevalue + "&Countrysortby=" + SortBy + "&Countrysortorder=" + SortOrder,
                        dataType: 'HTML',
                        contentType: 'application/html',
                        cache: false,
                        async: false,
                        success: function (data) {
                            $("#divCountry").html(data);
                        }
                    });
                }
            },
            items: {
                src: "/ConfigurationData/popupConfigCountryGrp?Parameters=" + GroupId
            },
            closeOnBgClick: false,
            mainClass: 'popInitiateReturn popCountryGroup',
        });
    }
    return false;
});
// Delete Country 
$('body').on('click', '.deleteEnrichment', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to delete record ?", callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/ConfigurationData/DeleteAPIGroup?Parameters=" + Parameters,
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {

                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                            message: data, callback: function () {
                                if (data == "Data deleted successfully") {
                                    $.ajax({
                                        type: 'GET',
                                        cache: false,
                                        url: '/ConfigurationData/indexDataEnrichmentSettings/',
                                        dataType: 'HTML',
                                        contentType: 'application/html',
                                        async: false,
                                        success: function (data) {
                                            $("#divPartialDataEnrichmentSettings").html(data);
                                        }
                                    });
                                }
                            }
                        });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});
//function DialogCustomConfirm(message, yesCallback) {
//    $(".popup-body.text-info").html(message);
//    $('.main-dialog-confirm').show();
//    //var dialog = $('#main-dialog-confirm').dialog();
//    $('#CustomeAlertYes').click(function () {
//        $('.main-dialog-confirm').hide();
//        yesCallback();
//    });
//    $('#CustomeAlertNo').click(function () {
//        $('.main-dialog-confirm').hide();
//    });
//}
//$('body').on('click', '.deleteEnrichment', function () {
//    if (confirm("Are you sure you want to delete record ?")) {
//        var Parameters = $(this).attr("id");
//        var token = $('input[name="__RequestVerificationToken"]').val();
//        $.ajax({
//            type: "POST",
//            url: "/ConfigurationData/DeleteAPIGroup?Parameters=" + Parameters,
//            dataType: "JSON",
//            headers: { "__RequestVerificationToken": token },
//            contentType: "application/json",
//            success: function (data) {
//                $(".popup-body.text-info").html(data);
//                $(".main-dialog-info").show();
//                if (data = "Data deleted successfully.") {
//                    $.ajax({
//                        type: 'GET',
//                        cache: false,
//                        url: '/ConfigurationData/indexDataEnrichmentSettings/',
//                        dataType: 'HTML',
//                        contentType: 'application/html',
//                        async: false,
//                        success: function (data) {
//                            $("#divPartialDataEnrichmentSettings").html(data);
//                        }
//                    });
//                }
//            },
//            error: function (xhr, ajaxOptions, thrownError) {
//            }
//        });
//        return true;
//    }
//    return false;
//});
// add DnbApi and make ajax call to open popup
$('body').on('click', '#btnAddAPI', function () {
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/ConfigurationData/popupDnbAPIGrp/'
        },
        callbacks: {
            close: function () {
                $.ajax({
                    type: 'GET',
                    cache: false,
                    url: '/ConfigurationData/indexDataEnrichmentSettings/',
                    dataType: 'HTML',
                    contentType: 'application/html',
                    async: false,
                    success: function (data) {
                        $("#divPartialDataEnrichmentSettings").html(data);
                    }
                });
            }

        },
        closeOnBgClick: false,
        mainClass: 'popAPIGrp'
    });
    return false;
});
// edit DnbApi and make ajax call to open popup
$('body').on('click', '.editApi', function () {
    var GroupId = this.id;
    if (GroupId != "") {
        $.magnificPopup.open({
            preloader: false,
            closeBtnInside: true,
            type: 'iframe',
            items: {
                src: "/ConfigurationData/popupDnbAPIGrp?Parameters=" + GroupId
            },
            callbacks: {
                close: function () {
                    $.ajax({
                        type: 'GET',
                        cache: false,
                        url: '/ConfigurationData/indexDataEnrichmentSettings/',
                        dataType: 'HTML',
                        contentType: 'application/html',
                        async: false,
                        success: function (data) {
                            $("#divPartialDataEnrichmentSettings").html(data);
                        }
                    });
                }
            },
            closeOnBgClick: false,
            mainClass: 'popAPIGrp'
        });
    }
});
// Reset password and make ajax call to open popup
$('body').on('click', '.ResetPassword', function () {
    var userId = $(this).attr("data-id");
    var EmailAddress = $(this).attr("data-email");
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/ConfigurationData/ResetPassword?UserId=' + userId + '&EmailAddress=' + EmailAddress
        },
        callbacks: {
            close: function () {
                //location.reload();
            }
        },
        closeOnBgClick: false,
        mainClass: 'popRestPassword'
    });
    return false;
});
// edit Custom Attribute and make ajax call to open popup
$('body').on('click', '.editCostomAttribute', function () {
    var AttributeId = this.id;
    if (AttributeId != "") {
        $.magnificPopup.open({
            preloader: false,
            closeBtnInside: true,
            type: 'iframe',
            callbacks: {
                close: function () {
                    $.ajax({
                        type: 'GET',
                        url: '/ConfigurationData/indexCompanyAttribute/',
                        dataType: 'HTML',
                        contentType: 'application/html',
                        async: false,
                        cache: false,
                        success: function (data) {
                            $("#divPartialCompanyAttribute").html(data);
                        }
                    });
                }
            },
            items: {
                src: "/ConfigurationData/popupConfigCustomAttribute?Parameters=" + AttributeId
            },
            closeOnBgClick: false,
            mainClass: 'popCustomAttibute',
        });
    }
    return false;
});
// add custom attribute and make ajax call to open popup
$('body').on('click', '#btnAddCUstomAttribute', function () {
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: "/ConfigurationData/popupConfigCustomAttribute"
        },
        callbacks: {
            close: function () {
                $.ajax({
                    type: 'GET',
                    url: '/ConfigurationData/indexCompanyAttribute/',
                    dataType: 'HTML',
                    contentType: 'application/html',
                    async: false,
                    cache: false,
                    success: function (data) {
                        $("#divPartialCompanyAttribute").html(data);
                    }
                });
            }
        },
        closeOnBgClick: false,
        mainClass: 'popCustomAttibute',
    });

    return false;
});
//Rest System data
$('body').on('click', '#btnResetSystemData', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to Reset Data ?", callback: function (result) {
            if (result) {
                $('#divProgress').show();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/ConfigurationData/ResetSystemData',
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "Json",
                    beforeSend: function () {
                    },
                    success: function (data) {
                        $('#divProgress').hide();
                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                            message: data,
                        });

                    }
                });
            }
        }
    });
    return false;
});
// Close Magnify popup
function backToparent() {
    $.magnificPopup.close();
}


// for the pagination event 
$("body").on('change', '.pagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#UserSortBy").val();
    var SortOrder = $("#UserSortOrder").val();
    var url = '/ConfigurationData/Index/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $("#divUser").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
$("body").on('change', '.CountrypagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    $("#CntryGrpPageValue").val(pagevalue);
    var SortBy = $("#CountrySortBy").val();
    var SortOrder = $("#CountrySortOrder").val();
    var url = '/ConfigurationData/indexCountryGrp' + "?Countrypagevalue=" + pagevalue + "&Countrysortby=" + SortBy + "&Countrysortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $("#divCountry").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function OnSuccess() {
    $('#divProgress').hide();
};

$("#form_ConfigData").submit(function () {
    if ($("#UserStatusCode").val() == "-1") {
        $("#spnUserStatusCode").show();
        return false;
    }
    else {
        return true;
    }
});

$("body").on('focus', '#EmailAddress', function () {
    $("#EmailAddress").attr('readonly', false);
});

$("body").on("click", ".deleteuser", function () {
    var pagevalue = $(".pagevalueChange").val();
    var SortBy = $("#UserSortBy").val();
    var SortOrder = $("#UserSortOrder").val();
    var url = '/ConfigurationData/Index/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    var Parameters = $(this).attr("id");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to delete this user ?", callback: function (result) {
            if (result) {
                $.post("/ConfigurationData/DeleteUser/", { Parameters: Parameters }, function (data) {
                    bootbox.alert({
                        title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                        message: data,
                    });
                    $.ajax({
                        type: "GET",
                        url: url,
                        dataType: "HTML",
                        contentType: "application/html",
                        cache: false,
                        data: {},
                        success: function (data) {
                            $("#divUser").html(data);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                });
            }
        }
    });
});

$("body").on("click", ".Activateuser", function () {
    var pagevalue = $(".pagevalueChange").val();
    var SortBy = $("#UserSortBy").val();
    var SortOrder = $("#UserSortOrder").val();
    var url = '/ConfigurationData/Index/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;

    var Parameters = $(this).attr("data-userid");
    $.ajaxSetup({ async: true });
    $("#divProgress").show();
    $.post("/ConfigurationData/Activateuser/", { Parameters: Parameters }, function (data) {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
            message: data,
        });
        $.ajax({
            type: "GET",
            url: url,
            dataType: "HTML",
            contentType: "application/html",
            data: {},
            cache: false,
            success: function (data) {
                $("#divUser").html(data);
                $("#divProgress").hide();
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    });
});

$('body').on('click', '.deleteAttribute', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to delete record ?", callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/ConfigurationData/DeleteCustomAttribute?Parameters=" + Parameters,
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    success: function (data) {
                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                            message: data, callback: function () {
                                if (data == "Data deleted successfully") {
                                    $.ajax({
                                        type: 'GET',
                                        url: '/ConfigurationData/indexCompanyAttribute/',
                                        dataType: 'HTML',
                                        cache: false,
                                        contentType: 'application/html',
                                        async: false,
                                        success: function (data) {
                                            $("#divPartialCompanyAttribute").html(data);
                                        }
                                    });
                                }
                            }
                        });
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

$('body').on('click', '.deleteCountryGroup', function () {
    var pagevalue = $("#CntryGrpPageValue").val();
    var SortBy = $("#CountrySortBy").val();
    var SortOrder = $("#CountrySortOrder").val();
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to delete record ?", callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/ConfigurationData/DeleteCountryGroup?Parameters=" + Parameters,
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                            message: data, callback: function () {
                                if (data == "Data deleted successfully") {
                                    $.ajax({
                                        type: 'GET',
                                        url: "/ConfigurationData/indexCountryGrp?Countrypagevalue=" + pagevalue + "&Countrysortby=" + SortBy + "&Countrysortorder=" + SortOrder,
                                        dataType: 'HTML',
                                        contentType: 'application/html',
                                        async: false,
                                        cache: false,
                                        success: function (data) {
                                            $("#divCountry").html(data);
                                        }
                                    });
                                }
                            }
                        });
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


$("body").on('click', '#btnAddManageTag', function () {
    var pagevalue = $("#ManageTagPageValue").val();
    var SortBy = $("#ManageTagSortBy").val();
    var SortOrder = $("#ManageTagSortOrder").val();
    $.magnificPopup.open({
        preloader: true,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/Data/AddTags/'
        },
        callbacks: {
            close: function () {
                $.ajax({
                    type: 'GET',
                    url: '/ConfigurationData/indexManageTags/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder,
                    dataType: 'HTML',
                    contentType: 'application/html',
                    async: false,
                    cache: false,
                    success: function (data) {
                        $("#divPartialManageTags").html(data);
                    }
                });
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupAddTags'
    });
});
$('body').on('click', '.deleteTag', function () {
    var pagevalue = $("#ManageTagPageValue").val();
    var SortBy = $("#ManageTagSortBy").val();
    var SortOrder = $("#ManageTagSortOrder").val();

    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to delete record ?", callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/ConfigurationData/DeleteTag?Parameters=" + Parameters,
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                            message: data, callback: function () {
                                if (data == "Data deleted successfully") {
                                    $.ajax({
                                        type: 'GET',
                                        url: '/ConfigurationData/indexManageTags/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder,
                                        dataType: 'HTML',
                                        contentType: 'application/html',
                                        async: false,
                                        cache: false,
                                        success: function (data) {
                                            $("#divPartialManageTags").html(data);
                                        }
                                    });
                                }
                            }
                        });
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

$("body").on('change', '.TagspagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    $("#ManageTagPageValue").val(pagevalue);
    var SortBy = $("#ManageTagSortBy").val();
    var SortOrder = $("#ManageTagSortOrder").val();
    var url = '/ConfigurationData/indexManageTags/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $("#divPartialManageTags").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function SetTagValue(OptionValue) {
    bootbox.alert({
        title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
        message: "Data created successfully.",
    });
    $.magnificPopup.close();
}


//Rest  System  data and configurations
$('body').on('click', '#btnResetSystemDataConfigurations', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to Reset Data ?", callback: function (result) {
            if (result) {
                $('#divProgress').show();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/ConfigurationData/ResetAllData',
                    data: '',
                    headers: { "__RequestVerificationToken": token },
                    dataType: "Json",
                    beforeSend: function () {
                    },
                    success: function (data) {
                        $('#divProgress').hide();
                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                            message: data,
                        });
                    }
                });
            }
        }
    });
    return false;
});

//export excel file of country group
$('body').on('click', '#btnExportToExcel', function () {
    $('<form action="/ConfigurationData/ExportToExcel"></form>').appendTo('body').submit();
});


//import excel file in country group
$('body').on('click', '#btnImportData', function () {
    var id = $(this).attr("id");
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/ConfigurationData/CountryImportData',
        },
        callbacks: {
            close: function () {
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupCompanyImportData'
    });
});
//Close Import panel
function CloseImportPanel() {
    $.magnificPopup.close();
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/ConfigurationData/CountryDataMatch/'
        },
        callbacks: {
            close: function () {
                $.ajax({
                    type: 'GET',
                    url: '/ConfigurationData/indexCountryGrp/',
                    dataType: 'HTML',
                    contentType: 'application/html',
                    async: false,
                    cache: false,
                    success: function (data) {
                        $("#divCountry").html(data);
                    }
                });
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupCompanyData'
    });
}

$('body').on('click', '#btnAddUserComments', function () {
    var pagevalue = $(".UserCommentspagevalueChange").val();
    var SortBy = $("#UserCommentsSortBy").val();
    var SortOrder = $("#UserCommentsSortOrder").val();
    var url = '/ConfigurationData/IndexUserComments/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: "/ConfigurationData/popupUserComments"
        },
        callbacks: {
            close: function () {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "HTML",
                    contentType: "application/html",
                    async: false,
                    cache:false,
                    success: function (data) {
                        $("#divPartialUserComments").html(data);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        },
        closeOnBgClick: false,
        mainClass: 'popupUserComments',
    });
    return false;
});


// for the pagination event 
$("body").on('change', '.UserCommentspagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#UserCommentsSortBy").val();
    var SortOrder = $("#UserCommentsSortOrder").val();
    var url = '/ConfigurationData/IndexUserComments/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache:false,
        success: function (data) {
            $("#divPartialUserComments").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});





// Delete CDS Environment 
$('body').on('click', '.deleteCDSEnvironment', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to delete record ?", callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/ConfigurationData/DeleteCDSEnvironment?Parameters=" + ConvertEncrypte(encodeURI(Parameters)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {

                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                            message: data, callback: function () {
                                if (data == "Data deleted successfully") {
                                    $.ajax({
                                        type: 'GET',
                                        cache: false,
                                        url: '/ConfigurationData/indexEnvironment/',
                                        dataType: 'HTML',
                                        contentType: 'application/html',
                                        async: false,
                                        success: function (data) {
                                            $("#divPartialCDSEnvironment").html(data);
                                        }
                                    });
                                }
                            }
                        });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});

$("body").on('click', '#btnAddEnvironment', function () {
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        height: '200px',
        items: {
            src: '/ConfigurationData/AddEnvironMent'
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: 'popupCDSEnvironment'
    });
});

function ReloadEnvironmentSection() {
    $.magnificPopup.close();
    $.ajax({
        type: 'GET',
        cache: false,
        url: '/ConfigurationData/indexEnvironment/',
        dataType: 'HTML',
        contentType: 'application/html',
        async: false,
        success: function (data) {

            $("#divPartialCDSEnvironment").html(data);
        }
    });
}
$("body").on('click', '#btnAddEntity', function () {
    $.magnificPopup.open({
        preloader: false,
        closeBtnInside: true,
        type: 'iframe',
        height: '200px',
        items: {
            src: '/ConfigurationData/AddEntity'
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: 'popupCDSEntity'
    });
});

function ReloadEntitySection() {
    $.magnificPopup.close();
    $.ajax({
        type: 'GET',
        cache: false,
        url: '/ConfigurationData/indexEntity/',
        dataType: 'HTML',
        contentType: 'application/html',
        async: false,
        success: function (data) {
            $("#divPartialCDSEntity").html(data);
        }
    });
}
// Delete CDS Environment 
$('body').on('click', '.deleteCDSEntity', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to delete record ?", callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/ConfigurationData/DeleteCDSEntity?Parameters=" + ConvertEncrypte(encodeURI(Parameters)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {

                        bootbox.alert({
                            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                            message: data, callback: function () {
                                if (data == "Data deleted successfully") {
                                    $.ajax({
                                        type: 'GET',
                                        cache: false,
                                        url: '/ConfigurationData/indexEntity/',
                                        dataType: 'HTML',
                                        contentType: 'application/html',
                                        async: false,
                                        success: function (data) {
                                            $("#divPartialCDSEntity").html(data);
                                        }
                                    });
                                }
                            }
                        });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});

// for the pagination event for Entity
$("body").on('change', '.CDSEntityPageValueChange', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#CDSEntitySortBy").val();
    var SortOrder = $("#CDSEntitySortOrder").val();
    var url = '/ConfigurationData/indexEntity/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divPartialCDSEntity").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
// for the pagination event for Environment
$("body").on('change', '.CDSEnvironmentPageValueChange', function () {

    var pagevalue = $(this)[0].value;
    var SortBy = $("#CDSEnvironmentSortBy").val();
    var SortOrder = $("#CDSEnvironmentSortOrder").val();
    var url = '/ConfigurationData/indexEnvironment/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divPartialCDSEnvironment").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
