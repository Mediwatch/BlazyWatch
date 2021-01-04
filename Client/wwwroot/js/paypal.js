function init_paypal(price) {
    if (document.getElementById("paypal-button-container") != null){
        paypal.Buttons({
            createOrder: function (data, actions) {
                return actions.order.create({
                    purchase_units: [{
                        amount: {
                            value: price
                        }
                    }]
                });
            },
            onApprove:  (data, actions) => {
                return actions.order.capture().then(function (details) {

                    alert('Transaction completed by ' + details.payer.name.given_name);
                //REMPLIRE LES PLACEHOLDER AVEC LES VALEUR DYNAMIQUE DE LA SESSION!
                body = JSON.stringify({
                    "invoiceId": String(details.id),
                    "userId": "PLACEHOLDER",
                    "formationId" : "PLACEHOLDER",
                    "billingAdress": details.purchase_units[0].shipping.address.address_line_1 + " " + details.purchase_units[0].shipping.address.admin_area_2 + " "+ details.purchase_units[0].shipping.address.postal_code,
                    "currency": details.purchase_units[0].amount.currency_code,
                    "price" : parseFloat(details.purchase_units[0].amount.value)
                })

                
                var xmlhttp = new XMLHttpRequest();   // new HttpRequest instance
                var theUrl = "Order";
                xmlhttp.open("POST", theUrl, true);
                xmlhttp.setRequestHeader("Content-Type", "application/json");
                xmlhttp.send(
                    body
                );
                }).then(() => {
                    DotNet.invokeMethodAsync('{APP ASSEMBLY}', 'OnPaypalDone');
                });
            },
            onCancel: (data, actions) =>{
                alert('Transaction canceled');
            }
        }).render('#paypal-button-container'); // Display payment options on your web page
    }
}


