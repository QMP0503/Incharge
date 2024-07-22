import Chart from 'chart.js/auto'

const ctx = document.getElementById('memberPie');

const active = document.getElementById('active');
const overdue = document.getElementById('overdue');
const noMembership = document.getElementById('noMembership');

const data = {
    labels: ['Active', 'Overdue', 'No Membership'],
    datasets: [
        {
            label: 'Membership data',
            data: [active.value, overdue.value, noMembership.value],
            backgroundColor: Object.values(Utils.CHART_COLORS),
        }
    ]
};

new Chart(ctx, {
    type: 'pie',
    data: data,
    options: {
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
            },
            title: {
                display: true,
                text: 'Membership Status'
            }
        }
    },
});