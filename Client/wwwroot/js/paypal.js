const baseOrderAmount = '15.00';        

function init_paypal(price) {
    if (document.getElementById("paypal-button-container") != null){
        alert('Transaction canceled');
        paypal.Buttons({
             // Set up the transaction
        createOrder: function (data, actions) {
            orderId = data.orderID;
            return fetch('/api/paypal/checkout/order/create/', {
                method: 'post'
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
                window.location.replace('/api/paypal/checkout/order/complete/' + data.orderID + '/@ViewBag.CurrencyCode');
                
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
        },

        // An error occurred during the transaction
        onError: function (err) {
            httpGet('/api/paypal/checkout/order/error/' + orderId + '/' + encodeURIComponent(err));
        }

        }).render('#paypal-button-container');
            
    }
}


