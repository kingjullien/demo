﻿function GetData(e, t) { var a = "False" == $(t).attr("IsLoad"), i = $('input[name="__RequestVerificationToken"]').val(); switch (a) { case !0: case "true": $(t).next("ul").length > 0 && $(t).next("ul").remove(), $.ajax({ type: "POST", url: "/FamilyTree/GetData/", data: JSON.stringify({ id: e }), contentType: "application/json", headers: { __RequestVerificationToken: i }, success: function (e) { e.Success && ($(t).after(e.ResponseString), $(t).attr("IsLoad", "True"), setTimeout(function () { PartialPageRefresh($(t).parent()) }, 500)) }, error: function (e, a, i) { $(t).attr("IsLoad", "False"), alert("Error") } }); case 1: case "1": case "on": case "yes": return !0; default: return !1 } } function RemovePageRefresh() { $(".tree > ul").removeAttr("role").find("ul").removeAttr("role"), $(el).find("li:has(ul)").removeClass("parent_li").removeAttr("role").find(" > span").removeAttr("title"), BindContextMenu() } function PartialPageRefresh(e) { $(".tree > ul").attr("role", "tree").find("ul").attr("role", "group"), $(e).find("li:has(ul)").addClass("parent_li").attr("role", "treeitem").find(" > span").attr("title", "Collapse this branch").on("click", function (e) { var t = $(this).parent("li.parent_li").find(" > ul > li"); t.is(":visible") ? (t.hide(), $(this).attr("title", "Expand this branch").find(" > i").removeClass().addClass("fa fa-lg fa-plus-circle")) : (t.show(), $(this).attr("title", "Collapse this branch").find(" > i").removeClass().addClass("fa fa-lg fa-minus-circle")), e.stopPropagation() }), BindContextMenu() } function PageRefresh() { $(".tree > ul").attr("role", "tree").find("ul").attr("role", "group"), $(".tree").find("li:has(ul)").addClass("parent_li").attr("role", "treeitem").find(" > span").attr("title", "Collapse this branch").on("click", function (e) { var t = $(this).parent("li.parent_li").find(" > ul > li"); t.is(":visible") ? (t.hide(), $(this).attr("title", "Expand this branch").find(" > i").removeClass().addClass("fa fa-lg fa-plus-circle")) : (t.show(), $(this).attr("title", "Collapse this branch").find(" > i").removeClass().addClass("fa fa-lg fa-minus-circle")), e.stopPropagation() }), BindContextMenu() } function BindContextMenu() { var e = !1; (e = "1" != $("#hdnIsEditable").val()) && (e = "1" == $("#hdnIsLockForEdit").val()); e || DragAndDrop.enable("#dragRoot"), $.contextMenu({ selector: ".context-menu-one", callback: function (e, t) { }, events: { show: function (e) { setTimeout(function () { e.$menu.find(".context-menu-disabled > span").attr("title", "To activate this feature or to learn more, please contact your Account executive or email us at sales@matchbookservices.com") }, 50) } }, items: { DeleteNode: { name: "Delete Node", disabled: function (e, t) { var a = !1; (a = "1" != $("#hdnIsEditable").val()) && (a = "1" == $("#hdnIsLockForEdit").val()); return a }, callback: function () { var e = $(this).attr("data-val"); bootbox.confirm({ title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> Confirm", message: "Are you sure you want to delete this node ?", callback: function (t) { if (t) { var a = $('input[name="__RequestVerificationToken"]').val(), i = "sourceFamilyTreeId:" + $("#FamilyTreeId").val() + "@#$sourceFamilyTreeDetailId:" + e, n = parent.ConvertEncrypte(encodeURI(i)).split("+").join("***"); $.ajax({ type: "POST", url: "/FamilyTree/DeleteFamilytreeNode/", data: JSON.stringify({ Parameters: n }), contentType: "application/json", headers: { __RequestVerificationToken: a }, success: function (e) { parent.ShowMessageNotification("success", e.Message, !0), e.result && window.parent.ChangeView() }, error: function (e, t, a) { } }) } } }) } }, "Add Node": { name: "Add Node", disabled: function (e, t) { var a = !1; (a = "1" != $("#hdnIsEditable").val()) && (a = "1" == $("#hdnIsLockForEdit").val()); return a }, callback: function () { var e = $(this).attr("data-val"); $.magnificPopup.open({ preloader: !1, closeBtnInside: !0, type: "iframe", items: { src: "/FamilyTree/AddFamilyNode?sourceFamilyTreeDetailId=" + e }, callbacks: { close: function () { } }, closeOnBgClick: !1, mainClass: "popupAddFamilyTreeNode" }) } } } }) } $(document).ready(function () { $(".tree > ul").attr("role", "tree").find("ul").attr("role", "group"), $(".tree").find("li:has(ul)").addClass("parent_li").attr("role", "treeitem").find(" > span").attr("title", "Collapse this branch").on("click", function (e) { var t = $(this).parent("li.parent_li").find(" > ul > li"); t.is(":visible") ? (t.hide(), $(this).attr("title", "Expand this branch").find(" > i").removeClass().addClass("fa fa-lg fa-plus-circle")) : (t.show(), $(this).attr("title", "Collapse this branch").find(" > i").removeClass().addClass("fa fa-lg fa-minus-circle")), e.stopPropagation() }), BindContextMenu() });