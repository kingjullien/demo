var DragAndDropRightMenu = (function (DragAndDrop) {

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

    function itemDroppedRightMenu(event, ui) {

        var destinationFamilyTreeDetailId = $(this).closest("li").attr("data-val");
        var sourceFamilyTreeDetailId = ui.draggable.closest("li").attr("data-val");
        var token = $('input[name="__RequestVerificationToken"]').val();
        if (!ui.draggable.closest("span").hasClass("node-cpe1")) {
            bootbox.confirm({
                title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: copyNode, callback: function (result) {
                    if (result) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        var QueryString = "sourceFamilyTreeId:" + $("#LeftView_FamilyTreeId").val() + "@#$sourceFamilyTreeDetailId:" + sourceFamilyTreeDetailId + "@#$destinationFamilyTreeId:" + $("#RightView_FamilyTreeId").val() + "@#$destinationFamilyTreeDetailId:" + destinationFamilyTreeDetailId;
                        var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                        $.ajax({
                            type: "POST",
                            url: "/FamilyTree/CopyFamilyTree/",
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
        else {
            bootbox.confirm({
                title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: moveNode, callback: function (result) {
                    if (result) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        var QueryString = "sourceFamilyTreeId:" + $("#RightView_FamilyTreeId").val() + "@#$sourceFamilyTreeDetailId:" + sourceFamilyTreeDetailId + "@#$destinationFamilyTreeId:" + $("#RightView_FamilyTreeId").val() + "@#$destinationFamilyTreeDetailId:" + destinationFamilyTreeDetailId;
                        var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                        $.ajax({
                            type: "POST",
                            url: "/FamilyTree/MoveFamilyTree/",
                            data: JSON.stringify({ Parameters: Parameters }),
                            contentType: "application/json",
                            headers: { "__RequestVerificationToken": token },
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


    }

    DragAndDrop.enable = function (selector) {

        $(selector).find(".node-cpe1").draggable({
            helper: "clone"
            , zIndex: 10000
            , snapMode: "inner"
        });

        $(selector).find(".node-cpe1, .node-facility").droppable({
            activeClass: "active",
            hoverClass: "hover",
            accept: shouldAcceptDrop,
            over: itemOver,
            out: itemOut,
            drop: itemDroppedRightMenu,
            greedy: true,
            tolerance: "pointer"
        });

    };

    return DragAndDrop;

})(DragAndDropRightMenu || {});

var DragAndDropLeftMenu = (function (DragAndDrop) {

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

    function itemDroppedLeftMenu(event, ui) {

        var destinationFamilyTreeDetailId = $(this).closest("li").attr("data-val");
        var sourceFamilyTreeDetailId = ui.draggable.closest("li").attr("data-val");
        if (!ui.draggable.closest("span").hasClass("node-cpe1")) {
            bootbox.confirm({
                title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: copyNode, callback: function (result) {
                    if (result) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        var QueryString = "sourceFamilyTreeId:" + $("#LeftView_FamilyTreeId").val() + "@#$sourceFamilyTreeDetailId:" + sourceFamilyTreeDetailId + "@#$destinationFamilyTreeId:" + $("#RightView_FamilyTreeId").val() + "@#$destinationFamilyTreeDetailId:" + destinationFamilyTreeDetailId;
                        var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
                        $.ajax({
                            type: "POST",
                            url: "/FamilyTree/CopyFamilyTree/",
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
        else {
            bootbox.confirm({
                title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: moveNode, callback: function (result) {
                    if (result) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        var QueryString = "sourceFamilyTreeId:" + $("#RightView_FamilyTreeId").val() + "@#$sourceFamilyTreeDetailId:" + sourceFamilyTreeDetailId + "@#$destinationFamilyTreeId:" + $("#RightView_FamilyTreeId").val() + "@#$destinationFamilyTreeDetailId:" + destinationFamilyTreeDetailId;
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

    }

    DragAndDrop.enable = function (selector) {
        $(selector).find(".node-cpeLeft").draggable({
            helper: "clone"
            , zIndex: 10000
            , snapMode: "inner"
        });

        $(selector).find(".node-cpe1").draggable({
            helper: "clone"
            , zIndex: 10000
            , snapMode: "inner"
        });

        $(selector).find(".node-cpe1, .node-facility").droppable({
            activeClass: "active",
            hoverClass: "hover",
            accept: shouldAcceptDrop,
            over: itemOver,
            out: itemOut,
            drop: itemDroppedLeftMenu,
            greedy: true,
            tolerance: "pointer"
        });

    };

    return DragAndDrop;

})(DragAndDropLeftMenu || {});

function GetDataSideBySide(id, el, type) {
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
                    data: JSON.stringify({ id: id, type: type }),
                    contentType: "application/json",
                    success: function (response) {

                        if (response.Success) {
                            $(el).after(response.ResponseString);
                            $(el).attr("IsLoad", "True");
                            setTimeout(
                                function () {
                                    PartialPageRefreshSideBySide($(el).parent());

                                }, 500);

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $(el).attr("IsLoad", "False");
                        alert("Error");
                    }
                });
            }
        case 1:
        case "1":
        case "on":
        case "yes":
            return true;
        default:
            //PageRefresh();
            return false;
    }
}

function PartialPageRefreshSideBySide(el) {
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
    BindContextMenuRight();
    BindContextMenuLeft();
}


$(function () {
    $("#dragRootLeft, #dragRootRight").sortable({
        connectWith: ".connectedSortable"
    }).disableSelection();
});

$(document).ready(function () {

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

    BindContextMenuRight();
    BindContextMenuLeft();
})



function BindContextMenuRight() {
    var txtActivateFeature = $('#ActivateFeature').val();
    var isDisabled = false;

    var hdnIsEditable = $("#hdnIsEditableRight").val();

    if (hdnIsEditable == "1") {
        isDisabled = false;
    }
    else {
        isDisabled = true;
    }

    if (isDisabled) {

        var hdnIsLockForEdit = $("#hdnIsLockForEditRight").val();

        if (hdnIsLockForEdit == "1") {
            isDisabled = true;
        }
        else {
            isDisabled = false;
        }
    }
    if (!isDisabled) {
        DragAndDropRightMenu.enable("#dragRootRight");
    }

    $.contextMenu({
        selector: '.context-menu-one-right',
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
                    var hdnIsEditable1 = $("#hdnIsEditableRight").val();
                    if (hdnIsEditable1 == "1") {
                        isDisabled1 = false;
                    }
                    else {
                        isDisabled1 = true;
                    }
                    if (isDisabled1) {
                        var hdnIsLockForEdit1 = $("#hdnIsLockForEditRight").val();
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
                    var _sourceFamilyTreeId = 0;
                    if ($("#rdoSingle").is(":checked")) {
                        _sourceFamilyTreeId = $("#FamilyTreeId").val();
                    }
                    else {
                        _sourceFamilyTreeId = $("#RightView_FamilyTreeId").val();
                    }
                    bootbox.confirm({
                        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteNode, callback: function (result) {
                            if (result) {
                                var QueryString = "sourceFamilyTreeId:" + _sourceFamilyTreeId + "@#$sourceFamilyTreeDetailId:" + sourceFamilyTreeDetailId;
                                $.ajax({
                                    type: "POST",
                                    url: "/FamilyTree/DeleteFamilytreeNode/",
                                    data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(QueryString)).split("+").join("***") }),
                                    contentType: "application/json",
                                    headers: { "__RequestVerificationToken": token },
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
            },
            "Add Node": {
                name: addNode,
                disabled: function (key, opt) {
                    var isDisabled1 = false;
                    var hdnIsEditable1 = $("#hdnIsEditableRight").val();
                    if (hdnIsEditable1 == "1") {
                        isDisabled1 = false;
                    }
                    else {
                        isDisabled1 = true;
                    }
                    if (isDisabled1) {
                        var hdnIsLockForEdit1 = $("#hdnIsLockForEditRight").val();
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

function BindContextMenuLeft() {
    DragAndDropLeftMenu.enable("#dragRootLeft");
}