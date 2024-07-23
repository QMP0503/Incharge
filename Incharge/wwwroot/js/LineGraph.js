
const ctx = document.getElementById('LineGraph');

const actions = [
    {
        name: 'pointStyle: circle (default)',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'circle';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: cross',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'cross';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: crossRot',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'crossRot';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: dash',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'dash';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: line',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'line';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: rect',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'rect';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: rectRounded',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'rectRounded';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: rectRot',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'rectRot';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: star',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'star';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: triangle',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = 'triangle';
            });
            chart.update();
        }
    },
    {
        name: 'pointStyle: false',
        handler: (chart) => {
            chart.data.datasets.forEach(dataset => {
                dataset.pointStyle = false;
            });
            chart.update();
        }
    }
];

new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [{
            label: 'Revenue',
            data: [65, 59, 80, 81, 56, 55, 40],
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1
        }]
    },
    options: {
        responsive: true,
        plugins: {
            title: {
                display: true,
                text: (ctx) => 'Point Style: ' + ctx.chart.data.datasets[0].pointStyle,
            }
        }
    }
});