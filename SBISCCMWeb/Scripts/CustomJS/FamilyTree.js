$("body").on('click', '.lnkCorporateLinkage', function () {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/FamilyTree/GetCorporateLinkageDuns",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#FamilyTreeModalMain").html(data);
            DraggableModalPopup("#FamilyTreeModal");
        }
    });
});

$(function () {
    $("#rdoSingle").attr("checked", true);
});

function SelectCorporateLinkageDuns(duns) {
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: addTree, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/FamilyTree/AddCorporateLinkageDuns/",
                    data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(duns)).split("+").join("***")  }),
                    contentType: "application/json",
                    success: function (data) {
                        ShowMessageNotification("success", data.Message, true);
                        if (data) {
                            ChangeView();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                    }
                });
            }
        }
    });
}

function CreateNewFamilyTree() {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/FamilyTree/AddFamilyTree",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#AddFamilyTreeModalMain").html(data);
            DraggableModalPopup("#AddFamilyTreeModal");
        }
    });
}

function DeleteFamilyTree() {

    var _FamilyTreeId = 0;
    if ($("#rdoSingle").is(":checked")) {
        _FamilyTreeId = $("#FamilyTreeId").val();
    }
    else {
        _FamilyTreeId = $("#RightView_FamilyTreeId").val();
    }
    if (_FamilyTreeId == "") {
        ShowMessageNotification("success", selectTree, false);
        return false;
    }
    if ($("#FamilyTreeId").val() != '' || $("#RightView_FamilyTreeId").val() != '') {
        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteFamilyTree, callback: function (result) {
                if (result) {
                    var token = $('input[name="__RequestVerificationToken"]').val();
                    $.ajax({
                        type: "POST",
                        url: "/FamilyTree/DeleteFamilyTree?Parameters=" + ConvertEncrypte(encodeURI(_FamilyTreeId)).split("+").join("***"),
                        dataType: "JSON",
                        contentType: "application/json",
                        headers: { "__RequestVerificationToken": token },
                        cache: false,
                        success: function (data) {
                            ShowMessageNotification("success", data.Message, true);
                            if (data.result) {
                                ChangeView();
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                        }
                    });
                }
            }
        });
    }
}

function DuplicateFamilyTree() {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/FamilyTree/DuplicateFamilyTree",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#DuplicateFamilyTreeModalMain").html(data);
            DraggableModalPopup("#DuplicateFamilyTreeModal");
        }
    });
}

var DragAndDrop = (function (DragAndDrop) {
    
    function shouldAcceptDrop(item) {
        var $target = $(this).closest("li");
        var $item = item.closest("li");

        if ($.contains($item[0], $target[0])) {
            // can't drop on one of your children!
            return false;
        }

        return true;

    }

    function itemOver(event, ui) {
    }

    function itemOut(event, ui) {
    }

    function itemDropped(event, ui) {
        var destinationFamilyTreeDetailId = $(this).closest("li").attr("data-val");
        var sourceFamilyTreeDetailId = ui.draggable.closest("li").attr("data-val");

        var $target = $(this).closest("li");
        var $item = ui.draggable.closest("li");

        bootbox.confirm({
            title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: moveNode, callback: function (result) {
                if (result) {
                    var token = $('input[name="__RequestVerificationToken"]').val();
                    var QueryString = "sourceFamilyTreeId:" + $("#FamilyTreeId").val() + "@#$sourceFamilyTreeDetailId:" + sourceFamilyTreeDetailId + "@#$destinationFamilyTreeId:" + $("#FamilyTreeId").val() + "@#$destinationFamilyTreeDetailId:" + destinationFamilyTreeDetailId;
                    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                    $.ajax({
                        type: "POST",
                        url: "/FamilyTree/MoveFamilyTree/",
                        data: JSON.stringify({ Parameters: Parameters }),
                        headers: { "__RequestVerificationToken": token },
                        contentType: "application/json",
                        success: function (data) {
                            if (data) {
                                ChangeView();
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {

                        }
                    });
                }
            }
        });


    }

    DragAndDrop.enable = function (selector) {
        $(selector).find(".node-cpe").draggable({
            helper: "clone"
            , zIndex: 10000
            , snapMode: "inner"
        });

        $(selector).find(".node-cpe, .node-facility").droppable({
            activeClass: "active",
            hoverClass: "hover",
            accept: shouldAcceptDrop,
            over: itemOver,
            out: itemOut,
            drop: itemDropped,
            greedy: true,
            tolerance: "pointer"
        });

    };

    return DragAndDrop;

})(DragAndDrop || {});

function AddNode() {
    var sourceFamilyTreeDetailId = "";
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/FamilyTree/AddFamilyNode?sourceFamilyTreeDetailId=" + parent.ConvertEncrypte(encodeURI(sourceFamilyTreeDetailId)).split("+").join("***"),
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#AddFamilyNodeModalMain").html(data);
            DraggableModalPopup("#AddFamilyNodeModal");
        }
    });
}

function ChangeView() {
    var isSingleView = 1;
    if ($("#rdoSingle").is(":checked")) {
        isSingleView = 1;
    }
    else {
        isSingleView = 0;
    }

    var id = 0;
    if ($("#FamilyTreeId").length > 0) {
        id = $("#FamilyTreeId").val();
    }
    if (id == '') {
        id = 0;
    }

    var leftId = 0;
    if ($("#LeftView_FamilyTreeId").length > 0) {
        leftId = $("#LeftView_FamilyTreeId").val();
    }
    if (leftId == '') {
        leftId = 0;
    }

    var rightId = 0;
    if ($("#RightView_FamilyTreeId").length > 0) {
        rightId = $("#RightView_FamilyTreeId").val();
    }
    if (rightId == '') {
        rightId = 0;
    }

    var token = $('input[name="__RequestVerificationToken"]').val();
    var QueryString = "isSingleView:" + isSingleView + "@#$id:" + id + "@#$leftId:" + leftId + "@#$rightId:" + rightId;
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: "POST",
        url: "/FamilyTree/ViewChange/",
        data: JSON.stringify({ Parameters: Parameters }),
        contentType: "application/json",
        headers: { "__RequestVerificationToken": token },

        success: function (response) {
            if (response.Success) {
                $("#loadTree").html('');
                $("#loadTree").html(response.ResponseString);
                PageRefresh();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
}

function BindFamilyTree(type, valuea) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var dddtype = valuea;
    $.ajax({
        type: "POST",
        url: "/FamilyTree/BindFamilyTree/",
        headers: { "__RequestVerificationToken": token },
        data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(dddtype)).split("+").join("***") }),
        contentType: "application/json",
        success: function (response) {
            if (response.Success) {

                if (type == "SinglePanel") {
                    $("#FamilyTreeId").empty();
                    $("#FamilyTreeId").append($("<option></option>").val("").html("--Select--"));
                    $.each(response.Object, function (e, value) {
                        $("#FamilyTreeId").append($("<option></option>").val(value.Key).html(value.Value));
                    });
                }
                else if (type == "SideBySideLeft") {
                    $("#LeftView_FamilyTreeId").empty();
                    $("#LeftView_FamilyTreeId").append($("<option></option>").val("").html("--Select--"));
                    $.each(response.Object, function (e, value) {
                        $("#LeftView_FamilyTreeId").append($("<option></option>").val(value.Key).html(value.Value));
                    });
                }
                else if (type == "SideBySideRight") {
                    $("#RightView_FamilyTreeId").empty();
                    $("#RightView_FamilyTreeId").append($("<option></option>").val("").html("--Select--"));
                    $.each(response.Object, function (e, value) {
                        $("#RightView_FamilyTreeId").append($("<option></option>").val(value.Key).html(value.Value));
                    });
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
}

function getTreeValue() {
    $("#dragRoot").children().each(function () {
    });
}
function GetData(id, el) {
    var isTrueSet = ($(el).attr("IsLoad") == 'False');

    switch (isTrueSet) {
        case true:
        case "true":
            {
                if ($(el).next('ul').length > 0) {
                    $(el).next('ul').remove();
                }

                $.ajax({
                    type: "POST",
                    url: "/FamilyTree/GetData/",
                    data: JSON.stringify({ id: id }),
                    contentType: "application/json",
                    success: function (response) {
                        if (response.Success) {
                            $(el).after(response.ResponseString);
                            $(el).attr("IsLoad", "True");
                            setTimeout(
                                function () {
                                    PartialPageRefresh($(el).parent());
                                }, 500);
                            //$(el).click();
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $(el).attr("IsLoad", "False");
                        //alert("Error");
                        ShowMessageNotification("error", data.Message, false);
                    }
                });
            }
        case 1:
        case "1":
        case "on":
        case "yes":
            return true;
        default:
            return false;
    }



}

function RemovePageRefresh() {
    $('.tree > ul').removeAttr('role').find('ul').removeAttr('role');

    $(el).find('li:has(ul)').removeClass('parent_li').removeAttr('role').find(' > span').removeAttr('title');
    BindContextMenu();
}

function PartialPageRefresh(el) {
    $('.tree > ul').attr('role', 'tree').find('ul').attr('role', 'group');

    $(el).find('li:has(ul)').addClass('parent_li').attr('role', 'treeitem').find(' > span').attr('title', 'Collapse this branch').on('click', function (e) {

        var children = $(this).parent('li.parent_li').find(' > ul > li');
        if (children.is(':visible')) {
            children.hide();
            $(this).attr('title', 'Expand this branch').find(' > i').removeClass().addClass('fa fa-lg fa-plus-circle');
        } else {
            children.show();
            $(this).attr('title', 'Collapse this branch').find(' > i').removeClass().addClass('fa fa-lg fa-minus-circle');
        }
        e.stopPropagation();
    });
    BindContextMenu();
}

function PageRefresh() {
    $('.tree > ul').attr('role', 'tree').find('ul').attr('role', 'group');

    $('.tree').find('li:has(ul)').addClass('parent_li').attr('role', 'treeitem').find(' > span').attr('title', 'Collapse this branch').on('click', function (e) {

        var children = $(this).parent('li.parent_li').find(' > ul > li');
        if (children.is(':visible')) {
            children.hide();
            $(this).attr('title', 'Expand this branch').find(' > i').removeClass().addClass('fa fa-lg fa-plus-circle');
        } else {
            children.show();
            $(this).attr('title', 'Collapse this branch').find(' > i').removeClass().addClass('fa fa-lg fa-minus-circle');
        }
        e.stopPropagation();
    });
    BindContextMenu();
}

function BindContextMenu() {
    var txtActivateFeature = $('#ActivateFeature').val();
    var isDisabled = false;

    var hdnIsEditable = $("#hdnIsEditable").val();

    if (hdnIsEditable == "1") {
        isDisabled = false;
    }
    else {
        isDisabled = true;
    }

    if (isDisabled) {

        var hdnIsLockForEdit = $("#hdnIsLockForEdit").val();

        if (hdnIsLockForEdit == "1") {
            isDisabled = true;
        }
        else {
            isDisabled = false;
        }
    }
    if (!isDisabled) {
        DragAndDrop.enable("#dragRoot");
    }

    $.contextMenu({
        selector: '.context-menu-one',
        callback: function (key, options) {
        },
        events: {
            show: function (opt) {
                setTimeout(function () {
                    opt.$menu.find('.context-menu-disabled > span').attr('title', txtActivateFeature);
                }, 50);
            }
        },
        items: {
            "DeleteNode": {
                name: deleteNodeSingle,
                disabled: function (key, opt) {
                    var isDisabled1 = false;
                    var hdnIsEditable1 = $("#hdnIsEditable").val();
                    if (hdnIsEditable1 == "1") {
                        isDisabled1 = false;
                    }
                    else {
                        isDisabled1 = true;
                    }
                    if (isDisabled1) {
                        var hdnIsLockForEdit1 = $("#hdnIsLockForEdit").val();
                        if (hdnIsLockForEdit1 == "1") {
                            isDisabled1 = true;
                        }
                        else {
                            isDisabled1 = false;
                        }
                    }
                    return isDisabled1;
                }, callback: function () {
                    var sourceFamilyTreeDetailId = $(this).attr("data-val");
                    var token = $('input[name="__RequestVerificationToken"]').val();
                    bootbox.confirm({
                        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteNode, callback: function (result) {
                            if (result) {
                                var QueryString = "sourceFamilyTreeId:" + $("#FamilyTreeId").val() + "@#$sourceFamilyTreeDetailId:" + sourceFamilyTreeDetailId;
                                $.ajax({
                                    type: "POST",
                                    url: "/FamilyTree/DeleteFamilytreeNode/",
                                    data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(QueryString)).split("+").join("***") }),
                                    contentType: "application/json",
                                    headers: { "__RequestVerificationToken": token },
                                    success: function (data) {
                                        parent.ShowMessageNotification("success", data.Message, false);
                                        if (data.result) {
                                            window.parent.ChangeView();
                                        }
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {

                                    }
                                });
                            }
                        }
                    });
                }

            },
            "Add Node": {
                name: addNode,
                disabled: function (key, opt) {
                    var isDisabled1 = false;
                    var hdnIsEditable1 = $("#hdnIsEditable").val();
                    if (hdnIsEditable1 == "1") {
                        isDisabled1 = false;
                    }
                    else {
                        isDisabled1 = true;
                    }
                    if (isDisabled1) {
                        var hdnIsLockForEdit1 = $("#hdnIsLockForEdit").val();
                        if (hdnIsLockForEdit1 == "1") {
                            isDisabled1 = true;
                        }
                        else {
                            isDisabled1 = false;
                        }
                    }
                    return isDisabled1;
                }
                , callback: function () {
                    var sourceFamilyTreeDetailId = $(this).attr("data-val");
                    // Changes for Converting magnific popup to modal popup
                    $.ajax({
                        type: 'GET',
                        url: "/FamilyTree/AddFamilyNode?Parameters=" + ConvertEncrypte(encodeURI(sourceFamilyTreeDetailId)).split("+").join("***"),
                        dataType: 'HTML',
                        async: false,
                        success: function (data) {
                            $("#AddFamilyNodeModalMain").html(data);
                            DraggableModalPopup("#AddFamilyNodeModal");
                        }
                    });
                }
            },
        }
    });
}
function AddFamilyTree() {
    var cnt = 0;
    var FamilyTreeName = $("#txtFamilyTreeName").val();
    var FamilyTreeType = $("#txtFamilyTreeType").val();
    var AlternateId = $("#txtAlternateId").val();
    if (FamilyTreeName != undefined && FamilyTreeName.trim() != "") {
        $("#spnFamilyTreeName").hide();
    }
    else {
        cnt++;
        $("#spnFamilyTreeName").show();

    }
    if (FamilyTreeType != undefined && FamilyTreeType.trim() != "") {
        $("#spnFamilyTreeType").hide();
    }
    else {
        cnt++;
        $("#spnFamilyTreeType").show();

    }
    if (AlternateId != undefined && AlternateId.trim() != "") {
        $("#spnAlternateId").hide();
    }
    else {
        cnt++;
        $("#spnAlternateId").show();
    }
    if (cnt > 0) {
        return false;
    }
    var QueryString = "FamilyTreeName:" + FamilyTreeName + "@#$FamilyTreeType:" + FamilyTreeType + "@#$AlternateId:" + AlternateId;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: "/FamilyTree/AddFamilyTree/",
        data: JSON.stringify({ Parameters: Parameters }),
        contentType: "application/json",
        headers: { "__RequestVerificationToken": token },
        success: function (data) {
            $("#AddFamilyTreeModal").modal("hide");
            parent.ShowMessageNotification("success", data.Message, false);
            if (data.result) {
                window.parent.ChangeView();
                window.parent.$.magnificPopup.close();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
}
function ConvertEncrypte(value) {
    var newvalue = "";
    if (value != '' && value != undefined) {
        $.ajax({
            type: "POST",
            url: "/Home/GetEncryptedString",
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