
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

let revenueString = document.getElementById("RevenueGraph").value;
let revenuelist = JSON.parse(revenueString);
let months = revenuelist.map(x => x.Month);
let revenue = revenuelist.map(x => x.Revenue);

let costString = document.getElementById("CostGraph").value;
let costList = JSON.parse(costString);
let cost = costList.map(x => x.Cost);
//let months = [];
//let revenue = [];
//business.forEach(element => {
//    months.push(element.Month);
//    revenue.push(element.Revenue);
//});

const data = {
    labels: months,
    datasets: [{
        label: 'Revenue',
        data: revenue,
        fill: false,
        borderColor: 'rgb(75, 192, 192)',
        tension: 0.1
    },
    {
        label: 'Cost',
        data: cost,
        fill: false,
        borderColor: 'rgb(255, 99, 132)',
        tension: 0.1
    }]
};

new Chart(ctx, {
    type: 'line',
    data: data,
    options: {
        responsive: true,
        plugins: {
            title: {
                display: true,
                text: 'Monthly Revenue by Year',
            }
        }
    }
});