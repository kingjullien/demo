﻿function closemsg(e) { e.parentNode.hidden = !0 } function MatchGradeValueChnage() { $("#form_windowAAC .chzn-select").val("").trigger("chosen:updated"); var e = $("#MatchGrade option:selected").text(), t = $("#ConfidenceCode").val(); "0" != $("#CriteriaGroupId").val() || "0" != t && null != t || ($("#ConfidenceCodeValue").val(""), $("#CompanyGradeValue").val($("#CompanyGradeValue option:first").val()), $("#StreetGradeValue").val($("#StreetGradeValue option:first").val()), $("#StreetNameGradeValue").val($("#StreetNameGradeValue option:first").val()), $("#CityGradeValue").val($("#CityGradeValue option:first").val()), $("#StateGradeValue").val($("#StateGradeValue option:first").val()), $("#AddressGradeValue").val($("#AddressGradeValue option:first").val()), $("#PhoneGradeValue").val($("#PhoneGradeValue option:first").val()), $("#ZipGradeValue").val($("#ZipGradeValue option:first").val()), $("#DensityValue").val($("#DensityValue option:first").val()), $("#UniquenessValue").val($("#UniquenessValue option:first").val()), $("#SICValue").val($("#SICValue option:first").val()), $("#ConfidenceCode").val(""), $("#CompanyGrade").val($("#CompanyGradeValue option:first").val()), $("#StreetGrade").val($("#StreetGradeValue option:first").val()), $("#StreetNameGrade").val($("#StreetNameGradeValue option:first").val()), $("#CityGrade").val($("#CityGradeValue option:first").val()), $("#StateGrade").val($("#StateGradeValue option:first").val()), $("#AddressGrade").val($("#AddressGradeValue option:first").val()), $("#PhoneGrade").val($("#PhoneGradeValue option:first").val()), $("#ZipGrade").val($("#ZipGradeValue option:first").val()), $("#Density").val($("#DensityValue option:first").val()), $("#Uniqueness").val($("#UniquenessValue option:first").val()), $("#SIC").val($("#SICValue option:first").val()), $("#CompanyCodeValue").val($("#CompanyCodeValue option:first").val()), $("#StreetCodeValue").val($("#StreetCodeValue option:first").val()), $("#StreetNameCodeValue").val($("#StreetNameCodeValue option:first").val()), $("#CityCodeValue").val($("#CityCodeValue option:first").val()), $("#StateCodeValue").val($("#StateCodeValue option:first").val()), $("#AddressCodeValue").val($("#AddressCodeValue option:first").val()), $("#PhoneCodeValue").val($("#PhoneCodeValue option:first").val()), $("#CompanyCode").val($("#CompanyCodeValue option:first").val()), $("#StreetCode").val($("#StreetCodeValue option:first").val()), $("#StreetName").val($("#StreetNameCodeValue option:first").val()), $("#CityCode").val($("#CityCodeValue option:first").val()), $("#StateCode").val($("#StateCodeValue option:first").val()), $("#AddressCode").val($("#AddressCodeValue option:first").val()), $("#PhoneCode").val($("#PhoneCodeValue option:first").val()), $("#ExcludeFromAutoAccept").prop("checked", !1), $(".ExcludedLable").removeClass("Excluded"), $("#GroupId").val($("#GroupId option:first").val()), $("#Tags").val(""), $("#CompanyScore").val("0"), RefreshMultiSelect()), e.length > 0 && t > 0 && ($("#ConfidenceCodeValue").val(t), $("#CompanyGradeValue").val(e.length > 0 ? e.substring(0, 1) : "Z"), $("#StreetGradeValue").val(e.length > 1 ? e.substring(1, 2) : "Z"), $("#StreetNameGradeValue").val(e.length > 2 ? e.substring(2, 3) : "Z"), $("#CityGradeValue").val(e.length > 3 ? e.substring(3, 4) : "Z"), $("#StateGradeValue").val(e.length > 4 ? e.substring(4, 5) : "Z"), $("#AddressGradeValue").val(e.length > 5 ? e.substring(5, 6) : "Z"), $("#PhoneGradeValue").val(e.length > 6 ? e.substring(6, 7) : "Z"), $("#ZipGradeValue").val(e.length > 7 ? e.substring(7, 8) : "Z"), $("#DensityValue").val(e.length > 8 ? e.substring(8, 9) : "Z"), $("#UniquenessValue").val(e.length > 9 ? e.substring(9, 10) : "Z"), $("#SICValue").val(e.length > 10 ? e.substring(10, 11) : "Z"), $("#CompanyCodeValue").val($("#CompanyCodeValue option:first").val()), $("#StreetCodeValue").val($("#StreetCodeValue option:first").val()), $("#StreetNameCodeValue").val($("#StreetNameCodeValue option:first").val()), $("#CityCodeValue").val($("#CityCodeValue option:first").val()), $("#StateCodeValue").val($("#StateCodeValue option:first").val()), $("#AddressCodeValue").val($("#AddressCodeValue option:first").val()), $("#PhoneCodeValue").val($("#PhoneCodeValue option:first").val()), $("#CompanyCode").val($("#CompanyCodeValue option:first").val()), $("#StreetCode").val($("#StreetCodeValue option:first").val()), $("#StreetName").val($("#StreetNameCodeValue option:first").val()), $("#CityCode").val($("#CityCodeValue option:first").val()), $("#StateCode").val($("#StateCodeValue option:first").val()), $("#AddressCode").val($("#AddressCodeValue option:first").val()), $("#PhoneCode").val($("#PhoneCodeValue option:first").val()), $("#ConfidenceCode").val(t), $("#CompanyGrade").val(e.length > 0 ? e.substring(0, 1) : "Z"), $("#StreetGrade").val(e.length > 1 ? e.substring(1, 2) : "Z"), $("#StreetNameGrade").val(e.length > 2 ? e.substring(2, 3) : "Z"), $("#CityGrade").val(e.length > 3 ? e.substring(3, 4) : "Z"), $("#StateGrade").val(e.length > 4 ? e.substring(4, 5) : "Z"), $("#AddressGrade").val(e.length > 5 ? e.substring(5, 6) : "Z"), $("#PhoneGrade").val(e.length > 6 ? e.substring(6, 7) : "Z"), $("#ZipGrade").val(e.length > 7 ? e.substring(7, 8) : "Z"), $("#Density").val(e.length > 8 ? e.substring(8, 9) : "Z"), $("#Uniqueness").val(e.length > 9 ? e.substring(9, 10) : "Z"), $("#SIC").val(e.length > 10 ? e.substring(10, 11) : "Z"), RefreshMultiSelect()), "True" == $("#IsReviewConfirm").val() && window.parent.$.magnificPopup.close() } function OnSuccess() { $("#divProgress").hide() } function WindowAACsuccess() { $("#InsertUpdateAutoAcceptanceModal").modal("hide"), MatchGradeValueChnage(), $("#form_windowAAC #CriteriaGroupId").val() > 0 ? ShowMessageNotification("success", settingsUpdated, !1, !0, ClosePopupReload) : ShowMessageNotification("success", settingsAdded, !1, !0, ClosePopupReload) } function LoadTags() { if ($("#AutoAcceptancneTagsList").length > 0) { var e = $("#AutoAcceptancneTagsList").val().split(","); null == e && "" == e || ($("#TagsValue option").each(function () { for (var t = 0; t < e.length; t++)$(this).val() == e[t] && $(this).attr("selected", "selected") }), $("#InsertUpdateAutoAcceptanceModal #form_windowAAC #Tags").val(e)) } $(".chzn-select").length > 0 && $(".chzn-select").chosen().change(function (e) { "TagsValue" == e.target.id && $("#InsertUpdateAutoAcceptanceModal #form_windowAAC #Tags").val($(this).val()), "ConfidenceCodeValue" == e.target.id && $("#InsertUpdateAutoAcceptanceModal #form_windowAAC #ConfidenceCode").val($(this).val()) }) } function BuildMultiSelect() { $("#ConfidenceCodeValue").multiselect({ includeSelectAllOption: !0, nonSelectedText: selectConfidenceCode, maxWidth: 236 }), $("#CompanyGradeValue").multiselect({ nonSelectedText: selectCompanyGrade }), $("#CompanyCodeValue").multiselect({ nonSelectedText: selectCompanyCode, maxHeight: 200 }), $("#StreetGradeValue").multiselect({ nonSelectedText: selectStreetGrade }), $("#StreetCodeValue").multiselect({ nonSelectedText: selectStreetCode }), $("#StreetNameGradeValue").multiselect({ nonSelectedText: selectStreetNameGrade }), $("#StreetNameCodeValue").multiselect({ nonSelectedText: selectStreetNameCode }), $("#CityGradeValue").multiselect({ nonSelectedText: selectCityGrade }), $("#CityCodeValue").multiselect({ nonSelectedText: selectCityCode }), $("#StateGradeValue").multiselect({ nonSelectedText: selectStateGrade }), $("#StateCodeValue").multiselect({ nonSelectedText: selectStateCode }), $("#AddressGradeValue").multiselect({ nonSelectedText: selectAddressGrade }), $("#AddressCodeValue").multiselect({ nonSelectedText: selectAddressCode }), $("#PhoneGradeValue").multiselect({ nonSelectedText: selectPhoneGrade }), $("#PhoneCodeValue").multiselect({ nonSelectedText: selectPhoneCode }), $("#ZipGradeValue").multiselect({ nonSelectedText: selectZipGrade }), $("#DensityValue").multiselect({ nonSelectedText: selectDensity }), $("#UniquenessValue").multiselect({ nonSelectedText: selectUniqueness }), $("#SICValue").multiselect({ nonSelectedText: selectSIC }) } function RefreshMultiSelect() { $("#ConfidenceCodeValue").multiselect("destroy").multiselect({ includeSelectAllOption: !0, nonSelectedText: selectConfidenceCode }), $("#CompanyGradeValue").multiselect("destroy").multiselect({ nonSelectedText: selectCompanyGrade }), $("#StreetGradeValue").multiselect("destroy").multiselect({ nonSelectedText: selectStreetGrade }), $("#StreetNameGradeValue").multiselect("destroy").multiselect({ nonSelectedText: selectStreetNameGrade }), $("#CityGradeValue").multiselect("destroy").multiselect({ nonSelectedText: selectCityGrade }), $("#StateGradeValue").multiselect("destroy").multiselect({ nonSelectedText: selectStateGrade }), $("#AddressGradeValue").multiselect("destroy").multiselect({ nonSelectedText: selectAddressGrade }), $("#PhoneGradeValue").multiselect("destroy").multiselect({ nonSelectedText: selectPhoneGrade }), $("#ZipGradeValue").multiselect("destroy").multiselect({ nonSelectedText: selectZipGrade }), $("#DensityValue").multiselect("destroy").multiselect({ nonSelectedText: selectDensity }), $("#UniquenessValue").multiselect("destroy").multiselect({ nonSelectedText: selectUniqueness }), $("#SICValue").multiselect("destroy").multiselect({ nonSelectedText: selectSIC }), $("#CompanyCodeValue").multiselect("destroy").multiselect({ nonSelectedText: selectCompanyCode, maxHeight: 200 }), $("#StreetCodeValue").multiselect("destroy").multiselect({ nonSelectedText: selectStreetCode }), $("#StreetNameCodeValue").multiselect("destroy").multiselect({ nonSelectedText: selectStreetNameCode }), $("#CityCodeValue").multiselect("destroy").multiselect({ nonSelectedText: selectCityCode }), $("#StateCodeValue").multiselect("destroy").multiselect({ nonSelectedText: selectStateCode }), $("#AddressCodeValue").multiselect("destroy").multiselect({ nonSelectedText: selectAddressCode }), $("#PhoneCodeValue").multiselect("destroy").multiselect({ nonSelectedText: selectPhoneCode }) } function MatchGradeComponentChange() { var e = $("#MatchGradeComponentCount").val(); "0" == e ? ($("#ZipGradeValue").multiselect("enable"), $("#DensityValue").multiselect("enable"), $("#UniquenessValue").multiselect("enable"), $("#SICValue").multiselect("enable")) : "7" == e ? ($("#ZipGradeValue").val($("#ZipGradeValue option:first").val()), $("#ZipGradeValue").multiselect("destroy").multiselect({ includeSelectAllOption: !1, nonSelectedText: selectZipGrade }), $("#ZipGradeValue").multiselect("disable"), $("#DensityValue").val($("#DensityValue option:first").val()), $("#DensityValue").multiselect("destroy").multiselect({ includeSelectAllOption: !1, nonSelectedText: selectDensity }), $("#DensityValue").multiselect("disable"), $("#UniquenessValue").val($("#UniquenessValue option:first").val()), $("#UniquenessValue").multiselect("destroy").multiselect({ includeSelectAllOption: !1, nonSelectedText: selectUniqueness }), $("#UniquenessValue").multiselect("disable"), $("#SICValue").val($("#SICValue option:first").val()), $("#SICValue").multiselect("destroy").multiselect({ includeSelectAllOption: !1, nonSelectedText: selectSIC }), $("#SICValue").multiselect("disable")) : "11" == e && ($("#ZipGradeValue").multiselect("enable"), $("#DensityValue").multiselect("enable"), $("#UniquenessValue").multiselect("enable"), $("#SICValue").multiselect("enable")) } function ClosePreviewPopUp() { $("#PreviewAutoAcceptanceModal").modal("hide"), $("#InsertUpdateAutoAcceptanceModal").modal("hide"), "false" == $("#isAutoAcceptance").val().toLowerCase() && ($("#PreviewAutoAcceptanceModal").modal("hide"), $("#InsertUpdateAutoAcceptanceModal").modal("hide")), window.ReloadPage() } function getScoreDropdown(e) { var t = 0, l = 100; null != e && (-1 !== jQuery.inArray("Z", e) ? (l = 0, $("#CompanyScore").attr("disabled", "disabled")) : -1 !== jQuery.inArray("F", e) && -1 !== jQuery.inArray("B", e) && -1 !== jQuery.inArray("A", e) || -1 !== jQuery.inArray("#", e) ? (t = 0, l = 100) : -1 !== jQuery.inArray("B", e) && -1 !== jQuery.inArray("A", e) ? (t = 37, l = 100) : -1 !== jQuery.inArray("B", e) && -1 !== jQuery.inArray("F", e) ? (t = 0, l = 80) : -1 !== jQuery.inArray("A", e) && -1 !== jQuery.inArray("F", e) ? (t = 0, l = 36) : -1 !== jQuery.inArray("F", e) ? l = 36 : -1 !== jQuery.inArray("B", e) ? (t = 37, l = 80) : -1 !== jQuery.inArray("A", e) && (t = 81)); var a = $("#IsLicenseEnableAdvancedMatch").val(); -1 == jQuery.inArray("Z", e) && "true" == a.toLowerCase() && $("#CompanyScore").attr("disabled", !1), $("#CompanyScore").empty(); var o = $("#CompanyScore"); for (i = t; i <= l; i++)o.append($("<option></option>").val(i).html(i)) } function MatchgreadValueChange() { $("#MatchGradeComponentCount").val("11") } $.UserRole = $("#UserRole").val(), $.UserLOBTag = $("#UserLOBTag").val(), $("body").on("click", "#ExcludeFromAutoAccept", function () { $(this).prop("checked") ? $(".ExcludedLable").addClass("Excluded") : $(".ExcludedLable").removeClass("Excluded") }), $("body").on("click", "#btnAutoAcceptanceSubmit", function () { var e = 0, t = $("#GroupId").val(), l = $("#Tags").val(); if ($(".customValidation").each(function () { if ("" == $(this).val() || null == $(this).val()) { var t = "#" + $(this).attr("id") + "Value"; $(t).next("div").find("button").addClass("custom-has-error"), e += 1 } else { t = "#" + $(this).attr("id") + "Value"; $(t).next("div").find("button").removeClass("custom-has-error") } }), null == t ? ($("#spnGroupId").show(), e += 1) : $("#spnGroupId").hide(), "lob" == $.UserRole.toLowerCase() && (null == l || "" == l ? ($("#spnTags").show(), e += 1) : $("#spnTags").hide()), e > 0) return !1; $("#divProgress").show() }), $("body").on("change", "#MatchGradeComponentCount", function () { MatchGradeComponentChange() }), $("body").on("click", "#btnAutoAcceptancePreview", function () { var e = 0, t = $("#GroupId").val(), l = $("#Tags").val(); if ($(".customValidation").each(function () { if ("" == $(this).val() || null == $(this).val()) { var t = "#" + $(this).attr("id") + "Value"; $(t).next("div").find("button").addClass("custom-has-error"), e += 1 } else { t = "#" + $(this).attr("id") + "Value"; $(t).next("div").find("button").removeClass("custom-has-error") } }), null == t ? ($("#spnGroupId").show(), e += 1) : $("#spnGroupId").hide(), "lob" == $("#UserRole").val().toLowerCase() && (null == l || "" == l ? ($("#spnTags").show(), e += 1) : $("#spnTags").hide()), e > 0) return !1; var a = $("#form_windowAAC").serialize(); $.ajax({ type: "GET", url: "/DandB/PreviewAutoAcceptance?Parameters=" + a, dataType: "HTML", success: function (e) { $("#PreviewAutoAcceptanceModalMain").html(e), DraggableModalPopup("#PreviewAutoAcceptanceModal") } }) }), $(document).on("click", ".btnPreviewApplyAutoAcceptance", function () { $.ajax({ type: "GET", url: "/DandB/PreviewApplyAutoAcceptance", success: function (e) { bootbox.alert({ title: "<i class='fa fa-info-circle' aria-hidden='true'></i> " + message, message: e.data, callback: function () { ClosePreviewPopUp() } }) } }) }), $(document).on("change", "#CompanyGradeValue", function () { getScoreDropdown($(this).val()) }), $(document).on("change", "#ZipGradeValue", function () { MatchgreadValueChange() }), $(document).on("change", "#DensityValue", function () { MatchgreadValueChange() }), $(document).on("change", "#UniquenessValue", function () { MatchgreadValueChange() }), $(document).on("change", "#SICValue", function () { MatchgreadValueChange() });