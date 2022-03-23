successUpdate = function (xhr) {
    alert(xhr);
};

failureUpdate = function (xhr) {
    alert(`Status code ${xhr.status}, message ${xhr.responseText}`);
}