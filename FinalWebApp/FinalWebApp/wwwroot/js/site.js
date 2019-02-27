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

    $("#StatisticsForm").submit(function (e) {
        var form = $(this);
        var url = form.attr('action');

        $.ajax({
            type: "post",
            url: url,
            data: form.serialize(), // serializes the form's elements.
            success: function (data) {
                d3.select("svg").remove();
                histogramChart1(data.items[0]);
                histogramChart2(data.items[1]);
            },
            error: function () {
                alert("Error");
            }
        });

        e.preventDefault(); // avoid to execute the actual submit of the form.
    });

    function histogramChart1(data) {
        var w = 200,                        //width
            h = 200,                            //height
            r = 100,                            //radius
            color = d3.scale.category20c();     //builtin range of colors


        var vis = d3.select("#StatisticsGraph1")
            .append("svg:svg")              //create the SVG element inside the <body>
            .data([data])                   //associate our data with the document
            .attr("width", w)           //set the width and height of our visualization (these will be attributes of the <svg> tag
            .attr("height", h)
            .append("svg:g")                //make a group to hold our pie chart
            .attr("transform", "translate(" + r + "," + r + ")")    //move the center of the pie chart from 0, 0 to radius, radius

        var arc = d3.svg.arc()              //this will create <path> elements for us using arc data
            .outerRadius(r);

        var pie = d3.layout.pie()           //this will create arc data for us given a list of values
            .value(function (d) { return d.count; });    //we must tell it out to access the value of each element in our data array

        var arcs = vis.selectAll("g.slice")     //this selects all <g> elements with class slice (there aren't any yet)
            .data(pie)                          //associate the generated pie data (an array of arcs, each having startAngle, endAngle and value properties)
            .enter()                            //this will create <g> elements for every "extra" data element that should be associated with a selection. The result is creating a <g> for every object in the data array
            .append("svg:g")                //create a group to hold each slice (we will have a <path> and a <text> element associated with each slice)
            .attr("class", "slice");    //allow us to style things in the slices (like text)

        arcs.append("svg:path")
            .attr("fill", function (d, i) { return color(i); }) //set the color for each slice to be chosen from the color function defined above
            .attr("d", arc);                                    //this creates the actual SVG path using the associated data (pie) with the arc drawing function

        arcs.append("svg:text")                                     //add a label to each slice
            .attr("transform", function (d) {                    //set the label's origin to the center of the arc
                //we have to make sure to set these before calling arc.centroid
                d.innerRadius = 0;
                d.outerRadius = r;
                return "translate(" + arc.centroid(d) + ")";        //this gives us a pair of coordinates like [50, 50]
            })
            .attr("text-anchor", "middle")                          //center the text on it's origin
            .text(function (d, i) { return data[i].name; });        //get the label from our original data array

    }
    
    function histogramChart2(data) {
        var w = 200,                        //width
            h = 200,                            //height
            r = 100,                            //radius
            color = d3.scale.category20c();     //builtin range of colors

        var vis = d3.select("#StatisticsGraph2")
            .append("svg:svg")              //create the SVG element inside the <body>
            .data([data])                   //associate our data with the document
            .attr("width", w)           //set the width and height of our visualization (these will be attributes of the <svg> tag
            .attr("height", h)
            .append("svg:g")                //make a group to hold our pie chart
            .attr("transform", "translate(" + r + "," + r + ")")    //move the center of the pie chart from 0, 0 to radius, radius

        var arc = d3.svg.arc()              //this will create <path> elements for us using arc data
            .outerRadius(r);

        var pie = d3.layout.pie()           //this will create arc data for us given a list of values
            .value(function (d) { return d.count; });    //we must tell it out to access the value of each element in our data array

        var arcs = vis.selectAll("g.slice")     //this selects all <g> elements with class slice (there aren't any yet)
            .data(pie)                          //associate the generated pie data (an array of arcs, each having startAngle, endAngle and value properties)
            .enter()                            //this will create <g> elements for every "extra" data element that should be associated with a selection. The result is creating a <g> for every object in the data array
            .append("svg:g")                //create a group to hold each slice (we will have a <path> and a <text> element associated with each slice)
            .attr("class", "slice");    //allow us to style things in the slices (like text)

        arcs.append("svg:path")
            .attr("fill", function (d, i) { return color(i); }) //set the color for each slice to be chosen from the color function defined above
            .attr("d", arc);                                    //this creates the actual SVG path using the associated data (pie) with the arc drawing function

        arcs.append("svg:text")                                     //add a label to each slice
            .attr("transform", function (d) {                    //set the label's origin to the center of the arc
                //we have to make sure to set these before calling arc.centroid
                d.innerRadius = 0;
                d.outerRadius = r;
                return "translate(" + arc.centroid(d) + ")";        //this gives us a pair of coordinates like [50, 50]
            })
            .attr("text-anchor", "middle")                          //center the text on it's origin
            .text(function (d, i) { return data[i].name; });        //get the label from our original data array

    }

	var today = new Date();
	var tomorrow = new Date(new Date().getTime() + 48 * 60 * 60 * 1000);

	$('#endDate').val(getDateString(today));
    $('#startDate').val(getDateString(tomorrow));
    //$('#startDate').val(getDateString(today));

    $('#endDate').attr('min', getDateString(today));
    $('#startDate').attr('min', document.getElementById('endDate').getAttribute('min'));

    $('#endDate').on('change', function () {
        $('#startDate').val(getDateString(new Date(new Date(document.getElementById('endDate').value).getTime() + 48 * 60 * 60 * 1000)));
        $('#startDate').attr('min', document.getElementById('endDate').value);
    });

	function getDateString(date) {
		var day = ("0" + date.getDate()).slice(-2);
		var month = ("0" + (date.getMonth() + 1)).slice(-2);

		return date.getFullYear() + "-" + (month) + "-" + (day);
	}

});
