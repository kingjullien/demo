$(document).ready(function () {
    LoadImportPanelMatchTags();
});
$.UserRole = $("#UserRole").val();
$.UserLOBTag = $("#UserLOBTag").val();

//Opens the popup for adding tags on clicking it
$("body").on('click', '.OpenTags', function () {
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

function LoadImportPanelMatchTags() {
    if ($("#OrbImportPanelTagsValue").length > 0) {
        $("#OrbImportPanelTagsValue").chosen().change(function (event) {
            if (event.target == this) {
                $("#OrbImportPanelMatchRefreshTags").val($(this).val());
            }
        });
    }
}

//Click Import data button and validation for the required fields.
function OIImportPanelRefreshValidation() {
    var Tags = $('#OrbImportPanelMatchRefreshTags').val();

    if ($.UserRole.toLowerCase() == 'lob' && Tags == '') {
        $("#spnTags").show();
        return false;
    }
    else {
        $("#spnTags").hide();
        return true;
    }
}