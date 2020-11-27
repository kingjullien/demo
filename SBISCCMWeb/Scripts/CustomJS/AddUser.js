function closemsg() {
    $(".alert-success").hide();
}
// Check validation of add user popup and if fail than return and not submit the form.
$('body').on('click', '.btnConfigDataUser', function () {

    var UserName = $("#UserName").val();
    //var LoginId = $("#LoginId").val();
    var PasswordHash = $("#PasswordHash").val();
    var EmailAddress = $("#EmailAddress").val();
    var count = 0;
    if (UserName == '') {
        $("#spnUserName").show();
        count++;
    }
    else {
        $("#spnUserName").hide();
    }
    //if (LoginId == 0) {
    //    $("#spnLoginId").show();
    //    count++;
    //}
    //else {
    //    $("#spnLoginId").hide();
    //}
    if (PasswordHash != undefined) {
        if (PasswordHash == '') {
            $("#spnpassword").show();
            count++;
        }
        else {
            $("#spnpassword").hide();
            var password = new RegExp('^((?=.*?[a-z])|(?=.*?[A-Z]))(?=.*?[0-9])(?=.*?[#?!@$%^<>*-]).{8,50}$');
            if (password.test(PasswordHash)) {
                $("#spnpasswordLength").hide();
            } else {
                $("#spnpasswordLength").show();
                count++;

            }

        }
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

    if (count > 0) {
        return false;
    }

});

$("body").on('click', '.OpenTags', function () {
    $.magnificPopup.open({
        preloader: true,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/Data/AddTags/'
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: 'popupAddTags'
    });
});
$(document).ready(function () {
    LoadTags();
});
function SetTagValue(OptionValue) {
    $.magnificPopup.close();
    //var x = document.getElementById("TagsValue");
    //var option = document.createElement("option");
    //option.text = OptionValue;
    //option.value = OptionValue;
    //x.add(option);
    //$(".chzn-select").trigger("chosen:updated");
      updateTagListOnchangeOrAdd();
}

function UserUpdateSuccess() {
    var message = $("#Messgae").val();
    //alert(message);
    bootbox.alert({
        title: "<i class='fa fa-info-circle' aria-hidden='true'></i> Message",
        message: message, callback: function () {
            LoadTags();
            if (message == "Data created successfully") {
                $('#form_ConfigData input[type=text]').val('');
                $('#form_ConfigData input[type=password]').val('');
                $('#form_ConfigData input[type=checkbox]').attr("checked", false);
                $("#form_ConfigData .chzn-select").val('').trigger("chosen:updated");
                $("#form_ConfigData #LOBTag").val('');
            }
        }
    });
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
            }
        });
    }
}
$('body').on('change', '#LOBTag', function () {
    updateTagListOnchangeOrAdd();
});
function updateTagListOnchangeOrAdd()
{
    var LOBTags = $("#LOBTag option:selected").text();
    LOBTags = LOBTags.toLowerCase() == "select log tag" ? "" : LOBTags;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        type: "POST",
        url: '/ConfigurationData/GetLOBTags',
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