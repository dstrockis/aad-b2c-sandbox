function fillDefault() {
    $("#tenant").val("fabrikamb2c.onmicrosoft.com");
    $("#client_id").val("90c0fe63-bcf2-44d5-8fb7-b8bbc0b29dc6");
    $("#sign_in_policy").val("b2c_1_sign_in");
    $("#sign_up_policy").val("b2c_1_sign_up");
    $("#edit_profile_policy").val("b2c_1_edit_profile");
}

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    console.log('Tooltip loaded');
});
