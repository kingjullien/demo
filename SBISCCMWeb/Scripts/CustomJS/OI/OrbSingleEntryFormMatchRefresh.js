$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();

//Opens the popup for adding tags on clicking it
$(document).on('click', '.OpenTags', function () {
    $.magnificPopup.open({
        preloader: true,
        closeBtnInside: true,
        type: 'iframe',
        items: {
            src: '/Tags/AddTags/?isAllowLOBTag=' + false
        },
        callbacks: {
            close: function () {

            }
        },
        closeOnBgClick: false,
        mainClass: 'popupAddTags'
    });
});

//Click Import data button and validation for the required fields.
function OISingleEntryMatchRefreshValidation() {
    var Tags = $('#OrbSingleEntryTags').val();

    if ($.UserRole.toLowerCase() == 'lob' && Tags == '') {
        $("#spnTags").show();
        return false;
    }
    else {
        $("#spnTags").hide();
        return true;
    }
}