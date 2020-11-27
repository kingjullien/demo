//popup size with tag enable disabled
var FilterColumnsArray = [];
$.IsTagsLicenseAllow = $("#IsTagsLicenseAllow").val();
$.UserType = $("#UserType").val();


$.LicenseEnabledDNB = $("#LicenseEnabledDNB").val();
$.LicenseEnabledOrb = $("#LicenseEnabledOrb").val();
$.BrandingString = $("#BrandingString").val();

$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});

$(document).ready(function () {
    $("#CountryP option[value='US']").attr("selected", "selected");
    var language = $("#LanguageP").val();
    var country = $("#CountryP").val();
    if (language != undefined)
        changeLanguageP(country)
    /* DO NOT REMOVE : GLOBAL FUNCTIONS!
     *
     * pageSetUp(); WILL CALL THE FOLLOWING FUNCTIONS
     *
     * // activate tooltips
     * $("[rel=tooltip]").tooltip();
     *
     * // activate popovers
     * $("[rel=popover]").popover();
     *
     * // activate popovers with hover states
     * $("[rel=popover-hover]").popover({ trigger: "hover" });
     *
     * // activate inline charts
     * runAllCharts();
     *
     * // setup widgets
     * setup_widgets_desktop();
     *
     * // run form elements
     * runAllForms();
     *
     ********************************
     *
     * pageSetUp() is needed whenever you load a page.
     * It initializes and checks for all basic elements of the page
     * and makes rendering easier.
     *
     */

    pageSetUp();

    /*
     * ALL PAGE RELATED SCRIPTS CAN GO BELOW HERE
     * eg alert("my home function");
     *
     * var pagefunction = function() {
     *   ...
     * }
     * loadScript("~/Scripts/plugin/_PLUGIN_NAME_.js", pagefunction);
     *
     * TO LOAD A SCRIPT:
     * var pagefunction = function (){
     *  loadScript(".../plugin.js", run_after_loaded);
     * }
     *
     * OR
     *
     * loadScript(".../plugin.js", run_after_loaded);
     */
})
//Quick Search for DanB
$('body').on('click', '.QuickSearch', function (e) {
    $('#divProgress').show();
    var Company = $("#txtPCompany").val().trim();

    var Address = $("#txtPAddress").val().trim();
    var Address2 = $("#txtPAddress2").val().trim();
    var City = $("#txtPCity").val().trim();
    var State = $("#txtPState").val().trim();
    var Phone = $("#txtPPhone").val().trim();
    var Zip = $("#txtPZip").val().trim();
    var Country = $("select#CountryP").val().trim();
    var Language = $("select#LanguageP").val();
    if (Language == undefined) {
        Language = "";
    }
    else {
        Language = Language.trim();
    }
    var cnt = 0;

    if (!validateFileName(Company)) {
        $("#spnPCompany").html("Company can't contain any of the following characters \r\n \\ / : * \" < > |");
        $("#spnPCompany").show();
        cnt++;
    } else {
        $("#spnPCompany").hide();
    }
    if (!validateFileName(Address)) {
        $("#spnPAddress").html("Address can't contain any of the following characters \r\n \\ / : * \" < > |");
        $("#spnPAddress").show();
        cnt++;
    } else {
        $("#spnPAddress").hide();
    }

    if (!validateFileName(Address2)) {
        $("#spnPAddress2").html("Address can't contain any of the following characters \r\n \\ / : * \" < > |");
        $("#spnPAddress2").show();
        cnt++;
    } else {
        $("#spnPAddress2").hide();
    }
    if (!validateFileName(City)) {
        $("#spnPCity").html("City can't contain any of the following characters \r\n \\ / : * \" < > |");
        $("#spnPCity").show();
        cnt++;
    } else {
        $("#spnPCity").hide();
    }
    if (!validateFileName(State)) {
        $("#spnPState").html("State can't contain any of the following characters \r\n \\ / : * \" < > |");
        $("#spnPState").show();
        cnt++;
    } else {
        $("#spnPState").hide();
    }
    if (!validateFileName(Phone)) {
        $("#spnPPhone").html("Phone can't contain any of the following characters \r\n \\ / : * \" < > |");
        $("#spnPPhone").show();
        cnt++;
    } else {
        $("#spnPPhone").hide();
    }
    if (!validateFileName(Zip)) {
        $("#spnPZip").html("Zip can't contain any of the following characters \r\n \\ / : * \" < > |");
        $("#spnPZip").show();
        cnt++;
    } else {
        $("#spnPZip").hide();
    }
    if (Company == "") {
        $("#spnPCompany").show();
        cnt++;
    } else {
        $("#spnPCompany").hide();
    } if (Country == "") {
        $("#spnPCountry").show();
        cnt++;
    } else {
        $("#spnPCountry").hide();
    }


    if (cnt > 0) {
        $('#divProgress').hide();
        return false;
    }

    var token = $('input[name="__RequestVerificationToken"]').val();
    var form = $("<form action='/SearchData/Index' method='post' id='QuickSearchFrm'></form>");
    form.append('<input type="hidden" name="__RequestVerificationToken" value="' + token + '">');
    form.append('<input type="hidden" name="CompanyName" value="' + Company + '">');
    form.append('<input type="hidden" name="Address" value="' + Address + '">');
    form.append('<input type="hidden" name="Address2" value="' + Address2 + '">');
    form.append('<input type="hidden" name="City" value="' + City + '">');
    form.append('<input type="hidden" name="State" value="' + State + '">');
    form.append('<input type="hidden" name="PhoneNbr" value="' + Phone + '">');
    form.append('<input type="hidden" name="Zip" value="' + Zip + '">');
    form.append('<input type="hidden" name="Country" value="' + Country + '">');
    form.append('<input type="hidden" name="Language" value="' + Language + '">');
    $('body').append(form);
    $("#QuickSearchFrm").submit();
});
$(document).ready(function () {
    $($('.mainNav ul')[0]).append($('.liCircleUserText'));
    //$($('.mainNav ul')[0]).append($('.alert-info'))
    $('#divProgress').hide();
    $("#divSmallBoxes").append();

    GetETLJobStatus();
    GetActiveUserData();
    var ExportNotificationTimeInterval = $("#ExportNotificationTimeInterval").val();

    var timeinterval = ExportNotificationTimeInterval * 60 * 1000;

    setInterval(function () {
        GetETLJobStatus();
        GetActiveUserData();
    }, 50000);

    if ($.BrandingString == "DandB" && $.LicenseEnabledDNB.toLowerCase() == "true") {
        setInterval(function () {
            GetNotification();
        }, timeinterval);
    }

    if ($.BrandingString != "DandB" && $.LicenseEnabledOrb.toLowerCase() == "true") {
        setInterval(function () {
            GetOIExportNotification();
        }, timeinterval);
    }

    //Rebind authorization token of ux default Credentials after every hour
    setInterval(function () {
        var authTokensInterval = localStorage.getItem("AuthTokensInterval");
        if (authTokensInterval == null) {
            localStorage.setItem("AuthTokensInterval", 1);
        }
        else {
            authTokensInterval = parseInt(authTokensInterval);
            if (authTokensInterval < 59) {
                localStorage.setItem("AuthTokensInterval", authTokensInterval + 1);
            }
            else {
                localStorage.setItem("AuthTokensInterval", 0);
                RebindUXAuthTokens();
            }
        }
    }, 60000);

});

$('#ComplianceReset').click(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var ComplianceReset = $('input#ComplianceReset').is(':checked');
    $.ajax({
        type: "POST",
        //url: "/Home/StoreComplianceValue/",
        url: "/Home/StoreComplianceValue?ComplianceReset=" + ComplianceReset,
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        global: false,
        success: function (data) {
            //ShowMessageNotification("success", data, true);
        },
        error: function (xhr, ajaxOptions, thrownError) { }
    });
});
function RebindUXAuthTokens() {
    $.ajax({
        type: "POST",
        url: "/Home/RebindUXAuthTokens/",
        dataType: "json",
        contentType: "application/json",
        global: false,
        success: function (data) { },
        error: function (xhr, ajaxOptions, thrownError) { }
    });
}
function GetNotification() {
    $.ajax({
        type: "POST",
        url: "/ExportView/ExportJobNotification/",
        dataType: "json",
        contentType: "application/json",
        global: false,
        success: function (data) {
            if (data != undefined && data != "") {
                GetMultipleNotifications(data, "<a href='/ExportView/Index' class='clsNotifiaction' style='color:#ffffff'> Click here</a>");
            } else {
                $("#divSmallBoxes").hide();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });

} function GetOIExportNotification() {
    $.ajax({
        type: "POST",
        url: "/OIExportView/ExportOIJobNotification/",
        dataType: "json",
        contentType: "application/json",
        global: false,
        success: function (data) {
            if (data != undefined && data != "") {
                GetMultipleNotifications(data, "<a href='/OI/ExportView' class='clsNotifiaction' style='color:#ffffff'> Click here</a>");
            } else {
                $("#divSmallBoxes").hide();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });

}
$(document).on('click', '.btnNotificationClose', function () {
    $(this).parent().parent().remove();
});
function GetMultipleNotifications(data, lnk) {
    var TopHeight = 10;
    $("#MySmallBox .SmallBox").each(function () {
        TopHeight += $(this).height() + 10;
    });
    var length = $("#MySmallBox").children().length;
    var mySmallboxId = "mySmallbox" + length;
    var srtSmallBoxDiv = '<div class="SmallBox animated fadeInRight fast" id="' + mySmallboxId + '" style="background-color: #1E90FF;top:' + TopHeight + 'px">' +
        '<div class="foto">' +
        '<i class="fa fa-bell swing animated"></i>' +
        '</div> ' +
        '<div class="textoFoto"> ' +
        '<span > Notification</span> ' +
        '<p > <div class="notificationText">' + data + lnk + '</div></p> ' +
        '</div> ' +
        '<div class="miniIcono"> ' +
        '<i class="btnNotificationClose fa fa-times" id = "botClose3" ></i> ' +
        '</div> ' +
        '</div>';
    $("#MySmallBox").append(srtSmallBoxDiv);
    setTimeout(function () {
        $("#" + mySmallboxId).remove();
    }, 5000);
}
function GetETLJobStatus() {
    $('#divProgress').hide();
    $.ajax({
        type: "POST",
        url: "/Home/GetETLJobStatus/",
        data: '',
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            $(".backgroundProcess").html(data.Data1.replace(/(?:\\[rn]|[\r\n]+)+/g, "<br /><br />").replace("\"", "").replace("\"", ""));
            if (data.Data2.trim().toLowerCase() === "successful") {
                $(".bgProcess").css({ "backgroundColor": "#6fd64b" });

            }
            else if (data.Data2.trim().toLowerCase() === "failed") {
                $(".bgProcess").css({ "backgroundColor": "#fe642e" });
                $(".backgroundProcess-new").css("height", "300");
                $(".bgProcess").css("height", "300");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    $('#divProgress').hide();
}
function GetActiveUserData() {
    $('#divProgress').hide();
    $.ajax({
        type: "POST",
        url: "/Home/GetActiveUserData/",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".ActiveUser").html(activeUsers + " <br /> " + data.length);
            $(".userdatacount-new").find('tr').remove();
            for (var i = 0; i < data.length; i++) {
                if (data[i].Image_path == null || data[i].Image_path == "") {
                    $(".userdatacount-new").append("<tr class='margin-3'><td class='captchaimgpadding'><img class='captchaimg' alt='Image' src='/Images/no-image.jpg'></td><td>" + data[i].UserName + "</td></tr>");
                }
                else {

                    var IsExist = UrlExists(data[i].Image_path);
                    if (IsExist) {
                        var imagepath = data[i].Image_path;
                        $(".userdatacount-new").append("<tr class='margin-3'><td class='captchaimgpadding'><img class='captchaimg' alt='Image' src='" + data[i].Image_path + "'></td><td>" + data[i].UserName + "</td></tr>");
                    } else {
                        $(".userdatacount-new").append("<tr class='margin-3'><td class='captchaimgpadding'><img class='captchaimg' alt='Image' src='/Images/no-image.jpg'></td><td>" + data[i].UserName + "</td></tr>");
                    }

                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    $('#divProgress').hide();
}
$('.process-popup').on('mouseenter mouseleave', function (e) {
    $(".bgProcess").show("slide", {
        direction: "left"
    }, 500);

});
$('.bgProcess').on('mouseleave', function (e) {
    $(".bgProcess").hide("slide", {
        direction: "left"
    }, 500);
});
/* active user*/
$('.ActiveUser').on('mouseenter mouseleave', function (e) {
    $(".userdatacount-new").show("slide", {
        direction: "left"
    }, 500);

});
$('.userdatacount-new').on('mouseleave', function (e) {
    $(".userdatacount-new").hide("slide", { direction: "left" }, 500);

});
$('.well').on('mouseenter mouseleave ', function (e) {
    $(".userdatacount-new").hide("slide", { direction: "left" }, 500);
    $(".bgProcess").hide("slide", { direction: "left" }, 500);

});
jQuery(document).ready(function (e) {
    var tt_exp = "1"; //set the exp time in min
    tt_exp = (parseInt(tt_exp) * 1800000); //set the time in microsecond 1800000

    //for timeout action even ajax call too.
    jQuery(document).bind("idle.idleTimer", function () {
        var SAMLSSOLogout = $("#SAMLSSOLogout").val();
        if (SAMLSSOLogout.toLowerCase() == "true") {
            window.location = "/Account/SAMLLogOff";
        }
        if (SAMLSSOLogout.toLowerCase() == "false") {
            window.location = "/Account/LogOff";
        }
    });
    //for the session is alive
    jQuery(document).bind("active.idleTimer", function () {


    });

    jQuery.idleTimer(tt_exp);


});
function validateFileName(fileName) {
    if (fileName.indexOf("\\") > -1 || fileName.indexOf("/") > -1 || fileName.indexOf(":") > -1 || fileName.indexOf("\"") > -1 || fileName.indexOf("<") > -1 || fileName.indexOf(">") > -1 || fileName.indexOf("?") > -1 || fileName.indexOf("|") > -1 || fileName.indexOf("*") > -1) { return false; } else {
        return true;
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
function UrlExists(url) {

    var http = new XMLHttpRequest();
    http.open('HEAD', url, false);
    http.send();
    return http.status != 404;
}
$('#FeedbackTypePos').click(function () {
    $("#FeedbackTypePos").addClass("iconClicked");
    $("#FeedbackTypeNeg").removeClass("iconClicked");
    var FeedbackType = $(this).attr("data-val");
    $("#FeedbackTypes").val(FeedbackType);
});

$('#FeedbackTypeNeg').click(function () {
    $("#FeedbackTypeNeg").addClass("iconClicked");
    $("#FeedbackTypePos").removeClass("iconClicked");
    var FeedbackType = $(this).attr("data-val");
    $("#FeedbackTypes").val(FeedbackType);
});
$('#btnSubmitFeedback').click(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var FeedbackType = $("#FeedbackTypes").val();
    var Feedback = $("#FeedbackTextBox").val();
    var FeedbackPath = $("#FeedbackPath").val();
    var cnt = 0;
    if (FeedbackType == "") {
        $("#spnFeedbackType").show();
        cnt++;
    }
    else {
        $("#spnFeedbackType").hide();
    }
    if (Feedback == "") {
        $("#spnFeedbackMessage").show();
        cnt++;
    }
    else {
        $("#spnFeedbackMessage").hide();
    }
    if (cnt > 0) {
        return false;
    }
    var QueryString = "FeedbackType:" + FeedbackType + "@#$Feedback:" + Feedback + "@#$FeedbackPath:" + FeedbackPath;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: 'POST',
        url: "/Feedback/AddFeedback?Parameters=" + Parameters,
        headers: { "__RequestVerificationToken": token },
        dataType: 'JSON',
        cache: false,
        contentType: 'application/json',
        success: function (data) {
            if (data != null) {
                parent.ShowMessageNotification("success", data, false);
                $(".feedbackBox").hide();
                $("#FeedbackTypePos").removeClass("iconClicked");
                $("#FeedbackTypeNeg").removeClass("iconClicked");
                $("#FeedbackTextBox").val("");
                $("#FeedbackTypes").val("");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function ReloadPage() {
    location.reload();
}
//REfresh D&B Dashboard
$(document).on('click', '.btnRefresh', function () {
    var buttonName = $("#ActiveStatisticsFilter").text().trim();
    var url = "/Home/Index";
    var LOBTags = "";
    var Tags = "";
    if (buttonName == "Filter Applied") {
        LOBTags = $("#LOBTag").val();
        Tags = $("#ActiveStatisticsFilterTags").val();
        var QueryString = "isMainRefresh:true" + "@#$Tags:" + Tags.split("::").join("&#58&#58") + "@#$LOBTags:" + LOBTags.replace("::", "&#58&#58");
        url = "/Home/Index?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    }
    $.ajax({
        type: "GET",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        success: function (data) {
            $("#divPartialIndex").html(data);
            var isMainRefresh = $("#isMainRefresh").val();
            LoadOnReady();
            if (buttonName == "Filter Applied") {
                $("#ActiveStatisticsFilter").html("Filter Applied <i class='fa fa-caret-down'></i>");
                Tags = $("#ActiveStatisticsFilterTags").val();
                LOBTags = $("#LOBTag").val();
                $("#ActiveStatisticsFilter").attr("data-content", "LOB Tag : " + LOBTags + "<br>Tags : " + Tags.replace("[*]", "No Tag"));
                $(".btnFiltered").removeAttr("hidden");
            }
            pageSetUp();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });

});
function loadScript(url, callback) {
    var script = document.createElement("script")
    script.type = "text/javascript";
    if (script.readyState) {  //IE
        script.onreadystatechange = function () {
            if (script.readyState == "loaded" ||
                script.readyState == "complete") {
                script.onreadystatechange = null;
                callback();
            }
        };
    } else {  //Others
        script.onload = function () {
            callback();
        };
    }
    script.src = url;
    document.getElementsByTagName("head")[0].appendChild(script);
}
$('body').on('change', '#CountryP', function () {
    var language = $("#LanguageP").val();
    var country = $(this).val();
    if (language != undefined)
        changeLanguageP(country)
});
function changeLanguageP(country) {
    $("#LanguageP").find("option").remove();
    $("#LanguageP").append('<option value=>Select Language</option>');
    if (country.toLowerCase() == "us") {
        $("#LanguageP").append('<option value=en-US>English</option>');
        $("#LanguageP").val("en-US");
    }
    else if (country.toLowerCase() == "jp") {
        $("#LanguageP").append('<option value=en-US>English</option>');
        $("#LanguageP").append('<option value=ja-JP>Japanese</option>');
        $("#LanguageP").val("ja-JP");
    }
    else if (country.toLowerCase() == "tw") {
        $("#LanguageP").append('<option value=en-US>English</option>');
        $("#LanguageP").append('<option value=zh-hant-TW>Traditional Chinese</option>');
        $("#LanguageP").val("zh-hant-TW");
    }
    else if (country.toLowerCase() == "kr") {
        $("#LanguageP").append('<option value=en-US>English</option>');
        $("#LanguageP").append('<option value=ko-hang-KR>Hangul</option>');
        $("#LanguageP").val("ko-hang-KR");
    }
    else if (country.toLowerCase() == "cn") {
        $("#LanguageP").append('<option value=en-US>English</option>');
        $("#LanguageP").append('<option value=zh-hans-CN>Simplified Chinese</option>');
        $("#LanguageP").val("zh-hans-CN");
    }
    else {
        $("#LanguageP").append('<option value=en-US>English</option>');
        $("#LanguageP").append('<option value=ja-JP>Japanese</option>');
        $("#LanguageP").append('<option value=zh-hans-CN>Simplified Chinese</option>');
        $("#LanguageP").append('<option value=zh-hant-TW>Traditional Chinese</option>');
        $("#LanguageP").append('<option value=ko-hang-KR>Hangul</option>');
        $("#LanguageP").val("en-US");
    }
}

function btnNotificationClose(randomID) {
    $("#" + randomID).remove();
}

$('body').on('click', '.DismissNotification', function () {
    var MessageId = $(this).attr("id");
    $.ajax({
        type: "POST",
        url: "/Notification/DismissNotificationById?MessageId=" + MessageId,
        dataType: 'json',
        contentType: "application/json",
        processData: false,
        async: false,
        success: function (response) {
            if (response == success) {
                $("#NotificationDiv .Notification-Containt li").each(function () {
                    if ($(this).attr("data-notificationid") == MessageId) {
                        $(this).remove();
                    }
                });
            }
        },
        error: function (xhr, status, error) {
        }
    });
});


$('body').on('click', '.ClsDismissNotification', function () {
    var MessageId = $(this).attr("id");
    $.ajax({
        type: "POST",
        url: "Notification/DismissNotificationById?MessageId=" + MessageId,
        dataType: 'json',
        contentType: "application/json",
        processData: false,
        async: false,
        success: function (response) {
            if (response == success) {
                bootbox.alert({
                    title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
                    message: notificationDismissed,
                    callback: function () {
                    }
                });
                $(".ClsDismissNotification").each(function () {
                    if ($(this).attr("id") == MessageId) {
                        $(this).remove();
                    }
                });

                $("#NotificationDiv .Notification-Containt li").each(function () {
                    if ($(this).attr("data-notificationid") == MessageId) {
                        $(this).remove();
                    }
                });
            }
        },
        error: function (xhr, status, error) {
        }
    });
});
//use for location reload at callback in ShowMessageNotification
function LocationReload() {
    location.reload();
}

//Use for Show Notification.
//show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
function ShowMessageNotification(MessageType, Message, isPopup, IsCallBack, FunctionName) {
    //check page is popup page or not if popup page than close popup page and show notification
    if (isPopup) {
        //window.parent.$.magnificPopup.close();
    }
    if (MessageType == "success") {
        $.notify({
            message: Message
        }, {
            //type: "warning",
            newest_on_top: true,
            z_index: 99999,
            delay: 10000,
        });

        //this is callback function.
        if (IsCallBack) {
            FunctionName();
        }
    }
    else {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
            message: Message, callback: function () {
                //this is callback function.
                if (IsCallBack) {
                    FunctionName();
                }
            }
        });
    }

}
//refresh OI dashboard
$(document).on('click', '.btnOIDashboardRefresh', function () {
    ReFreshOIActiveStatisticsFilter();
    BackgroundProcessStatisticsReport();
    ReFreshOIAPIUsagestatistics();
});

//Quick Search for OI
$('body').on('click', '.OIQuickSearch', function (e) {
    $('#divProgress').show();
    var Company = $("#txtPCompany").val().trim();
    var Address = $("#txtPAddress").val().trim();
    var Address2 = $("#txtPAddress2").val().trim();
    var City = $("#txtPCity").val().trim();
    var State = $("#txtPState").val().trim();
    var Phone = $("#txtPPhone").val().trim();
    var Zip = $("#txtPZip").val().trim();
    var Country = $("select#CountryP").val().trim();
    var cnt = 0;

    if (Company == '') {
        $("#spnPCompany").show();
        cnt++;
    }
    else {
        $("#spnPCompany").hide();
    }
    if (Country == '') {
        $("#spnPCountry").show();
        cnt++;
    }
    else {
        $("#spnPCountry").hide();
    }

    if (cnt > 0) {
        $('#divProgress').hide();
        return false;
    }
    var token = $('input[name="__RequestVerificationToken"]').val();
    var form = $("<form action='/OI/Searchdata' method='post' id='OIQuickSearchFrm'></form>");
    form.append('<input type="hidden" name="__RequestVerificationToken" value="' + token + '">');
    form.append('<input type="hidden" name="CompanyName" value="' + Company + '">');
    form.append('<input type="hidden" name="Address1" value="' + Address + '">');
    form.append('<input type="hidden" name="Address2" value="' + Address2 + '">');
    form.append('<input type="hidden" name="City" value="' + City + '">');
    form.append('<input type="hidden" name="State" value="' + State + '">');
    form.append('<input type="hidden" name="Country" value="' + Country + '">');
    form.append('<input type="hidden" name="Zipcode" value="' + Zip + '">');
    form.append('<input type="hidden" name="Telephone" value="' + Phone + '">');
    $('body').append(form);
    $("#OIQuickSearchFrm").submit();
});


//For redirecting to support ticket
function RedirectToCreateTicket() {
    $.magnificPopup.close();
    window.location.href = window.location.origin + "/Ticket/Create";
}

$('body').on('change', '#CountryP', function () {
    var country = $(this).val();
    var APItype = $("#APItype").val();
    if (APItype == "DirectPlus") {
        LoadTypeAheadQuickSearch();
    }
});

// TYPEAHEAD FUNCTIONALITY IN SEARCH DATA
function ShowLoadingImageQuickSearch() {
    $("#imgCompanyLoadForQuickSearch").show();
}

function HideLoadingImageQuickSearch() {
    $("#imgCompanyLoadForQuickSearch").hide();
}
function LoadTypeAheadQuickSearch() {
    if ($(".QuickSearchBox .QTypeAheadToggle").is(':checked')) {
        var defaultCountryCode = $('#CountryP').val();
        var listOfCompany = [];
        var dataSrc = function (request, response) {
            listOfCompany = [];
            ShowLoadingImageQuickSearch();
            setTimeout(function () {
                $.ajax({
                    type: "GET",
                    url: "/SearchData/SearchDataCompanyNameTypeAhead",
                    data: { "paramater": request.term, "defaultCountryCode": defaultCountryCode },
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        HideLoadingImageQuickSearch();
                        var JSONResponse = JSON.parse(data);
                        if (JSONResponse.error != undefined && JSONResponse.error.errorMessage != null) {
                            //ShowMessageNotification("success", JSONResponse.error.errorMessage, true);
                            return false;
                        }
                        else {
                            response($.map(JSONResponse.searchCandidates, function (value, key) {
                                return {
                                    label: value.organization.primaryName + (value.organization.primaryAddress.streetAddress != undefined ? ', ' + value.organization.primaryAddress.streetAddress.line1 : '') + (value.organization.primaryAddress.addressLocality != undefined ? ", " + value.organization.primaryAddress.addressLocality.name : ''),
                                    value: value.organization.primaryName,
                                    duns: value.organization.duns,
                                    address: (value.organization.primaryAddress.streetAddress != undefined ? ',  ' + value.organization.primaryAddress.streetAddress.line1 : '') + (value.organization.primaryAddress.addressLocality != undefined ? ",  " + value.organization.primaryAddress.addressLocality.name : ''),
                                    address1: (value.organization.primaryAddress.streetAddress != undefined ? value.organization.primaryAddress.streetAddress.line1 : ''),
                                    city: value.organization.primaryAddress.addressLocality != undefined ? value.organization.primaryAddress.addressLocality.name : '',
                                    state: value.organization.primaryAddress.addressRegion != undefined ? value.organization.primaryAddress.addressRegion.name : '',
                                    country: value.organization.primaryAddress.addressCountry.isoAlpha2Code != undefined ? value.organization.primaryAddress.addressCountry.isoAlpha2Code : '',
                                    fullDetail: value
                                };
                            }));
                        }
                    }
                });
            }, 1000);
        };

        $('.ui-front').addClass("quickSearchIcon");

        $("#txtPCompany").autocomplete({
            source: dataSrc,
            minLength: 3,
            select: function (event, ui) {
                FinalResponse = JSON.stringify(ui.item);
                $("#txtPCompany").val(ui.item.value);
                $("#txtPAddress").val(ui.item.address1);
                $("#txtPCity").val(ui.item.city);
                $("#txtPState").val(ui.item.state);
                $("#CountryP").val(ui.item.country);
                $('.QuickSearchBox').focus();
                checkQuickSearchBox();
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
    }
}

$("#txtPCompany").keypress(function () {
    var APItype = $("#APItype").val();
    if (APItype == "DirectPlus") {
        LoadTypeAheadQuickSearch();
    }
});
function checkQuickSearchBox() {
    $('.QuickSearchNew').addClass("displayBlock");
}
var cnt = 0;
$('body').on('click', '.QuickSearchBox', function () {
    if (cnt == 0) {
        $('.QuickSearchNew').addClass("displayBlock");
        cnt++;
    }
    else {
        $('.QuickSearchNew').removeClass("displayBlock");
    }
});
$('body').on('click', '.SendFeedbackBox', function () {
    $('.feedbackBox').show();
});



//Custom Filter
function InitFilters(arrColumnNames, FilterDataurl, FilterParentDiv, updateTargetId, dataTableId, Availoperators = "all") {
    FilterColumnsArray = [];
    if (!($(FilterParentDiv + ' .FilterContainer').children().not('.divFilterAddButton').length > 1)) {
        $(FilterParentDiv + " #FilterDataurl").val(FilterDataurl);
        $(FilterParentDiv + " #updateTargetId").val(updateTargetId);
        $(FilterParentDiv + " #DataTableId").val(dataTableId);
        $(FilterParentDiv + " #AvailOperators").val(Availoperators);
        var FilterId = 0;
        for (var i = 0; i < arrColumnNames.length; i++) {
            FilterColumnsArray.push(arrColumnNames[i]);
            $(FilterParentDiv + ' #filterColumn' + FilterId).append($("<option></option>").attr("value", arrColumnNames[i][0]).text(arrColumnNames[i][0]));
            if (i == FilterId) {
                $(FilterParentDiv + " #divFilterValue" + FilterId).append('<div id="divms' + (arrColumnNames[i][0] + FilterId) + '"><select multiple="true" data-detail="value" class="filterValue" name="filterVal' + (arrColumnNames[i][0] + FilterId) + '" id="filterVal' + (arrColumnNames[i][0] + FilterId) + '"></select></div>');
                $(FilterParentDiv + " #divFilterValue" + FilterId).append('<input class="filterValueText" type="text" name="filterVal' + (arrColumnNames[i][0] + FilterId) + '" id="filterVal' + (arrColumnNames[i][0] + FilterId) + '" style="display:none;" />');
            }
            else {
                $(FilterParentDiv + " #divFilterValue" + FilterId).append('<div id="divms' + (arrColumnNames[i][0] + FilterId) + '" style="display:none;"><select multiple="true" data-detail="value" class="filterValue" name="filterVal' + (arrColumnNames[i][0] + FilterId) + '" id="filterVal' + (arrColumnNames[i][0] + FilterId) + '" style="display:none;"></select></div>');
                $(FilterParentDiv + " #divFilterValue" + FilterId).append('<input class="filterValueText" type="text" name="filterVal' + (arrColumnNames[i][0] + FilterId) + '" id="filterVal' + (arrColumnNames[i][0] + FilterId) + '" style="display:none;" />');
            }

            //Remove Operator as per selection
            if (Availoperators != "all") {
                $(FilterParentDiv + " #filterOperator" + FilterId + " > option").each(function () {
                    if (this.value != Availoperators) {
                        this.remove();
                    }
                });
            }

            //Bind Value dropdown
            if (arrColumnNames[i].length > 2 && arrColumnNames[i][2] != "") {
                $.ajax({
                    type: "GET",
                    url: arrColumnNames[i][2],
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        //Select default value if provided
                        var DefaultSelectedVal = [];
                        if (arrColumnNames[i].length > 6) {
                            DefaultSelectedVal = arrColumnNames[i][6].split(',');
                        }
                        for (var j = 0; j < data.Data.length; j++) {
                            var checkIfFilterExist = DefaultSelectedVal.findIndex(x => x === data.Data[j].Value);
                            $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).append($("<option></option>").attr("value", data.Data[j].Value).text(data.Data[j].Text));
                            if (checkIfFilterExist > -1 && data.Data[j].Value != "") {
                                $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId) + " option[value='" + data.Data[j].Value + "']").attr("selected", true);
                            }
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }

            //If default filter then Remove close button and all the other options of oprator and show that filter div
            if (arrColumnNames[i].length > 3) {
                //For default filter
                if (arrColumnNames[i][3].split(':')[0] == "true") {
                    $(FilterParentDiv + " #filter" + FilterId).attr("data-default", "true");
                    $(FilterParentDiv + " #filterOperator" + FilterId + " > option").each(function () {
                        if (this.value != "equalto") {
                            this.remove();
                        }
                    });
                    $(FilterParentDiv + " #filter" + FilterId).show();
                    $(FilterParentDiv + " #divFilterInfo" + FilterId + " .close_button").hide();

                    //Conditions for default filter like multiselect,onlytext,onlyselect
                    if (arrColumnNames[i][3].split(':').length > 1 && arrColumnNames[i][3].split(':')[1] == "true") {
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect({
                            includeSelectAllOption: true,
                            nonSelectedText: 'Select',
                            maxHeight: 200,
                            numberDisplayed: 1,
                            selectAllValue: "all",
                            buttonClass: 'filterDDbtn',
                            enableCaseInsensitiveFiltering: true,
                            onDropdownHidden: function (event) {
                                if (event.clickEvent && (event.clickEvent.target.className == "filterColumn" || event.clickEvent.target.className == "filterOprator")) {
                                    event.preventDefault();
                                }
                                else {
                                    DoFiltering(FilterParentDiv);
                                }

                            }
                        });

                        //Select All options if no default value provided
                        if (arrColumnNames[i].length < 6 || (arrColumnNames[i].length > 6 && (arrColumnNames[i][6] == "" || arrColumnNames[i][6] == null || arrColumnNames[i][6] == undefined))) {
                            $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect('selectAll', false);
                            $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect('updateButtonText');
                        }
                    }
                    else if (arrColumnNames[i].length > 5 && arrColumnNames[i][5] == "onlytext") {
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).attr("data-Isdate", "onlytext");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).removeAttr("multiple");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).hide();
                        $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).show();

                        if (arrColumnNames[i].length > 6)
                            $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).val(arrColumnNames[i][6]);
                    }
                    else if (arrColumnNames[i].length > 5 && arrColumnNames[i][5] == "onlynumber") {
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).attr("data-Isdate", "onlynumber");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).removeAttr("multiple");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).hide();
                        $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).show();
                        $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).attr("onkeypress", "return isNumber(event)");

                        if (arrColumnNames[i].length > 6)
                            $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).val(arrColumnNames[i][6]);
                    }
                    else {
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).removeAttr("multiple");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect({
                            maxHeight: 200,
                            buttonClass: 'filterDDbtn',
                            enableCaseInsensitiveFiltering: true,
                            onChange: function (option, checked, select) {
                                //DoFiltering(FilterParentDiv);
                            },
                            onDropdownHidden: function (event) {
                                if (event.clickEvent && (event.clickEvent.target.className == "filterColumn" || event.clickEvent.target.className == "filterOprator")) {
                                    event.preventDefault();
                                }
                                else {
                                    DoFiltering(FilterParentDiv);
                                }
                            }
                        });
                    }


                    var newFilterId = parseInt(FilterId) + 1;
                    $(FilterParentDiv + " .divFilterAddButton").remove();

                    $(FilterParentDiv + ' .FilterContainer').append('<div class="filtergroup filter' + newFilterId + '" id="filter' + newFilterId + '" filter-id="' + newFilterId + '" data-default="false"><div class="filterControls" id="divfilterControls' + newFilterId + '"><div class="custom-dropdown-filter"><select name="filterColumn' + newFilterId + '" id="filterColumn' + newFilterId + '" class="filterColumn"></select></div><div class="custom-dropdown-filter"><select name="filterOperator' + newFilterId + '" id="filterOperator' + newFilterId + '" class="filterOprator"><option value="equalto">==</option><option value="notEqualTo">!=</option><option value="contains">Contains</option><option value="notContains">!Contains</option></select></div><div class="" id="divFilterValue' + newFilterId + '"></div></div><div class="filterDetails" id="divFilterInfo' + newFilterId + '" style="display:none"><span id="ColumnDetail' + newFilterId + '"></span><span id="OperatorDetail' + newFilterId + '"></span><span data-detail="value" id="ValueDetail' + newFilterId + '"></span><span class="close_button"><a href="javascript:void(0);" class="removeFilter" id="' + newFilterId + '"><i class="fa fa-times-circle"></i></a></span></div></div>');
                    $(FilterParentDiv + ' .FilterContainer').append('<div class="divFilterAddButton"><button class="btn btn-primary" id="btnAddFilter">' + addFilter + '</button></div>');
                    if (Availoperators != "all") {
                        $(FilterParentDiv + " #filterOperator" + newFilterId + " > option").each(function () {
                            if (this.value != Availoperators) {
                                this.remove();
                            }
                        });
                    }
                    $(FilterParentDiv + " #filter" + newFilterId).hide();
                }
                //If Not default
                else {
                    //if date filter
                    if (arrColumnNames[i].length > 5 && arrColumnNames[i][5].split(':')[0] == "date") {
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).attr("data-Isdate", "true");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).removeAttr("multiple");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect({
                            maxHeight: 200,
                            buttonClass: 'filterDDbtn',
                            onChange: function (option, checked, select) {
                                if ($(option).val() != "customdate") {
                                    DoFiltering(FilterParentDiv);
                                }
                                else {
                                    var selectId = $(option).parent().attr("id");
                                    $(option).parent().parent().hide();
                                    $(FilterParentDiv + " input#" + selectId).show();
                                    $(FilterParentDiv + " input#" + selectId).daterangepicker({
                                        applyClass: "btn-primary",
                                    }, function (start, end, label) {
                                        DoFiltering(FilterParentDiv);
                                    });
                                }
                            }
                        });
                    }
                    //only text filter
                    else if (arrColumnNames[i].length > 5 && arrColumnNames[i][5] == "onlytext") {
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).attr("data-Isdate", "onlytext");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).removeAttr("multiple");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).hide();
                        $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).show();

                        if (arrColumnNames[i].length > 6)
                            $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).val(arrColumnNames[i][6]);
                    }
                    //Only Number
                    else if (arrColumnNames[i].length > 5 && arrColumnNames[i][5] == "onlynumber") {
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).attr("data-Isdate", "onlynumber");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).removeAttr("multiple");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).hide();
                        $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).show();
                        $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).attr("onkeypress", "return isNumber(event)");

                        if (arrColumnNames[i].length > 6)
                            $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).val(arrColumnNames[i][6]);
                    }
                    //only select filter
                    else if (arrColumnNames[i].length > 5 && arrColumnNames[i][5] == "onlyselect") {
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).attr("data-Isdate", "onlyselect");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).removeAttr("multiple");
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).show();
                        $(FilterParentDiv + ' input#filterVal' + (arrColumnNames[i][0] + FilterId)).hide();

                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect({
                            maxHeight: 200,
                            buttonClass: 'filterDDbtn',
                            enableCaseInsensitiveFiltering: true,
                            onChange: function (option, checked, select) {
                                //DoFiltering(FilterParentDiv);
                            },
                            onDropdownHidden: function (event) {
                                if (event.clickEvent && (event.clickEvent.target.className == "filterColumn" || event.clickEvent.target.className == "filterOprator")) {
                                    event.preventDefault();
                                }
                                else {
                                    DoFiltering(FilterParentDiv);
                                }
                            }
                        });
                    }
                    //multiselect filter
                    else {
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect({
                            includeSelectAllOption: true,
                            nonSelectedText: 'Select',
                            maxHeight: 200,
                            numberDisplayed: 1,
                            selectAllValue: "all",
                            buttonClass: 'filterDDbtn',
                            enableCaseInsensitiveFiltering: true,
                            onDropdownHidden: function (event) {
                                if (event.clickEvent && (event.clickEvent.target.className == "filterColumn" || event.clickEvent.target.className == "filterOprator")) {
                                    event.preventDefault();
                                }
                                else {
                                    DoFiltering(FilterParentDiv);
                                }
                            }
                        });
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect('selectAll', false);
                        $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect('updateButtonText');
                    }
                }
            }
            else {
                $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect({
                    includeSelectAllOption: true,
                    nonSelectedText: 'Select',
                    maxHeight: 200,
                    numberDisplayed: 1,
                    selectAllValue: "all",
                    buttonClass: 'filterDDbtn',
                    enableCaseInsensitiveFiltering: true,
                    onDropdownHidden: function (event) {
                        if (event.clickEvent && (event.clickEvent.target.className == "filterColumn" || event.clickEvent.target.className == "filterOprator")) {
                            event.preventDefault();
                        }
                        else {
                            DoFiltering(FilterParentDiv);
                        }
                    }
                });
                $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect('selectAll', false);
                $(FilterParentDiv + ' select#filterVal' + (arrColumnNames[i][0] + FilterId)).multiselect('updateButtonText');
            }

            //for showing select value or text
            if (arrColumnNames[i].length > 4) {
                if (arrColumnNames[i][4] == "text") {
                    $(FilterParentDiv + " select#filterVal" + (arrColumnNames[i][0] + FilterId)).attr("data-detail", "text");
                    $(FilterParentDiv + " #ValueDetail" + FilterId).attr("data-detail", "text");
                }
            }

            if (arrColumnNames[i].length > 3 && arrColumnNames[i][3].split(':')[0] == "true") {
                FilterId++;
            }
        }
        $(FilterParentDiv + " #filter" + newFilterId + " #divfilterControls" + newFilterId).append('<div class="close_button"><a href="javascript:void(0);" class="removeFilter" id="' + newFilterId + '"><i class="fa fa-times-circle"></i></a></div>');

        if (FilterId > 0) {
            DoFiltering(FilterParentDiv, true);
        }
    }
    else {
        DoFiltering(FilterParentDiv);
    }
}

$(document).on("change", ".filterOprator", function () {
    var filterDiv = $(this).closest('div').parent().parent();
    var FilterParentDivId = "#" + filterDiv.closest('.FilterContainer').parent().parent().attr("id");
    var id = filterDiv.attr("filter-id");
    var filtercolunmName = filterDiv.find("#filterColumn" + id).val();
    var filtervalue = filterDiv.find("filterVal" + filtercolunmName + id);

    if ($(FilterParentDivId + " select#filterVal" + filtercolunmName + id).attr("data-Isdate") == "onlytext") {
        $(FilterParentDivId + " #divms" + filtercolunmName + id).hide();
        filterDiv.find("input#filterVal" + filtercolunmName + id).show();
    }
    else if ($(FilterParentDivId + " select#filterVal" + filtercolunmName + id).attr("data-Isdate") == "onlynumber") {
        $(FilterParentDivId + " #divms" + filtercolunmName + id).hide();
        filterDiv.find("input#filterVal" + filtercolunmName + id).show();
    }
    else if ($(FilterParentDivId + " select#filterVal" + filtercolunmName + id).attr("data-Isdate") == "onlyselect") {
        $(FilterParentDivId + " #divms" + filtercolunmName + id).show();
        filterDiv.find("input#filterVal" + filtercolunmName + id).hide();
    }
    else if ($(FilterParentDivId + " select#filterVal" + filtercolunmName + id).attr("data-Isdate") != "true") {
        if ($(this).val() != "equalto" && $(this).val() != "notEqualTo") {
            $(FilterParentDivId + " #divms" + filtercolunmName + id).hide();
            //filterDiv.find("select#filterVal" + filtercolunmName + id).hide();
            filterDiv.find("input#filterVal" + filtercolunmName + id).show();
        }
        else {
            $(FilterParentDivId + " #divms" + filtercolunmName + id).show();
            //filterDiv.find("select#filterVal" + filtercolunmName + id).show();
            filterDiv.find("input#filterVal" + filtercolunmName + id).hide();
        }
    }
});

$(document).on("change", ".filterColumn", function () {
    var filterDiv = $(this).closest('div').parent().parent();
    var FilterParentDivId = "#" + filterDiv.closest('.FilterContainer').parent().parent().attr("id");
    var id = filterDiv.attr("filter-id");
    var filtercolunmName = filterDiv.find("#filterColumn" + id).val();
    var operatorval = filterDiv.find("#filterOperator" + id).val();
    var filtervalue = filterDiv.find("filterVal" + filtercolunmName + id);

    filterDiv.find("#divFilterValue" + id).children().hide();
    if ($(FilterParentDivId + " select#filterVal" + filtercolunmName + id).attr("data-Isdate") == "onlytext") {
        $(FilterParentDivId + " #divms" + filtercolunmName + id).hide();
        filterDiv.find("input#filterVal" + filtercolunmName + id).show();
        $(FilterParentDivId + " #filterOperator" + id + " option[value='contains']").show();
        $(FilterParentDivId + " #filterOperator" + id + " option[value='notContains']").show();
    }
    else if ($(FilterParentDivId + " select#filterVal" + filtercolunmName + id).attr("data-Isdate") == "onlynumber") {
        $(FilterParentDivId + " #divms" + filtercolunmName + id).hide();
        filterDiv.find("input#filterVal" + filtercolunmName + id).show();
        $(FilterParentDivId + " #filterOperator" + id + " option[value='contains']").show();
        $(FilterParentDivId + " #filterOperator" + id + " option[value='notContains']").show();
    }
    else if ($(FilterParentDivId + " select#filterVal" + filtercolunmName + id).attr("data-Isdate") == "onlyselect") {
        $(FilterParentDivId + " #divms" + filtercolunmName + id).show();
        filterDiv.find("input#filterVal" + filtercolunmName + id).hide();
        $(FilterParentDivId + " #filterOperator" + id + " option[value='contains']").show();
        $(FilterParentDivId + " #filterOperator" + id + " option[value='notContains']").show();
    }
    else {
        if (operatorval != "equalto" && operatorval != "notEqualTo") {
            $(FilterParentDivId + " #divms" + filtercolunmName + id).hide();
            filterDiv.find("input#filterVal" + filtercolunmName + id).show();
            if ($(FilterParentDivId + " select#filterVal" + filtercolunmName + id).attr("data-Isdate") == "true") {
                $(FilterParentDivId + " #filterOperator" + id + " option[value='contains']").hide();
                $(FilterParentDivId + " #filterOperator" + id + " option[value='notContains']").hide();
            }
            else {
                $(FilterParentDivId + " #filterOperator" + id + " option[value='contains']").show();
                $(FilterParentDivId + " #filterOperator" + id + " option[value='notContains']").show();
            }
        }
        else {
            $(FilterParentDivId + " #divms" + filtercolunmName + id).show();
            filterDiv.find("input#filterVal" + filtercolunmName + id).hide();
            if ($(FilterParentDivId + " select#filterVal" + filtercolunmName + id).attr("data-Isdate") == "true") {
                $(FilterParentDivId + " #filterOperator" + id + " option[value='contains']").hide();
                $(FilterParentDivId + " #filterOperator" + id + " option[value='notContains']").hide();
            }
            else {
                $(FilterParentDivId + " #filterOperator" + id + " option[value='contains']").show();
                $(FilterParentDivId + " #filterOperator" + id + " option[value='notContains']").show();
            }
        }
    }
})

$(document).on("click", "#btnAddFilter", function (e) {
    e.stopPropagation();
    var i = 0;
    var FilderParentDivId = "#" + $(this).closest('.FilterContainer').parent().parent().attr("id");
    var cnt = $(FilderParentDivId + ' .FilterContainer').children().not('.divFilterAddButton').last().attr("filter-id");
    //var cnt = $(".FilterContainer").children().not('.divFilterAddButton').length;
    var newFilterId = parseInt(cnt) + 1;
    var avFilters = $(FilderParentDivId + " #AvailOperators").val();
    $(FilderParentDivId + " .divFilterAddButton").remove();
    $(FilderParentDivId + ' .FilterContainer').append('<div class="filtergroup filter' + newFilterId + '" id="filter' + newFilterId + '" filter-id="' + newFilterId + '" data-default="false"><div class="filterControls" id="divfilterControls' + newFilterId + '"><div class="custom-dropdown-filter"><select name="filterColumn' + newFilterId + '" id="filterColumn' + newFilterId + '" class="filterColumn"></select></div><div class="custom-dropdown-filter"><select name="filterOperator' + newFilterId + '" id="filterOperator' + newFilterId + '" class="filterOprator"><option value="equalto">==</option><option value="notEqualTo">!=</option><option value="contains">Contains</option><option value="notContains">!Contains</option></select></div><div class="" id="divFilterValue' + newFilterId + '"></div></div><div class="filterDetails" id="divFilterInfo' + newFilterId + '" style="display:none"><span id="ColumnDetail' + newFilterId + '"></span><span id="OperatorDetail' + newFilterId + '"></span><span data-detail="value" id="ValueDetail' + newFilterId + '"></span><span class="close_button"><a href="javascript:void(0);" class="removeFilter" id="' + newFilterId + '"><i class="fa fa-times-circle"></i></a></span></div></div>');
    $(FilderParentDivId + ' .FilterContainer').append('<div class="divFilterAddButton"><button class="btn btn-primary" id="btnAddFilter">' + addFilter + '</button></div>');

    if (avFilters != "all") {
        $(FilderParentDivId + " #filterOperator" + newFilterId + " > option").each(function () {
            if (this.value != avFilters) {
                this.remove();
            }
        });
    }

    $(FilderParentDivId + " #filterColumn" + (newFilterId - 1) + " > option").each(function () {
        $(FilderParentDivId + ' #filterColumn' + newFilterId).append($("<option></option>").attr("value", this.value).text(this.text));
        var DataValue = $(FilderParentDivId + " select#filterVal" + (this.text + (newFilterId - 1))).attr("data-detail");
        if (i == 0) {
            $(FilderParentDivId + " #divFilterValue" + newFilterId).append('<div id="divms' + (this.text + newFilterId) + '"><select multiple="true" data-detail="' + DataValue + '" class="filterValue" name="filterVal' + this.text + newFilterId + '" id="filterVal' + this.text + newFilterId + '"></select></div>');
            $(FilderParentDivId + " #divFilterValue" + newFilterId).append('<input class="filterValueText" type="text" name="filterVal' + this.text + newFilterId + '" id="filterVal' + this.text + newFilterId + '" style="display:none;" />');
        }
        else {
            $(FilderParentDivId + " #divFilterValue" + newFilterId).append('<div id="divms' + (this.text + newFilterId) + '" style="display:none"><select multiple="true" data-detail="' + DataValue + '" class="filterValue" name="filterVal' + this.text + newFilterId + '" id="filterVal' + this.text + newFilterId + '" style="display:none;"></select></div>');
            $(FilderParentDivId + " #divFilterValue" + newFilterId).append('<input class="filterValueText" type="text" name="filterVal' + this.text + newFilterId + '" id="filterVal' + this.text + newFilterId + '" style="display:none;" />');
        }

        var this1 = $(this);

        $(FilderParentDivId + " select#filterVal" + this.text + (newFilterId - 1) + " > option").each(function () {
            $(FilderParentDivId + ' select#filterVal' + this1.text() + newFilterId).append($("<option></option>").attr("value", this.value).text(this.text));
        });

        if ($(FilderParentDivId + ' select#filterVal' + (this1.text() + (newFilterId - 1))).attr("data-Isdate") == "true") {
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).attr("data-Isdate", "true");
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).removeAttr("multiple");
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).multiselect({
                maxHeight: 200,
                buttonClass: 'filterDDbtn',
                enableCaseInsensitiveFiltering: true,
                onChange: function (option, checked, select) {
                    if ($(option).val() != "customdate") {
                        DoFiltering(FilderParentDivId);
                    }
                    else {
                        var selectId = $(option).parent().attr("id");
                        $(option).parent().parent().hide();
                        $(FilderParentDivId + " input#" + selectId).show();
                        $(FilderParentDivId + " input#" + selectId).daterangepicker({
                            applyClass: "btn-primary",
                        }, function (start, end, label) {
                            DoFiltering(FilderParentDivId);
                        });
                    }
                }
            });
        }
        else if ($(FilderParentDivId + ' select#filterVal' + (this1.text() + (newFilterId - 1))).attr("data-Isdate") == "onlytext") {
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).attr("data-Isdate", "onlytext");
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).removeAttr("multiple");
            if (i == 0) {
                $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).hide();
                $(FilderParentDivId + ' input#filterVal' + (this1.text() + newFilterId)).show();
            }
        }
        else if ($(FilderParentDivId + ' select#filterVal' + (this1.text() + (newFilterId - 1))).attr("data-Isdate") == "onlynumber") {
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).attr("data-Isdate", "onlynumber");
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).removeAttr("multiple");
            $(FilderParentDivId + ' input#filterVal' + (this1.text() + newFilterId)).attr("onkeypress", "return isNumber(event)");
            if (i == 0) {
                $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).hide();
                $(FilderParentDivId + ' input#filterVal' + (this1.text() + newFilterId)).show();
            }
        }
        else if ($(FilderParentDivId + ' select#filterVal' + (this1.text() + (newFilterId - 1))).attr("data-Isdate") == "onlyselect") {
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).attr("data-Isdate", "onlyselect");
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).removeAttr("multiple");
            if (i == 0) {
                $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).show();
                $(FilderParentDivId + ' input#filterVal' + (this1.text() + newFilterId)).hide();
            }

            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).multiselect({
                maxHeight: 200,
                buttonClass: 'filterDDbtn',
                enableCaseInsensitiveFiltering: true,
                onChange: function (option, checked, select) {
                    //DoFiltering(FilderParentDivId);
                },
                onDropdownHidden: function (event) {
                    if (event.clickEvent && (event.clickEvent.target.className == "filterColumn" || event.clickEvent.target.className == "filterOprator")) {
                        event.preventDefault();
                    }
                    else {
                        DoFiltering(FilderParentDivId);
                    }
                }
            });
        }
        else {
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select',
                maxHeight: 200,
                numberDisplayed: 1,
                selectAllValue: "all",
                buttonClass: 'filterDDbtn',
                enableCaseInsensitiveFiltering: true,
                onDropdownHidden: function (event) {
                    if (event.clickEvent && (event.clickEvent.target.className == "filterColumn" || event.clickEvent.target.className == "filterOprator")) {
                        event.preventDefault();
                    }
                    else {
                        DoFiltering(FilderParentDivId);
                    }
                }
            });
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).multiselect('selectAll', false);
            $(FilderParentDivId + ' select#filterVal' + (this1.text() + newFilterId)).multiselect('updateButtonText');
        }
        i++;
    });
    if ($(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + newFilterId).val() + newFilterId).attr("data-Isdate") == "onlytext") {
        //do nothing
    }
    else if ($(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + newFilterId).val() + newFilterId).attr("data-Isdate") == "onlynumber") {
        //do nothing
    }
    else {
        if (e.isTrigger == undefined)
            $(FilderParentDivId + " #divms" + $(FilderParentDivId + " #filterColumn" + newFilterId).val() + newFilterId + " .multiselect.dropdown-toggle.filterDDbtn").dropdown('toggle');
    }
    $(FilderParentDivId + " #filter" + newFilterId + " #divfilterControls" + newFilterId).append('<div class="close_button"><a href="javascript:void(0);" class="removeFilter" id="' + newFilterId + '"><i class="fa fa-times-circle"></i></a></div>');
});

$(document).on('blur', ".filterValueText", function () {
    var FilderParentDivId = "#" + $(this).closest('.FilterContainer').parent().parent().attr("id");
    var selectId = $(this).attr("id");
    if ($(FilderParentDivId + ' select#' + selectId).attr("data-Isdate") != "true") {
        if ($(this).val() != "" && $(this).val() != undefined) {
            DoFiltering(FilderParentDivId);
        }
        else {
            var parendID = $(this).parent().attr("id").replace("divFilterValue", "");
            $(FilderParentDivId + " #filter" + parendID).remove();
            DoFiltering(FilderParentDivId);
        }
    }
});

$(document).on("click", ".removeFilter", function () {
    var i = 0;
    var cnt = 0;
    var Id = $(this).attr("id");
    var FilderParentDivId = "#" + $(this).closest('.FilterContainer').parent().parent().attr("id");
    $(FilderParentDivId + ' .FilterContainer').children().not('.divFilterAddButton').each(function () {
        if ($(this).attr("data-default") == "false") {
            cnt++;
        }
    });
    if (cnt > 1) {
        $(FilderParentDivId + " #filter" + Id).remove();
    }
    else {
        $(FilderParentDivId + " #filter" + Id).hide();
    }
    DoFiltering(FilderParentDivId);
})


function DoFiltering(FilderParentDivId, IsFirstTime = false) {
    var i = 0;
    var filterObj = [];
    if (IsFirstTime) {
        for (var j = 0; j < FilterColumnsArray.length; j++) {
            if (FilterColumnsArray[j].length > 7 && FilterColumnsArray[j][7] == "true") {
                $(FilderParentDivId + " #btnAddFilter").click();
                var fId = $(FilderParentDivId + ' .FilterContainer').children().not('.divFilterAddButton').last().attr("filter-id");
                $(FilderParentDivId + " #divFilterValue" + fId).children().hide()
                $(FilderParentDivId + " #filterColumn" + fId).val(FilterColumnsArray[j][0]);
                if ($(FilderParentDivId + " select#filterVal" + FilterColumnsArray[j][0] + fId).attr("data-Isdate") == "onlytext" || $(FilderParentDivId + " select#filterVal" + FilterColumnsArray[j][0] + fId).attr("data-Isdate") == "onlynumber") {
                    $(FilderParentDivId + " input#filterVal" + FilterColumnsArray[j][0] + fId).val(FilterColumnsArray[j][6]);
                    $(FilderParentDivId + " input#filterVal" + FilterColumnsArray[j][0] + fId).show();
                }
                else {
                    $(FilderParentDivId + " #divFilterValue" + fId + " #divms" + FilterColumnsArray[j][0] + fId).show();
                    if (FilterColumnsArray[j][6] != "" && FilterColumnsArray[j][6] != null) {
                        if ($(FilderParentDivId + " select#filterVal" + FilterColumnsArray[j][0] + fId).attr('multiple') == "multiple") {
                            $(FilderParentDivId + " select#filterVal" + FilterColumnsArray[j][0] + fId).val(FilterColumnsArray[j][6]);
                            $(FilderParentDivId + " select#filterVal" + FilterColumnsArray[j][0] + fId).multiselect('refresh');
                        }
                        else {
                            $(FilderParentDivId + " select#filterVal" + FilterColumnsArray[j][0] + fId + " option[value='" + FilterColumnsArray[j][6] + "']").attr('selected', 'selected');
                        }
                    }
                }
            }
        }
    }
    var visibleChild = 0;
    $(FilderParentDivId + ' .FilterContainer').children().not('.divFilterAddButton').each(function () {
        if ($(this).attr("style") != "display: none;") {
            visibleChild++;
        }
    });

    $(FilderParentDivId + ' .FilterContainer').children().not('.divFilterAddButton').each(function () {
        if ($(this).attr("style") != "display: none;") {
            var id = $(this).attr("filter-id");
            var field = $(FilderParentDivId + " #filterColumn" + id).val();
            var operator = $(FilderParentDivId + " #filterOperator" + id).val();
            var filterVal = '';
            if (operator == "equalto" || operator == "notEqualTo") {
                filterVal = $(FilderParentDivId + " select#filterVal" + field + id).val();
                if (filterVal == "customdate") {
                    filterVal = $(FilderParentDivId + " input#filterVal" + field + id).val();
                }
                if ($(FilderParentDivId + " select#filterVal" + field + id).attr("data-Isdate") == "onlytext" || $(FilderParentDivId + " select#filterVal" + field + id).attr("data-Isdate") == "onlynumber") {
                    filterVal = $(FilderParentDivId + " input#filterVal" + field + id).val();
                }
            }
            else {
                filterVal = $(FilderParentDivId + " input#filterVal" + field + id).val();
            }
            
            if (filterVal != "" && filterVal != undefined && filterVal != null) {
                var checkIfFilterExist = filterObj.findIndex(x => x.FieldName === field);
                if (checkIfFilterExist > -1) {
                    var sameFilterId = filterObj[checkIfFilterExist].FilterId;
                    $(FilderParentDivId + " #filter" + sameFilterId).remove();
                    filterObj.splice(checkIfFilterExist, 1);
                }
                filterObj.push({
                    "FieldName": field,
                    "Operator": operator,
                    "FilterValue": filterVal.toString(),
                    "FilterId": id
                });
            }
            else if (visibleChild == 1) {
                $(FilderParentDivId + " select#filterVal" + field + id).multiselect('selectAll', false);
                $(FilderParentDivId + " select#filterVal" + field + id).multiselect('updateButtonText');
                filterVal = $(FilderParentDivId + " select#filterVal" + field + id).val();
                filterObj.push({
                    "FieldName": field,
                    "Operator": operator,
                    "FilterValue": filterVal.toString(),
                    "FilterId": id
                });
            }
            else {
                $(FilderParentDivId + " #filter" + id).remove();
            }
        }
        i++;
    });

    if (filterObj.length > 0) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: $(FilderParentDivId + " #FilterDataurl").val(),
            data: JSON.stringify(filterObj),
            beforeSend: function () {
            },
            success: function (data) {
                $($(FilderParentDivId + " #updateTargetId").val()).html(data);
                if ($(FilderParentDivId + " #DataTableId").val() != '' && $(FilderParentDivId + " #DataTableId").val() != undefined && $(FilderParentDivId + " #DataTableId").val() != null) {
                    eval($(FilderParentDivId + " #DataTableId").val());
                }

                ShowFilterDetailsOnly(FilderParentDivId);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
}

function ShowFilterDetailsOnly(FilderParentDivId) {
    $(FilderParentDivId + ' .FilterContainer').children().not('.divFilterAddButton').each(function () {
        var id = $(this).attr("filter-id");
        if ($(FilderParentDivId + " #filter" + id).attr("style") != "display: none;") {
            $(FilderParentDivId + " #divfilterControls" + id).hide();
            $(FilderParentDivId + " #divFilterInfo" + id).show();
            $(FilderParentDivId + " #ColumnDetail" + id).text($(FilderParentDivId + " #filterColumn" + id).val());
            $(FilderParentDivId + " #OperatorDetail" + id).text(getOperatorText($(FilderParentDivId + " #filterOperator" + id).val()));
            if (($(FilderParentDivId + " #filterOperator" + id).val() != "equalto" && $(FilderParentDivId + " #filterOperator" + id).val() != "notEqualTo") || ($(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).attr("data-Isdate") == "onlytext" || $(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).attr("data-Isdate") == "onlynumber")) {
                $(FilderParentDivId + " #ValueDetail" + id).html("<strong>" + $(FilderParentDivId + " input#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).val() + "</strong>");
            }
            else {
                var SelectedVal = $(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).val();
                if (SelectedVal != "" && SelectedVal != undefined && SelectedVal != null) {
                    SelectedVal = SelectedVal.toString();
                }

                if ($(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).attr("data-detail") == "text") {
                    if (SelectedVal == "customdate") {
                        $(FilderParentDivId + " #ValueDetail" + id).html("<strong>" + $(FilderParentDivId + " input#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).val() + "</strong>");
                    } else {
                        if (SelectedVal.split(',').length == 1) {
                            $(FilderParentDivId + " #ValueDetail" + id).html("<strong>" + $(FilderParentDivId + " select#filterVal" + ($(FilderParentDivId + " #filterColumn" + id).val() + id) + " option:selected").html() + "</strong>");
                        }
                        else {
                            if ($(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + "#filterColumn" + id).val() + id + " option").length == SelectedVal.split(',').length) {
                                $(FilderParentDivId + " #ValueDetail" + id).html("<strong>All</strong>");
                            }
                            else {
                                $(FilderParentDivId + " #ValueDetail" + id).html("<strong>" + SelectedVal.split(',').length + " Selected</strong>");
                            }
                        }
                    }
                }
                else {
                    if (SelectedVal == "customdate" || ($(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).attr("data-Isdate") == "onlytext" || $(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).attr("data-Isdate") == "onlynumber")) {
                        $(FilderParentDivId + " #ValueDetail" + id).html("<strong>" + $(FilderParentDivId + " input#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).val() + "</strong>");
                    }
                    else {
                        if (SelectedVal.split(',').length == 1) {
                            $(FilderParentDivId + " #ValueDetail" + id).html("<strong>" + $(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id).val() + "</strong>");
                        }
                        else {
                            if ($(FilderParentDivId + " select#filterVal" + $(FilderParentDivId + " #filterColumn" + id).val() + id + " option").length == SelectedVal.split(',').length) {
                                $(FilderParentDivId + " #ValueDetail" + id).html("<strong>All</strong>");
                            }
                            else {
                                $(FilderParentDivId + " #ValueDetail" + id).html("<strong>" + SelectedVal.split(',').length + " Selected</strong>");
                            }
                        }
                    }
                }
            }
        }
    });
};

$(document).on("click", ".filterDetails", function (e) {
    e.stopPropagation();
    var FilderParentDivId = "#" + $(this).closest('.FilterContainer').parent().parent().attr("id");
    var divParentId = $(this).parent().attr("filter-id");
    $(FilderParentDivId + " #divfilterControls" + divParentId).show();
    $(FilderParentDivId + " #divFilterInfo" + divParentId).hide();
    var field = $(FilderParentDivId + " #filterColumn" + divParentId).val();
    var operator = $(FilderParentDivId + " #filterOperator" + divParentId).val();
    if (operator == "equalto" || operator == "notEqualTo") {
        if ($(FilderParentDivId + " select#filterVal" + field + divParentId).attr("data-Isdate") == "onlytext" || $(FilderParentDivId + " select#filterVal" + field + divParentId).attr("data-Isdate") == "onlynumber") {
            //do nothing
        }
        else {
            $(FilderParentDivId + " #divms" + $(FilderParentDivId + " #filterColumn" + divParentId).val() + divParentId + " .multiselect.dropdown-toggle.filterDDbtn").dropdown('toggle');
        }
    }
});


function getOperatorText(operator) {
    if (operator == "equalto")
        return "==";
    else if (operator == "notEqualTo")
        return "!=";
    else if (operator == "contains")
        return "Contains";
    else if (operator == "notContains")
        return "!Contains";
}


function InitDataTable(Selector, PageLenghtMenu, IsSearching, sortingArray) {
    $(Selector).DataTable({
        destroy: true,
        info: false,
        searching: IsSearching,
        paging: true,
        pageLength: PageLenghtMenu[0],
        lengthMenu: PageLenghtMenu,
        dom: '<"html5buttons"B>lTfgitp',
        order: sortingArray,
        buttons: [],
        columnDefs: [{ orderable: false, targets: 'nosort' }],
        language: {
            lengthMenu: "<strong>" + pageSize + "</strong> : _MENU_",
            emptyTable: noDataAvailable,
            paginate: {
                previous: previous,
                next: next
            }
        }
    });
}

function DraggableModalPopup(Selector) {
    if (!($('.modal.in').length)) {
    }
    $(Selector).modal({
        backdrop: 'static',
        keyboard: false,
        show: true
    });
    $('.modal-dialog').draggable({
        handle: ".modal-header,.modal-footer"
    });
}


function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}


function addNewTagsPopup() {
    $.ajax({
        type: 'GET',
        url: "/Tags/AddTags?isAllowLOBTag=" + false,
        dataType: 'HTML',
        success: function (data) {
            $("#AddTagsModalMain").html(data);
            DraggableModalPopup("#AddTagsModal");
        }
    });
}
function SetTagValue(OptionValue) {
    $("#AddTagsModal").modal("hide");
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message
    var x = document.getElementsByClassName("chzn-select");
    for (i = 0; i < x.length; i++) {
        $("#" + x[i].id).append($('<option>', { value: OptionValue }).text(OptionValue));
        $("#" + x[i].id).trigger("chosen:updated");
    }
}


function addNewCountryGroup() {
    $.ajax({
        type: 'GET',
        url: "/Portal/AddCountryGrpPopup",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#PortalCountryGroupModalMain").html(data);
            DraggableModalPopup("#PortalCountryGroupModal");
        }
    });
}
function CallbackCountryGroup(CountryGroupName) {
    $("#PortalCountryGroupModal").modal("hide");
    if (CountryGroupName != undefined) {
        $(".AddableCountryGroup").append(new Option(CountryGroupName.split("@#$")[0], CountryGroupName.split("@#$")[1]));
    }
}
function CallbackCountryGroupPopup(CountryGroupName, CountryGroupId) {
    $("#PortalCountryGroupModal").modal("hide");
    $("#GroupId").append('<option value=' + CountryGroupId + '>' + CountryGroupName.split("@#$")[0] + '</option>');
}
function CallbackCountryGroupPopupMatch(CountryGroupName, CountryGroupId) {
    $("#PortalCountryGroupModal").modal("hide");
    $("#CountryGroupId").append('<option value=' + CountryGroupId + '>' + CountryGroupName.split("@#$")[0] + '</option>');
    $("#GroupId").append('<option value=' + CountryGroupId + '>' + CountryGroupName.split("@#$")[0] + '</option>');
}
var isNotify = 0;
$('body').on('click', '#NotifyNotification', function () {
    if (isNotify == 0) {
        $('#NotificationDiv').addClass("displayBlock");
        isNotify++;
    }
    else {
        $('#NotificationDiv').removeClass("displayBlock");
        isNotify--;
    }
});

$(document).on('change', '.QuickSearchBox .QTypeAheadToggle', function () {
    if (!$(".QuickSearchBox .QTypeAheadToggle").is(':checked')) {
        $(".QuickSearchBox .lblQTypeAhead").removeClass("thColor");
        if ($(".QuickSearchBox #txtPCompany").autocomplete("instance") != undefined) {
            $(".QuickSearchBox #txtPCompany").autocomplete("disable");
        }
    }
    else {
        $(".QuickSearchBox .lblQTypeAhead").addClass("thColor");
        if ($(".QuickSearchBox #txtPCompany").autocomplete("instance") != undefined) {
            $(".QuickSearchBox #txtPCompany").autocomplete("enable");
        }
    }
});