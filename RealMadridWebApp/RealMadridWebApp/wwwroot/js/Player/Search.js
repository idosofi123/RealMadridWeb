$(function () {
 /*   $('form').submit(function (e) {
        e.preventDefault();

        var prefferedFoot = $('#prefferedFoot').val();
        var country = $('#country').val();
        var minAge = $('#minAge').val();
        var maxAge = $('#maxAge').val();


        $.ajax({
            url: '/Players/Search',
            data: { prefferedFoot, country: country.join(), minAge, maxAge  }
        }).done(function (data) {
            $('#renderHere').html('');

            var template = $('#overall-template').html();

            $.each(data, function (i, val) {

                var temp = template;

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('#renderHere').append(temp);
            });
        });
    });*/
});



