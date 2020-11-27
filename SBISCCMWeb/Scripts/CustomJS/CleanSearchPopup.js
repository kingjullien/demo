// Call method of page success when page post.
function ReloadSuccess() {
    $('#divProgress').hide();
}
// Open Context menu and display Add Company popup and Investigation value set.
$(function() {    
    var txtActivateFeature = $('#ActivateFeature').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var Investigation = $("#LicenseEnableInvestigations").val();
    var Compliance = $("#LicenseEnableCompliance").val();
    var APIType = $("#APITypeForInvestigation").val();
    if (Investigation == undefined || Investigation == "False") {
        Investigation = true;
    }

    if (Compliance == undefined || Compliance == "False") {
        Compliance = true;
    }
    else {
        Compliance = false;
    }

    if (Investigation == "True" && APIType != "DirectPlus") {
        Investigation = true;
    }
    $.contextMenu({
        selector: '.context-menu-one',
        callback: function(key, options) {
        },
        events: {
            show: function (opt) {
                setTimeout(function () {
                    opt.$menu.find('.context-menu-disabled > span').attr('title', txtActivateFeature);
                }, 50);
            }
        },
        items: {
            "AddCompany": {
                name: addMatchAsCompany, callback: function() {
                   
                    var matchRecord = $(this).attr("data-val");
                    var OriginalSrcRecId = $("#SrcRecId").val().trim();
                    var QueryString = "SrcRecId:" + SrcRecId;
                    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                    $.ajax({
                        type: "POST",
                        url: "/BadInputData/FillMatchData/",
                        data: JSON.stringify({ matchRecord: matchRecord }),
                        headers: { "__RequestVerificationToken": token },
                        dataType: "json",
                        contentType: "application/json",
                        success: function (data) {  
                                // Changes for Converting magnific popup to modal popup
                                $.ajax({
                                    type: 'GET',
                                    url: "/BadInputData/AddCompany?Parameters=" + Parameters,
                                    dataType: 'HTML',
                                    async: false,
                                    success: function (data) {
                                        $("#divProgress").hide();
                                        $("#SearchDataAddCompanyModalMain").html(data);
                                        DraggableModalPopup("#SearchDataAddCompanyModal");
                                    }
                                });
                        },
                        error: function(xhr, ajaxOptions, thrownError) {
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }
            },
            "AddInvestigation": {
                name: investigateRecord, disabled: Investigation, callback: function () {
                    var matchRecord = $(this).attr("data-val");
                    var dataObj = JSON.parse(matchRecord);
                    var InputId = $(this).attr("data-InputId");
                    var SrcId = $(this).attr("data-SrcId");
                    var Duns = $(this).attr("data-Duns");
                    var Tags = $(this).attr("data-Tags");
                    var Company = dataObj.DnBOrganizationName;
                    var Street = dataObj.DnBStreetAddressLine;
                    var City = dataObj.DnBPrimaryTownName;
                    var PostalCode = dataObj.DnBPostalCode;
                    var Country = dataObj.DnBCountryISOAlpha2Code;
                    var TradeStyle = dataObj.DnBTradeStyleName;
                    var Status = dataObj.DnBOperatingStatus;
                    var QueryString = "InputId:" + InputId + "@#$SrcId:" + SrcId + "@#$Duns:" + Duns + "@#$Tags:" + Tags + "@#$Company:" + Company + "@#$Street:" + Street + "@#$City:" + City + "@#$PostalCode:" + PostalCode + "@#$Country:" + Country + "@#$TradeStyle:" + TradeStyle + "@#$Status:" + Status;
                    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

                    // Changes for Converting magnific popup to modal popup
                    $.ajax({
                        type: 'GET',
                        url: "/ResearchInvestigation/iResearchInvestigationRecordsTargeted?Parameters=" + Parameters,
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#iResearchInvestigationRecordsTargetedModalMain").html(data);
                            DraggableModalPopup("#iResearchInvestigationRecordsTargetedModal");
                            var countSubTypes = $('#SubTypes').has('option').length;
                            if (countSubTypes == 0) {
                                $(".btnShowTargetedInvestigationMsg").show();
                            }
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }

            },
            "BenificiaryDetails": {
                name: benificiaryDetails, disabled: Compliance, callback: function () {
                    var matchRecord = $(this).attr("data-val");
                    var dataObj = JSON.parse(matchRecord);
                    var Duns = $(this).attr("data-Duns");
                    var Country = dataObj.DnBCountryISOAlpha2Code;
                    var formData = new FormData();
                    formData.append('DUNSNumber', Duns);
                    formData.append('Country', Country);
                    formData.append('isModalView', true);
                    // Changes for Converting magnific popup to modal popup
                    $.ajax({
                        type: 'POST',
                        url: "/BeneficialOwnership/SearchBeneficialOwnershipData",
                        data: formData,
                        //dataType: 'HTML',
                        contentType: false,
                        processData: false,
                        async: false,
                        success: function (data) {
                            $("#divBeneficialOwnershipData").html(data);
                            DraggableModalPopup("#BenificiaryDataModal");
                            InitComplianceRightClick();
                        }
                    });
                    return 'context-menu-icon context-menu-icon-quit';
                }
            }
        }
    });

    $('.context-menu-one').on('click', function(e) {
        return 'context-menu-icon context-menu-icon-quit';
    })
});
 
$('body').on('click', '.Enrichment', function () {
    var DunsNumber = $(this).attr("data-dunsnumber");
    var SrcId = $(this).attr("data-SrcId");
    var Company = $("#txtCompany").val().trim();
    var Address = $("#txtAddress").val() ? $("#txtAddress").val() + " " + $("#txtAddress2").val() : $("#txtAddress2").val();
    var City = $("#txtCity").val();
    var State = $("#txtState").val();
    var Postal = $("#txtZip").val();
    var CountryCode = $("select#Country").val();
    var Phone = $("#txtPhone").val();
    var Country = $(this).attr("data-country");
    var RegNo = $('#alt_registrationNum').attr("data-val");
    var Website = $('#alt_Website').attr("data-val");
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

function addHeight600Class(add) {
    if (add) {
        $(".popupiResearchInvestigationTargeted").addClass("height600");
    }
    else {
        $(".popupiResearchInvestigationTargeted").removeClass("height600");
    }

}