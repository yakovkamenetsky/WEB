// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/*
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
});*/

function showOrHideFeatures() {
    var features = document.getElementById("featuresView");
    if (features.style.display === "none") {
        features.style.display = "block";
    } else {
        features.style.display = "none";
    }
}

$(document).ready(function () {

    // Key up search the products
    $(".LoginAndRegisterForm").submit(function (e) {
        var form = $(this);
        var url = form.attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: form.serialize(), // serializes the form's elements.
            success: function (data) {
                if (data != null) {
                    alert(data);
                }
                window.location = "/";
                window.history.go();
            },
            error: function (data) {
                alert(data.responseText);
            },
        });

        e.preventDefault(); // avoid to execute the actual submit of the form.
    });

    $("#LogoutForm").submit(function (e) {
        var form = $(this);
        var url = form.attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: form.serialize(), // serializes the form's elements.
            success: function (data) {
                if (data != null) {
                    alert(data);
                }
                window.location = "/";
            },
            error: function (data) {
                alert(data.responseText);
            },
        });

        e.preventDefault(); // avoid to execute the actual submit of the form.
    });

    $("#ok-order").on('click', function (e) {
        window.history.back();
    })
   
    $("#orderNow").submit(function (e) {
        var form = $(this);
        var url = form.attr('action');

        $.ajax({
            type: "post",
            url: url,
            data: form.serialize(), // serializes the form's elements.
            success: function (data) {
                window.location = '/Orders/summery?id=' + data;
            },
            error: function (data) {
                if (data.status === 401) {
                    alert("You must log in");
                    $('#loginButton').click();
                }
            }
        });

        e.preventDefault(); // avoid to execute the actual submit of the form.
    });


    var today = new Date();
    var tomorrow = new Date(new Date().getTime() + 48 * 60 * 60 * 1000);

    $('#endDate').val(getDateString(today));
    $('#startDate').val(getDateString(tomorrow));

    function getDateString(date) {
        var day = ("0" + date.getDate()).slice(-2);
        var month = ("0" + (date.getMonth() + 1)).slice(-2);

        return date.getFullYear() + "-" + (month) + "-" + (day);
    }
});

