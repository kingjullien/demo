$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();
$.LicenseEnableSalesforce = $("#LicenseEnableSalesforce").val();
var userInitialSelectedPermissions;
var userInitialSelectedTypeCode;

//Feature
$("body").on('click', '#UpdateFeatureSubmit', function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var EnableChat = $('input#EnableChat').is(':checked');
    var EnableDataReset = $('input#EnableDataReset').is(':checked');

    var EnablePurgeArchivet = $('input#EnablePurgeArchivet').is(':checked');
    var ArchivePeriodDays = $('input#ArchivePeriodDays').val();
    var InactiveDays = $('#InactiveDays').val();


    if (InactiveDays == "") {
        $('.InactiveDaysError').removeAttr("hidden");
        return false;
    }
    var QueryString = "EnableChat:" + EnableChat + "@#$EnableDataReset:" + EnableDataReset + "@#$EnablePurgeArchivet:" + EnablePurgeArchivet + "@#$ArchivePeriodDays:" + ArchivePeriodDays + "@#$InactiveDays:" + InactiveDays;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: 'POST',
        url: "/Portal/FeatureSetting?Parameters=" + Parameters,
        dataType: 'JSON',
        headers: { "__RequestVerificationToken": token },
        cache: false,
        contentType: 'application/json;',
        success: function (data) {
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
            ShowMessageNotification("success", data, false);

        }
    });
});
//End Feature

//User Comments
function UpdateUserComments(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#UserCommentsMessage").remove();
        var pagevalue = $(".UserCommentspagevalueChange").val();
        var SortBy = $("#UserCommentsSortBy").val();
        var SortOrder = $("#UserCommentsSortOrder").val();
        var page = $("#UserCommentspage").val();
        var url = '/Portal/IndexUserComments/' + "?page=" + page + "&pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
        LoadUserComments(url);
    }
}

$("body").on('change', '.UserCommentspagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#UserCommentsSortBy").val();
    var SortOrder = $("#UserCommentsSortOrder").val();
    var url = '/Portal/IndexUserComments/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    LoadUserComments(url);
});

function OnSuccessUserComments() {
    $("table.UserCommentTB tbody tr:first").addClass("current")
    var Id = $("table.UserCommentTB tbody tr:first").attr("data-CommentId");
    if (Id == undefined) {
        Id = 0;
    }
    $.ajax({
        type: 'GET',
        url: "/Portal/AddUpdateUserComments",
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html',
        data: { Parameters: ConvertEncrypte(encodeURI(Id)).split("+").join("***") },
        success: function (data) {
            $("#AddUpdateUserCommentsId").html(data);
            if ($("table.UserCommentTB tbody tr").length == 0) {
                $("#editUserComment").attr("disabled", true);
            }
        }
    });
    $('#divProgress').hide();
}

$('body').on('click', '#btnUsersComments', function () {
    var comment = $("#Comment").val().trim();
    if (comment == "" || comment == undefined) {
        $("#spnComment").show();
        return false;
    }
    else {
        $("#spnComment").hide();
        return true;
    }
});

$('body').on('click', '.DeleteUserComment', function () {
    var Parameters = $(this).attr("id");

    var pagevalue = $(".UserCommentspagevalueChange").val();
    var SortBy = $("#UserCommentsSortBy").val();
    var SortOrder = $("#UserCommentsSortOrder").val();
    var pageNO = $("#UserCommentspage").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var url = '/Portal/IndexUserComments/' + "?page=" + pageNO + "&pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Portal/DeleteUserComment?Parameters=" + ConvertEncrypte(encodeURI(Parameters)).split("+").join("***"),
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                        ShowMessageNotification("success", data.message, false);
                        if (data.result) {
                            LoadUserComments(url);
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
                return true;
            }
        }
    });
    return false;
});

$("body").on('click', 'table.UserCommentTB tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("table.UserCommentTB tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var Id = $(this).attr("data-CommentId");
        $.ajax({
            type: 'GET',
            url: "/Portal/AddUpdateUserComments",
            dataType: 'HTML',
            cache: false,
            contentType: 'application/html',
            data: { Parameters: ConvertEncrypte(encodeURI(Id)).split("+").join("***") },
            success: function (data) {
                $("#AddUpdateUserCommentsId").html(data);
                if ($("table.UserCommentTB tbody tr").length == 0) {
                    $("#editUserComment").attr("disabled", true);
                }
            }
        });
    }
});

$("body").on("click", "#AddUserComment", function () {
    EnableUserComment();
    $("#CommentType").val($("#CommentType option:first").val());
    $('#Comment').val('');
    $('#CommentId').val('0');
    $('#btnUsersComments').val(add);
    $("table.UserCommentTB tr").each(function () {
        $(this).removeClass('current');
    });

});
$("body").on("click", "#editUserComment", function () {
    EnableUserComment();
    $('#btnUsersComments').val(update);
});
function EnableUserComment() {
    $('#CommentType').attr("disabled", false);
    $('#Comment').attr("disabled", false);
    $('#btnUsersComments').show();
}
//end User Comments

//Tag
$("body").on('click', '.deleteTag', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Portal/DeleteTag?Parameters=" + Parameters,
                    headers: { "__RequestVerificationToken": token },
                    dataType: "JSON",
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                        ShowMessageNotification("success", data, false);
                        $.pjax({
                            url: "/Portal/Tags", container: '#divPartialManageTags',
                            timeout: 50000
                        }).done(function () {
                            InitPortalTagsDataTable();
                            OnSuccessManageTag();
                        });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
                return true;
            }
        }
    });
    return false;
});

$("body").on('keypress', '#txtTags', function (e) {
    if (e.which == 13) {
        SaveTags();
        return false;
    }
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var result = ((keyCode >= 48 && keyCode <= 57) || keyCode == 45 || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || keyCode == 95);
    if (result == true && e.which == 13) {
        SaveTags();
        return false;
    }
    if (result == false) {
        $("#spnIsValidTag").show();
    }
    else if (result == true) {
        $("#spnIsValidTag").hide();
    }
    return result;
});

$("body").on('click', '#btnSubmitTag', function () {
    SaveTags();
});

function SaveTags() {
    var token = $('input[name="__RequestVerificationToken"]').val();

    if ($('#txtTags').val().trim() != "" && $("#TagTypeCode").val().trim() != "") {
        $("#spnSearchText").hide();
        $("#spnTag").hide();
        return true;

    } else {

        if ($('#txtTags').val().trim() == "") {
            $("#spnSearchText").show();
        } else {
            $("#spnSearchText").hide();
        }

        if ($("#TagTypeCode").val().trim() == "") {
            $("#spnTag").show();
        } else {
            $("#spnTag").hide();
        }
        return false;
    }
}
function UpdateMangeTagSuccess(response) {
    ShowMessageNotification("success", response.message, false);
    if (response.result) {
        $.pjax({
            url: "/Portal/Tags", container: '#divPartialManageTags',
            timeout: 50000
        }).done(function () {
            OnSuccessManageTag();
        });
    }
    else {
        if ($.UserRole.toLowerCase() == "lob") {
            $("#frmTag #TagTypeCode option[value='601006@#$LOB']").remove();
            $("#frmTag #LOBTag").val($.UserLOBTag);
            $("#frmTag #LOBTag").attr("disabled", true);
        }
    }
}


$("body").on('change', '#txtTags', function (event, node) {
    if ($(this).val() != "") {
        $("#spnSearchText").hide();
    }
    var tagsval = $(this).val();
    var tagval = $("#TagTypeCode").val().split('@#$')[1];
    if (tagval == undefined) {
        tagval = "";
    }
    var TypeCode = tagval;
    var newval = "[" + TypeCode + "::" + tagsval + "]";
    $("#txttag").text(newval);
});

$("body").on('change', '#TagTypeCode', function () {
    if ($("#TagTypeCode").val() != "") {
        $("#spnTag").hide();
    }
    var tagval = $("#TagTypeCode").val().split('@#$')[1];
    if (tagval == undefined) {
        tagval = "";
    }
    var tagsval = $("#txtTags").val();
    var TypeCode = tagval;
    var newval = "[" + TypeCode + "::" + tagsval + "]";
    $("#txttag").text(newval);
    if (tagval.toLowerCase() == "lob") {
        $("#LOBTag").val("");

        $(".clsLobtag").hide();
    }
    else {
        $(".clsLobtag").show();
    }

});

$("body").on('keyup', '#txtTags', function (event, node) {
    if ($(this).val() != "") {
        $("#spnSearchText").hide();
    }
    var tagsval = $(this).val();
    var tagval = $("#TagTypeCode").val().split('@#$')[1];
    if (tagval == undefined) {
        tagval = "";
    }
    var TypeCode = tagval;
    var newval = "[" + TypeCode + "::" + tagsval + "]";
    $("#txttag").text(newval);


});

$("body").on('change', '.TagspagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    $("#ManageTagPageValue").val(pagevalue);
    var SortBy = $("#ManageTagSortBy").val();
    var SortOrder = $("#ManageTagSortOrder").val();
    var url = '/Portal/indexManageTags/';
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        success: function (data) {
            $("#divPartialManageTags").html(data);
            InitPortalTagsDataTable();
            OnSuccessManageTag();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

function OnSuccessManageTag() {
    $("table.TagsTB tbody tr:first").addClass("current")
    var Id = $("table.TagsTB tbody tr:first").attr("data-TagId");
    $.ajax({
        type: 'GET',
        url: "/Portal/AddUpdateCompanyTags",
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html',
        data: { Parameters: ConvertEncrypte(encodeURI(Id)).split("+").join("***") },
        success: function (data) {
            $("#divPartialAddUpdateTags").html(data);
            InitPortalTagsDataTable();
            var Tagvalue = $("#Tagvalue").val();
            $('#TagTypeCode').val(Tagvalue);
            var tagval = $("#TagTypeCode").val() == null ? "" : $("#TagTypeCode").val().split('@#$')[1];
            if (tagval.toLowerCase() == "lob") {
                $("#LOBTag").val("");
                $("#LOBTag option:selected").text("");
                $(".clsLobtag").hide();
            }
            else {
                $(".clsLobtag").show();
            }
            if ($("table.TagsTB tbody tr").length == 0) {
                $("#editTag").attr("disabled", true);
            }
        }
    });
}

$("body").on('click', 'table.TagsTB tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("table.TagsTB tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var Id = $(this).attr("data-TagId");
        $.ajax({
            type: 'GET',
            url: "/Portal/AddUpdateCompanyTags",
            dataType: 'HTML',
            cache: false,
            contentType: 'application/html',
            data: { Parameters: ConvertEncrypte(encodeURI(Id)).split("+").join("***") },
            success: function (data) {
                $("#divPartialAddUpdateTags").html(data);
                var Tagvalue = $("#Tagvalue").val();
                $('#TagTypeCode').val(Tagvalue);

                var tagval = $("#TagTypeCode").val().split('@#$')[1];
                if (tagval.toLowerCase() == "lob") {
                    $("#LOBTag").val("");
                    $("#LOBTag option:selected").text("");
                    $(".clsLobtag").hide();
                }
                else {
                    $(".clsLobtag").show();
                }
            }
        });
    }
});


$("body").on("click", "#AddTag", function () {
    $("#frmTag #TagTypeCode").val($("#TagTypeCode option:first").val());
    $("#frmTag #LOBTag").val($("#LOBTag option:first").val());
    $("#frmTag #txtTags").val('');
    $("#frmTag #TagTypeCode").prop("disabled", false);
    $("#frmTag #LOBTag").prop("disabled", false);
    $("#frmTag #txtTags").prop("disabled", false);
    $("#frmTag #txttag").html('');
    $("#frmTag #TagId").val('0');
    $("#frmTag #btnSubmitTag").show();
    $("#frmTag #btnSubmitTag").val(add);
    $("table.TagsTB tr").each(function () {
        $(this).removeClass('current');
    });
    $(".clsLobtag").show();
    if ($.UserRole.toLowerCase() == "lob") {
        $("#frmTag #TagTypeCode option[value='601006@#$LOB']").remove();
        $("#frmTag #LOBTag").val($.UserLOBTag);
        $("#frmTag #LOBTag").attr("disabled", true);
    }

})

$("body").on("click", "#editTag", function () {
    $("#frmTag #LOBTag").prop("disabled", false);
    $("#frmTag #btnSubmitTag").show();
    $("#frmTag #btnSubmitTag").val(update);
    if ($.UserRole.toLowerCase() == "lob") {
        $("#frmTag #TagTypeCode option[value='601006@#$LOB']").remove();
    }
});
//Enad Tag


//Users
$("body").on("click", "table.userTB tbody tr", function () {
    if (!$(this).hasClass("current")) {
        $("table.userTB tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var UserId = $(this).attr("data-UserIds");
        $.ajax({
            type: 'GET',
            url: "/Portal/AddUpdateUsers",
            dataType: 'HTML',
            cache: false,
            contentType: 'application/html',
            data: { Parameters: UserId },
            success: function (data) {
                $("#divPartialAddUpdateUsers").html(data);

                updateTagListOnchangeOrAdd();
                LoadTags();
                $('#MultiSelectSecurityOptions').multiselect({
                    includeSelectAllOption: true,
                    nonSelectedText: 'Select Security Option',
                    onInitialized: function (select, container) {
                        userInitialSelectedPermissions = $('#MultiSelectSecurityOptions').val();
                        userInitialSelectedTypeCode = $("#UserTypeCode").val();
                    }
                });
                CheckUserTypeForUserRole();
            }
        });
    }
});

$("body").on("click", ".deleteuser", function () {
    var Parameters = $(this).attr("id");
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteUserMsg, callback: function (result) {
            if (result) {
                // Changes for Converting magnific popup to modal popup
                $.ajax({
                    type: 'GET',
                    url: "/Portal/DeleteUsers?Parameters=" + Parameters,
                    dataType: 'HTML',
                    async: false,
                    success: function (data) {
                        $("#divProgress").hide();
                        $("#PortalDeleteUserModalMain").html(data);
                        DraggableModalPopup("#PortalDeleteUserModal");
                    }
                });

            }
        }
    });
    return false;
});

$("body").on("click", "#btnDeleteUser", function (e) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var UserId = $("#UserId").val();
    var ReassignToUserId = $("#ReassignToUserId").val();

    if (ReassignToUserId == "") {
        $("#spnReassignToUserId").show();
        return false;
    }
    else {
        $("#spnReassignToUserId").hide();
    }

    var QueryString = "UserId:" + UserId + "@#$ReassignToUserId:" + ReassignToUserId;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    $.ajax({
        type: "POST",
        url: "/Portal/DeleteUserAfterReassign?Parameters=" + Parameters,
        headers: { "__RequestVerificationToken": token },
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            $("#PortalDeleteUserModal").modal("hide");
            ShowMessageNotification("success", data.message, false);
            if (data.result) {
                loadUserList();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
    e.stopImmediatePropagation();
    return false;
});

$("body").on("click", ".Activateuser", function () {
    var pagevalue = $(".userspagevalueChange").val();
    var SortBy = $("#UserSortBy").val();
    var SortOrder = $("#UserSortOrder").val();
    var page = $("#UserPage").val();
    var url = '/Portal/IndexUsers/';
    var Parameters = $(this).attr("data-userid");
    $.ajaxSetup({ async: true });
    $("#divProgress").show();
    $.post("/Portal/Activateuser/", { Parameters: Parameters }, function (data) {

        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", data, false);
        $.ajax({
            type: "GET",
            url: url,
            dataType: "HTML",
            contentType: "application/html",
            data: {},
            cache: false,
            success: function (data) {
                $("#divUser").html(data);
                InitPortalUserDataTable();
                OnSuccessUser();
                $("#divProgress").hide();
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    });
});

$('body').on('click', '#editUser', function () {
    EnableUserFields();
    CheckUserTypeForUserRole();
    $('#MultiSelectSecurityOptions').multiselect("enable");
});
$("body").on('change', '.userspagevalueChange', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#UserSortBy").val();
    var SortOrder = $("#UserSortOrder").val();
    var url = '/Portal/IndexUsers/';
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divUser").html(data);
            InitPortalUserDataTable();
            $(".userspagevalueChange").val(pagevalue);
            OnSuccessUser();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
$('body').on('click', '#AddUser', function () {
    $("table.userTB tr").each(function () {
        $(this).removeClass('current');
    });
    EnableUserFields();
    $("#UserMessgae").remove();
    $("#form_ConfigData #Tags").val("");
    $("#UserName").val("");
    $("#EmailAddress").val("");
    $("#UserStatusCode").val("");
    $("#SSOUser").val("");

    $("#UserTypeCode").val($("#UserTypeCode option:first").val());
    $("#LOBTag").val($("#LOBTag option:first").val());
    $("#btnConfigDataUser").val(add);
    $("#UserId").val("0");
    $("#TagsValue").val('0');
    $("#TagList").val("");
    $(".chzn-select").trigger("chosen:updated");
    $("#MultiSelectSecurityOptions option:selected").removeAttr("selected");
    $('#MultiSelectSecurityOptions').multiselect("enable");
    $('#MultiSelectSecurityOptions').multiselect("clearSelection");

    CheckUserTypeForUserRole();
    LoadTags();

});
function UserUpdateSuccess(data) {

    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", data.message, false, false);
    if (data.result) {
        $("#UserName").prop("disabled", true);
        $("#EmailAddress").prop("disabled", true);
        $("#UserStatusCode").prop("disabled", true);
        $("#UserTypeCode").prop("disabled", true);
        $("#LOBTag").prop("disabled", true);
        $("#TagsValue").prop("disabled", true);
        $(".chzn-select").trigger("chosen:updated");
        $("#IsApprover").prop("disabled", true);
        $("#EnableInvestigations").prop("disabled", true);
        $("#EnablePurgeData").prop("disabled", true);
        $("#EnableSearchByDUNS").prop("disabled", true);
        $("#EnableCreateAutoAcceptRules").prop("disabled", true);
        $("#btnConfigDataUser").hide();
        $("#form_ConfigData .OpenTags").hide();
        var pagevalue = $(".userspagevalueChange").val();
        var SortBy = $("#UserSortBy").val();
        var SortOrder = $("#UserSortOrder").val();
        var page = $("#UserPage").val();
        var url = '/Portal/Users/';
        var Userid = $("#UserId").val();
        $.pjax({
            url: "/Portal/Users", container: '#divUser',
            timeout: 50000,
        }).done(function () {
            InitPortalUserDataTable();
            OnSuccessUser();
        });
    }
}

function OnSuccessUser() {
    $('table.userTB tbody tr:first').addClass('current');
    var UserId = $('table.userTB tbody tr:first').attr("data-userids");
    $.ajax({
        type: 'GET',
        url: "/Portal/AddUpdateUsers",
        dataType: 'HTML',
        cache: false,
        contentType: 'application/html',
        data: { Parameters: UserId },
        success: function (data) {
            $("#divPartialAddUpdateUsers").html(data);

            updateTagListOnchangeOrAdd();
            LoadTags();
            $('#MultiSelectSecurityOptions').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Select Permission',
                onInitialized: function (select, container) {
                    userInitialSelectedPermissions = $('#MultiSelectSecurityOptions').val();
                    userInitialSelectedTypeCode = $("#UserTypeCode").val();
                }
            });
        }
    });
    $('#divProgress').hide();
}
function EnableUserFields() {
    $("#UserName").prop("disabled", false);

    $("#EmailAddress").prop("disabled", false);
    $("#UserStatusCode").prop("disabled", false);
    $("#UserTypeCode").prop("disabled", false);
    $("#LOBTag").prop("disabled", false);
    $("#TagsValue").prop("disabled", false);
    $(".chzn-select").trigger("chosen:updated");
    $("#IsApprover").prop("disabled", false);
    $("#EnableInvestigations").prop("disabled", false);
    $("#EnablePurgeData").prop("disabled", false);
    $("#EnableSearchByDUNS").prop("disabled", false);
    $("#EnableCreateAutoAcceptRules").prop("disabled", false);
    $("#SSOUser").attr("disabled", false);
    $("#btnConfigDataUser").show();
    $("#form_ConfigData .OpenTags").show();
}


function LoadTags() {
    var TagList = $("#TagList").val().split(',');
    if (TagList != null || TagList != "") {
        $(".chzn-select option").each(function () {
            for (var i = 0; i < TagList.length; i++) {
                if ($(this).val() == TagList[i]) {
                    $(this).attr("selected", "selected");
                }
            }
        });
        $("#Tags").val(TagList);
    }
    if ($(".chzn-select").length > 0) {
        $(".chzn-select").chosen().change(function (event) {
            if (event.target == this) {
                $("#Tags").val($(this).val());
                checkVAlidInclusiveTags();
            }
        });
    }
}
$('body').on('change', '#form_ConfigData #LOBTag', function () {
    updateTagListOnchangeOrAdd();
    $("#Tags").val('');
});
function updateTagListOnchangeOrAdd() {
    var LOBTags = $("#form_ConfigData #LOBTag").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: '/Portal/GetLOBTags',
        data: JSON.stringify({ LOBTag: LOBTags }),
        dataType: 'json',
        contentType: "application/json",
        headers: { "__RequestVerificationToken": token },
        processData: false,
        async: false,

        success: function (response) {
            $('#TagsValue').find('option').remove().end();
            var str = "";
            $.each(response.Data, function (i) {
                str = str + response.Data[i].Tag + ",";
                $('#TagsValue').append(
                    $('<option></option>').val(response.Data[i].Tag).html(response.Data[i].Tag)
                );
            });
            $('#TagsValue').trigger("chosen:updated");
        },
        error: function (xhr, status, error) {
        }
    });
}
$('body').on('click', '#btnConfigDataUser', function () {
    var UserName = $("#UserName").val().trim();
    var PasswordHash = $("#PasswordHash").val();
    var EmailAddress = $("#EmailAddress").val();
    var SSOUser = $("#SSOUser").val();
    var UserStatus = $('#UserTypeCode').val();
    var count = 0;
    if (UserName == '') {
        $("#spnUserName").show();
        count++;
    }
    else {
        $("#spnUserName").hide();
    }
    if (EmailAddress == '') {
        $("#spnEmail").show();
        count++;
    }
    else {
        if (!isValidEmailAddress(EmailAddress)) {
            $("#spnEmail").html("Please enter proper email.");
            $("#spnEmail").show();
            count++;
        }
        else {
            $("#spnEmail").hide();
        }
    }
    if (SSOUser == '') {
        $("#spnSSOUser").show();
        count++;
    }
    else {
        $("#spnSSOUser").hide();

    }
    if (UserStatus == "102001") {
        $("#Tags").val("");
    }
    if (count > 0) {
        return false;
    }
    else {
        var selected = $("#MultiSelectSecurityOptions option");
        selected.each(function () {
            var checkedItem = "#" + $(this).val();
            $(checkedItem).val(this.selected);
        });
    }
});
function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
}
function loadUserList() {
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/Portal/Users", container: '#divUser',
        timeout: 50000,
    }).done(function () {
        InitPortalUserDataTable();
        $('table.userTB tbody tr:first').addClass('current');
        OnSuccessUser();
    });
}

// End Users

// Country Group
$('body').on('click', '#btnConfigCountryGroupRight', function (e) {
    var selectedOpts = $('#objCountryGroup_AddSelectedCountry option:selected');
    if (selectedOpts.length == 0) {

        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", nothingToMove, false);
        e.preventDefault();
    }
    $('#objCountryGroup_RemoveSelectedCountry').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('body').on('click', '#btnConfigCountryAllRight', function (e) {
    var selectedOpts = $('#objCountryGroup_AddSelectedCountry option');
    if (selectedOpts.length == 0) {

        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", nothingToMove, false);
        e.preventDefault();
    }
    $('#objCountryGroup_RemoveSelectedCountry').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('body').on('click', '#btnConfigCountryGroupLeft', function (e) {
    var selectedOpts = $('#objCountryGroup_RemoveSelectedCountry option:selected');
    if (selectedOpts.length == 0) {

        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", nothingToMove, false);
        e.preventDefault();
    }
    $('#objCountryGroup_AddSelectedCountry').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('body').on('click', '#btnConfigCountryGroupAllLeft', function (e) {
    var selectedOpts = $('#objCountryGroup_RemoveSelectedCountry option');
    if (selectedOpts.length == 0) {

        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", nothingToMove, false);
        e.preventDefault();
    }
    $('#objCountryGroup_AddSelectedCountry').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('body').on('click', '#btnConfigCountryGroup', function (e) {
    var value = "";
    $('#objCountryGroup_RemoveSelectedCountry option').each(function () {
        value = value + $(this).val() + ",";
    });
    $("#objCountryGroup_ISOAlpha2Codes").val(value);
});

$('body').on('click', '.deleteCountryGroup', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Portal/DeleteCountryGroup?Parameters=" + Parameters,
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    cache: false,
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                        ShowMessageNotification("success", data.message, false);
                        if (data.result) {
                            var url = "/Portal/indexCountryGrp/";
                            LoadCountryGroup(url);
                        }

                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
                return true;
            }
        }
    });
    return false;
});

$(".CntryGrp tr").each(function () {
    $(this).removeClass('current');
});

$("body").on('click', 'table.CntryGrp tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("table.CntryGrp tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var Id = $(this).attr("data-cntrygroupid");
        $.ajax({
            type: 'GET',
            url: "/Portal/AddUpdateCountryGrp",
            dataType: 'HTML',
            contentType: 'application/html',
            data: { Parameters: Id },
            success: function (data) {
                $("#divPartialaddUpdateCountryGrp").html(data);
                if ($("table.CntryGrp tbody tr").length == 0) {
                    $("#editCountryGroup").attr("disabled", true);
                }
            }
        });
    }
});

$('body').on('click', '#editCountryGroup', function () {
    $("#objCountryGroup_GroupName").prop("disabled", false);
    $("#objCountryGroup_AddSelectedCountry").prop("disabled", false);
    $("#objCountryGroup_RemoveSelectedCountry").prop("disabled", false);
    $("#btnConfigCountryGroupRight").prop("disabled", false);
    $("#btnConfigCountryAllRight").prop("disabled", false);
    $("#btnConfigCountryGroupLeft").prop("disabled", false);
    $("#btnConfigCountryGroupAllLeft").prop("disabled", false);
    $("#btnConfigCountryGroup").show();
    $("#btnConfigCountryGroup").val(update);
});

$('body').on('click', '#AddCountryGroup', function () {
    $(".CntryGrp tr").each(function () {
        $(this).removeClass('current');
    });
    $("#objCountryGroup_GroupId").val("0");
    $("#objCountryGroup_GroupName").val("");
    $("#btnConfigCountryGroup").val(add);
    $("#CountryMessage").remove();
    var selectedOpts = $('#objCountryGroup_RemoveSelectedCountry option');
    $('#objCountryGroup_AddSelectedCountry').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    $("#objCountryGroup_GroupName").prop("disabled", false);
    $("#objCountryGroup_AddSelectedCountry").prop("disabled", false);
    $("#objCountryGroup_RemoveSelectedCountry").prop("disabled", false);
    $("#btnConfigCountryGroupRight").prop("disabled", false);
    $("#btnConfigCountryAllRight").prop("disabled", false);
    $("#btnConfigCountryGroupLeft").prop("disabled", false);
    $("#btnConfigCountryGroupAllLeft").prop("disabled", false);
    $("#btnConfigCountryGroup").show();
    $("#objCountryGroup_tmpName").val("");
});

$("body").on('change', '.CountrypagevalueChange', function () {
    var url = '/Portal/indexCountryGrp/';
    LoadCountryGroup(url);
});

function OnSuccessCountryGrp() {
    $('table.CntryGrp tbody tr:first').addClass('current');
    var Id = $('table.CntryGrp tbody tr:first').attr("data-cntrygroupid");
    $.ajax({
        type: 'GET',
        url: "/Portal/AddUpdateCountryGrp",
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        data: { Parameters: Id },
        success: function (data) {
            $("#divPartialaddUpdateCountryGrp").html(data);
            InitPortalCountryGroupDataTable();
            if ($("table.CntryGrp tbody tr").length == 0) {
                $("#editCountryGroup").attr("disabled", true);
            }
        }
    });
    $('#divProgress').hide();
}
$('body').on('click', '.btnConfigCountryGroup', function () {

    var OptionCount = $('select#objCountryGroup_RemoveSelectedCountry option').length;
    var GroupName = $("#objCountryGroup_GroupName").val().trim();
    var count = 0;
    if (GroupName == '') {
        $("#spnGroupName").show();
        count++;
    }
    else {
        $("#spnGroupName").hide();
    }
    if (OptionCount == 0) {
        $("#spnOptionValue").show();
        count++;
    }
    else {
        $("#spnOptionValue").hide();
    }
    if (count > 0) {
        return false;
    }
});
function UpdateCountryGroup(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#objCountryGroup_GroupName").prop("disabled", true);
        $("#objCountryGroup_AddSelectedCountry").prop("disabled", true);
        $("#objCountryGroup_RemoveSelectedCountry").prop("disabled", true);
        $("#btnConfigCountryGroupRight").prop("disabled", true);
        $("#btnConfigCountryAllRight").prop("disabled", true);
        $("#btnConfigCountryGroupLeft").prop("disabled", true);
        $("#btnConfigCountryGroupAllLeft").prop("disabled", true);
        $("#btnConfigCountryGroup").hide();
        $("#PortalCountryGroupModal").modal("hide");
        var url = "/Portal/indexCountryGrp/";
        LoadCountryGroup(url);
        var CountryGroupName = $(".CountryGroupName").val();
        CallbackCountryGroup(CountryGroupName);
    }
}
function UpdateCountryGroupPopup(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#objCountryGroup_GroupName").prop("disabled", true);
        $("#objCountryGroup_AddSelectedCountry").prop("disabled", true);
        $("#objCountryGroup_RemoveSelectedCountry").prop("disabled", true);
        $("#btnConfigCountryGroupRight").prop("disabled", true);
        $("#btnConfigCountryAllRight").prop("disabled", true);
        $("#btnConfigCountryGroupLeft").prop("disabled", true);
        $("#btnConfigCountryGroupAllLeft").prop("disabled", true);
        $("#btnConfigCountryGroup").hide();
        $("#PortalCountryGroupModal").modal("hide");
        var url = "/Portal/indexCountryGrp/";
        LoadCountryGroupPopup(url);
        var CountryGroupName = $(".CountryGroupName").val();
        var CountryGroupId = data.CountryId;
        CallbackCountryGroupPopupMatch(CountryGroupName, CountryGroupId);
    }
}

//export excel file of country group
$('body').on('click', '#btnExportToExcel', function () {
    $('<form action="/Portal/ExportToExcel"></form>').appendTo('body').submit();
});
$('body').on('click', '#btnImportData', function () {
    // Changes for Converting magnific popup to modal popup
    var id = $(this).attr("id");
    $.ajax({
        type: 'GET',
        url: "/Portal/CountryImportData",
        dataType: 'HTML',
        success: function (data) {
            $("#divProgress").hide();
            $("#PortalCountryGroupImportModalMain").html(data);
            DraggableModalPopup("#PortalCountryGroupImportModal");
        }
    });
});
function CloseImportPanel() {
    // Changes for Converting magnific popup to modal popup
    $("#PortalCountryGroupImportModal").modal("hide");
    $.ajax({
        type: 'GET',
        url: "/Portal/CountryDataMatch",
        dataType: 'HTML',
        async: false,
        success: function (data) {
            $("#CountryGroupImportModalMain").html(data);
            DraggableModalPopup("#CountryGroupImportModal");
        }
    });
}
//End Country Group

//Entity
$('body').on('click', '.deleteCDSEntity', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    var pagevalue = $("#CDSEntityPageValue").val();
    var SortBy = $("#CDSEntitySortBy").val();
    var SortOrder = $("#CDSEntitySortOrder").val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Portal/DeleteCDSEntity?Parameters=" + ConvertEncrypte(encodeURI(Parameters)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                        ShowMessageNotification("success", data.message, false);
                        if (data.result) {
                            $.ajax({
                                type: 'GET',
                                cache: false,
                                url: "/Portal/indexEntity?sortby=" + SortBy + "&sortorder=" + SortOrder + "&pagevalue=" + pagevalue,
                                dataType: 'HTML',
                                contentType: 'application/html',
                                async: false,
                                success: function (data) {
                                    $("#divPartialCDSEntity").html(data);
                                }
                            });
                        }

                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});
function addEntityToSelection(OptionValue) {
    $("#txtEntity").val("");
    var pagevalue = $(".CDSEntityPageValueChange").val();
    var SortBy = $("#CDSEntitySortBy").val();
    var SortOrder = $("#CDSEntitySortOrder").val();
    var url = '/Portal/indexEntity/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divPartialCDSEntity").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}
$("body").on('change', '.CDSEntityPageValueChange', function () {
    var pagevalue = $(this)[0].value;
    var SortBy = $("#CDSEntitySortBy").val();
    var SortOrder = $("#CDSEntitySortOrder").val();
    var url = '/Portal/indexEntity/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divPartialCDSEntity").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});
function OnSuccessEntity() {
    $('#divProgress').hide();
}
function sumbitntity() {
    var value = $("#txtEntity").val();
    if (value == "" || value == undefined) {
        $("#spnEntity").show();
        return false;
    } else {
        $("#spnEntity").hide();
        return true;
    }
}
//End Entity

//Environment
$('body').on('click', '.deleteCDSEnvironment', function () {
    var Parameters = $(this).attr("id");
    var pagevalue = $("#CDSEnvironmentPageValue").val();
    var SortBy = $("#CDSEnvironmentSortBy").val();
    var SortOrder = $("#CDSEnvironmentSortOrder").val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Portal/DeleteCDSEnvironment?Parameters=" + ConvertEncrypte(encodeURI(Parameters)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {
                        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
                        ShowMessageNotification("success", data, false);
                        if (data == "Data deleted successfully") {
                            $.ajax({
                                type: 'GET',
                                cache: false,
                                url: "/Portal/indexEnvironment?sortby=" + SortBy + "&sortorder=" + SortOrder + "&pagevalue=" + pagevalue,
                                dataType: 'HTML',
                                contentType: 'application/html',
                                async: false,
                                success: function (data) {
                                    $("#divPartialCDSEnvironment").html(data);
                                    onSuccessEnvironment();
                                }
                            });
                        }

                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
});

$("body").on('change', '.CDSEnvironmentPageValueChange', function () {

    var pagevalue = $(this)[0].value;
    var SortBy = $("#CDSEnvironmentSortBy").val();
    var SortOrder = $("#CDSEnvironmentSortOrder").val();
    var url = '/Portal/indexEnvironment/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divPartialCDSEnvironment").html(data);
            onSuccessEnvironment();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
});

function sumbitEnvironment() {
    var Environment = $("#txtEnvironment").val().trim();
    var OrganizationUrl = $("#txtOrganizationUrl").val().trim();
    var TenantId = $("#txtTenantId").val().trim();
    var cnt = 0;
    if (Environment == "" || Environment == undefined) {
        $("#spnEnvironment").show();
        cnt++;
    } else {
        $("#spnEnvironment").hide();
    }
    if (OrganizationUrl == "" || OrganizationUrl == undefined) {
        $("#spnOrganizationUrl").show();
        cnt++;
    } else {
        $("#spnOrganizationUrl").hide();
    }
    if (TenantId == "" || TenantId == undefined) {
        $("#spnTenantId").show();
        cnt++;
    } else {
        $("#spnTenantId").hide();
    }
    if (cnt > 0) {
        return false;
    }
}

function UpdateEnvironment(data) {
    $("body").append(data);
    var message = $("#EnvironmentMessage").val();
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", message, false);

    $("#EnvironmentMessage").remove();

    if (message.indexOf("Environment Name already exist.") > 0 || message == "Please fill proper information.") {
        return false;
    }
    $("#txtEnvironment").val("");
    $("#txtOrganizationUrl").val('');
    $("#txtTenantId").val('');
    var pagevalue = $("CDSEnvironmentPageValueChange").val();
    var SortBy = $("#CDSEnvironmentSortBy").val();
    var SortOrder = $("#CDSEnvironmentSortOrder").val();
    var url = '/Portal/indexEnvironment/' + "?pagevalue=" + pagevalue + "&sortby=" + SortBy + "&sortorder=" + SortOrder;
    $.ajax({
        type: "POST",
        url: url,
        dataType: "HTML",
        contentType: "application/html",
        async: false,
        cache: false,
        success: function (data) {
            $("#divPartialCDSEnvironment").html(data);
            onSuccessEnvironment();
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

function onSuccessEnvironment() {
    $("table.EnvironmentTB tr").each(function () {
        $(this).removeClass('current');
    });
    $('table.EnvironmentTB tbody tr:first').addClass('current');
    var OrganizationUrl = $('table.EnvironmentTB tbody tr:first').attr("data-OrganizationUrl");
    $.ajax({
        type: 'GET',
        url: "/Portal/AddEnvironMent",
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        data: { Parameters: ConvertEncrypte(encodeURI(OrganizationUrl)).split("+").join("***") },
        success: function (data) {
            if ($("table.EnvironmentTB tbody tr:first").html().indexOf("No records found") > 0) {
                $("table.EnvironmentTB tbody tr:first").removeClass("current");
            }
            $("#divPartialAddUpdateCDSEnvironment").html(data);
            if ($("table.EnvironmentTB tbody tr").length == 0) {
                $("#EditEnvironment").attr("disabled", true);
            }

        }
    });
    $('#divProgress').hide();
}

$('body').on('click', '#AddEnvironment', function () {
    $("table.EnvironmentTB tr").each(function () {
        $(this).removeClass('current');
    });
    $("#txtTenantId").val('');
    $("#txtEnvironment").val('');
    $("#txtOrganizationUrl").val('');

    $("#txtTenantId").attr("disabled", false);
    $("#txtEnvironment").attr("disabled", false);
    $("#txtOrganizationUrl").attr("readonly", false)
    $("#btnEnvironmentSubmit").show();
    $("#btnEnvironmentSubmit").val("Save");
});

$('body').on('click', '#EditEnvironment', function () {
    $("#txtTenantId").attr("disabled", false);
    $("#txtEnvironment").attr("disabled", false);
    $("#btnEnvironmentSubmit").show();

});

$("body").on('click', 'table.EnvironmentTB tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("table.EnvironmentTB tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var OrganizationUrl = $(this).attr("data-OrganizationUrl");
        $.ajax({
            type: 'GET',
            url: "/Portal/AddEnvironMent",
            dataType: 'HTML',
            contentType: 'application/html',
            cache: false,
            data: { Parameters: ConvertEncrypte(encodeURI(OrganizationUrl)).split("+").join("***") },
            success: function (data) {
                $("#divPartialAddUpdateCDSEnvironment").html(data);
            }
        });
    }
});
//end Environment

//Tab Change Event
$('body').on('click', '#IdFeatureTab', function () {
    if (!$(this).parent("li").hasClass("active")) {
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/Portal/Features", container: '#divPartialFeature',
            timeout: 50000
        }).done(function () {
        });
    }
});

$('body').on('click', '#IdAboutUsTab', function () {
    if (!$(this).parent("li").hasClass("active")) {
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/Portal/AboutUs", container: '#divPartialAboutuUs',
            timeout: 50000
        }).done(function () {
        });
    }
});

$('body').on('click', '#IdCommonTab', function () {

    if (!$(this).parent("li").hasClass("active")) {
        $("#CommonRightTab li").each(function () {
            $(this).removeClass("active");
        });
        $("#CommonRightTabcontent").children("div").each(function () {
            $(this).removeClass("active");
        });
        $("#IdRTabTags").removeClass("active");
        $("#IdRTabUserComment").removeClass("active");
        $("#IdRTabCompanyAttribute").removeClass("active");
        $("#IdRTabExportPageSettings").removeClass("active");
        $("#IdRTabDPMFTPConfiguration").removeClass("active");
        $("#IdRTabConfigurationSettings").removeClass("active");
        $("#CommonRightTab li:first").addClass("active");
        $("#CommonRightTabcontent").children("div:first").addClass("active");
        var url = "/Portal/indexCountryGrp";
        LoadCountryGroup(url);
    }
});

$('body').on('click', '#IdUsersTab', function () {
    if (!$(this).parent("li").hasClass("active")) {
        loadUserList();
    }
});

$('body').on('click', '#IdRTabDynamicCRM', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $.ajax({
            type: 'GET',
            url: "/Portal/indexEnvironment",
            dataType: 'HTML',
            contentType: 'application/html',
            cache: false,
            success: function (data) {
                $("#divPartialCDSEnvironment").html(data);
                onSuccessEnvironment();
            }
        });
    }
});

$('body').on('click', '#IdRTabExportPageSettings', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabCountryGroup").parent("li").removeClass("active");
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/Portal/ExportPageSettings", container: '#divPartialExportPageSettings',
            timeout: 50000,
        }).done(function () {
            if ($.UserRole.toLowerCase() == "lob") {
                $(".isDisabled").attr("disabled", true)
            }
        });
    }
});

$('body').on('click', '#IdRTabTags', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabCountryGroup").parent("li").removeClass("active");
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/Portal/Tags", container: '#divPartialManageTags',
            timeout: 50000
        }).done(function () {
            $("table.TagsTB tbody tr:first").addClass("current");
            var Id = $("table.TagsTB tbody tr:first").attr("data-TagId");
            $.ajax({
                type: 'GET',
                url: "/Portal/AddUpdateCompanyTags",
                dataType: 'HTML',
                cache: false,
                contentType: 'application/html',
                data: { Parameters: ConvertEncrypte(encodeURI(Id)).split("+").join("***") },
                success: function (data) {
                    $("#divPartialAddUpdateTags").html(data);
                    InitPortalTagsDataTable();
                    var Tagvalue = $("#Tagvalue").val();
                    $('#TagTypeCode').val(Tagvalue)
                    if ($("table.TagsTB tbody tr").length == 0) {
                        $("#editTag").attr("disabled", true);
                    }
                }
            });
        });


    }
});

$('body').on('click', '#IdRTabCountryGroup', function () {
    if (!$(this).parent("li").hasClass("active")) {
        var url = "/Portal/indexCountryGrp";
        LoadCountryGroup(url);
    }
});

function LoadCountryGroup(url) {
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/Portal/CountryGroup", container: '#divCountry',
        timeout: 50000
    }).done(function () {
        InitPortalCountryGroupDataTable();
        $('table.CntryGrp tbody tr:first').addClass('current');
        OnSuccessCountryGrp();
    });
}
function LoadCountryGroupPopup(url) {
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        success: function (data) {
            $("#divCountry").html(data);
            InitPortalCountryGroupDataTable();
            $('table.CntryGrp tbody tr:first').addClass('current');
            OnSuccessCountryGrp();
        }
    });
}
$('body').on('click', '#IdRTabUserComment', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabCountryGroup").parent("li").removeClass("active");
        var url = "/Portal/IndexUserComments";
        LoadUserComments(url);
    }
});

function LoadUserComments(url) {
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/Portal/UserComments", container: '#divPartialUserComments',
        timeout: 50000
    }).done(function () {
        InitPortalUserCommentsDataTable();
        OnSuccessUserComments();
    });
}

//End Tab Change Event

//About US 
$('body').on('click', '#getAPIkey', function () {
    var licenseCommandLine = $(this).attr("data-licenseCommandLine").toLowerCase();
    $.ajax({
        type: 'GET',
        url: "/Portal/APICredential?Parameters=",
        dataType: 'HTML',
        success: function (data) {
            $("#PortalAboutUsModalMain").html(data);
            // Changes for Converting magnific popup to modal popup
            DraggableModalPopup("#PortalAboutUsModal");
        }
    });
});
//End About US 

function ResetCallback() {
    window.parent.$.magnificPopup.close();
    location.reload();
}
$("body").on("change", "#form_ConfigData #UserTypeCode", function () {
    CheckUserTypeForUserRole();
});
function CheckUserTypeForUserRole() {
    var currentlySelectedValues = $('#MultiSelectSecurityOptions').val();
    var userCode = $("#form_ConfigData #UserTypeCode").val();
    var isImportEnable = $("#form_ConfigData #EnableImportData").val();
    var isExportEnable = $("#form_ConfigData #EnableExportData").val();
    var dropdownUser = "";
    if (userCode == "102001") {
        if (userCode == userInitialSelectedTypeCode) {
            $('#MultiSelectSecurityOptions').multiselect("clearSelection");
            $('#MultiSelectSecurityOptions').multiselect('select', userInitialSelectedPermissions);
        }
        $('#MultiSelectSecurityOptions').multiselect("clearSelection");
        currentlySelectedValues = [...currentlySelectedValues, 'EnableImportData', 'EnableExportData'];
        $('#MultiSelectSecurityOptions').multiselect('select', currentlySelectedValues);
        dropdownUser = $('.UserRoleMultiSelect').siblings().children('.multiselect-container').find('li');
        dropdownUser.each(function () {
            var input = $(this).children('a').children('label').children('input');
            if (input.val() == 'EnableImportData') {
                input.prop('disabled', true);
            }
            if (input.val() == 'EnableExportData') {
                input.prop('disabled', true);
            }
        });
        $("#form_ConfigData #TagsValue").prop("disabled", true);
        $("#form_ConfigData #TagsValue").val("0");
        $("#form_ConfigData #Tags").val();
        $("#form_ConfigData #TagsValue").attr("data-placeholder", notApplicable);
        $("#form_ConfigData .OpenTags").hide();
        $("#form_ConfigData #TagsInclusive").prop("checked", false);
        $("#form_ConfigData #TagsInclusive").prop("disabled", true);
    }
    else if (userCode == "102002") {
        var UserId = $("#form_ConfigData #UserId").val();
        if (userCode == userInitialSelectedTypeCode) {
            $('#MultiSelectSecurityOptions').multiselect('rebuild');
            $('#MultiSelectSecurityOptions').multiselect('deselectAll', false);
            $('#MultiSelectSecurityOptions').multiselect('updateButtonText', true);
            if (UserId > 0) {
                $('#MultiSelectSecurityOptions').multiselect('select', userInitialSelectedPermissions);
            }
        }
        else {
            UserId = $("#form_ConfigData #UserId").val();
            $('#MultiSelectSecurityOptions').multiselect('rebuild');
            $('#MultiSelectSecurityOptions').multiselect('deselectAll', false);
            $('#MultiSelectSecurityOptions').multiselect('updateButtonText', true);
            if (UserId > 0) {
                $('#MultiSelectSecurityOptions').multiselect('select', userInitialSelectedPermissions);
            }
            else {
                isImportEnable = "False";
                isExportEnable = "False";
            }
        }
        dropdownUser = $('.UserRoleMultiSelect').siblings().children('.multiselect-container').find('li');
        dropdownUser.each(function () {
            var input = $(this).children('a').children('label').children('input');
            if (input.val() == 'EnableImportData') {
                input.prop('disabled', false);
                if (isImportEnable == "True") {
                    input.prop('checked', true);
                }
                else if (isImportEnable == "False") {
                    input.prop('checked', false);
                }
            }
            if (input.val() == 'EnableExportData') {
                input.prop('disabled', false);
                if (isExportEnable == "True") {
                    input.prop('checked', true);
                }
                else if (isExportEnable == "False") {
                    input.prop('checked', false);
                }
            }
        });
        $("#form_ConfigData #TagsValue").prop("disabled", false);
        $("#form_ConfigData #TagsValue").attr("data-placeholder", addTagOptional);
        $("#form_ConfigData .OpenTags").show();
        checkVAlidInclusiveTags();

    }
    $("#form_ConfigData #TagsValue").trigger("chosen:updated");

    if ($.UserRole.toLowerCase() == "lob") {
        $("#form_ConfigData #ddLOBTag").val($.UserLOBTag);
        $("#form_ConfigData #LOBTag").val($.UserLOBTag);
        $("#form_ConfigData #ddLOBTag").attr("disabled", true);
    }
}
function checkVAlidInclusiveTags() {
    var userCode = $("#form_ConfigData #UserTypeCode").val();
    if (userCode == "102002") {
        var ValidTagsValue = $("#form_ConfigData #Tags").val();
        if (ValidTagsValue != '') {
            var arraya = $("#form_ConfigData #Tags").val().split(",");
            if (arraya.length > 1) {
                $("#form_ConfigData #TagsInclusive").prop("disabled", false);
            }
            else {
                $("#form_ConfigData #TagsInclusive").prop("disabled", true);
                $("#form_ConfigData #TagsInclusive").prop("checked", false);
            }
        }
        else {
            $("#form_ConfigData #TagsInclusive").prop("disabled", true);
            $("#form_ConfigData #TagsInclusive").prop("checked", false);
        }
    }
}
$("body").on("click", "#form_ExportPageSettings #btnSubmitExportPageSettings", function () {
    var PAGESIZEMATCHOUTPUT = $("#form_ExportPageSettings #Settings_37__SettingValue").val().trim();
    var PAGESIZEENRICHMENTOUTPUT = $("#form_ExportPageSettings #Settings_38__SettingValue").val().trim();
    var PAGESIZEMONITORINGOUTPUT = $("#form_ExportPageSettings #Settings_39__SettingValue").val().trim();
    var PAGESIZEACTIVEDATAQUEUEOUTPUT = $("#form_ExportPageSettings #Settings_40__SettingValue").val().trim();
    parseInt("000000")

    var cnt = 0;
    if (PAGESIZEMATCHOUTPUT == "" || parseInt(PAGESIZEMATCHOUTPUT) == 0) {
        $("#spnPageSizeMatchOutput").show();
        cnt++;
    }
    else {
        $("#spnPageSizeMatchOutput").hide();
    }
    if (PAGESIZEENRICHMENTOUTPUT == "" || parseInt(PAGESIZEENRICHMENTOUTPUT) == 0) {
        $("#spnPageSizeEnrichmentOutput").show();
        cnt++;
    }
    else {
        $("#spnPageSizeEnrichmentOutput").hide();
    }
    if (PAGESIZEMONITORINGOUTPUT == "" || parseInt(PAGESIZEMONITORINGOUTPUT) == 0) {
        $("#spnPageSizeMonitoringOutput").show();
        cnt++;
    }
    else {
        $("#spnPageSizeMonitoringOutput").hide();
    }
    if (PAGESIZEACTIVEDATAQUEUEOUTPUT == "" || parseInt(PAGESIZEACTIVEDATAQUEUEOUTPUT) == 0) {
        $("#spnPageSizeActiveDataQueueOutput").show();
        cnt++;
    }
    else {
        $("#spnPageSizeActiveDataQueueOutput").hide();
    }

    if (cnt > 0) {
        return false;
    }

    var data = $("#form_ExportPageSettings").serialize();
    $.post("/Portal/IndexExportPageSettings/", data).done(function (data) {
        //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
        ShowMessageNotification("success", data, false)

    });
});

$(".OnlyDigit").keypress(function (e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 8);
    return ret;

});
$("body").on('blur', '.OnlyDigit', function () {
    var text = $(this).val();
    if (text != "") {
        if (!$.isNumeric(text)) {
            $(this).val("");
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
            ShowMessageNotification("success", allowOnlyNumbers, false);

        }
    }
});

$('body').on('click', '#IdCommandLine', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#CommandRightTab li").removeClass("active");
        $("#IdRTabCommandUpload").addClass("active");
        $("#CommandRightTabcontent").children("div").each(function () {
            $(this).removeClass("active");
        });
        $("#RTabCommandUpload").show();
        $("#RTabCommandDownload").hide();
        $("#RTabCommandDownloadSetup").hide();
        //$("#CommandRightTab li:first").addClass("active");
        $("#CommandRightTabcontent").children("div:first").addClass("active");
        $("#IdRTabCommandDownload").removeClass("active");
        $("#IdRTabCommandDownloadSetup").removeClass("active");
        $("#RTabConfigureImports").removeClass("active");
        $("#tabPaneIntegrationGateway").addClass("active");
        $("#RTabConfigureImports").hide();
        // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        $.pjax({
            url: "/Portal/UploadConfiguration", container: '#divPartialCommandMapping',
            timeout: 50000
        }).done(function () {
            InitPortalUploadConfigurationDataTable();
            if ($("table#tbCmndMapping tbody tr").length == 0) {
                $("#EditCommandMapping").attr("disabled", true);
            }
            else {
                $("table#tbCmndMapping tbody tr:first").addClass("current");
            }
        });
    }
});


$('body').on('blur', '#InactiveDays', function () {
    var InactiveDays = $('#InactiveDays').val();
    if (InactiveDays < 30 || InactiveDays > 365) {
        $('#InactiveDays').val("");
        return false;
    }
    else {
        $('.InactiveDaysError').attr("hidden", "hidden");
        return true;
    }
});
// DPM FTP Configuration
$('body').on('click', '#IdRTabDPMFTPConfiguration', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabCountryGroup").parent("li").removeClass("active");
        LoadDPMFTPConfiguration();
    }
});
function LoadDPMFTPConfiguration() {
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/Portal/DPMFTPConfiguration", container: '#divPartialIndexFTPConfiguration',
        timeout: 50000,
    }).done(function () {
        InitPortalDPMFTPConfigurationDataTable();
        $('table.TbFTPConfiguration tbody tr:first').addClass('current');
        OnSuccessDPMFTPConfiguration();
    });
}
function OnSuccessDPMFTPConfiguration() {
    $('table.TbFTPConfiguration tbody tr:first').addClass('current');
    var Id = $('table.TbFTPConfiguration tbody tr:first').attr("data-FTPConfigurationId");
    $.ajax({
        type: 'GET',
        url: "/Portal/AddDPMFTPConfiguration",
        dataType: 'HTML',
        contentType: 'application/html',
        cache: false,
        data: { Parameters: Id },
        success: function (data) {
            $("#divPartialAddUpdateFTPConfiguration").html(data);
            if ($("table.TbFTPConfiguration tbody tr").length == 0) {
                $("#EditFTPConfiguration").attr("disabled", true);
            }
        }
    });
    $('#divProgress').hide();
}
function EnableDPMFTPConfigurationFields() {
    $("#Url").prop("disabled", false);
    $("#Host").prop("disabled", false);
    $("#Port").prop("disabled", false);
    $("#UserName").prop("disabled", false);
    $("#Password").prop("disabled", false);
    $("#InsertDPMFTPConfiguration").show();

}
$('body').on('click', '#AddFTPConfiguration', function () {
    $("#Url").val("");
    $("#Host").val("");
    $("#Port").val("");
    $("#UserName").val("");
    $("#Password").val("");
    $("#Id").val("");
    $("#InsertDPMFTPConfiguration").val(create);
    EnableDPMFTPConfigurationFields();
    $("table.TbFTPConfiguration tr").each(function () {
        $(this).removeClass('current');
    });
});
$('body').on('click', '#EditFTPConfiguration', function () {
    EnableDPMFTPConfigurationFields();
    $("#Password").val("");
    $("#InsertDPMFTPConfiguration").val(update);
});


$('body').on('click', '.deleteFTPConfiguration', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Portal/DeleteDPMFTPConfiguration?Parameters=" + Parameters,
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {
                        ShowMessageNotification("success", data, false);
                        LoadDPMFTPConfiguration();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    return false;
});

function UpdateDPMFTPConfigration(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        LoadDPMFTPConfiguration();
    }
}

$("body").on('click', 'table.TbFTPConfiguration tbody tr', function () {
    if (!$(this).hasClass("current")) {
        $("table.TbFTPConfiguration tr").each(function () {
            $(this).removeClass('current');
        });
        $(this).closest('tr').addClass('current');
        var Id = $(this).attr("data-FTPConfigurationId");
        $.ajax({
            type: 'GET',
            url: "/Portal/AddDPMFTPConfiguration",
            dataType: 'HTML',
            contentType: 'application/html',
            data: { Parameters: Id },
            success: function (data) {
                $("#divPartialAddUpdateFTPConfiguration").html(data);
                if ($("table.TbFTPConfiguration tbody tr").length == 0) {
                    $("#EditFTPConfiguration").attr("disabled", true);
                }
            }
        });
    }
});
//END DPM FTP Configuration

// Add configuration settings to UI - Client Portal
$('body').on('click', '#IdRTabConfigurationSettings', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#IdRTabCountryGroup").parent("li").removeClass("active");
        LoadDataSourceConfiguration();
    }
});
function LoadDataSourceConfiguration() {
    $.pjax({
        url: "/Portal/ExternalSource", container: '#divPartialIndexDataSourceConfiguration',
        timeout: 50000
    }).done(function () {
        InitPortalDataSourceConfigurationDataTable();
        $('table.TbDataSourceConfiguration tbody tr:first').addClass('current');
    });
}
// While adding new external data source
$("body").on('click', '#AddDataSource', function () {
    var Parameters = "";
    $.ajax({
        type: 'GET',
        url: "/Portal/InsertUpdateExternalDataSourceConfiguration?Parameters=" + Parameters,
        dataType: 'HTML',
        success: function (data) {
            $("#divProgress").hide();
            $("#AddDataSourceConfigurationModalMain").html(data);
            DraggableModalPopup("#AddDataSourceConfigurationModal");
            $(".rbtnAmazon").attr('checked', true);
        }
    });
});
// Change as per the radio button selection
$(document).on('change', 'input[type=radio][name=DataSource]', function () {
    if ($(this).is(":checked")) {
        var DataSourceCode = $('input[type=radio][name=DataSource]:checked').val();
        if (DataSourceCode == "AZURE") {
            $(".loadAmazonSection").addClass('hidetd');
            $(".loadAzureSection").removeClass('hidetd');
            $(".loadFTPSection").addClass('hidetd');
            $(".loadSFTPSection").addClass('hidetd');
            ClearDataSourceValuesOnChange();
        }
        else if (DataSourceCode == "FTP") {
            $(".loadAmazonSection").addClass('hidetd');
            $(".loadAzureSection").addClass('hidetd');
            $(".loadFTPSection").removeClass('hidetd');
            $(".loadSFTPSection").addClass('hidetd');
            ClearDataSourceValuesOnChange();
        }
        else if (DataSourceCode == "SFTP") {
            $(".loadAmazonSection").addClass('hidetd');
            $(".loadAzureSection").addClass('hidetd');
            $(".loadFTPSection").addClass('hidetd');
            $(".loadSFTPSection").removeClass('hidetd');
            ClearDataSourceValuesOnChange();
        }
        else {
            $(".loadAmazonSection").removeClass('hidetd');
            $(".loadAzureSection").addClass('hidetd');
            $(".loadFTPSection").addClass('hidetd');
            $(".loadSFTPSection").addClass('hidetd');
            ClearDataSourceValuesOnChange();
        }
    }
});
// While updating new external data source
$("body").on('click', '.editExternalDataSourceConfiguration', function () {
    var Parameters = $(this).attr("id");
    $.ajax({
        type: 'GET',
        url: "/Portal/InsertUpdateExternalDataSourceConfiguration?Parameters=" + Parameters,
        dataType: 'HTML',
        success: function (data) {
            $("#divProgress").hide();
            $("#AddDataSourceConfigurationModalMain").html(data);
            DraggableModalPopup("#AddDataSourceConfigurationModal");
        }
    });
});
// While deleting new external data source
$('body').on('click', '.deleteExternalDataSourceConfiguration', function () {
    var Id = $(this).attr("id");
    var UserId = $(this).attr("data-UserId");
    var QueryString = "Id:" + Id + "@#$UserId:" + UserId;
    var Parameters = ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");
    //var Parameters = 
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Portal/DeleteExternalDataSourceConfiguration?Parameters=" + Parameters,
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {
                        ShowMessageNotification("success", data.message, false);
                        LoadDataSourceConfiguration();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    return false;
});
// File format validation
$("body").on('change', '.browserFileForSFTP', function () {
        var fileExtension = ['pem'];
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
            var allowedFormats = "Only formats are allowed : " + fileExtension.join(', ');
            ShowMessageNotification("success", allowedFormats, false);
    }
    $("#spnSFTPSSHKeyFileUpload").hide();
});
// Fields validations while adding/updating external data source
$("body").on('click', '#btnSaveConfigurationSettings', function () {
    var DSCode = $('input[type="radio"][name=DataSource]:checked').val();
    $("#DataSourceCode").val(DSCode);
    var AccessKey = $("#amazon_AccessKey").val();
    var SecurityKey = $("#amazon_SecurityKey").val();
    var ServiceURL = $("#amazon_ServiceURL").val();
    var AmazonExternalDataStoreName = $("#amazon_AWSExternalDataStoreName").val();
    var AccountName = $("#azure_AccountName").val();
    var AccountKey = $("#azure_AccountKey").val();
    var EndpointSuffix = $("#azure_EndpointSuffix").val();
    var AzureExternalDataStoreName = $("#azure_AzureExternalDataStoreName").val();
    var FTPExternalDataSourceName = $("#ftp_FTPExternalDataStoreName").val();
    var Host = $("#ftp_Host").val();
    var UserName = $("#ftp_UserName").val();
    var Password = $("#ftp_Password").val();
    var SFTPHost = $("#sftp_SFTPHost").val();
    var SFTPPort = $("#sftp_SFTPPort").val();
    var SFTPUserName = $("#sftp_SFTPUserName").val();
    var SSHFilePath = $(".browserFileForSFTP").val();
    var SFTPExternalDataStoreName = $("#sftp_SFTPExternalDataStoreName").val(); 
    var cnt = 0;
    if (DSCode == "AWS") {
        if (AccessKey == '') {
            $("#spnAccessKey").show();
            cnt++;
        }
        else {
            $("#spnAccessKey").hide();
        }
        if (SecurityKey == '') {
            $("#spnSecurityKey").show();
            cnt++;
        }
        else {
            $("#spnSecurityKey").hide();
        }
        if (ServiceURL == '') {
            $("#spnServiceURL").show();
            cnt++;
        }
        else {
            $("#spnServiceURL").hide();
        }
        if (AmazonExternalDataStoreName == '') {
            $("#spnExternalDataSourceAmazon").show();
            cnt++;
        }
        else {
            $("#spnExternalDataSourceAmazon").hide();
        }
    }
    else if (DSCode == "AZURE") {
        if (AccountName == '') {
            $("#spnAccountName").show();
            cnt++;
        }
        else {
            $("#spnAccountName").hide();
        }
        if (AccountKey == '') {
            $("#spnAccountKey").show();
            cnt++;
        }
        else {
            $("#spnAccountKey").hide();
        }
        if (EndpointSuffix == '') {
            $("#spnEndpointSuffix").show();
            cnt++;
        }
        else {
            $("#spnEndpointSuffix").hide();
        }
        if (AzureExternalDataStoreName == '') {
            $("#spnExternalDataSourceAzure").show();
            cnt++;
        }
        else {
            $("#spnExternalDataSourceAzure").hide();
        }
    }
    else if (DSCode == "FTP") {
        if (Host == '') {
            $("#spnHost").show();
            cnt++;
        }
        else {
            $("#spnHost").hide();
        }
        if (UserName == '') {
            $("#spnUserName").show();
            cnt++;
        }
        else {
            $("#spnUserName").hide();
        }
        if (Password == '') {
            $("#spnPassword").show();
            cnt++;
        }
        else {
            $("#spnPassword").hide();
        }
        if (FTPExternalDataSourceName == '') {
            $("#spnExternalDataSourceFTP").show();
            cnt++;
        }
        else {
            $("#spnExternalDataSourceFTP").hide();
        }
    }
    else if (DSCode =="SFTP") {
        if (SFTPHost == '') {
            $("#spnSFTPHost").show();
            cnt++;
        }
        else {
            $("#spnSFTPHost").hide();
        }
        if (SFTPPort == '') {
            $("#spnSFTPPort").show();
            cnt++;
        }
        else {
            $("#spnSFTPPort").hide();
        }
        if (SFTPUserName == '') {
            $("#spnSFTPUserName").show();
            cnt++;
        }
        else {
            $("#spnSFTPUserName").hide();
        }
        if (SSHFilePath == '' || !SSHFilePath.includes("pem")) {
            $("#spnSFTPSSHKeyFileUpload").show();
            $(".browserFileForSFTP").val('');
            cnt++;
        }
        else {
            $("#spnSFTPSSHKeyFileUpload").hide();
        }
        if (SFTPExternalDataStoreName == '') {
            $("#spnExternalDataSourceSFTP").show();
            cnt++;
        }
        else {
            $("#spnExternalDataSourceSFTP").hide();
        }
    }
    if (cnt > 0) {
        return false;
    }
    else { return true; }
});
$("body").on('blur', '#ftp_Port', function () {
    var text = $(this).val();
    if (text != "") {
        if (!$.isNumeric(text)) {
            $(this).val("");
            //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass "error" than show bootbox message.
            parent.ShowMessageNotification("success", allowOnlyNumbers, false);
        }
    }
});
// Display notification message on success
function UpdateExternalDataSourceConfigration(data) {
    //show notification message instade of Bootbox Message...if you pass "success" than show Notification message and if you pass error than show bootbox message.
    ShowMessageNotification("success", data.message, false);
    if (data.result) {
        $("#AddDataSourceConfigurationModal").modal("hide");
        LoadDataSourceConfiguration();
    }
}
// Clear external data source values on radio button selection
function ClearDataSourceValuesOnChange() {
    $("#amazon_AccessKey").val('');
    $("#amazon_SecurityKey").val('');
    $("#amazon_ServiceURL").val('');
    $("#azure_AccountName").val('');
    $("#azure_AccountKey").val('');
    $("#azure_EndpointSuffix").val('');
    $("#ftp_Host").val('');
    $("#ftp_Port").val('');
    $("#ftp_UserName").val('');
    $("#ftp_Password").val('');
    $("#sftp_SFTPHost").val('');
    $("#sftp_SFTPPort").val('');
    $("#sftp_SFTPUserName").val('');
    $("#sftp_SSHFile").val('');
    $("#spnAccessKey").hide();
    $("#spnSecurityKey").hide();
    $("#spnServiceURL").hide();
    $("#spnExternalDataSourceAmazon").hide();
    $("#spnAccountName").hide();
    $("#spnAccountKey").hide();
    $("#spnEndpointSuffix").hide();
    $("#spnExternalDataSourceAzure").hide();
    $("#spnExternalDataSourceFTP").hide();
    $("#spnHost").hide();
    $("#spnUserName").hide();
    $("#spnPassword").hide();
    $("#spnSFTPHost").hide();
    $("#spnSFTPPort").hide();
    $("#spnSFTPUserName").hide();
    $("#spnExternalDataSourceSFTP").hide();
    $("#spnSFTPSSHKeyFileUpload").hide();
}
// END Add configuration settings to UI - Client Portal

//New Process settings for transfer duns enrichment MP-507
$("body").on('change', '#EnablePurgeArchivet', function () {
    if (!$('input#EnablePurgeArchivet').is(':checked')) {
        $("#ArchivePeriodDays").attr("disabled", false);
    }
    else {
        $("#ArchivePeriodDays").attr("disabled", "disabled");
    }
});

// User Story 105-File Import Configuration settings page - Client Portal
$('body').on('click', '#IdDataGovernance', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#CommandRightTab li").removeClass("active");
        $("#CommandRightTabcontent").children("div").each(function () {
            $(this).removeClass("active");
        });

        $("#FeatureTab").removeClass("active");
        $("#UsersTab").removeClass("active");
        $("#CommonTab").removeClass("active");
        $("#AboutUsTab").removeClass("active");

        $("#CommandRightTab li:first").addClass("active");
        $("#CommandLine").addClass("active");
        $("#RTabConfigureImports").show();
        $("#IdCommandLine").removeClass("active");
        LoadConfigureImports();
    }
});

$('body').on('click', '#IdRTabConfigureImports', function () {
    if (!$(this).parent("li").hasClass("active")) {
        $("#CommandRightTab li").removeClass("active");
        $("#CommandRightTabcontent").children("div").each(function () {
            $(this).removeClass("active");
        });

        $("#FeatureTab").removeClass("active");
        $("#UsersTab").removeClass("active");
        $("#CommonTab").removeClass("active");
        $("#AboutUsTab").removeClass("active");

        $("#CommandRightTab li:first").addClass("active");
        $("#CommandLine").addClass("active");
        $("#RTabConfigureImports").show();
        $("#IdCommandLine").removeClass("active");
        LoadConfigureImports();
    }
});
function InsertUpdateConfigureImports(Id, TemplateId) {
    // Changes for Converting magnific popup to modal popup
    $.ajax({
        type: 'GET',
        url: "/Portal/InsertUpdateConfigureImports?Parameters=" + ConvertEncrypte(encodeURI(Id)).split("+").join("***"),
        dataType: 'HTML',
        success: function (data) {
            $("#ConfigureImportsModalMain").html(data);
            DraggableModalPopup("#ConfigureImportsModal");
            if (TemplateId > 0) {
                $(".dropdownIcon").html("<i class='fa fa-info-circle fa-lg templateInfoIcon' style='color: black;' data-TemplateId='" + TemplateId + "' style='display: none;'></i>");
                $(".templateInfoIcon").show();
                $("#addTransferDunsTag").hide();
            }
            $("#PostLoadAction").trigger("change");
        }
    });
}
$('body').on('click', '.deleteConfigureImports', function () {
    var Parameters = $(this).attr("id");
    var token = $('input[name="__RequestVerificationToken"]').val();
    bootbox.confirm({
        title: "<i class='fa fa-check-square-o' aria-hidden='true'></i> " + confirmBox, message: deleteRecordMsg, callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Portal/DeleteConfigureImports?Parameters=" + ConvertEncrypte(encodeURI(Parameters)).split("+").join("***"),
                    dataType: "JSON",
                    headers: { "__RequestVerificationToken": token },
                    contentType: "application/json",
                    success: function (data) {
                        ShowMessageNotification("success", data.message, false);
                        LoadConfigureImports();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    });
    return false;
});
$("body").on('change', '#PostLoadAction', function () {
    var PostLoadAction = $("#PostLoadAction").val();
    if (PostLoadAction == "ARCHIVE") {
        $("#PostLoadActionParameters").attr("readonly", false);
        $(".setCheckBoxForPostLoadAction").show();
        $(".txtAppendUTCTime").show();
        $('#PostLoadActionParameters').attr('placeholder', ArchivePath);
    }
    else if (PostLoadAction == "RENAME") {
        $("#PostLoadActionParameters").attr("readonly", false);
        $(".setCheckBoxForPostLoadAction").show();
        $(".txtAppendUTCTime").show();
        $('#PostLoadActionParameters').attr('placeholder', NewFileExtension);
    }
    else {
        $("#PostLoadActionParameters").attr("readonly", true);
        $("#spnPostLoadActionParameters").hide();
        $(".setCheckBoxForPostLoadAction").hide();
        $(".txtAppendUTCTime").hide();
        $("#PostLoadActionParameters").val('');
        $('#PostLoadActionParameters').attr('placeholder', PostLoadActionParameters);
    }
});
$("body").on('change', '#TemplateId', function () {
    var Template = $("#TemplateId").val();
    if (Template > 0) {
        $(".dropdownIcon").html("<i class='fa fa-info-circle fa-lg templateInfoIcon' style='color: black;' data-TemplateId='" + Template + "' style='display: none;'></i>");
        $(".templateInfoIcon").show();
        $("#addTransferDunsTag").hide();
    }
    else {
        $(".templateInfoIcon").hide();
        $("#addTransferDunsTag").show();
    }
});
function addNewTemplate() {
    $.ajax({
        type: 'GET',
        url: "/ImportData/ImportFileIndex?IsTemplateSelected=" + false,
        dataType: 'HTML',
        success: function (data) {
            $("#ImportFileIndexModalMain").html(data);
            // reset modal if it isn't visible
            DraggableModalPopup("#ImportFileIndexModal");

        }
    });
}
function LoadConfigureImports() {
    $("#IdCommandLine").parent().removeClass("active");
    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
    $.pjax({
        url: "/Portal/ConfigureImports", container: '#divPartialConfigureImports',
        timeout: 50000
    }).done(function () {
        InitPortalConfigureImportDataTable();
        if ($("table#tbConfigureImport tbody tr").length == 0) {
            $("table#tbConfigureImport").attr("disabled", true);

        }
        else {
            $("table#tbConfigureImport tbody tr:first").addClass("current");

        }
    });
}
//END TASK User Story 105-File Import Configuration settings page - Client Portal

// Function for encrypting the parameters
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

function InitPortalUserDataTable() {
    InitDataTable(".userTB", [10, 20, 30], false, [0, "desc"]);
}
function InitPortalCountryGroupDataTable() {
    InitDataTable(".CntryGrp", [10, 20, 30], false, [0, "desc"]);
}
function InitPortalTagsDataTable() {
    InitDataTable(".TagsTB", [10, 20, 30], false, [0, "desc"]);
}
function InitPortalUserCommentsDataTable() {
    InitDataTable(".UserCommentTB", [10, 20, 30], false, [0, "desc"]);
}
function InitPortalDPMFTPConfigurationDataTable() {
    InitDataTable(".TbFTPConfiguration", [10, 20, 30], false, [0, "desc"]);
}
function InitPortalUploadConfigurationDataTable() {
    InitDataTable("#tbCmndMapping", [10, 20, 30], false, [0, "desc"]);
}
function InitPortalDataSourceConfigurationDataTable() {
    InitDataTable(".TbDataSourceConfiguration", [10, 20, 30], false, [0, "asc"]);
}
function InitPortalConfigureImportDataTable() {
    InitDataTable("#tbConfigureImport", [10, 20, 30], false, [0, "asc"]);
}