

const active = document.getElementById('active').value;
const overdue = document.getElementById('overdue').value;
const noMembership = document.getElementById('noMembership').value;

var pieData = {
    labels: ["Active", "Overdue", "No Membership"],
    datasets: [{
        data: [active, overdue, noMembership],
        backgroundColor: [
            "#878BB6",
            "#4ACAB4",
            "#FF8153"
        ]
    }]
};

var ctx = document.getElementById("myData").getContext("2d");

new Chart(ctx, {
    type: 'pie',
    data: pieData,
    options: {
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
            },
            title: {
                display: true,
                text: 'Memebership Distribution'
            }
        }
    },
});