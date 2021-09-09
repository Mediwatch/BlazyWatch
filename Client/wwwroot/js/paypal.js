
async function  init_paypal(id) {


    // if (document.getElementById("paypal-button-container") != null){
    //     alert('Transaction canceled');
    //     paypal.Buttons({
    //          // Set up the transaction
    //     createOrder: function (data, actions) {
    //         orderId = data.orderID;
    //         return fetch('/api/paypal/checkout/order/create/', {
    //             method: 'post'
    //         }).then(function (res) {
    //             return res.json();
    //         }).then(function (data) {
    //             return data.orderID;
    //         });
    //     },

    //     // Finalise the transaction
    //     onApprove: function (data, actions) {
    //         return fetch('/api/paypal/checkout/order/approved/' + data.orderID, {
    //             method: 'post'
    //         }).then(function (res) {
    //             return actions.order.capture();
    //         }).then(function (details) {

    //             // (Preferred) Notify the server that the transaction id complete and have a option to display an order completed screen.
    //             window.location.replace('/api/paypal/checkout/order/complete/' + data.orderID + '/@ViewBag.CurrencyCode');
                
    //             // OR
    //             // Notify the server that the transaction id complete
    //             //httpGet('/api/paypal/checkout/order/complete/' + data.orderID);

    //             // Show a success message to the buyer
    //             alert('Transaction completed by ' + details.payer.name.given_name + '!');
    //         });
    //     },

    //     // Buyer cancelled the payment
    //     onCancel: function (data, actions) {
    //         httpGet('/api/paypal/checkout/order/cancel/' + data.orderID);
    //     },

    //     // An error occurred during the transaction
    //     onError: function (err) {
    //         httpGet('/api/paypal/checkout/order/error/' + orderId + '/' + encodeURIComponent(err));
    //     }

    //     }).render('#paypal-button-container');
    // }
    if (document.getElementById("paypal-button-container") != null){
        paypal.Buttons({
        // Set up the transaction
        createOrder: function (data, actions) {
            orderId = data.orderID
            test = JSON.stringify({
                formationId: id
            })
            console.log(test);
            return fetch('/api/paypal/checkout/order/create/', {
                method: 'post',
                headers: {
                    'Content-Type': 'application/json',
                  },
                body: test
            }).then(function (res) {
                return res.json();
            }).then(function (data) {
                return data.orderID;
            });
        },

        // Finalise the transaction
        onApprove: function (data, actions) {
            return fetch('/api/paypal/checkout/order/approved/' + data.orderID, {
                method: 'post'
            }).then(function (res) {
                return actions.order.capture();
            }).then(function (details) {

                // (Preferred) Notify the server that the transaction id complete and have a option to display an order completed screen.
                httpGet('/api/paypal/checkout/order/complete/' +  data.orderID);
                
                // OR
                // Notify the server that the transaction id complete
                //httpGet('/api/paypal/checkout/order/complete/' + data.orderID);

                // Show a success message to the buyer
                alert('Transaction completed by ' + details.payer.name.given_name + '!');
            });
        },

        // Buyer cancelled the payment
        onCancel: function (data, actions) {
            httpGet('/api/paypal/checkout/order/cancel/' + data.orderID);
            alert('Transaction cancel by ' + details.payer.name.given_name + '!');
        },

        // An error occurred during the transaction
        onError: function (err) {
            console.log('Error');
            httpGet('/api/paypal/checkout/order/error/' + orderId + '/' + encodeURIComponent(err));
        }

    }).render('#paypal-button-container');
    }
}

function httpGet(url) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("GET", url, false);
    xmlHttp.send(null);
    return xmlHttp.responseText;
}
