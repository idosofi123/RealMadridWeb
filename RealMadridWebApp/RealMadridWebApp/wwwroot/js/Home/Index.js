var MIN_GOALS_CHART = 8;

$(function () {

    $("#refresh").click(function () {
        $.ajax({
            url: '/Players/GetRandomPlayer',
        }).done(function (player) {
            updateRandomPlayer(player);
        })
    });

        $.ajax({
            url: '/Matches/GetNextMatch',
        }).done(function (match) {
            updateNextMatch(match);
            setCounter(match.date);
        })

        $.ajax({
            url: '/Players/GetRandomPlayer',
        }).done(function (player) {
            updateRandomPlayer(player);
        })

        $.ajax({
            url: '/Matches/getGraphValues',
        }).done(function (gamesData) {
            initGraph(gamesData);
        })

});

function updateRandomPlayer(player) {

    document.getElementById("player").textContent = "Player - " + player.firstName + " " + player.lastName;
    document.getElementById("playerText").textContent = player.firstName + " was born in " + player.birthCountry.countryName;
    $("#playerImage").attr("src", player.imagePath);
    $("#routePlayerId").attr("href", "/Players/Details/" + player.playerId);
}

function updateNextMatch(match) {

    document.getElementById("competition").textContent = "Competition - " +match.competition.name;
    document.getElementById("homeTeam").textContent = "Real Madrid";
    document.getElementById("awayTeam").textContent = match.team.name;
    $("#homeImage").attr("src", "/Images/Teams/RealMadrid.png");
    $("#awayImage").attr("src", match.team.imagePath);
    $("#routeGameId").attr("href","/Matches/Details/" + match.id);
}

function setCounter(matchDate) {

    setInterval(function () {

        var now = new Date().getTime();

        var distance = new Date(matchDate).getTime() - now;

        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Output the result in an element with id="demo"
        document.getElementById("counter").innerHTML = "The game starts in: " +days + "d:" + hours + "h:"
            + minutes + "m:" + seconds + "s";

        // If the count down is over, write some text 
        if (distance < 0) {
            clearInterval(x);
            document.getElementById("counter").innerHTML = "Match start";
        }
    }, 1000);

}

function initGraph(gamesData) {

    gamesData = gamesData.reverse();

    var margin = { top: 10, right: 120, bottom: 60, left: 35 },
        width = 550 - margin.left - margin.right,
        height = 400 - margin.top - margin.bottom;
        
    var svg = d3.select("#goalsGraph")
        .append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform",
            "translate(" + margin.left + "," + margin.top + ")");

        var allGroup = ["Home", "Away"]
        var maxGoals = 0;

        var dataReady = allGroup.map(function (grpName) { 
            return {
                name: grpName,
                values: gamesData.map(function (d) {
                    maxGoals = (maxGoals < Math.max(d.homeGoals, d.awayGoals) ? Math.max(d.homeGoals, d.awayGoals) : maxGoals);
                    return { time: new Date(d.fullDate), value: (grpName == "Home") ? d.homeGoals : d.awayGoals };
                })
            };
        });

    var myColor = d3.scaleOrdinal()
        .domain(allGroup)
        .range(d3.schemeSet2);

    var currentDate = new Date();
    currentDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);

    var sixMonthBack = new Date();
    sixMonthBack = new Date(sixMonthBack.getFullYear(), sixMonthBack.getMonth(), 1);
    sixMonthBack.setMonth(sixMonthBack.getMonth() - 5);

    var x = d3.scaleTime()
        .domain([sixMonthBack, currentDate])
        .range([0, width]);

        svg.append("g")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x).tickFormat(d3.timeFormat("%B %Y")));

    maxGoals = (maxGoals < MIN_GOALS_CHART) ? MIN_GOALS_CHART : maxGoals;

    var y = d3.scaleLinear()
        .domain([0, maxGoals])
        .range([height, 0]);

    svg.append("g")
        .call(d3.axisLeft(y));

    // Add the lines
    var line = d3.line()
        .x(function (d) { return x(+d.time) })
        .y(function (d) { return y(+d.value) })
    svg.selectAll("myLines")
        .data(dataReady)
        .enter()
        .append("path")
        .attr("d", function (d) { return line(d.values) })
        .attr("stroke", function (d) { return myColor(d.name) })
        .style("stroke-width", 4)
        .style("fill", "none")

    // Add the points
    svg
        // First we need to enter in a group
        .selectAll("myDots")
        .data(dataReady)
        .enter()
        .append('g')
        .style("fill", function (d) { return myColor(d.name) })
        // Second we need to enter in the 'values' part of this group
        .selectAll("myPoints")
        .data(function (d) { return d.values })
        .enter()
        .append("circle")
        .attr("cx", function (d) { return x(d.time) })
        .attr("cy", function (d) { return y(d.value) })
        .attr("r", 5)
        .attr("stroke", "white")

    // Add a legend at the end of each line
    svg
        .selectAll("myLabels")
        .data(dataReady)
        .enter()
        .append('g')
        .append("text")
        .datum(function (d) { return { name: d.name, value: d.values[d.values.length - 1] }; }) // keep only the last value of each time series
        .attr("transform", function (d) { return "translate(" + x(d.value.time) + "," + y(d.value.value) + ")"; }) // Put the text at the position of the last point
        .attr("x", 12) // shift the text a bit more right
        .text(function (d) { return d.name; })
        .style("fill", function (d) { return myColor(d.name) })
        .style("font-size", 15)

}
