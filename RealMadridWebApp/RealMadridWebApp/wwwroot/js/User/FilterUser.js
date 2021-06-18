$(function () {
    $('form').submit(function (e) {
        e.preventDefault();

        var role = $('#role').val();

        var userName = $('#userName').val();

        var fromDate = $('#fromDate').val();
        var toDate = $('#toDate').val();

            $.ajax({
                url: '/Users/FilterUsers',
                data: {
                    'role': role,
                    'stringRole': role,
                    'userName': userName,
                    'fromDate': fromDate,
                    'toDate': toDate
                }
            }).done(function (data) {
                setUsersData(data);
            })
    });
});

function setUsersData(data) {

    var roles = [];

    $.ajax({
        url: '/Users/GetRolesValue',
    }).done(function (rolesData) {
        roles = rolesData;

        $('tbody').html('');
    
        var template = $('#user-row-template').html();

        $.each(data, function (i, val) {
            var temp = template;

            $.each(val, function (key, value) {

                value = (key == "type" ? roles[value] : value);
                value = (key.includes("Date") ? setDateFormat(value) : value);

                temp = temp.replaceAll('{' + key + '}', value);
            });
            $('tbody').append(temp);
        });
    });
}

function setDateFormat(dateVal) {

    dateVal = dateVal.substring(0, 10);
    return ( dateVal.substring(8, 10) + '/' + dateVal.substring(5, 7) + '/' + dateVal.substring(0, 4) );
}