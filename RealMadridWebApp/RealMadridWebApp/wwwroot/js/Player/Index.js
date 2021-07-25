const INITIAL_MIN_AGE = 1;
const INITAIL_MAX_AGE = 100;

$(function () {
    document.getElementById("minAgeRange").value = INITIAL_MIN_AGE;
    document.getElementById("minAgeLabel").innerHTML = "Min Age: " + INITIAL_MIN_AGE;

    document.getElementById("minAgeRange").onchange = function () {
        document.getElementById("minAgeLabel").innerHTML = "Min Age: " + this.value;
    };
    document.getElementById("maxAgeRange").value = INITAIL_MAX_AGE;
    document.getElementById("maxAgeLabel").innerHTML = "Min Age: " + INITAIL_MAX_AGE;

    document.getElementById("maxAgeRange").onchange = function () {
        document.getElementById("maxAgeLabel").innerHTML = "Min Age: " + this.value;
    };
});