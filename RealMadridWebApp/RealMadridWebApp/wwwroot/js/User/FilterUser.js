$(function () {
    $('form').submit(function (e) {
        e.preventDefault();

        var role = $('#role').val();

            $.ajax({
                url: '/Users/FilterUser',
                data: { 'role': role }
            }).done(function (data) {
                $('tbody').html('');

                var template = $('#user-row-template').html();

                $.each(data, function (i, val) {
                    var temp = template;

                    $.each(val, function (key, value) {

                        temp = temp.replaceAll('{' + key + '}', value);
                    });
                    $('tbody').append(temp);
                });
            })
    });
});