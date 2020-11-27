$(document).ajaxStart(function () {
    $('#divProgress').show();
}).ajaxStop(function () {
    $('#divProgress').hide();
});
$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});
var ElementType = "";


$('body').on('click', '#btnAddBusinessElements', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/DNBMonitoring/AddBusinessElement",
        dataType: 'HTML',
        success: function (data) {
            $("#AddBusinessElementModalMain").html(data);
            DraggableModalPopup("#AddBusinessElementModal");
            LoadContextMenuCondition();
        }
    });
});

$('body').on('click', '#btnCreateConditions', function () {

    $(".MultiConditionSection").show();
    $("#ParentElements").val($("#BusinessProductElement option:selected").text());
    $("#ParentCondition").val($("#SelectBusinessConditions option:selected").text());
    ElementType = $("#ElementType").val();

    $(".divConditon").each(function () {
        $(this).hide();
    });
    $(".Mainconditons").html('');
    $("#ConditionCount").val('0');
    $.ajax({
        type: "GET",
        url: "/DNBMonitoring/EmptyCondition",
        dataType: "JSON",
        contentType: "application/json",
        async: false,
        success: function (data) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    $(".lblCondition").text('');
    if (ElementType != undefined && ElementType == "numeric") {
        $(function () {
            $.contextMenu({
                selector: '.context-menu-Operators',
                trigger: 'left',
                callback: function (key, options) {
                },
                items: {
                    "ValueEquals": {
                        name: "Value Equals", callback: function () {
                            $(this).text('ValueEquals');
                            GetValueFromNode(this);
                        }
                    },
                    "ValueIncreaseByPercentage": {
                        name: "Value Increase By Percentage", callback: function () {
                            $(this).text('ValueIncreaseByPercentage');
                            GetValueFromNode(this);
                        }
                    },
                    "ValueDecreaseByPercentage": {
                        name: "Value Decrease By Percentage", callback: function () {
                            $(this).text('ValueDecreaseByPercentage');
                            GetValueFromNode(this);
                        }
                    },
                    "ValueChangeByPercentage": {
                        name: "Value Change By Percentage", callback: function () {
                            $(this).text('ValueChangeByPercentage');
                            GetValueFromNode(this);
                        }
                    },
                    "ValueIncreaseBy": {
                        name: "Value Increase By", callback: function () {
                            $(this).text('ValueIncreaseBy');
                            GetValueFromNode(this);
                        }
                    },
                    "ValueDecreaseBy": {
                        name: "Value Decrease By", callback: function () {
                            $(this).text('ValueDecreaseBy');
                            GetValueFromNode(this);
                        }
                    },
                    "ValueChangeBy": {
                        name: "Value Change By", callback: function () {
                            $(this).text('ValueChangeBy');
                            GetValueFromNode(this);
                        }
                    },
                    "ValueGoesBeyond": {
                        name: "Value Goes Beyond", callback: function () {
                            $(this).text('ValueGoesBeyond');
                            GetValueFromNode(this);
                        }
                    },
                    "ValueGoesBelow": {
                        name: "Value Goes Below", callback: function () {
                            $(this).text('ValueGoesBelow');
                            GetValueFromNode(this);
                        }
                    },
                },
            });

            $('.context-menu-Operators').change('click', function (e) {
            })
        });
    } else if (ElementType != undefined && ElementType == "string") {
        $(function () {
            $.contextMenu({
                selector: '.context-menu-Operators',
                trigger: 'left',
                callback: function (key, options) {
                },
                items: {
                    "ValueEquals": {
                        name: "Value Equals", callback: function () {
                            $(this).text('ValueEquals');
                            GetValueFromNode(this);
                        }
                    },
                },
            });
            $('.context-menu-Operators').change('click', function (e) {
            })
        });
    }
});

$('body').on('click', '#btncloseBusinessElements', function () {
    var BusinessCondionUpdateId = $("#BusinessCondionUpdateId").val();
    BusinessCondionUpdateId = BusinessCondionUpdateId == undefined ? null : BusinessCondionUpdateId;
    var BusinessProductElement = $("#BusinessProductElement option:selected").text();
    var ProductElementValue = $("#BusinessProductElement").val();;
    ProductElementValue = ProductElementValue.split("@#$");
    var BusinessProductElementId = ProductElementValue[0];
    var BusinessusinessConditions = $("#SelectBusinessConditions").val();
    var cnt = 0;

    if (BusinessProductElement == "-Select-" || BusinessCondionUpdateId == undefined) {
        $("#spnBusinessProductElement").show();
        cnt++;
    }
    else {
        $("#spnBusinessProductElement").hide();
    }
    if (BusinessusinessConditions == "-1" || BusinessusinessConditions == "" || BusinessusinessConditions == undefined) {
        $("#spnBusinessConditions").show();
        cnt++;
    }
    else {
        $("#spnBusinessConditions").hide();
    }
    if (cnt > 0) {
        return false;
    }
    var token = $('input[name="__RequestVerificationToken"]').val();
    var QueryString = "BusinessProductElement:" + BusinessProductElement + "@#$BusinessusinessConditions:" + BusinessusinessConditions + "@#$BusinessCondionUpdateId:" + BusinessCondionUpdateId + "@#$BusinessProductElementId:" + BusinessProductElementId;
    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/SaveBusinessElement/",
        data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(QueryString)).split("+").join("***") }),
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        cache: false,
        async: false,
        success: function (data) {
            $("#AddBusinessElementModal").modal("hide");
            LoadPartialBusinessElement();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

$('body').on('change', '#BusinessProductElement', function () {
    SetBusinessCondition();
    if ($("#SelectBusinessConditions").val().toLowerCase() == "multicondition") {
        $("#btnCreateConditions").show();
        $("#btncloseBusinessElements").hide();
    }
    else {
        $("#btnCreateConditions").hide();
        $("#btncloseBusinessElements").show();
        $(".MultiConditionSection").hide();
        $(".lblCondition").text('');
        $(".txtValue").each(function () {
            $(this).val('');
        });
        $(".divConditon").each(function () {
            $(this).hide();
        });
        $("#ConditionCount").val('0');
    }

});
function SetBusinessCondition() {
    var element = $("#BusinessProductElement option:selected").val();
    var SetIds = $("#SetId").val();
    var QueryString = 'element:' + element + '@#$SetIds:' + SetIds;
    $.ajax({
        url: '/DNBMonitoring/BussnessConditions',
        type: "POST",
        dataType: "JSON",
        data: { Parameters: ConvertEncrypte(encodeURI(QueryString).split("+").join("***")) },
        async: false,
        success: function (Data) {
            $("#SelectBusinessConditions").html("");
            $.each(Data.lstAllBussnessElements, function (i, elements) {
                $("#SelectBusinessConditions").append(
                    $('<option></option>').val(elements.Value).html(elements.Text));
            });
            $("#ElementType").val(Data.ElementType);
            $("#SetId").val('1');
        }
    });

}
$('body').on('change', '#SelectBusinessConditions', function () {
    if ($(this).val() == undefined) { return false; }
    if ($(this).val() == "-1") { return false; }
    var ParentElemets = $("#BusinessProductElement").val();
    var ParentCodition = $(this).val();
    var BusinessCondionUpdateId = $("#BusinessCondionUpdateId").val();

    if ($(this).val().toLowerCase() == "multicondition") {
        $("#btnCreateConditions").show();
        $("#btncloseBusinessElements").hide();

    }
    else {
        $("#btnCreateConditions").hide();
        $("#btncloseBusinessElements").show();
        $(".MultiConditionSection").hide();
        $(".lblCondition").text('');
        $(".txtValue").each(function () {
            $(this).val('');
        });
        $(".divConditon").each(function () {
            $(this).hide();
        });
        $("#ConditionCount").val('0');
    }
    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/CheckBusinessCondition?ParentElemets=" + ParentElemets + "&ParentCodition=" + ParentCodition + "&BusinessCondionUpdateId=" + BusinessCondionUpdateId,
        dataType: "JSON",
        contentType: "application/json",
        success: function (data) {
            if (data == "failure") {
                $("#SelectBusinessConditions").val("-1");
                bootbox.alert({
                    title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
                    message: "Element or Condition is already exist. please try with different Elements or Condition"
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    var ConditionsType = $(this).val();
    if (ConditionsType != undefined && ConditionsType.toLowerCase() == "multicondition") {
        $('#btnCreateConditions').show();
    }
    else {
        $('#btnCreateConditions').hide();
    }
});

//left click Menu On AND-OR Operator 
function LoadContextMenuCondition() {
    if ($(".context-menu-one").length > 0) {
        $(function () {

            $.contextMenu({
                selector: '.context-menu-one',
                trigger: 'left',
                callback: function (key, options) {
                },
                items: {

                    "And Group": {
                        name: "And Group", callback: function () {
                            $(this).text('And Group');
                            $(this).attr('data-val', 'And Group');
                        }
                    },
                    "OR Group": {
                        name: "OR Group", callback: function () {
                            $(this).text('OR Group');
                            $(this).attr('data-val', 'OR Group');
                        }
                    },
                    "And": {
                        name: "And", callback: function () {
                            $(".context-menu-one").text('And');
                        }
                    },
                    "OR": {
                        name: "OR", callback: function () {
                            $(".context-menu-one").text('OR');
                        }
                    },
                }
            });

            $('.context-menu-one').on('click', function (e) {
            })
        });
        $(function () {

            $.contextMenu({
                selector: '.context-menu-Grpone',
                trigger: 'left',
                callback: function (key, options) {
                },
                items: {
                    "And Group": {
                        name: "And Group", callback: function () {
                            $(this).text('And Group');
                            $(this).attr('data-val', 'And Group');
                        }
                    },
                    "OR Group": {
                        name: "OR Group", callback: function () {
                            $(this).text('OR Group');
                            $(this).attr('data-val', 'OR Group');
                        }
                    },
                    "And": {
                        name: "And", callback: function () {
                            $(this).text('And');
                            $(this).attr('data-val', 'GrpAnd');
                        }
                    },
                    "OR": {
                        name: "OR", callback: function () {
                            $(this).text('OR');
                            $(this).attr('data-val', 'GrpOR');
                        }
                    },
                }
            });

            $('.context-menu-Grpone').on('click', function (e) {
            })
        });
        //Left click Menu on Elements
        $(function () {
            $.contextMenu({
                selector: '.context-menu-Elements',
                trigger: 'left',
                callback: function (key, options) {
                },
                items: {
                    "element": {
                        name: "element", callback: function () {
                            $(this).text('element');
                            var Operators = $(this).parent().parent().find('.ConditonOperators').html();
                            var updatedId = $(this).parent().parent().find('.deleteCondition').attr("data-val");
                            var ConditonValue = $(this).parent().parent().find('.txtValue').val();
                            var TempGrpId = $(this).attr('data-Id');
                            var Condition = $(this).attr('data-node');

                            updateList(Operators, "element", ConditonValue, updatedId, TempGrpId, Condition, "Update");
                        }
                    },
                    "elementPrevious": {
                        name: "element Previous", callback: function () {
                            $(this).text('elementPrevious');
                            var Operators = $(this).parent().parent().find('.ConditonOperators').html();
                            var updatedId = $(this).parent().parent().find('.deleteCondition').attr("data-val");
                            var ConditonValue = $(this).parent().parent().find('.txtValue').val();
                            var TempGrpId = $(this).attr('data-Id');
                            var Condition = $(this).attr('data-node');
                            updateList(Operators, "elementPrevious", ConditonValue, updatedId, TempGrpId, Condition, "Update");
                        }
                    },
                }
            });

            $('.context-menu-one').on('click', function (e) {
            })
        });
        //Left click Menu on Operators
    }
}
function GetValueFromNode(e) {
    var txt = $(e).text();
    var element = $(e).parent().parent().find('.Elements').html();
    var updatedId = $(e).parent().parent().find('.deleteCondition').attr("data-val");
    var txtvalue = $(e).parent().parent().find('.txtValue').val();
    var TempGrpId = $(e).attr('data-Id');
    var Condition = $(e).attr('data-node');
    updateList(txt, element, txtvalue, updatedId, TempGrpId, Condition, "Update");
}
function updateList(Opetator, element, txtvalue, updatedId, TempGrpId, Condition, Command) {
    var QueryString = "Conditon:" + Condition + "@#$Element:" + element + "@#$ConditonOpetator:" + Opetator + "@#$ConditonValue:" + txtvalue + "@#$btnSubmit:" + Command + "@#$deletedId:" + updatedId + "@#$TempGrpId:" + TempGrpId + "@#$GrpCondition:" + "And" + "@#$IsCreate:" + false + "@#$IsGroup:" + false;;
    $.ajax({
        type: "GET",
        url: "/DNBMonitoring/AddElementCondition?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: "html",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {

            $("#ElementsList").html(data);
            var strCondition = $("#strCondition").each(function () {
                $(".lblCondition").text("Your Condition should be : " + $(this).val());
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$('body').on('click', '#btnSubmit', function () {

    var ConditionCount = $("#ConditionCount").val();
    if (parseInt(ConditionCount) <= 9) {
        var Conditon = $(".Conditon").html();
        var Element = $(".Mainconditons .row:last").children().find('.elements').html();
        var ConditonOpetator = $(".Mainconditons .row:last").children().find('.Operators').html();
        var ConditonValue = $(".Mainconditons .row:last").children().find('.txtValue').val();
        var IsGroup = false;
        var GrpCondition = "AND";
        if (Conditon == "And Group" || Conditon == "OR Group") {
            if (Conditon == "GrpAnd") {
                GrpCondition = "And";
            } else if (Conditon == "GrpOR") {
                GrpCondition = "OR";
            } else if (Conditon == "And Group") {
                GrpCondition = "AndGroup";
                IsGroup = true;
            }
            else if (Conditon == "OR Group") {
                GrpCondition = "ORGroup";
                IsGroup = true;
            }
            Conditon = "Group";
        }
        var QueryString = "Conditon:" + Conditon + "@#$Element:" + Element + "@#$ConditonOpetator:" + ConditonOpetator + "@#$ConditonValue:" + ConditonValue + "@#$btnSubmit:" + "Add" + "@#$deletedId:" + null + "@#$TempGrpId:" + 0 + "@#$GrpCondition:" + GrpCondition + "@#$IsCreate:" + false + "@#$IsGroup:" + IsGroup;

        $.ajax({
            type: "GET",
            url: "/DNBMonitoring/AddElementCondition?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
            dataType: "html",
            contentType: "application/html",
            cache: false,
            success: function (data) {
                $("#ElementsList").html(data);
                var strCondition = $("#strCondition").each(function () {
                    $(".lblCondition").text("Your Condition should be : " + $(this).val());
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    } else {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
            message: "You can add maximum 10 condition"
        });
    }
});
$('body').on('click', '.deleteCondition', function () {

    var TempGrpId = $(this).attr('data-Id');
    var Condition = $(this).attr('data-node');


    var QueryString = "Conditon:" + Condition + "@#$Element:" + "" + "@#$ConditonOpetator:" + "" + "@#$ConditonValue:" + "" + "@#$btnSubmit:" + "delete" + "@#$deletedId:" + "" + "@#$TempGrpId:" + TempGrpId + "@#$GrpCondition:" + "And" + "@#$IsCreate:" + false + "@#$IsGroup:" + false;;
    $.ajax({
        type: "GET",
        url: "/DNBMonitoring/AddElementCondition?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: "html",
        cache: false,
        contentType: "application/html",
        success: function (data) {
            $("#ElementsList").html(data);
            var strCondition = $("#strCondition").each(function () {
                $(".lblCondition").text("Your Condition should be : " + $(this).val());
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });

});


$('body').on('change', '.txtValue', function () {
    var Elements = $(this).parent().parent().find(".Elements").html();
    var Operators = $(this).parent().parent().find(".ConditonOperators").html();
    var ConditonValue = $(this).val();
    var updatedId = $(this).parent().parent().find('.deleteCondition').attr("data-val");
    var TempGrpId = $(this).attr('data-Id');
    var Condition = $(this).attr('data-node');
    updateList(Operators, Elements, ConditonValue, updatedId, TempGrpId, Condition, "Update");
});
 

$('body').on('click', '.DeleteBussnessCondition', function () {
    var QueryString = $(this).attr("id");
    $.ajax({
        type: "GET",
        url: "/DNBMonitoring/DeleteBusinessCondition?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: "html",
        contentType: "application/html",
        success: function (data) {
            $(".BusinessElements").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
$('body').on('click', '.AddinList', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var BusinessCondionUpdateId = $("#BusinessCondionUpdateId").val();
    BusinessCondionUpdateId = BusinessCondionUpdateId == undefined ? null : ConvertEncrypte(encodeURI(BusinessCondionUpdateId)).split("+").join("***");
    var ParentElements = $("#ParentElements").val();
    ParentElements = ConvertEncrypte(encodeURI(ParentElements)).split("+").join("***");
    var ParentCondition = $("#ParentCondition").val();
    ParentCondition = ConvertEncrypte(encodeURI(ParentCondition)).split("+").join("***");
    ProductElementId = $("#BusinessProductElement option:selected").val();
    ProductElementId = ConvertEncrypte(encodeURI(ProductElementId)).split("+").join("***");

    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/AddElementCondition",
        data: JSON.stringify({ BusinessCondionUpdateId: BusinessCondionUpdateId, ParentElements: ParentElements, ParentCondition: ParentCondition, ProductElementId: ProductElementId }),
        dataType: "json",
        contentType: "application/json",
        headers: { "__RequestVerificationToken": token },
        async: false,
        success: function (data) {
            $("#AddBusinessElementModal").modal("hide");
            LoadPartialBusinessElement();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function LoadPartialBusinessElement() {
    var url = '/DNBMonitoring/BusinessElement/';
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $("#DivElementConditions").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$('body').on('click', '.DeleteMAinConditon', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var QueryString = $(this).attr("id");
    $.ajax({
        type: "POST",
        url: "/DNBMonitoring/DeleteMainConditon?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: "JSON",
        headers: { "__RequestVerificationToken": token },
        contentType: "application/json",
        success: function (data) {
            if (data.result) {
                LoadPartialBusinessElement();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
$('body').on('click', '.EditMAinConditon', function () {
    var QueryString = $(this).attr("id");
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/DNBMonitoring/AddBusinessElement?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#AddBusinessElementModalMain").html(data);
            DraggableModalPopup("#AddBusinessElementModal");
        }
    });
});

// for the pagination event 
$("body").on('change', '.pagevalueChangeCondition', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#SortBy").val();
    var SortOrder = $("#SortOrder").val();
    var ProfileId = $("#ProfileId").val();

    if (ProfileId == 0) {
        ProfileId = null;
    }
    var url = '/DNBMonitoring/BusinessElement/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder + "&ProfileId=" + ProfileId;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $("#divPartialMonitoringData").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});


function OnSuccess() {
    $('#divProgress').hide();
};

$('body').on('click', '#btnGrpSubmit', function () {
    var ConditionCount = $("#ConditionCount").val();
    if (parseInt(ConditionCount) <= 9) {
        var Conditon = $(this).parent().find('.GrpConditon').attr('data-val');
        var TempGrpId = $(this).parent().find('.GrpConditon').attr('data-Id');
        var Element = $(".Mainconditons .row:last").children().find('.elements').html();
        var ConditonOpetator = $(".Mainconditons .row:last").children().find('.Operators').html();
        var ConditonValue = $(".Mainconditons .row:last").children().find('.txtValue').val();
        var IsGroup = false;
        var GrpCondition = "AND";
        if (Conditon == "GrpAnd" || Conditon == "GrpOR" || Conditon == "And Group" || Conditon == "OR Group") {
            if (Conditon == "GrpAnd") {
                GrpCondition = "And";
            } else if (Conditon == "GrpOR") {
                GrpCondition = "OR";
            } else if (Conditon == "And Group") {
                GrpCondition = "AndGroup";
                IsGroup = true;
            }
            else if (Conditon == "OR Group") {
                GrpCondition = "ORGroup";
                IsGroup = true;
            }
            Conditon = "Group";
        }
        var IsCreate = true;
        var QueryString = "Conditon:" + Conditon + "@#$Element:" + Element + "@#$ConditonOpetator:" + ConditonOpetator + "@#$ConditonValue:" + ConditonValue + "@#$btnSubmit:" + "Add" + "@#$deletedId:" + null + "@#$TempGrpId:" + TempGrpId + "@#$GrpCondition:" + GrpCondition + "@#$IsCreate:" + IsCreate + "@#$IsGroup:" + IsGroup;
        $.ajax({
            type: "GET",
            url: "/DNBMonitoring/AddElementCondition?Parameters=" + ConvertEncrypte(encodeURI(QueryString)).split("+").join("***"),
            dataType: "html",
            contentType: "application/html",
            cache: false,
            success: function (data) {
                $("#ElementsList").html(data);
                var strCondition = $("#strCondition").each(function () {
                    $(".lblCondition").text("Your Condition should be : " + $(this).val());
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    } else {
        bootbox.alert({
            title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message,
            message: "You can add maximum 10 condition"
        });
    }
});


$('body').on('click', '#btnBackToProfile', function () {
    $("#BusinessElementModal").modal("hide");
});

function backToparent() {
    location.reload();
}







