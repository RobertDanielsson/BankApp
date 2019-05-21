$(document).ready(() => {
    var page = 1;
    //const accountId = window.location.href.split("/")[6];

    $("#getTransactions").on("click", () => {
       $.getJSON("/account/getadditionaltransactions", {
           accountId: document.getElementById("accountId").value,
           page: page++
       })
       .done((data) => {
           $.each(data, function (index, item) {
               $("#myTable").append(`
               <tr>
               <td>${item.transactionId}</td>
               <td>${item.accountId}</td>
               <td>${item.date.substring(0, 10)}</td>
               <td>${item.type}</td>
               <td>${item.operation}</td>
               <td>${formatter.format(item.amount)}</td>
               <td>${formatter.format(item.balance)}</td>
               <td>${item.symbol}</td>
               <td>${item.bank}</td>
               <td>${item.account}</td>
               </tr>`)
           })
       })
    })

    const formatter = new Intl.NumberFormat('sv-SE', {
        style: 'currency',
        currency: 'SEK',
        minimumFractionDigits: 2
    })
})