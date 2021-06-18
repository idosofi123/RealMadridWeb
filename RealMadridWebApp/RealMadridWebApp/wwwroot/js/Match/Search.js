const MONTH_NAMES = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

const YEAR_OFFSET = 1900;

const DEFAULT_SCORE = "TBD";

$(function () {

    $('#searchForm').submit(function (e) {

        e.preventDefault();

        let teamId = $('#teamId').val();
        let competitionId = $('#competitionId').val();
        let fromDate = $('#fromDate').val();
        let toDate = $('#toDate').val();

        $.ajax({
            url: '/Matches/Search',
            data: {
                'teamId': teamId,
                'competitionId': competitionId,
                'fromDate': fromDate,
                'toDate': toDate
            }
        }).done(function (result) {

            $('#resultArea').html('');

            let groupTemplate = $('#matchGroupTemplate').html();
            let rowTemplate = $('#matchRowTemplate').html();

            $.each(result, function (groupIndex, group) {

                let thisGroupTemplate = groupTemplate.replaceAll('{groupTitle}', getMonthTitleOfGroup(group));

                let groupBody = '';

                $.each(group, function (matchIndex, match) {
                    let rowTemp = rowTemplate;

                    if (match.isAway) {
                        let temp = match.homeGoals;
                        match.homeGoals = match.awayGoals;
                        match.awayGoals = temp;
                    }

                    match.homeGoals ??= DEFAULT_SCORE;
                    match.awayGoals ??= DEFAULT_SCORE;
                    match.date = new Date(match.date).toLocaleString();

                    console.log(match.id);

                    // Replace all direct properties
                    $.each(match, function (key, value) {
                        rowTemp = rowTemp.replaceAll('{' + key + '}', value);
                    });

                    // Replace nested properties
                    rowTemp = rowTemp.replaceAll('{team.name}', match.team.name);
                    rowTemp = rowTemp.replaceAll('{team.imagePath}', match.team.imagePath);
                    rowTemp = rowTemp.replaceAll('{competition.imagePath}', match.competition.imagePath);

                    groupBody += rowTemp;
                });

                thisGroupTemplate = thisGroupTemplate.replaceAll('{groupBody}', groupBody);

                $('#resultArea').append(thisGroupTemplate);
            });
        });
    });
});


function getMonthTitleOfGroup(group) {
    let dateObj = new Date(group[0].date);
    return `${dateObj.getYear() + YEAR_OFFSET} - ${MONTH_NAMES[dateObj.getMonth()]}`;
}
