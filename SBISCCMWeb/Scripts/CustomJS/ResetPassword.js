function closemsg() {
    $(".alert-success").hide();
}
// Validate password Complexity

$('body').on('blur', '#PasswordHash', function () {
    $('#PasswordHash').attr('autocomplete', 'off');
    var passwordHash = $("#PasswordHash").val();
    if (passwordHash.length < 8) {
        $("#spnValidPassword").show();
    } else {
        $("#spnValidPassword").hide();
    }

    if (!checkStrength(passwordHash)) {
        $("#spnValidPassword").show();
    }
    else {
        $("#spnValidPassword").hide();
    }
});
//// Validate password and comfirm password are same or not.
$('body').on('blur', '#ConfirmPassword', function () {
    var passwordHash = $("#PasswordHash").val();
    var ConfirmPassword = $("#ConfirmPassword").val();
    var count = 0;
    if (ConfirmPassword == '') {
        //$("#spnConfirmPassword").show();
        count = 1;
    } else {
        //$("#spnConfirmPassword").hide();
    }
    if (count == 0) {
        if (passwordHash != ConfirmPassword) {
            $('.frmResetPassword #btnResetPassword').attr("disabled", true);
        } else {
            $('.frmResetPassword #btnResetPassword').attr("disabled", false);
        }
    }
    deleteAllCookies();
});
//// Validate Reset password fileds and password complexity and if fail than return false and not sumbit form
$('body').on('click', '#btnResetPassword', function () {

    var passwordHash = $("#PasswordHash").val();
    var ConfirmPassword = $("#ConfirmPassword").val();

    var count = 0;
    if (passwordHash == '') {
        $("#spnValidPassword").show();
        count++;
    }
    else {
        $("#spnValidPassword").hide();
    }
    if (ConfirmPassword == 0) {
        //$("#spnConfirmPassword").show();
        count++;
    }
    else {
        //$("#spnConfirmPassword").hide();
    }
    if (count == 0) {
        if (passwordHash != ConfirmPassword) {
            //$("#spnMatch").show();
            count++;
        } else {
            //$("#spnMatch").hide();
        }
    }

    if (!checkStrength(passwordHash)) {
        $("#spnValidPassword").show();
        count++;
    }
    else {
        $("#spnValidPassword").hide();
    }

    if (!checkStrength(ConfirmPassword)) {
        //$("#spnConfirmPassword").show();
        count++;
    }
    else {
        //$("#spnConfirmPassword").hide();
    }

    if (count > 0) {
        return false;
    }
});

// Delete all cookies
function deleteAllCookies() {
    var cookies = document.cookie.split(";");
    var d = new Date();
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=" + d.toGMTString() + ";" + ";"

    }
}

$("body").on('focus', '#Email', function () {
    $("#Email").attr('readonly', false);
});
$("body").on('focus', '#Password', function () {
    $("#Password").attr('readonly', false);
});
$("body").on('focus', '#PasswordHash', function () {
    $("#PasswordHash").attr('readonly', false);
    $('#PasswordHash').attr('autocomplete', 'off');
    //RandomPassword();
});
$("body").on('focus', '#ConfirmPassword', function () {
    $("#ConfirmPassword").attr('readonly', false);
});
$("#PasswordHash").focusout(function () {
});

$('#PasswordHash').keyup(function () {
    if (checkStrength($('#PasswordHash').val())) {
        $("#PasswordHash").show();
    }
    else {
        $("#PasswordHash").show();
    }
});
$('#Password').keyup(function () {
    if (checkStrength($('#Password').val())) {
        $("#Password").hide();
    }
    else {
        $("#Password").show();
    }
});
function checkStrength(password) {
    var strength = 0;
    //if any number is typed
    if (password.match(/([0-9])/)) {
        $('.NumberCheckBox').show();
        $('#listNumber').css('color', 'green');
        $('.NumberCheckBox').css('color', 'green');
    }
    else {
        $('#listNumber').css('color', 'black');
        $('.NumberCheckBox').hide();
    }

    //if any uppercase character is typed
    if (password.match(/([A-Z])/)) {
        $('.UpperCaseCheckBox').show();
        $('#listUpper').css('color', 'green');
        $('.UpperCaseCheckBox').css('color', 'green');
    }
    else {
        $('#listUpper').css('color', 'black');
        $('.UpperCaseCheckBox').hide();
    }

    //if any lowercase character is typed
    if (password.match(/([a-z])/)) {
        $('.LowerCaseCheckBox').show();
        $('#listLower').css('color', 'green');
        $('.LowerCaseCheckBox').css('color', 'green');
    }
    else {
        $('#listLower').css('color', 'black');
        $('.LowerCaseCheckBox').hide();
    }

    //if any special character is typed
    if (password.match(/([!,@,#,$,%,^,&,*,(,),-,+,<,>,?,_])/)) {
        $('.CharacterCheckBox').show();
        $('#listSpChar').css('color', 'green');
        $('.CharacterCheckBox').css('color', 'green');
    }
    else {
        $('#listSpChar').css('color', 'black');
        $('.CharacterCheckBox').hide();
    }

    if (password.length < 8) {
        return false;
    }

    if (password.match(/([0-9])/)) {
        strength += 1;
    }
    if (password.match(/([a-z])/)) {
        strength += 1;
    }
    if (password.match(/([A-Z])/)) {
        strength += 1;
    }
    if (password.match(/([!,@,#,$,%,^,&,*,(,),-,+,<,>,?,_])/)) {
        strength += 1;
    }

    // if any 3 out of 4 matches then border of the box becomes green
    if (strength >= 3) {
        $('.BorderColor').css("border", "green solid 1px");
        $('.NumberCheckBox').css('color', 'green');
        $('.UpperCaseCheckBox').css('color', 'green');
        $('.LowerCaseCheckBox').css('color', 'green');
        $('.CharacterCheckBox').css('color', 'green');
        return true;
    }
    else if (strength < 3) {
        $('.BorderColor').css("border", "none");
    }
    else {
        return false;
    }
}

function OnSuccessresetPassword() {
    backToparent();
}