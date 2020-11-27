function DuplocateFamilyTree() {
    var _FamilyTreeId = 0;
    var _FamilyTreeName = '';
    var _FamilyTreeType = '';


    if (window.parent.$("#rdoSingle").is(":checked")) {
        _FamilyTreeId = window.parent.$("#FamilyTreeId").val();
        _FamilyTreeName = $("#txtDuplicateFamilyTreeName").val();
        _FamilyTreeType = $("#txtDuplicateFamilyTreeType").val();
    }
    else {
        _FamilyTreeId = window.parent.$("#RightView_FamilyTreeId").val();
        _FamilyTreeName = $("#txtDuplicateFamilyTreeName").val();
        _FamilyTreeType = $("#txtDuplicateFamilyTreeType").val();
    }
    var cnt = 0;
    if (_FamilyTreeName == "" || (_FamilyTreeName && _FamilyTreeName.trim() == "")) {
        $("#spnFamilyTreeName").show();
        cnt++;
    }
    else {
        $("#spnFamilyTreeName").hide();
    }
    if (_FamilyTreeType == "" || (_FamilyTreeType && _FamilyTreeType.trim() == "")) {
        $("#spnFamilyTreeType").show();
        cnt++;
    }
    else {
        $("#spnFamilyTreeType").hide();
    }
    if (cnt > 0) {
        return false;
    }

    var token = $('input[name="__RequestVerificationToken"]').val();
    var QueryString = "FamilyTreeId:" + _FamilyTreeId + "@#$FamilyTreeName:" + _FamilyTreeName + "@#$FamilyTreeType:" + _FamilyTreeType;
    var Parameters = parent.ConvertEncrypte(encodeURI(QueryString)).split("+").join("***");

    $.ajax({
        type: "POST",
        url: "/FamilyTree/DuplicateFamilyTree/",
        data: JSON.stringify({ Parameters: Parameters }),
        contentType: "application/json",
        headers: { "__RequestVerificationToken": token },
        success: function (data) {
            $("#DuplicateFamilyTreeModal").modal("hide");
            parent.ShowMessageNotification("success", data.Message, false);
            if (data.result) {
                window.parent.ChangeView();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

        }
    });
}