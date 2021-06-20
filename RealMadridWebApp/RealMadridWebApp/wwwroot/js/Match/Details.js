const DIV_ID = "#winLoseChart"
const HEIGHT = 180;
const DONUT_WIDTH = 30;
const COLOR_SCHEME = ["#009933", "#990000", "#669999"];

// Initiate the chart on page load.
$(function () {
    initChart();
});

function initChart() {

    let won  = parseInt($(DIV_ID).attr('won'));
    let lost = parseInt($(DIV_ID).attr('lost'));
    let drew = parseInt($(DIV_ID).attr('drew'));

    const dataset = [
        { label: 'Won', count: won },
        { label: 'Lost', count: lost },
        { label: 'Drew', count: drew }
    ];

    const radius = HEIGHT / 2;

    const color = d3.scaleOrdinal().range(COLOR_SCHEME);

    const svg = d3.select(DIV_ID)
        .append('svg')
        .attr('width', HEIGHT)
        .attr('height', HEIGHT)
        .append('g')
        .attr('transform', 'translate(' + (HEIGHT / 2) + ',' + (HEIGHT / 2) + ')');

    const arc = d3.arc().innerRadius(radius - DONUT_WIDTH).outerRadius(radius);

    const pie = d3.pie().value(function (d) { return d.count; }).sort(null);

    const path = svg.selectAll('path')
        .data(pie(dataset))
        .enter()
        .append('path')
        .attr('d', arc)
        .attr('fill', function (d, i) {
            return color(d.data.label);
        });

}