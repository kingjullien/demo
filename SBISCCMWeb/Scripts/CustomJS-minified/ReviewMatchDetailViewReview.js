﻿function nextClick(t) { if ("disabled" != $(t).parent().attr("class")) { var e = $(t).attr("data-val"), a = $(t).attr("data-nextduns"), n = $(t).attr("data-next"), d = $(t).attr("data-Id"), s = $("#TopMatchCandidate").val(), r = $("#CountryGroup").val(), l = $("#OrderBy").val(); null == n && (n = ""), null == d && (d = ""); var c = "/Review/MatchDetailView_Item?Parameters=" + ConvertEncrypte(encodeURI("id:" + e + "@#$TopMatchCandidate:" + s + "@#$CountryGroup:" + r + "@#$dataNext:" + n + "@#$dataPrev:" + d + "@#$DUNS:" + a + "@#$OrderBy:" + l + "@#$IsPartialView:true")).split("+").join("***"); $.ajax({ type: "GET", url: c, dataType: "HTML", async: !1, cache: !1, contentType: "application/html", data: { pagevalue: $("#pagevalue").val() }, success: function (t) { $("#MatchedcontentPopup").html(t) } }), parent.matchItemNextClick($(t).data("val"), $(t).attr("data-Id")) } } function prevClick(t) { if ("disabled" != $(t).parent().attr("class")) { var e = $(t).attr("data-val"), a = $(t).attr("data-prevduns"), n = $(t).attr("data-Id"), d = $(t).attr("data-prev"), s = $("#TopMatchCandidate").val(), r = $("#CountryGroup").val(), l = $("#OrderBy").val(); null == n && (n = ""), null == d && (d = ""); var c = "/Review/MatchDetailView_Item?Parameters=" + ConvertEncrypte(encodeURI("id:" + e + "@#$TopMatchCandidate:" + s + "@#$CountryGroup:" + r + "@#$dataNext:" + n + "@#$dataPrev:" + d + "@#$DUNS:" + a + "@#$OrderBy:" + l + "@#$IsPartialView:true")).split("+").join("***"); $.ajax({ type: "GET", url: c, dataType: "HTML", async: !1, cache: !1, contentType: "application/html", data: { pagevalue: $("#pagevalue").val() }, success: function (t) { $("#MatchedcontentPopup").html(t) } }), parent.matchItemNextClick($(t).data("val"), $(t).attr("data-Id")) } } function ConvertEncrypte(t) { var e = ""; return "" != t && null != t && $.ajax({ type: "POST", url: "/Home/GetEncryptedString", data: JSON.stringify({ strValue: t }), dataType: "json", contentType: "application/json", async: !1, success: function (t) { e = t }, error: function (t, e, a) { } }), e } $(function () { $(".mfp-iframe").contents().find("html body #contentPopup .btnUnselected,html body #contentPopup .btnSelected").each(function () { var t = $("div#" + this.id.replace(" ", "")).attr("class"); $(this).removeAttr("class").addClass(t), t.indexOf("btnSelected") > -1 && $(this).parent().find(".displaylbl").text("Selected") }), $(".mfp-iframe").contents().find("html body #contentPopup .btnUnselected, html body #contentPopup .btnSelected").click(function () { if ("btnUnselected" == $(this)[0].classList[2]) { var t = $(this)[0].classList[0]; $("div#" + this.id).parent().parent().parent().parent().parent().parent().parent().prev("tr").find("." + t).removeClass("parentbtnMatchRejected").addClass("parentbtnUnselected"), $("div#" + this.id).parent().parent().parent().parent().parent().parent().parent().prev("tr").find("." + t).removeClass("parentbtnIdel").addClass("parentbtnUnselected"), $("div#" + this.id).parent().parent().parent().parent().parent().parent().parent().prev("tr").find("." + t).parent().prev().removeClass("NotSelected").addClass("SelectMathced"), $("." + t + ".btnSelected").each(function () { $(this).removeClass("btnSelected").addClass("btnUnselected") }), $(this).removeClass("btnUnselected").addClass("btnSelected"), $("div#" + this.id.replace(" ", "")).removeClass("btnUnselected").addClass("btnSelected"), CallAcceptLCMMatches(this.id), $(this).parent().find(".displaylbl").text("Selected") } else if ("btnSelected" == $(this)[0].classList[2]) { t = $(this)[0].classList[0]; $("." + t + ".btnSelected").each(function () { $(this).removeClass("btnSelected").addClass("btnUnselected") }), $(this).removeClass("btnSelected").addClass("btnUnselected"), $("div#" + this.id).removeClass("btnSelected").addClass("btnUnselected"); var e = "#" + t; $(".trStewardshipPortal_td12").find(e).parent().prev().removeClass("SelectMathced").addClass("NotSelected"), $(this).parent().find(".displaylbl").text("Select"), CallAcceptLCMMatches(this.id) } }) }), $("body").on("click", ".btnAddCompany", function () { var t = $('input[name="__RequestVerificationToken"]').val(), e = $(this).attr("data-val"); return $.ajax({ type: "POST", url: "/BadInputData/FillMatchData/", data: JSON.stringify({ matchRecord: e }), headers: { __RequestVerificationToken: t }, dataType: "json", contentType: "application/json", success: function (t) { "success" == t && $.magnificPopup.open({ preloader: !1, closeBtnInside: !0, type: "iframe", items: { src: "/BadInputData/AddCompany" }, callbacks: { close: function () { } }, closeOnBgClick: !1, mainClass: "popupAddressCompany" }) }, error: function (t, e, a) { } }), !1 });