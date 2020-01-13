$(function () {
    $(".date-box > input")
        .datepicker({ dateFormat: 'm/d/yy' })
        .attr('type', 'text');
});