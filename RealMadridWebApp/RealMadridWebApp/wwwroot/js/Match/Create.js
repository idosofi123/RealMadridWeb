// In case of refreshing
$(function () {
    if (new Date($("#dateInput").val()) <= new Date()) {
        $(".goalField").addClass("showAnimation");
    }
});

$("#dateInput").on("input", function (event) {

    // If entered date is not futuristic -
    if (new Date($(this).val()) <= new Date()) {
        $(".goalField").addClass("showAnimation");
    } else {
        $("#homeGoalInput").val("");
        $("#awayGoalInput").val("");
        $(".goalField").removeClass("showAnimation");
    }

});