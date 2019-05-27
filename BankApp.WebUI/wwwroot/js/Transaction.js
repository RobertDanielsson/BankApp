$(document).ready(() => {
    var page = 1;
    //const accountId = window.location.href.split("/")[6];

    $("#getTransactions").on("click", () => {
       $.getJSON("/transfer/getadditionaltransactions", {
           accountId: document.getElementById("accountId").value,
           page: page++
       })
       .done((data) => {
           $.each(data, function (index, item) {
               $('<tr>').append(
                   $('<td>').text(item.transactionId),
                   $('<td>').text(item.accountId),
                   $('<td>').text(item.date.substring(0, 10)),
                   $('<td>').text(item.type),
                   $('<td>').text(item.operation),
                   $('<td>').text(formatter.format(item.amount)),
                   $('<td>').text(formatter.format(item.balance)),
                   $('<td>').text(item.symbol),
                   $('<td>').text(item.bank),
                   $('<td>').text(item.account)
               ).appendTo('#myTable');
           })
       })
    })

    const formatter = new Intl.NumberFormat('se-SV', {
        style: 'currency',
        currency: 'SEK',
        minimumFractionDigits: 2
    })
})