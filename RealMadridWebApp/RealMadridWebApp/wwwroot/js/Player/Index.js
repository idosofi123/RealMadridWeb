const INITIAL_MIN_AGE = 0;
const INITAIL_MAX_AGE = 100;

$(function () {
    $("#minAge").val(INITIAL_MIN_AGE);
    $("#minAgeLabel").html("Min Age: " + INITIAL_MIN_AGE);

    $("#minAge").on('change', function () {
        $("#minAgeLabel").html("Min Age: " + this.value);
    });
   $("#maxAge").val(INITAIL_MAX_AGE);
    $("#maxAgeLabel").html("Min Age: " + INITAIL_MAX_AGE);

    $("#maxAge").on('change', function () {
        console.log('change');
        $("#maxAgeLabel").html("Min Age: " + this.value);
    });
});