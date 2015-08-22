function fillDefault() {
    $("#tenant").val("fabrikamb2c.onmicrosoft.com");
    $("#client_id").val("79467a70-1adc-41a2-9d0a-faebefb5866c");
    $("#sign_in_policy").val("b2c_1_sign_in");
    $("#sign_up_policy").val("b2c_1_sign_up");
    $("#edit_profile_policy").val("b2c_1_edit_profile");
}

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    console.log('Tooltip loaded');
});
