$(function () {
    $('form').submit(function (e) {
        e.preventDefault();

        var prefferedFoot = $('#prefferedFoot').val();
        var country = $('#country').val();
        var minAge = $('#minAge').val();
        var maxAge = $('#maxAge').val();


        $.ajax({
            url: '/Players/Search',
            data: { prefferedFoot, country: country.join(), minAge, maxAge  }
        }).done(function (data) {
            $('#playersSection').html('');

            var positionGrpTemplate = $('#positionGroupTemplate').html();
            var playerTemplate = $('#playerTemplate').html();

            if (data.length === 0) $('#playersSection').append($('#noDataFound').html())

            $.each(data, function (i, group) {

                var positionTemp = positionGrpTemplate.replace('{PositionName}', group.key.positionName).replace('{count}', group.count);
                let groupBody = '';

                $.each(group.players, function (i, player) {
                    var tempPlayerTemplate = playerTemplate;
                    $.each(player, function (key, value) {
                    tempPlayerTemplate = tempPlayerTemplate.replaceAll('{' + key + '}', value);
                    });
                    groupBody += tempPlayerTemplate.replaceAll('{birthCountry.imagePath}', player.birthCountry.imagePath); // nested prop
                });


                positionTemp = positionTemp.replaceAll('{groupBody}', groupBody);

                $('#playersSection').append(positionTemp);

            });
        });
    });
});



