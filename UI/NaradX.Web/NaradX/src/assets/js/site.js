$(document).ready(function () {
    $('#searchForm').submit(function (e) {
        e.preventDefault();
        var $form = $(this);
        $.ajax({
            url: $form.attr('action'),
            type: 'POST',
            data: $form.serialize(),
            success: function (result) {
                $('#contactsTableContainer').html(result);
            },
            error: function () {
                alert('Error loading contacts.');
            }
        });
    });
});