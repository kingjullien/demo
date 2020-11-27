function AddFamilyTreeNode() {
    var e = 0,
        a = 0,
        t = "",
        i = "",
        l = "",
        n = 0;
    window.parent.$("#rdoSingle").is(":checked")
        ? ((e = window.parent.$("#FamilyTreeId").val()), (a = $("#txtDetailId").val()), (t = $("#txtName").val()), (i = $("#txtNodeDisplayDetails").val()), (l = $("#txtNodeType").val()), (n = $("#hdnSourceFamilyDetailtreeId").val()))
        : ((e = window.parent.$("#RightView_FamilyTreeId").val()),
            (a = $("#txtDetailId").val()),
            (t = $("#txtName").val()),
            (i = $("#txtNodeDisplayDetails").val()),
            (l = $("#txtNodeType").val()),
            (n = $("#hdnSourceFamilyDetailtreeId").val()));
    var o = $('input[name="__RequestVerificationToken"]').val(),
        r = "FamilyTreeId:" + e + "@#$DetailId:" + a + "@#$NodeName:" + t + "@#$NodeDisplayDetail:" + i + "@#$NodeType:" + l + "@#$ParentFamilyTreeDetailId:" + n,
        d = parent.ConvertEncrypte(encodeURI(r)).split("+").join("***");
    $.ajax({
        type: "POST",
        url: "/FamilyTree/AddFamilyTreeNode",
        data: JSON.stringify({ Parameters: d }),
        contentType: "application/json",
        headers: { __RequestVerificationToken: o },
        success: function (e) {
            $("#AddFamilyNodeModal").modal("hide");
            parent.ShowMessageNotification("success", e.Message, !1), e.result && (window.parent.ChangeView(), window.parent.$.magnificPopup.close());
        },
        error: function (e, a, t) { },
    });
}
