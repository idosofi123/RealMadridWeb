$(function () {

        $.ajax({
            url: '/Matches/GetNextMatch',
        }).done(function (match) {
            updateNextMatch(match);

            //setCounter(match.date)
        })
});

function updateNextMatch(match) {

    document.getElementById("competition").textContent = "Competition - " +match.competition.name;
    //$("#competition").attr("src", match.competition.imagePath);
    document.getElementById("homeTeam").textContent = "Real Madrid";
    document.getElementById("awayTeam").textContent = match.team.name;
    $("#homeImage").attr("src", "/Images/Teams/RealMadrid.png");
    $("#awayImage").attr("src", match.team.imagePath);
    $("#routeId").attr("href","/Matches/Details/" + match.id);
}

//function setCounter(matchDate) {

//    setInterval(function () {

//        var now = new Date().getTime();

//        var distance = new Date(matchDate).getTime() - now;

//        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
//        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
//        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
//        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

//        // Output the result in an element with id="demo"
//        document.getElementById("counter").innerHTML = "The game starts in: " +days + "d:" + hours + "h:"
//            + minutes + "m:" + seconds + "s";

//        // If the count down is over, write some text 
//        if (distance < 0) {
//            clearInterval(x);
//            document.getElementById("counter").innerHTML = "Match start";
//        }
//    }, 1000);

//}
