const INITIAL_MIN_AGE = 16;
const INITAIL_MAX_AGE = 100;

$(function () {
    $("#playerImg").on('load', function () {
        alert("image is loaded");
    });

    $("#minAge").val(INITIAL_MIN_AGE);
    $("#minAgeLabel").html("Min Age: " + INITIAL_MIN_AGE);
    $("#maxAge").val(INITAIL_MAX_AGE);
    $("#maxAgeLabel").html("Max Age: " + INITAIL_MAX_AGE);

    $("#minAge").on('change', function () {
        const minVal = parseInt(this.value);
        const maxVal = parseInt($("#maxAge").val());

        if (minVal < INITIAL_MIN_AGE) {
            this.value = INITIAL_MIN_AGE;
        }
        if (minVal > maxVal) {
            $("#maxAge").val(this.value);
            $("#maxAgeLabel").html("Max Age: " + this.value);
        }
        $("#minAgeLabel").html("Min Age: " + this.value);
    });

    $("#maxAge").on('change', function () {

        const maxVal = parseInt(this.value);
        const minVal = parseInt($("#minAge").val());

        if (maxVal < INITIAL_MIN_AGE) {
            this.value = INITIAL_MIN_AGE;
        }
        if (maxVal < minVal) {
            $("#minAge").val(this.value);
            $("#minAgeLabel").html("Min Age: " + this.value);
        }
        $("#maxAgeLabel").html("Max Age: " + this.value);
    });
});
