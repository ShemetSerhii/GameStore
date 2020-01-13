$(function() {

    if (!$('#IsPublisher').checked) {
        $('#CompanyNameDiv').toggle();
    }

    $('#IsPublisher').bind("change", function () {
        $('#CompanyNameDiv').toggle();
    })
});