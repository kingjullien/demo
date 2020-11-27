$("body").on('blur', '#EmailAddress', function () {
    $("#Password").val('');
});
$("body").on('focus', '#Password', function () {
    $("#Password").attr('readonly', false);
    $("#Password").val('');

});
$("body").on('focus', '#EmailAddress', function () {
    $("#EmailAddress").attr('readonly', false);
});

$("body").on('focus', '#Email', function () {
    $("#Email").attr('readonly', false); 
});

$("body").on('click', '#copylink', function () {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($("#copyaddress").text()).select();
    document.execCommand("copy");
    $temp.remove();
});


