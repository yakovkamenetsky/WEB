﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    var body = $('body');
    var backgrounds = new Array(
        'url(../Images/mainBackground1.jpg) no-repeat center fixed',
        'url(../Images/mainBackground2.jpg) no-repeat center fixed'
    );
    var current = 0;

    function nextBackground() {
        body.css(
            'background',
            backgrounds[current = ++current % backgrounds.length]
        );
        body.css('background-size', 'cover');


        setTimeout(nextBackground, 5000);
    }

    setTimeout(nextBackground, 5000);
    body.css('background', backgrounds[0]);  
    body.css('background-size', 'cover');
});

$(function () {
    var editButton = this.getElementById("editButton");
    if (sessionStorage.getItem("isUserAdmin") == "true") {
        editButton.style.display == "block";
    } else {
        editButton.style.display == "none";
    }
});

function showOrHideFeatures() {
    var features = document.getElementById("featuresView");
    if (features.style.display === "none") {
        features.style.display = "block";
    } else {
        features.style.display = "none";
    }
}