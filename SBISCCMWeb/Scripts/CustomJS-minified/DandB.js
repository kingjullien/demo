﻿function LoadLicenceTab() { $(this).parent("li").hasClass("active") || (GetIndexDandBCredentials(), GetBackgroundProcessSettings(), GetDefaultInteractiveKeys(), GetDefaultKeysForEnrichment(), "lob" == $.UserRole.toLowerCase() && ($("#LicenceTab select").attr("disabled", !0), $("#LicenceTab input").attr("disabled", !0), $("#Licence input[type=radio]").attr("disabled", !1))) } function LoadIdentityResolutionTab() { GetIndexMinimumMatchCriteria(), "true" == $("#LicenseEnableTags").val().toLowerCase() && LoadIndexMinimumConfidenceCodeOverride() } function ExclusionLoadTags() { for (var t = 1; t <= 5; t++) { var a = "#Tags" + t, e = $("#TagList" + t).val().split(","); null == e && "" == e || ($(".clstag" + t + " option").each(function () { isInArray($(this).val(), e) && $(this).attr("selected", "selected") }), $(a).val(e)) } $(".chzn-select.Exclusion").length > 0 && $(".chzn-select.Exclusion").chosen().change(function (t) { var a = "#" + $(this).attr("id").replace("TagsValue", "Tags"); if (t.target == this) { var e = $(a).val(); null != e ? e += $(this).val() : e = $(this).val(), $(a).val($(this).val()) } }), $(".chzn-select").trigger("chosen:updated") } function DirectiveLoadTags() { for (var t = 6; t <= 8; t++) { var a = "#Tags" + t, e = $("#TagList" + t).val().split(","); null == e && "" == e || ($(".clstag" + t + " option").each(function () { isInArray($(this).val(), e) && $(this).attr("selected", "selected") }), $(a).val(e)) } $(".chzn-select.Directive").length > 0 && $(".chzn-select.Directive").chosen().change(function (t) { var a = "#" + $(this).attr("id").replace("TagsValue", "Tags"); if (t.target == this) { var e = $(a).val(); null != e ? e += $(this).val() : e = $(this).val(), $(a).val($(this).val()) } }), $(".chzn-select").trigger("chosen:updated") } function LoadTransferDunsTag() { var t = $("#TransferDunsTagList").val(); null == t && (t = ""), null == (t = t.split(",")) && "" == t || ($("#TransferDunsTagValue option").each(function () { for (var a = 0; a < t.length; a++)$(this).val() == t[a] && $(this).attr("selected", "selected") }), $("#TRANSFER_DUNS_AUTO_ENRICH_TAG").val(t)), $("#TransferDunsTagValue").length > 0 && $("#TransferDunsTagValue").chosen().change(function (t) { t.target == this && $("#TRANSFER_DUNS_AUTO_ENRICH_TAG").val($(this).val()) }) } function InitMinimumMatchCriteriaDataTable() { InitDataTable(".MinCCOverrideTB", [10, 20, 30], !1, [0, "desc"]) } function ResetCallback() { $("#ResetSystemDataModal").modal("hide"), location.reload() } function EnableDisableAutoEnrichTag() { $("input#TRANSFER_DUNS_AUTO_ENRICH").is(":checked") ? ($("#addTransferDunsTag").show(), $("#TransferDunsTagValue").attr("disabled", !1)) : ($("#addTransferDunsTag").hide(), $("#TransferDunsTagValue").attr("disabled", "disabled")), $("#TransferDunsTagValue").trigger("chosen:updated") } function SetAutoRefreshEnrichment() { $("input#EnrichmentAlwaysRefresh").is(":checked") ? ($("#ENRICHMENT_STALE_NBR_DAYS").val(-1), $("#ENRICHMENT_STALE_NBR_DAYS").attr("disabled", "disabled")) : ($("#ENRICHMENT_STALE_NBR_DAYS").val(7), $("#ENRICHMENT_STALE_NBR_DAYS").attr("disabled", !1)) } function LoadMonitoringPlusRegistrationDetail(t) { var a = "/DNBMonitoringDirectPlus/GetMonitoringPlusRegistrationDetail/?Parameters=" + ConvertEncrypte(encodeURI(t)).split("+").join("***"); $.ajax({ type: "POST", url: a, dataType: "HTML", contentType: "application/html", async: !1, success: function (t) { $("#partialMonitoringPlusRegistration").html(t), MonitoringTagLoad() }, error: function (t, a, e) { } }) } function LoadMonitoringDirectPlus() { $.pjax({ url: "/DNB/MonitoringDirectPlus", container: "#divPartialMonitoringDirectPlus", timeout: 5e4 }).done(function (t) { null != t && "" != t && null != t && t.indexOf("No record(s) Found") < 0 && $("table.MonitoringPlusRegistrationTb tbody tr:first").addClass("current"), MonitoringTagLoad() }) } function MonitoringTagLoad() { if ("" != $(".formMonitoringDnBPlus #MonitoringRegistrationTagList").val() && null != $(".formMonitoringDnBPlus #MonitoringRegistrationTagList").val() && null != $(".formMonitoringDnBPlus #MonitoringRegistrationTagList").val()) { var t = $(".formMonitoringDnBPlus #MonitoringRegistrationTagList").val().split(","); null == t && "" == t || ($(".chzn-select.monitoringDirectPlusTag option").each(function () { for (var a = 0; a < t.length; a++)$(this).val() == t[a] && $(this).attr("selected", "selected") }), $(".formMonitoringDnBPlus #messages_registration_Tags").val(t)), $(".chzn-select.monitoringDirectPlusTag").length > 0 && $(".chzn-select.monitoringDirectPlusTag").chosen().change(function (t) { t.target == this && $(".formMonitoringDnBPlus #messages_registration_Tags").val($(this).val()) }) } } function CheckRegistrationValidatiopn() { var t = $("#messages_registration_Tags").val(); return "" == t || "0" == t ? ($("#MonitoringDnBplusTag").show(), !1) : ($("#MonitoringDnBplusTag").hide(), !0) } function UpdateMonitoringRegistrationSuccess(t) { ShowMessageNotification("success", t.message), t.result && LoadMonitoringDirectPlus() } $.UserRole = $("#UserRole").val(), $.UserLOBTag = $("#UserLOBTag").val(), $(document).ready(function () { "steward" == $.UserType.toLowerCase() ? "true" == $("#IsActiveDirectPlus").val().toLowerCase() && LoadMonitoringDirectPlus() : (LoadTransferDunsTag(), EnableDisableAutoEnrichTag()); "lob" == $.UserRole.toLowerCase() && ($("#LicenceTab select").attr("disabled", !0), $("#LicenceTab input").attr("disabled", !0), $("#Licence input[type=radio]").attr("disabled", !1)) }), $("body").on("click", "#IdFeatureTab", function () { $(this).parent("li").hasClass("active") || $.pjax({ url: "/DNB/Feature", container: ".divPartialDandBFeature", timeout: 5e4 }).done(function (t) { $(".divPartialDandBFeature").html(t), LoadTransferDunsTag(), EnableDisableAutoEnrichTag() }) }), $("body").on("click", "#IdLicenceTab", function () { $.pjax({ url: "/DNB/License", container: "#divPartialDandBCredentials", timeout: 5e4 }).done(function () { LoadLicenceTab() }) }), $("body").on("click", "#IdIdentityResolutionTab", function () { if (!$(this).parent("li").hasClass("active")) { $("#CleanseMatchTab li").each(function () { $(this).removeClass("active") }), $("#CleanseMatchTabcontent").children("div").each(function () { $(this).removeClass("active") }), $("#CleanseMatchTab li:first").addClass("active"); var t = $("#CleanseMatchTab li:first").find("a").attr("href"); $(t).addClass("active"), $.pjax({ url: "/DNB/MinimumMatchCriteria", container: ".divPartialMinimumMatchCriteria", timeout: 5e4 }).done(function () { LoadIdentityResolutionTab() }) } }), $(document).on("click", "#IdDataEnrichmentTab", function () { $(".enrichTabs>li").removeClass("active"), $(".enrichContentDiv>div").removeClass("active"), $(".enrichTabs>li:first").addClass("active"), $(".enrichContentDiv>div:first").addClass("active"), LoadDataEnrichment() }), $("body").on("click", "#IdMonitoringTab", function () { $.pjax({ url: "/DNB/MinimumMatchCriteria", container: ".divPartialMinimumMatchCriteria", timeout: 5e4 }).done(function () { LoadIdentityResolutionTab() }), $(this).parent("li").hasClass("active") || ($("#Mornitoring20Credential").val("0"), $("#divPartialMonitoringTabs").html("")) }), $("body").on("click", "#IdRTabMinimumMatchCriteria", function () { $(this).parent("li").hasClass("active") || $.pjax({ url: "/DNB/MinimumMatchCriteria", container: ".divPartialMinimumMatchCriteria", timeout: 5e4 }).done(function () { LoadIdentityResolutionTab() }) }), $("body").on("click", "#IdRTabExclusionsCleanseMatch", function () { $(this).parent("li").hasClass("active") || GetIndexExclusionsCleanseMatch() }), $("body").on("click", "#IdRTabAutoAcceptance", function () { $(this).parent("li").hasClass("active") || LoadIndexAutoAcceptance() }), $("body").on("click", "#IdRTabAutoAcceptDirectives", function () { $(this).parent("li").hasClass("active") || LoadIndexAutoAcceptDirectives() }), $("body").on("click", "#IdRTabMonitoring", function () { $(this).parent("li").hasClass("active") || LoadMonitoring() }), $("body").on("click", "#IdRTabUserPreference", function () { $(this).parent("li").hasClass("active") || LoadUserPrefrence() }), $("body").on("click", "#IdMonitoringDirectPlusTab", function () { $(this).parent("li").hasClass("active") || LoadMonitoringDirectPlus() }), $("body").on("click", "#IdRTabNotificationProfile", function () { $(this).parent("li").hasClass("active") || LoadNotificationProfile() }), $("body").on("click", "#IdRTabDUNSRegistration", function () { $(this).parent("li").hasClass("active") || onloadDUNSregistration() }), $(document).on("keypress", ".OnlyDigit", function (t) { var a = 0 == t.keyCode ? t.charCode : t.keyCode; return a >= 48 && a <= 57 || 8 == a }), $("body").on("blur", ".OnlyDigit", function () { var t = $(this).val(); "" != t && ($.isNumeric(t) || ($(this).val(""), ShowMessageNotification("success", allowNumbers))) }), $("body").on("change", "#RegistrationsName", function () { LoadMonitoringPlusRegistrationDetail($(this).val()) }), $("body").on("click", "#IdRTabEnrichment", function () { $(this).parent("li").hasClass("active") || LoadDataEnrichment() }), $(document).on("click", "#IdRTabDataBlockGroups", function () { LoadDataBlockGroups() }), $("body").on("blur", "#EnrichmentPeriodDays", function () { var t = $("#EnrichmentPeriodDays").val(); return t < 1 || t > 30 ? ($("#EnrichmentPeriodDays").val(""), !1) : ($(".EnrichmentPeriodDaysError").attr("hidden", "hidden"), !0) }), $(document).on("change", "#AutoAcceptance_Active", function () { $(this).prop("checked") }), $("body").on("click", "#btnResetSystemData", function () { $.ajax({ type: "GET", url: "/DandB/ResetSystemData?isResetConfig=false", dataType: "HTML", success: function (t) { $("#ResetSystemDataModalMain").html(t), DraggableModalPopup("#ResetSystemDataModal") } }) }), $(document).on("change", "#TRANSFER_DUNS_AUTO_ENRICH", function () { EnableDisableAutoEnrichTag() }), $(document).on("change", "#EnrichmentAlwaysRefresh", function () { SetAutoRefreshEnrichment() }), $("body").on("click", "#btnDuplicateData", function () { $.IsTagsLicenseAllow.toLowerCase(), $.ajax({ type: "GET", url: "/DNBFeature/DuplicateData", dataType: "HTML", success: function (t) { $("#DeDuplicateDataModalMain").html(t), DraggableModalPopup("#DeDuplicateDataModal") } }) }), $("body").on("click", "#btnDeDuplicateData", function () { var t = $("#divDeDuplicate #Tag").val(), a = $("#divDeDuplicate #LOBTag").val(), e = $("#divDeDuplicate #CountryCode").val(), i = $("#divDeDuplicate #CountryGroupId").val(); null != t && "" != t && null != t || (t = ""), null != a && "" != a || (a = ""); var n = $('input[name="__RequestVerificationToken"]').val(); return bootbox.confirm({ title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: purgeDuplicatemsg, callback: function (s) { s && $.ajax({ type: "POST", url: "/DNBFeature/DuplicateData", data: JSON.stringify({ Tag: t, LOBTag: a, CountryCode: e, CountryGroupId: i }), headers: { __RequestVerificationToken: n }, dataType: "json", contentType: "application/json", cache: !1, success: function (t) { ShowMessageNotification("success", t.message), $("#DeDuplicateDataModal").modal("hide") }, error: function (t, a, e) { } }) } }), !0 }), $("body").on("click", ".cachedDataSettings", function () { $.ajax({ type: "GET", url: "/DNBFeature/CachedDataSettings", dataType: "HTML", success: function (t) { $("#CachedDataModalMain").html(t), DraggableModalPopup("#CachedDataModal") } }) }), $("body").on("click", "#editMonitoringPlusRegistration", function () { $("#messages_registration_reference").attr("disabled", !1), $("#messages_registration_productId").attr("disabled", !1), $("#messages_registration_versionId").attr("disabled", !1), $("#messages_registration_email").attr("disabled", !1), $("#messages_registration_fileTransferProfile").attr("disabled", !1), $("#messages_registration_description").attr("disabled", !1), $("#messages_registration_deliveryTrigger").attr("disabled", !1), $("#messages_registration_deliveryFrequency").attr("disabled", !1), $("#messages_dunsCount").attr("disabled", !1), $("#messages_registration_seedData").attr("disabled", !1), $("#messages_notificationsSuppressed").attr("disabled", !1), $("#UpdateMonitoringRegistrationDetail").show(), $(".tagstyleMonitoring").show(), $("#TagsValueMonitoringDnBplus").attr("disabled", !1), $(".chzn-select.monitoringDirectPlusTag").trigger("chosen:updated") }), $("body").on("click", "#btnSyncDUNS", function () { $.ajax({ type: "POST", url: "/DNBMonitoringDirectPlus/SyncDUNS", dataType: "json", contentType: "application/json", async: !1, success: function (t) { t.result && ShowMessageNotification("success", t.message) }, error: function (t, a, e) { } }) }), $("body").on("click", ".SuppUnsuppDUNS", function () { var t = $(this).attr("id"), a = $(this).attr("data-isSuprressed"), e = $(this).attr("data-AuthToken"); "True" == a ? unSuppDUNS : suppDUNS; var i = "referenceName:" + t + "@#$isSuprressed:" + a + "@#$AuthToken:" + e; $.ajax({ type: "POST", url: "/DNBMonitoringDirectPlus/SuppUnsuppDUNS", data: JSON.stringify({ Parameters: ConvertEncrypte(encodeURI(i)).split("+").join("***") }), dataType: "json", contentType: "application/json", async: !1, success: function (t) { ShowMessageNotification("success", t.message), t.result && LoadMonitoringDirectPlus() }, error: function (t, a, e) { } }) }), $("body").on("click", ".ViewDUNSDetails", function () { var t = "RegistrationsName:" + $(this).attr("data-RegistrationsName") + "@#$AuthToken:" + $(this).attr("data-AuthToken") + "@#$IsFromMainPage:true"; $.ajax({ type: "GET", url: "/DNBMonitoringDirectPlus/MonitoringPlusRegistrationDUNSDetails?Parameters=" + ConvertEncrypte(encodeURI(t)).split("+").join("***"), dataType: "HTML", success: function (t) { $("#MonitoringDirectPlusDUNSDetailModalMain").html(t), DraggableModalPopup("#MonitoringDirectPlusDUNSDetailModal") } }) }), $("body").on("click", "table.MonitoringPlusRegistrationTb tbody tr", function () { if (!$(this).hasClass("current")) { if ($(this).html().indexOf("No record(s) Found") > 0) return !1; $(".MonitoringPlusRegistrationTb tr").each(function () { $(this).removeClass("current") }), $(this).closest("tr").addClass("current"), LoadMonitoringPlusRegistrationDetail($(this).attr("data-MonitoringProfilePlusRegistrationID")) } });