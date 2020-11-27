$(document).ready(function () {

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
$('body').on('click', '.QuickSearch', function (e) {
    var Company = $("#txtPCompany").val();
    var Address = $("#txtPAddress").val();
    var City = $("#txtPCity").val();
    var State = $("#txtPState").val();
    var Phone = $("#txtPPhone").val();
    var Zip = $("#txtPZip").val();
    var Country = $("select#CountryP").val();
    if (!validateFileName(Company)) {
        alert("Company can't contain any of the following characters \r\n \\ / : * \" < > |");
        return false;
    }
    if (!validateFileName(Address)) {
        alert("Address can't contain any of the following characters \r\n \\ / : * \" < > |");
        return false;
    }
    if (!validateFileName(City)) {
        alert("City can't contain any of the following characters \r\n \\ / : * \" < > |");
        return false;
    }
    if (!validateFileName(State)) {
        alert("State can't contain any of the following characters \r\n \\ / : * \" < > |");
        return false;
    }
    if (!validateFileName(Phone)) {
        alert("Phone can't contain any of the following characters \r\n \\ / : * \" < > |");
        return false;
    }
    if (!validateFileName(Zip)) {
        alert("Zip can't contain any of the following characters \r\n \\ / : * \" < > |");
        return false;
    }

    //var url = url = "/SearchData/Index?Company=" + ConvertEncrypte(Company) + "&Address=" + ConvertEncrypte(Address) + "&Phone=" + ConvertEncrypte(Phone) + "&Country=" + ConvertEncrypte(encodeURI(Country)) + "&City=" + ConvertEncrypte(City) + "&State=" + ConvertEncrypte(State) + "&Zip=" + ConvertEncrypte(Zip) + "&strsearch=yes";
    var url = url = "/SearchData/Index?Company=" + Company + "&Address=" + Address + "&Phone=" + Phone + "&Country=" + encodeURI(Country) + "&City=" + City + "&State=" + State + "&Zip=" + Zip;
    if (Company != '' && Country != '') {
        window.open(url, "_self");

    } else {
         bootbox.alert({
								title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
								message: "Please fill Company and Country detail first."
							});
    }
});

$(document).ready(function () {
    $($('.mainNav ul')[0]).append($('.liCircleUserText'))
    $('#divProgress').hide();
    GetETLJobStatus();
    
    GetActiveUserData();
   
    setInterval(function () {
        GetETLJobStatus();
        GetActiveUserData();
    }, 50000);

  

});



$('.process-popup').on('mouseenter mouseleave', function (e) {
    $(".bgProcess").show("slide", { direction: "left" }, 500);

});

$('.bgProcess').on('mouseleave', function (e) {
    $(".bgProcess").hide("slide", { direction: "left" }, 500);
});


/* active user*/
$('.ActiveUser').on('mouseenter mouseleave', function (e) {
    $(".userdatacount-new").show("slide", { direction: "left" }, 500);

});

$('.userdatacount-new').on('mouseleave', function (e) {
    $(".userdatacount-new").hide("slide", { direction: "left" }, 500);

});
$('.well').on('mouseenter mouseleave ', function (e) {
    $(".userdatacount-new").hide("slide", { direction: "left" }, 500);
    $(".bgProcess").hide("slide", { direction: "left" }, 500);

});


//jQuery(document).ready(function (e) {
//    var tt_exp = "1"; //set the exp time in min
   
//    tt_exp = (parseInt(tt_exp) * 1800000); //set the time in microsecond 1800000

//    //for timeout action even ajax call too.
//    jQuery(document).bind("idle.idleTimer", function () {
//        $.ajax({
//            type: "POST",
//            url: "/Account/LogOff/",
//            data: '',
//            dataType: "html",
//            contentType: "application/json",
//            success: function (data) {
//                location.reload();

//            },
//            error: function (xhr, ajaxOptions, thrownError) {
//            }
//        });
//    });
//    //for the session is alive
//    jQuery(document).bind("active.idleTimer", function () {


//    });

//    jQuery.idleTimer(tt_exp);
//});
function validateFileName(fileName) {
    if (fileName.indexOf("\\") > -1 || fileName.indexOf("/") > -1 || fileName.indexOf(":") > -1 || fileName.indexOf("\"") > -1 || fileName.indexOf("<") > -1 || fileName.indexOf(">") > -1 || fileName.indexOf("?") > -1 || fileName.indexOf("|") > -1 || fileName.indexOf("*") > -1)
    { return false; } else {
        return true;
    }
}

function ConvertEncrypte(value) {
    var newvalue = "";
    if (value != '' && value != undefined) {
        $.ajax({
            type: "POST",
            url: "/SearchData/GetEncryptedString",
            data: JSON.stringify({ strValue: value }),
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
