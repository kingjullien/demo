﻿function onLoadCleansMatchImportData() { var a = $('input[name="__RequestVerificationToken"]').val(); $(".SelectBox").each(function () { var e = $(this).closest("tr").find(".spnExcelColumn").attr("data-val"), t = 0; $(".SelectBox option").each(function () { var a = $(this).text(); e == a && (t = $(this).val()), "confidencecode" != a.toLowerCase() && "confidencecodes" != a.toLowerCase() || "confidencecodes" == e.toLowerCase() && (t = $(this).val()), "companygrade" == a.toLowerCase() && "mg_company" == e.toLowerCase() && (t = $(this).val()), "streetgrade" == a.toLowerCase() && "mg_streetno" == e.toLowerCase() && (t = $(this).val()), "streetnamegrade" == a.toLowerCase() && "mg_streetname" == e.toLowerCase() && (t = $(this).val()), "citygrade" == a.toLowerCase() && "mg_city" == e.toLowerCase() && (t = $(this).val()), "stategrade" == a.toLowerCase() && "mg_state" == e.toLowerCase() && (t = $(this).val()), "addressgrade" == a.toLowerCase() && "mg_pobox" == e.toLowerCase() && (t = $(this).val()), "phonegrade" == a.toLowerCase() && "mg_phone" == e.toLowerCase() && (t = $(this).val()), "zipgrade" == a.toLowerCase() && "mg_postalcode" == e.toLowerCase() && (t = $(this).val()), "density" == a.toLowerCase() && "mg_density" == e.toLowerCase() && (t = $(this).val()), "uniqueness" == a.toLowerCase() && "mg_uniqueness" == e.toLowerCase() && (t = $(this).val()), "sic" == a.toLowerCase() && "mg_sic" == e.toLowerCase() && (t = $(this).val()), "companycode" == a.toLowerCase() && "mdp_company" == e.toLowerCase() && (t = $(this).val()), "streetcode" == a.toLowerCase() && "mdp_streetno" == e.toLowerCase() && (t = $(this).val()), "streetnamecode" == a.toLowerCase() && "mdp_streetname" == e.toLowerCase() && (t = $(this).val()), "citycode" == a.toLowerCase() && "mdp_city" == e.toLowerCase() && (t = $(this).val()), "statecode" == a.toLowerCase() && "mdp_state" == e.toLowerCase() && (t = $(this).val()), "addresscode" == a.toLowerCase() && "mdp_pobox" == e.toLowerCase() && (t = $(this).val()), "phonecode" == a.toLowerCase() && "mdp_phone" == e.toLowerCase() && (t = $(this).val()), "tags" == a.toLowerCase() && "tags" == e.toLowerCase() && (t = $(this).val()), "excludefromautoaccept" == a.toLowerCase() && "excludefromautoaccept" == e.toLowerCase() && (t = $(this).val()), "groupname" != a.toLowerCase() && "countrygroupname" != a.toLowerCase() || "countrygroupname" == e.toLowerCase() && (t = $(this).val()) }), $(this).val(t); var o = $(this).attr("id"), n = $(this).val(); 0 != parseInt(n) ? $.ajax({ type: "POST", url: "/DandB/UpdateExamples", data: JSON.stringify({ CurrentColumn: n }), dataType: "json", headers: { __RequestVerificationToken: a }, contentType: "application/json", success: function (a) { $("#" + o).parent().next().text(a) }, error: function (a, e, t) { } }) : $("#" + o).parent().next().text("") }), $(".chzn-select").length > 0 && $(".chzn-select").chosen().change(function (a) { a.target == this && $("#Tags").val($(this).val()) }), parseInt($("#DataColumn-0").val()) > 0 && parseInt($("#DataColumn-1").val()) > 0 && parseInt($("#DataColumn-2").val()) > 0 && parseInt($("#DataColumn-3").val()) > 0 && parseInt($("#DataColumn-4").val()) > 0 && parseInt($("#DataColumn-5").val()) > 0 && parseInt($("#DataColumn-6").val()) > 0 && parseInt($("#DataColumn-7").val()) > 0 && parseInt($("#DataColumn-8").val()) > 0 && parseInt($("#DataColumn-9").val()) > 0 && parseInt($("#DataColumn-10").val()) > 0 && parseInt($("#DataColumn-11").val()) > 0 && parseInt($("#DataColumn-12").val()) > 0 && parseInt($("#DataColumn-13").val()) > 0 && parseInt($("#DataColumn-14").val()) > 0 && parseInt($("#DataColumn-15").val()) > 0 && parseInt($("#DataColumn-16").val()) > 0 && parseInt($("#DataColumn-17").val()) > 0 && parseInt($("#DataColumn-18").val()) > 0 && parseInt($("#DataColumn-20").val()) > 0 && parseInt($("#DataColumn-21").val()) > 0 ? $("#btnInsertData").attr("disabled", !1) : $("#btnInsertData").attr("disabled", "disabled") } function ReloadWholePage() { window.parent.$.magnificPopup.close(); window.location.href = "/DandB/Index" } $("body").on("click", "#btnSubmitImportData", function () { var a = new FormData; if (null != $("#DAndBAutoAcceptanceImportModal #file")[0].files[0]) { a.append("file", $("#DAndBAutoAcceptanceImportModal #file")[0].files[0]), a.append("header", $("#WithHeader").prop("checked")); var e = $('input[name="__RequestVerificationToken"]').val(); $.ajax({ type: "POST", url: "/DandB/ImportData", data: a, headers: { __RequestVerificationToken: e }, dataType: "json", contentType: !1, processData: !1, async: !1, success: function (a) { return "Only formats allowed are :xls,xlsx" == a ? (ShowMessageNotification("success", a, !1), $("#DAndBAutoAcceptanceImportModal #file").val(""), !1) : a.indexOf("already belongs to this file") > -1 ? (ShowMessageNotification("success", a, !1), !1) : a.indexOf("Error:") > -1 ? (ShowMessageNotification("success", a, !1), !1) : void ("success" == a && parent.CloseImportPanel()) }, error: function (a, e, t) { } }) } else parent.ShowMessageNotification("success", selectFile, !1); return !1 }), $("body").on("change", ".SelectBox", function () { var a = $(this).attr("id"), e = $(this).val(), t = $('input[name="__RequestVerificationToken"]').val(); parseInt($("#DataColumn-0").val()) > 0 && parseInt($("#DataColumn-1").val()) > 0 && parseInt($("#DataColumn-2").val()) > 0 && parseInt($("#DataColumn-3").val()) > 0 && parseInt($("#DataColumn-4").val()) > 0 && parseInt($("#DataColumn-5").val()) > 0 && parseInt($("#DataColumn-6").val()) > 0 && parseInt($("#DataColumn-7").val()) > 0 && parseInt($("#DataColumn-8").val()) > 0 && parseInt($("#DataColumn-9").val()) > 0 && parseInt($("#DataColumn-10").val()) > 0 && parseInt($("#DataColumn-11").val()) > 0 && parseInt($("#DataColumn-12").val()) > 0 && parseInt($("#DataColumn-13").val()) > 0 && parseInt($("#DataColumn-14").val()) > 0 && parseInt($("#DataColumn-15").val()) > 0 && parseInt($("#DataColumn-16").val()) > 0 && parseInt($("#DataColumn-17").val()) > 0 && parseInt($("#DataColumn-18").val()) > 0 && parseInt($("#DataColumn-20").val()) > 0 && parseInt($("#DataColumn-21").val()) > 0 ? $("#btnInsertData").attr("disabled", !1) : $("#btnInsertData").attr("disabled", "disabled"), 0 != parseInt(e) ? $.ajax({ type: "POST", url: "/DandB/UpdateExamples", data: JSON.stringify({ CurrentColumn: e }), dataType: "json", contentType: "application/json", headers: { __RequestVerificationToken: t }, success: function (e) { $("#" + a).parent().next().text(e) }, error: function (a, e, t) { } }) : $("#" + a).parent().next().text("") }), $("body").on("click", "#btnInsertData", function () { var a = [], e = [], t = [], o = 0, n = 0, s = $("#Tags").val(), r = $("#IsTag").val(), l = $("#IsCompanyScore").val(), c = 0, i = $("#LicenseEnableTags").val(); if (null != s && "0" != s || (s = ""), $(".SelectBox").each(function () { a.push($(this).val()), o += 1, "0" == $(this).val() && 1, 20 == o && "False" == r && "true" == i.toLowerCase() ? e.push("Tags") : e.push($(this).find(":selected").text()), $(this).parent().removeClass("has-error") }), "false" == l.toLowerCase() && (c = "" == $("#txtCmpnyScore").val() ? 0 : $("#txtCmpnyScore").val(), e.push("CompanyScore")), a.length > 0) for (var p = 0; p < a.length; p++)for (var u = p; u < a.length; u++)if (p != u && a[p] == a[u] && (1, "0" != a[p])) { if ("False" == r) { var m = $("#DataColumn-" + p).val(); parseInt(m) > 0 && $("#DataColumn-" + p).parent().addClass("has-error"); m = $("#DataColumn-" + u).val(); parseInt(m) > 0 && $("#DataColumn-" + u).parent().addClass("has-error") } else { $("#DataColumn-" + p).parent().addClass("has-error"); m = $("#DataColumn-" + u).val(); parseInt(m) > 0 && $("#DataColumn-" + u).parent().addClass("has-error") } n += 1 } if ($(".spnExcelColumn").each(function () { var a = $(this).attr("data-val"); t.push(a) }), 0 == parseInt($("#DataColumn-0").val()) && 0 == parseInt($("#DataColumn-1").val()) && 0 == parseInt($("#DataColumn-2").val()) && 0 == parseInt($("#DataColumn-3").val()) && 0 == parseInt($("#DataColumn-4").val()) && 0 == parseInt($("#DataColumn-5").val()) && 0 == parseInt($("#DataColumn-6").val()) && 0 == parseInt($("#DataColumn-7").val()) && 0 == parseInt($("#DataColumn-8").val()) && 0 == parseInt($("#DataColumn-9").val()) && 0 == parseInt($("#DataColumn-10").val()) && 0 == parseInt($("#DataColumn-11").val()) && 0 == parseInt($("#DataColumn-12").val()) && 0 == parseInt($("#DataColumn-13").val()) && 0 == parseInt($("#DataColumn-14").val()) && 0 == parseInt($("#DataColumn-15").val()) && 0 == parseInt($("#DataColumn-16").val()) && 0 == parseInt($("#DataColumn-17").val()) && 0 == parseInt($("#DataColumn-18").val()) && 0 == parseInt($("#DataColumn-20").val()) && 0 == parseInt($("#DataColumn-21").val())) return !1; if (n > 0) return !1; var C = ""; if (e.length > 0) for (p = 0; p < e.length; p++)C = "" == C ? e[p] : C + "," + e[p]; var d = ""; if (t.length > 0) for (p = 0; p < t.length; p++)d = "" == d ? t[p] : d + "," + t[p]; if ($(".norecored").is(":visible")) parent.ShowMessageNotification("success", "Please Upload Data first.", !0), bootbox.hideAll(); else { var v = $('input[name="__RequestVerificationToken"]').val(), h = 0; $.ajax({ type: "POST", url: "/DandB/GetSecondaryAutoAcceptanceCriteriaGroupCount", dataType: "json", contentType: "application/json", async: !1, success: function (a) { h = a, $("#DAndBAutoAcceptImportModal").modal("hide") }, error: function (a, e, t) { } }), 0 == h ? $.ajax({ type: "POST", url: "/DandB/CleanseMatchDataMatchAutoAccept", data: JSON.stringify({ OrgColumnName: C, ExcelColumnName: d, Tags: s, IsOverWrite: !1, CompanyScore: c }), headers: { __RequestVerificationToken: v }, dataType: "json", contentType: "application/json", async: !1, cache: !1, success: function (a) { $("#DAndBAutoAcceptImportModal").modal("hide"), parent.ShowMessageNotification("success", a, !0, !0, parent.ClosePopupReload) }, error: function (a, e, t) { } }) : bootbox.confirm({ title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: overwrite, callback: function (a) { return a ? (bootbox.hideAll(), bootbox.confirm({ title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: overwriteRulesConfirmMessage, callback: function (o) { bootbox.hideAll(), o ? $.ajax({ type: "GET", url: "/DandB/DeleteComment?CriteriaId=0&ToCall=ImportData&OrgColumnName=" + e + "&ExcelColumnName=" + t + "&Tags=" + s + "&IsOverWrite=" + a + "&CompanyScore=" + c, dataType: "HTML", async: !1, success: function (a) { $("#divProgress").hide(), $("#DeleteAutoAcceptanceModalMain").html(a), DraggableModalPopup("#DeleteAutoAcceptanceDataModal") } }) : $.ajax({ type: "POST", url: "/DandB/CleanseMatchDataMatchAutoAccept", data: JSON.stringify({ OrgColumnName: C, ExcelColumnName: d, Tags: s, IsOverWrite: o, CompanyScore: c }), headers: { __RequestVerificationToken: v }, dataType: "json", contentType: "application/json", async: !1, cache: !1, success: function (a) { $("#DAndBAutoAcceptImportModal").modal("hide"), parent.ShowMessageNotification("success", a, !0, !0, parent.ClosePopupReload), bootbox.hideAll() }, error: function (a, e, t) { } }) } })) : $.ajax({ type: "POST", url: "/DandB/CleanseMatchDataMatchAutoAccept", data: JSON.stringify({ OrgColumnName: C, ExcelColumnName: d, Tags: s, IsOverWrite: a, CompanyScore: c }), headers: { __RequestVerificationToken: v }, dataType: "json", contentType: "application/json", async: !1, cache: !1, success: function (a) { bootbox.hideAll(), parent.ShowMessageNotification("success", a, !0, !0, parent.ClosePopupReload) }, error: function (a, e, t) { } }), !0 } }) } return !1 }), $("body").on("change", "#DAndBAutoAcceptanceImportModal #file", function () { if ("" != $("#DAndBAutoAcceptanceImportModal #file").val()) { var a = ["xls", "xlsx"]; -1 == $.inArray($(this).val().split(".").pop().toLowerCase(), a) && (parent.ShowMessageNotification("success", "Only formats allowed are : " + a.join(", "), !1), $("#DAndBAutoAcceptanceImportModal #file").val("")) } }), $("#txtCmpnyScore").keyup(function (a) { var e = parseInt("" == $(this).val() ? "0" : $(this).val()); e > 100 ? $("#txtCmpnyScore").val(0) : $("#txtCmpnyScore").val(e) }), $(document).on("blur", "#txtCmpnyScore", function () { var a = $(this).val(); "" != a && ($.isNumeric(a) ? parseInt("" == a ? "0" : a) > 100 ? $("#txtCmpnyScore").val(0) : $("#txtCmpnyScore").val(a) : ($("#txtCmpnyScore").val(0), parent.ShowMessageNotification("success", "Only numeric values accepted.", !0))) });