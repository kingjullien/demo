function OnSuccessSearchBeneficialOwnershipData(data) {
    if (!data.result && data.result != undefined) {
        ShowMessageNotification("success", data.message, false);
    }
    else {
        $("#divSearchDataList").html(data);
        $('#divBeneficialOwnershipData').hide();
        $('#divSearchDataList').show();
    }
}

$(document).on('change', '.rBtnOwnerRelToggle', function () {
    if ($(".rBtnOwnerRelToggle").is(':checked')) {
        $(".simpleonlyowners").hide();
        $(".benificiaryGraphView").show();
        $('.lblOwnerOnly').removeClass("thColor");
        $('.lblwithRel').addClass("thColor");
        //$("#benificiaryExportToPDF").attr("href","/BeneficialOwnership/DownloadBenificiaryDataAsPDF?view=withrelation");
    }
    else {
        $(".benificiaryGraphView").hide();
        $(".simpleonlyowners").show();
        $('.lblwithRel').removeClass("thColor");
        $('.lblOwnerOnly').addClass("thColor");
        //$("#benificiaryExportToPDF").attr("href", "/BeneficialOwnership/DownloadBenificiaryDataAsPDF?view=owneronly");
    }
});

$(document).on("click", "#showgraphinFullScreen", function () {
    $.ajax({
        type: "GET",
        url: "/BeneficialOwnership/GraphViewPopup/",
        dataType: 'HTML',
        success: function (data) {
            $("#BenificiaryGraphModalMain").html(data);
            DraggableModalPopup("#BenificiaryGraphModal");
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$(document).on('show.bs.modal', '#BenificiaryGraphModal', function () {
    $(this).find('.modal-body').css({
        width: screen.availWidth - 140, 
        height: screen.availHeight - 140,
    });
});

$(document).on('click', '#btnSearchBeneficialOwnershipData', function () {
    var cnt = 0;
    var Company = $('#form_SearchBeneficialOwnershipData #txtCompany').val();
    var DUNS = $('#form_SearchBeneficialOwnershipData #DUNSNumber').val();
    var Country = $('#form_SearchBeneficialOwnershipData #Country').val();

    if (!Company && !DUNS) {
        if (Company) {
            $('#form_SearchBeneficialOwnershipData #spnCompany').hide();
        }
        else {
            $('#form_SearchBeneficialOwnershipData #spnCompany').show();
            cnt++;
        }

        if (DUNS) {
            $('#form_SearchBeneficialOwnershipData .spnerrDUNS').hide();
        }
        else {
            $('#form_SearchBeneficialOwnershipData .spnerrDUNS').show();
            cnt++;
        }
    } else {
        $('#form_SearchBeneficialOwnershipData #spnCompany').hide();
        $('#form_SearchBeneficialOwnershipData .spnerrDUNS').hide();
    }
    if (Country) {
        $('#form_SearchBeneficialOwnershipData #spnCountry').hide();
    }
    else {
        $('#form_SearchBeneficialOwnershipData #spnCountry').show();
        cnt++;
    }

    if (cnt > 0)
        return false;
})

$(document).on('click', '#benificiaryBackToList', function () {
    $('#divBeneficialOwnershipData').hide();
    $("#divBenificiarySearchform").show();
    $('#divSearchDataList').show();
});

$(document).on('click', '.refreshbenificiarytbl', function () {
    InitDataTable("#OwnerTable", [10, 20, 50], false, []);
});

$(document).on('click', '.viewBenificiaryDetails', function () {
    var Duns = $(this).attr("data-duns");
    var Country = $(this).attr("data-country");
    var formData = new FormData();
    formData.append('DUNSNumber', Duns);
    formData.append('Country', Country);
    formData.append('isModalView', $("#BenificiaryDataModalMain").length > 0 ? true : false);
    $.ajax({
        type: 'POST',
        url: "/BeneficialOwnership/SearchBeneficialOwnershipData",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            $("#divBeneficialOwnershipData").html(data);
            $("#divBenificiarySearchform").hide();
            $("#divSearchDataList").hide();
            $("#divBeneficialOwnershipData").show();
            InitComplianceRightClick();
        }
    });
})

$(document).on('click', '#benificiaryRefreshAlldata', function () {
    var Duns = $(this).attr("data-duns");
    var Country = $(this).attr("data-country");
    var formData = new FormData();
    formData.append('DUNSNumber', Duns);
    formData.append('Country', Country);
    formData.append('isModalView', $("#BenificiaryDataModalMain").length > 0 ? true : false);
    formData.append('isRefresh', true);
    $.ajax({
        type: 'POST',
        url: "/BeneficialOwnership/SearchBeneficialOwnershipData",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            $("#divBeneficialOwnershipData").html(data);
            $("#divBenificiarySearchform").hide();
            $("#divSearchDataList").hide();
            $("#divBeneficialOwnershipData").show();
            InitComplianceRightClick();
        }
    });
})

// open match detail review popup
$('body').on('click', '#divSearchDataList .clsViewMatchedItemDetails', function () {
    $(".currentRow").each(function () {
        $(this).removeClass("current");
    });
    $(this).parent().parent().parent().addClass("current");
    $(this).parent().parent().parent().next().addClass("current");
    //Vijay - New code to use data from parent tr
    var row = $(this).closest('tr');
    var next = row.nextAll().eq(1);
    var prev = row.prevAll().eq(1);

    var data = row.attr("data-val");
    var dataNext = "";
    var dataPrev = "";
    if (next.length > 0) {
        dataNext = next.attr("id");
    }

    if (prev.length > 0) {
        dataPrev = prev.attr("id");
    }

    var DUNS = row.attr("id");
    //Vijay - New Code ends here

    //var QueryString = 'id:' + DUNS + '@#$childButtonId:' + this.id + '@#$dataNext:' + dataNext + '@#$dataPrev:' + dataPrev + '@#$IsPartialView:false';
    var QueryString = "id:" + 0 + "@#$childButtonId:" + DUNS + "@#$dataNext:" + dataNext + "@#$dataPrev:" + dataPrev + "@#$count:" + this.id + '@#$type:null' + '@#$IsPartialView:false';

    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/SearchData/cShowMatchedItesDetailsView?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#MatchDetailModalMain").html(data);
            DraggableModalPopup("#MatchDetailModal");
        }
    });
});

$('body').on('click', '#divSearchDataList .Enrichment,#OwnerTable .Enrichment', function () {
    var DunsNumber = $(this).attr("data-dunsnumber");
    var SrcId = "";
    var Company = $("#txtCompany").val() != undefined ? $("#txtCompany").val().trim() : "";
    var Address = $("#txtAddress").val() ? $("#txtAddress").val() + " " + $("#txtAddress2").val() : $("#txtAddress2").val();
    var City = $("#txtCity").val();
    var State = $("#txtState").val();
    var Postal = $("#txtZip").val();
    var CountryCode = $("select#Country").val();
    var Phone = $("#txtPhone").val();
    var Country = $(this).attr("data-country");
    var RegNo = "";
    var Website = "";

    var QueryString = "DunsNumber:" + DunsNumber + "@#$Company:" + Company + "@#$Address:" + Address + "@#$City:" + City + "@#$State:" + State + "@#$Postal:" + Postal + "@#$CountryCode:" + CountryCode + "@#$SrcId:" + SrcId + "@#$Phone:" + Phone + "@#$Country:" + Country + "@#$RegNo:" + RegNo + "@#$Website:" + Website;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/StewardshipPortal/PreviewEnrichmentData?Parameters=" + Parameters,
        dataType: 'HTML',
        success: function (data) {
            $("#EnrichmentDetailModalMain").html(data);
            DraggableModalPopup("#EnrichmentDetailModal");
        }
    });
});

$(document).on('click', '#beneficiaryScreeningData', function () {
    $.ajax({
        type: "POST",
        url: "/BeneficialOwnership/SendDataForScreening",
        dataType: "json",
        cache: false,
        success: function (data) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
            ShowMessageNotification("success", data.message);
            if (data.result) {
                var Duns = $('#benificiaryRefreshAlldata').attr("data-duns");
                var Country = $('#benificiaryRefreshAlldata').attr("data-country");
                var formData = new FormData();
                formData.append('DUNSNumber', Duns);
                formData.append('Country', Country);
                formData.append('isModalView', $("#BenificiaryDataModalMain").length > 0 ? true : false);
                $.ajax({
                    type: 'POST',
                    url: "/BeneficialOwnership/SearchBeneficialOwnershipData",
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        $("#divBeneficialOwnershipData").html(data);
                        $("#divBenificiarySearchform").hide();
                        $("#divSearchDataList").hide();
                        $("#divBeneficialOwnershipData").show();
                        InitComplianceRightClick();
                    }
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
})

function InitComplianceRightClick() {
    var isAllowed = $("#IsScreeningAllowed").val();
    $.contextMenu({
        selector: '.context-menu-one-compliance',
        callback: function (key, options) {
        },
        items: {
            "ScreenOwnership": {
                name: screenOwnerShip, disabled: isAllowed.toLowerCase() == "true" ? false : true, title: screenOwnerShip, callback: function () {
                    var memberId = $(this).attr('data-memberId');
                    var QueryString = "memberId:" + memberId;
                    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                    $.ajax({
                        type: "POST",
                        url: "/BeneficialOwnership/SendDataForScreening?Parameters=" + Parameters,
                        dataType: "json",
                        cache: false,
                        success: function (data) {
                            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
                            ShowMessageNotification("success", data.message);
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });

                    return 'context-menu-icon context-menu-icon-quit';
                }
            }
        }
    });

    $('.context-menu-one-compliance').on('click', function (e) {
        return 'context-menu-icon context-menu-icon-quit';
    })
}

$(document).on('click', '.btnScreeningDetails', function () {
    var alternetId = $(this).attr("data-memberId");
    var benificialType = $(this).attr("data-type");
    var name = $(this).attr("data-name");
    var QueryString = "alternetId:" + alternetId + "@#$benificialType:" + benificialType + "@#$name:" + name; 
    $.ajax({
        type: "GET",
        url: "/BeneficialOwnership/GetScreenResponse?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: "HTML",        
        cache: false,
        success: function (data) {
            $("#BenificiaryScreeningDetailsModalMain").html(data);
            DraggableModalPopup("#BenificiaryScreeningDetailsModal");
            InitDataTable("#tblScreeningData", [5, 10, 20], false, []);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
})


