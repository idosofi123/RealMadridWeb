// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var madridCanvas = document.getElementById("realMadridCanvas");
var ctx = madridCanvas.getContext("2d");
ctx.font = "30px Comic Sans MS";
var gradient = ctx.createLinearGradient(0, 0, madridCanvas.width, 0);
gradient.addColorStop("0", "blue");
gradient.addColorStop(".5", "#0e8af5");
gradient.addColorStop("1", "blue");

// Fill with gradient
ctx.strokeStyle = gradient;
ctx.strokeText("Real Madrid CF", 0, madridCanvas.height/1.5);
