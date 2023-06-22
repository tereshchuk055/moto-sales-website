$.ajax({
    type: "GET",
    url: "/Brand/GetStats",
    success: function (response) {
        var chart = anychart.pie(response.map((obj) => [obj.brandName, obj.number]));

        // create range color palette with color ranged between light blue and dark blue
        var palette = anychart.palettes.rangeColors();
        palette.items([{ color: '#f5c469' }, { color: '#ffa600' }]);

        // set chart title text settings
        chart
            .title('Sales by Brands')
            // set chart radius
            .innerRadius('40%')
            // set palette to the chart
            .palette(palette);

        // set container id for the chart
        chart.container('container1');
        // initiate chart drawing
        chart.draw();
    
    },
    failure: function (response) {
        console.log("Failure");
    }
});

$.ajax({
    type: "GET",
    url: "/Model/GetStats",
    success: function (response) {
        var chart = anychart.pie(response.map((obj) => [obj.modelName, obj.number]));

        // create range color palette with color ranged between light blue and dark blue
        var palette = anychart.palettes.rangeColors();
        palette.items([{ color: '#fc865b' }, { color: '#fc4300' }]);

        // set chart title text settings
        chart
            .title('Sales by Models')
            // set chart radius
            .innerRadius('40%')
            // set palette to the chart
            .palette(palette);

        // set container id for the chart
        chart.container('container2');
        // initiate chart drawing
        chart.draw();

    },
    failure: function (response) {
        console.log("Failure");
    }
});

$.ajax({
    type: "GET",
    url: "/Type/GetStats",
    success: function (response) {
        var chart = anychart.pie(response.map((obj) => [obj.typeName, obj.number]));

        // create range color palette with color ranged between light blue and dark blue
        var palette = anychart.palettes.rangeColors();
        palette.items([{ color: '#7cb1fc' }, { color: '#0a6efc' }]);

        // set chart title text settings
        chart
            .title('Sales by Types')
            // set chart radius
            .innerRadius('40%')
            // set palette to the chart
            .palette(palette);

        // set container id for the chart
        chart.container('container3');
        // initiate chart drawing
        chart.draw();

    },
    failure: function (response) {
        console.log("Failure");
    }
});