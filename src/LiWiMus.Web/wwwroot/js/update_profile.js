successUpdate = function (xhr) {
    alert(xhr);
};

failureUpdate = function (xhr) {
    alert(`Status code ${xhr.status}, message ${xhr.responseText}`);
}

$(document).ready(() => {
    let isShowSaveBtn = false;
    let saveBtn = $("#btn-save-profile-info");
    let editInputs = $(".disabled-input");
    $("#btn-edit-profile-info").click(() => {
        if (isShowSaveBtn) {
            $(saveBtn).hide();
        } else {
            $(saveBtn).show();
        }

        $(editInputs).prop('disabled', isShowSaveBtn);
        isShowSaveBtn = !isShowSaveBtn;
    })
})