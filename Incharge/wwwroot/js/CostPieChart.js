
const expenseString = document.getElementById("expenseString").value;


const expenseData = JSON.parse(expenseString);

const Wages = expenseData.Wages;
const Rent = expenseData.Rent;
const Utilities = expenseData.Utilities;
const Insurance = expenseData.Insurance;
const Equipment = expenseData.Equipment;
const OtherExpenses = expenseData.OtherExpenses;
const Maintenance = expenseData.Maintenance;

var pieData = {
    labels: ["Wage", "Rent", "Utilities", "Insurance", "Equipment", "Other", "Maintenance"],
    datasets: [{
        data: [Wages, Rent, Utilities, Insurance, Equipment, OtherExpenses, Maintenance],
        backgroundColor: [
            "#878BB6",
            "#4ACAB4",
            "#FF8153",
            "#ECC94B",
            "#F79D84",
            "#9B59B6",
            "#8E44AD"

        ]
    }]
};

var ctx = document.getElementById("myPieChart").getContext("2d");

new Chart(ctx, {
    type: 'pie',
    data: pieData,
    options: {
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
            },
            datalabels: {
                color: '#36A2EB',
            }
        }
    },
});





